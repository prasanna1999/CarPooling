using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.DataModels
{
    public class User
    {
        public User()
        {
            Rides = new List<Ride>();

            Bookings = new List<Booking>();

            Vehicles = new List<Vehicle>();
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Ride> Rides { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }
}
