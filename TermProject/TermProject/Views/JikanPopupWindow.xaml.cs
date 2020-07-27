using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TermProject.ViewModels;

namespace TermProject.Views
{
    /// <summary>
    /// Interaction logic for JikanPopupWindow.xaml
    /// </summary>
    public partial class JikanPopupWindow : Window
    {
        public JikanPopupWindow(PhotoGalleryViewModel parent)
        {
            DataContext = new JikanPopupViewModel(parent);
            InitializeComponent();
        }
    }
}
