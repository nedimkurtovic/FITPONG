using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FIT_PONG.Mobile.Models;
using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;

namespace FIT_PONG.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
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