using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling
{
    class CarPooling
    {
        public List<User> users = new List<User>();

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public User GetUser(string email)
        {
            User user = users.Find(x=>x.Email==email);
            return user;
        }

        public void AddRide(Ride ride,User user)
        {
            user.Rides.Add(ride);
        }

        public List<Ride> FindRide(string source,string destination,DateTime date)
        {
            List<Ride> rides = new List<Ride>();
            foreach(User user in users)
            {
                foreach(Ride ride in user.Rides)
                {
                    if(ride.From==source && ride.To==destination && ride.Date.Date==date.Date && ride.Date.TimeOfDay > date.TimeOfDay)
                    {
                        rides.Add(ride);
                    }
                }
            }
            return rides;
        }
    }
}
