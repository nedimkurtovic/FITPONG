using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class DefaultSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
/*Gradovi*/
SET IDENTITY_INSERT[dbo].[Gradovi] ON
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(1, N'Banovići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(2, N'Banja Luka')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(3, N'Berkovići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(4, N'Bihać')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(5, N'Bijeljina')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(6, N'Bileća')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(7, N'Blagaj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(8, N'Bosanska Kostajnica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(9, N'Bosanska Krupa')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(10, N'Bosanska Otoka')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(11, N'Bosanski Brod')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(12, N'Bosanski Petrovac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(13, N'Bosansko Grahovo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(14, N'Bratunac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(15, N'Brčko')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(16, N'Breza')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(17, N'Bugojno')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(18, N'Busovača')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(19, N'Bužim')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(20, N'Cazin')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(21, N'Čajniče')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(22, N'Čapljina')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(23, N'Čelić')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(24, N'Čelinac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(25, N'Čitluk')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(26, N'Derventa')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(27, N'Doboj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(28, N'Doboj Istok')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(29, N'Doboj Jug')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(30, N'Dobretići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(31, N'Domaljevac // Šamac(Bosanski Šamac)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(32, N'Donji Vakuf')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(33, N'Drvar')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(34, N'Foča')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(35, N'Foča-Ustikolina')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(36, N'Fojnica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(37, N'Gacko')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(38, N'Glamoč')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(39, N'Glavatičevo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(40, N'Goražde')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(41, N'Gornji Vakuf / Uskoplje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(42, N'Gračanica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(43, N'Gradačac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(44, N'Gradiška(Bosanska Gradiška)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(45, N'Grude')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(46, N'Hadžići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(47, N'Han Pijesak')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(48, N'Ilidža')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(49, N'Ilijaš')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(50, N'Istočno Sarajevo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(51, N'Jablanica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(52, N'Jajce')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(53, N'Jelah')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(54, N'Kakanj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(55, N'Kalesija')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(56, N'Kalinovik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(57, N'Kiseljak')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(58, N'Kladanj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(59, N'Ključ')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(60, N'Kneževo(Skender Vakuf)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(61, N'Konjic')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(62, N'Kotor Varoš')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(63, N'Kozarska Dubica(Bosanska Dubica)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(64, N'Kraljeva Sutjeska')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(65, N'Kreševo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(66, N'Krupa na Uni')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(67, N'Kulen Vakuf')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(68, N'Kupres')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(69, N'Laktaši')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(70, N'Livno')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(71, N'Lopare')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(72, N'Lukavac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(73, N'Ljubinje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(74, N'Ljubuški')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(75, N'Maglaj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(76, N'Međugorje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(77, N'Milići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(78, N'Modriča')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(79, N'Mostar')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(80, N'Mrkonjić Grad')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(81, N'Neum')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(82, N'Nevesinje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(83, N'Novi grad(Bosanski Novi)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(84, N'Novi Travnik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(85, N'Odžak')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(86, N'Olovo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(87, N'Orašje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(88, N'Osmaci')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(89, N'Pale')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(90, N'Petrovo(Bosansko Petrovo Selo)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(91, N'Počitelj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(92, N'Posušje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(93, N'Pale-Prača')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(94, N'Prijedor')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(95, N'Prnjavor')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(96, N'Prozor – Rama')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(97, N'Prusac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(98, N'Ravno')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(99, N'Rogatica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(100, N'Rudo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(101, N'Sanski Most')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(102, N'Sapna')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(103, N'Sarajevo(glavni grad)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(104, N'Sokolac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(105, N'Srbac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(106, N'Srebrenica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(107, N'Srebrenik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(108, N'Stolac')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(109, N'Šamac / Domaljevac(Bosanski Šamac)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(110, N'Šekovići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(111, N'Šipovo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(112, N'Široki Brijeg')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(113, N'Teočak')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(114, N'Teslić')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(115, N'Tešanj')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(116, N'Tomislavgrad(Duvno)')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(117, N'Travnik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(118, N'Trebinje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(119, N'Trnovo')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(120, N'Turbe')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(121, N'Tuzla')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(122, N'Ugljevik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(123, N'Usora')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(124, N'Ustiprača')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(125, N'Vareš')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(126, N'Velika Kladuša')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(127, N'Visoko')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(128, N'Višegrad')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(129, N'Vitez')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(130, N'Vlasenica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(131, N'Vogošća')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(132, N'Vukosavlje')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(133, N'Zavidovići')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(134, N'Zenica')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(135, N'Zvornik')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(136, N'Žepa')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(137, N'Žepče')
INSERT[dbo].[Gradovi]([ID],[Naziv]) VALUES(138, N'Živinice')

SET IDENTITY_INSERT[dbo].[Gradovi] OFF

/*Kategorije*/

SET IDENTITY_INSERT[dbo].[Kategorije] ON

INSERT[dbo].[Kategorije] ([ID], [Opis]) VALUES(1, N'Muškarci')
INSERT[dbo].[Kategorije]([ID], [Opis]) VALUES(2, N'Žene')
INSERT[dbo].[Kategorije]([ID], [Opis]) VALUES(3, N'Mix')
SET IDENTITY_INSERT[dbo].[Kategorije] OFF

/*SistemiTakmicenja*/

SET IDENTITY_INSERT[dbo].[SistemiTakmicenja] ON

INSERT[dbo].[SistemiTakmicenja] ([ID], [Opis]) VALUES(12, N'Single elimination')
INSERT[dbo].[SistemiTakmicenja]([ID], [Opis]) VALUES(14, N'Round robin')
SET IDENTITY_INSERT[dbo].[SistemiTakmicenja] OFF

/*StatusiTakmicenja*/

SET IDENTITY_INSERT[dbo].[StatusiTakmicenja] ON

INSERT[dbo].[StatusiTakmicenja]([ID], [Opis]) VALUES(1, N'Prijave u toku')
INSERT[dbo].[StatusiTakmicenja]([ID], [Opis]) VALUES(2, N'U toku')
INSERT[dbo].[StatusiTakmicenja]([ID], [Opis]) VALUES(3, N'Završeno')
INSERT[dbo].[StatusiTakmicenja] ([ID], [Opis]) VALUES(4, N'Kreirano')
SET IDENTITY_INSERT[dbo].[StatusiTakmicenja] OFF

/*VrsteTakmicenja*/
SET IDENTITY_INSERT[dbo].[VrsteTakmicenja] ON

INSERT[dbo].[VrsteTakmicenja] ([ID], [Naziv]) VALUES(1, N'Single')
INSERT[dbo].[VrsteTakmicenja]([ID], [Naziv]) VALUES(2, N'Double')
SET IDENTITY_INSERT[dbo].[VrsteTakmicenja] OFF

/*TipoviRezultata*/

SET IDENTITY_INSERT[dbo].[TipoviRezultata] ON

INSERT[dbo].[TipoviRezultata] ([ID], [Naziv]) VALUES(1, N'Pobjeda')
INSERT[dbo].[TipoviRezultata]([ID], [Naziv]) VALUES(2, N'Poraz')
SET IDENTITY_INSERT[dbo].[TipoviRezultata] OFF


/*StatusiUtakmice*/
SET IDENTITY_INSERT[dbo].[StatusiUtakmice] ON

INSERT[dbo].[StatusiUtakmice] ([ID], [Opis]) VALUES(1, N'Kreirana')
INSERT[dbo].[StatusiUtakmice] ([ID], [Opis]) VALUES(2, N'Završena')
SET IDENTITY_INSERT[dbo].[StatusiUtakmice] OFF

/*TipoviUtakmica*/

SET IDENTITY_INSERT[dbo].[TipoviUtakmica] ON

INSERT[dbo].[TipoviUtakmica] ([ID], [Naziv]) VALUES(1, N'Single')
INSERT[dbo].[TipoviUtakmica]([ID], [Naziv]) VALUES(2, N'Double')
SET IDENTITY_INSERT[dbo].[TipoviUtakmica] OFF

/*VrsteSuspenzije*/
SET IDENTITY_INSERT[dbo].[VrsteSuspenzije] ON
INSERT[dbo].[VrsteSuspenzije] ([ID], [Opis]) VALUES(1, N'Login')
INSERT[dbo].[VrsteSuspenzije]([ID], [Opis]) VALUES(2, N'Kreiranje takmičenja')
INSERT[dbo].[VrsteSuspenzije]([ID], [Opis]) VALUES(3, N'Prijava na takmičenja')
SET IDENTITY_INSERT[dbo].[VrsteSuspenzije] OFF");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM [dbo].[Gradovi]
DELETE FROM [dbo].[VrsteSuspenzije]
DELETE FROM [dbo].[TipoviUtakmica]
DELETE FROM [dbo].[StatusiUtakmice]
DELETE FROM [dbo].[TipoviRezultata]
DELETE FROM [dbo].[VrsteTakmicenja]
DELETE FROM [dbo].[StatusiTakmicenja]
DELETE FROM [dbo].[SistemiTakmicenja]
DELETE FROM [dbo].[Kategorije]
");
        }
    }
}
