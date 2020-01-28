﻿using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IRideService
    {
        List<Ride> ViewRides(User user);

        Ride GetRide(string rideId);

        List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers);

        void OfferRide(Ride ride);

        bool ModifyRide(Ride ride, int choise, int value);

        bool CancelRide(Ride ride);

        void ChangeRideStatus(Ride ride);
    }
}
