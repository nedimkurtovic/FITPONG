using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Database;

namespace FIT_PONG.Services.BL
{
    public class InitTakmicenja
    {
        private readonly MyDb db;

        public InitTakmicenja(MyDb instanca)
        {
            db = instanca;
        }

        //nedostaje validacija, pogledati init akciju u takmicenje kontroleru u WEBAPP
        public List<(string key, string error)> VratiListuErrora(Takmicenje _takmicenje)
        {
            var listaErrora = new List<(string key, string error)>();
            if (_takmicenje.Inicirano)
                listaErrora.Add(("", "Takmičenje je već inicirano"));
                if (_takmicenje.RokPocetkaPrijave != null && _takmicenje.RokZavrsetkaPrijave > DateTime.Now)
                    listaErrora.Add(("","Rok registracija mora zavrsiti prije generisanja rasporeda"));
            if (_takmicenje.Prijave.Count() < 4)
                listaErrora.Add(("","Takmičenje mora imati barem 4 igrača, otvorite ponovo registracije"));
            return listaErrora;
        }
        public void GenerisiRaspored(Takmicenje _takmicenje)
        {
            using (var transakcija = db.Database.BeginTransaction())
            {
                try
                {
                    Bracket noviBracket = new Bracket
                    {
                        Naziv = _takmicenje.Naziv + " bracket",
                        TakmicenjeID = _takmicenje.ID,
                        Runde = new List<Runda>()
                    };

                    _takmicenje.Bracketi.Add(noviBracket);
                    db.SaveChanges();

                    //kreirati runde(izracunati broj rundi od broja prijava i sistema takmicenja)
                    //otprilike ce biti nesto ovako : 	
                    (int runde, int byeovi) Runde = pomocnaFunkcijaIzracunajRunde(_takmicenje.Sistem, _takmicenje.Prijave.Count());
                    //kreirati runde i za svaku rundu ce biti potrebno kreirati utakmice
                    //otprilike ovako
                    int kopijaBrojIgraca = _takmicenje.Prijave.Count();  //brojIGraca(query je ovo) // ovo je ustvari brojPrijava a ne igraca)
                    int brojUtakmice = 1;
                    for (int i = 0; i < Runde.runde; i++)
                    {
                        Runda runda = new Runda
                        {
                            BracketID = noviBracket.ID,
                            BrojRunde = i + 1,
                            Utakmice = new List<Utakmica>()
                        };// atributi : BracketID postaviti, brojRunde = i+1 // datum pocetka bi se mogao pri evidenciji meca rjesavat,prvi 												mec koji se evidentira updateuje pocetkarunde datum
                        noviBracket.Runde.Add(runda);
                        db.SaveChanges();
                        KreirajTekmeURundi(ref kopijaBrojIgraca, runda.ID, _takmicenje.Sistem, ref brojUtakmice, _takmicenje.Vrsta);
                    }
                    RasporediIgrace(_takmicenje);

                    db.SaveChanges();
                    _takmicenje.Inicirano = true;
                    db.SaveChanges();
                    transakcija.Commit();
                }
                catch (Exception err)
                {
                    transakcija.Rollback();                    
                    throw err;
                }
            }
        }
        public void KreirajTekmeURundi(ref int kopijaBrojIgraca, int rundaID, Sistem_Takmicenja sistem, ref int brojUtakmice, Vrsta_Takmicenja vrstaTakmicenja)
        {
            switch (sistem.Opis)
            {
                //pripaziti na hardkodirano !!!! 
                case "Single elimination": KreirajTekmeUrundiSingleElimination(ref kopijaBrojIgraca, rundaID, ref brojUtakmice, vrstaTakmicenja); break;
                case "Round robin": KreirajTekmeURundiRoundRobin(ref kopijaBrojIgraca, rundaID, ref brojUtakmice, vrstaTakmicenja); break;
            }
        }
        public void RasporediIgrace(Takmicenje novoTakmicenje)//za round robin ne vrijedi seeded,samo za singl i double elim
        {
            switch (novoTakmicenje.Sistem.Opis)
            {
                case "Single elimination": RasporediIgraceSingleElimination(novoTakmicenje); break;
                case "Round robin": RasporediIgraceRoundRobin(novoTakmicenje); break;
            }
        }
        public void KreirajTekmeUrundiSingleElimination(ref int kopijaIgraca, int rundaID, ref int BrojUtakmice, Vrsta_Takmicenja vrstaTakmicenja)// i jos ostali potrebniparams)//single elimination only
        {
            if (!PotencijaDvice(kopijaIgraca))// ovo ce se jednom izvrsit
                kopijaIgraca = NadjiIducuVecuPotenciju(kopijaIgraca);
            kopijaIgraca /= 2;//broj utakmica
            for (int i = 0; i < kopijaIgraca; i++)
            {
                KreirajUtakmicu(rundaID, ref BrojUtakmice, vrstaTakmicenja);
            }
        }
        public void KreirajTekmeURundiRoundRobin(ref int kopijaBrojIgraca, int rundaID, ref int BrojUtakmice, Vrsta_Takmicenja vrstaTakmicenja)
        //konkretno za round robina ovo kopijabrojigraca i broj byeova se ne mijenja,broj byeova cak i ne treba ne odredjuje nista
        // moglo bi se izbacit to skroz
        {
            int brojTekmiPoRundi = -1;
            if (kopijaBrojIgraca % 2 == 0)
                brojTekmiPoRundi = kopijaBrojIgraca / 2;

            else
                brojTekmiPoRundi = (kopijaBrojIgraca + 1) / 2;

            for (int i = 0; i < brojTekmiPoRundi; i++)
                KreirajUtakmicu(rundaID, ref BrojUtakmice, vrstaTakmicenja);
        }

        public void RasporediIgraceSingleElimination(Takmicenje takmicenje)
        {
            List<(int prijavaID, int ELO)> Prijave = NadjiELOPrijavama(takmicenje); // dobavlja elo i smjesta ga u tuple s prijavom
            Prijave.Sort((y, x) => x.ELO.CompareTo(y.ELO)); // sortira silazno po elou 
            //jquery bracket prikazuje byeove ako kao ime igraca/tima posaljes NULL ,ja bih mogao iskoristit to i imati "univerzalan 	
            //slucaj" gdje cu samo prvu rundu postavit

            Runda runda = takmicenje.Bracketi.First().Runde.Where(x => x.BrojRunde == 1).FirstOrDefault();
            Runda drugaRunda = takmicenje.Bracketi.First().Runde.Where(x => x.BrojRunde == 2).FirstOrDefault();
            //kod single elim je samo jedan bracket
            List<(int? prijavaA, int? prijavaB)> Parovi = GetParove(Prijave);
            // dobavlja parove od prijava  ukljucujuci i byeove koji su null
            int BrojacParova = 0;
            foreach (Utakmica x in runda.Utakmice)
            {
                (int? pr1, int? pr2) Par = Parovi[BrojacParova++];
                RasporediUcescaNaUtakmicu(x, Par, true);
                db.SaveChanges();

                if (Par.pr1 == null || Par.pr2 == null)
                //ako je bye,odma ga na drugu rundu dodaj jer se taj mec nece manualno evidentirati kao ostali
                {
                    int BrojOdgovarajuceIduceUtakmice = NadjiOdgovarajucuIducuUtakmicu(x.BrojUtakmice, runda.Utakmice.Count());
                    Utakmica IducaRundaUtakmica =
                        db.Utakmice.Include(c => c.UcescaNaUtakmici)
                        .Where(c => c.Runda == drugaRunda && c.BrojUtakmice == BrojOdgovarajuceIduceUtakmice)
                        .FirstOrDefault();
                    RasporediUcescaNaUtakmicu(IducaRundaUtakmica, Par, true);
                    db.SaveChanges();
                }
            }
        }
        public int NadjiOdgovarajucuIducuUtakmicu(int brojTrenutneUtakmice, int BrojUtakmicaUTrenutnojRundi, int BrojPrveUtakmiceTrenutneRunde = 1)
        {
            int brojac = BrojPrveUtakmiceTrenutneRunde;
            for (int i = BrojUtakmicaUTrenutnojRundi; i >= 0; i--)
            {
                if (brojac == brojTrenutneUtakmice)
                    return brojTrenutneUtakmice + i;
                else
                    if ((brojac + 1) == brojTrenutneUtakmice)
                    return brojac + i;
                brojac += 2;
            }
            return -1;
        }
        public void RasporediUcescaNaUtakmicu(Utakmica x, (int? pr1, int? pr2) Par, bool PostaviELO)
        {
            int indexUcesca = x.OdgovarajuceMjestoUcesca();
            List<Igrac_Utakmica> UcesceUtakmice = x.UcescaNaUtakmici.GetRange(indexUcesca, x.UcescaNaUtakmici.Count() - indexUcesca);
            List<Igrac> igraciNaUtakmici = nadjiIgraceUPrijavama(Par.pr1, Par.pr2);
            int brojacIgraca = 0;
            foreach (Igrac_Utakmica iu in UcesceUtakmice)
            {
                //promijeni iu.IgracID na jednog od igraca iz IgracNaUtakmici[mozdabrojac++];
                //postavi pristupniELo na elo iz Igraca	
                if (brojacIgraca >= igraciNaUtakmici.Count())
                    iu.Igrac = null;
                else
                {
                    iu.Igrac = igraciNaUtakmici[brojacIgraca];
                    iu.IgracID = igraciNaUtakmici[brojacIgraca].ID;
                    if (PostaviELO)
                        iu.PristupniElo = igraciNaUtakmici[brojacIgraca].ELO;
                }
                brojacIgraca++;
                db.SaveChanges();
            }
        }
        public void RasporediIgraceRoundRobin(Takmicenje takmicenje)
        {
            //ok otprilike pseudoKod ide ovako nekako : 
            //prijave sortiraj po elou - isto kao u single elim mada je upitno da li je potrebno ovo,mozda je dovoljno samo prijave dobaviti...
            List<(int prijavaID, int ELO)?> Prijave = new List<(int prijavaID, int ELO)?>();
            List<(int prijavaID, int ELO)> dobavljenjePrijave = NadjiELOPrijavama(takmicenje); // dobavlja elo i smjesta ga u tuple s prijavom
            foreach ((int pr, int el) i in dobavljenjePrijave)
                Prijave.Add(i);
            Prijave.Sort((y, x) => x.GetValueOrDefault().ELO.CompareTo(y.GetValueOrDefault().ELO)); // sortira silazno po elou 

            //provjeri da li su prijave paran broj,ako nisu paran broj treba dodati jednu vise koja je null i koja predstavlja bye
            // kad si dodao jednu uzmi drugu polovinu niza i reverse ga,i nalijepi na prvu polovinu pa za npr broj ljudi od 6 imat ces 1 2 3 6 5 4 npr tj ako je
            // 5 ljudi onda ce bit 1 2 3 null 5 4
            if (Prijave.Count() % 2 != 0)
                Prijave.Add(null);
            int pola = Prijave.Count() / 2;
            int duzinaOstatka = Prijave.Count() - pola;
            List<(int prijava, int elo)?> DrugaPolovina = Prijave.GetRange(pola, duzinaOstatka);
            DrugaPolovina.Reverse();//obrnut ce redoslijed
            Prijave = Prijave.GetRange(0, pola);
            Prijave = Prijave.Concat(DrugaPolovina).ToList();


            //nakon toga trebas za svaku rundu napravit parove,kako se to radi ? 
            // imas for petlju koja ide kroz svaku rundu pojedinacno 
            //unutra imas jos jednu for petlju koja krece od 0 i ide to n = brojtimova(ukljucujucibye) / 2 // u mom slucaju konkretno to su utakmice
            // za svaku utakmicu dobavi Igrace za odredjenu prijavu , dobavi ucesca na utakmici i rasporedi igrace na ucesca na utakmici
            for (int i = 0; i < Prijave.Count() - 1; i++)//broj rundi
            {
                //round robin takodjer posjeduje samo jedan bracket pa mogu pitati na ovaj nacin
                Runda rundica = takmicenje.Bracketi.FirstOrDefault().Runde.Where(x => x.BrojRunde == i + 1).FirstOrDefault();
                int brojacUtakmica = 0;
                foreach (Utakmica x in rundica.Utakmice)
                {
                    (int? tim1, int? tim2) Par = (Prijave[brojacUtakmica].GetValueOrDefault().prijavaID,
                        Prijave[Prijave.Count() - brojacUtakmica - 1].GetValueOrDefault().prijavaID);
                    brojacUtakmica++;
                    RasporediUcescaNaUtakmicu(x, Par, false);
                }
                //uradi shuffle prijava odnosno pomjeri svaku prijavu ne dirajuci prvu za 
                //jedno mjesto udesno,znaci zadnja postaje druga,predzadnja zadnja 		//itd..	
                (int prijava, int elo)? temp = Prijave[1];
                for (int j = 1; j < Prijave.Count() - 1; j++)
                {
                    (int prijava, int elo)? pr = Prijave[j + 1];
                    Prijave[j + 1] = temp;
                    temp = pr;
                }
                Prijave[1] = temp;

            }
        }
        public void KreirajUtakmicu(int rundaID, ref int brojUtakmice, Vrsta_Takmicenja vrstaTakmicenja)
        {
            Utakmica novaUtakmica = new Utakmica
            {
                BrojUtakmice = brojUtakmice,
                RundaID = rundaID,
                UcescaNaUtakmici = new List<Igrac_Utakmica>(),
                StatusID = 1,
                TipUtakmice = db.TipoviUtakmica.Where(x => x.Naziv == vrstaTakmicenja.Naziv).FirstOrDefault(),
                IsEvidentirana = false
            };
            db.Utakmice.Add(novaUtakmica);
            db.SaveChanges();
            // sve na null osim BrojaUtakmice i pripadajuce runde


            int brojZapisa = 2; // jedan za home jedan za away
            if (vrstaTakmicenja.Naziv == "Double") // onda dva za home dva za away. i ovo rijesiti enumeracijom
                brojZapisa = 4;
            for (int i = 0; i < brojZapisa; i++)
            {
                Igrac_Utakmica novoUcesce = new Igrac_Utakmica
                {
                    Utakmica = novaUtakmica
                };
                db.IgraciUtakmice.Add(novoUcesce);
                db.SaveChanges();
                //new IgracUtakmica - > sve na null osim utakmicaID// ne moze ovo ovako,sta ako su doublovi-- rijeseno;
            }
            brojUtakmice++;
        }

        public List<Igrac> nadjiIgraceUPrijavama(int? prijava1, int? prijava2)
        {
            List<Igrac> ListaIgraca = new List<Igrac>();
            if (prijava1 != null)
            {
                List<Prijava_igrac> listaPrijavaIgrac = db.PrijaveIgraci.Where(x => x.PrijavaID == prijava1.GetValueOrDefault()).ToList();
                foreach (Prijava_igrac x in listaPrijavaIgrac)
                {
                    ListaIgraca.Add(db.Igraci.Find(x.IgracID));//tako nekako 
                }
            }
            if (prijava2 != null)
            {
                List<Prijava_igrac> listaPrijavaIgrac = db.PrijaveIgraci.Where(x => x.PrijavaID == prijava2.GetValueOrDefault()).ToList();
                foreach (Prijava_igrac x in listaPrijavaIgrac)
                {
                    ListaIgraca.Add(db.Igraci.Find(x.IgracID));//tako nekako 
                }
            }
            //ovdje bi lista igraca trebala imati count ili 2 ili 4;
            return ListaIgraca;

        }
        public List<(int? prijavaA, int? prijavaB)> GetParove(List<(int prijava, int elo)> ProslijedjenePrijave)
        //trebalo bi naglasit da je ovo za singleelim
        // i ovo proslijedjene prijave je vec sortirano silazno
        {
            List<(int? prijavaA, int? prijavaB)> Parovi = new List<(int? prijavaA, int? prijavaB)>();
            List<(int prijava, int elo)?> novaListaPrijava = new List<(int prijava, int elo)?>();
            if (!PotencijaDvice(ProslijedjenePrijave.Count()))
            {
                /*
                 * Konkretno ovdje kopiram postojece prijave u novu listu prijava koja moze sadrzavati nullove,
                 * i onda dodajem onoliko nullova kolika je razlika izmedju iducePotencije i trenutnog broja prijava
                 * ovi nullovi su ustvari BYEOVi odnosno mjesta kad igrac proalzi u drugu rundu
                 */
                int IducaPotencija = NadjiIducuVecuPotenciju(ProslijedjenePrijave.Count());
                for (int i = 0; i < IducaPotencija; i++)
                    if (i >= ProslijedjenePrijave.Count())
                        novaListaPrijava.Add(null);
                    else
                        novaListaPrijava.Add(ProslijedjenePrijave[i]);
            }
            else
            {
                for (int i = 0; i < ProslijedjenePrijave.Count(); i++)
                    novaListaPrijava.Add(ProslijedjenePrijave[i]);
            }

            //e sad treba samo uradit shuffle parova tako da je par 1,16 prvi a npr 2,15 zadnji..	
            //jer nema smisla da se u drugoj rundi nadju prvi najbolji i drugi najbolji
            //evo funkcije

            List<int?> prijaveSortirane = new List<int?>();
            foreach ((int prijava, int elo)? i in novaListaPrijava)
            {
                if (i != null)
                    prijaveSortirane.Add(i.GetValueOrDefault().prijava);
                else
                    prijaveSortirane.Add(null);
            }
            List<int?> bracket_list = prijaveSortirane; //upitno da li je potrebno,mogao sam odma gore rec list<int> bracket_list = novalista..
            int slice = 1;
            while (slice < bracket_list.Count() / 2)
            // ovo sve u sustini samo rasporedjuje parove na nacin da stavlja najjaceg sa najslabijim jer tako                                      
            //funkcionise takmicenje,naravno ja imam opciju ako nije seeded,random generisem seedove i rasporedim                                      
            // ljude,to je u drugim funkcijama rjeseno,sta je bio inace problem,ako bih isao ovom varijantom                                     
            //nadji elo rasporedi prvi zadnji,drugi predzadnji itd u drugoj rundi bi se sreli najjaci s drugim                                          
            // najjacim(po seedu odnosno elou) sto nije bas najefikasnije i najpozeljnije , ovo rjesava taj 							                                     
            //problem stavljajuci drugog najboljeg na zadnje mjesto i ostale parove takodjeru optimizovanom formatu shuffle-a
            //odnosno ovo steli da se 1 i 2 najbolji sretnu u finalu,tako takmicenja funkcionisu za single elim
            {
                var temp = bracket_list;
                bracket_list = new List<int?>();

                while (temp.Count() > 0)
                {
                    List<int?> prvislice = temp.GetRange(0, slice);
                    //temp = temp.Except(prvislice).ToList();
                    temp = temp.GetRange(slice, temp.Count() - slice);
                    List<int?> drugislice = temp.GetRange(temp.Count() - slice, slice);
                    //temp = temp.Except(drugislice).ToList();
                    temp = temp.GetRange(0, temp.Count() - slice);

                    bracket_list = bracket_list.Concat(prvislice).ToList();
                    bracket_list = bracket_list.Concat(drugislice).ToList();
                }
                slice *= 2;
            }
            for (int i = 0; i < bracket_list.Count(); i += 2)
            {
                Parovi.Add((bracket_list[i], bracket_list[i + 1]));
            }
            return Parovi;
        }

        public List<(int prijavaID, int ELO)> NadjiELOPrijavama(Takmicenje takmicenje)
        {
            List<(int prijava, int elo)> Lista = new List<(int prijava, int elo)>();
            if (takmicenje.Seeded)
            {
                for (int i = 0; i < takmicenje.Prijave.Count(); i++)
                {
                    int prijava = takmicenje.Prijave[i].ID;
                    if (takmicenje.Vrsta.Naziv == "Single")
                    {
                        int igracID = db.PrijaveIgraci.Where(x => x.PrijavaID == prijava).Select(c => c.IgracID).SingleOrDefault();
                        //ovo bi se moglo zamijeniti sa listom PrijavaIgraca unutar Prijava klase 
                        int ELO = db.Igraci.Find(igracID).ELO;
                        Lista.Add((prijava, ELO));
                    }
                    else
                    {
                        List<int> igraciID = db.PrijaveIgraci.Where(x => x.PrijavaID == prijava).Select(c => c.IgracID).ToList();
                        List<Igrac> Igraci = db.Igraci.Where(x => x.ID == igraciID[0] || x.ID == igraciID[1]).ToList();
                        int sumaElo = 0;
                        foreach (Igrac x in Igraci)
                        {
                            sumaElo += x.ELO;
                        }
                        sumaElo /= 2;//prosjek
                        Lista.Add((prijava, sumaElo));
                    }

                }
            }
            else //ako nije seeded  samo pokupi sve prijave i dodijeli random vrijednosti
            {
                int brojPrijava = takmicenje.Prijave.Count();
                List<int> ListaRandomELOa = GenerisiRandom(brojPrijava);
                int brojac = 0;
                foreach (Prijava x in takmicenje.Prijave)
                {
                    int prijavaID = x.ID;
                    int ELO = ListaRandomELOa[brojac++];
                    Lista.Add((prijavaID, ELO));
                }
            }
            return Lista;
        }
        public List<int> GenerisiRandom(int brojPrijava)
        {
            List<int> lista = new List<int>();
            Random randklasa = new Random();
            for (int i = 0; i < brojPrijava + 15; i++)
            {
                int broj = randklasa.Next(1, 100);//nece valjda nikad bit iznad 100 igraca // djeno prijedlog i-- implementirati
                if (!lista.Contains(broj))
                    lista.Add(broj);
            }
            return lista;
        }



        public (int BrojRundi, int brojByeova) pomocnaFunkcijaIzracunajRunde(Sistem_Takmicenja sistem, int brojIgraca)
        // razmisliti i o razbijanju ove funkcije u onu select funkciju kao IzracunajRunde pa switchcase na osnovu SistemaTakmicenja
        {
            int brojrundi = -1;
            int _byes = 0;
            if (sistem.Opis == "Single elimination")//mozda je pametnija varijanta preko enumeracije,za sad hardcoded
            {
                if (PotencijaDvice(brojIgraca))
                    //brojrundi = (int)Math.Log2(brojIgraca);
                    brojrundi = (int)Math.Log(brojIgraca,2.0);

                        else
                        {
                    int iducaPotencija = NadjiIducuVecuPotenciju(brojIgraca);

                    if (iducaPotencija != -1)//ne bi nikad teoretski trebalo vratit -1 
                    {
                        //brojrundi = (int)Math.Log2(iducaPotencija);
                        brojrundi = (int)Math.Log(iducaPotencija,2.0);
                        _byes = iducaPotencija - brojIgraca;
                    }
                }

            }
            else if (sistem.Opis == "Round robin")
            {
                if (brojIgraca % 2 == 0)
                {
                    brojrundi = brojIgraca - 1;

                }

                else
                {
                    brojrundi = brojIgraca;
                    _byes = 1;//svaki igrac nece igrati tacno po jednu rundu 
                }

            }
            return (BrojRundi: brojrundi, brojByeova: _byes);
        }
        public bool PotencijaDvice(int broj)
        {
            return (broj != 0) && ((broj & (broj - 1)) == 0);
        }

        int NadjiIducuVecuPotenciju(int BrojIgraca)
        {
            int vecapot = -1;
            for (int i = 0; i < 10; i++)
            {
                if ((int)Math.Pow(2, i) > BrojIgraca)
                {
                    return (int)Math.Pow(2, i);
                }
            }
            return vecapot;
        }
    }
}

