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
    public class AddOfficeViewModel : ViewModelBase
    {
        private AddOffice addOffice;

        #region Constructor
        //poziva pri otvaranju prozora AddOffice pri dodavanju novi h
        public AddOfficeViewModel(AddOffice addOfficeOpen)
        {
            office = new vwOffice();
            addOffice = addOfficeOpen;
        }

        //poziva pri otvaranju prozora AddOffice ali pri editovanju nekog office-a
        public AddOfficeViewModel(AddOffice addOfficeOpen, vwOffice OfficeEdit)
        {
            office = OfficeEdit;
            addOffice = addOfficeOpen;
        }
        #endregion //Constructor

        #region Properties

        private vwOffice office;
        public vwOffice Office
        {
            get { return office; }
            set
            {
                office = value;
                OnPropertyChanged("Office");
            }
        }

        private bool isUpdateOffice;
        public bool IsUpdateOffice
        {
            get { return isUpdateOffice; }
            set { isUpdateOffice = value; }
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
            addOffice.lblValidOfficeName.Visibility = Visibility.Hidden;
            addOffice.lblValidCity.Visibility = Visibility.Hidden;
            addOffice.lblValidAddress.Visibility = Visibility.Hidden;
            addOffice.lblValidPostalCode.Visibility = Visibility.Hidden;
            addOffice.lblValidPhone.Visibility = Visibility.Hidden;
            addOffice.lblValidMobile.Visibility = Visibility.Hidden;
            if (String.IsNullOrEmpty(office.OfficeName) || office.OfficeName.Length < 4 
                || office.OfficeName.Length > 20 )
            {
                addOffice.lblValidOfficeName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(office.City) ||office.City.Length < 4 
                || office.City.Length > 20 || !Regex.IsMatch(office.City, @"^[a-zA-Z]*$"))
            {
                addOffice.lblValidCity.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(office.Address) || office.Address.Length < 4 
                || office.Address.Length > 20)
            {
                addOffice.lblValidAddress.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(office.PostalCode) || !Regex.IsMatch(office.PostalCode, @"^[0-9]*$") 
                || office.PostalCode.Length != 6)
            {
                addOffice.lblValidPostalCode.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(office.Phone) || !Regex.IsMatch(office.Phone, @"^[0-9]*$") 
                || office.Phone.Length < 6 || office.Phone.Length > 12)
            {
                addOffice.lblValidPhone.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(office.Mobile) || !Regex.IsMatch(office.Mobile, @"^[0-9]*$")
               || office.Mobile.Length < 9 || office.Mobile.Length > 10)
            {
                addOffice.lblValidMobile.Visibility = Visibility.Visible;
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
                    wcf.AddOffice(Office);
                    isUpdateOffice = true;
                    addOffice.Close();
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
                addOffice.Close();
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
