using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Google.Authenticator;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using FIT_PONG.Services.Services.Autorizacija;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using FIT_PONG.Services.ML;
using Microsoft.EntityFrameworkCore.Internal;

namespace FIT_PONG.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly MyDb db;
        private readonly UserManager<IdentityUser<int>> usermanager;
        private readonly SignInManager<IdentityUser<int>> signinmanager;
        private readonly iEmailServis mailservis;
        private readonly IMapper mapper;
        private readonly IUsersAutorizator usersAutorizator;
        static MLContext mlContext = null;
        static ITransformer model = null;
        public UsersService(MyDb db,
                            SignInManager<IdentityUser<int>> singinmanager,
                            UserManager<IdentityUser<int>> usermanager,
                            iEmailServis mailservis,
                            IMapper mapper,
                            IUsersAutorizator usersAutorizator)
        {
            this.db = db;
            this.usermanager = usermanager;
            this.signinmanager = singinmanager;
            this.mailservis = mailservis;
            this.mapper = mapper;
            this.usersAutorizator = usersAutorizator;
        }

        public List<Users> Get(AccountSearchRequest obj)
        {
            var users = new List<Database.DTOs.Igrac>();
            if (obj.PrikaznoIme == null)
                users = db.Igraci.ToList();
            else
                users = db.Igraci.Where(x => x.PrikaznoIme.Contains(obj.PrikaznoIme)).ToList();

            var list = new List<SharedModels.Users>();

            foreach (var user in users)
            {
                var u = mapper.Map<SharedModels.Users>(user);
                u.ProfileImage = ProcesDobavljanjaSlike(user.ProfileImagePath);

                //try
                //{
                //    byte[] binarniZapis = File.ReadAllBytes(user.ProfileImagePath);
                //    Fajl fajl = new Fajl
                //    {
                //        Naziv = user.ProfileImagePath.Substring(user.ProfileImagePath.LastIndexOf("/")),
                //        BinarniZapis = binarniZapis
                //    };

                //    u.ProfileImage = fajl;

                //}
                //catch (Exception ex)
                //{

                //    throw ex;
                //}

                u.listaPrijava = GetPrijave(user.ID);
                u.statistike = mapper.Map<List<SharedModels.Statistike>>(db.Statistike.Where(d => d.IgracID == user.ID).ToList());
                list.Add(u);
            }

            return list;
        }

        public Users Get(int ID)
        {
            var user = db.Igraci.Find(ID);
            if (user == null)
                throw new UserException("Korisnik ne postoji u bazi.");

            var u = mapper.Map<SharedModels.Users>(user);
            u.ProfileImage = ProcesDobavljanjaSlike(user.ProfileImagePath);

            u.listaPrijava = GetPrijave(user.ID);
            u.statistike = mapper.Map<List<SharedModels.Statistike>>(db.Statistike.Where(d => d.IgracID == user.ID).ToList());

            return mapper.Map<SharedModels.Users>(u);
        }

        public async Task<Users> Register(AccountInsert obj)
        {
            if (PostojiPrikaznoIme(obj.PrikaznoIme))
                throw new UserException("Prikazno ime koje ste unijeli je vec zauzeto.");

            var user = new IdentityUser<int>
            {
                UserName = obj.Email,
                Email = obj.Email
            };

            var result = await usermanager.CreateAsync(user, obj.Password);
            if (result.Succeeded)
            {
                var igrac = new Igrac
                {
                    ID = user.Id,
                    ELO = 1000,
                    GradID = obj.GradId,
                    BrojPosjetaNaProfil = 0,
                    JacaRuka = obj.JacaRuka,
                    PrikaznoIme = obj.PrikaznoIme,
                    Spol = obj.Spol,
                    Visina = obj.Visina,
                    TwoFactorEnabled = false
                };
                if (obj.Slika != null)
                    igrac.ProfileImagePath = "~/igraci/" + ProcesSpremanjaSlike(obj.Slika);

                db.Add(igrac);
                db.SaveChanges();

                var token = await usermanager.GenerateEmailConfirmationTokenAsync(user);
                mailservis.PosaljiKonfirmacijskiMejl(token, user.Email, "api");
                return mapper.Map<SharedModels.Users>(obj);
            }
            else
            {
                var exception = new UserException();
                foreach (var error in result.Errors)
                {
                    exception.AddError(error.Code, error.Description);
                }
                throw exception;
            }

        }

        public async Task<Users> Login(Login obj)
        {
            var korisnik = await usermanager.FindByEmailAsync(obj.UserName);
            if (korisnik == null)
            {
                UserException ex = new UserException();
                ex.AddError("", "Neispravni podaci za login");
                throw ex;
            }
            
            var igrac = db.Igraci.Find(korisnik.Id);

            var rezultat = await signinmanager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false);

            if (rezultat.IsLockedOut)
            {
                TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                throw new Exception("Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
            }
            else if (rezultat.Succeeded)
            {
                var povratni = Get(igrac.ID);
                return povratni;
            }
            else if (await signinmanager.UserManager.CheckPasswordAsync(korisnik, obj.Password))
            {
                throw new UserException("Morate potvrditi mejl prije logiranja");
            }
            else
            {
                throw new UserException("Korisnik ne postoji");
            }

        }
        public async Task<SharedModels.Users> CheckValidanLogin(Login obj)
        {
            var korisnik = await usermanager.FindByEmailAsync(obj.UserName);
            if (korisnik == null)
            {
                UserException ex = new UserException();
                ex.AddError("", "Neispravni podaci za login");
                throw ex;
            }
            var igrac = db.Igraci.Find(korisnik.Id);
            var rezultat = await usermanager.CheckPasswordAsync(korisnik, obj.Password);
            if (rezultat)
                return mapper.Map<SharedModels.Users>(igrac);
            return null;
        }
        //OVO OSTAJE ZA RAZMISLJANJE...

        //public async void Logout(int id, string username)
        //{
        //    await signinmanager.SignOutAsync();
        //}

        public Users EditujProfil(int id, AccountUpdate obj)
        {
            Igrac igrac = db.Igraci.Find(id);
            if (igrac == null)
                throw new UserException("Igrac ne postoji u bazi.");

            if (obj.PrikaznoIme != igrac.PrikaznoIme && PostojiPrikaznoIme(obj.PrikaznoIme))
                throw new UserException("Prikazno ime je zauzeto.");

            igrac.JacaRuka = obj.JacaRuka;
            igrac.Visina = obj.Visina;
            igrac.PrikaznoIme = obj.PrikaznoIme;
            if (obj.GradId != 0)
                igrac.GradID = obj.GradId;
            db.Update(igrac);
            db.SaveChanges();

            return mapper.Map<SharedModels.Users>(igrac);
        }


        public async Task<string> SendConfirmationEmail(Email_Password_Request obj)
        {
            var user = await usermanager.FindByEmailAsync(obj.Email);

            if (user != null && !user.EmailConfirmed)
            {
                var token = await usermanager.GenerateEmailConfirmationTokenAsync(user);
                mailservis.PosaljiKonfirmacijskiMejl(token, user.Email, "api");
                return "Konfirmacijski mail uspjesno poslan.";
            }
            else
            {
                throw new UserException("Korisnik ne postoji u bazi ili je mail vec potvrdjen.");
            }

        }

        public async Task<string> SendPasswordChange(Email_Password_Request obj)
        {
            var user = await usermanager.FindByEmailAsync(obj.Email);
            if (user != null && user.EmailConfirmed)
            {
                var token = await usermanager.GeneratePasswordResetTokenAsync(user);

                try
                {
                    mailservis.PosaljiResetPassword(token, obj.Email, "api");
                    return "Poslan mail za promjenu passworda.";
                }
                catch (Exception)
                {
                    throw new UserException("Doslo je do greske prilikom promjene passworda.");
                }
            }
            throw new UserException("Korisnik ne postoji ili nije povrdio mail.");

        }

        public async Task<string> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                throw new UserException("UserID i token ne smiju biti null.");

            var user = await usermanager.FindByIdAsync(userId);
            if (user == null)
                throw new UserException("User ne postoji u bazi");

            if (user.EmailConfirmed)
                throw new UserException("Mail je vec potvrdjen.");

            var rezultat = await usermanager.ConfirmEmailAsync(user, token);
            if (!rezultat.Succeeded)
                throw new UserException("Doslo je do greske prilikom potvrde mejla.");



            return "Uspjesno ste potvrdili mail.";
        }

        public async Task<string> ConfirmPasswordChange(string loggedInUserName, PasswordPromjena obj)
        {
            var u = db.Igraci.Where(d => d.PrikaznoIme == loggedInUserName).FirstOrDefault();

            if (u != null)
            {
                var user = await usermanager.FindByIdAsync(u.ID.ToString());
                if (user != null)
                {
                    if (obj.password != obj.potvrdaPassword)
                        throw new UserException("Passwordi se ne slazu.");

                    var rezultat = await usermanager.ResetPasswordAsync(user, obj.token, obj.password);
                    if (rezultat.Succeeded)
                        return "Password je uspjesno resetovan.";
                    else
                    {
                        UserException exception = new UserException();

                        foreach (var error in rezultat.Errors)
                            exception.AddError("", error.Description);

                        throw exception;
                    }
                }
            }
            throw new UserException("User ne postoji u bazi.");
        }

        public string Postovanje(string loggedInUserName, int postovaniID)
        {
            var user1 = db.Users.Where(d => d.Email == loggedInUserName).FirstOrDefault();
            var user2 = db.Igraci.Find(postovaniID);

            if (user1 == null || user2 == null)
                throw new UserException("User ne postoji u bazi.");

            int postivalacID = user1.Id;

            var postovanje = db.Postovanja.Where(p => p.PostivalacID == postivalacID && p.PostovaniID == postovaniID).SingleOrDefault();
            if (postovanje != null)
                db.Remove(postovanje);
            else
            {
                postovanje = new Database.DTOs.Postovanje(postivalacID, postovaniID);
                db.Add(postovanje);
            }

            db.SaveChanges();

            return "Postovanje uspjesno azurirano.";
        }

        public string ResetProfilePicture(string loggedInUserName, int id)
        {
            var user = db.Users.Where(u => u.Email == loggedInUserName).FirstOrDefault();
            if (user == null)
                throw new UserException("User ne smije biti null.");

            usersAutorizator.AuthorizeUkloniSlikuProfila(user.Id, id);

            ValidirajUklanjanjeSlike(id);
            Igrac igrac = db.Igraci.Find(id);
            //var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            //if (userId != igracID)
            //    return VratiNijeAutorizovan();
            try
            {
                if (igrac.ProfileImagePath != "/profile_picture_default.png")
                {
                    ProcesBrisanjaSlike(igrac.ProfileImagePath);
                    igrac.ProfileImagePath = "/profile_picture_default.png";
                    db.SaveChanges();

                    return "Slika profila uspjesno uklonjena.";
                }

                return "Ne mozete ukloniti defaultnu sliku.";
            }
            catch (Exception)
            {

                throw new UserException("Doslo je do greske prilikom uklanjanja slike profila.");
            }

        }

        public string UpdateProfilePicture(string loggedInUserName, int id, Fajl Slika)
        {
            var user = db.Users.Where(u => u.Email == loggedInUserName).FirstOrDefault();
            if (user == null)
                throw new UserException("User ne smije biti null.");

            usersAutorizator.AuthorizeEditSlikuProfila(user.Id, id);
            ValidirajUpdateSliku(id, Slika);

            Igrac igrac = db.Igraci.Find(id);

            try
            {
                if (igrac.ProfileImagePath != null && igrac.ProfileImagePath != "/profile_picture_default.png")
                    ProcesBrisanjaSlike(igrac.ProfileImagePath);
                igrac.ProfileImagePath = "~/igraci/" + ProcesSpremanjaSlike(Slika);
                db.SaveChanges();

                return "Slika uspjesno promjenjena.";
            }
            catch (Exception)
            {
                throw new UserException("Doslo je do greske prilikom promjene slike.");
            }
        }

        public Users Suspenduj(int userId, SuspenzijaRequest obj)
        {
            Igrac igrac = db.Igraci.Find(userId);
            if (igrac == null)
                throw new UserException("Igrac ne postoji u bazi.");

            VrstaSuspenzije vrsta = db.VrsteSuspenzije.Where(d => d.Opis == obj.VrstaSuspenzije).SingleOrDefault();
            if (vrsta == null)
                throw new UserException("Vrsta suspenzije nije validna.");

            if (obj.DatumPocetka > obj.DatumZavrsetka)
                throw new UserException("Datum pocetka mora biti prije datuma zavrsetka suspenzije.");

            var suspenzija = new Suspenzija
            {
                IgracID = userId,
                DatumPocetka = obj.DatumPocetka,
                DatumZavrsetka = obj.DatumZavrsetka,
                VrstaSuspenzijeID = vrsta.ID
            };

            db.Add(suspenzija);
            db.SaveChanges();

            return mapper.Map<SharedModels.Users>(igrac);
        }


        //*********************************************************
        //              POMOCNE FUNKCIJE                           
        //*********************************************************

        //private async Task<Users> ProvjeriAutentifikaciju(Login obj)
        //{
        //    var korisnik = await usermanager.FindByEmailAsync(obj.UserName);

        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string userUniqueKey = GetUserUniqueKey(korisnik);
        //    var token = obj.Code;
        //    bool isValid = tfa.ValidateTwoFactorPIN(userUniqueKey, token);
        //    if (LockoutCheck(korisnik))
        //    {
        //        TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
        //        throw new UserException("Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
        //    }
        //    else
        //    {

        //        if (obj.Code != null && obj.Code == token)
        //        {
        //            await signinmanager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false);
        //            return mapper.Map<SharedModels.Users>(korisnik);
        //        }
        //        else
        //        {
        //            throw new UserException("Code je neispravan.");
        //        }
        //    }
        //}

        //private string GetUserUniqueKey(IdentityUser<int> korisnik)
        //{
        //    string key = korisnik.SecurityStamp.Substring(5, 10);
        //    string useruniquekey = korisnik.Email + key;
        //    return useruniquekey;
        //}

        private bool LockoutCheck(IdentityUser<int> korisnik)
        {
            if (korisnik.LockoutEnd == null)
                return false;
            if (korisnik.LockoutEnd < DateTime.Now)
            {
                if (korisnik.AccessFailedCount < 2)
                {
                    korisnik.AccessFailedCount++;
                    db.Update(korisnik);
                    db.SaveChanges();
                    return false;
                }
                korisnik.LockoutEnd = DateTime.Now.AddMinutes(1);
                korisnik.AccessFailedCount = 0;
                db.Update(korisnik);
                db.SaveChanges();
                return true;
            }
            return true;
        }

        private List<Prijave> GetPrijave(int userId)
        {
            var prijaveIgraci = db.PrijaveIgraci.Include(p => p.Prijava).Where(d => d.IgracID == userId).ToList();
            var prijave = new List<SharedModels.Prijave>();

            foreach (var pi in prijaveIgraci)
            {
                var prijava = new SharedModels.Prijave
                {
                    ID = pi.PrijavaID,
                    Naziv = pi.Prijava.Naziv,
                    Igrac1ID = pi.IgracID,
                    isTim = false
                };

                if (pi.Prijava.isTim)
                {
                    prijava.isTim = true;
                    var prijava2 = db.PrijaveIgraci
                                        .Include(p => p.Prijava)
                                        .Where(p => p.Prijava.Naziv == pi.Prijava.Naziv && p.IgracID != pi.IgracID)
                                        .FirstOrDefault();

                    if (prijava2 != null)
                        prijava.Igrac2ID = prijava2.IgracID;
                }
                prijave.Add(prijava);
            }

            return prijave;
        }
        public int GetRequestUserID(HttpRequest Request)
        {
            var credentials = GetCredentials(Request);
            var username = credentials[0];
            var user = GetUsera(username);
            return user.Id;

        }
        public string GetRequestUserName(HttpRequest Request)
        {
            var credentials = GetCredentials(Request);
            var username = credentials[0];
            var user = GetUsera(username);
            //potrebno prvojeriti da li ce se vracati rpikazno ime ili users.username i jos bitno:
            //potrebno je provjeriti da li ce se slati prikazno ime u authorization headeru
            //ili email, to sve zavisi od toga kako napravimo usera kad se kreira novi
            return user.UserName;
        }
        //public string GetEmail(HttpRequest Request)
        //{
        //    var credentials = GetCredentials(Request);
        //    var username = credentials[0];
        //    var user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
        //    return user != null ? user.Email : "";
        //}
        private string[] GetCredentials(HttpRequest Request)
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            return credentials;
        }
        private IdentityUser<int> GetUsera(string username)
        {
            var user = db.Users.Where(x => x.UserName == username).FirstOrDefault();
            if (user != null)
                return user;
            throw new UserException("User ne postoji ili je obrisan");
        }
        private bool PostojiPrikaznoIme(string prikaznoIme)
        {
            var igrac = db.Igraci.Where(i => i.PrikaznoIme == prikaznoIme).FirstOrDefault();

            return igrac != null;
        }
        private void ValidirajUpdateSliku(int id, Fajl Slika)
        {
            UserException exception = new UserException();

            Igrac igrac = db.Igraci.Find(id);

            if (igrac == null)
                exception.AddError(nameof(Igrac), "Igrac ne smije biti null.");

            if (Slika == null)
                exception.AddError(nameof(Slika), "Morate uploadat sliku.");

            if (!IsDozvoljenTip(Slika))
                exception.AddError(nameof(Slika), "Nedozovljen format fajla. Mozete uploadati samo slike.");

            if (exception.Errori.Count > 0)
                throw exception;
        }
        private void ValidirajUklanjanjeSlike(int id)
        {
            UserException exception = new UserException();

            Igrac igrac = db.Igraci.Find(id);

            if (igrac == null)
                exception.AddError(nameof(Igrac), "Igrac ne smije biti null.");

            if (exception.Errori.Count > 0)
                throw exception;
        }
        private bool IsDozvoljenTip(Fajl file)
        {
            int index = file.Naziv.LastIndexOf(".");
            string ekstenzija = file.Naziv.Substring(index);
            if (ekstenzija != "png" || ekstenzija != "jpg" || ekstenzija != "jpeg")
                return false;
            return true;
        }
        private string ProcesSpremanjaSlike(Fajl file)
        {
            var rootPathAplikacije = "~"; //istraziti kako izvuci root path aplikacije !!!

            using (MemoryStream ms = new MemoryStream(file.BinarniZapis))
            {
                Directory.CreateDirectory(Path.Combine(rootPathAplikacije, "igraci"));
                string imeFajla = Guid.NewGuid().ToString() + "_" + file.Naziv;
                string pathSpremanja = Path.Combine(rootPathAplikacije, "igraci", imeFajla);
                using (FileStream fileStream = new FileStream(pathSpremanja, FileMode.Create))
                    ms.CopyTo(fileStream);

                return imeFajla;
            }
        }
        private Fajl ProcesDobavljanjaSlike(string Path)
        {
            Fajl povratni = new Fajl();
            if (File.Exists(Path))
            {
                byte[] binZapis = File.ReadAllBytes(Path);
                var ime = Path.Substring(Path.LastIndexOf("/") + 1);
                povratni.BinarniZapis = binZapis;
                povratni.Naziv = ime;
                return povratni;
            }
            return null;

        }
        private void ProcesBrisanjaSlike(string putanja)
        {
            var rootPathAplikacije = "~"; //istraziti kako izvuci root path aplikacije !!!

            string filePutanja = Path.Combine(rootPathAplikacije, putanja.Substring(putanja.IndexOf("/") + 1));
            System.IO.File.Delete(filePutanja);
        }

        public List<SharedModels.Users> RecommendPostovanja(int id)
        {
            if (mlContext == null)
            {
                //STEP 1: Create MLContext to be shared across the model creation workflow objects 
                mlContext = new MLContext();

                var SviUseriSaPostovanjima = GetUserePostovanja();

                var data = new List<UserEntry>();
                foreach(var i in SviUseriSaPostovanjima)
                {
                    if(i.Postovanja.Count > 1)
                    {
                        var SviKojiPostujuFrajera = i.Postovanja.Select(x => x.PostivalacID).ToList();
                        SviKojiPostujuFrajera.ForEach(x =>
                        {
                            var povezaniBezNjega = i.Postovanja.Where(z => z.PostivalacID != x).ToList();
                            povezaniBezNjega.ForEach(z =>
                            {
                                data.Add(new UserEntry
                                {
                                    UserID = (uint)x,
                                    CoPostovanjeUserID = (uint)z.PostivalacID
                                });
                            });
                        });
                    }
                }

                var traindata = mlContext.Data.LoadFromEnumerable(data);
                //STEP 3: Your data is already encoded so all you need to do is specify options for MatrxiFactorizationTrainer with a few extra hyperparameters
                //        LossFunction, Alpa, Lambda and a few others like K and C as shown below and call the trainer. 
                MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
                options.MatrixColumnIndexColumnName = nameof(UserEntry.UserID);
                options.MatrixRowIndexColumnName = nameof(UserEntry.CoPostovanjeUserID);
                options.LabelColumnName = "Label";
                options.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
                options.Alpha = 0.01;
                options.Lambda = 0.025;
                // For better results use the following parameters
                //options.K = 100;
                options.C = 0.00001;

                //Step 4: Call the MatrixFactorization trainer by passing options.
                var est = mlContext.Recommendation().Trainers.MatrixFactorization(options);

                //STEP 5: Train the model fitting to the DataSet
                //Please add Amazon0302.txt dataset from https://snap.stanford.edu/data/amazon0302.html to Data folder if FileNotFoundException is thrown.
                model = est.Fit(traindata);
            }


            var sviUseri = db.Igraci.Where(x=>x.ID != id).ToList();
            var rezultati = new List<Tuple<Database.DTOs.Igrac, float>>();
            foreach(var i in sviUseri)
            {
                //STEP 6: Create prediction engine and predict the score for Product 63 being co-purchased with Product 3.
                //        The higher the score the higher the probability for this particular productID being co-purchased 
                var predictionengine = mlContext.Model.CreatePredictionEngine<UserEntry, CoPostovanje_prediction>(model);
                var prediction = predictionengine.Predict(
                                         new UserEntry()
                                         {
                                             UserID = (uint)id,
                                             CoPostovanjeUserID =(uint) i.ID
                                         });
                rezultati.Add(new Tuple<Igrac, float>(i, prediction.Score));
            }

            var poluFinalnaLista = rezultati.OrderByDescending(x => x.Item2).Select(x => x.Item1).Take(3).ToList();
            if(poluFinalnaLista.Count() < 3)
            {
                var pomozi = IgraciIzGrada(id, 3 - poluFinalnaLista.Count());
                poluFinalnaLista.AddRange(pomozi);
            }

            var finalnaLista = new List<SharedModels.Users>();
            foreach(var i in poluFinalnaLista)
            {
                finalnaLista.Add(Get(i.ID));
            }

            return finalnaLista;
        }


        private List<PomocniModelUserPostovanja> GetUserePostovanja()
        {
            var SviUseri = db.Igraci.ToList();
            var UseriSaPostovanjima = new List<PomocniModelUserPostovanja>();
            foreach (var i in SviUseri)
            {
                var postovanja = db.Postovanja.Where(x => x.PostovaniID == i.ID).ToList();
                UseriSaPostovanjima.Add(new PomocniModelUserPostovanja { Igrac = i, Postovanja = postovanja });
            }
            return UseriSaPostovanjima;
        }
        private List<Database.DTOs.Igrac> IgraciIzGrada(int id, int BrojIgraca)
        {
            var podaci = db.Igraci.Include(x => x.Grad).ToList();

            var konkretanIgrac = podaci.Where(x => x.ID == id).FirstOrDefault();

            var gradPoKojemSeTrazi = konkretanIgrac.Grad.Naziv;

            Random _random = new Random();
            var listaIstihPoGradu = new List<Database.DTOs.Igrac>();
            foreach(var i in podaci)
            {
                if (i.Grad.Naziv == gradPoKojemSeTrazi)
                    listaIstihPoGradu.Add(i);
            }

            var Oduzeti = listaIstihPoGradu.Take(BrojIgraca).ToList();
            return Oduzeti;
        }
































    }
}
