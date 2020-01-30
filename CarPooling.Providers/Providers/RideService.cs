using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class RideService : IRideService
    {
        public static List<Ride> rides = new List<Ride>();

        public void OfferRide(Ride ride)
        {
            rides.Add(ride);
        }

        public Ride GetRide(string rideId)
        {
            return rides.Find(ride => ride.Id == rideId);
        }

        public List<Ride> GetRides(User user)
        {
            return rides.FindAll(ride => ride.UserId == user.Id);
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
            IBookingService bookingService = new BookingService();
            bookingService.CancelAllRideBookings(ride.Id);
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

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> availableRides = new List<Ride>();
            foreach (Ride ride in rides)
            {
                int indexOfSource = ride.ViaPoints.IndexOf(source);
                int indexOfDestination = ride.ViaPoints.IndexOf(destination);
                int noOfSeats = CheckAvailableSeats(ride, source, destination, noOfPassengers);
                if (ride.Date.Date == date.Date && ride.Date.TimeOfDay >= date.TimeOfDay && noOfSeats >= noOfPassengers && ride.Status == RideStatus.NotYetStarted)
                {
                    if (indexOfSource == -1 || indexOfDestination == -1)
                        break;
                    else if (indexOfSource < indexOfDestination)
                    {
                        availableRides.Add(ride);
                    }
                }
            }
            return availableRides;
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
