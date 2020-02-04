using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Carpooling.DataStore;

namespace CarPooling.Providers
{
    public class RideService : IRideService
    {
        public void OfferRide(Ride ride)
        {
            Concerns.Ride _ride = ride.MapTo<Concerns.Ride>();
            DataStore.rides.Add(_ride);
            for (int i = 0; i < ride.ViaPoints.Count; i++)
            {
                Concerns.Location location = new Concerns.Location();
                location.Id = ride.Id + ride.ViaPoints[i];
                location.LocationName = ride.ViaPoints[i];
                location.Distance = ride.Distances[i];
                location.RideId = ride.Id;
                DataStore.locations.Add(location);
            }
        }

        public List<Ride> GetRides(string userId)
        {
            List<Concerns.Ride> _rides = DataStore.rides.FindAll(ride => ride.UserId == userId);
            List<Ride> rides = _rides.MapCollectionTo<Concerns.Ride, Ride>();
            foreach (Ride ride in rides)
            {
                List<Concerns.Location> locations = DataStore.locations.FindAll(location => location.RideId == ride.Id);
                ride.ViaPoints = new List<string>();
                ride.Distances = new List<int>();
                IBookingService bookingService = new BookingService();
                ride.Bookings = bookingService.GetRideBookings(ride.Id);
                foreach (Concerns.Location location in locations)
                {
                    ride.ViaPoints.Add(location.LocationName);
                    ride.Distances.Add(location.Distance);
                }
            }
            return rides;
        }

        public Ride GetRide(string rideId)
        {
            Concerns.Ride _ride = DataStore.rides.Find(x => x.Id == rideId);
            Ride ride = _ride.MapTo<Ride>();
            List<Concerns.Location> locations = DataStore.locations.FindAll(location => location.RideId == ride.Id);
            ride.ViaPoints = new List<string>();
            ride.Distances = new List<int>();
            IBookingService bookingService = new BookingService();
            ride.Bookings = bookingService.GetRideBookings(rideId);
            foreach (Concerns.Location location in locations)
            {
                ride.ViaPoints.Add(location.LocationName);
                ride.Distances.Add(location.Distance);
            }
            return ride;
        }

        public bool ModifyRide(Ride ride, int value)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            Concerns.Ride _ride = ride.MapTo<Concerns.Ride>();
            int index = DataStore.rides.FindIndex(x => x.Id == _ride.Id);
            DataStore.rides[index].NoOfVacentSeats = value;
            return true;
        }

        public bool CancelRide(Ride ride)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            Concerns.Ride _ride = ride.MapTo<Concerns.Ride>();
            int index = DataStore.rides.FindIndex(x => x.Id == _ride.Id);
            DataStore.rides[index].Status = "Cancelled";
            IBookingService bookingService = new BookingService();
            bookingService.CancelAllRideBookings(ride.Id);
            return true;
        }

        public void ChangeRideStatus(Ride ride)
        {
            if (ride.Date < DateTime.Now && ride.Status == RideStatus.NotYetStarted)
            {
                Concerns.Ride _ride = ride.MapTo<Concerns.Ride>();
                int index = DataStore.rides.FindIndex(x => x.Id == _ride.Id);
                DataStore.rides[index].Status = "Completed";
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

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> availableRides = new List<Ride>();
            List<Ride> rides = DataStore.rides.MapCollectionTo<Concerns.Ride, Ride>();
            foreach (Ride ride in rides)
            {
                List<Concerns.Location> locations = DataStore.locations.FindAll(location => location.RideId == ride.Id);
                ride.ViaPoints = new List<string>();
                ride.Distances = new List<int>();
                foreach (Concerns.Location location in locations)
                {
                    ride.ViaPoints.Add(location.LocationName);
                    ride.Distances.Add(location.Distance);
                }
            }
            foreach (Ride ride in rides)
            {
                List<string> viaPoints = new List<string>(ride.ViaPoints);
                viaPoints.Insert(0, ride.From);
                viaPoints.Add(ride.To);
                int indexOfSource = viaPoints.IndexOf(source);
                int indexOfDestination = viaPoints.IndexOf(destination);
                IRideService rideService = new RideService();
                int noOfSeats = rideService.CheckAvailableSeats(ride, source, destination, noOfPassengers);
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
