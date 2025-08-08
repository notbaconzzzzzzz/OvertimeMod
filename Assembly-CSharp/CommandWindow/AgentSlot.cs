/*
public virtual void SetUI(AgentModel agent) // Ego Gift Helper
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000956 RID: 2390
	public class AgentSlot : MonoBehaviour
	{
		// Token: 0x060047DE RID: 18398 RVA: 0x0003C463 File Offset: 0x0003A663
		public AgentSlot()
		{
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060047DF RID: 18399 RVA: 0x0003C472 File Offset: 0x0003A672
		// (set) Token: 0x060047E0 RID: 18400 RVA: 0x0003C47A File Offset: 0x0003A67A
		public bool OverlayEntered
		{
			get
			{
				return this._overlayEntered;
			}
			set
			{
				this._overlayEntered = value;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x0003C483 File Offset: 0x0003A683
		// (set) Token: 0x060047E2 RID: 18402 RVA: 0x0003C48B File Offset: 0x0003A68B
		public AgentState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this._state = value;
					this.SetFilter(value);
				}
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060047E3 RID: 18403 RVA: 0x0003C4A7 File Offset: 0x0003A6A7
		private bool IsOrderable
		{
			get
			{
				return this.State == AgentState.IDLE || this.State == AgentState.SUPPRESSING || (this.State == AgentState.MOVING && SefiraBossManager.Instance.IsWorkCancelable);
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060047E4 RID: 18404 RVA: 0x0003C4DD File Offset: 0x0003A6DD
		public AgentModel CurrentAgent
		{
			get
			{
				return this._currentAgent;
			}
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0003C4E5 File Offset: 0x0003A6E5
		public static float GetRate(float value, float max)
		{
			return value / max;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x0003C4EA File Offset: 0x0003A6EA
		public virtual void SetModel(AgentModel agent)
		{
			this._currentAgent = agent;
			this.SetUI(agent);
			this.CheckAgentState();
			this.SetFilter(this.State);
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0003C50C File Offset: 0x0003A70C
		private void OnEnable()
		{
			this.CheckAgentState();
			this.SetFilter(this.State);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x001AB994 File Offset: 0x001A9B94
		public virtual void SetUI(AgentModel agent)
		{
			this.Grade.sprite = DeployUI.GetAgentGradeSprite(agent);
			this.AgentName.text = agent.GetUnitName();
			this.CheckAgentState();
			this.SetFilter(this.State);
			this.portrait.SetWorker(agent);
			this.UpdateUI();
			this.WorkFilterControl.SetActive(false);
			if (this.State == AgentState.MOVING)
			{
				if (SefiraBossManager.Instance.IsWorkCancelable)
				{
					this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Cancel");
				}
				else
				{
					this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Cancel_Fail");
				}
			}
			else
			{
				UnitModel currentTarget = CommandWindow.CurrentWindow.CurrentTarget;
				this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_OrderManagement");
			}
			string id = string.Empty;
			float movementValue = agent.GetMovementValue();
			if (movementValue < 5f)
			{
				id = "Slowest";
			}
			else if (movementValue < 6f)
			{
				id = "Slow";
			}
			else if (movementValue < 7f)
			{
				id = "Normal";
			}
			else if (movementValue < 8f)
			{
				id = "Fast";
			}
			else
			{
				id = "Fastest";
			}
			this.WorkFilterSubText.text = LocalizeTextDataModel.instance.GetText("AgentState_MovementPrefix") + " : " + LocalizeTextDataModel.instance.GetText(id);
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x001ABB0C File Offset: 0x001A9D0C
		public virtual void UpdateUI()
		{
			this.CheckAgentState();
			this.SetFilter(this.State);
			if (this.CurrentAgent == null)
			{
				return;
			}
			this.HealthFill.fillAmount = AgentSlot.GetRate(this.CurrentAgent.hp, (float)this.CurrentAgent.maxHp);
			this.MentalFill.fillAmount = AgentSlot.GetRate(this.CurrentAgent.mental, (float)this.CurrentAgent.maxMental);
			int num = (int)this.CurrentAgent.hp;
			int num2 = (int)this.CurrentAgent.mental;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.HealthText.text = num + "/" + this.CurrentAgent.maxHp;
			this.MentalText.text = num2 + "/" + this.CurrentAgent.maxMental;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x001ABC08 File Offset: 0x001A9E08
		public void OnClickInfo()
		{
			if (this.CurrentAgent == null)
			{
				return;
			}
			if (AgentInfoWindow.currentWindow.PinnedAgent == this.CurrentAgent)
			{
				AgentInfoWindow.currentWindow.UnPinCurrentAgent();
			}
			else
			{
				AgentInfoWindow.currentWindow.PinCurrentAgent();
			}
			CommandWindow.CurrentWindow.audioClipPlayer.OnPlayInList(1);
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x000040A1 File Offset: 0x000022A1
		public virtual void SetUIColor()
		{
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x001ABC60 File Offset: 0x001A9E60
		public virtual void SetColor(Color c)
		{
			foreach (MaskableGraphic maskableGraphic in this.coloredTargets)
			{
				maskableGraphic.color = c;
			}
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x001ABCBC File Offset: 0x001A9EBC
		public virtual void CheckAgentState()
		{
			if (this.CurrentAgent == null)
			{
				this.State = AgentState.IDLE;
				return;
			}
			if (this.CurrentAgent.IsDead())
			{
				this.State = AgentState.DEAD;
				return;
			}
			if (this.CurrentAgent.IsPanic())
			{
				this.State = AgentState.PANIC;
				return;
			}
			if (this.CurrentAgent.IsCrazy())
			{
				this.State = AgentState.UNCONTROLLABLE;
				return;
			}
			if (this.CurrentAgent.IsSuppressing())
			{
				this.State = AgentState.SUPPRESSING;
				return;
			}
			if (this.CurrentAgent.GetState() == AgentAIState.MANAGE)
			{
				if (this.CurrentAgent.currentSkill == null)
				{
					this.State = AgentState.MOVING;
				}
				else
				{
					this.State = AgentState.WORKING;
				}
				return;
			}
			this.State = AgentState.IDLE;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x001ABD78 File Offset: 0x001A9F78
		public virtual void SetFilter(AgentState state)
		{
			try
			{
				if (CommandWindow.CurrentWindow)
				{
					if (this._state != AgentState.DEAD && this._state != AgentState.PANIC && this._state != AgentState.UNCONTROLLABLE)
					{
						if (this.State == AgentState.MOVING)
						{
							if (SefiraBossManager.Instance.IsWorkCancelable)
							{
								this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Cancel");
							}
							else
							{
								this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Cancel_Fail");
							}
							this.WorkFilterFill.color = CommandWindow.CurrentWindow.CancelOrcerColor;
						}
						else if (CommandWindow.CurrentWindow)
						{
							if (CommandWindow.CurrentWindow.CurrentWindowType == CommandType.Management)
							{
								this.WorkFilterFill.color = CommandWindow.CurrentWindow.OrderColor;
								SkillTypeInfo currentSkill = CommandWindow.CurrentWindow.CurrentSkill;
								if (currentSkill != null)
								{
									string text = string.Empty;
									text = currentSkill.name;
									string str = this.CurrentAgent.StatLevel((RwbpType)currentSkill.id).ToString();
									if (SefiraBossManager.Instance.CurrentActivatedSefira == SefiraEnum.MALKUT || CommandWindow.CurrentWindow.SelectedWork == 6L || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1))
									{
										text = "???";
										str = "?";
									}
									text = "Lv " + str + " " + text;
									this.WorkFilterText.text = text;
								}
							}
							else if (CommandWindow.CurrentWindow.CurrentWindowType == CommandType.KitCreature)
							{
								this.WorkFilterFill.color = CommandWindow.CurrentWindow.OrderColor;
								this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_KitOrderManagement");
							}
							else if (CommandWindow.CurrentWindow.CurrentWindowType == CommandType.Suppress)
							{
								this.WorkFilterFill.color = CommandWindow.CurrentWindow.SuppressingColor;
								this.WorkFilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_OrderSuppress");
							}
						}
					}
					switch (state)
					{
					case AgentState.DEAD:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.DeadColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Dead");
						this.SetColor(CommandWindow.CurrentWindow.DeadColor);
						break;
					case AgentState.PANIC:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.PanicColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Panic");
						this.SetColor(CommandWindow.CurrentWindow.PanicColor);
						break;
					case AgentState.UNCONTROLLABLE:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.UnconColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Uncon");
						this.SetColor(CommandWindow.CurrentWindow.UnconColor);
						break;
					case AgentState.WORKING:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.WorkingColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Working");
						break;
					case AgentState.SUPPRESSING:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.SuppressingColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Suppressing");
						break;
					case AgentState.MOVING:
						this.FilterControl.SetActive(true);
						this.FilterFill.color = CommandWindow.CurrentWindow.WorkingColor;
						this.FilterText.text = LocalizeTextDataModel.instance.GetText("AgentState_Moving");
						break;
					default:
						this.FilterControl.SetActive(false);
						this.SetColor(CommandWindow.CurrentWindow.imageColor);
						break;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x001AC19C File Offset: 0x001AA39C
		public virtual void OnOverlayEnter(int index)
		{
			if (this.CurrentAgent != null)
			{
				AgentInfoWindow.CreateWindow(this.CurrentAgent, false);
			}
			if (index == 0)
			{
				this.OnOverlayEnter();
			}
			else
			{
				CommandWindow.CurrentWindow.Regist();
			}
			CommandWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0003C520 File Offset: 0x0003A720
		public virtual void OnOvelrayExit(int index)
		{
			if (AgentInfoWindow.currentWindow.IsEnabled)
			{
				AgentInfoWindow.currentWindow.OnClearOverlay();
			}
			if (index == 0)
			{
				this.OnOvelrayExit();
			}
			else
			{
				CommandWindow.CurrentWindow.DeRegist();
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x001AC1EC File Offset: 0x001AA3EC
		public virtual void OnOverlayEnter()
		{
			if (this.State == AgentState.DEAD || this.State == AgentState.PANIC || this.State == AgentState.UNCONTROLLABLE)
			{
				return;
			}
			if (this.IsOrderable)
			{
				this._overlayEntered = true;
				this.WorkFilterControl.SetActive(true);
				this.FilterControl.SetActive(false);
				this.SetFilter(this.State);
				if (CommandWindow.CurrentWindow.CurrentWindowType == CommandType.Management || CommandWindow.CurrentWindow.CurrentWindowType == CommandType.KitCreature)
				{
					this.SetColor(CommandWindow.CurrentWindow.OrderColor);
				}
				else
				{
					this.SetColor(CommandWindow.CurrentWindow.SuppressingColor);
				}
			}
			CommandWindow.CurrentWindow.Regist();
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x001AC2A4 File Offset: 0x001AA4A4
		public virtual void OnOvelrayExit()
		{
			if (this.State == AgentState.DEAD || this.State == AgentState.PANIC || this.State == AgentState.UNCONTROLLABLE)
			{
				return;
			}
			this._overlayEntered = false;
			if (this.WorkFilterControl.activeInHierarchy)
			{
				this.WorkFilterControl.SetActive(false);
			}
			if (this.IsOrderable)
			{
				this.CheckAgentState();
				this.SetFilter(this.State);
			}
			CommandWindow.CurrentWindow.DeRegist();
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0003C556 File Offset: 0x0003A756
		public virtual void OnClick()
		{
			if (this.IsOrderable)
			{
				CommandWindow.CurrentWindow.OnClick(this.CurrentAgent);
			}
			AgentInfoWindow.currentWindow.PinCurrentAgent();
			this.UpdateUI();
			CommandWindow.CurrentWindow.audioClipPlayer.OnPlayInList(2);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0003C593 File Offset: 0x0003A793
		private void Update()
		{
			this.UpdateUI();
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0003C59B File Offset: 0x0003A79B
		private void Start()
		{
			this.WorkFilterFill.color = CommandWindow.CurrentWindow.OrderColor;
		}

		// Token: 0x04004228 RID: 16936
		public GameObject ActiveControl;

		// Token: 0x04004229 RID: 16937
		public GameObject Portrait;

		// Token: 0x0400422A RID: 16938
		public WorkerPortraitSetter portrait;

		// Token: 0x0400422B RID: 16939
		public Image Grade;

		// Token: 0x0400422C RID: 16940
		public Text AgentName;

		// Token: 0x0400422D RID: 16941
		public Image HealthFill;

		// Token: 0x0400422E RID: 16942
		public Text HealthText;

		// Token: 0x0400422F RID: 16943
		public Image MentalFill;

		// Token: 0x04004230 RID: 16944
		public Text MentalText;

		// Token: 0x04004231 RID: 16945
		[Header("Filter")]
		public GameObject FilterControl;

		// Token: 0x04004232 RID: 16946
		public Image FilterFill;

		// Token: 0x04004233 RID: 16947
		public Text FilterText;

		// Token: 0x04004234 RID: 16948
		public GameObject WorkFilterControl;

		// Token: 0x04004235 RID: 16949
		public Image WorkFilterFill;

		// Token: 0x04004236 RID: 16950
		public Text WorkFilterText;

		// Token: 0x04004237 RID: 16951
		public Text WorkFilterSubText;

		// Token: 0x04004238 RID: 16952
		public List<MaskableGraphic> coloredTargets;

		// Token: 0x04004239 RID: 16953
		private bool _overlayEntered;

		// Token: 0x0400423A RID: 16954
		private AgentState _state = AgentState.IDLE;

		// Token: 0x0400423B RID: 16955
		[NonSerialized]
		private AgentModel _currentAgent;
	}
}
