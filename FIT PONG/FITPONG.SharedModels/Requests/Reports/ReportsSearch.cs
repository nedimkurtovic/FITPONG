using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Reports
{
    public class ReportsSearch:SearchBase
    {
        public string Naslov { get; set; }
        public DateTime Datum{ get; set; }

        public ReportsSearch()
        {
            Limit = 10;
        }
        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }
    }
}
