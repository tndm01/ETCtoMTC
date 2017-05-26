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
    public class CommuterTicketProcess
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

        public CommuterTicketProcess(string fullSourcePath, string fullDesticationPath)
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
        /// <returns>trả về 1 danh sách các đối tượng ObjectCommuterTicket </returns>
        public List<ObjectCommuterTicket> ReadFile(string path)
        {
            string line;
            List<ObjectCommuterTicket> list = new List<ObjectCommuterTicket>();

            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    ObjectCommuterTicket obCommuterTicket = new ObjectCommuterTicket();
                    obCommuterTicket.TID = words[0].Trim();
                    obCommuterTicket.GIAVE = Int32.Parse(words[1].Trim());
                    obCommuterTicket.MSLOAIVE = words[2].Trim();
                    obCommuterTicket.MSloaixe = words[3].Trim();
                    obCommuterTicket.SoDangKiem = words[4].Trim();
                    obCommuterTicket.NGAYBD = DateTime.Parse(words[5].Trim());
                    obCommuterTicket.NGAYKT = DateTime.Parse(words[6].Trim());
                    obCommuterTicket.NGAYBAN = DateTime.Parse(words[7].Trim());
                    obCommuterTicket.GIOBAN = Int32.Parse(words[8].Trim());
                    obCommuterTicket.Ca = words[9].Trim();
                    obCommuterTicket.LOGIN = words[10].Trim();
                    obCommuterTicket.KH = words[11].Trim();
                    obCommuterTicket.DCKH = words[12].Trim();
                    list.Add(obCommuterTicket);
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
        /// <param name="CommuterTicket">đối tượng cần lưu</param>
        public void AddCommuterTicket(ObjectCommuterTicket CommuterTicket)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_ADD_COMMUTER_TICKET;

                //dien size
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 20).Value = CommuterTicket.TID;
                cmd.Parameters.Add("@GIOBAN", SqlDbType.Int).Value = CommuterTicket.GIOBAN;
                cmd.Parameters.Add("@NGAYBAN", SqlDbType.DateTime).Value = CommuterTicket.NGAYBAN;
                cmd.Parameters.Add("@NGAYBD", SqlDbType.DateTime).Value = CommuterTicket.NGAYBD;
                cmd.Parameters.Add("@NGAYKT", SqlDbType.DateTime).Value = CommuterTicket.NGAYKT;
                cmd.Parameters.Add("@SOXE", SqlDbType.VarChar, 15).Value = CommuterTicket.SOXE;
                cmd.Parameters.Add("@KH", SqlDbType.VarChar, 100).Value = CommuterTicket.KH;
                cmd.Parameters.Add("@DCKH", SqlDbType.VarChar, 200).Value = CommuterTicket.DCKH;
                cmd.Parameters.Add("@GIAVE", SqlDbType.Int).Value = CommuterTicket.GIAVE;
                cmd.Parameters.Add("@MSLOAIVE", SqlDbType.Char, 2).Value = CommuterTicket.MSLOAIVE;
                cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar, 20).Value = CommuterTicket.LOGIN;
                cmd.Parameters.Add("@Ca", SqlDbType.Char, 3).Value = CommuterTicket.Ca;
                cmd.Parameters.Add("@MSloaixe", SqlDbType.Char, 2).Value = CommuterTicket.MSloaixe;
                cmd.Parameters.Add("@HaTai", SqlDbType.Int).Value = CommuterTicket.HaTai;
                cmd.Parameters.Add("@SoDangKiem", SqlDbType.VarChar, 20).Value = CommuterTicket.SoDangKiem;
                cmd.Parameters.Add("@Expired", SqlDbType.Bit).Value = CommuterTicket.Expired;
                cmd.Parameters.Add("@ChuyenKhoan", SqlDbType.Int).Value = CommuterTicket.ChuyenKhoan;
                cmd.Parameters.Add("@MSTram", SqlDbType.Char, 1).Value = CommuterTicket.MSTram;
                //parameter khac null
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }


        public void ProcessCommuterTicket()
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
                    List<ObjectCommuterTicket> listCommuterTicket = ReadFile(item);
                    //try ctach readfile
                    // Insert dữ liệu vào database
                    foreach (ObjectCommuterTicket t in listCommuterTicket)
                    {
                        AddCommuterTicket(t);
                        //try catch
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
