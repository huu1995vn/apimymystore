using System.Collections.Generic;
using APIMyMyStore.Entites;

namespace APIMyMyStore.DataAccess  
{  
    public interface IDataAccessProvider  
    {  
        void AddUserRecord(User user);  
        void UpdateUserRecord(User user);  
        void DeleteUserRecord(int id);  
        User GetUserSingleRecord(int id);  
        List<User> GetUserRecords();  


        void AddAdminRecord(Admin user);  
        void UpdateAdminRecord(Admin user);  
        void DeleteAdminRecord(int id);  
        Admin GetAdminSingleRecord(int id);  
        List<Admin> GetAdminRecords();
    }  
}  