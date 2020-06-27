using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services;
using ReflectionIT.Mvc.Paging;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.ViewModels.FeedVMs
{
    public class DisplayFeedVM
    {
        public int ID { get; set; }
        public string naziv { get; set; }
        public DateTime DatumModifikacije{ get; set; }
        public PagingList<Objava> Objave { get; set; }
    }
}
