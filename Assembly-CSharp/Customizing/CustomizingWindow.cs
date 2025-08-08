/*
private void ReviseOpenAction(AgentModel agent) // Recustomizing; Title Specification
private void Update() // Copy Paste
public void Confirm() // Recustomizing
public void OpenAppearanceWindow() // Title Specification
private int GetCost() // Customizing appearence is now free; Title Specification
public void SetAgentStatBonus(AgentModel agent, AgentData data) // 
+public bool CopyAppearanceToClipboard() // 
+public bool PasteAppearanceFromClipboard() // 
+public static int GetTitleData(int tier, int ind) // Title Specification
+public static int GetTitleIndex(int ind) // Title Specification
*/
using System;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;

namespace Customizing
{
	// Token: 0x02000515 RID: 1301
	public class CustomizingWindow : MonoBehaviour
	{
		// Token: 0x06002E3B RID: 11835 RVA: 0x00004094 File Offset: 0x00002294
		public CustomizingWindow()
		{
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x0002BE71 File Offset: 0x0002A071
		public static CustomizingWindow CurrentWindow
		{
			get
			{
				return CustomizingWindow._currentWindow;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x0002BE78 File Offset: 0x0002A078
		public AgentModel CurrentAgent
		{
			get
			{
				return this._currentAgent;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x0002BE80 File Offset: 0x0002A080
		// (set) Token: 0x06002E3F RID: 11839 RVA: 0x0002BE88 File Offset: 0x0002A088
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
				this.rootObject.SetActive(this._isEnabled);
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x0002BEA2 File Offset: 0x0002A0A2
		public CustomizingType CurrentWindowType
		{
			get
			{
				return this._currentWindowType;
			}
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x001349C4 File Offset: 0x00132BC4
		private void OpenAction()
		{
			this.appearanceBlock.SetActive(true);
			CustomizingWindow.CurrentWindow.CurrentData = new AgentData();
			CustomizingWindow.CurrentWindow.IsEnabled = true;
			this.CurrentLobPoint.text = MoneyModel.instance.money + string.Empty;
			this.CurrentCost.text = 1 + string.Empty;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x00134A38 File Offset: 0x00132C38
		public void GenRandomSpriteSet(ref AgentData data)
		{
			WorkerSprite.WorkerSprite workerSprite = new WorkerSprite.WorkerSprite();
			WorkerSpriteManager.instance.GetRandomBasicData(workerSprite, true);
			data.appearance = new Appearance
			{
				spriteSet = workerSprite,
				Eyebrow_Battle = workerSprite.BattleEyeBrow,
				FrontHair = workerSprite.FrontHair,
				RearHair = workerSprite.RearHair,
				Eyebrow_Def = workerSprite.EyeBrow,
				Eyebrow_Panic = workerSprite.PanicEyeBrow,
				Eye_Battle = workerSprite.BattleEyeBrow,
				Eye_Def = workerSprite.Eye,
				Eye_Panic = workerSprite.EyePanic,
				Eye_Dead = workerSprite.EyeDead,
				Mouth_Def = workerSprite.Mouth,
				Mouth_Battle = workerSprite.BattleMouth,
				HairColor = workerSprite.HairColor,
				EyeColor = workerSprite.EyeColor
			};
			this.portrait.SetCustomizing(data, WorkerFaceType.DEFAULT);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x00134B18 File Offset: 0x00132D18
		public void GenUniqueSpriteSet(UniqueCreditAgentInfo info, ref AgentData data)
		{
			WorkerSprite.WorkerSprite workerSprite = new WorkerSprite.WorkerSprite();
			WorkerSpriteManager.instance.GetUniqueBasicData(workerSprite, info);
			data.appearance = new Appearance
			{
				spriteSet = workerSprite,
				Eyebrow_Battle = workerSprite.BattleEyeBrow,
				FrontHair = workerSprite.FrontHair,
				RearHair = workerSprite.RearHair,
				Eyebrow_Def = workerSprite.EyeBrow,
				Eyebrow_Panic = workerSprite.PanicEyeBrow,
				Eye_Def = workerSprite.Eye,
				Eye_Panic = workerSprite.EyePanic,
				Eye_Dead = workerSprite.EyeDead,
				Mouth_Def = workerSprite.Mouth,
				Mouth_Battle = workerSprite.BattleMouth,
				HairColor = workerSprite.HairColor,
				EyeColor = workerSprite.EyeColor
			};
			this.portrait.SetCustomizing(data, WorkerFaceType.DEFAULT);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x00134BEC File Offset: 0x00132DEC
		public void GenRandomFaceSpriteSet(ref AgentData data)
		{
			WorkerSprite.WorkerSprite workerSprite = new WorkerSprite.WorkerSprite();
			WorkerSpriteManager.instance.GetRandomBasicData(workerSprite, true);
			data.appearance.Eyebrow_Def = (data.appearance.spriteSet.EyeBrow = workerSprite.EyeBrow);
			data.appearance.Eyebrow_Panic = (data.appearance.spriteSet.PanicEyeBrow = workerSprite.PanicEyeBrow);
			data.appearance.Eyebrow_Battle = (data.appearance.spriteSet.BattleEyeBrow = workerSprite.BattleEyeBrow);
			Appearance appearance = data.appearance;
			Sprite sprite = workerSprite.Eye;
			data.appearance.spriteSet.Eye = sprite;
			appearance.Eye_Def = sprite;
			data.appearance.Eye_Panic = (data.appearance.spriteSet.EyePanic = workerSprite.EyePanic);
			data.appearance.Eye_Dead = (data.appearance.spriteSet.EyeDead = workerSprite.EyeDead);
			Appearance appearance2 = data.appearance;
			sprite = workerSprite.Mouth;
			data.appearance.spriteSet.Mouth = sprite;
			appearance2.Mouth_Def = sprite;
			data.appearance.Mouth_Battle = (data.appearance.spriteSet.BattleMouth = workerSprite.BattleMouth);
			data.appearance.EyeColor = (data.appearance.spriteSet.EyeColor = workerSprite.EyeColor);
			this.portrait.SetCustomizing(data, WorkerFaceType.DEFAULT);
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x00134D70 File Offset: 0x00132F70
		public void GenRandomHairSpriteSet(ref AgentData data)
		{
			WorkerSprite.WorkerSprite workerSprite = new WorkerSprite.WorkerSprite();
			WorkerSpriteManager.instance.GetRandomBasicData(workerSprite, true);
			data.appearance.FrontHair = workerSprite.FrontHair;
			data.appearance.RearHair = workerSprite.RearHair;
			data.appearance.spriteSet.FrontHair = workerSprite.FrontHair;
			data.appearance.spriteSet.RearHair = workerSprite.RearHair;
			data.appearance.HairColor = workerSprite.HairColor;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x00134DF4 File Offset: 0x00132FF4
		private void GenOpenAction()
		{
			this.GenRandomSpriteSet(ref this.CurrentData);
			this.CurrentData.stat = AgentModel.GetDefaultStat();
			AgentName fakeNameByInfo = AgentNameList.instance.GetFakeNameByInfo();
			if (fakeNameByInfo != null)
			{
				this.CurrentData.agentName = fakeNameByInfo;
				return;
			}
			Debug.LogError("Failed To Load AgentName");
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x0002BEAA File Offset: 0x0002A0AA
		private void ReviseOpenAction(AgentModel agent)
		{ // <Mod>
			CurrentData.agentName = agent._agentName;
			CurrentData.CustomName = agent.name;
			CurrentData.isCustomName = agent.iscustom;
			CurrentData.appearance = new Appearance
			{
				spriteSet = agent.spriteData,
				Eyebrow_Battle = agent.spriteData.BattleEyeBrow,
				FrontHair = agent.spriteData.FrontHair,
				RearHair = agent.spriteData.RearHair,
				Eyebrow_Def = agent.spriteData.EyeBrow,
				Eyebrow_Panic = agent.spriteData.PanicEyeBrow,
				Eye_Battle = agent.spriteData.BattleEyeBrow,
				Eye_Def = agent.spriteData.Eye,
				Eye_Panic = agent.spriteData.EyePanic,
				Eye_Dead = agent.spriteData.EyeDead,
				Mouth_Def = agent.spriteData.Mouth,
				Mouth_Battle = agent.spriteData.BattleMouth,
				HairColor = agent.spriteData.HairColor,
				EyeColor = agent.spriteData.EyeColor
			};
			this._currentAgent = agent;
			this.CurrentData.stat = agent.primaryStat;
			this.CurrentData.originalModel = agent;
			for (int i = 0; i < 4; i++)
			{
				CurrentData.customTitles[i] = GetTitleIndex(agent.forceTitles[i]);
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00134E44 File Offset: 0x00133044
		public static void ReviseWindow(AgentModel target)
		{
			if (target == null)
			{
				return;
			}
			CustomizingWindow.CurrentWindow.appearanceUI.CloseWindow();
			CustomizingWindow.CurrentWindow.OpenAction();
			CustomizingWindow.CurrentWindow.ReviseOpenAction(target);
			CustomizingWindow.CurrentWindow.statUI.Init(CustomizingWindow.CurrentWindow.CurrentData, false);
			CustomizingWindow.CurrentWindow._currentWindowType = CustomizingType.REVISE;
			TooltipMouseOver componentInChildren = CustomizingWindow.CurrentWindow.ConfirmButton.GetComponentInChildren<TooltipMouseOver>();
			componentInChildren.ID = "Tooltip_Agent@Confirm_Reinforcement";
			TooltipMouseOver componentInChildren2 = CustomizingWindow.CurrentWindow.CancelButton.GetComponentInChildren<TooltipMouseOver>();
			componentInChildren2.ID = "Tooltip_Agent@Cancel_Reinforcement";
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x00134ED8 File Offset: 0x001330D8
		public static void GenerationWindow()
		{
			CustomizingWindow.CurrentWindow.appearanceUI.CloseWindow();
			CustomizingWindow.CurrentWindow.OpenAction();
			CustomizingWindow.CurrentWindow.GenOpenAction();
			CustomizingWindow.CurrentWindow.statUI.Init(CustomizingWindow.CurrentWindow.CurrentData, true);
			CustomizingWindow.CurrentWindow._currentWindowType = CustomizingType.GENERATE;
			TooltipMouseOver componentInChildren = CustomizingWindow.CurrentWindow.ConfirmButton.GetComponentInChildren<TooltipMouseOver>();
			componentInChildren.ID = "Tooltip_Agent@Confirm_Employment";
			TooltipMouseOver componentInChildren2 = CustomizingWindow.CurrentWindow.CancelButton.GetComponentInChildren<TooltipMouseOver>();
			componentInChildren2.ID = "Tooltip_Agent@Cancel_Employment";
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0002BED0 File Offset: 0x0002A0D0
		public static void CloseWindow()
		{
			CustomizingWindow.CurrentWindow._currentAgent = null;
			CustomizingWindow.CurrentWindow.CurrentData = null;
			CustomizingWindow.CurrentWindow.IsEnabled = false;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x0002BEF3 File Offset: 0x0002A0F3
		private void Awake()
		{
			this.appearanceUI.portrait = this.portrait;
			this.AppearanceCostText.text = 1 + string.Empty;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x00134F64 File Offset: 0x00133164
		public void AgentInfoWindowInit()
		{
			if (CustomizingWindow.CurrentWindow != null && CustomizingWindow.CurrentWindow.gameObject != base.gameObject)
			{
				UnityEngine.Object.Destroy(CustomizingWindow.CurrentWindow.gameObject);
			}
			CustomizingWindow._currentWindow = this;
			this.statUI.Init();
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x0002BF21 File Offset: 0x0002A121
		private void Start()
		{
			this.appearanceUI.CloseWindow();
			this.SetText();
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x00134FBC File Offset: 0x001331BC
		private void SetText()
		{
			this.EyeTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Eye");
			this.BattleEyeTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_DeadEye");
			this.PanicEyeTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_PanicEye");
			this.EyebrowTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Eyebrow");
			this.BattleEyebrowTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_BattleEyebrow");
			this.PanicEyebrowTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_PanicEyebrow");
			this.MouthTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Mouth");
			this.BattleMouthTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_BattleMouth");
			this.NameTitle.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_Name");
			this.BlockText.text = LocalizeTextDataModel.instance.GetText("Customizing_Appearance");
			this.CurrentPointTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_CurrentPoint");
			this.CostTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Cost");
			this.CancelButtonText.text = LocalizeTextDataModel.instance.GetText("Customizing_Cancel");
			this.FrontHairTitle.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Customizing_Hair_Front"), LocalizeTextDataModel.instance.GetText("Customizing_Hair"));
			this.RearHairTitle.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Customizing_Hair_Rear"), LocalizeTextDataModel.instance.GetText("Customizing_Hair"));
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x0002BF34 File Offset: 0x0002A134
		private void Update()
		{ // <Mod>
			if (!this.IsEnabled)
			{
				return;
			}
			this.CostUpdate();
			if (Input.GetKeyDown(KeyCode.C) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				if (!CurrentData.isCustomAppearance)
				{
					OpenAppearanceWindow();
				}
				CopyAppearanceToClipboard();
			}
			if (Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				if (!CurrentData.isCustomAppearance)
				{
					OpenAppearanceWindow();
				}
				PasteAppearanceFromClipboard();
			}
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0002BF48 File Offset: 0x0002A148
		public void OpenAppearanceWindow()
		{ // <Mod> title specification
			this.appearanceBlock.SetActive(false);
			this.appearanceUI.OpenWindow(this.CurrentData);
			this.CurrentData.isCustomAppearance = true;
			this.CurrentData.isCustomTitles = false;
			OverlayManager.Instance.ClearOverlay();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x00003E35 File Offset: 0x00002035
		public void UpdatePortrait()
		{
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x00135184 File Offset: 0x00133384
		private int GetCost()
		{ // <Mod> customize appearence is now free; title specification
			int result;
			try
			{
				int num = (this.CurrentWindowType != CustomizingType.GENERATE) ? 0 : 1;
				if (this.CurrentData.isCustomAppearance && !SpecialModeConfig.instance.GetValue<bool>("FreeCustomAppearance"))
				{
					num++;
				}
				if (this.CurrentData.isCustomName)
				{
					num = num;
				}
				if (this.CurrentData.isCustomTitles)
				{
					num++;
				}
				num += this.statUI.CurrentAdditionalCost;
				result = num;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("{0}\r\n{1}", ex.ToString(), ex.StackTrace));
				result = -1;
			}
			return result;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x00135214 File Offset: 0x00133414
		private void CostUpdate()
		{
			int cost = this.GetCost();
			this.CurrentCost.text = cost + string.Empty;
			int money = MoneyModel.instance.money;
			if (money < cost)
			{
				this.CurrentCost.color = this.Red;
				this.CurrentCostPrefix.color = this.Red;
				this.ConfirmButton.interactable = false;
				this.ConfirmText.text = LocalizeTextDataModel.instance.GetText("NeedCost");
			}
			else
			{
				this.CurrentCost.color = this.Blue;
				this.CurrentCostPrefix.color = this.Blue;
				this.ConfirmButton.interactable = true;
				this.ConfirmText.text = ((this.CurrentWindowType != CustomizingType.GENERATE) ? LocalizeTextDataModel.instance.GetText("Customizing_Confirm") : LocalizeTextDataModel.instance.GetText("Hire"));
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x0013530C File Offset: 0x0013350C
		public void Cancel()
		{
			OverlayManager.Instance.ClearOverlay();
			CustomizingWindow.CloseWindow();
			this.appearanceUI.copied = null;
			if (this.CurrentWindowType == CustomizingType.GENERATE)
			{
				AgentInfoWindow.currentWindow.CloseWindow();
			}
			else
			{
				AgentInfoWindow.CreateWindow(AgentInfoWindow.currentWindow.CurrentAgent, true);
			}
			this.CurrentData = null;
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x00135368 File Offset: 0x00133568
		public void Confirm()
		{ // <Mod>
			OverlayManager.Instance.ClearOverlay();
			if (this.CurrentData == null)
			{
				return;
			}
			int cost = this.GetCost();
			if (!MoneyModel.instance.Pay(cost))
			{
				return;
			}
			try
			{
				this.CurrentData.statBonus = this.statUI.GetBonus();
				if (this.CurrentWindowType == CustomizingType.GENERATE)
				{
					if (this.appearanceUI.copied != null)
					{
						this.CurrentData.AppearCopy(this.appearanceUI.copied);
						this.appearanceUI.copied = null;
					}
					this.CurrentData.appearance.SetResrouceData();
					WorkerSpriteManager.instance.SetAgentBasicData(this.CurrentData.appearance.spriteSet, this.CurrentData.appearance);
					if (this.CurrentData.isCustomName)
					{
						this.CurrentData.agentName = AgentNameList.instance.GetCustomNameByInfo(this.CurrentData.CustomName, -1);
					}
					else
					{
						AgentNameList.instance.ExtractFromPool(this.CurrentData.agentName);
					}
					AgentModel agentModel = AgentManager.instance.AddAgentModelCustom(this.CurrentData);
					agentModel.iscustom = this.CurrentData.isCustomName;
					agentModel.isUniqueCredit = this.CurrentData.isUniqueCredit;
					agentModel.uniqueScriptIndex = this.CurrentData.uniqueScriptIndex;
					this.SetAgentStatBonus(agentModel, this.CurrentData);
					DeployUI.instance.AddAgent(agentModel);
				}
				else
				{
					if (appearanceUI.copied != null)
					{
						CurrentData.AppearCopy(appearanceUI.copied);
						appearanceUI.copied = null;
					}
					if (!appearanceBlock.activeInHierarchy)
					{
						CurrentAgent._agentName = CurrentData.agentName;
						CurrentAgent._agentName.metaInfo.nameDic.Clear();
						CurrentAgent._agentName.nameDic.Clear();
						foreach (string key in SupportedLanguage.GetSupprotedList())
						{
							CurrentAgent._agentName.metaInfo.nameDic.Add(key, CurrentData.CustomName);
							CurrentAgent._agentName.nameDic.Add(key, CurrentData.CustomName);
						}
						CurrentAgent.name = CurrentData.CustomName;
						CurrentAgent.iscustom = CurrentData.isCustomName;
						if (CurrentData.isCustomTitles)
						{
							for (int i = 0; i < 4; i++)
							{
								CurrentAgent.forceTitles[i] = GetTitleData(i, CurrentData.customTitles[i]);
							}
							switch (CurrentAgent.level)
							{
								case 1:
								case 2:
									if (CurrentAgent.forceTitles[0] != 0) CurrentAgent.ForcelyChangePrefix(CurrentAgent.forceTitles[0]);
									break;
								case 3:
									if (CurrentAgent.forceTitles[0] != 0) CurrentAgent.ForcelyChangePrefix(CurrentAgent.forceTitles[0]);
									if (CurrentAgent.forceTitles[1] != 0) CurrentAgent.ForcelyChangeSuffix(CurrentAgent.forceTitles[1]);
									break;
								case 4:
									if (CurrentAgent.forceTitles[2] != 0) CurrentAgent.ForcelyChangePrefix(CurrentAgent.forceTitles[2]);
									if (CurrentAgent.forceTitles[1] != 0) CurrentAgent.ForcelyChangeSuffix(CurrentAgent.forceTitles[1]);
									break;
								case 5:
									if (CurrentAgent.forceTitles[2] != 0) CurrentAgent.ForcelyChangePrefix(CurrentAgent.forceTitles[2]);
									if (CurrentAgent.forceTitles[3] != 0) CurrentAgent.ForcelyChangeSuffix(CurrentAgent.forceTitles[3]);
									break;
							}
						}
						CurrentData.appearance.SetResrouceData();
						WorkerSpriteManager.instance.SetAgentBasicData(CurrentData.appearance.spriteSet, CurrentData.appearance);
					}
					this.SetAgentStatBonus(this.CurrentAgent, this.CurrentData);
					Notice.instance.Send(NoticeName.InitAgent, new object[]
					{
						this.CurrentAgent
					});
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				MoneyModel.instance.Add(cost);
			}
			Notice.instance.Send(NoticeName.OnWorkerReinforcementAccepted, new object[]
			{
				this.CurrentAgent
			});
			this.Cancel();
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0013554C File Offset: 0x0013374C
		public void SetAgentStatBonus(AgentModel agent, AgentData data)
		{ // <Mod> fixed issue where the wrong stat level could be chosen
			int level = agent.level;
			int RLevel = data.RLevel + data.statBonus.rBonus;
			int WLevel = data.WLevel + data.statBonus.wBonus;
			int BLevel = data.BLevel + data.statBonus.bBonus;
			int PLevel = data.PLevel + data.statBonus.pBonus;
			agent.primaryStat.hp = this.SetRandomStatValue(agent.primaryStat.hp, data.RLevel, data.statBonus.rBonus);
			agent.primaryStat.mental = this.SetRandomStatValue(agent.primaryStat.mental, data.WLevel, data.statBonus.wBonus);
			agent.primaryStat.work = this.SetRandomStatValue(agent.primaryStat.work, data.BLevel, data.statBonus.bBonus);
			agent.primaryStat.battle = this.SetRandomStatValue(agent.primaryStat.battle, data.PLevel, data.statBonus.pBonus);
			agent.UpdateTitle(level, false);
			int[] levelThresholds = {0, 30, 45, 65, 85, 100};
			if (agent.originFortitudeLevel > RLevel)
			{
				agent.primaryStat.hp = levelThresholds[RLevel] - agent.titleBonus.hp - 1;
			}
			if (agent.originPrudenceLevel > WLevel)
			{
				agent.primaryStat.mental = levelThresholds[WLevel] - agent.titleBonus.mental - 1;
			}
			if (agent.originTemperanceLevel > BLevel)
			{
				agent.primaryStat.work = levelThresholds[BLevel] - agent.titleBonus.workProb - 1;
			}
			if (agent.originJusticeLevel > PLevel)
			{
				agent.primaryStat.battle = levelThresholds[PLevel] - agent.titleBonus.attackSpeed - 1;
			}
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0013561C File Offset: 0x0013381C
		public int SetRandomStatValue(int original, int currentLevel, int bounusLevel)
		{
			int num = currentLevel + bounusLevel;
			if (bounusLevel == 0)
			{
				return original;
			}
			int min;
			int max;
			switch (num)
			{
			case 1:
				return AgentModel.GetDefaultLevel1Stat();
			case 2:
				min = 30;
				max = 37;
				break;
			case 3:
				min = 45;
				max = 55;
				break;
			case 4:
				min = 65;
				max = 75;
				break;
			case 5:
				min = 85;
				max = 92;
				break;
			case 6:
				min = 110;
				max = 130;
				break;
			default:
				return original;
			}
			if (SpecialModeConfig.instance.GetValue<bool>("NoEXP"))
			{
				return (min + max + 1) / 2;
			}
			return UnityEngine.Random.Range(min, max);
		}

		// <Mod>
		public bool CopyAppearanceToClipboard()
		{
			if (CurrentData == null)
			{
				return false;
			}
			try
			{
				string str = "";
				if (CurrentData.isCustomName)
				{
					str += CurrentData.CustomName;
					str.Replace(@"\", @"\\");
					str.Replace(@"-", @"\-");
					str += " - ";
				}
				else
				{
					str += _currentAgent.GetUnitName();
					str.Replace(@"\", "\\");
					str.Replace(@"-", @"\-");
					str += " - ";
				}
				Color hairColor = CurrentData.appearance.HairColor;
				str += (int)(hairColor.r * 255f);
				str += " ";
				str += (int)(hairColor.g * 255f);
				str += " ";
				str += (int)(hairColor.b * 255f);
				str += " - ";
				str += appearanceUI.frontHair.GetSpriteIndex(CurrentData.appearance.FrontHair);
				str += " ";
				str += appearanceUI.rearHair.GetSpriteIndex(CurrentData.appearance.RearHair);
				str += " - ";
				str += appearanceUI.eye_Def.GetSpriteIndex(CurrentData.appearance.Eye_Def);
				str += " ";
				str += appearanceUI.eyebrow_Def.GetSpriteIndex(CurrentData.appearance.Eyebrow_Def);
				str += " ";
				str += appearanceUI.mouth_Def.GetSpriteIndex(CurrentData.appearance.Mouth_Def);
				str += " - ";
				str += appearanceUI.eye_Panic.GetSpriteIndex(CurrentData.appearance.Eye_Panic);
				str += " ";
				str += appearanceUI.eyebrow_Battle.GetSpriteIndex(CurrentData.appearance.Eyebrow_Battle);
				str += " ";
				str += appearanceUI.mouth_Battle.GetSpriteIndex(CurrentData.appearance.Mouth_Battle);
				str += " - ";
				str += appearanceUI.eye_Dead.GetSpriteIndex(CurrentData.appearance.Eye_Dead);
				str += " ";
				str += appearanceUI.eyebrow_Panic.GetSpriteIndex(CurrentData.appearance.Eyebrow_Panic);
				bool flag = false;
				for (int i = 0; i < 4; i++)
				{
					if (CurrentData.customTitles[i] != 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					str += " - ";
					for (int i = 0; i < 4; i++)
					{
						int a = CurrentData.customTitles[i];
						char b;
						if (a <= 9)
						{
							b = (char)((int)'0' + a);
						}
						else
						{
							b = (char)((int)'A' + a - 10);
						}
					}
				}
				UnityEngine.GUIUtility.systemCopyBuffer = str;
				return true;
			}
			catch
			{

			}
			return false;
		}

		// <Mod>
		public bool PasteAppearanceFromClipboard()
		{
			if (CurrentData == null)
			{
				return false;
			}
			try
			{
				string str = UnityEngine.GUIUtility.systemCopyBuffer;
				if (str == "")
				{
					return false;
				}
				string[] sep = new string[]
				{
					" - "
				};
				string[] sections = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);
				if (sections.Length < 5)
				{
					return false;
				}
				int num = 0;
				if (sections.Length >= 6)
				{
					string name = sections[num];
					name.Replace(@"\-", @"-");
					name.Replace(@"\\", @"\");
					appearanceUI.NameInput.text = name;
					appearanceUI.OnSetNametext(name);
					num++;
				}
				try
				{
					string[] items = sections[num].Split(' ');
					Color hairColor = new Color((float)int.Parse(items[0]) / 255f, (float)int.Parse(items[1]) / 255f, (float)int.Parse(items[2]) / 255f);
					CurrentData.appearance.HairColor = hairColor;
					appearanceUI.hairColor.CurrentColor = hairColor;
				}
				catch
				{

				}
				num++;
				try
				{
					string[] items = sections[num].Split(' ');
					appearanceUI.frontHair.SetInitialIndex(int.Parse(items[0]));
					appearanceUI.rearHair.SetInitialIndex(int.Parse(items[1]));
				}
				catch
				{

				}
				num++;
				try
				{
					string[] items = sections[num].Split(' ');
					appearanceUI.eye_Def.SetInitialIndex(int.Parse(items[0]));
					appearanceUI.eyebrow_Def.SetInitialIndex(int.Parse(items[1]));
					appearanceUI.mouth_Def.SetInitialIndex(int.Parse(items[2]));
				}
				catch
				{

				}
				num++;
				try
				{
					string[] items = sections[num].Split(' ');
					appearanceUI.eye_Panic.SetInitialIndex(int.Parse(items[0]));
					appearanceUI.eyebrow_Battle.SetInitialIndex(int.Parse(items[1]));
					appearanceUI.mouth_Battle.SetInitialIndex(int.Parse(items[2]));
				}
				catch
				{

				}
				num++;
				try
				{
					string[] items = sections[num].Split(' ');
					appearanceUI.eye_Dead.SetInitialIndex(int.Parse(items[0]));
					appearanceUI.eyebrow_Panic.SetInitialIndex(int.Parse(items[1]));
				}
				catch
				{

				}
				num++;
				try
				{
					string[] items = sections[num].Split(' ');
					char[] chars = items[0].ToCharArray();
					bool flag = false;
					for (int i = 0; i < 4; i++)
					{
						char chr = chars[i];
						int c = (int)chr;
						int ind = 0;
						if (c >= (int)'1' && c <= (int)'9')
						{
							ind = c - (int)'0';
						}
						else if (c >= (int)'A' && c <= (int)'Z')
						{
							ind = c - (int)'A' + 10;
						}
						else if (c >= (int)'a' && c <= (int)'z')
						{
							ind = c - (int)'a' + 10;
						}
						if (GetTitleData(i, ind) != 0)
						{
							CurrentData.customTitles[i] = ind;
							flag = true;
						}
					}
					if (flag)
					{
						CurrentData.isCustomTitles = true;
					}
				}
				catch
				{

				}
				return true;
			}
			catch
			{

			}
			return false;
		}

		// <Mod>
		public static int GetTitleData(int tier, int ind)
		{
			int title = 0;
			if (ind == 0) return 0;
			switch (tier)
			{
				case 0:
					switch (ind)
					{
						case 1: title = 1007; break;
						case 2: title = 1013; break;
						case 3: title = 1003; break;
						case 4: title = 1015; break;
						case 5: title = 1016; break;
						case 6: title = 1004; break;
						case 7: title = 1011; break;
						case 8: title = 1010; break;
						case 9: title = 1002; break;
						case 10: title = 1001; break;
						case 11: title = 1014; break;
						case 12: title = 1008; break;
						case 13: title = 1009; break;
						case 14: title = 1006; break;
						case 15: title = 1005; break;
						case 16: title = 1012; break;
					}
					break;
				case 1:
					switch (ind)
					{
						case 1: title = 3013; break;
						case 2: title = 3001; break;
						case 3: title = 3002; break;
						case 4: title = 3004; break;
						case 5: title = 3003; break;
						case 6: title = 3005; break;
						case 7: title = 3006; break;
						case 8: title = 3007; break;
						case 9: title = 3008; break;
						case 10: title = 3009; break;
						case 11: title = 3010; break;
						case 12: title = 3011; break;
						case 13: title = 3012; break;
					}
					break;
				case 2:
					switch (ind)
					{
						case 1: title = 4001; break;
						case 2: title = 4017; break;
						case 3: title = 4011; break;
						case 4: title = 4003; break;
						case 5: title = 4018; break;
						case 6: title = 4013; break;
						case 7: title = 4010; break;
						case 8: title = 4015; break;
						case 9: title = 4019; break;
						case 10: title = 4020; break;
						case 11: title = 4009; break;
						case 12: title = 4007; break;
						case 13: title = 4006; break;
						case 14: title = 4016; break;
						case 15: title = 4014; break;
						case 16: title = 4008; break;
						case 17: title = 4012; break;
						case 18: title = 4004; break;
						case 19: title = 4005; break;
						case 20: title = 4002; break;
					}
					break;
				case 3:
					switch (ind)
					{
						case 1: title = 5002; break;
						case 2: title = 5001; break;
						case 3: title = 5003; break;
						case 4: title = 5005; break;
						case 5: title = 5006; break;
						case 6: title = 5004; break;
						case 7: title = 5007; break;
						case 8: title = 5009; break;
						case 9: title = 5008; break;
						case 10: title = 5010; break;
						case 11: title = 5011; break;
						case 12: title = 5012; break;
						case 13: title = 5013; break;
					}
					break;
			}
			return title;
			/*
			if (title == 0) return null;
			return AgentTitleTypeList.instance.GetData(title);*/
		}

		// <Mod>
		public static int GetTitleIndex(int ind)
		{
			switch (ind)
			{
				case 0:
					return 0;
				case 1007:
				case 3013:
				case 4001:
				case 5002:
					return 1;
				case 1013:
				case 3001:
				case 4017:
				case 5001:
					return 2;
				case 1003:
				case 3002:
				case 4011:
				case 5003:
					return 3;
				case 1015:
				case 3004:
				case 4003:
				case 5005:
					return 4;
				case 1016:
				case 3003:
				case 4018:
				case 5006:
					return 5;
				case 1004:
				case 3005:
				case 4013:
				case 5004:
					return 6;
				case 1011:
				case 3006:
				case 4010:
				case 5007:
					return 7;
				case 1010:
				case 3007:
				case 4015:
				case 5009:
					return 8;
				case 1002:
				case 3008:
				case 4019:
				case 5008:
					return 9;
				case 1001:
				case 3009:
				case 4020:
				case 5010:
					return 10;
				case 1014:
				case 3010:
				case 4009:
				case 5011:
					return 11;
				case 1008:
				case 3011:
				case 4007:
				case 5012:
					return 12;
				case 1009:
				case 3012:
				case 4006:
				case 5013:
					return 13;
				case 1006:
				case 4016:
					return 14;
				case 1005:
				case 4014:
					return 15;
				case 1012:
				case 4008:
					return 16;
				case 4012:
					return 17;
				case 4004:
					return 18;
				case 4005:
					return 19;
				case 4002:
					return 20;
			}
			return 0;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x00003E35 File Offset: 0x00002035
		static CustomizingWindow()
		{
		}

		// Token: 0x04002BDA RID: 11226
		private static CustomizingWindow _currentWindow;

		// Token: 0x04002BDB RID: 11227
		public const int NameCustomCost = 0;

		// Token: 0x04002BDC RID: 11228
		public const int AppearanceCustomCost = 1;

		// Token: 0x04002BDD RID: 11229
		public GameObject rootObject;

		// Token: 0x04002BDE RID: 11230
		public GameObject appearanceBlock;

		// Token: 0x04002BDF RID: 11231
		public GameObject buttonControl;

		// Token: 0x04002BE0 RID: 11232
		[Space(10f)]
		public AppearanceUI appearanceUI;

		// Token: 0x04002BE1 RID: 11233
		public StatUI statUI;

		// Token: 0x04002BE2 RID: 11234
		public WorkerPortraitSetter portrait;

		// Token: 0x04002BE3 RID: 11235
		[Header("Color")]
		public Color Red;

		// Token: 0x04002BE4 RID: 11236
		public Color Blue;

		// Token: 0x04002BE5 RID: 11237
		public Color Normal;

		// Token: 0x04002BE6 RID: 11238
		[Header("TitleTexts")]
		public Text FaceTitle;

		// Token: 0x04002BE7 RID: 11239
		public Text EyeTitle;

		// Token: 0x04002BE8 RID: 11240
		public Text BattleEyeTitle;

		// Token: 0x04002BE9 RID: 11241
		public Text PanicEyeTitle;

		// Token: 0x04002BEA RID: 11242
		public Text EyebrowTitle;

		// Token: 0x04002BEB RID: 11243
		public Text BattleEyebrowTitle;

		// Token: 0x04002BEC RID: 11244
		public Text PanicEyebrowTitle;

		// Token: 0x04002BED RID: 11245
		public Text MouthTitle;

		// Token: 0x04002BEE RID: 11246
		public Text BattleMouthTitle;

		// Token: 0x04002BEF RID: 11247
		public Text NameTitle;

		// Token: 0x04002BF0 RID: 11248
		public Text BlockText;

		// Token: 0x04002BF1 RID: 11249
		public Text CurrentPointTitle;

		// Token: 0x04002BF2 RID: 11250
		public Text CostTitle;

		// Token: 0x04002BF3 RID: 11251
		public Text CancelButtonText;

		// Token: 0x04002BF4 RID: 11252
		public Text FrontHairTitle;

		// Token: 0x04002BF5 RID: 11253
		public Text RearHairTitle;

		// Token: 0x04002BF6 RID: 11254
		[NonSerialized]
		private AgentModel _currentAgent;

		// Token: 0x04002BF7 RID: 11255
		public AgentData CurrentData;

		// Token: 0x04002BF8 RID: 11256
		public Text CurrentLobPoint;

		// Token: 0x04002BF9 RID: 11257
		public Text CurrentCost;

		// Token: 0x04002BFA RID: 11258
		public Text CurrentCostPrefix;

		// Token: 0x04002BFB RID: 11259
		public Text AppearanceCostText;

		// Token: 0x04002BFC RID: 11260
		public Button ConfirmButton;

		// Token: 0x04002BFD RID: 11261
		public Text ConfirmText;

		// Token: 0x04002BFE RID: 11262
		public Button CancelButton;

		// Token: 0x04002BFF RID: 11263
		private bool _isEnabled;

		// Token: 0x04002C00 RID: 11264
		private CustomizingType _currentWindowType;
	}
}
