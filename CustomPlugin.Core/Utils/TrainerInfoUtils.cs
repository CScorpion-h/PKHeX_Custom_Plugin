using CustomPlugin.Core.Structure;
using PKHeX.Core;

namespace CustomPlugin.Core.Utils
{
    public static class TrainerInfoUtils
    {
        private static TrainerInfoInSav? trainerInfo;

        public static TrainerInfoInSav GetTrainerInfo(SaveFile sav)
        {
            return trainerInfo ?? new TrainerInfoInSav(sav);
        }
    }
}
