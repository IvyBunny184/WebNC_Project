using System;
using System.Collections.Generic;
using System.Linq;
using WebNC_Project.Models;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebNC_Project.DAO
{
    public static class SupplyDAO
    {
        public static async Task<IEnumerable<Supply>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Supplies.ToListAsync();
            }
        }

        public static async Task<Supply> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Supplies.Include(s => s.SuppliesForRooms.Select(r => r.Room)).SingleOrDefaultAsync(s => s.ID == id);
            }
        }

        public static async Task<int> Create(Supply sup)
        {
            using (ResortContext db = new ResortContext())
            {
                sup.ID = sup.ID.Trim().ToUpper();
                db.Supplies.Add(sup);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Supply sup = await db.Supplies.FindAsync(id);
                db.Supplies.Remove(sup);
                return db.SaveChanges();
            }
        }

        public static async Task<int> Edit(Supply sup, string editType, int? count)
        {
            using(ResortContext db = new ResortContext())
            {
                Supply enti =
                    await db.Supplies
                    .Include(s => s.SuppliesForRooms)
                    .SingleOrDefaultAsync(s => s.ID == sup.ID);
                if (enti != null)
                {
                    switch (editType)
                    {
                        case "newcount":
                            enti.Total = (int)count;
                            foreach (var item in db.SuppliesForRooms) db.SuppliesForRooms.Remove(item);
                            break;
                        case "addcount":
                            enti.Total += (int)count;
                            break;
                        default: break;
                    }
                    enti.Name = sup.Name;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }

        public static async Task<int> GiveSupplyForRoom(string roomID, string supID, int count)
        {
            using(ResortContext db = new ResortContext())
            {
                SuppliesForRoom suppliesForRoom = new SuppliesForRoom()
                {
                    RoomID = roomID,
                    SupplyID = supID,
                    Count = count
                };
                Supply supply = await db.Supplies.FindAsync(supID);
                supply.Total -= count;
                db.SuppliesForRooms.Add(suppliesForRoom);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> TakeSupplyFromRoom(string roomID, string supID, int count)
        {
            using (ResortContext db = new ResortContext())
            {
                SuppliesForRoom suppliesForRoom =
                    await db.SuppliesForRooms
                    .SingleOrDefaultAsync(sr => sr.RoomID == roomID && sr.SupplyID == supID);
                Supply supply = await db.Supplies.FindAsync(supID);
                suppliesForRoom.Count -= count;
                supply.Total += count;
                return await db.SaveChangesAsync();
            }
        }
    }
}