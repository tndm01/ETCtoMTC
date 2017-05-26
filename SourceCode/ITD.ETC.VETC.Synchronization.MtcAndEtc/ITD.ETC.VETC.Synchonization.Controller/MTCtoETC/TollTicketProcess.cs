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

namespace ITD.ETC.VETC.Synchonization.Controller.MTCtoETC
{
    public class TollTicketProcess : DBConnection
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

        public TollTicketProcess(string tableName, string fullSourcePath, string fullDesticationPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = fullSourcePath;
                _remotePath = fullDesticationPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #endregion
        #region ETC
        public List<ObjectTollTicket> ReadFile(string path)
        {
            string line;
            List<ObjectTollTicket> list = new List<ObjectTollTicket>();

            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    ObjectTollTicket oObjectTollTicket = new ObjectTollTicket();
                    oObjectTollTicket.GIAVE = Int32.Parse(words[0].Trim());
                    oObjectTollTicket.SOLUONG = Int32.Parse(words[1].Trim());
                    oObjectTollTicket.VEDAU = words[2].Trim();
                    //oObjectTollTicket.VECUOI = words[3].Trim();
                    oObjectTollTicket.LOGIN = words[4].Trim();
                    oObjectTollTicket.NGAYTAO = DateTime.Parse(words[5].Trim());
                    oObjectTollTicket.CAXUAT = words[6].Trim();
                    oObjectTollTicket.MANVXUAT = words[7].Trim();

                    list.Add(oObjectTollTicket);
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }

            file.Close();
            return list;
        }


        /// <summary>
        /// Hàm lưu 1 đối tượng xuống database
        /// </summary>
        /// <param name="ObjectTollTicket">đối tượng cần lưu</param>
        public void AddTollTicket(ObjectTollTicket ObjectTollTicket)
        {
            try
            {
                int count = ObjectTollTicket.SOLUONG;

                int countTemp = 100;
                string serial = ObjectTollTicket.VEDAU.Substring(8, 7);
                string baseTicket = ObjectTollTicket.VEDAU.Substring(0, 8);
                int serialStart = int.Parse(serial);
                string ticket;

                while (count > 100)
                {
                    count = count - countTemp;
                    ticket = baseTicket + serialStart.ToString("0000000");
                    //string cmd = StrogeInsertVe + " " + ticket + "," + countTemp + "," + Login + ",'1'";
                    //da.SQLcommandExecute(cmd);
                    AddTollTicket(countTemp, ticket, ObjectTollTicket.LOGIN);
                    serialStart += countTemp;

                }

                ticket = baseTicket + serialStart.ToString("0000000");
                //string cmd1 = StrogeInsertVe + " " + ticket + "," + count + "," + Login + ",'1'";
                //da.SQLcommandExecute(cmd1);
                AddTollTicket(count, ticket, ObjectTollTicket.LOGIN);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void AddTollTicket(int soluong, string startTicket, string employeeData)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_ACTIVE_TOLL_TICKET;


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SerialRoot", SqlDbType.VarChar).Value = startTicket;
                cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soluong;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar).Value = employeeData;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = "1";
                cmd.Parameters.Add("@Result", SqlDbType.SmallInt).Value = "0";
                
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }


        public void DeleteTollTicket(ObjectTollTicket ObjectTollTicket)
        {
            try
            {
                int count = ObjectTollTicket.SOLUONG;

                int countTemp = 100;
                string serial = ObjectTollTicket.VEDAU.Substring(8, 7);
                string baseTicket = ObjectTollTicket.VEDAU.Substring(0, 8);
                int serialStart = int.Parse(serial);
                string ticket;

                while (count > 100)
                {
                    count = count - countTemp;
                    ticket = baseTicket + serialStart.ToString("0000000");
                    //string cmd = StrogeInsertVe + " " + ticket + "," + countTemp + "," + Login + ",'1'";
                    //da.SQLcommandExecute(cmd);
                    DeleteTollTicket(countTemp, ticket, ObjectTollTicket.LOGIN);
                    serialStart += countTemp;

                }

                ticket = baseTicket + serialStart.ToString("0000000");
                //string cmd1 = StrogeInsertVe + " " + ticket + "," + count + "," + Login + ",'1'";
                //da.SQLcommandExecute(cmd1);
                DeleteTollTicket(count, ticket, ObjectTollTicket.LOGIN);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void DeleteTollTicket(int soluong, string startTicket, string employeeData)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_ACTIVE_TOLL_TICKET;


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SerialRoot", SqlDbType.VarChar).Value = startTicket;
                cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soluong;
                cmd.Parameters.Add("@Login", SqlDbType.VarChar).Value = employeeData;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = "1";
                cmd.Parameters.Add("@Result", SqlDbType.SmallInt).Value = "0";

                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void ProcessTollTicket()
        {
            //try catch
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
                List<ObjectTollTicket> listCommuterTicket = ReadFile(item);
                // Insert dữ liệu vào database
                foreach (ObjectTollTicket t in listCommuterTicket)
                {
                    if (item.Contains("INSERT"))
                    {
                        AddTollTicket(t);
                    }
                    if (item.Contains("UPDATE"))
                    {
                        AddTollTicket(t);
                    }
                    if (item.Contains("DELETE"))
                    {
                        DeleteTollTicket(t);
                    }
                }

            }
        }
        #endregion
        #region MTC
        public void ProcessTollTicketMTC()
        {
            try 
            { 
                //Lay du lieu tu Database
                TollTicketProcessDataMTC();

                //Luu vao file

                //Upload
            }
            catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void TollTicketProcessDataMTC()
        {
            try
            {
                // Get data from data base
                //GHI LOG
                DataTable dt = getTollTicketDataMTC();
                NLogHelper.Info("Get datatable :" + dt.Rows.Count.ToString() + "row");

                List<ObjectTollTicketCTPX> listItem = new List<ObjectTollTicketCTPX>();
                // process data
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectTollTicketCTPX tollTicket = new ObjectTollTicketCTPX();

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
                                var selectSingleNode = emElement.SelectSingleNode("GIAVE");
                                if (selectSingleNode != null)
                                    tollTicket.GIAVE = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SOLUONG");
                                if (selectSingleNode != null)
                                    tollTicket.SOLUONG = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("SERIAL_FROM");
                                if (selectSingleNode != null)
                                    tollTicket.SERIAL_FROM = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("SERIAL_TO");
                                if (selectSingleNode != null)
                                    tollTicket.SERIAL_TO = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MAPX");
                                if (selectSingleNode != null)
                                    tollTicket.MaPX = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NVNHAN");
                                if (selectSingleNode != null)
                                    tollTicket.MaNVNhan = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("NGAYXUAT");
                                if (selectSingleNode != null)
                                    tollTicket.NGAYTAO = DateTime.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("CA");
                                if (selectSingleNode != null)
                                    tollTicket.Ca = Int32.Parse(selectSingleNode.InnerText);
                                selectSingleNode = emElement.SelectSingleNode("NVXUAT");
                                if (selectSingleNode != null)
                                    tollTicket.MaNVXuat = selectSingleNode.InnerText;
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
                    string fileName = saveTollTicket2FileMTC(item);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        // Ftp transfer
                        //_fileTransferFtp.UploadFile(fileName, _remotePath);

                        // update database
                        updateSyncStatusMTC(item);
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
        private void updateSyncStatusMTC(ObjectTollTicketCTPX item)
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
        private string saveTollTicket2FileMTC(ObjectTollTicketCTPX item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                //DateTime.TryParse(item.NGAYBAN, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt", "VELUOT", item.MaPX, item.MaNVNhan, item.Action,
                     date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.GIAVE + ",");
                sb.Append(item.SOLUONG + ",");
                sb.Append(item.SERIAL_FROM + ",");
                sb.Append(item.SERIAL_TO + ",");
                sb.Append(item.MaNVNhan + ",");
                sb.Append(item.NGAYTAO + ",");
                sb.Append(item.Ca + ",");
                sb.Append(item.MaNVXuat);

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
        private DataTable getTollTicketDataMTC()
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
    }
}