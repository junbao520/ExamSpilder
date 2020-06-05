using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApp1.ViewModel.Common;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public class LoginViewModel : NotificationObject
    {
        public LoginViewModel()
        {
            obj.UserName = "admin";
            obj.Password = "123456";
            this.Gender =1;
        }

        /// <summary>
        /// Model对象
        /// </summary>
        private LoginModel obj = new LoginModel();

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return obj.UserName;
            }
            set
            {
                obj.UserName = value;
                this.RaisePropertyChanged("UserName");
            }
        }

        public string Password {
            get
            {
                return obj.Password;
            }
            set
            {
                obj.Password = value;
                this.RaisePropertyChanged("Password");
            }
        }

        public int Gender
        {
            get
            {
                return obj.Gender;
            }
            set
            {
                obj.Gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }


        private bool toClose = false;
        /// <summary>
        /// 是否要关闭窗口
        /// </summary>
        public bool ToClose
        {
            get
            {
                return toClose;
            }
            set
            {
                toClose = value;
                if (toClose)
                {
                    this.RaisePropertyChanged("ToClose");
                }
            }
        }

        private BaseCommand loginClick;
        /// <summary>
        /// 登录事件
        /// </summary>
        public BaseCommand LoginClick
        {
            get
            {
                if (loginClick == null)
                {
                    loginClick = new BaseCommand(new Action<object>(o =>
                    {
                        //执行登录逻辑
                        var pwd = this.Password;
                        var userName = this.UserName;

                        //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        //{
                        //    MessageBox.Show("OOO", "denglu chenggong ", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}));

                        WindowManager.Show("MainWindow3", null);
                        ToClose = true;

                    }));
                }
                return loginClick;
            }
        }
    }
}
