using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebNC_Project.Models
{
    public partial class ResortContext : DbContext
    {
        public ResortContext()
            : base("name=ResortContext")
        { }
        
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<SuppliesForRoom> SuppliesForRooms { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<BookingServices> BookingServices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasMany(e => e.BookingServices)
                .WithRequired(e => e.Booking)
                .HasForeignKey(e => e.BookingID);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.BookingServices)
                .WithRequired(e => e.Service)
                .HasForeignKey(e => e.ServiceID);

            modelBuilder.Entity<RoomType>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.RoomType)
                .HasForeignKey(e => e.TypeID);
        }
    }
}
