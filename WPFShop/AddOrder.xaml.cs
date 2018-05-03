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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
       public AddOrder()
        {
            InitializeComponent();
            this.DataContext = new AddOrderViewModel(this);
        }
        public AddOrder (vwOrder orderEdit)
        {
            InitializeComponent();
            this.DataContext = new AddOrderViewModel(this, orderEdit);
        }

        private void txtCientName_KeyDown(object sender, KeyEventArgs e) //za slova
        {

            //if (txtTotalPrice.Text.Length > 25)
            //{
            //    e.Handled = true;
            //    MessageBox.Show("Name should contain up to 25 letters!");
            //}
            //if (txtClientSurname.Text.Length > 25)
            //{
            //    e.Handled = true;
            //    MessageBox.Show("Lastame should contain up to 25 letters!");
            //}
        }

    }
}
