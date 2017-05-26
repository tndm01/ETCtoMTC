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

        public EmployeeProcess(string fullSourcePath, string fullDesticationPath)
        {
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion
        /// <summary>
        /// Hàm đọc file luu mỗi dòng thành 1 đối tượng
        /// </summary>
        /// <param name="path">đường dẫn file cần đọc</param>
        /// <returns>trả về 1 danh sách các đối tượng ObjectEmployee </returns>
        public List<ObjectEmployee> ReadFile(string path)
        {
            string line;
            List<ObjectEmployee> list = new List<ObjectEmployee>();

            StreamReader file = new StreamReader(path);
            try
            {
                while ((line = file.ReadLine()) != null)
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
            }catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }

            file.Close();
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
                NLogHelper.Info("ADD STORE_ADD_EMPLOYYE");

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

                    // Lấy danh sách các đối tượng
                    List<ObjectEmployee> listEmployee = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (ObjectEmployee t in listEmployee)
                    {
                        AddEmployee(t);
                    }
                }
            }catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }

            
        }
    }
}
