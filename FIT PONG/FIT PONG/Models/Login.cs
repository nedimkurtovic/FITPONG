using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Login
    {
        public int ID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash{ get; set; }
        [Required]
        public string PasswordSalt { get; set; }

        

    }
}
