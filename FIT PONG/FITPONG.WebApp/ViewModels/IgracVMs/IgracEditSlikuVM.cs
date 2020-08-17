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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public int ID { get; set; }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
                              //public IFormFile Slika { get; set; }
        public string ExistingProfileImagePath { get; set; }
    }
}
