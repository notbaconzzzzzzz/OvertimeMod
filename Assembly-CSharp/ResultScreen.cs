/*
Report PromotedAgents public void Make() // 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF2 RID: 2802
public class ResultScreen : MonoBehaviour, IAnimatorEventCalled
{
	// Token: 0x06005459 RID: 21593 RVA: 0x000044BC File Offset: 0x000026BC
	public ResultScreen()
	{
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x0600545A RID: 21594 RVA: 0x0004481D File Offset: 0x00042A1D
	public static ResultScreen instance
	{
		get
		{
			if (ResultScreen._instance == null)
			{
				ResultScreen._instance = new ResultScreen();
			}
			return ResultScreen._instance;
		}
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001E3E9C File Offset: 0x001E209C
	public void OnSuccessManagement()
	{
		this.controller.gameObject.SetActive(true);
		this.controller.Show();
		this.root.SetActive(true);
		this.report.OnManagementEnd();
		this.promotedScroll.MessageRecieveInit();
		this.missionScroll.MessageRecieveInit();
		CameraMover.instance.StopMove();
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x0004483E File Offset: 0x00042A3E
	public void OnEnable()
	{
		this.nextScneAnim.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x0600545D RID: 21597 RVA: 0x0004485B File Offset: 0x00042A5B
	public void Update()
	{
		if (!this.root.activeInHierarchy)
		{
			return;
		}
		this.report.Update();
	}

	// Token: 0x0600545E RID: 21598 RVA: 0x00044879 File Offset: 0x00042A79
	private void Awake()
	{
		if (ResultScreen._instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ResultScreen._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x001E3EFC File Offset: 0x001E20FC
	public void OnPointerEnter(int i)
	{
		ResultScreen.ReportButton button = this.report.GetButton(i);
		button.OnPointEnter();
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x001E3F1C File Offset: 0x001E211C
	public void OnPointerExit(int i)
	{
		ResultScreen.ReportButton button = this.report.GetButton(i);
		button.OnPointerExit();
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001E3F3C File Offset: 0x001E213C
	public void OnPointerClicK(int i)
	{
		ResultScreen.ReportButton button = this.report.GetButton(i);
		button.OnPointerClick();
	}

	// Token: 0x06005462 RID: 21602 RVA: 0x001E3F5C File Offset: 0x001E215C
	public void OnMoveNextStage()
	{
		if (this.root.activeInHierarchy)
		{
			ResultScreen.instance.report.nextDayClicked.SetActive(false);
			ResultScreen.instance.report.defaultButtons[0].SetActive(true);
			ResultScreen.instance.report.defaultButtons[1].SetActive(true);
			ResultScreen.instance.report.notButton[0].icons[1].SetActive(false);
			ResultScreen.instance.report.notButton[0].self.SetActive(false);
			GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
			GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
			CameraMover.instance.ReleaseMove();
			this.root.gameObject.SetActive(false);
			GameManager.currentGameManager.ExitStage();
			Notice.instance.Send(NoticeName.OnNextDay, new object[0]);
		}
	}

	// Token: 0x06005463 RID: 21603 RVA: 0x0000425D File Offset: 0x0000245D
	public void OnClickContinueGame()
	{
	}

	// Token: 0x06005464 RID: 21604 RVA: 0x000448A8 File Offset: 0x00042AA8
	public void OnCalled()
	{
		Debug.Log("NextScene");
		this.OnMoveNextStage();
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x00013C74 File Offset: 0x00011E74
	public void OnCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x00013C74 File Offset: 0x00011E74
	public void AgentReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x00013C74 File Offset: 0x00011E74
	public void SimpleReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x00013C74 File Offset: 0x00011E74
	public void AnimatorEventInit()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x00013C74 File Offset: 0x00011E74
	public void CreatureAnimCall(int i, CreatureBase script)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x00013C74 File Offset: 0x00011E74
	public void AttackCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x00013C74 File Offset: 0x00011E74
	public void AttackDamageTimeCalled()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x00013C74 File Offset: 0x00011E74
	public void SoundMake(string src)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x0000425D File Offset: 0x0000245D
	// Note: this type is marked as 'beforefieldinit'.
	static ResultScreen()
	{
	}

	// Token: 0x04004DD8 RID: 19928
	private static ResultScreen _instance;

	// Token: 0x04004DD9 RID: 19929
	private const string AgentSlotSrc = "UIComponent/ResultScreenAgentSlot";

	// Token: 0x04004DDA RID: 19930
	private const string MissionSlotSrc = "UIComponent/MissionSlot";

	// Token: 0x04004DDB RID: 19931
	public GameObject root;

	// Token: 0x04004DDC RID: 19932
	public ResultScreen.Report report;

	// Token: 0x04004DDD RID: 19933
	public ScrollExchanger promotedScroll;

	// Token: 0x04004DDE RID: 19934
	public ScrollExchanger missionScroll;

	// Token: 0x04004DDF RID: 19935
	public Image bg;

	// Token: 0x04004DE0 RID: 19936
	public Image graph;

	// Token: 0x04004DE1 RID: 19937
	public bool isInited;

	// Token: 0x04004DE2 RID: 19938
	public UIController controller;

	// Token: 0x04004DE3 RID: 19939
	public Animator nextScneAnim;

	// Token: 0x02000AF3 RID: 2803
	[Serializable]
	public class HistoryData
	{
		// Token: 0x0600546E RID: 21614 RVA: 0x00004230 File Offset: 0x00002430
		public HistoryData()
		{
		}

		// Token: 0x04004DE4 RID: 19940
		public Text[] datas;
	}

	// Token: 0x02000AF4 RID: 2804
	[Serializable]
	public class ReportButton
	{
		// Token: 0x0600546F RID: 21615 RVA: 0x000448BA File Offset: 0x00042ABA
		public ReportButton()
		{
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x001E4064 File Offset: 0x001E2264
		public void OnPointEnter()
		{
			this.bg.GetComponent<Image>().color = this.yellow;
			this.ScoreSound.OnPlayInList(0);
			if (this.index == 1)
			{
				this.icon.GetComponent<RectTransform>().localRotation = Quaternion.identity;
				ResultScreen.instance.report.defaultButtons[1].SetActive(false);
				ResultScreen.instance.report.notButton[0].self.SetActive(true);
				string textAppend = LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					"ResultScreen",
					"ReturnToSaveTail"
				});
				ResultScreen.instance.report.notButton[0].txt.text = string.Format("{0}    {1} DAY {2}", string.Empty, (GlobalGameManager.instance.PreLoadData() + 1).ToString(), (!(textAppend == "UNKNOWN")) ? textAppend : string.Empty);
			}
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x001E4168 File Offset: 0x001E2368
		public void OnPointerExit()
		{
			this.bg.GetComponent<Image>().color = this.Green;
			if (this.index == 1)
			{
				this.icon.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 70f);
				ResultScreen.instance.report.defaultButtons[1].SetActive(true);
				ResultScreen.instance.report.notButton[0].self.SetActive(false);
			}
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x001E41F0 File Offset: 0x001E23F0
		public void OnPointerClick()
		{
			this.OnPointerExit();
			switch (this.index)
			{
			case 1:
			{
				ResultScreen.instance.report.defaultButtons[0].SetActive(false);
				ResultScreen.instance.report.defaultButtons[1].SetActive(false);
				ResultScreen.instance.report.returnClicked.SetActive(true);
				ResultScreen.instance.report.notButton[0].self.SetActive(true);
				ResultScreen.instance.report.notButton[0].icons[0].SetActive(true);
				string textAppend = LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					"ResultScreen",
					"ReturnToSaveTail"
				});
				ResultScreen.instance.report.notButton[0].txt.text = string.Format("{0}    {1} DAY {2}", string.Empty, (GlobalGameManager.instance.PreLoadData() + 1).ToString(), (!(textAppend == "UNKNOWN")) ? textAppend : string.Empty);
				this.ScoreSound.OnPlayInList(1);
				break;
			}
			case 2:
				ResultScreen.instance.report.defaultButtons[0].SetActive(false);
				ResultScreen.instance.report.defaultButtons[1].SetActive(false);
				ResultScreen.instance.report.nextDayClicked.SetActive(true);
				ResultScreen.instance.report.notButton[0].self.SetActive(true);
				ResultScreen.instance.report.notButton[0].icons[1].SetActive(true);
				ResultScreen.instance.report.notButton[0].txt.text = "  " + LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					"ResultScreen",
					"NextDay"
				});
				this.ScoreSound.OnPlayInList(1);
				break;
			case 3:
				if (ResultScreen.instance.root.activeInHierarchy)
				{
					GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
					GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
				}
				this.ScoreSound.OnPlayInList(3);
				Notice.instance.Send(NoticeName.OnClickNextDayAcceptInResult, new object[0]);
				if (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL)
				{
					ResultScreen.instance.nextScneAnim.transform.parent.gameObject.SetActive(true);
					ResultScreen.instance.nextScneAnim.SetTrigger("Start_Main");
				}
				break;
			case 4:
				this.ScoreSound.OnPlayInList(2);
				ResultScreen.instance.report.nextDayClicked.SetActive(false);
				ResultScreen.instance.report.defaultButtons[0].SetActive(true);
				ResultScreen.instance.report.defaultButtons[1].SetActive(true);
				ResultScreen.instance.report.notButton[0].icons[1].SetActive(false);
				ResultScreen.instance.report.notButton[0].self.SetActive(false);
				break;
			case 5:
				this.ScoreSound.OnPlayInList(3);
				ResultScreen.instance.report.returnClicked.SetActive(false);
				ResultScreen.instance.report.defaultButtons[0].SetActive(true);
				ResultScreen.instance.report.defaultButtons[1].SetActive(true);
				ResultScreen.instance.report.notButton[0].icons[0].SetActive(false);
				ResultScreen.instance.report.notButton[0].self.SetActive(false);
				GameManager.currentGameManager.RestartGame();
				break;
			case 6:
				this.ScoreSound.OnPlayInList(2);
				ResultScreen.instance.report.returnClicked.SetActive(false);
				ResultScreen.instance.report.defaultButtons[0].SetActive(true);
				ResultScreen.instance.report.defaultButtons[1].SetActive(true);
				ResultScreen.instance.report.notButton[0].icons[0].SetActive(false);
				ResultScreen.instance.report.notButton[0].self.SetActive(false);
				break;
			}
		}

		// Token: 0x04004DE5 RID: 19941
		public int index = -1;

		// Token: 0x04004DE6 RID: 19942
		public GameObject button;

		// Token: 0x04004DE7 RID: 19943
		public GameObject bg;

		// Token: 0x04004DE8 RID: 19944
		public GameObject icon;

		// Token: 0x04004DE9 RID: 19945
		public Color yellow;

		// Token: 0x04004DEA RID: 19946
		public Color Green;

		// Token: 0x04004DEB RID: 19947
		public AudioClipPlayer ScoreSound;
	}

	// Token: 0x02000AF5 RID: 2805
	[Serializable]
	public class NotButton
	{
		// Token: 0x06005473 RID: 21619 RVA: 0x00004230 File Offset: 0x00002430
		public NotButton()
		{
		}

		// Token: 0x04004DEC RID: 19948
		public GameObject self;

		// Token: 0x04004DED RID: 19949
		public GameObject[] icons;

		// Token: 0x04004DEE RID: 19950
		public Text txt;
	}

	// Token: 0x02000AF6 RID: 2806
	[Serializable]
	public class Report
	{
		// Token: 0x06005474 RID: 21620 RVA: 0x00004230 File Offset: 0x00002430
		public Report()
		{
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x001E4680 File Offset: 0x001E2880
		public void OnManagementEnd()
		{
			CameraMover.instance.StopMove();
			Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
			this.results = new List<Result>(GlobalHistory.instance.GetResults());
			this.selectedOrdsAndEmers = new List<History>(GlobalHistory.instance.GetOrdsAndEmers());
			int num = 0;
			int num2 = 0;
			int num3 = PlayerModel.instance.GetDay() + 1;
			int moneyReward = GameManager.currentGameManager.GetMoneyReward();
			this.promoted.Make();
			this.missionBoard.Make(MissionManager.instance.GetReadyToClearMissions());
			this.day.text = num3.ToString("D2");
			for (int i = 0; i < this.results.Count<Result>(); i++)
			{
				int num4 = 0;
				if (this.results[i].workerDie > 0)
				{
					Text text = this.histories[i].datas[num4];
					text.color = this.textRed;
					text.text = string.Format("{0} {1}{2}", LocalizeTextDataModel.instance.GetTextAppend(new string[]
					{
						"ResultScreen",
						"DieHead"
					}), this.results[i].workerDie, LocalizeTextDataModel.instance.GetTextAppendFailEmpty(new string[]
					{
						"ResultScreen",
						"ManCount"
					}));
					num4++;
				}
				if (this.results[i].workerPanic > 0)
				{
					Text text2 = this.histories[i].datas[num4];
					text2.color = this.textBlue;
					text2.text = string.Format("{0} {1}{2}", LocalizeTextDataModel.instance.GetTextAppend(new string[]
					{
						"ResultScreen",
						"PanicHead"
					}), this.results[i].workerPanic, LocalizeTextDataModel.instance.GetTextAppendFailEmpty(new string[]
					{
						"ResultScreen",
						"ManCount"
					}));
					num4++;
				}
			}
			for (int j = 0; j < openedAreaList.Count<Sefira>(); j++)
			{
				Sefira sefira = openedAreaList[j];
				foreach (AgentModel agentModel in sefira.agentList)
				{
					if (!agentModel.IsDead() && !agentModel.IsPanic())
					{
						num2++;
					}
				}
			}
			if (this.selectedOrdsAndEmers.Count > 0)
			{
				for (int k = 0; k < 9; k++)
				{
					if (this.selectedOrdsAndEmers[k] != null)
					{
						if (this.selectedOrdsAndEmers[k].GetHistoryType() == History.HistoryType.ORDEAL_ENABLE)
						{
							this.ordsAndEmers[k].SetActive(true);
							this.ordsAndEmers[k].GetComponentInChildren<Text>().text = this.selectedOrdsAndEmers[k].GetEvent().metaInfo.GetName();
							Vector2 anchoredPosition = this.ordsAndEmers[k].GetComponent<RectTransform>().anchoredPosition;
							anchoredPosition.y = (float)(240 - 60 * k);
							this.ordsAndEmers[k].GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y);
						}
						else if (this.selectedOrdsAndEmers[k].GetHistoryType() == History.HistoryType.EMERGENCY)
						{
							int num5 = k + 9;
							this.ordsAndEmers[num5].SetActive(true);
							switch (this.selectedOrdsAndEmers[k].GetEmergency())
							{
							case EmergencyLevel.NORMAL:
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().text = "Trumpet End";
								for (int l = num5 - 1; l >= 9; l--)
								{
									if (this.ordsAndEmers[l].activeInHierarchy)
									{
										Color color = this.ordsAndEmers[l].GetComponentInChildren<Text>().color;
										this.ordsAndEmers[num5].GetComponentInChildren<Text>().color = color;
										this.ordsAndEmers[num5].GetComponentInChildren<Image>().color = color;
										break;
									}
								}
								break;
							case EmergencyLevel.LEVEL1:
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().text = "First Trumpet";
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().color = this.emergencyColor[0];
								this.ordsAndEmers[num5].GetComponentInChildren<Image>().color = this.emergencyColor[0];
								break;
							case EmergencyLevel.LEVEL2:
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().text = "Second Trumpet";
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().color = this.emergencyColor[1];
								this.ordsAndEmers[num5].GetComponentInChildren<Image>().color = this.emergencyColor[1];
								break;
							case EmergencyLevel.LEVEL3:
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().text = "Third Trumpet";
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().color = this.emergencyColor[2];
								this.ordsAndEmers[num5].GetComponentInChildren<Image>().color = this.emergencyColor[2];
								break;
							case EmergencyLevel.CHAOS:
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().text = "Fourth Trumpet";
								this.ordsAndEmers[num5].GetComponentInChildren<Text>().color = this.emergencyColor[3];
								this.ordsAndEmers[num5].GetComponentInChildren<Image>().color = this.emergencyColor[3];
								break;
							}
							Vector2 anchoredPosition2 = this.ordsAndEmers[num5].GetComponent<RectTransform>().anchoredPosition;
							anchoredPosition2.y = (float)(240 - 60 * k);
							this.ordsAndEmers[num5].GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPosition2.x, anchoredPosition2.y);
						}
					}
				}
			}
			this.time[0].text = "00:00";
			for (int m = 0; m < this.results.Count; m++)
			{
				this.time[m + 1].text = ((int)(this.results[m].time / 60f)).ToString("D2") + ":" + ((int)(this.results[m].time % 60f)).ToString("D2");
				num += this.results[m].workerDie;
			}
			num = AgentManager.instance.GetAgentList().Count - num2;
			float survivalRate = AgentManager.instance.GetSurvivalRate();
			this.time[5].text = survivalRate.ToString("P0");
			Color color2 = this.SurvivalRateColor.Evaluate(1f - survivalRate);
			this.time[5].color = color2;
			this.workerPromoted.text = this.promoted.promoted.ToString("D2");
			this.workerDead[0].text = num.ToString("D2");
			this.workerAlive[0].text = num2.ToString("D2");
			this.tortalLob.text = moneyReward.ToString();
			this.gatheredEnergy.text = ((int)EnergyModel.instance.GetEnergy()).ToString("D3");
			this.stageRewardInfo = StageRewardTypeList.instance.GetData(PlayerModel.instance.GetDay() + 1);
			this.Rank.text = GameManager.currentGameManager.GetStageRank(survivalRate).ToString();
			this.Reward.text = moneyReward.ToString();
			this.workerDead[1].text = string.Format("{0} {1}{2}", LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"ResultScreen",
				"Dead"
			}), num.ToString("D2"), LocalizeTextDataModel.instance.GetTextAppendFailEmpty(new string[]
			{
				"ResultScreen",
				"ManCount"
			}));
			this.workerAlive[1].text = string.Format("{0} {1}{2}", LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"ResultScreen",
				"Alive"
			}), num2.ToString("D2"), LocalizeTextDataModel.instance.GetTextAppendFailEmpty(new string[]
			{
				"ResultScreen",
				"ManCount"
			}));
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x001E4F90 File Offset: 0x001E3190
		public ResultScreen.ReportButton GetButton(int id)
		{
			ResultScreen.ReportButton result = null;
			foreach (ResultScreen.ReportButton reportButton in this.buttons)
			{
				if (id.Equals(reportButton.index))
				{
					result = reportButton;
					break;
				}
			}
			return result;
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x000448C9 File Offset: 0x00042AC9
		public void Update()
		{
			this.promoted.Update();
			this.missionBoard.Update();
		}

		// Token: 0x04004DEF RID: 19951
		public Color textRed;

		// Token: 0x04004DF0 RID: 19952
		public Color textBlue;

		// Token: 0x04004DF1 RID: 19953
		public Color textYellow;

		// Token: 0x04004DF2 RID: 19954
		public Text day;

		// Token: 0x04004DF3 RID: 19955
		public Text gatheredEnergy;

		// Token: 0x04004DF4 RID: 19956
		public Text tortalLob;

		// Token: 0x04004DF5 RID: 19957
		public Text workerPromoted;

		// Token: 0x04004DF6 RID: 19958
		public Text Rank;

		// Token: 0x04004DF7 RID: 19959
		public Text Reward;

		// Token: 0x04004DF8 RID: 19960
		public Text[] workerAlive;

		// Token: 0x04004DF9 RID: 19961
		public Text[] workerDead;

		// Token: 0x04004DFA RID: 19962
		public Text[] time;

		// Token: 0x04004DFB RID: 19963
		public List<Result> results;

		// Token: 0x04004DFC RID: 19964
		public List<History> selectedOrdsAndEmers;

		// Token: 0x04004DFD RID: 19965
		public GameObject[] ordsAndEmers;

		// Token: 0x04004DFE RID: 19966
		public GameObject nextDayClicked;

		// Token: 0x04004DFF RID: 19967
		public GameObject returnClicked;

		// Token: 0x04004E00 RID: 19968
		public GameObject[] defaultButtons;

		// Token: 0x04004E01 RID: 19969
		public GameObject rootGameObject;

		// Token: 0x04004E02 RID: 19970
		public ResultScreen.ReportButton[] buttons;

		// Token: 0x04004E03 RID: 19971
		public ResultScreen.NotButton[] notButton;

		// Token: 0x04004E04 RID: 19972
		public ResultScreen.HistoryData[] histories;

		// Token: 0x04004E05 RID: 19973
		public ResultScreen.Report.PromotedAgents promoted;

		// Token: 0x04004E06 RID: 19974
		public ResultScreen.Report.ClearedMissionBoard missionBoard;

		// Token: 0x04004E07 RID: 19975
		public Color[] emergencyColor;

		// Token: 0x04004E08 RID: 19976
		public Gradient SurvivalRateColor;

		// Token: 0x04004E09 RID: 19977
		private StageRewardTypeInfo stageRewardInfo;

		// Token: 0x02000AF7 RID: 2807
		[Serializable]
		public class PromotedAgents
		{
			// Token: 0x06005478 RID: 21624 RVA: 0x000448E1 File Offset: 0x00042AE1
			public PromotedAgents()
			{
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06005479 RID: 21625 RVA: 0x000448F4 File Offset: 0x00042AF4
			private RectTransform scrollRect
			{
				get
				{
					return this.scroll.gameObject.GetComponent<RectTransform>();
				}
			}

			// Token: 0x0600547A RID: 21626 RVA: 0x001E4FD8 File Offset: 0x001E31D8
			public void Make()
			{ // <Mod> Added old level to the notice send
				foreach (ResultScreenAgentSlot resultScreenAgentSlot in this.slots)
				{
					UnityEngine.Object.Destroy(resultScreenAgentSlot.gameObject);
				}
				this.slots.Clear();
				this.promoted = 0;
				foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
				{
					if (!agentModel.IsDead() && !agentModel.IsPanic())
					{
						int level = agentModel.level;
						int[] oldStatLevels = new int[]
						{
							agentModel.fortitudeLevel,
							agentModel.prudenceLevel,
							agentModel.temperanceLevel,
							agentModel.justiceLevel
						};
						WorkerPrimaryStatBonus workerPrimaryStatBonus = agentModel.UpdatePrimaryStat();
						if (workerPrimaryStatBonus.hp != agentModel.primaryStat.hp || workerPrimaryStatBonus.mental != agentModel.primaryStat.mental || workerPrimaryStatBonus.battle != agentModel.primaryStat.battle || workerPrimaryStatBonus.work != agentModel.primaryStat.work)
						{
							if (level < agentModel.level)
							{
								this.promoted++;
								Notice.instance.Send(NoticeName.OnAgentPromote, new object[]
								{
									agentModel,
                                    level,
                                    oldStatLevels
								});
							}
							GameObject gameObject = Prefab.LoadPrefab("UIComponent/ResultScreenAgentSlot");
							CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
							canvasGroup.interactable = false;
							canvasGroup.blocksRaycasts = false;
							ResultScreenAgentSlot component = gameObject.GetComponent<ResultScreenAgentSlot>();
							component.Init(agentModel, workerPrimaryStatBonus, oldStatLevels, level);
							this.slots.Add(component);
						}
					}
				}
				this.SetList();
			}

			// Token: 0x0600547B RID: 21627 RVA: 0x001E51E0 File Offset: 0x001E33E0
			public void SetList()
			{
				float x = this.defaultX;
				float num = this.defaultY;
				int count = this.slots.Count;
				float num2 = 0f;
				for (int i = 0; i < count; i++)
				{
					ResultScreenAgentSlot resultScreenAgentSlot = this.slots[i];
					resultScreenAgentSlot.rect.SetParent(this.listParent);
					resultScreenAgentSlot.rect.localRotation = Quaternion.identity;
					resultScreenAgentSlot.rect.localPosition = new Vector3(x, num, 0f);
					resultScreenAgentSlot.rect.localScale = Vector3.one;
					num2 = this.Spacing.y + resultScreenAgentSlot.rect.rect.height * resultScreenAgentSlot.rect.localScale.y;
					num -= num2;
				}
				if (count >= 3)
				{
					num -= num2;
				}
				this.listParent.sizeDelta = new Vector2(this.listParent.sizeDelta.x, Math.Abs(num));
			}

			// Token: 0x0600547C RID: 21628 RVA: 0x001E52F0 File Offset: 0x001E34F0
			public void Update()
			{
				float y = this.listParent.anchoredPosition.y;
				float num = this.listParent.sizeDelta.y - this.scrollRect.sizeDelta.y;
				if (y <= 4f)
				{
					if (this.Arrow_Up.activeInHierarchy)
					{
						this.Arrow_Up.gameObject.SetActive(false);
					}
				}
				else if (!this.Arrow_Up.activeInHierarchy)
				{
					this.Arrow_Up.gameObject.SetActive(true);
				}
				if (y >= num - 4f)
				{
					if (this.Arrow_Down.activeInHierarchy)
					{
						this.Arrow_Down.gameObject.SetActive(false);
					}
				}
				else if (!this.Arrow_Down.activeInHierarchy)
				{
					this.Arrow_Down.gameObject.SetActive(true);
				}
			}

			// Token: 0x04004E0A RID: 19978
			public List<ResultScreenAgentSlot> slots = new List<ResultScreenAgentSlot>();

			// Token: 0x04004E0B RID: 19979
			public RectTransform listParent;

			// Token: 0x04004E0C RID: 19980
			public ScrollRect scroll;

			// Token: 0x04004E0D RID: 19981
			public GameObject Arrow_Up;

			// Token: 0x04004E0E RID: 19982
			public GameObject Arrow_Down;

			// Token: 0x04004E0F RID: 19983
			public float defaultX;

			// Token: 0x04004E10 RID: 19984
			public float defaultY;

			// Token: 0x04004E11 RID: 19985
			public Vector2 Spacing;

			// Token: 0x04004E12 RID: 19986
			private const float scrollSpace = 4f;

			// Token: 0x04004E13 RID: 19987
			public int promoted;
		}

		// Token: 0x02000AF8 RID: 2808
		[Serializable]
		public class ClearedMissionBoard
		{
			// Token: 0x0600547D RID: 21629 RVA: 0x00044906 File Offset: 0x00042B06
			public ClearedMissionBoard()
			{
			}

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x0600547E RID: 21630 RVA: 0x00044919 File Offset: 0x00042B19
			private RectTransform scrollRect
			{
				get
				{
					return this.scroll.gameObject.GetComponent<RectTransform>();
				}
			}

			// Token: 0x0600547F RID: 21631 RVA: 0x001E53E0 File Offset: 0x001E35E0
			public void Make(List<Mission> cleared)
			{
				foreach (MissionSlot missionSlot in this.missions)
				{
					UnityEngine.Object.Destroy(missionSlot.gameObject);
				}
				this.missions.Clear();
				for (int i = 0; i < cleared.Count; i++)
				{
					Mission mission = cleared[i];
					GameObject gameObject = Prefab.LoadPrefab("UIComponent/MissionSlot");
					CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
					canvasGroup.interactable = false;
					canvasGroup.blocksRaycasts = false;
					MissionSlot component = gameObject.GetComponent<MissionSlot>();
					component.Init(mission);
					this.missions.Add(component);
				}
				this.SetList();
			}

			// Token: 0x06005480 RID: 21632 RVA: 0x001E54B4 File Offset: 0x001E36B4
			public void SetList()
			{
				float x = this.defaultX;
				float num = this.defaultY;
				float num2 = 0f;
				for (int i = 0; i < this.missions.Count; i++)
				{
					MissionSlot missionSlot = this.missions[i];
					missionSlot.rect.SetParent(this.listParent);
					missionSlot.rect.localRotation = Quaternion.identity;
					missionSlot.rect.localPosition = new Vector3(x, num, 0f);
					missionSlot.rect.localScale = Vector3.one;
					num2 = this.Spacing.y + missionSlot.rect.rect.height * missionSlot.rect.localScale.y;
					num -= num2;
				}
				this.listParent.sizeDelta = new Vector2(this.listParent.sizeDelta.x, num2);
			}

			// Token: 0x06005481 RID: 21633 RVA: 0x001E55AC File Offset: 0x001E37AC
			public void Update()
			{
				float y = this.listParent.anchoredPosition.y;
				float num = this.listParent.sizeDelta.y - this.scrollRect.sizeDelta.y;
				if (y <= 2f)
				{
					if (this.Arrow_Up.activeInHierarchy)
					{
						this.Arrow_Up.gameObject.SetActive(false);
					}
				}
				else if (!this.Arrow_Up.activeInHierarchy)
				{
					this.Arrow_Up.gameObject.SetActive(true);
				}
				if (y >= num - 2f)
				{
					if (this.Arrow_Down.activeInHierarchy)
					{
						this.Arrow_Down.gameObject.SetActive(false);
					}
				}
				else if (!this.Arrow_Down.activeInHierarchy)
				{
					this.Arrow_Down.gameObject.SetActive(true);
				}
			}

			// Token: 0x04004E14 RID: 19988
			public RectTransform listParent;

			// Token: 0x04004E15 RID: 19989
			public ScrollRect scroll;

			// Token: 0x04004E16 RID: 19990
			public GameObject Arrow_Up;

			// Token: 0x04004E17 RID: 19991
			public GameObject Arrow_Down;

			// Token: 0x04004E18 RID: 19992
			private List<MissionSlot> missions = new List<MissionSlot>();

			// Token: 0x04004E19 RID: 19993
			public float defaultX;

			// Token: 0x04004E1A RID: 19994
			public float defaultY;

			// Token: 0x04004E1B RID: 19995
			public Vector2 Spacing;

			// Token: 0x04004E1C RID: 19996
			private const float scrollSpace = 2f;
		}
	}
}
