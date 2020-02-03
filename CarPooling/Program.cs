using CarPooling.Contracts;
using CarPooling.Extensions;
using CarPooling.Providers;
using CarPooling.DataModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using AutoMapper;

namespace CarPooling
{
    class Program
    {
        private static readonly CarPooling CarPooling = new CarPooling();
        private static readonly InputHandler InputHandler = new InputHandler();
        private static readonly InputValidator InputValidator = new InputValidator();
        private static User user;
        public static string dateFormat = "MM/dd/yyyy HH:mm";
        static void Main(string[] args)
        {
            Program program = new Program();
            ConfigureProfiles();
            do
            {
                program.InitialSelection();
            } while (true);
        }

        public static void ConfigureProfiles()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.ToLowerInvariant().StartsWith("CarPooling")).ToArray();
            Mapper.Initialize(cfg => { cfg.AddMaps(assemblies); cfg.ValidateInlineMaps = false; });
        }

        public void InitialSelection()
        {
            Console.Clear();
            Console.WriteLine("Choose one option");
            Console.WriteLine("1. Register\n2. Login\n3. Exit");
            int choise = InputHandler.GetInt();
            switch ((InitialMenu)choise)
            {
                case InitialMenu.Register:
                    RegisterUser();
                    break;
                case InitialMenu.Login:
                    Login();
                    break;
                case InitialMenu.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter above mentioned options only.");
                    break;
            }
        }

        public void RegisterUser()
        {
            string name, phoneNumber, email, password;
            Console.WriteLine("Enter your name");
            do
            {
                name = InputHandler.GetString();
            } while (InputValidator.ValidateName(name));
            Console.WriteLine("Enter your phone number");
            do
            {
                phoneNumber = InputHandler.GetString();
            } while (InputValidator.ValidatePhoneNumber(phoneNumber));
            Console.WriteLine("Enter your email");
            do
            {
                do
                {
                    email = InputHandler.GetString();
                } while (InputValidator.ValidateEmail(email));
                if (CarPooling.GetUser(email) != null)
                    Console.WriteLine("Email already exists. Try another");
            } while (CarPooling.GetUser(email) != null);
            Console.WriteLine("Enter your password");
            do
            {
                password = InputHandler.GetPassword();
            } while (InputValidator.ValidatePassword(password));
            User newUser = new User
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Password = password,
                Email = email,
                Id = name + email
            };
            CarPooling.AddUser(newUser);
            Console.WriteLine("\nRegistration Successful");
            user = newUser;
            do
            {
                UserSelection();
            } while (user != null);
        }

        public void Login()
        {
            IUserValidator userValidator = new UserValidator();
            string email, password;
            bool isValidLogin = false;
            Console.WriteLine("Enter your email");
            do
            {
                do
                {
                    email = InputHandler.GetString();
                } while (InputValidator.ValidateEmail(email));

                user = CarPooling.GetUser(email);
                if (user == null)
                {
                    Console.WriteLine("Email does not exist. Do you want to go to main menu? Type y to go to main menu or press any key to continue");
                    string choise = InputHandler.GetString().ToLower();
                    if (choise == "y")
                    {
                        InitialSelection();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please enter correct email");
                    }
                }
            } while (user == null);
            Console.WriteLine("Enter password");
            do
            {
                do
                {
                    password = InputHandler.GetPassword();
                } while (InputValidator.ValidatePassword(password));
                isValidLogin = userValidator.ValidateUserCredentials(user, password);
                if (!isValidLogin)
                {
                    Console.WriteLine("Incorrect password. Do you want to go to main menu? Type y to go to main menu or press any key to continue");
                    string choise = InputHandler.GetString().ToLower();
                    if (choise == "y")
                    {
                        InitialSelection();
                    }
                    else
                    {
                        Console.WriteLine("Please enter correct password");
                    }
                }
                else
                {
                    Console.WriteLine("\nLogin success");
                }
            } while (!isValidLogin);
            do
            {
                UserSelection();
            } while (user != null);
        }

        public void UserSelection()
        {
            Console.WriteLine("Select One\n1. Offer a Ride\n2. Find a Ride\n3. View Your Rides\n4. View Your Bookings\n5. Logout");
            int choise = InputHandler.GetInt();
            switch ((UserMenu)choise)
            {
                case UserMenu.OfferRide:
                    OfferRide();
                    break;
                case UserMenu.FindRide:
                    FindRide();
                    break;
                case UserMenu.ViewYourRides:
                    ViewRides();
                    break;
                case UserMenu.ViewYourBookings:
                    ViewBookings();
                    break;
                case UserMenu.Logout:
                    user = null;
                    break;
                default:
                    Console.WriteLine("Please enter mentioned options only");
                    break;
            }
            return;
        }

        public void OfferRide()
        {
            IRideService rideService = new RideService();
            string from, to;
            double price;
            int noOfVacantSeats, distance;
            DateTime date, endDate;
            Console.WriteLine("Enter your source:");
            do
            {
                from = InputHandler.GetString();
            } while (InputValidator.ValidateLength(from));
            Console.WriteLine("Enter your destination:");
            do
            {
                to = InputHandler.GetString();
            } while (InputValidator.ValidateLength(to));
            Console.WriteLine($"Enter date and time of journey {dateFormat}");
            do
            {
                date = InputHandler.GetDate(dateFormat);
            } while (InputValidator.CompareDate(DateTime.Now, date));
            Console.WriteLine($"Enter estimated end date and time of journey {dateFormat}");
            do
            {
                endDate = InputHandler.GetDate(dateFormat);
            } while (InputValidator.CompareDate(date, endDate));
            Console.WriteLine("Enter distance from source to destination in Kms");
            distance = InputHandler.GetInt();
            Console.WriteLine("Enter price per KM");
            price = InputHandler.GetDouble();
            Console.WriteLine("Enter number of vacant seats");
            do
            {
                noOfVacantSeats = InputHandler.GetInt();
                if (noOfVacantSeats <= 0)
                {
                    Console.WriteLine("Please enter minimum one seat");
                }
            } while (noOfVacantSeats <= 0);
            IVehicleService vehicleService = new VehicleService();
            List<Vehicle> vehicles = vehicleService.GetVehicles(user);
            Vehicle vehicle;
            if (vehicles.Count == 0)
            {
                vehicle = AddVehicle();
            }
            else
            {
                int count = 1;
                foreach (Vehicle _vehicle in vehicles)
                {
                    Console.WriteLine(count + ".\t Vehicle Model:" + _vehicle.Model + "\t Vehicle Number:" + _vehicle.CarNumber);
                    count++;
                }
                Console.WriteLine("Select a vehicle mentioned above or press another number to add new vehicle");
                int val = InputHandler.GetInt();
                if (val <= vehicles.Count)
                {
                    vehicle = vehicles[val - 1];
                }
                else
                {
                    vehicle = AddVehicle();
                }
            }
            Console.WriteLine("Do you want to enter via points? If yes type 'y' else press any key");
            string choise = InputHandler.GetString();
            List<string> viaPoints = new List<string>();
            List<int> distances = new List<int>();
            if (choise == "y")
            {
                string viaPoint;
                int previousDist = 0, dist;
                do
                {
                    Console.WriteLine("Enter via point or type enter to exit:");
                    viaPoint = InputHandler.GetString();
                    if (viaPoint == "") break;
                    viaPoints.Add(viaPoint);
                    Console.WriteLine("Enter distance from source to via point in KMs:");
                    do
                    {
                        dist = InputHandler.GetInt();
                        if (dist <= previousDist)
                        {
                            Console.WriteLine("Please enter correct distance");
                        }
                    } while (dist <= previousDist);
                    previousDist = dist;
                    distances.Add(dist);
                } while (viaPoints.Count < 8);
            }
            Ride ride = new Ride
            {
                From = from,
                To = to,
                Date = date,
                Time = date,
                EndDate = endDate,
                Price = price,
                Distance = distance,
                Distances = distances,
                NoOfVacentSeats = noOfVacantSeats,
                UserId = user.Id,
                Type = BookingType.ManualApproval,
                Id = user.Id + from + to + date.Date.ToString("d"),
                ViaPoints = viaPoints,
                VehicleId = vehicle.Id
            };
            rideService.OfferRide(ride, user);
            Console.WriteLine("Ride added successfully");
        }

        Vehicle AddVehicle()
        {
            string model, vehicleNumber;
            Console.WriteLine("Please enter vehicle model");
            do
            {
                model = InputHandler.GetString();
            } while (InputValidator.ValidateLength(model));
            Console.WriteLine("Please enter vehicle number");
            do
            {
                vehicleNumber = InputHandler.GetString();
            } while (InputValidator.ValidateLength(model));
            Vehicle vehicle = new Vehicle()
            {
                Model = model,
                CarNumber = vehicleNumber,
                UserId = user.Id,
                Id = user.Id + model
            };
            IVehicleService vehicleService = new VehicleService();
            vehicleService.AddVehicle(vehicle, user);
            return vehicle;
        }

        void ViewRides()
        {
            IRideService rideService = new RideService();
            List<Ride> rides = rideService.GetRides(user);
            if (rides.Count > 0)
            {
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("No\t|From\t|To  \t|Date\t\t\t|Price\t|Status");
                Console.WriteLine("--------------------------------------------------------------------------");
                int i = 1;
                foreach (Ride ride in rides)
                {
                    double price = rideService.GetPrice(ride.From, ride.To, ride);
                    rideService.ChangeRideStatus(ride);
                    Console.WriteLine(i + "\t|" + ride.From + "\t|" + ride.To + "\t|" + ride.Date + "\t|" + price + "\t|" + ride.Status);
                    i++;
                }
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("Select ride which you want to view. Select another number to go to user menu");
                int select;
                select = InputHandler.GetInt();
                if (select > 0 && select <= rides.Count)
                {
                    ViewRide(rides[select - 1]);
                }
            }
            else
            {
                Console.WriteLine("No rides yet.");
            }
        }

        void FindRide()
        {
            IRideService rideService = new RideService();
            string source, destination;
            DateTime date;
            int noOfPassengers;
            double price = 0;
            Console.WriteLine("Enter pick up point:");
            do
            {
                source = InputHandler.GetString();
            } while (InputValidator.ValidateLength(source));
            Console.WriteLine("Enter drop point");
            do
            {
                destination = InputHandler.GetString();
            } while (InputValidator.ValidateLength(destination));
            Console.WriteLine($"Enter date and time of journey in {dateFormat}");
            do
            {
                date = InputHandler.GetDate(dateFormat);
            } while (InputValidator.CompareDate(DateTime.Now, date));
            Console.WriteLine("Enter No of Pasengers");
            noOfPassengers = InputHandler.GetInt();
            List<Ride> rides = CarPooling.FindRide(source, destination, date, noOfPassengers);
            if (rides.Count == 0)
            {
                Console.WriteLine("No rides found between " + source + " and " + destination + " on " + date);
            }
            else
            {
                int i = 1, num;
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("No\t|From\t|To  \t|Start Date and Time\t|End Date and Time\t|Price Per Head\t");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                foreach (Ride ride in rides)
                {
                    price = rideService.GetPrice(source, destination, ride);
                    Console.WriteLine(i + "\t|" + source + "\t|" + destination + "\t|" + ride.Date + "\t|" + ride.EndDate + "\t|" + price + "\t");
                    i++;
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Do you want to book a car? Type 'y' to proceed otherwise press any key to go to user menu");
                string choise = InputHandler.GetString().ToLower();
                if (choise == "y")
                {
                    do
                    {
                        Console.WriteLine("Please select a car which you want to book");
                        num = InputHandler.GetInt();
                    } while (num > rides.Count || num <= 0);
                    Ride ride = rides[num - 1];
                    Booking booking = new Booking()
                    {
                        Id = ride.Id + user.Name,
                        UserId = user.Id,
                        From = source,
                        To = destination,
                        Date = ride.Date,
                        Time = ride.Date,
                        NoOfPersons = noOfPassengers,
                        RideId = ride.Id,
                        Price = price
                    };
                    BookACar(ride, booking);
                }
            }
        }

        void BookACar(Ride ride, Booking booking)
        {
            IBookingService bookingService = new BookingService();

            if (bookingService.AddBooking(ride, user, booking))
            {
                Console.WriteLine("Waiting for approval");
            }
            else
            {
                Console.WriteLine("You can not book your ride");
                ViewRide(ride);
            }
        }

        void ViewBookings()
        {
            IBookingService bookingService = new BookingService();
            List<Booking> bookings = bookingService.GetBookings(user);
            if (bookings.Count > 0)
            {
                foreach (Booking booking in bookings)
                {
                    Console.WriteLine("From: " + booking.From + ",\nTo: " + booking.To + ",\nDate: " + booking.Date + ",\nNo of seats: " + booking.NoOfPersons + ",\nPrice per head: " + booking.Price + ",\nApproval Status: " + booking.Status);
                    if (booking.Date < DateTime.Now)
                    {
                        Console.WriteLine("Ride already finished");
                    }
                    else
                    {
                        ModifyBooking(booking);
                    }
                }
                Console.WriteLine("No more bookings.");
            }
            else
            {
                Console.WriteLine("No bookings yet");
            }
        }

        void ViewRide(Ride ride)
        {
            Console.WriteLine("From: " + ride.From + ",\nTo: " + ride.To + ",\nDate: " + ride.Date);
            if (ride.Status == RideStatus.Cancelled)
            {
                Console.WriteLine("Ride Cancelled");
                return;
            }
            else if (ride.Date < DateTime.Now)
            {
                Console.WriteLine("Ride already finished.");
                return;
            }
            IRideService rideService = new RideService();
            Console.WriteLine("Select one: \n1. Modify Ride \n2. Cancel Ride \n3. View Your Ride Bookings \n4. Return to user menu");
            int choise;
            do
            {
                choise = InputHandler.GetInt();
                switch (choise)
                {
                    case 1:
                        ModifyRide(ride);
                        break;
                    case 2:
                        if (rideService.CancelRide(ride))
                            Console.WriteLine("Cancelled successfully");
                        else
                            Console.WriteLine("Ride cannot be cancelled");
                        break;
                    case 3:
                        ViewRideBookings(ride);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Please enter above values only");
                        break;
                }
            } while (choise != 1 && choise != 2 && choise != 3);
        }

        void ModifyRide(Ride ride)
        {
            IRideService rideService = new RideService();
            Console.WriteLine("Choose one \n1.No Of Seats \nPress any number to go to user menu other than above mentioned");
            int choise = InputHandler.GetInt();
            if (choise != 1)
                return;
            Console.WriteLine("Enter Changed Value");
            int value = InputHandler.GetInt();
            if (rideService.ModifyRide(ride, value))
            {
                Console.WriteLine("Modified successfully");
            }
            else
            {
                Console.WriteLine("Ride cannot be modified");
            }
        }

        void ViewRideBookings(Ride ride)
        {
            IBookingService bookingService = new BookingService();
            List<Booking> bookings = bookingService.GetRideBookings(ride);
            if (bookings.Count == 0)
            {
                Console.WriteLine("No one booked your ride yet");
                return;
            }
            foreach (Booking booking in bookings)
            {
                Console.WriteLine("Pick Up: " + booking.From + ",\nDrop: " + booking.To + ",\nDate: " + booking.Date + ",\nNo of seats: " + booking.NoOfPersons + ",\nPrice per head: " + booking.Price + ",\nApproval Status: " + booking.Status);
                if (ride.Type == BookingType.ManualApproval && booking.Status == BookingStatus.Pending)
                {
                    Console.WriteLine("Select one \n1. Approve Booking\n2. Reject Booking\nPress any another number to go to next booking.");
                    int choise = InputHandler.GetInt();
                    switch (choise)
                    {
                        case 1:
                            if (bookingService.ApproveBooking(ride, booking))
                            {
                                Console.WriteLine("Approved Booking");
                            }
                            else
                            {
                                Console.WriteLine("Number of vacant seats are less than the requirement");
                            }
                            break;
                        case 2:
                            bookingService.RejectBooking(booking);
                            Console.WriteLine("Rejected Booking");
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.WriteLine("No more bookings");
        }

        void ModifyBooking(Booking booking)
        {
            IRideService rideService = new RideService();
            Ride ride = CarPooling.GetRide(booking.RideId);
            if (ride.Status == RideStatus.Cancelled)
            {
                Console.WriteLine("Ride Cancelled");
                return;
            }
            else if (booking.Status == BookingStatus.Rejected)
            {
                Console.WriteLine("Sorry your booking is rejected");
                return;
            }
            Console.WriteLine("Select One: \n1. Modify Booking \n2. Cancel Booking \nPress any another number to go to next booking.");
            int choise;
            IBookingService bookingService = new BookingService();
            do
            {
                choise = InputHandler.GetInt();
                switch (choise)
                {
                    case 1:
                        Console.WriteLine("Do you want to modify Number of persons? If yes press 'y' otherwise press any key");
                        string key = InputHandler.GetString();
                        if (key == "y")
                        {
                            Console.WriteLine("Enter number of persons");
                            int noOfPersons = InputHandler.GetInt();
                            if (bookingService.ModifyBooking(booking, noOfPersons, ride))
                            {
                                Console.WriteLine("Updated succesfully");
                            }
                            else
                            {
                                Console.WriteLine("Update failed");
                            }
                        }
                        break;
                    case 2:
                        if (bookingService.CancelBooking(booking, ride))
                        {
                            Console.WriteLine("Cancelled successfully");
                        }
                        else
                        {
                            Console.WriteLine("Booking already cancelled");
                        }
                        break;
                    default:
                        return;
                }
            } while (choise != 1 && choise != 2);
        }
    }
}