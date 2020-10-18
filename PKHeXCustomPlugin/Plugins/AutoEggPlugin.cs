using CustomPlugin.Core.Logic;
using CustomPlugin.Core.Utils;
using System.Windows.Forms;

namespace PKHeXCustomPlugin.Plugins
{
    class AutoEggPlugin : CustomPlugin
    {
        public override string Name => "AutoSetEgg";

        public override string MenuItemText => "一键变蛋";

        public override int Priority => 3;
        
        protected override void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = CreateMenuItem(MenuItemText, Name);
            ctrl.ShortcutKeys = Keys.Control | Keys.D2;
            ctrl.Click += (s, e) => AutoEgg();
            tools.DropDownItems.Add(ctrl);
        }

        private void AutoEgg()
        {
            var curBox = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            if (curBox)
            {
                AutoEggCurrentBox();
                return;
            }

            var all = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            if (all)
                AutoEggAllBoxes();
            else
                AutoEggCurrent();
        }

        private void AutoEggCurrent()
        {
            var pkm = PKMEditor.PreparePKM();
            if (pkm.IsEgg)
                return;     // already egg, should not modify it

            var sav = SaveFileEditor.SAV;
            var result = sav.SetEggCurrent(pkm);

            // Check Null
            if (result == null) {
                WinFormUtils.Alert($"Current Pokemon should not to be an Egg!");
                return; 
            }
            PKMEditor.PopulateFields(result);
            WinFormUtils.Alert($"Set Current Pokemon to Egg!");
        }

        private void AutoEggAllBoxes()
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.SetEggBoxes();
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Set {count} Pokemon To Egg across all boxes");
        }

        private void AutoEggCurrentBox()
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.SetEggBox(sav.CurrentBox);
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Set {count} Pokemon To Egg in current box");
        }
    }
}
