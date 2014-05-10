using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Emotional_Tweets_Model;
using Emotional_Tweets_Provider;

namespace Emotional_Tweets_Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            List<Tweet> model = new List<Tweet>();
            if (!string.IsNullOrEmpty(Request.Form["txtSearch"]))
            {
                RestApi restApi = new RestApi();
                model = restApi.GetTweets(Request.Form["txtSearch"]);
                    
            }
            return View(model);
        }
           }
}
