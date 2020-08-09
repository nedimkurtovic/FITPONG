using FIT_PONG.Mobile.ViewModels.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersLista : ContentPage
    {
        UsersListaViewModel usersListaViewModel;
        public UsersLista()
        {
            InitializeComponent();
            BindingContext = usersListaViewModel = new UsersListaViewModel();
        }
        
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var gridlejaut = (BindableObject)sender;
            var _user = (SharedModels.Users)gridlejaut.BindingContext;
            await Navigation.PushAsync(new UsersMain(_user));
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}