using PKHeX.Core;
using System.Collections.Generic;

namespace CustomPlugin.Core.Editing
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonInfoEdits
    {

        public static void ClearAllRibbon(this PKM pkm)
        {
            IEnumerable<RibbonInfo> riblist = RibbonInfo.GetRibbonInfo(pkm);
            foreach (var rib in riblist)
                ReflectUtil.SetValue(pkm, rib.Name, rib.RibbonCount < 0 ? false : (object)0);
        }

        public static void SetIVs(this PKM pkm)
        {
            if (pkm.IVTotal == 0)
                pkm.SetMaximumIVs();
        }

        internal static void SetMaximumIVs(this PKM pkm)
        {
            int maxIV = pkm.GetMaximumIV(0);
            int[] ivs = pkm.IVs;
            for (int i = 0; i < ivs.Length; i++)
                ivs[i] = maxIV;
        }

        public static void SetMovesPPUpsToZero(this PKM pkm)
        {
            pkm.Move1_PPUps = 0;
            pkm.Move2_PPUps = 0;
            pkm.Move3_PPUps = 0;
            pkm.Move4_PPUps = 0;
        }

        public static void ClearCurrentHandler(this PKM pkm)
        {
            pkm.HT_Name = $"";
            pkm.HT_Gender = 0;
            pkm.HT_Friendship = 0;
            pkm.CurrentHandler = 0;

            if (pkm.Format == 8)
            {
                PK8 pk8 = (PK8)pkm;
                pk8.HT_Language = 0;
            }
        }
    }
}
