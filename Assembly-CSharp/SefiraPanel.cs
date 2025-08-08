using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000992 RID: 2450
public class SefiraPanel : MonoBehaviour, IDeployResetCalled
{
	// Token: 0x06004A4A RID: 19018 RVA: 0x0003E0A4 File Offset: 0x0003C2A4
	public SefiraPanel()
	{
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0003E0E2 File Offset: 0x0003C2E2
	public Sefira Sefira
	{
		get
		{
			return this._sefira;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06004A4C RID: 19020 RVA: 0x0003E0EA File Offset: 0x0003C2EA
	public CanvasGroup Group
	{
		get
		{
			if (this._group == null)
			{
				this._group = base.gameObject.AddComponent<CanvasGroup>();
			}
			return this._group;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0003E114 File Offset: 0x0003C314
	public bool IsActivated
	{
		get
		{
			return this.Sefira.activated;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0003E121 File Offset: 0x0003C321
	public SefiraUIColor SefiraUIColor
	{
		get
		{
			if (this._sefiraUIColor == null)
			{
				this._sefiraUIColor = UIColorManager.instance.GetSefiraColor(this._sefira);
			}
			return this._sefiraUIColor;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06004A4F RID: 19023 RVA: 0x0003E14A File Offset: 0x0003C34A
	public Color SefiraColor
	{
		get
		{
			return this._sefiraColor;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0003E152 File Offset: 0x0003C352
	public GameObject ActiveControl
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06004A51 RID: 19025 RVA: 0x0003E15A File Offset: 0x0003C35A
	public int CurrentAgentCount
	{
		get
		{
			return this._currentAgentCount;
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06004A52 RID: 19026 RVA: 0x0003E162 File Offset: 0x0003C362
	// (set) Token: 0x06004A53 RID: 19027 RVA: 0x0003E16A File Offset: 0x0003C36A
	public SefiraPanel.PanelDataState PanelState
	{
		get
		{
			return this._dataState;
		}
		set
		{
			if (this.LinkedSefira == null)
			{
				this.OnSetPanel(value);
			}
			else
			{
				this.LinkedSefira.OnSetPanel(value);
			}
		}
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06004A54 RID: 19028 RVA: 0x0003E195 File Offset: 0x0003C395
	// (set) Token: 0x06004A55 RID: 19029 RVA: 0x0003E19D File Offset: 0x0003C39D
	public SefiraPanel.PanelDataState OverlayState
	{
		get
		{
			return this._overlayState;
		}
		set
		{
			this.OnOverlayPanel(value);
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06004A56 RID: 19030 RVA: 0x0003E1A6 File Offset: 0x0003C3A6
	private bool _isAllcoateable
	{
		get
		{
			return this._currentAgentCount < this._allocateMax;
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06004A57 RID: 19031 RVA: 0x0003E1B6 File Offset: 0x0003C3B6
	public bool DragEntered
	{
		get
		{
			return this.dragEntered;
		}
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x001B7A00 File Offset: 0x001B5C00
	private void OnOverlayPanel(SefiraPanel.PanelDataState state)
	{
		this._overlayState = state;
		switch (state)
		{
		case SefiraPanel.PanelDataState.MAIN:
			this.mainRoot.gameObject.SetActive(true);
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(false);
			break;
		case SefiraPanel.PanelDataState.RESEARCH:
			this.mainRoot.gameObject.SetActive(false);
			this.leftRoot.gameObject.SetActive(true);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(false);
			break;
		case SefiraPanel.PanelDataState.DESCRIPTION:
			this.mainRoot.gameObject.SetActive(false);
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(true);
			this.functionRoot.gameObject.SetActive(false);
			break;
		case SefiraPanel.PanelDataState.FUNCTION:
			this.mainRoot.gameObject.SetActive(false);
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(true);
			break;
		}
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x001B7B54 File Offset: 0x001B5D54
	private void OnSetPanel(SefiraPanel.PanelDataState state)
	{
		this._dataState = state;
		if (this.DescriptionButton == null || this.ResearchButton == null || this.FunctionButton == null)
		{
			this.mainRoot.gameObject.SetActive(true);
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(false);
			return;
		}
		if (this.missionUI != null && this.missionUI.hasUniqueDisable)
		{
			if (state == SefiraPanel.PanelDataState.MAIN)
			{
				this.missionUI.gameObject.SetActive(true);
			}
			else if (!this.missionUI.notDisabled)
			{
				this.missionUI.gameObject.SetActive(false);
			}
		}
		switch (state)
		{
		case SefiraPanel.PanelDataState.MAIN:
			this.DescriptionButton.interactable = true;
			this.ResearchButton.interactable = true;
			this.FunctionButton.interactable = true;
			this.DescriptionButton.OnPointerExit(null);
			this.ResearchButton.OnPointerExit(null);
			this.FunctionButton.OnPointerExit(null);
			this.mainRoot.gameObject.SetActive(true);
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(false);
			if (this.SefiraBossButton != null && !this.SefiraBossButton.GetComponent<Animator>().GetBool("IsSelected"))
			{
				this.mainRoot.GetChild(0).gameObject.SetActive(true);
			}
			break;
		case SefiraPanel.PanelDataState.RESEARCH:
			this.DescriptionButton.interactable = true;
			this.ResearchButton.interactable = false;
			this.FunctionButton.interactable = true;
			this.DescriptionButton.OnPointerExit(null);
			this.FunctionButton.OnPointerExit(null);
			if (!this.DoNotCloseMain)
			{
				this.mainRoot.gameObject.SetActive(false);
			}
			else
			{
				this.mainRoot.GetChild(0).gameObject.SetActive(false);
			}
			this.leftRoot.gameObject.SetActive(true);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(false);
			break;
		case SefiraPanel.PanelDataState.DESCRIPTION:
			this.DescriptionButton.interactable = false;
			this.ResearchButton.interactable = true;
			this.FunctionButton.interactable = true;
			this.ResearchButton.OnPointerExit(null);
			this.FunctionButton.OnPointerExit(null);
			if (!this.DoNotCloseMain)
			{
				this.mainRoot.gameObject.SetActive(false);
			}
			else
			{
				this.mainRoot.GetChild(0).gameObject.SetActive(false);
			}
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(true);
			this.functionRoot.gameObject.SetActive(false);
			break;
		case SefiraPanel.PanelDataState.FUNCTION:
			this.DescriptionButton.interactable = true;
			this.ResearchButton.interactable = true;
			this.FunctionButton.interactable = false;
			this.ResearchButton.OnPointerExit(null);
			this.DescriptionButton.OnPointerExit(null);
			if (!this.DoNotCloseMain)
			{
				this.mainRoot.gameObject.SetActive(false);
			}
			else
			{
				this.mainRoot.GetChild(0).gameObject.SetActive(false);
			}
			this.leftRoot.gameObject.SetActive(false);
			this.rightRoot.gameObject.SetActive(false);
			this.functionRoot.gameObject.SetActive(true);
			break;
		}
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x001B7F30 File Offset: 0x001B6130
	private void ScrollInit(Transform root)
	{
		IEnumerator enumerator = root.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				MaskableGraphic component = transform.GetComponent<MaskableGraphic>();
				if (component != null && component.raycastTarget && transform.GetComponent<ScrollExchanger>() == null && transform.GetComponent<ScrollRect>() == null)
				{
					ScrollExchanger scrollExchanger = transform.gameObject.AddComponent<ScrollExchanger>();
					scrollExchanger.messageReceiver = DeployUI.instance;
					scrollExchanger.MessageRecieveInit();
				}
				this.ScrollInit(transform);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0003E1BE File Offset: 0x0003C3BE
	public void InitResearch()
	{
		if (this.researchPanelController != null)
		{
			this.researchPanelController.Init(this.Sefira);
		}
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x001B7FF0 File Offset: 0x001B61F0
	public void Init(SefiraEnum sefira)
	{
		this._sefira = SefiraManager.instance.GetSefira(sefira);
		this._sefiraColor = UIColorManager.instance.GetSefiraColor(this.Sefira).imageColor;
		this.ActiveControl.SetActive(SefiraManager.instance.IsOpened(sefira));
		this.dropScript.SetDropEvent(new Drop.OnDropEvent(this.DropEvent));
		if (this.SefiraName != null)
		{
			this.SefiraName.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				this.Sefira.name,
				"Name"
			});
		}
		if (this.MissionPanelAnim != null)
		{
			this.MissionPanelAnim.enabled = false;
		}
		if (this.DescriptionText != null)
		{
			this.DescriptionText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Allocate",
				"DescriptionButton"
			});
		}
		if (this.ResearchText != null)
		{
			this.ResearchText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Allocate",
				"ResearchButton"
			});
		}
		if (this.FunctionText != null)
		{
			this.FunctionText.text = LocalizeTextDataModel.instance.GetText("department_ability");
		}
		if (this.Sefira.sefiraEnum == SefiraEnum.DAAT)
		{
			base.gameObject.SetActive(false);
		}
		List<CreatureModel> creatureList = this.Sefira.creatureList;
		int count = creatureList.Count;
		this.SetSclae(1f);
		int sefiraLevel = (int)this.Sefira.sefiraEnum / (int)SefiraEnum.TIPERERTH1;
		for (int i = 0; i < ((this.Sefira.sefiraEnum == SefiraEnum.KETHER) ? 8 : 4); i++)
		{
			SefiraPanel.CreaturePortrait creaturePortrait = this.creatureSlots[i];
			creaturePortrait.index = i;
			if (i < count)
			{
				creaturePortrait.SetCreature(creatureList[i], sefiraLevel, this.SefiraColor);
				TooltipMouseOver component = UnityEngine.Object.Instantiate<GameObject>(this.TooltipObj, creaturePortrait.portrait.transform).GetComponent<TooltipMouseOver>();
				component.ID = "Tooltip_ManagementPart@Creature_Encyclopedia";
			}
			else
			{
				creaturePortrait.SetEmpty(this._sefiraColor);
			}
		}
		this.ScrollInit(base.transform);
		this.Registration();
		foreach (ScrollExchanger scrollExchanger in this.scrollExchanged)
		{
			scrollExchanger.scrollRect = DeployUI.instance.sefiraList.scrollRect;
		}
		this.allocateAgents.Clear();
		foreach (AgentModel item in this.Sefira.agentList)
		{
			this.allocateAgents.Add(item);
		}
		this._allocateMax = this.Sefira.allocateMax;
		this.AgentListInit();
		this.CreaturePortraitInit();
		Color c;
		if (!this.IsActivated)
		{
			this.Group.interactable = false;
			this.Group.blocksRaycasts = false;
			c = Color.gray;
		}
		else
		{
			this.Group.interactable = true;
			this.Group.blocksRaycasts = true;
			c = DeployUI.instance.DeployColorSet[0];
		}
		this.decorations.Init(this.IsActivated, c);
		foreach (GameObject gameObject in this.activateEffected)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(this.IsActivated);
			}
		}
		this.InitResearch();
		this.SetTextureColor();
		this.SetAlterGraphicsColor(this.SefiraColor);
		this.SetDescriptionText();
		this.missionPanelStartTimer.StartTimer(UnityEngine.Random.Range(0.5f, 1.5f));
		if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
		{
			if (this.missionUI != null)
			{
				if (this.missionUI.hasUniqueDisable)
				{
					this.missionUI.UniqueDisable();
				}
				else
				{
					this.missionUI.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			Mission currentSefiraMission = MissionManager.instance.GetCurrentSefiraMission(this.Sefira.sefiraEnum);
			if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
			{
				currentSefiraMission = MissionManager.instance.GetCurrentSefiraMission(SefiraEnum.DUMMY);
			}
			if (this.missionUI == null && this.MissionPanelAnim != null)
			{
				this.missionUI = this.MissionPanelAnim.GetComponent<SefiraPanelMissionUI>();
			}
			if (this.missionUI != null)
			{
				if (currentSefiraMission != null)
				{
					List<string> list;
					if (!SefiraBossManager.Instance.IsBossStartable(this.Sefira.sefiraEnum, out list) && SefiraBossManager.Instance.IsBossReady(this.Sefira.sefiraEnum))
					{
						if (list.Count > 0)
						{
							this.missionUI.InitRequireBossStarting(list);
						}
						else if (this.missionUI.hasUniqueDisable)
						{
							this.missionUI.UniqueDisable();
						}
						else
						{
							this.missionUI.gameObject.SetActive(false);
						}
					}
					else
					{
						this.missionUI.InitProgressMission(currentSefiraMission);
					}
				}
				else
				{
					List<string> list2;
					bool flag;
					MissionManager.instance.GetAvailableMission(this.Sefira.sefiraEnum, out list2, out flag);
					if (list2.Count != 0)
					{
						if (flag)
						{
							this.missionUI.InitRequireBossStarting(list2);
						}
						else
						{
							this.missionUI.InitRequireMission(list2);
						}
					}
					else if (this.missionUI.hasUniqueDisable)
					{
						this.missionUI.UniqueDisable();
					}
					else
					{
						this.missionUI.gameObject.SetActive(false);
					}
				}
			}
		}
		if (this.Function_SefiraFunction_Context != null)
		{
			int[] officerAliveValues = SefiraAbilityValueInfo.GetOfficerAliveValues(sefira);
			Sefira sefira2 = this._sefira;
			if (this._sefira.sefiraEnum == SefiraEnum.TIPERERTH2)
			{
				sefira2 = SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH1);
			}
			this.Function_SefiraFunction_Context.text = string.Format(LocalizeTextDataModel.instance.GetText("officer_alive_ability_desc_" + sefira2.name.ToLower()), officerAliveValues[1], officerAliveValues[2], officerAliveValues[3]);
		}
		if (this.Function_SefiraTenure_Context != null)
		{
			int[] continuousServiceValues = SefiraAbilityValueInfo.GetContinuousServiceValues(sefira);
			this.Function_SefiraTenure_Context.text = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_desc_" + this._sefira.name.ToLower()), continuousServiceValues[0], continuousServiceValues[1], continuousServiceValues[2]);
		}
		this.SetBossState();
		if (this.SefiraBossButton != null)
		{
			this.SetBossPanel(this.SefiraBossButton.GetComponent<Animator>().GetBool("IsSelected"));
		}
		this.PanelState = SefiraPanel.PanelDataState.MAIN;
		if (SefiraBossManager.Instance.CurrentActivatedSefira == SefiraEnum.KETHER && this.SefiraBossButton != null)
		{
			this.SefiraBossButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x001B8788 File Offset: 0x001B6988
	public void SetDescriptionText()
	{
		if (this.DescriptionTextArea != null)
		{
			this.DescriptionTextArea.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				this.Sefira.name,
				"LongDesc"
			});
		}
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0003E1E2 File Offset: 0x0003C3E2
	public bool DropEvent(params object[] param)
	{
		return param[0] is AgentModel && this.AddAgent((AgentModel)param[0]);
	}

	// Token: 0x06004A5F RID: 19039 RVA: 0x001B87D8 File Offset: 0x001B69D8
	private void SetTextureColor()
	{
		Color imageColor = this.SefiraUIColor.imageColor;
		if (!this.Sefira.activated)
		{
			imageColor.r = 0.4f;
			imageColor.g = 0.4f;
			imageColor.b = 0.4f;
		}
		foreach (MaskableGraphic maskableGraphic in this.graphics)
		{
			maskableGraphic.color = imageColor;
		}
		if (this.FrameOutline != null)
		{
			this.FrameOutline.effectColor = imageColor;
		}
	}

	// Token: 0x06004A60 RID: 19040 RVA: 0x001B8894 File Offset: 0x001B6A94
	public void SetAlterGraphicsColor(Color altercolor)
	{
		foreach (MaskableGraphic maskableGraphic in this.alterGraphics)
		{
			maskableGraphic.color = altercolor;
		}
	}

	// Token: 0x06004A61 RID: 19041 RVA: 0x001B88F0 File Offset: 0x001B6AF0
	private void AgentListInit()
	{
		this._currentAgentCount = 0;
		for (int i = 0; i < this.agentSlots.Length; i++)
		{
			this.agentSlots[i].index = i;
			this.agentSlots[i].panel = this;
			this.agentSlots[i].GetComponent<DragOverDrop>().dropScript = this.dropScript;
			this.agentSlots[i].ui.SetColor(this.SefiraUIColor.imageColor);
		}
		foreach (AgentModel agent in this.allocateAgents)
		{
			this.InitialAddAgent(agent);
		}
		for (int j = this._currentAgentCount; j < this._allocateMax; j++)
		{
			this.agentSlots[j].SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
		}
		for (int k = this._allocateMax; k < 5; k++)
		{
			this.agentSlots[k].SetPanelState(DeploySefiraAgentSlot.PanelState.LOCKED);
		}
	}

	// Token: 0x06004A62 RID: 19042 RVA: 0x0003E201 File Offset: 0x0003C401
	private void Start()
	{
		if (this.SefiraBossButton != null)
		{
			this.SefiraBossButton.GetComponent<Animator>().SetBool("IsSelected", false);
		}
	}

	// Token: 0x06004A63 RID: 19043 RVA: 0x0000431D File Offset: 0x0000251D
	private void OnDestroy()
	{
	}

	// Token: 0x06004A64 RID: 19044 RVA: 0x001B8A14 File Offset: 0x001B6C14
	private void Update()
	{
		if (this.removeCoolTimer.RunTimer())
		{
		}
		if (this.missionPanelStartTimer.RunTimer() && this.MissionPanelAnim != null)
		{
			this.MissionPanelAnim.enabled = true;
			this.MissionPanelAnim.speed = UnityEngine.Random.Range(1f, 1.5f);
		}
		if (this.dragEntered)
		{
			this.SetSclae(1.1f);
			if (Input.GetMouseButtonUp(0))
			{
				this.OnPanelExit();
				this.SetSclae(1f);
			}
		}
		else
		{
			if (this.LinkedSefira != null && this.LinkedSefira.DragEntered)
			{
				return;
			}
			this.SetSclae(1f);
		}
		foreach (SefiraPanel.CreaturePortrait creaturePortrait in this.creatureSlots)
		{
			creaturePortrait.UpdateCheck();
		}
	}

	// Token: 0x06004A65 RID: 19045 RVA: 0x001B8B04 File Offset: 0x001B6D04
	public bool AddAgent(AgentModel agent)
	{
		if (this.allocateAgents.Count >= 5)
		{
			return false;
		}
		if (this.allocateAgents.Count == this._allocateMax)
		{
			return false;
		}
		if (this.Sefira.sefiraEnum == SefiraEnum.DAAT)
		{
			return false;
		}
		if (this.Sefira.sefiraEnum != SefiraEnum.KETHER && SefiraBossManager.Instance.CheckBossActivation(this.Sefira.sefiraEnum))
		{
			return false;
		}
		if (this.allocateAgents.Contains(agent))
		{
			return false;
		}
		agent.SetCurrentSefira(this.Sefira.indexString);
		this.allocateAgents.Add(agent);
		int currentAgentCount = this._currentAgentCount;
		this.agentSlots[currentAgentCount].SetModel(agent);
		this.agentSlots[currentAgentCount].ui.SetColor(this.SefiraUIColor.imageColor);
		agent.GetMovableNode().SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(this.Sefira.indexString));
		this._currentAgentCount++;
		this.SefiraSound.OnPlayInList(2);
		return true;
	}

	// Token: 0x06004A66 RID: 19046 RVA: 0x001B8C1C File Offset: 0x001B6E1C
	public void InitialAddAgent(AgentModel agent)
	{
		agent.SetCurrentSefira(this.Sefira.indexString);
		int currentAgentCount = this._currentAgentCount;
		this.agentSlots[currentAgentCount].SetModel(agent);
		this.agentSlots[currentAgentCount].ui.SetColor(this.SefiraUIColor.imageColor);
		agent.GetMovableNode().SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(this.Sefira.indexString));
		this._currentAgentCount++;
	}

	// Token: 0x06004A67 RID: 19047 RVA: 0x001B8C9C File Offset: 0x001B6E9C
	public void OnRemoveAgent(int index)
	{
		if (index < this._currentAgentCount)
		{
			AgentModel model = this.agentSlots[index].Model;
			if (model == null)
			{
				this.agentSlots[index].SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
				this.agentSlots[index].ui.SetColor(this.SefiraUIColor.imageColor);
			}
			if (this.RemoveAgent(model))
			{
				DeployUI.instance.agentList.AddAgent(model);
			}
		}
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x001B8D18 File Offset: 0x001B6F18
	public void OnRemoveAgent_NotAddToList(int index)
	{
		if (index < this._currentAgentCount)
		{
			AgentModel model = this.agentSlots[index].Model;
			if (model == null)
			{
				this.agentSlots[index].SetActive(false);
				this.agentSlots[index].ui.SetColor(this.SefiraUIColor.imageColor);
			}
			if (this.RemoveAgentNotRemoved(model))
			{
			}
		}
	}

	// Token: 0x06004A69 RID: 19049 RVA: 0x001B8D84 File Offset: 0x001B6F84
	public bool RemoveAgent(AgentModel agent)
	{
		if (agent == null)
		{
			return false;
		}
		if (this.removeCoolTimer.started)
		{
			return false;
		}
		this.removeCoolTimer.StartTimer(0.2f);
		if (this.allocateAgents.Contains(agent))
		{
			this.allocateAgents.Remove(agent);
			for (int i = 0; i < 5; i++)
			{
				DeploySefiraAgentSlot deploySefiraAgentSlot = this.agentSlots[i];
				if (deploySefiraAgentSlot.Model == agent)
				{
					int num = i;
					deploySefiraAgentSlot.ui.SetNormalColor(this.SefiraUIColor.imageColor);
					this.agentSlots[num].SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
					this.agentSlots[num].ui.SetColor(this.SefiraUIColor.imageColor);
					agent.SetCurrentSefira("0");
					for (int j = i; j < this._currentAgentCount; j++)
					{
						if (j < 4)
						{
							DeploySefiraAgentSlot deploySefiraAgentSlot2 = this.agentSlots[j];
							DeploySefiraAgentSlot deploySefiraAgentSlot3 = this.agentSlots[j + 1];
							if (deploySefiraAgentSlot3.IsActivated)
							{
								if (deploySefiraAgentSlot3.GetPanelState() == DeploySefiraAgentSlot.PanelState.FILLED)
								{
									deploySefiraAgentSlot2.SetModel(deploySefiraAgentSlot3.Model);
									deploySefiraAgentSlot2.ui.SetColor(this.SefiraUIColor.imageColor);
								}
								if (deploySefiraAgentSlot3.GetPanelState() != DeploySefiraAgentSlot.PanelState.LOCKED)
								{
									deploySefiraAgentSlot3.SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
									deploySefiraAgentSlot3.ui.SetColor(this.SefiraUIColor.imageColor);
								}
							}
						}
					}
					break;
				}
			}
			this._currentAgentCount--;
			this.SefiraSound.OnPlayInList(3);
			return true;
		}
		return false;
	}

	// Token: 0x06004A6A RID: 19050 RVA: 0x001B8F10 File Offset: 0x001B7110
	public bool RemoveAgentNotRemoved(AgentModel agent)
	{
		if (agent == null)
		{
			return false;
		}
		if (this.removeCoolTimer.started)
		{
			return false;
		}
		this.removeCoolTimer.StartTimer(0.2f);
		if (this.allocateAgents.Contains(agent))
		{
			this.allocateAgents.Remove(agent);
			for (int i = 0; i < 5; i++)
			{
				DeploySefiraAgentSlot deploySefiraAgentSlot = this.agentSlots[i];
				if (deploySefiraAgentSlot.Model == agent)
				{
					int num = i;
					deploySefiraAgentSlot.ui.SetNormalColor(this.SefiraUIColor.imageColor);
					this.agentSlots[num].SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
					for (int j = i; j < this._currentAgentCount; j++)
					{
						if (j < 4)
						{
							DeploySefiraAgentSlot deploySefiraAgentSlot2 = this.agentSlots[j];
							DeploySefiraAgentSlot deploySefiraAgentSlot3 = this.agentSlots[j + 1];
							if (deploySefiraAgentSlot3.IsActivated)
							{
								if (deploySefiraAgentSlot3.GetPanelState() == DeploySefiraAgentSlot.PanelState.FILLED)
								{
									deploySefiraAgentSlot2.SetModel(deploySefiraAgentSlot3.Model);
								}
								if (deploySefiraAgentSlot3.GetPanelState() != DeploySefiraAgentSlot.PanelState.LOCKED)
								{
									deploySefiraAgentSlot3.SetPanelState(DeploySefiraAgentSlot.PanelState.EMPTY);
									deploySefiraAgentSlot3.ui.SetNormalColor(this.SefiraUIColor.imageColor);
								}
							}
						}
					}
					break;
				}
			}
			this._currentAgentCount--;
			return true;
		}
		return false;
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x0003E22A File Offset: 0x0003C42A
	public void OnClickAgentSlot(int index)
	{
		this._currentClickTarget = index;
	}

	// Token: 0x06004A6C RID: 19052 RVA: 0x001B9050 File Offset: 0x001B7250
	public void OnClickAgentSlot(BaseEventData bData)
	{
		PointerEventData pointerEventData = bData as PointerEventData;
		if (pointerEventData.clickCount > 1)
		{
			Debug.Log("Double");
			this.OnRemoveAgent(this._currentClickTarget);
			this._currentClickTarget = -1;
		}
		else if (this._currentClickTarget < this._currentAgentCount)
		{
			AgentModel model = this.agentSlots[this._currentClickTarget].Model;
			if (model != null)
			{
				if (AgentInfoWindow.currentWindow.IsEnabled)
				{
					if (AgentInfoWindow.currentWindow.CurrentAgent == model)
					{
						AgentInfoWindow.currentWindow.UnPinCurrentAgent();
					}
					else
					{
						AgentInfoWindow.currentWindow.PinCurrentAgent();
					}
				}
				AgentInfoWindow.CreateWindow(model, false);
			}
		}
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x0003E233 File Offset: 0x0003C433
	private void ResetPanel()
	{
		if (this.researchPanelController != null)
		{
			this.researchPanelController.ResetAll();
		}
		this.PanelState = SefiraPanel.PanelDataState.MAIN;
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x0003E258 File Offset: 0x0003C458
	public void OnClickPanel()
	{
		this.ResetPanel();
	}

	// Token: 0x06004A6F RID: 19055 RVA: 0x0003E260 File Offset: 0x0003C460
	public void OnClickResearch()
	{
		this.PanelState = SefiraPanel.PanelDataState.RESEARCH;
		this.SefiraSound.OnPlayInList(0);
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x0003E275 File Offset: 0x0003C475
	public void OnClickDescription()
	{
		this.PanelState = SefiraPanel.PanelDataState.DESCRIPTION;
		this.SefiraSound.OnPlayInList(0);
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x0003E28A File Offset: 0x0003C48A
	public void OnClickSefiraFunction()
	{
		this.PanelState = SefiraPanel.PanelDataState.FUNCTION;
		this.SefiraSound.OnPlayInList(0);
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x0001EE9D File Offset: 0x0001D09D
	public void OnOverlayEnter(int index)
	{
		if (index == 0)
		{
		}
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x0001EE9D File Offset: 0x0001D09D
	public void OnOverlayExit(int index)
	{
		if (index == 0)
		{
		}
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x0003E29F File Offset: 0x0003C49F
	public void Registration()
	{
		DeployUI.instance.RegistDeployReset(this);
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x0003E2AC File Offset: 0x0003C4AC
	public void DeployResetCalled()
	{
		if (!this.IsActivated)
		{
			return;
		}
		this.ResetPanel();
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x001B9104 File Offset: 0x001B7304
	public void OnPanelEnter(BaseEventData bData)
	{
		if (!this._isAllcoateable)
		{
			return;
		}
		PointerEventData pointerEventData = bData as PointerEventData;
		if (pointerEventData.dragging && pointerEventData.pointerDrag != null)
		{
			MonoBehaviour[] components = pointerEventData.pointerDrag.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in components)
			{
				if (monoBehaviour is IDraggableObject)
				{
					this.dragEntered = true;
					this.dropScript.transform.SetAsLastSibling();
					this.OnClickPanel();
				}
			}
		}
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x0003E2C0 File Offset: 0x0003C4C0
	public void OnPanelExit()
	{
		if (this.dragEntered)
		{
			this.dragEntered = false;
			this.dropScript.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06004A78 RID: 19064 RVA: 0x001B9194 File Offset: 0x001B7394
	public void OnAgentSlotEnter(DeploySefiraAgentSlot slot)
	{
		if (slot.GetPanelState() == DeploySefiraAgentSlot.PanelState.FILLED)
		{
			slot.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			slot.ui.SetNormalColor(DeployUI.instance.UIOverlayColor);
			AgentInfoWindow.CreateWindow(slot.Model, false);
		}
	}

	// Token: 0x06004A79 RID: 19065 RVA: 0x001B91F4 File Offset: 0x001B73F4
	public void OnAgentSlotExit(DeploySefiraAgentSlot slot)
	{
		slot.ui.SetNormalColor(this.SefiraColor);
		slot.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		if (AgentInfoWindow.currentWindow.IsEnabled && AgentInfoWindow.currentWindow.WindowType == AgentInfoWindow.AgentInfoWindowType.NORMAL)
		{
			AgentInfoWindow.currentWindow.OnClearOverlay();
		}
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x001B9260 File Offset: 0x001B7460
	public void SetSclae(float factor)
	{
		foreach (ScaleSetter scaleSetter in this.scaleSetted)
		{
			scaleSetter.scaleFactor = factor;
		}
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x0000431D File Offset: 0x0000251D
	public void OnManageStart()
	{
	}

	// Token: 0x06004A7C RID: 19068 RVA: 0x001B92BC File Offset: 0x001B74BC
	public void OnClickCreaturePortriat(int i)
	{
		if (this.creatureSlots[i].isInit)
		{
			if (!this.creatureSlots[i].creature.script.OnOpenCollectionWindow())
			{
				return;
			}
			CreatureInfoWindow.CreateWindow(this.creatureSlots[i].creature.metadataId);
			this.SefiraSound.OnPlayInList(1);
		}
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x001B931C File Offset: 0x001B751C
	public void OnCreaturePortraitEnter(int i)
	{
		if (this.creatureSlots[i].isInit)
		{
			this.creatureSlots[i].portrait.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
			this.SefiraSound.OnPlayInList(0);
		}
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x0003E2E4 File Offset: 0x0003C4E4
	public void OnCreaturePortraitExit(int i)
	{
		if (this.creatureSlots[i].isInit)
		{
			this.creatureSlots[i].portrait.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x001B9374 File Offset: 0x001B7574
	public void CreaturePortraitInit()
	{
		int num = (this.Sefira.sefiraEnum != SefiraEnum.KETHER) ? 4 : 8;
		for (int i = 0; i < num; i++)
		{
			SefiraPanel.CreaturePortrait portrait = this.creatureSlots[i];
			Transform parent = portrait.portrait.transform.parent;
			EventTrigger eventTrigger = parent.GetComponent<EventTrigger>();
			if (eventTrigger == null)
			{
				eventTrigger = parent.gameObject.AddComponent<EventTrigger>();
			}
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.OnClickCreaturePortriat(portrait.index);
			});
			eventTrigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerEnter;
			entry2.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.OnCreaturePortraitEnter(portrait.index);
			});
			eventTrigger.triggers.Add(entry2);
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			entry3.eventID = EventTriggerType.PointerExit;
			entry3.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.OnCreaturePortraitExit(portrait.index);
			});
			eventTrigger.triggers.Add(entry3);
		}
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x0000431D File Offset: 0x0000251D
	public void DeployColorSetted(Color c)
	{
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x001B94A0 File Offset: 0x001B76A0
	public void OnDeallocateAll()
	{
		int num = this.CurrentAgentCount - 1;
		for (int i = num; i >= 0; i--)
		{
			this.OnRemoveAgent(i);
			this.removeCoolTimer.StopTimer();
		}
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x001B94DC File Offset: 0x001B76DC
	public void SetBossState()
	{
		if (this.sefiraBossDescPanel != null)
		{
			this.sefiraBossDescPanel.gameObject.SetActive(false);
		}
		if (this.SefiraBossButton != null)
		{
			if (SefiraBossManager.Instance.IsBossReady(this.Sefira.sefiraEnum) && this.Sefira.sefiraEnum != SefiraEnum.KETHER)
			{
				this.SefiraBossButton.gameObject.SetActive(true);
				if (SefiraBossManager.Instance.IsBossStartable(this.Sefira.sefiraEnum))
				{
					this.SefiraBossButton.interactable = true;
					if (this.SefiraBossButtonText != null)
					{
						this.SefiraBossButtonText.text = LocalizeTextDataModel.instance.GetText("SefiraBoss_Button_Normal");
					}
				}
				else
				{
					this.SefiraBossButton.interactable = false;
					if (this.SefiraBossButtonText != null)
					{
						this.SefiraBossButtonText.text = LocalizeTextDataModel.instance.GetText("SefiraBoss_Button_Cannot");
					}
				}
			}
			else
			{
				this.SefiraBossButton.gameObject.SetActive(false);
			}
		}
		SefiraEnum sefiraEnum = this.Sefira.sefiraEnum;
		if (sefiraEnum == SefiraEnum.TIPERERTH2)
		{
			sefiraEnum = SefiraEnum.TIPERERTH1;
		}
		if (sefiraEnum == SefiraEnum.KETHER)
		{
			int num = 0;
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1))
			{
				num = 1;
			}
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E2))
			{
				num = 2;
			}
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E3))
			{
				num = 3;
			}
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
			{
				num = 4;
			}
			if (this.SefiraBossDesc_Clear != null)
			{
				this.SefiraBossDesc_Clear.text = LocalizeTextDataModel.instance.GetText(string.Format("boss_{0}_clear_{1}", sefiraEnum.ToString().ToLower(), num));
			}
			if (this.SefiraBossDesc_Condition != null)
			{
				this.SefiraBossDesc_Condition.text = LocalizeTextDataModel.instance.GetText(string.Format("boss_{0}_condition_{1}", sefiraEnum.ToString().ToLower(), num));
			}
		}
		else
		{
			if (this.SefiraBossDesc_Clear != null)
			{
				this.SefiraBossDesc_Clear.text = LocalizeTextDataModel.instance.GetText(string.Format("boss_{0}_clear", sefiraEnum.ToString().ToLower()));
			}
			if (this.SefiraBossDesc_Condition != null)
			{
				this.SefiraBossDesc_Condition.text = LocalizeTextDataModel.instance.GetText(string.Format("boss_{0}_condition", sefiraEnum.ToString().ToLower()));
			}
		}
		if (SefiraBossManager.Instance.IsKetherBoss() && sefiraEnum == SefiraEnum.KETHER)
		{
			if (this.sefiraBossDescPanel != null)
			{
				this.sefiraBossDescPanel.Show();
			}
			if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
			{
				this.Kether_Chain.SetActive(true);
				this.Kether_Pivot.SetActive(false);
			}
			else
			{
				this.Kether_Chain.SetActive(false);
				this.Kether_Pivot.SetActive(true);
			}
		}
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x001B97F4 File Offset: 0x001B79F4
	public void OnStartBossSession()
	{
		if (DeployUI.instance.OnClickSefiraBossSession(this.Sefira.sefiraEnum))
		{
			this.PanelState = SefiraPanel.PanelDataState.MAIN;
			this.OnDeallocateAll();
			this.SetBossPanel(true);
			if (this.sefiraBossDescPanel != null)
			{
				this.sefiraBossDescPanel.Show();
			}
			if (this.Sefira.sefiraEnum == SefiraEnum.TIPERERTH1)
			{
				SefiraPanel sefiraPanel = DeployUI.instance.sefiraList.GetSefiraPanel(SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).indexString);
				if (sefiraPanel != null)
				{
					sefiraPanel.PanelState = SefiraPanel.PanelDataState.MAIN;
					sefiraPanel.OnDeallocateAll();
					sefiraPanel.SetBossPanel(true);
					if (sefiraPanel.sefiraBossDescPanel != null)
					{
						sefiraPanel.sefiraBossDescPanel.Show();
					}
				}
			}
		}
		else
		{
			this.DisableSefiraBoss();
			if (this.Sefira.sefiraEnum == SefiraEnum.TIPERERTH1)
			{
				SefiraPanel sefiraPanel2 = DeployUI.instance.sefiraList.GetSefiraPanel(SefiraManager.instance.GetSefira(SefiraEnum.TIPERERTH2).indexString);
				if (sefiraPanel2 != null)
				{
					sefiraPanel2.DisableSefiraBoss();
				}
			}
		}
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x001B9908 File Offset: 0x001B7B08
	public void SetBossPanel(bool state)
	{
		foreach (GameObject gameObject in this.sefiraBossRelated)
		{
			gameObject.SetActive(!state);
		}
		if (this.SefiraBossButton != null)
		{
			this.SefiraBossButton.GetComponent<Animator>().SetBool("IsSelected", state);
			this.SefiraBossButton.GetComponent<Animator>().SetTrigger("Normal");
		}
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x0003E314 File Offset: 0x0003C514
	public void OnBossDescDisabled()
	{
		this.SetBossPanel(false);
		this.PanelState = SefiraPanel.PanelDataState.MAIN;
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x0003E324 File Offset: 0x0003C524
	public void DisableSefiraBoss()
	{
		if (this.sefiraBossDescPanel != null)
		{
			this.sefiraBossDescPanel.Hide();
		}
	}

	// Token: 0x040044D6 RID: 17622
	public const int maxAgentCount = 5;

	// Token: 0x040044D7 RID: 17623
	private const string noDataPortraitSrc = "Sprites/Unit/creature/NoData";

	// Token: 0x040044D8 RID: 17624
	private const float _logoDefault = 0.98f;

	// Token: 0x040044D9 RID: 17625
	private const float _logoTrans = 1.02f;

	// Token: 0x040044DA RID: 17626
	private Sefira _sefira;

	// Token: 0x040044DB RID: 17627
	[NonSerialized]
	public List<AgentModel> allocateAgents = new List<AgentModel>();

	// Token: 0x040044DC RID: 17628
	public List<MaskableGraphic> graphics;

	// Token: 0x040044DD RID: 17629
	public List<MaskableGraphic> alterGraphics;

	// Token: 0x040044DE RID: 17630
	public Outline FrameOutline;

	// Token: 0x040044DF RID: 17631
	[Space(15f)]
	public DeploySefiraAgentSlot[] agentSlots;

	// Token: 0x040044E0 RID: 17632
	public SefiraPanel.CreaturePortrait[] creatureSlots;

	// Token: 0x040044E1 RID: 17633
	public Drop dropScript;

	// Token: 0x040044E2 RID: 17634
	public Text SefiraName;

	// Token: 0x040044E3 RID: 17635
	public RectTransform leftRoot;

	// Token: 0x040044E4 RID: 17636
	public RectTransform rightRoot;

	// Token: 0x040044E5 RID: 17637
	public RectTransform mainRoot;

	// Token: 0x040044E6 RID: 17638
	public RectTransform functionRoot;

	// Token: 0x040044E7 RID: 17639
	public Button DescriptionButton;

	// Token: 0x040044E8 RID: 17640
	public Text DescriptionText;

	// Token: 0x040044E9 RID: 17641
	public Button ResearchButton;

	// Token: 0x040044EA RID: 17642
	public Text ResearchText;

	// Token: 0x040044EB RID: 17643
	public Button FunctionButton;

	// Token: 0x040044EC RID: 17644
	public Text FunctionText;

	// Token: 0x040044ED RID: 17645
	public GameObject TooltipObj;

	// Token: 0x040044EE RID: 17646
	[Header("Sefira Function")]
	public Text Function_SefiraFunction_Context;

	// Token: 0x040044EF RID: 17647
	public Text Function_SefiraTenure_Context;

	// Token: 0x040044F0 RID: 17648
	public List<ScrollExchanger> scrollExchanged;

	// Token: 0x040044F1 RID: 17649
	public List<GameObject> activateEffected;

	// Token: 0x040044F2 RID: 17650
	public SefiraResearchPanel researchPanelController;

	// Token: 0x040044F3 RID: 17651
	public Animator MissionPanelAnim;

	// Token: 0x040044F4 RID: 17652
	public SefiraPanelMissionUI missionUI;

	// Token: 0x040044F5 RID: 17653
	public bool DoNotCloseMain;

	// Token: 0x040044F6 RID: 17654
	public AudioClipPlayer SefiraSound;

	// Token: 0x040044F7 RID: 17655
	[Header("SefiraBoss")]
	public Button SefiraBossButton;

	// Token: 0x040044F8 RID: 17656
	public Text SefiraBossButtonText;

	// Token: 0x040044F9 RID: 17657
	public List<GameObject> sefiraBossRelated;

	// Token: 0x040044FA RID: 17658
	public UIController sefiraBossDescPanel;

	// Token: 0x040044FB RID: 17659
	public Text SefiraBossDesc_Clear;

	// Token: 0x040044FC RID: 17660
	public Text SefiraBossDesc_Condition;

	// Token: 0x040044FD RID: 17661
	public GameObject Kether_Pivot;

	// Token: 0x040044FE RID: 17662
	public GameObject Kether_Chain;

	// Token: 0x040044FF RID: 17663
	private CanvasGroup _group;

	// Token: 0x04004500 RID: 17664
	private SefiraUIColor _sefiraUIColor;

	// Token: 0x04004501 RID: 17665
	private Color _sefiraColor;

	// Token: 0x04004502 RID: 17666
	public SefiraPanel LinkedSefira;

	// Token: 0x04004503 RID: 17667
	private int _currentAgentCount;

	// Token: 0x04004504 RID: 17668
	private int _allocateMax = 3;

	// Token: 0x04004505 RID: 17669
	private int _currentClickTarget = -1;

	// Token: 0x04004506 RID: 17670
	private SefiraPanel.PanelDataState _dataState;

	// Token: 0x04004507 RID: 17671
	private SefiraPanel.PanelDataState _overlayState = SefiraPanel.PanelDataState.RESEARCH;

	// Token: 0x04004508 RID: 17672
	private UnscaledTimer removeCoolTimer = new UnscaledTimer();

	// Token: 0x04004509 RID: 17673
	public Text DescriptionTextArea;

	// Token: 0x0400450A RID: 17674
	[Space(10f)]
	public SefiraPanel.Decorations decorations;

	// Token: 0x0400450B RID: 17675
	public List<ScaleSetter> scaleSetted;

	// Token: 0x0400450C RID: 17676
	public SefiraLevel level;

	// Token: 0x0400450D RID: 17677
	private bool dragEntered;

	// Token: 0x0400450E RID: 17678
	private Timer missionPanelStartTimer = new Timer();

	// Token: 0x02000993 RID: 2451
	public enum PanelDataState
	{
		// Token: 0x04004510 RID: 17680
		MAIN,
		// Token: 0x04004511 RID: 17681
		RESEARCH,
		// Token: 0x04004512 RID: 17682
		DESCRIPTION,
		// Token: 0x04004513 RID: 17683
		FUNCTION
	}

	// Token: 0x02000994 RID: 2452
	[Serializable]
	public class Decorations
	{
		// Token: 0x06004A87 RID: 19079 RVA: 0x000042F0 File Offset: 0x000024F0
		public Decorations()
		{
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x001B99A4 File Offset: 0x001B7BA4
		public void Init(bool active, Color c)
		{
			foreach (GameObject gameObject in this.decorations)
			{
				gameObject.SetActive(active);
			}
			foreach (MaskableGraphic maskableGraphic in this.graphics)
			{
				maskableGraphic.color = c;
			}
		}

		// Token: 0x04004514 RID: 17684
		public List<GameObject> decorations;

		// Token: 0x04004515 RID: 17685
		public List<MaskableGraphic> graphics;
	}

	// Token: 0x02000995 RID: 2453
	[Serializable]
	public class CreaturePortrait
	{
		// Token: 0x06004A89 RID: 19081 RVA: 0x0003E342 File Offset: 0x0003C542
		public CreaturePortrait()
		{
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0003E35C File Offset: 0x0003C55C
		public void SetCreature(CreatureModel creature, int sefiraLevel, Color sefiraColor)
		{
			this.creature = creature;
			this.isInit = true;
			this.sefiraColor = sefiraColor;
			this.UpdateCheck();
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x001B9A4C File Offset: 0x001B7C4C
		public void SetEmpty(Color sefiraColor)
		{
			this.portrait.color = DeployUI.instance.UIDefaultBlack;
			foreach (MaskableGraphic maskableGraphic in this.riskColor)
			{
				maskableGraphic.color = sefiraColor;
			}
			this.riskLevel.text = "EMPTY";
			this.riskLevel.color = sefiraColor;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x001B9ADC File Offset: 0x001B7CDC
		public void UpdateCheck()
		{
			if (!this.isInit)
			{
				return;
			}
			string text = "Unknown";
			if (this.creature.metaInfo.creatureWorkType == CreatureWorkType.KIT)
			{
				int observeCost = this.creature.observeInfo.GetObserveCost(CreatureModel.regionName[0]);
				if (this.creature.metaInfo.creatureKitType == CreatureKitType.ONESHOT)
				{
					if (this.creature.observeInfo.totalKitUseCount >= observeCost)
					{
						this.portrait.sprite = Add_On.GetPortrait(this.creature.metaInfo.portraitSrcForcely);
						text = this.creature.metaInfo.riskLevelForce;
						this.riskLevel.color = UIColorManager.instance.RiskLevelColor[(int)this.creature.metaInfo.GetRiskLevel()];
						using (List<MaskableGraphic>.Enumerator enumerator = this.riskColor.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MaskableGraphic maskableGraphic = enumerator.Current;
								maskableGraphic.color = this.riskLevel.color;
							}
							goto IL_3A0;
						}
					}
					this.portrait.sprite = Resources.Load<Sprite>("Sprites/Unit/creature/NoData");
					foreach (MaskableGraphic maskableGraphic2 in this.riskColor)
					{
						maskableGraphic2.color = this.sefiraColor;
					}
					this.riskLevel.color = this.sefiraColor;
				}
				else
				{
					if ((int)this.creature.observeInfo.totalKitUseTime >= observeCost)
					{
						this.portrait.sprite = Add_On.GetPortrait(this.creature.metaInfo.portraitSrcForcely);
						text = this.creature.metaInfo.riskLevelForce;
						this.riskLevel.color = UIColorManager.instance.RiskLevelColor[(int)this.creature.metaInfo.GetRiskLevel()];
						using (List<MaskableGraphic>.Enumerator enumerator = this.riskColor.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MaskableGraphic maskableGraphic3 = enumerator.Current;
								maskableGraphic3.color = this.riskLevel.color;
							}
							goto IL_3A0;
						}
					}
					this.portrait.sprite = Resources.Load<Sprite>("Sprites/Unit/creature/NoData");
					foreach (MaskableGraphic maskableGraphic4 in this.riskColor)
					{
						maskableGraphic4.color = this.sefiraColor;
					}
					this.riskLevel.color = this.sefiraColor;
				}
			}
			else if (this.creature.observeInfo.GetObserveState(CreatureModel.regionName[0]))
			{
				this.portrait.sprite = Add_On.GetPortrait(this.creature.metaInfo.portraitSrcForcely);
				foreach (MaskableGraphic maskableGraphic5 in this.riskColor)
				{
					maskableGraphic5.color = DeployUI.GetCreatureRiskLevelColor(this.creature.metaInfo.GetRiskLevel());
				}
				text = this.creature.metaInfo.riskLevelForce;
				this.riskLevel.color = UIColorManager.instance.RiskLevelColor[(int)this.creature.metaInfo.GetRiskLevel()];
			}
			else
			{
				this.portrait.sprite = Resources.Load<Sprite>("Sprites/Unit/creature/NoData");
				foreach (MaskableGraphic maskableGraphic6 in this.riskColor)
				{
					maskableGraphic6.color = this.sefiraColor;
					this.riskLevel.color = this.sefiraColor;
				}
			}
			IL_3A0:
			this.riskLevel.text = text;
		}

		// Token: 0x04004516 RID: 17686
		[NonSerialized]
		public CreatureModel creature;

		// Token: 0x04004517 RID: 17687
		public Image portrait;

		// Token: 0x04004518 RID: 17688
		public List<MaskableGraphic> riskColor;

		// Token: 0x04004519 RID: 17689
		public Text riskLevel;

		// Token: 0x0400451A RID: 17690
		public int index = -1;

		// Token: 0x0400451B RID: 17691
		public bool isInit;

		// Token: 0x0400451C RID: 17692
		private Color sefiraColor = Color.white;
	}

	// Token: 0x02000996 RID: 2454
	[Serializable]
	public class SefiraPanelButton
	{
		// Token: 0x06004A8D RID: 19085 RVA: 0x0003E379 File Offset: 0x0003C579
		public SefiraPanelButton()
		{
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0003E3A5 File Offset: 0x0003C5A5
		public void Init()
		{
			this.isEnabled = true;
			this.startRate = 0f;
			this.fadeIn = true;
			this.SetColor(0f);
			this.Disabled.enabled = false;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0000431D File Offset: 0x0000251D
		public void OnEnter()
		{
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0000431D File Offset: 0x0000251D
		public void OnExit()
		{
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x001B9EE4 File Offset: 0x001B80E4
		private void SetColor(float rate)
		{
			Color color = this.Overlay.color;
			color.a = rate;
			this.Overlay.color = color;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x001B9F14 File Offset: 0x001B8114
		public void Update()
		{
			float rate = this.transTimer.Rate;
			if (this.fadeIn)
			{
				this.SetColor(this.startRate + rate * (1f - this.startRate));
			}
			else
			{
				this.SetColor(1f - (this.startRate + rate * (1f - this.startRate)));
			}
			this.transTimer.RunTimer();
		}

		// Token: 0x0400451D RID: 17693
		public Image Overlay;

		// Token: 0x0400451E RID: 17694
		public Image Disabled;

		// Token: 0x0400451F RID: 17695
		public float transitionTime = 0.5f;

		// Token: 0x04004520 RID: 17696
		private UnscaledTimer transTimer = new UnscaledTimer();

		// Token: 0x04004521 RID: 17697
		public bool isEnabled = true;

		// Token: 0x04004522 RID: 17698
		private float startRate;

		// Token: 0x04004523 RID: 17699
		private bool fadeIn = true;
	}
}
