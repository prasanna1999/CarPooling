﻿using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carpooling.DataStore;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Providers
{
    public class RideService : IRideService
    {
        public void OfferRide(Ride ride)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                db.Add(ride.MapTo<Concerns.Ride>());
                db.SaveChanges();
            }
        }

        public List<Ride> GetRides(string userId)
        {
            List<Concerns.Ride> rides;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                rides = db.Ride.Where(x => x.UserId == userId).ToList();
            }
            return rides.MapCollectionTo<Concerns.Ride, Ride>();   
        }

        public Ride GetRide(string rideId)
        {
            Concerns.Ride ride;
            using(var db = new Concerns.CarPoolingDbContext())
            {
                ride = db.Ride.Find(rideId);
            }
            return ride.MapTo<Ride>();
        }

        public bool ModifyRide(Ride ride, int value)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                Concerns.Ride _ride = db.Ride.Find(ride.Id);
                _ride.NoOfVacentSeats = value;
                db.SaveChanges();
            }
            return true;
        }

        public bool CancelRide(Ride ride)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                Concerns.Ride _ride = db.Ride.Find(ride.Id);
                _ride.Status = "Cancelled";
                db.SaveChanges();
            }
            return true;
        }

        public void ChangeRideStatus(Ride ride)
        {
            if (ride.Date < DateTime.Now && ride.Status == RideStatus.NotYetStarted)
            {
                using (var db = new Concerns.CarPoolingDbContext())
                {
                    Concerns.Ride _ride = db.Ride.Find(ride.Id);
                    _ride.Status = "Completed";
                    db.SaveChanges();
                }
            }
        }

        public double GetPrice(string pickUp, string drop, Ride ride)
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
                ride.Locations=locationService.GetLocations(ride.Id);
                IBookingService bookingService = new BookingService();
                ride.Bookings = bookingService.GetRideBookings(ride.Id);
            }
            foreach (Ride ride in rides)
            {
                List<string> viaPoints = new List<string>(ride.Locations.Select(obj=> obj.LocationName).ToList());
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
