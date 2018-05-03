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
    /*Constructor u klasi AddBossViewModel.cs mora da pozove metode 
  * GetAllOffice da bi se listama OfficeList 
  * dodelio sadržaj iz baze i prikazao u 
  * ComboBox-ovima bind-ovanim za te liste
  */
    public class AddOrderViewModel : ViewModelBase
    {
        private AddOrder addOrder;
        private bool edit;
        private bool edited = false;

        #region Constructor
        //poziva pri otvaranju prozora AddBoss pri dodavanju novih boss
        public AddOrderViewModel(AddOrder addOrderOpen)
        {
            order = new vwOrder();
            addOrder = addOrderOpen;
            edit = false;
            using (Service1Client wcf = new Service1Client())
            {
                EmployeeList = wcf.GetAllEmployee(null).ToList();
                AlbumList = wcf.GetAllAlbum(null).ToList();
                CustomerList = wcf.GetAllCustomer(null).ToList();
            }
        }

        //poziva pri otvaranju prozora AddBoss ali pri editovanju nekog boss
        public AddOrderViewModel(AddOrder addOrderOpen, vwOrder OrderEdit)
        {
            order = OrderEdit;
            addOrder = addOrderOpen;
            edit = true;
            edited = true;
            using (Service1Client wcf = new Service1Client())
            {
                EmployeeList = wcf.GetAllEmployee(null).ToList();
                AlbumList = wcf.GetAllAlbum(null).ToList();
                CustomerList = wcf.GetAllCustomer(null).ToList();
            }
        }
        #endregion //Constuctor

        #region Properties
        /*Ova klasa mora, pored Propertie-a Student, Suject i Result \
         * imati još dva, čiji su tipovi List<vwStudentList> i
         * List<vwSubjectList> za koje će biti bind-ovani ComboBox-ovi
         * i dve komande za Save i Close 
         * (opisano za klasu AddSudentViewModel.cs).
         * */
        private vwOrder order; // Properti za ORDER
        public vwOrder Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
        private bool isUpdateOrder;
        public bool IsUpdateOrder
        {
            get { return isUpdateOrder; }
            set { isUpdateOrder = value; }
        }

        private vwEmployee employee; //Propertie za EMPLOYEE
        public vwEmployee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }
        private bool isUpdateEmployee;
        public bool IsUpdateEmployee
        {
            get { return isUpdateEmployee; }
            set { isUpdateEmployee = value; }
        }
        private List<vwEmployee> employeeList; //employeeList
        public List<vwEmployee> EmployeeList //EmployeeList
        {
            get
            {
                return employeeList;
            }

            set
            {
                employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        private vwAlbum album; //Propertie za ALBUM
        public vwAlbum Album
        {
            get { return album; }
            set
            {
                album = value;
                OnPropertyChanged("Album");
            }
        }
        private bool isUpdateAlbum;
        public bool IsUpdateAlbum
        {
            get { return isUpdateAlbum; }
            set { isUpdateAlbum = value; }
        }
        private List<vwAlbum> albumList; //albumList
        public List<vwAlbum> AlbumList //AlbumList
        {
            get
            {
                return albumList;
            }

            set
            {
                albumList = value;
                OnPropertyChanged("AlbumList");
            }
        }

        private vwCustomer customer; //Propertie za CUSTOMER
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
        private List<vwCustomer> customerList; //customerList
        public List<vwCustomer> CustomerList //CustomerList
        {
            get
            {
                return customerList;
            }

            set
            {
                customerList = value;
                OnPropertyChanged("CustomerList");
            }
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
            addOrder.lblValidEmployee.Visibility = Visibility.Hidden;
            addOrder.lblValidAlbum.Visibility = Visibility.Hidden;
            addOrder.lblValidCustomer.Visibility = Visibility.Hidden;
            addOrder.lblValidDate.Visibility = Visibility.Hidden;
            addOrder.lblValidPrice.Visibility = Visibility.Hidden;
            addOrder.lblValidNumber.Visibility = Visibility.Hidden;

            if (edited == true)
            {
                var employee = (from e in EmployeeList where e.EmployeeID == order.EmployeeID select e).First();
                Employee = employee;

                var customer = (from c in CustomerList where c.CustomerID == order.CustomerID select c).First();
                Customer = customer;
                edited = false;
            }

            if (addOrder.cmbEmployee.SelectedItem == null)
            {
                addOrder.lblValidEmployee.Visibility = Visibility.Visible;
                return false;
            }
            else if (addOrder.cmbAlbum.SelectedItem == null)
            {
                addOrder.lblValidAlbum.Visibility = Visibility.Visible;
                return false;
            }
            else if (addOrder.cmbCustomer.SelectedItem == null)
            {
                addOrder.lblValidCustomer.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(addOrder.txtOrderDate.Text))
            {
                addOrder.lblValidDate.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(order.TotalPrice) || !Regex.IsMatch(order.TotalPrice, @"^[0-9]+(\.[0-9]{1,2})?$")
                || order.TotalPrice.Length < 4)
            {
                addOrder.lblValidPrice.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(addOrder.txtNumberOfPieces.Text.ToString()) || order.NumberOfPieces > 10
                || !Regex.IsMatch(addOrder.txtNumberOfPieces.Text.ToString(), @"^[0-9]*$"))
            {
                addOrder.lblValidNumber.Visibility = Visibility.Visible;
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
                    int albumIndex = (int)addOrder.cmbAlbum.SelectedValue;
                    string title = addOrder.cmbAlbum.Text;
                    //int albumIndex = (from a in AlbumList where a.Title == title select a.AlbumID).First();
                    int number = Convert.ToInt32(order.NumberOfPieces);
                    //Order.AlbumID = albumIndex;
                    int albumID = albumIndex;
                    bool? isOrder = null;
                    if (edit == true)
                    {
                        isOrder = wcf.IsOrderStorageEDIT(Order.OrderID, albumID, number);//check dali ima albuma kada editujemo na stanju!!!
                    }
                    else
                    {
                        isOrder = wcf.IsOrderStorage(albumID, number); // check dali ima albuma na stanju!!!
                    }
                    if (isOrder == true)
                    {
                        string fullName = addOrder.cmbEmployee.Text;
                        var names = fullName.Split(' ');
                        string firstName = names[0];
                        Order.EmployeeName = firstName;

                        int emNameIndex = (int)addOrder.cmbEmployee.SelectedValue;
                        Order.EmployeeID = emNameIndex;

                        string fullNameCustomer = addOrder.cmbCustomer.Text;
                        var nameCustomer = fullNameCustomer.Split(' ');
                        string firstNameCustomer = nameCustomer[0];
                        Order.Name = firstNameCustomer;

                        int customerIndex = (int)addOrder.cmbCustomer.SelectedValue;
                        Order.CustomerID = customerIndex;

                        //string title = addOrder.cmbAlbum.Text;
                        //int albumIndex = (from a in AlbumList where a.Title == title select a.AlbumID).First();
                        //Order.AlbumID = albumIndex;
                        Order.AlbumID = albumIndex;
                       
                        wcf.AddOrder(Order);
                        isUpdateOrder = true;
                        addOrder.Close();
                    }
                    else
                    {
                        int? numberOnState = (from s in AlbumList 
                                                where s.AlbumID == albumID 
                                                select s.Storage).FirstOrDefault();
                        MessageBox.Show("The rest is only: " + numberOnState.ToString() + " albums in the state\n" +
                                    "You can not order: " + number.ToString() + " albums");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
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
                addOrder.Close();
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
