/*
public float GetProb() // 
+public int GiftMultiplierCost() // 
+public int GiftMultiplierBracket() // 
+public int GiftRiskLevel() // 
+public CreatureTypeInfo creature; // 
+many public static arrays // 
*/
using System;
using System.Linq; //
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
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		return Mathf.Max(0, this.cost - (int)((float)(this.cost * num) / 100f));
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x0016FAC4 File Offset: 0x0016DCC4
	public float GetProb()
	{ // <Mod>
		float p = this.prob;
		p += EGOrealizationManager.instance.GiftUpgrade(this);
        if (SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") ? 
			ResearchDataModel.instance.IsUpgradedAbility("spend_pe_for_gift") : 
			SpecialModeConfig.instance.GetValue<bool>("SpendPEForEgoGiftChance"))
        {
		    p *= (float)GiftMultiplier[GiftMultiplierBracket()];
        }
		if (ResearchDataModel.instance.IsUpgradedAbility("add_efo_gift_prob"))
		{
			p *= 2f;
		}
		return p;
	}

	// <Mod>
	public int GiftMultiplierCost()
	{
		int bracket = GiftMultiplierBracket();
		if (bracket <= 0) return 0;
		int cost = GiftPECost[GiftRiskLevel - 1];
		float probMult = (float)GiftMultiplier[bracket];
		cost = (int)((float)cost * (probMult - 1f) / probMult);
		return cost;
	}

	// <Mod>
	public int GiftMultiplierBracket()
	{
		CreatureObserveInfoModel observeInfo = CreatureManager.instance.GetObserveInfo_Mod(creature.LcId);
		if (observeInfo == null) return 0;
        int PE = observeInfo.cubeNum;
		if (PE < GiftThreshTwice[GiftRiskLevel - 1])
		{
			return 0;
		}
		if (PE < GiftThreshThrice[GiftRiskLevel - 1])
		{
			return 1;
		}
		if (PE < GiftThreshQuince[GiftRiskLevel - 1])
		{
			return 2;
		}
		return 3;
	}

	// <Mod>
	public int GiftRiskLevel
	{
		get
		{
			if (_giftRiskLevel != 0)
			{
				return _giftRiskLevel;
			}
			int risk = 1;
			if (EquipmentTypeInfo.BossIds.Contains(equipTypeInfo.LcId)) {
				risk = 6;
			} else {
				if (creature != null) {
					if (creature.LcId == 100064L) {
						risk = 5;
					} else {
						risk = (int)creature.GetRiskLevel() + 1;
					}
				}
			}
			_giftRiskLevel = risk;
			return risk;
		}
	}

	private int _giftRiskLevel = 0;

	// Token: 0x04003430 RID: 13360
	public EquipmentTypeInfo equipTypeInfo;

	// Token: 0x04003431 RID: 13361
	public int level;

	// Token: 0x04003432 RID: 13362
	public int cost;

	// Token: 0x04003433 RID: 13363
	public float prob;

	//> <Mod>
	public CreatureTypeInfo creature;

	public static int[] GiftMultiplier = new int[]
	{
		1,
		2,
		3,
		5
	};
	public static int[] GiftPECost = new int[]
	{
		80,
		120,
		180,
		270,
		666,
		1333
	};
	public static int[] GiftThreshTwice = new int[]
	{
		100,
		200,
		350,
		600,
		1000,
		3333
	};
	public static int[] GiftThreshThrice = new int[]
	{
		140,
		300,
		525,
		1000,
		2000,
		6666
	};
	public static int[] GiftThreshQuince = new int[]
	{
		200,
		450,
		900,
		1800,
		4000,
		9999
	};
	//< <Mod>
}
