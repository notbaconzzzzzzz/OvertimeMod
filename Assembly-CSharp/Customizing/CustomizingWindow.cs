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
		{
			this._currentAgent = agent;
			this.CurrentData.stat = agent.primaryStat;
			this.CurrentData.originalModel = agent;
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
		{
			if (!this.IsEnabled)
			{
				return;
			}
			this.CostUpdate();
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0002BF48 File Offset: 0x0002A148
		public void OpenAppearanceWindow()
		{
			this.appearanceBlock.SetActive(false);
			this.appearanceUI.OpenWindow(this.CurrentData);
			this.CurrentData.isCustomAppearance = true;
			OverlayManager.Instance.ClearOverlay();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x00003E35 File Offset: 0x00002035
		public void UpdatePortrait()
		{
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x00135184 File Offset: 0x00133384
		private int GetCost()
		{
			int result;
			try
			{
				int num = (this.CurrentWindowType != CustomizingType.GENERATE) ? 0 : 1;
				if (this.CurrentData.isCustomAppearance)
				{
					num++;
				}
				if (this.CurrentData.isCustomName)
				{
					num = num;
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
		{
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
		{
			int level = agent.level;
			agent.primaryStat.hp = this.SetRandomStatValue(agent.primaryStat.hp, agent.Rstat, data.statBonus.rBonus);
			agent.primaryStat.mental = this.SetRandomStatValue(agent.primaryStat.mental, agent.Wstat, data.statBonus.wBonus);
			agent.primaryStat.work = this.SetRandomStatValue(agent.primaryStat.work, agent.Bstat, data.statBonus.bBonus);
			agent.primaryStat.battle = this.SetRandomStatValue(agent.primaryStat.battle, agent.Pstat, data.statBonus.pBonus);
			agent.UpdateTitle(level);
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
			return UnityEngine.Random.Range(min, max);
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
