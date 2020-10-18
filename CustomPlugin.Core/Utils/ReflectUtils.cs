using System;
using System.Collections.Generic;
using System.Reflection;

namespace CustomPlugin.Core.Utils
{
    public static class ReflectUtils
    {
        public static SortedDictionary<string, MethodInfo> methods = new SortedDictionary<string, MethodInfo>();

        public static BindingFlags InstanceBindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static void AddMethodToList(Assembly assembly, string typeName, string methodName)
        {
            var type = assembly.GetType(typeName);
            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo != null)
                methods.Add(methodName, methodInfo);
        }

        public static T GetPropertyValue<T>(Object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName, InstanceBindFlags);
            if (property == null)
                return default;
            return (T)property.GetValue(obj);
        }

        public static void SetPropertyValue(Object obj, string propertyName, Object value)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName, InstanceBindFlags);
            if (property == null)
                return;
            Object v = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(obj, v, null);
        }
    }
}