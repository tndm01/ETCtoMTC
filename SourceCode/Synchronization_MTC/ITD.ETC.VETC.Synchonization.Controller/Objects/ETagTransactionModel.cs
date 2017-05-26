using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ETagTransactionModel : ModelBase
    {
        public string MaGiaoDich { get; set; }

        public string ThoiGianGiaoDich { get; set; }

        public string MaETag { get; set; }

        public string GiaVe { get; set; }

        public string SoXeNhanDang { get; set; }

        public string MaNhanVien { get; set; }

        public string MaCa { get; set; }

        public string MaLan { get; set; }

        public string MaVe { get; set; }

        public string SoXeDangKy { get; set; }
        //public ETagTransactionModel(){
        //    MaGiaoDich = "magiaodich";
        //    ThoiGianGiaoDich = "today";
        //    MaETag = "etag";
        //    GiaVe = "10000";
        //    SoXeNhanDang = "62R1-8301";
        //    MaNhanVien = "manhanvien";
        //    MaCa = "maca";
        //    MaLan = "malan";
        //    MaVe = "mave";
        //}
        //public ETagTransactionModel(string maGiaoDich, string thoiGianGiaoDich, string maETag, string giaVe,
        //    string soXeNhanDang, string maNhanVien, string maCa, string malan, string mave)
        //{
        //    MaGiaoDich = maGiaoDich;
        //    ThoiGianGiaoDich = thoiGianGiaoDich;
        //    MaETag = maETag;
        //    GiaVe = giaVe;
        //    SoXeNhanDang = soXeNhanDang;
        //    MaNhanVien = maNhanVien;
        //    MaCa = maCa;
        //    MaLan = malan;
        //    MaVe = mave;
        //}
    }
}
