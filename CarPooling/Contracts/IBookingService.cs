using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IBookingService
    {
        bool AddBooking(Ride ride,User user,Booking booking);

        List<Booking> ViewBookings(User user);

        bool CancelBooking(Booking booking,Ride ride);

        bool ModifyBooking(Booking booking, int noOfPersons, Ride ride);
    }
}
