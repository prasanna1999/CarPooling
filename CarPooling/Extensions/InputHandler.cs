using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Extensions
{
    class InputHandler
    {
        public int GetInt()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Please enter numbers only");
            }
            return result;
        }

        public double GetDouble()
        {
            double result;
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Please enter numbers only");
            }
            return result;
        }

        public string GetString()
        {
            string result = Console.ReadLine();
            return result;
        }

        public DateTime GetDate()
        {
            DateTime date;
            while (!DateTime.TryParseExact(Console.ReadLine(), "M/d/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Please enter correct date");
            }
            return date;
        }

    }
}
