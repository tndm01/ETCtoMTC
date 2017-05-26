using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using System.Xml;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class EtagRegisterProcess
    {
        #region Field
        private string _databaseTableName;

        // object data base
        private DataBaseHelper _mydatabaseHelper;

        // object fileTransferFtp
        private FileTransferFtp _fileTransferFtp;

        // object eTag
        //private ETagTransactionModel _oETag;
        // local path
        private string _localPath;

        // remotepath
        private string _remotePath;

        // object eTag
    
        #endregion

        #region ETC
     
       
       
      
        public EtagRegisterProcess(string tableName, string etagLocalFolderPath, string etagFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = etagLocalFolderPath;
                _remotePath = etagFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void EtagProcessData()
        {
            try
            {
                // Get data from data base
                DataTable dt = getEtagRegisterData();

                List<ObjectEtagRegisterModel> listItem = new List<ObjectEtagRegisterModel>();
                // process data
                if (dt != null)
                {
                    NLogHelper.Info(String.Format("JobName: {0}, Get database success, Num of line are {1}", "EtagRegister ETC-MTC", dt.Rows.Count));
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectEtagRegisterModel etag = new ObjectEtagRegisterModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        etag.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
                        etag.Action = (string)row["Action"];

                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader rd = new StringReader(xml);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            XmlElement emElement = doc.DocumentElement;

                            if (emElement != null)
                            {
                                var selectSingleNode = emElement.SelectSingleNode("MaETag");
                                if (selectSingleNode != null)
                                    etag.MaETag = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GiaVe");
                                if (selectSingleNode != null)
                                    etag.GiaVe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MaLoaiVe");
                                if (selectSingleNode != null)
                                    etag.MaVe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SoXe");
                                if (selectSingleNode != null)
                                    etag.SoXeNhanDang = selectSingleNode.InnerText;                             
                                selectSingleNode = emElement.SelectSingleNode("NgayBatDau");
                                if (selectSingleNode != null)
                                    etag.NgayBatDau = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NgayKetThuc");
                                if (selectSingleNode != null)
                                    
                                    etag.NgayKetThuc = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("MaNhanVien");
                                if (selectSingleNode != null)
                                    etag.MaNhanVien = selectSingleNode.InnerText;
                                listItem.Add(etag);
                            }
                        }


                    }
                }

                // Date to Create subfolder
                DateTime date = DateTime.Now;
                _localPath = _localPath + "/" + date.ToString("yyyyMMdd");
                _remotePath = _remotePath + "/" + date.ToString("yyyyMMdd");
                // Save to file and transfer
                foreach (var item in listItem)
                {
                    string fileName = saveEtag2File(item);
                    NLogHelper.Info(fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        // Ftp transfer
                        //_fileTransferFtp.UploadFile(fileName, _remotePath);

                        // update database
                        updateSyncStatus(item);
                    }


                }

                // ftp folder
                // performance
                // Need Create Sub folder by date in Ftp????
                _fileTransferFtp.UploadDirectory(_localPath, _remotePath);
                

            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        private void updateSyncStatus(ObjectEtagRegisterModel item)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_SYNC_DATA;
                cmd.Parameters.Add("@TrackingId", SqlDbType.BigInt).Value = item.TrackingId;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        /// <summary>
        /// Save Data to File
        /// </summary>
        /// <param name="item"></param>
        private string saveEtag2File(ObjectEtagRegisterModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
               // DateTime date = DateTime.Parse(item.NgayBatDau);
               // DateTime.TryParse(item.NgayBatDau, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", "EtagThang", item.SoXeNhanDang, item.Action, item.NgayBatDau.ToString("yyyyMMddHHmmss"));
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;


                // repare data
                StringBuilder sb = new StringBuilder();
            
                sb.Append(item.MaETag + ",");
                sb.Append(item.GiaVe + ",");
                sb.Append(item.MaVe + ",");
                sb.Append(item.SoXeNhanDang + ",");
                sb.Append(item.NgayBatDau.ToString("yyyy-MM-dd") + ",");
                sb.Append(item.NgayKetThuc.ToString("yyyy-MM-dd") + ",");
                sb.Append(item.MaNhanVien);


                // Write to File
                File.WriteAllText(filepath, sb.ToString());
                NLogHelper.Info(String.Format("Save file EtagRegister "));
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                filepath = string.Empty;
            }
            return filepath;
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <returns></returns>
        private DataTable getEtagRegisterData()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_GET_SYNC_DATA;
                cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = _databaseTableName;

                DataTable dt = _mydatabaseHelper.GetDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return null;
        }
        #endregion
        #region MTC
        public List<ObjectEtagRegisterModel> ReadFile(string path)
        {
           // string line;
            List<ObjectEtagRegisterModel> list = new List<ObjectEtagRegisterModel>();

         //   string[] name = path.Split('_');
          //  string SoXeDangKy = name[1];

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    try
                    {

                        string[] words = line.Split(',');
                        ObjectEtagRegisterModel etag = new ObjectEtagRegisterModel();
                        etag.MaETag = words[0].Trim();
                        etag.GiaVe = words[1].Trim();
                        etag.MaVe = words[2].Trim();
                        etag.SoXeNhanDang = words[3].Trim();
                        etag.NgayBatDau = DateTime.Parse(words[4].Trim());
                        etag.NgayKetThuc = DateTime.Parse( words[5].Trim());
                        etag.MaNhanVien = words[6].Trim();
                        list.Add(etag);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error(ex);
                    }

                }
            }
           
           // file.Close();
            return list;
        }

        public void AddEtagRegister(ObjectEtagRegisterModel Etagregister)
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_ETAGREGISTER;


                cmd.Parameters.Add("@MaETag", SqlDbType.VarChar,50).Value = Etagregister.MaETag;
                cmd.Parameters.Add("@GiaVe", SqlDbType.VarChar,50).Value = Etagregister.GiaVe;
                cmd.Parameters.Add("@MaLoaiVe", SqlDbType.VarChar,50).Value = Etagregister.MaVe;
                cmd.Parameters.Add("@SoXe", SqlDbType.VarChar,50).Value = Etagregister.SoXeNhanDang;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value = Etagregister.NgayBatDau;
                cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value = Etagregister.NgayKetThuc;
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.VarChar,50).Value = Etagregister.MaNhanVien;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
               // NLogHelper.Info("ADD STORE_ADD_ETAGREGISTER");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        //UPDATE EtagRegister
        public void UpdateEtagRegister(ObjectEtagRegisterModel Etagregister)
        {
            try
            {


                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_ETAGREGISTER;


                cmd.Parameters.Add("@MaETag", SqlDbType.VarChar,50).Value = Etagregister.MaETag;
                cmd.Parameters.Add("@GiaVe", SqlDbType.VarChar,50).Value = Etagregister.GiaVe;
                cmd.Parameters.Add("@MaLoaiVe", SqlDbType.VarChar,50).Value = Etagregister.MaVe;
                cmd.Parameters.Add("@SoXe", SqlDbType.VarChar,50).Value = Etagregister.SoXeNhanDang;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value = Etagregister.NgayBatDau;
                cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value = Etagregister.NgayKetThuc;
                cmd.Parameters.Add("@MaNhanVien", SqlDbType.VarChar,50).Value = Etagregister.MaNhanVien;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
              //  NLogHelper.Info("UPDATE STORE_UPDATE_ETAGREGISTER");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        //DELETE EtagRegister
        public void DeleteEtagRegister(ObjectEtagRegisterModel Etagregister)
        {
            try
            {


                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_ETAGREGISTER;


                cmd.Parameters.Add("@MaETag", SqlDbType.VarChar, 50).Value = Etagregister.MaETag;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
               // NLogHelper.Info("DETELE STORE_DETELE_ETAGREGISTER");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }

        public void ProcessEtagRegisterMTC()
        {
            try
            {
                _localPath = _localPath + "/" + DateTime.Now.ToString("yyyyMMdd");
                _remotePath = _remotePath + "/" + DateTime.Now.ToString("yyyyMMdd");
                //download
                if (!Directory.Exists(_localPath))
                {
                    Directory.CreateDirectory(_localPath);
                }
                List<string> files = _fileTransferFtp.DownloadDirectory(_localPath, _remotePath);

                foreach (var item in files)

                {

                    // Lấy danh sách các đối tượng
                    List<ObjectEtagRegisterModel> listEtagregister = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (ObjectEtagRegisterModel t in listEtagregister)
                    {
                        if (item.Contains("INSERT"))
                        {
                            AddEtagRegister(t);
                            
                        }
                        if (item.Contains("UPDATE"))
                        {
                            UpdateEtagRegister(t);

                        }
                        if (item.Contains("DELETE"))
                        {
                            DeleteEtagRegister(t);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }


        }


        #endregion
    }

}
