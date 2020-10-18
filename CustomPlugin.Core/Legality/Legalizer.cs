using PKHeX.Core;

namespace CustomPlugin.Core.Legality
{
    static class Legalizer
    {
        public static void SetMetValid(PKM pkm)
        {
            var encounter = EncounterSuggestion.GetSuggestedMetInfo(pkm);
            if (encounter == null || pkm.Format >= 3 && encounter.Location < 0)
                return;

            int level = encounter.LevelMin;
            int location = encounter.Location;
            int minlvl = EncounterSuggestion.GetLowestLevel(pkm, encounter.LevelMin);
            if (minlvl == 0)
                minlvl = level;
            if (pkm.CurrentLevel >= minlvl && pkm.Met_Level == level && pkm.Met_Location == location)
                return;
            if (minlvl < level)
                minlvl = level;

            if (pkm.Format >= 3)
            {
                pkm.Met_Location = location;
                pkm.Met_Level = encounter.GetSuggestedMetLevel(pkm);
                // 相遇地点狩猎区
                int pkmLocation = pkm.Met_Location;
                if (pkmLocation == 52 || pkmLocation == 57 || pkmLocation == 136 || pkmLocation == 202)
                    pkm.Ball = 5;
                if (pkm.Ball == 5 && pkmLocation != 52 && pkmLocation != 57 && pkmLocation != 136 && pkmLocation != 202)
                    pkm.Ball = 1;

                if (pkm.Gen6 && pkm.WasEgg)
                    pkm.SetHatchMemory6();
            }

            if (pkm.CurrentLevel < minlvl)
                pkm.CurrentLevel = minlvl;
        }
    }
}
