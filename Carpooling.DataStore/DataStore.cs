using CarPooling.Concerns;
using System;
using System.Collections.Generic;

namespace Carpooling.DataStore
{
    public class DataStore
    {
        public static List<User> users = new List<User>();

        public static List<Ride> rides = new List<Ride>();

        public static List<Booking> bookings = new List<Booking>();

        public static List<Vehicle> vehicles = new List<Vehicle>();

        public static List<Location> locations = new List<Location>();
    }
}
