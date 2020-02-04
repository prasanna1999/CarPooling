using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IRideService
    {
        List<Ride> GetRides(string userId);

        double GetPrice(string pickUp, string drop, Ride ride);

        void OfferRide(Ride ride);

        bool ModifyRide(Ride ride, int value);

        bool CancelRide(Ride ride);

        void ChangeRideStatus(Ride ride);

        Ride GetRide(string rideId);

        List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers);

        int CheckAvailableSeats(Ride ride, string pickUp, string drop, int noOfPassengers);
    }
}
