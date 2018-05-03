using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFShop.Commands;
using WPFShop.ServiceReferenceShop;


namespace WPFShop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Login login;

        #region Constructor
        //poziva pri otvaranju prozora Login 
        public LoginViewModel(Login loginOpen)
        {
            user = new vwUser();
            login = loginOpen;
        }

        #endregion //Constructor

        #region Properties

        private vwUser user;
        public vwUser User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        //private bool isUpdateUser;
        //public bool IsUpdateUser
        //{
        //    get { return isUpdateUser; }
        //    set { isUpdateUser = value; }
        //}
        #endregion // Properties

        // commands

        private ICommand _login;
        public ICommand Login
        {
            get
            {
                if (_login == null)
                {
                    _login = new RelayCommand(param => ExecuteLogin(), param => CanExecuteLogin());
                }
                return _login;
            }
        }

        //void Execute(object parameter)
        //{
        //    var passwordBox = parameter as PasswordBox;
        //    var password = passwordBox.Password;
        //    //Now go ahead and check the user name and password
        //}

        private void ExecuteLogin()
        {
            try
            {
                string user = login.txtUser.Text;
                string pass = login.txtPassword.Password;
                if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    CheckUser(user, pass);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteLogin()
        {
            return true;
        }

        private void CheckUser(string user, string pasword)
        {
            using (Service1Client db = new Service1Client())
            {
                List<vwUser> listAllUser = db.GetAllUser().ToList();
                bool usercheck = false;

                for (int i = 0; i < listAllUser.Count; i++)
                {
                    //MessageBox.Show(listAllUser[i].username.ToString() + "   " + listAllUser[i].password.ToString());
                    if (listAllUser[i].username.Equals(user) && listAllUser[i].password == (pasword))
                    {
                        usercheck = true;
                        MainWindow main = new MainWindow();
                        main.txtSearch.Visibility = Visibility.Hidden;
                        main.lblSearch.Visibility = Visibility.Hidden;
                        main.lblOfficeList.Visibility = Visibility.Hidden;
                        main.lblBossList.Visibility = Visibility.Hidden;
                        main.lblEmployeeList.Visibility = Visibility.Hidden;
                        main.lblOrderList.Visibility = Visibility.Hidden;
                        main.lblAlbumList.Visibility = Visibility.Hidden;
                        main.lblArtistList.Visibility = Visibility.Hidden;
                        main.lblGenreList.Visibility = Visibility.Hidden;
                        main.lblCustomerList.Visibility = Visibility.Hidden;
                        //MessageBox.Show(listAllUser[i].username.ToString() + "   " + listAllUser[i].password.ToString());
                        if (listAllUser[i].role == "admin")
                        {
                            //MainWindow main = new MainWindow();
                            main.Title = "Boss MainWindow";
                            login.Close();
                            main.ShowDialog();
                        }
                        else
                        {
                            //MainWindow main = new MainWindow();
                            main.Title = "Employee MainWindow";
                            main.btnMenageBoss.Visibility = Visibility.Hidden;
                            main.btnMenageOffice.Visibility = Visibility.Hidden;
                            main.btnMenageEmployee.Visibility = Visibility.Hidden;
                            login.Close();
                            main.ShowDialog();
                        }
                    }
                    else
                    {
                    }
                }
                if (usercheck == false)
                {
                    MessageBox.Show("Wrong username or password");
                }
                //vwUser user = (from e in listAllUser where e.password == password select e).First();

                //if (user == null)
                //{
                //    _loginWindow.tbEmail.Clear();
                //    _loginWindow.labelMessage.Content = "There is no employee with email: " + email + "\n If you forgot your email please contact your Human resources manager";
                //}
            }
        }

    //    // PASSWORD 
    //public static readonly DependencyProperty BoundPassword =
    //    DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(LoginViewModel), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));
 
    //public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
    //    "BindPassword", typeof (bool), typeof (LoginViewModel), new PropertyMetadata(false, OnBindPasswordChanged));
 
    //  private static readonly DependencyProperty UpdatingPassword =
    //      DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(LoginViewModel), new PropertyMetadata(false));
 
    //  private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //  {
    //      PasswordBox box = d as PasswordBox;
 
    //      // only handle this event when the property is attached to a PasswordBox
    //      // and when the BindPassword attached property has been set to true
    //      if (d == null || !GetBindPassword(d))
    //      {
    //          return;
    //      }
 
    //      // avoid recursive updating by ignoring the box's changed event
    //      box.PasswordChanged -= HandlePasswordChanged;
 
    //      string newPassword = (string)e.NewValue;
 
    //      if (!GetUpdatingPassword(box))
    //      {
    //          box.Password = newPassword;
    //      }
 
    //      box.PasswordChanged += HandlePasswordChanged;
    //  }
 
    //  private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    //  {
    //      // when the BindPassword attached property is set on a PasswordBox,
    //      // start listening to its PasswordChanged event
 
    //      PasswordBox box = dp as PasswordBox;
 
    //      if (box == null)
    //      {
    //          return;
    //      }
 
    //      bool wasBound = (bool)(e.OldValue);
    //      bool needToBind = (bool)(e.NewValue);
 
    //      if (wasBound)
    //      {
    //          box.PasswordChanged -= HandlePasswordChanged;
    //      }
 
    //      if (needToBind)
    //      {
    //          box.PasswordChanged += HandlePasswordChanged;
    //      }
    //  }
 
    //  private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
    //  {
    //      PasswordBox box = sender as PasswordBox;
 
    //      // set a flag to indicate that we're updating the password
    //      SetUpdatingPassword(box, true);
    //      // push the new password into the BoundPassword property
    //      SetBoundPassword(box, box.Password);
    //      SetUpdatingPassword(box, false);
    //  }
 
    //  public static void SetBindPassword(DependencyObject dp, bool value)
    //  {
    //      dp.SetValue(BindPassword, value);
    //  }
 
    //  public static bool GetBindPassword(DependencyObject dp)
    //  {
    //      return (bool)dp.GetValue(BindPassword);
    //  }
 
    //  public static string GetBoundPassword(DependencyObject dp)
    //  {
    //      return (string)dp.GetValue(BoundPassword);
    //  }
 
    //  public static void SetBoundPassword(DependencyObject dp, string value)
    //  {
    //      dp.SetValue(BoundPassword, value);
    //  }
 
    //  private static bool GetUpdatingPassword(DependencyObject dp)
    //  {
    //      return (bool)dp.GetValue(UpdatingPassword);
    //  }
 
    //  private static void SetUpdatingPassword(DependencyObject dp, bool value)
    //  {
    //      dp.SetValue(UpdatingPassword, value);
    //  }
    }
}
