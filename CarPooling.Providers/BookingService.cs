using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Carpooling.DataStore;

namespace CarPooling.Providers
{
    public class BookingService : IBookingService
    {
        public bool AddBooking(Ride ride, Booking booking)
        {
            if (ride.UserId != booking.UserId)
            {
                if (ride.Type == BookingType.AutoApproval)
                {

                    booking.Status = BookingStatus.Approved;
                }
                else
                {
                    booking.Status = BookingStatus.Pending;
                }
                Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
                DataStore.bookings.Add(_booking);
                return true;
            }
            return false;
        }

        public void CancelAllRideBookings(string rideId)
        {
            foreach (Concerns.Booking booking in DataStore.bookings)
            {
                if (booking.RideId == rideId)
                {
                    booking.Status = "Cancelled";
                }
            }
        }

        public bool CancelBooking(Booking booking)
        {
            if (booking.Status == BookingStatus.Cancelled)
                return false;
            Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
            int index = DataStore.bookings.FindIndex(x => x.Id == _booking.Id);
            DataStore.bookings[index].Status = "Cancelled";
            return true;
        }

        public bool ModifyBooking(Booking booking, int noOfPersons, Ride ride)
        {
            Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
            int index = DataStore.bookings.FindIndex(x => x.Id == _booking.Id);
            IRideService rideService = new RideService();
            int noOfSeats = rideService.CheckAvailableSeats(ride, booking.From, booking.To, booking.NoOfPersons);
            if (noOfSeats + booking.NoOfPersons >= noOfPersons && booking.Status == BookingStatus.Approved)
            {
                if (ride.Type == BookingType.AutoApproval)
                {
                    DataStore.bookings[index].Status = "Approved";
                }
                else
                {
                    DataStore.bookings[index].Status = "Pending";
                }
                DataStore.bookings[index].NoOfPersons = noOfPersons;
                return true;
            }
            else if (booking.Status == BookingStatus.Pending)
            {
                if (noOfSeats >= noOfPersons)
                {
                    DataStore.bookings[index].Status = "Pending";
                    DataStore.bookings[index].NoOfPersons = noOfPersons;
                    return true;
                }
            }
            return false;
        }

        public List<Booking> GetBookings(string userId)
        {
            List<Concerns.Booking> bookings = DataStore.bookings.FindAll(booking => booking.UserId == userId);
            return bookings.MapCollectionTo<Concerns.Booking, Booking>();
        }

        public List<Booking> GetRideBookings(string rideId)
        {
            List<Concerns.Booking> bookings = DataStore.bookings.FindAll(booking => booking.RideId == rideId);
            return bookings.MapCollectionTo<Concerns.Booking, Booking>();
        }

        public bool ApproveBooking(Ride ride, Booking booking)
        {
            IRideService rideService = new RideService();
            int noOfSeats = rideService.CheckAvailableSeats(ride, booking.From, booking.To, booking.NoOfPersons);
            if (noOfSeats >= booking.NoOfPersons)
            {
                Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
                int index=DataStore.bookings.FindIndex(x=>x.Id ==_booking.Id);
                DataStore.bookings[index].Status="Approved";
                return true;
            }
            else
                return false;
        }

        public void RejectBooking(Booking booking)
        {
            Concerns.Booking _booking = booking.MapTo<Concerns.Booking>();
            int index = DataStore.bookings.FindIndex(x => x.Id == _booking.Id);
            DataStore.bookings[index].Status = "Rejected";
        }
    }
}