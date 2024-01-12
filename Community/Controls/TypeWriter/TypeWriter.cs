using Community.Controls.Base;
using Community.Helpers;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.TypeWriter"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.TypeWriter;assembly=Community.Controls.TypeWriter"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:TypeWriter/>
    ///
    /// </summary>
    public class TypeWriter : ControlBase
    {
        private const string CanvasTemplateName = "PART_Canvas";
        private const string TextBoxTemplateName = "PART_TextBox";
        private const string BorderTemplateName = "PART_Border";
        private const string VisualBrushTemplateName = "PART_VisualBrush";

        private Canvas _canvas;
        private TextBox _textBox;
        private Border _border;
        private VisualBrush _visualBrush;

        static TypeWriter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TypeWriter), new FrameworkPropertyMetadata(typeof(TypeWriter)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TypeWriter),
                new PropertyMetadata("HEY!"));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            _textBox = GetTemplateChild(TextBoxTemplateName) as TextBox;
            _border = GetTemplateChild(BorderTemplateName) as Border;
            _visualBrush = GetTemplateChild(VisualBrushTemplateName) as VisualBrush;
            if (_canvas == null) return;
            Hook();
            Loaded += _this_Loaded;
            Unloaded += _this_UnLoaded;
        }

        private void _this_Loaded(object s, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(Window.GetWindow(this)).Handle;
            Win32ApiHelper.ClientToScreen(hwnd, ref relativePoint);
        }

        private void _this_UnLoaded(object s, RoutedEventArgs e)
        {
            Unhook();
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int HC_ACTION = 0;
        private int hMouseHook = 0;
        private static Win32ApiHelper.HookProc MouseHookProcedure;

        private void Hook()
        {
            MouseHookProcedure = new Win32ApiHelper.HookProc(MouseHookProc);
            hMouseHook = Win32ApiHelper.SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    MouseHookProcedure,
                    Win32ApiHelper.GetModuleHandle(null),
                    0);
        }

        private void Unhook()
        {
            if (hMouseHook != 0)
            {
                bool retMouse = Win32ApiHelper.UnhookWindowsHookEx(hMouseHook);
            }
        }

        POINT relativePoint = new POINT(0, 0);

        int writerIndex = -1;
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION && wParam == WM_KEYDOWN)
            {
                writerIndex = writerIndex + 1 >= Text.Length ? -1 : writerIndex + 1;
                _textBox.Text += (writerIndex == -1 ? "\n" : Text[writerIndex].ToString());
                if (_textBox.ActualHeight > _canvas.ActualHeight / 2 + _textBox.FontSize * 2)
                {
                    _textBox.Text = _textBox.Text.Remove(0, Text.Length);
                }
                //获取最后一个字符的索引
                int lastCharacterIndex = _textBox.Text.Length - 1;
                // 获取最后一个字符相对于控件的位置
                Rect lastCharacterRect = _textBox.GetRectFromCharacterIndex(lastCharacterIndex);
                // 创建一个用于测量文本宽度的FormattedText对象
                FormattedText formattedText = new FormattedText(
                    _textBox.Text.Split('\n').Last(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(_textBox.FontFamily, _textBox.FontStyle, _textBox.FontWeight, _textBox.FontStretch),
                    _textBox.FontSize,
                    Brushes.Black
                );

                // 创建一个用于测量文本宽度的FormattedText对象
                FormattedText formattedText_line = new FormattedText(
                    Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(_textBox.FontFamily, _textBox.FontStyle, _textBox.FontWeight, _textBox.FontStretch),
                    _textBox.FontSize,
                    Brushes.Black
                );

                double textLeft = _canvas.ActualWidth / 2 - formattedText.WidthIncludingTrailingWhitespace;
                double textTop = _canvas.ActualHeight / 2 - _textBox.ActualHeight;
                double backLeft = - textLeft;
                double backTop = - textTop;
                while (backLeft < 0) { backLeft += formattedText_line.WidthIncludingTrailingWhitespace;}
                while(backTop < 0) { backTop += formattedText_line.Height; }
                _visualBrush.Viewport = new Rect(0, 0, formattedText_line.WidthIncludingTrailingWhitespace, formattedText_line.Height);
                _visualBrush.Transform = new TranslateTransform(-backLeft, -backTop);
                Canvas.SetLeft(_textBox, textLeft);
                Canvas.SetTop(_textBox, textTop);
            }
            return Win32ApiHelper.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }


        private void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
        {
            Storyboard story = new Storyboard();
            story.FillBehavior = FillBehavior.HoldEnd;
            story.RepeatBehavior = RepeatBehavior.Forever;

            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(timeSpan);

            string tmp = string.Empty;
            foreach (char c in textToAnimate)
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames, txt.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));
            story.Children.Add(stringAnimationUsingKeyFrames);

            story.Begin(txt);
        }

        /// <summary>
        /// 重写的Dispose方法
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
                Unhook();
                Loaded -= _this_Loaded;
                Unloaded -= _this_UnLoaded;
            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
