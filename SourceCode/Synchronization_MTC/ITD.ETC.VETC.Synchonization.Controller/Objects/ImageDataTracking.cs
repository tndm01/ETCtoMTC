using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITD.ETC.VETC.Synchonization.Controller.Objects
{
    public class ImageDataTracking
    {
        public String ImageID { get; set; }

        public long ImageTrackingID { get; set; }

        public int ImageTrackingStatus { get; set; }

        public string LaneID { get; set; }
    }
}
