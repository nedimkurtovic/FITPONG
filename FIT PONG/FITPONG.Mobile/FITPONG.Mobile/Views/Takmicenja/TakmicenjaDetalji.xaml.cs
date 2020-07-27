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
    public partial class TakmicenjaDetalji : ContentPage
    {
        TakmicenjaDetaljiViewModel viewModel;
        public TakmicenjaDetalji(TakmicenjaDetaljiViewModel vm)
        {
            InitializeComponent();
            BindingContext = viewModel = vm;
        }
    }
}