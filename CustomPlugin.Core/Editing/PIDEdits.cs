using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace CustomPlugin.Core.Editing
{
    public static class PIDEdits
    {
        public static List<int> UnownForm { get; set; } = new List<int>();

        public static void Gen678Shiny(this PKM pkm, Shiny type)
        {
            pkm.SetPIDGender(pkm.Gender);
            CommonEdits.SetShiny(pkm, type);
            pkm.SetRandomEC();
        }

        public static void Gen345Shiny(this PKM pkm, Shiny type)
        {
            
        }

        public static void Gen12Shiny(this PKM pkm)
        {
            pkm.SetShiny();
        }

        public static void Gen678UnShiny(this PKM pkm)
        { 
            pkm.SetShiny();
            pkm.SetUnshiny();
            pkm.SetRandomEC();
        }

        public static void Gen345UnShiny(this PKM pkm)
        {
            pkm.SetShiny();
            bool flag = pkm.IsEgg || pkm.WasEgg || pkm.Met_Level <= 1;
            if (flag)
            {
                pkm.SetPIDGender(pkm.Gender);
                return;
            }

            if (pkm.Format == 3)
                pkm.Gen3UnShiny();
            if (pkm.Format == 4)
                pkm.Gen4UnShiny();
            if (pkm.Format == 5)
                pkm.Gen3UnShiny();

            var newPID = pkm.PID;
            pkm.SetGender(PKX.GetGenderFromPID(pkm.Species, pkm.PID));
            pkm.RefreshAbility(pkm.GenNumber == 3? 0 : pkm.PIDAbility);
            pkm.PID = newPID;
        }

        public static void Gen3UnShiny(this PKM pkm)
        {
            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;

            if (pkm.Version == (int)GameVersion.CXD) {
                rng = RNG.XDRNG;
                type = PIDType.CXD;
            }
            if (pkm.Species == 201)
                type = PIDType.Method_3_Unown;

            SetPIDIV(pkm, rng, type);

        }

        public static void Gen4UnShiny(this PKM pkm)
        {
            if (pkm.GenNumber == 3)
            {
                Gen3UnShiny(pkm);
                return;
            }
            SetPIDIV(pkm, RNG.LCRNG, PIDType.Method_1);
        }

        public static void Gen5UnShiny(this PKM pkm)
        {
            SetPIDIV(pkm, RNG.LCRNG, PIDType.Method_1);
        }

        /// <summary>
        /// 说明：对1-2代的精灵进行非闪光处理（随机个体值）
        /// 细节：未知图腾形态与个体值相关|默认无需出现相同未知图腾
        /// </summary>
        /// <param name="pkm"></param>
        public static void Gen12UnShiny(this PKM pkm)
        {
            pkm.SetRandomIVs();
            if (pkm.Species == 201) //未知图腾
            {
                if (UnownForm.Count == 26)
                    UnownForm.Clear();
                while (UnownForm.Contains(pkm.AltForm))
                    pkm.SetRandomIVs();
                UnownForm.Add(pkm.AltForm);
            }
        }

        public static void SetPIDIV(PKM pkm, RNG method, PIDType type)
        {
            IEnumerable<uint> seeds;
            uint pid;
            uint[] half;
            do
            {
                if (pkm.Species == 201)
                {
                    pkm.SetPIDUnown3(pkm.AltForm);
                    pid = pkm.PID;
                } else
                    pid = Util.Rand32();

                half = GetHalfPID(pid);
                seeds = MethodFinder.GetSeedsFromPIDEuclid(method, half[0], half[1]);
            } while (seeds.Count() == 0);
            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
        }

        public static uint[] GetHalfPID(uint pid)
        {
            var top = pid >> 16;
            var bot = pid & 0xFFFF;
            return new uint[] { top, bot };
        }
    }
}
