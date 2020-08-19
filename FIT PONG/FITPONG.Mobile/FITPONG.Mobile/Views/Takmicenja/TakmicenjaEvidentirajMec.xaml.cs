using FIT_PONG.Mobile.ViewModels.Takmicenja;
using FIT_PONG.SharedModels.Requests.Takmicenja;
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
    public partial class TakmicenjaEvidentirajMec : ContentPage
    {
        TakmicenjaEvidentirajMecViewModel viewModel;
        public TakmicenjaEvidentirajMec(EvidencijaMeca _evidencija,SharedModels.Takmicenja takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaEvidentirajMecViewModel(_evidencija,takmicenje);

        }

        private async void Evidentiraj_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.Evidencija();
            if(rezultat)
            {
                
                //ovaj dio koda treba napravit extenzijom, vec sam ga 3 puta koristio
                var mainStranica = Navigation.NavigationStack[1];
                Navigation.InsertPageBefore(new TakmicenjaMain(new TakmicenjaDetaljiViewModel(viewModel.takmicenje)), mainStranica);
                bool brisi = false;

                List<Page> listaBrisanja = new List<Page>();
                foreach (var i in Navigation.NavigationStack)
                {
                    if (brisi)
                        listaBrisanja.Add(i);
                    if (i is TakmicenjaMain && !brisi)
                        brisi = true;
                }
                //kad naleti na prvu main stavlja na true i sve ostale stranice brise poslije
                var a = Navigation.NavigationStack[0].BindingContext as TakmicenjaListaViewModel;
                if (a.DobaviTakmicenja.CanExecute(null))
                    a.DobaviTakmicenja.Execute(null);
                foreach (var i in listaBrisanja)
                    Navigation.RemovePage(i);
            }
        }
    }
}