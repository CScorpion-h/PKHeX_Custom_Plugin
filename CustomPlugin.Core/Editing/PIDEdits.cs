using CustomPlugin.Core.Legality;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

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
            bool flag = pkm.IsEgg || pkm.WasEgg || pkm.Met_Level <= 1;
            if (flag)
            {
                pkm.SetPIDGender(pkm.Gender);
                CommonEdits.SetShiny(pkm, type);
                return;
            }

            if (pkm.Format == 3)
                pkm.Gen3Shiny(type);
           if (pkm.Format == 4)
                pkm.Gen4Shiny(type);
            if (pkm.Format == 5)
                pkm.Gen5Shiny(type);

            SetBasicData(pkm);
        }

        public static void Gen5Shiny(this PKM pkm, Shiny shinyType)
        {
            if (pkm.GenNumber == 3)
            {
                if (pkm.Met_Location == Locations.Transfer4 && pkm.Ball == 4)
                {
                    pkm.SetPIDGender(pkm.Gender);
                    CommonEdits.SetShiny(pkm, shinyType);
                    return;
                }
                Gen3UnShiny(pkm);
                return;
            }

            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;
            IEnumerable<uint> seeds = null;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                CommonEdits.SetShiny(pkm, shinyType);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (!pkm.IsShiny || seeds == null);

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));

        }

        public static void Gen4Shiny(this PKM pkm, Shiny shinyType)
        {
            if (pkm.GenNumber == 3)
            {
                if (pkm.Met_Location == 55 && pkm.Ball == 4)
                {
                    pkm.SetPIDGender(pkm.Gender);
                    CommonEdits.SetShiny(pkm, shinyType);
                    return;
                }
                Gen3Shiny(pkm, shinyType);
                return;
            }

            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;
            if (pkm.Met_Location == 233)
                type = PIDType.Pokewalker;

            IEnumerable<uint> seeds;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                CommonEdits.SetShiny(pkm, shinyType);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (!pkm.IsShiny || seeds == null);

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
        }

        public static void Gen3Shiny(this PKM pkm, Shiny shinyType)
        {
            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;

            if (pkm.Species == 201)
                type = PIDType.Method_1_Unown;

            IEnumerable<uint> seeds;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                CommonEdits.SetShiny(pkm, shinyType);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (!pkm.IsShiny || seeds == null);

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
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
                pkm.Gen5UnShiny();

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

            IEnumerable<uint> seeds;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (pkm.IsShiny || seeds == null);

            if (pkm.Species == 201)
                type = PIDType.Method_1_Unown;

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
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
            
            IEnumerable<uint> seeds;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (pkm.IsShiny || seeds == null);

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
        }

        public static void Gen5UnShiny(this PKM pkm)
        {
            var generation = pkm.GenNumber;
            if (generation == 3)
            {
                if (pkm.Met_Location == Locations.Transfer4 && pkm.Ball == 4)
                {
                    pkm.SetPIDGender(pkm.Gender);
                    return;
                }
                Gen3UnShiny(pkm);
                return;
            }

            RNG rng = RNG.LCRNG;
            PIDType type = PIDType.Method_1;
            IEnumerable<uint> seeds = null;
            do
            {
                pkm.SetPIDGender(pkm.Gender);
                seeds = GetSeedsFromPID(pkm.PID, rng);
            } while (pkm.IsShiny || seeds == null);

            PIDGenerator.SetValuesFromSeed(pkm, type, seeds.ElementAt(0));
        }

        internal static void SetBasicData(PKM pkm)
        {
            var newPID = pkm.PID;
            var ability = pkm.GenNumber < 5 ? (int)(newPID & 1) : (int)(newPID >> 16) & 1;
            pkm.SetGender(PKX.GetGenderFromPID(pkm.Species, newPID));
            if (pkm.GenNumber < 5)
                pkm.SetNature((int)(newPID % 25));
            switch (pkm.GenNumber)
            {
                case 3:
                    var pk3 = new PK3 { PID = newPID, Species = pkm.Species };
                    var pi3 = (PersonalInfoG3)pk3.PersonalInfo;
                    if (!pi3.HasSecondAbility)
                        pkm.SetAbility(pi3.Ability1);
                    else
                        pkm.SetAbility(pi3.Abilities[ability]);
                    break;
                default:
                    pkm.RefreshAbility(ability);
                    break;
            }
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

        internal static void SetUnownForm(PKM pkm)
        {
            
        }

        internal static IEnumerable<uint>? GetSeedsFromPID(uint pid, RNG rng)
        {

            var half = GetHalfPID(pid);
            var seeds = MethodFinder.GetSeedsFromPIDEuclid(rng, half[0], half[1]);
            if (seeds.Count() == 0)
                return null;
            return seeds;
        }

        public static uint[] GetHalfPID(uint pid)
        {
            var top = pid >> 16;
            var bot = pid & 0xFFFF;
            return new uint[] { top, bot };
        }
    }
}
