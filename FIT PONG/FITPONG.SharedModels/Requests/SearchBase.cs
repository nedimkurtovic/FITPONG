using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests
{
    public abstract class SearchBase:ICloneable
    {
        public int Page { get; set; }
        private int _limit = 10;
        public int Limit { 
            get 
            {
                return _limit;
            } 
            set 
            {
                _limit = (value > maxLimit || value <= 0) ? maxLimit : value;
            }
        }


        private int maxLimit = 30; 
        public SearchBase()
        {
            this.Page = 1;
            this.Limit = 10;
        }
        public abstract object Clone();
    }
}
