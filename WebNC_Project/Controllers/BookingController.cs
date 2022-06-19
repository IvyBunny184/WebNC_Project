using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe.Infrastructure;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using WebNC_Project.App_Start;
using Stripe.Checkout;
using Stripe;
using WebNC_Project.ViewModel;

namespace WebNC_Project.Controllers
{
    [CustomerAuthentication]
    public class BookingController : Controller
    {
        // GET: Booking
        public async Task<ActionResult> Index()
        {
            var result = await BookingDAO.BookingsOfCustomer((string)Session["Customer"]);
            return View(result);
        }

        public async Task<ActionResult> Create(string room)
        {
            ViewBag.ListRoom = await ListRoom();
            DateTime today = DateTime.Now.AddDays(1);
            Booking booking = new Booking()
            {
                CustomerID = (string)Session["Customer"],
                RoomID = room,
                CheckinDate = today.Date,
                CheckoutDate = today.AddDays(1).Date
            };
            return View(booking);
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Create(Booking model)
        {
            ViewBag.ListRoom = await ListRoom();
            if (!ModelState.IsValid) return View(model);
            if(!(await CustomerDAO.CanBooking((string)Session["Customer"])))
            {
                ModelState.AddModelError("RoomID", "You already have a order and can not book more");
                return View(model);
            }
            if(model.CheckinDate.Date >= model.CheckoutDate.Date)
            {
                ModelState.AddModelError("CheckinDate", "Check in date must smaller than check out date");
                return View(model);
            }
            if(!(await RoomDAO.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate)))
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
                if(Booking.GetPrice(model, false) < voucher.Condition)
                {
                    ModelState.AddModelError("VoucherCode", $"Voucher {voucher.Code} can apply for bill with value equal or bigger than {voucher.Condition} VND");
                    return View(model);
                }
            }
            try
            {
                await BookingDAO.Create(model);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("RoomID", "An exception when create your booking request");
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.ID = id;
            ViewBag.ListRoom = await ListRoom();
            var result = await BookingDAO.GetByID(id);
            return PartialView(result);
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Edit(Booking model)
        {
            ViewBag.ListRoom = await ListRoom();
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
                if (voucher == null)
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
        public async Task<ActionResult> Cancel(int id)
        {
            var enti = await BookingDAO.GetByID(id);
            if(enti.Status != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                await BookingDAO.CheckBooking(id, "cancel");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Payment(int id)
        {
            ViewBag.Bill = await BookingDAO.GetByID(id);
            return PartialView();
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Payment(PaymentViewModel paymentInfo)
        {
            try
            {
                StripeConfiguration.SetApiKey("sk_test_51LCP3IDPE2cLj6JmdJgGWRZbtVHfaavulRrxpNQEuTgYZBHCY8szDOYGo4uu9qOI3Iq0U5UNwvdXBZTTmyFrlSgO00IC5xX4wQ");
                var bill = await BookingDAO.GetByID(paymentInfo.BillID);
                var customerInfo = new CustomerCreateOptions
                {
                    Description = paymentInfo.CardName,
                    Source = paymentInfo.StripeToken,
                    Email = paymentInfo.Email,
                    Metadata = new Dictionary<string, string>()
                {
                    { "Phone",paymentInfo.Phone }
                }
                };
                var customerService = new CustomerService();
                Stripe.Customer customer = customerService.Create(customerInfo);
                var options = new ChargeCreateOptions
                {
                    Amount = (long?)(Booking.GetPrice(bill) * 100 / 23000),
                    Currency = "usd",
                    Description = "Chare for bill #" + paymentInfo.BillID,
                    Customer = customer.Id
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                await BookingDAO.CheckBooking(paymentInfo.BillID, "payment");
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPayment");
            }
        }

        public ActionResult ErrorPayment()
        {
            return View();
        }

        public async Task<ActionResult> FeedBack(int id)
        {
            var billinfo = await BookingDAO.GetByID(id);
            FeedBackViewModel fb = new FeedBackViewModel()
            {
                BillID = id,
                Content = billinfo.FeedBack,
                Rate = (billinfo.Rate ?? 5)
            };
            return PartialView(fb);
        }

        [HttpPost]
        public async Task<ActionResult> FeedBack(FeedBackViewModel model)
        {
            if (!ModelState.IsValid) return PartialView(model);
            var billinfo = await BookingDAO.GetByID(model.BillID);
            if(billinfo == null)
            {
                ModelState.AddModelError("Content", "Bill not found");
                return PartialView(model);
            }
            try
            {
                await BookingDAO.FeedBack(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        #region List Select
        private async Task<SelectList> ListRoom()
        {
            return new SelectList(await RoomDAO.GetAll(), "ID", "Name");
        }

        private async Task<MultiSelectList> ListServices()
        {
            return new SelectList(await ServiceDAO.GetAll(), "ID", "Name");
        }

        private async Task<SelectList> ListVoucher()
        {
            return new SelectList(await VoucherDAO.GetAvailable(), "Code");
        }
        #endregion
    }
}