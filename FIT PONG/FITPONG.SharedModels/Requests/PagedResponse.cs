using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests
{
    public class PagedResponse<T> where T : class
    {
        public List<T> Stavke{ get; set; }
        public int TotalPageCount { get; set; }
        public Uri IducaStranica { get; set; }
        public Uri ProslaStranica { get; set; }
    }
}
