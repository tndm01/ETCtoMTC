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
    public class SpecialTransactionProcess
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

        public SpecialTransactionProcess(string tableName, string specialLocalFolderPath, string specialFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = specialLocalFolderPath;
                _remotePath = specialFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void updateSyncStatus(SpecialTransactionModel item)
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

        private string saveEtag2File(SpecialTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                DateTime.TryParse(item.GioMo, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt", "UUTIEN", item.ImageId,
                    item.LoaiUuTien,item.MsLane, date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.ImageId + ",");
                sb.Append(item.ThoiGianGiaoDich + ",");
                sb.Append(item.Tid + ",");
                sb.Append(item.SoXe_ND + ",");
                sb.Append(item.Login + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MsLane + ",");
                sb.Append(item.LoaiUuTien + ",");
                sb.Append(0 + ",");
                sb.Append(0);
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

        public bool SpecialReasonTransaction(SpecialTransactionModel oSpecialTransactionModel)
        {
            if (oSpecialTransactionModel.Reason == 2 && oSpecialTransactionModel.F2 == 0)
            {
                 oSpecialTransactionModel.LoaiUuTien = 20;
            }
            else if(oSpecialTransactionModel.Reason == 2 && oSpecialTransactionModel.F2 == 1)
            {
                oSpecialTransactionModel.LoaiUuTien = 21;
            }
            else if (oSpecialTransactionModel.Reason == 3 && oSpecialTransactionModel.F2 == 0)
            {
                oSpecialTransactionModel.LoaiUuTien = 30;
            }
            else if (oSpecialTransactionModel.Reason == 3 && oSpecialTransactionModel.F2 == 1)
            {
                oSpecialTransactionModel.LoaiUuTien = 31;
            }
            return true;
        }

        public void SpecialProcessData()
        {
            try
            {
                // Get data from data base
                DataTable dt = getSpecialTransactionData();

                List<SpecialTransactionModel> listItem = new List<SpecialTransactionModel>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SpecialTransactionModel special = new SpecialTransactionModel();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        special.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
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
                                    special.ImageId = selectSingleNode.InnerText;
                                    DateTime dtran = Utility.GetDateTimefromTranID(special.ImageId);
                                    special.ThoiGianGiaoDich = dtran.ToString("HH:mm:ss");
                                }

                                selectSingleNode = emElement.SelectSingleNode("SoXe_ND");
                                if (selectSingleNode != null)
                                    special.SoXe_ND = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("LOGIN");
                                if (selectSingleNode != null)
                                    special.Login = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSLANE");
                                if (selectSingleNode != null)
                                    special.MsLane = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Ca");
                                if (selectSingleNode != null)
                                    special.Ca = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("TID");
                                if (selectSingleNode != null)
                                    special.Tid = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("Reason");
                                if (selectSingleNode != null)
                                    special.Reason = int.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SyncEtcMtc");
                                if (selectSingleNode != null)
                                    special.SyncEtcMtc = int.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SyncFeBe");
                                if (selectSingleNode != null)
                                    special.SyncFebe = int.Parse(selectSingleNode.InnerText);

                                SpecialReasonTransaction(special);

                                listItem.Add(special);
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
