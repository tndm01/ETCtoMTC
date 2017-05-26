using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    class ObjectTollTicketCTPX : ModelBase
    {
        public int MaPX { get; set; }
        public string MSLOAIVE { get; set; } //Get giave
        public string SERIAL_FROM { get; set; }
        public string SERIAL_TO { get; set; }
        public int SOLUONG { get; set; }
        public string Editable { get; set; }

        public string MaNVNhan { get; set; }

        public int GIAVE { get; set; }
        public DateTime NGAYTAO { get; set; }
        public int Ca { get; set; }
        public string MaNVXuat { get; set; }



        public ObjectTollTicketCTPX() { }

        public ObjectTollTicketCTPX(int maPX, string msLoaiXe, string serial_From,string serial_To,int soLuong,string edittable) 
        {
            MaPX = maPX;
            MSLOAIVE = msLoaiXe;
            SERIAL_FROM = serial_From;
            SERIAL_TO = serial_To;
            SOLUONG = soLuong;
            Editable = edittable;
        }
    }
}
