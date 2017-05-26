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

        public VehiclePlate(string tableName, string fullSourcePath, string fullDesticationPath)
        {
            _databaseTableName = tableName;
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion Field

        #region ETC

        /// <summary>
        /// Hàm đọc file luu mỗi dòng thành 1 đối tượng
        /// </summary>
        /// <param name="path">đường dẫn file cần đọc</param>
        /// <returns>trả về 1 danh sách các đối tượng ObjectVehiclePlate </returns>
        public List<ObjectVehiclePlate> ReadFile(string path)
        {
           // string line;
            List<ObjectVehiclePlate> list = new List<ObjectVehiclePlate>();

            StreamReader file = new StreamReader(path);

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    try
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
                    catch (Exception ex)
                    {
                        NLogHelper.Error(ex);
                    }
                }
            }


            file.Close();
            return list;
        }
        //add vehicle
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
                cmd.Parameters.Add("@SODANGKIEM", SqlDbType.VarChar, 30).Value = VehiclePlate.SoDangKiem;
                cmd.Parameters.Add("@TRONGTAI", SqlDbType.VarChar, 50).Value = VehiclePlate.TaiTrong;
                cmd.Parameters.Add("@ENABLED", SqlDbType.TinyInt).Value = VehiclePlate.TrangThai;
                cmd.Parameters.Add("@GHICHU", SqlDbType.VarChar, 250).Value = VehiclePlate.GhiChu;
                cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar, 20).Value = VehiclePlate.NhanVienNhap;
                cmd.Parameters.Add("@NGAYNHAP", SqlDbType.DateTime).Value = VehiclePlate.NgayNhap;
                cmd.Parameters.Add("@F0", SqlDbType.Char, 1).Value = VehiclePlate.TrangThaiMacDinh;
                cmd.Parameters.Add("@F1", SqlDbType.Char, 1).Value = VehiclePlate.HienGhiChu;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = VehiclePlate.XeUuTien;
                cmd.Parameters.Add("@GHICHU_F1", SqlDbType.VarChar, 50).Value = VehiclePlate.GhiChuOLan;
                cmd.Parameters.Add("@MSTRAM", SqlDbType.Char, 1).Value = VehiclePlate.MaTram;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                // NLogHelper.Info("ADD STORE_ADD_VEHICLE");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        //Update vehicle
        public void UpdateVehiclePlate(ObjectVehiclePlate VehiclePlate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_VEHICLE;

                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = VehiclePlate.SoXe;
                cmd.Parameters.Add("@MSLOAIVE", SqlDbType.VarChar, 2).Value = VehiclePlate.MaLoaiVe;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = VehiclePlate.GiaVe;
                cmd.Parameters.Add("@NGAYDANGKY", SqlDbType.DateTime).Value = VehiclePlate.NgayDangKy;
                cmd.Parameters.Add("@SODANGKIEM", SqlDbType.VarChar, 30).Value = VehiclePlate.SoDangKiem;
                cmd.Parameters.Add("@TRONGTAI", SqlDbType.VarChar, 50).Value = VehiclePlate.TaiTrong;
                cmd.Parameters.Add("@ENABLED", SqlDbType.TinyInt).Value = VehiclePlate.TrangThai;
                cmd.Parameters.Add("@GHICHU", SqlDbType.VarChar, 250).Value = VehiclePlate.GhiChu;
                cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar, 20).Value = VehiclePlate.NhanVienNhap;
                cmd.Parameters.Add("@NGAYNHAP", SqlDbType.DateTime).Value = VehiclePlate.NgayNhap;
                cmd.Parameters.Add("@F0", SqlDbType.Char, 1).Value = VehiclePlate.TrangThaiMacDinh;
                cmd.Parameters.Add("@F1", SqlDbType.Char, 1).Value = VehiclePlate.HienGhiChu;
                cmd.Parameters.Add("@F2", SqlDbType.Char, 1).Value = VehiclePlate.XeUuTien;
                cmd.Parameters.Add("@GHICHU_F1", SqlDbType.VarChar, 50).Value = VehiclePlate.GhiChuOLan;
                cmd.Parameters.Add("@MSTRAM", SqlDbType.Char, 1).Value = VehiclePlate.MaTram;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
                // NLogHelper.Info("UPDATE STORE_UPDATE_VEHICLE");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        //delete vehicle
        public void DeleteVehiclePlate(ObjectVehiclePlate VehiclePlate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_DELETE_VEHICLE;

                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = VehiclePlate.SoXe;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
                //  NLogHelper.Info("DETELE STORE_DELETE_VEHICLE");
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void ProcessVehiclePlateETC()
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

                        if (item.Contains("INSERT"))
                        {
                            AddVehiclePlate(t);
                            
                        }
                        if (item.Contains("UPDATE"))
                        {
                            UpdateVehiclePlate(t);

                        }
                        if (item.Contains("DELETE"))
                        {
                            DeleteVehiclePlate(t);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #endregion ETC

        #region MTC

        private void updateSyncStatus(ObjectVehiclePlate item)
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

        private string saveVehiclePlateFile(ObjectVehiclePlate item)
        {
            string filepath = string.Empty;
            try
            {
                // Create file name
                filepath = _localPath;
                DateTime date = DateTime.Now;
                //DateTime.TryParse(item.NgayDangKy, out date);

                string fileName = String.Format("{0}_{1}_{2}_{3}.txt", "Datasoxe", item.SoXe, item.Action,
                    date.ToString("yyyyMMddHHmmss"));

                //Create Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += "/" + fileName;

                // repare data
                StringBuilder sb = new StringBuilder();
                sb.Append(item.SoXe + ",");
                sb.Append(item.MaLoaiVe + ",");
                sb.Append(item.GiaVe + ",");
                sb.Append(item.NgayDangKy + ",");
                sb.Append(item.SoDangKiem + ",");
                sb.Append(item.TaiTrong + ",");
                sb.Append(item.TrangThai + ",");
                sb.Append(item.GhiChu + ",");
                sb.Append(item.NhanVienNhap + ",");
                sb.Append(item.NgayNhap + ",");
                sb.Append(item.TrangThaiMacDinh + ",");
                sb.Append(item.HienGhiChu + ",");
                sb.Append(item.XeUuTien + ",");
                sb.Append(item.GhiChuOLan + ",");
                sb.Append(item.MaTram);

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

        private DataTable getVehiclePlateTransactionData()
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

        public void ProcessVehiclePlateMTC()
        {
            try
            {
                // Get data from data base
                DataTable dt = getVehiclePlateTransactionData();

                List<ObjectVehiclePlate> listItem = new List<ObjectVehiclePlate>();
                // process data
                if (dt != null)
                {
                    NLogHelper.Info(string.Format("Get {0} record in table {1}", dt.Rows.Count.ToString(), _databaseTableName));
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectVehiclePlate vehiclePlate = new ObjectVehiclePlate();

                        string xml = row["DataTracking"].ToString().Trim();
                        // gan tracking id
                        vehiclePlate.TrackingId = (long)row["TrackingID"];//Int64.Parse(row["TrackingID"].ToString());
                        vehiclePlate.Action = (string)row["Action"];
                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader rd = new StringReader(xml);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            XmlElement emElement = doc.DocumentElement;

                            if (emElement != null)
                            {
                                var selectSingleNode = emElement.SelectSingleNode("SoXe");
                                if (selectSingleNode != null)
                                    vehiclePlate.SoXe = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("MSLoaiVe");
                                if (selectSingleNode != null)
                                    vehiclePlate.MaLoaiVe = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("GiaVe");
                                if (selectSingleNode != null)
                                    vehiclePlate.GiaVe = int.Parse(selectSingleNode.InnerText.Trim());
                                selectSingleNode = emElement.SelectSingleNode("NgayDangKy");
                                if (selectSingleNode != null)
                                    vehiclePlate.NgayDangKy = DateTime.Parse(selectSingleNode.InnerText.Trim());
                                selectSingleNode = emElement.SelectSingleNode("SoDangkiem");
                                if (selectSingleNode != null)
                                    vehiclePlate.SoDangKiem = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Taitrong");
                                if (selectSingleNode != null)
                                    vehiclePlate.TaiTrong = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("ENABLED");
                                if (selectSingleNode != null)
                                    vehiclePlate.TrangThai = int.Parse(selectSingleNode.InnerText.Trim());
                                selectSingleNode = emElement.SelectSingleNode("Ghichu");
                                if (selectSingleNode != null)
                                    vehiclePlate.GhiChu = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("Login");
                                if (selectSingleNode != null)
                                    vehiclePlate.NhanVienNhap = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("F0");
                                if (selectSingleNode != null)
                                    vehiclePlate.TrangThaiMacDinh = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("F1");
                                if (selectSingleNode != null)
                                    vehiclePlate.HienGhiChu = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("F2");
                                if (selectSingleNode != null)
                                    vehiclePlate.XeUuTien = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("GhiChu_F1");
                                if (selectSingleNode != null)
                                    vehiclePlate.GhiChuOLan = selectSingleNode.InnerText.Trim();
                                selectSingleNode = emElement.SelectSingleNode("MSTram");
                                if (selectSingleNode != null)
                                    vehiclePlate.MaTram = selectSingleNode.InnerText.Trim();
                                listItem.Add(vehiclePlate);
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
                    string fileName = saveVehiclePlateFile(item);
                    FileInfo finfo = new FileInfo(fileName);
                    NLogHelper.Info("Success " + finfo.Name);

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

        #endregion MTC
    }
}