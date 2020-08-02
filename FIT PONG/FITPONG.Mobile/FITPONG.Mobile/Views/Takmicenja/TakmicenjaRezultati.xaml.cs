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
    public partial class TakmicenjaRezultati : ContentPage
    {
        TakmicenjaRezultatiViewModel viewModel;
        public TakmicenjaRezultati(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaRezultatiViewModel(_takmicenje);
            if(viewModel.DobaviRezultateKomanda.CanExecute(null))
                viewModel.DobaviRezultateKomanda.Execute(null);
            listViewGrupisani.ItemsSource = viewModel.listaGrupisanihStavki;
            listViewGrupisani.IsGroupingEnabled = true;
            listViewGrupisani.GroupDisplayBinding = new Binding("NaslovGrupe");
            listViewGrupisani.GroupShortNameBinding = new Binding("NaslovGrupe");
        }
        

    }
}