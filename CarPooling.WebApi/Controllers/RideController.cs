﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPooling.Contracts;
using CarPooling.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPooling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        public IRideService rideService;
        public RideController(IRideService rideService)
        {
            this.rideService = rideService;
        }

        [HttpGet("{id}")]
        public Ride Get(string id)
        {
            return rideService.GetRide(id);
        }

        [Route("userRides/{id}")]
        public List<Ride> GetUserRides(string id)
        {
            return rideService.GetRides(id);
        }

        [HttpPost]
        public void Post(Ride ride)
        {
            rideService.OfferRide(ride);
        }


        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Ride ride)
        {
            rideService.ModifyRide(id, ride);
        }

    }
}
