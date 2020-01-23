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
        
        public List<Ride> FindRide(string source,string destination,DateTime date,int noOfPassengers)
        {
            List<Ride> rides = new List<Ride>();
            foreach(User user in users)
            {
                foreach(Ride ride in user.Rides)
                {
                    if(ride.From==source && ride.To==destination && ride.Date.Date==date.Date && ride.Date.TimeOfDay > date.TimeOfDay && ride.NoOfVacentSeats>=noOfPassengers && ride.Status==RideStatus.NotYetStarted)
                    {
                        rides.Add(ride);
                    }
                }
            }
            return rides;
        }
    }
}
