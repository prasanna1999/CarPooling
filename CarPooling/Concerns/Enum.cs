using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Concerns
{
    public enum BookingStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public enum BookingType
    {
        AutoApproval = 1,
        ManualApproval
    }

    public enum RideStatus
    {
        NotYetStarted,
        Cancelled,
        Completed
    }

    public enum InitialMenu
    {
        Register = 1,
        Login,
        Exit
    }

    public enum UserMenu
    {
        OfferRide = 1,
        FindRide,
        ViewYourRides,
        ViewYourBookings,
        Logout
    }
}
