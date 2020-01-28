using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    public class VehicleService : IVehicleService
    {
        public static List<Vehicle> vehicles = new List<Vehicle>();

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
        }

        public List<Vehicle> GetVehicles(string userId)
        {
            return vehicles.FindAll(vehicle => vehicle.UserId == userId);
        }
    }
}
