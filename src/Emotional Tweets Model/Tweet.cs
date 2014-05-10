using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emotional_Tweets_Model
{
    public class Tweet
    {
        public string CreationDate { get; set; }
        public Int32 Likes { get; set; }
        public Int32 Retweeted_Count { get; set; } 
        public Int64 Id { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }


        public string Summary { get; set; }
        public HappyNess LevelHappyNess { get; set; }
    }

    public enum HappyNess
    {
        Happy = 1,
        Sad = 2,
        Indifferent = 3
    }
}