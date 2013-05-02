using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Twilio;
using Twilio.TwiML;
using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		const string ACCOUNT_SID = "ACf351d8cc47408a1379f4fac824a76d8b";
		const string AUTH_TOKEN = "8fbca9931950d9c055e29e55f5a7f089";
		const string VERIFIED_NUMBER = "+1 951-200-5443";

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

			var twilio = new TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN);

			var options = new CallOptions();
			options.From = VERIFIED_NUMBER;
			options.To =  model.Phone;

			var urlHelper = new UrlHelper(ControllerContext.RequestContext);
			options.Url = urlHelper.Action("CallConnectUrlResponder", null, 
					new { tts = Server.UrlEncode(model.Text) }, 
					Request.Url.Scheme);

			twilio.SendSmsMessage(VERIFIED_NUMBER, "+380631205443", 
					String.Format("Try: {0}, {1}, {2}", model.Phone, model.Text, options.Url));

			TwilioBase result = twilio.InitiateOutboundCall(options);

			if (null != result.RestException)
			{
				return RedirectToAction("Index", new { resultMessage = result.RestException.Message });
			}

			return RedirectToAction("Index", new { resultMessage = "Success!" });
		}


		public HttpResponseMessage CallConnectUrlResponder()
		{
			Response.ContentType = "text/xml";
			string tts = Convert.ToString(Request["tts"]);
			tts = Server.UrlDecode(tts);

			new TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN).
					SendSmsMessage(VERIFIED_NUMBER, "+380631205443", "Callback: " + tts);

			var twilioResponse = new TwilioResponse();
			twilioResponse.Say(tts);
			twilioResponse.Hangup();

			return new HttpResponseMessage(HttpStatusCode.OK) { 
					Content = new StringContent(twilioResponse.Element.ToString()) 
				};
		}
	}
}
