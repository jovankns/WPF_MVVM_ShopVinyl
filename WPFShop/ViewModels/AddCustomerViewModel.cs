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
   
    public class AddCustomerViewModel : ViewModelBase
    {
        private AddCustomer addCustomer;

        #region Constructor
        //poziva pri otvaranju prozora AddBoss pri dodavanju novih boss
        public AddCustomerViewModel(AddCustomer addCustomerOpen)
        {
            customer = new vwCustomer();
            addCustomer = addCustomerOpen;
        }

        //poziva pri otvaranju prozora AddBoss ali pri editovanju nekog boss
        public AddCustomerViewModel(AddCustomer addCustomerOpen, vwCustomer CustomerEdit)
        {
            customer = CustomerEdit;
            addCustomer = addCustomerOpen;
        }
        #endregion //Constuctor

        #region Properties
        /*Ova klasa mora, pored Propertie-a Student, Suject i Result \
         * imati još dva, čiji su tipovi List<vwStudentList> i
         * List<vwSubjectList> za koje će biti bind-ovani ComboBox-ovi
         * i dve komande za Save i Close 
         * (opisano za klasu AddSudentViewModel.cs).
         * */
        private vwCustomer customer; // Properti za CUSTOMER
        public vwCustomer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        private bool isUpdateCustomer;
        public bool IsUpdateCustomer
        {
            get { return isUpdateCustomer; }
            set { isUpdateCustomer = value; }
        }
        #endregion // Properties

        #region Commands
        private ICommand save; // SAVE
        public ICommand Save // SAVE
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
        private bool CanSaveExecute() //CanSaveExecute
        {
            addCustomer.lblValidName.Visibility = Visibility.Hidden;
            addCustomer.lblValidLastName.Visibility = Visibility.Hidden;
            addCustomer.lblValiCountry.Visibility = Visibility.Hidden;
            addCustomer.lblValidAddress.Visibility = Visibility.Hidden;
            addCustomer.lblValidCity.Visibility = Visibility.Hidden;
            addCustomer.lblValidMobile.Visibility = Visibility.Hidden;
            if (String.IsNullOrEmpty(customer.Name) || customer.Name.Length < 4
                || customer.Name.Length > 15 || !Regex.IsMatch(customer.Name, @"^[a-zA-Z]*$"))
            {
                addCustomer.lblValidName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(customer.LastName) || customer.LastName.Length < 4
                || customer.LastName.Length > 15 || !Regex.IsMatch(customer.LastName, @"^[a-zA-Z]*$"))
            {
                addCustomer.lblValidLastName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(customer.Country) || customer.Country.Length < 4
                || customer.Country.Length > 15 || !Regex.IsMatch(customer.Country, @"^[a-zA-Z]*$"))
            {
                addCustomer.lblValiCountry.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(customer.Address) || customer.Address.Length < 2 
                || customer.Address.Length > 15)
            {
                addCustomer.lblValidAddress.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(customer.City) || customer.City.Length < 2
                || customer.City.Length > 15 || !Regex.IsMatch(customer.City, @"^[a-zA-Z]*$"))
            {
                addCustomer.lblValidCity.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(customer.Mobile) || !Regex.IsMatch(customer.Mobile, @"^[0-9]*$")
               || customer.Mobile.Length < 9 || customer.Mobile.Length > 10)
            {
                addCustomer.lblValidMobile.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SaveExecute() //SaveExecut
        {
            try
            {
                using (Service1Client wcf = new Service1Client())
                {
                    wcf.AddCustomer(Customer);
                    isUpdateCustomer = true;
                    addCustomer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand close; //CLOSE
        public ICommand Close //CLOSE
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
        private void CloseExecute() //CloseExecute
        {
            try
            {
                addCustomer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanCloseExecute() //CanCloseExecute
        {
            return true;
        }
        #endregion
    }
}
