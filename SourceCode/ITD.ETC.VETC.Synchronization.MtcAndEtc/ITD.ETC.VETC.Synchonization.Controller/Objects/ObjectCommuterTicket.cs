using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ObjectCommuterTicket : ModelBase
    {
        private string _tID = String.Empty;
        public string TID
        {
            get { return _tID; }
            set { _tID = value; }
        }

        private int _gioBan;
        public int GIOBAN
        {
            get { return _gioBan; }
            set { _gioBan = value; }
        }

        private DateTime _ngayBan;
        public DateTime NGAYBAN
        {
            get { return _ngayBan; }
            set { _ngayBan = value; }

        }

        private DateTime _ngayBD;
        public DateTime NGAYBD
        {
            get { return _ngayBD; }
            set { _ngayBD = value; }

        }

        private DateTime _ngayKT;
        public DateTime NGAYKT
        {
            get { return _ngayKT; }
            set { _ngayKT = value; }

        }

        private string _soXe = String.Empty;
        public string SOXE
        {
            get { return _soXe; }
            set { _soXe = value; }

        }

        private string _kH = String.Empty;
        public string KH
        {
            get { return _kH; }
            set { _kH = value; }

        }

        private string _dCKH = String.Empty;
        public string DCKH
        {
            get { return _dCKH; }
            set { _dCKH = value; }

        }

        private int _giaVe;
        public int GIAVE
        {
            get { return _giaVe; }
            set { _giaVe = value; }

        }

        private string _mSLoaiVe = String.Empty;
        public string MSLOAIVE
        {
            get { return _mSLoaiVe; }
            set { _mSLoaiVe = value; }

        }

        private string _login = String.Empty;
        public string LOGIN
        {
            get { return _login; }
            set { _login = value; }

        }

        private string _ca = String.Empty;
        public string Ca
        {
            get { return _ca; }
            set { _ca = value; }

        }

        private string _mSLoaiXe = String.Empty;
        public string MSloaixe
        {
            get { return _mSLoaiXe; }
            set { _mSLoaiXe = value; }

        }

        private int _haTai;
        public int HaTai
        {
            get { return _haTai; }
            set { _haTai = value; }

        }

        private string _soDangKiem = String.Empty;
        public string SoDangKiem
        {
            get { return _soDangKiem; }
            set { _soDangKiem = value; }

        }

        private bool _expired;
        public bool Expired
        {
            get { return _expired; }
            set { _expired = value; }

        }

        private int _chuyenKhoan;
        public int ChuyenKhoan
        {
            get { return _chuyenKhoan; }
            set { _chuyenKhoan = value; }

        }

        private string _mSTram =String.Empty;
        public string MSTram
        {
            get { return _mSTram; }
            set { _mSTram = value; }
        }

        //public ObjectCommuterTicket()
        //{
        //    _tID = "B";
        //    _gioBan = 12;
        //    _ngayBan = DateTime.Parse("2008-10-29");
        //    _ngayBD = DateTime.Parse("2008-11-29");
        //    _ngayKT = DateTime.Parse("2008-12-29");
        //    _soXe = "1";
        //    _kH = "kH";
        //    _dCKH = "dCKH";
        //    _giaVe = 1000;
        //    _mSLoaiVe = "12";
        //    _login = "login";
        //    _ca = "ca1";
        //    _mSLoaiXe = "m1";
        //    _haTai = 2;
        //    _soDangKiem = "so";
        //    _expired = true;
        //    _chuyenKhoan = 1;
        //    _mSTram = "4";
        //}
        public ObjectCommuterTicket()
        { }
        public ObjectCommuterTicket(
            string tID,
            int gioBan,
            DateTime ngayBan,
            DateTime ngayBD,
            DateTime ngayKT,
            string soXe,
            string kH,
            string dCKH,
            int giaVe,
            string mSLoaiVe,
            string login,
            string ca,
            string mSLoaiXe,
            int haTai,
            string soDangKiem,
            bool expired,
            int chuyenKhoan,
            string mSTram)
        {
            _tID = tID;
            _gioBan = gioBan;
            _ngayBan = ngayBan;
            _ngayBD = ngayBD;
            _ngayKT = ngayKT;
            _soXe = soXe;
            _kH = kH;
            _dCKH = dCKH;
            _giaVe = giaVe;
            _mSLoaiVe = mSLoaiVe;
            _login = login;
            _ca = ca;
            _mSLoaiXe = mSLoaiXe;
            _haTai = haTai;
            _soDangKiem = soDangKiem;
            _expired = expired;
            _chuyenKhoan = chuyenKhoan;
            _mSTram = mSTram;
        }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ITD.ETC.VETC.Synchonization.Controller.Objects
//{
//    public class ObjectCommuterTicket
//    {
//        private string _tID;
//        public string TID
//        {
//            get { return _tID; }
//            set { _tID = value; }
//        }

//        private string _gioBan;
//        public string GIOBAN
//        {
//            get { return _gioBan; }
//            set { _gioBan = value; }
//        }

//        private string _ngayBan;
//        public string NGAYBAN
//        {
//            get { return _ngayBan; }
//            set { _ngayBan = value; }

//        }

//        private string _ngayBD;
//        public string NGAYBD
//        {
//            get { return _ngayBD; }
//            set { _ngayBD = value; }

//        }

//        private string _ngayKT;
//        public string NGAYKT
//        {
//            get { return _ngayKT; }
//            set { _ngayKT = value; }

//        }

//        private string _soXe;
//        public string SOXE
//        {
//            get { return _soXe; }
//            set { _soXe = value; }

//        }

//        private string _kH;
//        public string KH
//        {
//            get { return _kH; }
//            set { _kH = value; }

//        }

//        private string _dCKH;
//        public string DCKH
//        {
//            get { return _dCKH; }
//            set { _dCKH = value; }

//        }

//        private string _giaVe;
//        public string GIAVE
//        {
//            get { return _giaVe; }
//            set { _giaVe = value; }

//        }

//        private string _mSLoaiVe;
//        public string MSLOAIVE
//        {
//            get { return _mSLoaiVe; }
//            set { _mSLoaiVe = value; }

//        }

//        private string _login;
//        public string LOGIN
//        {
//            get { return _login; }
//            set { _login = value; }

//        }

//        private string _ca;
//        public string Ca
//        {
//            get { return _ca; }
//            set { _ca = value; }

//        }

//        private string _mSLoaiXe;
//        public string MSloaixe
//        {
//            get { return _mSLoaiXe; }
//            set { _mSLoaiXe = value; }

//        }

//        private string _haTai;
//        public string HaTai
//        {
//            get { return _haTai; }
//            set { _haTai = value; }

//        }

//        private string _soDangKiem;
//        public string SoDangKiem
//        {
//            get { return _soDangKiem; }
//            set { _soDangKiem = value; }

//        }

//        private string _expired;
//        public string Expired
//        {
//            get { return _expired; }
//            set { _expired = value; }

//        }

//        private string _chuyenKhoan;
//        public string ChuyenKhoan
//        {
//            get { return _chuyenKhoan; }
//            set { _chuyenKhoan = value; }

//        }

//        private string _mSTram;
//        public string MSTram
//        {
//            get { return _mSTram; }
//            set { _mSTram = value; }
//        }

//        public ObjectCommuterTicket()
//        { }
//        public ObjectCommuterTicket(
//            string tID,
//            string gioBan,
//            string ngayBan,
//            string ngayBD,
//            string ngayKT,
//            string soXe,
//            string kH,
//            string dCKH,
//            string giaVe,
//            string mSLoaiVe,
//            string login,
//            string ca,
//            string mSLoaiXe,
//            string haTai,
//            string soDangKiem,
//            string expired,
//            string chuyenKhoan,
//            string mSTram)
//        {
//            _tID = tID;
//            _gioBan = gioBan;
//            _ngayBan = ngayBan;
//            _ngayBD = ngayBD;
//            _ngayKT = ngayKT;
//            _soXe = soXe;
//            _kH = kH;
//            _dCKH = dCKH;
//            _giaVe = giaVe;
//            _mSLoaiVe = mSLoaiVe;
//            _login = login;
//            _ca = ca;
//            _mSLoaiXe = mSLoaiXe;
//            _haTai = haTai;
//            _soDangKiem = soDangKiem;
//            _expired = expired;
//            _chuyenKhoan = chuyenKhoan;
//            _mSTram = mSTram;
//        }
//    }
//}
