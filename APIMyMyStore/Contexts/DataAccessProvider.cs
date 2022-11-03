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
            _context.Users.Add(user);  
            _context.SaveChanges();  
        }  
  
        public void UpdateUserRecord(User user)  
        {  
            _context.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);  
            _context.SaveChanges();  
        }  
  
        public void DeleteUserRecord(int id)  
        {  
            var entity = _context.Users.FirstOrDefault(t => t.id == id);  
            _context.Users.Remove(entity);  
            _context.SaveChanges();  
        }  
  
        public User GetUserSingleRecord(int id)  
        {  
            return _context.Users.FirstOrDefault(t => t.id == id);  
        }  
  
        public List<User> GetUserRecords()  
        {  
            return _context.Users.ToList();  
        }  
        #endregion

        #region Admin
        public void AddAdminRecord(Admin Admin)  
        {   
            _context.Admins.Add(Admin);  
            _context.SaveChanges();  
        }  
  
        public void UpdateAdminRecord(Admin Admin)  
        {  
            _context.Entry(Admin).State = EntityState.Modified;
            _context.Admins.Update(Admin);  
            _context.SaveChanges();  
        }  
  
        public void DeleteAdminRecord(int id)  
        {  
            var entity = _context.Admins.FirstOrDefault(t => t.id == id);  
            _context.Admins.Remove(entity);  
            _context.SaveChanges();  
        }  
  
        public Admin GetAdminSingleRecord(int id)  
        {  
            return _context.Admins.FirstOrDefault(t => t.id == id);  
        }  
  
        public List<Admin> GetAdminRecords()  
        {  
            return _context.Admins.ToList();  
        }  
        #endregion
        
    }  
}  