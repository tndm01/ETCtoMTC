using System;
namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ObjectVehiclePlate : ModelBase
    {
        private string _soXe;
        public string SoXe
        {
            get { return _soXe; }
            set { _soXe = value; }
        }

        private string _maLoaiVe;
        public string MaLoaiVe
        {
            get
            {
                return _maLoaiVe;
            }

            set
            {
                _maLoaiVe = value;
            }
        }

        private int _giaVe;
        public int GiaVe
        {
            get { return _giaVe; }
            set { _giaVe = value; }
        }

        private DateTime _ngayDangKy;
        public DateTime NgayDangKy 
        {
            get { return _ngayDangKy; }
            set { _ngayDangKy = value; }
        }

        private string _soDangKiem;
        public string SoDangKiem
        {
            get { return _soDangKiem; }
            set { _soDangKiem = value; }
        }

        private string _taiTrong;
        public string TaiTrong
        {
            get { return _taiTrong; }
            set { _taiTrong = value; }
        }

        private int _trangThai;
        public int TrangThai
        {
            get { return _trangThai; }
            set { _trangThai = value; }
        }

        private string _ghiChu;
        public string GhiChu
        {
            get { return _ghiChu; }
            set { _ghiChu = value; }
        }

        private string _nhanVienNhap;
        public string NhanVienNhap
        {
            get { return _nhanVienNhap; }
            set { _nhanVienNhap = value; }
        }

        private DateTime _ngayNhap;
        public DateTime NgayNhap
        {
            get { return _ngayNhap; }
            set { _ngayNhap = value; }
        }

        private string _trangThaiMacDinh;
        public string TrangThaiMacDinh
        {
            get { return _trangThaiMacDinh; }
            set { _trangThaiMacDinh = value; }
        }

        private string _hienGhiChu;
        public string HienGhiChu
        {
            get { return _hienGhiChu; }
            set { _hienGhiChu = value; }
        }

        private string _xeUuTien;
        public string XeUuTien
        {
            get { return _xeUuTien; }
            set { _xeUuTien = value; }
        }

        private string _ghiChuOLan;
        public string GhiChuOLan
        {
            get { return _ghiChuOLan; }
            set { _ghiChuOLan = value; }
        }

        private string _maTram;
        public string MaTram
        {
            get { return _maTram; }
            set { _maTram = value; }
        }

        
    }
}