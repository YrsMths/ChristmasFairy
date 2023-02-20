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

namespace ZYM.Development.Views.Christmas
{
    /// <summary>
    /// OpenTheGiftWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpenTheGiftWindow : BasicWindow
    {
        public OpenTheGiftWindow()
        {
            InitializeComponent();
            List<ImageSource> images = new List<ImageSource>();
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_bell.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_candy.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_crutch.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_deer.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_fruit.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_garland.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_gift.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_gingerbreadman.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_glove.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_hat.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_sled.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_snowman.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_sock.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_spirit.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_star.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/christmas_tree.png", UriKind.RelativeOrAbsolute)));
            DispreadBox.Gifts = images.ToArray();
        }
    }
}
