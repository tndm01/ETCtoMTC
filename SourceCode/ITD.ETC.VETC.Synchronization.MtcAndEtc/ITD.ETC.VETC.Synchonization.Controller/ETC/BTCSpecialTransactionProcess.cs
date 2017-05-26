using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class BTCSpecialTransactionProcess
    {
        #region Field

        private string _databaseTableName;

        // object data base
        private DataBaseHelper _mydatabaseHelper;

        // object fileTransferFtp
        private FileTransferFtp _fileTransferFtp;

        // local path
        private string _localPath;

        // remotepath
        private string _remotePath;
        #endregion

        #region Method

        public BTCSpecialTransactionProcess(string tableName, string specialLocalFolderPath, string specialFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = specialLocalFolderPath;
                _remotePath = specialFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        private string saveEtag2File(BTCSpecialTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                DateTime.TryParse(item.Ngayqua, out date);

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
                sb.Append(item.ThoiGianGiaoDich + ",");
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

        private DataTable getSpecialTransactionData()
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

        public void SpecialBTCProcessData()
        {
            try
            {
                // Get data from data base
                DataTable dt = getSpecialTransactionData();

                List<BTCSpecialTransactionModel> listItem = new List<BTCSpecialTransactionModel>();
                // process data
                if (dt != null)
                {
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
                    string fileName = saveEtag2File(item);

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
    }
}
