using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfApp1
{
    /// <summary>
    /// 接收PasswordBox的密码修改事件
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;
            string password = PasswordBoxHelper.GetPassword(box);
            if (box != null && box.Password != password)
            {
                PasswordBoxHelper.SetPassword(box, box.Password);
            }
        }
    }
}
