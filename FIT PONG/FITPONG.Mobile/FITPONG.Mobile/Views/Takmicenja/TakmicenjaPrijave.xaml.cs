using FIT_PONG.Mobile.ViewModels.Takmicenja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Takmicenja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakmicenjaPrijave : ContentPage
    {
        TakmicenjaPrijaveViewModel viewModel;
        public TakmicenjaPrijave(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaPrijaveViewModel(_takmicenje);
            lblEmpty.IsVisible = viewModel.ListaPrijava.Count == 0;
        }

        private async void btnBlokiraj_Clicked(object sender, EventArgs e)
        {
            var x = ((Button)sender).BindingContext;

            await viewModel.BlokirajPrijavu(int.Parse(x.ToString()));
        }
    }
}