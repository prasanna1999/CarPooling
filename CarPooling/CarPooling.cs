using CarPooling.Contracts;
using CarPooling.DataModels;
using CarPooling.Providers;
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

        public Ride GetRide(string rideId)
        {
            Ride ride = null;
            users.ForEach(user =>
            {
                foreach (Ride _ride in user.Rides)
                {
                    if (_ride.Id == rideId)
                        ride = _ride;
                }
            });
            return ride;
        }

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> availableRides = new List<Ride>();
            foreach (User user in users)
            {
                foreach (Ride ride in user.Rides)
                {
                    List<string> viaPoints = new List<string>(ride.ViaPoints);
                    viaPoints.Insert(0, ride.From);
                    viaPoints.Add(ride.To);
                    int indexOfSource = viaPoints.IndexOf(source);
                    int indexOfDestination = viaPoints.IndexOf(destination);
                    IRideService rideService = new RideService();
                    int noOfSeats = rideService.CheckAvailableSeats(ride, source, destination, noOfPassengers);
                    if (ride.Date.Date == date.Date && ride.Date.TimeOfDay >= date.TimeOfDay && noOfSeats >= noOfPassengers && ride.Status == RideStatus.NotYetStarted)
                    {
                        if (indexOfSource == -1 || indexOfDestination == -1)
                            break;
                        else if (indexOfSource < indexOfDestination)
                        {
                            availableRides.Add(ride);
                        }
                    }
                }
            }
            return availableRides;
        }
    }
}
