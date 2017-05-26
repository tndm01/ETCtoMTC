using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class EtagTransactionProcess 
    {
        #region Field
        private string _databaseTableName;

        // object data base
        private DataBaseHelper _mydatabaseHelper;

        // object fileTransferFtp
        private FileTransferFtp _fileTransferFtp;

        // object eTag
        //private ETagTransactionModel _oETag;
        // local path lưu dữ liệu Etag
        private string _localPath;

        // remotepath lưu dữ liệu Etag trên server
        private string _remotePath;

        #endregion

        #region Method
        //public EtagTransactionProcess()
        //{
        //    _mydatabaseHelper = new DataBaseHelper(@"Data Source=VIET;Initial Catalog=SyncModule;Integrated Security=True");
        //    _fileTransferFtp = new FileTransferFtp();
        //    _localPath = @"F:\ThuMucLocal\";
        //    _remotePath = "/";
        //    _isFile = false;
        //}

        public EtagTransactionProcess(string tableName, string etagLocalFolderPath, string etagFtpFolderPath)
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
        //}
        /// <summary>
        /// Hàm up load sử dụng ftp
        /// </summary>
        /// <param name="localPath">địa chỉ thư mục local</param>
        /// <param name="remotePath">địa chỉ thư mục hoặc file trên server</param>
        /// <param name="isFile">địa chỉ file hay thư mục, nếu là file vẫn trả về list string 1 phần tử</param>
        /// <returns></returns>
        public List<string> Upload(string localPath, string remotePath, bool isFile)
        {
            List<string> results = null;
            if (isFile == true)
            {
                _fileTransferFtp.UploadFile(localPath, remotePath);
                results.Add(remotePath);
                return results;
            }
            else
            {
                results = _fileTransferFtp.UploadDirectory(localPath, remotePath);
                return results;
            }
        }
      

        /// <summary>
        /// Get Etag transaction Data from Database
        /// </summary>
        public void EtagProcessDataUp()
        {
            try
            {
                // Get data from data base
                DataTable dt = getEtagTransactionData();

                List<ETagTransactionModel> listItem = new List<ETagTransactionModel>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ETagTransactionModel etag = new ETagTransactionModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        etag.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader rd = new StringReader(xml);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            XmlElement emElement = doc.DocumentElement;

                            if (emElement != null)
                            {
                                var selectSingleNode = emElement.SelectSingleNode("ImageID");
                                if (selectSingleNode != null)
                                {
                                    etag.MaGiaoDich = selectSingleNode.InnerText;
                                    DateTime dtran = Utility.GetDateTimefromTranID(etag.MaGiaoDich);//yyyyMMddHHmmss
                                    //etag.ThoiGianGiaoDich = dtran.ToString("HH:mm:ss");
                                    etag.ThoiGianGiaoDich = dtran;
                                }
                                //selectSingleNode = emElement.SelectSingleNode("CheckDate");
                                //if (selectSingleNode != null)
                                //    etag.ThoiGianGiaoDich = selectSingleNode.InnerText.Replace("T", " ");
                                selectSingleNode = emElement.SelectSingleNode("ObuID");
                                if (selectSingleNode != null)
                                    etag.MaETag = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ChargeAmount");
                                if (selectSingleNode != null)
                                    etag.GiaVe = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("RecogPlateNumber");
                                if (selectSingleNode != null)
                                    etag.SoXeNhanDang = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("LoginID");
                                if (selectSingleNode != null)
                                    etag.MaNhanVien = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ShiftID");
                                if (selectSingleNode != null)
                                    etag.MaCa = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("LaneID");
                                if (selectSingleNode != null)
                                    etag.MaLan = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("TicketID");
                                if (selectSingleNode != null)
                                    etag.MaVe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("RegisPlateNumber");
                                if (selectSingleNode != null)
                                    etag.SoXeDangKy = selectSingleNode.InnerText;

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

                    if (!string.IsNullOrEmpty(fileName))
                    {
                         // Ftp transfer
                        //_fileTransferFtp.UploadFile(fileName, _remotePath);

                        // update database
                        updateSyncStatus(item);
                        NLogHelper.Info("Update sync status");
                    }
                }
                // ftp folder
                // performance
                // Need Create Sub folder by date in Ftp????
                _fileTransferFtp.UploadDirectory(_localPath, _remotePath);
                NLogHelper.Info("Upload form" + _localPath + " to " + _remotePath);

            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        /// <summary>
        /// Update Sync Status
        /// </summary>
        /// <param name="item"></param>
        private void updateSyncStatus(ETagTransactionModel item)
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
        private string saveEtag2File(ETagTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                //DateTime date = DateTime.Now;

                //DateTime.TryParse(item.ThoiGianGiaoDich, out date);
                
                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", "ETAG", item.SoXeDangKy,
                    item.MaLan, item.ThoiGianGiaoDich.ToString("yyyyMMddHHmmss"));
                //item.MaETag
                //date.ToString("yyyyMMddHHmmss")
                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.MaGiaoDich + ",");
                sb.Append(item.ThoiGianGiaoDich.ToString("HH:mm:ss") + ",");
                sb.Append(item.MaETag + ",");
                sb.Append(item.GiaVe + ",");
                sb.Append(item.SoXeNhanDang + ",");
                sb.Append(item.MaNhanVien + ",");
                sb.Append(item.MaCa + ",");
                sb.Append(item.MaLan + ",");
                sb.Append(item.MaVe);

                // Write to File
                File.WriteAllText(filepath, sb.ToString());
                NLogHelper.Info("Write Success" + filepath);
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
        private DataTable getEtagTransactionData()
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
        /// <summary>
        /// Đặt địa chỉ hình từ imageID
        /// </summary>
        /// <param name="imageID">mã giao dịch</param>
        /// <param name="laneID">mã làn</param>
        /// <returns></returns>
        //private string Image(string imageID, string laneID)
        //{
        //    //yyyy MM dd HH mm ss
        //    //2016 09 01 07 09 08
        //    //0123 45 67 89 1011 1213
        //    string yyyy = imageID.Substring(0, 4);
        //    string MM = imageID.Substring(4, 2);
        //    string dd = imageID.Substring(6, 2);
        //    string HH = imageID.Substring(8, 2);
        //    //parse ra hh giờ từ 0 đến 12
        //    string hh = null;
        //    if(int.Parse(HH) > 12)
        //    {
        //        hh = (int.Parse(HH) - 12).ToString();
        //    }
        //    string mm = imageID.Substring(10, 2);
        //    string ss = imageID.Substring(12, 2);
        //    //nếu imageID có hơn 14 ký tự
        //    string etagID = null;
        //    if (imageID.Length > 14)
        //    {
        //        etagID = imageID.Substring(14);
        //    }
        //    string imagePath = null;
        //    //yyyyMMddhh\LLL\yyyyMMddHHmmss_LLL.jpg
        //    imagePath = yyyy + MM + dd + hh + "/" + laneID + "/";
        //    return imagePath;
        //}
        public void EtagProcessDataDown()
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
                    List<ETagTransactionModel> listOETags = ReadFile(item);
                    //Insert dữ liệu vào database
                    foreach (ETagTransactionModel oETag in listOETags)
                    {
                        InsertEtagTransaction(oETag);
                        //if(item.Contains("INSERT"))
                        //    InsertEtagTransaction(oETag);
                        //if (item.Contains("UPDATE"))
                        //    UpdateEtagTransaction(oETag);
                        //if (item.Contains("DELETE"))
                        //    DeleteEtagTransaction(oETag);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public List<ETagTransactionModel> ReadFile(string path)
        {
            //string line;
            List<ETagTransactionModel> list = new List<ETagTransactionModel>();

            //StreamReader file = new StreamReader(path);
            //lấy số xe đăng ký ở tên file
            string[] name = path.Split('_');
            string SoXeDangKy = name[1];

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    try
                    {
                        string[] words = line.Split(',');
                        ETagTransactionModel oETagTransactionModel = new ETagTransactionModel();
                        oETagTransactionModel.MaGiaoDich = words[0].Trim();
                        oETagTransactionModel.ThoiGianGiaoDich = DateTime.Parse(words[1].Trim());
                        oETagTransactionModel.MaETag = words[2].Trim();
                        oETagTransactionModel.GiaVe = Int32.Parse(words[3].Trim());
                        oETagTransactionModel.SoXeNhanDang = words[4].Trim();
                        oETagTransactionModel.MaNhanVien = words[5].Trim();
                        oETagTransactionModel.MaCa = words[6].Trim();
                        oETagTransactionModel.MaLan = words[7].Trim();
                        oETagTransactionModel.MaVe = words[8].Trim();
                        oETagTransactionModel.SoXeDangKy = SoXeDangKy;
                        list.Add(oETagTransactionModel);
                        NLogHelper.Info("Add object ETagTransactionModel success");
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error(ex);
                    }
                }
            }
            return list;
        }
        public void InsertEtagTransaction(ETagTransactionModel oETagTransactionModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_TRP_tblCheckObuAccount_RFID;

                cmd.Parameters.Add("@ImageID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaGiaoDich;
                cmd.Parameters.Add("@CheckDate", SqlDbType.DateTime).Value = oETagTransactionModel.ThoiGianGiaoDich.ToString("HH:mm:ss");
                cmd.Parameters.Add("@ObuID", SqlDbType.VarChar, 50).Value = oETagTransactionModel.MaETag;
                cmd.Parameters.Add("@ChargeAmount", SqlDbType.Int).Value = oETagTransactionModel.GiaVe;
                cmd.Parameters.Add("@RecogPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeNhanDang;
                cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaNhanVien;
                cmd.Parameters.Add("@ShiftID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaCa;
                cmd.Parameters.Add("@LaneID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaLan;
                cmd.Parameters.Add("@TicketID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaVe;
                cmd.Parameters.Add("@RegisPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeDangKy;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("Insert success");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void UpdateEtagTransaction(ETagTransactionModel oETagTransactionModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_TRP_tblCheckObuAccount_RFID;

                cmd.Parameters.Add("@ImageID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaGiaoDich;
                cmd.Parameters.Add("@CheckDate", SqlDbType.DateTime).Value = oETagTransactionModel.ThoiGianGiaoDich;
                cmd.Parameters.Add("@ObuID", SqlDbType.VarChar, 50).Value = oETagTransactionModel.MaETag;
                cmd.Parameters.Add("@ChargeAmount", SqlDbType.Int).Value = oETagTransactionModel.GiaVe;
                cmd.Parameters.Add("@RecogPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeNhanDang;
                cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaNhanVien;
                cmd.Parameters.Add("@ShiftID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaCa;
                cmd.Parameters.Add("@LaneID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaLan;
                cmd.Parameters.Add("@TicketID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaVe;
                cmd.Parameters.Add("@RegisPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeDangKy;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("Update success");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void DeleteEtagTransaction(ETagTransactionModel oETagTransactionModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_TRP_tblCheckObuAccount_RFID;

                cmd.Parameters.Add("@ImageID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaGiaoDich;
                NLogHelper.Info("Delete success");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        #endregion
    }
}
