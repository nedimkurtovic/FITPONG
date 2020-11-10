using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Naslovnica, Title="Naslovnica" },
                new HomeMenuItem {Id = MenuItemType.Profil, Title="Profil" },
                new HomeMenuItem {Id = MenuItemType.Igrači, Title="Igrači" },
                new HomeMenuItem {Id = MenuItemType.Takmičenja, Title="Takmičenja" },
                new HomeMenuItem {Id = MenuItemType.Chat, Title="Chat" },
                new HomeMenuItem {Id = MenuItemType.Reports, Title="Prijavi grešku" },
                new HomeMenuItem {Id = MenuItemType.Logout, Title="Logout" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
          
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
                
                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                if (id == (int)MenuItemType.Logout)
                {
                    BaseAPIService.Password = "";
                    BaseAPIService.Username = "";
                    BaseAPIService.ID = default;
                    BaseAPIService.User = default;
                    RootPage.UgasiNotifikacije();
                    Application.Current.MainPage = new NavigationPage(new Mobile.Views.Users.Login());
                }
                else
                {
                    await RootPage.NavigateFromMenu(id);
                }
            };


        }
    }
}