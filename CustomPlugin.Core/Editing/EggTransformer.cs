using CustomPlugin.Core.Utils;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CustomPlugin.Core.Editing
{
    public static class EggTransformer
    {

        public static PKM SetPkmToEgg(this PKM pkm, GameVersion gameVersion)
        {
            int origin = pkm.GenNumber;
            int generation = pkm.Format;

            if (pkm.WasEvent ||        //Check FatefulEncounter
                 generation == 1 || Legal.NoHatchFromEgg.Contains(pkm.Species) ||   // NoHatch
                 origin != generation)    // Not the same generation
                return null;
            if (pkm.Species == 854 && pkm.AltForm == 1    // Antique Sinistea
                || pkm.Species == 855 && pkm.AltForm == 1 // Antique Polteageist
                || pkm.Species == 479 && pkm.AltForm != 0) // Polteageist
                return null;

            return SetToEgg(pkm, origin, gameVersion);
        }

        private static PKM SetToEgg(PKM pkm, int origin, GameVersion gameVersion)
        {
            int dayCare = origin <= 4 ? Locations.Daycare4 : Locations.Daycare5;
            int metLevel = origin <= 4 ? 0 : 1;
            int currentLevel = origin <= 4 ? 5 : 1;

            // 非初级形态
            PKM tmp = pkm.Clone();
            tmp.IsEgg = true;
            tmp.Egg_Location = dayCare;
            tmp.Data[0xA8] = tmp.Data[0xA8 + 1] = 0;      //Milotic
            List<CheckResult> checkResult = EncounterFinder.FindVerifiedEncounter(tmp).Parse;
            if (checkResult.IsPropStrInEleList("Comment", LegalityCheckStrings.LEvoInvalid))
                return null;

            int language = pkm.Language;
            pkm.Nickname = SpeciesName.GetSpeciesNameGeneration((int)Species.None, language, pkm.Format);
            pkm.IsNicknamed = true;
            pkm.IsEgg = true;
            pkm.HeldItem = 0;
            pkm.CurrentLevel = currentLevel;
            pkm.StatNature = pkm.Nature;
            pkm.RefreshAbility(new Random().Next(0, 3));

            pkm.SetIVs();
            pkm.EVs = new int[6];

            pkm.Ball = (int)Ball.Poke;
            pkm.Met_Location = 0;
            pkm.Met_Level = metLevel;
            pkm.Egg_Location = dayCare;

            ReflectUtils.methods.TryGetValue("GetEggMoves", out MethodInfo method);
            int[] result = (int[])method.Invoke(null, new object[] { pkm, pkm.Species, pkm.AltForm, gameVersion });
            pkm.SetMoves(new List<int>());
            if (result.Length == 0)
                pkm.SetRelearnMoves(pkm.GetSuggestedRelearnMoves());
            else
                pkm.SetRelearnMoves(GetEggMovesRandom(pkm, result));
            pkm.SetMoves(pkm.RelearnMoves);
            pkm.SetMovesPPUpsToZero();
            pkm.SetMaximumPPCurrent();
            pkm.FixMoves();

            // 8代独有数据
            if (pkm.Format == 8)
            {
                PK8 pk8 = (PK8)pkm;
                pk8.DynamaxLevel = 0;
                pk8.CanGigantamax = false;
            }

            pkm.ClearAllRibbon();
            pkm.ClearCurrentHandler();
            pkm.ClearMemories();
            pkm.ClearRecordFlags();
            pkm.CurrentFriendship = 4;

            return pkm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkm"></param>
        /// <param name="moves"></param>
        /// <returns></returns>
        private static int[] GetEggMovesRandom(PKM pkm, int[] moves)
        {
            int length = moves.Length;
            int count = length >= 4 ? 4 : length;
            List<int> eggMoves = new List<int>();
            Random rand = new Random();

            while (eggMoves.Count < count)
            {
                int move = moves[rand.Next(0, length)];
                if (!eggMoves.Contains(move))
                    eggMoves.Add(move);
            }

            while (eggMoves.Count < 4)
                eggMoves.Add(new LegalityAnalysis(pkm).GetSuggestedRelearnMoves()[eggMoves.Count - length]);

            return eggMoves.ToArray();
        }
    }
}
