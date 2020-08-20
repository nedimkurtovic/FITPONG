using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Mobile.ViewModels
{
    public class MainPageViewModel:BaseViewModel
    {
        public NotifikacijeService notifikacijeService { get; set; }
        public MainPageViewModel()
        {
            notifikacijeService = new NotifikacijeService();
            notifikacijeService.Init();
        }
    }
}
