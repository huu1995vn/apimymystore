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
    }  
}  