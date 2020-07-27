using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BingLibrary
{
    public class BingImageOfTheDay
    {
        private const string url_begin = @"https://www.bing.com";
        private const string url = @"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8&mkt=en-US";
        private BingData bingData;

        public BingImageOfTheDay()
        {
            BingData = InitializeBingDataFromWebApi();
        }

        public BingData BingData
        {
            get { return bingData; }
            set { bingData = value; }
        }

        private BingData InitializeBingDataFromWebApi()
        {
            using (WebClient client = new WebClient())
            {
                string jsonData = client.DownloadString(url);
                BingData bd = JsonConvert.DeserializeObject<BingData>(jsonData);
                
                foreach (BingImage image in bd.images)
                {
                    image.url = url_begin + image.url;
                }

                return bd;
            }
        }
    }
}
