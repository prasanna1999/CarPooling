using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IRideService
    {
        List<Ride> GetRides(User user);

        int GetPrice(string source, string destination, Ride ride);

        void OfferRide(Ride ride, User user);

        bool ModifyRide(Ride ride, int value);

        bool CancelRide(Ride ride);

        void ChangeRideStatus(Ride ride);

        int CheckAvailableSeats(Ride ride, string source, string destination, int noOfPassengers);
    }
}
