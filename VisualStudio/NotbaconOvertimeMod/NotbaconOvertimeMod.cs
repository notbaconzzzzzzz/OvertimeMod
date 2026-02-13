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
            EquipmentTypeInfo.BossLongIds.AddRangeToArray(BossLongIds);
            EquipmentTypeInfo.BossIds.AddRangeToArray(BossIds);
            UnitEGOgiftSpace.exclusiveGifts.AddRangeToArray(exclusiveGifts);
            OvertimeModCommands commands = new OvertimeModCommands();
            commands.SetList();
            ConsoleCommand.instance.moddedCommands.Add(commands);
        }

        public static LcIdLong[] BossLongIds = new LcIdLong[] {
            new LcIdLong("NotbaconOvertimeMod", 130913L),
            new LcIdLong("NotbaconOvertimeMod", 130922L),
            new LcIdLong("NotbaconOvertimeMod", 130926L),
            new LcIdLong("NotbaconOvertimeMod", 130927L),
            new LcIdLong("NotbaconOvertimeMod", 130928L),
            new LcIdLong("NotbaconOvertimeMod", 130929L),
            new LcIdLong("NotbaconOvertimeMod", 130941L),
        };

        public static LcId[] BossIds = new LcId[] {
            new LcId("NotbaconOvertimeMod", 2309131),
            new LcId("NotbaconOvertimeMod", 3309131),
            new LcId("NotbaconOvertimeMod", 4309131),
            new LcId("NotbaconOvertimeMod", 4309132),
            new LcId("NotbaconOvertimeMod", 4309133),
            new LcId("NotbaconOvertimeMod", 4309134),
            new LcId("NotbaconOvertimeMod", 4309135),
            new LcId("NotbaconOvertimeMod", 4309136),
            new LcId("NotbaconOvertimeMod", 4309137),
            new LcId("NotbaconOvertimeMod", 2309221),
            new LcId("NotbaconOvertimeMod", 3309221),
            new LcId("NotbaconOvertimeMod", 4309221),
            new LcId("NotbaconOvertimeMod", 230926),
            new LcId("NotbaconOvertimeMod", 330926),
            new LcId("NotbaconOvertimeMod", 430926),
            new LcId("NotbaconOvertimeMod", 230927),
            new LcId("NotbaconOvertimeMod", 330927),
            new LcId("NotbaconOvertimeMod", 430927),
            new LcId("NotbaconOvertimeMod", 230928),
            new LcId("NotbaconOvertimeMod", 330928),
            new LcId("NotbaconOvertimeMod", 430928),
            new LcId("NotbaconOvertimeMod", 230929),
            new LcId("NotbaconOvertimeMod", 330929),
            new LcId("NotbaconOvertimeMod", 430929),
            new LcId("NotbaconOvertimeMod", 230941),
            new LcId("NotbaconOvertimeMod", 330941),
            new LcId("NotbaconOvertimeMod", 4309411),
            new LcId("NotbaconOvertimeMod", 4309412),
            new LcId("NotbaconOvertimeMod", 4309413),
            new LcId("NotbaconOvertimeMod", 4309414),
        };

        public static LcId[][] exclusiveGifts = new LcId[][]
        {
            new LcId[]
            { // Garlands
                new LcId("NotbaconOvertimeMod", 4309131),
                new LcId("NotbaconOvertimeMod", 4309132),
                new LcId("NotbaconOvertimeMod", 4309133),
                new LcId("NotbaconOvertimeMod", 4309134),
                new LcId("NotbaconOvertimeMod", 4309135),
                new LcId("NotbaconOvertimeMod", 4309136),
            },
            new LcId[]
            { // Vestiges
                new LcId("NotbaconOvertimeMod", 430926),
                new LcId("NotbaconOvertimeMod", 430927),
                new LcId("NotbaconOvertimeMod", 430928),
                new LcId("NotbaconOvertimeMod", 430929),
            },
            new LcId[]
            { // Da Capo Aria
                new LcId(400019),
                new LcId("NotbaconOvertimeMod", 4309411),
            },
            new LcId[]
            { // Moonlight Sonata
                new LcId(400065),
                new LcId("NotbaconOvertimeMod", 4309412),
            },
            new LcId[]
            { // Gone, but not Forgotten
                new LcId("NotbaconOvertimeMod", 430963),
                new LcId("NotbaconOvertimeMod", 4309413),
            },
            new LcId[]
            { // Caged Bird
                new LcId("NotbaconOvertimeMod", 430938),
                new LcId("NotbaconOvertimeMod", 4309414),
            },
            new LcId[]
            { // Blizzard
                new LcId("NotbaconOvertimeMod", 4309531),
                new LcId("NotbaconOvertimeMod", 4309532),
            },
        };
    }
}
