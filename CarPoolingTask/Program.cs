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
                Email = email
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
                UserSelection(user);
            } while (user != null);
        }

        public void UserSelection(User user)
        {
            Console.WriteLine("Select One\n1. Offer a Ride\n2. Find a Ride\n3. View Your Rides\n4. View Your Bookings\n5. Logout");
            int choise = InputHandler.GetInt();
            switch ((UserMenu)choise)
            {
                case UserMenu.OfferRide:
                    OfferRide(user);
                    break;
                case UserMenu.FindRide:
                    FindRide();
                    break;
                case UserMenu.ViewYourRides:
                    ViewRides(user);
                    break;
                case UserMenu.ViewYourBookings:
                    break;
                case UserMenu.Logout:
                    user = null;
                    break;
            }
        }

        public void OfferRide(User user)
        {
            string from, to;
            double price;
            int noOfVacentSeats;
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
            Ride ride = new Ride
            {
                From = from,
                To = to,
                Date = date,
                Time = date,
                Price = price,
                NoOfVacentSeats = noOfVacentSeats,
                UserId = user.Id
            };
            CarPooling.AddRide(ride, user);
        }

        void ViewRides(User user)
        {
            IRideService rideService = new RideService();
            List<Ride> rides = rideService.ViewRides(user);
            foreach(Ride ride in rides)
            {
                Console.WriteLine(ride.Id+". From: "+ride.From+", To: "+ride.To+", Date: "+ride.Date+", Price: "+ride.Price);
            }
        }

        void FindRide()
        {
            string source, destination;
            DateTime date;
            Console.WriteLine("Enter Source:");
            do
            {
                source = InputHandler.GetString();
            } while (InputValidator.ValidateName(source));
            Console.WriteLine("Enter Destination");
            do {
                destination = InputHandler.GetString();
            } while (InputValidator.ValidateName(destination));
            Console.WriteLine("Enter Date and Time of Journey:");
            do
            {
                date = InputHandler.GetDate();
            } while (InputValidator.ValidateDate(date));
            List<Ride> rides = CarPooling.FindRide(source, destination, date);
            if (rides.Count == 0)
            {
                Console.WriteLine("No rides exists between " + source + " and " + destination + " on " + date);
            }
            else
            {
                foreach (Ride ride in rides)
                {
                    Console.WriteLine("Id: " + ride.Id + " From: " + ride.From + ", To: " + ride.To + ", Date: " + ride.Date + ", Price:" + ride.Price);
                }
            }
        }
    }
}
