using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common.NotifyBase
{
    public class PropertyNotifyObject : NotifyPropertyBase
    {
        private Dictionary<object, object> _valueDictionary = new Dictionary<object, object>();

        public T GetValue<T>(string propertyName)
        {
            object _propertyValue;

            if (!_valueDictionary.TryGetValue(propertyName, out _propertyValue))
            {
                _propertyValue = default(T);
                _valueDictionary.Add(propertyName, _propertyValue);

            }

            return (T)_propertyValue;
        }

        public void SetValue(string propertyName, object value)
        {
            if (!_valueDictionary.ContainsKey(propertyName) || _valueDictionary[propertyName] != (object)value)
            {

                _valueDictionary[propertyName] = value;

                OnPropertyChanged(propertyName);
            }
        }
    }

    public static class PropertyNotifyObjectEx
    {
        public static U GetValue<T, U>(this T t, Expression<Func<T, U>> exp) where T : PropertyNotifyObject
        {
            var name = exp.GetName<T, U>();
            return t.GetValue<U>(name);
        }

        public static void SetValue<T, U>(this T t, Expression<Func<T, U>> exp, U value) where T : PropertyNotifyObject
        {
            var name = exp.GetName<T, U>();

            t.SetValue(name, value);
        }
    }
}
