using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_work_Module_11_2
{
    public interface IUserManagementServicr
    {
        bool Register(string usernaem, string password);
        bool Login (string username, string password);
    }

    public interface IHotelService
    {
        List<Hotel> SearchHotels(string location, string hotelClass, double maxPrice);
        void AddHotel(Hotel hotel);
    }

    public interface IBookingService
    {
        bool CheckAvailability(Hotel hotel, DateTime startDate, DateTime endDate);
        void BookRoom(Hotel hotel, User user, DateTime startDate, DateTime endDate);
    }
    public interface IPaymentService
    {
        bool ProcessPayment(User user, double amount);
    }
    public interface INotificationService
    {
        void SendBookingConfirmation(User user, Hotel hotel);
        void SendPaymentConfirmation(User user);
    }
    public class Hotel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string HotelClass { get; set; }
        public double PricePerNight { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Booking
    {
        public Hotel Hotel { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UserManagementService : IUserManagementServicr
    {
        private Dictionary<string, string> users = new Dictionary<string, string>();

        public bool Register(string username, string password)
        {
            if (!users.ContainsKey(username))
            {
                users[username] = password;
                return true;
            }
            return false;
        }

        public bool Login(string username, string password)
        {
            return users.ContainsKey(username) && users[username] == password;
        }
    }
    public class HotelService : IHotelService
    {
        private List<Hotel> hotels = new List<Hotel>();

        public List<Hotel> SearchHotels(string location, string hotelClass, double maxPrice)
        {
            return hotels.Where(h => h.Location == location && h.HotelClass == hotelClass && h.PricePerNight <= maxPrice).ToList();
        }

        public void AddHotel(Hotel hotel)
        {
            hotels.Add(hotel);
        }
    }
    public class BookingService : IBookingService
    {
        private List<Booking> bookings = new List<Booking>();

        public bool CheckAvailability(Hotel hotel, DateTime startDate, DateTime endDate)
        {
            return !bookings.Any(b => b.Hotel == hotel && b.StartDate < endDate && b.EndDate > startDate);
        }

        public void BookRoom(Hotel hotel, User user, DateTime startDate, DateTime endDate)
        {
            bookings.Add(new Booking { Hotel = hotel, User = user, StartDate = startDate, EndDate = endDate });
        }
    }
    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(User user, double amount)
        {
            // Здесь может быть интеграция с реальной системой оплаты
            Console.WriteLine($"Платеж на сумму {amount} для пользователя {user.Username} обработан.");
            return true;
        }
    }
    public class NotificationService : INotificationService
    {
        public void SendBookingConfirmation(User user, Hotel hotel)
        {
            Console.WriteLine($"Уведомление: Бронирование подтверждено для {user.Username}. Отель: {hotel.Name}");
        }

        public void SendPaymentConfirmation(User user)
        {
            Console.WriteLine($"Уведомление: Платеж подтвержден для {user.Username}.");
        }
    }
    public class Program
    {
        public static void Main()
        {
            // Создание сервисов
            IUserManagementServicr userService = new UserManagementService();
            IHotelService hotelService = new HotelService();
            IBookingService bookingService = new BookingService();
            IPaymentService paymentService = new PaymentService();
            INotificationService notificationService = new NotificationService();

            // Регистрация пользователей
            userService.Register("user1", "password1");

            // Логин пользователя
            if (userService.Login("user1", "password1"))
            {
                Console.WriteLine("Пользователь успешно авторизован.");
            }

            // Добавление отелей
            hotelService.AddHotel(new Hotel { Name = "Hotel A", Location = "New York", HotelClass = "5 Star", PricePerNight = 200 });
            hotelService.AddHotel(new Hotel { Name = "Hotel B", Location = "New York", HotelClass = "4 Star", PricePerNight = 150 });

            // Поиск отелей
            var hotels = hotelService.SearchHotels("New York", "5 Star", 250);
            Console.WriteLine("Найденные отели:");
            foreach (var hotel in hotels)
            {
                Console.WriteLine($"{hotel.Name} - {hotel.PricePerNight}$");
            }

            // Бронирование отеля
            var selectedHotel = hotels.FirstOrDefault();
            if (selectedHotel != null && bookingService.CheckAvailability(selectedHotel, DateTime.Now, DateTime.Now.AddDays(2)))
            {
                bookingService.BookRoom(selectedHotel, new User { Username = "user1" }, DateTime.Now, DateTime.Now.AddDays(2));
                notificationService.SendBookingConfirmation(new User { Username = "user1" }, selectedHotel);
            }

            // Оплата
            paymentService.ProcessPayment(new User { Username = "user1" }, 200);
            notificationService.SendPaymentConfirmation(new User { Username = "user1" });

            Console.ReadKey();
        }
    }

}
