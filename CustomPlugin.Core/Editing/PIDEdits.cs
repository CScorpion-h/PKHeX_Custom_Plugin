using PKHeX.Core;
using System;
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

            SetBasicData(pkm);
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
                if (pkm.Met_Location == 55 && pkm.Ball == 4)
                {
                    pkm.SetPIDGender(pkm.Gender);
                    return;
                }
                Gen3UnShiny(pkm);
                return;
            }

            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;
            if (pkm.Met_Location == 233)
                type = PIDType.Pokewalker;
            SetPIDIV(pkm, rng, type);
        }

        public static void Gen5UnShiny(this PKM pkm)
        {
            SetPIDIV(pkm, RNG.LCRNG, PIDType.Method_1);
        }

        internal static void SetBasicData(PKM pkm)
        {
            var newPID = pkm.PID;
            var ability = pkm.GenNumber < 5 ? newPID & 0x0000_0001 : newPID & 0x0001_0000;
            if (pkm.Format != pkm.GenNumber && pkm.GenNumber == 3)
            {
                PK3 pk3 = new PK3 { PID=newPID };
                pk3.RefreshAbility(pk3.PIDAbility);
                pkm.AbilityNumber = pk3.AbilityNumber;
            } else
            {
                pkm.RefreshAbility((int)ability);
            }
            pkm.SetGender(PKX.GetGenderFromPID(pkm.Species, newPID));
            pkm.PID = newPID;
        }
            
        /// <summary>
        /// 说明：对1-2代的精灵进行非闪光处理（随机个体值）
        /// 细节：未知图腾形态与个体值相关|默认无需出现相同未知图腾
        /// </summary>
        /// <param name="pkm"></param>
        public static void Gen12UnShiny(this PKM pkm)
        {
            pkm.SetRandomIVs();
            if (pkm.Species == 201) //Unown
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
