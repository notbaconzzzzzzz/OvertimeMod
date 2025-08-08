/*
public void OnGameInit() // 
public bool ActivateOrdeal(OrdealBase ordeal, bool remove = true) // 
+public List<OrdealBase> GetOrdealList() // 
+public void InitializeSpawnTimes() // 
+public int GetOrdealSpawnTime(OrdealLevel level) // 
+private int[] _ordealSpawnPhase // 
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class OrdealManager
{
	// Token: 0x06003C80 RID: 15488 RVA: 0x001794DC File Offset: 0x001776DC
	public OrdealManager()
	{
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06003C81 RID: 15489 RVA: 0x0003522E File Offset: 0x0003342E
	public static OrdealManager instance
	{
		get
		{
			if (OrdealManager._instance == null)
			{
				OrdealManager._instance = new OrdealManager();
			}
			return OrdealManager._instance;
		}
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x001795A0 File Offset: 0x001777A0
	public void OnGameInit()
	{ // <Mod>
		this.ordealCreatureList = new List<OrdealCreatureModel>();
		int day = PlayerModel.instance.GetDay();
		this.InitAvailableFixers();
		this._currentOrdealLevel = 1;
		this._elapsedTime = 0f;
		this._ordealList.Clear();
		foreach (OrdealBase item in OrdealGenInfo.GenerateOrdeals(day))
		{
			this._ordealList.Add(item);
		}
		_secondaryOrdealList.Clear();
		if (SpecialModeConfig.instance.GetValue<bool>("SecondaryQliphothOverload"))
		{
			foreach (OrdealBase item in OrdealGenInfo.GenerateSecondaryOrdeals(day, _ordealList))
			{
				_secondaryOrdealList.Add(item);
			}
		}
		InitializeSpawnTimes();
		this.UpdateOrdealUI();
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x00035249 File Offset: 0x00033449
	public void InitAvailableFixers()
	{
		this.availableFixers.Clear();
		this.availableFixers.Add(RwbpType.R);
		this.availableFixers.Add(RwbpType.W);
		this.availableFixers.Add(RwbpType.B);
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x00179640 File Offset: 0x00177840
	public OrdealLevel GetMaxOrdealLevel()
	{
		OrdealLevel ordealLevel = OrdealLevel.DAWN;
		if (SpecialModeConfig.instance.GetValue<bool>("EarlyOvertimeOrdeals"))
		{
			foreach (OrdealBase ordealBase in this._ordealList)
			{
				if ((int)ordealBase.level - 5 == (int)ordealLevel)
				{
					ordealLevel++;
				}
				else
				{
					if (ordealLevel != OrdealLevel.DAWN)
					{
						break;
					}
				}
			}
		}
		else
		{
			foreach (OrdealBase ordealBase in this._ordealList)
			{
				if (ordealBase.level < OrdealLevel.OVERTIME_DAWN && ordealLevel < ordealBase.level)
				{
					ordealLevel = ordealBase.level;
				}
			}
		}
		return ordealLevel;
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x0000403D File Offset: 0x0000223D
	private void UpdateOrdealUI()
	{
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x001796AC File Offset: 0x001778AC
	public void ClearCreatures()
	{
		foreach (OrdealCreatureModel ordealCreatureModel in this.ordealCreatureList)
		{
			ordealCreatureModel.OnDestroy();
		}
		this.ordealCreatureList.Clear();
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x0000403D File Offset: 0x0000223D
	public void OnStageStart()
	{
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x0003527A File Offset: 0x0003347A
	public void OnStageRelease()
	{
		this.ClearCreatures();
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x00179714 File Offset: 0x00177914
	public bool ActivateOrdeal(OrdealBase ordeal, bool remove = true)
	{ // <Mod>
		if (!ordeal.IsStartable())
		{
			return false;
		}
		try
		{
			if (remove || !_activatedOrdeals.Contains(ordeal))
			{
				ordeal.isStarted = true;
			}
			ordeal.OnOrdealStart();
			Notice.instance.Send(NoticeName.OnOrdealStarted, new object[]
			{
				ordeal
			});
			this._activatedOrdeals.Add(ordeal);
			if (remove)
			{
				this._ordealList.Remove(ordeal);
				_secondaryOrdealList.Remove(ordeal);
			}
			Debug.Log(string.Format("Ordeal : {0} Activated", ordeal.level));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return false;
		}
		return true;
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x001797BC File Offset: 0x001779BC
	public void OnFixedUpdate()
	{
		foreach (OrdealCreatureModel ordealCreatureModel in this.ordealCreatureList)
		{
			ordealCreatureModel.OnFixedUpdate();
		}
		foreach (OrdealBase ordealBase in this._activatedOrdeals)
		{
			ordealBase.FixedUpdate();
		}
		foreach (OrdealBase item in this._removedOrdeals)
		{
			this._activatedOrdeals.Remove(item);
		}
		this._removedOrdeals = new List<OrdealBase>();
		this.UpdateOrdealUI();
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x001798C8 File Offset: 0x00177AC8
	public OrdealCreatureModel AddCreature(long metadataId, MapNode pos, OrdealBase ordealBase)
	{ // <Patch>
		return AddCreature_Mod(new LobotomyBaseMod.LcIdLong(metadataId), pos, ordealBase);
		/*
		OrdealCreatureModel ordealCreatureModel = new OrdealCreatureModel((long)this.nextInstId++);
		this.BuildCreature(ordealCreatureModel, metadataId);
		ordealCreatureModel.GetMovableNode().SetCurrentNode(pos);
		ordealCreatureModel.GetMovableNode().SetActive(true);
		ordealCreatureModel.baseMaxHp = ordealCreatureModel.metaInfo.maxHp;
		ordealCreatureModel.hp = (float)ordealCreatureModel.metaInfo.maxHp;
		ordealCreatureModel.SetOrdealBase(ordealBase);
		this.ordealCreatureList.Add(ordealCreatureModel);
		Notice.instance.Send(NoticeName.AddOrdealCreature, new object[]
		{
			ordealCreatureModel
		});
		ordealCreatureModel.script.OnInit();
		Sefira sefira = SefiraManager.instance.GetSefira(pos.GetAttachedPassage().GetSefiraName());
		ordealCreatureModel.sefira = sefira;
		ordealCreatureModel.sefiraNum = sefira.indexString;
		return ordealCreatureModel;*/
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00179994 File Offset: 0x00177B94
	private void BuildCreature(OrdealCreatureModel model, long metadataId)
	{ // <Patch>
		BuildCreature_Mod(model, new LobotomyBaseMod.LcIdLong(metadataId));
		/*
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(metadataId);
		object obj = null;
		foreach (Assembly assembly in Add_On.instance.AssemList)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == data.script)
				{
					obj = Activator.CreateInstance(type);
				}
			}
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(Type.GetType(data.script));
		}
		model.script = (CreatureBase)obj;
		model.observeInfo = new CreatureObserveInfoModel(metadataId);
		string text = "1";
		model.sefira = SefiraManager.instance.GetSefira(text);
		model.sefiraNum = text;
		model.metadataId = metadataId;
		model.metaInfo = data;
		if (CreatureTypeList.instance.GetSkillTipData(metadataId) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(metadataId).GetCopy();
		}
		model.script.SetModel(model);
		model.script.OnInitialBuild();*/
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x0000403D File Offset: 0x0000223D
	private void OnAddCreatureWorkCount(OrdealBase ordeal)
	{
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x0000403D File Offset: 0x0000223D
	public void OnAddCreatureWorkCount()
	{
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00035282 File Offset: 0x00033482
	public void OnOrdealEnd(OrdealBase ordeal, bool b)
	{
		Notice.instance.Send(NoticeName.OrdealEnd, new object[]
		{
			ordeal,
			b
		});
		this._removedOrdeals.Add(ordeal);
		this.UpdateOrdealUI();
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x000352B8 File Offset: 0x000334B8
	public OrdealCreatureModel[] GetOrdealCreatureList()
	{
		return this.ordealCreatureList.ToArray();
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x000352C5 File Offset: 0x000334C5
	public List<OrdealBase> GetActivatedOrdeals()
	{
		return this._activatedOrdeals;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00179AC8 File Offset: 0x00177CC8
	public bool CheckOrdealContains(OrdealLevel level, out OrdealBase ordeal)
	{
		ordeal = null;
		if (SpecialModeConfig.instance.GetValue<bool>("EarlyOvertimeOrdeals"))
		{
			foreach (OrdealBase ordealBase in this._ordealList)
			{
				if ((int)ordealBase.level - 4 == (int)level)
				{
					ordeal = ordealBase;
					break;
				}
			}
		}
		else
		{
			foreach (OrdealBase ordealBase in this._ordealList)
			{
				if (ordealBase.level == level)
				{
					ordeal = ordealBase;
					break;
				}
			}
		}
		return ordeal != null;
	}

	// <Patch>
	private void BuildCreature_Mod(OrdealCreatureModel model, LobotomyBaseMod.LcIdLong metadataId)
	{
		CreatureTypeInfo data_Mod = CreatureTypeList.instance.GetData_Mod(metadataId);
		object obj = LobotomyBaseMod.ExtenionUtil.GetTypeInstance<CreatureBase>(data_Mod.script);
		if (obj == null)
		{
			obj = Activator.CreateInstance(Type.GetType(data_Mod.script));
		}
		model.script = (CreatureBase)obj;
		model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
		model.observeInfo.InitData_Mod(metadataId);
		string text = "1";
		model.sefira = SefiraManager.instance.GetSefira(text);
		model.sefiraNum = text;
		model.metadataId = metadataId.id;
		model.metaInfo = data_Mod;
		if (CreatureTypeList.instance.GetSkillTipData_Mod(metadataId) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId).GetCopy();
		}
		model.script.SetModel(model);
		model.script.OnInitialBuild();
	}

	// <Patch>
	public OrdealCreatureModel AddCreature_Mod(LobotomyBaseMod.LcIdLong metadataId, MapNode pos, OrdealBase ordealBase)
	{
		int num = this.nextInstId;
		this.nextInstId = num + 1;
		OrdealCreatureModel ordealCreatureModel = new OrdealCreatureModel((long)num);
		this.BuildCreature_Mod(ordealCreatureModel, metadataId);
		ordealCreatureModel.GetMovableNode().SetCurrentNode(pos);
		ordealCreatureModel.GetMovableNode().SetActive(true);
		ordealCreatureModel.baseMaxHp = ordealCreatureModel.metaInfo.maxHp;
		ordealCreatureModel.hp = (float)ordealCreatureModel.metaInfo.maxHp;
		ordealCreatureModel.SetOrdealBase(ordealBase);
		this.ordealCreatureList.Add(ordealCreatureModel);
		Notice.instance.Send(NoticeName.AddOrdealCreature, new object[]
		{
			ordealCreatureModel
		});
		ordealCreatureModel.script.OnInit();
		Sefira sefira = SefiraManager.instance.GetSefira(pos.GetAttachedPassage().GetSefiraName());
		ordealCreatureModel.sefira = sefira;
		ordealCreatureModel.sefiraNum = sefira.indexString;
		return ordealCreatureModel;
	}

    //> <Mod>
	public bool CheckSecondaryOrdealContains(OrdealLevel level, out OrdealBase ordeal)
	{
		ordeal = null;
		foreach (OrdealBase ordealBase in this._secondaryOrdealList)
		{
			if (ordealBase.level == level)
			{
				ordeal = ordealBase;
				break;
			}
		}
		return ordeal != null;
	}

    public List<OrdealBase> GetOrdealList()
    {
        return _ordealList;
    }

    public List<OrdealBase> GetSecondaryOrdealList()
    {
        return _secondaryOrdealList;
    }

	public void InitializeSpawnTimes()
	{
		_ordealSpawnTime[0] = 2;
		_ordealSpawnTime[1] = UnityEngine.Random.Range(3, 6);
		_ordealSpawnTime[2] = UnityEngine.Random.Range(6, 8);
		_ordealSpawnTime[3] = 8;
		_ordealSpawnTime[4] = 12;
		_ordealSpawnTime[5] = UnityEngine.Random.Range(13, 16);
		_ordealSpawnTime[6] = UnityEngine.Random.Range(16, 18);
		_ordealSpawnTime[7] = 18;
	}

	public int GetOrdealSpawnTime(OrdealLevel level)
	{
		return _ordealSpawnTime[(int)level];
	}

	public void AdjustSpawnTimeForSefiraBoss(SefiraBossBase sefiraBoss)
	{
		sefiraBoss.AdjustOrdealSpawnTime(_ordealSpawnTime);
	}
	//< <Mod>

	// Token: 0x0400373F RID: 14143
	private static OrdealManager _instance;

	// Token: 0x04003740 RID: 14144
	private int _currentOrdealLevel = 1;

	// Token: 0x04003741 RID: 14145
	private readonly int[] ordealTimer1 = new int[]
	{
		60,
		60,
		8,
		10,
		13,
		10,
		11,
		13,
		15,
		18,
		15,
		17,
		19,
		22,
		25,
		22,
		24,
		27,
		30,
		34
	};

	// Token: 0x04003742 RID: 14146
	private readonly int[] ordealTimer2 = new int[]
	{
		60,
		60,
		8,
		10,
		13,
		10,
		11,
		13,
		15,
		18,
		15,
		17,
		19,
		22,
		25,
		22,
		24,
		27,
		30,
		34
	};

	// Token: 0x04003743 RID: 14147
	private readonly int[] ordealTimer = new int[]
	{
		60,
		60,
		8,
		10,
		13,
		10,
		11,
		13,
		15,
		18,
		15,
		17,
		19,
		22,
		25,
		22,
		24,
		27,
		30,
		34
	};

	// Token: 0x04003744 RID: 14148
	private float _elapsedTime;

	// Token: 0x04003745 RID: 14149
	private float _remainTime;

	// Token: 0x04003746 RID: 14150
	private Queue<OrdealBase> _ordealQueue = new Queue<OrdealBase>();

	// Token: 0x04003747 RID: 14151
	private List<OrdealBase> _ordealList = new List<OrdealBase>();

	// Token: 0x04003748 RID: 14152
	private List<OrdealBase> _activatedOrdeals = new List<OrdealBase>();

	// Token: 0x04003749 RID: 14153
	private List<OrdealBase> _removedOrdeals = new List<OrdealBase>();

	// <Mod>
	private List<OrdealBase> _secondaryOrdealList = new List<OrdealBase>();

	// <Mod>
	private int[] _ordealSpawnTime = new int[]
	{
		2,
		3,
		6,
		8,
		12,
		13,
		16,
		18
	};

	// Token: 0x0400374A RID: 14154
	private readonly int[] needWorkCount = new int[]
	{
		7,
		7,
		8,
		10,
		13,
		10,
		11,
		13,
		15,
		18,
		15,
		17,
		19,
		22,
		25,
		22,
		24,
		27,
		30,
		34
	};

	// Token: 0x0400374B RID: 14155
	private List<OrdealCreatureModel> ordealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x0400374C RID: 14156
	public List<RwbpType> availableFixers = new List<RwbpType>();

	// Token: 0x0400374D RID: 14157
	private int nextInstId = 1;
}
