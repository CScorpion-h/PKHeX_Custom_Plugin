using CustomPlugin.Core.Structure;
using CustomPlugin.Core.Utils;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CustomPlugin.Core.Logic
{
    public static class CheckLimitedPKM
    {
        private static readonly Lazy<LimitedVerPKM> lazy =
  new Lazy<LimitedVerPKM>(() => new LimitedVerPKM());

        public static LimitedVerPKM Instance => lazy.Value;

        private static string? CurrentPkmVer { get; set; }
        private static string? CurrentGameVer { get; set; }

        public const BindingFlags InstanceBindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Check for Game-exclusive Pokemon
        /// </summary>
        /// <param name="pkm">Pokemon to be checked</param>
        /// <param name="gameVersion">Current game version</param>
        /// <returns>Whether it is the result of Game-exclusive Pokemon.</returns>
        public static bool IsRealLimited(PKM pkm, int gameVersion)
        {
            bool isHatch = false;
            bool isLimitedPkm = IsLimitedPkm(pkm.Version, pkm.Species, gameVersion);
            bool temp = ArrayUtils.IsEleInArray(Instance.GetHatchZeroArr(), CurrentPkmVer);// Check for Egg-Hatch pokemon
            if (temp)
                /// Before Gen4, need to check whether met_level is 0
                isHatch = pkm.Met_Level == 0 ? false : true;
            else
                isHatch = pkm.Met_Level == 1 ? false : true;
            return isLimitedPkm && isHatch;
        }

        /// <summary>
        /// Check if the current pokemon can be captured in the both versions.
        /// </summary>
        /// <param name="version">Pokemon original version</param>
        /// <param name="species">Pokemon species</param>
        /// <param name="gameVersion">Current game version</param>
        /// <returns></returns>
        private static bool IsLimitedPkm(int version, int species, int gameVersion)
        {
            CurrentPkmVer = CommonUtil.GetGameVersionStrByKey(version);
            CurrentGameVer = CommonUtil.GetGameVersionStrByKey(gameVersion);

            List<int> limitedPkms = ReflectUtils.GetPropertyValue<List<int>>(Instance, CurrentPkmVer); ;
            List<int> limitedGamePkms = ReflectUtils.GetPropertyValue<List<int>>(Instance, CurrentGameVer); ;
            if (limitedPkms != null && limitedPkms.Contains(species))
            {
                if (limitedGamePkms != null && limitedGamePkms.Contains(species))
                    return false;
                return true;
            }
            return false;
        }
    }
}
