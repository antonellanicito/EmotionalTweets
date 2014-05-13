using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emotional_Tweets_Model;
namespace Emotional_Tweets_Provider.Contracts
{
    public interface IRestApi
    {
        List<Tweet> GetTweets(string input);
        HappyNess GetHappyness(string lang, string text);
        
    }
}
