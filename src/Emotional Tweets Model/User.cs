using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emotional_Tweets_Model
{
    public class User
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string language { get; set; }
        public User (Dictionary<string, Object> user)
        {
            
            this.Id = Convert.ToInt64(user["id"]);
            this.Name = user["name"].ToString();
            this.language = user["lang"].ToString();
        
        }
    }
}
