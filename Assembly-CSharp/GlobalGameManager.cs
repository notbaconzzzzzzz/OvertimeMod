using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200059C RID: 1436
public class GlobalGameManager : MonoBehaviour, IObserver
{
	// Token: 0x06003112 RID: 12562 RVA: 0x0014D428 File Offset: 0x0014B628
	public GlobalGameManager()
	{
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06003113 RID: 12563 RVA: 0x0002D9D7 File Offset: 0x0002BBD7
	public static GlobalGameManager instance
	{
		get
		{
			return GlobalGameManager._instance;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06003115 RID: 12565 RVA: 0x0002D9E7 File Offset: 0x0002BBE7
	// (set) Token: 0x06003114 RID: 12564 RVA: 0x0002D9DE File Offset: 0x0002BBDE
	public SystemLanguage Language
	{
		get
		{
			return this._language;
		}
		set
		{
			this._language = value;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06003116 RID: 12566 RVA: 0x0002D9EF File Offset: 0x0002BBEF
	private string logSrc
	{
		get
		{
			return this.GetLogSrc();
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06003117 RID: 12567 RVA: 0x0002D9F7 File Offset: 0x0002BBF7
	private string stateSrc
	{
		get
		{
			return Application.persistentDataPath + "170808state.dat";
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06003118 RID: 12568 RVA: 0x00013D75 File Offset: 0x00011F75
	// (set) Token: 0x06003119 RID: 12569 RVA: 0x0002DA08 File Offset: 0x0002BC08
	public bool tutorialPlayed
	{
		get
		{
			return true;
		}
		set
		{
			this._tutorialPlayed = value;
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x0600311A RID: 12570 RVA: 0x0002DA11 File Offset: 0x0002BC11
	// (set) Token: 0x0600311B RID: 12571 RVA: 0x0002DA19 File Offset: 0x0002BC19
	public bool isPlayingTutorial
	{
		get
		{
			return this._isPlayingTutorial;
		}
		set
		{
			this._isPlayingTutorial = value;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x0600311C RID: 12572 RVA: 0x0002DA22 File Offset: 0x0002BC22
	public static GlobalGameManager.LanguageFont currentLanguageFont
	{
		get
		{
			return GlobalGameManager._currentLanguageFont;
		}
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x0014D4B4 File Offset: 0x0014B6B4
	private void Awake()
	{
		if (GlobalGameManager._instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GlobalGameManager._instance = this;
		this.LoadStateData();
		AudioListener.volume = this.sceneDataSaver.currentVolume;
		GameSettingModel.instance.InitResolution();
		string a = this.language;
		if (a == "vn")
		{
			this.Language = SystemLanguage.Vietnamese;
		}
		else if (a == "bg")
		{
			this.Language = SystemLanguage.Bulgarian;
		}
		else if (a == "ru")
		{
			this.Language = SystemLanguage.Russian;
		}
		else if (a == "es")
		{
			this.Language = SystemLanguage.Spanish;
		}
		else if (a == "jp")
		{
			this.Language = SystemLanguage.Japanese;
		}
		else if (a == "kr")
		{
			this.Language = SystemLanguage.Korean;
		}
		else if (a == "cn_tr")
		{
			this.Language = SystemLanguage.ChineseTraditional;
		}
		else if (a == "cn")
		{
			this.Language = SystemLanguage.Chinese;
		}
		else
		{
			this.Language = SystemLanguage.English;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.saveFileName = Application.persistentDataPath + "/saveData170808.dat";
		this.saveGlobalFileName = Application.persistentDataPath + "/saveGlobal170808.dat";
		this.saveUnlimitFileName = Application.persistentDataPath + "/saveUnlimitV5170808.dat";
		this.saveEtcFileName = Application.persistentDataPath + this.etcRemembered + "170808.dat";
		MapGraph.instance.LoadMap();
		GameStaticDataLoader.LoadStaticData();
		Notice.instance.Observe(NoticeName.AutoSaveTimer, this);
		AgentManager.instance.Init();
		this.SetLanguageFont();
		this.LoadGlobalData();
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x0014D668 File Offset: 0x0014B868
	public SystemLanguage GetLanguage(string str)
	{
		switch (str)
		{
			case "vn":
				return SystemLanguage.Vietnamese;
			case "bg":
				return SystemLanguage.Bulgarian;
			case "ru":
				return SystemLanguage.Russian;
			case "es":
				return SystemLanguage.Spanish;
			case "jp":
				return SystemLanguage.Japanese;
			case "kr":
				return SystemLanguage.Korean;
			case "cn_tr":
				return SystemLanguage.ChineseTraditional;
			case "cn":
				return SystemLanguage.Chinese;
			default:
				return SystemLanguage.English;
		}
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x0014D784 File Offset: 0x0014B984
	public string GetLanguage(SystemLanguage ln)
	{
		switch (ln)
		{
		case SystemLanguage.Vietnamese:
			return "vn";
		case SystemLanguage.ChineseSimplified:
			break;
		case SystemLanguage.ChineseTraditional:
			return "cn_tr";
		default:
			if (ln == SystemLanguage.Bulgarian)
			{
				return "bg";
			}
			if (ln != SystemLanguage.Chinese)
			{
				if (ln == SystemLanguage.Japanese)
				{
					return "jp";
				}
				if (ln != SystemLanguage.Korean)
				{
					if (ln != SystemLanguage.English)
					{
						if (ln == SystemLanguage.Russian)
						{
							return "ru";
						}
						if (ln == SystemLanguage.Spanish)
						{
							return "es";
						}
					}
					return "en";
				}
				return "kr";
			}
			break;
		}
		return "cn";
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x0014D800 File Offset: 0x0014BA00
	public void ChangeFont(string language, FontType type, string fontName)
	{
		GlobalGameManager.LanguageFont languageFont = this.GetLanguageFont(language);
		if (languageFont != null)
		{
			foreach (GlobalGameManager.LanguageFont languageFont2 in this.fontList)
			{
				if (languageFont2.language == language)
				{
					foreach (GlobalGameManager.FontData fontData in languageFont2.list)
					{
						if (fontData.type == type)
						{
							fontData.font = Font.CreateDynamicFontFromOSFont(fontName, 16);
						}
					}
				}
			}
		}
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x0014D8D4 File Offset: 0x0014BAD4
	public GlobalGameManager.LanguageFont GetLanguageFont(string language)
	{
		GlobalGameManager.LanguageFont result = null;
		if (language == "cn" || language == "jp" || language == "cn_tr" || language == "vn" || language == "bg")
		{
			language = "en";
		}
		foreach (GlobalGameManager.LanguageFont languageFont in this.fontList)
		{
			if (languageFont.language == language)
			{
				result = languageFont;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x0014D980 File Offset: 0x0014BB80
	public void SetLanguageFont()
	{
		GlobalGameManager.LanguageFont languageFont = this.GetLanguageFont(this.language);
		GlobalGameManager._currentLanguageFont = languageFont;
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x0002DA29 File Offset: 0x0002BC29
	private void OnEnable()
	{
		Application.logMessageReceived += this.MessageHandler;
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x0002DA3C File Offset: 0x0002BC3C
	private void OnDisable()
	{
		Application.logMessageReceived -= this.MessageHandler;
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x0002DA4F File Offset: 0x0002BC4F
	private void OnApplicationQuit()
	{
		this.SaveLogs();
		this.SaveStateData();
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x0014D9A0 File Offset: 0x0014BBA0
	private void SaveLogs()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Log/");
		CultureInfo provider = new CultureInfo("en-GB");
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		string text = string.Format("[{0}]\t[{3}]{1}Log Output:{1}{2}{1}", new object[]
		{
			this.BuildVer,
			Environment.NewLine,
			this.logOutput,
			DateTime.Now.ToString(provider)
		});
		text.Trim();
		File.WriteAllText(this.logSrc, text);
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0002DA5D File Offset: 0x0002BC5D
	private void MessageHandler(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			this.logOutput += string.Format("Log : {0}{2}{2}StackTrace : {1}{2}{2}", logString, stackTrace, Environment.NewLine);
		}
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x000040A1 File Offset: 0x000022A1
	private void Start()
	{
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000040A1 File Offset: 0x000022A1
	private void OnLevelWasLoaded()
	{
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x0014DA30 File Offset: 0x0014BC30
	private void Update()
	{
		this.ScreenWidth = Screen.width;
		if (this.isLoaded)
		{
			if (this.canvas.GetComponent<CanvasGroup>().alpha > 0f)
			{
				this.canvas.GetComponent<CanvasGroup>().alpha -= 0.04f;
			}
			else
			{
				this.isLoaded = false;
				this.canvas.GetComponent<CanvasGroup>().alpha = 1f;
				this.loadingScreen.currentLoadingScene.gameObject.SetActive(false);
				this.loadingScreen.gameObject.SetActive(false);
				Notice.instance.Send(NoticeName.OnLoadingEnd, new object[0]);
			}
		}
		if (SceneManager.GetActiveScene().name == "StartScene")
		{
			Debug.Log("load....");
			if (GlobalEtcDataModel.instance.trueEndingDone)
			{
				SceneManager.LoadScene("AlterTitleScene");
			}
			else
			{
				SceneManager.LoadScene("NewTitleScene");
			}
		}
		if (this.calcTime)
		{
			this.playTime += Time.deltaTime;
		}
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x0002DA8E File Offset: 0x0002BC8E
	private void LateUpdate()
	{
		SpecialUnitClickManager.instance.OnLateUpdate();
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x0002DA9A File Offset: 0x0002BC9A
	public bool IsPlaying()
	{
		return this.bPlayingGame;
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x0014DB40 File Offset: 0x0014BD40
	public void InitStoryMode()
	{
		MoneyModel.instance.Init();
		SefiraManager.instance.ClearUnitData();
		OfficerManager.instance.Clear();
		AgentManager.instance.Clear();
		AgentManager.instance.LoadDelAgentData();
		CreatureManager.instance.Clear();
		PlayerModel.instance.Init();
		this.gameMode = GameMode.STORY_MODE;
		StageRewardTypeInfo data = StageRewardTypeList.instance.GetData(0);
		if (data != null)
		{
			foreach (StageRewardTypeInfo.AgentRewardInfo agentRewardInfo in data.agentList)
			{
				AgentModel agentModel = AgentManager.instance.AddSpareAgentModel();
				agentModel.SetCurrentSefira(agentRewardInfo.sephira);
				agentModel.GetMovableNode().SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(agentRewardInfo.sephira));
			}
			MoneyModel.instance.Add(data.money);
		}
		this.bPlayingGame = true;
		this.calcTime = true;
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x0014DC38 File Offset: 0x0014BE38
	public void InitTutorial(int step)
	{
		this.ReleaseGame();
		CreatureManager.instance.ResetObserveData();
		GlobalEtcDataModel.instance.ResetGlobalData();
		ResearchDataModel.instance.Init();
		InventoryModel.Instance.Init();
		MissionManager.instance.Init();
		SefiraCharacterManager.instance.Init();
		MoneyModel.instance.Init();
		SefiraManager.instance.ClearUnitData();
		OfficerManager.instance.Clear();
		AgentManager.instance.Clear();
		AgentManager.instance.LoadDelAgentData();
		CreatureManager.instance.Clear();
		PlayerModel.instance.Init();
		PlayerModel.instance.SetDay(0);
		this.gameMode = GameMode.TUTORIAL;
		SefiraManager.instance.MakeTutorialCreature();
		this.bPlayingGame = true;
		if (!this.calcTime)
		{
			this.calcTime = true;
		}
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x0002DAA2 File Offset: 0x0002BCA2
	public void InitHidden()
	{
		this.gameMode = GameMode.HIDDEN;
		this.bPlayingGame = true;
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x0014DD04 File Offset: 0x0014BF04
	public void ReleaseGame()
	{
		InventoryModel.Instance.OnReleaseGame();
		MissionManager.instance.ReleaseGame();
		MoneyModel.instance.Init();
		SefiraManager.instance.ClearUnitData();
		OfficerManager.instance.Clear();
		AgentManager.instance.Clear();
		CreatureManager.instance.Clear();
		PlayerModel.instance.Init();
		SefiraManager.instance.Clear();
		MapGraph.instance.Reset();
		AgentNameList.instance.OnInit();
		new GameStaticDataLoader().LoadSefiraIsolateData();
		this.gameMode = GameMode.NONE;
		this.bPlayingGame = false;
		this.calcTime = false;
		this.lastLoaded = false;
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x0014DDA8 File Offset: 0x0014BFA8
	public void StoryReturnTitle()
	{
		this.gameMode = GameMode.NONE;
		this.bPlayingGame = false;
		this.calcTime = false;
		this.lastLoaded = false;
		this.ReleaseGame();
		this.LoadGlobalData();
		GlobalGameManager.instance.loadingScene = "DefaultEndScene";
		if (GlobalEtcDataModel.instance.trueEndingDone)
		{
			GlobalGameManager.instance.loadingScreen.LoadScene("AlterTitleScene");
			return;
		}
		GlobalGameManager.instance.loadingScreen.LoadScene("NewTitleScene");
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x000040A1 File Offset: 0x000022A1
	public void SaveDataWithCheckPoint()
	{
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x0014DE24 File Offset: 0x0014C024
	public Dictionary<string, object> GetSaveDayData()
	{
		return new Dictionary<string, object>
		{
			{
				"saveInnerVer",
				"ver1"
			},
			{
				"day",
				PlayerModel.instance.GetDay()
			},
			{
				"money",
				MoneyModel.instance.GetSaveData()
			},
			{
				"agents",
				AgentManager.instance.GetSaveData()
			},
			{
				"creatures",
				CreatureManager.instance.GetSaveData()
			},
			{
				"playerData",
				PlayerModel.instance.GetSaveData()
			},
			{
				"sefiras",
				SefiraManager.instance.GetSaveData()
			},
			{
				"saveState",
				this.saveState
			},
			{
				"agentName",
				AgentNameList.instance.GetSaveData()
			}
		};
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0014DEF4 File Offset: 0x0014C0F4
	public void SaveData(bool saveCheckPoint = false)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("saveVer", "ver1");
		dictionary.Add("playTime", this.playTime);
		int day = PlayerModel.instance.GetDay();
		dictionary.Add("lastDay", day);
		Dictionary<int, Dictionary<string, object>> dictionary2 = new Dictionary<int, Dictionary<string, object>>();
		Dictionary<string, object> saveDayData = this.GetSaveDayData();
		dictionary2.Add(PlayerModel.instance.GetDay(), saveDayData);
		if (saveCheckPoint)
		{
			Dictionary<string, object> value = saveDayData;
			dictionary.Add("checkPointDay", day + 10000);
			dictionary2.Add(day + 10000, value);
		}
		else
		{
			Dictionary<string, object> dic = this.LoadSaveFile();
			int num = 0;
			Dictionary<string, object> value2 = null;
			Dictionary<int, Dictionary<string, object>> dictionary3 = null;
			if (GameUtil.TryGetValue<int>(dic, "checkPointDay", ref num) && GameUtil.TryGetValue<Dictionary<int, Dictionary<string, object>>>(dic, "dayList", ref dictionary3) && dictionary3.TryGetValue(num, out value2))
			{
				dictionary.Add("checkPointDay", num);
				dictionary2.Add(num, value2);
			}
		}
		dictionary.Add("dayList", dictionary2);
		SaveUtil.WriteSerializableFile(this.saveFileName, dictionary);
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0014E018 File Offset: 0x0014C218
	private int GetDayFromSaveData(Dictionary<string, object> dayData)
	{
		int result = -1;
		string a = "old";
		GameUtil.TryGetValue<string>(dayData, "saveInnerVer", ref a);
		if (a != "old")
		{
			GameUtil.TryGetValue<int>(dayData, "day", ref result);
		}
		return result;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x0014E05C File Offset: 0x0014C25C
	public int PreLoadData()
	{
		if (!File.Exists(this.saveFileName))
		{
			return 0;
		}
		int result = 0;
		try
		{
			Dictionary<string, object> dic = SaveUtil.ReadSerializableFile(this.saveFileName);
			string a = "old";
			GameUtil.TryGetValue<string>(dic, "saveVer", ref a);
			if (a == "old")
			{
				GameUtil.TryGetValue<int>(dic, "lastDay", ref result);
			}
			else
			{
				GameUtil.TryGetValue<int>(dic, "lastDay", ref result);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return 0;
		}
		return result;
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x0014E0F8 File Offset: 0x0014C2F8
	public int LoadCheckPointDay()
	{
		if (!File.Exists(this.saveFileName))
		{
			return 0;
		}
		int result = 0;
		try
		{
			Dictionary<string, object> dic = SaveUtil.ReadSerializableFile(this.saveFileName);
			GameUtil.TryGetValue<int>(dic, "checkPointDay", ref result);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return 0;
		}
		return result;
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x0014E15C File Offset: 0x0014C35C
	private void LoadDay(Dictionary<string, object> data)
	{
		Dictionary<string, object> dic = null;
		Dictionary<string, object> dic2 = null;
		Dictionary<string, object> dic3 = null;
		Dictionary<string, object> dic4 = null;
		Dictionary<string, object> dic5 = null;
		Dictionary<string, object> dic6 = null;
		string text = "old";
		GameUtil.TryGetValue<string>(data, "saveInnerVer", ref text);
		int dayFromSaveData = this.GetDayFromSaveData(data);
		GameUtil.TryGetValue<Dictionary<string, object>>(data, "money", ref dic);
		GameUtil.TryGetValue<Dictionary<string, object>>(data, "agents", ref dic2);
		GameUtil.TryGetValue<Dictionary<string, object>>(data, "creatures", ref dic3);
		GameUtil.TryGetValue<Dictionary<string, object>>(data, "playerData", ref dic4);
		GameUtil.TryGetValue<Dictionary<string, object>>(data, "sefiras", ref dic5);
		GameUtil.TryGetValue<string>(data, "saveState", ref this.saveState);
		if (GameUtil.TryGetValue<Dictionary<string, object>>(data, "agentName", ref dic6))
		{
			AgentNameList.instance.LoadData(dic6);
		}
		MoneyModel.instance.LoadData(dic);
		PlayerModel.instance.LoadData(dic4);
		SefiraManager.instance.LoadData(dic5);
		AgentManager.instance.LoadCustomAgentData();
		CreatureManager.instance.LoadData(dic3);
		AgentManager.instance.LoadDelAgentData();
		AgentManager.instance.LoadData(dic2);
		this.gameMode = GameMode.STORY_MODE;
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x0002DAB2 File Offset: 0x0002BCB2
	private void LoadData_preprocess()
	{
		if (!File.Exists(this.saveFileName))
		{
			Debug.Log("save file don't exists");
			return;
		}
		this.ReleaseGame();
		this.bPlayingGame = true;
		this.calcTime = true;
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x0002DAE3 File Offset: 0x0002BCE3
	public Dictionary<string, object> LoadSaveFile()
	{
		return SaveUtil.ReadSerializableFile(this.saveFileName);
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x0014E264 File Offset: 0x0014C464
	public Dictionary<string, object> LoadEtcFile()
	{
		Dictionary<string, object> dictionary = SaveUtil.ReadSerializableFile(this.saveEtcFileName);
		try
		{
			List<long> list = new List<long>();
			if (GameUtil.TryGetValue<List<long>>(dictionary, "waitingCreature", ref list))
			{
				foreach (long id in list)
				{
					if (!PlayerModel.instance.IsWaitingCreature(id))
					{
						PlayerModel.instance.AddWaitingCreature(id);
					}
				}
			}
		}
		catch
		{
		}
		return dictionary;
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0014E310 File Offset: 0x0014C510
	public void LoadData(SaveType saveType)
	{
		this.LoadData_preprocess();
		Dictionary<string, object> dic = this.LoadSaveFile();
		string text = "old";
		GameUtil.TryGetValue<string>(dic, "saveVer", ref text);
		GameUtil.TryGetValue<float>(dic, "playTime", ref this.playTime);
		int key = 0;
		int key2 = 0;
		Dictionary<int, Dictionary<string, object>> dictionary = null;
		GameUtil.TryGetValue<Dictionary<int, Dictionary<string, object>>>(dic, "dayList", ref dictionary);
		GameUtil.TryGetValue<int>(dic, "checkPointDay", ref key2);
		GameUtil.TryGetValue<int>(dic, "lastDay", ref key);
		Dictionary<string, object> dictionary2 = null;
		if (!dictionary.TryGetValue(key, out dictionary2))
		{
			throw new Exception("lastDay not found (saveVer : " + text + ")");
		}
		Dictionary<string, object> data = dictionary2;
		if (text == "old")
		{
			this.LoadDay(dictionary2);
			this.SaveData(true);
		}
		else
		{
			Dictionary<string, object> data2 = null;
			bool flag = dictionary.TryGetValue(key2, out data2);
			if (saveType == SaveType.LASTDAY)
			{
				this.LoadDay(data);
				if (!GlobalGameManager.instance.dlcCreatureOn)
				{
					bool flag2 = CreatureManager.instance.ReplaceAllDlcCreature();
					flag2 = (InventoryModel.Instance.RemoveAllDlcEquipment() || flag2);
					flag2 = (AgentManager.instance.RemoveAllDlcEquipment() || flag2);
					if (flag2)
					{
						Debug.Log("exists removed DLC data");
						this.SaveData(false);
						this.SaveGlobalData();
					}
				}
			}
			else
			{
				if (saveType != SaveType.CHECK_POINT)
				{
					throw new Exception("invalid SaveType");
				}
				this.LoadDay(data2);
				if (GlobalGameManager.instance.dlcCreatureOn)
				{
					this.SaveData(false);
				}
				else
				{
					bool flag3 = CreatureManager.instance.ReplaceAllDlcCreature();
					flag3 = (InventoryModel.Instance.RemoveAllDlcEquipment() || flag3);
					flag3 = (AgentManager.instance.RemoveAllDlcEquipment() || flag3);
					if (flag3)
					{
						Debug.Log("exists removed DLC data");
						this.SaveData(true);
						this.SaveGlobalData();
					}
					else
					{
						this.SaveData(false);
					}
				}
			}
		}
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x0014E4F8 File Offset: 0x0014C6F8
	public void SaveUnlimitData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("saveVer", "final1");
		dictionary.Add("money", MoneyModel.instance.GetSaveData());
		dictionary.Add("agents", AgentManager.instance.GetSaveData());
		dictionary.Add("creatures", CreatureManager.instance.GetSaveData());
		SaveUtil.WriteSerializableFile(this.saveUnlimitFileName, dictionary);
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x0014E568 File Offset: 0x0014C768
	public void SaveEtcData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		try
		{
			SefiraBossManager.Instance.SaveBossSessionData(dictionary);
		}
		catch
		{
		}
		try
		{
			List<long> list = new List<long>();
			if (PlayerModel.instance.IsWaitingCreatureExist())
			{
				foreach (long item in PlayerModel.instance.addedCreature)
				{
					list.Add(item);
				}
			}
			dictionary.Add("waitingCreature", list);
		}
		catch
		{
		}
		SaveUtil.WriteSerializableFile(this.saveEtcFileName, dictionary);
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x0014E634 File Offset: 0x0014C834
	public void LoadUnlimitData()
	{
		if (!File.Exists(this.saveUnlimitFileName))
		{
			Debug.Log("save file don't exists");
			return;
		}
		this.ReleaseGame();
		this.bPlayingGame = true;
		this.calcTime = true;
		string text = "old";
		Dictionary<string, object> dic = SaveUtil.ReadSerializableFile(this.saveUnlimitFileName);
		Dictionary<string, object> dic2 = null;
		Dictionary<string, object> dic3 = null;
		Dictionary<string, object> dic4 = null;
		GameUtil.TryGetValue<string>(dic, "saveVer", ref text);
		GameUtil.TryGetValue<Dictionary<string, object>>(dic, "money", ref dic2);
		GameUtil.TryGetValue<Dictionary<string, object>>(dic, "agents", ref dic3);
		GameUtil.TryGetValue<Dictionary<string, object>>(dic, "creatures", ref dic4);
		MoneyModel.instance.LoadData(dic2);
		if (text == "old")
		{
			PlayerModel.instance.SetDay(35);
		}
		else
		{
			PlayerModel.instance.SetDay(50);
		}
		PlayerModel.instance.UnlimitMode(text);
		CreatureManager.instance.LoadData(dic4);
		AgentManager.instance.LoadData(dic3);
		this.gameMode = GameMode.UNLIMIT_MODE;
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x0002DAF0 File Offset: 0x0002BCF0
	public bool ExistSaveData()
	{
		return File.Exists(this.saveFileName);
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x0002DAFD File Offset: 0x0002BCFD
	public bool ExistUnlimitData()
	{
		return File.Exists(this.saveUnlimitFileName);
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0002DB0A File Offset: 0x0002BD0A
	public bool ExistEtcData()
	{
		return File.Exists(this.saveEtcFileName);
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x0002DB17 File Offset: 0x0002BD17
	public void RemoveUnlimitData()
	{
		SaveUtil.DeleteSerializableFile(this.saveUnlimitFileName);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x0002DB24 File Offset: 0x0002BD24
	public void RemoveSaveData()
	{
		SaveUtil.DeleteSerializableFile(this.saveFileName);
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x0002DB31 File Offset: 0x0002BD31
	public void RemoveEtcData()
	{
		SaveUtil.DeleteSerializableFile(this.saveEtcFileName);
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x0014E724 File Offset: 0x0014C924
	public void SaveGlobalData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("observe", CreatureManager.instance.GetSaveObserveData());
		dictionary.Add("etcData", GlobalEtcDataModel.instance.GetGlobalSaveData());
		dictionary.Add("inventory", InventoryModel.Instance.GetGlobalSaveData());
		dictionary.Add("research", ResearchDataModel.instance.GetSaveData());
		dictionary.Add("missions", MissionManager.instance.GetSaveData());
		dictionary.Add("sefiraCharactes", SefiraCharacterManager.instance.GetSaveData());
		SaveUtil.WriteSerializableFile(this.saveGlobalFileName, dictionary);
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x0002DB3E File Offset: 0x0002BD3E
	public bool TryGetGlobalData(out Dictionary<string, object> dictionary)
	{
		if (File.Exists(this.saveGlobalFileName))
		{
			dictionary = SaveUtil.ReadSerializableFile(this.saveGlobalFileName);
			return true;
		}
		dictionary = null;
		return false;
	}

	// Token: 0x06003148 RID: 12616 RVA: 0x0002DB63 File Offset: 0x0002BD63
	public bool TrySetGlobalInventoryData(Dictionary<string, object> dictionary)
	{
		if (File.Exists(this.saveGlobalFileName))
		{
			SaveUtil.WriteSerializableFile(this.saveGlobalFileName, dictionary);
		}
		return false;
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x0014E7C4 File Offset: 0x0014C9C4
	public void LoadGlobalData()
	{
		try
		{
			if (!File.Exists(this.saveGlobalFileName))
			{
				CreatureManager.instance.ResetObserveData();
				GlobalEtcDataModel.instance.ResetGlobalData();
				ResearchDataModel.instance.Init();
				InventoryModel.Instance.Init();
				MissionManager.instance.Init();
				SefiraCharacterManager.instance.Init();
			}
			else
			{
				Dictionary<string, object> dic = SaveUtil.ReadSerializableFile(this.saveGlobalFileName);
				Dictionary<string, object> dic2 = null;
				Dictionary<string, object> dic3 = null;
				Dictionary<string, object> dic4 = null;
				Dictionary<string, object> dic5 = null;
				Dictionary<string, object> dic6 = null;
				Dictionary<string, object> dic7 = null;
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "observe", ref dic2))
				{
					CreatureManager.instance.LoadObserveData(dic2);
				}
				else
				{
					CreatureManager.instance.ResetObserveData();
				}
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "etcData", ref dic4))
				{
					GlobalEtcDataModel.instance.LoadGlobalData(dic4);
				}
				else
				{
					GlobalEtcDataModel.instance.ResetGlobalData();
				}
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "research", ref dic5))
				{
					ResearchDataModel.instance.LoadData(dic5);
				}
				else
				{
					ResearchDataModel.instance.Init();
				}
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "missions", ref dic6))
				{
					MissionManager.instance.LoadData(dic6);
				}
				else
				{
					MissionManager.instance.Init();
				}
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "inventory", ref dic3))
				{
					InventoryModel.Instance.LoadGlobalData(dic3);
				}
				else
				{
					InventoryModel.Instance.Init();
				}
				if (GameUtil.TryGetValue<Dictionary<string, object>>(dic, "sefiraCharactes", ref dic7))
				{
					SefiraCharacterManager.instance.LoadData(dic7);
				}
				else
				{
					SefiraCharacterManager.instance.Init();
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/Glerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x0014E96C File Offset: 0x0014CB6C
	public void RemoveGlobalData()
	{
		SaveUtil.DeleteSerializableFile(this.saveGlobalFileName);
		CreatureManager.instance.ResetObserveData();
		GlobalEtcDataModel.instance.ResetGlobalData();
		ResearchDataModel.instance.Init();
		InventoryModel.Instance.Init();
		MissionManager.instance.Init();
		SefiraCharacterManager.instance.Init();
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x0002DB82 File Offset: 0x0002BD82
	public void OnNotice(string name, params object[] param)
	{
		if (NoticeName.AutoSaveTimer == name)
		{
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x0014E9C0 File Offset: 0x0014CBC0
	public void ChangeLanguage(SystemLanguage value)
	{
		this.Language = value;
		this.language = this.GetLanguage(this.Language);
		GameStaticDataLoader.ReloadData();
		this.SetLanguageFont();
		Notice.instance.Send(NoticeName.LanaguageChange, new object[0]);
		ManualUI.Instance.Reload();
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x0014EA10 File Offset: 0x0014CC10
	public string GetCurrentLanguage()
	{
		SystemLanguage systemLanguage = this.Language;
		switch (systemLanguage)
		{
		case SystemLanguage.Vietnamese:
			return "vn";
		case SystemLanguage.ChineseSimplified:
			break;
		case SystemLanguage.ChineseTraditional:
			return "cn_tr";
		default:
			if (systemLanguage == SystemLanguage.Bulgarian)
			{
				return "bg";
			}
			if (systemLanguage != SystemLanguage.Chinese)
			{
				if (systemLanguage == SystemLanguage.Japanese)
				{
					return "jp";
				}
				if (systemLanguage == SystemLanguage.Korean)
				{
					return "kr";
				}
				if (systemLanguage == SystemLanguage.English)
				{
					return "en";
				}
				if (systemLanguage == SystemLanguage.Russian)
				{
					return "ru";
				}
				if (systemLanguage != SystemLanguage.Spanish)
				{
					return "en";
				}
				return "es";
			}
			break;
		}
		return "cn";
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x0002DB94 File Offset: 0x0002BD94
	private string GetLogSrc()
	{
		return string.Concat(new object[]
		{
			Application.persistentDataPath,
			"/Log/170808_",
			this.logCount,
			".txt"
		});
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x0014EA9C File Offset: 0x0014CC9C
	public void LoadStateData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		if (!File.Exists(this.stateSrc))
		{
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(this.stateSrc, FileMode.Open);
		Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
		fileStream.Close();
		string empty = string.Empty;
		if (GameUtil.TryGetValue<string>(dic, "language", ref empty))
		{
			this.language = empty;
		}
		float currentBgmVolume = 1f;
		float currentVolume = 1f;
		if (GameUtil.TryGetValue<float>(dic, "bgmVolume", ref currentBgmVolume))
		{
			this.sceneDataSaver.currentBgmVolume = currentBgmVolume;
		}
		else
		{
			this.sceneDataSaver.currentBgmVolume = 0.8f;
		}
		if (GameUtil.TryGetValue<float>(dic, "masterVolume", ref currentVolume))
		{
			this.sceneDataSaver.currentVolume = currentVolume;
		}
		bool tooltipState = false;
		if (GameUtil.TryGetValue<bool>(dic, "tooltip", ref tooltipState))
		{
			this.sceneDataSaver.tooltipState = tooltipState;
		}
		bool flag = true;
		if (GameUtil.TryGetValue<bool>(dic, "dlcCreatureOn", ref flag))
		{
			this.dlcCreatureOn = flag;
		}
		if (GameUtil.TryGetValue<int>(dic, "logIndex", ref this.logCount))
		{
			Debug.Log("Read Last Log : " + this.logCount);
			this.logCount = (this.logCount + 1) % 10;
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x0014EC00 File Offset: 0x0014CE00
	public void SaveStateData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(this.stateSrc, FileMode.Create);
		dictionary.Add("bgmVolume", this.sceneDataSaver.currentBgmVolume);
		dictionary.Add("masterVolume", this.sceneDataSaver.currentVolume);
		dictionary.Add("tooltip", this.sceneDataSaver.tooltipState);
		dictionary.Add("dlcCreatureOn", this.dlcCreatureOn);
		dictionary.Add("language", this.GetCurrentLanguage());
		dictionary.Add("logIndex", this.logCount);
		binaryFormatter.Serialize(fileStream, dictionary);
		fileStream.Close();
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000040A1 File Offset: 0x000022A1
	static GlobalGameManager()
	{
	}

	// Token: 0x04002EB3 RID: 11955
	public int ScreenWidth = Screen.width;

	// Token: 0x04002EB4 RID: 11956
	private static GlobalGameManager _instance;

	// Token: 0x04002EB5 RID: 11957
	private string saveFileName;

	// Token: 0x04002EB6 RID: 11958
	private string saveGlobalFileName;

	// Token: 0x04002EB7 RID: 11959
	private string saveUnlimitFileName;

	// Token: 0x04002EB8 RID: 11960
	private string saveEtcFileName;

	// Token: 0x04002EB9 RID: 11961
	private const string SAVE_VER = "ver1";

	// Token: 0x04002EBA RID: 11962
	private const string ver = "170808";

	// Token: 0x04002EBB RID: 11963
	public const string verPathName = "170808";

	// Token: 0x04002EBC RID: 11964
	public const string saveVerName = "170808";

	// Token: 0x04002EBD RID: 11965
	private SystemLanguage _language = SystemLanguage.English;

	// Token: 0x04002EBE RID: 11966
	public string language = "en";

	// Token: 0x04002EBF RID: 11967
	[NonSerialized]
	public bool dlcCreatureOn = true;

	// Token: 0x04002EC0 RID: 11968
	private bool bPlayingGame;

	// Token: 0x04002EC1 RID: 11969
	public bool lastLoaded;

	// Token: 0x04002EC2 RID: 11970
	public bool isLoaded;

	// Token: 0x04002EC3 RID: 11971
	public string loadingScene = "TitleEndScene";

	// Token: 0x04002EC4 RID: 11972
	public string storySaveDir = "/story";

	// Token: 0x04002EC5 RID: 11973
	public string saveState = "story";

	// Token: 0x04002EC6 RID: 11974
	public string singledaySave = "/singleData";

	// Token: 0x04002EC7 RID: 11975
	public string etcRemembered = "/etc";

	// Token: 0x04002EC8 RID: 11976
	private int logCount;

	// Token: 0x04002EC9 RID: 11977
	private const int logMax = 10;

	// Token: 0x04002ECA RID: 11978
	public GameMode gameMode;

	// Token: 0x04002ECB RID: 11979
	public int tutorialStep = 1;

	// Token: 0x04002ECC RID: 11980
	public float playTime;

	// Token: 0x04002ECD RID: 11981
	public bool _tutorialPlayed;

	// Token: 0x04002ECE RID: 11982
	private bool _isPlayingTutorial;

	// Token: 0x04002ECF RID: 11983
	private string logOutput = string.Empty;

	// Token: 0x04002ED0 RID: 11984
	private string logStack = string.Empty;

	// Token: 0x04002ED1 RID: 11985
	public const int checkPointOffset = 10000;

	// Token: 0x04002ED2 RID: 11986
	private bool calcTime;

	// Token: 0x04002ED3 RID: 11987
	public LoadingScreen loadingScreen;

	// Token: 0x04002ED4 RID: 11988
	public SceneDataSave sceneDataSaver;

	// Token: 0x04002ED5 RID: 11989
	public Canvas canvas;

	// Token: 0x04002ED6 RID: 11990
	[SerializeField]
	private List<GlobalGameManager.LanguageFont> fontList;

	// Token: 0x04002ED7 RID: 11991
	[HideInInspector]
	public Dictionary<string, object> preLoadedTutorialData;

	// Token: 0x04002ED8 RID: 11992
	public string BuildVer;

	// Token: 0x04002ED9 RID: 11993
	private static GlobalGameManager.LanguageFont _currentLanguageFont;

	// Token: 0x0200059D RID: 1437
	[Serializable]
	public class FontData
	{
		// Token: 0x06003152 RID: 12626 RVA: 0x00004074 File Offset: 0x00002274
		public FontData()
		{
		}

		// Token: 0x04002EDC RID: 11996
		public Font font;

		// Token: 0x04002EDD RID: 11997
		public FontType type;
	}

	// Token: 0x0200059E RID: 1438
	[Serializable]
	public class LanguageFont
	{
		// Token: 0x06003153 RID: 12627 RVA: 0x00004074 File Offset: 0x00002274
		public LanguageFont()
		{
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0014ECE0 File Offset: 0x0014CEE0
		public GlobalGameManager.FontData GetFont(FontType type)
		{
			GlobalGameManager.FontData result = null;
			foreach (GlobalGameManager.FontData fontData in this.list)
			{
				if (fontData.type == type)
				{
					result = fontData;
					break;
				}
			}
			return result;
		}

		// Token: 0x04002EDE RID: 11998
		public string language;

		// Token: 0x04002EDF RID: 11999
		public List<GlobalGameManager.FontData> list;
	}
}
