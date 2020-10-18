using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomPlugin.Core.Structure
{
    /// <summary>
    /// 
    /// </summary>
    public class TrainerInfoInSav
    {
        private readonly SaveFile sav;

        public TrainerInfoInSav(SaveFile saveFile) => sav = saveFile;

        public string OT => sav.OT;
        public int Gender => sav.Gender;
        public int Game => sav.Game;
        public int Language => sav.Language;
        public int Generation => sav.Generation;

        public int TID { get => sav.TID; set => throw new ArgumentException("Setter for this object should never be called."); }
        public int SID { get => sav.SID; set => throw new ArgumentException("Setter for this object should never be called."); }
    }
}
