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

        public bool CancelBooking(Booking booking,Ride ride)
        {
            if (booking.Status == BookingStatus.Cancelled)
                return false;
            if (booking.Status == BookingStatus.Approved)
                ride.NoOfVacentSeats = ride.NoOfVacentSeats - booking.NoOfPersons;
            booking.Status = BookingStatus.Cancelled;
            return true;
            
        }

        public bool ModifyBooking(Booking booking, int noOfPersons, Ride ride)
        {
            if (ride.NoOfVacentSeats + booking.NoOfPersons >= noOfPersons)
            {
                if(ride.Type==BookingType.AutoApproval)
                    ride.NoOfVacentSeats = ride.NoOfVacentSeats + booking.NoOfPersons - noOfPersons;
                else
                {
                    booking.Status = BookingStatus.Pending;
                }
                booking.NoOfPersons = noOfPersons;
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
