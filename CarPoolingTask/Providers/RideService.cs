using CarPooling.Concerns;
using CarPooling.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Providers
{
    class RideService: IRideService
    {
        public List<Ride> ViewRides(User user)
        {
            return user.Rides;
        }
    }
}
