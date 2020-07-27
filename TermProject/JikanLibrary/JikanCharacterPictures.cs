using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JikanLibrary.Models;
using Newtonsoft.Json;

namespace JikanLibrary
{
    public class JikanCharacterPictures
    {
        private string _firstname;
        private string _lastname;
        private const string JIKAN_SEARCH_CHARACTERS_API_URI = @"https://api.jikan.moe/v3/search/character?q=";
        private const string JIKAN_SEARCH_CHARACTER_PICTURES_API_URI = @"https://api.jikan.moe/v3/character/"; 
        private IList<int> _charactersIds = new List<int>();

        public JikanCharacterPictures(string firstname, string lastname)
        {
            _firstname = firstname;
            _lastname = lastname;
            JikanCharactersData = InitializeCharactersFromWebApi();
            Task.Delay(2000);       // Delay to limit the speed of the calls to the api, so the api wont get block by the myanimelist website
            JikanCharactersPicturesData = InitializeCharacterPicturesFromWebApi();
        }

        public JikanCharactersData JikanCharactersData { get; set; }
        public IList<JikanCharacterPicturesData> JikanCharactersPicturesData { get; set; }

        private JikanCharactersData InitializeCharactersFromWebApi()
        {
            JikanCharactersData jcd = new JikanCharactersData();
            try
            {
                using (WebClient client = new WebClient())
                {
                    string jsonData = "";
                    if (string.IsNullOrEmpty(_lastname))
                    {
                        jsonData = client.DownloadString($"{JIKAN_SEARCH_CHARACTERS_API_URI}{_firstname}");
                    }
                    else
                    {
                        jsonData = client.DownloadString($"{JIKAN_SEARCH_CHARACTERS_API_URI}{_lastname},{_firstname}");
                    }
                    jcd = JsonConvert.DeserializeObject<JikanCharactersData>(jsonData);
                    foreach (Result r in jcd.results)
                    {
                        _charactersIds.Add(r.mal_id);
                    }
                    return jcd;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return jcd;
        }

        private IList<JikanCharacterPicturesData> InitializeCharacterPicturesFromWebApi()
        {
            using (WebClient client = new WebClient())
            {
                IList<JikanCharacterPicturesData> charactersPictures = new List<JikanCharacterPicturesData>();
                try
                {
                    foreach (int id in _charactersIds)
                    {
                        string jsonData = client.DownloadString($"{JIKAN_SEARCH_CHARACTER_PICTURES_API_URI}{id}/pictures");
                        JikanCharacterPicturesData jcpd = JsonConvert.DeserializeObject<JikanCharacterPicturesData>(jsonData);
                        charactersPictures.Add(jcpd);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                return charactersPictures;
            }
        }
    }
}
