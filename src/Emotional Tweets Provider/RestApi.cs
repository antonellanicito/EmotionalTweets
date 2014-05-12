using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emotional_Tweets_Provider.Contracts;
using Emotional_Tweets_Model;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

namespace Emotional_Tweets_Provider

{
    
    public class RestApi : IRestApi
    {
        //OKKK
        private string Accesskey = "*****";
        private string Accesskeysecret = "****************";



        private string restApiUrl = "https://api.twitter.com/1.1/search/tweets.json?src=typd&q=";
        //example = "https://api.twitter.com/1.1/search/tweets.json?src=typd&q=%40twitterapi"
        private string mashapeApiUrl = "https://sentimentalsentimentanalysis.p.mashape.com/sentiment/current/classify_text/";
        private string twitterToken;
        private string mashapeToken;
        public RestApi()
        {
            //token = getToken();
            twitterToken = ConfigurationManager.AppSettings["token"];
            mashapeToken = ConfigurationManager.AppSettings["mashapeToken"];
        }

        public List<Tweet> GetTweets(string input)
        {

            List<Tweet> result = new List<Tweet>();
            HttpWebRequest request = Utils.WebManager.CreateRequest(restApiUrl, input, "GET", "Authorization", "Bearer  " + twitterToken, null);
            string response = Utils.WebManager.GetResponse(request);
            return transformJson(response);


        }


        #region "Private Mehods"

        private List<Tweet> transformJson(string json)
        {
            List<Tweet> result = new List<Tweet>();
            var serializer = new JavaScriptSerializer();
            List<Dictionary<string, Object>> statuses = ((ArrayList)serializer.Deserialize<Dictionary<string, Object>>(json).ToArray()[0].Value).ToArray()
                    .Select(c => (Dictionary<string, Object>)c).ToList();

            result = statuses.Select(c => new Tweet
                                        {
                                            Id = Convert.ToInt64(c["id"]),
                                            Text = c["text"].ToString(),
                                            language = c["lang"].ToString(),
                                            creationDate = decodeDate(c["created_at"].ToString()),
                                            User = new User((Dictionary<string, Object>)c["user"])
                                        })
                                        .Select(t => new Tweet
                                                {
                                                    Id = t.Id,
                                                    Text = t.Text,
                                                    language = t.language,
                                                    creationDate = t.creationDate,
                                                    User = t.User,
                                                    LevelHappyNess = GetTweetHappyness(t)
                                                }
                                                ).ToList();
            return result;

        }
        private HappyNess GetTweetHappyness(Tweet tweet)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("lang", tweet.language.ToLower());
                parameters.Add("text", tweet.Text);
                HttpWebRequest request = Utils.WebManager.CreateRequest(mashapeApiUrl, null, "POST", "X-Mashape-Authorization", mashapeToken, parameters);

                string response = Utils.WebManager.GetResponse(request);

                return decodeHappyness(response);
            }
            catch 
            {
                return HappyNess.None;
            }
        }

        private HappyNess decodeHappyness(string jsonResponse)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Deserialize<Dictionary<string, string>>(jsonResponse);
            var sent = json["sent"];
            var value = json["value"];

            System.Globalization.CultureInfo culInfo = new System.Globalization.CultureInfo("en");
            Double decValue, decValueMin, decValueMax ;
            bool decValid = Double.TryParse(value, System.Globalization.NumberStyles.Number, culInfo.NumberFormat, out decValue)
                            & Double.TryParse("-0.5", System.Globalization.NumberStyles.Number, culInfo.NumberFormat, out decValueMin)
                            & Double.TryParse("0.5", System.Globalization.NumberStyles.Number, culInfo.NumberFormat, out decValueMax);

            if (decValid)
            {
                if (decValue.CompareTo(decValueMax) <= 0 | decValue.CompareTo(decValueMin) >= 0)
                    return HappyNess.Indifferent;
                else
                    return Double.Parse(sent, culInfo) > 0 ? HappyNess.Happy : HappyNess.Sad;
            }
            return HappyNess.None;
        }
        private DateTime decodeDate(string strDate)
        {   

            
            DateTime dateresult = new DateTime();
            List<string> time;
            List<string> date = strDate.Trim().Split(' ').ToList();
            
            dateresult = Convert.ToDateTime(date[1] + "-" + date[2] + "-" + date[5]);
            time = date[3].Split(':').ToList();
            return dateresult.AddHours(Convert.ToDouble(time[0])).AddMinutes(Convert.ToDouble(time[1])).AddSeconds(Convert.ToDouble(time[2]));
        }
            
        #endregion
        #region "Token -  I used this method just one time to get token from twitter, and I put it app.config"
        private string getToken()
        {
            string bearToken = "";
            string param = System.Web.HttpUtility.HtmlEncode(Accesskey) + ":" + System.Web.HttpUtility.HtmlEncode(Accesskeysecret);
            string paramEncoded = Base64Encode(param);



            HttpWebRequest request = WebRequest.Create("https://api.twitter.com/oauth2/token") as HttpWebRequest;
            request.Headers.Add("Authorization", "Basic " + paramEncoded);
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Method = "POST";

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("grant_type=client_credentials");
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            try
            {

                WebResponse response = request.GetResponse();
                string jsonResponse = String.Empty;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonResponse = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    var  json = serializer.Deserialize<Dictionary<string, string>>(jsonResponse);
                    bearToken = json["access_token"];

                }
            }
            catch (Exception ex)
            {

            }
            return bearToken;
        }
        private static string Base64Encode(string plainText)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(plainText);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        #endregion

        #region "Old Mehods"
        //public List<Tweet> GetTweets(string input)
        //{ 

        //    List<Tweet> result = new List<Tweet>();
        //    string request = createRequestUrl(input);
        //    string response = GetResponse(request, "GET", "Bearer  " + token);
        //    return transformJson(response);


        //}
        //private string GetResponse(string requestUrl, string method, string auth)
        //{
        //    HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
        //    request.Headers.Add("Authorization", auth);
        //    request.Method = method;
        //    WebResponse response = (WebResponse)request.GetResponse();
        //    string jsonResponse = String.Empty;

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
        //        jsonResponse = reader.ReadToEnd();


        //    }
        //    return jsonResponse;
        //}
        #endregion
    }

   }

