using Community.Controls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZYM.Development.Views
{
    /// <summary>
    /// GodWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SantaWindow : Window
    {
        public SantaWindow(double FontSize, Size WindowSize)
        {
            InitializeComponent();
            DispalyBorder.Height = WindowSize.Height / 3.0;
            DispalyBorder.Width = WindowSize.Width / 3.0;
            SantasWord_TextBox.FontSize = FontSize / 3.0;
            SantasWord_TextBox.Text = Properties.Settings.Default.SantasWord;
        }

        public event EventHandler Event_Yes;

        public void Save()
        {
            Properties.Settings.Default.SantasWord = SantasWord_TextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as ButtonEx).Uid)
            {
                case "Close":
                    this.Close();
                    break;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                (sender as Window).DragMove();
            }
        }

        private void SantasWord_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Event_Yes?.Invoke(sender, e);
        }
    }
}
