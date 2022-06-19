using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebNC_Project.DAO
{
    public static class SuppliesForRoomDAO
    {
        public static async Task<IEnumerable<SuppliesForRoom>> GetSuppliesOfRoom(string roomID)
        {
            using(ResortContext db = new ResortContext())
            {
                return
                    await db.SuppliesForRooms
                    .Include(s => s.Supply)
                    .Where(s => s.RoomID == roomID)
                    .ToListAsync();
            }
        }
        public static async Task<IEnumerable<SuppliesForRoom>> GetRoomsOfSupply(string supID)
        {
            using (ResortContext db = new ResortContext())
            {
                return
                    await db.SuppliesForRooms
                    .Include(s => s.Room)
                    .Where(s => s.SupplyID == supID)
                    .ToListAsync();
            }
        }

        public static async Task<SuppliesForRoom> Single(string roomID, string supID)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.SuppliesForRooms.Where(sr => sr.RoomID == roomID && sr.SupplyID == supID).SingleOrDefaultAsync();
            }
        }

        public static async Task<int> Create(SuppliesForRoom model)
        {
            using (ResortContext db = new ResortContext())
            {
                var sup = await db.Supplies.FindAsync(model.SupplyID);
                sup.Total -= model.Count;
                db.SuppliesForRooms.Add(model);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> GiveSPForRoom(SuppliesForRoom model)
        {
            using (ResortContext db = new ResortContext())
            {
                SuppliesForRoom enti = await db.SuppliesForRooms.Where(sr => sr.RoomID == model.RoomID && sr.SupplyID == model.SupplyID).SingleOrDefaultAsync();
                if (enti == null) return await Create(model);
                else
                {
                    var sup = await db.Supplies.FindAsync(model.SupplyID);
                    sup.Total -= model.Count;
                    enti.Count += model.Count;
                }
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string roomID, string supID)
        {
            using (ResortContext db = new ResortContext())
            {
                var enti =
                    await db.SuppliesForRooms
                    .Include(s => s.Supply)
                    .FirstOrDefaultAsync(sr => sr.RoomID == roomID && sr.SupplyID == supID);
                Supply sp = await db.Supplies.FindAsync(supID);
                sp.Total += enti.Count;
                db.SuppliesForRooms.Remove(enti);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> RemoveSPFromRoom(SuppliesForRoom model)
        {
            using (ResortContext db = new ResortContext())
            {
                var enti = await db.SuppliesForRooms.Where(sr => sr.RoomID == model.RoomID && sr.SupplyID == model.SupplyID).SingleOrDefaultAsync();
                if (enti.Count <= model.Count) return await Remove(model.RoomID, model.SupplyID);
                Supply sp = await db.Supplies.FindAsync(model.SupplyID);
                sp.Total += model.Count;
                enti.Count -= model.Count; 
                return await db.SaveChangesAsync();
            }
        }
    }
}