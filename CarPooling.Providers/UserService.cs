using Carpooling.DataStore;
using CarPooling.Contracts;
using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarPooling.Providers
{
    public class UserService : IUserService
    {
        public void AddUser(User user)
        {
            Concerns.User _user = user.MapTo<Concerns.User>();
            using(var db = new Concerns.CarPoolingDbContext())
            {
                db.Add(_user);
                db.SaveChanges();
            }
        }

        public User GetUser(string email)
        {
            Concerns.User user;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                user = db.User.Where(x=>x.Email==email).SingleOrDefault();
            }
            return user.MapTo<User>();
        }

    }
}
