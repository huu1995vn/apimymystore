using System.Collections.Generic;  
using System.Linq;
using APIMyMyStore.Entites;
using Microsoft.EntityFrameworkCore;

namespace APIMyMyStore.DataAccess  
{  
    public class DataAccessProvider: IDataAccessProvider  
    {  
        private readonly PostgreSqlContext _context;  
  
        public DataAccessProvider(PostgreSqlContext context)  
        {  
            _context = context;  
        }  
        #region User
        public void AddUserRecord(User user)  
        {   
            _context.users.Add(user);  
            _context.SaveChanges();  
        }  
  
        public void UpdateUserRecord(User user)  
        {  
            _context.Entry(user).State = EntityState.Modified;
            _context.users.Update(user);  
            _context.SaveChanges();  
        }  
  
        public void DeleteUserRecord(int id)  
        {  
            var entity = _context.users.FirstOrDefault(t => t.id == id);  
            _context.users.Remove(entity);  
            _context.SaveChanges();  
        }  
  
        public User GetUserSingleRecord(int id)  
        {  
            return _context.users.FirstOrDefault(t => t.id == id);  
        }  
  
        public List<User> GetUserRecords()  
        {  
            return _context.users.ToList();  
        }  
        #endregion

        #region Admin
        public void AddAdminRecord(Admin Admin)  
        {   
            _context.admins.Add(Admin);  
            _context.SaveChanges();  
        }  
  
        public void UpdateAdminRecord(Admin Admin)  
        {  
            _context.Entry(Admin).State = EntityState.Modified;
            _context.admins.Update(Admin);  
            _context.SaveChanges();  
        }  
  
        public void DeleteAdminRecord(int id)  
        {  
            var entity = _context.admins.FirstOrDefault(t => t.id == id);  
            _context.admins.Remove(entity);  
            _context.SaveChanges();  
        }  
  
        public Admin GetAdminSingleRecord(int id)  
        {  
            return _context.admins.FirstOrDefault(t => t.id == id);  
        }  
  
        public List<Admin> GetAdminRecords()  
        {  
            return _context.admins.ToList();  
        }  
        #endregion
        
    }  
}  