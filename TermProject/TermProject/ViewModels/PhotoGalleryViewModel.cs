using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using BingLibrary;
using ILibrary;
using LocalLibrary;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Interactivity;
using Prism.Mvvm;
using ReditLibrary;
using TermProject.Models;
using TermProject.Views;
using Image = System.Drawing.Image;

namespace TermProject.ViewModels
{
    

    public class PhotoGalleryViewModel : INotifyPropertyChanged
    {
        private BingImageOfTheDay _bingImageOfTheDay;
        private RedditEarthPorn _redditEarthPorn;
        private ObservableCollection<IData> _images;
        private int _mainWindowHeight = 450;
        private int _mainWindowWidth = 800;
        private IList<string> _selectedImageDetails;
        private ObservableCollection<DirectoryData> _localDirectories;
        private JikanPopupWindow _jikanPopup;
        private FullSizeImagePopupWindow _fullSizeImagePopup;

        public PhotoGalleryViewModel()
        {
            BingImageOfTheDay = new BingImageOfTheDay();
            RedditEarthPorn = new RedditEarthPorn();

            // upon startup the images gets populated with the bing data..
            Images = InitializeBingImages();
            SelectedImageChangedCommand = new DelegateCommand<object>(SelectedImageChanged);
            LoadLocalImagesCommand = new DelegateCommand<object>(LoadLocalImages);
            LoadBingImagesCommand = new DelegateCommand(LoadBingImages);
            LoadRedditImagesCommand = new DelegateCommand(LoadRedditImages);
            AddLocalDirectoryCommand = new DelegateCommand(AddLocalDirectory);
            ShowJikanPopupWindowCommand = new DelegateCommand(ShowJikanPopupWindow);
            ViewSelectedImageFullSizeCommand = new DelegateCommand<object>(ViewSelectedImageFullSize);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public BingImageOfTheDay BingImageOfTheDay
        {
            get => _bingImageOfTheDay;
            set => _bingImageOfTheDay = value;
        }

        public RedditEarthPorn RedditEarthPorn
        {
            get => _redditEarthPorn;
            set => _redditEarthPorn = value;
        }

        public ObservableCollection<IData> Images
        {
            get => _images;
            set
            {
                _images = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DirectoryData> LocalDirectories
        {
            get => _localDirectories ?? (_localDirectories = new ObservableCollection<DirectoryData>());
            set
            {
                _localDirectories = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<IData> InitializeBingImages()
        {
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (BingImage image in _bingImageOfTheDay.BingData.images)
            {
                images.Add(image);
            }
            return images;
        }

        private ObservableCollection<IData> InitializeRedditImages()
        {
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (Child child in RedditEarthPorn.RedditData.data.children)
            {
                images.Add(child.data);
            }
            return images;
        }

        private ObservableCollection<IData> InitializeLocalImages(string directoryName)
        {
            DirectoryData dd = LocalDirectories.FirstOrDefault(dir => dir.DirectoryName == directoryName);
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach(LocalImage image in dd.Images)
            {
                images.Add(image);
            }
            return images;
        }

        private ObservableCollection<IData> InitializeImagesOnlyRedditCurrently()
        {
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (Child child in RedditEarthPorn.RedditData.data.children)
            {
                images.Add(child.data);
            }
            return images;
        }

        private ObservableCollection<IData> InitializeImagesOnlyBingCurrently()
        {
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (BingImage image in _bingImageOfTheDay.BingData.images)
            {
                images.Add(image);
            }
            return images;
        }

        public int MainWindowHeight
        {
            get => _mainWindowHeight;
            set => _mainWindowHeight = value;
        }

        public int MainWindowWidth
        {
            get => _mainWindowWidth;
            set
            {
                _mainWindowWidth = value;
                NotifyPropertyChanged("PhotoGalleryWrapPanelWidth");
            }
        }

        public int PhotoGalleryWrapPanelWidth
        {
            get
            {
                double width = System.Convert.ToDouble(MainWindowWidth);
                double result = (.6 * width);
                return System.Convert.ToInt32(result);
            }
        }

        public int PhotoGalleryImageWidth
        {
            get
            {
                double width = System.Convert.ToDouble(MainWindowWidth);
                // there are 10 columns where the photogallery is taking up 6 of them so, multiply width by .6 to get the width of the phpto gallery,
                // we want to display 3 images in each row so divide that result by 3
                // take that result and minus 11 for the width of the scrollbar
                double result = ((.6 * width) / 3) - 11;
                return System.Convert.ToInt32(result);
            }
        }

        public IList<string> SelectedImageDetails
        {
            get => _selectedImageDetails;
            set
            {
                if (_selectedImageDetails == null) _selectedImageDetails = new List<string>();
                else _selectedImageDetails.Clear();
                _selectedImageDetails = value;
                NotifyPropertyChanged();
            }
        }

        public JikanPopupWindow JikanPopup
        {
            get => _jikanPopup;
            set => _jikanPopup = value;
        }

        public FullSizeImagePopupWindow FullSizeImagePopup
        {
            get => _fullSizeImagePopup;
            set => _fullSizeImagePopup = value;
        }

        #region commands
        public DelegateCommand<object> SelectedImageChangedCommand { get; set; }
        private void SelectedImageChanged(object selectedItem)
        {
            IData image = selectedItem as IData;
            if (image == null) return;
            SelectedImageDetails = image.ImageData();
        }

        public DelegateCommand<object> ViewSelectedImageFullSizeCommand { get; set; }

        private void ViewSelectedImageFullSize(object selectedItem)
        {
            IData image = selectedItem as IData;
            if (image == null) return;
            WebRequest request = WebRequest.Create(image.URL);
            using (WebResponse response = request.GetResponse())
            {
                Image picture = Image.FromStream(response.GetResponseStream());
                FullSizeImage fullSizeImage = new FullSizeImage();
                fullSizeImage.Width = picture.Width;
                fullSizeImage.Height = picture.Height;
                fullSizeImage.URL = image.URL;
                FullSizeImagePopup = new FullSizeImagePopupWindow(fullSizeImage);
                FullSizeImagePopup.Show();
            }
        }
        public DelegateCommand<object> LoadLocalImagesCommand { get; set; }
        private void LoadLocalImages(object selectedItem)
        {
            DirectoryData dd = selectedItem as DirectoryData;
            LocalDirectories = LocalDirectories;
            selectedItem = null;
            SelectedImageDetails = null;
            SelectedImageDetails = new List<string>();
            Images = InitializeLocalImages(dd.DirectoryName);
        }

        public DelegateCommand LoadBingImagesCommand { get; set; }
        private void LoadBingImages()
        {
            SelectedImageDetails = null;
            SelectedImageDetails = new List<string>();
            Images = InitializeBingImages();
        }
        public DelegateCommand LoadRedditImagesCommand { get; set; }
        private void LoadRedditImages()
        {
            SelectedImageDetails = null;
            SelectedImageDetails = new List<string>();
            Images = InitializeRedditImages();
        }

        public DelegateCommand AddLocalDirectoryCommand { get; set; }
        private void AddLocalDirectory()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();
                string path = fbd.SelectedPath;
                Console.ReadLine();
                DirectoryData dd = new DirectoryData();
                dd.Path = fbd.SelectedPath;
                IList<string> contents = fbd.SelectedPath.Split('\\');
                dd.DirectoryName = contents.Last().ToString();
                foreach (string file in Directory.EnumerateFiles(fbd.SelectedPath, "*.*").Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg")))
                {
                    string imagePath = file;
                    DateTime lastModified = System.IO.File.GetLastWriteTime(imagePath);
                    LocalImage li = new LocalImage();
                    li.DateTaken = lastModified;
                    IList<string> fileContents = file.Split('\\');
                    li.FileName = fileContents.Last().ToString();
                    li.URL = imagePath;
                    dd.Images.Add(li);
                }
                LocalDirectories.Add(dd);
            }
        }

        public DelegateCommand ShowJikanPopupWindowCommand { get; set; }

        private void ShowJikanPopupWindow()
        {
            JikanPopup = new JikanPopupWindow(this);
            JikanPopup.Show();
        }
        #endregion
    }
}
