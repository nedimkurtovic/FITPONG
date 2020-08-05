using FIT_PONG.Mobile.ViewModels.Takmicenja;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    public partial class TakmicenjaEvidencije : ContentPage
    {
        TakmicenjaEvidencijeViewModel viewModel;
        public TakmicenjaEvidencije(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaEvidencijeViewModel(_takmicenje);
            if (viewModel.GetEvidencije.CanExecute(null))
                viewModel.GetEvidencije.Execute(null);
            lblNemaDostupnih.IsVisible = viewModel.ListaEvidencija.Count == 0;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var grid = (BindableObject)sender;
            var evidencija = (SharedModels.Requests.Takmicenja.EvidencijaMeca)grid.BindingContext;
            await Navigation.PushAsync(new TakmicenjaEvidentirajMec(evidencija, viewModel.Takmicenje));
        }
    }
}