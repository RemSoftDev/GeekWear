using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Linq;
using GeekWear.Services;

namespace GeekWear.Models
{
	[Authorize]
	public class CartController : Controller
	{
		private ApplicationDbContext _db = new ApplicationDbContext();
		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		// GET: Cart
		public ActionResult Index(string message)
		{
			var cart = (CartViewModel)Session["Cart"];
			//if (cart == null) return RedirectToAction("Index");

			var model = new PaymentCartViewModel
			{
				Cart = cart,
				ErrorMessage = message
			};
			return View("Cart", model);
		}

		// POST Payment
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(PaymentCartViewModel model)
		{
			var cart = (CartViewModel)Session["Cart"];
			//if (cart == null) return RedirectToAction("Index");
			model.Cart = cart;

			if (ModelState.IsValid)
			{
				var userId = User.Identity.GetUserId();
				var user = UserManager.FindById(userId);

				var order = new Order
				{
					UserId = user.Id,
					Projects = model.Cart.Items.Select(x => x.Project).ToList()
				};
				_db.Orders.Add(order);
				_db.SaveChanges();

				var orderCrypto = new OrderStrCrypto
				{
					OrderId = order.Id,
					UserId = user.Id
				};
				var orderId = orderCrypto.Encrypt();
				var paymentFailureUrl = string.Format("{0}://{1}/PaymentGateway/PaymentFailure?orderId={2}", Request.Url.Scheme, Request.Url.Authority, orderId);
				var expressCheckoutSuccessUrl = string.Format("{0}://{1}/PaymentGateway/ExpressCheckoutSuccess?orderId={2}", Request.Url.Scheme, Request.Url.Authority, orderId);
				var result = PayPalService.Pay(user.Email, model.Cart, expressCheckoutSuccessUrl, paymentFailureUrl);
				if (result.Success)
				{
					return Redirect(result.RedirectUrl);
				}
				else
				{
					model.ErrorMessage = result.ErrorMessage;
				}
			}
			return View("Cart", model);
		}

		// GET: Thankyou
		public ActionResult Thankyou(string redirectUrl)
		{
			return View();
		}

	}
}