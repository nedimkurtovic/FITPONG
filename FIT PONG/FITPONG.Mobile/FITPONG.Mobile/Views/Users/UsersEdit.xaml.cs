using FIT_PONG.Mobile.Helpers;
using FIT_PONG.Mobile.ViewModels.Users;
using FIT_PONG.SharedModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersEdit : ContentPage
    {
        UsersEditViewModel viewModel;
        public UsersEdit(SharedModels.Users user)
        {
            BindingContext = viewModel = new UsersEditViewModel(user);
            InitializeComponent();
            jacarukaPicker.SelectedItem = user.JacaRuka;
        }

        private async void btnEditSliku_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Greska", "Nije moguće odabrati sliku na ovom uređaju", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            if (file == null)
                return;

            var nizBajtova = Helperi.ReadToEnd(file.GetStream());
            var naziv = file.Path.Substring(file.Path.LastIndexOf("\\") + 1);
            Fajl slikaProfila = new Fajl
            {
                Naziv = naziv,
                BinarniZapis = nizBajtova
            };

            viewModel.SlikaProfila = slikaProfila;

            var rezultat = await viewModel.EditujSliku();

            if (rezultat != default(SharedModels.Users))
                RefreshDetalje(rezultat);
                    
        }

        private void gradPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var grad = piker.ItemsSource[piker.SelectedIndex] as Gradovi;
                viewModel.GradId = grad.ID;
            }
            else
                viewModel.GradId = -1;

        }

        private void jacarukaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var jacaruka = piker.ItemsSource[piker.SelectedIndex] as string;
                viewModel.JacaRuka = jacaruka;
            }
            else
                viewModel.JacaRuka = null;
        }

        private async void btnSpasi_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.SpasiIzmjene();

            if (rezultat != default(SharedModels.Users))
                RefreshDetalje(rezultat);

        }

        private async void btnResetSliku_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.ResetSliku();

            if (rezultat != default(SharedModels.Users))
                RefreshDetalje(rezultat);
        }

        private void RefreshDetalje(SharedModels.Users rezultat)
        {
            var mainStranica = Navigation.NavigationStack[0];
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
            //kad naleti na prvu main stavlja na true i sve ostale stranice brise poslije
            var a = Navigation.NavigationStack[0].BindingContext as UsersListaViewModel;
            if (a != null && a.DobaviUsere.CanExecute(null))
                a.DobaviUsere.Execute(null);
            foreach (var i in listaBrisanja)
                Navigation.RemovePage(i);
        }
    }
}