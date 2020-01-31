using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class VehicleService : IVehicleService
    {
        public void AddVehicle(Vehicle vehicle, User user)
        {
            user.Vehicles.Add(vehicle);
        }

        public List<Vehicle> GetVehicles(User user)
        {
            return user.Vehicles;
        }
    }
}
