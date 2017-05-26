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

        //public void EtagProcess()
        //{
        //    try
        //    {
        //        // lay du lieu tu data base
        //        DataTable dt = GetData();
        //        // parser  file
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            //_oETag = ParseToObject(row);
        //            ETagTransactionModel abcd = ParseToObject(row);
        //            string fileName = _localPath + dt.Rows.IndexOf(row).ToString() + ".txt";
        //            SaveFile(abcd, fileName);
        //        }
        //        List<string> results = Upload(_localPath, _remotePath, _isFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //    }


        //}
        /// <summary>
        /// getdata  từ database của bảng etag
        /// </summary>
        /// <returns></returns>
        //public DataTable GetData()
        //{
        //    try
        //    {
        //        _mydatabaseHelper = new DataBaseHelper(@"Data Source=VIET;Initial Catalog=SyncModule;Integrated Security=True");
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "ETAGTEST_SELECT";
        //        DataTable dt = _mydatabaseHelper.GetDataTable(cmd);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //    }
        //    return null;
        //}
        /// <summary>
        /// gán dữ liệu lấy được ở database cho đối tượng objectETag
        /// </summary>
        /// <param name="row">lấy dữ liệu ở mỗi dòng trong datatable</param>
        /// <returns>trả về đối tượng etag</returns>
        //public ETagTransactionModel ParseToObject(DataRow row)
        //{
        //    try
        //    {
        //        ETagTransactionModel abcd = new ETagTransactionModel();
        //        abcd.MaGiaoDich = row["MaGiaoDich"].ToString().Trim();
        //        abcd.ThoiGianGiaoDich = row["ThoiGianGiaoDich"].ToString().Trim();
        //        abcd.MaETag = row["MaETag"].ToString().Trim();
        //        abcd.SoXeNhanDang = row["SoXeNhanDang"].ToString().Trim();
        //        abcd.MaNhanVien = row["MaNhanVien"].ToString().Trim();
        //        abcd.MaCa = row["MaCa"].ToString().Trim();
        //        abcd.MaLan = row["MaLan"].ToString().Trim();
        //        abcd.MaVe = row["MaVe"].ToString().Trim();
        //        return abcd;
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //        return null;
        //    }
        //}
        /// <summary>
        /// ghi file xuống địa chỉ local
        /// </summary>
        /// <param name="oETag">đối tượng Etag</param>
        /// <param name="filePath">tên file</param>
        //public void SaveFile(ETagTransactionModel oETag, string fileName)
        //{
            
        //    try
        //    {
        //        using (StreamWriter sw = new StreamWriter(fileName))
        //        {
        //            sw.Write(oETag.MaGiaoDich.ToString() + ",");
        //            sw.Write(oETag.ThoiGianGiaoDich.ToString() + ",");
        //            sw.Write(oETag.MaETag.ToString() + ",");
        //            sw.Write(oETag.GiaVe.ToString() + ",");
        //            sw.Write(oETag.SoXeNhanDang.ToString() + ",");
        //            sw.Write(oETag.MaNhanVien.ToString() + ",");
        //            sw.Write(oETag.MaCa.ToString() + ",");
        //            sw.Write(oETag.MaLan.ToString() + ",");
        //            sw.Write(oETag.MaVe.ToString() + ";");
        //            sw.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //    }
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
        public void EtagProcessData()
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
                                    etag.ThoiGianGiaoDich = dtran.ToString("HH:mm:ss");
                                }
                                //selectSingleNode = emElement.SelectSingleNode("CheckDate");
                                //if (selectSingleNode != null)
                                //    etag.ThoiGianGiaoDich = selectSingleNode.InnerText.Replace("T", " ");
                                selectSingleNode = emElement.SelectSingleNode("ObuID");
                                if (selectSingleNode != null)
                                    etag.MaETag = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ChargeAmount");
                                if (selectSingleNode != null)
                                    etag.GiaVe = selectSingleNode.InnerText;
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
                    }


                }
                //foreach (var item in listItem)
                //{
                //    //Create image subfolder
                //    string pathImage = Image(item.MaGiaoDich, item.MaLan);
                //    string localPathHinhLan = _localPath + "HinhLan/" + pathImage + item.MaGiaoDich + "_" + item.MaLan + ".JPG";
                //    //string localPathHinhLan = @"F:\ThuMucLocal\HinhLan\2016090107\abcdef\20160901070908_abcdef.JPG";
                //    string remotePathHinhLan = _remotePath + "HinhLan/" + pathImage + "/";
                //    _fileTransferFtp.UploadFile(localPathHinhLan, remotePathHinhLan);

                //    string localPathHinhND = _localPath + "HinhND/" + pathImage + item.MaGiaoDich + "_" + item.MaLan + ".JPG";
                //    string remotePathHinhND = _remotePath + "HinhND/" + pathImage + "/";
                //    _fileTransferFtp.UploadFile(localPathHinhND, remotePathHinhND);
                //}

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
                DateTime date = DateTime.Now;
                DateTime.TryParse(item.ThoiGianGiaoDich, out date);
                
                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", item.MaETag, item.SoXeDangKy,
                    item.MaLan, date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.MaGiaoDich + ",");
                sb.Append(item.ThoiGianGiaoDich + ",");
                sb.Append(item.MaETag + ",");
                sb.Append(item.GiaVe + ",");
                sb.Append(item.SoXeNhanDang + ",");
                sb.Append(item.MaNhanVien + ",");
                sb.Append(item.MaCa + ",");
                sb.Append(item.MaLan + ",");
                sb.Append(item.MaVe);

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
        private string Image(string imageID, string laneID)
        {
            //yyyy MM dd HH mm ss
            //2016 09 01 07 09 08
            //0123 45 67 89 1011 1213
            string yyyy = imageID.Substring(0, 4);
            string MM = imageID.Substring(4, 2);
            string dd = imageID.Substring(6, 2);
            string HH = imageID.Substring(8, 2);
            //parse ra hh giờ từ 0 đến 12
            string hh = null;
            if(int.Parse(HH) > 12)
            {
                hh = (int.Parse(HH) - 12).ToString();
            }
            string mm = imageID.Substring(10, 2);
            string ss = imageID.Substring(12, 2);
            //nếu imageID có hơn 14 ký tự
            string etagID = null;
            if (imageID.Length > 14)
            {
                etagID = imageID.Substring(14);
            }
            string imagePath = null;
            //yyyyMMddhh\LLL\yyyyMMddHHmmss_LLL.jpg
            imagePath = yyyy + MM + dd + hh + "/" + laneID + "/";
            return imagePath;
        }
        #endregion
    }
}
