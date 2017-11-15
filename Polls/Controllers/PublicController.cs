using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using Polls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Polls.Controllers
{



    // [Route(Name ="Public")]
    // [RoutePrefix("Public")]
    //  [Route("{action=index}")] //Specifies the Index action as default for this route prefix
    public class PublicController : Controller
    {

        #region Route public, ""
        [Route("")]
        [Route("public")]
        public ActionResult PublicIndex()
        {
            // return Redirect("~/Public/c/catname=All");
            return RedirectToRoute("PublicDefaultPage", new { catname = "All" });

        }
        #endregion
        #region Route  [Route("Public/c/{catname?}" replaced with [Route("public/category/        
        [Route("public/category/{catname?}", Name = "PublicDefaultPage")]
        public ActionResult Index(int? page, string catname = "")
        {

            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetCategories");
            var request_Categories = new RestRequest(Method.POST);
            request_Categories.AddHeader("content-type", "application/json");
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
                if (!string.IsNullOrEmpty(catname) && catname != "All" && catname != "catname=All")
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();
            }
            Session["PageUrl"] = "UserName";

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }
        #endregion
        #region Route Public/u/ replaced with public/user/
        [Route("public/user/{username}")]
        public ActionResult UserName(int? page, string userId = "", string username = "")
        {
            ViewBag.UserName = username;
            var response = GetPublicPolls(page);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetCategories");
            var request_Categories = new RestRequest(Method.POST);
            request_Categories.AddHeader("content-type", "application/json");
            IRestResponse<List<GetCategoriesResponse>> response_Categories = client_Categories.Execute<List<GetCategoriesResponse>>(request_Categories);
            if (response_Categories.StatusCode.ToString() == "OK" && response_Categories.Data != null)
            {
                ViewBag.Categories = response_Categories.Data;
            }
            else
            {
                ViewBag.Categories = null;
            }

            if (!string.IsNullOrEmpty(userId))
                pools = response.Where(x => x.userId.ToLower() == userId.ToLower()).ToList();
            else
                pools = response.ToList();

            Session["PageUrl"] = "UserName";

            return View(pools.ToPagedList(page ?? 1, 20));
        }
        #endregion
        #region GetPublicPolls
        private List<MyPolls> GetPublicPolls(int? page)
        {
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            if (page == 0)
            {
                pollsparameter.pageSize = 20;
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
        #endregion
        #region Route("public/polls/category/sub-category/{question}"
        [Route("public/polls/category/sub-category/{question}")]
        public ActionResult GetPollResult(string question)
        {

            PollResultViewModel pollResultviewModel = new PollResultViewModel();
            string pollId = string.Empty;
            if (Session["question"] != null)
            {
                pollId = Session["question"].ToString();
            }
            else
            {
                pollId = question;
            }
            if (pollId == null)
            {
                // return RedirectToAction("Index", "Book");
                return Redirect("~/Public");
            }
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPollResult");
            GetPollRequest requestbody = new GetPollRequest();
            requestbody.viewAll = true;
            requestbody.pollId = Convert.ToInt32(pollId);


            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(requestbody);

            IRestResponse<List<PollResult>> response = client.Execute<List<PollResult>>(request);
            if (response.StatusCode.ToString() == "OK" && response.Data != null)
            {
                pollResultviewModel.PollResults = response.Data.OrderBy(c => c.choice).ToList();
            }



            List<MyPolls> response_poll = GetPublicPolls(0);
            if (response_poll != null)
            {
                // pollResultviewModel.myPolls = response_poll.Data;
                var pollresult = response_poll.Where(m => m.pollId == Convert.ToInt32(pollId)).FirstOrDefault();
                if (pollresult != null)
                {
                    var image = string.IsNullOrEmpty(pollresult.firstImagePath) ? "" : pollresult.firstImagePath;
                    var secimage = string.IsNullOrEmpty(pollresult.secondImagePath) ? "" : pollresult.secondImagePath;
                    pollresult.firstImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.firstImagePath;
                    pollresult.secondImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.secondImagePath;
                    pollresult.firstImagePathFull = Common.CommonUtility.FullImageBaseUrl + image;
                    pollresult.secondImagePathfull = Common.CommonUtility.FullImageBaseUrl + secimage;
                    pollResultviewModel.myPolls = pollresult;
                }
                return View(pollResultviewModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }
        #endregion
        #region Set Id's In Session due to show required var in URL
        /// <summary>
        /// Set user
        /// </summary>
        /// <param name="userId">GUID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetProfileId(string userId)
        {
            Session["userId"] = userId;
            return Json("{'success':'true' }", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Set Poll Id
        /// </summary>
        /// <param name="pollId">poll Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetPollId(string pollId)
        {
            Session["question"] = pollId;
            return Json("{'success':'true' }", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region [Route("Public/Viewprofile/ replaced with [Route("public/profiles/
        [Route("public/profiles/{userName}")]
        public ActionResult Viewprofile(string userName)
        {
            string userId = string.Empty;
            if (Session["userId"] != null)
            {
                userId = Session["userId"].ToString();
            }
            else
            {
                userId = userName;
            }
            if (userId == "")
            {
                return RedirectToAction("Index");
            }
            ViewPublicProfile obj = new ViewPublicProfile();
            obj.id = userId;
            obj.viewAll = true;
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/ViewPublicProfile");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(obj);
            request.AddHeader("content-type", "application/json");
            IRestResponse<ViewPublicProfileResponse> response = client.Execute<ViewPublicProfileResponse>(request);
            ViewPublicProfileResponse profile = new Models.ViewPublicProfileResponse();
            if (response.StatusCode.ToString() == "OK")
            {
                // profile = response.Data;
                profile = JsonConvert.DeserializeObject<ViewPublicProfileResponse>(response.Content);

            }
            PublicViewProfileViewModel viewmodel = new PublicViewProfileViewModel();

            var list_poll = GetPublicPolls(0);
            ViewBag.count = list_poll.Count();
            viewmodel.MyPolls = list_poll.Take(5).ToList();
            viewmodel.PublicProfileResponse = profile;
            return View(viewmodel);

        }
        #endregion
        #region [Route("Public/u/All" replaced with [Route("public/profiles/",
        [Route("public/profiles/", Name = "PublicUser")]
        public ActionResult PublicUserList(int? page)
        {

            Session["PageUrl"] = "UserNameAll";
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/ListPublicUsers");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<ViewPublicProfileResponse>> response = client.Execute<List<ViewPublicProfileResponse>>(request);
            List<ViewPublicProfileResponse> profile = new List<ViewPublicProfileResponse>();
            JsonItem<ViewPublicProfileResponse> profiles = new JsonItem<ViewPublicProfileResponse>();
            if (response.StatusCode.ToString() == "OK")
            {
                // JObject jobject = JObject.Parse(response.Content);

                //var resultObjects = AllChildren(JObject.Parse(response.Content))
                //.First(c => c.Type == JTokenType.Array && c.Path.Contains(jobject.Path))
                //.Children<JObject>();
                //foreach (var obj in resultObjects)
                //    profile.Add(JsonConvert.DeserializeObject<ViewPublicProfileResponse>(obj.ToString()));
                // profiles = JsonConvert.DeserializeObject<ViewPublicProfileResponse[]>(response.Content);

                profiles = Common.CommonUtility.DoSerialize<JsonItem<ViewPublicProfileResponse>>(response.Content);
                // var myname = DeserializeNames<JsonItemProperty<ViewPublicProfileResponse>(),Content.ToString());
            }

            return View(profiles.item.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }
        #endregion
        #region public/polls
        [Route("public/polls", Name = "PublicPolls")]
        public ActionResult PublicPolls(int? page)
        {
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            if (page == 0)
            {
                pollsparameter.pageSize = 20;
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
            List<MyPolls> pools = null;
            if (response.StatusCode.ToString() == "OK")
                pools = response.Data;

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }
        #endregion
        #region AllChildren
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
        #endregion

        #region Code Backup
        #region GetPollResult
        //[Route("Public/GetPollResult")]
        //public ActionResult GetPollResult(string pollId)
        //{
        //    PollResultViewModel pollResultviewModel = new PollResultViewModel();
        //    if (pollId == null)
        //    {
        //        // return RedirectToAction("Index", "Book");
        //        return Redirect("~/Public");
        //    }
        //    var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPollResult");
        //    GetPollRequest requestbody = new GetPollRequest();
        //    requestbody.viewAll = true;
        //    requestbody.pollId = Convert.ToInt32(pollId);
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddJsonBody(requestbody);
        //    IRestResponse<List<PollResult>> response = client.Execute<List<PollResult>>(request);
        //    if (response.StatusCode.ToString() == "OK" && response.Data != null)
        //    {
        //        pollResultviewModel.PollResults = response.Data.OrderBy(c => c.choice).ToList();
        //    }
        //    List<MyPolls> response_poll = GetPublicPolls(0);
        //    if (response_poll != null)
        //    {
        //        // pollResultviewModel.myPolls = response_poll.Data;
        //        var pollresult = response_poll.Where(m => m.pollId == Convert.ToInt32(pollId)).FirstOrDefault();
        //        if (pollresult != null)
        //        {
        //            var image = string.IsNullOrEmpty(pollresult.firstImagePath) ? "" : pollresult.firstImagePath;
        //            var secimage = string.IsNullOrEmpty(pollresult.secondImagePath) ? "" : pollresult.secondImagePath;
        //            pollresult.firstImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.firstImagePath;
        //            pollresult.secondImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.secondImagePath;
        //            pollresult.firstImagePathFull = Common.CommonUtility.FullImageBaseUrl + image;
        //            pollresult.secondImagePathfull = Common.CommonUtility.FullImageBaseUrl + secimage;
        //            pollResultviewModel.myPolls = pollresult;
        //        }
        //        return View(pollResultviewModel);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
        //    }
        //    return View(); // modify as per your need
        //}
        #endregion
        #region PublicPolls
        //[Route("Public/PublicPolls", Name = "PublicPolls")]
        //public ActionResult PublicPolls(int? page)
        //{
        //    GetMyPollsModel pollsparameter = new GetMyPollsModel();
        //    if (page == 0)
        //    {
        //        pollsparameter.pageSize = 20;
        //        pollsparameter.pageNumber = 1;
        //    }
        //    else
        //    {
        //        pollsparameter.pageSize = 20;
        //        pollsparameter.pageNumber = (page ?? 1);

        //    }

        //    var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddJsonBody(pollsparameter);
        //    IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
        //    List<MyPolls> pools = null;
        //    if (response.StatusCode.ToString() == "OK")
        //        pools = response.Data;

        //    return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        //}
        #endregion
        #endregion
    }


}