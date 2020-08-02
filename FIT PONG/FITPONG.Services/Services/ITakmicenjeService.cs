using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public interface ITakmicenjeService
    {
		List<SharedModels.Takmicenja> Get(SharedModels.Requests.Takmicenja.TakmicenjeSearch obj);
		SharedModels.Takmicenja GetByID(int id);
		//moguce je i da ce se id kreatora slati a ne username
		SharedModels.Takmicenja Add(SharedModels.Requests.Takmicenja.TakmicenjaInsert obj, int KreatorID);
		SharedModels.Takmicenja Update(int id, SharedModels.Requests.Takmicenja.TakmicenjaUpdate obj);//autorizovat
		SharedModels.Takmicenja Delete(int id);//autorizovat
		SharedModels.Takmicenja Initialize(int id);//autorizovat //ili da vraca mdoel takmicejna ili string uspjesan init
		List<RasporedStavka> GetRaspored(int id); 
		List<EvidencijaMeca> GetEvidencije(string KorisnikUsername, int takmid);//do userid je potrebno doc preko logovane sesije ili nekog requesta ili auth podataka
		//mogao bi ovaj evidentiraj mec da vraca konkretan mec sto se evidentirao
		EvidencijaMeca EvidentirajMec(int takmid, EvidencijaMeca obj);//ovo treba autorizovati ali mislim da 
		//bi dobro bilo izdvojiti tu autorizaciju u authorizeAttribute klasu neku custom i stavit je iznad akcije
		List<TabelaStavka> GetTabela(int takmid);
		EvidencijaMeca GetIgraceZaEvidenciju(EvidencijaMeca obj, int takmid);

	
	}
}
