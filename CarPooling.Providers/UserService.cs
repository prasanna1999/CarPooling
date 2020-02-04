using Carpooling.DataStore;
using CarPooling.Contracts;
using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class UserService : IUserService
    {
        public void AddUser(User user)
        {
            Concerns.User _user = user.MapTo<Concerns.User>();
            DataStore.users.Add(_user);
        }

        public User GetUser(string email)
        {
            Concerns.User user = DataStore.users.Find(x => x.Email == email);
            return user.MapTo<User>();
        }

    }
}
