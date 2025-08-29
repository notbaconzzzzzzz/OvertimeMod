/*
private void TryPlayNextSubStory() // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000A7F RID: 2687
public class StorySceneController : MonoBehaviour
{
	// Token: 0x0600513E RID: 20798 RVA: 0x00042B29 File Offset: 0x00040D29
	public StorySceneController()
	{
	}

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x0600513F RID: 20799 RVA: 0x00042B5C File Offset: 0x00040D5C
	public static StorySceneController instance
	{
		get
		{
			return StorySceneController._instance;
		}
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x00042B63 File Offset: 0x00040D63
	private void Awake()
	{
		StorySceneController._instance = this;
		this._curState = StorySceneController.StorySceneState.MAIN_STORY;
		this.seedUI.gameObject.SetActive(false);
		this.bossAppearUI.Hide();
	}

	// Token: 0x06005141 RID: 20801 RVA: 0x001E0B30 File Offset: 0x001DED30
	private void Start()
	{
		this._isOpenedArea = false;
		if (!GlobalGameManager.instance.IsPlaying())
		{
			GlobalGameManager.instance.InitStoryMode();
		}
		this._sefiraOrderQueue = new Queue<SefiraEnum>(this._sefiraOrder);
		foreach (SefiraEnum item in this._sefiraOrder)
		{
			this._sefiraOrderQueue.Enqueue(item);
		}
		this.missionShader.SetActive(false);
	}

	// Token: 0x06005142 RID: 20802 RVA: 0x00042B8E File Offset: 0x00040D8E
	private void LoadStoryWithFade(string storyId)
	{
		this.fadeUI.StartFade(new StoryFadeUI.StoryFadeEnd(this.LoadStory), storyId);
	}

	// Token: 0x06005143 RID: 20803 RVA: 0x00042BA8 File Offset: 0x00040DA8
	private void LoadStory(string storyId)
	{
		this.storyUI.LoadStory(new string[]
		{
			storyId
		});
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x001E0BA8 File Offset: 0x001DEDA8
	private void SetStoryKether(int day)
	{
		MissionManager.beHonest = true;
		if (PlayerModel.instance.ketherGameOver)
		{
			if (day == 47)
			{
				this.LoadStory("47_end");
			}
			else if (day == 48)
			{
				this.LoadStory("48_end");
			}
			else if (day == 49)
			{
				this.LoadStory("49_end");
			}
			else
			{
				this.OnEndStory();
			}
		}
		else if (day == 47)
		{
			bool flag = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.MALKUT);
			bool flag2 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.YESOD);
			bool flag3 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.HOD);
			bool flag4 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.NETZACH);
			if (!flag)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("47_noM");
			}
			else if (!flag2)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("47_noY");
			}
			else if (!flag3)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("47_noH");
			}
			else if (!flag4)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("47_noN");
			}
			else
			{
				this.LoadStory("47_suc");
			}
		}
		else if (day == 48)
		{
			bool flag5 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1);
			bool flag6 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.GEBURAH);
			bool flag7 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED);
			if (!flag5)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("48_noT");
			}
			else if (!flag6)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("48_noG");
			}
			else if (!flag7)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("48_noC");
			}
			else
			{
				this.LoadStory("48_suc");
			}
		}
		else if (day == 49)
		{
			bool flag8 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.BINAH);
			bool flag9 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH);
			if (!flag8)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("49_noB");
			}
			else if (!flag9)
			{
				PlayerModel.instance.SetKetherGameOver();
				this.LoadStory("49_noH");
			}
			else
			{
				this.LoadStory("49_suc");
			}
		}
		else if (day == 50)
		{
			this._curState = StorySceneController.StorySceneState.SEFIRA_FINALE_STORY;
			this._finaleSefira = SefiraEnum.KETHER;
			this.OnEndStory();
		}
		MissionManager.beHonest = false;
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x000043E5 File Offset: 0x000025E5
	private void PrintQueue(Queue<SefiraEnum> q)
	{
	}

	// Token: 0x06005146 RID: 20806 RVA: 0x001E0E14 File Offset: 0x001DF014
	public void InitStory()
	{
		if (this.storyUI.initialized)
		{
			return;
		}
		this.storyUI.Init();
		this.storyUI.SetEndCallback(new StoryUI.StoryEndFunc(this.OnEndStory));
		int num = PlayerModel.instance.GetDay() + 1;
		if (GlobalGameManager.instance.gameMode == GameMode.HIDDEN)
		{
			this.LoadStory("51");
			return;
		}
		if (num >= 47)
		{
			this.SetStoryKether(num);
			return;
		}
		Mission mission = null;
		List<Mission> readyToClearMissions = MissionManager.instance.GetReadyToClearMissions();
		foreach (Mission mission2 in readyToClearMissions)
		{
			if (mission2.successCondition.condition_Type == ConditionType.DESTROY_CORE)
			{
				mission = mission2;
				break;
			}
		}
		if (mission != null)
		{
			this._curState = StorySceneController.StorySceneState.SEFIRA_FINALE_STORY;
			this._finaleSefira = mission.metaInfo.sefira;
			this.LoadStory(mission.metaInfo.clear);
			MissionManager.instance.ClearMission(mission);
		}
		else
		{
			this._curState = StorySceneController.StorySceneState.MAIN_STORY;
			this.LoadStory(num.ToString());
		}
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x00042BBF File Offset: 0x00040DBF
	private bool ExistsSefiraMemory()
	{
		return MissionManager.instance.GetCurrentSefiraMission(SefiraEnum.MALKUT) != null || MissionManager.instance.GetClearedOrClosedMissionNum(SefiraEnum.MALKUT) > 0;
	}

	// Token: 0x06005148 RID: 20808 RVA: 0x001E0F50 File Offset: 0x001DF150
	public void OnEndSeedUI(string angelaStory)
	{
		if (angelaStory != string.Empty)
		{
			this._curState = StorySceneController.StorySceneState.SEFIRA_FINALE_ANGELA;
			this.LoadStory(angelaStory);
		}
		else
		{
			this._curState = StorySceneController.StorySceneState.MAIN_STORY;
			this.LoadStory((PlayerModel.instance.GetDay() + 1).ToString());
		}
	}

	// Token: 0x06005149 RID: 20809 RVA: 0x001E0FA8 File Offset: 0x001DF1A8
	public void OnEndStory()
	{
		if (GlobalGameManager.instance.gameMode == GameMode.HIDDEN)
		{
			GlobalEtcDataModel.instance.hiddenEndingDone = true;
			GlobalGameManager.instance.SaveGlobalData();
			GlobalGameManager.instance.ReleaseGame();
			SceneManager.LoadScene("AlterTitleScene");
			return;
		}
		if (PlayerModel.instance.ketherGameOver)
		{
			GlobalGameManager.instance.ReleaseGame();
			GlobalGameManager.instance.loadingScene = "DefaultEndScene";
			GlobalGameManager.instance.loadingScreen.LoadTitleScene();
			return;
		}
		if (this._curState == StorySceneController.StorySceneState.SEFIRA_FINALE_STORY)
		{
			this.seedUI.gameObject.SetActive(true);
			int num = MissionManager.instance.GetClearedOrClosedBossMissionNum();
			int num2 = PlayerModel.instance.GetDay() + 1;
			if (num2 == 50)
			{
				num = 10;
			}
			int from = (num - 1) * 10;
			int num3 = num * 10;
			SefiraEnum sefiraEnum = this._finaleSefira;
			if (sefiraEnum == SefiraEnum.TIPERERTH2)
			{
				sefiraEnum = SefiraEnum.TIPERERTH1;
			}
			string text = LocalizeTextDataModel.instance.GetText(string.Format("boss_{0}_story_finish", sefiraEnum.ToString().ToLower()));
			string nextAngelaStroy = string.Empty;
			if (num3 != 10)
			{
				if (num3 != 30)
				{
					if (num3 != 50)
					{
						if (num3 != 70)
						{
							if (num3 == 90)
							{
								nextAngelaStroy = "boss_sixth";
							}
						}
						else
						{
							nextAngelaStroy = "boss_fifth";
						}
					}
					else
					{
						nextAngelaStroy = "boss_fourth";
					}
				}
				else
				{
					nextAngelaStroy = "boss_third";
				}
			}
			else
			{
				nextAngelaStroy = "boss_second";
			}
			this.seedUI.Show(text, from, num3, new StorySeedUI.StorySeedEnd(this.OnEndSeedUI), nextAngelaStroy);
		}
		else if (this._curState == StorySceneController.StorySceneState.SEFIRA_FINALE_ANGELA)
		{
			this._curState = StorySceneController.StorySceneState.MAIN_STORY;
			this.LoadStoryWithFade((PlayerModel.instance.GetDay() + 1).ToString());
		}
		else if (this._curState == StorySceneController.StorySceneState.MAIN_STORY)
		{
			int day = PlayerModel.instance.GetDay();
			if (!PlayerModel.instance.memoryInit && day == 0 && this.ExistsSefiraMemory())
			{
				PlayerModel.instance.Remember();
				this.LoadStory("remember");
				return;
			}
			PlayerModel.instance.Remember();
			this.TryPlayNextSubStory();
		}
		else if (this._curState == StorySceneController.StorySceneState.SUB_STORY)
		{
			if (this._newMission != null)
			{
				if (this._newMission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
				{
					this.bossAppearUI.Show(new CheckPointUI.EndEvnet(this.OnEndStory));
				}
				else
				{
					MissionPopupUI.instance.Init(this._newMission, new MissionPopupUI.MissionPopupCallbackFunc(this.OnEndStory));
					this.missionShader.SetActive(true);
				}
				this._newMission = null;
			}
			else if (this._firstBossMission)
			{
				this._firstBossMission = false;
				this.LoadStoryWithFade("boss_first");
			}
			else
			{
				this.TryPlayNextSubStory();
			}
		}
		else if (this._curState == StorySceneController.StorySceneState.EVENT_STORY)
		{
		}
	}

	// Token: 0x0600514A RID: 20810 RVA: 0x001E1294 File Offset: 0x001DF494
	private void TryPlayNextSubStory()
	{ // <Mod>
		bool flag = false;
		while (this._sefiraOrderQueue.Count != 0)
		{
			SefiraEnum sefiraEnum = this._sefiraOrderQueue.Dequeue();
			bool flag2 = false;
			if (PlayerModel.instance.GetDay() >= 40)
			{
				if (sefiraEnum == SefiraEnum.BINAH || sefiraEnum == SefiraEnum.CHOKHMAH)
				{
					flag2 = true;
				}
			}
			if (flag2 || sefiraEnum == SefiraEnum.MALKUT || SefiraManager.instance.IsOpened(sefiraEnum))
			{
				Debug.Log("Next : " + sefiraEnum);
				this.PrintQueue(this._sefiraOrderQueue);
				Mission mission = MissionManager.instance.GetReadyToClearMission(sefiraEnum);
				if (mission != null && mission.successCondition.condition_Type == ConditionType.DESTROY_CORE)
				{
					mission = null;
				}
				if (mission != null)
				{
					MissionManager.instance.ClearMission(mission);
				}
				if (mission != null && mission.metaInfo.clear != string.Empty)
				{
					this._curState = StorySceneController.StorySceneState.SUB_STORY;
					this.LoadStoryWithFade(mission.metaInfo.clear);
					flag = true;
					break;
				}
				Mission availableMission = MissionManager.instance.GetAvailableMission(sefiraEnum);
				if (availableMission != null)
				{
					this._curState = StorySceneController.StorySceneState.SUB_STORY;
					this.LoadStoryWithFade(availableMission.metaInfo.intro);
					if (availableMission.successCondition.condition_Type == ConditionType.DESTROY_CORE && !MissionManager.instance.ExistsBossMission() && !MissionManager.instance.ExistsFinishedBossMission())
					{
						this._firstBossMission = true;
					}
					MissionManager.instance.StartMission(availableMission.metaInfo.id);
					this._newMission = availableMission;
					flag = true;
					break;
				}
			}
		}
		this.missionShader.SetActive(false);
		if (!flag)
		{
			this.storyUI.Clear();
			ExpandUI.instance.Init(new ExpandUI.OnOpenEvent(this.LoadMainScene));
		}
	}

	// Token: 0x0600514B RID: 20811 RVA: 0x000043E5 File Offset: 0x000025E5
	private void LoadMainScene()
	{
	}

	// Token: 0x04004B2A RID: 19242
	private static StorySceneController _instance;

	// Token: 0x04004B2B RID: 19243
	private StorySceneController.StorySceneState _curState;

	// Token: 0x04004B2C RID: 19244
	private SefiraEnum _finaleSefira = SefiraEnum.DUMMY;

	// Token: 0x04004B2D RID: 19245
	public StoryUI storyUI;

	// Token: 0x04004B2E RID: 19246
	public StoryChallengeModeUI challangeModeUI;

	// Token: 0x04004B2F RID: 19247
	public StorySeedUI seedUI;

	// Token: 0x04004B30 RID: 19248
	public BossMissionAppearUI bossAppearUI;

	// Token: 0x04004B31 RID: 19249
	public StoryFadeUI fadeUI;

	// Token: 0x04004B32 RID: 19250
	private Queue<StorySceneController.SubStorySceneData> _missionSceneQueue;

	// Token: 0x04004B33 RID: 19251
	private List<Mission> _nextMissions;

	// Token: 0x04004B34 RID: 19252
	private List<Mission> _clearedMissions;

	// Token: 0x04004B35 RID: 19253
	private SefiraEnum[] _sefiraOrder = new SefiraEnum[]
	{
		SefiraEnum.MALKUT,
		SefiraEnum.YESOD,
		SefiraEnum.NETZACH,
		SefiraEnum.HOD,
		SefiraEnum.TIPERERTH1,
		SefiraEnum.GEBURAH,
		SefiraEnum.CHESED,
		SefiraEnum.CHOKHMAH,
		SefiraEnum.BINAH,
		SefiraEnum.DAAT
	};

	// Token: 0x04004B36 RID: 19254
	private Queue<SefiraEnum> _sefiraOrderQueue = new Queue<SefiraEnum>();

	// Token: 0x04004B37 RID: 19255
	private bool _isOpenedArea;

	// Token: 0x04004B38 RID: 19256
	private Mission _newMission;

	// Token: 0x04004B39 RID: 19257
	private bool _firstBossMission;

	// Token: 0x04004B3A RID: 19258
	public GameObject missionShader;

	// Token: 0x02000A80 RID: 2688
	private class SubStorySceneData
	{
		// Token: 0x0600514C RID: 20812 RVA: 0x000043B8 File Offset: 0x000025B8
		public SubStorySceneData()
		{
		}

		// Token: 0x04004B3B RID: 19259
		public Mission clearMission;

		// Token: 0x04004B3C RID: 19260
		public Mission introMission;
	}

	// Token: 0x02000A81 RID: 2689
	public enum StorySceneState
	{
		// Token: 0x04004B3E RID: 19262
		SEFIRA_FINALE_STORY,
		// Token: 0x04004B3F RID: 19263
		SEFIRA_FINALE_ANGELA,
		// Token: 0x04004B40 RID: 19264
		MAIN_STORY,
		// Token: 0x04004B41 RID: 19265
		SUB_STORY,
		// Token: 0x04004B42 RID: 19266
		EVENT_STORY,
		// Token: 0x04004B43 RID: 19267
		OPEN_AREA
	}
}
