using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Model
{
    public class EnumAndConst
    {
        #region Job Name

        public const string STORE_GET_SYNC_DATA = "";

        #endregion

        public enum JobGoupType
        {
            Mtc = 1,
            Etc = 2,
        }
    }
}
