using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class FullSizeImagePopupViewModel
    {
        public FullSizeImagePopupViewModel(FullSizeImage image)
        {
            ImageHeight = image.Height;
            ImageWidth = image.Width;
            ImageURL = image.URL;
        }

        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public string ImageURL { get; set; }
    }
}
