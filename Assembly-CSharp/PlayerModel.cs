using System;
using System.Collections.Generic;
using GameStatusUI;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
[Serializable]
public class PlayerModel
{
	// Token: 0x0600384B RID: 14411 RVA: 0x000324B7 File Offset: 0x000306B7
	private PlayerModel()
	{
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x0600384C RID: 14412 RVA: 0x000324CA File Offset: 0x000306CA
	public bool memoryInit
	{
		get
		{
			return this._memoryInit;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x0600384D RID: 14413 RVA: 0x000324D2 File Offset: 0x000306D2
	public bool ketherGameOver
	{
		get
		{
			return this._ketherGameOver;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x0600384E RID: 14414 RVA: 0x000324DA File Offset: 0x000306DA
	public static PlayerModel instance
	{
		get
		{
			if (PlayerModel._instance == null)
			{
				PlayerModel._instance = new PlayerModel();
			}
			return PlayerModel._instance;
		}
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x000324F5 File Offset: 0x000306F5
	public void Init()
	{
		this._ketherGameOver = false;
		this._memoryInit = false;
		this.day = 0;
		this.addedCreature.Clear();
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x00032517 File Offset: 0x00030717
	public void InitAddingCreatures()
	{
		this.addedCreature.Clear();
	}

	// Token: 0x06003851 RID: 14417 RVA: 0x00032524 File Offset: 0x00030724
	public void AddWaitingCreature(long id)
	{
		this.addedCreature.Enqueue(id);
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x00032532 File Offset: 0x00030732
	public bool GetWaitingCreature(out long id)
	{
		id = -1L;
		if (this.addedCreature.Count == 0)
		{
			return false;
		}
		id = this.addedCreature.Dequeue();
		if (GlobalGameManager.instance.ExistEtcData())
		{
			GlobalGameManager.instance.SaveEtcData();
		}
		return true;
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x0016B80C File Offset: 0x00169A0C
	public List<long> CopyWaitingCreatures()
	{
		List<long> list = new List<long>();
		foreach (long item in this.addedCreature)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x0016B870 File Offset: 0x00169A70
	public bool IsWaitingCreatureExist()
	{
		if (this.day >= 20 && this.day < 25)
		{
			return this.addedCreature.Count >= 2;
		}
		if (this.day >= 45 && this.day < 50)
		{
			return this.addedCreature.Count >= 2;
		}
		return this.addedCreature.Count >= 1;
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00032571 File Offset: 0x00030771
	public bool IsWaitingCreature(long id)
	{
		return this.addedCreature.Contains(id);
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x0003257F File Offset: 0x0003077F
	public void Remember()
	{
		this._memoryInit = true;
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x00032588 File Offset: 0x00030788
	public void SetKetherGameOver()
	{
		this._ketherGameOver = true;
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x00032591 File Offset: 0x00030791
	public int GetOpenedAreaCount()
	{
		return SefiraManager.instance.GetOpendSefiraList().Length;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x0003259F File Offset: 0x0003079F
	public void SetDay(int day)
	{
		this.day = day;
		Notice.instance.Send(NoticeName.UpdateDay, new object[0]);
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x000325BD File Offset: 0x000307BD
	public void Nextday()
	{
		this.day++;
		GlobalGameManager.instance.lastLoaded = false;
		Notice.instance.Send(NoticeName.UpdateDay, new object[0]);
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x0016B8E8 File Offset: 0x00169AE8
	private void TempMakeCreature()
	{
		switch (this.day / 5)
		{
		case 0:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.MALKUT);
			break;
		case 1:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.YESOD);
			break;
		case 2:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.NETZACH);
			break;
		case 3:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.HOD);
			break;
		case 4:
		case 5:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.TIPERERTH1);
			break;
		case 6:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.GEBURAH);
			break;
		case 7:
			SefiraManager.instance.OpenSefiraWithCreature(SefiraEnum.CHESED);
			break;
		}
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x0016B99C File Offset: 0x00169B9C
	public void UnlimitMode(string saveVer)
	{
		for (int i = 0; i < 5; i++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.MALKUT);
		}
		for (int j = 0; j < 5; j++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.YESOD);
		}
		for (int k = 0; k < 5; k++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.NETZACH);
		}
		for (int l = 0; l < 5; l++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.HOD);
		}
		for (int m = 0; m < 5; m++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.TIPERERTH1);
		}
		for (int n = 0; n < 5; n++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.GEBURAH);
		}
		for (int num = 0; num < 5; num++)
		{
			SefiraManager.instance.OpenSefira(SefiraEnum.CHESED);
		}
		if (saveVer != "old")
		{
			for (int num2 = 0; num2 < 5; num2++)
			{
				SefiraManager.instance.OpenSefira(SefiraEnum.BINAH);
			}
			for (int num3 = 0; num3 < 5; num3++)
			{
				SefiraManager.instance.OpenSefira(SefiraEnum.CHOKHMAH);
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				SefiraManager.instance.OpenSefira(SefiraEnum.KETHER);
			}
		}
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x000325ED File Offset: 0x000307ED
	public int GetDay()
	{
		return this.day;
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x000325F5 File Offset: 0x000307F5
	public Sefira[] GetOpenedAreaList()
	{
		return SefiraManager.instance.GetOpendSefiraList();
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x0016BAF8 File Offset: 0x00169CF8
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"day",
				this.day
			}
		};
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x0016BB24 File Offset: 0x00169D24
	public void LoadData(Dictionary<string, object> dic)
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		this._memoryInit = false;
		this._ketherGameOver = false;
		GameUtil.TryGetValue<int>(dic, "day", ref this.day);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00032601 File Offset: 0x00030801
	// Note: this type is marked as 'beforefieldinit'.
	static PlayerModel()
	{
	}

	// Token: 0x04003381 RID: 13185
	private const int First = 10;

	// Token: 0x04003382 RID: 13186
	private const int Second = 20;

	// Token: 0x04003383 RID: 13187
	private const int Third = 30;

	// Token: 0x04003384 RID: 13188
	private const long nullcreature = 100005L;

	// Token: 0x04003385 RID: 13189
	private const long orchestra = 100019L;

	// Token: 0x04003386 RID: 13190
	public static PlayerModel.EmergencyController emergencyController = new PlayerModel.EmergencyController();

	// Token: 0x04003387 RID: 13191
	public Vector3 playerSpot;

	// Token: 0x04003388 RID: 13192
	private int day;

	// Token: 0x04003389 RID: 13193
	private bool _memoryInit;

	// Token: 0x0400338A RID: 13194
	private bool _ketherGameOver;

	// Token: 0x0400338B RID: 13195
	private static PlayerModel _instance;

	// Token: 0x0400338C RID: 13196
	public EmergencyLevel currentEmergencyLevel;

	// Token: 0x0400338D RID: 13197
	public Queue<long> addedCreature = new Queue<long>();

	// Token: 0x020006D5 RID: 1749
	public class EmergencyController
	{
		// Token: 0x06003862 RID: 14434 RVA: 0x0003260D File Offset: 0x0003080D
		public EmergencyController()
		{
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06003863 RID: 14435 RVA: 0x00032620 File Offset: 0x00030820
		// (set) Token: 0x06003864 RID: 14436 RVA: 0x00032628 File Offset: 0x00030828
		private float currentScore
		{
			get
			{
				return this._currentScore;
			}
			set
			{
				if (value > 100f)
				{
					this._currentScore = 100f;
				}
				else if (value < 0f)
				{
					this._currentScore = 0f;
				}
				else
				{
					this._currentScore = value;
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x00032667 File Offset: 0x00030867
		// (set) Token: 0x06003866 RID: 14438 RVA: 0x0016BB58 File Offset: 0x00169D58
		public EmergencyLevel currentLevel
		{
			get
			{
				return this._currentLevel;
			}
			set
			{
				if (this._currentLevel != value)
				{
					this._currentLevel = value;
					BgmManager.instance.SetBgm(this.currentLevel);
					GameStatusUI.GameStatusUI.Window.EmergencyActivate((int)value);
					EmergencyLevel currentLevel = this._currentLevel;
					if (currentLevel != EmergencyLevel.NORMAL)
					{
					}
				}
			}
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x0003266F File Offset: 0x0003086F
		public void Init()
		{
			this.currentScore = 0f;
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x0003267C File Offset: 0x0003087C
		public void OnStageRelease()
		{
			this.currentScore = 0f;
			this.emergecyID.Clear();
			this.currentLevel = EmergencyLevel.NORMAL;
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x0003269B File Offset: 0x0003089B
		public void OnStageEnd()
		{
			this._currentLevel = EmergencyLevel.NORMAL;
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0016BBB0 File Offset: 0x00169DB0
		public float GetLevelScore(RiskLevel level)
		{
			switch (level)
			{
			case RiskLevel.ZAYIN:
				return PlayerModel.EmergencyController.ZAYIN;
			case RiskLevel.TETH:
				return PlayerModel.EmergencyController.TETH;
			case RiskLevel.HE:
				return PlayerModel.EmergencyController.HE;
			case RiskLevel.WAW:
				return PlayerModel.EmergencyController.WAW;
			case RiskLevel.ALEPH:
				return PlayerModel.EmergencyController.ALEPH;
			default:
				return 0f;
			}
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000326A4 File Offset: 0x000308A4
		public void AddScore(CreatureModel model)
		{
			this.AddScore(this.GetScore(model));
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000326B3 File Offset: 0x000308B3
		public void AddScore(RiskLevel level)
		{
			this.AddScore(this.GetScore(level));
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x000326C2 File Offset: 0x000308C2
		public float GetScore(CreatureModel model)
		{
			return this.GetScore(CreatureTypeInfo.GetRiskLevelStringToEnum(model.metaInfo.riskLevelForce));
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0016BC00 File Offset: 0x00169E00
		public float GetScore(RiskLevel level)
		{
			float levelScore = this.GetLevelScore(level);
			int num = PlayerModel.instance.GetOpenedAreaList().Length;
			if (num == 0)
			{
				num = 1;
			}
			return levelScore / (float)num;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000326DA File Offset: 0x000308DA
		public void AddScore(float val)
		{
			this.currentScore += val;
			this.SetLevel();
			BgmManager.instance.BlockRecover();
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0016BC30 File Offset: 0x00169E30
		private void SetLevel()
		{
			int num = -1;
			for (int i = 0; i < PlayerModel.EmergencyController.Range.Length; i++)
			{
				int num2 = PlayerModel.EmergencyController.Range[i];
				if (this.currentScore <= (float)num2)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 3;
			}
			if (num != (int)this.currentLevel)
			{
				Notice.instance.Send(NoticeName.OnEmergencyLevelChanged, new object[]
				{
					num
				});
				Debug.Log(string.Concat(new object[]
				{
					"Emergency level changed from ",
					this.currentLevel,
					" to ",
					num
				}));
			}
			switch (num)
			{
			case 0:
				this.currentLevel = EmergencyLevel.NORMAL;
				break;
			case 1:
				this.currentLevel = EmergencyLevel.LEVEL1;
				break;
			case 2:
				this.currentLevel = EmergencyLevel.LEVEL2;
				break;
			case 3:
				this.currentLevel = EmergencyLevel.LEVEL3;
				break;
			default:
				return;
			}
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000326FA File Offset: 0x000308FA
		public void ReduceSore(float val)
		{
			this.currentScore -= val;
			this.Print();
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x00032710 File Offset: 0x00030910
		public float GetCurrentScore()
		{
			return this.currentScore;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x00032718 File Offset: 0x00030918
		public void SetCurrentScore(float val)
		{
			this.currentScore = val;
			this.SetLevel();
			if (val == 0f)
			{
				BgmManager.instance.ResetBgm();
			}
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x0003273C File Offset: 0x0003093C
		public void Print()
		{
			Debug.Log("Current Emergency Score : " + this.currentScore);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x00032758 File Offset: 0x00030958
		public void Apply(long id)
		{
			if (this.emergecyID.Contains(id))
			{
				return;
			}
			this.emergecyID.Add(id);
			GameManager.currentGameManager.emergency = false;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00032783 File Offset: 0x00030983
		public void Exit(long id)
		{
			if (this.emergecyID.Contains(id))
			{
				this.emergecyID.Remove(id);
			}
			if (this.emergecyID.Count == 0)
			{
				GameManager.currentGameManager.emergency = true;
			}
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x0016BD30 File Offset: 0x00169F30
		// Note: this type is marked as 'beforefieldinit'.
		static EmergencyController()
		{
		}

		// Token: 0x0400338E RID: 13198
		private float _currentScore;

		// Token: 0x0400338F RID: 13199
		public static float ZAYIN = 5f;

		// Token: 0x04003390 RID: 13200
		public static float TETH = 20f;

		// Token: 0x04003391 RID: 13201
		public static float HE = 40f;

		// Token: 0x04003392 RID: 13202
		public static float WAW = 60f;

		// Token: 0x04003393 RID: 13203
		public static float ALEPH = 75f;

		// Token: 0x04003394 RID: 13204
		public static int[] Range = new int[]
		{
			10,
			50,
			80,
			100
		};

		// Token: 0x04003395 RID: 13205
		private EmergencyLevel _currentLevel;

		// Token: 0x04003396 RID: 13206
		private List<long> emergecyID = new List<long>();
	}
}
