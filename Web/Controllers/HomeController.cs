using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;
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
			//This does not work on AppHarbor as composes internal representation (with specific server port), 
			//while application on AppHarbor responds on regular 80
			//options.Url = urlHelper.Action("CallConnectUrlResponder", null, 
			//		new { tts = Server.UrlEncode(model.Text) }, 
			//		Request.Url.Scheme);

			options.Url = "http://reminder-1.apphb.com/Home/CallConnectUrlResponder?tts=" 
					+ Server.UrlEncode(model.Text);

			twilio.SendSmsMessage(VERIFIED_NUMBER, "+380631205443",
					String.Format("Try: {0}, {1}, {2}", model.Phone, model.Text, options.Url));

			TwilioBase result = twilio.InitiateOutboundCall(options);

			if (null != result.RestException)
			{
				return RedirectToAction("Index", new { resultMessage = result.RestException.Message });
			}

			return RedirectToAction("Index", new { resultMessage = "Success!" });
		}


		public ActionResult CallConnectUrlResponder()
		{
			Response.ContentType = "text/xml";
			string tts = Convert.ToString(Request["tts"]);
			tts = Server.UrlDecode(tts);

			#region Diagnostics
			new TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN).
					SendSmsMessage(VERIFIED_NUMBER, "+380631205443", 
					String.Format("Callback: {0}, {1}", tts, Request.RawUrl));
			#endregion

			var twilioResponse = new TwilioResponse();
			twilioResponse.Say(tts);
			twilioResponse.Hangup();

			return new TwiMLResult(twilioResponse);
		}
	}
}
