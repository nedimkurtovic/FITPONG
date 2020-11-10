using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FIT_PONG.Mobile.Services;
using FIT_PONG.Mobile.Views;

namespace FIT_PONG.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new Mobile.Views.Users.Login());
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
