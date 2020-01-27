using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class RideService : IRideService
    {
        public void OfferRide(Ride ride, User user)
        {
            user.Rides.Add(ride);
        }

        public List<Ride> ViewRides(User user)
        {
            return user.Rides;
        }

        public bool ModifyRide(Ride ride, User user, int choise, int value)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            if (choise == 1)
            {
                ride.Price = value;
            }
            else if (choise == 2)
            {
                ride.NoOfVacentSeats = value;
            }
            return true;
        }

        public bool CancelRide(Ride ride, User user)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            ride.Status = RideStatus.Cancelled;
            for (int i = 0; i < ride.Bookings.Count; i++)
            {
                ride.Bookings[i].Status = BookingStatus.Cancelled;
            }
            return true;
        }

        public List<Booking> ViewRideBookings(Ride ride)
        {
            return ride.Bookings;
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

        public void ChangeRideStatus(Ride ride)
        {
            if (ride.Date < DateTime.Now && ride.Status == RideStatus.NotYetStarted)
            {
                ride.Status = RideStatus.Completed;
            }
        }
    }
}
