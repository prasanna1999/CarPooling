using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IVehicleService
    {
        void AddVehicle(Vehicle vehicle);

        List<Vehicle> GetVehicles(string userId);

        Vehicle GetVehicle(string vehicleId);
    }
}
