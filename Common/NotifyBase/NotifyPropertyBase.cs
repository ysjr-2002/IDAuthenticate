using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common.NotifyBase
{
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public static class NotifyPropertyBaseEx
    {
        public static string GetName<T, U>(this Expression<Func<T, U>> exp)
        {
            string _pName = "";
            if (exp.Body is MemberExpression)
            {
                _pName = (exp.Body as MemberExpression).Member.Name;
            }

            else if (exp.Body is UnaryExpression)
            {
                _pName = ((exp.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            return _pName;
        }
    }
}
