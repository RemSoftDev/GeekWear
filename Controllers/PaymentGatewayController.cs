using GeekWear.Models;
using GeekWear.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekWear.Controllers
{
	public class PaymentGatewayController : Controller
	{
		private ApplicationDbContext _db = new ApplicationDbContext();

		// GET: PaymentGateway
		public ActionResult ExpressCheckoutSuccess(string token, string orderId)
		{
			if (string.IsNullOrEmpty(token))
			{
				return RedirectToAction("index", "home");
			}

			if (string.IsNullOrEmpty(orderId))
			{
				return RedirectToAction("index", "home");
			}

			var orderModel = OrderStrCrypto.Decrypt(orderId);
			if (orderModel == null)
			{
				return RedirectToAction("index", "home");
			}

			var order = _db.Orders.FirstOrDefault(x => x.Id == orderModel.OrderId && x.UserId == orderModel.UserId);

			if (order == null)
			{
				return RedirectToAction("index", "home");
			}

			var result = PayPalService.GetPayment(token);
			order.Status = result.Success ? OrderStatus.Success : OrderStatus.Fail;
			order.OrderDate = result.OrderDate;
			order.TotalCost = result.Success ? result.OrderTotal : (decimal?)null;
			_db.SaveChanges();

			if (result.Success)
			{
				Session["cart"] = null;
				return RedirectToAction("Thankyou", "Cart");
			}
			else
			{
				Session["cart"] = null;
				return RedirectToAction("Index", "Cart", new { message = result.ErrorMessage });
			}
		}

		// GET: PaymentFailure
		public ActionResult PaymentFailure(string token, string orderId)
		{
			if (string.IsNullOrEmpty(token))
			{
				return RedirectToAction("index", "home");
			}

			if (string.IsNullOrEmpty(orderId))
			{
				return RedirectToAction("index", "home");
			}

			var orderModel = OrderStrCrypto.Decrypt(orderId);
			if (orderModel == null)
			{
				return RedirectToAction("index", "home");
			}

			var order = _db.Orders.FirstOrDefault(x => x.Id == orderModel.OrderId && x.UserId == orderModel.UserId);

			if (order == null)
			{
				return RedirectToAction("index", "home");
			}

			var result = PayPalService.GetPayment(token);
			order.Status = OrderStatus.Canceled;
			order.OrderDate = DateTimeOffset.UtcNow;
			_db.SaveChanges();

			return RedirectToAction("index", "Cart");
		}
	}
}