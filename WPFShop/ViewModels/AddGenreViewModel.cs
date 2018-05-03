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
    public class AddGenreViewModel : ViewModelBase
    {
        private AddGenre addGenre;

        #region Constructor
        //poziva pri otvaranju prozora AddGenre pri dodavanju novih
        public AddGenreViewModel(AddGenre addGenreOpen)
        {
           genre = new vwGenre();
           addGenre = addGenreOpen;
        }

        //poziva pri otvaranju prozora AddGenre ali pri editovanju nekog genre-a
        public AddGenreViewModel(AddGenre addGenreOpen, vwGenre GenreEdit)
        {
            genre = GenreEdit;
            addGenre = addGenreOpen;
        }
        #endregion //Constructor

        #region Properties

        private vwGenre genre;
        public vwGenre Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                OnPropertyChanged("Genre");
            }
        }

        private bool isUpdateGenre;
        public bool IsUpdateGenre
        {
            get { return isUpdateGenre; }
            set { isUpdateGenre = value; }
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
            addGenre.lblValidName.Visibility = Visibility.Hidden;
            if (String.IsNullOrEmpty(genre.Name) || genre.Name.Length < 4
                || genre.Name.Length > 15 || !Regex.IsMatch(genre.Name, @"^[a-zA-Z]*$"))
            {
                addGenre.lblValidName.Visibility = Visibility.Visible;
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
                    wcf.AddGenre(Genre);
                    isUpdateGenre = true;
                    addGenre.Close();
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
                addGenre.Close();
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
