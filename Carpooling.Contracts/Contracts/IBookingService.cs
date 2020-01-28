using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IBookingService
    {
        bool AddBooking(Ride ride, Booking booking);

        List<Booking> ViewBookings(User user);

        bool CancelBooking(Booking booking, Ride ride);

        void CancelAllRideBookings(string rideId);

        bool ModifyBooking(Booking booking, int noOfPersons, Ride ride);

        List<Booking> ViewRideBookings(Ride ride);

        bool ApproveBooking(Ride ride, Booking booking);

        void RejectBooking(Booking booking);
    }
}
