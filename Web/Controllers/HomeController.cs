using System;
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


		[HttpPost]
		public ActionResult Try(PhoneModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}
			string accountSid = "ACf351d8cc47408a1379f4fac824a76d8b";
			string authToken = "8fbca9931950d9c055e29e55f5a7f089";
			string verifiedNumber = "+1 951-200-5443";

			var twilio = new TwilioRestClient(accountSid, authToken);

			var options = new CallOptions();
			options.From = verifiedNumber;
			options.To =  model.Phone;

			var urlHelper = new UrlHelper(ControllerContext.RequestContext);
			options.Url = urlHelper.Action("CallConnectUrlResponder", null, null, Request.Url.Scheme);
			options.Url = "http://demo.twilio.com/docs/voice.xml";

			TwilioBase result;

			if (!String.IsNullOrEmpty(model.Text))
			{
				result = twilio.SendSmsMessage(options.From, options.To, model.Text);
			}
			else
			{
				result = twilio.InitiateOutboundCall(options);
			}

			if (null != result.RestException)
			{
				return RedirectToAction("Index", new {resultMessage = result.RestException.Message });
			}

			return RedirectToAction("Index", new { resultMessage = "Success!" });
		}
	}
}
