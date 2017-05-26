using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ObjectTollTicket : ModelBase
    {
        public string TID { get; set; }
        public string LOGIN { get; set; }
        public int GIAVE { get; set; }
        public int SOLUONG { get; set; }
        public string VEDAU { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string CAXUAT { get; set; }
        public string MANVXUAT { get; set; }


        public ObjectTollTicket() { }

        public ObjectTollTicket(string iTD,string login) 
        {
            TID = iTD;
            LOGIN = login;
        }
    }
}
