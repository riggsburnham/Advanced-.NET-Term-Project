using System;
using System.Collections.Generic;
using System.Text;
using ILibrary;

namespace JikanLibrary.Models
{

    public class JikanCharacterPicturesData
    {
        public string request_hash { get; set; }
        public bool request_cached { get; set; }
        public int request_cache_expiry { get; set; }
        public Picture[] pictures { get; set; }
    }

    public class Picture : IData
    {
        public string large { get; set; }
        public string small { get; set; }

        public string URL
        {
            get => large; 
            set => large = value;
        }
        public IList<string> ImageData()
        {
            IList<string> imageData = new List<string>();
            imageData.Add("Large URL:");
            imageData.Add(large);
            imageData.Add("");
            imageData.Add("Small URL:");
            imageData.Add(small);
            imageData.Add("");
            return imageData;
        }
    }

}
