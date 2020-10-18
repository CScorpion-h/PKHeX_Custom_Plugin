using CustomPlugin.Core.Legality;
using CustomPlugin.Core.Logic;
using CustomPlugin.Core.Structure;
using CustomPlugin.Core.Utils;
using PKHeX.Core;

namespace CustomPlugin.Core.Editing
{
    /// <summary>
    /// Specific logic to personalize <see cref="PKM"/>
    /// </summary>
    public static class Personalizer
    {
        static TrainerInfoInSav? trainerInfo;

        /// <summary>
        /// Tries to set the <see cref="PKM"/> to personal
        /// </summary>
        /// <param name="pkm">Pokemon to be personalized</param>
        /// <returns>Personalized Pokemon</returns>
        public static PKM Personalize(this SaveFile sav, PKM pkm)
        {
            int origin = pkm.GenNumber;
            int generation = pkm.Format;

            if (origin != generation)   // not in the same generation
            {
                pkm.HT_Name = trainerInfo.OT;
                pkm.HT_Gender = trainerInfo.Gender;
                pkm.HT_Friendship = pkm.CurrentFriendship;
                pkm.CurrentHandler = 1;
                return pkm;
            }

            // ↓ in the same generation
            if (pkm.WasEvent || pkm.WasEventEgg || pkm.WasGiftEgg || IsInGameTrade(pkm))  // Check FatefulEncounter
                return pkm;
            if (pkm.IsEgg)
                return pkm.Personalize(false);

            GameVersion gameVersion = sav.Version;
            return PersonalizePokemon(pkm, gameVersion);
        }

        /// <summary>
        /// Truly code to personalize <see cref="PKM"/>
        /// </summary>
        /// <param name="pkm">Pokemon to be personalized</param>
        /// <param name="gameVersion">Current Game Version</param>
        /// <returns></returns>
        private static PKM PersonalizePokemon(PKM pkm, GameVersion gameVersion)
        {
            bool isLimited = isLimitedPokemon(pkm, gameVersion);
            if (!isLimited)
            {
                // Change the original version to game version
                pkm.Version = (int)gameVersion != 56 ? (int)gameVersion : (int)gameVersion;
                // change version => change met data
                if (pkm.Format >= 3)
                    Legalizer.SetMetValid(pkm);
            }
            Personalize(pkm, isLimited);
            
            int format = pkm.Format;
            if (format == 6 || format == 7 || format == 8)
            {
                // Check if it is a pokemon that evolves to link trade
                if (!LinkTradeEvo.GetLinkTradeEvoPkmList.Contains(pkm.Species) && !isLimited)
                {
                    pkm.ClearCurrentHandler();

                    if (pkm is IGeoTrack g)
                    { 
                        // to be modified
                        g.Geo1_Country = 0;
                        g.Geo1_Region = 0;
                        g.Geo2_Country = 0;
                        g.Geo2_Region = 0;
                        g.Geo3_Country = 0;
                        g.Geo3_Region = 0;
                        g.Geo4_Country = 0;
                        g.Geo4_Region = 0;
                        g.Geo5_Country = 0;
                        g.Geo5_Region = 0;
                    }

                    ITrainerMemories trainerMemories = (ITrainerMemories)pkm;
                    Extensions.ClearMemoriesHT(trainerMemories);
                }
            }
            return pkm;
        }

        /// <summary>
        /// Check pokemon that can be capture in the current version
        /// </summary>
        /// <param name="pkm">Catchable pokemon to be checked</param>
        /// <param name="gameVersion">Current game version</param>
        /// <returns></returns>
        private static bool isLimitedPokemon(this PKM pkm, GameVersion gameVersion)
        {
            int pkmVersion = pkm.Version;
            bool isLimited = false;
            if (pkmVersion != (int)gameVersion)     // not in the same version
            {
                int version = (int)gameVersion;
                switch (pkm.Format)
                {
                    case 3:
                        isLimited = (pkmVersion == (int)GameVersion.CXD) ? true : CheckLimitedPKM.IsRealLimited(pkm, version);
                        break;
                    case 4:
                        if (pkmVersion == (int)GameVersion.DP)
                            version = pkmVersion == (int)GameVersion.D ? (int)GameVersion.P : (int)GameVersion.D;
                        isLimited = CheckLimitedPKM.IsRealLimited(pkm, version);
                        break;
                    default:
                        isLimited = CheckLimitedPKM.IsRealLimited(pkm, version);
                        break;
                }
            }

            return isLimited;
        }

        /// <summary>
        /// Universal personalization
        /// </summary>
        /// <param name="pkm">Pokemon to modify</param>
        /// <param name="isLimited">Causes the TID/SID value to be changed</param>
        /// <returns>Basic personalized pokemon</returns>
        private static PKM Personalize(this PKM pkm, bool isLimited = false)
        {
            var xorNum = isLimited? 10001011 : 0;       //10001011 to be modified
            pkm.TID = trainerInfo.TID ^ xorNum;
            pkm.SID = trainerInfo.SID ^ xorNum;
            pkm.OT_Name = trainerInfo.OT;
            pkm.OT_Gender = trainerInfo.Gender;
            return pkm;
        }

        /// <summary>
        /// Check if pokemon's encounter type is an in-game trade
        /// </summary>
        /// <param name="pkm">Pokemon to be checked</param>
        /// <returns></returns>
        private static bool IsInGameTrade(PKM pkm)
        {
            switch(pkm.Met_Location)
            {
                case Locations.LinkTrade2NPC:
                case Locations.LinkTrade3NPC:
                case Locations.LinkTrade4NPC:
                case Locations.LinkTrade5NPC:
                case Locations.LinkTrade6NPC:
                    return true;
                default:
                    return false;
            }
        }

        internal static bool IsPersonalPKM(this PKM pkm, SaveFile sav)
        {
            trainerInfo = TrainerInfoUtils.GetTrainerInfo(sav);
            if (trainerInfo.SID == pkm.SID && trainerInfo.TID == pkm.TID)
                return true;
            return false;
        }


    }
}
