using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class UserValidator : IUserValidator
    {
        public bool ValidateUserCredentials(User user, string password)
        {
            if (user.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
