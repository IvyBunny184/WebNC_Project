using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebNC_Project.DAO
{
    public static class ServiceDAO
    {
        public static async Task<IEnumerable<Service>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Services.ToListAsync();
            }
        }

        public static async Task<IEnumerable<Service>> Search(string name)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Services.Where(s => s.Name.Contains(name)).ToListAsync();
            }
        }

        public static async Task<Service> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Services.FindAsync(id);
            }
        }

        public static async Task<int> Create(Service service)
        {
            using (ResortContext db = new ResortContext())
            {
                service.ID = service.ID.Trim().ToUpper();
                service.Description = service.Description.Trim();
                db.Services.Add(service);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Service service = await db.Services.FindAsync(id);
                db.Services.Remove(service);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Service service)
        {
            using (ResortContext db = new ResortContext())
            {
                Service enti = await db.Services.SingleOrDefaultAsync(s => s.ID == service.ID);
                if (enti != null)
                {
                    enti.Name = service.Name;
                    enti.Description = service.Description;
                    enti.Price = service.Price;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}