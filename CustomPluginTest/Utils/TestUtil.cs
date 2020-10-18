using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomPluginTest
{
    class TestUtil
    {

        public static string GetTestFolder(string name)
        {
            var folder = Directory.GetCurrentDirectory();
            while (!folder.EndsWith(nameof(CustomPluginTest)))
                folder = Directory.GetParent(folder).FullName;
            return Path.Combine(folder, name);
        }

    }
}
