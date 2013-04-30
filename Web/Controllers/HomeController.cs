using System.Diagnostics;
using System.Web.Mvc;
using Twilio;
using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string resultMessage)
		{
			ViewBag.ResultMessage = resultMessage;

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}


		[HttpPost]
		public ActionResult Try(PhoneModel model)
		{
			Debug.WriteLine("Try method");
			string accountSid = "AC77d12b79ec308151eb23371ce27c0202";
			string authToken = "ecf3f79e6f5adaeab7ef2efe6abadd8e";

			string myNumber = "+380631205443";
			string twilioNumber1 = "+14158141829";
			string twilioNumber = "+1415-944-4090";
			string verifiedNumber = "+1 951-200-5443";
			string nirajNumber = "(908) 616-3168";

			var twilio = new TwilioRestClient(accountSid, authToken);

			var options = new CallOptions();
			options.From = nirajNumber;
			options.To = myNumber;

			var urlHelper = new UrlHelper(ControllerContext.RequestContext);
			options.Url = urlHelper.Action("CallConnectUrlResponder", null, null, Request.Url.Scheme);
			options.Url = "http://twilio.com";

			//var result = twilio.InitiateOutboundCall(options);

			var result = twilio.SendSmsMessage(nirajNumber, nirajNumber, "Test message from inside the application");
			if (null != result.RestException)
			{
				return RedirectToAction("Index", new { resultMessage = result.RestException.Message });
			}

			return RedirectToAction("Index", new { resultMessage = "Success!" });
		}

		private void CallConect(Call obj)
		{
			Debug.WriteLine("Call connect: " + obj.ToString());
		}


		public ActionResult CallConnectUrlResponder()
		{
			Debug.WriteLine("Call of url callback");
			return Json(new { success = true });
		}
	}
}
