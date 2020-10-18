using CustomPlugin.Core.Editing;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace CustomPluginTest
{
    public class PIDTests
    {
        private static readonly string PKMFolder = TestUtil.GetTestFolder("Sample");

        [Fact]
        public static void CheckGen3PID()
        {
            var pk3 = new PK3(File.ReadAllBytes(PKMFolder + @"\003 - VENUSAUR - 156F1CC41245.pk3"));
            Assert.NotNull(pk3);
            PIDEdits.Gen345UnShiny(pk3);

            Assert.True(new LegalityAnalysis(pk3).Valid, "Set Gen3 Pokemon UnShiny Successfully");
        }

    }
}
