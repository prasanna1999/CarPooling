using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarPooling.Providers
{
    public class RideService : IRideService
    {
        Concerns.CarPoolingDbContext db;
        public RideService()
        {
            db = new Concerns.CarPoolingDbContext();
        }

        public void OfferRide(Ride ride)
        {
            db.Add(ride.MapTo<Concerns.Ride>());
            db.SaveChanges();
        }

        public List<Ride> GetRides(string userId)
        {
            return db.Ride.Where(x => x.UserId == userId).ToList().MapCollectionTo<Concerns.Ride, Ride>();
        }

        public Ride GetRide(string rideId)
        {
            return db.Ride.Find(rideId).MapTo<Ride>();
        }

        public bool ModifyRide(string rideId, Ride ride)
        {
            Concerns.Ride dbRide = db.Ride.Find(rideId);
            Concerns.Ride ride1 = ride.MapTo<Concerns.Ride>();
            dbRide.Status = ride1.Status;
            dbRide.NoOfVacentSeats = ride1.NoOfVacentSeats;
            db.SaveChanges();
            return true;
        }

        public int GetPrice(string pickUp, string drop, Ride ride)
        {
            List<Location> locations = new List<Location>(ride.Locations);
            List<string> viaPoints = locations.Select(obj => obj.LocationName).ToList();
            List<int> distances = locations.Select(obj => obj.Distance).ToList();
            viaPoints.Insert(0, ride.From);
            viaPoints.Add(ride.To);
            distances.Insert(0, 0);
            distances.Add(ride.Distance);
            int indexOfSource = viaPoints.IndexOf(pickUp);
            int indexOfDestination = viaPoints.IndexOf(drop);
            return (distances[indexOfDestination] - distances[indexOfSource]) * ride.Price;
        }

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> availableRides = new List<Ride>();
            List<Ride> rides;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                rides = db.Ride.ToList().MapCollectionTo<Concerns.Ride, Ride>();
            }
            foreach (Ride ride in rides)
            {
                ILocationService locationService = new LocationService();
                ride.Locations = locationService.GetLocations(ride.Id);
                IBookingService bookingService = new BookingService();
                ride.Bookings = bookingService.GetRideBookings(ride.Id);
            }
            foreach (Ride ride in rides)
            {
                List<string> viaPoints = new List<string>(ride.Locations.Select(obj => obj.LocationName).ToList());
                viaPoints.Insert(0, ride.From);
                viaPoints.Add(ride.To);
                int indexOfSource = viaPoints.IndexOf(source);
                int indexOfDestination = viaPoints.IndexOf(destination);
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

        public int CheckAvailableSeats(Ride ride, string pickUp, string drop, int noOfPassengers)
        {
            int noOfSeats = ride.NoOfVacentSeats;
            List<string> viaPoints = new List<string>(ride.Locations.Select(obj => obj.LocationName).ToList());
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
