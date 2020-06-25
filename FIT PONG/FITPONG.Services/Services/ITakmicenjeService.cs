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
		SharedModels.Takmicenja Add(SharedModels.Requests.Takmicenja.TakmicenjaInsert obj, string KreatorUsername);
		SharedModels.Takmicenja Update(int id, SharedModels.Requests.Takmicenja.TakmicenjaUpdate obj);//autorizovat
		SharedModels.Takmicenja Delete(int id);//autorizovat
		SharedModels.Takmicenja Initialize(int id);//autorizovat //ili da vraca mdoel takmicejna ili string uspjesan init
		//modelzaraspored GetRaspored(int id);
		//modelzaevidencije GetEvidencije(int userid, int takmid);//do userid je potrebno doc preko logovane sesije ili nekog requesta ili auth podataka
		//mogao bi ovaj evidentiraj mec da vraca konkretan mec sto se evidentirao
		//? EvidentirajMec(int takmid, SharedModels.Requests.Takmicenja.EvidencijaMeca obj);
		//modelzatabelu GetTabela(int takmid);

	}
}
