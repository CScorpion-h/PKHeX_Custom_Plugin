using PKHeX.Core;
using System.Collections.Generic;
using CustomPlugin.Core.Editing;

namespace CustomPlugin.Core.Logic
{
    /// <summary>
    /// Multiple extended methods
    /// </summary>
    public static partial class PluginLogic
    {

        /// <summary>
        /// Personalize all <see cref="PKM"/> in all boxes.
        /// </summary>
        /// <param name="sav">Save File to personalize</param>
        /// <returns>Count of Pokemon that are personal</returns>
        public static int PersonalizeBoxes(this SaveFile sav)
        {
            if (!sav.HasBox)
                return -1;
            var count = 0;
            for (int i = 0; i < sav.BoxCount; i++)
            {
                var result = sav.PersonalizeBox(i);
                if (result < 0)
                    return result;
                count += result;
            }
            return count;
        }

        /// <summary>-
        /// Personalize all <see cref="PKM"/> in the specific box
        /// </summary>
        /// <param name="sav">Save File to personalize</param>
        /// <param name="box">Box to personalize</param>
        /// <returns>Count of Pokemon that are personal</returns>
        public static int PersonalizeBox(this SaveFile sav, int box)
        {
            var data = sav.GetBoxData(box);
            var count = sav.PersonalizeAll(data);
            if (count > 0)
                sav.SetBoxData(data, box);
            return count;
        }

        /// <summary>
        /// Personalize all <see cref="PKM"/> in the <see cref="data"/>.
        /// </summary>
        /// <param name="sav">Save File to personalize</param>
        /// <param name="data">Data to personalize</param>
        /// <returns></returns>
        public static int PersonalizeAll(this SaveFile sav, IList<PKM> data)
        {
            var count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var pkm = data[i];
                if (pkm == null || pkm.Species <= 0 || pkm.IsPersonalPKM(sav))
                    continue;

                var result = sav.Personalize(pkm);
                data[i] = result;
                count++;
            }
            return count;
        }
    }
}
