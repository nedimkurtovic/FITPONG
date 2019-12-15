using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.IgracVMs
{
    public class IgracEditSlikuVM:IgracDodajVM
    {
        public int ID { get; set; }
        //public IFormFile Slika { get; set; }
        public string ExistingProfileImagePath { get; set; }
    }
}
