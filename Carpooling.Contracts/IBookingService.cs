using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IBookingService
    {
        bool AddBooking(Ride ride, User user, Booking booking);

        List<Booking> GetBookings(User user);

        bool CancelBooking(Booking booking, Ride ride);

        bool ModifyBooking(Booking booking, int noOfPersons, Ride ride);

        List<Booking> GetRideBookings(Ride ride);

        bool ApproveBooking(Ride ride, Booking booking);

        void RejectBooking(Booking booking);
    }
}
