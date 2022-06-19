using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class PermissionDAO
    {
        public static async Task<IEnumerable<Permission>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Permissions.ToListAsync();
            }
        }
    }
}