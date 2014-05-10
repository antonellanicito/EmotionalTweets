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

        private string token;

        public RestApi()
        {
            //token = getToken();
            token = ConfigurationManager.AppSettings["Token"];
            
        }
        public List<Tweet> GetTweets(string input)
        { 
            
            List<Tweet> result = new List<Tweet>();
            string request = createRequestUrl(input);
            string response = MakeRequest(request, "GET");
            return result;
        }


        private string createRequestUrl(string queryString)
        {
            string UrlRequest = restApiUrl + System.Web.HttpUtility.HtmlEncode(queryString);
               return (UrlRequest);
        }

        private string MakeRequest(string requestUrl, string method)
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            request.Headers.Add("Authorization", "Bearer  " + token);
            request.Method = method;
            WebResponse response = (WebResponse) request.GetResponse();

           return "";
          }

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


        }

   }

