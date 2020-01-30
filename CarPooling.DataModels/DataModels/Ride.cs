using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.DataModels
{
    public class Ride
    {
        public Ride()
        {
            Bookings = new List<Booking>();

            Status = RideStatus.NotYetStarted;

            DateCreated = DateTime.Now;
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int NoOfVacentSeats { get; set; }

        public List<string> ViaPoints { get; set; }

        public List<int> Distances { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public DateTime EndDate { get; set; }

        public BookingType Type { get; set; }

        public RideStatus Status { get; set; }

        public string VehicleId { get; set; }

        public DateTime DateCreated { get; set; }

        public List<Booking> Bookings { get; set; }
    }

}
