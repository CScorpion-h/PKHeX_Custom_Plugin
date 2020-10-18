using CustomPlugin.Core.Logic;
using CustomPlugin.Core.Utils;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKHeXCustomPlugin.Plugins
{
    class AutoPIDPlugin : CustomPlugin
    {
        public override string Name => "RandomPID";
        public override string MenuItemText => "随机PID";
        public override int Priority => 4;

        public string ButtonName { get; set; }
        private static readonly string[] flags = { "UnShiny", "StarShiny", "SquareShiny" };

        protected override void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = CreateMenuItem(MenuItemText, Name);
            var autoUnShiny = CreateMenuItem("非闪", "UnShiny");
            autoUnShiny.ShortcutKeys = Keys.Control | Keys.D3;
            var autoStarShiny = CreateMenuItem("星闪", "StarShiny");
            autoStarShiny.ShortcutKeys = Keys.Control | Keys.D4;
            var autoSquareShiny = CreateMenuItem("方闪", "SquareShiny");
            autoSquareShiny.ShortcutKeys = Keys.Control | Keys.D5;
            
            autoUnShiny.Click += (s, e) => AutoSetRandPID(s, e);
            autoStarShiny.Click += (s, e) => AutoSetRandPID(s, e);
            autoSquareShiny.Click += (s, e) => AutoSetRandPID(s, e);

            ctrl.DropDownItems.Add(autoUnShiny);
            ctrl.DropDownItems.Add(autoStarShiny);
            ctrl.DropDownItems.Add(autoSquareShiny);
            tools.DropDownItems.Add(ctrl);
        }

        private void AutoSetRandPID(object sender, EventArgs e)
        {
            var flag = ((ToolStripMenuItem)sender).Name;
            var current = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            if (current)
            {
                RandPIDCurrent(flag);
                return;
            }
            
            var all = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            if (all)
                RandPIDAllBoxes(flag);
            else
                RandPIDCurrentBox(flag);
        }

        private void RandPIDCurrentBox(string flag)
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.SetPIDBox(sav.CurrentBox, flag);
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Set {count} Pokemon To {flag} in current box");
        }

        private void RandPIDAllBoxes(string flag)
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.SetPIDBoxes(flag);
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Set {count} Pokemon To {flag} across all boxes");
        }

        private void RandPIDCurrent(string flag)
        {
            var pkm = PKMEditor.PreparePKM();
            var sav = SaveFileEditor.SAV;
            var result = sav.SetPIDCurrent(pkm, flag);

            LegalityAnalysis la = new LegalityAnalysis(result);
            if (!la.Valid)
            { 
                WinFormUtils.Alert($"Can not Set Current Pokemon to {flag}!");
                return;
            }

            PKMEditor.PopulateFields(result);
            WinFormUtils.Alert($"Set Current Pokemon to {flag}!");
        }
    }
}
