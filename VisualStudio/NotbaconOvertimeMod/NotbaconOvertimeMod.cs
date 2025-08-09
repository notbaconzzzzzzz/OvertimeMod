using Harmony;
using LobotomyBaseMod;
using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconOvertimeMod : ModInitializer
    {
        public override void OnInitialize()
        {
            EquipmentTypeInfo.BossIds.AddRangeToArray(BossIds);
            UnitEGOgiftSpace.exclusiveGifts.AddRangeToArray(exclusiveGifts);
        }

        public static LcId[] BossIds = new LcId[] {
            new LcId("NotbaconOvertimeMod", 4309131),
            new LcId("NotbaconOvertimeMod", 4309132),
            new LcId("NotbaconOvertimeMod", 4309133),
            new LcId("NotbaconOvertimeMod", 4309134),
            new LcId("NotbaconOvertimeMod", 4309135),
            new LcId("NotbaconOvertimeMod", 4309136),
            new LcId("NotbaconOvertimeMod", 4309137),
        };

        public static LcId[][] exclusiveGifts = new LcId[][]
        {
            new LcId[]
            {
                new LcId("NotbaconOvertimeMod", 4309131),
                new LcId("NotbaconOvertimeMod", 4309132),
                new LcId("NotbaconOvertimeMod", 4309133),
                new LcId("NotbaconOvertimeMod", 4309134),
                new LcId("NotbaconOvertimeMod", 4309135),
                new LcId("NotbaconOvertimeMod", 4309136),
            }
        };
    }
}
