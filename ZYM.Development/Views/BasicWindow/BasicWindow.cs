using Community.Controls.Runtimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZYM.Development.Views
{
    public abstract class BasicWindow : Window, IDisposable
    {
        public virtual void Refresh() { }
        public void Dispose()
        {
            User32Interop.DestroyWindow(this.GetHandle());
            GC.SuppressFinalize(this);
        }
    }
}
