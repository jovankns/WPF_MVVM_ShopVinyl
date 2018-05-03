using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFShopVinyl
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        //tblOffice
        [OperationContract]
        List<vwUser> GetAllUser();

        //tblOffice
        [OperationContract]
        List<vwOffice> GetAllOffice(string search);

        //tblBoss
        [OperationContract]
        List<vwBoss> GetAllBoss(string search);

        //tblEmployee
        [OperationContract]
        List<vwEmployee> GetAllEmployee(string search);

        //tblOrder 
        [OperationContract]
        List<vwOrder> GetAllOrder(string search);

        //tblAlbum
        [OperationContract]
        List<vwAlbum> GetAllAlbum(string search);

        //tblGenre
        [OperationContract]
        List<vwGenre> GetAllGenre(string search);

        //tblArtist
        [OperationContract]
        List<vwArtist> GetAllArtist(string search);

        //tblCustomer
        [OperationContract]
        List<vwCustomer> GetAllCustomer(string search);

        // OFFICE ADD I DELETE i bool check za DELETE
        [OperationContract]
        vwOffice AddOffice(vwOffice office);

        [OperationContract]
        void DeleteOffice(int officeID);

        //tbl Office delete office check za DELETE
        [OperationContract]
        bool IsOfficeID(int officeId);

        // BOSS ADD I DELETE i bool check za BOSS
        [OperationContract]
        vwBoss AddBoss(vwBoss boss);

        [OperationContract]
        void DeleteBoss(int bossID);
        //tbl Boss delete boos check za DELETE ne treba
        //[OperationContract]
        //bool IsBossID(int bossId);

        // EMPLOYEE ADD I DELETE
        [OperationContract]
        vwEmployee AddEmployee(vwEmployee employee);

        [OperationContract]
        void DeleteEmployee(int employeeID);

        // ORDER ADD I DELETE
        [OperationContract]
        vwOrder AddOrder(vwOrder order);

        [OperationContract]
        void DeleteOrder(int orderID);
        //Check dali albuma ima na stanju za ADD ORDER u SAVE Command
        [OperationContract]
        bool IsOrderStorage(int albumId,int number);

        //Check dali albuma ima na stanju za EDIT ORDER u SAVE Command
        [OperationContract]
        bool IsOrderStorageEDIT(int orderID, int albumId, int number);

        // ALBUM ADD I DELETE
        [OperationContract]
        vwAlbum AddAlbum(vwAlbum album);

        [OperationContract]
        void DeleteAlbum(int albumID);

        // GENRE ADD I DELETE
        [OperationContract]
        vwGenre AddGenre(vwGenre genre);

        [OperationContract]
        void DeleteGenre(int genreID);

        // ARTIST ADD I DELETE
        [OperationContract]
        vwArtist AddArtist(vwArtist artist);

        [OperationContract]
        void DeleteArtist(int artistID);

        // CUSTOMER ADD I DELETE
        [OperationContract]
        vwCustomer AddCustomer(vwCustomer customer);

        [OperationContract]
        void DeleteCustomer(int customerID);

        // OVO NE DIRATI
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
