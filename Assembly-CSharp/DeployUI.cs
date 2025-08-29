/*
private void CheckResearchAvailable() // Squash 4th Mission Research
public bool CheckBossClear() // Overtime Core Suppression Rewards
public void CheckResearch() // 
public bool OnClickSefiraBossSession(SefiraEnum sefira) // Overtime Core Suppressions
*/
using System;
using System.Collections.Generic;
using System.IO;
using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200098B RID: 2443
public class DeployUI : MonoBehaviour, IScrollMessageReciever
{
	// Token: 0x060049C2 RID: 18882 RVA: 0x0003D9EF File Offset: 0x0003BBEF
	public DeployUI()
	{
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x060049C3 RID: 18883 RVA: 0x0003DA18 File Offset: 0x0003BC18
	public static DeployUI instance
	{
		get
		{
			return DeployUI._instance;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x060049C4 RID: 18884 RVA: 0x0003DA1F File Offset: 0x0003BC1F
	public int CurrentMoney
	{
		get
		{
			return MoneyModel.instance.money;
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x060049C5 RID: 18885 RVA: 0x0003DA2B File Offset: 0x0003BC2B
	// (set) Token: 0x060049C6 RID: 18886 RVA: 0x0003DA33 File Offset: 0x0003BC33
	public SefiraLevel CurrentLevel
	{
		get
		{
			return this._currentLevel;
		}
		private set
		{
			this.OnSetLevel(value);
		}
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x060049C7 RID: 18887 RVA: 0x0003DA3C File Offset: 0x0003BC3C
	// (set) Token: 0x060049C8 RID: 18888 RVA: 0x0003DA44 File Offset: 0x0003BC44
	public int CurrentColorIndex
	{
		get
		{
			return this._currentColorIndex;
		}
		private set
		{
			this._currentColorIndex = value;
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x060049C9 RID: 18889 RVA: 0x0003DA4D File Offset: 0x0003BC4D
	public Color CurrentDeployColor
	{
		get
		{
			return this.DeployColorSet[0];
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x060049CA RID: 18890 RVA: 0x0003CAC9 File Offset: 0x0003ACC9
	public int Day
	{
		get
		{
			return PlayerModel.instance.GetDay();
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x060049CB RID: 18891 RVA: 0x0003DA60 File Offset: 0x0003BC60
	// (set) Token: 0x060049CC RID: 18892 RVA: 0x0003DA68 File Offset: 0x0003BC68
	public bool StartAble
	{
		get
		{
			return this._startAble;
		}
		private set
		{
			this.OnSetStartState(value);
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x060049CD RID: 18893 RVA: 0x0003DA71 File Offset: 0x0003BC71
	// (set) Token: 0x060049CE RID: 18894 RVA: 0x0003DA79 File Offset: 0x0003BC79
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
		set
		{
			this._isEnabled = value;
			this.canvas.gameObject.SetActive(value);
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x060049CF RID: 18895 RVA: 0x0003DA93 File Offset: 0x0003BC93
	// (set) Token: 0x060049D0 RID: 18896 RVA: 0x0003DA9B File Offset: 0x0003BC9B
	public bool IsGameStarted
	{
		get
		{
			return this._isGameStarted;
		}
		set
		{
			this._isGameStarted = value;
		}
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x0003DAA4 File Offset: 0x0003BCA4
	private void Awake()
	{
		if (DeployUI._instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		DeployUI._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x0003DAD3 File Offset: 0x0003BCD3
	public Vector2 GetCanvasPosition(RectTransform target)
	{
		return Vector2.one;
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x0003DADA File Offset: 0x0003BCDA
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x001B5B18 File Offset: 0x001B3D18
	private void Update()
	{
		if (!this._moveAreaTimer.started || this._moveAreaTimer.RunTimer())
		{
		}
		this.InputCheck();
		this.pointCount.text = MoneyModel.instance.money.ToString();
		this.CheckStartState();
	}

	// Token: 0x060049D5 RID: 18901 RVA: 0x001B5B74 File Offset: 0x001B3D74
	public void Init()
	{
		this.checkPointUI.gameObject.SetActive(false);
		this._isGameStarted = false;
		this.sefiraList.Init();
		this.agentList.Init(AgentManager.instance.GetAgentSpareList());
		this.dayCount.text = this.Day + 1 + string.Empty;
		this.pointCount.text = MoneyModel.instance.money.ToString();
		this._isEnabled = true;
		this.InitialResearchProcedure();
		this.VolumeSetting();
		StageTypeInfo.instnace.GetEnergyNeed(this.Day).ToString();
		this.SetDayIcon();
		this.CurrentLevel = SefiraLevel.UP;
		foreach (ScrollExchanger scrollExchanger in this.scroll)
		{
			scrollExchanger.MessageRecieveInit();
		}
		bool flag = SefiraManager.instance.IsOpened("Tiphereth1");
		bool flag2 = SefiraManager.instance.IsOpened("Binah") || SefiraManager.instance.IsOpened("Chokhmah");
		this.AreaMoveButton[0].interactable = true;
		this.AreaMoveButton[1].interactable = flag;
		this.AreaMoveButton[2].interactable = flag2;
		this.ordeal.text = LocalizeTextDataModel.instance.GetText("OrdealMax");
		this.OrdealText.text = string.Empty;
		foreach (GameObject gameObject in this.middleAreaConnected)
		{
			gameObject.SetActive(flag);
		}
		foreach (GameObject gameObject2 in this.lowerAreaConnected)
		{
			gameObject2.SetActive(flag2);
		}
		this.SetOrdealText(OrdealManager.instance.GetMaxOrdealLevel());
		bool flag3 = SefiraBossManager.Instance.DisplayTutorial() && this.Day % 5 != 0;
		this.SefiraBossTutorialRoot.SetActive(flag3);
		if (flag3)
		{
			this.researchWindow.SetActive(false);
			this.InitBossSetting();
			return;
		}
		if (SefiraManager.instance.IsOpened(SefiraEnum.CHOKHMAH) || SefiraManager.instance.IsOpened(SefiraEnum.BINAH))
		{
			this.OnClickMoveArea(2);
		}
		else if (SefiraManager.instance.IsOpened(SefiraEnum.TIPERERTH1))
		{
			this.OnClickMoveArea(1);
		}
		try
		{
			if (SefiraBossManager.Instance.IsKetherBoss())
			{
				SefiraBossUI.Instance.OnEnterSefiraBossSession();
			}
			if (SefiraManager.instance.IsOpened(SefiraEnum.KETHER))
			{
				this.KetherConnected[0].SetActive(true);
				this.KetherConnected[1].SetActive(true);
				if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
				{
					this.KetherConnected[2].SetActive(true);
				}
				else
				{
					this.KetherConnected[2].SetActive(false);
				}
			}
			else
			{
				this.KetherConnected[0].SetActive(false);
				this.KetherConnected[1].SetActive(false);
				this.KetherConnected[2].SetActive(false);
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/DPerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x060049D6 RID: 18902 RVA: 0x001B5ED8 File Offset: 0x001B40D8
	public void InitBossSetting()
	{
		SefiraPanel sefiraPanel = this.sefiraList.GetSefiraPanel(SefiraManager.instance.GetSefira(SefiraEnum.HOD).indexString);
		sefiraPanel.SefiraBossButton.enabled = false;
		sefiraPanel.SefiraBossButton.GetComponent<Animator>().SetTrigger("Highlighted");
		sefiraPanel.SetBossState();
		sefiraPanel.sefiraBossDescPanel.Show();
		sefiraPanel.SetBossPanel(true);
		Debug.Log("Tutorial setting");
		this.currentTutorialIndex = 0;
		this.SetBossTutorial();
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x001B5F54 File Offset: 0x001B4154
	private void SetBossTutorial()
	{
		try
		{
			this.SefiraBossTutorial_Right.interactable = true;
			this.SefiraBossTutorial_Left.interactable = true;
			this.SefiraBossTutorialIndex.text = string.Format("{0} / {1}", this.currentTutorialIndex + 1, SefiraBossUI.Instance.tutorialSpriteSet.Count);
			SefiraBossUI.TutorialElement tutorialElement = SefiraBossUI.Instance.tutorialSpriteSet[this.currentTutorialIndex];
			RectTransform rectTransform = null;
			if (tutorialElement.horizontal)
			{
				rectTransform = this.SefiraBossTutorial_HorizontalText;
				this.SefiraBossTutorial_HorizontalText.gameObject.SetActive(true);
				this.SefiraBossTutorial_VerticalText.gameObject.SetActive(false);
			}
			else
			{
				this.SefiraBossTutorial_HorizontalText.gameObject.SetActive(false);
				this.SefiraBossTutorial_VerticalText.gameObject.SetActive(true);
				rectTransform = this.SefiraBossTutorial_VerticalText;
			}
			if (this.currentTutorialIndex == 0)
			{
				this.SefiraBossZeroEmpty.gameObject.SetActive(true);
			}
			else
			{
				this.SefiraBossZeroEmpty.gameObject.SetActive(false);
			}
			string text = LocalizeTextDataModel.instance.GetText(tutorialElement.keyValue);
			foreach (Text text2 in this.SefiraBossTutorial_Text)
			{
				text2.text = text;
			}
			rectTransform.anchoredPosition = tutorialElement.pos;
			this.SefiraBossTutorialBlock.sprite = tutorialElement.block;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			this.OnBossTutorialEnd();
		}
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x001B6118 File Offset: 0x001B4318
	public void OnBossTutorialEnd()
	{
		SefiraBossManager.Instance.OnTutorialEnd();
		SefiraPanel sefiraPanel = this.sefiraList.GetSefiraPanel(SefiraManager.instance.GetSefira(SefiraEnum.HOD).indexString);
		sefiraPanel.SefiraBossButton.enabled = true;
		sefiraPanel.SefiraBossButton.OnPointerExit(null);
		sefiraPanel.SetBossPanel(false);
		sefiraPanel.sefiraBossDescPanel.Hide();
		Debug.Log("Tutorial End?");
		this.Init();
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x001B6188 File Offset: 0x001B4388
	public void VolumeSetting()
	{
		float currentVolume = GlobalGameManager.instance.sceneDataSaver.currentVolume;
		float currentBgmVolume = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
		EscapeUI.instance.MasterFill.fillAmount = currentVolume;
		EscapeUI.instance.MasterVolume.value = currentVolume;
		EscapeUI.instance.MusicFill.fillAmount = currentBgmVolume;
		EscapeUI.instance.MusicVolume.value = currentBgmVolume;
		BgmManager.instance.InitVolume(currentVolume, currentBgmVolume);
		Debug.Log(string.Format("Volume Setted Master {0}, BGM {1}", currentVolume, currentBgmVolume));
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x001B6228 File Offset: 0x001B4428
	private void InitialResearchProcedure()
	{
		Debug.Log("Research");
		bool flag = this.CheckBossClear();
		this.CheckResearchAvailable();
		if (flag)
		{
			return;
		}
		this.CheckResearch();
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x001B625C File Offset: 0x001B445C
	private void SetDayIcon()
	{
		int day = this.Day;
		int num = day % 5;
		for (int i = 0; i < 5; i++)
		{
			if (i <= num)
			{
				this.DayCircleImage[i].color = this.UIDefaultBlack;
			}
			else
			{
				this.DayCircleImage[i].color = this.CurrentDeployColor;
			}
		}
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x000040A1 File Offset: 0x000022A1
	public void OrdealTextSetting(bool hasOrdeal, string text)
	{
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x000040A1 File Offset: 0x000022A1
	public void TextCheckPoint()
	{
	}

	// Token: 0x060049DE RID: 18910 RVA: 0x0003DAE2 File Offset: 0x0003BCE2
	public void AddAgent(AgentModel agent)
	{
		this.agentList.AddAgent(agent);
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x001B62B8 File Offset: 0x001B44B8
	public void AddAgent()
	{
		AgentModel agent = AgentManager.instance.BuyAgent();
		this.agentList.AddAgent(agent);
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x0003DAF0 File Offset: 0x0003BCF0
	public void BuyAgent()
	{
		if (MoneyModel.instance.money < 1)
		{
			return;
		}
		this.OpenCustomizingWindow();
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x0003DB09 File Offset: 0x0003BD09
	public static Sprite GetAgentGenderImage(string gender)
	{
		if (gender.Trim().ToLower() == "male")
		{
			return DeployUI.instance.Gender_Male;
		}
		return DeployUI.instance.Gender_Female;
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x001B62DC File Offset: 0x001B44DC
	public void ResetAll()
	{
		foreach (IDeployResetCalled deployResetCalled in this.resetTargets)
		{
			try
			{
				if (deployResetCalled != null)
				{
					deployResetCalled.DeployResetCalled();
				}
			}
			catch (Exception ex)
			{
			}
		}
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x0003DB3A File Offset: 0x0003BD3A
	public void RegistDeployReset(IDeployResetCalled called)
	{
		if (!this.resetTargets.Contains(called))
		{
			this.resetTargets.Add(called);
		}
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x001B6354 File Offset: 0x001B4554
	public static Sprite GetAgentGradeSprite(AgentModel agent)
	{
		int level = agent.level;
		if (level <= 0 || level > 5)
		{
			Debug.LogError("Agent level range is odd : " + level);
			return DeployUI.instance.AgentGradeImage[0];
		}
		return DeployUI.instance.AgentGradeImage[level - 1];
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x001B63A8 File Offset: 0x001B45A8
	public static Sprite GetGradeSprite(int level)
	{
		if (level < 1 || level > DeployUI.instance.AgentGradeImage.Length)
		{
			Debug.LogError("index out of range : " + level);
			return DeployUI.instance.AgentGradeImage[0];
		}
		return DeployUI.instance.AgentGradeImage[level - 1];
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x001B6400 File Offset: 0x001B4600
	public static Sprite[] GetAgentWorkIcon(AgentModel agent)
	{
		Sprite[] array = new Sprite[3];
		AgentModel.SkillInfo[] normalSKill = agent.GetNormalSKill();
		for (int i = 0; i < 3; i++)
		{
			array[i] = DeployUI.instance.defaultWorkIcon[(int)(checked((IntPtr)(unchecked(normalSKill[i].skill.id - 1L))))];
		}
		return array;
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x001B6450 File Offset: 0x001B4650
	public string GetHireText(bool isEnter)
	{
		string result = string.Empty;
		if (isEnter)
		{
			if (this.CurrentMoney >= 1)
			{
				result = "LOB " + 1;
			}
			else
			{
				result = LocalizeTextDataModel.instance.GetText("NeedCost");
			}
		}
		else
		{
			result = LocalizeTextDataModel.instance.GetText("Hire");
		}
		return result;
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x001B64B4 File Offset: 0x001B46B4
	public string GetCustomHireText(bool isEnter)
	{
		string result = string.Empty;
		if (isEnter)
		{
			if (this.CurrentMoney >= 1)
			{
				result = "LOB " + 1;
			}
			else
			{
				result = LocalizeTextDataModel.instance.GetText("NeedCost");
			}
		}
		else
		{
			result = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Allocate",
				"CustomAgent"
			});
		}
		return result;
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x0003DB59 File Offset: 0x0003BD59
	public static Color GetCreatureRiskLevelColor(RiskLevel level)
	{
		return UIColorManager.instance.GetRiskColor(level);
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x001B6528 File Offset: 0x001B4728
	private void CheckResearchAvailable()
	{ // <Mod> Squash 4th Mission Research
		List<Mission> clearedMissions = MissionManager.instance.GetClearedMissions();
		List<string> list = new List<string>();
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			if (GlobalGameManager.instance.tutorialStep == 2)
			{
				Sefira sefira = SefiraManager.instance.GetSefira(SefiraEnum.DUMMY);
				list.Add(sefira.name);
			}
		}
		else
		{
			foreach (Mission mission in clearedMissions)
			{
				if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
				{
					if (mission.metaInfo.sefira_Level % 5 == 1)
					{
						MissionManager.instance.CloseClearedMission(mission);
						continue;
					}
				}
				else if (mission.metaInfo.sefira_Level % 5 == 4)
				{
					MissionManager.instance.CloseClearedMission(mission);
					continue;
				}
				if (mission.successCondition.condition_Type != ConditionType.DESTROY_CORE)
				{
					list.Add(mission.sefira_Name);
				}
			}
		}
		this.SetResearchWaitQueue(list.ToArray());
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x001B65F4 File Offset: 0x001B47F4
	public void SetResearchWaitQueue(params string[] sefira)
	{
		this.ResearchUpgradeWaitQueue.Clear();
		foreach (string str in sefira)
		{
			Sefira sefira2 = SefiraManager.instance.GetSefira(str);
			if (sefira2 != null && this.CheckResearchRemains(sefira2))
			{
				this.ResearchUpgradeWaitQueue.Enqueue(sefira2);
			}
		}
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x0003DB66 File Offset: 0x0003BD66
	private bool CheckResearchRemains(Sefira target)
	{
		return ResearchDataModel.instance.GetRemainResearchListBySefira(target.indexString).Count > 0;
	}

	// Token: 0x060049ED RID: 18925 RVA: 0x001B6650 File Offset: 0x001B4850
	public bool CheckBossClear()
	{ // <Mod>
		List<Mission> clearedMissions = MissionManager.instance.GetClearedMissions();
		foreach (Mission mission in clearedMissions)
		{
			if (mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				bool isOvertime = mission.metaInfo.sefira_Level > 5;
				this.researchWindow.Init(SefiraManager.instance.GetSefira(mission.metaInfo.sefira));
				this.researchWindow.MakeSefiraBossReward(mission.metaInfo.sefira, isOvertime);
				this.researchWindow.SetActive(true);
				MissionManager.instance.CloseClearedMission(mission);
				return true;
			}
			if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch") && mission.metaInfo.sefira_Level % 5 == 1)
			{
				bool isOvertime = mission.metaInfo.sefira_Level > 5;
				this.researchWindow.Init(SefiraManager.instance.GetSefira(mission.metaInfo.sefira));
				this.researchWindow.MakeSefiraBossReward(mission.metaInfo.sefira, isOvertime);
				this.researchWindow.SetActive(true);
				MissionManager.instance.CloseClearedMission(mission);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060049EE RID: 18926 RVA: 0x001B6714 File Offset: 0x001B4914
	public void CheckResearch()
	{ // <Mod>
		int line = 0;
		try
		{
			line++;
			if (this.ResearchUpgradeWaitQueue.Count > 0)
			{
				line = 1000;
				line++;
				this.researchWindow.Init(this.ResearchUpgradeWaitQueue.Dequeue());
				line++;
				this.researchWindow.SetActive(true);
				line++;
			}
			else
			{
				line = 2000;
				this.researchWindow.SetActive(false);
				line++;
				this.sefiraList.ResearchInit();
				line++;
				if (!GlobalGameManager.instance.lastLoaded)
				{
					line++;
					Notice.instance.Send(NoticeName.OnResearchEnd, new object[0]);
					line++;
					ControlGroupManager.instance.ResetUpperGroups();
					line++;
					if (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL)
					{
						line++;
						if (GlobalGameManager.instance.gameMode != GameMode.UNLIMIT_MODE)
						{
							line++;
							if (this.CheckPointCheck())
							{
								line += 100;
								this.researchWindow.SetActive(false);
								line++;
								GlobalGameManager.instance.saveState = "manage";
								line++;
								GlobalGameManager.instance.SaveData(true);
								line++;
								this.checkPointUI.OnCheckpointUpdate(this.Day + 1);
								line++;
							}
							else
							{
								line += 200;
								GlobalGameManager.instance.saveState = "manage";
								line++;
								GlobalGameManager.instance.SaveData(false);
								line++;
								this.checkPointUI.gameObject.SetActive(false);
								line++;
							}
							line++;
							int day = PlayerModel.instance.GetDay() + 1;
							line++;
							GlobalEtcDataModel.instance.UpdateUnlockedMaxDay(day);
							line++;
							GlobalGameManager.instance.SaveGlobalData();
							line++;
						}
					}
				}
				GlobalGameManager.instance.lastLoaded = false;
			}
		}
		catch (Exception ex2)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/Deployerror.txt", ex2.Message + Environment.NewLine + ex2.StackTrace + Environment.NewLine + line);
		}
	}

	// Token: 0x060049EF RID: 18927 RVA: 0x001B685C File Offset: 0x001B4A5C
	private bool CheckPointCheck()
	{
		int day = this.Day;
		return day % 5 == 0;
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x001B6878 File Offset: 0x001B4A78
	public void OnClickStartGame()
	{
		Notice.instance.Send(NoticeName.OnClickStartGame, new object[0]);
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			return;
		}
		if (!this.StartAble)
		{
			return;
		}
		this.canvas.gameObject.SetActive(false);
		this.OnManagementStart();
		this._isGameStarted = true;
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x001B68D8 File Offset: 0x001B4AD8
	public void OnManagementStart()
	{
		if (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL && GlobalGameManager.instance.gameMode != GameMode.UNLIMIT_MODE)
		{
			GlobalGameManager.instance.SaveData(false);
			GlobalGameManager.instance.SaveGlobalData();
		}
		this.sefiraList.OnManageStart();
		CameraMover.instance.SetSettingToStart();
		GameManager.currentGameManager.StartGame();
		SefiraBossUI.Instance.OnStageStart();
	}

	// Token: 0x060049F2 RID: 18930 RVA: 0x0003DB80 File Offset: 0x0003BD80
	public void MakeStartSound()
	{
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated())
		{
			base.transform.GetComponent<AudioClipPlayer>().OnPlayInList(16);
		}
		else
		{
			base.transform.GetComponent<AudioClipPlayer>().OnPlayInList(9);
		}
	}

	// Token: 0x060049F3 RID: 18931 RVA: 0x001B6944 File Offset: 0x001B4B44
	public void OnSetLevel(SefiraLevel level)
	{
		foreach (MaskableGraphic maskableGraphic in this.coloredTargets)
		{
		}
		for (int i = 0; i < 2; i++)
		{
			Button button = this.AreaMoveButton[i];
			button.OnPointerExit(null);
		}
		if (level != SefiraLevel.UP)
		{
			if (level != SefiraLevel.MIDDILE)
			{
				if (level == SefiraLevel.DOWN)
				{
					if (this._currentLevel != SefiraLevel.DOWN)
					{
						this.AreaMoveAnim.SetInteger("Level", 2);
					}
				}
			}
			else if (this._currentLevel != SefiraLevel.MIDDILE)
			{
				this.AreaMoveAnim.SetInteger("Level", 1);
			}
		}
		else if (this._currentLevel != SefiraLevel.UP)
		{
			this.AreaMoveAnim.SetInteger("Level", 0);
		}
		this.SetDayIcon();
		this._currentLevel = level;
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x001B6A48 File Offset: 0x001B4C48
	public void OnClickMoveArea(int i)
	{
		DeployUI.instance.GetComponent<AudioClipPlayer>().OnPlayInList(8);
		if (i != 0)
		{
			if (i != 1)
			{
				if (i == 2)
				{
					if (SefiraManager.instance.IsOpened(SefiraEnum.BINAH) || SefiraManager.instance.IsOpened(SefiraEnum.CHOKHMAH))
					{
						this.CurrentLevel = SefiraLevel.DOWN;
					}
				}
			}
			else if (SefiraManager.instance.IsOpened(SefiraEnum.TIPERERTH1))
			{
				this.CurrentLevel = SefiraLevel.MIDDILE;
			}
		}
		else
		{
			this.CurrentLevel = SefiraLevel.UP;
		}
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x001B6AD4 File Offset: 0x001B4CD4
	public void SefiraTutorialIncrease()
	{
		if (this.currentTutorialIndex == SefiraBossUI.Instance.tutorialSpriteSet.Count - 1)
		{
			this.SefiraBossTutorial_Right.interactable = false;
			this.OnBossTutorialEnd();
			return;
		}
		this.currentTutorialIndex++;
		this.SetBossTutorial();
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0003DBBA File Offset: 0x0003BDBA
	public void SefiraTutorialDecrease()
	{
		if (this.currentTutorialIndex == 0)
		{
			return;
		}
		this.currentTutorialIndex--;
		this.SetBossTutorial();
		if (this.currentTutorialIndex == 0)
		{
			this.SefiraBossTutorial_Left.interactable = false;
		}
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x001B6B24 File Offset: 0x001B4D24
	private void InputCheck()
	{
		if (this.IsEnabled && !this._isGameStarted)
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !CreatureInfoWindow.CurrentWindow.IsEnabled && !InventoryUI.CurrentWindow.IsEnabled && !OptionUI.Instance.IsEnabled)
			{
				if (EscapeUI.instance.IsOpened)
				{
					EscapeUI.CloseWindow();
				}
				else
				{
					EscapeUI.OpenWindow();
				}
			}
			if (SefiraBossManager.Instance.IsTutorial)
			{
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					this.SefiraTutorialIncrease();
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					this.SefiraTutorialDecrease();
				}
			}
		}
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x001B6BDC File Offset: 0x001B4DDC
	private void MoveSefira(global::MoveDirection dir)
	{
		if (!SefiraManager.instance.IsOpened(SefiraEnum.TIPERERTH1))
		{
			return;
		}
		if (dir == global::MoveDirection.UP)
		{
			if (this.CurrentLevel == SefiraLevel.MIDDILE)
			{
				this.OnClickMoveArea(0);
			}
			else if (this.CurrentLevel == SefiraLevel.DOWN)
			{
				this.OnClickMoveArea(1);
			}
		}
		else if (dir == global::MoveDirection.DOWN)
		{
			if (this.CurrentLevel == SefiraLevel.UP)
			{
				this.OnClickMoveArea(1);
			}
			else if (this.CurrentLevel == SefiraLevel.MIDDILE)
			{
				this.OnClickMoveArea(2);
			}
		}
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x001B6C64 File Offset: 0x001B4E64
	public void OnScroll(PointerEventData eventData)
	{
		if (this._moveAreaTimer.started)
		{
			return;
		}
		this._moveAreaTimer.StartTimer(0.5f);
		if (eventData.scrollDelta.y >= 0f)
		{
			this.MoveSefira(global::MoveDirection.UP);
		}
		else if (eventData.scrollDelta.y < 0f)
		{
			this.MoveSefira(global::MoveDirection.DOWN);
		}
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x0003DBF3 File Offset: 0x0003BDF3
	public void OnStartButtonEnter()
	{
		this._startButtonOverlayed = true;
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x0003DBFC File Offset: 0x0003BDFC
	public void OnStartButtonExit()
	{
		this._startButtonOverlayed = false;
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x0003DC05 File Offset: 0x0003BE05
	private void OnSetStartState(bool state)
	{
		this._startAble = state;
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x001B6CD8 File Offset: 0x001B4ED8
	private void CheckStartState()
	{
		Sefira sefira = null;
		this.StartAble = SefiraManager.instance.StartValidateCheck(ref sefira);
		if (this.StartAble)
		{
			if (SefiraBossManager.Instance.IsAnyBossSessionActivated())
			{
				this.StartButtonText.text = LocalizeTextDataModel.instance.GetText("SefiraBossSessionStart");
			}
			else
			{
				this.StartButtonText.text = LocalizeTextDataModel.instance.GetText("ManagementStart");
			}
			if (!this._startButtonOverlayed)
			{
				this.StartButtonText.color = this.CurrentDeployColor;
				this.StartButtonTexture.color = this.UIDefaultBlack;
			}
			else
			{
				this.StartButtonText.color = this.UIDefaultBlack;
				this.StartButtonTexture.color = this.CurrentDeployColor;
			}
		}
		else
		{
			this.StartButtonText.text = LocalizeTextDataModel.instance.GetText("NeedAgent");
			if (!this._startButtonOverlayed)
			{
				this.StartButtonText.color = UIColorManager.instance.UIRedColor;
			}
			else
			{
				this.StartButtonText.color = this.UIDefaultBlack;
			}
		}
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x001B6DF8 File Offset: 0x001B4FF8
	public void SetOrdealText(OrdealLevel level)
	{
		if (this.Day < OrdealGenInfo._dawnAdditionDay || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
		{
			this.OrdealText.text = LocalizeTextDataModel.instance.GetText("Inventory_NoRequire");
			return;
		}
		switch (level)
		{
		case OrdealLevel.DAWN:
			this.OrdealText.text = LocalizeTextDataModel.instance.GetText("Ordeal_Dawn");
			break;
		case OrdealLevel.NOON:
			this.OrdealText.text = LocalizeTextDataModel.instance.GetText("Ordeal_Noon");
			break;
		case OrdealLevel.DUSK:
			this.OrdealText.text = LocalizeTextDataModel.instance.GetText("Ordeal_Dusk");
			break;
		case OrdealLevel.MIDNIGHT:
			this.OrdealText.text = LocalizeTextDataModel.instance.GetText("Ordeal_Midnight");
			break;
		}
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x0003DC0E File Offset: 0x0003BE0E
	public void OpenCustomizingWindow()
	{
		AgentInfoWindow.GenerateWindow();
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x0001DF7F File Offset: 0x0001C17F
	public void OpenInventroyUI()
	{
		InventoryUI.CreateWindow();
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x001B6ED8 File Offset: 0x001B50D8
	public bool OnClickSefiraBossSession(SefiraEnum sefira)
	{ // <Mod>
		if (SefiraBossManager.Instance.CheckBossActivation(sefira))
		{
			SefiraBossManager.Instance.SetActivatedBoss(SefiraEnum.DUMMY);
			SefiraBossUI.Instance.OnExitSefiraBossSession();
			return false;
		}
		SefiraEnum currentActivatedSefira = SefiraBossManager.Instance.CurrentActivatedSefira;
		if (currentActivatedSefira != SefiraEnum.DUMMY)
		{
			SefiraPanel sefiraPanel = this.sefiraList.GetSefiraPanel(SefiraManager.instance.GetSefira(SefiraBossManager.Instance.CurrentActivatedSefira).indexString);
			if (sefiraPanel != null)
			{
				sefiraPanel.DisableSefiraBoss();
			}
		}
		bool isOvertime = MissionManager.instance.GetBossMission(sefira).metaInfo.sefira_Level > 5;
		if (SefiraBossManager.Instance.OnStartBossSession(sefira, isOvertime))
		{
			SefiraBossUI.Instance.OnEnterSefiraBossSession();
			base.transform.GetComponent<AudioClipPlayer>().OnPlayInList(15);
			return true;
		}
		return false;
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x0003DC16 File Offset: 0x0003BE16
	public void OpenManual()
	{
		ManualUI.Instance.OpenManual();
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x0003DC22 File Offset: 0x0003BE22
	static DeployUI()
	{
	}

	// Token: 0x0400443B RID: 17467
	public const int AgentMaxLevel = 5;

	// Token: 0x0400443C RID: 17468
	public const int AgentHireCost = 1;

	// Token: 0x0400443D RID: 17469
	public const int AgentCustomCost = 1;

	// Token: 0x0400443E RID: 17470
	public const string Point = "LOB";

	// Token: 0x0400443F RID: 17471
	public const int CheckPointDayCount = 5;

	// Token: 0x04004440 RID: 17472
	public static int[] CusomAgentHireCost = new int[]
	{
		4,
		5,
		6,
		8,
		10
	};

	// Token: 0x04004441 RID: 17473
	private static DeployUI _instance = null;

	// Token: 0x04004442 RID: 17474
	private List<IDeployResetCalled> resetTargets = new List<IDeployResetCalled>();

	// Token: 0x04004443 RID: 17475
	private SefiraLevel _currentLevel;

	// Token: 0x04004444 RID: 17476
	public DeploySefiraList sefiraList;

	// Token: 0x04004445 RID: 17477
	public DeployAgentList agentList;

	// Token: 0x04004446 RID: 17478
	public CheckPointUI checkPointUI;

	// Token: 0x04004447 RID: 17479
	public ResearchWindow researchWindow;

	// Token: 0x04004448 RID: 17480
	[Space(10f)]
	public Text dayCount;

	// Token: 0x04004449 RID: 17481
	public Text pointCount;

	// Token: 0x0400444A RID: 17482
	public Color UIDefaultBlack;

	// Token: 0x0400444B RID: 17483
	public Color UIOverlayColor;

	// Token: 0x0400444C RID: 17484
	public Color UIDefaultFill;

	// Token: 0x0400444D RID: 17485
	public Sprite[] AgentGradeImage;

	// Token: 0x0400444E RID: 17486
	public Sprite[] AgentLifeStyleImage;

	// Token: 0x0400444F RID: 17487
	public Color[] AgentLifeStyleThemeColor;

	// Token: 0x04004450 RID: 17488
	public Sprite Gender_Male;

	// Token: 0x04004451 RID: 17489
	public Sprite Gender_Female;

	// Token: 0x04004452 RID: 17490
	public Sprite[] defaultWorkIcon;

	// Token: 0x04004453 RID: 17491
	public Canvas canvas;

	// Token: 0x04004454 RID: 17492
	public Vector2 CanavsRect;

	// Token: 0x04004455 RID: 17493
	public Color[] RiskLevelColor;

	// Token: 0x04004456 RID: 17494
	public Image[] DayCircleImage;

	// Token: 0x04004457 RID: 17495
	public Text goal;

	// Token: 0x04004458 RID: 17496
	public Text ordeal;

	// Token: 0x04004459 RID: 17497
	public RectTransform MoveControl;

	// Token: 0x0400445A RID: 17498
	public List<ScrollExchanger> scroll;

	// Token: 0x0400445B RID: 17499
	public Button[] AreaMoveButton;

	// Token: 0x0400445C RID: 17500
	public Animator AreaMoveAnim;

	// Token: 0x0400445D RID: 17501
	[Space(15f)]
	public List<MaskableGraphic> coloredTargets;

	// Token: 0x0400445E RID: 17502
	[Space(15f)]
	public Color[] DeployColorSet;

	// Token: 0x0400445F RID: 17503
	private int _currentColorIndex;

	// Token: 0x04004460 RID: 17504
	public Text StartButtonText;

	// Token: 0x04004461 RID: 17505
	public Image StartButtonTexture;

	// Token: 0x04004462 RID: 17506
	public Text OrdealText;

	// Token: 0x04004463 RID: 17507
	[Header("MiddleAreaConnection")]
	public List<GameObject> middleAreaConnected;

	// Token: 0x04004464 RID: 17508
	[Header("LowerAreaConnection")]
	public List<GameObject> lowerAreaConnected;

	// Token: 0x04004465 RID: 17509
	[Header("SefiraBossTutorial")]
	public GameObject SefiraBossTutorialRoot;

	// Token: 0x04004466 RID: 17510
	public RectTransform SefiraBossTutorial_VerticalText;

	// Token: 0x04004467 RID: 17511
	public RectTransform SefiraBossTutorial_HorizontalText;

	// Token: 0x04004468 RID: 17512
	public List<Text> SefiraBossTutorial_Text;

	// Token: 0x04004469 RID: 17513
	public Image SefiraBossTutorialBlock;

	// Token: 0x0400446A RID: 17514
	public GameObject SefiraBossZeroEmpty;

	// Token: 0x0400446B RID: 17515
	public Button SefiraBossTutorial_Right;

	// Token: 0x0400446C RID: 17516
	public Button SefiraBossTutorial_Left;

	// Token: 0x0400446D RID: 17517
	public Text SefiraBossTutorialIndex;

	// Token: 0x0400446E RID: 17518
	public GameObject[] KetherConnected;

	// Token: 0x0400446F RID: 17519
	private int currentTutorialIndex;

	// Token: 0x04004470 RID: 17520
	private Queue<Sefira> ResearchUpgradeWaitQueue = new Queue<Sefira>();

	// Token: 0x04004471 RID: 17521
	private Timer _moveAreaTimer = new Timer();

	// Token: 0x04004472 RID: 17522
	private bool _startButtonOverlayed;

	// Token: 0x04004473 RID: 17523
	private bool _startAble;

	// Token: 0x04004474 RID: 17524
	private bool _isEnabled;

	// Token: 0x04004475 RID: 17525
	private bool _isGameStarted;
}
