using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace WebNC_Project.DAO
{
    public class VoucherDAO
    {
        public static async Task<IEnumerable<Voucher>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Vouchers.ToListAsync();
            }
        }

        public static async Task<IEnumerable<Voucher>> GetAvailable()
        {
            using (ResortContext db = new ResortContext())
            {
                var now = DateTime.Now;
                return await db.Vouchers
                    .Where(v => DbFunctions.TruncateTime(v.ToDate) >= now.Date)
                    .ToListAsync();
            }
        }

        public static async Task<IEnumerable<Voucher>> Search(string code)
        {
            using (ResortContext db = new ResortContext())
            {
                var now = DateTime.Now;
                return await db.Vouchers
                    .Where(v => DbFunctions.TruncateTime(v.ToDate) >= now.Date && v.Code.Contains(code))
                    .ToListAsync();
            }
        }

        public static async Task<Voucher> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Vouchers.FindAsync(id);
            }
        }

        public static async Task<int> Create(Voucher voucher)
        {
            using (ResortContext db = new ResortContext())
            {
                voucher.Code = voucher.Code.Trim().ToUpper();
                db.Vouchers.Add(voucher);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Voucher voucher = await db.Vouchers.FindAsync(id);
                db.Vouchers.Remove(voucher);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Voucher voucher)
        {
            using (ResortContext db = new ResortContext())
            {
                Voucher enti = await db.Vouchers.SingleOrDefaultAsync(s => s.Code.ToLower() == voucher.Code.Trim().ToLower());
                if (enti != null)
                {
                    enti.Condition = voucher.Condition;
                    enti.Discount = voucher.Discount;
                    enti.FromDate = voucher.FromDate;
                    enti.ToDate = voucher.ToDate;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}