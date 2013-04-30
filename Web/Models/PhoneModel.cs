using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
	public class PhoneModel
	{
		[Required]
		public string Phone { get; set; }
		public string Text { get; set; }
	}
}