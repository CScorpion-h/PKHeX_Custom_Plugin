using CustomPlugin.Core.Logic;
using CustomPlugin.Core.Utils;
using System.Windows.Forms;

namespace PKHeXCustomPlugin.Plugins
{
    class PersonalizePlugin : CustomPlugin
    {
        public override string Name => "Personalize Pokemon";

        public override string MenuItemText => "一键自id";

        public override int Priority => 1;

        protected override void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = CreateMenuItem(MenuItemText, Name);
            ctrl.ShortcutKeys = Keys.Control | Keys.D1;
            ctrl.Click += (s, e) => Personalize();
            tools.DropDownItems.Add(ctrl);    
        }

        private void Personalize()
        {
            var curBox = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            if (curBox)
                PersonalizeCurrentBox();
            else
                PersonalizeAll();
        }

        private void PersonalizeCurrentBox()
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.PersonalizeBox(sav.CurrentBox);
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Personalize {count} Pokemon in current box");
        }

        private void PersonalizeAll()
        {
            var sav = SaveFileEditor.SAV;
            var count = sav.PersonalizeBoxes();
            if (count <= 0)
                return;
            SaveFileEditor.ReloadSlots();
            WinFormUtils.Alert($"Personalize {count} Pokemon across all boxes");
        }
    }
}
