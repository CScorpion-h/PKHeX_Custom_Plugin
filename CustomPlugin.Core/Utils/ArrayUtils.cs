using System.Collections.Generic;

namespace CustomPlugin.Core.Utils
{
    static class ArrayUtils
    {
        public static bool IsPropStrInEleList<T>(this List<T> list, string propName, object value)
        {
            foreach (T t in list)
            {
                if (ReflectUtils.GetPropertyValue<object>(t, propName) == value)
                    return true;
            }
            return false;
        }

        public static IEnumerable<T> CheckNull<T>(this IEnumerable<T> list)
        {
            return list ?? new List<T>(0);
        }

        public static bool IsEleInArray<T>(T[] arr, T element)
        {
            foreach (T index in arr)
            {
                if (index.Equals(element))
                    return true;
            }
            return false;
        }
    }
}
