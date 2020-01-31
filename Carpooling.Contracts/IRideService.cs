using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IRideService
    {
        List<Ride> GetRides(User user);

        double GetPrice(string pickUp, string drop, Ride ride);

        void OfferRide(Ride ride, User user);

        bool ModifyRide(Ride ride, int value);

        bool CancelRide(Ride ride);

        void ChangeRideStatus(Ride ride);

        int CheckAvailableSeats(Ride ride, string pickUp, string drop, int noOfPassengers);
    }
}
