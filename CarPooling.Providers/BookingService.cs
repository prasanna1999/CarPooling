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
            Ride ride;
            ride = db.Ride.Find(booking.RideId).MapTo<Ride>();
            Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
            db.Add(_booking);
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

        public bool ModifyBooking(Booking booking, int noOfPersons, Ride ride)
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
            return false;
        }

        public List<Booking> GetBookings(string userId)
        {
            return db.Booking.Where(x => x.UserId == userId).ToList().MapCollectionTo<Concerns.Booking, Booking>();

        }

        public List<Booking> GetRideBookings(string rideId)
        {
            return db.Booking.Where(x => x.RideId == rideId).ToList().MapCollectionTo<Concerns.Booking, Booking>();
        }

        public bool ApproveBooking(string bookingId)
        {
            db.Booking.Find(bookingId).Status = "Approved";
            db.SaveChanges();
            return true;
        }

        public void RejectBooking(string bookingId)
        {
            db.Booking.Find(bookingId).Status = "Rejected";
            db.SaveChanges();
        }

        public bool CancelBooking(string id)
        {
            Concerns.Booking booking = db.Booking.Find(id);
            booking.Status = "Cancelled";
            db.SaveChanges();
            return true;
        }
    }
}