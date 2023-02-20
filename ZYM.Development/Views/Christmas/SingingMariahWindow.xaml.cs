using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// SingingMariahWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SingingMariahWindow : BasicWindow
    {
        public SingingMariahWindow()
        {
            InitializeComponent();
            this.Loaded += Init;
        }

        private void Init(object sender, RoutedEventArgs e)
        {
            List<ImageSource> images = new List<ImageSource>();
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah0.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah1.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah2.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah2.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah3.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah4.png", UriKind.RelativeOrAbsolute)));
            singingMariah.Sources = images.ToArray();

            List<ImageSource> serialImages0 = new List<ImageSource>();
            serialImages0.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_running0.png", UriKind.RelativeOrAbsolute)));
            serialImages0.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_running1.png", UriKind.RelativeOrAbsolute)));
            serialImages0.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_running2.png", UriKind.RelativeOrAbsolute)));

            List<ImageSource> serialImages1 = new List<ImageSource>();
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing0.png", UriKind.RelativeOrAbsolute)));
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing1.png", UriKind.RelativeOrAbsolute)));
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing2.png", UriKind.RelativeOrAbsolute)));
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing3.png", UriKind.RelativeOrAbsolute)));
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing4.png", UriKind.RelativeOrAbsolute)));
            serialImages1.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Mariah/Mariah_singing5.png", UriKind.RelativeOrAbsolute)));

            singingMariah.SerialSources = new List<ImageSource[]>()
            {
                serialImages0.ToArray(),
                serialImages1.ToArray()
            };
        }
    }
}
