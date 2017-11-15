using Polls.Filters;
using Polls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polls.Controllers
{
    [CheckSession]
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///  View Profile 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewProfile(string userId)
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            var client = new RestClient(Common.CommonUtility.ApirUrl + "Polls/ViewProfile");
            var request = new RestRequest(Method.POST);
           
                request.AddHeader("token", loginRespone.token);
            if (userId!= null && userId != "")
                request.AddHeader("userid", userId);
            else
                request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");
            IRestResponse<Profile> response = client.Execute<Profile>(request);
            Profile profile = new Models.Profile();
            if (response.StatusCode.ToString() == "OK")
            {
                profile = response.Data;
            }

            return View(profile);
        }
        /// <summary>
        ///   Update Profile   
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ViewProfile(Profile profile)
        {
            if (ModelState.IsValid)
            {
                LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
                var client = new RestClient(Common.CommonUtility.ApirUrl + "Polls/UpdateProfile");
                var request = new RestRequest(Method.POST);
                request.AddHeader("token", loginRespone.token);
                request.AddHeader("userid", loginRespone.userId);
                request.AddJsonBody(profile.newProfileContact);
                request.AddHeader("content-type", "application/json");
                IRestResponse<UpdateProfile> response = client.Execute<UpdateProfile>(request);

                if (response.StatusCode.ToString() == "OK")
                {

                }
            }
            return View();
        }
    }
}