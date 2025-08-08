using System;
using UnityEngine;

// Token: 0x020006FA RID: 1786
public class CreatureEquipmentMakeInfo
{
	// Token: 0x06003958 RID: 14680 RVA: 0x00003E08 File Offset: 0x00002008
	public CreatureEquipmentMakeInfo()
	{
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x0016FA84 File Offset: 0x0016DC84
	public int GetCostAfterUpgrade()
	{
		int num = 0;
		num += SefiraAbilityValueInfo.binahOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.BINAH)];
		return Mathf.Max(0, this.cost - (int)((float)(this.cost * num) / 100f));
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x0016FAC4 File Offset: 0x0016DCC4
	public float GetProb()
	{
		if (ResearchDataModel.instance.IsUpgradedAbility("add_efo_gift_prob"))
		{
			return this.prob + this.prob;
		}
		return this.prob;
	}

	// Token: 0x04003430 RID: 13360
	public EquipmentTypeInfo equipTypeInfo;

	// Token: 0x04003431 RID: 13361
	public int level;

	// Token: 0x04003432 RID: 13362
	public int cost;

	// Token: 0x04003433 RID: 13363
	public float prob;
}
