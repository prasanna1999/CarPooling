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

        void ModifyRide(Ride ride, User user,int choise,int value);

        void CancelRide(Ride ride, User user);

        List<Booking> ViewRideBookings(Ride ride);

        bool ApproveBooking(Ride ride,Booking booking);
    }
}
