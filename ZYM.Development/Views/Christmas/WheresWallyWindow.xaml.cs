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
using System.Windows.Threading;

namespace ZYM.Development.Views.Christmas
{
    /// <summary>
    /// HiddenMariahWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WheresWallyWindow : BasicWindow
    {
        List<ImageSource> wallys = new List<ImageSource>();
        int index = 0;
        public WheresWallyWindow()
        {
            InitializeComponent();
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-01.png", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-02.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-03.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-04.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-05.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-06.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-07.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-08.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-09.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-10.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-11.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-12.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-13.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-14.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-15.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-16.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-17.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-18.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-19.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-20.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-21.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-22.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-23.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-24.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-25.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-26.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-27.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-28.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-29.jpg", UriKind.RelativeOrAbsolute)));
            wallys.Add(new BitmapImage(new Uri("/ZYM.Development;component/Images/Wally/wheres-wally-30.jpg", UriKind.RelativeOrAbsolute)));
            whereswally.BottomImage = wallys[new Random().Next(0, wallys.Count)];
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(600) };
            timer.Tick += delegate
            {
                index = index >= wallys.Count - 1 ? 0 : index + 1;
                whereswally.BottomImage = wallys[index];
            };
            timer.Start();
        }
    }
}
