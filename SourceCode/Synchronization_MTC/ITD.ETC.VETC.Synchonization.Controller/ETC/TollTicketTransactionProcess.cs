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

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class TollTicketTransactionProcess
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
        public TollTicketTransactionProcess(string tableName, string tollTicketLocalFolderPath, string tollTicketFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = tollTicketLocalFolderPath;
                _remotePath = tollTicketFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
       

        public void TollTicketProcessData()
        {
            try
            {
                // Get data from data base
                DataTable dt = getTollTicketTransactionData();

                List<TollTicketTransactionModel> listItem = new List<TollTicketTransactionModel>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TollTicketTransactionModel tollTicket = new TollTicketTransactionModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        tollTicket.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
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
                                    tollTicket.TID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSLANE");
                                if (selectSingleNode != null)
                                    tollTicket.MSLANE = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GIAVE");
                                if (selectSingleNode != null)
                                    tollTicket.GIAVE = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GIOSOAT");
                                if (selectSingleNode != null)
                                    tollTicket.GIOSOAT = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("NGAYSOAT");
                                if (selectSingleNode != null)
                                    tollTicket.NGAYSOAT = selectSingleNode.InnerText;

                                selectSingleNode = emElement.SelectSingleNode("Login");
                                if (selectSingleNode != null)
                                    tollTicket.Login = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Ca");
                                if (selectSingleNode != null)
                                    tollTicket.Ca = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSLoaive");
                                if (selectSingleNode != null)
                                    tollTicket.MSLoaive = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSloaixe");
                                if (selectSingleNode != null)
                                    tollTicket.MSloaixe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Checker");
                                if (selectSingleNode != null)
                                    tollTicket.Checker = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SoXe_ND");
                                if (selectSingleNode != null)
                                    tollTicket.SoXe_ND = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F0");
                                if (selectSingleNode != null)
                                    tollTicket.F0 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F1");
                                if (selectSingleNode != null)
                                    tollTicket.F1 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F2");
                                if (selectSingleNode != null)
                                    tollTicket.F2 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ImageID");
                                if (selectSingleNode != null)
                                    tollTicket.ImageID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SyncEtcMtc");
                                if (selectSingleNode != null)
                                    tollTicket.SyncEtcMtc = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    tollTicket.SyncFeBe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("EtagID");
                                if (selectSingleNode != null)
                                    tollTicket.EtagID = selectSingleNode.InnerText;

                                listItem.Add(tollTicket);
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
                    string fileName = saveTollTicket2File(item);

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

        /// <summary>
        /// Update Sync Status
        /// </summary>
        /// <param name="item"></param>
        private void updateSyncStatus(TollTicketTransactionModel item)
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
        private string saveTollTicket2File(TollTicketTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                DateTime.TryParse(item.NGAYSOAT, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", item.TID, item.SoXe_ND,
                    item.MSLANE, date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.TID + ",");
                sb.Append(item.MSLANE + ",");
                sb.Append(item.GIAVE + ",");
                sb.Append(item.GIOSOAT + ",");
                sb.Append(item.NGAYSOAT + ",");
                sb.Append(item.Login + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MSLoaive + ",");
                sb.Append(item.MSloaixe);
                sb.Append(item.Checker + ",");
                sb.Append(item.SoXe_ND + ",");
                sb.Append(item.F0 + ",");
                sb.Append(item.F1 + ",");
                sb.Append(item.F2 + ",");
                sb.Append(item.ImageID + ",");
                sb.Append(item.SyncEtcMtc + ",");
                sb.Append(item.SyncFeBe + ",");
                sb.Append(item.EtagID);

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
        private DataTable getTollTicketTransactionData()
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
