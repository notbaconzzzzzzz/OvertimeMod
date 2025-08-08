using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059D RID: 1437
public static class SupportedLanguage
{
	// Token: 0x0600313B RID: 12603 RVA: 0x001528F4 File Offset: 0x00150AF4
	public static List<string> GetSupprotedList()
	{
		return new List<string>
		{
			"en",
			"kr",
			"cn",
			"cn_tr",
			"jp",
			"ru",
			"vn",
			"bg",
			"es"
		};
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0015296C File Offset: 0x00150B6C
	public static string GetCurrentLanguageName(SystemLanguage language)
	{
		switch (language)
		{
		case SystemLanguage.Vietnamese:
			return "Tiếng Việt";
		case SystemLanguage.ChineseSimplified:
			break;
		case SystemLanguage.ChineseTraditional:
			return "中文(繁體)";
		default:
			if (language == SystemLanguage.Bulgarian)
			{
				return "български";
			}
			if (language != SystemLanguage.Chinese)
			{
				if (language == SystemLanguage.Japanese)
				{
					return "日本語";
				}
				if (language != SystemLanguage.Korean)
				{
					if (language != SystemLanguage.English)
					{
						if (language == SystemLanguage.Russian)
						{
							return "русский";
						}
						if (language == SystemLanguage.Spanish)
						{
							return "Español Latinoamérica";
						}
					}
					return "English";
				}
				return "한국어";
			}
			break;
		}
		return "中文(简体)";
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x001529E8 File Offset: 0x00150BE8
	public static string GetCurrentLanguageName(string language)
	{
		switch (language)
		{
			case "en":
				return "English";
			case "kr":
				return "한국어";
			case "cn":
				return "中文(简体)";
			case "cn_tr":
				return "中文(繁體)";
			case "jp":
				return "日本語";
			case "ru":
				return "русский";
			case "vn":
				return "Tiếng Việt";
			case "bg":
				return "български";
			case "es":
				return "Español Latinoamérica";
		}
		return "English";
	}

	// Token: 0x04002EC2 RID: 11970
	public const string kr = "kr";

	// Token: 0x04002EC3 RID: 11971
	public const string kr_Name = "한국어";

	// Token: 0x04002EC4 RID: 11972
	public const string cn = "cn";

	// Token: 0x04002EC5 RID: 11973
	public const string cn_Name = "中文(简体)";

	// Token: 0x04002EC6 RID: 11974
	public const string cn_tr = "cn_tr";

	// Token: 0x04002EC7 RID: 11975
	public const string cn_tr_Name = "中文(繁體)";

	// Token: 0x04002EC8 RID: 11976
	public const string en = "en";

	// Token: 0x04002EC9 RID: 11977
	public const string en_Name = "English";

	// Token: 0x04002ECA RID: 11978
	public const string jp = "jp";

	// Token: 0x04002ECB RID: 11979
	public const string jp_Name = "日本語";

	// Token: 0x04002ECC RID: 11980
	public const string ru = "ru";

	// Token: 0x04002ECD RID: 11981
	public const string ru_Name = "русский";

	// Token: 0x04002ECE RID: 11982
	public const string vn = "vn";

	// Token: 0x04002ECF RID: 11983
	public const string vn_Name = "Tiếng Việt";

	// Token: 0x04002ED0 RID: 11984
	public const string bg = "bg";

	// Token: 0x04002ED1 RID: 11985
	public const string bg_Name = "български";

	// Token: 0x04002ED2 RID: 11986
	public const string es = "es";

	// Token: 0x04002ED3 RID: 11987
	public const string es_Name = "Español Latinoamérica";
}
