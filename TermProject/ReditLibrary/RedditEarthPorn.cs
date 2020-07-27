using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReditLibrary
{
    public class RedditEarthPorn
    {
        private const string url = @"https://www.reddit.com/r/EarthPorn/top/.json";
        private RedditData redditData;

        public RedditEarthPorn()
        {
            RedditData = InitializeRedditEarthPornFromWebApi();
        }

        public RedditData RedditData
        {
            get { return redditData; }
            set { redditData = value; }
        }

        private RedditData InitializeRedditEarthPornFromWebApi()
        {
            using (WebClient client = new WebClient())
            {
                string jsonData = client.DownloadString(url);
                RedditData rep = JsonConvert.DeserializeObject<RedditData>(jsonData);
                return rep;
            }
        }
    }
}
