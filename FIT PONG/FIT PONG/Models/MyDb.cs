using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class MyDb:DbContext
    {
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Bracket> Brackets { get; set; }
        public DbSet<Igrac_Utakmica> IgraciUtakmice { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Prijava> Prijave { get; set; }
        public DbSet<Prijava_igrac> PrijaveIgraci { get; set; }
        public DbSet<Runda> Runde { get; set; }
        public DbSet<Sistem_Takmicenja> SistemiTakmicenja { get; set; }
        public DbSet<Stanje_Prijave> StanjaPrijave { get; set; }
        public DbSet<Status_Takmicenja> StatusiTakmicenja { get; set; }
        public DbSet<Status_Utakmice> StatusiUtakmice { get; set; }
        public DbSet<Takmicenje> Takmicenja { get; set; }
        public DbSet<Tip_Rezultata> TipoviRezultata { get; set; }
        public DbSet<Tip_Utakmice> TipoviUtakmica { get; set; }
        public DbSet<Utakmica> Utakmice { get; set; }
        public DbSet<Vrsta_Takmicenja> VrsteTakmicenja { get; set; }
        public DbSet<Grad> Gradovi { get; set; }
        public DbSet<Igrac> Igraci { get; set; }
        public DbSet<Konverzacija> Konverzacije { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Permisija> Permisije { get; set; }
        public DbSet<Poruka> Poruke { get; set; }
        public DbSet<Postovanje> Postovanja { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Statistika> Statistike { get; set; }
        public DbSet<TipStatusaPoruke> TipoviStatusaPoruke { get; set; }
        public DbSet<Uloga> Uloge { get; set; }
        public DbSet<UlogaPermisija> UlogePermisije { get; set; }
        public DbSet<User> Useri { get; set; }
        public DbSet<UserKonverzacija> UseriKonverzacije { get; set; }
        public DbSet<UserUloga> UseriUloge { get; set; }
        public DbSet<Objava> Objave { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedObjava> FeedsObjave { get; set; }
        public DbSet<BlokLista> BlokListe { get; set; }



        //ostaje pitanje za razmotrit kako ce se ovo ponasat sa identitycontextom jer cemo morat mijenjat ono sto nasljedjuje
        //nas kontekst(taj konteskt dodaje one tabele i to),vjerujem da i on nasljedjuje context
        //obicni samo ne znam kako ce se konkretno ponasati ovo base(opcije) da li ce identitycontext znat to proslijedit??
        public MyDb(DbContextOptions<MyDb> opcije) : base(opcije) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Ova funkcija sad postaje prazna jer cemo injectat u konstruktor kreirane MyDb options
        }
        public MyDb() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Postovanje>()
                .HasKey(pp => new { pp.PostivalacID, pp.PostovaniID });

            modelBuilder.Entity<Prijava_igrac>()
             .HasKey(pi => new { pi.IgracID, pi.PrijavaID});

            modelBuilder.Entity<UlogaPermisija>()
             .HasKey(up => new { up.UlogaID, up.PermisijaID});

            modelBuilder.Entity<UserKonverzacija>()
             .HasKey(uk => new { uk.UserID, uk.KonverzacijaID});

            modelBuilder.Entity<Igrac_Utakmica>()
             .HasKey(iu => new { iu.IgracID, iu.UtakmicaID});

            modelBuilder.Entity<UserUloga>()
             .HasKey(uu => new { uu.UserID, uu.UlogaID});

            modelBuilder.Entity<BlokLista>()
             .HasKey(bl => new { bl.IgracID, bl.TakmicenjeID});


        }


    }
}
