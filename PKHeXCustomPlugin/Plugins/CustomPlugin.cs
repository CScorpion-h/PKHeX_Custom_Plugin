using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using PKHeX.Core;
using CustomPlugin.Core.Utils;
using CustomPlugin.Core.Structure;
using CustomPlugin.Core.Logic;

namespace PKHeXCustomPlugin.Plugins
{
    /// <summary>
    /// Base Plugin : provide basic function
    /// </summary>
    public abstract class CustomPlugin : IPlugin
    {
        private const string MenuParent = "Menu_Tools";
        private const string MenuName = "Menu_CustomPlugin";
        private const string CtrlName = "自定义插件";    // => GetString(name, Language)
        protected static bool LimitedVerPKMLoaded = false;
        protected static bool MethodListLoaded = false;
        
        public ISaveFileProvider SaveFileEditor { get; private set; }
        public IPKMView PKMEditor { get; private set; }
        
        public abstract string Name { get; }
        public abstract string MenuItemText { get; }
        public abstract int Priority { get; } // Loading order, lowest is first.

        public void Initialize(params object[] args)
        {
            Debug.WriteLine($"Loading {Name}...");
            
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider);
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView);
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip);

            InitialMethodList();
            InitialLimitedVerPKM();

            LoadMenuStrip(menu);
        }

        /// <summary>
        /// initial the method in PKHeX.Core
        /// </summary>
        private void InitialMethodList()
        {
            if (!MethodListLoaded)
            { 
                Assembly coreAsmbly = typeof(ISaveFileProvider).GetTypeInfo().Assembly;
                ReflectUtils.AddMethodToList(coreAsmbly, "PKHeX.Core.MoveEgg", "GetEggMoves");
                MethodListLoaded = true;
            }
        }

        /// <summary>
        /// initial the list of pokemon that can be capture in different version
        /// </summary>
        private void InitialLimitedVerPKM()
        {
            // to be modified code ...
            if (!LimitedVerPKMLoaded)
            {
                LimitedVerPKM lazy = CheckLimitedPKM.Instance;
                //Initial Limited Version Pokemon
                lazy.InitLimitedVerPkm("pokemon_FR");
                lazy.InitLimitedVerPkm("pokemon_LG");
                lazy.InitLimitedVerPkm("pokemon_R");
                lazy.InitLimitedVerPkm("pokemon_S");
                lazy.InitLimitedVerPkm("pokemon_E");
                //CXD = GetStrings("pokemon_CXD", "text");
                lazy.InitLimitedVerPkm("pokemon_D");
                lazy.InitLimitedVerPkm("pokemon_P");
                lazy.InitLimitedVerPkm("pokemon_Pt");
                lazy.InitLimitedVerPkm("pokemon_HG");
                lazy.InitLimitedVerPkm("pokemon_SS");
                lazy.InitLimitedVerPkm("pokemon_W");
                lazy.InitLimitedVerPkm("pokemon_B");
                lazy.InitLimitedVerPkm("pokemon_W2");
                lazy.InitLimitedVerPkm("pokemon_B2");
                lazy.InitLimitedVerPkm("pokemon_X");
                lazy.InitLimitedVerPkm("pokemon_Y");
                lazy.InitLimitedVerPkm("pokemon_OR");
                lazy.InitLimitedVerPkm("pokemon_AS");
                lazy.InitLimitedVerPkm("pokemon_SN");
                lazy.InitLimitedVerPkm("pokemon_MN");
                lazy.InitLimitedVerPkm("pokemon_US");
                lazy.InitLimitedVerPkm("pokemon_UM");
                lazy.InitLimitedVerPkm("pokemon_GP");
                lazy.InitLimitedVerPkm("pokemon_GE");
                lazy.InitLimitedVerPkm("pokemon_SW");
                lazy.InitLimitedVerPkm("pokemon_SH");
                LimitedVerPKMLoaded = true;
            }
        }

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            var tools = items.Find(MenuParent, false)[0] as ToolStripDropDownItem;
            var basemenuSearch = tools.DropDownItems.Find(MenuName, false);
            var basemenu = GetBaseMenu(tools, basemenuSearch);
            AddPluginControl(basemenu);
        }

       
        private static ToolStripMenuItem GetBaseMenu(ToolStripDropDownItem tools, ToolStripItem[] basemenuSearch)
        {
            if (basemenuSearch.Length != 0) //If already loaded, there is no need to load it again.
                return (ToolStripMenuItem)basemenuSearch[0];

            var basemenu = CreateMenuItem(CtrlName, MenuName);
            tools.DropDownItems.Add(basemenu);
            return basemenu;
        }

        protected static ToolStripMenuItem CreateMenuItem(string ctrlText, string ctrlName)
        {
            return new ToolStripMenuItem(ctrlText)
            {
                // to be added ...
                Name = ctrlName
            };
        }

        protected abstract void AddPluginControl(ToolStripDropDownItem tools);

        public void NotifySaveLoaded()
        {
            Console.WriteLine($"{Name} was notified that a Save File was just loaded.");
        }

        public bool TryLoadFile(string filePath)
        {
            Console.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
            return false; // no action taken
        }
    }
}
