using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using System.Data;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller;
using System.Xml;

namespace ITD.ETC.VETC.Synchonization.Controller.MTCtoETC
{
    public class EmployeeProcess
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

        public EmployeeProcess(string tableName, string fullSourcePath, string fullDesticationPath)
        {
            _databaseTableName = tableName;
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion
        #region ETC

        /// <summary>
        /// Hàm đọc file luu mỗi dòng thành 1 đối tượng
        /// </summary>
        /// <param name="path">đường dẫn file cần đọc</param>
        /// <returns>trả về 1 danh sách các đối tượng ObjectEmployee </returns>
        public List<ObjectEmployee> ReadFile(string path)
        {
           // string line;
            List<ObjectEmployee> list = new List<ObjectEmployee>();

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    try
                    {
                        string[] words = line.Split(',');
                        ObjectEmployee obEmployee = new ObjectEmployee();
                        obEmployee.MaNhanVien = words[0].Trim();
                        obEmployee.MaTo = words[1].Trim();
                        obEmployee.HoNhanVien = words[2].Trim();
                        obEmployee.TenNhanVien = words[3].Trim();
                        obEmployee.TenKhongDau = words[4].Trim();
                        obEmployee.DiaChi = words[5].Trim();
                        obEmployee.DienThoai = words[6].Trim();
                        obEmployee.GhiChu = words[7].Trim();
                        obEmployee.MaTram = words[8].Trim();

                        list.Add(obEmployee);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error(ex);
                    }
                }
            }
            
           // file.Close();
            return list;
        }


        /// <summary>
        /// Hàm lưu 1 đối tượng xuống database
        /// </summary>
        /// <param name="CommuterTicket">đối tượng cần lưu</param>
        public void AddEmployee(ObjectEmployee Employee)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_EMPLOYYE;


                cmd.Parameters.Add("@MSNV", SqlDbType.VarChar).Value = Employee.MaNhanVien;
                cmd.Parameters.Add("@MSTO", SqlDbType.Char).Value = Employee.MaTo;
                cmd.Parameters.Add("@HONV", SqlDbType.VarChar).Value = Employee.HoNhanVien;
                cmd.Parameters.Add("@TENNV", SqlDbType.VarChar).Value = Employee.TenNhanVien;
                cmd.Parameters.Add("@TEN_SEARCH", SqlDbType.VarChar).Value = Employee.TenKhongDau;
                cmd.Parameters.Add("@DIACHI", SqlDbType.VarChar).Value = Employee.DiaChi;
                cmd.Parameters.Add("@DIENTHOAI", SqlDbType.VarChar).Value = Employee.DienThoai;
                cmd.Parameters.Add("@GHICHU", SqlDbType.VarChar).Value = Employee.GhiChu;
                cmd.Parameters.Add("@MSTram", SqlDbType.Char).Value = Employee.MaTram;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                // NLogHelper.Info("ADD STORE_ADD_EMPLOYYE");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        //update employee
        public void UpdateEmployee(ObjectEmployee Employee)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_EMPLOYEE;


                cmd.Parameters.Add("@MSNV", SqlDbType.VarChar).Value = Employee.MaNhanVien;
                cmd.Parameters.Add("@MSTO", SqlDbType.Char).Value = Employee.MaTo;
                cmd.Parameters.Add("@HONV", SqlDbType.VarChar).Value = Employee.HoNhanVien;
                cmd.Parameters.Add("@TENNV", SqlDbType.VarChar).Value = Employee.TenNhanVien;
                cmd.Parameters.Add("@TEN_SEARCH", SqlDbType.VarChar).Value = Employee.TenKhongDau;
                cmd.Parameters.Add("@DIACHI", SqlDbType.VarChar).Value = Employee.DiaChi;
                cmd.Parameters.Add("@DIENTHOAI", SqlDbType.VarChar).Value = Employee.DienThoai;
                cmd.Parameters.Add("@GHICHU", SqlDbType.VarChar).Value = Employee.GhiChu;
                cmd.Parameters.Add("@MSTram", SqlDbType.Char).Value = Employee.MaTram;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
             //   NLogHelper.Info("UPDATE STORE_ADD_UPDATE_EMPLOYEE");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }
        //delete employee
        public void Deletemployee(ObjectEmployee Employee)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_EMPLOYEE;


                cmd.Parameters.Add("@MSNV", SqlDbType.VarChar).Value = Employee.MaNhanVien;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
               // NLogHelper.Info("DETELE STORE_ADD_DETELE_EMPLOYEE");

            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }


        public void ProcessEmployee()
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
                    List<ObjectEmployee> listEmployee = ReadFile(item);
                    // Insert dữ liệu vào database
                   


                    foreach (ObjectEmployee t in listEmployee)
                    {
                        if (item.Contains("INSERT"))
                        {
                           AddEmployee(t);
                           
                        }
                        if (item.Contains("UPDATE"))
                        {
                            UpdateEmployee(t);

                        }
                        if (item.Contains("DELETE"))
                        {
                            Deletemployee(t);


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }


        }

        #endregion

        #region MTC
        public void ProcessEmployeeMTC()
        {
            try
            {
                DataTable dt = getEmployeeData();

                List<ObjectEmployee> listItem = new List<ObjectEmployee>();
                // process data
                if (dt != null)
                {
                    NLogHelper.Info(String.Format("JobName: {0}, Get database success, Num of line are {1}", "Eployee MTC-ETC", dt.Rows.Count));
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectEmployee etag = new ObjectEmployee();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        etag.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
                        etag.Action = (string)row["Action"];

                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader rd = new StringReader(xml);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            XmlElement emElement = doc.DocumentElement;

                            if (emElement != null)
                            {
                                var selectSingleNode = emElement.SelectSingleNode("MSNV");
                                if (selectSingleNode != null)
                                    etag.MaNhanVien = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSTO");
                                if (selectSingleNode != null)
                                    etag.MaTo = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("HONV");
                                if (selectSingleNode != null)
                                    etag.HoNhanVien = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("TENNV");
                                if (selectSingleNode != null)
                                    etag.TenNhanVien = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("TEN_SEARCH");
                                if (selectSingleNode != null)
                                    etag.TenKhongDau = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("DIACHI");
                                if (selectSingleNode != null)
                                    etag.DiaChi = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("DIENTHOAI");
                                if (selectSingleNode != null)
                                    etag.DienThoai = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("GHICHU");
                                if (selectSingleNode != null)
                                    etag.GhiChu = selectSingleNode.InnerText;
                                selectSingleNode = emElement.SelectSingleNode("MSTram");
                                if (selectSingleNode != null)
                                    etag.MaTram = selectSingleNode.InnerText;
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
                    string fileName = saveEmployeeFile(item);
                    NLogHelper.Info(fileName);
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
        private void updateSyncStatus(ObjectEmployee item)
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
        private string saveEmployeeFile(ObjectEmployee item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                // DateTime.TryParse(item.NgayBatDau, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}_{4}.txt", "NhanVien", item.Action, item.MaNhanVien, item.TenKhongDau, date.ToString("yyyyMMddHHmmss"));
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;


                // repare data
                StringBuilder sb = new StringBuilder();

                sb.Append(item.MaNhanVien + ",");
                sb.Append(item.MaTo + ",");
                sb.Append(item.HoNhanVien + ",");
                sb.Append(item.TenNhanVien + ",");
                sb.Append(item.TenKhongDau + ",");
                sb.Append(item.DiaChi + ",");
                sb.Append(item.DienThoai + ",");
                sb.Append(item.GhiChu + ",");
                sb.Append(item.MaTram);


                // Write to File
                File.WriteAllText(filepath, sb.ToString());
                NLogHelper.Info(String.Format("Save file Employee"));
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                filepath = string.Empty;
            }
            return filepath;
        }



        private DataTable getEmployeeData()
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
            #endregion
        }
    }
}
