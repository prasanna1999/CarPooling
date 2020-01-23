using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class BookingService:IBookingService
    {

        public bool AddBooking(Ride ride, User user, Booking booking)
        {
            if (ride.UserId != booking.UserId)
            {
                if (ride.Type == BookingType.AutoApproval)
                {
                    booking.Status = BookingStatus.Approved;
                    ride.NoOfVacentSeats = ride.NoOfVacentSeats - booking.NoOfPersons;
                }
                else
                {
                    booking.Status = BookingStatus.Pending;
                }
                ride.Bookings.Add(booking);
                user.Bookings.Add(booking);
                return true;
            }
            return false;
        }

        public List<Booking> ViewBookings(User user)
        {
            List<Booking> bookings = user.Bookings;
            return bookings;
        }
    }
}
