using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IUserService
    {
        void AddUser(User user);

        User GetUser(string email);
    }
}
