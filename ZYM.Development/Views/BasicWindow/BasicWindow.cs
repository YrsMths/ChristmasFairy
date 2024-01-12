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

        /// <summary>
        /// 释放标记
        /// </summary>
        protected bool disposed;
        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~BasicWindow()
        {
            //必须为false
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            User32Interop.DestroyWindow(this.GetHandle());
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {

            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
