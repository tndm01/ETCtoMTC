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
    public class SpecialProcess
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

        public SpecialProcess(string tableName,string fullSourcePath, string fullDesticationPath)
        {
            _databaseTableName = tableName;
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion

        #region ETC

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

        private string savesSpecial2File(SpecialTransactionModel item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = item.ThoiGianGiaoDich;

                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt", "UUTIEN", item.ImageId,
                    item.LoaiUuTien, item.MsLane, date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.ImageId + ",");
                sb.Append(item.ThoiGianGiaoDich.ToString("HH:mm:ss") + ",");
                sb.Append(item.Tid + ",");
                sb.Append(item.SoXe_ND + ",");
                sb.Append(item.Login + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MsLane + ",");
                sb.Append(item.LoaiUuTien + ",");
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

        public bool SpecialReasonTransaction(SpecialTransactionModel oSpecialTransactionModel)
        {
            try
            {
                if (oSpecialTransactionModel.Reason == 2 && oSpecialTransactionModel.F2 == 0)
                {
                    oSpecialTransactionModel.LoaiUuTien = 20;
                }
                else if (oSpecialTransactionModel.Reason == 2 && oSpecialTransactionModel.F2 == 1)
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
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return true;
        }

        public void ProcessSpecialETC()
        {
            try
            {
                // Get data from data base
                DataTable dt = getSpecialTransactionData();

                List<SpecialTransactionModel> listItem = new List<SpecialTransactionModel>();
                // process data
                if (dt != null)
                {
                    NLogHelper.Info(string.Format("Get {0} record in table {1} ", dt.Rows.Count.ToString(), _databaseTableName));
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
                                    special.ThoiGianGiaoDich = DateTime.Parse(dtran.ToString("HH:mm:ss"));
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
                    string fileName = savesSpecial2File(item);

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

        #endregion Method

        #region MTC
        public List<SpecialTransactionModel> ReadFile(string path)
        {
            // string line;
            List<SpecialTransactionModel> list = new List<SpecialTransactionModel>();

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    try
                    {
                        string[] words = line.Split(',');
                        SpecialTransactionModel obSpecial = new SpecialTransactionModel();
                        obSpecial.ImageId = words[0].Trim();
                        obSpecial.ThoiGianGiaoDich = DateTime.Parse(words[1].Trim());
                        obSpecial.Tid = words[2].Trim();
                        obSpecial.SoXe_ND = words[3].Trim();
                        obSpecial.Login = words[4].Trim();
                        obSpecial.Ca = words[5].Trim();
                        obSpecial.MsLane = words[6].Trim();
                        obSpecial.LoaiUuTien = int.Parse(words[7].Trim());
                        obSpecial.SyncEtcMtc = int.Parse(words[8].Trim());
                        obSpecial.SyncFebe = int.Parse(words[9].Trim());

                        list.Add(obSpecial);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error(ex);
                    }
                }
            }
            return list;
        }

        public bool ChangeReasonSpecial(SpecialTransactionModel obSpecial)
        {
            try
            {
                if(obSpecial.LoaiUuTien == 20)
                {
                    obSpecial.Reason = 2;
                    obSpecial.F2 = 0;
                }
                else if(obSpecial.LoaiUuTien == 21)
                {
                    obSpecial.Reason = 2;
                    obSpecial.F2 = 1;
                }
                else if(obSpecial.LoaiUuTien == 30)
                {
                    obSpecial.Reason = 3;
                    obSpecial.F2 = 0;
                }
                else if(obSpecial.LoaiUuTien == 31)
                {
                    obSpecial.Reason = 3;
                    obSpecial.F2 = 1;
                }
            }
            catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return true;
        }

        public void AddVehiclePlate(SpecialTransactionModel obSpecial)
        {
            try
            {
                var selectSingleNode = obSpecial.ImageId;
                if (selectSingleNode != null)
                {
                    DateTime dtran = Utility.GetDateTimefromTranID(obSpecial.ImageId);
                    obSpecial.NgayMo = dtran.ToString("yyyy/MM/dd");
                    int hOPen = int.Parse(dtran.ToString("HH")) * 3600;
                    int mOpen = int.Parse(dtran.ToString("mm")) * 60;
                    int sOpen = int.Parse(dtran.ToString("ss"));
                    obSpecial.GioMo = (hOPen + mOpen + sOpen).ToString();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_FORCEOPEN;
                cmd.Parameters.Add("@NgayMo", SqlDbType.DateTime).Value = obSpecial.NgayMo;
                cmd.Parameters.Add("@GioMo", SqlDbType.Int).Value = obSpecial.GioMo;
                cmd.Parameters.Add("@Ngaydong", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@Giodong", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar, 20).Value = obSpecial.Login;
                cmd.Parameters.Add("@MsLane", SqlDbType.Char, 3).Value = obSpecial.MsLane;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = obSpecial.Ca;
                cmd.Parameters.Add("@Tid", SqlDbType.VarChar, 30).Value = obSpecial.Tid;
                cmd.Parameters.Add("@Reason", SqlDbType.SmallInt).Value = obSpecial.Reason;
                cmd.Parameters.Add("@Checker", SqlDbType.VarChar, 20).Value = obSpecial.ThoiGianGiaoDich.ToString("HH:mm:ss");
                cmd.Parameters.Add("@SoXe_ND", SqlDbType.VarChar, 20).Value = obSpecial.SoXe_ND;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = obSpecial.F2;
                cmd.Parameters.Add("@ImageID", SqlDbType.NVarChar, 20).Value = obSpecial.ImageId;
                cmd.Parameters.Add("@SyncEtcMtc", SqlDbType.Int).Value = obSpecial.SyncEtcMtc;
                cmd.Parameters.Add("@SyncFeBe", SqlDbType.Int).Value = obSpecial.SyncFebe;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("Success Insert ADD STORE_ADD_FORCEOPEN");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void ProcessSpecialMTC()
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
                    List<SpecialTransactionModel> listSpecial = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (SpecialTransactionModel t in listSpecial)
                    {
                        ChangeReasonSpecial(t);
                        AddVehiclePlate(t);
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