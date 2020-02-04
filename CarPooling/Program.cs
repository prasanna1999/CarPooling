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
            IUserService userService = new UserService();
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
                if (userService.GetUser(email) != null)
                    Console.WriteLine("Email already exists. Try another");
            } while (userService.GetUser(email) != null);
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
            userService.AddUser(newUser);
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
            IUserService userService = new UserService();
            string email, password;
            bool isValidLogin = false;
            Console.WriteLine("Enter your email");
            do
            {
                do
                {
                    email = InputHandler.GetString();
                } while (InputValidator.ValidateEmail(email));

                user = userService.GetUser(email);
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
            List<Vehicle> vehicles = vehicleService.GetVehicles(user.Id);
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
            rideService.OfferRide(ride);
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
            vehicleService.AddVehicle(vehicle);
            return vehicle;
        }

        void ViewRides()
        {
            IRideService rideService = new RideService();
            IVehicleService vehicleService = new VehicleService();
            List<Ride> rides = rideService.GetRides(user.Id);
            if (rides.Count > 0)
            {
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("No\t|From\t|To  \t|Date\t\t\t|Price\t|Vehicle Model\t|Vehicle Number\t|Status");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                int i = 1;
                foreach (Ride ride in rides)
                {
                    Vehicle vehicle = vehicleService.GetVehicle(ride.VehicleId);
                    double price = rideService.GetPrice(ride.From, ride.To, ride);
                    rideService.ChangeRideStatus(ride);
                    Console.WriteLine(i + "\t|" + ride.From + "\t|" + ride.To + "\t|" + ride.Date + "\t|" + price + "\t|" + vehicle.Model + "\t\t|" + vehicle.CarNumber + "\t\t|" + ride.Status);
                    i++;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------");
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
            List<Ride> rides = rideService.FindRide(source, destination, date, noOfPassengers);
            if (rides.Count == 0)
            {
                Console.WriteLine("No rides found between " + source + " and " + destination + " on " + date);
            }
            else
            {
                IVehicleService vehicleService = new VehicleService();
                int i = 1, num;
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("No\t|From\t|To  \t|Start Date and Time\t|End Date and Time\t|Vehicle Model\t|Vehicle Number\t|Price Per Head\t");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
                foreach (Ride ride in rides)
                {
                    Vehicle vehicle = vehicleService.GetVehicle(ride.VehicleId);
                    price = rideService.GetPrice(source, destination, ride);
                    Console.WriteLine(i + "\t|" + source + "\t|" + destination + "\t|" + ride.Date + "\t|" + ride.EndDate + "\t|" + vehicle.Model + "\t\t|" + vehicle.CarNumber + "\t\t|" + price + "\t");
                    i++;
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
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
                        Id = ride.Id + user.Name + DateTime.Now,
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

            if (bookingService.AddBooking(ride, booking))
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
            List<Booking> bookings = bookingService.GetBookings(user.Id);
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
            List<Booking> bookings = ride.Bookings;
            if (bookings.Count == 0)
            {
                Console.WriteLine("No one booked your ride yet");
                return;
            }
            Console.WriteLine("SNO\t|Pick Up\t|Drop\t|Date\t\t|No of seats\t|Price per head\t|Approval Status");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            int count = 1;
            foreach (Booking booking in bookings)
            {
                Console.WriteLine(count + "\t|" + booking.From + "\t\t|" + booking.To + "\t\t|" + booking.Date + "\t|" + booking.NoOfPersons + "\t|" + booking.Price + "\t|" + booking.Status);
                count++;
            }

            Console.WriteLine("Select the booking you want to approve or reject. Type any other number to go to user menu");
            int num;
            num = InputHandler.GetInt();
            if (num <= bookings.Count && num > 0)
            {
                Booking booking = bookings[num - 1];
                if (ride.Type == BookingType.ManualApproval && booking.Status == BookingStatus.Pending)
                {
                    Console.WriteLine("Select one \n1. Approve Booking\n2. Reject Booking\nPress any another number to go to user menu.");
                    int select = InputHandler.GetInt();
                    switch (select)
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
                else
                {
                    Console.WriteLine("Booking already " + booking.Status);
                }
            }
        }

        void ModifyBooking(Booking booking)
        {
            IRideService rideService = new RideService();
            Ride ride = rideService.GetRide(booking.RideId);
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
            else if (booking.Status == BookingStatus.Cancelled)
            {
                Console.WriteLine("Sorry your booking is cancelled");
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
                        if (bookingService.CancelBooking(booking))
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