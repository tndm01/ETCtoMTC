using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ObjecVehiclePlate
    {
        public string SoXe { get; set; }
        public string MaLoaiVe { get; set; }
        public string GiaVe { get; set; }
        public string NgayDangKy { get; set; }
        public string SoDangKiem { get; set; }
        public string TaiTrong { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string NhanVienNhap { get; set; }
        public string NgayNhap { get; set; }
        public string TrangThaiMacDinh { get; set; }
        public string HienGhiChu { get; set; }
        public string XeUuTien { get; set; }
        public string GhiChuOLan { get; set; }
        public string MaTram { get; set; }
        public ObjecVehiclePlate(string soxe, string maloaive, string giave, string ngaydangky, string sodangkiem,
                                 string taitrong, string trangthai, string ghichu, string nhanviennhap,string ngaynhap,
                                  string trangthaimacdinh,string hienghichu,string xeuutien,string ghichuolan,string matram)
        {
            SoXe =soxe;
            MaLoaiVe = maloaive;
            GiaVe = giave;
            NgayDangKy = ngaydangky;
            SoDangKiem =sodangkiem;
            TaiTrong = taitrong;
            TrangThai = trangthai;
            GhiChu = ghichu;
            NhanVienNhap = nhanviennhap;
            NgayNhap = ngaynhap;
            TrangThaiMacDinh = trangthaimacdinh;
            HienGhiChu =hienghichu;
            XeUuTien =xeuutien;
            GhiChuOLan = ghichuolan;
            MaTram = matram;
        }
    }
}
