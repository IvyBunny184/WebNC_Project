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
    public static class CustomerDAO
    {
        public static async Task<IEnumerable<Customer>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Customers.ToListAsync();
            }
        }

        public static async Task<Customer> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Customers.FindAsync(id);
            }
            
        }

        public static async Task<int> Create(Customer customer)
        {
            using (ResortContext db = new ResortContext())
            {
                db.Customers.Add(customer);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Customer customer = await db.Customers.FindAsync(id);
                db.Customers.Remove(customer);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Customer customer)
        {
            using (ResortContext db = new ResortContext())
            {
                Customer enti = await db.Customers.FindAsync(customer.ID);
                if (enti != null)
                {
                    enti.Name = customer.Name;
                    enti.Birth = customer.Birth;
                    enti.Gender = customer.Gender;
                    enti.Password = customer.Password;
                    enti.Phone = customer.Phone;
                    enti.Email = customer.Email;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }

        public static async Task<int> ChangePass(string id, string pass)
        {
            using(ResortContext db = new ResortContext())
            {
                Customer model = await db.Customers.FindAsync(id);
                model.Password = pass;
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<bool> CanBooking(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                var invoices = await db.Bookings.Where(b => b.CustomerID == id).OrderByDescending(b => b.ID).FirstOrDefaultAsync();
                if (invoices == null || invoices.Status == "cancel" || invoices.Status == "checkout") return true;
                DateTime date = DateTime.Now;
                if (invoices.Status == "payment" && invoices.CheckoutDate.Date < DateTime.Now.Date) return true;
                return false;
            }
        }
    }
}