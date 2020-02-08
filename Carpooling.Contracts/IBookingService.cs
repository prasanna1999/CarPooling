﻿using CarPooling.DataModels;
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

        bool CancelBooking(string id);

        bool ModifyBooking(Booking booking, int noOfPersons, Ride ride);

        List<Booking> GetRideBookings(string rideId);

        bool ApproveBooking(string bookingId);

        void RejectBooking(string bookingId);
    }
}
