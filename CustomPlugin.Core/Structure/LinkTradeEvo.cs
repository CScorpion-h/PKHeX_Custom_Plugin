using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomPlugin.Core.Structure
{
    class LinkTradeEvo
    {
        public enum LinkTradeEvoPkm
        {
            胡地 = 65,
            怪力 = 68,
            隆隆岩 = 76,
            耿鬼 = 94,
            蚊香蛙皇 = 186,
            呆呆王 = 199,
            大钢蛇 = 208,
            巨钳螳螂 = 212,
            刺龙王 = 230,
            多边兽2 = 233,
            美纳斯 = 350,
            猎斑鱼 = 367,
            樱花鱼 = 368,
            多边兽z = 474,
            超甲狂犀 = 464,
            电击魔兽 = 466,
            鸭嘴炎兽 = 467,
            黑夜魔灵 = 477,
            庞岩怪 = 526,
            修建老匠 = 534,
            骑士蜗牛 = 589,
            敏捷冲 = 617,
            芳香精 = 683,
            胖甜妮 = 685,
            朽木妖 = 709,
            南瓜怪人 = 711
        }

        public static List<int> GetLinkTradeEvoPkmList => Enum.GetValues(typeof(LinkTradeEvoPkm)).Cast<int>().ToList();
    }
}
