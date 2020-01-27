using CarPooling.Concerns;
using CarPooling.Contracts;
using CarPooling.Extensions;
using CarPooling.Providers;
using System;
using System.Collections.Generic;

namespace CarPooling
{
    class Program
    {
        private static readonly CarPooling CarPooling = new CarPooling();
        private static readonly InputHandler InputHandler = new InputHandler();
        private static readonly InputValidator InputValidator = new InputValidator();
        static void Main(string[] args)
        {
            Program program = new Program();
            do
            {
                program.InitialSelection();
            } while (true);
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
                password = InputHandler.GetString();
            } while (InputValidator.ValidatePassword(password));
            User user = new User
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Password = password,
                Email = email,
                Id = name + email
            };
            CarPooling.AddUser(user);
        }

        public void Login()
        {
            IUserValidator userValidator = new UserValidator();
            string email, password;
            bool isValidLogin = false;
            User user;
            Console.WriteLine("Enter your email");
            do
            {
                email = InputHandler.GetString();
            } while (InputValidator.ValidateEmail(email));
            do
            {
                user = CarPooling.GetUser(email);
                if (user == null)
                {
                    Console.WriteLine("Incorrect email id. Do you want to exit? Type y to exit or press any key to continue");
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
                    password = InputHandler.GetString();
                } while (InputValidator.ValidatePassword(password));
                isValidLogin = userValidator.ValidateUserCredentials(user, password);
                if (!isValidLogin)
                {
                    Console.WriteLine("Incorrect password. Do you want to exit? Type y to exit or press any key to continue");
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
                    Console.WriteLine("Login success");
                }
            } while (!isValidLogin);
            do
            {
                user = UserSelection(user);
            } while (user != null);
        }

        public User UserSelection(User user)
        {
            Console.WriteLine("Select One\n1. Offer a Ride\n2. Find a Ride\n3. View Your Rides\n4. View Your Bookings\n5. Logout");
            int choise = InputHandler.GetInt();
            switch ((UserMenu)choise)
            {
                case UserMenu.OfferRide:
                    OfferRide(user);
                    break;
                case UserMenu.FindRide:
                    FindRide(user);
                    break;
                case UserMenu.ViewYourRides:
                    ViewRides(user);
                    break;
                case UserMenu.ViewYourBookings:
                    ViewBookings(user);
                    break;
                case UserMenu.Logout:
                    user = null;
                    break;
                default:
                    Console.WriteLine("Please enter mentioned options only");
                    break;
            }
            return user;
        }

        public void OfferRide(User user)
        {
            IRideService rideService = new RideService();
            string from, to;
            double price;
            int noOfVacentSeats, type;
            DateTime date;
            Console.WriteLine("Enter your source:");
            do
            {
                from = InputHandler.GetString();
            } while (InputValidator.ValidateName(from));
            Console.WriteLine("Enter your destination:");
            do
            {
                to = InputHandler.GetString();
            } while (InputValidator.ValidateName(to));
            Console.WriteLine("Enter date and time of journey mm/dd/yyyy hh:mm");
            do
            {
                date = InputHandler.GetDate();
            } while (InputValidator.ValidateDate(date));
            Console.WriteLine("Enter price");
            price = InputHandler.GetDouble();
            Console.WriteLine("Enter number of vacent seats");
            do
            {
                noOfVacentSeats = InputHandler.GetInt();
                if (noOfVacentSeats <= 0)
                {
                    Console.WriteLine("Please enter minimum one seat");
                }
            } while (noOfVacentSeats <= 0);
            Console.WriteLine("1. Auto Approval or 2. Manual Approval");
            do
            {
                type = InputHandler.GetInt();
                if (type != 1 && type != 2)
                    Console.WriteLine("Please enter 1 or 2");
            } while (type != 1 && type != 2);
            Vehicle vehicle;
            if (user.Vehicles.Count == 0)
            {
                vehicle = AddVehicle(user);
            }
            else
            {
                int i = 1;
                foreach (Vehicle vehi in user.Vehicles)
                {
                    Console.WriteLine(i + ". Vehicle Model:" + vehi.Model + " Vehicle Number:" + vehi.CarNumber);
                    i++;
                }
                Console.WriteLine("Select a vehicle mentioned above or press another number to add new vehicle");
                int val = InputHandler.GetInt();
                if (val <= user.Vehicles.Count)
                {
                    vehicle = user.Vehicles[val - 1];
                }
                else
                {
                    vehicle = AddVehicle(user);
                }
            }
            Console.WriteLine("Do you want to enter via points? If yes type 'y' else press any key");
            string choise = InputHandler.GetString();
            List<string> viaPoints = new List<string>();
            if (choise == "y")
            {
                string viaPoint;
                do
                {
                    Console.WriteLine("Enter via point or type enter to exit:");
                    viaPoint = InputHandler.GetString();
                    if (viaPoint == "") break;
                    viaPoints.Add(viaPoint);
                } while (viaPoints.Count < 8);
            }
            viaPoints.Insert(0, from);
            viaPoints.Add(to);
            Ride ride = new Ride
            {
                From = from,
                To = to,
                Date = date,
                Time = date,
                Price = price,
                NoOfVacentSeats = noOfVacentSeats,
                UserId = user.Id,
                Type = (BookingType)type,
                Id = user.Id + from + to + date.Date.ToString("d"),
                ViaPoints = viaPoints,
                Vehicle = vehicle
            };
            rideService.OfferRide(ride, user);
            Console.WriteLine("Ride added successfully");
        }

        Vehicle AddVehicle(User user)
        {
            Console.WriteLine("Please enter vehicle model");
            string model = InputHandler.GetString();
            Console.WriteLine("Please enter vehicle number");
            string vehicleNumber = InputHandler.GetString();
            Vehicle vehicle = new Vehicle()
            {
                Model = model,
                CarNumber = vehicleNumber,
                UserId = user.Id
            };
            user.Vehicles.Add(vehicle);
            return vehicle;
        }

        void ViewRides(User user)
        {
            IRideService rideService = new RideService();
            List<Ride> rides = rideService.ViewRides(user);
            if (rides.Count > 0)
            {
                int i = 1;
                foreach (Ride ride in rides)
                {
                    rideService.ChangeRideStatus(ride);
                    Console.WriteLine(i + ". From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price: " + ride.Price + ", Status: " + ride.Status);
                    i++;
                }
                Console.WriteLine("Select ride which you want to view. Select another number to exit");
                int select;
                select = InputHandler.GetInt();
                if (select > 0 && select <= rides.Count)
                {
                    ViewRide(rides[select - 1], user);
                }
            }
            else
            {
                Console.WriteLine("No rides yet.");
            }
        }

        void FindRide(User user)
        {
            string source, destination;
            DateTime date;
            int noOfPassengers;
            Console.WriteLine("Enter Source:");
            do
            {
                source = InputHandler.GetString();
            } while (InputValidator.ValidateName(source));
            Console.WriteLine("Enter Destination");
            do
            {
                destination = InputHandler.GetString();
            } while (InputValidator.ValidateName(destination));
            Console.WriteLine("Enter Date and Time of Journey in MM/DD/YYYY HH:MM");
            do
            {
                date = InputHandler.GetDate();
            } while (InputValidator.ValidateDate(date));
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
                foreach (Ride ride in rides)
                {
                    Console.WriteLine(i + "." + ride.Id + " From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price:" + ride.Price);
                    i++;
                }
                Console.WriteLine("Do you want to book a car? Type 'y' to proceed otherwise press any key to exit");
                string choise = InputHandler.GetString().ToLower();
                if (choise == "y")
                {
                    do
                    {
                        Console.WriteLine("Please select a car which you want to book");
                        num = InputHandler.GetInt();
                    } while (num > rides.Count || num <= 0);
                    Booking booking = new Booking()
                    {
                        Id = rides[num - 1].Id + user.Name,
                        UserId = user.Id,
                        From = source,
                        To = destination,
                        Date = rides[num - 1].Date,
                        Time = rides[num - 1].Date,
                        NoOfPersons = noOfPassengers,
                        RideId = rides[num - 1].Id
                    };
                    BookACar(rides[num - 1], user, booking);
                }
            }
        }

        void BookACar(Ride ride, User user, Booking booking)
        {
            IBookingService bookingService = new BookingService();

            if (bookingService.AddBooking(ride, user, booking))
            {
                Console.WriteLine("Booked car successfully");
            }
            else
            {
                Console.WriteLine("You can not book your ride");
                ViewRide(ride, user);
            }
        }

        void ViewBookings(User user)
        {
            IBookingService bookingService = new BookingService();
            List<Booking> bookings = bookingService.ViewBookings(user);
            if (bookings.Count > 0)
            {
                foreach (Booking booking in bookings)
                {
                    Console.WriteLine("From: " + booking.From + ", To: " + booking.To + ", Date: " + booking.Date + ", Approval Status: " + booking.Status);
                    if (booking.Date < DateTime.Now)
                    {
                        Console.WriteLine("Ride already finished");
                    }
                    else
                    {
                        ModifyBooking(user, booking);
                    }
                }
            }
            else
            {
                Console.WriteLine("No bookings yet");
            }
        }

        void ViewRide(Ride ride, User user)
        {
            Console.WriteLine("From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price: " + ride.Price);
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
            Console.WriteLine("Select one: 1. Modify Ride 2. Cancel Ride 3. View Your Ride Bookings 4. Exit");
            int choise;
            do
            {
                choise = InputHandler.GetInt();
                switch (choise)
                {
                    case 1:
                        ModifyRide(ride, user);
                        break;
                    case 2:
                        if (rideService.CancelRide(ride, user))
                            Console.WriteLine("Cancelled successfully");
                        else
                            Console.WriteLine("Ride cannot be cancelled");
                        break;
                    case 3:
                        ViewRideBookings(ride, user);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Please enter above values only");
                        break;
                }
            } while (choise != 1 && choise != 2 && choise != 3);
        }

        void ModifyRide(Ride ride, User user)
        {
            IRideService rideService = new RideService();
            Console.WriteLine("Choose one 1. Price 2.No Of Seats \n Press any number to exit other than above mentioned");
            int choise = InputHandler.GetInt();
            if (choise != 1 && choise != 2)
                return;
            Console.WriteLine("Enter Changed Value");
            int value = InputHandler.GetInt();
            if (rideService.ModifyRide(ride, user, choise, value))
            {
                Console.WriteLine("Modified successfully");
            }
            else
            {
                Console.WriteLine("Ride cannot be modified");
            }
        }

        void ViewRideBookings(Ride ride, User user)
        {
            IRideService rideService = new RideService();
            List<Booking> bookings = rideService.ViewRideBookings(ride);
            if (bookings.Count == 0)
            {
                Console.WriteLine("No one booked your ride yet");
            }
            foreach (Booking booking in bookings)
            {
                Console.WriteLine("From: " + booking.From + ", To: " + booking.To + ", Date: " + booking.Date + ", Approval Status: " + booking.Status);
                if (ride.Type == BookingType.ManualApproval && booking.Status == BookingStatus.Pending)
                {
                    Console.WriteLine("Do you want to Approve this booking? If yes press y, otherwise press any key");
                    string choise = InputHandler.GetString().ToLower();
                    if (choise == "y")
                    {
                        if (rideService.ApproveBooking(ride, booking))
                        {
                            Console.WriteLine("Approved Booking");
                        }
                        else
                        {
                            Console.WriteLine("Number of vacent seats are less than the requirement");
                        }
                    }
                }
            }
        }

        void ModifyBooking(User user, Booking booking)
        {
            Ride ride = CarPooling.GetRide(booking.RideId);
            if (ride.Status == RideStatus.Cancelled)
            {
                Console.WriteLine("Ride Cancelled");
                return;
            }
            Console.WriteLine("Select One: 1. Modify Booking 2. Cancel Booking 3. Exit");
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
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Please enter above values only");
                        break;
                }
            } while (choise != 1 && choise != 2);
        }
    }
}