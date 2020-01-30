using CarPooling.DataModels;
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

        public List<Ride> GetRides(User user)
        {
            return user.Rides;
        }

        public bool ModifyRide(Ride ride, int value)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            ride.NoOfVacentSeats = value;
            return true;
        }

        public bool CancelRide(Ride ride)
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

        public void ChangeRideStatus(Ride ride)
        {
            if (ride.Date < DateTime.Now && ride.Status == RideStatus.NotYetStarted)
            {
                ride.Status = RideStatus.Completed;
            }
        }

        public int GetPrice(string source, string destination, Ride ride)
        {
            int indexOfSource = ride.ViaPoints.IndexOf(source);
            int indexOfDestination = ride.ViaPoints.IndexOf(destination);
            return (ride.Distances[indexOfDestination] - ride.Distances[indexOfSource]) * 3;
        }


        public int CheckAvailableSeats(Ride ride, string source, string destination, int noOfPassengers)
        {
            int noOfSeats = ride.NoOfVacentSeats;
            IBookingService bookingService = new BookingService();
            List<Booking> bookings = bookingService.GetRideBookings(ride);
            int indexOfSource = ride.ViaPoints.IndexOf(source);
            int indexOfDestination = ride.ViaPoints.IndexOf(destination);
            List<int> filledSeats = new List<int>();
            for (int i = 0; i < ride.ViaPoints.Count; i++)
                filledSeats.Add(0);
            foreach (Booking booking in bookings)
            {
                int indexOfBookedSource = ride.ViaPoints.IndexOf(booking.From);
                int indexOfBookedDestination = ride.ViaPoints.IndexOf(booking.To);
                for (int i = indexOfBookedSource; i < indexOfBookedDestination; i++)
                {
                    if (booking.Status == BookingStatus.Approved)
                    {
                        filledSeats[i] = filledSeats[i] + booking.NoOfPersons;
                    }
                }
            }
            for (int i = indexOfSource; i < indexOfDestination; i++)
            {
                if (ride.NoOfVacentSeats - filledSeats[i] < noOfPassengers)
                {
                    noOfSeats = 0;
                    break;
                }
            }
            return noOfSeats;
        }
    }
}
