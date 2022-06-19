using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class RoomTypeDAO
    {
        public static async Task<IEnumerable<RoomType>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.RoomTypes.Include(r => r.Rooms).ToListAsync();
            }
        }

        public static async Task<RoomType> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.RoomTypes.Include(r => r.Rooms).SingleOrDefaultAsync(r => r.ID == id);
            }
        }

        public static async Task<int> Create(RoomType type)
        {
            using (ResortContext db = new ResortContext())
            {
                type.ID = type.ID.Trim().ToUpper();
                db.RoomTypes.Add(type);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                RoomType type = await db.RoomTypes.FindAsync(id);
                db.RoomTypes.Remove(type);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(RoomType type)
        {
            using(ResortContext db = new ResortContext())
            {
                RoomType enti = await db.RoomTypes.FindAsync(type.ID);
                if (enti != null)
                {
                    enti.NameType = type.NameType;
                    return db.SaveChanges();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}