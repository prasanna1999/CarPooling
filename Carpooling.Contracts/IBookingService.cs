using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IBookingService
    {
        bool AddBooking(Booking booking);

        List<Booking> GetBookings(string userId);

        void CancelAllRideBookings(string rideId);

        bool ChangeBooking(string id, Booking booking);

        List<Booking> GetRideBookings(string rideId);

    }
}
