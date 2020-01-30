using System;

namespace CarPooling.DataModels
{
    public class Booking
    {
        public Booking()
        {
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }

        public string UserId { get; set; }

        public string RideId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public double Price { get; set; }

        public int NoOfPersons { get; set; }

        public DateTime DateCreated { get; set; }

        public BookingStatus Status { get; set; }
    }
}
