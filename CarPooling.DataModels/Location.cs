﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.DataModels
{
    public class Location
    {
        public string Id { get; set; }
        
        public string RideId { get; set; }
        
        public string LocationName { get; set; }
        
        public int Distance { get; set; }
    }
}
