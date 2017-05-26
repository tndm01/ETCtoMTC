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

        #region Method
     
       
       
      
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
                                    etag.NgayBatDau = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("NgayKetThuc");
                                if (selectSingleNode != null)
                                    etag.NgayKetThuc = selectSingleNode.InnerText;
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
                DateTime date = DateTime.Now;
                DateTime.TryParse(item.NgayBatDau, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", "EtagThang", item.SoXeNhanDang, item.Action, date.ToString("yyyyMMddHHmmss"));
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
                sb.Append(item.NgayBatDau + ",");
                sb.Append(item.NgayKetThuc + ",");
                sb.Append(item.MaNhanVien );


                // Write to File
                File.WriteAllText(filepath, sb.ToString());
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

    }

}
