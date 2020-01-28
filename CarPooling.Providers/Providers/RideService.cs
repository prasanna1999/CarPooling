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

        public List<Ride> ViewRides(User user)
        {
            return rides.FindAll(ride => ride.UserId == user.Id);
        }

        public bool ModifyRide(Ride ride, int choise, int value)
        {
            if (ride.Status != RideStatus.NotYetStarted)
                return false;
            if (choise == 1)
            {
                ride.Price = value;
            }
            else if (choise == 2)
            {
                ride.NoOfVacentSeats = value;
            }
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

        public List<Ride> FindRide(string source, string destination, DateTime date, int noOfPassengers)
        {
            List<Ride> availableRides = new List<Ride>();
            foreach (Ride ride in rides)
            {
                int noOfSeats = ride.NoOfVacentSeats;
                IBookingService bookingService = new BookingService();
                List<Booking> bookings = bookingService.ViewRideBookings(ride);
                int indexOfSource = ride.ViaPoints.IndexOf(source);
                int indexOfDestination = ride.ViaPoints.IndexOf(destination);
                foreach(Booking booking in bookings)
                {
                    int indexOfBookedSource = ride.ViaPoints.IndexOf(booking.From);
                    int indexOfBookedDestination = ride.ViaPoints.IndexOf(booking.To);
                    if ((indexOfBookedSource <= indexOfSource && indexOfBookedDestination <= indexOfSource) || (indexOfBookedSource >= indexOfDestination && indexOfBookedDestination >= indexOfDestination))
                        continue;
                    noOfSeats = noOfSeats - booking.NoOfPersons;
                }
                if (ride.Date.Date == date.Date && ride.Date.TimeOfDay >= date.TimeOfDay && noOfSeats >= noOfPassengers && ride.Status == RideStatus.NotYetStarted)
                {
                    if (indexOfSource == -1 || indexOfDestination == -1)
                        break;
                    else if (indexOfSource < indexOfDestination)
                        availableRides.Add(ride);
                }
            }
            return availableRides;
        }
    }
}
