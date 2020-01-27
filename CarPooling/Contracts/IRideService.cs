using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    interface IRideService
    {
        List<Ride> ViewRides(User user);

        void OfferRide(Ride ride, User user);

        bool ModifyRide(Ride ride, User user,int choise,int value);

        bool CancelRide(Ride ride, User user);

        List<Booking> ViewRideBookings(Ride ride);

        bool ApproveBooking(Ride ride,Booking booking);

        void ChangeRideStatus(Ride ride);
    }
}
