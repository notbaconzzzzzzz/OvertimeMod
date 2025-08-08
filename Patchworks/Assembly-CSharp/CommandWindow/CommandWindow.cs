/*
+public bool SelectionMode // 
+private bool _selectionMode // 
+public static CommandWindow CreateWindow(CommandType type, UnitModel target, bool mode) // 
public static CommandWindow CreateWindow(CommandType type, UnitModel target) // 
private void CheckMalkutBoss() // 
public void OnClick(AgentModel actor) // 
private void UpdateMouseSelectedList() // Fixed scrolling improperly
*/
using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000966 RID: 2406
	public class CommandWindow : MonoBehaviour, IObserver, IScrollMessageReciever, IScrollTarget
	{
		// Token: 0x0600484B RID: 18507 RVA: 0x001B0180 File Offset: 0x001AE380
		public CommandWindow()
		{
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600484C RID: 18508 RVA: 0x0003CA55 File Offset: 0x0003AC55
		public static CommandWindow CurrentWindow
		{
			get
			{
				return CommandWindow._currentWindow;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600484D RID: 18509 RVA: 0x0003CA5C File Offset: 0x0003AC5C
		// (set) Token: 0x0600484E RID: 18510 RVA: 0x0003CA64 File Offset: 0x0003AC64
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
				this.RootControl.SetActive(value);
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600484F RID: 18511 RVA: 0x0003CA79 File Offset: 0x0003AC79
		public Sefira CurrentSefira
		{
			get
			{
				return this._currentSefira;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x0003CA81 File Offset: 0x0003AC81
		public CommandType CurrentWindowType
		{
			get
			{
				return this._currentWindowType;
			}
		}

		// <Mod>
		public bool SelectionMode
		{
			get
			{
				return _selectionMode;
			}
		}

		private bool _selectionMode = false;

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06004851 RID: 18513 RVA: 0x0003CA89 File Offset: 0x0003AC89
		public UnitModel CurrentTarget
		{
			get
			{
				return this._currentTarget;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x0003CA91 File Offset: 0x0003AC91
		// (set) Token: 0x06004853 RID: 18515 RVA: 0x001B01CC File Offset: 0x001AE3CC
		public long SelectedWork
		{
			get
			{
				return this._selectedWork;
			}
			set
			{
				this._selectedWork = value;
				if (this._selectedWork == -1L)
				{
					this.ManagementAgentSlotParent.SetActive(false);
					this.SefiraMovementUI.ActiveControl.SetActive(false);
					return;
				}
				this.ManagementAgentSlotParent.SetActive(true);
				if (this.CheckSefiraMovementEnable() && UnitMouseEventManager.instance.GetSelectedAgents().Count == 0)
				{
					this.SefiraMovementUI.ActiveControl.SetActive(true);
				}
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x0003CA99 File Offset: 0x0003AC99
		public SkillTypeInfo CurrentSkill
		{
			get
			{
				return SkillTypeList.instance.GetData(this.SelectedWork);
			}
		}

		// <Mod>
		public static CommandWindow CreateWindow(CommandType type, UnitModel target, bool mode)
		{
			if (mode && type == CommandType.Management)
			{
				if (!CurrentWindow.IsEnabled)
				{
					CurrentWindow.IsEnabled = true;
				}
				CurrentWindow._currentWindowType = CommandType.Management;
				CurrentWindow._selectionMode = true;
				CurrentWindow._currentTarget = target;
				Sefira sefira = null;
				if (target is CreatureModel)
				{
					sefira = (target as CreatureModel).sefira;
				}
				else if (target is WorkerModel)
				{
					sefira = (target as WorkerModel).GetCurrentSefira();
				}
				CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
				if (CurrentWindow.CurrentSefira == null || CurrentWindow.CurrentSefira != sefira)
				{
					CurrentWindow._currentSefira = sefira;
				}
				if (UnitMouseEventManager.instance.GetSelectedAgents().Count > 0)
				{
					CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					CurrentWindow.page = 0;
					CurrentWindow.UpdateMouseSelectedList();
				}
				else
				{
					CurrentWindow.WorkScrollUp.SetActive(false);
					CurrentWindow.WorkScrollDown.SetActive(false);
					CurrentWindow.SetAgentList(type, CurrentWindow.CurrentSefira);
					if (CurrentWindow.CheckSefiraMovementEnable())
					{
						CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(true);
					}
					else
					{
						CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					}
				}
				CurrentWindow.PositionPivot.anchoredPosition = CurrentWindow.WorkCommmandPosition;
				CurrentWindow.SefiraMovementPivot.anchoredPosition = CurrentWindow.Position_Work;
				CurrentWindow.Suppress_ActiveControl.SetActive(false);
				CurrentWindow.WorkAllocate_ActiveControl.SetActive(true);
				CurrentWindow.Management_ActiveControl.SetActive(true);
				CurrentWindow.KitCreature_ActiveControl.SetActive(false);
				CurrentWindow.WorkAllocate.SetData(target);
				Button[] workButton = CurrentWindow.WorkButton;
				for (int i = 0; i < workButton.Length; i++)
				{
					workButton[i].interactable = true;
				}
				if (target is CreatureModel)
				{
					CreatureModel creatureModel = target as CreatureModel;
					for (int j = 1; j <= 4; j++)
					{
						Button button = CurrentWindow.WorkButton[j - 1];
						Image component = button.transform.Find("Icon").GetComponent<Image>();
						LocalizeTextLoadScript component2 = button.transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>();
						switch (j)
						{
						case 1:
							component.sprite = CurrentWindow.Work_R;
							component2.SetText("Rwork");
							break;
						case 2:
							component.sprite = CurrentWindow.Work_W;
							component2.SetText("Wwork");
							break;
						case 3:
							component.sprite = CurrentWindow.Work_B;
							component2.SetText("Bwork");
							break;
						case 4:
							component.sprite = CurrentWindow.Work_P;
							component2.SetText("Pwork");
							break;
						}
					}
					if (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
					{
						creatureModel.script.OnOpenCommandWindow(CurrentWindow.WorkButton);
					}
				}
				CurrentWindow.SelectedWork = -1L;
				CurrentWindow.CheckMalkutBoss();
				return CurrentWindow;
			}
			return CreateWindow(type, target);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x001B0240 File Offset: 0x001AE440
		public static CommandWindow CreateWindow(CommandType type, UnitModel target)
		{ // <Mod> Something?; Overtime Yesod Suppression
			if (!CommandWindow.CurrentWindow.IsEnabled)
			{
				CommandWindow.CurrentWindow.IsEnabled = true;
			}
			CommandWindow.CurrentWindow._currentWindowType = type;
			CurrentWindow._selectionMode = false;
			CommandWindow.CurrentWindow._currentTarget = target;
			Sefira sefira = null;
			if (target is CreatureModel)
			{
				sefira = (target as CreatureModel).sefira;
			}
			else if (target is WorkerModel)
			{
				sefira = (target as WorkerModel).GetCurrentSefira();
			}
			CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
			if (CommandWindow.CurrentWindow.CurrentSefira == null || CommandWindow.CurrentWindow.CurrentSefira != sefira)
			{
				CommandWindow.CurrentWindow._currentSefira = sefira;
			}
			if (type == CommandType.Management)
			{
				if (UnitMouseEventManager.instance.GetSelectedAgents().Count > 0)
				{
					CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					CommandWindow.CurrentWindow.page = 0;
					CommandWindow.CurrentWindow.UpdateMouseSelectedList();
				}
				else
				{
					CommandWindow.CurrentWindow.WorkScrollUp.SetActive(false);
					CommandWindow.CurrentWindow.WorkScrollDown.SetActive(false);
					CommandWindow.CurrentWindow.SetAgentList(type, CommandWindow.CurrentWindow.CurrentSefira);
					if (CommandWindow.CurrentWindow.CheckSefiraMovementEnable())
					{
						CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(true);
					}
					else
					{
						CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					}
				}
				CommandWindow.CurrentWindow.PositionPivot.anchoredPosition = CommandWindow.CurrentWindow.WorkCommmandPosition;
				CommandWindow.CurrentWindow.SefiraMovementPivot.anchoredPosition = CommandWindow.CurrentWindow.Position_Work;
				CommandWindow.CurrentWindow.Suppress_ActiveControl.SetActive(false);
				CommandWindow.CurrentWindow.WorkAllocate_ActiveControl.SetActive(true);
				CommandWindow.CurrentWindow.Management_ActiveControl.SetActive(true);
				CommandWindow.CurrentWindow.KitCreature_ActiveControl.SetActive(false);
				CommandWindow.CurrentWindow.WorkAllocate.SetData(target);
				Button[] workButton = CommandWindow.CurrentWindow.WorkButton;
				for (int i = 0; i < workButton.Length; i++)
				{
					workButton[i].interactable = true;
				}
				if (target is CreatureModel)
				{
					CreatureModel creatureModel = target as CreatureModel;
					for (int j = 1; j <= 4; j++)
					{
						Button button = CommandWindow.CurrentWindow.WorkButton[j - 1];
						Image component = button.transform.Find("Icon").GetComponent<Image>();
						LocalizeTextLoadScript component2 = button.transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>();
						switch (j)
						{
						case 1:
							component.sprite = CommandWindow.CurrentWindow.Work_R;
							component2.SetText("Rwork");
							break;
						case 2:
							component.sprite = CommandWindow.CurrentWindow.Work_W;
							component2.SetText("Wwork");
							break;
						case 3:
							component.sprite = CommandWindow.CurrentWindow.Work_B;
							component2.SetText("Bwork");
							break;
						case 4:
							component.sprite = CommandWindow.CurrentWindow.Work_P;
							component2.SetText("Pwork");
							break;
						}
					}
					if (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
					{
						creatureModel.script.OnOpenCommandWindow(CommandWindow.CurrentWindow.WorkButton);
					}
				}
				CommandWindow.CurrentWindow.SelectedWork = -1L;
			}
			else if (type == CommandType.KitCreature)
			{
				if (UnitMouseEventManager.instance.GetSelectedAgents().Count > 0)
				{
					CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					CommandWindow.CurrentWindow.page = 0;
					CommandWindow.CurrentWindow.UpdateMouseSelectedList();
				}
				else
				{
					CommandWindow.CurrentWindow.WorkScrollUp.SetActive(false);
					CommandWindow.CurrentWindow.WorkScrollDown.SetActive(false);
					CommandWindow.CurrentWindow.SetAgentList(type, CommandWindow.CurrentWindow.CurrentSefira);
					if (CommandWindow.CurrentWindow.CheckSefiraMovementEnable())
					{
						CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(true);
					}
					else
					{
						CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
					}
				}
				CommandWindow.CurrentWindow.PositionPivot.anchoredPosition = CommandWindow.CurrentWindow.WorkCommmandPosition;
				CommandWindow.CurrentWindow.SefiraMovementPivot.anchoredPosition = CommandWindow.CurrentWindow.Position_Work;
				CommandWindow.CurrentWindow.Suppress_ActiveControl.SetActive(false);
				CommandWindow.CurrentWindow.WorkAllocate_ActiveControl.SetActive(true);
				CommandWindow.CurrentWindow.Management_ActiveControl.SetActive(false);
				CommandWindow.CurrentWindow.KitCreature_ActiveControl.SetActive(true);
				CommandWindow.CurrentWindow.kitCreatureRegion.SetData(target);
				Button[] workButton = CommandWindow.CurrentWindow.WorkButton;
				for (int i = 0; i < workButton.Length; i++)
				{
					workButton[i].interactable = true;
				}
				if (target is CreatureModel)
				{
					CreatureModel creatureModel2 = target as CreatureModel;
					Image component3 = CommandWindow.CurrentWindow.WorkButton[2].transform.Find("Icon").GetComponent<Image>();
					if (creatureModel2.script is Freischutz)
					{
						component3.sprite = CommandWindow.CurrentWindow.Work_S;
						CommandWindow.CurrentWindow.WorkButton[2].transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>().SetText("Swork");
					}
					else
					{
						component3.sprite = CommandWindow.CurrentWindow.Work_B;
						CommandWindow.CurrentWindow.WorkButton[2].transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>().SetText("Bwork");
					}
				}
				CommandWindow.CurrentWindow.SelectedWork = -1L;
				CommandWindow.CurrentWindow.OnWorkSelect(5);
			}
			else
			{
				CommandWindow.CurrentWindow.WorkScrollUp.SetActive(false);
				CommandWindow.CurrentWindow.WorkScrollDown.SetActive(false);
				CommandWindow.CurrentWindow.SetAgentList(type, CommandWindow.CurrentWindow.CurrentSefira);
				if (CommandWindow.CurrentWindow.CheckSefiraMovementEnable())
				{
					CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(true);
				}
				else
				{
					CommandWindow.CurrentWindow.SefiraMovementUI.ActiveControl.SetActive(false);
				}
				CommandWindow.CurrentWindow.PositionPivot.anchoredPosition = CommandWindow.CurrentWindow.SuppressPosition;
				CommandWindow.CurrentWindow.SefiraMovementPivot.anchoredPosition = CommandWindow.CurrentWindow.Position_Suppress;
				CommandWindow.CurrentWindow.WorkAllocate_ActiveControl.SetActive(false);
				CommandWindow.CurrentWindow.Suppress_ActiveControl.SetActive(true);
				if (target is CreatureModel)
				{
					CreatureModel creatureModel3 = target as CreatureModel;
					if (creatureModel3.script is RedShoes)
					{
						target = ((creatureModel3.script as RedShoes).skill as RedShoesSkill).attractTargetAgent;
					}
				}
				if (target is CreatureModel)
				{
					CommandWindow.CurrentWindow.CretureSuppress.SetData(target);
					CommandWindow.CurrentWindow.WorkerSuppress.ActiveControl.SetActive(false);
					CommandWindow.CurrentWindow.CretureSuppress.ActiveControl.SetActive(true);
				}
				else if (target is WorkerModel)
				{
					CommandWindow.CurrentWindow.WorkerSuppress.SetData(target);
					CommandWindow.CurrentWindow.CretureSuppress.ActiveControl.SetActive(false);
					CommandWindow.CurrentWindow.WorkerSuppress.ActiveControl.SetActive(true);
				}
			}
			CommandWindow.CurrentWindow.CheckMalkutBoss();
			return CommandWindow.CurrentWindow;
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x001B090C File Offset: 0x001AEB0C
		private void CheckMalkutBoss()
		{ // <Mod>
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.MALKUT, false) || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1))
			{
				LocalizeTextLoadScript[] array = this.workNames;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetTextForcely("???");
				}
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001B0954 File Offset: 0x001AEB54
		private void Awake()
		{
			if (CommandWindow._currentWindow != null)
			{
				if (CommandWindow._currentWindow.gameObject != null)
				{
					UnityEngine.Object.Destroy(CommandWindow._currentWindow.gameObject);
				}
				CommandWindow._currentWindow = null;
			}
			CommandWindow._currentWindow = this;
			this.IsEnabled = false;
			Notice.instance.Observe(NoticeName.OnStageStart, this);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x000043A5 File Offset: 0x000025A5
		private void Start()
		{
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x000043A5 File Offset: 0x000025A5
		private void Update()
		{
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x0003CAAB File Offset: 0x0003ACAB
		private void OnDestroy()
		{
			Notice.instance.Remove(NoticeName.OnStageStart, this);
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0003CABD File Offset: 0x0003ACBD
		public void OnStageStart()
		{
			this.SefiraMovementUI.AreaInit();
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x000043A5 File Offset: 0x000025A5
		private void OnEnable()
		{
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x001B09B4 File Offset: 0x001AEBB4
		public void SetAgentList(CommandType type, Sefira sefira)
		{
			List<AgentModel> list = new List<AgentModel>(sefira.agentList.ToArray());
			int count = list.Count;
			this.SefiraMovementUI.SetCurrentSefira(sefira.sefiraEnum);
			this._currentSefira = sefira;
			bool flag = false;
			if (this.CurrentTarget is CreatureModel && (this.CurrentTarget as CreatureModel).metaInfo.creatureKitType == CreatureKitType.EQUIP)
			{
				flag = true;
			}
			List<AgentModel> list2 = new List<AgentModel>();
			bool flag2 = false;
			if (this.CurrentTarget is CreatureModel && type == CommandType.Management)
			{
				CreatureModel creatureModel = this.CurrentTarget as CreatureModel;
				if (creatureModel.script is OneBadManyGood && creatureModel.script.HasUniqueWorkSelect(1) == 6)
				{
					foreach (AgentModel agentModel in list)
					{
						if (!agentModel.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER) || agentModel.IsDead())
						{
							list2.Add(agentModel);
						}
					}
					flag2 = true;
				}
			}
			if (!flag2)
			{
				foreach (AgentModel agentModel2 in list)
				{
					if (type == CommandType.Management)
					{
						if (!agentModel2.CheckWorkCommand())
						{
							list2.Add(agentModel2);
						}
					}
					else if (type == CommandType.KitCreature)
					{
						if (!agentModel2.CheckWorkCommand())
						{
							list2.Add(agentModel2);
						}
						if (flag && agentModel2.Equipment.kitCreature != null)
						{
							list2.Add(agentModel2);
						}
					}
					else if (type == CommandType.Suppress && !agentModel2.CheckSuppressCommand())
					{
						list2.Add(agentModel2);
					}
				}
			}
			foreach (AgentModel item in list2)
			{
				list.Remove(item);
			}
			this.SetAgentList(type, list);
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x001B0BA8 File Offset: 0x001AEDA8
		public void SetAgentList(CommandType type, List<AgentModel> agents)
		{
			int count = agents.Count;
			if (type == CommandType.Management)
			{
				for (int i = 0; i < 5; i++)
				{
					if (i < count)
					{
						this.ManagementSlots[i].gameObject.SetActive(true);
						this.ManagementSlots[i].SetModel(agents[i]);
					}
					else
					{
						this.ManagementSlots[i].gameObject.SetActive(false);
					}
					this.ManagementSlots[i].SetCreature(this._currentTarget as CreatureModel);
				}
				return;
			}
			if (type == CommandType.KitCreature)
			{
				for (int j = 0; j < 5; j++)
				{
					if (j < count)
					{
						this.ManagementSlots[j].gameObject.SetActive(true);
						this.ManagementSlots[j].SetModel(agents[j]);
					}
					else
					{
						this.ManagementSlots[j].gameObject.SetActive(false);
					}
					this.ManagementSlots[j].SetCreature(this._currentTarget as CreatureModel);
				}
				return;
			}
			for (int k = 0; k < 5; k++)
			{
				if (k < count)
				{
					this.SuppressSlots[k].gameObject.SetActive(true);
					this.SuppressSlots[k].SetModel(agents[k]);
				}
				else
				{
					this.SuppressSlots[k].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x0003CACA File Offset: 0x0003ACCA
		public void CloseWindow()
		{
			this.IsEnabled = false;
			this.DeRegist();
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x001B0D08 File Offset: 0x001AEF08
		public void OnClick(AgentModel actor)
		{ // <Mod>
			if (actor == null)
			{
				return;
			}
			if (this.CurrentWindowType == CommandType.Management)
			{
				if (CommandWindow.isWorkOrderQueueEnabled && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
				{
					actor.ClearWorkOrderQueue();
					return;
				}
				CreatureModel creature = CurrentTarget as CreatureModel;
				IsolateRoom room = creature.Unit.room;
				if (SelectionMode || CommandWindow.isWorkOrderQueueEnabled && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || !(!room.IsWorking && !room.IsWorkAllocated && !creature.IsEscaped() && creature.script.IsWorkable() && creature.script.CanEnterRoom())))
				{
					string txt = "";
					if (!actor.CanQueueWorkOrder())
					{
						/*
						txt = "Cannot Queue -- Agent Invalid\n";
						txt += "Agent Queue:\n";
						foreach (QueuedWorkOrder order2 in actor.GetWorkOrderQueue())
						{
							txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
						}
						txt += "Creature Queue:\n";
						foreach (QueuedWorkOrder order2 in room.GetWorkOrderQueue())
						{
							txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
						}
						Notice.instance.Send(NoticeName.AddSystemLog, new object[]
						{
							txt
						});*/
						return;
					}
					if (!room.CanQueueWorkOrder())
					{
						/*
						txt = "Cannot Queue -- Creature Invalid\n";
						txt += "Agent Queue:\n";
						foreach (QueuedWorkOrder order2 in actor.GetWorkOrderQueue())
						{
							txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
						}
						txt += "Creature Queue:\n";
						foreach (QueuedWorkOrder order2 in room.GetWorkOrderQueue())
						{
							txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
						}
						Notice.instance.Send(NoticeName.AddSystemLog, new object[]
						{
							txt
						});*/
						return;
					}
					SkillTypeInfo data2 = SkillTypeList.instance.GetData(SelectedWork);
					QueuedWorkOrder order = new QueuedWorkOrder(actor, creature, data2);
					actor.EnqueueWorkOrder(order);
					room.EnqueueWorkOrder(order);
					/*
					txt = "Successfully Queued\n";
					txt += "Agent Queue:\n";
					foreach (QueuedWorkOrder order2 in actor.GetWorkOrderQueue())
					{
						txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
					}
					txt += "Creature Queue:\n";
					foreach (QueuedWorkOrder order2 in room.GetWorkOrderQueue())
					{
						txt += order2.agent.instanceId + " (" + order2.isAgentFront + "), " + order2.creature.instanceId + " (" + order2.isCreatureFront + ")\n";
					}
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						txt
					});*/
					return;
				}
				if (!actor.CheckWorkCommand())
				{
					CommandWindow.CurrentWindow.SetAgentList(this.CurrentWindowType, CommandWindow.CurrentWindow.CurrentSefira);
					return;
				}
				if (actor.GetState() == AgentAIState.MANAGE)
				{
					if (creature == actor.target)
					{
						if (actor.currentSkill == null)
						{
							actor.ForcelyCancelWork();
						}
						return;
					}
					if (actor.currentSkill == null)
					{
						actor.ForcelyCancelWork();
					}
					else
					{
						actor.StopAction();
					}
				}
				SkillTypeInfo data = SkillTypeList.instance.GetData(this.SelectedWork);
				actor.ManageCreature(creature, data, this.GetWorkSprite((RwbpType)this.SelectedWork));
				actor.counterAttackEnabled = false;
				room.OnWorkAllocated(actor);
				creature.script.OnWorkAllocated(data, actor);
				AngelaConversation.instance.MakeMessage(AngelaMessageState.MANAGE_START, new object[]
				{
					actor,
					data,
					creature
				});
				this.CloseWindow();
				return;
			}
			else
			{
				if (this.CurrentWindowType == CommandType.KitCreature)
				{
					CreatureModel creatureModel2 = this.CurrentTarget as CreatureModel;
					if (actor.GetState() == AgentAIState.MANAGE)
					{
						if (creatureModel2 == actor.target)
						{
							if (actor.currentSkill == null)
							{
								actor.ForcelyCancelWork();
							}
							return;
						}
						if (actor.currentSkill == null)
						{
							actor.ForcelyCancelWork();
						}
						else
						{
							actor.StopAction();
						}
					}
					actor.ManageKitCreature(creatureModel2);
					actor.counterAttackEnabled = false;
					creatureModel2.Unit.room.OnWorkAllocated(actor);
					this.CloseWindow();
					return;
				}
				if (actor.GetState() == AgentAIState.SUPPRESS_CREATURE && actor.target == this.CurrentTarget)
				{
					actor.ForcelyCancelSuppress();
					return;
				}
				if (actor.GetState() == AgentAIState.SUPPRESS_WORKER && actor.targetWorker == this.CurrentTarget)
				{
					actor.ForcelyCancelSuppress();
					return;
				}
				actor.Suppress(this.CurrentTarget, false);
				return;
			}
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0003CAD9 File Offset: 0x0003ACD9
		private int GetSelectedWorkId(int id)
		{
			return SefiraBossManager.Instance.GetWorkId(id);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x001B0EC8 File Offset: 0x001AF0C8
		public void OnWorkSelect(int id)
		{
			if (CommandWindow.CurrentWindow._currentTarget is CreatureModel)
			{
				CreatureModel creatureModel = CommandWindow.CurrentWindow._currentTarget as CreatureModel;
				id = creatureModel.script.HasUniqueWorkSelect(id);
				if (creatureModel.script.HasUniqueCommandAction(id))
				{
					CommandWindow.CurrentWindow.audioClipPlayer.OnPlayInList(1);
					this.CloseWindow();
					return;
				}
			}
			if (id == 7)
			{
				this.SelectedWork = 7L;
			}
			else if (id == 6)
			{
				this.SelectedWork = 6L;
			}
			else if (id != 5)
			{
				this.SelectedWork = (long)this.GetSelectedWorkId(id);
			}
			else
			{
				this.SelectedWork = 5L;
			}
			for (int i = 0; i < 4; i++)
			{
				Button button = this.WorkButton[i];
				if (id == 7 && i == 1)
				{
					button.interactable = false;
				}
				else if (i == id - 1)
				{
					button.interactable = false;
				}
				else
				{
					button.interactable = true;
					button.OnPointerExit(null);
				}
			}
			SkillTypeInfo data = SkillTypeList.instance.GetData((long)id);
			foreach (ManagementSlot managementSlot in this.ManagementSlots)
			{
				managementSlot.OnSelectWork(data);
			}
			CommandWindow.CurrentWindow.audioClipPlayer.OnPlayInList(1);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0003CAE6 File Offset: 0x0003ACE6
		public void OnNotice(string notice, params object[] param)
		{
			if (notice == NoticeName.OnStageStart)
			{
				this.OnStageStart();
			}
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0003CAFB File Offset: 0x0003ACFB
		public bool CheckSefiraMovementEnable()
		{
			return ResearchDataModel.instance.IsUpgradedAbility("cooperate_command");
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x001B1004 File Offset: 0x001AF204
		public void OnScroll(BaseEventData bData)
		{
			PointerEventData eventData = bData as PointerEventData;
			this.OnScroll(eventData);
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0003CB0C File Offset: 0x0003AD0C
		public void OnSefiraMove(SefiraEnum target)
		{
			if (this.CurrentSefira.sefiraEnum == target)
			{
				return;
			}
			if (!this.SefiraMovementUI.CheckContains(target))
			{
				return;
			}
			this.SetAgentList(this.CurrentWindowType, SefiraManager.instance.GetSefira(target));
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x001B1020 File Offset: 0x001AF220
		public void OnClickNextSefira()
		{
			SefiraEnum sefira = SefiraEnum.DUMMY;
			if (this.SefiraMovementUI.MoveNextSefira(this.CurrentSefira.sefiraEnum, out sefira))
			{
				this.SetAgentList(this.CurrentWindowType, SefiraManager.instance.GetSefira(sefira));
			}
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x001B1064 File Offset: 0x001AF264
		public void OnClickPrevSefira()
		{
			SefiraEnum sefira = SefiraEnum.DUMMY;
			if (this.SefiraMovementUI.MovePrevSefira(this.CurrentSefira.sefiraEnum, out sefira))
			{
				this.SetAgentList(this.CurrentWindowType, SefiraManager.instance.GetSefira(sefira));
			}
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x001B10A8 File Offset: 0x001AF2A8
		public void OnScroll(PointerEventData eventData)
		{
			if (this.CurrentWindowType == CommandType.Suppress && this.CurrentTarget is WorkerModel && UnitMouseEventManager.instance.GetSelectedAgents().Count == 1)
			{
				this.OnScroll_sefira(eventData);
				return;
			}
			if (UnitMouseEventManager.instance.GetSelectedAgents().Count == 0)
			{
				this.OnScroll_sefira(eventData);
				return;
			}
			this.OnScroll_mouseSelected(eventData);
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x001B1104 File Offset: 0x001AF304
		private void OnScroll_sefira(PointerEventData eventData)
		{
			if (!this.CheckSefiraMovementEnable())
			{
				return;
			}
			if (eventData.scrollDelta.y > 0f)
			{
				SefiraEnum sefira = SefiraEnum.DUMMY;
				if (this.SefiraMovementUI.MovePrevSefira(this.CurrentSefira.sefiraEnum, out sefira))
				{
					this.SetAgentList(this.CurrentWindowType, SefiraManager.instance.GetSefira(sefira));
					return;
				}
			}
			else if (eventData.scrollDelta.y < 0f)
			{
				SefiraEnum sefira2 = SefiraEnum.DUMMY;
				if (this.SefiraMovementUI.MoveNextSefira(this.CurrentSefira.sefiraEnum, out sefira2))
				{
					this.SetAgentList(this.CurrentWindowType, SefiraManager.instance.GetSefira(sefira2));
				}
			}
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x001B11A8 File Offset: 0x001AF3A8
		private void OnScroll_mouseSelected(PointerEventData eventData)
		{
			List<AgentModel> selectedAgents = UnitMouseEventManager.instance.GetSelectedAgents();
			if (eventData.scrollDelta.y > 0f)
			{
				if (this.page <= 0)
				{
					return;
				}
				this.page--;
			}
			else if (eventData.scrollDelta.y < 0f)
			{
				if (this.page + 1 > (selectedAgents.Count - 1) / 5)
				{
					return;
				}
				this.page++;
			}
			this.UpdateMouseSelectedList();
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x001B1228 File Offset: 0x001AF428
		private void UpdateMouseSelectedList()
		{ // <Mod>
			List<AgentModel> selectedAgents = UnitMouseEventManager.instance.GetSelectedAgents();
			List<AgentModel> list = new List<AgentModel>();
			foreach (AgentModel agentModel in selectedAgents)
			{
				if (this.CurrentWindowType == CommandType.Suppress)
				{
					if (!agentModel.CheckSuppressCommand())
					{
						list.Add(agentModel);
					}
				}
				else if (!agentModel.CheckWorkCommand())
				{
					list.Add(agentModel);
				}
			}
			foreach (AgentModel item in list)
			{
				selectedAgents.Remove(item);
			}
			this.SetAgentList(this.CurrentWindowType, selectedAgents.GetRange(this.page * 5, Mathf.Min(5, selectedAgents.Count - this.page * 5)));
			if (this.page <= 0)
			{
				this.WorkScrollUp.SetActive(false);
			}
			else
			{
				this.WorkScrollUp.SetActive(true);
			}
			if (this.page + 1 > (selectedAgents.Count - 1) / 5)
			{
				this.WorkScrollDown.SetActive(false);
				return;
			}
			this.WorkScrollDown.SetActive(true);
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x0003CB43 File Offset: 0x0003AD43
		public void Regist()
		{
			CameraMover.instance.Registration(this, false);
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x0003CB51 File Offset: 0x0003AD51
		public void DeRegist()
		{
			CameraMover.instance.DeRegistration();
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x000043A5 File Offset: 0x000025A5
		public void AddTrigger()
		{
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x001B1370 File Offset: 0x001AF570
		public Sprite GetWorkSprite(RwbpType type)
		{
			switch (type)
			{
			case RwbpType.N:
				if (this.SelectedWork == 6L)
				{
					return this.Work_C;
				}
				if (this.SelectedWork == 7L)
				{
					return this.Work_I;
				}
				return this.Work_R;
			case RwbpType.R:
				return this.Work_R;
			case RwbpType.W:
				return this.Work_W;
			case RwbpType.B:
				return this.Work_B;
			case RwbpType.P:
				return this.Work_P;
			default:
				if (this.SelectedWork == 6L)
				{
					return this.Work_C;
				}
				if (this.SelectedWork == 7L)
				{
					return this.Work_I;
				}
				return this.Work_R;
			}
		}

		// <Mod>
		public static bool isWorkOrderQueueEnabled
		{
			get
			{
				if (SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
				{
					return ResearchDataModel.instance.IsUpgradedAbility("work_order_queue");
				}
				return SpecialModeConfig.instance.GetValue<bool>("WorkOrderQueue");
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x000043A5 File Offset: 0x000025A5
		static CommandWindow()
		{
		}

		// Token: 0x040042B5 RID: 17077
		public const int RWork = 1;

		// Token: 0x040042B6 RID: 17078
		public const int WWork = 2;

		// Token: 0x040042B7 RID: 17079
		public const int BWork = 3;

		// Token: 0x040042B8 RID: 17080
		public const int PWork = 4;

		// Token: 0x040042B9 RID: 17081
		private static CommandWindow _currentWindow;

		// Token: 0x040042BA RID: 17082
		private bool _isEnabled;

		// Token: 0x040042BB RID: 17083
		private Sefira _currentSefira;

		// Token: 0x040042BC RID: 17084
		private CommandType _currentWindowType = CommandType.Management;

		// Token: 0x040042BD RID: 17085
		[NonSerialized]
		private UnitModel _currentTarget;

		// Token: 0x040042BE RID: 17086
		private long _selectedWork = -1L;

		// Token: 0x040042BF RID: 17087
		[NonSerialized]
		public int page;

		// Token: 0x040042C0 RID: 17088
		public GameObject RootControl;

		// Token: 0x040042C1 RID: 17089
		public RectTransform PositionPivot;

		// Token: 0x040042C2 RID: 17090
		public RectTransform SefiraMovementPivot;

		// Token: 0x040042C3 RID: 17091
		public AudioClipPlayer audioClipPlayer;

		// Token: 0x040042C4 RID: 17092
		public GameObject WorkAllocate_ActiveControl;

		// Token: 0x040042C5 RID: 17093
		[Header("Management")]
		public GameObject Management_ActiveControl;

		// Token: 0x040042C6 RID: 17094
		public WorkAllocateRegion WorkAllocate;

		// Token: 0x040042C7 RID: 17095
		public Button[] WorkButton;

		// Token: 0x040042C8 RID: 17096
		public GameObject WorkScrollUp;

		// Token: 0x040042C9 RID: 17097
		public GameObject WorkScrollDown;

		// Token: 0x040042CA RID: 17098
		public LocalizeTextLoadScript[] workNames;

		// Token: 0x040042CB RID: 17099
		[Header("KitCreature")]
		public GameObject KitCreature_ActiveControl;

		// Token: 0x040042CC RID: 17100
		public KitCreatureRegion kitCreatureRegion;

		// Token: 0x040042CD RID: 17101
		[Header("Suppress")]
		public GameObject Suppress_ActiveControl;

		// Token: 0x040042CE RID: 17102
		public CreatureSuppressRegion CretureSuppress;

		// Token: 0x040042CF RID: 17103
		public WorkerSuppressRegion WorkerSuppress;

		// Token: 0x040042D0 RID: 17104
		[Header("SefiraMovement")]
		public SefiraMovement SefiraMovementUI;

		// Token: 0x040042D1 RID: 17105
		[Header("Agent Slots")]
		public GameObject ManagementAgentSlotParent;

		// Token: 0x040042D2 RID: 17106
		public List<ManagementSlot> ManagementSlots;

		// Token: 0x040042D3 RID: 17107
		public List<SuppressSlot> SuppressSlots;

		// Token: 0x040042D4 RID: 17108
		[Header("Color")]
		public Color DeadColor;

		// Token: 0x040042D5 RID: 17109
		public Color PanicColor;

		// Token: 0x040042D6 RID: 17110
		public Color UnconColor;

		// Token: 0x040042D7 RID: 17111
		public Color WorkingColor;

		// Token: 0x040042D8 RID: 17112
		public Color SuppressingColor;

		// Token: 0x040042D9 RID: 17113
		public Color OrderColor;

		// Token: 0x040042DA RID: 17114
		public Color SefiraDisabledColor;

		// Token: 0x040042DB RID: 17115
		public Color CancelOrcerColor;

		// Token: 0x040042DC RID: 17116
		[Header("WorkIcon")]
		public Sprite Work_R;

		// Token: 0x040042DD RID: 17117
		public Sprite Work_W;

		// Token: 0x040042DE RID: 17118
		public Sprite Work_B;

		// Token: 0x040042DF RID: 17119
		public Sprite Work_P;

		// Token: 0x040042E0 RID: 17120
		public Sprite Work_S;

		// Token: 0x040042E1 RID: 17121
		public Sprite Work_V;

		// Token: 0x040042E2 RID: 17122
		public Sprite Work_C;

		// Token: 0x040042E3 RID: 17123
		public Sprite Work_I;

		// Token: 0x040042E4 RID: 17124
		public Sprite Work_Protection;

		// Token: 0x040042E5 RID: 17125
		[Header("ColorSet")]
		public Color imageColor;

		// Token: 0x040042E6 RID: 17126
		public Color textColor;

		// Token: 0x040042E7 RID: 17127
		[Header("Position")]
		public Vector2 WorkCommmandPosition;

		// Token: 0x040042E8 RID: 17128
		public Vector2 SuppressPosition;

		// Token: 0x040042E9 RID: 17129
		[NonSerialized]
		public Vector2 Position_Work = new Vector2(398f, 41f);

		// Token: 0x040042EA RID: 17130
		public Vector2 Position_Suppress = new Vector2(402f, 230f);

		// Token: 0x040042EB RID: 17131
		public float LeftPos;

		// Token: 0x040042EC RID: 17132
		public float RightPos;
	}
}
