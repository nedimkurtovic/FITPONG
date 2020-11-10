using FIT_PONG.Mobile.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnesiMejl : ContentPage
    {
        UnesiMejlViewModel viewModel;

        public UnesiMejl(string tip)
        {
            BindingContext = viewModel = new UnesiMejlViewModel(tip);
            viewModel.Navigacija = Navigation;
            InitializeComponent();
        }
    }
}