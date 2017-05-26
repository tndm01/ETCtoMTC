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
    public class VehiclePlate
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


        public VehiclePlate(string fullSourcePath, string fullDesticationPath)
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
        /// <returns>trả về 1 danh sách các đối tượng ObjectVehiclePlate </returns>
        public List<ObjectVehiclePlate> ReadFile(string path)
        {

            string line;
            List<ObjectVehiclePlate> list = new List<ObjectVehiclePlate>();

            StreamReader file = new StreamReader(path);

            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(',');

                    ObjectVehiclePlate obVehiclePlate = new ObjectVehiclePlate();
                    obVehiclePlate.SoXe = words[0].Trim();
                    obVehiclePlate.MaLoaiVe = words[1].Trim();
                    obVehiclePlate.GiaVe = int.Parse(words[2].Trim());
                    obVehiclePlate.NgayDangKy = DateTime.Parse(words[3].Trim());
                    obVehiclePlate.SoDangKiem = words[4].Trim();
                    obVehiclePlate.TaiTrong = words[5].Trim();
                    obVehiclePlate.TrangThai = int.Parse(words[6].Trim());
                    obVehiclePlate.GhiChu = words[7].Trim();
                    obVehiclePlate.NhanVienNhap = words[8].Trim();
                    obVehiclePlate.NgayNhap = DateTime.Parse(words[9].Trim());
                    obVehiclePlate.TrangThaiMacDinh = words[10].Trim();
                    obVehiclePlate.HienGhiChu = words[11].Trim();
                    obVehiclePlate.XeUuTien = words[12].Trim();
                    obVehiclePlate.GhiChuOLan = words[13].Trim();
                    obVehiclePlate.MaTram = words[14].Trim();



                    list.Add(obVehiclePlate);

                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            file.Close();
            return list;

        }

        public void AddVehiclePlate(ObjectVehiclePlate VehiclePlate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_VEHICLE;

                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = VehiclePlate.SoXe;
                cmd.Parameters.Add("@MSLOAIVE", SqlDbType.VarChar, 2).Value = VehiclePlate.MaLoaiVe;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = VehiclePlate.GiaVe;
                cmd.Parameters.Add("@NGAYDANGKY", SqlDbType.DateTime).Value = VehiclePlate.NgayDangKy;
                cmd.Parameters.Add("@SODANGKIEM", SqlDbType.VarChar,30).Value = VehiclePlate.SoDangKiem;
               cmd.Parameters.Add("@TRONGTAI", SqlDbType.VarChar,50).Value = VehiclePlate.TaiTrong;
               cmd.Parameters.Add("@ENABLED", SqlDbType.TinyInt).Value = VehiclePlate.TrangThai;
               cmd.Parameters.Add("@GHICHU", SqlDbType.VarChar,250).Value = VehiclePlate.GhiChu;
               cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar,20).Value = VehiclePlate.NhanVienNhap;
               cmd.Parameters.Add("@NGAYNHAP", SqlDbType.DateTime).Value = VehiclePlate.NgayNhap;
               cmd.Parameters.Add("@F0", SqlDbType.Char,1).Value = VehiclePlate.TrangThaiMacDinh;
               cmd.Parameters.Add("@F1", SqlDbType.Char,1).Value = VehiclePlate.HienGhiChu;
               cmd.Parameters.Add("@F2", SqlDbType.Char,1).Value = VehiclePlate.XeUuTien;
               cmd.Parameters.Add("@GHICHU_F1", SqlDbType.VarChar,50).Value = VehiclePlate.GhiChuOLan;
               cmd.Parameters.Add("@MSTRAM", SqlDbType.Char,1).Value = VehiclePlate.MaTram;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                NLogHelper.Info("ADD STORE_ADD_VEHICLE");
                 
            }
            catch (Exception ex)
            {

                NLogHelper.Error(ex);

            }
        }

        public void ProcessVehiclePlate()
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
                    List<ObjectVehiclePlate> listVehicle = ReadFile(item);
                    // Insert dữ liệu vào database
                    foreach (ObjectVehiclePlate t in listVehicle)
                    {
                        AddVehiclePlate(t);

                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
    }
}
