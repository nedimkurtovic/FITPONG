using FIT_PONG.Mobile.ViewModels.Takmicenja;
using FIT_PONG.Mobile.ViewModels.Takmicenja.Objave;
using FIT_PONG.Mobile.Views.Takmicenja.Objave;
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
    public partial class TakmicenjaMain : TabbedPage
    {
        TakmicenjaDetaljiViewModel viewModel;
        public TakmicenjaMain(TakmicenjaDetaljiViewModel model)
        {
            InitializeComponent();

            BindingContext = viewModel = model;
            var TakmicenjaDetaljiStranica = new TakmicenjaDetalji(model);
            TakmicenjaDetaljiStranica.Title = "Detalji";
            Children.Add(TakmicenjaDetaljiStranica);

            var TakmicenjaRezultatiStranica = new TakmicenjaRezultati(model.Takmicenje);
            TakmicenjaRezultatiStranica.Title = "Rezultati";
            Children.Add(TakmicenjaRezultatiStranica);

            var TakmicenjaTabelaStranica = new TakmicenjaTabela(model.Takmicenje);
            TakmicenjaTabelaStranica.Title = "Tabela";
            Children.Add(TakmicenjaTabelaStranica);

            var TakmicenjaEvidentirajMecStranica = new TakmicenjaEvidencije(model.Takmicenje);
            TakmicenjaEvidentirajMecStranica.Title = "Evidentiraj meč";
            Children.Add(TakmicenjaEvidentirajMecStranica);

            var TakmicenjaObjaveStranica = new TakmicenjaObjave(model.Takmicenje);
            TakmicenjaObjaveStranica.Title = "Objave";
            Children.Add(TakmicenjaObjaveStranica);

            Title = model.Takmicenje.Naziv;
        }

        public void OsvjeziListuObjava()
        {
            var children = Children;
            foreach (var i in children)
            {
                if (i is TakmicenjaObjave)
                {
                    var b = i.BindingContext as TakmicenjaObjaveViewModel;
                    if (b.DobaviObjaveKomanda.CanExecute(null))
                    {
                        b.DobaviObjaveKomanda.Execute(null);
                        return;
                    }
                }
      
            }

        }
    }
}