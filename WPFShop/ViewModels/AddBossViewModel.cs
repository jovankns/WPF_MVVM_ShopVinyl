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
    public class AddBossViewModel : ViewModelBase
    {
        private AddBoss addBoss;
        
        #region Constructor
        //poziva pri otvaranju prozora AddBoss pri dodavanju novih boss
        public AddBossViewModel(AddBoss addBossOpen)
        {
            boss = new vwBoss();
            addBoss = addBossOpen;
           
            using (Service1Client wcf = new Service1Client())
            {
                OfficeList = wcf.GetAllOffice(null).ToList();
            }
        }

        //poziva pri otvaranju prozora AddBoss ali pri editovanju nekog boss
        public AddBossViewModel(AddBoss addBossOpen, vwBoss BossEdit)
        {
            boss = BossEdit;
            addBoss = addBossOpen;

            using (Service1Client wcf = new Service1Client())
            {
               OfficeList = wcf.GetAllOffice(null).ToList();
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
        private vwBoss boss; // Properti za BOSS
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

        private vwOffice office; //Propertie za OFFICE
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

        private List<vwOffice> officeList; //officeList
        public List<vwOffice> OfficeList //OfficeList
        {
            get
            {
                return officeList;
            }

            set
            {
                officeList = value;
                OnPropertyChanged("OfficeList");
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
            addBoss.lblValidOffice.Visibility = Visibility.Hidden;
            addBoss.lblValidName.Visibility = Visibility.Hidden;
            addBoss.lblValidLastName.Visibility = Visibility.Hidden;
            addBoss.lblValidDate.Visibility = Visibility.Hidden;
            addBoss.lblValidAddress.Visibility = Visibility.Hidden;
            addBoss.lblValidCity.Visibility = Visibility.Hidden;
            addBoss.lblValidMobile.Visibility = Visibility.Hidden;
            addBoss.lblValidJob.Visibility = Visibility.Hidden;
            if (addBoss.cmbOffice.SelectedItem == null)
            {
                addBoss.lblValidOffice.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.BossName) || boss.BossName.Length < 4
                || boss.BossName.Length > 15 || !Regex.IsMatch(boss.BossName, @"^[a-zA-Z]*$"))
            {
                addBoss.lblValidName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.BossLastName) || boss.BossLastName.Length < 4
                || boss.BossLastName.Length > 15 || !Regex.IsMatch(boss.BossLastName, @"^[a-zA-Z]*$"))
            {
                addBoss.lblValidLastName.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(addBoss.txtBirthDate.Text))
            {
                addBoss.lblValidDate.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.Address) || boss.Address.Length < 4 || boss.Address.Length > 15)
            {
                addBoss.lblValidAddress.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.City) || boss.City.Length < 4
                || boss.City.Length > 15 || !Regex.IsMatch(boss.City, @"^[a-zA-Z]*$"))
            {
                addBoss.lblValidCity.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.Mobile) || !Regex.IsMatch(boss.Mobile, @"^[0-9]*$")
               || boss.Mobile.Length < 9 || boss.Mobile.Length > 10)
            {
                addBoss.lblValidMobile.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(boss.JobDescritions) || boss.JobDescritions.Length < 4
                || boss.JobDescritions.Length > 15 || Regex.IsMatch(boss.JobDescritions, @"[\d]"))
            {
                addBoss.lblValidJob.Visibility = Visibility.Visible;
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
                    int? index = (int)addBoss.cmbOffice.SelectedValue;
                    //string name = addBoss.cmbOffice.Text;
                    //int ofNameIndex = (from of in OfficeList where of.OfficeName == name select of.OfficeID).First();
                    Boss.OfficeID = index;

                    wcf.AddBoss(Boss);
                    isUpdateBoss = true;
                    addBoss.Close();
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
                addBoss.Close();
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
