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
using TermProject.Models;
using TermProject.ViewModels;

namespace TermProject.Views
{
    /// <summary>
    /// Interaction logic for FullSizeImageWindow.xaml
    /// </summary>
    public partial class FullSizeImagePopupWindow : Window
    {
        public FullSizeImagePopupWindow(FullSizeImage image)
        {
            DataContext = new FullSizeImagePopupViewModel(image);
            InitializeComponent();
        }
    }
}
