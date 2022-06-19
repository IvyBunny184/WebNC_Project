using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.ViewModel
{
    public class HomePageViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }

        public IEnumerable<Voucher> Vouchers { get; set; }

        public IEnumerable<Service> Services { get; set; }
    }
}