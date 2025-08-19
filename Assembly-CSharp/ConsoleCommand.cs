using System;
using System.Collections.Generic;
using System.IO; // 
using GlobalBullet;
using Inventory;
using Rabbit;
using UnityEngine;
using UnityEngine.SceneManagement;
using LobotomyBaseMod; // 

// Token: 0x0200037D RID: 893
public class ConsoleCommand
{
	// Token: 0x06001B6A RID: 7018 RVA: 0x000E48F0 File Offset: 0x000E2AF0
	public ConsoleCommand()
	{ // <Mod>
		this.lib = new Dictionary<string, string>();
		this.creatureCommand = new List<string>();
		this.standardCommand = new List<string>();
		this.agentCommand = new List<string>();
		this.officerCommand = new List<string>();
		this.rootCommand = new List<string>();
		this.configCommand = new List<string>();
		this.SetList();
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001B6B RID: 7019 RVA: 0x0001DE49 File Offset: 0x0001C049
	public static ConsoleCommand instance
	{
		get
		{
			if (ConsoleCommand._instance == null)
			{
				ConsoleCommand._instance = new ConsoleCommand();
			}
			return ConsoleCommand._instance;
		}
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x000E494C File Offset: 0x000E2B4C
	public void SetList()
	{
		this.creatureCommand.Add(ConsoleCommand.AddCreatureFeeling);
		this.creatureCommand.Add(ConsoleCommand.SubCreatureFeeling);
		this.creatureCommand.Add(ConsoleCommand.SetCreatureObservable);
		this.creatureCommand.Add(ConsoleCommand.TakePhysicalDamage);
		this.creatureCommand.Add(ConsoleCommand.AddCumlativeCube);
		this.creatureCommand.Add(ConsoleCommand.QliportCounterReduce);
		this.creatureCommand.Add(ConsoleCommand.SuppressAll);
		this.standardCommand.Add(ConsoleCommand.AddSystemLog);
		this.standardCommand.Add(ConsoleCommand.EnergyFill);
		this.standardCommand.Add(ConsoleCommand.AngelaDescMake);
		this.standardCommand.Add(ConsoleCommand.OpenListWindow);
		this.standardCommand.Add(ConsoleCommand.AddMoney);
		this.standardCommand.Add(ConsoleCommand.InventoryOpen);
		this.standardCommand.Add(ConsoleCommand.EmregencyAdd);
		this.standardCommand.Add(ConsoleCommand.DamageInvoking);
		this.standardCommand.Add(ConsoleCommand.MakeEquipment);
		this.standardCommand.Add(ConsoleCommand.ActivateOrdeal);
		this.standardCommand.Add(ConsoleCommand.FullAmmo);
		this.standardCommand.Add(ConsoleCommand.InvokeOverload);
		this.standardCommand.Add(ConsoleCommand.Boss);
		this.standardCommand.Add(ConsoleCommand.SefiraBossConversation);
		this.standardCommand.Add(ConsoleCommand.WaitingCreature);
		this.standardCommand.Add(ConsoleCommand.MissionClear);
		this.standardCommand.Add(ConsoleCommand.AllocateAgents);
		this.standardCommand.Add(ConsoleCommand.RabbitProtocol);
		this.standardCommand.Add(ConsoleCommand.PresentCluster);
		this.standardCommand.Add(ConsoleCommand.ClearOverload);
		this.standardCommand.Add(ConsoleCommand.ClearBoss);
		this.standardCommand.Add(ConsoleCommand.ResearchAll);
		this.standardCommand.Add(ConsoleCommand.DeallocateAll);
		this.standardCommand.Add(ConsoleCommand.MissionAdd);
		this.agentCommand.Add(ConsoleCommand.TakePhysicalDamage);
		this.agentCommand.Add(ConsoleCommand.TakeMentalDamage);
		this.agentCommand.Add(ConsoleCommand.SuperSoldier);
		this.agentCommand.Add(ConsoleCommand.Encounter);
		this.agentCommand.Add(ConsoleCommand.GiftAdd);
		this.agentCommand.Add(ConsoleCommand.GiftRemove);
		this.agentCommand.Add(ConsoleCommand.DamageForcely);
		this.officerCommand.Add(ConsoleCommand.MakePanicAll);
		this.officerCommand.Add(ConsoleCommand.TakePhysicalDamage);
		this.rootCommand.Add(ConsoleCommand.ChangeLanguage);
		// <Mod>
		creatureCommand.Add("meltdown");
		configCommand.Add("changebasicmode");
		configCommand.Add("changespecialmode");
		configCommand.Add("changeovertimemode");
		configCommand.Add("changereworkmode");
		configCommand.Add("changebasicmodetrans");
		configCommand.Add("changespecialmodetrans");
		configCommand.Add("changeovertimemodetrans");
		configCommand.Add("changereworkmodetrans");
		configCommand.Add("changebasicmodetemp");
		configCommand.Add("changespecialmodetemp");
		configCommand.Add("changeovertimemodetemp");
		configCommand.Add("changereworkmodetemp");
		configCommand.Add("changebasicmodequiet");
		configCommand.Add("changespecialmodequiet");
		configCommand.Add("changeovertimemodequiet");
		configCommand.Add("changereworkmodequiet");
		configCommand.Add("overtimetoggle");
		configCommand.Add("overtimetoggletrans");
		configCommand.Add("overtimetoggletemp");
		configCommand.Add("overtimetogglequiet");
		configCommand.Add("reworktier");
		configCommand.Add("reworktiertrans");
		configCommand.Add("reworktiertemp");
		configCommand.Add("reworktierquiet");
		configCommand.Add("resetmodestransient");
		configCommand.Add("resetmodesall");
		standardCommand.Add("enterovertime");
		standardCommand.Add("justgetonwithit");
		standardCommand.Add("littlebabyman");
		standardCommand.Add("samuraijack");
		standardCommand.Add("allmine");
		standardCommand.Add("trainingarc");
		agentCommand.Add("removeallgifts");
		agentCommand.Add("titlebonus");
		creatureCommand.Add("replace");
		standardCommand.Add("agentlist");
		standardCommand.Add("creaturelist");
		creatureCommand.Add("swap");
		standardCommand.Add("imlosingmymind");
		standardCommand.Add("redact");
		standardCommand.Add("loadoutload");
		standardCommand.Add("loadoutsave");
		standardCommand.Add("secondaryoverload");
		rootCommand.Add("resolution");
		standardCommand.Add("recyclebin");
		creatureCommand.Add("workdefault");
		standardCommand.Add("workdefault");
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000E4BEC File Offset: 0x000E2DEC
	public void CreatureCommandOperation(int index, bool global, params object[] param)
	{
		if (index < 0 || index >= this.creatureCommand.Count)
		{
			Debug.LogError("Error in CreatureCommand : " + index);
			return;
		}
		if (global)
		{
			Debug.Log(index);
			switch (index)
			{
			case 0:
			{
				float value = (float)param[1];
				foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
				{
					this.AddCreatureFeelingCommand(creatureModel.instanceId, value);
				}
				goto IL_206;
			}
			case 1:
			{
				float value2 = (float)param[1];
				foreach (CreatureModel creatureModel2 in CreatureManager.instance.GetCreatureList())
				{
					this.SubCreatureFeelingCommand(creatureModel2.instanceId, value2);
				}
				goto IL_206;
			}
			case 2:
				foreach (CreatureModel creatureModel3 in CreatureManager.instance.GetCreatureList())
				{
					this.SetObservable(creatureModel3.instanceId);
				}
				goto IL_206;
			case 3:
				goto IL_206;
			case 4:
			{
				float num = (float)param[1];
				int num2 = (int)num;
				foreach (CreatureModel creatureModel4 in CreatureManager.instance.GetCreatureList())
				{
					Debug.Log("ADD : " + num2);
					creatureModel4.observeInfo.cubeNum += num2;
					creatureModel4.observeInfo.totalKitUseCount += num2;
					creatureModel4.observeInfo.totalKitUseTime += num;
				}
				foreach (CreatureModel creatureModel5 in CreatureManager.instance.GetCreatureList())
				{
					creatureModel5.SubQliphothCounter();
				}
				goto IL_206;
			}
			case 6:
				this.SuppressAllCommand();
				goto IL_206;
			case 10:
				if (((string)param[1]).ToLower() == "reset")
				{
					WorkQueuePreferanceManager.instance.SaveDefaultCondition(null);
				}
				else
				{
					QueuedWorkOrder.QueueConditionInfo condition = new QueuedWorkOrder.QueueConditionInfo();
					condition.minHp = float.Parse((string)param[1]);
					condition.minMental = float.Parse((string)param[2]);
					condition.delay = float.Parse((string)param[3]);
					condition.yieldAgent = bool.Parse((string)param[4]);
					condition.yieldCreature = bool.Parse((string)param[5]);
					condition.asapAgent = bool.Parse((string)param[6]);
					condition.asapCreature = bool.Parse((string)param[7]);
					condition.impassableAgent = bool.Parse((string)param[8]);
					condition.impassableCreature = bool.Parse((string)param[9]);
					WorkQueuePreferanceManager.instance.SaveDefaultCondition(condition);
				}
				break;
			}
			return;
			IL_206:;
		}
		else
		{
			long num3 = (long)param[0];
			CreatureModel creature = CreatureManager.instance.GetCreature(num3);
			if (creature == null) return;
			switch (index)
			{
			case 0:
			{
				float value3 = (float)param[1];
				this.AddCreatureFeelingCommand(num3, value3);
				break;
			}
			case 1:
			{
				float value4 = (float)param[1];
				this.SubCreatureFeelingCommand(num3, value4);
				break;
			}
			case 2:
				this.SetObservable(num3);
				break;
			case 3:
			{
				float value5 = (float)param[1];
				this.CreatureTakeDamageCommand(num3, value5);
				break;
			}
			case 4:
			{
				float num4 = (float)param[1];
				int num5 = (int)num4;
				Debug.Log("ADD : " + num5);
				creature.observeInfo.cubeNum += num5;
				creature.observeInfo.totalKitUseCount += num5;
				creature.observeInfo.totalKitUseTime += num4;
				break;
			}
			case 5:
			{
				if (creature != null)
				{
					float num6 = 1f;
					if (param.Length > 1)
					{
						num6 = (float)param[1];
					}
					int num7 = (int)num6;
					if (num7 > 0)
					{
						for (int i = 0; i < num7; i++)
						{
							creature.SubQliphothCounter();
						}
					}
					else
					{
						num7 *= -1;
						for (int i = 0; i < num7; i++)
						{
							creature.AddQliphothCounter();
						}
					}
				}
				break;
			}
			case 7:
            {
                OverloadType type = OverloadType.DEFAULT;
				float timer = 60f;
				bool isNatural = false;
                if (param.Length >= 2)
				{
                    switch (((string)param[1]).Substring(0, 1).ToLower())
                    {
                        case "p":
                            type = OverloadType.PAIN;
                            break;
                        case "g":
                            type = OverloadType.GRIEF;
                            break;
                        case "r":
                            type = OverloadType.RUIN;
                            break;
                        case "o":
                            type = OverloadType.OBLIVION;
                            break;
                    }
                }
                if (param.Length >= 3)
				{
                    timer = float.Parse((string)param[2]);
                }
                if (param.Length >= 4)
				{
                    isNatural = bool.Parse((string)param[3]);
                }
				CreateOverload(creature, type, timer, isNatural);
				break;
            }
			case 8:
				if (param.Length >= 3)
				{
					CreatureManager.instance.ReplaceCreature_Mod(new LcIdLong((string)param[1], long.Parse((string)param[2])), creature);
					return;
				}
				else
				{
					CreatureManager.instance.ReplaceCreature(long.Parse((string)param[1]), creature);
				}
				if (!GameManager.currentGameManager.ManageStarted)
				{
					GlobalGameManager.instance.SaveData(false);
				}
				break;
			case 9:
				long num8 = long.Parse((string)param[1]);
				CreatureModel creature2 = CreatureManager.instance.GetCreature(num8);
				if (creature2 == null) return;
				SwapAbnormalities(creature, creature2);
				break;
			case 10:
				if (((string)param[1]).ToLower() == "reset")
				{
					WorkQueuePreferanceManager.instance.SaveDefaultCondition(null, creature);
				}
				else
				{
					QueuedWorkOrder.QueueConditionInfo condition = new QueuedWorkOrder.QueueConditionInfo();
					condition.minHp = float.Parse((string)param[1]);
					condition.minMental = float.Parse((string)param[2]);
					condition.delay = float.Parse((string)param[3]);
					condition.yieldAgent = bool.Parse((string)param[4]);
					condition.yieldCreature = bool.Parse((string)param[5]);
					condition.asapAgent = bool.Parse((string)param[6]);
					condition.asapCreature = bool.Parse((string)param[7]);
					condition.impassableAgent = bool.Parse((string)param[8]);
					condition.impassableCreature = bool.Parse((string)param[9]);
					WorkQueuePreferanceManager.instance.SaveDefaultCondition(condition, creature);
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000E4FB4 File Offset: 0x000E31B4
	public void StandardCommandOperation(int index, params object[] param)
	{
		if (index < 0 || index >= this.standardCommand.Count)
		{
			Debug.LogError("Error in StandardCommand : " + index);
			return;
		}
		switch (index)
		{
		case 0:
			this.AddSystemLogCommand((string)param[0]);
			break;
		case 1:
		{
			float value0 = (float)param[0];
			this.EnergyFillCommand(value0);
			break;
		}
		case 2:
			this.AddAngelaDescCommand((string)param[0]);
			break;
		case 3:
			this.OpenAgentListWindow((string)param[0]);
			break;
		case 4:
		{
			int value2 = (int)float.Parse((string)param[0]);
			this.AddMoneyCommand(value2);
			break;
		}
		case 5:
			this.OpenInventory();
			break;
		case 6:
			this.AddEmergeny((int)float.Parse((string)param[0]));
			break;
		case 7:
			this.DamageInvoke((string)param[0], (string)param[1], (string)param[2]);
			break;
		case 8:
			if (param.Length >= 3)
			{
				this.GenerateEquipment((string)param[0], (int)float.Parse((string)param[1]), (int)float.Parse((string)param[2]));
			}
			else if (param.Length == 2)
			{
				float result;
				if (float.TryParse((string)param[0], out result))
				{
					this.GenerateEquipment((int)result, (int)float.Parse((string)param[1]));
				}
				else
				{
					this.GenerateEquipment((string)param[0], (int)float.Parse((string)param[1]));
				}
			}
			else
			{
				this.GenerateEquipment((int)float.Parse((string)param[0]));
			}
			break;
		case 9:
			if (param.Length >= 2)
			{
				this.ActivateOrdealSystem((int)float.Parse((string)param[0]), (string)param[1]);
				break;
			}
			this.ActivateOrdealSystem((int)float.Parse((string)param[0]));
			break;
		case 10:
			this.FullFillAmmo();
			break;
		case 11:
			this.OverloadInvoke((int)float.Parse((string)param[0]));
			break;
		case 12:
			if (param.Length >= 2)
			{
				SefiraBossInvoke(((string)param[0]).Trim(), (bool)param[1]);
			}
			else
			{
				SefiraBossInvoke(((string)param[0]).Trim());
			}
			break;
		case 13:
			if (param == null || param.Length == 0)
			{
				string empty = string.Empty;
				if (SefiraBossManager.Instance.TryGetBossDesc(SefiraBossManager.Instance.CurrentActivatedSefira, SefiraBossDescType.DEFAULT, out empty))
				{
					this.MakeSefiraBossDesc(empty);
				}
				return;
			}
			this.MakeSefiraBossDesc(((string)param[0]).Trim());
			break;
		case 14:
			if (param.Length >= 2)
			{
				this.AddWaitingGenCreature((string)param[0], (long)float.Parse((string)param[1]));
			}
			else
			{
				this.AddWaitingGenCreature((long)float.Parse((string)param[0]));
			}
			break;
		case 15:
			MissionManager.instance.DebugMissionClear((int)float.Parse((string)param[0]));
			break;
		case 16:
			this.AllocateAllAgentsToAllSefira();
			break;
		case 17:
			this.RabbitOpen();
			break;
		case 18:
			this.PresentClusterAction();
			break;
		case 19:
			this.ClearOverloadAction();
			break;
		case 20:
			this.ClearSefiraBoss();
			break;
		case 21:
			this.AllResearchUpgrade();
			break;
		case 22:
			this.DeAllocateAgentFromSefira();
			break;
		case 23:
		{
			int metaid = (int)float.Parse((string)param[0]);
			this.AddMission(metaid);
			break;
		}
		case 24:
			PlayerModel.instance.ForcelyEnterOvertimeMode();
			break;
		case 25:
			PrepareOvertime();
			break;
		case 26:
			ResetWhiteNight();
			break;
		case 27:
			ReturnToMemRepo(int.Parse((string)param[0]));
			break;
		case 28:
			CompleteInventory();
			break;
		case 29:
			MaxStatAllEmployees();
			break;
		case 30:
			string txt;
			string str = "";
			foreach (AgentModel agent in AgentManager.instance.GetAgentList())
			{
				txt = agent.instanceId + " : " + agent.name;
				Notice.instance.Send("AddSystemLog", new object[]
				{
					txt
				});
				str += txt + "\n";
			}
			foreach (AgentModel agent in AgentManager.instance.GetAgentSpareList())
			{
				txt = agent.instanceId + " : " + agent.name;
				Notice.instance.Send("AddSystemLog", new object[]
				{
					txt
				});
				str += txt + "\n";
			}
			UnityEngine.GUIUtility.systemCopyBuffer = str;
			break;
		case 31:
			string txt2;
			string str2 = "";
			foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
			{
				txt2 = creature.instanceId + " : " + creature.GetUnitName();
				Notice.instance.Send("AddSystemLog", new object[]
				{
					txt2
				});
				str2 += txt2 + "\n";
			}
			UnityEngine.GUIUtility.systemCopyBuffer = str2;
			break;
		case 32:
			InventoryModel.Instance.Init();/*
			InventoryModel inventory = InventoryModel.Instance;
			List<EquipmentModel> list = new List<EquipmentModel>(inventory.GetAllEquipmentList());
			foreach (EquipmentModel equipment in list)
			{
				inventory.RemoveEquipment(equipment);
			}*/
			break;
		case 33:
			if (param.Length >= 1)
			{
				long num = long.Parse((string)param[0]);
				CreatureObserveInfoModel info = CreatureManager.instance.GetObserveInfo(num);
				if (info != null)
				{
					info.OnResetObserve();
					info.observeProgress = 0;
					info.cubeNum = 0;
					info.totalKitUseCount = 0;
					info.totalKitUseTime = 0f;
				}
			}
			else
			{
				foreach (CreatureObserveInfoModel creature in CreatureManager.instance.GetObserveInfoList())
				{
					creature.OnResetObserve();
					creature.observeProgress = 0;
					creature.cubeNum = 0;
					creature.totalKitUseCount = 0;
					creature.totalKitUseTime = 0f;
				}
			}
			break;
		case 34:
			EquipmentLoadoutManager.instance.Load(int.Parse((string)param[0]));
			break;
		case 35:
			EquipmentLoadoutManager.instance.Save(int.Parse((string)param[0]));
			break;
		case 36:
			int value3 = 1;
			if (param.Length >= 1)
			{
				value3 = int.Parse((string)param[0]);
			}
			CreatureOverloadManager.instance.AddSecondaryGague(value3);
			break;
		case 37:
			while (PlayerModel.instance.GetWaitingCreature_Mod(out LcIdLong id))
			{

			}
			break;
		case 38:
			LobotomyBaseMod.LcIdLong lcId = null;
			QueuedWorkOrder.QueueConditionInfo condition = new QueuedWorkOrder.QueueConditionInfo();
			int ind = 0;
			if (param.Length >= 11)
			{
				string modId = (string)param[ind++];
				long creatureId = long.Parse((string)param[ind++]);
				lcId = new LobotomyBaseMod.LcIdLong(modId, creatureId);
			}
			else if (param.Length >= 10)
			{
				long creatureId = long.Parse((string)param[ind++]);
				lcId = new LobotomyBaseMod.LcIdLong(creatureId);
			}
			condition.minHp = float.Parse((string)param[ind++]);
			condition.minMental = float.Parse((string)param[ind++]);
			condition.delay = float.Parse((string)param[ind++]);
			condition.yieldAgent = bool.Parse((string)param[ind++]);
			condition.yieldCreature = bool.Parse((string)param[ind++]);
			condition.asapAgent = bool.Parse((string)param[ind++]);
			condition.asapCreature = bool.Parse((string)param[ind++]);
			condition.impassableAgent = bool.Parse((string)param[ind++]);
			condition.impassableCreature = bool.Parse((string)param[ind++]);
			WorkQueuePreferanceManager.instance.SaveDefaultCondition(condition, lcId);
			break;
		default:
			return;
		}
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000E5268 File Offset: 0x000E3468
	public void AgentCommandOperation(int index, params object[] param)
	{
		foreach (object obj in param)
		{
			Debug.Log(param);
		}
		if (index < 0 || index >= this.agentCommand.Count)
		{
			Debug.LogError("Error in AgentCommand : " + index);
			return;
		}
		switch (index)
		{
		case 0:
		{
			float num = (float)param[1];
			long num2 = (long)param[0];
			Debug.Log(num2 + " // " + num);
			this.TakePhysicalDamageCommand((long)param[0], num);
			break;
		}
		case 1:
		{
			float num3 = (float)param[1];
			long num4 = (long)param[0];
			Debug.Log(num4 + " // " + num3);
			this.TakeMentalDamageCommand((long)param[0], num3);
			break;
		}
		case 2:
		{
			float value = (float)param[1];
			long id = (long)param[0];
			this.SuperSoldierCommand(id, value);
			break;
		}
		case 3:
		{
			long id2 = (long)param[0];
			int level = (int)((float)param[1]);
			this.EncounterAction(id2, level);
			break;
		}
		case 4:
		{
			if (param.Length >= 3)
			{
				long id3 = (long)param[0];
				int equipid = (int)((float)param[2]);
				string equipmod = (string)param[1];
				this.AddGift_Mod(id3, new LobotomyBaseMod.LcId(equipmod, equipid));
			}
			else
			{
				long id3 = (long)param[0];
				int equipid = (int)((float)param[1]);
				this.AddGift(id3, equipid);
			}
			break;
		}
		case 5:
		{
			long id4 = (long)param[0];
			int equipid2 = (int)((float)param[1]);
			this.RemoveGift(id4, equipid2);
			break;
		}
		case 6:
		{
			RwbpType type = (RwbpType)param[0];
			long id5 = (long)param[1];
			int value2 = (int)param[2];
			this.Damage(type, id5, value2);
			break;
		}
		case 7:
		{
			long id6 = long.Parse((string)param[0]);
			RemoveAllGifts(id6);
			break;
		}
		case 8:
		{
			long id6 = long.Parse((string)param[0]);
			int prefix = int.Parse((string)param[1]);
			int suffix = int.Parse((string)param[2]);
			ChangeTitleBonus(id6, prefix, suffix);
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x000E5434 File Offset: 0x000E3634
	public void OfficerCommandOperation(int index, params object[] param)
	{
		if (index != 0)
		{
			if (index != 1)
			{
				return;
			}
			float value = float.Parse(param[0] as string);
			this.OfficerTakePhysicalDamage(value);
		}
		else
		{
			this.MakePanicAllCommand();
		}
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x000E547C File Offset: 0x000E367C
	public void RootCommandOperation(int index, params object[] param)
	{
		List<string> list = new List<string>();
		foreach (object obj in param)
		{
			list.Add((string)obj);
		}
		Debug.Log(index);
		foreach (string message in list)
		{
			Debug.Log(message);
		}
		if (index == 0)
		{
			this.ChangeLanguageCommad(list[0]);
		}
		else if (index == 1)
		{
			this.ChangeResolutionCommad(int.Parse(list[0]), int.Parse(list[1]));
		}
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000E552C File Offset: 0x000E372C
	public void BetaCommandOperation(string cmd, string[] param)
	{
		string text = cmd.ToLower();
		if (text != null)
		{
			if (text == "energy")
			{
				this.BetaAddEnergy(int.Parse(param[2]));
				return;
			}
			if (text == "meltdown")
			{
				this.BetaMeltdown();
				return;
			}
			if (text == "lob")
			{
				this.BetaLob(int.Parse(param[2]));
				return;
			}
			if (text == "pebox")
			{
				this.BetaPeBox(int.Parse(param[2]));
				return;
			}
			if (text == "equip")
			{
				this.BetaEquip();
				return;
			}
			if (text == "storytester")
			{
				this.BetaStoryTester();
				return;
			}
		}
	}

	// <Mod>
	public void ConfigCommandOperation(int index, params object[] param)
	{
		if (index < 0 || index >= this.configCommand.Count)
		{
			Debug.LogError("Error in ConfigCommand : " + index);
			return;
		}
		if (index < 16)
		{
			string mode = (string)param[0];
			string value = (string)param[1];
			SpecialModeConfig.ModeType modeType = SpecialModeConfig.ModeType.BASIC;
			if (index % 4 == 1)
			{
				modeType = SpecialModeConfig.ModeType.SPECIAL;
			}
			else if (index % 4 == 2)
			{
				modeType = SpecialModeConfig.ModeType.OVERTIME;
			}
			else if (index % 4 == 3)
			{
				modeType = SpecialModeConfig.ModeType.REWORK;
			}
			SpecialModeConfig.SaveType saveType = SpecialModeConfig.SaveType.PERSISTANT;
			if (index >= 12)
			{
				saveType = SpecialModeConfig.SaveType.QUIET;
			}
			else if (index >= 8)
			{
				saveType = SpecialModeConfig.SaveType.NONE;
			}
			else if (index >= 4)
			{
				saveType = SpecialModeConfig.SaveType.TRANSIENT;
			}
			if (value == "reset")
			{
				SpecialModeConfig.instance.ResetValue(mode, modeType, saveType);
				return;
			}
			Type variableType = SpecialModeConfig.instance.GetModeType(mode, modeType);
			if (variableType == typeof(bool))
			{
				bool boolvalue;
				if (!bool.TryParse(value, out boolvalue)) return;
				SpecialModeConfig.instance.ChangeValue(mode, boolvalue, modeType, saveType);
			}
			else if (variableType == typeof(int))
			{
				int intvalue;
				if (!int.TryParse(value, out intvalue)) return;
				SpecialModeConfig.instance.ChangeValue(mode, intvalue, modeType, saveType);
			}
			else if (variableType == typeof(float))
			{
				float floatvalue;
				if (!float.TryParse(value, out floatvalue)) return;
				SpecialModeConfig.instance.ChangeValue(mode, floatvalue, modeType, saveType);
			}
			else if (variableType == typeof(SuccessRateDisplayMode))
			{
				SuccessRateDisplayMode enumvalue = SuccessRateDisplayMode.PERCENTAGE;
				switch (value.ToLower())
				{
					case "percentage":
					case "percentageonly":
					case "1":
					case "p":
					case "po":
						enumvalue = SuccessRateDisplayMode.PERCENTAGE;
						break;
					case "percentageandtext":
					case "percentagetext":
					case "textandpercentage":
					case "textpercentage":
					case "both":
					case "2":
					case "pat":
					case "pt":
					case "tap":
					case "tp":
						enumvalue = SuccessRateDisplayMode.PERCENTAGE_AND_TEXT;
						break;
					case "text":
					case "textonly":
					case "3":
					case "to":
					case "t":
						enumvalue = SuccessRateDisplayMode.TEXT_ONLY;
						break;
				}
				SpecialModeConfig.instance.ChangeValue(mode, enumvalue, modeType, saveType);
			}
			else if (variableType == typeof(AbnoHpDisplayMode))
			{
				AbnoHpDisplayMode enumvalue = AbnoHpDisplayMode.NAME_ONLY;
				switch (value.ToLower())
				{
					case "name":
					case "nameonly":
					case "1":
					case "n":
					case "no":
						enumvalue = AbnoHpDisplayMode.NAME_ONLY;
						break;
					case "nameandhp":
					case "namehp":
					case "hpandname":
					case "hpname":
					case "nameandhealth":
					case "namehealth":
					case "healthandname":
					case "healthname":
					case "both":
					case "2":
					case "nah":
					case "nh":
					case "han":
					case "hn":
					case "nahp":
					case "nhp":
					case "hpan":
					case "hpn":
						enumvalue = AbnoHpDisplayMode.NAME_AND_HP;
						break;
					case "health":
					case "healthonly":
					case "hponly":
					case "3":
					case "ho":
					case "h":
					case "hpo":
					case "hp":
						enumvalue = AbnoHpDisplayMode.HP_ONLY;
						break;
				}
				SpecialModeConfig.instance.ChangeValue(mode, enumvalue, modeType, saveType);
			}
			return;
		}
		switch (index)
		{
		case 16:
			SpecialModeConfig.instance.ChangeOvertimeMode(bool.Parse((string)param[0]), SpecialModeConfig.SaveType.PERSISTANT);
			break;
		case 17:
			SpecialModeConfig.instance.ChangeOvertimeMode(bool.Parse((string)param[0]), SpecialModeConfig.SaveType.TRANSIENT);
			break;
		case 18:
			SpecialModeConfig.instance.ChangeOvertimeMode(bool.Parse((string)param[0]), SpecialModeConfig.SaveType.NONE);
			break;
		case 19:
			SpecialModeConfig.instance.ChangeOvertimeMode(bool.Parse((string)param[0]), SpecialModeConfig.SaveType.QUIET);
			break;
		case 20:
			SpecialModeConfig.instance.ChangeReworkTier(int.Parse((string)param[0]), SpecialModeConfig.SaveType.PERSISTANT);
			break;
		case 21:
			SpecialModeConfig.instance.ChangeReworkTier(int.Parse((string)param[0]), SpecialModeConfig.SaveType.TRANSIENT);
			break;
		case 22:
			SpecialModeConfig.instance.ChangeReworkTier(int.Parse((string)param[0]), SpecialModeConfig.SaveType.NONE);
			break;
		case 23:
			SpecialModeConfig.instance.ChangeReworkTier(int.Parse((string)param[0]), SpecialModeConfig.SaveType.QUIET);
			break;
		case 24:
			SpecialModeConfig.instance.ResetTransients();
			break;
		case 25:
			SpecialModeConfig.instance.ResetAll();
			break;
		default:
			return;
		}
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x0001DE64 File Offset: 0x0001C064
	public void AddSystemLogCommand(string msg)
	{
		Notice.instance.Send("AddSystemLog", new object[]
		{
			msg
		});
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0001DE7F File Offset: 0x0001C07F
	public void AddAngelaDescCommand(string desc)
	{
		AngelaConversationUI.instance.AddAngelaMessage(desc);
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000E55F0 File Offset: 0x000E37F0
	public void AddCreatureFeelingCommand(long id, float value)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(id);
		if (creature != null)
		{
			Debug.Log(string.Concat(new object[]
			{
				id,
				" ",
				creature.model.metaInfo.name,
				"  added Feeling ",
				value
			}));
		}
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000E565C File Offset: 0x000E385C
	public void CreatureTakeDamageCommand(long id, float value)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(id);
		if (creature != null)
		{
			creature.model.TakeDamage(new DamageInfo(RwbpType.R, value));
			Debug.Log(string.Concat(new object[]
			{
				id,
				" ",
				creature.model.metaInfo.name,
				"  Take PhyscialDamage ",
				value
			}));
		}
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000E56D8 File Offset: 0x000E38D8
	public void SubCreatureFeelingCommand(long id, float value)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(id);
		if (creature != null)
		{
			Debug.Log(string.Concat(new object[]
			{
				id,
				" ",
				creature.model.metaInfo.name,
				"  sub Feeling ",
				value
			}));
		}
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000E5744 File Offset: 0x000E3944
	public void SetObservable(long id)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(id);
		if (creature != null)
		{
		}
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x000E576C File Offset: 0x000E396C
	public void SuppressAllCommand()
	{
		DamageInfo[] array = new DamageInfo[]
		{
			new DamageInfo(RwbpType.P, 1000000f),
			new DamageInfo(RwbpType.W, 1000000f),
			new DamageInfo(RwbpType.B, 1000000f),
			new DamageInfo(RwbpType.R, 1000000f)
		};
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			if (creatureModel.IsEscapedOnlyEscape())
			{
				foreach (DamageInfo dmg in array)
				{
					if (creatureModel.IsEscapedOnlyEscape())
					{
						creatureModel.TakeDamage(dmg);
					}
				}
			}
		}
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000E580C File Offset: 0x000E3A0C
	public void TakePhysicalDamageCommand(long id, float value)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.DebugDamage(RwbpType.R, (int)value);
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000E5830 File Offset: 0x000E3A30
	public void OfficerTakePhysicalDamage(float value)
	{
		foreach (Sefira sefira in SefiraManager.instance.sefiraList)
		{
			if (sefira.activated)
			{
				foreach (OfficerModel officerModel in sefira.officerList)
				{
					officerModel.TakeDamage(new DamageInfo(RwbpType.R, value));
				}
			}
		}
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000E58E8 File Offset: 0x000E3AE8
	public void TakeMentalDamageCommand(long id, float value)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.TakeDamage(new DamageInfo(RwbpType.W, (float)((int)value)));
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000E5910 File Offset: 0x000E3B10
	public void SuperSoldierCommand(long id, float value)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.hp = value;
		agent.mental = value;
		agent.primaryStat.hp = (int)value;
		agent.primaryStat.mental = (int)value;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x0001DE8C File Offset: 0x0001C08C
	public void EnergyFillCommand(float value)
	{
		EnergyModel.instance.AddEnergy(value);
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0000403D File Offset: 0x0000223D
	public void OpenAgentListWindow(string sefira)
	{
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0001DE99 File Offset: 0x0001C099
	public void AddMoneyCommand(int value)
	{
		MoneyModel.instance.Add(value);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000E5954 File Offset: 0x000E3B54
	public void MakePanicAllCommand()
	{
		foreach (Sefira sefira in SefiraManager.instance.sefiraList)
		{
			if (sefira.activated)
			{
				foreach (OfficerModel officerModel in sefira.officerList)
				{
					officerModel.TakeDamage(new DamageInfo(RwbpType.W, (float)officerModel.maxMental));
				}
			}
		}
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000E5A10 File Offset: 0x000E3C10
	public void ChangeLanguageCommad(string ln)
	{ // <Patch>
		string currentLanguage = GlobalGameManager.instance.GetCurrentLanguage();
		Debug.Log(ln);
		if (currentLanguage == ln)
		{
			return;
		}
		GlobalGameManager.instance.ChangeLanguage_new(ln);
		if (GlobalEtcDataModel.instance.trueEndingDone)
		{
			SceneManager.LoadSceneAsync("AlterTitleScene");
		}
		else
		{
			SceneManager.LoadSceneAsync("NewTitleScene");
		}
	}

	// <Mod>
	public void ChangeResolutionCommad(int w, int h)
	{
		Resolution resolution = new Resolution
		{
			width = w,
			height = h
		};
		OptionUI.Instance.OnSetResolution(resolution);
		OptionUI.Instance.OnClickSaveAndQuit();
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0001DEA6 File Offset: 0x0001C0A6
	public void OpenInventory()
	{
		InventoryUI.CreateWindow();
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x0001DEAE File Offset: 0x0001C0AE
	public void AddEmergeny(int emergency)
	{
		PlayerModel.emergencyController.AddScore((float)emergency);
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000E5A7C File Offset: 0x000E3C7C
	public void EncounterAction(long id, int level)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		if (level < 0 || level > 4)
		{
			level = 0;
		}
		agent.InitialEncounteredCreature((RiskLevel)level);
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000E5AB0 File Offset: 0x000E3CB0
	public void AddGift(long id, int equipid)
	{
		EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipmentForcely(equipid);
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.AttachEGOgift(equipmentModel as EGOgiftModel);
	}

	public void AddGift_Mod(long id, LcId equipid)
	{
		EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipmentForcely_Mod(equipid);
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.AttachEGOgift(equipmentModel as EGOgiftModel);
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000E5AE4 File Offset: 0x000E3CE4
	public void ClearOverloadAction()
	{
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			if (creatureModel.isOverloaded)
			{
				creatureModel.CancelOverload();
				creatureModel.ClearProbReduction();
			}
		}
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x0001DEBC File Offset: 0x0001C0BC
	public void ClearSefiraBoss()
	{
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated())
		{
			SefiraBossManager.Instance.ForcelyClear();
		}
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x0001DED7 File Offset: 0x0001C0D7
	public void AllResearchUpgrade()
	{
		ResearchDataModel.instance.UpgradeAllResearch();
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000E5B30 File Offset: 0x000E3D30
	public void DeAllocateAgentFromSefira()
	{
		try
		{
			if (GameManager.currentGameManager.state == GameState.STOP)
			{
				foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
				{
					if (sefira.sefiraEnum != SefiraEnum.DAAT)
					{
						int count = sefira.agentList.Count;
						SefiraPanel sefiraPanel = DeployUI.instance.sefiraList.GetSefiraPanel(sefira.indexString);
						for (int j = 0; j < count; j++)
						{
							sefiraPanel.OnRemoveAgent(0);
						}
					}
				}
				DeployUI.instance.Init();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x0001DEE3 File Offset: 0x0001C0E3
	public void AddMission(int metaid)
	{
		MissionManager.instance.StartMission(metaid);
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x000E5BF0 File Offset: 0x000E3DF0
	public void PresentClusterAction()
	{
		int[] array = new int[]
		{
			400009,
			400013,
			400001,
			400012,
			400021,
			400037,
			400003,
			400011,
			400002,
			400033,
			400032,
			400039,
			400035,
			400029,
			400048,
			400023,
			400036,
			400042,
			400019
		};
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			foreach (int id in array)
			{
				try
				{
					EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipmentForcely(id);
					agentModel.AttachEGOgift(equipmentModel as EGOgiftModel);
				}
				catch (Exception ex)
				{
				}
			}
		}
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x000E5CB0 File Offset: 0x000E3EB0
	public void RemoveGift(long id, int equipid)
	{
		RemoveGift_Mod(id, new LobotomyBaseMod.LcId(equipid));
		/*
		AgentModel agent = AgentManager.instance.GetAgent(id);
		EGOgiftModel egogiftModel = null;
		foreach (EGOgiftModel egogiftModel2 in agent.GetAllGifts())
		{
			if (egogiftModel2.metaInfo.id == equipid)
			{
				egogiftModel = egogiftModel2;
				break;
			}
		}
		if (egogiftModel != null)
		{
			agent.ReleaseEGOgift(egogiftModel);
		}*/
	}

	// <Patch>
	public static void RemoveGift_Mod(long id, LobotomyBaseMod.LcId equipid)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		EGOgiftModel egogiftModel = null;
		foreach (EGOgiftModel egogiftModel2 in agent.GetAllGifts())
		{
			if (EquipmentTypeInfo.GetLcId(egogiftModel2.metaInfo) == equipid)
			{
				egogiftModel = egogiftModel2;
				break;
			}
		}
		if (egogiftModel != null)
		{
			agent.ReleaseEGOgift(egogiftModel);
		}
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000E5D38 File Offset: 0x000E3F38
	public void Damage(RwbpType type, long id, int value)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		if (agent == null)
		{
			return;
		}
		agent.TakeDamage(null, new DamageInfo(type, (float)value));
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x000E5D6C File Offset: 0x000E3F6C
	public void DamageInvoke(string type, string defense, string damage)
	{
		float num = float.Parse(defense);
		float num2 = float.Parse(damage);
		int num3 = 1;
		AgentModel agent;
		while ((agent = AgentManager.instance.GetAgent((long)num3)) == null)
		{
			num3++;
			if (num3 > 10)
			{
				return;
			}
		}
		string text = type.Trim().ToLower();
		if (text != null)
		{
			RwbpType type2;
			if (!(text == "r"))
			{
				if (!(text == "w"))
				{
					if (!(text == "b"))
					{
						if (!(text == "p"))
						{
							return;
						}
						type2 = RwbpType.P;
					}
					else
					{
						type2 = RwbpType.B;
					}
				}
				else
				{
					type2 = RwbpType.W;
				}
			}
			else
			{
				type2 = RwbpType.R;
			}
			DamageEffect.Invoker(agent.GetMovableNode(), type2, (int)num2, (DefenseInfo.Type)num, agent);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(agent, type2, num, agent.GetMovableNode().GetDirection());
			return;
		}
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
	public void GenerateEquipment(int id)
	{ // <Mod>
		GenerateEquipment("", id, 1);
	}

	// <Mod>
	public void GenerateEquipment(int id, int count)
	{ // <Mod>
		GenerateEquipment("", id, count);
	}

	// <Patch>
	public void GenerateEquipment(string modid, int id)
	{ // <Mod>
		GenerateEquipment(modid, id, 1);
	}

	// <Mod>
	public void GenerateEquipment(string modid, int id, int count)
	{
		LcId lcId = new LcId(modid, id);
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				InventoryModel.Instance.CreateEquipment_Mod(lcId);
			}
		}
		else
		{
			List<EquipmentModel> list = new List<EquipmentModel>(InventoryModel.Instance.GetEquipmentListByTypeInfo_Mod()[lcId]);
			for (int i = 0; i < -count; i++)
			{
				if (i > list.Count - 1) return;
				EquipmentModel equipment = list[i];
				if (equipment == null) break;
				InventoryModel.Instance.RemoveEquipment(equipment);
			}
		}
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000E5E5C File Offset: 0x000E405C
	public void ActivateOrdealSystem(int level)
	{
		if (level < 0 || level > 7)
		{
			Debug.Log("Ordeal Level is Overflowed");
			return;
		}
		OrdealBase ordeal = null;
		if (!OrdealManager.instance.CheckOrdealContains((OrdealLevel)level, out ordeal))
		{
			Debug.Log("Could not find Ordeal of " + (OrdealLevel)level);
			return;
		}
		Debug.Log(string.Format("Tried Ordeal System {0} // result : {1}", (OrdealLevel)level, OrdealManager.instance.ActivateOrdeal(ordeal, true)));
	}

	// <Mod>
	public void ActivateOrdealSystem(int level, string type)
	{
		OrdealBase ordeal = GetOrdealByLetter(level, type);
		if (ordeal == null)
		{
			return;
		}
		if (level >= 4 && ordeal.level < OrdealLevel.OVERTIME_DAWN)
		{
			ordeal.level += 4;
		}
		OrdealManager.instance.ActivateOrdeal(ordeal, false);
	}
	
	// <Mod>
	private OrdealBase GetOrdealByLetter(int level, string type)
	{
		string firstLetter = type.Substring(0, 1).ToLower();
		switch (level)
		{
			case 0:
				switch (firstLetter)
				{
					case "g":
						return new MachineDawnOrdeal();
					case "a":
						return new BugDawnOrdeal();
					case "v":
						return new OutterGodDawnOrdeal();
					case "c":
						return new CircusDawnOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.DAWN);
				}
				return null;
			case 1:
				switch (firstLetter)
				{
					case "g":
						return new MachineNoonOrdeal();
					case "v":
						return new OutterGodNoonOrdeal();
					case "c":
						return new CircusNoonOrdeal();
					case "i":
						return new ScavengerNoonOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.NOON);
				}
				return null;
			case 2:
				switch (firstLetter)
				{
					case "g":
						return new MachineDuskOrdeal();
					case "a":
						return new BugDuskOrdeal();
					case "c":
						return new CircusDuskOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.DUSK);
				}
				return null;
			case 3:
				switch (firstLetter)
				{
					case "g":
						return new MachineMidnightOrdeal();
					case "a":
						return new BugMidnightOrdeal();
					case "v":
						return new OutterGodMidnightOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.MIDNIGHT);
				}
				return null;
			case 4:
				switch (firstLetter)
				{
					case "g":
						return new OvertimeMachineDawnOrdeal();
					case "a":
						return new OvertimeBugDawnOrdeal();
					case "v":
						return new OvertimeOutterGodDawnOrdeal();
					case "c":
						return new OvertimeCircusDawnOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.DAWN);
				}
				return null;
			case 5:
				switch (firstLetter)
				{
					case "g":
						return new OvertimeMachineNoonOrdeal();
					case "v":
						return new OutterGodNoonOrdeal();
					case "c":
						return new CircusNoonOrdeal();
					case "i":
						return new ScavengerNoonOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.NOON);
				}
				return null;
			case 6:
				switch (firstLetter)
				{
					case "g":
						return new MachineDuskOrdeal();
					case "a":
						return new BugDuskOrdeal();
					case "c":
						return new CircusDuskOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.DUSK);
				}
				return null;
			case 7:
				switch (firstLetter)
				{
					case "g":
						return new MachineMidnightOrdeal();
					case "a":
						return new BugMidnightOrdeal();
					case "v":
						return new OutterGodMidnightOrdeal();
					case "w":
						return new FixerOrdeal(OrdealLevel.MIDNIGHT);
				}
				return null;
		}
		return null;
	}
	
	// Token: 0x06001B92 RID: 7058 RVA: 0x0001DEFE File Offset: 0x0001C0FE
	public void FullFillAmmo()
	{
		GlobalBulletManager.instance.currentBullet = GlobalBulletManager.instance.maxBullet;
		GlobalBulletManager.instance.Reload();
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x000E5ED4 File Offset: 0x000E40D4
	public void OverloadInvoke(int count)
	{
		for (int i = 0; i < count; i++)
		{
			CreatureOverloadManager.instance.AddOverloadGague();
		}
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x000E5F00 File Offset: 0x000E4100
	public void SefiraBossInvoke(string sefira, bool isOvertime = false)
	{
		try
		{
			if (SefiraBossManager.Instance.CurrentActivatedSefira == SefiraManager.instance.GetSefira(sefira).sefiraEnum)
			{
				SefiraBossManager.Instance.SetActivatedBoss(SefiraEnum.DUMMY);
			}
			else
			{
				SefiraBossManager.Instance.SetActivatedBoss(SefiraManager.instance.GetSefira(sefira).sefiraEnum, isOvertime);
			}
		}
		catch (Exception ex)
		{
			SefiraBossManager.Instance.SetActivatedBoss(SefiraEnum.DUMMY);
		}
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000E5F80 File Offset: 0x000E4180
	public void MakeSefiraBossDesc(string desc)
	{
		try
		{
			SefiraBossDescUI sefiraBossDescUI = SefiraBossDescUI.GenDesc(desc, 0.3f);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000E5FBC File Offset: 0x000E41BC
	public void AddWaitingGenCreature(long id)
	{
		try
		{
			PlayerModel.instance.AddWaitingCreature(id);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	public void AddWaitingGenCreature(string modid, long id)
	{
		try
		{
			PlayerModel.instance.AddWaitingCreature_Mod(new LcIdLong(modid, id));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x0001DF14 File Offset: 0x0001C114
	public void RabbitOpen()
	{
		RabbitProtocolWindow.Window.OnOpen();
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000E5FF8 File Offset: 0x000E41F8
	public void AllocateAllAgentsToAllSefira()
	{
		try
		{
			if (GameManager.currentGameManager.state == GameState.STOP)
			{
				foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
				{
					if (sefira.sefiraEnum != SefiraEnum.DAAT)
					{
						int allocateMax = sefira.allocateMax;
						int count = sefira.agentList.Count;
						int num = allocateMax - count;
						for (int j = 0; j < num; j++)
						{
							AgentModel add = AgentManager.instance.AddSpareAgentModel();
							sefira.AddAgent(add);
						}
					}
				}
				DeployUI.instance.Init();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000E60BC File Offset: 0x000E42BC
	public void BetaAddEnergy(int value)
	{
		int day = PlayerModel.instance.GetDay();
		float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
		EnergyModel.instance.AddEnergy(energyNeed * (float)value / 100f);
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000E60F4 File Offset: 0x000E42F4
	public void BetaMeltdown()
	{
		for (int i = 0; i < CreatureOverloadManager.instance.qliphothOverloadMax; i++)
		{
			CreatureOverloadManager.instance.AddOverloadGague();
		}
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x0001DE99 File Offset: 0x0001C099
	public void BetaLob(int value)
	{
		MoneyModel.instance.Add(value);
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000E6128 File Offset: 0x000E4328
	public void BetaPeBox(int value)
	{
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			creatureModel.observeInfo.cubeNum += value;
			creatureModel.observeInfo.totalKitUseCount += value;
			creatureModel.observeInfo.totalKitUseTime += (float)value;
		}
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x0001DF20 File Offset: 0x0001C120
	public void BetaEquip()
	{
		InventoryModel.Instance.CreateEquipment(177777);
		InventoryModel.Instance.CreateEquipment(277777);
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x0001DF42 File Offset: 0x0001C142
	public void BetaStoryTester()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.ReturnToTitle();
		}
		GlobalGameManager.instance.loadingScene = "DefaultEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("StoryTester");
	}

	//> <Mod>
	public void CreateOverload(CreatureModel creature, OverloadType type = OverloadType.DEFAULT, float timer = 60f, bool isNatural = false)
	{
		if (type == OverloadType.DEFAULT)
		{
			creature.ActivateOverload(CreatureOverloadManager.instance.GetQliphothOverloadLevel(), timer, OverloadType.DEFAULT, isNatural);
			return;
		}
		Vestige.OvertimeOverloadManager.instance.MakeOverload(type, creature, timer, isNatural);
	}

	public void PrepareOvertime()
	{
		GlobalEtcDataModel.instance.unlockedMaxDay = 50;
		for (int i = 0; i < 10; i++)
		{
			if (i == 5)
			{
				i++;
			}
			SefiraEnum sefira = (SefiraEnum)i;
			Mission mission = MissionManager.instance.GetCurrentSefiraMission(sefira);
			if (mission != null)
			{
				mission.isCleared = true;
				MissionManager.instance.ClearMission(mission);
				MissionManager.instance.CloseClearedMission(mission);
			}
			for (int j = 0; j < 5; j++)
			{
				mission = MissionManager.instance.GetNextMission(sefira);
				if (mission == null || mission.metaInfo.sefira_Level > 5)
				{
					break;
				}
				MissionManager.instance.StartMission(mission.metaInfo.id);
				mission.isCleared = true;
				MissionManager.instance.ClearMission(mission);
				MissionManager.instance.CloseClearedMission(mission);
			}
			List<ResearchItemModel> research = ResearchDataModel.instance.GetModelBySefira(SefiraManager.instance.GetSefira(sefira).indexString);
			for (int j = 0; j < 3; j++)
			{
				ResearchDataModel.instance.UpgradeResearch(research[j].info.id, true);
			}
		}
	}

	public void ResetWhiteNight()
	{
		CreatureModel deathAngel = CreatureManager.instance.FindCreature(100015L);
		if (deathAngel != null)
		{
			CreatureManager.instance.ReplaceCreature(100014L, deathAngel);
		}
		string text = Application.persistentDataPath + "/creatureData/100014.dat";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
	}

	public void ReturnToMemRepo(int day)
	{
		if (GlobalGameManager.instance.ExistSaveData())
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			GameManager.currentGameManager.EndGame();
			GameManager.currentGameManager.ForceRelease();
			long num = -1L;
			while (PlayerModel.instance.GetWaitingCreature(out num))
			{
			}
			GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
			GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
			GlobalGameManager.instance.SaveGlobalData();
			GlobalGameManager.instance.ForcelyLoadDay(day + 9999, SaveType.CHECK_POINT);
			CreatureGenerate.CreatureGenerateInfoManager.Instance.Init();
			GlobalGameManager.instance.lastLoaded = true;
			GlobalGameManager.instance.loadingScene = "StoryEndScene";
			GlobalGameManager.instance.loadingScreen.LoadScene("Main");
		}
	}

	public void CompleteInventory()
	{
		foreach (EquipmentTypeInfo equipment in EquipmentTypeList.instance.GetAllData())
		{
			if (equipment.type == EquipmentTypeInfo.EquipmentType.SPECIAL) continue;
			if (equipment.type == EquipmentTypeInfo.EquipmentType.WEAPON && equipment.sprite == string.Empty) continue;
			if (equipment.type == EquipmentTypeInfo.EquipmentType.ARMOR && equipment.armorId == 0 && equipment.id != 22 || equipment.id == 200)  continue;
			for (int i = 0; i < equipment.MaxNum; i++)
			{
				InventoryModel.Instance.CreateEquipment(equipment.id);
			}
		}
	}

	public void MaxStatAllEmployees()
	{
		foreach (AgentModel agent in AgentManager.instance.GetAgentList())
		{
			agent.primaryStatExp.hp += 999;
			agent.primaryStatExp.mental += 999;
			agent.primaryStatExp.work += 999;
			agent.primaryStatExp.battle += 999;
		}
	}

	public void RemoveAllGifts(long id)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		foreach (EGOgiftModel egogiftModel in agent.GetAllGifts())
		{
			agent.ReleaseEGOgift(egogiftModel);
		}
	}

	public void ChangeTitleBonus(long id, int prefix, int suffix)
	{
		AgentModel agent = AgentManager.instance.GetAgent(id);
		agent.ForcelyChangePrefix(prefix);
		agent.ForcelyChangeSuffix(suffix);
	}

	public void SwapAbnormalities(CreatureModel creatureModel, CreatureModel creatureModel2)
	{
		CreatureManager.instance.ChangeCreaturePos(creatureModel, creatureModel2);
		if (!GameManager.currentGameManager.ManageStarted)
		{
			GlobalGameManager.instance.SaveData(false);
		}
		DeployUI.instance.sefiraList.Init();
		if (creatureModel.metaInfo.LcId == 100020L || creatureModel2.metaInfo.LcId == 100020L)
		{
			// GlobalGameManager.instance.LoadData(SaveType.LASTDAY);
		}
		else if (creatureModel.GetWorkspaceNode().GetPosition() - new Vector3(creatureModel.basePosition.x, creatureModel.basePosition.y, 0f) != creatureModel2.GetWorkspaceNode().GetPosition() - new Vector3(creatureModel2.basePosition.x, creatureModel2.basePosition.y, 0f))
		{
			// GlobalGameManager.instance.LoadData(SaveType.LASTDAY);
		}
	}
	//< <Mod>

	// Token: 0x06001B9F RID: 7071 RVA: 0x000E6194 File Offset: 0x000E4394
	static ConsoleCommand()
	{
	}

	// Token: 0x04001C60 RID: 7264
	public static string CreatureCommand = "creature";

	// Token: 0x04001C61 RID: 7265
	public static string StandardCommand = "standard";

	// Token: 0x04001C62 RID: 7266
	public static string AgentCommand = "agent";

	// Token: 0x04001C63 RID: 7267
	public static string OfficerCommand = "officer";

	// Token: 0x04001C64 RID: 7268
	public static string AngelaCommand = "angela";

	// Token: 0x04001C65 RID: 7269
	public static string BetaCommand = "beta";

	// Token: 0x04001C66 RID: 7270
	public static string RootCommand = "root";

	// <Mod>
	public static string ConfigCommand = "config";

	// Token: 0x04001C67 RID: 7271
	public static string AddCreatureFeeling = "addfeeling";

	// Token: 0x04001C68 RID: 7272
	public static string SubCreatureFeeling = "subfeeling";

	// Token: 0x04001C69 RID: 7273
	public static string SetCreatureObservable = "setobservable";

	// Token: 0x04001C6A RID: 7274
	public static string OpenListWindow = "agentlistwindow";

	// Token: 0x04001C6B RID: 7275
	public static string AddMoney = "salmonsushi";

	// Token: 0x04001C6C RID: 7276
	public static string AddSystemLog = "addsystemlog";

	// Token: 0x04001C6D RID: 7277
	public static string ActivateOrdeal = "ordeal";

	// Token: 0x04001C6E RID: 7278
	public static string FullAmmo = "sniperelite";

	// Token: 0x04001C6F RID: 7279
	public static string TakePhysicalDamage = "takephysicaldamage";

	// Token: 0x04001C70 RID: 7280
	public static string TakeMentalDamage = "takementaldamage";

	// Token: 0x04001C71 RID: 7281
	public static string EnergyFill = "expectopatronum";

	// Token: 0x04001C72 RID: 7282
	public static string MakePanicAll = "crusio";

	// Token: 0x04001C73 RID: 7283
	public static string AngelaDescMake = "angela";

	// Token: 0x04001C74 RID: 7284
	public static string ChangeLanguage = "language";

	// Token: 0x04001C75 RID: 7285
	public static string SuperSoldier = "captainlobotomy";

	// Token: 0x04001C76 RID: 7286
	public static string InventoryOpen = "jarvis";

	// Token: 0x04001C77 RID: 7287
	public static string EmregencyAdd = "dragons";

	// Token: 0x04001C78 RID: 7288
	public static string Encounter = "iamyourfather";

	// Token: 0x04001C79 RID: 7289
	public static string AddCumlativeCube = "cakeislie";

	// Token: 0x04001C7A RID: 7290
	public static string DamageInvoking = "bang";

	// Token: 0x04001C7B RID: 7291
	public static string DamageForcely = "damage";

	// Token: 0x04001C7C RID: 7292
	public static string GiftAdd = "present";

	// Token: 0x04001C7D RID: 7293
	public static string GiftRemove = "theft";

	// Token: 0x04001C7E RID: 7294
	public static string MakeEquipment = "forge";

	// Token: 0x04001C7F RID: 7295
	public static string QliportCounterReduce = "yannokakora";

	// Token: 0x04001C80 RID: 7296
	public static string InvokeOverload = "overload";

	// Token: 0x04001C81 RID: 7297
	public static string Boss = "bossiscoming";

	// Token: 0x04001C82 RID: 7298
	public static string SefiraBossConversation = "blahblah";

	// Token: 0x04001C83 RID: 7299
	public static string WaitingCreature = "fifo";

	// Token: 0x04001C84 RID: 7300
	public static string MissionClear = "mission";

	// Token: 0x04001C85 RID: 7301
	public static string AllocateAgents = "allinone";

	// Token: 0x04001C86 RID: 7302
	public static string RabbitProtocol = "rabbitprotocol";

	// Token: 0x04001C87 RID: 7303
	public static string PresentCluster = "merrychristmas";

	// Token: 0x04001C88 RID: 7304
	public static string ClearOverload = "clearoverload";

	// Token: 0x04001C89 RID: 7305
	public static string SuppressAll = "cryofbanshee";

	// Token: 0x04001C8A RID: 7306
	public static string ClearBoss = "checkmate";

	// Token: 0x04001C8B RID: 7307
	public static string ResearchAll = "alldone";

	// Token: 0x04001C8C RID: 7308
	public static string DeallocateAll = "iamfire";

	// Token: 0x04001C8D RID: 7309
	public static string MissionAdd = "missionimpossible";

	// Token: 0x04001C8E RID: 7310
	public List<string> creatureCommand;

	// Token: 0x04001C8F RID: 7311
	public List<string> standardCommand;

	// Token: 0x04001C90 RID: 7312
	public List<string> agentCommand;

	// Token: 0x04001C91 RID: 7313
	public List<string> officerCommand;

	// Token: 0x04001C92 RID: 7314
	public List<string> rootCommand;

	// <Mod>
	public List<string> configCommand;

	// Token: 0x04001C93 RID: 7315
	public Dictionary<string, string> lib;

	// Token: 0x04001C94 RID: 7316
	private static ConsoleCommand _instance;

	// <Mod>
	public List<ConsoleCommandsBase> moddedCommands;
}
