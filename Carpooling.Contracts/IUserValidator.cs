using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IUserValidator
    {
        bool ValidateUserCredentials(User user, string password);
    }
}
