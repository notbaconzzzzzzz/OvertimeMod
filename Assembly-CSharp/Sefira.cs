/*
private void UpdateSefiraAce() // 
private void InitRecovoerPassages() // 
public void OnFixedUpdate() // 
public void ReturnAgentsToSefira() // 
*/
using System;
using System.Collections.Generic;
using GameStatusUI;
using GlobalBullet;
using UnityEngine;

// Token: 0x020007E8 RID: 2024
public class Sefira
{
	// Token: 0x06003E5B RID: 15963 RVA: 0x00182BD0 File Offset: 0x00180DD0
	public Sefira(string name, int index, string indexString, SefiraEnum sefiraEnum)
	{
		this.name = name;
		this.index = index;
		this.indexString = indexString;
		this._sefiraEnum = sefiraEnum;
		this.creatureList = new List<CreatureModel>();
		this.officerList = new List<OfficerModel>();
		this.workingList = new List<int>();
		this.idleList = new List<int>();
		this.agentList = new List<AgentModel>();
		this.officerCnt = 0;
		this.escapedCreatures = new List<CreatureModel>();
		this.departPassageList = new List<PassageObjectModel>();
		this.passageList = new List<PassageObjectModel>();
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06003E5C RID: 15964 RVA: 0x00036659 File Offset: 0x00034859
	public bool activated
	{
		get
		{
			return this._activated;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06003E5D RID: 15965 RVA: 0x00036661 File Offset: 0x00034861
	public SefiraEnum sefiraEnum
	{
		get
		{
			return this._sefiraEnum;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06003E5E RID: 15966 RVA: 0x00036669 File Offset: 0x00034869
	public int openLevel
	{
		get
		{
			return this._openLevel;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06003E5F RID: 15967 RVA: 0x00036671 File Offset: 0x00034871
	public int allocateMax
	{
		get
		{
			if (this._sefiraEnum == SefiraEnum.TIPERERTH1 || this._sefiraEnum == SefiraEnum.TIPERERTH2)
			{
				return 5;
			}
			return Mathf.Min(5, 2 + this._openLevel);
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06003E60 RID: 15968 RVA: 0x0003669B File Offset: 0x0003489B
	private float regenerationValue
	{
		get
		{
			return (float)(6 + ResearchDataModel.instance.GetAgentStatBonus().regeneration);
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06003E61 RID: 15969 RVA: 0x000366AF File Offset: 0x000348AF
	private float regenerationMentalValue
	{
		get
		{
			return (float)(6 + ResearchDataModel.instance.GetAgentStatBonus().regenerationMental);
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06003E62 RID: 15970 RVA: 0x000366C3 File Offset: 0x000348C3
	public float recoverElapsedTime
	{
		get
		{
			return this._recoverElapsedTime;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06003E63 RID: 15971 RVA: 0x000366CB File Offset: 0x000348CB
	public bool activatedEmptyCounter
	{
		get
		{
			return this._activatedEmptyCounter;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06003E64 RID: 15972 RVA: 0x000366D3 File Offset: 0x000348D3
	public float agentEmptyElapsedTime
	{
		get
		{
			return this._agentEmptyElapsedTime;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06003E65 RID: 15973 RVA: 0x000366DB File Offset: 0x000348DB
	public bool agentDeadPenaltyActivated
	{
		get
		{
			return this._agentDeadPenaltyActivated;
		}
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x000366E3 File Offset: 0x000348E3
	public void LoadData(Dictionary<string, object> dic)
	{
		GameUtil.TryGetValue<bool>(dic, "activated", ref this._activated);
		GameUtil.TryGetValue<int>(dic, "openLevel", ref this._openLevel);
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x00182CD0 File Offset: 0x00180ED0
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"activated",
				this._activated
			},
			{
				"openLevel",
				this._openLevel
			}
		};
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x00036709 File Offset: 0x00034909
	public void AddOpenLevel()
	{
		this._openLevel++;
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x00036719 File Offset: 0x00034919
	public void SetOpenLevel(int level)
	{
		this._openLevel = level;
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x00036722 File Offset: 0x00034922
	public void Activate()
	{
		this._activated = true;
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x0003672B File Offset: 0x0003492B
	public void ResetPassageData()
	{
		this.departPassageList = new List<PassageObjectModel>();
		this.passageList = new List<PassageObjectModel>();
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x00036743 File Offset: 0x00034943
	public void AddUnit(OfficerModel add)
	{
		this.officerList.Add(add);
		this.officerCnt++;
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x0003675F File Offset: 0x0003495F
	public void AddAgent(AgentModel add)
	{
		if (this.agentList.Contains(add))
		{
			return;
		}
		this.agentList.Add(add);
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x00182D10 File Offset: 0x00180F10
	public void OnStageStart_first()
	{
		this.deadRate = 0;
		this.panicRate = 0;
		if (this.activated)
		{
			if (this.agentList.Count == 0)
			{
				Notice.instance.Send(NoticeName.SefiraDisabled, new object[]
				{
					this
				});
				this.SefiraClosed = true;
			}
			else
			{
				Notice.instance.Send(NoticeName.SefiraEnabled, new object[]
				{
					this
				});
				this.SefiraClosed = false;
			}
		}
		this.InitPassages();
		this.InitDepartList();
		this.InitOfficerGroup();
		this.InitRecovoerPassages();
		this._abilityCheckTimer.StopTimer();
		this._currentOfficerAliveLevel = 0;
		this._agentCheckTimer.StopTimer();
		this._activatedEmptyCounter = false;
		this._agentEmptyElapsedTime = 0f;
		this._agentDeadCheckTimer.StopTimer();
		this._agentDeadPenaltyActivated = false;
		this.UpdateOfficerAliveLevel();
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0003677F File Offset: 0x0003497F
	public void RemoveAgent(AgentModel unit)
	{
		this.agentList.Remove(unit);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0003678E File Offset: 0x0003498E
	public void ClearAgent()
	{
		this.agentList.Clear();
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x00182DEC File Offset: 0x00180FEC
	public void InitCreatureArray()
	{
		this.creatureAry = this.creatureList.ToArray();
		for (int i = 0; i < this.creatureList.Count; i++)
		{
			this.idleList.Add(i);
		}
		this.isWorking = new bool[this.creatureList.Count];
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x0003679B File Offset: 0x0003499B
	public void ClearCreature()
	{
		this.creatureList.Clear();
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x000367A8 File Offset: 0x000349A8
	public void SetSefiraPassage(PassageObjectModel passage)
	{
		this.sefiraPassage = passage;
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x000367B1 File Offset: 0x000349B1
	public void AddDepartmentPassage(PassageObjectModel passage)
	{
		this.departPassageList.Add(passage);
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x00182E48 File Offset: 0x00181048
	public void AddPassage(PassageObjectModel passage)
	{
		this.passageList.Add(passage);
		if (passage.GetPassageType() == PassageType.DEPARTMENT)
		{
			this.departPassageList.Add(passage);
		}
		else if (passage.GetPassageType() == PassageType.SEFIRA)
		{
			this.departPassageList.Add(passage);
			this.sefiraPassage = passage;
		}
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x00182E9C File Offset: 0x0018109C
	private void AssignOfficerDept()
	{
		int count = this.officerList.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			int num2 = this.openedDepartmentList.Count;
			if (num2 < 1)
			{
				num2 = 1;
			}
			this.officerList[i].deptNum = num++ % num2;
		}
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x000367BF File Offset: 0x000349BF
	public MapNode[] GetDepartNodeToArray(int index)
	{
		if (index >= this.openedDepartmentList.Count)
		{
			Debug.Log("Invalid depart index");
			return null;
		}
		return this.openedDepartmentList[index].GetNodeList();
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x00182EF8 File Offset: 0x001810F8
	public MapNode GetDepartNodeByRandom(int index)
	{
		MapNode[] nodeList = this.openedDepartmentList[index].GetNodeList();
		int num = UnityEngine.Random.Range(0, nodeList.Length);
		return nodeList[num];
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x00182F24 File Offset: 0x00181124
	public MapNode GetRandomWayPoint()
	{
		MapNode[] sefiraPassagePointNode = MapGraph.instance.GetSefiraPassagePointNode(this.indexString);
		List<int> list = new List<int>();
		if (sefiraPassagePointNode.Length == 0)
		{
			Debug.LogError("Sefira Node's Count is 0");
			return null;
		}
		for (int i = 0; i < sefiraPassagePointNode.Length; i++)
		{
			list.Add(i);
		}
		while (list.Count != 0)
		{
			int num = UnityEngine.Random.Range(0, list.Count);
			int num2 = list[num];
			MapNode mapNode = sefiraPassagePointNode[num2];
			if (mapNode.GetElevator() == null)
			{
				return mapNode;
			}
			list.Remove(num2);
		}
		Debug.LogError("Error in Sefir Node Getting");
		return null;
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x00182FD4 File Offset: 0x001811D4
	public MapNode GetOtherDepartNode(int index)
	{
		int num = 0;
		if (this.openedDepartmentList.Count > 1)
		{
			while ((num = UnityEngine.Random.Range(0, this.openedDepartmentList.Count)) == index)
			{
			}
		}
		MapNode[] nodeList = this.openedDepartmentList[num].GetNodeList();
		int num2 = UnityEngine.Random.Range(0, nodeList.Length);
		return nodeList[num2];
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x00183038 File Offset: 0x00181238
	public CreatureModel GetIdleCreature()
	{
		if (this.idleList.Count == 0)
		{
			return null;
		}
		int num = UnityEngine.Random.Range(0, this.idleList.Count);
		int num2 = this.idleList[num];
		this.isWorking[num2] = true;
		this.idleList.Remove(num2);
		this.workingList.Add(num2);
		return this.creatureAry[num2];
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x001830A0 File Offset: 0x001812A0
	public void EndCreatureWork(CreatureModel cm)
	{
		int num = this.creatureList.FindIndex((CreatureModel x) => x.Equals(cm));
		if (num == -1)
		{
			Debug.Log("Cannot find input CreatureModel");
			return;
		}
		this.isWorking[num] = false;
		this.idleList.Add(num);
		this.workingList.Remove(num);
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x000367EF File Offset: 0x000349EF
	public void ClearOfficer()
	{
		this.officerList.Clear();
		this.officerCnt = 0;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x00036803 File Offset: 0x00034A03
	public void OnStageStart()
	{
		if (ResearchDataModel.instance.IsUpgradedAbility("regeneration_speed"))
		{
			this._recoverSpeedUp = true;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("regeneration_upgrade"))
		{
			this._recoverUpgrade = true;
		}
		this.UpdateSefiraAce();
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x00183108 File Offset: 0x00181308
	private void UpdateSefiraAce()
	{ // <Mod>
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2 && !SpecialModeConfig.instance.GetValue<bool>("TwoTipherethCaptains"))
		{
			return;
		}
		List<AgentModel> list = new List<AgentModel>();
		bool flag = false;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (agentModel.isAce)
			{
				flag = true;
				break;
			}
			if (agentModel.continuousServiceDay >= 7)
			{
				list.Add(agentModel);
			}
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1 && !SpecialModeConfig.instance.GetValue<bool>("TwoTipherethCaptains"))
		{
			foreach (AgentModel agentModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).agentList)
			{
				if (agentModel2.isAce)
				{
					flag = true;
					break;
				}
				if (agentModel2.continuousServiceDay >= 7)
				{
					list.Add(agentModel2);
				}
			}
		}
		if (flag)
		{
			return;
		}
		if (list.Count > 0)
		{
			list.Sort((AgentModel x, AgentModel y) => y.originFortitudeStat + y.originPrudenceStat + y.originTemperanceStat + y.originJusticeStat - (x.originFortitudeStat + x.originPrudenceStat + x.originTemperanceStat + x.originJusticeStat));
			AgentModel maxAgent = list[0];
			List<AgentModel> list2 = list.FindAll((AgentModel x) => maxAgent.originFortitudeStat + maxAgent.originPrudenceStat + maxAgent.originTemperanceStat + maxAgent.originJusticeStat == x.originFortitudeStat + x.originPrudenceStat + x.originTemperanceStat + x.originJusticeStat);
			list2[UnityEngine.Random.Range(0, list2.Count)].SetToAce();
		}
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x00183294 File Offset: 0x00181494
	private void InitPassages()
	{
		foreach (PassageObjectModel passageObjectModel in this.passageList)
		{
			passageObjectModel.OnStageStart();
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x001832F0 File Offset: 0x001814F0
	private void InitDepartList()
	{
		this.openedDepartmentList = new List<PassageObjectModel>();
		foreach (PassageObjectModel passageObjectModel in this.departPassageList)
		{
			if (passageObjectModel.isActivate)
			{
				this.openedDepartmentList.Add(passageObjectModel);
			}
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x00183368 File Offset: 0x00181568
	private void InitOfficerGroup()
	{
		if (this.openedDepartmentList.Count <= 0)
		{
			return;
		}
		if (SefiraBossManager.Instance.CheckBossActivation(this.sefiraEnum))
		{
			return;
		}
		if (this.sefiraEnum == SefiraEnum.DAAT)
		{
			return;
		}
		if (this.sefiraEnum == SefiraEnum.KETHER)
		{
			return;
		}
		this._maxOfficer = 10;
		if (PlayerModel.instance.GetDay() >= 45)
		{
			this._maxOfficer = 5;
		}
		int num = Mathf.Min(this._maxOfficer, this.openLevel * 2);
		for (int i = 0; i < num; i++)
		{
			OfficerManager.instance.CreateOfficerModel(this.indexString);
		}
		this.AssignOfficerDept();
		foreach (OfficerModel officerModel in this.officerList)
		{
			int deptNum = officerModel.deptNum;
			MapNode[] nodeList = this.openedDepartmentList[deptNum].GetNodeList();
			int num2 = UnityEngine.Random.Range(0, nodeList.Length);
			MapNode mapNode = nodeList[num2];
			officerModel.MoveToNode(mapNode.GetId());
		}
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0018349C File Offset: 0x0018169C
	private void InitRecovoerPassages()
	{ // <Mod>
		this._checkPassages = new List<PassageObjectModel>();
		this._recoverPassages = new List<PassageObjectModel>();
		this._recoverAdditionalPassages = new List<PassageObjectModel>();
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			return;
		}
		if (!this.activated)
		{
			return;
		}
		if (this.sefiraPassage != null)
		{
			this._checkPassages.Add(this.sefiraPassage);
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			this._checkPassages.AddRange(this.departPassageList);
			this._checkPassages.Add(SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).sefiraPassage);
			this._checkPassages.AddRange(SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).departPassageList);
		}
		else if (this.sefiraEnum == SefiraEnum.CHESED || this.sefiraEnum == SefiraEnum.GEBURAH)
		{
			this._checkPassages.AddRange(this.departPassageList);
		}
		this._recoverPassages.AddRange(this._checkPassages);
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.NETZACH))
		{
			foreach (PassageObjectModel passageObjectModel in this.passageList)
			{
				if (passageObjectModel.type == PassageType.HORIZONTAL || passageObjectModel.type == PassageType.DEPARTMENT)
				{
					this._recoverAdditionalPassages.Add(passageObjectModel);
				}
			}
			if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
			{
				foreach (PassageObjectModel passageObjectModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).passageList)
				{
					if (passageObjectModel2.type == PassageType.HORIZONTAL || passageObjectModel2.type == PassageType.DEPARTMENT)
					{
						this._recoverAdditionalPassages.Add(passageObjectModel2);
					}
				}
			}
			foreach (PassageObjectModel item in this._checkPassages)
			{
				this._recoverAdditionalPassages.Remove(item);
			}
		}
		foreach (PassageObjectModel item in _recoverPassages)
		{
			item.isRegenerator = true;
		}
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x001836E0 File Offset: 0x001818E0
	public void OnFixedUpdate()
	{ // <Mod>
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			return;
		}
		if (!this.activated)
		{
			return;
		}
		if (this._abilityCheckTimer.RunTimer())
		{
			this.UpdateOfficerAliveLevel();
		}
		if (!this._abilityCheckTimer.started)
		{
			this._abilityCheckTimer.StartTimer(1f);
		}
		if (this._agentCheckTimer.RunTimer())
		{
			this.UpdateAgentDeadState();
		}
		if (!this._agentCheckTimer.started)
		{
			this._agentCheckTimer.StartTimer(0.22f);
		}
		if (this._activatedEmptyCounter)
		{
			this._agentEmptyElapsedTime += Time.deltaTime;
			if (this._agentEmptyElapsedTime >= 30f)
			{
				this._agentEmptyElapsedTime = 0f;
				this.ActivateAgentEmptyPenalty();
			}
		}
		this.isRecoverActivated = true;
		foreach (PassageObjectModel passageObjectModel in this._checkPassages)
		{
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, false))
			{
				this.isRecoverActivated = false;
				break;
			}
			foreach (MovableObjectNode movableObjectNode in passageObjectModel.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit is CreatureModel)
				{
					CreatureModel creatureModel = (CreatureModel)unit;
					if (creatureModel.state == CreatureState.ESCAPE)
					{
						this.isRecoverActivated = false;
						break;
					}
				}
			}
		}
		float num = Time.deltaTime;
		if (this._recoverSpeedUp)
		{
			num *= 1.25f;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("officer_department_bonus") && !SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true))
		{
			switch (GetOfficerAliveLevel())
			{
				case 1:
					num *= 10f/9.5f;
					break;
				case 2:
					num *= 10f/8.5f;
					break;
				case 3:
					num *= 10f/7.5f;
					break;
			}
		}
		if (!this.isRecoverActivated)
		{
			if (this._recoverUpgrade)
			{
				if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.NETZACH))
				{
					num /= 1.5f;
				}
				else
				{
					num /= 2f;
				}
			}
			else
			{
				num = 0f;
			}
		}
		this._recoverElapsedTime += num;
		float num2 = 0f;
		if (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true))
		{
			num2 += (float)(6 * SefiraAbilityValueInfo.netzachOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.NETZACH)]) / 100f;
			if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
			{
				num2 *= 2;
			}
		}
		if (this._recoverElapsedTime > 10f)
		{
			OvertimeNetzachBossBuf.IsRegenerator = true;
			this._recoverElapsedTime -= 10f;
			float totalHPrecovered = 0f;
			float totalSPrecovered = 0f;
			float recoverFactor = 1f;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true))
			{
				recoverFactor = 2f;
				if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.NETZACH))
				{
					recoverFactor = 1.5f;
				}
			}
			foreach (PassageObjectModel passageObjectModel2 in this._recoverPassages)
			{
				foreach (MovableObjectNode movableObjectNode2 in passageObjectModel2.GetEnteredTargets())
				{
					UnitModel unit2 = movableObjectNode2.GetUnit();
					if (unit2 is WorkerModel)
					{
						WorkerModel workerModel = (WorkerModel)unit2;
						if (!workerModel.IsDead())
						{
							if (!workerModel.HasEquipment(200015))
							{
								if (!workerModel.IsPanic())
								{
									float prevHP = workerModel.hp;
									float HPrecovered = 0f;
									if (!workerModel.HasUnitBuf(UnitBufType.QUEENBEE_SPORE))
									{
                                        prevHP = workerModel.hp;
										HPrecovered = workerModel.RecoverHPv2((this.regenerationValue + num2) / recoverFactor);
									}
                                    float prevSP = workerModel.mental;
									float SPrecovered = workerModel.RecoverMentalv2((this.regenerationMentalValue + num2) / recoverFactor);
									if (workerModel is AgentModel)
									{
										prevHP = workerModel.maxHp - prevHP;
										if (HPrecovered > prevHP)
										{
											HPrecovered = prevHP;
										}
										totalHPrecovered += HPrecovered;
										prevSP = workerModel.maxMental - prevSP;
										if (SPrecovered > prevSP)
										{
											SPrecovered = prevSP;
										}
										totalSPrecovered += SPrecovered;
									}
								}
							}
						}
					}
				}
			}
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true))
			{
				recoverFactor = 1f;
			}
			else
			{
				recoverFactor = 2f;
				if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.NETZACH))
				{
					recoverFactor = 1.5f;
				}
			}
			foreach (PassageObjectModel passageObjectModel3 in this._recoverAdditionalPassages)
			{
				foreach (MovableObjectNode movableObjectNode3 in passageObjectModel3.GetEnteredTargets())
				{
					UnitModel unit3 = movableObjectNode3.GetUnit();
					if (unit3 is WorkerModel)
					{
						WorkerModel workerModel = (WorkerModel)unit3;
						if (!workerModel.IsDead())
						{
							if (!workerModel.HasEquipment(200015))
							{
								if (!workerModel.IsPanic())
								{
									float prevHP = workerModel.hp;
									float HPrecovered = 0f;
									if (!workerModel.HasUnitBuf(UnitBufType.QUEENBEE_SPORE))
									{
                                        prevHP = workerModel.hp;
										HPrecovered = workerModel.RecoverHPv2((this.regenerationValue + num2) / recoverFactor);
									}
                                    float prevSP = workerModel.mental;
									float SPrecovered = workerModel.RecoverMentalv2((this.regenerationMentalValue + num2) / recoverFactor);
									if (workerModel is AgentModel)
									{
										prevHP = workerModel.maxHp - prevHP;
										if (HPrecovered > prevHP)
										{
											HPrecovered = prevHP;
										}
										totalHPrecovered += HPrecovered;
										prevSP = workerModel.maxMental - prevSP;
										if (SPrecovered > prevSP)
										{
											SPrecovered = prevSP;
										}
										totalSPrecovered += SPrecovered;
									}
								}
							}
						}
					}
				}
			}
			OvertimeNetzachBossBuf.IsRegenerator = false;
			Notice.instance.Send(NoticeName.RecoverByRegenerator, new object[]
			{
				totalHPrecovered,
				1,
				this
			});
			Notice.instance.Send(NoticeName.RecoverByRegenerator, new object[]
			{
				totalSPrecovered,
				2,
				this
			});
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x00183B58 File Offset: 0x00181D58
	public void ReturnAgentsToSefira()
	{ // <Mod>
		Notice.instance.Send(NoticeName.OnClickRecallButton, new object[]
		{
			this
		});
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead())
			{
				if (!agentModel.IsCrazy() && agentModel.currentSkill == null && ((!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.MALKUT, false) && !SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1)) || agentModel.GetState() != AgentAIState.MANAGE))
				{
					agentModel.ResetWaitingPassage();
				}
				else
				{
					agentModel.SetWaitingPassage(sefiraPassage);
					PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(sefiraPassage);
					if (passageObject != null)
					{
						passageObject.OnPointEnter();
						passageObject.OnPointerClick();
					}
				}
			}
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			Sefira sefira = SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2);
			if (sefira != null)
			{
				sefira.ReturnAgentsToSefira();
			}
		}
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x00036841 File Offset: 0x00034A41
	public OfficerSpecialAction GetRandomSpecialAction()
	{
		return this.officerSpecialAction.GetRandomAction();
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0003684E File Offset: 0x00034A4E
	public void ResetSpecaialAction(OfficerSpecialAction osa)
	{
		this.officerSpecialAction.ResetAction(osa);
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x00183C4C File Offset: 0x00181E4C
	private void UpdateOfficerAliveLevel()
	{
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			return;
		}
		int num = 0;
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead() && !officerModel.IsPanic())
			{
				num++;
			}
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			foreach (OfficerModel officerModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).officerList)
			{
				if (!officerModel2.IsDead() && !officerModel2.IsPanic())
				{
					num++;
				}
			}
		}
		int currentOfficerAliveLevel = this._currentOfficerAliveLevel;
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			if (num <= 0)
			{
				this._currentOfficerAliveLevel = 0;
			}
			else if (num <= this._maxOfficer * 8 / 10)
			{
				this._currentOfficerAliveLevel = 1;
			}
			else if (num <= this._maxOfficer * 16 / 10)
			{
				this._currentOfficerAliveLevel = 2;
			}
			else
			{
				this._currentOfficerAliveLevel = 3;
			}
		}
		else if (num <= 0)
		{
			this._currentOfficerAliveLevel = 0;
		}
		else if (num <= this._maxOfficer * 4 / 10)
		{
			this._currentOfficerAliveLevel = 1;
		}
		else if (num <= this._maxOfficer * 8 / 10)
		{
			this._currentOfficerAliveLevel = 2;
		}
		else
		{
			this._currentOfficerAliveLevel = 3;
		}
		SefiraEnum sefiraEnum = this.sefiraEnum;
		if (sefiraEnum != SefiraEnum.CHESED)
		{
			if (sefiraEnum == SefiraEnum.BINAH)
			{
				if (currentOfficerAliveLevel != this._currentOfficerAliveLevel && CreatureInfoWindow.CurrentWindow != null && CreatureInfoWindow.CurrentWindow.IsEnabled)
				{
					CreatureInfoWindow.CurrentWindow.OnBinahAbilityChanged();
				}
			}
		}
		else
		{
			GlobalBulletManager.instance.UpdateMaxBullet();
		}
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x00183E74 File Offset: 0x00182074
	private void UpdateAgentEmptyState()
	{
		int num = 0;
		foreach (AgentModel agentModel in this._enteredAgentList)
		{
			if (!agentModel.IsDead() && !agentModel.IsPanic())
			{
				num++;
				break;
			}
		}
		bool flag = false;
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead() && !officerModel.IsCrazy())
			{
				flag = true;
				break;
			}
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			foreach (OfficerModel officerModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).officerList)
			{
				if (!officerModel2.IsDead() && !officerModel2.IsCrazy())
				{
					flag = true;
					break;
				}
			}
		}
		if (num > 0 || !flag)
		{
			this._activatedEmptyCounter = false;
			this._agentEmptyElapsedTime = 0f;
		}
		else
		{
			this._activatedEmptyCounter = true;
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x00183FF8 File Offset: 0x001821F8
	private void UpdateAgentDeadState()
	{
		if (this._agentDeadPenaltyActivated)
		{
			return;
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			return;
		}
		if (SefiraBossManager.Instance.CurrentActivatedSefira == this.sefiraEnum)
		{
			return;
		}
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead())
			{
				return;
			}
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			foreach (AgentModel agentModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).agentList)
			{
				if (!agentModel2.IsDead())
				{
					return;
				}
			}
		}
		this.ActivateAgentDeadPanelty();
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x001840FC File Offset: 0x001822FC
	private void ActivateAgentEmptyPenalty()
	{
		this._activatedEmptyCounter = false;
		foreach (OfficerModel officerModel in this.officerList)
		{
			officerModel.PanicOfficer(true);
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			foreach (OfficerModel officerModel2 in SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).officerList)
			{
				officerModel2.PanicOfficer(true);
			}
		}
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x001841C0 File Offset: 0x001823C0
	private void ActivateAgentDeadPanelty()
	{
		this._agentDeadPenaltyActivated = true;
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead())
			{
				officerModel.PrepareToSuicide();
			}
		}
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.OnActivateAgentDeadPenalty();
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH1)
		{
			SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).ActivateAgentDeadPanelty();
		}
		foreach (PassageObjectModel passageObjectModel in this.passageList)
		{
			passageObjectModel.DisableSefira();
		}
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0003685C File Offset: 0x00034A5C
	public int GetOfficerAliveLevel()
	{
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			return SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH1).GetOfficerAliveLevel();
		}
		return Mathf.Clamp(this._currentOfficerAliveLevel, 0, 3);
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x001842E8 File Offset: 0x001824E8
	public void CheckAgentStateForEmergency()
	{
		int num = 0;
		int num2 = 0;
		int count = this.agentList.Count;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (agentModel.IsDead())
			{
				num2++;
			}
			else if (agentModel.IsPanic())
			{
				num++;
			}
		}
		int num3 = count;
		int num4 = (int)((float)num / (float)num3);
		int num5 = (int)((float)num2 / (float)count);
		if (this.panicRate < num4)
		{
			this.panicRate = num4;
			switch (num4)
			{
			case 4:
				this.SendEmergencyScore(5, true);
				break;
			case 6:
				this.SendEmergencyScore(10, true);
				break;
			case 8:
				this.SendEmergencyScore(20, true);
				break;
			case 10:
				this.SendEmergencyScore(30, true);
				break;
			}
		}
		else if (this.panicRate > num4)
		{
			this.panicRate = num4;
			switch (num4)
			{
			case 4:
				this.SendEmergencyScore(5, false);
				break;
			case 6:
				this.SendEmergencyScore(10, false);
				break;
			case 8:
				this.SendEmergencyScore(20, false);
				break;
			case 10:
				this.SendEmergencyScore(30, false);
				break;
			}
		}
		if (num5 == 8)
		{
			Debug.Log("80% agent dead");
			this.SendEmergencyScore(30, true);
		}
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0000425D File Offset: 0x0000245D
	public void SendEmergencyScore(int val, bool isAdd)
	{
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x001844A0 File Offset: 0x001826A0
	public AgentModel[] GetAgentInSefira()
	{
		List<AgentModel> list = new List<AgentModel>();
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead() && !agentModel.IsPanic())
			{
				if (agentModel.IsInSefira())
				{
					list.Add(agentModel);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x00184530 File Offset: 0x00182730
	public bool IsAgentInEsacpedCreaturePassage(AgentModel checkTarget, out CreatureModel targetCreature)
	{
		PassageObjectModel passage = checkTarget.GetMovableNode().GetPassage();
		if (passage != null)
		{
			foreach (CreatureModel creatureModel in this.escapedCreatures)
			{
				PassageObjectModel passage2 = creatureModel.GetMovableNode().GetPassage();
				if (creatureModel.script is RedShoes)
				{
					RedShoesSkill redShoesSkill = creatureModel.script.skill as RedShoesSkill;
					if (redShoesSkill.attractTargetAgent == null)
					{
						continue;
					}
					passage2 = redShoesSkill.attractTargetAgent.GetMovableNode().GetPassage();
				}
				if (passage2 == passage)
				{
					targetCreature = creatureModel;
					return true;
				}
			}
		}
		targetCreature = null;
		return false;
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x00184608 File Offset: 0x00182808
	public bool IsAgentInDeadAgentPassage(AgentModel checkTarget)
	{
		if (checkTarget.IsDead())
		{
			return false;
		}
		PassageObjectModel passage = checkTarget.GetMovableNode().GetPassage();
		if (passage != null)
		{
			foreach (AgentModel agentModel in this.agentList)
			{
				if (agentModel != checkTarget)
				{
					if (agentModel.IsDead())
					{
						PassageObjectModel passage2 = agentModel.GetMovableNode().GetPassage();
						if (passage2 == passage)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x00036888 File Offset: 0x00034A88
	public bool IsAnyCreatureEscaped()
	{
		return this.escapedCreatures.Count != 0;
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0003689B File Offset: 0x00034A9B
	public void OnSuppressedCreature(CreatureModel target)
	{
		if (this.escapedCreatures.Contains(target))
		{
			this.escapedCreatures.Remove(target);
		}
		if (SefiraManager.instance.CheckEscapedState())
		{
			GameStatusUI.GameStatusUI.Window.sceneController.IsCreatureEscaped = false;
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x000368DA File Offset: 0x00034ADA
	public bool CheckEscapedCreature()
	{
		return this.escapedCreatures.Count == 0;
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x000368EA File Offset: 0x00034AEA
	public void OnEscapeCreature(CreatureModel target)
	{
		if (!this.escapedCreatures.Contains(target))
		{
			this.escapedCreatures.Add(target);
		}
		GameStatusUI.GameStatusUI.Window.sceneController.IsCreatureEscaped = true;
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x00036919 File Offset: 0x00034B19
	public List<CreatureModel> GetEscapedCreatures()
	{
		return this.escapedCreatures;
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0000425D File Offset: 0x0000245D
	public void OnAgentGetPanic(AgentModel panicAgent)
	{
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x001846B8 File Offset: 0x001828B8
	public void OnAgentReturnControll()
	{
		int count = this.agentList.Count;
		bool flag = false;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead() && !agentModel.IsPanic() && !agentModel.IsCrazy())
			{
				flag = true;
				break;
			}
		}
		if (this.SefiraClosed && flag)
		{
			this.SefiraClosed = false;
			SefiraManager.instance.AddActivatedSefira(this);
			if (GameStatusUI.GameStatusUI.Window.sceneController.IsAgentAlldead)
			{
				GameStatusUI.GameStatusUI.Window.sceneController.IsAgentAlldead = false;
			}
		}
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x00184790 File Offset: 0x00182990
	public void OnAgentCannotControll(AgentModel deadAgent)
	{
		if (this.SefiraClosed)
		{
			return;
		}
		int num = this.agentList.Count;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (agentModel.IsDead() || agentModel.IsPanic() || agentModel.IsCrazy())
			{
				num--;
			}
			if (agentModel.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
			{
				num--;
			}
		}
		this.CheckAgentStateForEmergency();
		if (num == 0)
		{
			Debug.Log("Alldead");
			this.SefiraClosed = true;
			SefiraManager.instance.DisabledSefira(this);
			if (SefiraManager.instance.GameOverCheck())
			{
				if (!PlaySpeedSettingUI.instance.available)
				{
					PlaySpeedSettingUI.instance.ForcleyReleaseSetting();
				}
				AngelaConversation.instance.MakeMessage(AngelaMessageState.GAMEOVER, new object[]
				{
					this
				});
				if (!GameStatusUI.GameStatusUI.Window.sceneController.IsAgentAlldead)
				{
					GameStatusUI.GameStatusUI.Window.sceneController.IsAgentAlldead = true;
				}
				BgmManager.instance.BlockRecoverInf();
			}
		}
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x001848C8 File Offset: 0x00182AC8
	public bool CheckAgentControll()
	{
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead() && !agentModel.IsPanic() && !agentModel.IsCrazy())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x00184950 File Offset: 0x00182B50
	public bool CheckOfficerControll()
	{
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead() && !officerModel.IsPanic())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x00036921 File Offset: 0x00034B21
	public bool IsSefiraClosed()
	{
		return this.SefiraClosed;
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x001849CC File Offset: 0x00182BCC
	public int GetAliveAgentCnt()
	{
		int num = 0;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x00184A38 File Offset: 0x00182C38
	public int GetAliveOfficerCnt()
	{
		int num = 0;
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x00036929 File Offset: 0x00034B29
	public int GetCurrentAbilityLevel()
	{
		if (this.activated)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x00184AA4 File Offset: 0x00182CA4
	public void EnterAgent(MovableObjectNode unit)
	{
		AgentModel agentModel = unit.GetUnit() as AgentModel;
		if (agentModel == null)
		{
			return;
		}
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH1).EnterAgent(unit);
			return;
		}
		if (!this._enteredAgentList.Contains(agentModel))
		{
			this._enteredAgentList.Add(agentModel);
		}
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x00184B00 File Offset: 0x00182D00
	public void ExitAgent(MovableObjectNode unit)
	{
		AgentModel item = unit.GetUnit() as AgentModel;
		if (this.sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH1).ExitAgent(unit);
			return;
		}
		if (this._enteredAgentList.Contains(item))
		{
			this._enteredAgentList.Remove(item);
		}
	}

	// Token: 0x0400390F RID: 14607
	public const int OpenMin = 0;

	// Token: 0x04003910 RID: 14608
	public const int OpenMax = 5;

	// Token: 0x04003911 RID: 14609
	public string name;

	// Token: 0x04003912 RID: 14610
	public int index;

	// Token: 0x04003913 RID: 14611
	public string indexString;

	// Token: 0x04003914 RID: 14612
	private bool _activated;

	// Token: 0x04003915 RID: 14613
	public List<PassageObjectModel> departPassageList;

	// Token: 0x04003916 RID: 14614
	public List<PassageObjectModel> openedDepartmentList;

	// Token: 0x04003917 RID: 14615
	public List<PassageObjectModel> passageList;

	// Token: 0x04003918 RID: 14616
	public List<PassageObjectModel> connectedPassageList;

	// Token: 0x04003919 RID: 14617
	public PassageObjectModel sefiraPassage;

	// Token: 0x0400391A RID: 14618
	public List<OfficerModel> officerList;

	// Token: 0x0400391B RID: 14619
	public List<AgentModel> agentList;

	// Token: 0x0400391C RID: 14620
	public List<CreatureModel> creatureList;

	// Token: 0x0400391D RID: 14621
	private bool SefiraClosed;

	// Token: 0x0400391E RID: 14622
	public OfficerSpecialActionList officerSpecialAction = new OfficerSpecialActionList();

	// Token: 0x0400391F RID: 14623
	private List<CreatureModel> escapedCreatures;

	// Token: 0x04003920 RID: 14624
	private CreatureModel[] creatureAry;

	// Token: 0x04003921 RID: 14625
	private bool[] isWorking;

	// Token: 0x04003922 RID: 14626
	private List<int> idleList;

	// Token: 0x04003923 RID: 14627
	private List<int> workingList;

	// Token: 0x04003924 RID: 14628
	private int officerCnt;

	// Token: 0x04003925 RID: 14629
	private List<PassageObjectModel> _checkPassages = new List<PassageObjectModel>();

	// Token: 0x04003926 RID: 14630
	private List<PassageObjectModel> _recoverPassages = new List<PassageObjectModel>();

	// Token: 0x04003927 RID: 14631
	private List<PassageObjectModel> _recoverAdditionalPassages = new List<PassageObjectModel>();

	// Token: 0x04003928 RID: 14632
	public SefiraIsolateManagement isolateManagement;

	// Token: 0x04003929 RID: 14633
	private SefiraEnum _sefiraEnum = SefiraEnum.DUMMY;

	// Token: 0x0400392A RID: 14634
	private int panicRate;

	// Token: 0x0400392B RID: 14635
	private int deadRate;

	// Token: 0x0400392C RID: 14636
	private int _openLevel;

	// Token: 0x0400392D RID: 14637
	private int _allocateMax = 3;

	// Token: 0x0400392E RID: 14638
	private int _maxOfficer = 10;

	// Token: 0x0400392F RID: 14639
	public bool isRecoverActivated;

	// Token: 0x04003930 RID: 14640
	public const float recoverTime = 10f;

	// Token: 0x04003931 RID: 14641
	private float _recoverElapsedTime;

	// Token: 0x04003932 RID: 14642
	private bool _recoverSpeedUp;

	// Token: 0x04003933 RID: 14643
	private bool _recoverUpgrade;

	// Token: 0x04003934 RID: 14644
	private Timer _abilityCheckTimer = new Timer();

	// Token: 0x04003935 RID: 14645
	private int _currentOfficerAliveLevel;

	// Token: 0x04003936 RID: 14646
	private Timer _agentCheckTimer = new Timer();

	// Token: 0x04003937 RID: 14647
	private bool _activatedEmptyCounter;

	// Token: 0x04003938 RID: 14648
	private float _agentEmptyElapsedTime;

	// Token: 0x04003939 RID: 14649
	private bool _agentDeadPenaltyActivated;

	// Token: 0x0400393A RID: 14650
	public const float agentDeadPenalty = 50f;

	// Token: 0x0400393B RID: 14651
	private Timer _agentDeadCheckTimer = new Timer();

	// Token: 0x0400393C RID: 14652
	private List<AgentModel> _enteredAgentList = new List<AgentModel>();
}
