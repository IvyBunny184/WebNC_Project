namespace WebNC_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.String(nullable: false, maxLength: 20),
                        RoomID = c.String(nullable: false, maxLength: 10),
                        CheckinDate = c.DateTime(nullable: false),
                        CheckoutDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        Adult = c.Int(nullable: false),
                        Child = c.Int(nullable: false),
                        VoucherCode = c.String(maxLength: 20),
                        FeedBack = c.String(),
                        Rate = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .ForeignKey("dbo.Vouchers", t => t.VoucherCode)
                .Index(t => t.CustomerID)
                .Index(t => t.RoomID)
                .Index(t => t.VoucherCode);
            
            CreateTable(
                "dbo.BookingServices",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        ServiceID = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.BookingID, t.ServiceID })
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 50),
                        Birth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(),
                        Gender = c.Boolean(nullable: false),
                        Password = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 30),
                        TypeID = c.String(nullable: false, maxLength: 10),
                        Description = c.String(nullable: false),
                        Status = c.String(),
                        Adult = c.Int(nullable: false),
                        Child = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RoomTypes", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        URL = c.String(nullable: false, maxLength: 450),
                        RoomID = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.URL)
                .ForeignKey("dbo.Rooms", t => t.RoomID)
                .Index(t => t.RoomID);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        NameType = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SuppliesForRooms",
                c => new
                    {
                        RoomID = c.String(nullable: false, maxLength: 10),
                        SupplyID = c.String(nullable: false, maxLength: 10),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoomID, t.SupplyID })
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .ForeignKey("dbo.Supplies", t => t.SupplyID, cascadeDelete: true)
                .Index(t => t.RoomID)
                .Index(t => t.SupplyID);
            
            CreateTable(
                "dbo.Supplies",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 30),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 20),
                        Discount = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ToDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Condition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 50),
                        Birth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Gender = c.Boolean(nullable: false),
                        PermissionID = c.String(maxLength: 10),
                        Password = c.String(nullable: false, maxLength: 20),
                        Email = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Permissions", t => t.PermissionID)
                .Index(t => t.PermissionID);
            
            CreateTable(
                "dbo.Rules",
                c => new
                    {
                        RuleCode = c.String(nullable: false, maxLength: 10),
                        Description = c.String(),
                        Condition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RuleCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.Bookings", "VoucherCode", "dbo.Vouchers");
            DropForeignKey("dbo.SuppliesForRooms", "SupplyID", "dbo.Supplies");
            DropForeignKey("dbo.SuppliesForRooms", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "TypeID", "dbo.RoomTypes");
            DropForeignKey("dbo.Images", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Bookings", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Bookings", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.BookingServices", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.BookingServices", "ServiceID", "dbo.Services");
            DropIndex("dbo.Staffs", new[] { "PermissionID" });
            DropIndex("dbo.SuppliesForRooms", new[] { "SupplyID" });
            DropIndex("dbo.SuppliesForRooms", new[] { "RoomID" });
            DropIndex("dbo.Images", new[] { "RoomID" });
            DropIndex("dbo.Rooms", new[] { "TypeID" });
            DropIndex("dbo.BookingServices", new[] { "ServiceID" });
            DropIndex("dbo.BookingServices", new[] { "BookingID" });
            DropIndex("dbo.Bookings", new[] { "VoucherCode" });
            DropIndex("dbo.Bookings", new[] { "RoomID" });
            DropIndex("dbo.Bookings", new[] { "CustomerID" });
            DropTable("dbo.Rules");
            DropTable("dbo.Staffs");
            DropTable("dbo.Permissions");
            DropTable("dbo.Vouchers");
            DropTable("dbo.Supplies");
            DropTable("dbo.SuppliesForRooms");
            DropTable("dbo.RoomTypes");
            DropTable("dbo.Images");
            DropTable("dbo.Rooms");
            DropTable("dbo.Customers");
            DropTable("dbo.Services");
            DropTable("dbo.BookingServices");
            DropTable("dbo.Bookings");
        }
    }
}
