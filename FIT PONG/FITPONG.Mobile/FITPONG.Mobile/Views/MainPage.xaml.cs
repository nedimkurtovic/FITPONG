using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FIT_PONG.Mobile.Models;
using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using FIT_PONG.Mobile.ViewModels;

namespace FIT_PONG.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        BaseAPIService apiServis = new BaseAPIService("users");
        MainPageViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
            BindingContext = viewModel = new MainPageViewModel();
            _ = viewModel.notifikacijeService.ConnectAsync();
            viewModel.notifikacijeService.primiNotifikacije += NotifikacijeService_primiNotifikacije;

        }


        private void NotifikacijeService_primiNotifikacije(object sender, MessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.DisplayAlert("OBAVIJEST", e.Message, "OK");
            });
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Naslovnica:
                        MenuPages.Add(id, new NavigationPage(new Views.Naslovnica()));
                        break;
                    case (int)MenuItemType.Igrači:
                        MenuPages.Add(id, new NavigationPage(new Views.Users.UsersLista()));
                        break;
                    case (int)MenuItemType.Takmičenja:
                        MenuPages.Add(id, new NavigationPage(new Views.Takmicenja.TakmicenjaLista()));
                        break;
                    case (int)MenuItemType.Reports:
                        MenuPages.Add(id, new NavigationPage(new Views.Reports.ReportsDodaj()));
                        break;
                    case (int)MenuItemType.Profil:
                        {
                            var usr = BaseAPIService.User;
                            MenuPages.Add(id, new NavigationPage(new UsersMain(usr)));
                            break;
                        }
                    case (int)MenuItemType.Chat:
                        MenuPages.Add(id, new NavigationPage(new Views.Chat.ChatMain()));
                        break;
                }
            }

            var newPage = MenuPages[id];

                if (newPage != null && Detail != newPage)
                {
                    Detail = newPage;

                    if (Device.RuntimePlatform == Device.Android)
                        await Task.Delay(100);

                    IsPresented = false;
                }
        }
    }
}