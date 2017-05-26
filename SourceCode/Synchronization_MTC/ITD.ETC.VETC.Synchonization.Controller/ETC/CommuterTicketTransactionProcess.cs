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
    public class CommuterTicketTransactionProcess
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

        #endregion

        #region Method

        public CommuterTicketTransactionProcess(string tableName, string commuterTicketLocalFolderPath, string commuterTickKetFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = commuterTicketLocalFolderPath;
                _remotePath = commuterTickKetFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        

        public void CommuterTicketProcessData()
        {
            try
            {
                // Get data from data base
                DataTable dt = getCommuterTicketTransactionData();

                List<CommuterTicketTransactionModel> listItem = new List<CommuterTicketTransactionModel>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CommuterTicketTransactionModel commuterTicket = new CommuterTicketTransactionModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        commuterTicket.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
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
                                selectSingleNode = emElement.SelectSingleNode("GIOSOAT");
                                if (selectSingleNode != null)
                                    commuterTicket.GIOSOAT = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("NGAYSOAT");
                                if (selectSingleNode != null)
                                    commuterTicket.NGAYSOAT = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSLANE");
                                if (selectSingleNode != null)
                                    commuterTicket.MSLANE = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Login");
                                if (selectSingleNode != null)
                                    commuterTicket.Login = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Ca");
                                if (selectSingleNode != null)
                                    commuterTicket.Ca = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSLoaive");
                                if (selectSingleNode != null)
                                    commuterTicket.MSLoaive = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSloaixe");
                                if (selectSingleNode != null)
                                    commuterTicket.MSloaixe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Checker");
                                if (selectSingleNode != null)
                                    commuterTicket.Checker = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SoXe_ND");
                                if (selectSingleNode != null)
                                    commuterTicket.SoXe_ND = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F0");
                                if (selectSingleNode != null)
                                    commuterTicket.F0 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F1");
                                if (selectSingleNode != null)
                                    commuterTicket.F1 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("F2");
                                if (selectSingleNode != null)
                                    commuterTicket.F2 = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("ImageID");
                                if (selectSingleNode != null)
                                    commuterTicket.ImageID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SyncEtcMtc");
                                if (selectSingleNode != null)
                                    commuterTicket.SyncEtcMtc = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    commuterTicket.SyncFeBe = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("EtagID");
                                if (selectSingleNode != null)
                                    commuterTicket.EtagID = selectSingleNode.InnerText;

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
                    string fileName = saveCommuterTicket2File(item);

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
        private void updateSyncStatus(CommuterTicketTransactionModel item)
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
        private string saveCommuterTicket2File(CommuterTicketTransactionModel item)
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
                sb.Append(item.GIOSOAT + ",");
                sb.Append(item.NGAYSOAT + ",");
                sb.Append(item.MSLANE + ",");
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
        private DataTable getCommuterTicketTransactionData()
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
