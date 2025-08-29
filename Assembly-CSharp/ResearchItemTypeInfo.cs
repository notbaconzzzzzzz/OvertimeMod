using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000723 RID: 1827
public class ResearchItemTypeInfo
{
	// Token: 0x06003A8E RID: 14990 RVA: 0x00034289 File Offset: 0x00032489
	public ResearchItemTypeInfo()
	{
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x000342A7 File Offset: 0x000324A7
	public static int CompareById(ResearchItemTypeInfo a, ResearchItemTypeInfo b)
	{
		if (a == null || b == null)
		{
			Debug.Log("Error in comparison");
			return 0;
		}
		return a.id.CompareTo(b.id);
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x0017CC28 File Offset: 0x0017AE28
	public ResearchItemDesc GetDesc()
	{
		string currentLanguage = GlobalGameManager.instance.GetCurrentLanguage();
		ResearchItemDesc result = null;
		if (this.desc.TryGetValue(currentLanguage, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0017CC58 File Offset: 0x0017AE58
	public Sprite GetIcon()
	{
		Sprite result = null;
		if (this.atlas == string.Empty)
		{
			result = Resources.Load<Sprite>("Sprites/UI/Icons/Research/" + this.icon);
		}
		else
		{
			Sprite[] array = Resources.LoadAll<Sprite>("Sprites/UI/Icons/Research/" + this.atlas);
			foreach (Sprite sprite in array)
			{
				if (sprite.name.Equals(this.icon))
				{
					result = sprite;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x04003599 RID: 13721
	private const string iconDefSrc = "Sprites/UI/Icons/Research/";

	// Token: 0x0400359A RID: 13722
	public int id;

	// Token: 0x0400359B RID: 13723
	public string icon;

	// Token: 0x0400359C RID: 13724
	public string atlas = string.Empty;

	// Token: 0x0400359D RID: 13725
	public string sephira;

	// Token: 0x0400359E RID: 13726
	public ResearchType type;

	// Token: 0x0400359F RID: 13727
	public int maxLevel;

	// Token: 0x040035A0 RID: 13728
	public int[] cost;

	// Token: 0x040035A1 RID: 13729
	public Dictionary<string, ResearchItemDesc> desc;

	// Token: 0x040035A2 RID: 13730
	public ResearchUpgradeInfo[] upgradeInfos;

	// Token: 0x040035A3 RID: 13731
	public List<int> prevResearch = new List<int>();

    // <Mod>
    public bool isOvertime;
}
