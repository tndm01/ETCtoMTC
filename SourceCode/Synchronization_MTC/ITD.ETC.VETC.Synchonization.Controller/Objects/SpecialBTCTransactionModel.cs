using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class SpecialBTCTransactionModel : ModelBase
    {
        #region Properties
        public string ID { get; set; }
        public string LoaiXe { get; set; }
        public string MsLane { get; set; }
        public string Login { get; set; }
        public string Ngayqua { get; set; }
        public int Gioqua { get; set; }
        public string Ca { get; set; }
        public string Checker { get; set; }
        public string F0 { get; set; }
        public string F1 { get; set; }
        public string F2 { set; get; }
        public string Soxe_ND { set; get; }
        public string ImageID { set; get; }
        public string IsConsider { set; get; }
        public int SyncEtcMtc { set; get; }
        public int SyncFebe { set; get; }
        public string EtagId { set; get; }
        public string ThoiGianGiaoDich { set; get;}
        #endregion
    }
}
