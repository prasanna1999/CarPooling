using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface IVehicleService
    {
        void AddVehicle(Vehicle vehicle, User user);

        List<Vehicle> GetVehicles(User user);
    }
}
