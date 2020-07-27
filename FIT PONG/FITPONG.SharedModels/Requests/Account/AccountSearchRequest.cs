using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class AccountSearchRequest:SearchBase
    {
        public string PrikaznoIme { get; set; }

        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }
    }
}
