using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Objave
{
    public class ObjaveSearch:SearchBase
    {
        public string Naziv { get; set; }

        public override object Clone()
        {
            var jsonStr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonStr, this.GetType());
        }
    }
}
