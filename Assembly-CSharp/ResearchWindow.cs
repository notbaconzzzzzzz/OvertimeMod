/*
+public void MakeSefiraBossReward(SefiraEnum sefiraEnum, bool isOvertime) // 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000993 RID: 2451
public class ResearchWindow : MonoBehaviour, IAnimatorEventCalled
{
	// Token: 0x06004A3B RID: 19003 RVA: 0x0003DF7A File Offset: 0x0003C17A
	public ResearchWindow()
	{
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0003DFA3 File Offset: 0x0003C1A3
	public Sefira CurrentSefira
	{
		get
		{
			return this._currentSefira;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x0003DFAB File Offset: 0x0003C1AB
	public SefiraUIColor UIColor
	{
		get
		{
			return this._uiColor;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06004A3E RID: 19006 RVA: 0x0003DFB3 File Offset: 0x0003C1B3
	private UIController ConversationAnim
	{
		get
		{
			return this.Conversation.transform.parent.GetComponent<UIController>();
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06004A3F RID: 19007 RVA: 0x0003DFCA File Offset: 0x0003C1CA
	public bool IsDragging
	{
		get
		{
			return this._isDragging;
		}
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x001B8790 File Offset: 0x001B6990
	public void Init(Sefira sefira)
	{
		this._currentSefira = sefira;
		this._uiColor = UIColorManager.instance.GetSefiraColor(sefira);
		this.ui.SetUI(sefira.name);
		this._bossState = false;
		this.SetColor();
		this.LowerArea.gameObject.SetActive(true);
		this.SetActive(false);
		this.selectedPanel.SetSelectedPanel();
		this.selectedPanel.SetWindow(this);
		this.researchPanelArea.SetActive(true);
		this.sefiraBossRoot.SetActive(false);
		this.SetPanelController(false);
		this.sefiraBossButton.SetActive(false);
		this.Areaname.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			sefira.name,
			"Name"
		});
		this.title.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			sefira.name,
			"Name"
		}), LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			"Allocate",
			"Research"
		}));
		foreach (ResearchPanel researchPanel in this.panels)
		{
			researchPanel.SetWindow(this);
			researchPanel.PanelReset();
		}
		foreach (ColorMultiplier colorMultiplier in this.buttonColorMultiplier)
		{
			colorMultiplier.Init(this._uiColor.imageColor);
		}
		foreach (ColorMultiplier colorMultiplier2 in this.instructions)
		{
			colorMultiplier2.Init(this._uiColor.imageColor);
		}
		this.SetPanelPosition(this.ResearchDataInit());
		this.dropHandler.SetDropEvent(new Drop.OnDropEvent(this.OnDropEvent));
		this.selectedPanel.SetColor(this.UIColor.imageColor);
		this.OnSetPanel(false);
		this.SetInst(this.grayFactor);
		this.DropFeildPivot.transform.localScale = Vector3.one;
		this.ConversationAnim.gameObject.SetActive(false);
		this.sefiraBossRoot.SetActive(false);
		this.Portrait.sprite = CharacterResourceDataModel.instance.GetSefiraPortrait(this.CurrentSefira.sefiraEnum, true);
		this.controller.Show();
	}

	// Token: 0x06004A41 RID: 19009 RVA: 0x001B8A20 File Offset: 0x001B6C20
	public void MakeSefiraBossReward(SefiraEnum sefiraEnum)
	{ // <Mod>
		MakeSefiraBossReward(sefiraEnum, false);
	}

	// <Mod>
	public void MakeSefiraBossReward(SefiraEnum sefiraEnum, bool isOvertime)
	{
		this._bossState = true;
		this.researchPanelArea.SetActive(false);
		this.sefiraBossRoot.SetActive(true);
		this.LowerArea.gameObject.SetActive(false);
		this.sefiraBossButton.SetActive(true);
		int num = 0;
		IEnumerator enumerator = this.sefiraBoss_ListParent.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (!(transform == this.sefiraBoss_ListParent.transform))
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
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
		if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
		{
			Mission mission = MissionManager.instance.GetClearedMissions().Find((Mission x) => x.metaInfo.sefira == CurrentSefira.sefiraEnum && (x.successCondition.condition_Type == ConditionType.DESTROY_CORE || x.metaInfo.sefira_Level % 5 == 1));
			if (mission != null && mission.successCondition.condition_Type != ConditionType.DESTROY_CORE)
			{
				if (isOvertime)
				{
					sefiraBoss_Prefix.text = string.Format("{0} {1}", SefiraName.GetSefiraCharName(sefiraEnum), "Mission Penalty");
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
					gameObject3.transform.SetParent(this.sefiraBoss_ListParent);
					gameObject3.transform.localScale = Vector3.one;
					gameObject3.transform.GetChild(1).GetComponent<Text>().text = "You will no longer be alerted to events happening in the department.";
					return;
				}
				else
				{
					sefiraBoss_Prefix.text = string.Format("{0} {1}", SefiraName.GetSefiraCharName(sefiraEnum), "Mission Penalty");
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
					gameObject3.transform.SetParent(this.sefiraBoss_ListParent);
					gameObject3.transform.localScale = Vector3.one;
					gameObject3.transform.GetChild(1).GetComponent<Text>().text = "The department will now be influenced by Qliphoth Meltdowns.";
					return;
				}
			}
		}
		if (SefiraBossManager.Instance.TryGetBossDescCount(sefiraEnum, isOvertime, SefiraBossDescType.REWARD, out num))
		{
			this.sefiraBoss_Prefix.text = string.Format("{0} {1}", SefiraName.GetSefiraCharName(sefiraEnum), LocalizeTextDataModel.instance.GetText(isOvertime ? "boss2_common_clear" : "boss_common_clear"));
			if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
			{
				string empty = "Lose the following effects:";
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
				gameObject.transform.SetParent(this.sefiraBoss_ListParent);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.GetChild(1).GetComponent<Text>().text = empty;
			}
			for (int i = 0; i < num; i++)
			{
				string empty = string.Empty;
                if (sefiraEnum == SefiraEnum.HOD && !isOvertime && SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
                {
                    empty = LocalizeTextDataModel.instance.GetText("boss_hod_reward_0_alt");
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
					gameObject.transform.SetParent(this.sefiraBoss_ListParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.GetChild(1).GetComponent<Text>().text = empty;
                }
				else if (SefiraBossManager.Instance.TryGetBossDesc(sefiraEnum, isOvertime, SefiraBossDescType.REWARD, i, out empty))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
					gameObject.transform.SetParent(this.sefiraBoss_ListParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.GetChild(1).GetComponent<Text>().text = empty;
				}
			}
		}
		if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
		{
			return;
		}
		string text = LocalizeTextDataModel.instance.GetText(isOvertime ? "boss2_common_qliphoth" : "boss_common_qliphoth");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.sefiraTextUnit);
		gameObject2.transform.SetParent(this.sefiraBoss_ListParent);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.GetChild(1).GetComponent<Text>().text = text;
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x001B8BFC File Offset: 0x001B6DFC
	public void SetInst(float factor)
	{
		foreach (ColorMultiplier colorMultiplier in this.instructions)
		{
			colorMultiplier.ColorFactor = factor;
		}
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x001B8C30 File Offset: 0x001B6E30
	private int ResearchDataInit()
	{ // <Mod>
		bool isOvertime = false;
		Mission mission = MissionManager.instance.GetClearedMissions().Find((Mission x) => x.metaInfo.sefira == CurrentSefira.sefiraEnum && x.successCondition.condition_Type != ConditionType.DESTROY_CORE);
		if (mission != null && mission.metaInfo.sefira_Level > 5) isOvertime = true;
		List<ResearchItemModel> remainResearchListBySefira = ResearchDataModel.instance.GetRemainResearchListBySefira(this.CurrentSefira.indexString);
		if (isOvertime)
		{
			remainResearchListBySefira = remainResearchListBySefira.FindAll((ResearchItemModel x) => x.info.isOvertime);
		}
		else
		{
			remainResearchListBySefira = remainResearchListBySefira.FindAll((ResearchItemModel x) => !x.info.isOvertime);
		}
		if (remainResearchListBySefira.Count < 0)
		{
			Debug.LogError("Shouldn't be opened this research window ui, this area has no researches");
			return 0;
		}
		int count = remainResearchListBySefira.Count;
		for (int i = 0; i < 4; i++)
		{
			if (i < count)
			{
				this.panels[i].SetData(remainResearchListBySefira[i]);
				this.SetPanelActivate(this.panels[i], true);
			}
			else
			{
				this.SetPanelActivate(this.panels[i], false);
			}
		}
		return remainResearchListBySefira.Count;
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x001B8CD4 File Offset: 0x001B6ED4
	public void SetPanelActivate(ResearchPanel panel, bool state)
	{
		if (state)
		{
			if (panel.RectTransform.parent != this.layoutParent)
			{
				panel.RectTransform.SetParent(this.layoutParent);
			}
		}
		else if (panel.RectTransform.parent != this.disableParent)
		{
			panel.RectTransform.SetParent(this.disableParent);
		}
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x0003DFD2 File Offset: 0x0003C1D2
	public void SetActive(bool state)
	{
		this.rootObject.SetActive(state);
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x001B8D44 File Offset: 0x001B6F44
	private void SetColor()
	{
		foreach (MaskableGraphic maskableGraphic in this.coloredTargets)
		{
			maskableGraphic.color = this.UIColor.imageColor;
		}
		foreach (ResearchPanel researchPanel in this.panels)
		{
			researchPanel.SetColor(this.UIColor.imageColor);
		}
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x001B8E00 File Offset: 0x001B7000
	private void SetPanelPosition(int max)
	{
		for (int i = 0; i < max; i++)
		{
			ResearchPanel researchPanel = this.panels[i];
			researchPanel.SetIndex(i, 4 - max);
		}
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x001B8E38 File Offset: 0x001B7038
	public bool OnDropEvent(params object[] param)
	{
		Debug.Log("Drop");
		ResearchItemModel researchItemModel = null;
		if (param[0] is ResearchItemModel)
		{
			researchItemModel = (param[0] as ResearchItemModel);
		}
		this.selectedPanel.gameObject.SetActive(true);
		if (researchItemModel != null)
		{
			this.selectedPanel.SetData(researchItemModel);
		}
		this.OnSetPanel(true);
		return true;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x001B8E94 File Offset: 0x001B7094
	public void OnSetPanel(bool isSet)
	{
		if (isSet)
		{
			this.LowerArea.SetAsLastSibling();
			this.backGround.enabled = false;
			this.selectedEffectImage.enabled = true;
			this.dropArea.raycastTarget = false;
			this.ButtonArea.gameObject.SetActive(true);
			this.HelpArea.gameObject.SetActive(false);
			this.Conversation.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				this.CurrentSefira.name,
				"ResearchCheck"
			});
			this.ConversationAnim.gameObject.SetActive(true);
			this.ConversationAnim.Show();
			Notice.instance.Send(NoticeName.OnResearchPanelDropped, new object[0]);
		}
		else
		{
			this.LowerArea.SetAsFirstSibling();
			this.backGround.enabled = true;
			this.selectedEffectImage.enabled = false;
			this.dropArea.raycastTarget = true;
			this.HelpArea.gameObject.SetActive(true);
			this.ButtonArea.gameObject.SetActive(false);
			this.selectedPanel.gameObject.SetActive(false);
			this.ConversationAnim.Hide();
		}
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x001B8FCC File Offset: 0x001B71CC
	public void OnConfirm()
	{
		this.controller.Hide();
		if (this._bossState)
		{
			this._bossState = false;
			return;
		}
		ResearchItemModel currentModel = this.selectedPanel.CurrentModel;
		if (currentModel != null)
		{
			this.Upgrade(currentModel);
		}
		else
		{
			Debug.LogError("Research Confirm Failed");
		}
		Debug.Log("Confirm");
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x001B902C File Offset: 0x001B722C
	private void Upgrade(ResearchItemModel data)
	{
		if (ResearchDataModel.instance.UpgradeResearch(data.info.id, false))
		{
			MissionManager.instance.CloseClearedMission_ExceptBoss(this._currentSefira.sefiraEnum);
			Notice.instance.Send(NoticeName.MakeName(NoticeName.UpdateResearchItem, new string[]
			{
				data.info.id.ToString()
			}), new object[0]);
		}
		else
		{
			Debug.LogError("Upgrade Failed");
		}
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x001B90B4 File Offset: 0x001B72B4
	public void OnDiscard()
	{
		this.OnSetPanel(false);
		Debug.Log("Discard");
		foreach (ResearchPanel researchPanel in this.panels)
		{
			researchPanel.PanelReset();
		}
		this._isDragging = false;
		this.OnExit(null);
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x001B9130 File Offset: 0x001B7330
	public void OnEnter(BaseEventData bpData)
	{
		PointerEventData pointerEventData = bpData as PointerEventData;
		if (pointerEventData.dragging)
		{
			this.DropFeildPivot.localScale = new Vector3(1.1f, 1.1f, 1f);
		}
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x0003DFE0 File Offset: 0x0003C1E0
	public void OnExit(BaseEventData pData)
	{
		this.DropFeildPivot.localScale = Vector3.one;
		if (!this._isDragging)
		{
			this.ResearchSound.OnPlayInList(3);
		}
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x0003E009 File Offset: 0x0003C209
	public void OnBeginDrag(ResearchPanel panel)
	{
		this._isDragging = true;
		this.instTimer.StartTimer(this.instTime);
		this.gray = false;
		this.SetInst(1f);
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x0003E035 File Offset: 0x0003C235
	public void OnEndDrag()
	{
		this._isDragging = false;
		this.instTimer.StopTimer();
		this.SetInst(this.grayFactor);
		this.gray = true;
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x001B9170 File Offset: 0x001B7370
	private void Update()
	{
		if (this.instTimer.RunTimer())
		{
			if (this.gray)
			{
				this.SetInst(1f);
				this.gray = false;
			}
			else
			{
				this.SetInst(this.grayFactor);
				this.gray = true;
			}
			this.instTimer.StartTimer(this.instTime);
		}
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x0003E05C File Offset: 0x0003C25C
	public void OnPanelEnter()
	{
		this.SetInst(1f);
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0003E069 File Offset: 0x0003C269
	public void OnPanelExit()
	{
		this.SetInst(this.grayFactor);
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0003E077 File Offset: 0x0003C277
	public void OnCalled()
	{
		DeployUI.instance.CheckResearch();
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x0003E083 File Offset: 0x0003C283
	public void OnCalled(int i)
	{
		if (i == 0)
		{
			this.SetPanelController(true);
		}
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x001B91D4 File Offset: 0x001B73D4
	private void SetPanelController(bool state)
	{
		foreach (ResearchPanel researchPanel in this.panels)
		{
			researchPanel.Anim.enabled = state;
		}
	}

	// Token: 0x06004A57 RID: 19031 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AgentReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void SimpleReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AnimatorEventInit()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void CreatureAnimCall(int i, CreatureBase script)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AttackCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AttackDamageTimeCalled()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void SoundMake(string src)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0003E09C File Offset: 0x0003C29C
	// Note: this type is marked as 'beforefieldinit'.
	static ResearchWindow()
	{
	}

	// Token: 0x040044BE RID: 17598
	private const int _researchMaxPerArea = 4;

	// Token: 0x040044BF RID: 17599
	public static float[,] positionFactor = new float[,]
	{
		{
			-3f,
			-1f,
			1f,
			3f
		},
		{
			-2.5f,
			0.5f,
			2.5f,
			0f
		},
		{
			-2f,
			2f,
			0f,
			0f
		},
		{
			0.5f,
			0f,
			0f,
			0f
		}
	};

	// Token: 0x040044C0 RID: 17600
	public GameObject rootObject;

	// Token: 0x040044C1 RID: 17601
	private Sefira _currentSefira;

	// Token: 0x040044C2 RID: 17602
	private SefiraUIColor _uiColor;

	// Token: 0x040044C3 RID: 17603
	public ResearchWindow.UI ui;

	// Token: 0x040044C4 RID: 17604
	public UIController controller;

	// Token: 0x040044C5 RID: 17605
	public List<MaskableGraphic> coloredTargets;

	// Token: 0x040044C6 RID: 17606
	public List<ResearchPanel> panels;

	// Token: 0x040044C7 RID: 17607
	public ResearchPanel selectedPanel;

	// Token: 0x040044C8 RID: 17608
	public RectTransform layoutParent;

	// Token: 0x040044C9 RID: 17609
	public RectTransform disableParent;

	// Token: 0x040044CA RID: 17610
	public RectTransform LowerArea;

	// Token: 0x040044CB RID: 17611
	public Image backGround;

	// Token: 0x040044CC RID: 17612
	public Image selectedEffectImage;

	// Token: 0x040044CD RID: 17613
	public Image dropArea;

	// Token: 0x040044CE RID: 17614
	public Drop dropHandler;

	// Token: 0x040044CF RID: 17615
	public RectTransform HelpArea;

	// Token: 0x040044D0 RID: 17616
	public RectTransform ButtonArea;

	// Token: 0x040044D1 RID: 17617
	public AudioClipPlayer ResearchSound;

	// Token: 0x040044D2 RID: 17618
	public Text title;

	// Token: 0x040044D3 RID: 17619
	public Text Areaname;

	// Token: 0x040044D4 RID: 17620
	public ColorMultiplier[] buttonColorMultiplier;

	// Token: 0x040044D5 RID: 17621
	public ColorMultiplier[] instructions;

	// Token: 0x040044D6 RID: 17622
	public Image Portrait;

	// Token: 0x040044D7 RID: 17623
	public Text Conversation;

	// Token: 0x040044D8 RID: 17624
	public RectTransform DropFeildPivot;

	// Token: 0x040044D9 RID: 17625
	private bool _isDragging;

	// Token: 0x040044DA RID: 17626
	private bool gray;

	// Token: 0x040044DB RID: 17627
	private UnscaledTimer instTimer = new UnscaledTimer();

	// Token: 0x040044DC RID: 17628
	public float instTime = 1f;

	// Token: 0x040044DD RID: 17629
	public float grayFactor = 0.3f;

	// Token: 0x040044DE RID: 17630
	[Header("SefiraBoss")]
	public GameObject researchPanelArea;

	// Token: 0x040044DF RID: 17631
	public GameObject sefiraBossRoot;

	// Token: 0x040044E0 RID: 17632
	public Text sefiraBoss_Prefix;

	// Token: 0x040044E1 RID: 17633
	public RectTransform sefiraBoss_ListParent;

	// Token: 0x040044E2 RID: 17634
	public GameObject sefiraTextUnit;

	// Token: 0x040044E3 RID: 17635
	public GameObject sefiraBossButton;

	// Token: 0x040044E4 RID: 17636
	private bool _bossState;

	// Token: 0x02000994 RID: 2452
	[Serializable]
	public class UI
	{
		// Token: 0x06004A5F RID: 19039 RVA: 0x00004378 File Offset: 0x00002578
		public UI()
		{
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x001B9238 File Offset: 0x001B7438
		public void SetUI(string sefiraName)
		{
			this.AreaName.text = sefiraName;
			this.AreaDesc.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				sefiraName,
				"Name"
			}) + " " + LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Allocate",
				"Research"
			});
			this.HelpDescription.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Allocate",
				"Research_Help"
			});
		}

		// Token: 0x040044E5 RID: 17637
		public Text AreaName;

		// Token: 0x040044E6 RID: 17638
		public Text AreaDesc;

		// Token: 0x040044E7 RID: 17639
		public Text HelpDescription;
	}
}
