using CustomPlugin.Core.Editing;
using CustomPlugin.Core.Legality;
using PKHeX.Core;
using System.Collections.Generic;

namespace CustomPlugin.Core.Logic
{
    /// <summary>
    /// Multiple extended methods
    /// </summary>
    public static partial class PluginLogic
    {
        /// <summary>
        /// Randomize the PID of active pokemon 
        /// </summary>
        /// <param name="sav">Save File to randomize</param>
        /// <param name="pkm">active pokemon</param>
        /// <param name="flag">Shiny/UnShiny flag</param>
        /// <returns>Pokemon with new PID</returns>
        public static PKM SetPIDCurrent(this SaveFile sav, PKM pkm, string flag)
        {
            var result = pkm.SetRandomPID(sav.Version, flag);
            return result;
        }

        /// <summary>
        /// Randomize the PID of all <see cref="PKM"/> in all boxes.
        /// </summary>
        /// <param name="sav">Save File to randomize</param>
        /// <param name="flag">Shiny/UnShiny flag</param>
        /// <returns>Count of Pokemon that are randomized PID</returns>
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

        /// <summary>
        /// Randomize the PID of all <see cref="PKM"/> in the specific box.
        /// </summary>
        /// <param name="sav">Save File to randomize</param>
        /// <param name="box">Box to randomize</param>
        /// <param name="flag">Shiny/UnShiny flag</param>
        /// <returns>Count of Pokemon that are randomized PID</returns>
        public static int SetPIDBox(this SaveFile sav, int box , string flag)
        {
            var data = sav.GetBoxData(box);
            var count = sav.SetPIDAll(data, flag);
            if (count > 0)
                sav.SetBoxData(data, box);
            return count;
        }

        /// <summary>
        /// Randomize the PID of all <see cref="PKM"/> in the <see cref="data"/>.
        /// </summary>
        /// <param name="sav">Save File to personalize</param>
        /// <param name="data">Data to personalize</param>
        /// <param name="flag">Shiny/UnShiny flag</param>
        /// <returns></returns>
        private static int SetPIDAll(this SaveFile sav, IList<PKM> data, string flag)
        {
            var count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var pkm = data[i];
                if (pkm == null || pkm.Species <= 0)
                    continue;

                var result = pkm.SetRandomPID(sav.Version, flag);
                if (result.Species == 201)
                    Legalizer.SetMetValid(result);
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
