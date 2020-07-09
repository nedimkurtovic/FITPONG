using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Takmicenja
{
    public class TakmicenjeSearch:SearchBase
    {
        public string Naziv { get; set; }

        public override object Clone()
        {
            var jsonObjString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonObjString,this.GetType());
        }
    }
}
