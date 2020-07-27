using System;

namespace JikanLibrary.Models
{
    public class JikanCharactersData
    {
        public string request_hash { get; set; }
        public bool request_cached { get; set; }
        public int request_cache_expiry { get; set; }
        public Result[] results { get; set; }
        public int last_page { get; set; }
    }

    public class Result
    {
        public int mal_id { get; set; }
        public string url { get; set; }
        public string image_url { get; set; }
        public string name { get; set; }
        public string[] alternative_names { get; set; }
        public Anime[] anime { get; set; }
        public Manga[] manga { get; set; }
    }

    public class Anime
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Manga
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}
