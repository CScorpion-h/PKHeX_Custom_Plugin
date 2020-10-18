using CustomPlugin.Core.Editing;
using PKHeX.Core;
using System.Collections.Generic;

namespace CustomPlugin.Core.Logic
{
    /// <summary>
    /// Multiple extended methods
    /// </summary>
    public static partial class PluginLogic
    {
        public static PKM SetPIDCurrent(this SaveFile sav, PKM pkm, string flag)
        {
            var result = pkm.SetRandomPID(sav.Version, flag);
            return result;
        }

        public static int SetPIDBoxes(this SaveFile sav, string flag)
        {
            if (!sav.HasBox)
                return -1;
            var count = 0;
            for (int i = 0; i < sav.BoxCount; i++)
            {
                var result = sav.SetPIDBox(i, flag);
                if (result < 0)
                    return result;
                count += result;
            }
            return count;
        }

        public static int SetPIDBox(this SaveFile sav, int box , string flag)
        {
            var data = sav.GetBoxData(box);
            var count = sav.SetPIDAll(data, flag);
            if (count > 0)
                sav.SetBoxData(data, box);
            return count;
        }

        private static int SetPIDAll(this SaveFile sav, IList<PKM> data, string flag)
        {
            var count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var pkm = data[i];
                if (pkm == null || pkm.Species <= 0)
                    continue;

                var result = pkm.SetRandomPID(sav.Version, flag);
                LegalityAnalysis la = new LegalityAnalysis(result);
                if (!la.Valid)
                    continue;

                data[i] = result;
                count++;
            }
            return count;
        }
    }
}
