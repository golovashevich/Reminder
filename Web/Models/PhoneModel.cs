using Resources;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
	public class PhoneModel
	{
		[Display(Name = "ModelPhone", ResourceType = typeof(Phone))]
		[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Validation))]
		public string Phone { get; set; }

		[Display(Name = "ModelText", ResourceType = typeof(Phone))]
		[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Validation))]
		public string Text { get; set; }
	}
}