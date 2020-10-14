using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Aktivnosti
{
    public class AktivnostiSearch:SearchBase
    {
        public override object Clone()
        {
            var jsonStr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonStr, this.GetType());
        }
    }
}
