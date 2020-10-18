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
        /// 
        /// </summary>
        /// <param name="pkm"></param>
        /// <param name="gameVersion"></param>
        /// <returns></returns>
        public static bool IsRealLimited(PKM pkm, int gameVersion)
        {
            bool isHatch = false;
            // 是否为版本限定宝可梦
            bool isLimitedPkm = IsLimitedPkm(pkm.Version, pkm.Species, gameVersion);
            // 判断是哪个区间的版本
            bool temp = ArrayUtils.IsEleInArray(Instance.GetHatchZeroArr(), CurrentPkmVer);
            if (temp)
                // 前四代需要查看宝可梦相遇等级是否为0，如果等级为0说明可以直接修改初训家
                isHatch = pkm.Met_Level == 0 ? false : true;
            else
                isHatch = pkm.Met_Level == 1 ? false : true;
            return isLimitedPkm && isHatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="species"></param>
        /// <param name="gameVersion"></param>
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
