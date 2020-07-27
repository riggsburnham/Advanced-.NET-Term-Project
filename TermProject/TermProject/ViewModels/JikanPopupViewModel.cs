using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ILibrary;
using Prism.Commands;
using TermProject.Views;
using JikanLibrary;
using JikanLibrary.Models;

namespace TermProject.ViewModels
{
    public class JikanPopupViewModel : INotifyPropertyChanged
    {
        #region popupwindow code

        #region private member variables
        private const string SHOW_UI_ELEMENT = "visible";
        private const string HIDE_UI_ELEMENT = "hidden";
        private bool _searchByAnimeRadioButton = false;
        private bool _searchByCharacterRadioButton = false;
        private string _showSearchByAnime = HIDE_UI_ELEMENT;
        private string _showSearchByCharacter = HIDE_UI_ELEMENT;
        private string _showSearchButton = HIDE_UI_ELEMENT;
        private string _animeName = "";
        private string _characterFirstName = "";
        private string _characterLastName = "";
        private PhotoGalleryViewModel _parent;
        private string _searchImagesSpinnerVisibility = HIDE_UI_ELEMENT;
        #endregion

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region constructor
        public JikanPopupViewModel(PhotoGalleryViewModel parent)
        {
            MouseX = Cursor.Position.X;
            MouseY = Cursor.Position.Y;
            Parent = parent;
            SearchByAnimeCommand = new DelegateCommand(SearchByAnime);
            SearchByCharacterCommand = new DelegateCommand(SearchByCharacter);
            JikanSearchCommand = new DelegateCommand(async () => await JikanSearch());
            JikanPopupClosingCommand = new DelegateCommand(JikanPopupClosing);
        }
        #endregion

        #region properties
        public string ShowSearchByAnime
        {
            get => _showSearchByAnime;
            set
            {
                _showSearchByAnime = value;
                NotifyPropertyChanged();
            }
        }

        public string ShowSearchByCharacter
        {
            get => _showSearchByCharacter;
            set
            {
                _showSearchByCharacter = value;
                NotifyPropertyChanged();
            }
        }

        public string ShowSearchButton
        {
            get => _showSearchButton;
            set
            {
                _showSearchButton = value;
                NotifyPropertyChanged();
            }
        }

        public string AnimeName
        {
            get => _animeName;
            set
            {
                _animeName = value;
                NotifyPropertyChanged();
            }
        }

        public string CharacterFirstName
        {
            get => _characterFirstName;
            set
            {
                _characterFirstName = value;
                NotifyPropertyChanged();
            }
        }

        public string CharacterLastName
        {
            get => _characterLastName;
            set
            {
                _characterLastName = value;
                NotifyPropertyChanged();
            }
        }

        public int MouseX { get; set; } = 0;
        public int MouseY { get; set; } = 0;

        public PhotoGalleryViewModel Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public string SearchImagesSpinnerVisibility
        {
            get => _searchImagesSpinnerVisibility;
            set
            {
                _searchImagesSpinnerVisibility = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region functions

        private ObservableCollection<IData> InitializeJikanCharacterImages()
        {
            JikanCharacterPictures jikanCharacterPicutres = new JikanCharacterPictures(CharacterFirstName, CharacterLastName);
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (JikanCharacterPicturesData character in jikanCharacterPicutres.JikanCharactersPicturesData)
            {
                foreach (Picture characterImage in character.pictures)
                {
                    images.Add(characterImage);
                }
            }
            return images;
        }

        private ObservableCollection<IData> InitializeJikanAnimeImages()
        {
            JikanAnimePictures jikanAnimePictures = new JikanAnimePictures(AnimeName);
            ObservableCollection<IData> images = new ObservableCollection<IData>();
            foreach (JikanResult animeImage in jikanAnimePictures.AnimeData.results)
            {
                // ignore r and rx anime, dont want to show nsfw images in class
                if (string.IsNullOrEmpty(animeImage.rated)) continue;
                if (animeImage.rated.ToLower() == "rx") continue;
                images.Add(animeImage);
            }

            return images;
        }
        #endregion

        #region commands
        public DelegateCommand SearchByAnimeCommand { get; set; }
        private void SearchByAnime()
        {
            if (_searchByAnimeRadioButton == false)
            {
                _searchByAnimeRadioButton = true;
                _searchByCharacterRadioButton = false;
                ShowSearchByCharacter = HIDE_UI_ELEMENT;
                ShowSearchByAnime = SHOW_UI_ELEMENT;
                ShowSearchButton = SHOW_UI_ELEMENT;
                CharacterFirstName = "";
                CharacterLastName = "";
            }
        }

        public DelegateCommand SearchByCharacterCommand { get; set; }
        private void SearchByCharacter()
        {
            if (_searchByCharacterRadioButton == false)
            {
                _searchByCharacterRadioButton = true;
                _searchByAnimeRadioButton = false;
                ShowSearchByAnime = HIDE_UI_ELEMENT;
                ShowSearchByCharacter = SHOW_UI_ELEMENT;
                ShowSearchButton = SHOW_UI_ELEMENT;
                AnimeName = "";
            }
        }

        public DelegateCommand JikanSearchCommand { get; set; }

        private async Task JikanSearch()
        {
            ShowSearchByAnime = HIDE_UI_ELEMENT;
            ShowSearchByCharacter = HIDE_UI_ELEMENT;
            ShowSearchButton = HIDE_UI_ELEMENT;
            SearchImagesSpinnerVisibility = SHOW_UI_ELEMENT;
            if (_searchByCharacterRadioButton)
            {
                // popupate the images by finding the images of characters that match the supplied name
                Parent.SelectedImageDetails = null;
                Parent.SelectedImageDetails = new List<string>();
                Parent.Images = await Task.Run(InitializeJikanCharacterImages);
            }
            else if (_searchByAnimeRadioButton)
            {
                // populate the images by finding the images of the anime that match the supplied name
                Parent.SelectedImageDetails = null;
                Parent.SelectedImageDetails = new List<string>();
                Parent.Images = await Task.Run(InitializeJikanAnimeImages);
                Console.ReadLine();
            }
            SearchImagesSpinnerVisibility = HIDE_UI_ELEMENT;
            Parent.JikanPopup.Hide();
        }

        public DelegateCommand JikanPopupClosingCommand { get; set; }

        private void JikanPopupClosing()
        {
            Parent.JikanPopup = null;
        }
        #endregion

        #endregion
    }
}
