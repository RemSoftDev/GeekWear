using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekWear.Models
{
	public enum OrderStatus
	{
		UnCompleted = 0,
		Success = 1,
		Fail = 2,
		Canceled = 3
	}

	public class Order
	{
		[Key]
		public int Id { get; set; }

		public OrderStatus Status { get; set; }

		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }

		public List<Project> Projects { get; set; }

		public DateTimeOffset? OrderDate { get; set; }

		public decimal? TotalCost { get; set; }
	}
}