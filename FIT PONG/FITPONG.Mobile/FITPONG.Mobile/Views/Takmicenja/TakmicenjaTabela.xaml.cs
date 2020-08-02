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
    public partial class TakmicenjaTabela : ContentPage
    {
        TakmicenjaTabelaViewModel viewModel;
        public TakmicenjaTabela(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaTabelaViewModel(_takmicenje);
            if (viewModel.DobaviTabelu.CanExecute(null))
                viewModel.DobaviTabelu.Execute(null);
        }
    }
}