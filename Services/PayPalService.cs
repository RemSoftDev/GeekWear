using System.Linq;
using GeekWear.Models;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using PayPal.Permissions.Model;
using System;
using System.Collections.Generic;

namespace GeekWear.Services
{
	public class PayPalResponceModel
	{
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }
		public string RedirectUrl { get; set; }
	}
	public class PayPalConfirmPaymentModel
	{
		public bool Success { get; set; }
		public string PaymentRequestID { get; set; }
		public decimal OrderTotal { get; set; }
		public string ErrorMessage { get; set; }
		public DateTimeOffset? OrderDate { get; set; }
	}

	public static class PayPalService
	{
		public static PayPalConfirmPaymentModel GetPayment(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				throw new ArgumentNullException("token");
			}

			Dictionary<string, string> config = PayPal.Api.ConfigManager.Instance.GetProperties();
			PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(config);
			GetExpressCheckoutDetailsReq getECWrapper = new GetExpressCheckoutDetailsReq();
			getECWrapper.GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token);

			GetExpressCheckoutDetailsResponseType getECResponse = service.GetExpressCheckoutDetails(getECWrapper);

			DoExpressCheckoutPaymentRequestType request = new DoExpressCheckoutPaymentRequestType();
			DoExpressCheckoutPaymentRequestDetailsType requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
			request.DoExpressCheckoutPaymentRequestDetails = requestDetails;

			requestDetails.PaymentDetails = getECResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails;
			requestDetails.Token = getECResponse.GetExpressCheckoutDetailsResponseDetails.Token;
			requestDetails.PayerID = getECResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;

			DoExpressCheckoutPaymentReq wrapper = new DoExpressCheckoutPaymentReq();
			wrapper.DoExpressCheckoutPaymentRequest = request;
			DoExpressCheckoutPaymentResponseType doECResponse = service.DoExpressCheckoutPayment(wrapper);
			if (doECResponse.Ack == AckCodeType.SUCCESS)
			{
				var paymentInfo = doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.First();

				var confirmModel = new PayPalConfirmPaymentModel
				{
					Success = true,
					PaymentRequestID = paymentInfo.PaymentRequestID,
					OrderTotal = decimal.Parse(paymentInfo.GrossAmount.value),
					OrderDate = string.IsNullOrEmpty(paymentInfo.PaymentDate)
						? DateTimeOffset.Parse(paymentInfo.PaymentDate)
						: (DateTimeOffset?)null
				};
				return confirmModel;
			}
			return new PayPalConfirmPaymentModel
			{
				Success = false,
				ErrorMessage = string.Join(". ", doECResponse.Errors.Select(x => x.LongMessage).ToList())
			};
		}
		public static PayPalResponceModel Pay(string email, CartViewModel cart, string expressCheckoutSuccessUrl, string paymentFailureUrl)
		{
			Dictionary<string, string> config = PayPal.Api.ConfigManager.Instance.GetProperties();
			PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(config);

			SetExpressCheckoutRequestType setExpressCheckoutReqType = new SetExpressCheckoutRequestType();
			SetExpressCheckoutRequestDetailsType details = new SetExpressCheckoutRequestDetailsType();
			List<PaymentDetailsType> payDetails = new List<PaymentDetailsType>();
			PaymentDetailsType paydtl = new PaymentDetailsType();
			paydtl.PaymentAction = PaymentActionCodeType.ORDER;

			BasicAmountType shippingTotal = new BasicAmountType();
			shippingTotal.value = cart.ShippingCost.ToString();
			shippingTotal.currencyID = CurrencyCodeType.USD;

			decimal itemsTotal = 0.0M;

			List<PaymentDetailsItemType> lineItems = new List<PaymentDetailsItemType>();

			foreach (var cartItem in cart.Items)
			{
				PaymentDetailsItemType item = new PaymentDetailsItemType();
				BasicAmountType amt = new BasicAmountType();
				amt.currencyID = CurrencyCodeType.USD;

				amt.value = "50";
				item.Quantity = cartItem.Count;
				itemsTotal += 50 * cartItem.Count;

				item.Name = string.Format("{0} {1} Shirt", cartItem.Project.ShirtColor, cartItem.Project.SizeString);
				item.Amount = amt;

				lineItems.Add(item);
			}

			decimal orderTotal = itemsTotal + cart.ShippingCost;
			paydtl.OrderTotal = new BasicAmountType(CurrencyCodeType.USD, Convert.ToString(orderTotal));
			paydtl.PaymentDetailsItem = lineItems;

			paydtl.ShippingTotal = shippingTotal;
			paydtl.PaymentDetailsItem = lineItems;
			payDetails.Add(paydtl);

			details.BuyerEmail = email;
			details.PaymentDetails = payDetails;
			details.ReturnURL = expressCheckoutSuccessUrl;
			details.CancelURL = paymentFailureUrl;

			setExpressCheckoutReqType.SetExpressCheckoutRequestDetails = details;
			SetExpressCheckoutReq expressCheckoutReq = new SetExpressCheckoutReq();
			expressCheckoutReq.SetExpressCheckoutRequest = setExpressCheckoutReqType;
			SetExpressCheckoutResponseType response = null;
			try
			{
				response = service.SetExpressCheckout(expressCheckoutReq);
			}
			catch (Exception ex)
			{
				return new PayPalResponceModel
				{
					Success = false,
					ErrorMessage = ex.Message
				};
			}

			if (!response.Ack.ToString().Trim().ToUpper().Equals(AckCode.FAILURE.ToString()) && !response.Ack.ToString().Trim().ToUpper().Equals(AckCode.FAILUREWITHWARNING.ToString()))
			{
				var redirectUrl = string.Format("{0}_express-checkout&token={1}", config["paypalResultUrl"], response.Token);
				return new PayPalResponceModel
				{
					Success = true,
					RedirectUrl = redirectUrl
				};
			}

			return new PayPalResponceModel
			{
				Success = false,
				ErrorMessage = string.Join(". ", response.Errors.Select(x => x.LongMessage).ToList())
			};
		}
	}
}