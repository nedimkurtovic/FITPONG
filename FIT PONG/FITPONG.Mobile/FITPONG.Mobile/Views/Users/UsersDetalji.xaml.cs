using FIT_PONG.Mobile.ViewModels.Users;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class UsersDetalji : ContentPage
    {
        UsersDetaljiViewModel viewModel;
        public UsersDetalji(UsersDetaljiViewModel model)
        {
            InitializeComponent();
            BindingContext = viewModel = model;
            if (viewModel.GetPreporuceneCommand.CanExecute(null))
                viewModel.GetPreporuceneCommand.Execute(null);
            btnEdit.IsVisible = viewModel.Vlasnik();

        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsersEdit(viewModel.User));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var steklejaut = (BindableObject)sender;
            var usr = (SharedModels.Users)steklejaut.BindingContext;
            await Navigation.PushAsync(new UsersDetalji(new UsersDetaljiViewModel(usr)));
        }

        private void btnPostovanje_Clicked(object sender, EventArgs e)
        {

        }
    }
}