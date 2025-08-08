/*
public void RandomizeWorkId() // Optimized
*/
using System;
using System.Collections.Generic;
using GameStatusUI;
using KetherBoss;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class SefiraBossManager : IObserver
{
	// Token: 0x0600406A RID: 16490 RVA: 0x0018E144 File Offset: 0x0018C344
	private SefiraBossManager()
	{
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
		Notice.instance.Observe(NoticeName.Update, this);
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x0600406B RID: 16491 RVA: 0x00037891 File Offset: 0x00035A91
	public static SefiraBossManager Instance
	{
		get
		{
			if (SefiraBossManager._instance == null)
			{
				SefiraBossManager._instance = new SefiraBossManager();
			}
			return SefiraBossManager._instance;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x0600406C RID: 16492 RVA: 0x000378AC File Offset: 0x00035AAC
	public SefiraEnum CurrentActivatedSefira
	{
		get
		{
			return this.currentActivated;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x0600406D RID: 16493 RVA: 0x000378B4 File Offset: 0x00035AB4
	// (set) Token: 0x0600406E RID: 16494 RVA: 0x000378BC File Offset: 0x00035ABC
	public SefiraBossBase CurrentBossBase
	{
		get
		{
			return this._currentBossBase;
		}
		private set
		{
			this._currentBossBase = value;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x0600406F RID: 16495 RVA: 0x000378C5 File Offset: 0x00035AC5
	public bool IsCleared
	{
		get
		{
			return this._isCleared;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06004070 RID: 16496 RVA: 0x000378CD File Offset: 0x00035ACD
	public bool IsRecoverBlocked
	{
		get
		{
			return this._isRecoverBlocked;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x06004071 RID: 16497 RVA: 0x000378D5 File Offset: 0x00035AD5
	public bool IsWorkCancelable
	{
		get
		{
			return this._isWorkCancelable;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06004072 RID: 16498 RVA: 0x000378DD File Offset: 0x00035ADD
	public bool IsTutorial
	{
		get
		{
			return this._tutorial;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06004073 RID: 16499 RVA: 0x000378E5 File Offset: 0x00035AE5
	// (set) Token: 0x06004074 RID: 16500 RVA: 0x000378ED File Offset: 0x00035AED
	public bool TutorialPlayed
	{
		get
		{
			return this._tutorialPlayed;
		}
		set
		{
			this._tutorialPlayed = value;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06004075 RID: 16501 RVA: 0x000378F6 File Offset: 0x00035AF6
	public Dictionary<string, bool> ClearState
	{
		get
		{
			return this._clearState;
		}
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x000378FE File Offset: 0x00035AFE
	private void OnDestroy()
	{
		Notice.instance.Remove(NoticeName.FixedUpdate, this);
		Notice.instance.Remove(NoticeName.Update, this);
	}

	// Token: 0x06004077 RID: 16503 RVA: 0x00037920 File Offset: 0x00035B20
	public void SetRecoverBlockState(bool state)
	{
		this._isRecoverBlocked = state;
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x00037929 File Offset: 0x00035B29
	public void SetWorkCancelableState(bool state)
	{
		this._isWorkCancelable = state;
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x0018E1A8 File Offset: 0x0018C3A8
	public void Init()
	{
		this._isCleared = false;
		this.currentActivated = SefiraEnum.DUMMY;
		this._bossBgmDic.Clear();
		this.SetRecoverBlockState(false);
		this.SetWorkCancelableState(true);
		this.SetKeyCounts();
		this.ClearState.Clear();
		foreach (string key in SefiraBossManager.ketherSaveRegions)
		{
			this.ClearState.Add(key, false);
		}
		if (GlobalGameManager.instance.ExistEtcData())
		{
			try
			{
				this.LoadSaveData(GlobalGameManager.instance.LoadEtcFile());
			}
			catch (FileReadException ex)
			{
				Debug.LogError("Reading file failure " + ex.fileName);
				GlobalGameManager.instance.RemoveEtcData();
			}
		}
		this._tutorial = false;
		if (MissionManager.instance.ExistsBossMission() && !this.TutorialPlayed)
		{
			this._tutorial = true;
		}
		int day = PlayerModel.instance.GetDay();
		if (day >= 45 && GlobalGameManager.instance.gameMode == GameMode.STORY_MODE)
		{
			try
			{
				this.SetActivatedBoss(SefiraEnum.KETHER);
			}
			catch (Exception ex2)
			{
			}
		}
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x0018E2E0 File Offset: 0x0018C4E0
	public void SaveBossSessionData(Dictionary<string, object> dic)
	{
		dic.Add("sefirabossTutorialPlayed", this.TutorialPlayed);
		foreach (string key in SefiraBossManager.ketherSaveRegions)
		{
			dic.Add(key, this.ClearState[key]);
		}
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x0018E33C File Offset: 0x0018C53C
	public void LoadSaveData(Dictionary<string, object> data)
	{
		object obj = null;
		if (data.TryGetValue("sefirabossTutorialPlayed", out obj))
		{
			this.TutorialPlayed = (bool)obj;
		}
		else
		{
			this.TutorialPlayed = false;
		}
		foreach (string key in SefiraBossManager.ketherSaveRegions)
		{
			object obj2 = false;
			if (data.TryGetValue(key, out obj2))
			{
				this.ClearState[key] = (bool)obj2;
			}
		}
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x0018E3BC File Offset: 0x0018C5BC
	public void AddBossBgm(params string[] bgmSrc)
	{
		this._bossBgmDic.Clear();
		for (int i = 0; i < bgmSrc.Length; i++)
		{
			this._bossBgmDic.Add(i, bgmSrc[i]);
		}
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x0018E3F8 File Offset: 0x0018C5F8
	public void PlayBossBgm(int index = -1)
	{
		if (index == -1)
		{
			BgmManager.instance.SetBossClip();
			return;
		}
		string empty = string.Empty;
		if (this._bossBgmDic.TryGetValue(index, out empty))
		{
			BgmManager.instance.SetBossClip(empty);
		}
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x0018E440 File Offset: 0x0018C640
	private void SetKeyCounts()
	{
		this.keyCountDic.Clear();
		for (int i = 0; i < 11; i++)
		{
			SefiraEnum sefiraEnum = (SefiraEnum)i;
			string arg = sefiraEnum.ToString().ToLower();
			if (sefiraEnum == SefiraEnum.KETHER)
			{
				for (int j = 1; j <= 4; j++)
				{
					KetherBossType ketherBossType = (KetherBossType)j;
					foreach (string arg2 in SefiraBossManager.keyValues)
					{
						string text = string.Format("boss_{0}_{2}_{1}_", arg, ketherBossType.ToString(), arg2);
						int value = 0;
						for (int l = 0; l < 15; l++)
						{
							string id = text + l;
							string text2 = LocalizeTextDataModel.instance.GetText(id);
							if (text2 == "UNKNOWN")
							{
								break;
							}
							value = l + 1;
						}
						this.keyCountDic.Add(text, value);
					}
				}
			}
			else
			{
				foreach (string arg3 in SefiraBossManager.keyValues)
				{
					string text3 = string.Format("boss_{0}_{1}_", arg, arg3);
					int value2 = 0;
					int num = 0;
					while (i < 15)
					{
						string id2 = text3 + num;
						string text4 = LocalizeTextDataModel.instance.GetText(id2);
						if (text4 == "UNKNOWN")
						{
							break;
						}
						value2 = num + 1;
						num++;
					}
					this.keyCountDic.Add(text3, value2);
				}
			}
		}
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x0018E5E0 File Offset: 0x0018C7E0
	public bool TryGetBossDesc(SefiraEnum sefira, SefiraBossDescType type, out string text)
	{
		string text2 = string.Empty;
		if (sefira != SefiraEnum.KETHER)
		{
			text2 = string.Format("boss_{0}_{1}_", sefira.ToString().ToLower(), SefiraBossManager.keyValues[(int)type]);
		}
		else
		{
			text2 = string.Format("boss_{0}_{2}_{1}_", sefira.ToString().ToLower(), this.GetKetherBossType(), SefiraBossManager.keyValues[(int)type]);
		}
		int max = -1;
		if (!this.keyCountDic.TryGetValue(text2, out max))
		{
			text = "UNKNOWN";
			return false;
		}
		text2 += UnityEngine.Random.Range(0, max);
		text = LocalizeTextDataModel.instance.GetText(text2);
		return true;
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x0018E694 File Offset: 0x0018C894
	public bool TryGetBossDescCount(SefiraEnum sefira, SefiraBossDescType type, out int max)
	{
		string key = string.Empty;
		if (sefira != SefiraEnum.KETHER)
		{
			key = string.Format("boss_{0}_{1}_", sefira.ToString().ToLower(), SefiraBossManager.keyValues[(int)type]);
		}
		else
		{
			key = string.Format("boss_{0}_{2}_{1}_", sefira.ToString().ToLower(), this.GetKetherBossType(), SefiraBossManager.keyValues[(int)type]);
		}
		return this.keyCountDic.TryGetValue(key, out max);
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x0018E714 File Offset: 0x0018C914
	public bool TryGetBossDesc(SefiraEnum sefira, SefiraBossDescType type, int index, out string text)
	{
		string text2 = string.Empty;
		if (sefira != SefiraEnum.KETHER)
		{
			text2 = string.Format("boss_{0}_{1}_", sefira.ToString().ToLower(), SefiraBossManager.keyValues[(int)type]);
		}
		else
		{
			text2 = string.Format("boss_{0}_{2}_{1}_", sefira.ToString().ToLower(), this.GetKetherBossType(), SefiraBossManager.keyValues[(int)type]);
		}
		int num = -1;
		if (!this.keyCountDic.TryGetValue(text2, out num))
		{
			text = "UNKNOWN";
			return false;
		}
		text2 += index;
		text = LocalizeTextDataModel.instance.GetText(text2);
		return text != "UNKNOWN";
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x00037932 File Offset: 0x00035B32
	public void SetActivatedBoss(SefiraEnum sefira)
	{
		this.currentActivated = sefira;
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x0003793B File Offset: 0x00035B3B
	public bool CheckBossActivation(SefiraEnum sefira)
	{
		return (this.currentActivated == SefiraEnum.TIPERERTH1 && sefira == SefiraEnum.TIPERERTH2) || this.currentActivated == sefira;
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x0003795B File Offset: 0x00035B5B
	public bool IsAnyBossSessionActivated()
	{
		return this.currentActivated != SefiraEnum.DUMMY;
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x0003796A File Offset: 0x00035B6A
	public void OnOverloadActivated(int currentValue)
	{
		if (this.CheckBossActivation(SefiraEnum.MALKUT))
		{
			this.RandomizeWorkId();
		}
		if (this.CurrentBossBase != null)
		{
			this.CurrentBossBase.OnOverloadActivated(currentValue);
		}
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x0018E7D0 File Offset: 0x0018C9D0
	public void OnStageStart()
	{
		this.workId = new int[]
		{
			1,
			2,
			3,
			4
		};
		SefiraBossBase sefiraBossBase = this.GenBossBase();
		if (sefiraBossBase != null)
		{
			this.CurrentBossBase = sefiraBossBase;
			sefiraBossBase.OnStageStart();
			if (sefiraBossBase.IsStartEmergencyBgm())
			{
				this.PlayBossBgm(-1);
			}
		}
		if (this.CheckBossActivation(SefiraEnum.HOD))
		{
			this.GenerateHodBuf();
		}
		if (this.CheckBossActivation(SefiraEnum.YESOD))
		{
			this.GenYesodBossSetting();
		}
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x0018E844 File Offset: 0x0018CA44
	private SefiraBossBase GenBossBase()
	{
		if (this.currentActivated == SefiraEnum.DUMMY)
		{
			return null;
		}
		switch (this.currentActivated)
		{
		case SefiraEnum.MALKUT:
			return new MalkutBossBase();
		case SefiraEnum.YESOD:
			return new YesodBossBase();
		case SefiraEnum.HOD:
			return new HodBossBase();
		case SefiraEnum.NETZACH:
			return new NetzachBossBase();
		case SefiraEnum.TIPERERTH1:
			return new TipherethBossBase();
		case SefiraEnum.GEBURAH:
			return new GeburahBossBase();
		case SefiraEnum.CHESED:
			return new ChesedBossBase();
		case SefiraEnum.BINAH:
			return new BinahBossBase();
		case SefiraEnum.CHOKHMAH:
			return new ChokhmahBossBase();
		case SefiraEnum.KETHER:
			switch (PlayerModel.instance.GetDay())
			{
			case 45:
				return new KetherZeroBossBase();
			case 46:
				return new KetherUpperBossBase();
			case 47:
				return new KetherMiddleBossBase();
			case 48:
				return new KetherLowerBossBase();
			case 49:
				return new KetherLastBossBase();
			default:
				return new KetherZeroBossBase();
			}
			break;
		}
		return null;
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x00037995 File Offset: 0x00035B95
	public void OnStageEnd()
	{
		if (this.CheckBossActivation(SefiraEnum.YESOD))
		{
			this.ResetYesodBossSetting();
		}
		this.CurrentBossBase = null;
		GlobalGameManager.instance.SaveEtcData();
		this.currentActivated = SefiraEnum.DUMMY;
	}

	// Token: 0x06004089 RID: 16521 RVA: 0x0018E928 File Offset: 0x0018CB28
	public void GenerateHodBuf()
	{
		List<HodBossBuf> list = new List<HodBossBuf>();
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			HodBossBuf hodBossBuf = new HodBossBuf();
			agentModel.AddUnitBuf(hodBossBuf);
			list.Add(hodBossBuf);
		}
		if (this.CurrentBossBase != null && this.CurrentBossBase is HodBossBase)
		{
			(this.CurrentBossBase as HodBossBase).bufList = list;
		}
		if (this.IsKetherBoss(KetherBossType.E1))
		{
			(this.CurrentBossBase as KetherUpperBossBase).bufList = list;
		}
	}

	// Token: 0x0600408A RID: 16522 RVA: 0x0018E9E4 File Offset: 0x0018CBE4
	public int GetWorkId(int id)
	{
		if (this.IsKetherBoss())
		{
			if (this.CurrentBossBase == null || !(this.CurrentBossBase is KetherUpperBossBase))
			{
				return id;
			}
		}
		else if (!this.CheckBossActivation(SefiraEnum.MALKUT))
		{
			return id;
		}
		return this.workId[id - 1];
	}

	// Token: 0x0600408B RID: 16523 RVA: 0x0018EA38 File Offset: 0x0018CC38
	public void RandomizeWorkId()
	{ // <Mod> optimized
		int rand = UnityEngine.Random.Range(1, 24);
		List<int> list = new List<int>();
		for (int i = 0; i < 4; i++)
		{
			list.Add(i + 1);
		}
		for (int i = 0; i < 4; i++)
		{
			int ind = rand % list.Count;
			rand /= list.Count;
			workId[i] = list[ind];
			list.RemoveAt(ind);
		}
	}

	// Token: 0x0600408C RID: 16524 RVA: 0x0018EB00 File Offset: 0x0018CD00
	public void GenYesodBossSetting()
	{
		Camera cam = UIActivateManager.instance.GetCam();
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/YesodBossRenderCamera");
		YesodBossCameraScript component = gameObject.GetComponent<YesodBossCameraScript>();
		gameObject.transform.localPosition = cam.transform.localPosition;
		gameObject.transform.localScale = cam.transform.localScale;
		gameObject.transform.localRotation = cam.transform.localRotation;
		component.Init(cam);
		this.cameraScript = component;
		if (this.CurrentBossBase != null && this.CurrentBossBase is YesodBossBase)
		{
			(this.CurrentBossBase as YesodBossBase).SetCameraScript(component);
		}
		else if (this.IsKetherBoss(KetherBossType.E1))
		{
			(this.CurrentBossBase as KetherUpperBossBase).SetCameraScript(component);
		}
	}

	// Token: 0x0600408D RID: 16525 RVA: 0x0018EBC8 File Offset: 0x0018CDC8
	public void ResetYesodBossSetting()
	{
		Camera cam = UIActivateManager.instance.GetCam();
		if (this.cameraScript != null)
		{
			UnityEngine.Object.DestroyObject(this.cameraScript.gameObject);
			this.cameraScript = null;
		}
		try
		{
			cam.GetComponent<CameraFilterPack_FX_8bits>().enabled = false;
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		cam.clearFlags = CameraClearFlags.Depth;
		cam.targetTexture.Release();
		cam.targetTexture = null;
	}

	// Token: 0x0600408E RID: 16526 RVA: 0x0018EC50 File Offset: 0x0018CE50
	public bool IsBossReady(SefiraEnum sefira)
	{
		if (PlayerModel.instance.GetDay() >= 45)
		{
			return true;
		}
		if (this._tutorial && PlayerModel.instance.GetDay() % 5 != 0)
		{
			return sefira == SefiraEnum.YESOD || sefira == SefiraEnum.HOD || sefira == SefiraEnum.NETZACH;
		}
		return MissionManager.instance.ExistsBossMission(sefira);
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x0018ECB0 File Offset: 0x0018CEB0
	public bool IsBossStartable(SefiraEnum sefira, out List<string> require)
	{
		int num = 21;
		switch (sefira)
		{
		case SefiraEnum.MALKUT:
		case SefiraEnum.YESOD:
		case SefiraEnum.HOD:
		case SefiraEnum.NETZACH:
			num = 21;
			break;
		case SefiraEnum.TIPERERTH1:
		case SefiraEnum.TIPERERTH2:
		case SefiraEnum.GEBURAH:
		case SefiraEnum.CHESED:
			num = 36;
			break;
		case SefiraEnum.BINAH:
		case SefiraEnum.CHOKHMAH:
			num = 41;
			break;
		}
		require = new List<string>();
		if (this._tutorial && PlayerModel.instance.GetDay() % 5 != 0)
		{
			return sefira == SefiraEnum.YESOD || sefira == SefiraEnum.HOD;
		}
		int day = PlayerModel.instance.GetDay();
		bool flag = true;
		if (day % 5 == 0)
		{
			flag = false;
		}
		if (day >= 45)
		{
			return false;
		}
		if (day < num)
		{
			require.Add(string.Format(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_day"), num + 1, num));
			flag = false;
		}
		if (!flag)
		{
			require.Add(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_checkpoint"));
		}
		return flag;
	}

	// Token: 0x06004090 RID: 16528 RVA: 0x0018EDB4 File Offset: 0x0018CFB4
	public bool IsBossStartable(SefiraEnum sefira)
	{
		List<string> list;
		return this.IsBossStartable(sefira, out list);
	}

	// Token: 0x06004091 RID: 16529 RVA: 0x0018EDCC File Offset: 0x0018CFCC
	public bool OnStartBossSession(SefiraEnum sefira)
	{
		if (!this.IsBossReady(sefira))
		{
			return false;
		}
		if (!this.IsBossStartable(sefira))
		{
			return false;
		}
		bool result;
		try
		{
			this.SetActivatedBoss(sefira);
			Sefira sefira2 = SefiraManager.instance.GetSefira(sefira);
			result = true;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			result = false;
		}
		return result;
	}

	// Token: 0x06004092 RID: 16530 RVA: 0x0018EE30 File Offset: 0x0018D030
	public SefiraBossCreatureModel AddCreature(MapNode pos, SefiraBossBase sefiraBoss, string scriptName, string animName, long metaId = 400001L)
	{
		SefiraBossCreatureModel sefiraBossCreatureModel = new SefiraBossCreatureModel((long)this.nextInstId++);
		this.BuildCreature(sefiraBossCreatureModel, metaId, scriptName);
		sefiraBossCreatureModel.GetMovableNode().SetCurrentNode(pos);
		sefiraBossCreatureModel.GetMovableNode().SetActive(true);
		sefiraBossCreatureModel.baseMaxHp = sefiraBossCreatureModel.metaInfo.maxHp;
		sefiraBossCreatureModel.hp = (float)sefiraBossCreatureModel.metaInfo.maxHp;
		sefiraBossCreatureModel.SetSefiraBoss(sefiraBoss);
		Notice.instance.Send(NoticeName.AddSefiraBossCreature, new object[]
		{
			sefiraBossCreatureModel,
			animName
		});
		sefiraBossCreatureModel.script.OnInit();
		Sefira sefira = SefiraManager.instance.GetSefira(sefiraBoss.sefiraEnum);
		sefiraBossCreatureModel.sefira = sefira;
		sefiraBossCreatureModel.sefiraNum = sefira.indexString;
		sefiraBoss.modelList.Add(sefiraBossCreatureModel);
		return sefiraBossCreatureModel;
	}

	// Token: 0x06004093 RID: 16531 RVA: 0x0018EEFC File Offset: 0x0018D0FC
	private void BuildCreature(SefiraBossCreatureModel model, long metadataid, string scriptName)
	{
		model.observeInfo = new CreatureObserveInfoModel(metadataid);
		model.observeInfo.OnObserveRegion("stat");
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(metadataid);
		model.metadataId = metadataid;
		model.metaInfo = data;
		if (CreatureTypeList.instance.GetSkillTipData(metadataid) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(metadataid).GetCopy();
		}
		if (scriptName == string.Empty)
		{
			scriptName = data.script;
		}
		object obj = Activator.CreateInstance(Type.GetType(scriptName));
		if (obj is CreatureBase)
		{
			model.script = (CreatureBase)obj;
		}
		else
		{
			Debug.Log("Creature Script not found");
		}
		model.script.SetModel(model);
		model.script.OnInitialBuild();
	}

	// Token: 0x06004094 RID: 16532 RVA: 0x000379C2 File Offset: 0x00035BC2
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.FixedUpdate)
		{
			this.OnFixedUpdate();
		}
		else if (notice == NoticeName.Update)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x000379F5 File Offset: 0x00035BF5
	public bool IsClearedDay()
	{
		return this.CurrentActivatedSefira == SefiraEnum.DUMMY || (this.CurrentBossBase != null && this.CurrentBossBase.IsCleared());
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x00037A1E File Offset: 0x00035C1E
	public void OnUpdate()
	{
		if (this.CurrentBossBase != null)
		{
			this.CurrentBossBase.Update();
		}
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x0018EFC0 File Offset: 0x0018D1C0
	public void OnFixedUpdate()
	{
		if (this.CurrentBossBase != null)
		{
			this.CurrentBossBase.FixedUpdate();
			if (this.IsClearedDay())
			{
				if (!this.IsCleared)
				{
					this._isCleared = true;
					this.CurrentBossBase.OnCleared();
				}
				if (this.CurrentBossBase != null && this.CurrentBossBase.IsReadyToClose())
				{
					Notice.instance.Send(NoticeName.OnDestroyBossCore, new object[]
					{
						this.currentActivated
					});
					if (!GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared)
					{
						GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared = true;
						GameStatusUI.GameStatusUI.Window.sceneController.OnClickNextDay();
						if (!GameManager.currentGameManager.StageEnded)
						{
							GameManager.currentGameManager.ClearAction();
						}
					}
				}
			}
		}
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x000378DD File Offset: 0x00035ADD
	public bool DisplayTutorial()
	{
		return this._tutorial;
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x00037A36 File Offset: 0x00035C36
	public void OnTutorialEnd()
	{
		this._tutorial = false;
		this._tutorialPlayed = true;
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x0018F098 File Offset: 0x0018D298
	public void ForcelyClear()
	{
		Notice.instance.Send(NoticeName.OnDestroyBossCore, new object[]
		{
			this.currentActivated
		});
		if (!GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared)
		{
			GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared = true;
			GameStatusUI.GameStatusUI.Window.sceneController.OnClickNextDay();
			if (!GameManager.currentGameManager.StageEnded)
			{
				GameManager.currentGameManager.ClearAction();
			}
		}
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x00037A46 File Offset: 0x00035C46
	public bool IsKetherBoss()
	{
		return (PlayerModel.instance.GetDay() >= 45 || this.CurrentActivatedSefira == SefiraEnum.KETHER) && GlobalGameManager.instance.gameMode != GameMode.UNLIMIT_MODE;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x0018F114 File Offset: 0x0018D314
	public bool IsKetherBoss(KetherBossType type)
	{
		if (!this.IsKetherBoss())
		{
			return false;
		}
		if (this.CurrentBossBase == null)
		{
			int day = PlayerModel.instance.GetDay();
			int num = day - 45;
			return num == (int)type;
		}
		switch (type)
		{
		case KetherBossType.E0:
			return this.CurrentBossBase is KetherZeroBossBase;
		case KetherBossType.E1:
			return this.CurrentBossBase is KetherUpperBossBase;
		case KetherBossType.E2:
			return this.CurrentBossBase is KetherMiddleBossBase;
		case KetherBossType.E3:
			return this.CurrentBossBase is KetherLowerBossBase;
		case KetherBossType.E4:
			return this.CurrentBossBase is KetherLastBossBase;
		default:
			return false;
		}
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x00037A79 File Offset: 0x00035C79
	public KetherBossType GetKetherBossType()
	{
		if (this.IsKetherBoss())
		{
			return (this.CurrentBossBase as KetherBossBase).type;
		}
		return KetherBossType.E0;
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x0018F1C0 File Offset: 0x0018D3C0
	static SefiraBossManager()
	{
	}

	// Token: 0x04003B9A RID: 15258
	private const string yesodCamera = "Effect/SefiraBoss/YesodBossRenderCamera";

	// Token: 0x04003B9B RID: 15259
	private const string keyFormat = "boss_{0}_{1}_";

	// Token: 0x04003B9C RID: 15260
	private const string ketherKeyFormat = "boss_{0}_{2}_{1}_";

	// Token: 0x04003B9D RID: 15261
	public static string[] keyValues = new string[]
	{
		"default",
		"overload1",
		"overload2",
		"overload3",
		"overload4",
		"overload5",
		"overload6",
		"overload7",
		"overload8",
		"overload9",
		"reward",
		"finish",
		"battle"
	};

	// Token: 0x04003B9E RID: 15262
	public static string[] ketherSaveRegions = new string[]
	{
		"e0",
		"e1",
		"e2",
		"e3",
		"e4"
	};

	// Token: 0x04003B9F RID: 15263
	private static SefiraBossManager _instance = null;

	// Token: 0x04003BA0 RID: 15264
	private const int keyTravelMax = 15;

	// Token: 0x04003BA1 RID: 15265
	public const int EndingStartDay = 45;

	// Token: 0x04003BA2 RID: 15266
	private int[] workId;

	// Token: 0x04003BA3 RID: 15267
	private SefiraEnum currentActivated = SefiraEnum.DUMMY;

	// Token: 0x04003BA4 RID: 15268
	private YesodBossCameraScript cameraScript;

	// Token: 0x04003BA5 RID: 15269
	private Dictionary<string, int> keyCountDic = new Dictionary<string, int>();

	// Token: 0x04003BA6 RID: 15270
	private SefiraBossBase _currentBossBase;

	// Token: 0x04003BA7 RID: 15271
	private int nextInstId = 1;

	// Token: 0x04003BA8 RID: 15272
	private bool _isCleared;

	// Token: 0x04003BA9 RID: 15273
	private Dictionary<int, string> _bossBgmDic = new Dictionary<int, string>();

	// Token: 0x04003BAA RID: 15274
	private bool _isRecoverBlocked;

	// Token: 0x04003BAB RID: 15275
	private bool _isWorkCancelable;

	// Token: 0x04003BAC RID: 15276
	private bool _tutorial;

	// Token: 0x04003BAD RID: 15277
	private bool _tutorialPlayed;

	// Token: 0x04003BAE RID: 15278
	private Dictionary<string, bool> _clearState = new Dictionary<string, bool>();
}
