using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class BookingService : IBookingService
    {
        public static List<Booking> bookings = new List<Booking>();

        public bool AddBooking(Ride ride, Booking booking)
        {
            if (ride.UserId != booking.UserId)
            {
                if (ride.Type == BookingType.AutoApproval)
                {
                    ride.NoOfVacentSeats = ride.NoOfVacentSeats - booking.NoOfPersons;
                    booking.Status = BookingStatus.Approved;
                }
                else
                {
                    booking.Status = BookingStatus.Pending;
                }
                bookings.Add(booking);
                return true;
            }
            return false;
        }

        public void CancelAllRideBookings(string rideId)
        {
            foreach (Booking booking in bookings)
            {
                if (booking.RideId == rideId)
                {
                    booking.Status = BookingStatus.Cancelled;
                }
            }
        }

        public bool CancelBooking(Booking booking, Ride ride)
        {
            if (booking.Status == BookingStatus.Cancelled)
                return false;
            if (booking.Status == BookingStatus.Approved)
            {
                ride.NoOfVacentSeats = ride.NoOfVacentSeats + booking.NoOfPersons;
            }
            booking.Status = BookingStatus.Cancelled;
            return true;
        }

        public bool ModifyBooking(Booking booking, int noOfPersons, Ride ride)
        {
            if (ride.NoOfVacentSeats + booking.NoOfPersons >= noOfPersons && booking.Status == BookingStatus.Approved)
            {
                if (ride.Type == BookingType.AutoApproval)
                    ride.NoOfVacentSeats = ride.NoOfVacentSeats + booking.NoOfPersons - noOfPersons;
                else
                {
                    booking.Status = BookingStatus.Pending;
                }
                booking.NoOfPersons = noOfPersons;
                return true;
            }
            else if (booking.Status == BookingStatus.Pending)
            {
                if (ride.NoOfVacentSeats >= noOfPersons)
                {
                    booking.Status = BookingStatus.Pending;
                    booking.NoOfPersons = noOfPersons;
                    return true;
                }
            }
            return false;
        }

        public List<Booking> ViewBookings(User user)
        {
            return bookings.FindAll(booking => booking.UserId == user.Id);
        }

        public List<Booking> ViewRideBookings(Ride ride)
        {
            return bookings.FindAll(booking => booking.RideId == ride.Id);
        }

        public bool ApproveBooking(Ride ride, Booking booking)
        {
            if (ride.NoOfVacentSeats >= booking.NoOfPersons)
            {
                booking.Status = BookingStatus.Approved;
                ride.NoOfVacentSeats = ride.NoOfVacentSeats - booking.NoOfPersons;
                return true;
            }
            else
                return false;
        }

        public void RejectBooking(Booking booking)
        {
            booking.Status = BookingStatus.Rejected;
        }
    }
}
