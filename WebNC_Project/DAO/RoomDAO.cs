using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class RoomDAO
    {
        public static async Task<IEnumerable<Room>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.SuppliesForRooms.Select(s => s.Supply)).ToListAsync();
            }
        }

        public static async Task<IEnumerable<Room>> Search(string name)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.SuppliesForRooms.Select(s => s.Supply))
                    .Where(r => r.Name.Contains(name)).ToListAsync();
            }
        }

        public static async Task<Room> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.SuppliesForRooms.Select(s => s.Supply)).SingleOrDefaultAsync(r => r.ID == id);
            }
        }

        public static async Task<int> Create(Room room)
        {
            using (ResortContext db = new ResortContext())
            {
                room.ID = room.ID.Trim().ToUpper();
                db.Rooms.Add(room);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                //Room room = await db.Rooms.Include(r => r.Images).SingleOrDefaultAsync(r => r.ID == id);
                Room room = await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.SuppliesForRooms.Select(s => s.Supply)).SingleOrDefaultAsync(r => r.ID == id);
                foreach (var img in room.Images.ToList())
                {
                    db.Images.Remove(img);
                }
                foreach(var s in room.SuppliesForRooms.ToList())
                {
                    Supply supply = await db.Supplies.FindAsync(s.SupplyID);
                    supply.Total += s.Count;
                    db.SuppliesForRooms.Remove(s);
                }
                db.Rooms.Remove(room);
                return db.SaveChanges();
            }
        }

        public static async Task<int> EditInfo(Room room)
        {
            using (ResortContext db = new ResortContext())
            {
                Room enti = await db.Rooms.SingleOrDefaultAsync(s => s.ID.ToLower() == room.ID.Trim().ToLower());
                if (enti != null)
                {
                    enti.Name = room.Name;
                    enti.Price = room.Price;
                    enti.Description = room.Description;
                    enti.TypeID = room.TypeID;
                    enti.Status = room.Status;
                    enti.Adult = room.Adult;
                    enti.Child = room.Child;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }

        [Obsolete]
        public static async Task<bool> CheckRoom(string id, DateTime from, DateTime to, int invoice = 0)
        {
            using(ResortContext db = new ResortContext())
            {
                var list = await db.Rooms.Include(r => r.Bookings)
                    .Where(r => r.Bookings.Any(b =>
                    ((EntityFunctions.TruncateTime(b.CheckinDate) <= from.Date && from.Date <= EntityFunctions.TruncateTime(b.CheckoutDate)) ||
                    (EntityFunctions.TruncateTime(b.CheckinDate) <= to.Date && to.Date <= EntityFunctions.TruncateTime(b.CheckoutDate))) &&
                    (b.Status != "cancel") && (b.ID != invoice)))
                    .Select(r => r.ID)
                    .ToListAsync();
                if (list.Contains(id)) return false;
                return true;
            }
        }

        [Obsolete]
        public static async Task<IEnumerable<Room>> GetRoomAvailable(DateTime from, DateTime to)
        {
            using (ResortContext db = new ResortContext())
            {
                var list = await db.Rooms.Include(r => r.Bookings)
                    .Where(r => r.Bookings.Any(b =>
                    ((EntityFunctions.TruncateTime(b.CheckinDate) <= from.Date && from.Date <= EntityFunctions.TruncateTime(b.CheckoutDate)) ||
                    (EntityFunctions.TruncateTime(b.CheckinDate) <= to.Date && to.Date <= EntityFunctions.TruncateTime(b.CheckoutDate))) &&
                    (b.Status != "cancel")))
                    .Select(r => r.ID)
                    .ToListAsync();
                return await db.Rooms.Include(r => r.Bookings)
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.SuppliesForRooms.Select(s => s.Supply))
                    .Where(r => !list.Contains(r.ID)).ToListAsync();
            }
        }
    }
}