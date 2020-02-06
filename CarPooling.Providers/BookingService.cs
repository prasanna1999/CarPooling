using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Carpooling.DataStore;
using System.Linq;

namespace CarPooling.Providers
{
    public class BookingService : IBookingService
    {
        public bool AddBooking(Booking booking)
        {
            Ride ride;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                ride = db.Ride.Find(booking.RideId).MapTo<Ride>();
            }

            Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
            using (var db = new Concerns.CarPoolingDbContext())
            {
                db.Add(_booking);
                db.SaveChanges();
            }
            return true;
        }

        public void CancelAllRideBookings(string rideId)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                foreach (Concerns.Booking booking in db.Booking)
                {
                    if (booking.RideId == rideId)
                    {
                        booking.Status = "Cancelled";
                    }
                }
                db.SaveChanges();
            }
        }

        public bool CancelBooking(Booking booking)
        {
            if (booking.Status == BookingStatus.Cancelled)
                return false;
            using (var db = new Concerns.CarPoolingDbContext())
            {
                Concerns.Booking _booking = db.Booking.Find(booking.Id);
                _booking.Status = "Cancelled";
                db.SaveChanges();
            }
            return true;
        }

        public bool ModifyBooking(Booking booking, int noOfPersons, Ride ride)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                Concerns.Booking _booking = db.Booking.Find(booking.Id);
                IRideService rideService = new RideService();
                int noOfSeats = rideService.CheckAvailableSeats(ride, booking.From, booking.To, booking.NoOfPersons);
                if (noOfSeats + booking.NoOfPersons >= noOfPersons && booking.Status == BookingStatus.Approved)
                {
                    if (ride.Type == BookingType.AutoApproval)
                    {
                        _booking.Status = "Approved";
                    }
                    else
                    {
                        _booking.Status = "Pending";
                    }
                    _booking.NoOfPersons = noOfPersons;
                    return true;
                }
                else if (booking.Status == BookingStatus.Pending)
                {
                    if (noOfSeats >= noOfPersons)
                    {
                        _booking.Status = "Pending";
                        _booking.NoOfPersons = noOfPersons;
                        return true;
                    }
                }
                db.SaveChanges();
            }
            return false;
        }

        public List<Booking> GetBookings(string userId)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                return db.Booking.Where(x => x.UserId == userId).ToList().MapCollectionTo<Concerns.Booking, Booking>();
            }
        }

        public List<Booking> GetRideBookings(string rideId)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                return db.Booking.Where(x => x.RideId == rideId).ToList().MapCollectionTo<Concerns.Booking, Booking>();
            }
        }

        public bool ApproveBooking(Ride ride, Booking booking)
        {
            IRideService rideService = new RideService();
            int noOfSeats = rideService.CheckAvailableSeats(ride, booking.From, booking.To, booking.NoOfPersons);
            if (noOfSeats >= booking.NoOfPersons)
            {
                using (var db = new Concerns.CarPoolingDbContext())
                {
                    Concerns.Booking _booking = db.Booking.Find(booking.Id);
                    _booking.Status = "Approved";
                    db.SaveChanges();
                }
                return true;
            }
            else
                return false;
        }

        public void RejectBooking(Booking booking)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                Concerns.Booking _booking = db.Booking.Find(booking.Id);
                _booking.Status = "Rejected";
                db.SaveChanges();
            }
        }
    }
}