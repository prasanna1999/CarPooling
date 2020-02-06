using CarPooling.DataModels;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Carpooling.DataStore;
using System.Linq;

namespace CarPooling.Providers
{
    public class VehicleService : IVehicleService
    {
        public void AddVehicle(Vehicle vehicle)
        {
            Concerns.Vehicle _vehicle = vehicle.MapTo<Concerns.Vehicle>();
            using(var db = new Concerns.CarPoolingDbContext())
            {
                db.Add(_vehicle);
                db.SaveChanges();
            }
        }

        public List<Vehicle> GetVehicles(string userId)
        {
            using (var db = new Concerns.CarPoolingDbContext())
            {
                return db.Vehicle.Where(x=>x.UserId == userId).ToList().MapCollectionTo<Concerns.Vehicle, Vehicle>();
            }
        }

        public Vehicle GetVehicle(string vehicleId)
        {
            using(var db = new Concerns.CarPoolingDbContext())
            {
                return db.Vehicle.Find(vehicleId).MapTo<Vehicle>();
            }
        }
    }
}
