using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using WebNC_Project.Models;
using System.Data.Entity.Core.Objects;
using WebNC_Project.ViewModel;

namespace WebNC_Project.DAO
{
    public static class BookingDAO
    {
        public static async Task<IEnumerable<Booking>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.BookingServices.Select(s => s.Service))
                    .ToListAsync();
            }
        }

        public static async Task<IEnumerable<Booking>> BookingsOfCustomer(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.BookingServices.Select(s => s.Service))
                    .Where(b => b.CustomerID == id)
                    .OrderByDescending(b => b.ID)
                    .ToListAsync();
            }
        }

        public static async Task<Booking> GetByID(int id)
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.BookingServices.Select(s => s.Service))
                    .SingleOrDefaultAsync(b => b.ID == id);
            }
        }

        [Obsolete]
        public static async Task<IEnumerable<Customer>> GetCustomerAvailable()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Customers
                    .Include(c => c.Bookings)
                    .Where(c => c.Bookings.All(b => b.Status == "cancel" || (b.Status == "payment" && EntityFunctions.TruncateTime(b.CheckoutDate) <= DateTime.Now)))
                    .ToListAsync();
            }
        }

        [Obsolete]
        public static async Task<IEnumerable<Room>> GetRoomAvailable(DateTime from, DateTime to)
        {
            using(ResortContext db = new ResortContext())
            {
                var list = await db.Rooms.Include(r => r.Bookings)
                    .Where(r => r.Bookings.Any(b =>
                    ((EntityFunctions.TruncateTime(b.CheckinDate) <= from.Date && from.Date <= EntityFunctions.TruncateTime(b.CheckoutDate)) ||
                    (EntityFunctions.TruncateTime(b.CheckinDate) <= to.Date && to.Date <= EntityFunctions.TruncateTime(b.CheckoutDate))) &&
                    (b.Status != "cancel")))
                    .Select(r => r.ID)
                    .ToListAsync();
                return await db.Rooms.Include(r => r.Bookings)
                    .Where(r => !list.Contains(r.ID)).ToListAsync();
            }
        }

        public static async Task<int> Create(Booking model)
        {
            using (ResortContext db = new ResortContext())
            {
                db.Bookings.Add(model);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Booking model)
        {
            using (ResortContext db = new ResortContext())
            {
                Booking enti = await db.Bookings.FindAsync(model.ID);
                enti.CustomerID = model.CustomerID;
                enti.CheckinDate = model.CheckinDate;
                enti.CheckoutDate = model.CheckoutDate;
                enti.Adult = model.Adult;
                enti.Child = model.Child;
                enti.RoomID = model.RoomID;
                enti.VoucherCode = model.VoucherCode;
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<IEnumerable<string>> GetServicesOfBill(int id)
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.BookingServices.Where(b => b.BookingID == id).Select(b => b.ServiceID).ToListAsync();
            }
        }

        public static async Task<int> AddSV(BookingServices model)
        {
            using (ResortContext db = new ResortContext())
            {
                db.BookingServices.Add(model);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> RemoveSV(BookingServices model)
        {
            using (ResortContext db = new ResortContext())
            {
                var enti = await db.BookingServices.Where(b => b.BookingID == model.BookingID && b.ServiceID == model.ServiceID).SingleOrDefaultAsync();
                db.BookingServices.Remove(enti);                
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(int id)
        {
            using (ResortContext db = new ResortContext())
            {
                var booking = await db.Bookings.Where(b => b.ID == id).Include(b => b.BookingServices).SingleOrDefaultAsync();
                foreach(var item in booking.BookingServices.ToList())
                {
                    db.BookingServices.Remove(item);
                }
                db.Bookings.Remove(booking);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> CheckBooking(int id, string status)
        {
            using (ResortContext db = new ResortContext())
            {
                Booking booking = await db.Bookings.FindAsync(id);
                booking.Status = status;
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> FeedBack(FeedBackViewModel model)
        {
            using (ResortContext db = new ResortContext())
            {
                Booking booking = await db.Bookings.FindAsync(model.BillID);
                booking.FeedBack = model.Content;
                booking.Rate = model.Rate;
                return await db.SaveChangesAsync();
            }
        }
    }
}