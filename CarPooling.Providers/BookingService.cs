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
        Concerns.CarPoolingDbContext db;
        public BookingService()
        {
            db = new Concerns.CarPoolingDbContext();
        }

        public bool AddBooking(Booking booking)
        {
            db.Add(booking.MapTo<Concerns.Booking>());
            db.SaveChanges();
            return true;
        }

        public List<Booking> GetBookings(string userId)
        {
            return db.Booking.Where(x => x.UserId == userId).ToList().MapCollectionTo<Concerns.Booking, Booking>();
        }

        public List<Booking> GetRideBookings(string rideId)
        {
            return db.Booking.Where(x => x.RideId == rideId).ToList().MapCollectionTo<Concerns.Booking, Booking>();
        }

        public bool ChangeBooking(string bookingId, Booking booking)
        {
            Concerns.Booking dbBooking = db.Booking.Find(bookingId);
            Concerns.Booking booking1 = booking.MapTo<Concerns.Booking>();
            dbBooking.Status = booking1.Status;
            dbBooking.NoOfPersons = booking1.NoOfPersons;
            db.SaveChanges();
            return true;
        }

        public void CancelAllRideBookings(string rideId)
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
}