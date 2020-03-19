using FIT_PONG.Models;
using FIT_PONG.Models.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FIT_PONG.Test
{
    [TestClass]
    public class ELOcalculator
    {
        [TestMethod]
        [DataRow(1000, 1000, 1, 1016)]
        [DataRow(1000, 1000, 0, 984)]
        [DataRow(500, 500, 1, 516)]
        [DataRow(500, 500, 0, 484)]
        [DataRow(500, 1000, 1, 530)]
        [DataRow(500, 1000, 0, 498)]
        public void VratiRezultateZaSingle(int igrac1, int igrac2, int score, int ocekivano)
        {
            ELOCalculator x = new ELOCalculator();
            int rezultat = x.VratiEloSingle(igrac1, igrac2, score);
            Assert.AreEqual(ocekivano, rezultat);
        }
        [TestMethod]
        [DataRow(500, 500, 500, 500, 1, 516)]
        [DataRow(1000, 1000, 1000, 1000, 1, 1016)]
        [DataRow(1000, 1000, 2000, 2000, 1, 1032)]
        [DataRow(1000, 1200, 500, 500, 1, 1001)]
        [DataRow(500, 1000, 500, 1000, 0, 484)]
        [DataRow(2000, 1000, 1000, 1000, 0, 1970)]
        public void VratiRezultateZaDouble(int pozivatelj, int igrac2, int igrac3, int igrac4, int score, int ocekivano)
        {
            ELOCalculator x = new ELOCalculator();
            int rezultat = x.VratiEloDouble(pozivatelj, igrac2, igrac3, igrac4, score);
            Assert.AreEqual(ocekivano, rezultat);
        }
    }
    [TestClass]
    public class InicijalizatorTakmicenja
    {
        [TestMethod]
        [DataRow("Single elimination", 7, 3)]
        [DataRow("Single elimination", 5, 3)]
        [DataRow("Single elimination", 10, 4)]
        [DataRow("Single elimination", 14, 4)]
        [DataRow("Single elimination", 18, 5)]
        [DataRow("Round robin", 5, 5)]
        [DataRow("Round robin", 8, 7)]
        [DataRow("Round robin", 13, 13)]
        [DataRow("Round robin", 4, 3)]
        [DataRow("Round robin", 9, 9)]

        public void Test_BrojRundiNaOsnovuSistemaIBrojaIgraca(string sistem, int brojIgraca, int ocekivano)
        {
            Sistem_Takmicenja d = new Sistem_Takmicenja { Opis = sistem };
            //obzirom da se ne koristi baza za ovu funckiju moze se slobodno poslati null vrijednost
            InitTakmicenja x = new InitTakmicenja(null);
            int rezultat = x.pomocnaFunkcijaIzracunajRunde(d, brojIgraca).BrojRundi;
            Assert.AreEqual(ocekivano, rezultat);
        }
        [TestMethod]
        [DataRow(4, true)]
        [DataRow(7, false)]
        [DataRow(8, true)]
        [DataRow(15, false)]
        [DataRow(305, false)]
        [DataRow(64, true)]
        [DataRow(32, true)]
        [DataRow(12, false)]
        public void Test_BrojPotencijaDvice(int broj, bool ocekivano)
        {
            InitTakmicenja x = new InitTakmicenja(null);

            bool rezultat = x.PotencijaDvice(broj);
            Assert.AreEqual(ocekivano, rezultat);
        }

    }

    [TestClass]
    public class EvidentorTestovi
    {
        [TestMethod]
        public void Test_IgracSadrzanUPrijavi_SaljeIgracaKaoNull()
        {
            Evidentor x = new Evidentor(null, null);
            List<Prijava_igrac> lista = new List<Prijava_igrac>() {
                new Prijava_igrac() { IgracID= 2},
                new Prijava_igrac() { IgracID= 3},
                new Prijava_igrac() { IgracID= 5},
                new Prijava_igrac() { IgracID= 6}
            };
            bool rezultat = x.PrijavaSadrziIgraca(null, lista);
            Assert.AreEqual(false, rezultat);
        }
        [TestMethod]
        [DataRow(3, true)]
        [DataRow(5, true)]
        [DataRow(6, true)]
        [DataRow(9, false)]
        [DataRow(200, false)]
        [DataRow(11, false)]
        [DataRow(13, false)]
        public void Test_IgracSadrzanUPrijavi_SaljeIgraca(int igracId, bool ocekivano)
        {
            Evidentor x = new Evidentor(null, null);
            List<Prijava_igrac> lista = new List<Prijava_igrac>() {
                new Prijava_igrac() { IgracID= 2},
                new Prijava_igrac() { IgracID= 3},
                new Prijava_igrac() { IgracID= 5},
                new Prijava_igrac() { IgracID= 6}
            };
            bool rezultat = x.PrijavaSadrziIgraca(igracId, lista);
            Assert.AreEqual(ocekivano, rezultat);
        }

        [TestMethod]
        [DataRow(5, true)]
        [DataRow(4, true)]
        [DataRow(1, true)]
        [DataRow(0, true)]
        [DataRow(-2, false)]
        [DataRow(290, false)]
        [DataRow(50, false)]
        [DataRow(30, false)]
        [DataRow(24, false)]
        [DataRow(int.MinValue, false)]
        [DataRow(int.MaxValue, false)]
        public void Test_ValidanRezultat(int broj, bool ocekivano)
        {
            Evidentor x = new Evidentor(null, null);
            bool rezultat = x.ValidanRezultat(broj);
            Assert.AreEqual(ocekivano, rezultat);
        }
    }
}