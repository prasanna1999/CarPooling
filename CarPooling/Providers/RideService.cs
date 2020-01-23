using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    class RideService: IRideService
    {
        public void OfferRide(Ride ride,User user)
        {
            user.Rides.Add(ride);
        }

        public List<Ride> ViewRides(User user)
        {
            return user.Rides;
        }

        public void ModifyRide(Ride ride, User user, int choise,int value)
        {
            if (choise == 1)
            {
                ride.Price = value;
            }
            else if (choise == 2)
            {
                ride.NoOfVacentSeats = value;
            }
        }

        public void CancelRide(Ride ride, User user)
        {
            ride.Status = RideStatus.Cancelled;
            for(int i = 0; i < ride.Bookings.Count; i++)
            {
                ride.Bookings[i].Status = BookingStatus.Cancelled;
            }
        }

        public List<Booking> ViewRideBookings(Ride ride)
        {
            return ride.Bookings;
        }

        public bool ApproveBooking(Ride ride,Booking booking)
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
    }
}
