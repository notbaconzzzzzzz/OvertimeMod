using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
public class ResearchItemModel
{
	// Token: 0x06003954 RID: 14676 RVA: 0x000043D4 File Offset: 0x000025D4
	public ResearchItemModel()
	{
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x0017776C File Offset: 0x0017596C
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"researchItemTypeId",
				this.info.id
			},
			{
				"curLevel",
				this.curLevel
			}
		};
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x001777B4 File Offset: 0x001759B4
	public void LoadData(Dictionary<string, object> dic)
	{
		int num = 0;
		GameUtil.TryGetValue<int>(dic, "researchItemTypeId", ref num);
		this.info = ResearchItemTypeList.instance.GetData(num);
		if (this.info == null)
		{
			Debug.Log("skill not found >> " + num);
		}
		GameUtil.TryGetValue<int>(dic, "curLevel", ref this.curLevel);
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x000336FD File Offset: 0x000318FD
	public ResearchUpgradeInfo GetCurrentUpgradeInfo()
	{ // <Mod>
        int index = curLevel - 1;
        if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch") && (!info.isOvertime || SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions")))
        {
            index = info.upgradeInfos.Length - curLevel - 1;
        }
		SefiraEnum sefEnum = SefiraEnum.DUMMY;
		if (SefiraManager.instance.GetSefira(info.sephira) != null)
		{
			sefEnum = SefiraManager.instance.GetSefira(info.sephira).sefiraEnum;
		}
        if (SefiraBossManager.Instance.NegateResearch(sefEnum))
        {
            index = -1;
        }
		if (index < 0 || index >= info.upgradeInfos.Length)
		{
			return null;
		}
		return info.upgradeInfos[index];
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x00033721 File Offset: 0x00031921
	public ResearchUpgradeInfo GetNextUpgradeInfo()
	{ // <Mod>
        int index = curLevel;
        /*
        if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
        {
            index = info.upgradeInfos.Length - curLevel - 2;
        }*/
		if (index < 0 || index >= info.upgradeInfos.Length)
		{
			return null;
		}
		return info.upgradeInfos[index];
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x0003374F File Offset: 0x0003194F
	public static int CompareById(ResearchItemModel a, ResearchItemModel b)
	{
		return ResearchItemTypeInfo.CompareById(a.info, b.info);
	}

	// Token: 0x0400342A RID: 13354
	public int curLevel;

	// Token: 0x0400342B RID: 13355
	public ResearchItemTypeInfo info;
}
