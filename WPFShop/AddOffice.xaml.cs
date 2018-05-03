using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFShop.ServiceReferenceShop;
using WPFShop.ViewModels;

namespace WPFShop
{
    /// <summary>
    /// Interaction logic for AddOffice.xaml
    /// </summary>
    public partial class AddOffice : Window
    {
        public AddOffice()
        {
            InitializeComponent();
            this.DataContext = new AddOfficeViewModel(this);
        }
        public AddOffice(vwOffice officeEdit)
        {
            InitializeComponent();
            this.DataContext = new AddOfficeViewModel(this, officeEdit);
        }


        private void txtCity_KeyDown(object sender, KeyEventArgs e) //za slova
        {

            if (txtCity.Text.Length > 15)
            {
                e.Handled = true;
                MessageBox.Show("City should contain up to 15 letters!");
            }
            //if (txtClientSurname.Text.Length > 25)
            //{
            //    e.Handled = true;
            //    MessageBox.Show("Lastame should contain up to 25 letters!");
            //}
        }

        //private void txtMobile_KeyDown(object sender, KeyEventArgs e) //za brojeve
        //{
        //    if (e.Key < Key.D0 || e.Key > Key.D9)
        //    {
        //        if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
        //        {
        //            if (e.Key != Key.Back && e.Key != Key.Tab)
        //            {
        //                e.Handled = true;
        //                MessageBox.Show("Please fill this field with numbers!");
        //            }
        //        }
        //    }
        //}

        //private void txtManMobile_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string email = txtClientEmail.Text;
        //    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        //    Match match = regex.Match(email);
        //    if (!match.Success)
        //        MessageBox.Show("email format should be in example@example.com format");
        //}
    }
}
