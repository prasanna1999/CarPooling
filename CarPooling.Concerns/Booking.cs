using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarPooling.Concerns
{
    public class Booking
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Ride")]
        public string RideId { get; set; }
        public Ride Ride { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int NoOfPersons { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
