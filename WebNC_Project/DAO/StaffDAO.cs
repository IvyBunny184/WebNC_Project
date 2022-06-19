using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class StaffDAO
    {
        public static async Task<IEnumerable<Staff>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Staffs.Include(s => s.Permission).ToListAsync();
            }
        }

        public static async Task<Staff> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Staffs.Include(s => s.Permission).SingleOrDefaultAsync(s => s.ID == id);
            }
        }

        public static async Task<int> Create(Staff staff)
        {
            using (ResortContext db = new ResortContext())
            {
                db.Staffs.Add(staff);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Staff staff = await db.Staffs.FindAsync(id);
                db.Staffs.Remove(staff);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Staff staff)
        {
            using (ResortContext db = new ResortContext())
            {
                Staff enti = await db.Staffs.FindAsync(staff.ID);
                if (enti != null)
                {
                    enti.Name = staff.Name;
                    enti.Birth = staff.Birth;
                    enti.Gender = staff.Gender;
                    enti.Password = staff.Password;
                    enti.PermissionID = staff.PermissionID;
                    enti.Phone = staff.Phone;
                    enti.Email = staff.Email;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}