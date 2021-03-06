﻿using FIT_PONG.Mobile.ViewModels.Takmicenja;
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
    public partial class TakmicenjaLista : ContentPage
    {
        TakmicenjaListaViewModel viewModel;
        public TakmicenjaLista()
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaListaViewModel();
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var gridlejaut = (BindableObject)sender;
            var _takmicenje = (SharedModels.Takmicenja)gridlejaut.BindingContext;
            await Navigation.PushAsync(new TakmicenjaMain(new TakmicenjaDetaljiViewModel(_takmicenje)));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (viewModel.ListaTakmicenja.Count == 0)
            //    viewModel.IsBusy = true;

        }

        private async void Dodaj_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TakmicenjaDodaj());
        }

        private void btnDobaviJos_Clicked(object sender, EventArgs e)
        {

            if (viewModel.DobaviJosTakmicenja.CanExecute(null))
                viewModel.DobaviJosTakmicenja.Execute(null);

        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            if (viewModel.DobaviTakmicenja.CanExecute(null))
                viewModel.DobaviTakmicenja.Execute(null);

        }

    }
}