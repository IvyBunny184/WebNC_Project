using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication,ServerAuthorize("MANAGER", "STAFF")]
    public class BookingsController : Controller
    {
        // GET: Server/Bookings
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await BookingDAO.GetAll());
        }

        public async Task<ActionResult> Details(int id)
        {
            Booking result = await BookingDAO.GetByID(id);
            double price = result.Room.Price;
            foreach (var sv in result.BookingServices) price += sv.Service.Price;
            ViewBag.Price = price;
            return View(result);
        }

        [Obsolete]
        public async Task<List<SelectListItem>> GetCustomer()
        {
            var list = await BookingDAO.GetCustomerAvailable();
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var item in list) result.Add(new SelectListItem() { Value = item.ID, Text = item.IDWithName });
            return result;
        }

        public async Task<SelectList> GetRoom()
        {
            return new SelectList(await RoomDAO.GetAll(), "ID", "Display");
        }

        [Obsolete]
        public async Task<ActionResult> Create()
        {
            ViewBag.ListCus = await GetCustomer();
            ViewBag.ListRoom = await GetRoom();
            return View(new Booking()
            {
                CheckinDate = DateTime.Now.Date,
                CheckoutDate = DateTime.Now.AddDays(1).Date
            });
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Create(Booking model)
        {
            ViewBag.ListCus = await GetCustomer();
            ViewBag.ListRoom = await GetRoom();
            if (!ModelState.IsValid) return View(model);
            if (!(await CustomerDAO.CanBooking(model.CustomerID)))
            {
                ModelState.AddModelError("CustomerID", "Customer already have an order and can not book more");
                return View(model);
            }
            if (model.CheckinDate.Date >= model.CheckoutDate.Date)
            {
                ModelState.AddModelError("CheckinDate", "Check in date must smaller than check out date");
                return View(model);
            }
            if (!(await RoomDAO.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate)))
            {
                ModelState.AddModelError("RoomID", $"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
                return View(model);
            }
            var room = await RoomDAO.GetByID(model.RoomID);
            if (room.Adult < model.Adult)
            {
                ModelState.AddModelError("Adult", $"Max adult of room {model.RoomID} is {room.Adult}");
                return View(model);
            }
            if (room.Child < model.Child)
            {
                ModelState.AddModelError("Child", $"Max child of room {model.RoomID} is {room.Child}");
                return View(model);
            }
            string validCheckIn = Booking.ValidCheckin(model.CheckinDate, DateTime.Now);
            if (validCheckIn != null)
            {
                ModelState.AddModelError("CheckinDate", validCheckIn);
                return View(model);
            }
            if (model.ValidCheckout != null)
            {
                ModelState.AddModelError("CheckoutDate", model.ValidCheckout);
                return View(model);
            }
            if (model.VoucherCode != null)
            {
                var voucher = await VoucherDAO.GetByID(model.VoucherCode);
                if (voucher == null)
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {model.VoucherCode} does not existed");
                    return View(model);
                }
                if (!(voucher.FromDate.Date <= model.CheckinDate.Date && model.CheckinDate.Date <= voucher.ToDate.Date) &&
                !(voucher.FromDate.Date <= model.CheckoutDate.Date && model.CheckoutDate.Date <= voucher.ToDate.Date))
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {voucher.Code} can apply for bill booking between {voucher.FromDate.ToString("dd/MM/yyyyy")} and {voucher.ToDate.ToString("dd/MM/yyyyy")}");
                    return View(model);
                }
                model.Room = await RoomDAO.GetByID(model.RoomID);
                if (Booking.GetPrice(model,false) < voucher.Condition)
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {voucher.Code} can apply for bill with value equal or bigger than {voucher.Condition} VND");
                    return View(model);
                }
            }
            try
            {
                await BookingDAO.Create(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("RoomID", "An exception when create your booking request");
                return View(model);
            }
        }

        [Obsolete]
        public async Task<ActionResult> Edit(int id)
        {
            var result = await BookingDAO.GetByID(id);
            List<SelectListItem> listcus = await GetCustomer();
            Customer customer = await CustomerDAO.GetByID(result.CustomerID);
            listcus.Insert(0, new SelectListItem() { Text = customer.IDWithName, Value = customer.ID });
            ViewBag.ListCus = listcus;
            ViewBag.ListRoom = await GetRoom();
            return PartialView(result);
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Edit(Booking model)
        {
            List<SelectListItem> listcus = (List<SelectListItem>)await GetCustomer();
            Customer customer = await CustomerDAO.GetByID(model.CustomerID);
            listcus.Insert(0, new SelectListItem() { Text = customer.IDWithName, Value = customer.ID });
            ViewBag.ListCus = listcus;
            ViewBag.ListRoom = await GetRoom();
            if (!ModelState.IsValid) return PartialView(model);
            if (!(await RoomDAO.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate, model.ID)))
            {
                ModelState.AddModelError("RoomID", $"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
                return PartialView(model);
            }
            if (model.CheckinDate.Date >= model.CheckoutDate.Date)
            {
                ModelState.AddModelError("CheckinDate", "Check in date must smaller than check out date");
                return PartialView(model);
            }
            var room = await RoomDAO.GetByID(model.RoomID);
            if (room.Adult < model.Adult)
            {
                ModelState.AddModelError("Adult", $"Max adult of room {model.RoomID} is {room.Adult}");
                return PartialView(model);
            }
            if (room.Child < model.Child)
            {
                ModelState.AddModelError("Child", $"Max child of room {model.RoomID} is {room.Child}");
                return PartialView(model);
            }
            string validCheckIn = Booking.ValidCheckin(model.CheckinDate, model.CheckinDate);
            if (validCheckIn != null)
            {
                ModelState.AddModelError("CheckinDate", validCheckIn);
                return PartialView(model);
            }
            if (model.ValidCheckout != null)
            {
                ModelState.AddModelError("CheckoutDate", model.ValidCheckout);
                return PartialView(model);
            }
            if (model.VoucherCode != null)
            {
                var voucher = await VoucherDAO.GetByID(model.VoucherCode);
                if(voucher == null)
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {model.VoucherCode} does not existed");
                    return PartialView(model);
                }
                if (!(voucher.FromDate.Date <= model.CheckinDate.Date && model.CheckinDate.Date <= voucher.ToDate.Date) &&
                !(voucher.FromDate.Date <= model.CheckoutDate.Date && model.CheckoutDate.Date <= voucher.ToDate.Date))
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {voucher.Code} can apply for bill booking between {voucher.FromDate.ToString("dd/MM/yyyyy")} and {voucher.ToDate.ToString("dd/MM/yyyyy")}");
                    return PartialView(model);
                }
                var bill = await BookingDAO.GetByID(model.ID);
                model.Room = bill.Room;
                model.BookingServices = bill.BookingServices;
                if (Booking.GetPrice(model, false) < voucher.Condition)
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {voucher.Code} can apply for bill with value equal or bigger than {voucher.Condition} VND");
                    return PartialView(model);
                }
            }
            try
            {
                await BookingDAO.Edit(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ModelState.AddModelError("", $"Error when edit information of booking #{model.ID}");
                return PartialView(model);
            }
        }

        public async Task<ActionResult> CallServices(int id)
        {
            ViewBag.ID = id;
            ViewBag.ListSV = await BookingDAO.GetServicesOfBill(id);
            var result = await ServiceDAO.GetAll();
            return PartialView(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddSV(BookingServices model)
        {
            try
            {
                await BookingDAO.AddSV(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveSV(BookingServices model)
        {
            try
            {
                await BookingDAO.RemoveSV(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AcceptBill(int id, string status)
        {
            try
            {
                await BookingDAO.CheckBooking(id, status);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var e = ex;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await BookingDAO.Remove(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<MultiSelectList> ListServices()
        {
            return new SelectList(await ServiceDAO.GetAll(), "ID", "Name");
        }

        private async Task<SelectList> ListVoucher()
        {
            return new SelectList(await VoucherDAO.GetAvailable(), "Code");
        }
    }
}