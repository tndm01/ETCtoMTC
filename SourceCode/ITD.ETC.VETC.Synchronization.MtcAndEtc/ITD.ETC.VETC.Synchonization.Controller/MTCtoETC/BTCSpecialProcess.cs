using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;

namespace ITD.ETC.VETC.Synchonization.Controller.MTCtoETC
{
    public class BTCSpecialProcess
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

        public BTCSpecialProcess(string tableName, string fullSourcePath, string fullDesticationPath)
        {
            _databaseTableName = tableName;
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion Field

        #region ETC

        private void updateSyncStatus(BTCSpecialTransactionModel item)
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

        private string savesBTCSpecial2File(BTCSpecialTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = item.ThoiGianGiaoDich;

                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", "BTC", item.ImageID,
                   item.MsLane, date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.ImageID + ",");
                sb.Append(item.ThoiGianGiaoDich.ToString("HH:mm:ss") + ",");
                sb.Append(item.ID + ",");
                sb.Append(item.LoaiXe + ",");
                sb.Append(item.Soxe_ND + ",");
                sb.Append(item.Login + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MsLane + ",");
                sb.Append(item.SyncEtcMtc + ",");
                sb.Append(item.SyncFebe);
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

        private DataTable getBTCSpecialTransactionData()
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

        public void ProcessBTCSpecialETC()
        {
            try
            {
                // Get data from data base
                DataTable dt = getBTCSpecialTransactionData();

                List<BTCSpecialTransactionModel> listItem = new List<BTCSpecialTransactionModel>();
                // process data
                if (dt != null)
                {
                    NLogHelper.Info(string.Format("Get {0} record in table {1}", dt.Rows.Count.ToString(), _databaseTableName));
                    foreach (DataRow row in dt.Rows)
                    {
                        BTCSpecialTransactionModel specialBTC = new BTCSpecialTransactionModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        specialBTC.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
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
                                    specialBTC.ImageID = selectSingleNode.InnerText;
                                    DateTime dtran = Utility.GetDateTimefromTranID(specialBTC.ImageID);
                                    specialBTC.ThoiGianGiaoDich = DateTime.Parse(dtran.ToString("HH:mm:ss"));
                                }
                                selectSingleNode = emElement.SelectSingleNode("Checker");
                                if (selectSingleNode != null)
                                    specialBTC.Checker = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ID");
                                if (selectSingleNode != null)
                                    specialBTC.ID = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Loaixe");
                                if (selectSingleNode != null)
                                    specialBTC.LoaiXe = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Soxe_ND");
                                if (selectSingleNode != null)
                                    specialBTC.Soxe_ND = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Login");
                                if (selectSingleNode != null)
                                    specialBTC.Login = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Ca");
                                if (selectSingleNode != null)
                                    specialBTC.Ca = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("MSLane");
                                if (selectSingleNode != null)
                                    specialBTC.MsLane = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("SyncEtcMtc");
                                if (selectSingleNode != null)
                                    specialBTC.SyncEtcMtc = int.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    specialBTC.SyncFebe = int.Parse(selectSingleNode.InnerText);
                                listItem.Add(specialBTC);
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
                    string fileName = savesBTCSpecial2File(item);
                    if (!string.IsNullOrEmpty(fileName))
                    {
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

        #endregion 

        #region MTC

        public List<BTCSpecialTransactionModel> ReadFile(string path)
        {
            List<BTCSpecialTransactionModel> list = new List<BTCSpecialTransactionModel>();

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                try
                {
                    if (line != "")
                    {
                        string[] words = line.Split(',');
                        BTCSpecialTransactionModel obSpecial = new BTCSpecialTransactionModel();
                        obSpecial.ImageID = words[0].Trim();
                        obSpecial.ThoiGianGiaoDich = DateTime.Parse(words[1].Trim());
                        obSpecial.ID = words[2].Trim();
                        obSpecial.LoaiXe = words[3].Trim();
                        obSpecial.Soxe_ND = words[4].Trim();
                        obSpecial.Login = words[5].Trim();
                        obSpecial.Ca = words[6].Trim();
                        obSpecial.MsLane = words[7].Trim();
                        obSpecial.SyncEtcMtc = int.Parse(words[8].Trim());
                        obSpecial.SyncFebe = int.Parse(words[9].Trim());

                        list.Add(obSpecial);
                    }
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }
            return list;
        }

        public void AddBTCSpecial(BTCSpecialTransactionModel obBTCSpecial)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_SOATVE_BTC;
                cmd.Parameters.Add("@ID", SqlDbType.VarChar, 20).Value = obBTCSpecial.ID;
                cmd.Parameters.Add("@LoaiXe", SqlDbType.Char, 2).Value = obBTCSpecial.LoaiXe;
                cmd.Parameters.Add("@MsLane", SqlDbType.Char, 3).Value = obBTCSpecial.MsLane;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar, 20).Value = obBTCSpecial.Login;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = obBTCSpecial.Ca;
                cmd.Parameters.Add("@Checker", SqlDbType.VarChar, 20).Value = obBTCSpecial.ThoiGianGiaoDich.ToString("HH:mm:ss");
                cmd.Parameters.Add("@F0", SqlDbType.Char, 1).Value = 0;
                cmd.Parameters.Add("@F1", SqlDbType.Char, 1).Value = 0;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = 0;
                cmd.Parameters.Add("@Soxe_ND", SqlDbType.NVarChar, 20).Value = obBTCSpecial.Soxe_ND;
                cmd.Parameters.Add("@ImageID", SqlDbType.NVarChar, 20).Value = obBTCSpecial.ImageID;
                cmd.Parameters.Add("@SyncEtcMtc", SqlDbType.Int).Value = obBTCSpecial.SyncEtcMtc;
                cmd.Parameters.Add("@SyncFeBe", SqlDbType.Int).Value = obBTCSpecial.SyncFebe;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("Insert success ADD STORE_ADD_SOATVE_BTC");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void ProcessBTCSpecialMTC()
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
                    List<BTCSpecialTransactionModel> listSpecial = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (BTCSpecialTransactionModel t in listSpecial)
                    {
                        AddBTCSpecial(t);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #endregion MTC
    }
}