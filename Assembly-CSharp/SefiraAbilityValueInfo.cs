using System;

// Token: 0x020006ED RID: 1773
public class SefiraAbilityValueInfo
{
    // Token: 0x06003936 RID: 14646 RVA: 0x0016F5BC File Offset: 0x0016D7BC
    public static int[] GetOfficerAliveValues(SefiraEnum sefira)
    {
        switch (sefira)
        {
            case SefiraEnum.MALKUT:
                return SefiraAbilityValueInfo.malkuthOfficerAliveValues;
            case SefiraEnum.YESOD:
                return SefiraAbilityValueInfo.yesodOfficerAliveValues;
            case SefiraEnum.HOD:
                return SefiraAbilityValueInfo.hodOfficerAliveValues;
            case SefiraEnum.NETZACH:
                return SefiraAbilityValueInfo.netzachOfficerAliveValues;
            case SefiraEnum.TIPERERTH1:
            case SefiraEnum.TIPERERTH2:
                return SefiraAbilityValueInfo.tipherethOfficerAliveValues;
            case SefiraEnum.GEBURAH:
                return SefiraAbilityValueInfo.geburahOfficerAliveValues;
            case SefiraEnum.CHESED:
                return SefiraAbilityValueInfo.chesedOfficerAliveValues;
            case SefiraEnum.BINAH:
                return SefiraAbilityValueInfo.binahOfficerAliveValues;
            case SefiraEnum.CHOKHMAH:
                return SefiraAbilityValueInfo.hokmaOfficerAliveValues;
            default:
                return SefiraAbilityValueInfo.malkuthOfficerAliveValues;
        }
    }

    // Token: 0x06003937 RID: 14647 RVA: 0x0016F638 File Offset: 0x0016D838
    public static int[] GetContinuousServiceValues(SefiraEnum sefira)
    {
        switch (sefira)
        {
            case SefiraEnum.MALKUT:
                return SefiraAbilityValueInfo.malkuthContinuousServiceValues;
            case SefiraEnum.YESOD:
                return SefiraAbilityValueInfo.yesodContinuousServiceValues;
            case SefiraEnum.HOD:
                return SefiraAbilityValueInfo.hodContinuousServiceValues;
            case SefiraEnum.NETZACH:
                return SefiraAbilityValueInfo.netzachContinuousServiceValues;
            case SefiraEnum.TIPERERTH1:
            case SefiraEnum.TIPERERTH2:
                return SefiraAbilityValueInfo.tipherethContinuousServiceValues;
            case SefiraEnum.GEBURAH:
                return SefiraAbilityValueInfo.geburahContinuousServiceValues;
            case SefiraEnum.CHESED:
                return SefiraAbilityValueInfo.chesedContinuousServiceValues;
            case SefiraEnum.BINAH:
                return SefiraAbilityValueInfo.binahContinuousServiceValues;
            case SefiraEnum.CHOKHMAH:
                return SefiraAbilityValueInfo.hokmaContinuousServiceValues;
            case SefiraEnum.KETHER:
                return SefiraAbilityValueInfo.ketherContinuousServiceValues;
            default:
                return SefiraAbilityValueInfo.malkuthContinuousServiceValues;
        }
    }

    // Token: 0x040033FD RID: 13309
    public static readonly int[] malkuthOfficerAliveValues = new int[]
    {
        0,
        1,
        3,
        5
    };

    // Token: 0x040033FE RID: 13310
    public static readonly int[] yesodOfficerAliveValues = new int[]
    {
        0,
        1,
        3,
        5
    };

    // Token: 0x040033FF RID: 13311
    public static readonly int[] netzachOfficerAliveValues = new int[]
    {
        0,
        10,
        30,
        50
    };

    // Token: 0x04003400 RID: 13312
    public static readonly int[] hodOfficerAliveValues = new int[]
    {
        0,
        1,
        3,
        5
    };

    // Token: 0x04003401 RID: 13313
    public static readonly int[] tipherethOfficerAliveValues = new int[]
    {
        0,
        3,
        6,
        10
    };

    // Token: 0x04003402 RID: 13314
    public static readonly int[] chesedOfficerAliveValues = new int[]
    {
        0,
        1,
        2,
        3
    };

    // Token: 0x04003403 RID: 13315
    public static readonly int[] geburahOfficerAliveValues = new int[]
    {
        0,
        1,
        3,
        5
    };

    // Token: 0x04003404 RID: 13316
    public static readonly int[] binahOfficerAliveValues = new int[]
    {
        0,
        4,
        8,
        12
    };

    // Token: 0x04003405 RID: 13317
    public static readonly int[] hokmaOfficerAliveValues = new int[]
    {
        0,
        1,
        2,
        3
    };

    // Token: 0x04003406 RID: 13318
    public static readonly int[] malkuthContinuousServiceValues = new int[]
    {
        3,
        5,
        7,
        10
    };

    // Token: 0x04003407 RID: 13319
    public static readonly int[] yesodContinuousServiceValues = new int[]
    {
        3,
        5,
        7,
        10
    };

    // Token: 0x04003408 RID: 13320
    public static readonly int[] netzachContinuousServiceValues = new int[]
    {
        3,
        5,
        7,
        10
    };

    // Token: 0x04003409 RID: 13321
    public static readonly int[] hodContinuousServiceValues = new int[]
    {
        5,
        10,
        15,
        4
    };

    // Token: 0x0400340A RID: 13322
    public static readonly int[] tipherethContinuousServiceValues = new int[]
    {
        1,
        2,
        3,
        5
    };

    // Token: 0x0400340B RID: 13323
    public static readonly int[] chesedContinuousServiceValues = new int[]
    {
        3,
        6,
        10,
        20
    };

    // Token: 0x0400340C RID: 13324
    public static readonly int[] geburahContinuousServiceValues = new int[]
    {
        3,
        6,
        10,
        20
    };

    // Token: 0x0400340D RID: 13325
    public static readonly int[] binahContinuousServiceValues = new int[]
    {
        3,
        6,
        10,
        20
    };

    // Token: 0x0400340E RID: 13326
    public static readonly int[] hokmaContinuousServiceValues = new int[]
    {
        2,
        3,
        4,
        6
    };

    // Token: 0x0400340F RID: 13327
    public static readonly int[] ketherContinuousServiceValues = new int[]
    {
        3,
        4,
        5,
        7
    };
}
