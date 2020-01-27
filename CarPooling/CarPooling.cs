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
            User user = users.Find(x => x.Email == email);
            return user;
        }

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> rides = new List<Ride>();
            foreach (User user in users)
            {
                foreach (Ride ride in user.Rides)
                {
                    if (ride.Date.Date == date.Date && ride.Date.TimeOfDay >= date.TimeOfDay && ride.NoOfVacentSeats >= noOfPassengers && ride.Status == RideStatus.NotYetStarted)
                    {

                        int indexOfSource = ride.ViaPoints.IndexOf(source);
                        int indexOfDestination = ride.ViaPoints.IndexOf(destination);
                        if (indexOfSource == -1 || indexOfDestination == -1)
                            break;
                        else if (indexOfSource < indexOfDestination)
                            rides.Add(ride);

                    }
                }
            }
            return rides;
        }

        public Ride GetRide(string rideId)
        {
            foreach (User user in users)
            {
                foreach (Ride ride in user.Rides)
                {
                    if (ride.Id == rideId)
                        return ride;
                }
            }
            return null;
        }
    }
}
