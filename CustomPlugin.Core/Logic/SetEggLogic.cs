using CustomPlugin.Core.Editing;
using PKHeX.Core;
using System.Collections.Generic;

namespace CustomPlugin.Core.Logic
{
    public static partial class PluginLogic
    {
        public static PKM SetEggCurrent(this SaveFile sav, PKM pkm)
        {
            return pkm.SetPkmToEgg(sav.Version);
        }

        public static int SetEggBoxes(this SaveFile sav)
        {
            if (!sav.HasBox)
                return -1;
            var count = 0;
            for (int i = 0; i < sav.BoxCount; i++)
            {
                var result = sav.SetEggBox(i);
                if (result < 0)
                    return result;
                count += result;
            }
            return count;
        }

        public static int SetEggBox(this SaveFile sav, int box)
        {
            var data = sav.GetBoxData(box);
            var count = sav.SetEggAll(data);
            if (count > 0)
                sav.SetBoxData(data, box);
            return count;
        }

        private static int SetEggAll(this SaveFile sav, IList<PKM> data)
        {
            var count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var pkm = data[i];
                if (pkm == null || pkm.Species <= 0 || pkm.IsEgg)
                    continue;

                var result = pkm.SetPkmToEgg(sav.Version);
                if (result == null)
                    continue;

                data[i] = result;
                count++;
            }
            return count;
        }
    }
}
