using System;
using System.Windows.Forms;

namespace CustomPlugin.Core.Utils
{
    public static class WinFormUtils
    {
        public static DialogResult Alert(params string[] lines)
        {
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, nameof(Alert), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
