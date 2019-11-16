using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Ovo polje je obavezno.")]
        [RegularExpression(@"[a-zA-Z ]*", ErrorMessage ="Polje smije sadržavati samo slova.")]
        [StringLength(30)]
        public string Ime { get; set; }
        
        [Required]
        [RegularExpression(@"[a-zA-Z ]*", ErrorMessage = "Polje smije sadržavati samo slova.")]
        [StringLength(30)]
        public string Prezime { get; set; }
        
        
        public DateTime DatumRodjenja{ get; set; }

        [Required]
        [RegularExpression(@"\w+\.\w+@edu\.fit\.ba", ErrorMessage = "Email mora biti u formatu ime.prezime@edu.fit.ba")]
        [StringLength(60)]
        public string Email { get; set; }

        public int GradID { get; set; }
        public Grad Grad { get; set; }

        public int LoginID { get; set; }
        public Login Login { get; set; }

        public User()
        {
            Ime = "NOT SET";
            Prezime = "NOT SET";
            Email = "NOT SET";
            DatumRodjenja = DateTime.Parse("1.1.1900");
            GradID = -1;
            LoginID = -1;
        }

        public User(string ime, string prezime, DateTime datumRodjenja, string email, int gradID, int loginID)
        {
            Ime = ime;
            Prezime= prezime;
            DatumRodjenja = new DateTime(datumRodjenja.Ticks);
            Email= email;
            GradID = gradID;
            LoginID = loginID;
        }

    }
}
