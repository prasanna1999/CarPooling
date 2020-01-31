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

        public double GetPrice(string pickUp, string drop, Ride ride)
        {
            List<string> viaPoints = new List<string>(ride.ViaPoints);
            viaPoints.Insert(0, ride.From);
            viaPoints.Add(ride.To);
            List<int> distances = new List<int>(ride.Distances);
            distances.Insert(0, 0);
            distances.Add(ride.Distance);
            int indexOfSource = viaPoints.IndexOf(pickUp);
            int indexOfDestination = viaPoints.IndexOf(drop);
            return (distances[indexOfDestination] - distances[indexOfSource]) * ride.Price;
        }


        public int CheckAvailableSeats(Ride ride, string pickUp, string drop, int noOfPassengers)
        {
            int noOfSeats = ride.NoOfVacentSeats;
            List<string> viaPoints = new List<string>(ride.ViaPoints);
            viaPoints.Insert(0, ride.From);
            viaPoints.Add(ride.To);
            int indexOfSource = viaPoints.IndexOf(pickUp);
            int indexOfDestination = viaPoints.IndexOf(drop);
            List<int> filledSeats = new List<int>();
            for (int i = 0; i < viaPoints.Count; i++)
                filledSeats.Add(0);
            foreach (Booking booking in ride.Bookings)
            {
                int indexOfBookedSource = viaPoints.IndexOf(booking.From);
                int indexOfBookedDestination = viaPoints.IndexOf(booking.To);
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
