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
    public class VehicleController : ControllerBase
    {
        IVehicleService vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [Route("userVehicles/{userId}")]
        public List<Vehicle> GetUserVehicles(string userId)
        {
            return vehicleService.GetVehicles(userId);
        }

        [HttpGet("{id}", Name = "Get")]
        public Vehicle Get(string id)
        {
            return vehicleService.GetVehicle(id);
        }

        [HttpPost]
        public void Post(Vehicle vehicle)
        {
            vehicleService.AddVehicle(vehicle);
        }
    }
}
