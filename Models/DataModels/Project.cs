using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekWear.Models
{
	public enum Size
	{
		XS = 0,
		S = 1,
		M = 2,
		L = 3,
		XL = 4,
		XXL = 5,
		XXXL = 6
	}
	public class Project
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Transform { get; set; }

		public string TextInput { get; set; }

		[Required]
		public string ShirtColor { get; set; }

		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }

		public Size Size { get; set; }
		[NotMapped]
		public string SizeString
		{
			get
			{
				return Size.ToString();
			}
		}
	}
}
