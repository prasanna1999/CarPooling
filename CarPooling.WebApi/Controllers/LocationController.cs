using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPooling.Contracts;
using CarPooling.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPooling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("{rideId}")]
        public List<Location> Get(string rideId)
        {
            return locationService.GetLocations(rideId);
        }

        [HttpPost]
        public void Post(List<Location> locations)
        {
            locationService.AddLocations(locations);
        }
    }
}