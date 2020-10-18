using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomPlugin.Core.Utils
{
    public static partial class CommonUtil
    {

        private static readonly Assembly thisAssembly = typeof(CommonUtil).GetTypeInfo().Assembly;
        private static readonly string[] manifestResourceNames = thisAssembly.GetManifestResourceNames();
        private static readonly Dictionary<string, string> resourceNameMap = new Dictionary<string, string>();

        // 防止多次读取资源文件
        public static readonly Dictionary<string, string> stringDictionaryCache = new Dictionary<string, string>();
        // 资源文件内容转换成数据的线程锁
        private static readonly object getStringDictLoadLock = new object();

        private static string? FileType { get; set; }
        // 读取列表
        public static string GetFileContent(string fileName, string type = "text") => GetFileContent($"{type}_{fileName}");

        public static string GetFileContent(string fileName)
        {
            string data;
            // 判断是否读取过资源文件
            if (IsStringLoadCached(fileName, out var result))
                return result;
            data = GetStringResource(fileName);
            if (data != null && data.Length != 0)
                stringDictionaryCache.Add(fileName, data);
            return data;
        }

        private static SortedDictionary<int, string> HandleJsonToData(string fileName, string jsonFile)
        {
            SortedDictionary<int, string> data = new SortedDictionary<int, string>();
            if (jsonFile == null)
                return data;
            return data;
        }

        /**
         * 获取资源文件并读取内容
         */
        private static string? GetStringResource(string fileName)
        {
            FileType = fileName.Split('_')[0];
            if (!resourceNameMap.TryGetValue(fileName, out var resourceName))
            {
                if (FileType == "text")
                {
                    bool Match(string x) => x.StartsWith("CustomPlugin.Core.Resources.text.") && x.EndsWith($"{fileName}.txt", StringComparison.OrdinalIgnoreCase);
                    resourceName = Array.Find(manifestResourceNames, Match);
                }
                else if (FileType == "json")
                {
                    bool Match(string x) => x.StartsWith("CustomPlugin.Core.Resources.json.") && x.EndsWith($"{fileName}.json", StringComparison.OrdinalIgnoreCase);
                    resourceName = Array.Find(manifestResourceNames, Match);
                }

                if (resourceName == null)
                    return null;
                resourceNameMap.Add(fileName, resourceName);
            }

            using var resource = thisAssembly.GetManifestResourceStream(resourceName);
            if (resource == null)
                return null;
            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();
        }

        private static bool IsStringLoadCached(string fileName, out string result)
        {
            lock (getStringDictLoadLock)
                return stringDictionaryCache.TryGetValue(fileName, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetGameVersionStrByKey(int key)
        {
            return Enum.GetName(typeof(GameVersion), key);
        }
    }
}
