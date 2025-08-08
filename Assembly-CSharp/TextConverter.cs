using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000BD4 RID: 3028
public class TextConverter
{
	// Token: 0x06005BA1 RID: 23457 RVA: 0x00004378 File Offset: 0x00002578
	public TextConverter()
	{
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x00209EB0 File Offset: 0x002080B0
	private static string SelectRandomWord(string random_list)
	{
		List<string> list = new List<string>();
		Match match = Regex.Match(random_list.Substring(1, random_list.Length - 2), "{[^}]*}");
		while (match.Success)
		{
			list.Add(match.Value);
			match = match.NextMatch();
		}
		if (list.Count == 0)
		{
			return string.Empty;
		}
		string text = list[UnityEngine.Random.Range(0, list.Count)];
		return text.Substring(1, text.Length - 2);
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x00209F34 File Offset: 0x00208134
	public static string GetTextFromFormatText(string format_text, params string[] param)
	{
		string text = format_text;
		Match match = Regex.Match(format_text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			text = text.Replace(match.Value, TextConverter.SelectRandomWord(match.Value));
			match = match.NextMatch();
		}
		for (int i = 0; i < param.Length; i++)
		{
			text = text.Replace("#" + i, param[i]);
		}
		return text;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x00209FB0 File Offset: 0x002081B0
	public static string GetTextFromFormatText(string format_text, CreatureModel model, params string[] param)
	{
		string text = format_text;
		Match match = Regex.Match(format_text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			text = text.Replace(match.Value, TextConverter.SelectRandomWord(match.Value));
			match = match.NextMatch();
		}
		for (int i = 0; i < param.Length; i++)
		{
			text = text.Replace("#" + i, param[i]);
		}
		return text.Replace("$0", model.metaInfo.collectionName);
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x0020A040 File Offset: 0x00208240
	private static string[] SelectProcessWord(string random_list)
	{
		List<string> list = new List<string>();
		Match match = Regex.Match(random_list.Substring(1, random_list.Length - 2), "{[^}]*}");
		while (match.Success)
		{
			list.Add(match.Value.Substring(1, match.Value.Length - 2));
			match = match.NextMatch();
		}
		return list.ToArray();
	}

	// Token: 0x06005BA6 RID: 23462 RVA: 0x0020A0AC File Offset: 0x002082AC
	public static string[] GetTextFromFormatProcessText(string format_text, params string[] param)
	{
		string[] array = new string[0];
		Match match = Regex.Match(format_text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			string[] array2 = TextConverter.SelectProcessWord(match.Value);
			array = new string[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = format_text.Replace(match.Value, array2[i]);
				for (int j = 0; j < param.Length; j++)
				{
					array[i] = array[i].Replace("#" + j, param[j]);
				}
			}
			match = match.NextMatch();
		}
		return array;
	}

	// Token: 0x06005BA7 RID: 23463 RVA: 0x0020A158 File Offset: 0x00208358
	public static string[] GetTextFromFormatProcessText(string format_text, CreatureModel model, params string[] param)
	{
		string[] array = new string[0];
		Match match = Regex.Match(format_text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			string[] array2 = TextConverter.SelectProcessWord(match.Value);
			array = new string[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = format_text.Replace(match.Value, array2[i]);
				for (int j = 0; j < param.Length; j++)
				{
					array[i] = array[i].Replace("#" + j, param[j]);
				}
				array[i] = array[i].Replace("$0", model.metaInfo.collectionName);
			}
			match = match.NextMatch();
		}
		return array;
	}

	// Token: 0x06005BA8 RID: 23464 RVA: 0x0020A220 File Offset: 0x00208420
	public static string[] GetTextFromFormatProcessText(string format_text)
	{
		string[] array = new string[0];
		Match match = Regex.Match(format_text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			string[] array2 = TextConverter.SelectProcessWord(match.Value);
			array = new string[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = format_text.Replace(match.Value, array2[i]);
			}
			match = match.NextMatch();
		}
		return array;
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x0020A294 File Offset: 0x00208494
	public static string TranslateDescData(string descData)
	{
		string text = string.Empty;
		Match match = Regex.Match(descData, "\"[^\"]*\"");
		while (match.Success)
		{
			text = text + match.Value.Substring(1, match.Value.Length - 2) + "\n";
			match = match.NextMatch();
		}
		return text;
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x0020A2F0 File Offset: 0x002084F0
	public static string GetTextFromFormatAlter(string text)
	{
		string text2 = string.Empty;
		Match match = Regex.Match(text, "\\[[^\\]]*\\]");
		while (match.Success)
		{
			text2 += match.Value.Substring(1, match.Value.Length - 2);
			match = match.NextMatch();
		}
		text2 = text2.Trim();
		int num = text2.IndexOf('}');
		if (num != -1)
		{
			text2 = text2.Remove(num, 1);
		}
		int num2 = text2.IndexOf('{');
		if (num2 != -1)
		{
			text2 = text2.Remove(num2, 1);
		}
		return text2;
	}
}
