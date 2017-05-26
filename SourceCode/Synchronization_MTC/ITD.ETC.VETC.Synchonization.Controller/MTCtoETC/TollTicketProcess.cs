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

        public TollTicketProcess(string fullSourcePath, string fullDesticationPath)
        {
            this._localPath = fullSourcePath;
            this._remotePath = fullDesticationPath;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion

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
                    oObjectTollTicket.TID = words[3].Trim();
                    oObjectTollTicket.LOGIN = words[4].Trim();
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
                //cmd.Parameters.Add("@Result", SqlDbType.SmallInt) = 0;


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
                    AddTollTicket(t);
                }
            }
        }
    }
}