using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
public class Mission
{
	// Token: 0x060037A2 RID: 14242 RVA: 0x00167070 File Offset: 0x00165270
	public Mission()
	{
		this.conditions = new List<Condition>();
		this.successCondition = new Condition();
		this.baseConditions = new List<Condition>();
		this.failConditions = new List<Condition>();
		this.checkedObjects = new List<object>();
		this.doneConditions = new List<bool>();
		this.isCleared = false;
		this.isInProcess = false;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0003230C File Offset: 0x0003050C
	public Mission(MissionTypeInfo metadata)
	{
		this.metaInfo = metadata;
		this.Init(metadata);
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x060037A4 RID: 14244 RVA: 0x00032322 File Offset: 0x00030522
	public string sefira_Name
	{
		get
		{
			return this.metaInfo.sefira_Name;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0003232F File Offset: 0x0003052F
	public bool isGlobal
	{
		get
		{
			return this.metaInfo.isGlobal;
		}
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x001670D4 File Offset: 0x001652D4
	public void Init(MissionTypeInfo metadata)
	{
		this.conditions = new List<Condition>();
		this.successCondition = new Condition();
		this.failConditions = new List<Condition>();
		this.baseConditions = new List<Condition>();
		this.checkedObjects = new List<object>();
		this.doneConditions = new List<bool>();
		this.isCleared = false;
		this.isInProcess = false;
		foreach (MissionConditionTypeInfo missionConditionTypeInfo in metadata.conditions)
		{
			Condition cond = new Condition();
			cond.metaInfo = missionConditionTypeInfo;
			if (!this.conditions.Exists((Condition x) => x.index == cond.metaInfo.index))
			{
				this.doneConditions.Add(true);
			}
			this.conditions.Add(cond);
			switch (cond.condition_Category)
			{
			case ConditionCategory.WORK_CONDITION:
			case ConditionCategory.AGENT_CONDITION:
			case ConditionCategory.CREATURE_CONDITION:
				this.baseConditions.Add(cond);
				break;
			case ConditionCategory.FAIL_CONDITION:
				this.failConditions.Add(cond);
				break;
			case ConditionCategory.ACTION_CONDITION:
				this.successCondition = cond;
				break;
			}
		}
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x00167238 File Offset: 0x00165438
	public void LoadData(Dictionary<string, object> dic)
	{
		int id = 0;
		GameUtil.TryGetValue<int>(dic, "metadataId", ref id);
		this.metaInfo = MissionTypeList.instance.GetData(id);
		if (this.metaInfo == null)
		{
			Debug.LogError("mission metadata is not found");
		}
		this.Init(this.metaInfo);
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x00167288 File Offset: 0x00165488
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"metadataId",
				this.metaInfo.id
			}
		};
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x001672B8 File Offset: 0x001654B8
	public void OnEnabled()
	{
		this.checkedObjects.Clear();
		this.isCleared = false;
		this.isInProcess = true;
		foreach (Condition condition in this.conditions)
		{
			condition.current = 0;
		}
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0000425D File Offset: 0x0000245D
	public void OnDisabled()
	{
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x00167330 File Offset: 0x00165530
	public void CheckConditions(string notice, params object[] param)
	{
		if (!this.isInProcess)
		{
			return;
		}
		if (this.isCleared)
		{
			return;
		}
		if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
		{
			return;
		}
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated() && this.successCondition.condition_Type != ConditionType.DESTROY_CORE)
		{
			return;
		}
		for (int i = 0; i < this.doneConditions.Count; i++)
		{
			this.doneConditions[i] = false;
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			this.baseConditions[j].current = 0;
		}
		if (notice == NoticeName.OnReleaseWork)
		{
			if (this.successCondition.condition_Type == ConditionType.CLEAR_WITH_AGENT_BY_CONDITION)
			{
				List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
				int num = 0;
				foreach (AgentModel agentModel in list)
				{
					if (!agentModel.IsDead())
					{
						if (!agentModel.IsPanic())
						{
							if (this.CheckAgent(agentModel))
							{
								num++;
							}
						}
					}
				}
				this.successCondition.current = num;
			}
			if (param.Length == 0)
			{
				return;
			}
			CreatureModel creatureModel = param[0] as CreatureModel;
			if (creatureModel == null)
			{
				return;
			}
			if (!this.isGlobal && !creatureModel.sefira.name.Contains(this.sefira_Name))
			{
				return;
			}
			if (!this.CheckCreature(creatureModel))
			{
				return;
			}
			if (creatureModel.currentSkill == null)
			{
				return;
			}
			if (!this.CheckWork(creatureModel.currentSkill))
			{
				return;
			}
			AgentModel agent = creatureModel.currentSkill.agent;
			if (!this.CheckAgent(agent))
			{
				return;
			}
			bool flag = true;
			for (int k = 0; k < this.failConditions.Count; k++)
			{
				if (this.failConditions[k].condition_Type == ConditionType.WORK_BAD && creatureModel.currentSkill.GetCurrentFeelingState() == CreatureFeelingState.BAD)
				{
					this.failConditions[k].current++;
				}
			}
			if (this.successCondition.condition_Type == ConditionType.WORK)
			{
				for (int l = 0; l < this.failConditions.Count; l++)
				{
					ConditionType condition_Type = this.failConditions[l].condition_Type;
					if (condition_Type != ConditionType.AGENT_DEAD)
					{
						if (condition_Type != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid failCondition");
						}
						else if (creatureModel.currentSkill.agent.IsPanic())
						{
							this.failConditions[l].current++;
						}
					}
					else if (creatureModel.currentSkill.agent.IsDead())
					{
						this.failConditions[l].current++;
					}
					if (!this.doneConditions[this.failConditions[l].index])
					{
						this.doneConditions[this.failConditions[l].index] = this.CheckDefault(this.failConditions[l]);
					}
				}
				for (int m = 0; m < this.failConditions.Count; m++)
				{
					if (!this.doneConditions[this.failConditions[m].index])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.successCondition.current++;
					this.CheckSuccess();
				}
			}
		}
		else if (notice == NoticeName.OnStageEnd)
		{
			if (this.successCondition.condition_Type == ConditionType.CLEAR_DAY)
			{
				List<History> list2 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list3 = new List<History>();
				bool flag2 = true;
				for (int n = 0; n < this.failConditions.Count; n++)
				{
					ConditionType condition_Type2 = this.failConditions[n].condition_Type;
					if (condition_Type2 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type2 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list3.AddRange(list2.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num2 = 0; num2 < list3.Count; num2++)
							{
								if (this.isGlobal || list3[num2].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list3[num2].GetWorker()))
									{
										this.failConditions[n].current++;
									}
								}
							}
						}
					}
					else
					{
						list3.AddRange(list2.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num3 = 0; num3 < list3.Count; num3++)
						{
							if (this.isGlobal || list3[num3].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list3[num3].GetWorker()))
								{
									this.failConditions[n].current++;
								}
							}
						}
					}
					list3.Clear();
					if (!this.doneConditions[this.failConditions[n].index])
					{
						this.doneConditions[this.failConditions[n].index] = this.CheckDefault(this.failConditions[n]);
					}
				}
				for (int num4 = 0; num4 < this.failConditions.Count; num4++)
				{
					if (!this.doneConditions[this.failConditions[num4].index])
					{
						flag2 = false;
						break;
					}
				}
				if (flag2 && this.successCondition.condition_Type == ConditionType.CLEAR_DAY)
				{
					this.successCondition.current++;
					this.CheckSuccess();
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.CLEAR_TIME)
			{
				List<History> list4 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list5 = new List<History>();
				bool flag3 = true;
				for (int num5 = 0; num5 < this.failConditions.Count; num5++)
				{
					ConditionType condition_Type3 = this.failConditions[num5].condition_Type;
					if (condition_Type3 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type3 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list5.AddRange(list4.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num6 = 0; num6 < list5.Count; num6++)
							{
								if (this.isGlobal || list5[num6].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list5[num6].GetWorker()))
									{
										this.failConditions[num5].current++;
									}
								}
							}
						}
					}
					else
					{
						list5.AddRange(list4.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num7 = 0; num7 < list5.Count; num7++)
						{
							if (this.isGlobal || list5[num7].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list5[num7].GetWorker()))
								{
									this.failConditions[num5].current++;
								}
							}
						}
					}
					list5.Clear();
					if (!this.doneConditions[this.failConditions[num5].index])
					{
						this.doneConditions[this.failConditions[num5].index] = this.CheckDefault(this.failConditions[num5]);
					}
				}
				for (int num8 = 0; num8 < this.failConditions.Count; num8++)
				{
					if (!this.doneConditions[this.failConditions[num8].index])
					{
						flag3 = false;
						break;
					}
				}
				if (flag3 && this.successCondition.condition_Type == ConditionType.CLEAR_TIME)
				{
					this.successCondition.current = (int)GlobalHistory.instance.GetCurrentTime();
					this.CheckSuccess();
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.QLIPHOTH_OVERLOAD)
			{
				List<History> list6 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list7 = new List<History>();
				bool flag4 = true;
				for (int num9 = 0; num9 < this.failConditions.Count; num9++)
				{
					ConditionType condition_Type4 = this.failConditions[num9].condition_Type;
					if (condition_Type4 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type4 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list7.AddRange(list6.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num10 = 0; num10 < list7.Count; num10++)
							{
								if (this.isGlobal || list7[num10].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list7[num10].GetWorker()))
									{
										this.failConditions[num9].current++;
									}
								}
							}
						}
					}
					else
					{
						list7.AddRange(list6.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num11 = 0; num11 < list7.Count; num11++)
						{
							if (this.isGlobal || list7[num11].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list7[num11].GetWorker()))
								{
									this.failConditions[num9].current++;
								}
							}
						}
					}
					list7.Clear();
					if (!this.doneConditions[this.failConditions[num9].index])
					{
						this.doneConditions[this.failConditions[num9].index] = this.CheckDefault(this.failConditions[num9]);
					}
				}
				for (int num12 = 0; num12 < this.failConditions.Count; num12++)
				{
					if (!this.doneConditions[this.failConditions[num12].index])
					{
						flag4 = false;
						break;
					}
				}
				if (flag4 && this.successCondition.condition_Type == ConditionType.QLIPHOTH_OVERLOAD)
				{
					this.successCondition.current = CreatureOverloadManager.instance.GetQliphothOverloadLevel();
					this.CheckSuccess();
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.ISOLATE_OVERLOAD)
			{
				List<History> list8 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list9 = new List<History>();
				bool flag5 = true;
				for (int num13 = 0; num13 < this.failConditions.Count; num13++)
				{
					ConditionType condition_Type5 = this.failConditions[num13].condition_Type;
					if (condition_Type5 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type5 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list9.AddRange(list8.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num14 = 0; num14 < list9.Count; num14++)
							{
								if (this.isGlobal || list9[num14].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list9[num14].GetWorker()))
									{
										this.failConditions[num13].current++;
									}
								}
							}
						}
					}
					else
					{
						list9.AddRange(list8.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num15 = 0; num15 < list9.Count; num15++)
						{
							if (this.isGlobal || list9[num15].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list9[num15].GetWorker()))
								{
									this.failConditions[num13].current++;
								}
							}
						}
					}
					list9.Clear();
					if (!this.doneConditions[this.failConditions[num13].index])
					{
						this.doneConditions[this.failConditions[num13].index] = this.CheckDefault(this.failConditions[num13]);
					}
				}
				for (int num16 = 0; num16 < this.failConditions.Count; num16++)
				{
					if (!this.doneConditions[this.failConditions[num16].index])
					{
						flag5 = false;
						break;
					}
				}
				if (flag5 && this.successCondition.condition_Type == ConditionType.ISOLATE_OVERLOAD)
				{
					this.CheckSuccess();
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.CREATURES_IN_CONDITION_ALL)
			{
				List<CreatureModel> list10 = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
				bool flag6 = true;
				foreach (CreatureModel creatureModel2 in list10)
				{
					if (!(creatureModel2.script is DontTouchMe))
					{
						if (!this.CheckCreature(creatureModel2))
						{
							flag6 = false;
							break;
						}
					}
				}
				if (flag6)
				{
					this.successCondition.current = 1;
					this.CheckSuccess();
				}
				else
				{
					this.successCondition.current = 0;
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.EQUIPMENTS_IN_CONDITION)
			{
				List<EquipmentModel> list11 = new List<EquipmentModel>(InventoryModel.Instance.GetAllEquipmentList());
				bool flag7 = true;
				int num17 = 0;
				foreach (EquipmentModel equip in list11)
				{
					if (this.CheckEquipment(equip))
					{
						num17++;
					}
				}
				if (flag7)
				{
					this.successCondition.current = num17;
					this.CheckSuccess();
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.CLEAR_WITH_AGENT_BY_CONDITION)
			{
				List<AgentModel> list12 = new List<AgentModel>(AgentManager.instance.GetAgentList());
				int num18 = 0;
				foreach (AgentModel agentModel2 in list12)
				{
					if (!agentModel2.IsDead())
					{
						if (!agentModel2.IsPanic())
						{
							if (this.CheckAgent(agentModel2))
							{
								num18++;
							}
						}
					}
				}
				this.successCondition.current = num18;
				this.CheckSuccess();
			}
		}
		else if (notice == NoticeName.OnCreatureSuppressed)
		{
			if (this.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE_BY_KIND || this.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE)
			{
				CreatureModel creatureModel3 = param[0] as CreatureModel;
				if (creatureModel3 == null)
				{
					return;
				}
				if (creatureModel3.hp > 0f && !(creatureModel3.script is RedShoes) && !(creatureModel3.script is PinkCorps))
				{
					return;
				}
				if (creatureModel3 is OrdealCreatureModel)
				{
					return;
				}
				if (creatureModel3 is ChildCreatureModel)
				{
					return;
				}
				if (!this.isGlobal && !creatureModel3.sefira.name.Contains(this.sefira_Name))
				{
					return;
				}
				if (!this.CheckCreature(creatureModel3))
				{
					return;
				}
				if (this.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE_BY_KIND && this.checkedObjects.Contains(creatureModel3))
				{
					return;
				}
				List<History> list13 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list14 = new List<History>();
				bool flag8 = true;
				for (int num19 = list13.Count - 1; num19 >= 0; num19--)
				{
					if (list13[num19].GetHistoryType() == History.HistoryType.CREATURE_ESCAPE && list13[num19].GetCreature() == creatureModel3)
					{
						break;
					}
					list14.Add(list13[num19]);
				}
				for (int num20 = 0; num20 < this.failConditions.Count; num20++)
				{
					ConditionType condition_Type6 = this.failConditions[num20].condition_Type;
					if (condition_Type6 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type6 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							for (int num21 = 0; num21 < list14.Count; num21++)
							{
								if (list14[num21].GetHistoryType() == History.HistoryType.WORKER_PANIC && creatureModel3.encounteredWorker.Contains(list14[num21].GetWorker()) && this.CheckAgent(list14[num21].GetWorker()))
								{
									this.failConditions[num20].current++;
								}
							}
						}
					}
					else
					{
						for (int num22 = 0; num22 < list14.Count; num22++)
						{
							if (list14[num22].GetHistoryType() == History.HistoryType.WORKER_DIE && creatureModel3.encounteredWorker.Contains(list14[num22].GetWorker()) && this.CheckAgent(list14[num22].GetWorker()))
							{
								this.failConditions[num20].current++;
							}
						}
					}
					list14.Clear();
					if (!this.doneConditions[this.failConditions[num20].index])
					{
						this.doneConditions[this.failConditions[num20].index] = this.CheckDefault(this.failConditions[num20]);
					}
				}
				for (int num23 = 0; num23 < this.failConditions.Count; num23++)
				{
					if (!this.doneConditions[this.failConditions[num23].index])
					{
						flag8 = false;
						break;
					}
				}
				if (flag8 && (this.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE_BY_KIND || this.successCondition.condition_Type == ConditionType.SUPPRESS_CREATURE))
				{
					if (!this.checkedObjects.Contains(creatureModel3))
					{
						this.checkedObjects.Add(creatureModel3);
					}
					this.successCondition.current++;
					this.CheckSuccess();
				}
			}
		}
		else if (notice == NoticeName.OnAgentPromote)
		{
			if (this.successCondition.condition_Type == ConditionType.PROMOTE_AGENT)
			{
				AgentModel agentModel3 = param[0] as AgentModel;
				List<History> list15 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list16 = new List<History>();
				bool flag9 = true;
				for (int num24 = 0; num24 < this.failConditions.Count; num24++)
				{
					ConditionType condition_Type7 = this.failConditions[num24].condition_Type;
					if (condition_Type7 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type7 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list16.AddRange(list15.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num25 = 0; num25 < list16.Count; num25++)
							{
								if (this.isGlobal || list16[num25].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list16[num25].GetWorker()))
									{
										this.failConditions[num24].current++;
									}
								}
							}
						}
					}
					else
					{
						list16.AddRange(list15.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num26 = 0; num26 < list16.Count; num26++)
						{
							if (this.isGlobal || list16[num26].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list16[num26].GetWorker()))
								{
									this.failConditions[num24].current++;
								}
							}
						}
					}
					list16.Clear();
					if (!this.doneConditions[this.failConditions[num24].index])
					{
						this.doneConditions[this.failConditions[num24].index] = this.CheckDefault(this.failConditions[num24]);
					}
				}
				for (int num27 = 0; num27 < this.failConditions.Count; num27++)
				{
					if (!this.doneConditions[this.failConditions[num27].index])
					{
						flag9 = false;
						break;
					}
				}
				if (flag9 && this.successCondition.condition_Type == ConditionType.PROMOTE_AGENT)
				{
					if (agentModel3.IsDead())
					{
						return;
					}
					if (this.CheckAgent(agentModel3))
					{
						this.successCondition.current++;
					}
					this.CheckSuccess();
				}
			}
		}
		else if (notice == NoticeName.OnQliphothOverloadLevelChanged)
		{
			if (this.successCondition.condition_Type == ConditionType.QLIPHOTH_OVERLOAD)
			{
				List<History> list17 = new List<History>(GlobalHistory.instance.GetHistory());
				List<History> list18 = new List<History>();
				bool flag10 = true;
				for (int num28 = 0; num28 < this.failConditions.Count; num28++)
				{
					ConditionType condition_Type8 = this.failConditions[num28].condition_Type;
					if (condition_Type8 != ConditionType.AGENT_DEAD)
					{
						if (condition_Type8 != ConditionType.AGENT_PANIC)
						{
							Debug.Log("Invalid fail condition");
						}
						else
						{
							list18.AddRange(list17.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_PANIC));
							for (int num29 = 0; num29 < list18.Count; num29++)
							{
								if (this.isGlobal || list18[num29].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
								{
									if (this.CheckAgent(list18[num29].GetWorker()))
									{
										this.failConditions[num28].current++;
									}
								}
							}
						}
					}
					else
					{
						list18.AddRange(list17.FindAll((History x) => x.GetHistoryType() == History.HistoryType.WORKER_DIE));
						for (int num30 = 0; num30 < list18.Count; num30++)
						{
							if (this.isGlobal || list18[num30].GetWorker().GetCurrentSefira().name.Contains(this.sefira_Name))
							{
								if (this.CheckAgent(list18[num30].GetWorker()))
								{
									this.failConditions[num28].current++;
								}
							}
						}
					}
					list18.Clear();
					if (!this.doneConditions[this.failConditions[num28].index])
					{
						this.doneConditions[this.failConditions[num28].index] = this.CheckDefault(this.failConditions[num28]);
					}
				}
				for (int num31 = 0; num31 < this.failConditions.Count; num31++)
				{
					if (!this.doneConditions[this.failConditions[num31].index])
					{
						flag10 = false;
						break;
					}
				}
				if (flag10 && this.successCondition.condition_Type == ConditionType.QLIPHOTH_OVERLOAD)
				{
					if (param[0] != null && param[0] is int)
					{
						this.successCondition.current = (int)param[0];
					}
					else
					{
						this.successCondition.current = CreatureOverloadManager.instance.GetQliphothOverloadLevel();
					}
				}
			}
		}
		else if (notice == NoticeName.OnIsolateOverloaded)
		{
			if (this.successCondition.condition_Type == ConditionType.ISOLATE_OVERLOAD)
			{
				if (param.Length <= 0)
				{
					return;
				}
				CreatureModel creatureModel4 = param[0] as CreatureModel;
				if (creatureModel4 == null)
				{
					return;
				}
				if (!this.isGlobal && !creatureModel4.sefira.name.Contains(this.sefira_Name))
				{
					return;
				}
				if (!this.CheckCreature(creatureModel4))
				{
					return;
				}
				this.successCondition.current++;
			}
		}
		else if (notice == NoticeName.WorkToOverloaded)
		{
			if (this.successCondition.condition_Type == ConditionType.WORK_TO_OVERLOADED)
			{
				if (param.Length <= 0)
				{
					return;
				}
				CreatureModel creatureModel5 = param[0] as CreatureModel;
				if (creatureModel5 == null)
				{
					return;
				}
				if (!this.isGlobal && !creatureModel5.sefira.name.Contains(this.sefira_Name))
				{
					return;
				}
				this.successCondition.current++;
				this.CheckSuccess();
			}
		}
		else if (notice == NoticeName.OrdealEnd)
		{
			if (this.successCondition.condition_Type == ConditionType.ORDEAL_END)
			{
				if (param.Length <= 0)
				{
					return;
				}
				OrdealBase ordealBase = param[0] as OrdealBase;
				if (!(bool)param[1])
				{
					return;
				}
				if (ordealBase == null)
				{
					return;
				}
				if (!this.CheckOrdeal(ordealBase))
				{
					return;
				}
				this.successCondition.current++;
				this.CheckSuccess();
			}
		}
		else if (notice == NoticeName.OnStageStart)
		{
			if (this.successCondition.condition_Type == ConditionType.AGENT_PER_SEFIRA)
			{
				Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
				int num32 = 0;
				if (this.successCondition.goal_Type == GoalType.MIN)
				{
					num32 = 6;
				}
				foreach (Sefira sefira in openedAreaList)
				{
					List<AgentModel> list19 = new List<AgentModel>(sefira.agentList);
					int num34 = 0;
					foreach (AgentModel agent2 in list19)
					{
						if (this.CheckAgent(agent2))
						{
							num34++;
						}
					}
					GoalType goal_Type = this.successCondition.goal_Type;
					if (goal_Type != GoalType.MAX)
					{
						if (goal_Type != GoalType.MIN)
						{
							if (goal_Type == GoalType.SAME)
							{
								if (num34 != this.successCondition.goal)
								{
									num32 = 1;
								}
							}
						}
						else if (num34 < num32)
						{
							num32 = num34;
						}
					}
					else if (num34 > num32)
					{
						num32 = num34;
					}
				}
				this.successCondition.current = num32;
				this.CheckSuccess();
			}
			else if (this.successCondition.condition_Type == ConditionType.CLEAR_WITH_AGENT_BY_CONDITION)
			{
				List<AgentModel> list20 = new List<AgentModel>(AgentManager.instance.GetAgentList());
				int num35 = 0;
				foreach (AgentModel agentModel4 in list20)
				{
					if (!agentModel4.IsDead())
					{
						if (!agentModel4.IsPanic())
						{
							if (this.CheckAgent(agentModel4))
							{
								num35++;
							}
						}
					}
				}
				this.successCondition.current = num35;
			}
			else if (this.successCondition.condition_Type == ConditionType.EQUIPMENTS_IN_CONDITION)
			{
				List<EquipmentModel> list21 = new List<EquipmentModel>(InventoryModel.Instance.GetAllEquipmentList());
				bool flag11 = true;
				int num36 = 0;
				foreach (EquipmentModel equip2 in list21)
				{
					if (this.CheckEquipment(equip2))
					{
						num36++;
					}
				}
				if (flag11)
				{
					this.successCondition.current = num36;
				}
			}
			else if (this.successCondition.condition_Type == ConditionType.CREATURES_IN_CONDITION_ALL)
			{
				List<CreatureModel> list22 = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
				bool flag12 = true;
				foreach (CreatureModel creatureModel6 in list22)
				{
					if (!(creatureModel6.script is DontTouchMe))
					{
						if (!this.CheckCreature(creatureModel6))
						{
							flag12 = false;
							break;
						}
					}
				}
				if (flag12)
				{
					this.successCondition.current = 1;
				}
				else
				{
					this.successCondition.current = 0;
				}
			}
		}
		else if (notice == NoticeName.MakeEquipment)
		{
			if (this.successCondition.condition_Type == ConditionType.MAKE_EQUIPMENT)
			{
				if (param.Length <= 0)
				{
					return;
				}
				EquipmentModel equipmentModel = param[0] as EquipmentModel;
				if (equipmentModel == null)
				{
					return;
				}
				if (!this.CheckEquipment(equipmentModel))
				{
					return;
				}
				this.successCondition.current++;
				this.CheckSuccess();
			}
			else if (this.successCondition.condition_Type == ConditionType.EQUIPMENTS_IN_CONDITION)
			{
				List<EquipmentModel> list23 = new List<EquipmentModel>(InventoryModel.Instance.GetAllEquipmentList());
				bool flag13 = true;
				int num37 = 0;
				foreach (EquipmentModel equip3 in list23)
				{
					if (this.CheckEquipment(equip3))
					{
						num37++;
					}
				}
				if (flag13)
				{
					this.successCondition.current = num37;
				}
			}
		}
		else
		{
			if (notice == NoticeName.OnNextDay)
			{
				if (!this.CheckSuccess())
				{
					this.isInProcess = false;
					this.isCleared = false;
					this.OnDisabled();
				}
				return;
			}
			if (notice == NoticeName.OnDestroyBossCore)
			{
				if (this.successCondition.condition_Type == ConditionType.DESTROY_CORE)
				{
					SefiraEnum sefiraEnum = (SefiraEnum)param[0];
					if (sefiraEnum == this.metaInfo.sefira)
					{
						this.successCondition.current++;
					}
					this.CheckSuccess();
				}
			}
			else if (notice == NoticeName.OnAgentDead || notice == NoticeName.OnAgentPanic || notice == NoticeName.OnAgentPanicReturn)
			{
				if (this.successCondition.condition_Type == ConditionType.CLEAR_WITH_AGENT_BY_CONDITION)
				{
					List<AgentModel> list24 = new List<AgentModel>(AgentManager.instance.GetAgentList());
					int num38 = 0;
					foreach (AgentModel agentModel5 in list24)
					{
						if (!agentModel5.IsDead())
						{
							if (!agentModel5.IsPanic())
							{
								if (this.CheckAgent(agentModel5))
								{
									num38++;
								}
							}
						}
					}
					this.successCondition.current = num38;
				}
			}
			else if (notice == NoticeName.RemoveEquipment)
			{
				if (this.successCondition.condition_Type == ConditionType.EQUIPMENTS_IN_CONDITION)
				{
					List<EquipmentModel> list25 = new List<EquipmentModel>(InventoryModel.Instance.GetAllEquipmentList());
					bool flag14 = true;
					int num39 = 0;
					foreach (EquipmentModel equip4 in list25)
					{
						if (this.CheckEquipment(equip4))
						{
							num39++;
						}
					}
					if (flag14)
					{
						this.successCondition.current = num39;
					}
				}
			}
			else if (notice == NoticeName.CreatureObserveLevelAdded && this.successCondition.condition_Type == ConditionType.CREATURES_IN_CONDITION_ALL)
			{
				List<CreatureModel> list26 = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
				bool flag15 = true;
				foreach (CreatureModel creatureModel7 in list26)
				{
					if (!(creatureModel7.script is DontTouchMe))
					{
						if (!this.CheckCreature(creatureModel7))
						{
							flag15 = false;
							break;
						}
					}
				}
				if (flag15)
				{
					this.successCondition.current = 1;
				}
				else
				{
					this.successCondition.current = 0;
				}
			}
		}
		Notice.instance.Send(NoticeName.OnMissionProgressed, new object[]
		{
			this
		});
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x00169728 File Offset: 0x00167928
	private bool CheckSuccess()
	{
		bool flag = this.CheckDefault(this.successCondition);
		if (flag)
		{
			this.isCleared = true;
			this.OnDisabled();
			return true;
		}
		return false;
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x00169758 File Offset: 0x00167958
	private bool CheckDefault(Condition condition)
	{
		GoalType goal_Type = condition.goal_Type;
		if (goal_Type == GoalType.MAX)
		{
			return condition.goal >= condition.current;
		}
		if (goal_Type != GoalType.MIN)
		{
			return goal_Type == GoalType.SAME && condition.goal == condition.current;
		}
		return condition.goal <= condition.current;
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x001697BC File Offset: 0x001679BC
	private bool CheckWork(UseSkill skill)
	{
		for (int i = 0; i < this.baseConditions.Count; i++)
		{
			if (this.baseConditions[i].condition_Category == ConditionCategory.WORK_CONDITION)
			{
				this.doneConditions[this.baseConditions[i].index] = false;
				this.baseConditions[i].current = 0;
			}
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			if (this.baseConditions[j].condition_Category == ConditionCategory.WORK_CONDITION)
			{
				ConditionType condition_Type = this.baseConditions[j].condition_Type;
				if (condition_Type != ConditionType.WORK_RESULT)
				{
					if (condition_Type == ConditionType.WORK_TYPE)
					{
						this.baseConditions[j].current = (int)skill.skillTypeInfo.id;
					}
				}
				else
				{
					this.baseConditions[j].current = (int)skill.targetCreature.feelingState;
				}
				if (!this.doneConditions[this.baseConditions[j].index])
				{
					this.doneConditions[this.baseConditions[j].index] = this.CheckDefault(this.baseConditions[j]);
				}
			}
		}
		for (int k = 0; k < this.baseConditions.Count; k++)
		{
			if (this.baseConditions[k].condition_Category == ConditionCategory.WORK_CONDITION && !this.doneConditions[this.baseConditions[k].index])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x00169970 File Offset: 0x00167B70
	private bool CheckCreature(CreatureModel creature)
	{
		for (int i = 0; i < this.baseConditions.Count; i++)
		{
			if (this.baseConditions[i].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				this.doneConditions[this.baseConditions[i].index] = false;
				this.baseConditions[i].current = 0;
			}
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			if (this.baseConditions[j].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				ConditionType condition_Type = this.baseConditions[j].condition_Type;
				if (condition_Type != ConditionType.CREATURE_LEVEL)
				{
					if (condition_Type == ConditionType.CREATURE_OBSERVATION)
					{
						this.baseConditions[j].current = creature.observeInfo.GetObservationLevel();
					}
				}
				else if (creature.script is PinkCorps)
				{
					this.baseConditions[j].current = 5;
				}
				else
				{
					this.baseConditions[j].current = creature.GetRiskLevel();
				}
				if (!this.doneConditions[this.baseConditions[j].index])
				{
					this.doneConditions[this.baseConditions[j].index] = this.CheckDefault(this.baseConditions[j]);
				}
			}
		}
		for (int k = 0; k < this.baseConditions.Count; k++)
		{
			if (this.baseConditions[k].condition_Category == ConditionCategory.CREATURE_CONDITION && !this.doneConditions[this.baseConditions[k].index])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x00169B48 File Offset: 0x00167D48
	private bool CheckAgent(AgentModel agent)
	{
		for (int i = 0; i < this.baseConditions.Count; i++)
		{
			if (this.baseConditions[i].condition_Category == ConditionCategory.AGENT_CONDITION)
			{
				this.doneConditions[this.baseConditions[i].index] = false;
				this.baseConditions[i].current = 0;
			}
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			if (this.baseConditions[j].condition_Category == ConditionCategory.AGENT_CONDITION)
			{
				switch (this.baseConditions[j].condition_Type)
				{
				case ConditionType.AGENT_STAT:
				{
					int current = 0;
					switch (this.baseConditions[j].stat)
					{
					case 1:
						current = agent.fortitudeStat;
						break;
					case 2:
						current = agent.prudenceStat;
						break;
					case 3:
						current = agent.temperanceStat;
						break;
					case 4:
						current = agent.justiceStat;
						break;
					default:
						Debug.Log("Invalid stat");
						break;
					}
					this.baseConditions[j].current = current;
					break;
				}
				case ConditionType.AGENT_LEVEL:
					this.baseConditions[j].current = agent.level;
					break;
				case ConditionType.EGO_GIFT_COUNT:
					this.baseConditions[j].current = agent.Equipment.gifts.CountGifts();
					break;
				}
				if (!this.doneConditions[this.baseConditions[j].index])
				{
					this.doneConditions[this.baseConditions[j].index] = this.CheckDefault(this.baseConditions[j]);
				}
			}
		}
		for (int k = 0; k < this.baseConditions.Count; k++)
		{
			if (this.baseConditions[k].condition_Category == ConditionCategory.AGENT_CONDITION && !this.doneConditions[this.baseConditions[k].index])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x00169D94 File Offset: 0x00167F94
	private bool CheckOrdeal(OrdealBase ordeal)
	{
		for (int i = 0; i < this.baseConditions.Count; i++)
		{
			if (this.baseConditions[i].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				this.doneConditions[this.baseConditions[i].index] = false;
				this.baseConditions[i].current = 0;
			}
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			if (this.baseConditions[j].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				ConditionType condition_Type = this.baseConditions[j].condition_Type;
				if (condition_Type == ConditionType.CREATURE_LEVEL)
				{
					this.baseConditions[j].current = (int)ordeal.level;
				}
				if (!this.doneConditions[this.baseConditions[j].index])
				{
					this.doneConditions[this.baseConditions[j].index] = this.CheckDefault(this.baseConditions[j]);
				}
			}
		}
		for (int k = 0; k < this.baseConditions.Count; k++)
		{
			if (this.baseConditions[k].condition_Category == ConditionCategory.CREATURE_CONDITION && !this.doneConditions[this.baseConditions[k].index])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x00169F1C File Offset: 0x0016811C
	private bool CheckEquipment(EquipmentModel equip)
	{
		for (int i = 0; i < this.baseConditions.Count; i++)
		{
			if (this.baseConditions[i].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				this.doneConditions[this.baseConditions[i].index] = false;
				this.baseConditions[i].current = 0;
			}
		}
		for (int j = 0; j < this.baseConditions.Count; j++)
		{
			if (this.baseConditions[j].condition_Category == ConditionCategory.CREATURE_CONDITION)
			{
				ConditionType condition_Type = this.baseConditions[j].condition_Type;
				if (condition_Type == ConditionType.CREATURE_LEVEL)
				{
					this.baseConditions[j].current = (int)equip.metaInfo.Grade;
				}
				if (!this.doneConditions[this.baseConditions[j].index])
				{
					this.doneConditions[this.baseConditions[j].index] = this.CheckDefault(this.baseConditions[j]);
				}
			}
		}
		for (int k = 0; k < this.baseConditions.Count; k++)
		{
			if (this.baseConditions[k].condition_Category == ConditionCategory.CREATURE_CONDITION && !this.doneConditions[this.baseConditions[k].index])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003321 RID: 13089
	public MissionTypeInfo metaInfo;

	// Token: 0x04003322 RID: 13090
	public List<Condition> conditions;

	// Token: 0x04003323 RID: 13091
	public Condition successCondition;

	// Token: 0x04003324 RID: 13092
	public List<Condition> failConditions;

	// Token: 0x04003325 RID: 13093
	public List<Condition> baseConditions;

	// Token: 0x04003326 RID: 13094
	public List<object> checkedObjects;

	// Token: 0x04003327 RID: 13095
	public bool isCleared;

	// Token: 0x04003328 RID: 13096
	public bool isInProcess;

	// Token: 0x04003329 RID: 13097
	public List<bool> doneConditions;
}
