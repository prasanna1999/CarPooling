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
                email = InputHandler.GetString();
            } while (InputValidator.ValidateEmail(email));
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
                password = InputHandler.GetString();
            } while (InputValidator.ValidatePassword(password));
            do
            {
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
            noOfVacentSeats = InputHandler.GetInt();
            Console.WriteLine("1. Auto Approval or 2. Manual Approval");
            do
            {
                type = InputHandler.GetInt();
            } while (type != 1 && type != 2);
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
                Id = user.Id + from + to + date.Date.ToString("d")
            };
            rideService.OfferRide(ride, user);
            Console.WriteLine("Ride added successfully");
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
                    Console.WriteLine(i + ". From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price: " + ride.Price);
                    i++;
                }
                Console.WriteLine("Do you want to view any ride? Press 'y' to continue otherwise press any key");
                string choice=InputHandler.GetString();
                if (choice == "y")
                {
                    Console.WriteLine("Select a number which you want to change");
                    int select;
                    do
                    {
                        select = InputHandler.GetInt();
                    } while (select < 0 || select > rides.Count);
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
            Console.WriteLine("Enter Date and Time of Journey:");
            do
            {
                date = InputHandler.GetDate();
            } while (InputValidator.ValidateDate(date));
            Console.WriteLine("Enter No of Paasengers");
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
                        Date = date,
                        Time = date,
                        NoOfPersons = noOfPassengers,
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
                ViewRide(ride,user);
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
                    Console.WriteLine("From: " + booking.From + ", To: " + booking.To + ", Date: " + booking.Date+", Approval Status: "+booking.Status);
                    if (booking.Date < DateTime.Now)
                    {
                        Console.WriteLine("Ride already finished");
                    }
                    else
                    {
                        ViewBooking(user,booking);
                    }
                }
            }
            else
            {
                Console.WriteLine("No bookings yet");
            }
        }

        void ViewRide(Ride ride,User user)
        {
            Console.WriteLine("From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price: " + ride.Price);
            if (ride.Date < DateTime.Now)
            {
                Console.WriteLine("Ride already finished.");
                return;
            }
            Console.WriteLine("Select one: 1. Modify Ride 2. Cancel Ride 3. View Your Ride Bookings 4. Exit");
            int choise = InputHandler.GetInt();
            IRideService rideService = new RideService();
            switch (choise)
            {
                case 1:
                    ModifyRide(ride, user);
                    break;
                case 2:
                    rideService.CancelRide(ride, user);
                    Console.WriteLine("Cancelled Successfully");
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
            rideService.ModifyRide(ride, user,choise,value);
        }

        void ViewRideBookings(Ride ride, User user)
        {
            IRideService rideService = new RideService();
            List<Booking> bookings = rideService.ViewRideBookings(ride);
            foreach(Booking booking in bookings)
            {
                Console.WriteLine("From: "+booking.From+", To: "+booking.To+", Date: "+booking.Date+", Approval Status: "+booking.Status);
                if (ride.Type == BookingType.ManualApproval && booking.Status==BookingStatus.Pending)
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

        void ViewBooking(User user,Booking booking)
        {
            Console.WriteLine("From: " + booking.From + ", To: " + booking.To + ", Date: " + booking.Date + ", Approval Status: " + booking.Status);
            Console.WriteLine("Select One: 1. Modify Booking 2. Cancel Booking 3. Exit");
        }
    }
}