using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarPooling.Concerns
{
    public class Vehicle
    {
        [Key]
        public string Id { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string CarNumber { get; set; }
    }
}
