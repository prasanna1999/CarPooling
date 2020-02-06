using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarPooling.Concerns
{
    public class Location
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("Ride")]
        public string RideId { get; set; }
        public Ride Ride { get; set; }

        [Required]
        public string LocationName { get; set; }

        [Required]
        public int Distance { get; set; }
    }
}
