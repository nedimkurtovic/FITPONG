using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class FeedObjava
    {
        public int ID { get; set; }
        public int FeedID{ get; set; }
        public Feed Feed { get; set; }

        public int ObjavaID { get; set; }
        public Objava Objava{ get; set; }

    }
}
