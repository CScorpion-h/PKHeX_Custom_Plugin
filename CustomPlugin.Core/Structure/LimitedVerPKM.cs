using CustomPlugin.Core.Consistent;
using CustomPlugin.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CustomPlugin.Core.Structure
{
    /// <summary>
    /// Various versions of capturable pokemon
    /// </summary>
    public class LimitedVerPKM
    {
        // FireRed
        private static List<int> FR { get; set; }
        // LeafGreen
        private static List<int> LG { get; set; }
        // Ruby
        private static List<int> R { get; set; }
        // Sapphire
        private static List<int> S { get; set; }
        // Emerald
        private static List<int> E { get; set; }
        // Pokémon Colosseum &amp; Pokémon XD
        //private static readonly List<int> CXD { get; set; }

        // Diamond
        private static List<int> D { get; set; }
        // Pearl
        private static List<int> P { get; set; }
        // Platinum
        private static List<int> Pt { get; set; }
        // Heart Gold
        private static List<int> HG { get; set; }
        // Soul Silver
        private static List<int> SS { get; set; }

        // White
        private static List<int> W { get; set; }
        // Black
        private static List<int> B { get; set; }
        // White 2
        private static List<int> W2 { get; set; }
        // Black 2
        private static List<int> B2 { get; set; }

        // X
        private static List<int> X { get; set; }
        // Y
        private static List<int> Y { get; set; }
        // Omega Ruby
        private static List<int> OR { get; set; }
        // Alpha Sapphire
        private static List<int> AS { get; set; }

        // Sun
        private static List<int> SN { get; set; }
        // Moon
        private static List<int> MN { get; set; }
        // Ultra Sun
        private static List<int> US { get; set; }
        // Ultra Moon
        private static List<int> UM { get; set; }

        // Let's Go Pikachu
        private static List<int> GP { get; set; }
        // Let's Go Eevee
        private static List<int> GE { get; set; }
        // Sword
        private static List<int> SW { get; set; }
        // Shield
        private static List<int> SH { get; set; }

        /// <summary>
        /// Met_level is zero before Gen4
        /// </summary>
        internal static string[] HatchLevelZeroVer =
        {
            "FR", "LG",
            "S", "R", "E",
            "D", "P", "PT"
        };


        public LimitedVerPKM()
        {
        }

        public void InitLimitedVerPkm(string fileName, string type = "json")
        {
            string rawData = CommonUtil.GetFileContent(fileName, type);
            if (rawData == null)
                return;
            HandleJsonToData(rawData);
        }

        private void HandleJsonToData(string rawData)
        {
            
            RootPokedex rootPokedex = (RootPokedex)DeserializeObject(rawData);
            PokedexCapturable Pokemon = rootPokedex.Pokemon;
            string gameVersion = rootPokedex.GameVersion;

            List<int> list = AddPokemon(Pokemon);
            ReflectUtils.SetPropertyValue(this, gameVersion, list);
        }

        private object DeserializeObject(string rawData)
        {
            var serializer = new DataContractJsonSerializer(typeof(RootPokedex));
            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(rawData));
            return serializer.ReadObject(mStream);
        }

        private static List<int> AddPokemon(PokedexCapturable pokemon)
        {
            List<int> list = new List<int>();

            foreach (string species in pokemon.Gen1.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen2.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen3.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen4.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen5.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen6.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen7.CheckNull())
                list.Add(int.Parse(species));
            foreach (string species in pokemon.Gen8.CheckNull())
                list.Add(int.Parse(species));
            return list;
        }

        public string[] GetHatchZeroArr()
        {
            return HatchLevelZeroVer;
        }
    }
}
