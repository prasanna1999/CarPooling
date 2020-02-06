using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarPooling.Concerns
{
    public class Ride
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Distance { get; set; }

        [Required]
        public int NoOfVacentSeats { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("Vehicle")]
        public string VehicleId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

    }
}
