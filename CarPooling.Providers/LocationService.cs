using CarPooling.Contracts;
using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarPooling.Providers
{
    public class LocationService : ILocationService
    {
        public void AddLocations(List<Location> locations)
        {
            List<Concerns.Location> _locations = locations.MapCollectionTo<Location,Concerns.Location>();
            foreach(Concerns.Location location in _locations)
            {
                using(var db = new Concerns.CarPoolingDbContext())
                {
                    db.Location.Add(location);
                    db.SaveChanges();
                }
            }
        }

        public List<Location> GetLocations(string rideId)
        {
            using(var db = new Concerns.CarPoolingDbContext())
            {
                return db.Location.Where(x => x.RideId == rideId).ToList().MapCollectionTo<Concerns.Location, Location>();
            }
        }
    }
}
