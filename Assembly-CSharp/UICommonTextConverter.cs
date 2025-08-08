/*
public static string GetPercentText(float rate) // return number instead
+public static string GetInfoPercentText(float rate) // return number without decimal points
*/
using System;
using UnityEngine; // 

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

    public static string GetPercentText(float rate)
    { // <Mod> returns a number instead
        int num = Mathf.RoundToInt(rate * 1000f);
		string text = "";
		switch (SpecialModeConfig.instance.GetValue<SuccessRateDisplayMode>("RealTimeSuccessRate"))
		{
			case SuccessRateDisplayMode.PERCENTAGE:
				text = string.Format("{0}.{1}%", num / 10, num % 10);
				break;
			case SuccessRateDisplayMode.PERCENTAGE_AND_TEXT:
				text = string.Format("{0}.{1}% ({2})", num / 10, num % 10, GetPercentText((int)(rate / 0.2f)));
				break;
			case SuccessRateDisplayMode.TEXT_ONLY:
				text = GetPercentText((int)(rate / 0.2f));
				break;
		}
        return text;
    }

    // <Mod> returns a number instead
    public static string GetInfoPercentText(float rate)
    {
        int num = Mathf.RoundToInt(rate * 100f);
		string text = "";
		switch (SpecialModeConfig.instance.GetValue<SuccessRateDisplayMode>("InfoWindowSuccessRate"))
		{
			case SuccessRateDisplayMode.PERCENTAGE:
				text = string.Format("{0}%", num);
				break;
			case SuccessRateDisplayMode.PERCENTAGE_AND_TEXT:
				text = string.Format("{0}% ({1})", num, GetPercentText((int)(rate / 0.2f)));
				break;
			case SuccessRateDisplayMode.TEXT_ONLY:
				text = GetPercentText((int)(rate / 0.2f));
				break;
		}
        return text;
    }
}
