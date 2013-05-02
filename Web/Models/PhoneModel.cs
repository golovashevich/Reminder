using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
	public class PhoneModel
	{
		[Required]
		public string Phone { get; set; }

		[Required]
		public string Text { get; set; }
	}
}