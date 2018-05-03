using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WPFShop;
using WPFShop.Commands;
using WPFShop.ServiceReferenceShop;

using System.IO;
using System.Data;
using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Media;

 namespace WPFShop.ViewModels
{
    class MainWindowViewModel :ViewModelBase
    {
        MainWindow main;

        #region Constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
            using (Service1Client wcf = new Service1Client())
            {
                OfficeList = wcf.GetAllOffice(null).ToList();
                BossList = wcf.GetAllBoss(null).ToList();
                EmployeeList = wcf.GetAllEmployee(null).ToList();
                OrderList = wcf.GetAllOrder(null).ToList();
                AlbumList = wcf.GetAllAlbum(null).ToList();
                GenreList = wcf.GetAllGenre(null).ToList();
                ArtistList = wcf.GetAllArtist(null).ToList();
                CustomerList = wcf.GetAllCustomer(null).ToList();
            }
        }

        private void ClassExportToPdf(DataGrid grid) // za export PDF FILE-ova
        {
            PdfPTable table = new PdfPTable(grid.Columns.Count);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            string nameFile = gridDataName + ".pdf";

            PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(nameFile, System.IO.FileMode.Create));
            doc.Open();

            for (int j = 0; j < grid.Columns.Count; j++)
            {
                table.AddCell(new Phrase(grid.Columns[j].Header.ToString()));
            }
            table.HeaderRows = 1;

            IEnumerable itemsSource = grid.ItemsSource as IEnumerable;
            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                        for (int i = 0; i < grid.Columns.Count; ++i)
                        {
                            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(i);
                            TextBlock txt = cell.Content as TextBlock;
                            if (txt != null)
                            {
                                table.AddCell(new Phrase(txt.Text));
                            }
                        }
                    }
                }
                doc.Add(table);
                string message = ("You exported a datagrid to a file named: " + nameFile);
                MessageBox.Show(message);
                doc.Close();
            }
        }

        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject //  za export PDF FILE-ova
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        //private ListCollectionView _view;
        //public ICollectionView View
        //{
        //    get { return this._view; }
        //}
        private string _TextSearch;
        public string TextSearch
        {
            get { return _TextSearch; }
            set
            {
                _TextSearch = value;
                OnPropertyChanged("TextSearch");

                if (String.IsNullOrEmpty(value))
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        OfficeList = wcf.GetAllOffice(null).ToList();
                        BossList = wcf.GetAllBoss(null).ToList();
                        EmployeeList = wcf.GetAllEmployee(null).ToList();
                        OrderList = wcf.GetAllOrder(null).ToList();
                        AlbumList = wcf.GetAllAlbum(null).ToList();
                        GenreList = wcf.GetAllGenre(null).ToList();
                        ArtistList = wcf.GetAllArtist(null).ToList();
                        CustomerList = wcf.GetAllCustomer(null).ToList();
                    }
                    //View.Filter = null;
                }
                else
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        string s = value.ToString();
                        OfficeList = wcf.GetAllOffice(s).ToList();
                        BossList = wcf.GetAllBoss(s).ToList();
                        EmployeeList = wcf.GetAllEmployee(s).ToList();
                        OrderList = wcf.GetAllOrder(s).ToList();
                        AlbumList = wcf.GetAllAlbum(s).ToList();
                        GenreList = wcf.GetAllGenre(s).ToList();
                        ArtistList = wcf.GetAllArtist(s).ToList();
                        CustomerList = wcf.GetAllCustomer(s).ToList();
                        //View.Filter = new Predicate<object>(o => (()o).FirstName == value);
                    }
                }
            }
        }
        //public List<vwOffice> GetByName(string name)
        //{
        //    using (var context = new Service1Client())
        //    {
        //        return context.GetAllOffice.Where(i => i.FirstName == name).ToList();
        //    }
        //}


        //public MainWindowViewModel(MainWindow mainOpen,vwOffice officeForEdit,string input)
        //{
        //    main = mainOpen;
        //    using (Service1Client wcf = new Service1Client())
        //    {
        //        OfficeList = wcf.GetAllOffice(input).ToList();
        //        BossList = wcf.GetAllBoss().ToList();
        //        EmployeeList = wcf.GetAllEmployee().ToList();
        //        OrderList = wcf.GetAllOrder().ToList();
        //        AlbumList = wcf.GetAllAlbum().ToList();
        //        GenreList = wcf.GetAllGenre().ToList();
        //        ArtistList = wcf.GetAllArtist().ToList();
        //        CustomerList = wcf.GetAllCustomer().ToList();
        //    }
        //}

        #endregion //Constructor

        #region Properties
        //private DataGrid g = null;
        string gridDataName = null;
        private ICommand exportToPDF;
        public ICommand ExportToPDF
        {
            get
            {
                if (exportToPDF == null)
                {
                    exportToPDF = new RelayCommand(param => SaveExportToPDF(), param => CanExportToPDF());
                }
                return exportToPDF;
            }
        }
        private void SaveExportToPDF()
        {

            if (gridDataName != null)
            {
                if (gridDataName == "ListOffice")
                {
                    ClassExportToPdf(main.DataGridOffices);
                }
                else if (gridDataName == "ListBoss")
                {
                    ClassExportToPdf(main.DataGridBoss);
                }
                else if ( gridDataName == "ListEmployee")
                {
                    ClassExportToPdf(main.DataGridEmployee);
                }
                else if (gridDataName == "ListOrder")
                {
                    ClassExportToPdf(main.DataGridOrder);
                }
                else if (gridDataName == "ListAlbum")
                {
                    ClassExportToPdf(main.DataGridAlbum);
                }
                else if (gridDataName == "ListGenre")
                {
                    ClassExportToPdf(main.DataGridGenre);
                }
                else if (gridDataName == "ListArtist")
                {
                    ClassExportToPdf(main.DataGridArtist);
                }
                else if (gridDataName == "ListCustomer")
                {
                    ClassExportToPdf(main.DataGridCustomer);
                }
            }
         
        }
        private bool showView = false; //ako nije prikazan nijedan view na pocetku
        private bool CanExportToPDF()
        {
            if (showView)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*U regionu Properties su potrebna tri Propertie-a: 
         * Student, Suject i Result kao i još tri za StudentList , 
         * SubjectList i ResultList (kao oni za koje su u klasi AddResultViewModel.cs
         * bind-ovani ComboBox-ovi).Da bi se omogućila vidljivost DataGridova za prikaz
         * Studenata i Subject-a samo prilikom klika na dugme, potrebna su još dva 
         * Propertie-a tipa Visibility, koji će u početku biti setovani na Collapsed a 
         * prilikom klika na dugme MenageStudent/MenageSubject vrednost će im biti 
         * setovana na Visible
         * */
        //private string txtSearch_KeyUp; //Office List
        //public string TxtSearch_KeyUp //Office List
        //{
        //    get
        //    {
        //        return txtSearch_KeyUp;
        //    }

        //    set
        //    {
        //        txtSearch_KeyUp = value;
        //        OnPropertyChanged("txtSearch_KeyUp");
        //    }
        //}

        private vwOffice office; // OFFICE
        public vwOffice Office // Properti za Office
        {
            get { return office; }
            set
            {
                office = value;
                OnPropertyChanged("Office");
            }
        }
        private bool isUpdateOffice; //UPDATE OFFICE
        public bool IsUpdateOffice
        {
            get { return isUpdateOffice; }
            set { isUpdateOffice = value; }
        }
        private List<vwOffice> officeList; //Office List
        public List<vwOffice> OfficeList //Office List
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

        private vwBoss boss; // BOSS
        public vwBoss Boss // Properti za Boss
        {
            get { return boss; }
            set
            {
                boss = value;
                OnPropertyChanged("Boss");
            }
        }
        private bool isUpdateBoss; //UPDATE BOSS
        public bool IsUpdateBoss
        {
            get { return isUpdateBoss; }
            set { isUpdateBoss = value; }
        }
        private List<vwBoss> bossList; //Boss List
        public List<vwBoss> BossList //Boss List
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

        private vwEmployee employee; //  EMPLOYEEE
        public vwEmployee Employee // Properti za Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }
        private bool isUpdateEmployee; //UPDATE Employe
        public bool IsUpdateEmployee
        {
            get { return isUpdateEmployee; }
            set { isUpdateEmployee = value; }
        }
        private List<vwEmployee> employeeList; //Employee List
        public List<vwEmployee> EmployeeList //Employee List
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

        private vwOrder order; //  ORDER
        public vwOrder Order // Properti za Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
        private bool isUpdateOrder; //UPDATE Order
        public bool IsUpdateOrder
        {
            get { return isUpdateOrder; }
            set { isUpdateOrder = value; }
        }
        private List<vwOrder> orderList; //Employee List
        public List<vwOrder> OrderList //Employee List
        {
            get
            {
                return orderList;
            }

            set
            {
                orderList = value;
                OnPropertyChanged("OrderList");
            }
        }

        private vwAlbum album; //  ALBUM
        public vwAlbum Album // Properti za Album
        {
            get { return album; }
            set
            {
                album = value;
                OnPropertyChanged("Album");
            }
        }
        private bool isUpdateAlbum; //UPDATE Album
        public bool IsUpdateAlbum
        {
            get { return isUpdateAlbum; }
            set { isUpdateAlbum = value; }
        }
        private List<vwAlbum> albumList; //Album List
        public List<vwAlbum> AlbumList //Album List
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

        private vwGenre genre; //  GENRE
        public vwGenre Genre // Properti za GENRE
        {
            get { return genre; }
            set
            {
                genre = value;
                OnPropertyChanged("Genre");
            }
        }
        private bool isUpdateGenre; //UPDATE Genre
        public bool IsUpdateGenre
        {
            get { return isUpdateGenre; }
            set { isUpdateGenre = value; }
        }
        private List<vwGenre> genreList; //GenreList
        public List<vwGenre> GenreList //GenreList
        {
            get
            {
                return genreList;
            }

            set
            {
                genreList = value;
                OnPropertyChanged("GenreList");
            }
        }

        private vwArtist artist; //  ARTIST
        public vwArtist Artist // Properti za Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged("Artist");
            }
        }
        private bool isUpdateArtist; //UPDATE Artist
        public bool IsUpdateArtist
        {
            get { return isUpdateArtist; }
            set { isUpdateArtist = value; }
        }
        private List<vwArtist> artistList; //artistList
        public List<vwArtist> ArtistList //ArtistList
        {
            get
            {
                return artistList;
            }

            set
            {
                artistList = value;
                OnPropertyChanged("ArtistList");
            }
        }

        private vwCustomer customer; //  CUSTOMER
        public vwCustomer Customer // Properti za Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        private bool isUpdateCustomer; //UPDATE Artist
        public bool IsUpdateCustomer
        {
            get { return isUpdateCustomer; }
            set { isUpdateCustomer = value; }
        }
        private List<vwCustomer> customerList; //customerList
        public List<vwCustomer> CustomerList //CustomerList
        {
            get { return customerList; }
            set
            {
                customerList = value;
                OnPropertyChanged("CustomerList");
            }
        }
   
        // VIEW-RI PROPERTY ZA HIDE , SHOW GRID-OVA
        private Visibility viewOffice = Visibility.Collapsed;//ViewOffice
        public Visibility ViewOffice
        {
            get { return viewOffice; }
            set
            {
                viewOffice = value;
                OnPropertyChanged("ViewOffice");
            }
        }

        private Visibility viewBoss = Visibility.Collapsed;//ViewBoss
        public Visibility ViewBoss
        {
            get { return viewBoss; }
            set
            {
                viewBoss = value;
                OnPropertyChanged("ViewBoss");
            }
        }

        private Visibility viewEmployee = Visibility.Collapsed;//ViewEmployee
        public Visibility ViewEmployee
        {
            get { return viewEmployee; }
            set
            {
                viewEmployee = value;
                OnPropertyChanged("ViewEmployee");
            }
        }

        private Visibility viewOrder = Visibility.Collapsed;//ViewOrder
        public Visibility ViewOrder
        {
            get { return viewOrder; }
            set
            {
                viewOrder = value;
                OnPropertyChanged("ViewOrder");
            }
        }

        private Visibility viewAlbum = Visibility.Collapsed;//ViewAlbum
        public Visibility ViewAlbum
        {
            get { return viewAlbum; }
            set
            {
                viewAlbum = value;
                OnPropertyChanged("ViewAlbum");
            }
        }

        private Visibility viewGenre = Visibility.Collapsed;//ViewGenre
        public Visibility ViewGenre
        {
            get { return viewGenre; }
            set
            {
                viewGenre = value;
                OnPropertyChanged("ViewGenre");
            }
        }

        private Visibility viewArtist = Visibility.Collapsed;//ViewArtist
        public Visibility ViewArtist
        {
            get { return viewArtist; }
            set
            {
                viewArtist = value;
                OnPropertyChanged("ViewArtist");
            }
        }

        private Visibility viewCustomer = Visibility.Collapsed;//ViewCustomer
        public Visibility ViewCustomer
        {
            get { return viewCustomer; }
            set
            {
                viewCustomer = value;
                OnPropertyChanged("ViewCustomer");
            }
        }
        #endregion //Properties

        #region Commands
        private ICommand _logout; //LOGOUT
        public ICommand Logout //LOGOUT
        {
            get
            {
                if (_logout == null)
                {
                    _logout = new RelayCommand(param => ExecuteLogout(), param => CanExecuteLogout());
                }
                return _logout;
            }
        }
        private bool CanExecuteLogout()
        {
            return true;
        }
        private void ExecuteLogout()
        {
            try
            {
                Login log = new Login();
                main.Close();
                log.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private ICommand menageOffice; //Menage Office
        public ICommand MenageOffice //Menage Office
        {
            get
            {
                if (menageOffice == null)
                {
                    menageOffice = new RelayCommand(param => MenageOfficeExecute(), param => CanMenageOfficeExecute());
                }
                return menageOffice;
            }
        }
        private void MenageOfficeExecute()
        {
            try
            {
                gridDataName = "ListOffice";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Visible;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Visible;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                main.SizeToContent = SizeToContent.Width;
                main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageOfficeExecute()
        {
            return true;
        }

        private ICommand menageBoss; //menageBoss
        public ICommand MenageBoss //MenageBoss
        {
            get
            {
                if (menageBoss == null)
                {
                    menageBoss = new RelayCommand(param => MenageBossExecute(), param => CanMenageBossExecute());
                }
                return menageBoss;
            }
        }
        private void MenageBossExecute()
        {
            try
            {
                gridDataName = "ListBoss";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Visible;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Visible;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageBossExecute()
        {
            return true;
        }

        private ICommand menageEmployee; //menageEmployee
        public ICommand MenageEmployee //MenageEmployee
        {
            get
            {
                if (menageEmployee == null)
                {
                    menageEmployee = new RelayCommand(param => MenageEmployeeExecute(), param => CanMenageEmployeeExecute());
                }
                return menageEmployee;
            }
        }
        private void MenageEmployeeExecute()
        {
            try
            {
                gridDataName = "ListEmployee";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Visible;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Visible;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageEmployeeExecute()
        {
            return true;
        }

        private ICommand menageOrder; //menageOrder
        public ICommand MenageOrder //MenageOrder
        {
            get
            {
                if (menageOrder == null)
                {
                    menageOrder = new RelayCommand(param => MenageOrderExecute(), param => CanMenageOrderExecute());
                }
                return menageOrder;
            }
        }
        private void MenageOrderExecute()
        {
            try
            {
                gridDataName = "ListOrder";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Visible;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Visible;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageOrderExecute()
        {
            return true;
        }


        private ICommand menageAlbum; //menageAlbum
        public ICommand MenageAlbum //MenageAlbum
        {
            get
            {
                if (menageAlbum == null)
                {
                    menageAlbum = new RelayCommand(param => MenageAlbumExecute(), param => CanMenageAlbumExecute());
                }
                return menageAlbum;
            }
        }
        private void MenageAlbumExecute()
        {
            try
            {
                gridDataName = "ListAlbum";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Visible;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Visible;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageAlbumExecute()
        {
            return true;
        }

        private ICommand menageGenre; //menageGenre
        public ICommand MenageGenre //MenageGenre
        {
            get
            {
                if (menageGenre == null)
                {
                    menageGenre = new RelayCommand(param => MenageGenreExecute(), param => CanMenageGenreExecute());
                }
                return menageGenre;
            }
        }
        private void MenageGenreExecute()
        {
            try
            {
                gridDataName = "ListGenre";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Visible;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Visible;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageGenreExecute()
        {
            return true;
        }

        private ICommand menageArtist; //menageArtist
        public ICommand MenageArtist //MenageArtist
        {
            get
            {
                if (menageArtist == null)
                {
                    menageArtist = new RelayCommand(param => MenageArtistExecute(), param => CanMenageArtistExecute());
                }
                return menageArtist;
            }
        }
        private void MenageArtistExecute()
        {
            try
            {
                gridDataName = "ListArtist";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Visible;
                ViewCustomer = Visibility.Hidden;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Visible;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Hidden;
                //main.SizeToContent = SizeToContent.Width;
                //main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageArtistExecute()
        {
            return true;
        }

        private ICommand menageCustomer; //menageCustomer
        public ICommand MenageCustomer //MenageCustomer
        {
            get
            {
                if (menageCustomer == null)
                {
                    menageCustomer = new RelayCommand(param => MenageCustomerExecute(), param => CanMenageCustomerExecute());
                }
                return menageCustomer;
            }
        }
        private void MenageCustomerExecute()
        {
            try
            {
                gridDataName = "ListCustomer";
                showView = true;
                main.txtSearch.Visibility = Visibility.Visible;
                main.lblSearch.Visibility = Visibility.Visible;
                ViewOffice = Visibility.Hidden;
                ViewBoss = Visibility.Hidden;
                ViewEmployee = Visibility.Hidden;
                ViewOrder = Visibility.Hidden;
                ViewAlbum = Visibility.Hidden;
                ViewGenre = Visibility.Hidden;
                ViewArtist = Visibility.Hidden;
                ViewCustomer = Visibility.Visible;
                Office = null;
                Boss = null;
                Employee = null;
                Order = null;
                Album = null;
                Genre = null;
                Artist = null;
                Customer = null;
                main.lblOfficeList.Visibility = Visibility.Hidden;
                main.lblBossList.Visibility = Visibility.Hidden;
                main.lblEmployeeList.Visibility = Visibility.Hidden;
                main.lblOrderList.Visibility = Visibility.Hidden;
                main.lblAlbumList.Visibility = Visibility.Hidden;
                main.lblArtistList.Visibility = Visibility.Hidden;
                main.lblGenreList.Visibility = Visibility.Hidden;
                main.lblCustomerList.Visibility = Visibility.Visible;
                main.SizeToContent = SizeToContent.Width;
                main.SizeToContent = SizeToContent.Height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanMenageCustomerExecute()
        {
            return true;
        }

        // ADD COMANDE ZA OTVARANJE PROZORA ZA ADD-OVANJE I EDITOVANJE
        //OFFICE
             /*Klikom na dugme Add Office poziva se komanda AddNewOffice u okviru koje se
         * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
         * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
         * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
         * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
         * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewOffice;  //  OFFICE
        public ICommand AddNewOffice
        {
            get
            {
                if (addNewOffice == null)
                {
                    addNewOffice = new RelayCommand(param => AddNewOfficeExecute(), param => CanAddNewOfficeExecute());
                }
                return addNewOffice;
            }
        }
        private void AddNewOfficeExecute()
        {
            try
            {
                AddOffice addOffice = new AddOffice();
                addOffice.ShowDialog();
                if ((addOffice.DataContext as AddOfficeViewModel).IsUpdateOffice == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        OfficeList = wcf.GetAllOffice(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewOfficeExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editOffice;
        public ICommand EditOffice
        {
            get
            {
                if (editOffice == null)
                {
                    editOffice = new RelayCommand(param => EditOfficeExecute(), param => CanEditOfficeExecute());
                }
                return editOffice;
            }
        }
        private void EditOfficeExecute()
        {
            try
            {
                if (Office != null)
                {
                    AddOffice addOffice = new AddOffice(Office);
                    addOffice.Title = "Edit Office";
                    addOffice.ShowDialog();
                    if ((addOffice.DataContext as AddOfficeViewModel).IsUpdateOffice == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            OfficeList = wcf.GetAllOffice(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditOfficeExecute()
        {
            if (Office == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteOffice;
        public ICommand DeleteOffice
        {
            get
            {
                if (deleteOffice == null)
                {
                    deleteOffice= new RelayCommand(param => DeleteOfficeExecute(), param => CanDeleteOfficeExecute());
                }
                return deleteOffice;
            }
        }

        private void DeleteOfficeExecute()
        {
            try
            {
                if (Office != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int officeID = office.OfficeID;
                        bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        if (isOffice == false)
                        {
                            wcf.DeleteOffice(officeID);
                            OfficeList = wcf.GetAllOffice(null).ToList();
                        }
                        else
                        {
                            MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteOfficeExecute()
        {
            if (Office == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // BOSS
        /*Klikom na dugme Add Boss poziva se komanda AddNewBoss u okviru koje se
    * izvršava metoda AddNewBossExecute, prilikom čega se otvara novi prozor
    * AddBoss i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridBoss. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllBoss i ono sto se dobije kao rezultat se snima u 
    * Propertie BossList za koji je bind-ovan DataGrid.*/
        private ICommand addNewBoss;  //  BOSS
        public ICommand AddNewBoss
        {
            get
            {
                if (addNewBoss == null)
                {
                    addNewBoss = new RelayCommand(param => AddNewBossExecute(), param => CanAddNewBossExecute());
                }
                return addNewBoss;
            }
        }
        private void AddNewBossExecute()
        {
            try
            {
                AddBoss addBoss = new AddBoss();
                addBoss.ShowDialog();
                if ((addBoss.DataContext as AddBossViewModel).IsUpdateBoss == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        BossList = wcf.GetAllBoss(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewBossExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editBoss;
        public ICommand EditBoss
        {
            get
            {
                if (editBoss == null)
                {
                    editBoss = new RelayCommand(param => EditBossExecute(), param => CanEditBossExecute());
                }
                return editBoss;
            }
        }
        private void EditBossExecute()
        {
            try
            {
                if (Boss != null)
                {
                    AddBoss addBoss = new AddBoss(Boss);
                    addBoss.Title = "Edit Boss";
                    addBoss.ShowDialog();
                    if ((addBoss.DataContext as AddBossViewModel).IsUpdateBoss == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            BossList = wcf.GetAllBoss(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditBossExecute()
        {
            if (Boss == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteBoss;
        public ICommand DeleteBoss
        {
            get
            {
                if (deleteBoss == null)
                {
                    deleteBoss = new RelayCommand(param => DeleteBossExecute(), param => CanDeleteBossExecute());
                }
                return deleteBoss;
            }
        }

        private void DeleteBossExecute()
        {
            try
            {
                if (Boss != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int bossID = boss.BossID;
                        //bool isBoss = wcf.IsBossID(bossID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isBoss == false)
                        //{
                            wcf.DeleteBoss(bossID);
                            BossList = wcf.GetAllBoss(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteBossExecute()
        {
            if (Boss == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // EMPLOYEE
        /*Klikom na dugme Add Office poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewEmployee;  //  EMPLOYEE
        public ICommand AddNewEmployee
        {
            get
            {
                if (addNewEmployee == null)
                {
                    addNewEmployee = new RelayCommand(param => AddNewEmployeeExecute(), param => CanAddNewEmployeeExecute());
                }
                return addNewEmployee;
            }
        }
        private void AddNewEmployeeExecute()
        {
            try
            {
                AddEmployee addEmployee = new AddEmployee();
                addEmployee.ShowDialog();
                if ((addEmployee.DataContext as AddEmployeeViewModel).IsUpdateEmployee == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        EmployeeList = wcf.GetAllEmployee(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewEmployeeExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editEmployee;
        public ICommand EditEmployee
        {
            get
            {
                if (editEmployee == null)
                {
                    editEmployee = new RelayCommand(param => EditEmployeeExecute(), param => CanEditEmployeeExecute());
                }
                return editEmployee;
            }
        }
        private void EditEmployeeExecute()
        {
            try
            {
                if (Employee != null)
                {
                    
                    AddEmployee addEmployee = new AddEmployee(Employee);
                    addEmployee.Title = "Edit Employee";
                    addEmployee.ShowDialog();

                    if ((addEmployee.DataContext as AddEmployeeViewModel).IsUpdateEmployee == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            EmployeeList = wcf.GetAllEmployee(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditEmployeeExecute()
        {
            if (Employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteEmployee;
        public ICommand DeleteEmployee
        {
            get
            {
                if (deleteEmployee == null)
                {
                    deleteEmployee = new RelayCommand(param => DeleteEmployeeExecute(), param => CanDeleteEmployeeExecute());
                }
                return deleteEmployee;
            }
        }

        private void DeleteEmployeeExecute()
        {
            try
            {
                if (Employee != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int employeeID = employee.EmployeeID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                            wcf.DeleteEmployee(employeeID);
                            EmployeeList = wcf.GetAllEmployee(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteEmployeeExecute()
        {
            if (Employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // ORDER
        /*Klikom na dugme Add Order poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewOrder;  //  Order
        public ICommand AddNewOrder
        {
            get
            {
                if (addNewOrder == null)
                {
                    addNewOrder = new RelayCommand(param => AddNewOrderExecute(), param => CanAddNewOrderExecute());
                }
                return addNewOrder;
            }
        }
        private void AddNewOrderExecute()
        {
            try
            {
                AddOrder addOrder = new AddOrder();
                addOrder.ShowDialog();
                if ((addOrder.DataContext as AddOrderViewModel).IsUpdateOrder == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        OrderList = wcf.GetAllOrder(null).ToList();
                        AlbumList = wcf.GetAllAlbum(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewOrderExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editOrder;
        public ICommand EditOrder
        {
            get
            {
                if (editOrder == null)
                {
                    editOrder = new RelayCommand(param => EditOrderExecute(), param => CanEditOrderExecute());
                }
                return editOrder;
            }
        }
        private void EditOrderExecute()
        {
            try
            {
                if (Order != null)
                {
                    //<ComboBox IsEditable="False" IsHitTestVisible="False" Focusable="False">
                    //IsEditable=false will make sure you can't type in it
                    //IsHitTestVisible=false will make sure you can't click it
                    //Focusable=false will make sure you can't use tab to give it focus and change it with the arrow keys
                    AddOrder addOrder = new AddOrder(Order);
                    addOrder.Title = "Edit Order";
                    addOrder.cmbAlbum.IsEditable = false;
                    addOrder.cmbAlbum.IsHitTestVisible = false;
                    addOrder.cmbAlbum.Focusable = false;
                    addOrder.ShowDialog();
                    if ((addOrder.DataContext as AddOrderViewModel).IsUpdateOrder == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            OrderList = wcf.GetAllOrder(null).ToList();
                            AlbumList = wcf.GetAllAlbum(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditOrderExecute()
        {
            if (Order == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteOrder;
        public ICommand DeleteOrder
        {
            get
            {
                if (deleteOrder == null)
                {
                    deleteOrder = new RelayCommand(param => DeleteOrderExecute(), param => CanDeleteOrderExecute());
                }
                return deleteOrder;
            }
        }

        private void DeleteOrderExecute()
        {
            try
            {
                if (Order != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int orderID = order.OrderID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                        wcf.DeleteOrder(orderID);
                        OrderList = wcf.GetAllOrder(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteOrderExecute()
        {
            if (Order == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // ALBUM
        /*Klikom na dugme Add Album poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewAlbum;  //  Album
        public ICommand AddNewAlbum
        {
            get
            {
                if (addNewAlbum == null)
                {
                    addNewAlbum = new RelayCommand(param => AddNewAlbumExecute(), param => CanAddNewAlbumExecute());
                }
                return addNewAlbum;
            }
        }
        private void AddNewAlbumExecute()
        {
            try
            {
                AddAlbum addAlbum = new AddAlbum();
                addAlbum.ShowDialog();
                if ((addAlbum.DataContext as AddAlbumViewModel).IsUpdateAlbum == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        AlbumList = wcf.GetAllAlbum(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewAlbumExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editAlbum;
        public ICommand EditAlbum
        {
            get
            {
                if (editAlbum == null)
                {
                    editAlbum = new RelayCommand(param => EditAlbumExecute(), param => CanEditAlbumExecute());
                }
                return editAlbum;
            }
        }
        private void EditAlbumExecute()
        {
            try
            {
                if (Album != null)
                {
                    AddAlbum addAlbum = new AddAlbum(Album);
                    addAlbum.Title = "Edit Album";
                    addAlbum.ShowDialog();
                    if ((addAlbum.DataContext as AddAlbumViewModel).IsUpdateAlbum == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            AlbumList = wcf.GetAllAlbum(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditAlbumExecute()
        {
            if (Album == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteAlbum;
        public ICommand DeleteAlbum
        {
            get
            {
                if (deleteAlbum == null)
                {
                    deleteAlbum = new RelayCommand(param => DeleteAlbumExecute(), param => CanDeleteAlbumExecute());
                }
                return deleteAlbum;
            }
        }

        private void DeleteAlbumExecute()
        {
            try
            {
                if (Album != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int albumID = album.AlbumID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                        wcf.DeleteAlbum(albumID);
                        AlbumList = wcf.GetAllAlbum(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteAlbumExecute()
        {
            if (Album == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // GENRE
        /*Klikom na dugme Add Order poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewGenre;  //  Genre
        public ICommand AddNewGenre
        {
            get
            {
                if (addNewGenre == null)
                {
                    addNewGenre = new RelayCommand(param => AddNewGenreExecute(), param => CanAddNewGenreExecute());
                }
                return addNewGenre;
            }
        }
        private void AddNewGenreExecute()
        {
            try
            {
                AddGenre addGenre = new AddGenre();
                addGenre.ShowDialog();
                if ((addGenre.DataContext as AddGenreViewModel).IsUpdateGenre == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        GenreList = wcf.GetAllGenre(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewGenreExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editGenre;
        public ICommand EditGenre
        {
            get
            {
                if (editGenre == null)
                {
                    editGenre = new RelayCommand(param => EditGenreExecute(), param => CanEditGenreExecute());
                }
                return editGenre;
            }
        }
        private void EditGenreExecute()
        {
            try
            {
                if (Genre != null)
                {
                    AddGenre addGenre = new AddGenre(Genre);
                    addGenre.Title = "Edit Genre";
                    addGenre.ShowDialog();
                    if ((addGenre.DataContext as AddGenreViewModel).IsUpdateGenre == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            GenreList = wcf.GetAllGenre(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditGenreExecute()
        {
            if (Genre == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteGenre;
        public ICommand DeleteGenre
        {
            get
            {
                if (deleteGenre == null)
                {
                    deleteGenre = new RelayCommand(param => DeleteGenreExecute(), param => CanDeleteGenreExecute());
                }
                return deleteGenre;
            }
        }

        private void DeleteGenreExecute()
        {
            try
            {
                if (Genre != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int genreID = genre.GenreID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                        wcf.DeleteGenre(genreID);
                        GenreList = wcf.GetAllGenre(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteGenreExecute()
        {
            if (Genre == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // ARTIST
        /*Klikom na dugme Add Artist poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewArtist;  //  Artist
        public ICommand AddNewArtist
        {
            get
            {
                if (addNewArtist == null)
                {
                    addNewArtist = new RelayCommand(param => AddNewArtistExecute(), param => CanAddNewArtistExecute());
                }
                return addNewArtist;
            }
        }
        private void AddNewArtistExecute()
        {
            try
            {
                AddArtist addArtist = new AddArtist();
                addArtist.ShowDialog();
                if ((addArtist.DataContext as AddArtistViewModel).IsUpdateArtist == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        ArtistList = wcf.GetAllArtist(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewArtistExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editArtist;
        public ICommand EditArtist
        {
            get
            {
                if (editArtist == null)
                {
                    editArtist = new RelayCommand(param => EditArtistExecute(), param => CanEditArtistExecute());
                }
                return editArtist;
            }
        }
        private void EditArtistExecute()
        {
            try
            {
                if (Artist != null)
                {
                    AddArtist addArtist = new AddArtist(Artist);
                    addArtist.Title = "Edit Title";
                    addArtist.ShowDialog();
                    if ((addArtist.DataContext as AddArtistViewModel).IsUpdateArtist == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            ArtistList = wcf.GetAllArtist(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditArtistExecute()
        {
            if (Artist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteArtist;
        public ICommand DeleteArtist
        {
            get
            {
                if (deleteArtist == null)
                {
                    deleteArtist = new RelayCommand(param => DeleteArtistExecute(), param => CanDeleteArtistExecute());
                }
                return deleteArtist;
            }
        }

        private void DeleteArtistExecute()
        {
            try
            {
                if (Artist != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int artistID = artist.ArtistID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                        wcf.DeleteArtist(artistID);
                        ArtistList = wcf.GetAllArtist(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteArtistExecute()
        {
            if (Artist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // CUSTOMER
        /*Klikom na dugme Add Customer poziva se komanda AddNewOffice u okviru koje se
    * izvršava metoda AddNewOffieExecute, prilikom čega se otvara novi prozor
    * AddOffice i proverava se da li je izvršena neka promena i ako jeste Update-uje se 
    * DataGridOffice. Update-ovanje se vrši tako što se ponovo poziva metoda sa 
    * Service-a GetAllOffice i ono sto se dobije kao rezultat se snima u 
    * Propertie OfficeList za koji je bind-ovan DataGird.*/
        private ICommand addNewCustomer;  //  Customer
        public ICommand AddNewCustomer
        {
            get
            {
                if (addNewCustomer == null)
                {
                    addNewCustomer = new RelayCommand(param => AddNewCustomerExecute(), param => CanAddNewCustomerExecute());
                }
                return addNewCustomer;
            }
        }
        private void AddNewCustomerExecute()
        {
            try
            {
                AddCustomer addCustomer = new AddCustomer();
                addCustomer.ShowDialog();
                if ((addCustomer.DataContext as AddCustomerViewModel).IsUpdateCustomer == true)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        CustomerList = wcf.GetAllCustomer(null).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddNewCustomerExecute()
        {
            return true;
        }

        /*Prilikom klika na dugme Edit Office se izvršava komanda EditOffice u okviru čije metode
         * EditOfficetExecute se prvo proverava da li je neki Office selektovan i ako jeste otvara
         * se prozor AddSOffice kojem se prosleĎuje Office koji je selektovan. U metodi 
         * CanEditOfficeExecute se postavlja uslov da ako ni jedan Office nije selektovan 
         * metoda vraća false i onemogućuje klik na dugme Edit Office.*/
        private ICommand editCustomer;
        public ICommand EditCustomer
        {
            get
            {
                if (editCustomer == null)
                {
                    editCustomer = new RelayCommand(param => EditCustomerExecute(), param => CanEditCustomerExecute());
                }
                return editCustomer;
            }
        }
        private void EditCustomerExecute()
        {
            try
            {
                if (Customer != null)
                {
                    AddCustomer addCustomer = new AddCustomer(Customer);
                    addCustomer.Title = "Edit Customer";
                    addCustomer.ShowDialog();
                    if ((addCustomer.DataContext as AddCustomerViewModel).IsUpdateCustomer == false)
                    {
                        using (Service1Client wcf = new Service1Client())
                        {
                            CustomerList = wcf.GetAllCustomer(null).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditCustomerExecute()
        {
            if (Customer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Klikom na dugme Delete Office se izvršava komanda DeleteOffice u okviru čije metode 
         * DeleteOfficeExecute se prvo proverava da li je neki Office selektovan, zatim se pomoću 
         * metode na Service-u IsOfficeID proverava da li se ključ tog Office nalazi u tabeli Result // ovo ne treba
         * i ako se ne nalazi vriši se brisanje a ukoliko se nalazi korisniku se pojavljuje poruka 
         * da tog Studenta nije moguće obrisati. Metoda CanDeleteStudentExecute onemogućava klik 
         * na dugme Delete Student ukoliko ni jedan student nije selektovan.*/
        private ICommand deleteCustomer;
        public ICommand DeleteCustomer
        {
            get
            {
                if (deleteCustomer == null)
                {
                    deleteCustomer = new RelayCommand(param => DeleteCustomerExecute(), param => CanDeleteCustomerExecute());
                }
                return deleteCustomer;
            }
        }

        private void DeleteCustomerExecute()
        {
            try
            {
                if (Customer != null)
                {
                    using (Service1Client wcf = new Service1Client())
                    {
                        int customerID = customer.CustomerID;
                        //bool isOffice = wcf.IsOfficeID(officeID); // ovu metodu ubaciti u wcf jedino ako ocemo 
                        //if (isOffice == false)
                        //{
                        wcf.DeleteCustomer(customerID);
                        CustomerList = wcf.GetAllCustomer(null).ToList();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ovo je ovde ako cemo da pozivano metodu u wcf servicu za proveru dali postoji u drugoj tabeli");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteCustomerExecute()
        {
            if (Customer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion //Commands
    }
}
