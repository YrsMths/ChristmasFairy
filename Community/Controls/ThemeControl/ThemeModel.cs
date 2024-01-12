using Community.Controls.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Community.Controls
{
    public class ThemeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    
    private bool _isChecked;

        /// <summary>
        ///     whether to choose
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public FestvialType Type { get; set; }
        /// <summary>
        /// display image
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        ///     display color
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     resource path
        /// </summary>
        public string ResourcePath { get; set; }
    }
}
