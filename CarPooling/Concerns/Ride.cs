using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Concerns
{
    public class Ride
    {
        public Ride()
        {
            Bookings = new List<Booking>();

            Status = RideStatus.NotYetStarted;
            
        }
        public string Id { get; set; }

        public string UserId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public double Distance { get; set; }

        public double Price { get; set; }

        public int NoOfVacentSeats { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public BookingType Type { get; set; }

        public RideStatus Status { get; set; }

        public Vehicle Vehicle { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
