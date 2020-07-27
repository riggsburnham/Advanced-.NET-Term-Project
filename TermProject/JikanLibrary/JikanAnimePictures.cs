using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using JikanLibrary.Models;
using Newtonsoft.Json;

namespace JikanLibrary
{
    public class JikanAnimePictures
    {
        private string _animeName;
        private const string JIKAN_SEARCH_ANIME_API_URI = @"https://api.jikan.moe/v3/search/anime?q=";

        public JikanAnimePictures(string animeName)
        {
            _animeName = animeName;

            AnimeData = InitializeAnimePicturesFromWebApi();
        }

        public JikanAnimeData AnimeData { get; set; }

        private JikanAnimeData InitializeAnimePicturesFromWebApi()
        {
            using (WebClient client = new WebClient())
            {
                JikanAnimeData animeData = new JikanAnimeData();
                try
                {
                    string jsonData = client.DownloadString($"{JIKAN_SEARCH_ANIME_API_URI}{_animeName}");
                    animeData = JsonConvert.DeserializeObject<JikanAnimeData>(jsonData);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                return animeData;
            }
        }
    }
}
