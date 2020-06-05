using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1.ViewModel.Login
{
    public class NotEmptyCheck : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "不能为空";
        }
    }

    public class UserNameExists : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var name = value as string;
            if (name.Contains("abc"))
            {
                return true;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return "用户名必须包含abc";
        }
    }

}
