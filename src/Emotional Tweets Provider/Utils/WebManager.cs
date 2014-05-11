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

namespace Emotional_Tweets_Provider.Utils
{
    public static class WebManager
    {
        
        public static HttpWebRequest CreateRequest(string url, string queryString, string method, string auth, Dictionary<string, string> fields)
        {

            string UrlRequest = url + (!string.IsNullOrEmpty(queryString) ? System.Web.HttpUtility.HtmlEncode(queryString) : "");
            HttpWebRequest request = WebRequest.Create(UrlRequest) as HttpWebRequest;
            request.Headers.Add("Authorization", auth);
            request.Method = method;
            if (fields != null)
            {
                StringBuilder postData = new StringBuilder();
                foreach (KeyValuePair<string, string> field in fields)
                {

                    postData.Append( field.Key + "=" + HttpUtility.UrlEncode(field.Value) + (fields.Last().Key.Equals(field.Key) ? "" : "&") );
                    
                }
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData.ToString());
                request.ContentLength = bytes.Length;
                var stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
            return request;
        }


        public static string GetResponse(HttpWebRequest request)
        {
            WebResponse response = (WebResponse)request.GetResponse();
            string jsonResponse = String.Empty;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                jsonResponse = reader.ReadToEnd();


            }
            return jsonResponse;
        }


    }

}
