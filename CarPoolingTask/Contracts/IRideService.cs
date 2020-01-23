using CarPooling.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Contracts
{
    interface IRideService
    {
        List<Ride> ViewRides(User user);
        
    }
}
