using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emotional_Tweets_Model
{
    public class Tweet
    {
        public Int64 Id { get; set; }
        public string Text { get; set; }
        public string language { get; set; }

        
        public HappyNess LevelHappyNess { get; set; }
    }

    public enum HappyNess
    {
        None = 0,
        Happy = 1,
        Sad = 2,
        Indifferent = 3
    }
}