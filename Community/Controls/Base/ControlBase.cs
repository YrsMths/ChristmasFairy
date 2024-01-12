using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Community.Controls.Base
{
    public class ControlBase : Control, IDisposable
    {
        /// <summary>
        /// 释放标记
        /// </summary>
        protected bool disposed;
        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~ControlBase()
        {
            //必须为false
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
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
