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

        #region ETC
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
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() + "row");

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
                        tollTicket.Action = (string)row["Action"];
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
                                    tollTicket.GIAVE = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("GIOSOAT");
                                if (selectSingleNode != null)
                                    tollTicket.GIOSOAT = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NGAYSOAT");
                                if (selectSingleNode != null)
                                    tollTicket.NGAYSOAT = DateTime.Parse(selectSingleNode.InnerText);

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
                                    tollTicket.SyncEtcMtc = Int32.Parse( selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    tollTicket.SyncFeBe = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("EtagID");
                                if (selectSingleNode != null)
                                    tollTicket.EtagID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSTRAM");
                                if (selectSingleNode != null)
                                    tollTicket.MSTRAM = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("KHUHOI");
                                if (selectSingleNode != null)
                                    tollTicket.KHUHOI = selectSingleNode.InnerText;

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
                string mave = item.TID.Replace("/","");
                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt","VELUOT",mave,item.MSLoaive,
                    item.MSLANE,item.NGAYSOAT.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                
               
                sb.Append(item.ImageID + ",");
                //sb.Append(item.NGAYSOAT.ToString("HH:mm:ss") + ",");
                sb.Append(Utility.GetDateTimefromTranID(item.ImageID).ToString("HH:mm:ss") + ",");
                sb.Append(item.TID + ",");
                sb.Append(item.GIAVE + ",");
                sb.Append(item.SoXe_ND + ",");
                sb.Append(item.Login + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MSLANE + ",");
                sb.Append(item.MSLoaive + ",");
                sb.Append(item.MSTRAM + ",");
                sb.Append(item.KHUHOI );
                

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
        private DataTable getTollTicketTransactionData()
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

        #region MTC
        public List<TollTicketTransactionModel> ReadFile(string path)
        {
            string line;
            List<TollTicketTransactionModel> list = new List<TollTicketTransactionModel>();

            StreamReader file = new StreamReader(path);
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    TollTicketTransactionModel tollTicket = new TollTicketTransactionModel();
                    tollTicket.ImageID = words[0].Trim();
                    tollTicket.NGAYSOAT = Utility.GetDateTimefromTranID(tollTicket.ImageID);
                    tollTicket.TID = words[2].Trim();
                    tollTicket.GIAVE = Int32.Parse(words[3].Trim());
                    tollTicket.SoXe_ND = words[4].Trim();
                    tollTicket.Login = words[5].Trim();
                    tollTicket.Ca = words[6].Trim();
                    tollTicket.MSLANE = words[7].Trim();
                    tollTicket.MSLoaive = words[8].Trim();
                    tollTicket.MSTRAM = words[9].Trim();
                    tollTicket.KHUHOI = words[10].Trim();
                   
                    //Còn data chưa set xong

                    list.Add(tollTicket);

                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            file.Close();
            return list;
        }
        public void AddTollTicketTransaction(TollTicketTransactionModel tollTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_TOLL_TICKET_TRANSACTION;


                cmd.Parameters.Add("@ImageID", SqlDbType.NVarChar, 20).Value = tollTicket.ImageID;
                cmd.Parameters.Add("@NGAYSOAT", SqlDbType.DateTime).Value = tollTicket.NGAYSOAT;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = tollTicket.TID;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = tollTicket.GIAVE;
                cmd.Parameters.Add("@SoXe_ND", SqlDbType.VarChar, 20).Value = tollTicket.SoXe_ND;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar, 20).Value = tollTicket.Login;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = tollTicket.Ca;
                cmd.Parameters.Add("@MSLANE", SqlDbType.Char, 3).Value = tollTicket.MSLANE;
                cmd.Parameters.Add("@MSLoaive", SqlDbType.Char, 2).Value = tollTicket.MSLoaive;
                cmd.Parameters.Add("@MSTRAM", SqlDbType.Char, 1).Value = tollTicket.MSTRAM;
                cmd.Parameters.Add("@KHUHOI", SqlDbType.Char, 1).Value = tollTicket.KHUHOI;

                cmd.Parameters.Add("@GIOSOAT", SqlDbType.Int).Value = tollTicket.GIOSOAT;
                cmd.Parameters.Add("@MSloaixe", SqlDbType.Char, 2).Value = String.Empty;//commuterTicket.MSloaixe;
                cmd.Parameters.Add("@Checker", SqlDbType.VarChar, 20).Value = String.Empty; //commuterTicket.Checker;
                cmd.Parameters.Add("@F0", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F0;
                cmd.Parameters.Add("@F1", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F1;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F2;
                cmd.Parameters.Add("@SyncEtcMtc", SqlDbType.Int).Value = tollTicket.SyncEtcMtc;
                cmd.Parameters.Add("@SyncFeBe", SqlDbType.Int).Value = tollTicket.SyncFeBe;
                cmd.Parameters.Add("@EtagID", SqlDbType.VarChar, 24).Value = String.Empty; //commuterTicket.EtagID;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("ADD STORE_ADD_COMMUTER_TICKET_TRANSACTION");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        public void ProcessTollTicketTransactionMTC()
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
                    List<TollTicketTransactionModel> TollTicketTransactionModel = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (TollTicketTransactionModel t in TollTicketTransactionModel)
                    {
                        AddTollTicketTransaction(t);
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
