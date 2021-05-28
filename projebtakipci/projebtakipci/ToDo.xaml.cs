using projebtakipci.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projebtakipci
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDo : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        public ToDo()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            int id = Convert.ToInt32(Application.Current.Properties["id"]);
            base.OnAppearing();
            var allPersons = await firebaseHelper.GetDuty(id);
            lstPersons.ItemsSource = allPersons;
        }
        private async void checkedchange(object sender, EventArgs e)
        {

            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.AutomationId != null)
            {
                await firebaseHelper.UpdateDuty(Convert.ToInt32(checkbox.AutomationId), checkbox.IsChecked);
            }
        }
    }
}