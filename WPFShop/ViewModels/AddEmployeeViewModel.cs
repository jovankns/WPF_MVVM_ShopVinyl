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
   
    /*Constructor u klasi AddEmployeeViewModel.cs mora da pozove metode 
   * GetAllBoss da bi se listi BossList 
   * dodelio sadržaj iz baze i prikazao u 
   * ComboBox-u bind-ovanim za tu listu
   */
    public class AddEmployeeViewModel : ViewModelBase
    {
        private AddEmployee addEmployee;
        private bool edited = false;
        #region Constructor
        //poziva pri otvaranju prozora AddEmployee pri dodavanju novih employee
        public AddEmployeeViewModel(AddEmployee addEmployeeOpen)
        {
            employee = new vwEmployee();
            addEmployee = addEmployeeOpen;

            using (Service1Client wcf = new Service1Client())
            {
                BossList = wcf.GetAllBoss(null).ToList();
            }
        }

        //poziva pri otvaranju prozora AddEmployee ali pri editovanju nekog employee
        public AddEmployeeViewModel(AddEmployee addEmployeeOpen, vwEmployee EmployeeEdit)
        {
            employee = EmployeeEdit;
            addEmployee = addEmployeeOpen;
            edited = true;
            using (Service1Client wcf = new Service1Client())
            {
                BossList = wcf.GetAllBoss(null).ToList();
            }
        }
        #endregion //Constuctor

        #region Properties
        /*Ova klasa mora, pored Propertie-a Employee, Boss \
         * imati još jedan, čiji su tipovi List<vwEmployeeList> i
         * List<vwEmployeeList> za koje će biti bind-ovani ComboBox
         * i dve komande za Save i Close 
         * (opisano za klasu AddEmployeeViewModel.cs).
         * */
        private vwEmployee employee; // Properti za Employee
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

        private vwBoss boss; //Propertie za BOSS
        public vwBoss Boss
        {
            get { return boss; }
            set
            {
                boss = value;
                OnPropertyChanged("Boss");
            }

        }
        private bool isUpdateBoss;
        public bool IsUpdateBoss
        {
            get { return isUpdateBoss; }
            set { isUpdateBoss = value; }
        }

        private List<vwBoss> bossList; //bossList
        public List<vwBoss> BossList //BossList
        {
            get
            {
                return bossList;
            }

            set
            {
                bossList = value;
                OnPropertyChanged("BossList");
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
            addEmployee.lblValidBoss.Visibility = Visibility.Hidden;
            addEmployee.lblValidName.Visibility = Visibility.Hidden;
            addEmployee.lblValidLastName.Visibility = Visibility.Hidden;
            addEmployee.lblValidDate.Visibility = Visibility.Hidden;
            addEmployee.lblValidAddress.Visibility = Visibility.Hidden;
            addEmployee.lblValidCity.Visibility = Visibility.Hidden;
            addEmployee.lblValidMobile.Visibility = Visibility.Hidden;
            addEmployee.lblValidJob.Visibility = Visibility.Hidden;

            if (edited == true)
            {
                var boss = (from b in BossList where b.BossID == employee.BossID select b).First();
                //addEmployee.cmbBoss.SelectedIndex = boss.BossID;
                Boss = boss;
                edited = false;
            }

            if (addEmployee.cmbBoss.SelectedItem == null)
            {
                addEmployee.lblValidBoss.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(employee.EmployeeName) || employee.EmployeeName.Length < 4
                || employee.EmployeeName.Length > 15 || !Regex.IsMatch(employee.EmployeeName, @"^[a-zA-Z]*$"))
            {
                addEmployee.lblValidName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(employee.EmployeeLastName) || employee.EmployeeLastName.Length < 4
                || employee.EmployeeLastName.Length > 25 || !Regex.IsMatch(employee.EmployeeLastName, @"^[a-zA-Z]*$"))
            {
                addEmployee.lblValidLastName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(addEmployee.txtBirthDate.Text))
            {
                addEmployee.lblValidDate.Visibility = Visibility.Visible;
                return false;
            }
            //!Regex.IsMatch(employee.Address, @"([A-Z|a-z| ]*) ([A-Z|a-z]*) ([\d|A-Z]*)")
            else if (String.IsNullOrEmpty(employee.Address) || employee.Address.Length < 4 
                || employee.Address.Length > 15)
            {
                addEmployee.lblValidAddress.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(employee.City) || employee.City.Length < 4 
                || employee.City.Length > 15 || !Regex.IsMatch(employee.City,@"^[a-zA-Z]"))
            {
                addEmployee.lblValidCity.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(employee.Mobile) || !Regex.IsMatch(employee.Mobile, @"^[0-9]*$")
               || employee.Mobile.Length < 9 || employee.Mobile.Length > 10)
            {
                addEmployee.lblValidMobile.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(employee.JobDescription) || employee.JobDescription.Length < 4
                || employee.JobDescription.Length > 15 || !Regex.IsMatch(employee.JobDescription, @"^[a-zA-Z]"))
            {
                addEmployee.lblValidJob.Visibility = Visibility.Visible;
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
                    int? index = (int)addEmployee.cmbBoss.SelectedValue;
                    string fullName = addEmployee.cmbBoss.Text;
                    var names = fullName.Split(' ');
                    string firstName = names[0];
                    //string lastName = names[1];

                    //int boNameIndex = (from boss in BossList where boss.BossName.Equals(name) select boss.BossID).First();
                    Employee.BossID = index;
                    Employee.BossName = firstName;

                    wcf.AddEmployee(Employee);
                    isUpdateEmployee = true;
                    addEmployee.Close();
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
                addEmployee.Close();
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
