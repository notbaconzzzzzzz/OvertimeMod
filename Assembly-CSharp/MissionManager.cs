/*
private MissionManager() // 
public void OnStageStart() // 
public Mission GetNextMission(SefiraEnum sefira, out List<string> requireTextList, out bool isBossMission) // 
+public Mission GetNextMission(SefiraEnum sefira) // 
public Mission GetAvailableMission(SefiraEnum sefira, out List<string> requireTextList, out bool isBossMission) // 
+public bool ExistsOvertimeBossMission(SefiraEnum sefira) // 
public bool ExistsFinishedBossMission(SefiraEnum sefira) // 
+public bool ExistsFinishedOvertimeBossMission(SefiraEnum sefira) // 
*/
using System;
using System.Linq; //
using System.Collections.Generic;
using UnityEngine; // 

// Token: 0x020006BB RID: 1723
public class MissionManager : IObserver
{
	// Token: 0x060037C1 RID: 14273 RVA: 0x00169F48 File Offset: 0x00168148
	private MissionManager()
	{ // <Mod>
		Notice.instance.Observe(NoticeName.OnReleaseWork, this);
		Notice.instance.Observe(NoticeName.OnStageEnd, this);
		Notice.instance.Observe(NoticeName.OnAgentPromote, this);
		Notice.instance.Observe(NoticeName.OnCreatureSuppressed, this);
		Notice.instance.Observe(NoticeName.OnNextDay, this);
		Notice.instance.Observe(NoticeName.OnQliphothOverloadLevelChanged, this);
		Notice.instance.Observe(NoticeName.WorkToOverloaded, this);
		Notice.instance.Observe(NoticeName.OrdealEnd, this);
		Notice.instance.Observe(NoticeName.OnStageStart, this);
		Notice.instance.Observe(NoticeName.MakeEquipment, this);
		Notice.instance.Observe(NoticeName.OnDestroyBossCore, this);
		Notice.instance.Observe(NoticeName.OnIsolateOverloaded, this);
		Notice.instance.Observe(NoticeName.OnAgentPanic, this);
		Notice.instance.Observe(NoticeName.OnAgentPanicReturn, this);
		Notice.instance.Observe(NoticeName.OnAgentDead, this);
		Notice.instance.Observe(NoticeName.RemoveEquipment, this);
		Notice.instance.Observe(NoticeName.CreatureObserveLevelAdded, this);
		/*>*/
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
		Notice.instance.Observe(NoticeName.OnEscape, this);
		Notice.instance.Observe(NoticeName.OnOpenNameplate, this);
		Notice.instance.Observe(NoticeName.RecoverByRegenerator, this);
		Notice.instance.Observe(NoticeName.RecoverByBullet, this);
		Notice.instance.Observe(NoticeName.OnOfficerDie, this);
		Notice.instance.Observe(NoticeName.AddExcessEnergy, this);
		Notice.instance.Observe(NoticeName.BlockDamageByShield, this);
		Notice.instance.Observe(NoticeName.CreatureDamagedByAgent, this);
		Notice.instance.Observe(NoticeName.OnUseBullet, this);
		Notice.instance.Observe(NoticeName.OnPause, this);
		Notice.instance.Observe(NoticeName.CreatureHitWorker, this);
		Notice.instance.Observe(NoticeName.Update, this);
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x060037C2 RID: 14274 RVA: 0x00032141 File Offset: 0x00030341
	public static MissionManager instance
	{
		get
		{
			if (MissionManager._instance == null)
			{
				MissionManager._instance = new MissionManager();
			}
			return MissionManager._instance;
		}
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0016A06C File Offset: 0x0016826C
	public void Init()
	{
		this.remainMissions = new List<Mission>();
		this.missionsInProgress = new List<Mission>();
		this.clearedMissions = new List<Mission>();
		this.closedMissions = new List<Mission>();
		foreach (MissionTypeInfo metadata in MissionTypeList.instance.GetList())
		{
			this.remainMissions.Add(new Mission(metadata));
		}
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0016A100 File Offset: 0x00168300
	public void DebugMissionClear(int index)
	{
		Mission mission = this.missionsInProgress.Find((Mission x) => x.metaInfo.sefira == (SefiraEnum)index);
		if (mission != null)
		{
			mission.isCleared = true;
		}
		Notice.instance.Send(NoticeName.OnMissionProgressed, new object[0]);
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x0016A154 File Offset: 0x00168354
	public void LoadData(Dictionary<string, object> dic)
	{
		string a = "old";
		GameUtil.TryGetValue<string>(dic, "ver", ref a);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		List<Dictionary<string, object>> list3 = new List<Dictionary<string, object>>();
		List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>();
		this.remainMissions = new List<Mission>();
		foreach (MissionTypeInfo metadata in MissionTypeList.instance.GetList())
		{
			this.remainMissions.Add(new Mission(metadata));
		}
		if (a == "old")
		{
			GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "missionsInProgress", ref list2);
			this.missionsInProgress = new List<Mission>();
			this.clearedMissions = new List<Mission>();
			this.closedMissions = new List<Mission>();
			foreach (Dictionary<string, object> dic2 in list2)
			{
				Mission m = new Mission();
				m.LoadData(dic2);
				m.OnEnabled();
				this.missionsInProgress.Add(m);
				this.remainMissions.RemoveAll((Mission x) => x.metaInfo.id == m.metaInfo.id);
			}
			foreach (ResearchItemModel researchItemModel in ResearchDataModel.instance.GetUpgradedResearchList())
			{
				SefiraEnum se = SefiraManager.instance.GetSefira(researchItemModel.info.sephira).sefiraEnum;
				List<Mission> list5 = this.remainMissions.FindAll((Mission x) => x.metaInfo.sefira == se);
				Mission mission = null;
				int num = 999;
				foreach (Mission mission2 in list5)
				{
					if (mission2.metaInfo.sefira_Level < num)
					{
						mission = mission2;
						num = mission2.metaInfo.sefira_Level;
					}
				}
				if (mission != null)
				{
					this.remainMissions.Remove(mission);
					this.closedMissions.Add(mission);
				}
			}
		}
		else if (a == "boss1")
		{
			this.missionsInProgress = new List<Mission>();
			this.clearedMissions = new List<Mission>();
			this.closedMissions = new List<Mission>();
			GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "missionsInProgress", ref list2);
			GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "clearedMissions", ref list3);
			GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "closedMissions", ref list4);
			foreach (Dictionary<string, object> dic3 in list2)
			{
				Mission m = new Mission();
				m.LoadData(dic3);
				m.OnEnabled();
				this.missionsInProgress.Add(m);
				this.remainMissions.RemoveAll((Mission x) => x.metaInfo.id == m.metaInfo.id);
			}
			foreach (Dictionary<string, object> dic4 in list3)
			{
				Mission m = new Mission();
				m.LoadData(dic4);
				this.clearedMissions.Add(m);
				this.remainMissions.RemoveAll((Mission x) => x.metaInfo.id == m.metaInfo.id);
			}
			foreach (Dictionary<string, object> dic5 in list4)
			{
				Mission m = new Mission();
				m.LoadData(dic5);
				this.closedMissions.Add(m);
				this.remainMissions.RemoveAll((Mission x) => x.metaInfo.id == m.metaInfo.id);
			}
		}
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x0016A608 File Offset: 0x00168808
	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("ver", "boss1");
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (Mission mission in this.missionsInProgress)
		{
			list.Add(mission.GetSaveData());
		}
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		foreach (Mission mission2 in this.clearedMissions)
		{
			list2.Add(mission2.GetSaveData());
		}
		List<Dictionary<string, object>> list3 = new List<Dictionary<string, object>>();
		foreach (Mission mission3 in this.closedMissions)
		{
			list3.Add(mission3.GetSaveData());
		}
		dictionary.Add("missionsInProgress", list);
		dictionary.Add("clearedMissions", list2);
		dictionary.Add("closedMissions", list3);
		return dictionary;
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0016A764 File Offset: 0x00168964
	public void OnStageStart()
	{ // <Mod>
		foreach (Mission mission in this.missionsInProgress)
		{
			if (SefiraManager.instance.IsOpened(mission.metaInfo.sefira) || mission.metaInfo.sefira == SefiraEnum.DUMMY)
			{
				mission.OnEnabled();
			}
			else
			{
				mission.isInProcess = false;
			}
			if (mission.successCondition.condition_Type == ConditionType.CLEAR_TIME)
			{
				MissionConditionTypeInfo metaInfo = mission.successCondition.metaInfo;
				int num = PlayerModel.instance.GetDay() + 1;
				metaInfo.goal = (int)((float)metaInfo.minimumSecond + ((float)num - metaInfo.var1) * metaInfo.var2);
				if (metaInfo.goal < metaInfo.minimumSecond)
				{
					metaInfo.goal = metaInfo.minimumSecond;
				}
			}/*
			else if (mission.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE_BY_KIND)
			{
				MissionConditionTypeInfo metaInfo = mission.successCondition.metaInfo;
				if (metaInfo.percent > 0f)
				{
					int num = 0;
					foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
					{
						if (creature.metaInfo.isEscapeAble)
						{
							num++;
						}
					}
					metaInfo.goal = Mathf.CeilToInt(num * metaInfo.percent);
				}
			}*/
			foreach (Condition condition in mission.failConditions)
			{
				if (condition.condition_Type == ConditionType.CLERK_DEAD)
				{
					MissionConditionTypeInfo metaInfo = condition.metaInfo;
					if (metaInfo.percent > 0f)
					{
						int num = 0;
						foreach (Sefira sefira in SefiraManager.instance.GetOpendSefiraList())
						{
							num += Mathf.Min(10, sefira.openLevel * 2);
						}
						metaInfo.goal = Mathf.CeilToInt(num * metaInfo.percent);
					}
				}
			}
		}
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x0016A8D8 File Offset: 0x00168AD8
	public void ReleaseGame()
	{
		foreach (Mission mission in this.missionsInProgress)
		{
			foreach (Condition condition in mission.conditions)
			{
				condition.current = 0;
			}
			mission.OnEnabled();
		}
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x0016A980 File Offset: 0x00168B80
	public void ClearMission(Mission m)
	{
		if (m.isCleared)
		{
			this.clearedMissions.Add(m);
			this.missionsInProgress.Remove(m);
		}
		else if (!m.isInProcess)
		{
			foreach (Condition condition in m.conditions)
			{
				condition.current = 0;
			}
			m.OnEnabled();
		}
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x0016AA18 File Offset: 0x00168C18
	public Mission CheckMissionComplete(SefiraEnum sefira)
	{
		if (!SefiraManager.instance.IsOpened(sefira))
		{
			return null;
		}
		Mission mission = this.missionsInProgress.Find((Mission x) => x.metaInfo.sefira == sefira);
		if (mission == null)
		{
			return null;
		}
		if (mission.isCleared)
		{
			this.clearedMissions.Add(mission);
			this.missionsInProgress.Remove(mission);
			return mission;
		}
		if (!mission.isInProcess)
		{
			foreach (Condition condition in mission.conditions)
			{
				condition.current = 0;
			}
			mission.OnEnabled();
		}
		return null;
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0016AAF0 File Offset: 0x00168CF0
	public Mission GetAvailableMission(SefiraEnum sefira)
	{
		List<string> list;
		bool flag;
		return this.GetAvailableMission(sefira, out list, out flag);
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0016AB08 File Offset: 0x00168D08
	public Mission GetNextMission(SefiraEnum sefira, out List<string> requireTextList, out bool isBossMission)
	{ // <Mod>
		requireTextList = new List<string>();
		isBossMission = false;
		if (sefira != SefiraEnum.MALKUT && !SefiraManager.instance.IsOpened(sefira) && PlayerModel.instance.GetDay() < 40)
		{
			return null;
		}
		if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
		{
			if (missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type != ConditionType.DESTROY_CORE))
			{
				return null;
			}
		}
		else if (this.missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira))
		{
			return null;
		}
		Mission mission = null;
		List<Mission> list = this.remainMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira);
		int num = 999;
		int nonCount = 0;
		int overCount = 0;
		foreach (Mission mission2 in list)
		{
			if (mission2.metaInfo.sefira_Level < num)
			{
				mission = mission2;
				num = mission2.metaInfo.sefira_Level;
			}
			if (mission2.metaInfo.sefira_Level > 5)
			{
				overCount++;
			}
			else
			{
				nonCount++;
			}
		}
		if (mission == null)
		{
			return null;
		}
		bool isOvertime = mission.metaInfo.sefira_Level > 5;
		bool giveEarly = false;
		if (isOvertime)
		{
			if (!SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
			{
				return null;
			}
			if (!PlayerModel.instance.IsOvertimeMode() && !SpecialModeConfig.instance.GetValue<bool>("JailbreakOvertimeMissions"))
			{
				requireTextList.Add("Return to Day 1 to enable Overtime Missions");
				return mission;
			}
			giveEarly = 45 - overCount <= PlayerModel.instance.GetDay();
		}
		else
		{
			giveEarly = 45 - nonCount <= PlayerModel.instance.GetDay();
		}
		if (!isOvertime && !giveEarly)
		{
			if (sefira == SefiraEnum.MALKUT)
			{
				if (mission.metaInfo.sefira_Level - 1 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 1));
				}
			}
			else if (mission.metaInfo.sefira_Level > SefiraManager.instance.GetSefiraOpenLevel(sefira))
			{
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level));
			}
		}
		else if (!giveEarly)
		{
			if (sefira == SefiraEnum.MALKUT)
			{
				if (mission.metaInfo.sefira_Level - 6 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 6));
				}
			}
			else if (mission.metaInfo.sefira_Level - 5 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
			{
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 5));
			}
		}
		if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
		{
			isBossMission = true;
		}
		else
		{
			isBossMission = false;
		}
		foreach (MissionPrerequisite missionPrerequisite in mission.metaInfo.requires)
		{
			int clearedOrClosedMissionNum = this.GetClearedOrClosedMissionNum(missionPrerequisite.sefira);
			SefiraLevel sefiraLevel = SefiraManager.instance.GetSefiraLevel(sefira);
			SefiraLevel sefiraLevel2 = SefiraManager.instance.GetSefiraLevel(missionPrerequisite.sefira);
			if (clearedOrClosedMissionNum < missionPrerequisite.level)
			{
				if (isBossMission)
				{
					if (sefiraLevel == SefiraLevel.MIDDILE)
					{
						if (sefiraLevel > sefiraLevel2)
						{
							if (requireTextList.Count == 0)
							{
								requireTextList.Add(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_up"));
							}
							else
							{
								requireTextList[0] = LocalizeTextDataModel.instance.GetText("SefiraBossCondition_up");
							}
							break;
						}
					}
					else if (sefiraLevel == SefiraLevel.DOWN && sefiraLevel > sefiraLevel2)
					{
						if (requireTextList.Count == 0)
						{
							requireTextList.Add(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_middle"));
						}
						else
						{
							requireTextList[0] = LocalizeTextDataModel.instance.GetText("SefiraBossCondition_middle");
						}
						break;
					}
				}
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOtherOpenLevel"), SefiraName.GetLocalizingSefiraName(missionPrerequisite.sefira), missionPrerequisite.level));
			}
		}
		if(!isOvertime)
		{
			if (sefira == SefiraEnum.GEBURAH)
			{
				beHonest = true;
				if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE && !this.ExistsBossMission(SefiraEnum.CHESED) && !this.ExistsFinishedBossMission(SefiraEnum.CHESED))
				{
					int num2 = 5;
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.CHESED), num2));
				}
				beHonest = false;
			}
			else if (sefira == SefiraEnum.CHESED && mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				int num3 = 5;
				if (SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.GEBURAH) < num3)
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.GEBURAH), num3));
				}
			}
		}
		else
		{
			if (sefira == SefiraEnum.GEBURAH)
			{
				if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE && !this.ExistsOvertimeBossMission(SefiraEnum.CHESED) && !this.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
				{
					int num2 = 5;
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.CHESED), num2));
				}
			}
			else if (sefira == SefiraEnum.CHESED && mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				int num3 = 5;
				if (SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.GEBURAH) < num3)
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.GEBURAH), num3));
				}
			}
		}
		return mission;
	}

	// <Mod>
	public Mission GetNextMission(SefiraEnum sefira)
	{
		Mission mission = null;
		List<Mission> list = this.remainMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira);
		int num = 999;
		foreach (Mission mission2 in list)
		{
			if (mission2.metaInfo.sefira_Level < num)
			{
				mission = mission2;
				num = mission2.metaInfo.sefira_Level;
			}
		}
		if (mission == null)
		{
			return null;
		}
		bool isOvertime = mission.metaInfo.sefira_Level > 5;
		if (isOvertime)
		{
			if (!SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
			{
				return null;
			}
		}
		return mission;
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x0016AF10 File Offset: 0x00169110
	public Mission GetAvailableMission(SefiraEnum sefira, out List<string> requireTextList, out bool isBossMission)
	{ // <Mod>
		requireTextList = new List<string>();
		isBossMission = false;
		if (sefira != SefiraEnum.MALKUT && !SefiraManager.instance.IsOpened(sefira) && PlayerModel.instance.GetDay() < 40)
		{
			return null;
		}
		if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
		{
			if (!missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE))
			{
				List<Mission> list2 = remainMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE);
				Mission mission3 = null;
				int num4 = 999;
				foreach (Mission mission2 in list2)
				{
					if (mission2.metaInfo.sefira_Level < num4)
					{
						mission3 = mission2;
						num4 = mission2.metaInfo.sefira_Level;
					}
				}
				if (mission3 == null) return null;
				bool isOvertime2 = mission3.metaInfo.sefira_Level > 5;
				if (isOvertime2 && !SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions")) return null;
				isBossMission = true;
				return mission3;
			}
			if (missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira && x.metaInfo.sefira_Level < 5 && x.successCondition.condition_Type != ConditionType.DESTROY_CORE))
			{
				if (SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") && !missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira && x.metaInfo.sefira_Level > 5 && x.successCondition.condition_Type != ConditionType.DESTROY_CORE))
				{
					List<Mission> list2 = remainMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira && x.metaInfo.sefira_Level > 5 && x.successCondition.condition_Type != ConditionType.DESTROY_CORE);
					Mission mission3 = null;
					int num4 = 999;
					foreach (Mission mission2 in list2)
					{
						if (mission2.metaInfo.sefira_Level < num4)
						{
							mission3 = mission2;
							num4 = mission2.metaInfo.sefira_Level;
						}
					}
					if (mission3 != null && mission3.metaInfo.sefira_Level - 5 <= SefiraManager.instance.GetSefiraOpenLevel(sefira)) return mission3;
				}
				return null;
			}
		}
		else if (this.missionsInProgress.Exists((Mission x) => x.metaInfo.sefira == sefira))
		{
			return null;
		}
		Mission mission = null;
		List<Mission> list = this.remainMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira);
		int num = 999;
		int nonCount = 0;
		int overCount = 0;
		foreach (Mission mission2 in list)
		{
			if (mission2.metaInfo.sefira_Level < num)
			{
				mission = mission2;
				num = mission2.metaInfo.sefira_Level;
			}
			if (mission2.metaInfo.sefira_Level > 5)
			{
				overCount++;
			}
			else
			{
				nonCount++;
			}
		}
		if (mission == null)
		{
			return null;
		}
		bool isOvertime = mission.metaInfo.sefira_Level > 5;
		bool giveEarly = false;
		if (isOvertime)
		{
			if (!SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
			{
				return null;
			}
			if (!PlayerModel.instance.IsOvertimeMode() && !SpecialModeConfig.instance.GetValue<bool>("JailbreakOvertimeMissions") && !SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
			{
				requireTextList.Add("Return to Day 1 to enable Overtime Missions");
				return null;
			}
			giveEarly = 45 - overCount >= PlayerModel.instance.GetDay();
		}
		else
		{
			giveEarly = 45 - nonCount >= PlayerModel.instance.GetDay();
		}
		if (!isOvertime && !giveEarly)
		{
			if (sefira == SefiraEnum.MALKUT)
			{
				if (mission.metaInfo.sefira_Level - 1 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 1));
				}
			}
			else if (mission.metaInfo.sefira_Level > SefiraManager.instance.GetSefiraOpenLevel(sefira))
			{
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level));
			}
		}
		else if (!giveEarly)
		{
			if (sefira == SefiraEnum.MALKUT)
			{
				if (mission.metaInfo.sefira_Level - 6 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 6));
				}
			}
			else if (mission.metaInfo.sefira_Level - 5 > SefiraManager.instance.GetSefiraOpenLevel(sefira))
			{
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel"), mission.metaInfo.sefira_Level - 5));
			}
		}
		if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
		{
			isBossMission = true;
		}
		else
		{
			isBossMission = false;
		}
		foreach (MissionPrerequisite missionPrerequisite in mission.metaInfo.requires)
		{
			int clearedOrClosedMissionNum = this.GetClearedOrClosedMissionNum(missionPrerequisite.sefira);
			SefiraLevel sefiraLevel = SefiraManager.instance.GetSefiraLevel(sefira);
			SefiraLevel sefiraLevel2 = SefiraManager.instance.GetSefiraLevel(missionPrerequisite.sefira);
			if (clearedOrClosedMissionNum < missionPrerequisite.level)
			{
				if (isBossMission)
				{
					if (sefiraLevel == SefiraLevel.MIDDILE)
					{
						if (sefiraLevel > sefiraLevel2)
						{
							if (requireTextList.Count == 0)
							{
								requireTextList.Add(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_up"));
							}
							else
							{
								requireTextList[0] = LocalizeTextDataModel.instance.GetText("SefiraBossCondition_up");
							}
							break;
						}
					}
					else if (sefiraLevel == SefiraLevel.DOWN && sefiraLevel > sefiraLevel2)
					{
						if (requireTextList.Count == 0)
						{
							requireTextList.Add(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_middle"));
						}
						else
						{
							requireTextList[0] = LocalizeTextDataModel.instance.GetText("SefiraBossCondition_middle");
						}
						break;
					}
				}
				requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOtherOpenLevel"), SefiraName.GetLocalizingSefiraName(missionPrerequisite.sefira), missionPrerequisite.level));
			}
		}
		if(!isOvertime)
		{
			if (sefira == SefiraEnum.GEBURAH)
			{
				beHonest = true;
				if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE && !this.ExistsBossMission(SefiraEnum.CHESED) && !this.ExistsFinishedBossMission(SefiraEnum.CHESED))
				{
					int num2 = 5;
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.CHESED), num2));
				}
				beHonest = false;
			}
			else if (sefira == SefiraEnum.CHESED && mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				int num3 = 5;
				if (SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.GEBURAH) < num3)
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.GEBURAH), num3));
				}
			}
		}
		else
		{
			if (sefira == SefiraEnum.GEBURAH)
			{
				if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE && !this.ExistsOvertimeBossMission(SefiraEnum.CHESED) && !this.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
				{
					int num2 = 5;
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.CHESED), num2));
				}
			}
			else if (sefira == SefiraEnum.CHESED && mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				int num3 = 5;
				if (SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.GEBURAH) < num3)
				{
					requireTextList.Add(string.Format(LocalizeTextDataModel.instance.GetText("MissionConditionOpenLevel_new"), SefiraName.GetLocalizingSefiraName(SefiraEnum.GEBURAH), num3));
				}
			}
		}
		if (requireTextList.Count != 0)
		{
			return null;
		}
		return mission;
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x0016B328 File Offset: 0x00169528
	public void RemoveTutorialMission()
	{
		Mission mission = this.remainMissions.Find((Mission x) => x.metaInfo.id == 0);
		if (mission != null)
		{
			mission.isInProcess = false;
			mission.isCleared = false;
			this.missionsInProgress.Remove(mission);
		}
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x0016B380 File Offset: 0x00169580
	public void StartTutorialMission()
	{
		Mission mission = this.remainMissions.Find((Mission x) => x.metaInfo.id == 0);
		if (mission != null)
		{
			mission.OnEnabled();
			this.missionsInProgress.Add(mission);
		}
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x0016B3D0 File Offset: 0x001695D0
	public void StartMission(int id)
	{
		Mission mission = this.remainMissions.Find((Mission x) => x.metaInfo.id == id);
		mission.OnEnabled();
		this.remainMissions.Remove(mission);
		this.missionsInProgress.Add(mission);
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x0003215C File Offset: 0x0003035C
	public List<Mission> GetReadyToClearMissions()
	{
		return this.missionsInProgress.FindAll((Mission x) => x.isCleared && (SefiraManager.instance.IsOpened(x.metaInfo.sefira) || x.metaInfo.sefira == SefiraEnum.DUMMY));
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0016B424 File Offset: 0x00169624
	public Mission GetReadyToClearMission(SefiraEnum sefiraEnum)
	{
		return this.missionsInProgress.Find((Mission x) => x.isCleared && SefiraManager.instance.IsOpened(x.metaInfo.sefira) && x.metaInfo.sefira == sefiraEnum);
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x00032186 File Offset: 0x00030386
	public List<Mission> GetClearedMissions()
	{
		return this.clearedMissions.FindAll((Mission x) => SefiraManager.instance.IsOpened(x.metaInfo.sefira));
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x000321B0 File Offset: 0x000303B0
	public List<Mission> GetMissionsInProgress()
	{
		return this.missionsInProgress.FindAll((Mission x) => SefiraManager.instance.IsOpened(x.metaInfo.sefira) || x.metaInfo.sefira == SefiraEnum.DUMMY);
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x0016B458 File Offset: 0x00169658
	public bool ExistsBossMission()
	{
		Mission mission = this.missionsInProgress.Find((Mission x) => x.successCondition != null && x.successCondition.condition_Type == ConditionType.DESTROY_CORE);
		return mission != null;
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x0016B498 File Offset: 0x00169698
	public Mission GetBossMission(SefiraEnum sefira)
	{
		return this.missionsInProgress.Find((Mission x) => x.successCondition != null && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira == sefira);
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0016B4CC File Offset: 0x001696CC
	public bool ExistsBossMission(SefiraEnum sefira)
	{
		Mission bossMission = this.GetBossMission(sefira);
		return bossMission != null;
	}

	// <Mod>
	public bool ExistsOvertimeBossMission(SefiraEnum sefira)
	{
		Mission bossMission = this.GetBossMission(sefira);
		return bossMission != null && bossMission.metaInfo.sefira_Level > 5;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0016B4E8 File Offset: 0x001696E8
	public bool ExistsClearedMission(SefiraEnum sefira)
	{
		Mission mission = this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == sefira);
		return mission != null;
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0016B524 File Offset: 0x00169724
	public bool ExistsFinishedBossMission()
	{
		bool result;
		if (this.clearedMissions.Find((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE) == null)
		{
			result = (this.closedMissions.Find((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE) != null);
		}
		else
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x0016B590 File Offset: 0x00169790
	public bool ExistsFinishedBossMission(SefiraEnum sefira)
	{ // <Mod>
		bool result;
		if (sefira == SefiraEnum.TIPERERTH1 || sefira == SefiraEnum.TIPERERTH2)
		{
			if (this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == SefiraEnum.TIPERERTH1 && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level <= 5) == null)
			{
				result = (this.closedMissions.Find((Mission x) => x.metaInfo.sefira == SefiraEnum.TIPERERTH1 && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level <= 5) != null);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level <= 5) != null || this.closedMissions.Find((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level <= 5) != null;
		}
		if (!beHonest)
		{
			if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch")) result = !result;
			if (SefiraBossManager.Instance.NegateResearch(sefira)) result = false;
		}
		return result;
	}

	// <Mod>
	public bool ExistsFinishedOvertimeBossMission(SefiraEnum sefira)
	{
		bool result;
		if (sefira == SefiraEnum.TIPERERTH1 || sefira == SefiraEnum.TIPERERTH2)
		{
			if (this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == SefiraEnum.TIPERERTH1 && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level > 5) == null)
			{
				result = (this.closedMissions.Find((Mission x) => x.metaInfo.sefira == SefiraEnum.TIPERERTH1 && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level > 5) != null);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level > 5) != null || this.closedMissions.Find((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type == ConditionType.DESTROY_CORE && x.metaInfo.sefira_Level > 5) != null;
		}
		if (!beHonest)
		{
			if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch") && SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions")) result = !result;
			if (SefiraBossManager.Instance.NegateResearch(sefira)) result = false;
		}
		return result;
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x000321DA File Offset: 0x000303DA
	public void CloseClearedMission(Mission m)
	{
		if (m != null)
		{
			this.clearedMissions.Remove(m);
			this.closedMissions.Add(m);
		}
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0016B660 File Offset: 0x00169860
	public void CloseClearedMission_ExceptBoss(SefiraEnum sefira)
	{
		Mission mission = this.clearedMissions.Find((Mission x) => x.metaInfo.sefira == sefira && x.successCondition.condition_Type != ConditionType.DESTROY_CORE);
		if (mission != null)
		{
			this.clearedMissions.Remove(mission);
			this.closedMissions.Add(mission);
		}
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0016B6B4 File Offset: 0x001698B4
	public Mission GetCurrentSefiraMission(SefiraEnum sefira)
	{
		foreach (Mission mission in this.missionsInProgress)
		{
			if (SefiraName.GetSefiraEnum(mission.metaInfo.sefira_Name) == sefira)
			{
				return mission;
			}
		}
		return null;
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0016B72C File Offset: 0x0016992C
	public List<Mission> GetClearedOrClosedBossMissions()
	{
		List<Mission> list = new List<Mission>();
		list.AddRange(this.closedMissions.FindAll((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE));
		list.AddRange(this.clearedMissions.FindAll((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE));
		return list;
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0016B79C File Offset: 0x0016999C
	public int GetClearedOrClosedBossMissionNum()
	{
		return this.closedMissions.FindAll((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE).Count + this.clearedMissions.FindAll((Mission x) => x.successCondition.condition_Type == ConditionType.DESTROY_CORE).Count;
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0016B804 File Offset: 0x00169A04
	public int GetClearedOrClosedMissionNum(SefiraEnum sefira)
	{
		return this.clearedMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira).Count + this.closedMissions.FindAll((Mission x) => x.metaInfo.sefira == sefira).Count;
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x0016B858 File Offset: 0x00169A58
	public void OnNotice(string notice, params object[] param)
	{
		for (int i = 0; i < this.missionsInProgress.Count; i++)
		{
			this.missionsInProgress[i].CheckConditions(notice, param);
		}
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x000040A1 File Offset: 0x000022A1
	// Note: this type is marked as 'beforefieldinit'.
	static MissionManager()
	{
	}

	// Token: 0x04003339 RID: 13113
	private static MissionManager _instance;

	// Token: 0x0400333A RID: 13114
	private List<Mission> remainMissions;

	// Token: 0x0400333B RID: 13115
	private List<Mission> missionsInProgress;

	// Token: 0x0400333C RID: 13116
	private List<Mission> clearedMissions;

	// Token: 0x0400333D RID: 13117
	private List<Mission> closedMissions;

	// <Mod>
	public static bool beHonest = false;
}
