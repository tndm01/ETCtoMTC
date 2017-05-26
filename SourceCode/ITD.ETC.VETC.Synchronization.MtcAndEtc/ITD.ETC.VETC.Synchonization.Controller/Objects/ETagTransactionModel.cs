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

        public DateTime ThoiGianGiaoDich { get; set; }

        public string MaETag { get; set; }

        public int GiaVe { get; set; }

        public string SoXeNhanDang { get; set; }

        public string MaNhanVien { get; set; }

        public string MaCa { get; set; }

        public string MaLan { get; set; }

        public string MaVe { get; set; }

        public string SoXeDangKy { get; set; }

    }
}
