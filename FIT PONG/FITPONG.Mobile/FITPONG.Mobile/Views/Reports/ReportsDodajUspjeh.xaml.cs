using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportsDodajUspjeh : ContentPage
    {
        public ReportsDodajUspjeh()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Navigation.InsertPageBefore(new ReportsDodaj(), this);
            Navigation.PopAsync();
        }
    }
}