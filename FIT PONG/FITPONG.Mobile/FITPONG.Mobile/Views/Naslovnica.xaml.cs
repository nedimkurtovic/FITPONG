using FIT_PONG.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Naslovnica : ContentPage
    {
        readonly NaslovnicaViewModel viewModel;
        public Naslovnica()
        {
            InitializeComponent();
            BindingContext = viewModel = new NaslovnicaViewModel();
            _ = viewModel.LampicaServis.ConnectAsync();
            

            viewModel.LampicaServis.StigloTrenutnoStanje += LampicaServis_StigloTrenutnoStanje;
            viewModel.LampicaServis.PromijenjenaSlika += LampicaServis_PromijenjenaSlika;
            viewModel.LampicaServis.PromijenjenaPoruka += LampicaServis_PromijenjenaPoruka;
            
        }
        protected async override void OnAppearing()
        {
            await viewModel.LampicaServis.GetTrenutnuSlikuAsync();
            base.OnAppearing();
        }
        private void LampicaServis_PromijenjenaPoruka(object sender, APIServices.MessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblPoruka.Text = e.Message;
            });
        }

        private void LampicaServis_PromijenjenaSlika(object sender, APIServices.MessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var sours = e.Message;
                if (DeviceInfo.Platform == DevicePlatform.UWP)
                    sours = "Assets/" + e.Message;
                SlikaLampica.Source = sours;
                viewModel.LampicaServis.TrenutnaSlika = e.Message;
            });
        }

        private void LampicaServis_StigloTrenutnoStanje(object sender, APIServices.MessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var sours = e.Message;
                if (DeviceInfo.Platform == DevicePlatform.UWP)
                    sours = "Assets/" + e.Message;
                SlikaLampica.Source = sours;
                viewModel.LampicaServis.TrenutnaSlika = e.Message;
            });
        }

    }
}