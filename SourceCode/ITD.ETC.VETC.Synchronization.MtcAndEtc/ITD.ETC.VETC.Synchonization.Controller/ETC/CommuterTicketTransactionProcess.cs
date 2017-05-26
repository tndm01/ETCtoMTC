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

        #region ETC

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
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() + "row");

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
                                    commuterTicket.GIOSOAT = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NGAYSOAT");
                                if (selectSingleNode != null)
                                    commuterTicket.NGAYSOAT = DateTime.Parse(selectSingleNode.InnerText);
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
                                    commuterTicket.SyncEtcMtc = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    commuterTicket.SyncFeBe = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("EtagID");
                                if (selectSingleNode != null)
                                    commuterTicket.EtagID = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSTRAM");
                                if (selectSingleNode != null)
                                    commuterTicket.MSTRAM = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("KHUHOI");
                                if (selectSingleNode != null)
                                    commuterTicket.KHUHOI = selectSingleNode.InnerText;
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

                string mave = item.TID.Replace("/","");
                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt","VETHANG",mave,item.MSLoaive,item.MSLANE,
                    item.NGAYSOAT.ToString("yyyyMMddHHmmss"));

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
                sb.Append(item.GIAVE + ",");//Not exist
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
        private DataTable getCommuterTicketTransactionData()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_GET_SYNC_DATA;
                cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = _databaseTableName;

                DataTable dt = _mydatabaseHelper.GetDataTable(cmd);
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() +"row");
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
        public List<CommuterTicketTransactionModel> ReadFile(string path)
        {
            string line;
            List<CommuterTicketTransactionModel> list = new List<CommuterTicketTransactionModel>();

            StreamReader file = new StreamReader(path);
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    CommuterTicketTransactionModel commuterTicket = new CommuterTicketTransactionModel();
                    commuterTicket.ImageID = words[0].Trim();
                    commuterTicket.NGAYSOAT = Utility.GetDateTimefromTranID(commuterTicket.ImageID);
                    commuterTicket.TID = words[2].Trim();
                    commuterTicket.GIAVE = Int32.Parse(words[3].Trim());
                    commuterTicket.SoXe_ND = words[4].Trim();
                    commuterTicket.Login = words[5].Trim();
                    commuterTicket.Ca = words[6].Trim();
                    commuterTicket.MSLANE = words[7].Trim();
                    commuterTicket.MSLoaive = words[8].Trim();
                    commuterTicket.MSTRAM = words[9].Trim();
                    commuterTicket.KHUHOI = words[10].Trim();
                    
                    
                    //Còn data chưa set xong
                    list.Add(commuterTicket);

                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            file.Close();
            return list;
        }
        public void AddCommuterTicketTransaction(CommuterTicketTransactionModel commuterTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_COMMUTER_TICKET_TRANSACTION;

                cmd.Parameters.Add("@ImageID", SqlDbType.NVarChar, 20).Value = commuterTicket.ImageID;
                cmd.Parameters.Add("@NGAYSOAT", SqlDbType.DateTime).Value = commuterTicket.NGAYSOAT;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = commuterTicket.TID;
                //cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = commuterTicket.GIAVE;
                cmd.Parameters.Add("@SoXe_ND", SqlDbType.VarChar, 20).Value = commuterTicket.SoXe_ND;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar, 20).Value = commuterTicket.Login;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = commuterTicket.Ca;
                cmd.Parameters.Add("@MSLANE", SqlDbType.Char, 3).Value = commuterTicket.MSLANE;
                cmd.Parameters.Add("@MSLoaive", SqlDbType.Char, 2).Value = commuterTicket.MSLoaive;
                cmd.Parameters.Add("@MSTRAM", SqlDbType.Char, 1).Value = commuterTicket.MSTRAM;
                cmd.Parameters.Add("@KHUHOI", SqlDbType.Char, 1).Value = commuterTicket.KHUHOI;


                cmd.Parameters.Add("@GIOSOAT", SqlDbType.Int).Value = commuterTicket.GIOSOAT;
                cmd.Parameters.Add("@MSloaixe", SqlDbType.Char, 2).Value = String.Empty;//commuterTicket.MSloaixe;
                cmd.Parameters.Add("@Checker", SqlDbType.VarChar, 20).Value = String.Empty; //commuterTicket.Checker;
                cmd.Parameters.Add("@F0", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F0;
                cmd.Parameters.Add("@F1", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F1;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = String.Empty; //commuterTicket.F2;
                cmd.Parameters.Add("@SyncEtcMtc", SqlDbType.Int).Value = commuterTicket.SyncEtcMtc;
                cmd.Parameters.Add("@SyncFeBe", SqlDbType.Int).Value =  commuterTicket.SyncFeBe;
                cmd.Parameters.Add("@EtagID", SqlDbType.VarChar, 24).Value = String.Empty; //commuterTicket.EtagID;
               

                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("ADD STORE_ADD_COMMUTER_TICKET_TRANSACTION");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        public void ProcessCommuterTicketTransactionMTC()
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
                    List<CommuterTicketTransactionModel> listCommuterTicketTransaction = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (CommuterTicketTransactionModel t in listCommuterTicketTransaction)
                    {
                        AddCommuterTicketTransaction(t);
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
