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
        NaslovnicaViewModel viewModel;
        public Naslovnica()
        {
            InitializeComponent();
            BindingContext = viewModel = new NaslovnicaViewModel();
            viewModel.lampicaServis.ConnectAsync();
            

            viewModel.lampicaServis.StigloTrenutnoStanje += LampicaServis_StigloTrenutnoStanje;
            viewModel.lampicaServis.PromijenjenaSlika += LampicaServis_PromijenjenaSlika;
            viewModel.lampicaServis.PromijenjenaPoruka += LampicaServis_PromijenjenaPoruka;

            //Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
            //{
            //    KaruselObavijesti.Position = (KaruselObavijesti.Position + 1) % viewModel.ListaPoruka.Count;
            //    return true;
            //}));
        }
        protected async override void OnAppearing()
        {
            await viewModel.lampicaServis.GetTrenutnuSlikuAsync();
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
                viewModel.lampicaServis.TrenutnaSlika = e.Message;
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
                viewModel.lampicaServis.TrenutnaSlika = e.Message;
            });
        }
    }
}