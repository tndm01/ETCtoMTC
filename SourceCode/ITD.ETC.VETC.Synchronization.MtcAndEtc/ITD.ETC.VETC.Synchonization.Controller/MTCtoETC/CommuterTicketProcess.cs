using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;

namespace ITD.ETC.VETC.Synchonization.Controller.MTCtoETC
{
    public class CommuterTicketProcess 
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

        public CommuterTicketProcess(string tableName, string fullSourcePath, string fullDesticationPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = fullSourcePath;
                _remotePath = fullDesticationPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #endregion

        #region ETC

        /// <summary>
        /// Hàm đọc file luu mỗi dòng thành 1 đối tượng
        /// </summary>
        /// <param name="path">đường dẫn file cần đọc</param>
        /// <returns>trả về 1 danh sách các đối tượng ObjectCommuterTicket </returns>
        public List<ObjectCommuterTicket> ReadFile(string path)
        {
            string line;
            List<ObjectCommuterTicket> list = new List<ObjectCommuterTicket>();

            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    ObjectCommuterTicket obCommuterTicket = new ObjectCommuterTicket();
                    obCommuterTicket.TID = words[0].Trim();
                    obCommuterTicket.GIAVE = Int32.Parse(words[1].Trim());
                    obCommuterTicket.MSLOAIVE = words[2].Trim();
                    obCommuterTicket.MSloaixe = words[3].Trim();
                    obCommuterTicket.SoDangKiem = words[4].Trim();
                    obCommuterTicket.SOXE = words[4].Trim();
                    obCommuterTicket.NGAYBD = DateTime.Parse(words[5].Trim());
                    obCommuterTicket.NGAYKT = DateTime.Parse(words[6].Trim());
                    obCommuterTicket.NGAYBAN = DateTime.Parse(words[7].Trim());
                    obCommuterTicket.GIOBAN = Int32.Parse(words[8].Trim());
                    obCommuterTicket.Ca = words[9].Trim();
                    obCommuterTicket.LOGIN = words[10].Trim();
                    obCommuterTicket.KH = words[11].Trim();
                    obCommuterTicket.DCKH = words[12].Trim();
                    list.Add(obCommuterTicket);
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }

            file.Close();
            return list;

        }


        /// <summary>
        /// Hàm lưu 1 đối tượng xuống database
        /// </summary>
        /// <param name="CommuterTicket">đối tượng cần lưu</param>
        public void AddCommuterTicket(ObjectCommuterTicket CommuterTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_COMMUTER_TICKET;

                //dien size
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = CommuterTicket.TID;
                cmd.Parameters.Add("@GIOBAN", SqlDbType.Int).Value = CommuterTicket.GIOBAN;
                cmd.Parameters.Add("@NGAYBAN", SqlDbType.DateTime).Value = CommuterTicket.NGAYBAN;
                cmd.Parameters.Add("@NGAYBD", SqlDbType.DateTime).Value = CommuterTicket.NGAYBD;
                cmd.Parameters.Add("@NGAYKT", SqlDbType.DateTime).Value = CommuterTicket.NGAYKT;
                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = CommuterTicket.SOXE;
                cmd.Parameters.Add("@KH", SqlDbType.VarChar, 100).Value = CommuterTicket.KH;
                cmd.Parameters.Add("@DCKH", SqlDbType.VarChar, 200).Value = CommuterTicket.DCKH;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = CommuterTicket.GIAVE;
                cmd.Parameters.Add("@MSLOAIVE", SqlDbType.Char, 2).Value = CommuterTicket.MSLOAIVE;
                cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar, 20).Value = CommuterTicket.LOGIN;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = CommuterTicket.Ca;
                cmd.Parameters.Add("@MSloaixe", SqlDbType.Char, 2).Value = CommuterTicket.MSloaixe;
                cmd.Parameters.Add("@HaTai", SqlDbType.Int).Value = CommuterTicket.HaTai;
                cmd.Parameters.Add("@SoDangKiem", SqlDbType.VarChar, 20).Value = CommuterTicket.SoDangKiem;
                cmd.Parameters.Add("@Expired", SqlDbType.Bit).Value = CommuterTicket.Expired;
                cmd.Parameters.Add("@ChuyenKhoan", SqlDbType.Int).Value = CommuterTicket.ChuyenKhoan;
                cmd.Parameters.Add("@MSTram", SqlDbType.Char, 1).Value = CommuterTicket.MSTram;
                //parameter khac null
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void UpdateCommuterTicket(ObjectCommuterTicket CommuterTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_COMMUTER_TICKET;

                //dien size
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = CommuterTicket.TID;
                cmd.Parameters.Add("@GIOBAN", SqlDbType.Int).Value = CommuterTicket.GIOBAN;
                cmd.Parameters.Add("@NGAYBAN", SqlDbType.DateTime).Value = CommuterTicket.NGAYBAN;
                cmd.Parameters.Add("@NGAYBD", SqlDbType.DateTime).Value = CommuterTicket.NGAYBD;
                cmd.Parameters.Add("@NGAYKT", SqlDbType.DateTime).Value = CommuterTicket.NGAYKT;
                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = CommuterTicket.SOXE;
                cmd.Parameters.Add("@KH", SqlDbType.VarChar, 100).Value = CommuterTicket.KH;
                cmd.Parameters.Add("@DCKH", SqlDbType.VarChar, 200).Value = CommuterTicket.DCKH;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = CommuterTicket.GIAVE;
                cmd.Parameters.Add("@MSLOAIVE", SqlDbType.Char, 2).Value = CommuterTicket.MSLOAIVE;
                cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar, 20).Value = CommuterTicket.LOGIN;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = CommuterTicket.Ca;
                cmd.Parameters.Add("@MSloaixe", SqlDbType.Char, 2).Value = CommuterTicket.MSloaixe;
                cmd.Parameters.Add("@HaTai", SqlDbType.Int).Value = CommuterTicket.HaTai;
                cmd.Parameters.Add("@SoDangKiem", SqlDbType.VarChar, 20).Value = CommuterTicket.SoDangKiem;
                cmd.Parameters.Add("@Expired", SqlDbType.Bit).Value = CommuterTicket.Expired;
                cmd.Parameters.Add("@ChuyenKhoan", SqlDbType.Int).Value = CommuterTicket.ChuyenKhoan;
                cmd.Parameters.Add("@MSTram", SqlDbType.Char, 1).Value = CommuterTicket.MSTram;
                //parameter khac null
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void DeleteCommuterTicket(ObjectCommuterTicket CommuterTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_COMMUTER_TICKET;

                //dien size
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = CommuterTicket.TID;
                
                //parameter khac null
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void ProcessCommuterTicket()
        {
            //try catch
            try
            {
                //tạo thư mục theo ngày
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
                    List<ObjectCommuterTicket> listCommuterTicket = ReadFile(item);
                    //try ctach readfile
                    // Insert dữ liệu vào database
                    foreach (ObjectCommuterTicket t in listCommuterTicket)
                    {
                        if (item.Contains("INSERT"))
                        {
                            AddCommuterTicket(t);
                        }
                        if (item.Contains("UPDATE"))
                        {
                            UpdateCommuterTicket(t);
                        }
                        if (item.Contains("DELETE"))
                        {
                            DeleteCommuterTicket(t);
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
        #region MTC
        public void ProcessCommuterTicketMTC()
        {
            try 
            { 
                //Lay du lieu tu Database
                CommuterTicketProcessDataMTC();

                //Luu vao file

                //Upload
            }
            catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void CommuterTicketProcessDataMTC()
        {
            try
            {
                // Get data from data base
                DataTable dt = getCommuterTicketDataMTC();
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() + "row");
                List<ObjectCommuterTicket> listItem = new List<ObjectCommuterTicket>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectCommuterTicket commuterTicket = new ObjectCommuterTicket();

                        string xml = row["DataTracking"].ToString().Trim();
                        //gan tracking id
                        commuterTicket.TrackingId = (long)row["TrackingID"];
                        //Int64.Parse(row["TrackingID"].ToString());
                        commuterTicket.Action = (string)row["Action"];
                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader rd = new StringReader(xml);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            XmlElement emElement = doc.DocumentElement;

                            if (emElement != null)
                            {
                                var selectSingleNode = emElement.SelectSingleNode("TID");
                                if (selectSingleNode != null)
                                    commuterTicket.TID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GIOBAN");
                                if (selectSingleNode != null)
                                    commuterTicket.GIOBAN = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NGAYBAN");
                                if (selectSingleNode != null)
                                    commuterTicket.NGAYBAN = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NGAYBD");
                                if (selectSingleNode != null)
                                    commuterTicket.NGAYBD = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NGAYKT");
                                if (selectSingleNode != null)
                                    commuterTicket.NGAYKT = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SOXE");
                                if (selectSingleNode != null)
                                    commuterTicket.SOXE = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("KH");
                                if (selectSingleNode != null)
                                    commuterTicket.KH = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("DCKH");
                                if (selectSingleNode != null)
                                    commuterTicket.DCKH = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GIAVE");
                                if (selectSingleNode != null)
                                    commuterTicket.GIAVE = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("MSLOAIVE");
                                if (selectSingleNode != null)
                                    commuterTicket.MSLOAIVE = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("LOGIN");
                                if (selectSingleNode != null)
                                    commuterTicket.LOGIN = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Ca");
                                if (selectSingleNode != null)
                                    commuterTicket.Ca = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSloaixe");
                                if (selectSingleNode != null)
                                    commuterTicket.MSloaixe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("HaTai");
                                if (selectSingleNode != null)
                                    commuterTicket.HaTai = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SoDangKiem");
                                if (selectSingleNode != null)
                                    commuterTicket.SoDangKiem = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Expired");
                                if (selectSingleNode != null)
                                    commuterTicket.Expired = selectSingleNode.InnerText != "1" ? false : true;//Boolean.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("ChuyenKhoan");
                                if (selectSingleNode != null)
                                    commuterTicket.ChuyenKhoan = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("MSTram");
                                if (selectSingleNode != null)
                                    commuterTicket.MSTram = selectSingleNode.InnerText;

                                listItem.Add(commuterTicket);
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
                    string fileName = saveCommuterTicket2FileMTC(item);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        // Ftp transfer
                        //_fileTransferFtp.UploadFile(fileName, _remotePath);

                        // update database
                        updateSyncStatusMTC(item);
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
        private void updateSyncStatusMTC(ObjectCommuterTicket item)
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
        private string saveCommuterTicket2FileMTC(ObjectCommuterTicket item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
              
                //THEM ACTION
                string mave = item.TID.Replace("/", "");
                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}_{5}.txt","VETHANG",mave, item.MSLOAIVE, item.SOXE,item.Action,
                     item.NGAYBAN.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.TID + ",");
                sb.Append(item.GIAVE + ",");
                sb.Append(item.MSLOAIVE + ",");
                sb.Append(item.MSloaixe + ",");
                sb.Append(item.SOXE + ",");
                sb.Append(item.NGAYBD + ",");
                sb.Append(item.NGAYKT + ",");
                sb.Append(item.NGAYBAN + ",");
                sb.Append(item.GIOBAN + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.LOGIN + ",");
                sb.Append(item.KH + ",");
                sb.Append(item.DCKH);

                // Write to File
                File.WriteAllText(filepath, sb.ToString());
                NLogHelper.Info("Success Write to File :" + filepath);
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
        private DataTable getCommuterTicketDataMTC()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_GET_SYNC_DATA;
                cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = _databaseTableName;

                DataTable dt = _mydatabaseHelper.GetDataTable(cmd);
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() + "row");
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return null;
        }
        #endregion
    }
}
