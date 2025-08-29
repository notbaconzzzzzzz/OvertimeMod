/*
public List<ResearchItemModel> GetRemainResearchListBySefira(string sefira) // Filter Overtime Research
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GlobalBullet;
using UnityEngine;

// Token: 0x020006DF RID: 1759
public class ResearchDataModel
{
	// Token: 0x060038D0 RID: 14544 RVA: 0x0016DE48 File Offset: 0x0016C048
	public ResearchDataModel()
	{
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00032EC8 File Offset: 0x000310C8
	public static ResearchDataModel instance
	{
		get
		{
			if (ResearchDataModel._instance == null)
			{
				ResearchDataModel._instance = new ResearchDataModel();
			}
			return ResearchDataModel._instance;
		}
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x0016DEA8 File Offset: 0x0016C0A8
	public void Init()
	{
		this.researchDatas = new Dictionary<int, ResearchItemModel>();
		foreach (ResearchItemTypeInfo researchItemTypeInfo in ResearchItemTypeList.instance.GetList())
		{
			ResearchItemModel researchItemModel = new ResearchItemModel();
			researchItemModel.curLevel = 0;
			researchItemModel.info = researchItemTypeInfo;
			this.researchDatas.Add(researchItemTypeInfo.id, researchItemModel);
		}
		this.agentStatBonus = new ResearchUnitStatUpgrade();
		this.promotionEasilyUpgrade = new ResearchPromotionEasily();
		this.weaponLevelResearchInfo = new Dictionary<int, int>();
		this.sephiraabilityLevel = new Dictionary<string, int>();
		this.specialAbility = new Dictionary<string, bool>();
		this.UpdateResearch();
		Notice.instance.Send(NoticeName.InitResearchItem, new object[0]);
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x0016DF84 File Offset: 0x0016C184
	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			list.Add(researchItemModel.GetSaveData());
		}
		dictionary.Add("research", list);
		return dictionary;
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x0016E004 File Offset: 0x0016C204
	public void LoadData(Dictionary<string, object> dic)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "research", ref list);
		this.Init();
		foreach (Dictionary<string, object> dic2 in list)
		{
			ResearchItemModel researchItemModel = new ResearchItemModel();
			researchItemModel.LoadData(dic2);
			if (researchItemModel.info != null)
			{
				this.researchDatas[researchItemModel.info.id] = researchItemModel;
			}
		}
		this.UpdateResearch();
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x0016E0A8 File Offset: 0x0016C2A8
	public ResearchItemModel GetResearchItem(int id)
	{
		ResearchItemModel result = null;
		this.researchDatas.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0016E0C8 File Offset: 0x0016C2C8
	public int GetResearchCost(int id)
	{
		ResearchItemModel researchItemModel = null;
		if (this.researchDatas.TryGetValue(id, out researchItemModel))
		{
			ResearchUpgradeInfo nextUpgradeInfo = researchItemModel.GetNextUpgradeInfo();
			if (nextUpgradeInfo != null)
			{
				return nextUpgradeInfo.cost;
			}
		}
		return 9999;
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x0016E104 File Offset: 0x0016C304
	public bool UpgradeResearch(int id, bool forcely = false)
	{
		ResearchItemModel researchItemModel = null;
		if (!this.researchDatas.TryGetValue(id, out researchItemModel))
		{
			return false;
		}
		if (researchItemModel.curLevel >= researchItemModel.info.maxLevel)
		{
			return false;
		}
		if (researchItemModel.GetNextUpgradeInfo() == null)
		{
			return false;
		}
		Sefira sefira = SefiraManager.instance.GetSefira(researchItemModel.info.sephira);
		if (sefira != null && !MissionManager.instance.ExistsClearedMission(sefira.sefiraEnum) && !forcely && GlobalGameManager.instance.gameMode != GameMode.TUTORIAL)
		{
			Debug.LogError("not enough research point");
			return false;
		}
		ResearchItemTypeInfo info = researchItemModel.info;
		if (info.prevResearch.Count != 0)
		{
			int num = info.prevResearch.Count;
			foreach (int key in info.prevResearch)
			{
				ResearchItemModel researchItemModel2 = null;
				if (this.researchDatas.TryGetValue(key, out researchItemModel2) && researchItemModel2.curLevel > 0)
				{
					num--;
				}
			}
			if (num > 0)
			{
				return false;
			}
		}
		researchItemModel.curLevel++;
		this.UpdateResearch();
		return true;
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x0016E25C File Offset: 0x0016C45C
	public void UpdateResearch()
	{
		ResearchUnitStatUpgrade researchUnitStatUpgrade = new ResearchUnitStatUpgrade();
		ResearchPromotionEasily researchPromotionEasily = new ResearchPromotionEasily();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		Dictionary<string, bool> dictionary3 = new Dictionary<string, bool>();
		Dictionary<GlobalBulletType, bool> dictionary4 = new Dictionary<GlobalBulletType, bool>();
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			ResearchUpgradeInfo currentUpgradeInfo = researchItemModel.GetCurrentUpgradeInfo();
			if (currentUpgradeInfo != null)
			{
				if (currentUpgradeInfo.agentStatBonus != null)
				{
					researchUnitStatUpgrade.hp += currentUpgradeInfo.agentStatBonus.hp;
					researchUnitStatUpgrade.mental += currentUpgradeInfo.agentStatBonus.mental;
					researchUnitStatUpgrade.mentalDefense += currentUpgradeInfo.agentStatBonus.mentalDefense;
					researchUnitStatUpgrade.movement += currentUpgradeInfo.agentStatBonus.movement;
					researchUnitStatUpgrade.physicalDefense += currentUpgradeInfo.agentStatBonus.physicalDefense;
					researchUnitStatUpgrade.regeneration += currentUpgradeInfo.agentStatBonus.regeneration;
					researchUnitStatUpgrade.regenerationDelay += currentUpgradeInfo.agentStatBonus.regenerationDelay;
					researchUnitStatUpgrade.regenerationMental += currentUpgradeInfo.agentStatBonus.regenerationMental;
					researchUnitStatUpgrade.regenerationMentalDelay += currentUpgradeInfo.agentStatBonus.regenerationMentalDelay;
					researchUnitStatUpgrade.workSpeed += currentUpgradeInfo.agentStatBonus.workSpeed;
					researchUnitStatUpgrade.workEnergy += currentUpgradeInfo.agentStatBonus.workEnergy;
					researchUnitStatUpgrade.workProb += currentUpgradeInfo.agentStatBonus.workProb;
				}
				if (currentUpgradeInfo.departAbility != null)
				{
					dictionary2[currentUpgradeInfo.departAbility.sephira] = currentUpgradeInfo.departAbility.level;
				}
				if (currentUpgradeInfo.specialAbility != null)
				{
					dictionary3[currentUpgradeInfo.specialAbility.name] = true;
				}
				if (currentUpgradeInfo.promotionEasily != null)
				{
					researchPromotionEasily.value = currentUpgradeInfo.promotionEasily.value;
				}
				if (currentUpgradeInfo.bulletAility != null)
				{
					dictionary4[currentUpgradeInfo.bulletAility.bulletType] = true;
				}
			}
		}
		this.agentStatBonus = researchUnitStatUpgrade;
		this.sephiraabilityLevel = dictionary2;
		this.promotionEasilyUpgrade = researchPromotionEasily;
		this.specialAbility = dictionary3;
		this.bulletAbility = dictionary4;
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x00032EE3 File Offset: 0x000310E3
	public ResearchUnitStatUpgrade GetAgentStatBonus()
	{
		return this.agentStatBonus;
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x0016E4E4 File Offset: 0x0016C6E4
	public int GetSephiraAbility(Sefira sephira)
	{
		int result;
		if (this.sephiraabilityLevel.TryGetValue(sephira.indexString, out result))
		{
			return result;
		}
		return 0;
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x00032EEB File Offset: 0x000310EB
	public bool IsUpgradedAbility(string name)
	{
		return this.specialAbility.ContainsKey(name);
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x00032F01 File Offset: 0x00031101
	public bool IsUpgradedBullet(GlobalBulletType type)
	{
		return this.bulletAbility.ContainsKey(type);
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x00032F17 File Offset: 0x00031117
	public float GetPromotionEasilyValue()
	{
		return this.promotionEasilyUpgrade.value;
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x0016E50C File Offset: 0x0016C70C
	public List<ResearchItemModel> GetModelBySefira(string sefira)
	{ // <Mod> Filter Overtime Research
		List<ResearchItemModel> list = new List<ResearchItemModel>();
		bool includeOvertime = SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions");
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			if (researchItemModel.info.sephira == sefira && (includeOvertime || !researchItemModel.info.isOvertime))
			{
				list.Add(researchItemModel);
			}
		}
		List<ResearchItemModel> list2 = list;
		if (ResearchDataModel.cache0 == null)
		{
			ResearchDataModel.cache0 = new Comparison<ResearchItemModel>(ResearchItemModel.CompareById);
		}
		list2.Sort(ResearchDataModel.cache0);
		return list;
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x0016E5B0 File Offset: 0x0016C7B0
	public List<ResearchItemModel> GetRemainResearchListBySefira(string sefira)
	{ // <Mod> Filter Overtime Research
		List<ResearchItemModel> list = new List<ResearchItemModel>();
		// int overtimeCheck = 0;
		// bool includeOvertime = SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") && MissionManager.instance.ExistsFinishedBossMission(SefiraManager.instance.GetSefira(sefira).sefiraEnum) && MissionManager.instance.GetClearedOrClosedMissionNum(SefiraManager.instance.GetSefira(sefira).sefiraEnum) > 5;
		bool includeOvertime = SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions");
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			if (researchItemModel.info.sephira == sefira)
			{
				// overtimeCheck++;
				// if ((includeOvertime || overtimeCheck <= 3) && researchItemModel.curLevel < 1)
				if ((includeOvertime || !researchItemModel.info.isOvertime) && researchItemModel.curLevel < 1)
				{
					list.Add(researchItemModel);
				}
			}
		}
		List<ResearchItemModel> list2 = list;
		if (ResearchDataModel.cache1 == null)
		{
			ResearchDataModel.cache1 = new Comparison<ResearchItemModel>(ResearchItemModel.CompareById);
		}
		list2.Sort(ResearchDataModel.cache1);
		return list;
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x0016E660 File Offset: 0x0016C860
	public List<ResearchItemModel> GetUpgradedResearchListBySefira(SefiraEnum sefira)
	{
		List<ResearchItemModel> list = new List<ResearchItemModel>();
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			if (SefiraName.GetSefiraEnum(researchItemModel.info.sephira) == sefira && researchItemModel.curLevel >= 1)
			{
				list.Add(researchItemModel);
			}
		}
		List<ResearchItemModel> list2 = list;
		if (ResearchDataModel.cache2 == null)
		{
			ResearchDataModel.cache2 = new Comparison<ResearchItemModel>(ResearchItemModel.CompareById);
		}
		list2.Sort(ResearchDataModel.cache2);
		return list;
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x0016E710 File Offset: 0x0016C910
	public List<ResearchItemModel> GetUpgradedResearchList()
	{
		List<ResearchItemModel> list = new List<ResearchItemModel>();
		foreach (ResearchItemModel researchItemModel in this.researchDatas.Values)
		{
			if (researchItemModel.curLevel >= 1)
			{
				list.Add(researchItemModel);
			}
		}
		return list;
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x00032F24 File Offset: 0x00031124
	public static string ConvertResearchType(ResearchType type)
	{
		switch (type)
		{
		case ResearchType.NORMAL:
			return "Normal";
		case ResearchType.PASSIVE:
			return "Passive";
		case ResearchType.WEAPON:
			return "Weapon";
		default:
			return string.Empty;
		}
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x0016E784 File Offset: 0x0016C984
	public static ResearchType ConvertResearchType(string type)
	{
		string text = type.ToLower();
		if (text != null)
		{
			if (text == "normal")
			{
				return ResearchType.NORMAL;
			}
			if (text == "passive")
			{
				return ResearchType.PASSIVE;
			}
			if (text == "weapon")
			{
				return ResearchType.WEAPON;
			}
		}
		return ResearchType.NORMAL;
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x0016E7DC File Offset: 0x0016C9DC
	public ResearchItemModel GetModel(int id)
	{
		ResearchItemModel result = null;
		if (this.researchDatas.TryGetValue(id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0016E804 File Offset: 0x0016CA04
	public static int CurrentLevel(int id)
	{
		ResearchItemModel researchItem = ResearchDataModel.instance.GetResearchItem(id);
		if (researchItem == null)
		{
			return 0;
		}
		return researchItem.curLevel;
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x00032F54 File Offset: 0x00031154
	public static int NextLevel(int id)
	{
		return ResearchDataModel.CurrentLevel(id) + 1;
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x00032F5E File Offset: 0x0003115E
	public static int GetLevel(int id, int value)
	{
		return ResearchDataModel.CurrentLevel(id) + value;
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x0016E82C File Offset: 0x0016CA2C
	public void UpgradeAllResearch()
	{
		foreach (int id in this.researchDatas.Keys)
		{
			this.UpgradeResearch(id, true);
		}
	}

	// Token: 0x040033C3 RID: 13251
	private static ResearchDataModel _instance;

	// Token: 0x040033C4 RID: 13252
	private Dictionary<int, ResearchItemModel> researchDatas = new Dictionary<int, ResearchItemModel>();

	// Token: 0x040033C5 RID: 13253
	private ResearchUnitStatUpgrade agentStatBonus = new ResearchUnitStatUpgrade();

	// Token: 0x040033C6 RID: 13254
	private ResearchPromotionEasily promotionEasilyUpgrade = new ResearchPromotionEasily();

	// Token: 0x040033C7 RID: 13255
	private Dictionary<int, int> weaponLevelResearchInfo = new Dictionary<int, int>();

	// Token: 0x040033C8 RID: 13256
	private Dictionary<string, int> sephiraabilityLevel = new Dictionary<string, int>();

	// Token: 0x040033C9 RID: 13257
	private Dictionary<string, bool> specialAbility = new Dictionary<string, bool>();

	// Token: 0x040033CA RID: 13258
	private Dictionary<GlobalBulletType, bool> bulletAbility = new Dictionary<GlobalBulletType, bool>();

	// Token: 0x040033CB RID: 13259
	private static Comparison<ResearchItemModel> cache0;

	// Token: 0x040033CC RID: 13260
	private static Comparison<ResearchItemModel> cache1;

	// Token: 0x040033CD RID: 13261
	private static Comparison<ResearchItemModel> cache2;
}
