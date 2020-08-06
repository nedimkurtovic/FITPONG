using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja.Objave
{
    public class TakmicenjaObjaveDetaljiViewModel:BaseViewModel
    {
        public TakmicenjaObjaveDetaljiViewModel(SharedModels.Objave _objava , SharedModels.Takmicenja _takmicenje)
        {
            Objava = _objava;
            Takmicenje = _takmicenje;
            objaveAPIService = new BaseAPIService("objave");
        }

        public bool Vlasnik()
        {
            return BaseAPIService.ID == Takmicenje.KreatorID;
        }
        public BaseAPIService objaveAPIService { get; set; }
        public SharedModels.Objave Objava { get; set; }
        public SharedModels.Takmicenja Takmicenje { get; set; }

        public async Task<SharedModels.Objave> ObrisiObjavu()
        {
            var rezultat = await objaveAPIService.Delete<SharedModels.Objave>(Objava.ID);
            return rezultat;
        }
    }
}
