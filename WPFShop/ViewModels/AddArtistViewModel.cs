using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFShop.Commands;
using WPFShop.ServiceReferenceShop;

namespace WPFShop.ViewModels
{
    public class AddArtistViewModel : ViewModelBase
    {
        private AddArtist addArtist;

        #region Constructor
        //poziva pri otvaranju prozora AddOffice pri dodavanju novi h
        public AddArtistViewModel(AddArtist addArtistOpen)
        {
            artist = new vwArtist();
            addArtist = addArtistOpen;
        }

        //poziva pri otvaranju prozora AddOffice ali pri editovanju nekog office-a
        public AddArtistViewModel(AddArtist addArtistOpen, vwArtist ArtistEdit)
        {
            artist = ArtistEdit;
            addArtist = addArtistOpen;
        }
        #endregion //Constructor

        #region Properties

        private vwArtist artist;
        public vwArtist Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged("Artist");
            }
        }

        private bool isUpdateArtist;
        public bool IsUpdateArtist
        {
            get { return isUpdateArtist; }
            set { isUpdateArtist = value; }
        }
        #endregion // Properties

        #region Commands
        /*U okviru metode Execute za komandu Save se poziva metoda sa 
         * servisa kojoj se prosleĎuje Propertie za 
         * koji su bind-ovani TextBox-ovi u AddOffice.xaml-u. 
         * Propertie IsUpdateOffice se postavlja na true da bi
         * se nakon snimanja podataka u bazu Update-ovao sadržaj tabele 
         * u Main.xaml-u koji prikazu sadržaj tabele Office.*/
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }
        /*Execute, čiji sadržaj govori šta treba da se izvrši u okviru ove komande i
        -CanExecut, čiji sadržaj govori u kojim slučajevima treba onemogućiti izvršavanje komande*/
        private bool CanSaveExecute() /*U okviru metode CanExecute za komandu Save je
              * onemogućeno izvršavanje komande, sve dok se ne unesu obavezna polja.*/
        {
            addArtist.lblValidArtist.Visibility = Visibility.Hidden;
            if (String.IsNullOrEmpty(artist.ArtistName) || artist.ArtistName.Length < 4
                || artist.ArtistName.Length > 15 || Regex.IsMatch(artist.ArtistName, @"[\d]"))
            {
                addArtist.lblValidArtist.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                return true;
            }
        }
        private void SaveExecute()//SaveExecute
        {
            try
            {
                using (Service1Client wcf = new Service1Client())
                {
                    wcf.AddArtist(Artist);
                    isUpdateArtist = true;
                    addArtist.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand close;//CLOSE
        public ICommand Close//CLOSE
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }

        private void CloseExecute()//CloseExecute
        {
            try
            {
                addArtist.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanCloseExecute()//CanCloseExecute
        {
            return true;
        }
        #endregion
    }
}
