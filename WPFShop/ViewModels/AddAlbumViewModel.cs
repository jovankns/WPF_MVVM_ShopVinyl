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
    public class AddAlbumViewModel : ViewModelBase
    {
        private AddAlbum addAlbum;
        
        #region Constructor
        //poziva pri otvaranju prozora AddBoss pri dodavanju novih albuma
        public AddAlbumViewModel(AddAlbum addAlbumOpen)
        {
            album = new vwAlbum();
            addAlbum = addAlbumOpen;
            using (Service1Client wcf = new Service1Client())
            {
                GenreList = wcf.GetAllGenre(null).ToList();
                ArtistList = wcf.GetAllArtist(null).ToList();
            }
        }

        //poziva pri otvaranju prozora AddAlbum ali pri editovanju nekog albuma
        public AddAlbumViewModel(AddAlbum addAlbumOpen, vwAlbum AlbumEdit)
        {
            album = AlbumEdit;
            addAlbum = addAlbumOpen;

            using (Service1Client wcf = new Service1Client())
            {
                GenreList = wcf.GetAllGenre(null).ToList();
                ArtistList = wcf.GetAllArtist(null).ToList();
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
        private vwAlbum album; // Properti za ALBUM
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

        private vwGenre genre; //Propertie za GENRE
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
        private List<vwGenre> genreList; //employeeList
        public List<vwGenre> GenreList //EmployeeList
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

        private vwArtist artist; //Propertie za ARTIST
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
        private List<vwArtist> artistList; //albumList
        public List<vwArtist> ArtistList //AlbumList
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
            addAlbum.lblValidGenre.Visibility = Visibility.Hidden;
            addAlbum.lblValidArtist.Visibility = Visibility.Hidden;
            addAlbum.lblValidTitle.Visibility = Visibility.Hidden;
            addAlbum.lblValidPrice.Visibility = Visibility.Hidden;
            addAlbum.lblValidStorage.Visibility = Visibility.Hidden;
            if (addAlbum.cmbGenre.SelectedItem == null)
            {
                addAlbum.lblValidGenre.Visibility = Visibility.Visible;
                return false;
            }
            else if (addAlbum.cmbArtist.SelectedItem == null)
            {
                addAlbum.lblValidArtist.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(album.Title) || album.Title.Length < 4 || album.Title.Length > 50 
                || Regex.IsMatch(album.Title, @"[\d]"))
            {
                addAlbum.lblValidTitle.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(album.Price) || !Regex.IsMatch(album.Price, @"^[0-9]+(\.[0-9]{1,2})?$")
                || album.Price.Length < 5)
            {
                addAlbum.lblValidPrice.Visibility = Visibility.Visible;
                return false;
            }
            else if (String.IsNullOrEmpty(addAlbum.txtStorage.Text.ToString()) 
                || !Regex.IsMatch(addAlbum.txtStorage.Text.ToString(), @"^[0-9]*$") || album.Storage > 99)
            {
                addAlbum.lblValidStorage.Visibility = Visibility.Visible;
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
                    string genreName = addAlbum.cmbGenre.Text;
                    string artistName = addAlbum.cmbArtist.Text;
                    int geNameIndex = (from ge in GenreList where ge.Name == genreName select ge.GenreID).First();
                    Album.GenreID = geNameIndex;

                    int artistIndex = (from ar in ArtistList where ar.ArtistName == artistName select ar.ArtistID).First();
                    Album.ArtistID = artistIndex;

                    wcf.AddAlbum(Album);
                    isUpdateAlbum = true;
                    addAlbum.Close();
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
                addAlbum.Close();
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
