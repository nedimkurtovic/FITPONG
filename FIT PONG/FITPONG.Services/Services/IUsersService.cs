using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Services
{
	//Treba promisliti o return tipovima ja ovo nisam kompletno uradio, gledaj vracat sta mislis da ce olaksat
    public interface IUsersService
    {
		List<SharedModels.Users> Get(SharedModels.Requests.Account.AccountSearchRequest obj);
		SharedModels.Users Get(int ID);
		Task<SharedModels.Users> Register(SharedModels.Requests.Account.AccountInsert obj);
		Task<SharedModels.Users> Login(SharedModels.Requests.Account.Login obj); //Umjesto profile pic path treba Slika 
		// ((pitanje za vjezbe kako osigurati da ne mozes pozvati logout za drugog korisnika) //autorizovat
		//naredne zabiljeske se odnose na Logout metodu ali i sve metode gdje je potreban userID
		//drugo pitanje je sta je pametnije odabrati, username ili id? Ja bih isao ovom logikom : Obzirom da ce se 
		//pri svakom requestu slati username i pw koje btw treba dekodirati jer su u base64 ja mislim
		//vidi ovo (https://github.com/amelmusic/RS2-2020/blob/master/eProdaja/eProdaja/Security/BasicAuthenticationHandler.cs)
		//ovo ti se desi prije nego sto dodje ja mislim u prvu liniju koda na akciji u kontroleru, 
		//sto znaci da moramo "ponoviti" isti proces ili u akciji ili u servisu da bismo dosli do usernamea, 
		//mozda je pametnije ovo : ovaj dio koda koji se nalazi u njega u try bloku, 
		//da izmjestimo u jednu funkcijicu malu ovdje u servisu i onda kad god nam treba ID od usera,
		//proslijedimo Request.Headers["Authorization"]
		//toj nasoj metodi u servisu i ona nam vrati id ako postoji, ako ne postoji baca exception il nesto
		//moguce da je logout bespotreban i visak obzirom da se pri svakom requestu salje username i pw
		//ovo treba odluciti---> //stavio sam string kao poruka : "Korisnik odjavljen" npr i to 
		
		//string Logout(int id || string username);
		
		SharedModels.Users EditujProfil(int id, SharedModels.Requests.Account.AccountUpdate obj);
		Task<string> SendConfirmationEmail(SharedModels.Requests.Account.Email_Password_Request obj);
		Task<string> ConfirmEmail(string userId, string token);
		Task<string> SendPasswordChange(SharedModels.Requests.Account.Email_Password_Request obj);
		Task<string> ConfirmPasswordChange(string loggedInUserName, PasswordPromjena obj);
		string ResetProfilePicture(string loggedInUserName, int id);//autorizovat
		string UpdateProfilePicture(string loggedInUserName, int id, Fajl Slika);//klasa slika umjesto niz byteova //autorizovat
		string Postovanje(string loggedInUserName, int postovaniID);
		string GetRequestUserName(HttpRequest Request);
		int GetRequestUserID(HttpRequest Request);
		SharedModels.Users Suspenduj(int userId, SuspenzijaRequest obj);
		Task<SharedModels.Users> CheckValidanLogin(Login obj);

	}
}
