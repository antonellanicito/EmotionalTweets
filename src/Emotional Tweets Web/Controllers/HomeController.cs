using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Emotional_Tweets_Model;
using Emotional_Tweets_Provider;
using Emotional_Tweets_Provider.Contracts;
namespace Emotional_Tweets_Web.Controllers
{
    public class HomeController : Controller
    {
        //
        private IRestApi restApi;




        
        public ActionResult Index()
        {
            List<Tweet> model = new List<Tweet>();
            try
            {
                if (!string.IsNullOrEmpty(Request.Form["txtSearch"]))
                {
                    restApi = new RestApi();
                    model = restApi.GetTweets(Request.Form["txtSearch"]);

                }
            }
            catch { }
            return View(model);
        }
           }
}
