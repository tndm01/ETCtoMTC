using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class CommuterTicketTransactionModel : ModelBase
    {
        public string TID { get; set; }
        public string GIOSOAT { get; set; }
        public string NGAYSOAT { get; set; }
        public string MSLANE { get; set; }
        public string Login { get; set; }
        public string Ca { get; set; }
        public string MSLoaive { get; set; }
        public string MSloaixe { get; set; }
        public string Checker { get; set; }
        public string SoXe_ND { get; set; }
        public string F0 { get; set; }
        public string F1 { get; set; }
        public string F2 { get; set; }
        public string ImageID { get; set; }
        public string SyncEtcMtc { get; set; }
        public string SyncFeBe { get; set; }
        public string EtagID { get; set; }
    }
}
