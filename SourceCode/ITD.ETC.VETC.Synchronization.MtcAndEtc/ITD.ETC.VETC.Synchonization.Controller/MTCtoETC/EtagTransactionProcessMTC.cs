using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;

namespace ITD.ETC.VETC.Synchonization.Controller.MTCtoETC
{
    public class EtagTransactionProcessMTC
    {
        #region Field
        private string _databaseTableName;

        // object data base
        private DataBaseHelper _mydatabaseHelper;

        // object fileTransferFtp
        private FileTransferFtp _fileTransferFtp;

        // object eTag
        //private ETagTransactionModel _oETag;
        // local path lưu dữ liệu Etag
        private string _localPath;

        // remotepath lưu dữ liệu Etag trên server
        private string _remotePath;

        #endregion

        public EtagTransactionProcessMTC(string tableName, string etagLocalFolderPath, string etagFtpFolderPath)
        {
            try
            {
                _databaseTableName = tableName;
                _localPath = etagLocalFolderPath;
                _remotePath = etagFtpFolderPath;
                _mydatabaseHelper = DataBaseHelper.GetInstance();
                _fileTransferFtp = FileTransferFtp.GetInstance();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void EtagProcessDataDown()
        {
            //try catch
            try
            {
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
                    List<ETagTransactionModel> listOETags = ReadFile(item);
                    //try ctach readfile
                    //Insert dữ liệu vào database
                    foreach (ETagTransactionModel oETag in listOETags)
                    {
                        AddEtagTransaction(oETag);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public List<ETagTransactionModel> ReadFile(string path)
        {
            string line;
            List<ETagTransactionModel> list = new List<ETagTransactionModel>();

            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    ETagTransactionModel oETagTransactionModel = new ETagTransactionModel();
                    oETagTransactionModel.MaGiaoDich = words[0].Trim();
                    oETagTransactionModel.ThoiGianGiaoDich = DateTime.Parse(words[1].Trim());
                    oETagTransactionModel.MaETag = words[2].Trim();
                    oETagTransactionModel.GiaVe = Int32.Parse(words[3].Trim());
                    oETagTransactionModel.SoXeNhanDang = words[4].Trim();
                    oETagTransactionModel.MaNhanVien = words[5].Trim();
                    oETagTransactionModel.MaCa = words[6].Trim();
                    oETagTransactionModel.MaLan = words[7].Trim();
                    oETagTransactionModel.MaVe = words[8].Trim();

                    list.Add(oETagTransactionModel);
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }

            file.Close();
            return list;
        }
        public void AddEtagTransaction(ETagTransactionModel oETagTransactionModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_TRP_tblCheckObuAccount_RFID;

                cmd.Parameters.Add("@ImageID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaGiaoDich;
                cmd.Parameters.Add("@CheckDate", SqlDbType.DateTime).Value = oETagTransactionModel.ThoiGianGiaoDich;
                cmd.Parameters.Add("@ObuID", SqlDbType.VarChar, 50).Value = oETagTransactionModel.MaETag;
                cmd.Parameters.Add("@ChargeAmount", SqlDbType.Int).Value = oETagTransactionModel.GiaVe;
                cmd.Parameters.Add("@RecogPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeNhanDang;
                cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaNhanVien;
                cmd.Parameters.Add("@ShiftID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaCa;
                cmd.Parameters.Add("@LaneID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaLan;
                cmd.Parameters.Add("@TicketID", SqlDbType.VarChar, 20).Value = oETagTransactionModel.MaVe;
                cmd.Parameters.Add("@RegisPlateNumber", SqlDbType.VarChar, 12).Value = oETagTransactionModel.SoXeDangKy;

                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
    }
}
