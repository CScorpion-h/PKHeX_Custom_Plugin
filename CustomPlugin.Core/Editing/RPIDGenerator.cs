using PKHeX.Core;
using System;

namespace CustomPlugin.Core.Editing
{
    internal static class RPIDGenerator
    {
        private static string[] flags = { "UnShiny", "StarShiny", "SquareShiny" };

        public static PKM SetRandomPID(this PKM pkm, GameVersion gameVersion, string flag)
        {
            if (flag == flags[0])   //UnShiny
                return SetUnShinyPID(pkm, gameVersion);

            if (flag == flags[1])    // StarShiny
                return SetStarShinyPID(pkm, gameVersion);
            
            // SquareShiny
            return SetSquareShinyPID(pkm, gameVersion);
        }

        private static PKM SetUnShinyPID(PKM pkm, GameVersion gameVersion)
        {
            PKM tmp = pkm.Clone();
            switch (tmp.Format)
            {
                case 1:
                case 2:
                    tmp.Gen12UnShiny();
                    return tmp;
                case 3:
                case 4:
                case 5:
                    tmp.Gen345UnShiny();
                    return tmp;
                default:    // Gen6-Gen8
                    tmp.Gen678UnShiny();
                    return tmp;
            }
        }

        private static PKM SetStarShinyPID(PKM pkm, GameVersion gameVersion)
        {
            PKM tmp = pkm.Clone();
            switch (tmp.Format)
            {
                case 1:
                case 2:
                    tmp.Gen12Shiny();
                    return tmp;
                case 3:
                case 4:
                case 5:
                    tmp.Gen345Shiny(Shiny.AlwaysStar);
                    return tmp;
                default:    // Gen6-Gen8
                    tmp.Gen678Shiny(Shiny.AlwaysStar);
                    return tmp;
            }
        }

        private static PKM SetSquareShinyPID(PKM pkm, GameVersion gameVersion)
        {
            PKM tmp = pkm.Clone();
            switch (tmp.Format)
            {
                case 1:
                case 2:
                    tmp.Gen12Shiny();
                    return tmp;
                case 3:
                case 4:
                case 5:
                    tmp.Gen345Shiny(Shiny.AlwaysSquare);
                    return tmp;
                default:    // Gen6-Gen8
                    tmp.Gen678Shiny(Shiny.AlwaysSquare);
                    return tmp;
            }
        }
    }
}
