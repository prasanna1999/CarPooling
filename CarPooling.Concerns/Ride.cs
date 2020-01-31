using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Concerns
{
    public class Ride
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int Price { get; set; }

        public int Distance { get; set; }

        public int NoOfVacentSeats { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public DateTime EndDate { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string VehicleId { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
