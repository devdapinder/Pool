using Newtonsoft.Json;
using PagedList;
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
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", new { catname = "All" });
        }

        [Route("Home/Index/{catname}", Name = "UserHome")]
        public ActionResult Index(int? page, string catname = "", string username = "")
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginRespone.token);
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetCategories");
            var request_Categories = new RestRequest(Method.POST);
            request_Categories.AddHeader("content-type", "application/json");
            request_Categories.AddHeader("userid", loginRespone.userId);
            request_Categories.AddHeader("token", loginRespone.token);
            IRestResponse<List<GetCategoriesResponse>> response_Categories = client_Categories.Execute<List<GetCategoriesResponse>>(request_Categories);
            if (response_Categories.StatusCode.ToString() == "OK" && response_Categories.Data != null)
            {
                ViewBag.Categories = response_Categories.Data;
            }
            else
            {
                ViewBag.Categories = null;
            }

            if (response.StatusCode.ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(catname) && catname != "All")
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();

                if (!string.IsNullOrEmpty(username))
                {
                    pools = pools.Where(x => x.userName.ToLower() == username.ToLower()).ToList();
                }
            }

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }

        private ActionResult GetAllFilterCategories()
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            var client = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetAllFilterCategories");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("token", loginRespone.token);

            IRestResponse<List<GetAllFilterbyCategoryResponse>> response = client.Execute<List<GetAllFilterbyCategoryResponse>>(request);

            if (response.StatusCode.ToString() == "OK" && response.Data != null)
            {

                return View("Index", "Home"); // modify as per your need
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }

        public ActionResult GetPollResult(string pollId)
        {

            PollResultViewModel pollResultviewModel = new PollResultViewModel();
            if (pollId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            var client = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetPollResult");
            GetPollRequest requestbody = new GetPollRequest();
            requestbody.viewAll = true;
            requestbody.pollId = Convert.ToInt32(pollId);


            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("token", loginRespone.token);
            request.AddJsonBody(requestbody);

            IRestResponse<List<PollResult>> response = client.Execute<List<PollResult>>(request);
            if (response.StatusCode.ToString() == "OK" && response.Data != null)
            {
                pollResultviewModel.PollResults = response.Data.OrderBy(c => c.choice).ToList();
            }       
           List<MyPolls> response_poll = GetPublicPolls(0);
            if (response_poll!=null && response_poll.Count>0)
            {
                // pollResultviewModel.myPolls = response_poll.Data;
                var pollresult = response_poll.Where(m => m.pollId == Convert.ToInt32(pollId)).FirstOrDefault();
                if (pollresult != null)
                {
                    string FirstImage = pollresult.firstImagePath;
                    string secondImage = pollresult.secondImagePath;
                    pollresult.firstImagePath = Common.CommonUtility.ThumbnailBaseUrl + Convert.ToString(pollresult.firstImagePath);
                    pollresult.secondImagePath = Common.CommonUtility.ThumbnailBaseUrl + Convert.ToString(pollresult.secondImagePath);
                    pollresult.firstImagePathFull = Common.CommonUtility.FullImageBaseUrl + FirstImage;
                    pollresult.secondImagePathfull = Common.CommonUtility.FullImageBaseUrl + secondImage;
                    pollResultviewModel.myPolls = pollresult;
                }

                //  ViewBag.Question = pollresult.question;
                //  ViewBag.responseCompleted = pollresult.responseCompleted;
                //  return View(response.Data.OrderBy(m => m.choice).ToList()); // modify as per your need
                return View(pollResultviewModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }

        private List<MyPolls> GetPublicPolls(int? page)
        {
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            if (page == 0)
            {
                pollsparameter.pageSize = 100;
                pollsparameter.pageNumber = 1;
            }
            else
            {
                pollsparameter.pageSize = 20;
                pollsparameter.pageNumber = (page ?? 1);

            }

            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            if (response.StatusCode.ToString() == "OK")
                return response.Data;
            else
                return new List<MyPolls>();
        }

        public ActionResult MyPolls(int? page, string catname = "", string username = "")
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetMyPolls");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginRespone.token);
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            if (response.StatusCode.ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(catname))
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();

                if (!string.IsNullOrEmpty(username))
                {
                    pools = pools.Where(x => x.userName.ToLower() == username.ToLower()).ToList();
                }
            }

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }

        //[Route("Home/PublicUser", Name = "PublicUser")]
        //public ActionResult PublicUserList(int? page)
        //{
        //    LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
        //    GetMyPollsModel pollsparameter = new GetMyPollsModel();
        //    pollsparameter.pageSize = 20;
        //    pollsparameter.pageNumber = (page ?? 1);
        //    var client = new RestClient(Common.Common.ApirUrl + "Polls/GetListPublicProfile");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("token", loginRespone.token);
        //    request.AddHeader("userid", loginRespone.userId);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddJsonBody(pollsparameter);
        //    IRestResponse<List<ViewPublicProfileResponse>> response = client.Execute<List<ViewPublicProfileResponse>>(request);
        //    List<ViewPublicProfileResponse> profile = null;
        //    if (response.StatusCode.ToString() == "OK")
        //    {

        //        profile = JsonConvert.DeserializeObject<List<ViewPublicProfileResponse>>(response.Content);

        //    }

        //    return View(profile.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        //}
    }

    public class getMyPolRequest
    {
        public int pollId { get; set; }
    }
}