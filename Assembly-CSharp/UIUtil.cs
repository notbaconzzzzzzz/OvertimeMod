using System;
using Assets.Scripts.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AB2 RID: 2738
public class UIUtil
{
	// Token: 0x06005214 RID: 21012 RVA: 0x00003E08 File Offset: 0x00002008
	public UIUtil()
	{
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x001DAB20 File Offset: 0x001D8D20
	public static void SetUIAlpha(Image image, float value)
	{
		Color color = image.color;
		color.a = value;
		image.color = color;
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x001DAB20 File Offset: 0x001D8D20
	public static void SetUIAlpha(Text text, float value)
	{
		Color color = text.color;
		color.a = value;
		text.color = color;
	}

	// Token: 0x06005217 RID: 21015 RVA: 0x001DAB44 File Offset: 0x001D8D44
	public static void DefenseSetFull(DefenseInfo defenseInfo, Text[] text, string divider = "")
	{
		text[0].text = string.Format("{0}{2}({1:f1})", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.R)), defenseInfo.R, divider);
		text[1].text = string.Format("{0}{2}({1:f1})", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.W)), defenseInfo.W, divider);
		text[2].text = string.Format("{0}{2}({1:f1})", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.B)), defenseInfo.B, divider);
		text[3].text = string.Format("{0}{2}({1:f1})", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.P)), defenseInfo.P, divider);
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x001DAC10 File Offset: 0x001D8E10
	public static void DefenseSetOnlyText(DefenseInfo defenseInfo, Text[] text)
	{
		text[0].text = string.Format("{0}", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.R)));
		text[1].text = string.Format("{0}", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.W)));
		text[2].text = string.Format("{0}", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.B)));
		text[3].text = string.Format("{0}", UIUtil.GetDefenseText(defenseInfo.GetDefenseType(defenseInfo.P)));
	}

	// Token: 0x06005219 RID: 21017 RVA: 0x001DACAC File Offset: 0x001D8EAC
	public static void DefenseSetFactor(DefenseInfo defenseInfo, Text[] text, bool bracket = false)
	{
		string format = (!bracket) ? "{0:0.0}" : "({0:0.0})";
		text[0].text = string.Format(format, defenseInfo.R);
		text[1].text = string.Format(format, defenseInfo.W);
		text[2].text = string.Format(format, defenseInfo.B);
		text[3].text = string.Format(format, defenseInfo.P);
	}

	// Token: 0x0600521A RID: 21018 RVA: 0x001DAD34 File Offset: 0x001D8F34
	public static string GetDefenseText(DefenseInfo.Type type)
	{
		string empty = string.Empty;
		return EnumTextConverter.GetDefenseType(type);
	}

	// Token: 0x0600521B RID: 21019 RVA: 0x001DAD50 File Offset: 0x001D8F50
	public static void SetDefenseTypeIcon(DefenseInfo defenseInfo, Image[] renderer)
	{
		renderer[0].sprite = IconManager.instance.GetDefenseIcon(defenseInfo.GetDefenseType(defenseInfo.R));
		renderer[1].sprite = IconManager.instance.GetDefenseIcon(defenseInfo.GetDefenseType(defenseInfo.W));
		renderer[2].sprite = IconManager.instance.GetDefenseIcon(defenseInfo.GetDefenseType(defenseInfo.B));
		renderer[3].sprite = IconManager.instance.GetDefenseIcon(defenseInfo.GetDefenseType(defenseInfo.P));
	}

	// Token: 0x04004BC8 RID: 19400
	public const float quater = 0.25f;

	// Token: 0x04004BC9 RID: 19401
	public const float half = 0.5f;

	// Token: 0x04004BCA RID: 19402
	public const float quaterHalf = 0.75f;

	// Token: 0x04004BCB RID: 19403
	public const float full = 1f;
}
