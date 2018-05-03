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
    /// Interaction logic for AddArtist.xaml
    /// </summary>
    public partial class AddArtist : Window
    {
        public AddArtist()
        {
            InitializeComponent();
            this.DataContext = new AddArtistViewModel(this);
        }
        public AddArtist(vwArtist artistEdit)
        {
            InitializeComponent();
            this.DataContext = new AddArtistViewModel(this, artistEdit);
        }
    }
}
