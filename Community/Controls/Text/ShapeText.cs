using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    public class ShapeText : Shape
    {
        static ShapeText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShapeText), new FrameworkPropertyMetadata(typeof(ShapeText)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ShapeText), new PropertyMetadata("TEST"));

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ShapeText), new PropertyMetadata("微软雅黑"));
        
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(ShapeText), new PropertyMetadata());

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(ShapeText), new PropertyMetadata(null));

        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(ShapeText), new PropertyMetadata(null));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(ShapeText), new PropertyMetadata(12));
        
        private double _height;
        private double _width;
        private Geometry _textGeometry;
        
        [Localizability(LocalizationCategory.Text)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        protected sealed override Geometry DefiningGeometry
        {
            get
            {
                return _textGeometry ?? Geometry.Empty;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.RealizeGeometry();
            return new Size(Math.Min(availableSize.Width, _width), Math.Min(availableSize.Height, _height));
        }

        private void RealizeGeometry()
        {
            var formattedText = new FormattedText(
                                       Text,
                                       CultureInfo.CurrentCulture,
                                       FlowDirection.LeftToRight,
                                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Transparent);
            _height = formattedText.Height;
            _width = formattedText.Width;
            _textGeometry = formattedText.BuildGeometry(new Point());
        }
    }
}
