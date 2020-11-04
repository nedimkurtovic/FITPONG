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
        readonly UsersDetaljiViewModel viewModel;
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

        private async void btnPostovanje_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.Postovanje();

            if (rezultat != default(SharedModels.Users))
            {
                Page mainStranica = null;
                int pozicijaListe = -1;
                for (int i = 0; i < Navigation.NavigationStack.Count; i++)
                {
                    if (Navigation.NavigationStack[i] is UsersLista)
                        pozicijaListe = i;
                    if (Navigation.NavigationStack[i] is UsersMain)
                    {
                        mainStranica = Navigation.NavigationStack[i];
                        break;
                    }
                }
                if (mainStranica != null)
                {
                    Navigation.InsertPageBefore(new UsersMain(rezultat), mainStranica);
                    bool brisi = false;

                    List<Page> listaBrisanja = new List<Page>();
                    foreach (var i in Navigation.NavigationStack)
                    {
                        if (brisi)
                            listaBrisanja.Add(i);
                        if (i is UsersMain && !brisi)
                            brisi = true;
                    }
                    UpdateListu(pozicijaListe);
                    foreach (var i in listaBrisanja)
                        Navigation.RemovePage(i);
                }
                else
                {
                    await Navigation.PushAsync(new UsersMain(rezultat));
                    UpdateListu(pozicijaListe);
                }
            }
        }
        private void UpdateListu(int pozicija)
        {
            if (pozicija == -1)
                return;
            var a = Navigation.NavigationStack[pozicija].BindingContext as UsersListaViewModel;
            if (a != null && a.DobaviUsere.CanExecute(null))
                a.DobaviUsere.Execute(null);
        }
    }
}