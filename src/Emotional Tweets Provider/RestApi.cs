using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emotional_Tweets_Provider.Contracts;
using Emotional_Tweets_Model;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
namespace Emotional_Tweets_Provider
{
    public class RestApi : IRestApi
    {
        private string restApiUrl = "https://api.twitter.com/1.1/search/tweets.json?src=typd&q=";
        //example = "https://api.twitter.com/1.1/search/tweets.json?src=typd&q=%40twitterapi"
        public List<Tweet> GetTweets(string input)
        { 
            
            List<Tweet> result = new List<Tweet>();
            string request = createRequest(input);
            string response = MakeRequest(request);
            return result;
        }





        private string createRequest(string queryString)
        {
            string UrlRequest = restApiUrl + System.Web.HttpUtility.HtmlEncode(queryString);
            return (UrlRequest);
        }

        private string MakeRequest(string requestUrl)
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                
                //{
                //    if (response.StatusCode != HttpStatusCode.OK)
                //        throw new Exception(String.Format(
                //        "Server error (HTTP {0}: {1}).",
                //        response.StatusCode,
                //        response.StatusDescription));
                //    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                //    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                //    Response jsonResponse
                //    = objResponse as Response;
                //    return jsonResponse;
                //}
                return "";
            }
    
        }
    }

