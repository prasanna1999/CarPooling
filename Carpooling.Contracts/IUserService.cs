using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Contracts
{
    public interface IUserService
    {
        void AddUser(User user);

        User GetUser(string email);

        Task<User> Authenticate(string username, string password);
    }
}
