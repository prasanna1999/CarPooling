using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace CarPooling.Extensions
{
    class InputValidator
    {
        public bool ValidateName(string name)
        {
            if (name.Length < 3)
            {
                Console.WriteLine("Please enter name with minimum 3 characters..");
                return true;
            }
            return false;
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (Regex.Match(phoneNumber, @"^([0-9]{10})$").Success)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Please enter valid phone number..");
                return true;
            }
        }

        public bool ValidateEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return false;
            }
            catch
            {
                Console.WriteLine("Please enter valid email");
                return true;
            }
        }

        public bool ValidatePassword(string password)
        {
            if (password.Length < 4)
            {
                Console.WriteLine("Please enter password with minimum 4 characters..");
                return true;
            }
            return false;
        }

        public bool ValidateArea(string area)
        {
            if (area.Length < 1)
            {
                Console.WriteLine("Please enter minimum 1 character");
                return true;
            }
            return false;
        }

        public bool ValidateDate(DateTime date)
        {
            if (date < DateTime.Now)
            {
                Console.WriteLine("Please enter valid date");
                return true;
            }
            return false;
        }

        public bool ValidateEndDate(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                Console.WriteLine("Please enter valid end date");
                return true;
            }
            return false;
        }
    }
}
