using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    public interface ILocationService
    {
        void AddLocations(List<Location> location);

        List<Location> GetLocations(string rideId);
    }
}
