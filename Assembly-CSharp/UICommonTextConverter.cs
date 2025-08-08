using System;

// Token: 0x02000ACB RID: 2763
public class UICommonTextConverter
{
    // Token: 0x0600530B RID: 21259 RVA: 0x00003DF4 File Offset: 0x00001FF4
    public UICommonTextConverter()
    {
    }

    // Token: 0x0600530C RID: 21260 RVA: 0x001DEED8 File Offset: 0x001DD0D8
    public static string GetPercentText(int level)
    {
        string text = "ui_";
        if (level < 1)
        {
            text += "Lowest";
        }
        else if (level < 2)
        {
            text += "Low";
        }
        else if (level < 3)
        {
            text += "Middle";
        }
        else if (level < 4)
        {
            text += "High";
        }
        else
        {
            text += "Highest";
        }
        return LocalizeTextDataModel.instance.GetText(text);
    }

    // Token: 0x0600530D RID: 21261 RVA: 0x00043082 File Offset: 0x00041282
    public static string GetPercentText(float rate)
    {
        return UICommonTextConverter.GetPercentText((int)(rate / 0.2f));
    }
}
