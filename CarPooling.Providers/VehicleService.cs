using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Carpooling.DataStore;

namespace CarPooling.Providers
{
    public class VehicleService : IVehicleService
    {
        public void AddVehicle(Vehicle vehicle)
        {
            Concerns.Vehicle _vehicle = vehicle.MapTo<Concerns.Vehicle>();
            DataStore.vehicles.Add(_vehicle);
        }

        public List<Vehicle> GetVehicles(string userId)
        {
            List<Concerns.Vehicle> vehicles = DataStore.vehicles.FindAll(vehicle => vehicle.UserId == userId);
            return vehicles.MapCollectionTo<Concerns.Vehicle, Vehicle>();
        }

        public Vehicle GetVehicle(string vehicleId)
        {
            Concerns.Vehicle vehicle = DataStore.vehicles.Find(x => x.Id == vehicleId);
            return vehicle.MapTo<Vehicle>();
        }
    }
}
