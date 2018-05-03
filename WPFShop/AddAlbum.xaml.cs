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
    /// Interaction logic for AddAlbum.xaml
    /// </summary>
    public partial class AddAlbum : Window
    {
         public AddAlbum()
        {
            InitializeComponent();
            this.DataContext = new AddAlbumViewModel(this);
        }
        public AddAlbum (vwAlbum albumEdit)
        {
            InitializeComponent();
            this.DataContext = new AddAlbumViewModel(this, albumEdit);
        }
    }
}
