using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.ViewModel.Common
{
    public static class WindowExt
    {
        public static void Register(this Window win, string key)
        {
            WindowManager.Register(key, win.GetType());
        }

        public static void Register(this Window win, string key, Type t)
        {
            WindowManager.Register(key, t);
        }

        public static void Register<T>(this Window win, string key)
        {
            WindowManager.Register<T>(key);
        }
    }
}
