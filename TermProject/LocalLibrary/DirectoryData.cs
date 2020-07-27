using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILibrary;

namespace LocalLibrary
{
    public class DirectoryData
    {
        public DirectoryData()
        {
            Images = new List<LocalImage>();
        }
        public IList<LocalImage> Images { get; set; }
        public string Path { get; set; }
        public string DirectoryName { get; set; }
    }

    public class LocalImage : IData
    {
        public string URL { get; set; }
        public string FileName { get; set; }
        public DateTime DateTaken { get; set; }
        public IList<string> ImageData()
        {
            IList<string> imageData = new List<string>();
            imageData.Add("Filename:");
            imageData.Add(FileName);
            imageData.Add("");
            imageData.Add("Date Taken:");
            imageData.Add(DateTaken.ToLongDateString());
            imageData.Add("");
            imageData.Add("Path:");
            imageData.Add(URL);
            imageData.Add("");
            return imageData;
        }
    }
}
