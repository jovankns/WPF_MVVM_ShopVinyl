using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFShopVinyl
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        List<vwUser> IService1.GetAllUser() //GET ALL USER
        {
            try
            {
                using (VinylRecordsShopEntities ctx = new VinylRecordsShopEntities())
                {
                    List<vwUser> list = new List<vwUser>();
                    list = (from x in ctx.vwUsers select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwOffice> IService1.GetAllOffice(string Search) // GET ALL OFFICE
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwOffice> list = new List<vwOffice>();
                    list = (from x in context.vwOffices select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var nameMatches = x.OfficeName != null && x.OfficeName.ToLower().Contains(search);
                            //var addressMatches = x.Address.ToLower().StartsWith(search);
                            var addressMatches = x.Address != null && x.Address.ToLower().Contains(search);
                            var cityMatches = x.City != null && x.City.ToLower().Contains(search);
                            var postalCodeMatches = x.PostalCode.ToLower().Contains(search);
                            var mobileMatches = x.Mobile != null && x.Mobile.ToLower().Contains(search);
                            var phoneMatches = x.Phone != null && x.Phone.ToLower().Contains(search);
                            return nameMatches || addressMatches || cityMatches || postalCodeMatches
                                || mobileMatches || phoneMatches;
                            //return nameMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwBoss> IService1.GetAllBoss(string Search) // GET ALL BOSS
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwBoss> list = new List<vwBoss>();
                    list = (from b in context.vwBosses select b).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var nameOMatches = x.OfficeName != null && x.OfficeName.ToLower().Contains(search);
                            var nameMatches = x.BossName != null && x.BossName.ToLower().Contains(search);
                            var lastNameMatches = x.BossLastName != null && x.BossLastName.ToLower().Contains(search);
                            var birthDateMatches = x.BirthDate != null && x.BirthDate.ToString().ToLower().Contains(search);
                            var addressMatches = x.Address != null && x.Address.ToLower().Contains(search);
                            var cityMatches = x.City != null && x.City.ToLower().Contains(search);
                            var mobileMatches = x.Mobile != null && x.Mobile.ToLower().Contains(search);
                            var jobMatches = x.JobDescritions != null && x.JobDescritions.ToLower().Contains(search);
                            return nameOMatches || nameMatches || lastNameMatches || birthDateMatches
                                || addressMatches || cityMatches || mobileMatches || jobMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwEmployee> IService1.GetAllEmployee(string Search) // GET ALL EMPLOYEES
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwEmployee> list = new List<vwEmployee>();
                    list = (from e in context.vwEmployees select e).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var nameBMatches = x.BossName != null && x.BossName.ToLower().Contains(search);
                            var nameMatches = x.EmployeeName != null && x.EmployeeName.ToLower().Contains(search);
                            var lastNameMatches = x.EmployeeLastName != null && x.EmployeeLastName.ToLower().Contains(search);
                            var birthDateMatches = x.BirthDate != null && x.BirthDate.ToString().ToLower().Contains(search);
                            var addressMatches = x.Address != null && x.Address.ToLower().Contains(search);
                            var cityMatches = x.City != null && x.City.ToLower().Contains(search);
                            var mobileMatches = x.Mobile != null && x.Mobile.ToLower().Contains(search);
                            var jobMatches = x.JobDescription != null && x.JobDescription.ToLower().Contains(search);
                            return nameBMatches || nameMatches || lastNameMatches || birthDateMatches
                                || addressMatches || cityMatches || mobileMatches || jobMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwOrder> IService1.GetAllOrder(string Search) // GET ALL ORDERS
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwOrder> list = new List<vwOrder>();
                    list = (from x in context.vwOrders select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var employeeNameMatches = x.EmployeeName != null && x.EmployeeName.ToLower().Contains(search);
                            var titleMatches = x.Title != null && x.Title.ToLower().Contains(search);
                            var customerMatches = x.Name != null && x.Name.ToLower().Contains(search);
                            var dateMatches = x.OrderDate != null && x.OrderDate.ToString().ToLower().Contains(search);
                            var totalPriceMatches = x.TotalPrice != null && x.TotalPrice.ToString().ToLower().Contains(search);
                            var numberOfPieces = x.NumberOfPieces != null && x.NumberOfPieces.ToString().ToLower().Contains(search);
                            return employeeNameMatches || titleMatches || customerMatches || dateMatches
                                || totalPriceMatches || numberOfPieces;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwAlbum> IService1.GetAllAlbum(string Search) // GET ALL ALBUMS
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwAlbum> list = new List<vwAlbum>();
                    list = (from x in context.vwAlbums select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var genreMatches = x.Name != null && x.Name.ToLower().Contains(search);
                            var artistMatches = x.ArtistName != null && x.ArtistName.ToLower().Contains(search);
                            var titleMatches = x.Title != null && x.Title.ToLower().Contains(search);
                            var storageMatches = x.Storage != null && x.Storage.ToString().ToLower().Contains(search);
                            return genreMatches || artistMatches || titleMatches || storageMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwGenre> IService1.GetAllGenre(string Search) // GET ALL GENRES
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwGenre> list = new List<vwGenre>();
                    list = (from x in context.vwGenres select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var name = x.Name != null && x.Name.ToLower().Contains(search);
                            var desc = x.Description != null && x.Description.ToLower().Contains(search);
                            return name || desc;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwArtist> IService1.GetAllArtist(string Search) // GET ALL ARTIST
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwArtist> list = new List<vwArtist>();
                    list = (from x in context.vwArtists select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var nameMatches = x.ArtistName != null && x.ArtistName.ToLower().Contains(search);
                            return nameMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        List<vwCustomer> IService1.GetAllCustomer(string Search) // GET ALL CUSTOMERS
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    List<vwCustomer> list = new List<vwCustomer>();
                    list = (from x in context.vwCustomers select x).ToList();
                    if (!String.IsNullOrEmpty(Search))
                    {
                        list = list.Where(x =>
                        {
                            var search = Search.ToLower();
                            var nameMatches = x.Name != null && x.Name.ToLower().Contains(search);
                            var lastNameMatches = x.LastName != null && x.LastName.ToLower().Contains(search);
                            var countryMatches = x.Country != null && x.Country.ToLower().Contains(search);
                            var addressMatches = x.Address != null && x.Address.ToLower().Contains(search);
                            var cityMatches = x.City != null && x.City.ToLower().Contains(search);
                            var mobileMatches = x.Mobile != null && x.Mobile.ToLower().Contains(search);
                            return nameMatches || lastNameMatches || countryMatches || addressMatches
                                || cityMatches || mobileMatches;
                        }).ToList();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        // ADD , EDIT AND DETELE OFFICE
        vwOffice IService1.AddOffice(vwOffice office) // ADD OFFICE
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (office.OfficeID == 0)
                    {   //ZA ADD
                        tblOffice newOffice = new tblOffice();
                        newOffice.OfficeName = office.OfficeName;
                        newOffice.City = office.City;
                        newOffice.Address = office.Address;
                        newOffice.PostalCode = office.PostalCode;
                        newOffice.Phone = office.Phone;
                        newOffice.Mobile = office.Mobile;
                        context.tblOffices.Add(newOffice);
                        context.SaveChanges();
                        office.OfficeID = office.OfficeID;
                        return office;
                    }
                    else
                    { // ZA EDIT
                        tblOffice officeToEdit = (from f in context.tblOffices where f.OfficeID == office.OfficeID select f).First();
                        officeToEdit.OfficeName = office.OfficeName;
                        officeToEdit.City = office.City;
                        officeToEdit.Address = office.Address;
                        officeToEdit.PostalCode = office.PostalCode;
                        officeToEdit.Phone = office.Phone;
                        officeToEdit.Mobile = office.Mobile;
                        //officeToEdit.OfficeID = office.OfficeID;
                        context.Entry(officeToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return office;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteOffice(int officeID) // DeleteOffice
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblOffice officeToDelete = (from f in context.tblOffices where f.OfficeID == officeID select f).First();
                    context.tblOffices.Remove(officeToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
        // tblOffice za delete Office
        bool IService1.IsOfficeID(int officeId)
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    int result = (from x in context.vwOffices where x.OfficeID == officeId select x.OfficeID).First();

                    if (result == 0) // samo ako ne postoji u tabeli vrati se 0
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message.ToString());
                return false;
            }
        }

        // ADD , EDIT I DELETE ZA BOSS 
        vwBoss IService1.AddBoss(vwBoss boss) // AddBoss
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (boss.BossID == 0)
                    {   // ZA ADD 
                        tblBoss newBoss = new tblBoss();
                        newBoss.OfficeID = boss.OfficeID;
                        newBoss.BossName = boss.BossName;
                        newBoss.BossLastName = boss.BossLastName;
                        newBoss.BirthDate = boss.BirthDate;
                        newBoss.Address = boss.Address;
                        newBoss.City = boss.City;
                        newBoss.Mobile = boss.Mobile;
                        newBoss.JobDescritions = boss.JobDescritions;
                        context.tblBosses.Add(newBoss);
                        context.SaveChanges();
                        boss.BossID = newBoss.BossID;
                        return boss;
                    }
                    else
                    {   // ZA EDIT
                        tblBoss bossToEdit = (from r in context.tblBosses where r.BossID == boss.BossID select r).First();
                        bossToEdit.OfficeID = boss.OfficeID;
                        bossToEdit.BossName = boss.BossName;
                        bossToEdit.BossLastName = boss.BossLastName;
                        bossToEdit.BirthDate = boss.BirthDate;
                        bossToEdit.Address = boss.Address;
                        bossToEdit.City = boss.City;
                        bossToEdit.Mobile = boss.Mobile;
                        bossToEdit.JobDescritions = boss.JobDescritions;
                        //bossToEdit.BossID = boss.BossID;
                        context.Entry(bossToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return boss;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteBoss(int bossID) // DeleteBoss
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblBoss bossToDelete = (from r in context.tblBosses where r.BossID == bossID select r).First();
                    context.tblBosses.Remove(bossToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA EMPLOYEE 
        vwEmployee IService1.AddEmployee(vwEmployee employee) // AddEmployee
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (employee.EmployeeID == 0)
                    {   // ZA ADD 
                        tblEmployee newEmployee = new tblEmployee();
                        newEmployee.BossID = employee.BossID;
                        newEmployee.EmployeeName = employee.EmployeeName;
                        newEmployee.EmployeeLastName = employee.EmployeeLastName;
                        newEmployee.BirthDate = employee.BirthDate;
                        newEmployee.Address = employee.Address;
                        newEmployee.City = employee.City;
                        newEmployee.Mobile = employee.Mobile;
                        newEmployee.JobDescription = employee.JobDescription;
                        context.tblEmployees.Add(newEmployee);
                        context.SaveChanges();
                        employee.EmployeeID = newEmployee.EmployeeID;
                        return employee;
                    }
                    else
                    {   // ZA EDIT
                        tblEmployee employeeToEdit = (from r in context.tblEmployees where r.EmployeeID == employee.EmployeeID select r).First();
                        employeeToEdit.BossID = employee.BossID;
                        employeeToEdit.EmployeeName = employee.EmployeeName;
                        employeeToEdit.EmployeeLastName = employee.EmployeeLastName;
                        employeeToEdit.BirthDate = employee.BirthDate;
                        employeeToEdit.Address = employee.Address;
                        employeeToEdit.City = employee.City;
                        employeeToEdit.Mobile = employee.Mobile;
                        employeeToEdit.JobDescription = employee.JobDescription;
                        //employeeToEdit.EmployeeID = employee.EmployeeID;
                        context.Entry(employeeToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return employee;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteEmployee(int employeeID) // DeleteEmployee
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblEmployee employeeToDelete = (from r in context.tblEmployees where r.EmployeeID == employeeID select r).First();
                    context.tblEmployees.Remove(employeeToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA ORDER
        bool IService1.IsOrderStorage(int albumId,int number) //dali ima dovoljno albuma kada se pravi nov order
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    int storage = (int)(from x in context.vwAlbums where x.AlbumID == albumId select x.Storage).First();
                    int result = storage - number;
                    if (result >= 0) // ako je ima albuma tj broj albuma vraca true
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message.ToString());
                return false;
            }
        }

        bool IService1.IsOrderStorageEDIT(int orderID, int albumId, int number) // provera dali ima dovoljno albuma kada se edituje order
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    int numberOfPieces = (int)(from x in context.vwOrders where x.OrderID == orderID select x.NumberOfPieces).First();
                    int storage = (int)(from x in context.vwAlbums where x.AlbumID == albumId select x.Storage).First();
                    int result = (storage + numberOfPieces) - number;
                    if (result >= 0) // ako je ima albuma tj broj albuma vraca true
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message.ToString());
                return false;
            }
        }

        vwOrder IService1.AddOrder(vwOrder order) // AddOrder
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (order.OrderID == 0)
                    {   // ZA ADD 
                        tblAlbum albumToEdit = (from r in context.tblAlbums where r.AlbumID == order.AlbumID select r).First();
                        //albumToEdit.GenreID = album.GenreID;
                        //albumToEdit.ArtistID = album.ArtistID;
                        //albumToEdit.Title = album.Title;
                        //albumToEdit.Price = album.Price;
                        albumToEdit.Storage -= order.NumberOfPieces;
                        context.Entry(albumToEdit).State = EntityState.Modified;
                        context.SaveChanges();

                        tblOrder newOrder = new tblOrder();
                        newOrder.EmployeeID = order.EmployeeID;
                        newOrder.AlbumID = order.AlbumID;
                        newOrder.CustomerID = order.CustomerID;
                        newOrder.OrderDate = order.OrderDate;
                        newOrder.TotalPrice = order.TotalPrice;
                        newOrder.NumberOfPieces = order.NumberOfPieces;
                        context.tblOrders.Add(newOrder);
                        context.SaveChanges();
                        order.OrderID = newOrder.OrderID;
                        return order;
                    }
                    else
                    {   // ZA EDIT
                        int numberOfPieces = (int)(from x in context.vwOrders where x.OrderID == order.OrderID select x.NumberOfPieces).First();
                        tblAlbum albumToEdit = (from r in context.tblAlbums where r.AlbumID == order.AlbumID select r).First();
                        
                        albumToEdit.Storage = (albumToEdit.Storage + numberOfPieces) - order.NumberOfPieces;
                        context.Entry(albumToEdit).State = EntityState.Modified;
                        context.SaveChanges();

                        tblOrder orderToEdit = (from r in context.tblOrders where r.OrderID == order.OrderID select r).First();
                        orderToEdit.EmployeeID = order.EmployeeID;
                        orderToEdit.AlbumID = order.AlbumID;
                        orderToEdit.CustomerID = order.CustomerID;
                        orderToEdit.OrderDate = order.OrderDate;
                        orderToEdit.TotalPrice = order.TotalPrice;
                        orderToEdit.NumberOfPieces = order.NumberOfPieces;
                        //orderToEdit.OrderID = order.OrderID;
                        context.Entry(orderToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return order;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteOrder(int orderID) // DeleteOrder
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblOrder orderToDelete = (from r in context.tblOrders where r.OrderID == orderID select r).First();
                    context.tblOrders.Remove(orderToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA ALBUM 
        vwAlbum IService1.AddAlbum(vwAlbum album) // AddAlbum
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (album.AlbumID == 0)
                    {   // ZA ADD 
                        tblAlbum newAlbum = new tblAlbum();
                        newAlbum.GenreID = album.GenreID;
                        newAlbum.ArtistID = album.ArtistID;
                        newAlbum.Title = album.Title;
                        newAlbum.Price = album.Price;
                        newAlbum.Storage = album.Storage;
                        context.tblAlbums.Add(newAlbum);
                        context.SaveChanges();
                        album.AlbumID = newAlbum.AlbumID;
                        return album;
                    }
                    else
                    {   // ZA EDIT
                        tblAlbum albumToEdit = (from r in context.tblAlbums where r.AlbumID == album.AlbumID select r).First();
                        albumToEdit.GenreID = album.GenreID;
                        albumToEdit.ArtistID = album.ArtistID;
                        albumToEdit.Title = album.Title;
                        albumToEdit.Price = album.Price;
                        albumToEdit.Storage = album.Storage;
                        context.Entry(albumToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return album;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteAlbum(int albumID) // DeleteAlbum
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblAlbum albumToDelete = (from r in context.tblAlbums where r.AlbumID == albumID select r).First();
                    context.tblAlbums.Remove(albumToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA GENRE 
        vwGenre IService1.AddGenre(vwGenre genre) // AddGenre
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (genre.GenreID == 0)
                    {   // ZA ADD 
                        tblGenre newGenre = new tblGenre();
                        newGenre.Name = genre.Name;
                        newGenre.Description = genre.Description;
                        context.tblGenres.Add(newGenre);
                        context.SaveChanges();
                        genre.GenreID = newGenre.GenreID;
                        return genre;
                    }
                    else
                    {   // ZA EDIT
                        tblGenre genreToEdit = (from r in context.tblGenres where r.GenreID == genre.GenreID select r).First();
                        genreToEdit.Name = genre.Name;
                        genreToEdit.Description = genre.Description;
                        context.Entry(genreToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return genre;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteGenre(int genreID) // DeleteGenre
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblGenre genreToDelete = (from r in context.tblGenres where r.GenreID == genreID select r).First();
                    context.tblGenres.Remove(genreToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA ARTIST 
        vwArtist IService1.AddArtist(vwArtist artist) // AddArtist
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (artist.ArtistID == 0)
                    {   // ZA ADD 
                        tblArtist newArtist = new tblArtist();
                        newArtist.ArtistName = artist.ArtistName;
                        context.tblArtists.Add(newArtist);
                        context.SaveChanges();
                        artist.ArtistID = newArtist.ArtistID;
                        return artist;
                    }
                    else
                    {   // ZA EDIT
                        tblArtist artistToEdit = (from r in context.tblArtists where r.ArtistID == artist.ArtistID select r).First();
                        artistToEdit.ArtistName = artist.ArtistName;
                        context.Entry(artistToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return artist;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteArtist(int artistID) // DeleteArtist
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblArtist artistToDelete = (from r in context.tblArtists where r.ArtistID == artistID select r).First();
                    context.tblArtists.Remove(artistToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ADD , EDIT I DELETE ZA CUSTOMER 
        vwCustomer IService1.AddCustomer(vwCustomer customer) // AddCustomer
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    if (customer.CustomerID == 0)
                    {   // ZA ADD 
                        tblCustomer newCustomer = new tblCustomer();
                        newCustomer.Name = customer.Name;
                        newCustomer.LastName = customer.LastName;
                        newCustomer.Country = customer.Country;
                        newCustomer.Address = customer.Address;
                        newCustomer.City = customer.City;
                        newCustomer.Mobile = customer.Mobile;
                        context.tblCustomers.Add(newCustomer);
                        context.SaveChanges();
                        customer.CustomerID = newCustomer.CustomerID;
                        return customer;
                    }
                    else
                    {   // ZA EDIT
                        tblCustomer customerToEdit = (from r in context.tblCustomers where r.CustomerID == customer.CustomerID select r).First();
                        customerToEdit.Name = customer.Name;
                        customerToEdit.LastName = customer.LastName;
                        customerToEdit.Country = customer.Country;
                        customerToEdit.Address = customer.Address;
                        customerToEdit.City = customer.City;
                        customerToEdit.Mobile = customer.Mobile;
                        context.Entry(customerToEdit).State = EntityState.Modified;
                        context.SaveChanges();
                        return customer;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        void IService1.DeleteCustomer(int customerID) // DeleteCustomer
        {
            try
            {
                using (VinylRecordsShopEntities context = new VinylRecordsShopEntities())
                {
                    tblCustomer customerToDelete = (from r in context.tblCustomers where r.CustomerID == customerID select r).First();
                    context.tblCustomers.Remove(customerToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // ovo je bilo ovde i pre
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
