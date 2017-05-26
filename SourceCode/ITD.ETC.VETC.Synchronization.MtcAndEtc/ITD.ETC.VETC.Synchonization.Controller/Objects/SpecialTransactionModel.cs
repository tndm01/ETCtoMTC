using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class SpecialTransactionModel : ModelBase
    {
        #region Properties
        public string NgayMo{ set; get; }
        public string GioMo { get; set; }
        public string Login { get; set; }
        public string MsLane { get; set; }
        public string NgayDong { get; set; }
        public int GioDong { get; set; }
        public string Ca { get; set; }
        public string Tid { get; set; }
        public int Reason { get; set; }
        public string Checker { get; set; }
        public string SoXe_ND { set; get; }
        public string F0 { set; get; }
        public string F1 { set; get; }
        public int F2 { set; get; }
        public string ImageId { set; get; }
        public int IsConsider { set; get; }
        public string Note { set; get; }
        public int FP { set; get; }
        public int SyncEtcMtc { set; get; }
        public int SyncFebe { set; get; }
        public string EtagId { set; get; }
        public DateTime ThoiGianGiaoDich { set; get; }
        public int LoaiUuTien { set; get; }

        
        #endregion
    }
}
