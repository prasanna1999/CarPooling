using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPooling.Contracts;
using CarPooling.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPooling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        IBookingService bookingService;
        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpGet]
        [Route("userBookings/{userId}")]
        public List<Booking> Get(string userId)
        {
            return bookingService.GetBookings(userId);
        }

        [HttpGet]
        [Route("rideBookings/{userId}")]
        public List<Booking> GetRideBookings(string rideId)
        {
            return bookingService.GetRideBookings(rideId);
        }

        [HttpPost]
        public bool Post(Booking booking)
        {
            return bookingService.AddBooking(booking);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public void Put(string id)
        {
            bookingService.CancelBooking(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
