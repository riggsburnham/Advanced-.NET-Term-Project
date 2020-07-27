using System;
using System.Collections.Generic;
using System.Text;
using ILibrary;

namespace JikanLibrary.Models
{
    public class JikanAnimeData
    {
        public string request_hash { get; set; }
        public bool request_cached { get; set; }
        public int request_cache_expiry { get; set; }
        public int last_page { get; set; }
        public JikanResult[] results { get; set; }
    }

    public class JikanResult : IData
    {
        public int mal_id { get; set; }
        public string url { get; set; }
        public string image_url { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public int? episodes { get; set; }
        public bool airing { get; set; }
        public string rating { get; set; }
        public float? score { get; set; }
        public int members { get; set; }
        public string synopsis { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string rated { get; set; }
        public string URL { get => image_url; set => image_url = value; }
        public IList<string> ImageData()
        {
            IList<string> imageData = new List<string>();
            imageData.Add("Anime Title:");
            imageData.Add(title);
            imageData.Add("");
            imageData.Add("Episodes:");
            imageData.Add(episodes.ToString());
            imageData.Add("");
            imageData.Add("Score:");
            imageData.Add(score.ToString());
            imageData.Add("");
            imageData.Add("Rating:");
            imageData.Add(rated);
            imageData.Add("");
            imageData.Add("Synopsis:");
            imageData.Add(synopsis);
            imageData.Add("");
            return imageData;
        }
    }

}
