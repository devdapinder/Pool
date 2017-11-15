using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Polls.Models;
using System.Web.Security;
using Polls.Common;

namespace Polls.Controllers
{
    public class LoginController : Controller
    {

       
        [HttpGet]
        [Route("public/login")]
        public ActionResult Index()
        {

            return View();// Redirect("Index");
        }
        [HttpPost]
        [Route("public/login")]
        public ActionResult Index(Login login)
        {
            login.deviceId = Guid.NewGuid().ToString();// "d4bc2ea4-1868-469b-a6c3-0f518e4f0218";
            login.isPartner = false;
            Newdevice obj = new Newdevice();
            obj.deviceVersion = Request.Browser.Version;// "59.0.3071.115";
            obj.browserVersion = Request.Browser.Version;// "59.0.3071.115";
            obj.browserName = Request.Browser.Browser;// "Chrome";
            obj.browserUserAgent = Request.UserAgent;// "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 59.0.3071.115 Safari / 537.36";
            obj.snsType = "Web";
            obj.deviceName = "Win32";
            obj.devicePlatform = Request.Browser.Platform;// "Windows";
            obj.snsDeviceId = Guid.NewGuid().ToString();// "d4bc2ea4-1868-469b-a6c3-0f518e4f0218";
            login.newDevice = obj;

            var client = new RestClient(Common.CommonUtility.ApirUrl+"Polls/Login");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("version", "1.0");

            request.AddJsonBody(login);

            IRestResponse<LoginResponse> response = client.Execute<LoginResponse>(request);          


            if (response.StatusCode.ToString() == "OK" && response.Data.userId != null)
            {
                Session["UserDetails"] = response.Data;
                Session["username"] = response.Data.displayName;
                LoggedInUserDetails.Username = response.Data.displayName;
                return RedirectToRoute("UserHome", new { catname = "All"});
            }
            else {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }


            return View();
        }
      
        public ActionResult LogOff()
        {
            Session.Abandon();
            Session.Clear();
            Session["UserDetails"] = null;
            return RedirectToAction("Index","Login");
        }

    }
}