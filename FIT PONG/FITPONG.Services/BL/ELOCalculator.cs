using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services.BL
{
    public class ELOCalculator
    {
        //K predstavlja faktor tezine promjene, koristit ce se standardni 32, u narednim implementacijama ove aplikacije moguce je
        //doraditi da bude dinamican tj da se za igrace sa vecim ELO-om smanjuje, mada to onda postaje nefer prema ljudima s vecim
        //ELO-om jer im je isplativije ne igrati, puno vise gube nego sto mogu dobiti,
        //sto je problem samog dizajna ELO sistema a ne nase implementacije istog
        private static int K = 32;
        public int VratiEloSingle(int PozivateljELO, int SuparnikELO, int Score)
        {
            double R1 = Math.Pow(10, ((double)PozivateljELO / (double)400));
            double R2 = Math.Pow(10, ((double)SuparnikELO / (double)400));
            double E = R1 / (R1 + R2);
            int noviElo =(int) Math.Round(PozivateljELO + K * (Score - E));
            return noviElo;
        }
        public int VratiEloDouble(int PozivateljELO, int KolegaELO, int Suparnik1ELO, int Suparnik2ELO, int Score)
        {
            int prosjekPozivatelj = (int) Math.Round(Prosjek(PozivateljELO, KolegaELO)); 
            int prosjekSuparnik = (int) Math.Round(Prosjek(Suparnik1ELO, Suparnik2ELO));
            double R1 = Math.Pow(10, ((double)prosjekPozivatelj / (double)400));
            double R2 = Math.Pow(10, ((double)prosjekSuparnik / (double)400));
            double E = R1 / (R1 + R2);
            int noviElo = (int)Math.Round(PozivateljELO + K * (Score - E));
            int razlika = noviElo - PozivateljELO;
            return PozivateljELO + razlika;
            //nisam siguran jos kako cemo pozivati konkretno funkciju, kako sam je trenutno implementirao apsolutno za svaki zapis
            //iz igrac_utakmica tabele ce se posebno morati pozvati ova funkcija, nije problem ni napraviti neku tuple varijantu koja 
            //vracala rezultat za pozivatelja i kolegu, ili za pozivatelja,kolegu,suparnika1,suparnika2 ili ako bi gledali single
            //varijantu(funkcija iznad) mogla bi postojati varijanta koja vraca za pozivatelja i suparnika, opet naglasavam
            //nismo implementirali funkciju evidencija rezultata iz koje ce se ovo ionako pozivati, tako da podlozno je promjenama
           
        }
        public double GetVjerovatnoca(int timA, int timB)
        {
            if (timA == 0)
                return 0;
            if (timB == 0)
                return 1;

            double R1 = Math.Pow(10, ((double)timA / (double)400));
            double R2 = Math.Pow(10, ((double)timB / (double)400));
            double E = R1 / (R1 + R2);
            return E;
        }
        public double Prosjek(int a, int b)
        {
            //lafo osigurava da ne bude overflow, mada izgleda da ovo nije potpuno rjesenje
            return (a / 2) + (b / 2) + ((a % 2 + b % 2) / 2);
        }
    }
}
