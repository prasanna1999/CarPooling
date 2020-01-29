using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IRideService
    {
        List<Ride> GetRides(User user);

        Ride GetRide(string rideId);

        List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers);

        void OfferRide(Ride ride);

        bool ModifyRide(Ride ride, int choise, int value);

        bool CancelRide(Ride ride);

        void ChangeRideStatus(Ride ride);

        int CheckAvailableSeats(Ride ride, string source, string destination, int noOfPassengers);
    }
}
