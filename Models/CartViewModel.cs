using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace GeekWear.Models
{
	//public enum CardType { Visa, Mastercard, Discover, Amex, Switch, Solo, Maestro }

	public class PaymentCartViewModel
	{
		//[Required, Display(Name = "Name on Card")]
		//public string NameOnCard { get; set; }

		//[Required, Display(Name = "Card Number")]
		//public string CardNumber { get; set; }

		//[Required, Display(Name = "Card Type")]
		//public CardType? CardType { get; set; }

		//[Required, Display(Name = "Expiry Date")]
		//public int ExpiryDate { get; set; }

		//public int ExpiryYear { get; set; }

		//[Required, Display(Name = "CVV")]
		//public string Cvv { get; set; }
		public string ErrorMessage { get; set; }

		public CartViewModel Cart { get; set; }
	}

	public class CartViewModel
	{
		public CartViewModel()
		{
			Items = new List<CartViewModelItem>();
		}
		public decimal ShippingCost { get; set; }

		public List<CartViewModelItem> Items { get; set; }
	}

	public class CartViewModelItem
	{
		public int Count { get; set; }
		public Project Project { get; set; }
	}
}