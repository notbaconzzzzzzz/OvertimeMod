using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000880 RID: 2176
public class SefiraManager : IObserver
{
	// Token: 0x060042E7 RID: 17127 RVA: 0x0019901C File Offset: 0x0019721C
	private SefiraManager()
	{
		this.Init();
		Notice.instance.Observe(NoticeName.Update, this);
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x060042E8 RID: 17128 RVA: 0x00038EDC File Offset: 0x000370DC
	public static SefiraManager instance
	{
		get
		{
			if (SefiraManager._instance == null)
			{
				SefiraManager._instance = new SefiraManager();
			}
			return SefiraManager._instance;
		}
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x00199068 File Offset: 0x00197268
	public bool GameOverCheck()
	{
		bool result = true;
		foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
		{
			if (!sefira.IsSefiraClosed())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x001990A8 File Offset: 0x001972A8
	private void Init()
	{
		this.sefiraList = new List<Sefira>();
		this.activatedSefira = new List<Sefira>();
		this._sefiraDic = new Dictionary<SefiraEnum, Sefira>();
		Sefira[] array = new Sefira[13];
		array[0] = new Sefira("Malkut", 1, "1", SefiraEnum.MALKUT);
		array[3] = new Sefira("Netzach", 2, "2", SefiraEnum.NETZACH);
		array[2] = new Sefira("Hod", 3, "3", SefiraEnum.HOD);
		array[1] = new Sefira("Yesod", 4, "4", SefiraEnum.YESOD);
		array[4] = new Sefira("Tiphereth1", 5, "5", SefiraEnum.TIPERERTH1);
		array[5] = new Sefira("Tiphereth2", 6, "6", SefiraEnum.TIPERERTH2);
		array[6] = new Sefira("Geburah", 7, "7", SefiraEnum.GEBURAH);
		array[7] = new Sefira("Chesed", 8, "8", SefiraEnum.CHESED);
		array[8] = new Sefira("Binah", 9, "9", SefiraEnum.BINAH);
		array[9] = new Sefira("Chokhmah", 10, "10", SefiraEnum.CHOKHMAH);
		array[10] = new Sefira("Kether", 11, "11", SefiraEnum.KETHER);
		array[11] = new Sefira("Daat", 12, "12", SefiraEnum.DAAT);
		array[12] = new Sefira("Dummy", 13, "13", SefiraEnum.DUMMY);
		for (int i = 0; i < this.sefiraIndexMax; i++)
		{
			this.sefiraList.Add(array[i]);
			this._sefiraDic.Add(array[i].sefiraEnum, array[i]);
		}
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x00038EF7 File Offset: 0x000370F7
	public void Clear()
	{
		this.Init();
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x0019922C File Offset: 0x0019742C
	public void ClearUnitData()
	{
		for (int i = 0; i < this.sefiraIndexMax; i++)
		{
			this.sefiraList[i].ClearAgent();
			this.sefiraList[i].ClearCreature();
			this.sefiraList[i].ClearOfficer();
		}
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x00199284 File Offset: 0x00197484
	public void ClearOfficer()
	{
		for (int i = 0; i < this.sefiraIndexMax; i++)
		{
			this.sefiraList[i].ClearOfficer();
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x001992BC File Offset: 0x001974BC
	public void ResetPassageData()
	{
		for (int i = 0; i < this.sefiraIndexMax; i++)
		{
			this.sefiraList[i].ResetPassageData();
		}
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x001992F4 File Offset: 0x001974F4
	public void LoadData(Dictionary<string, object> dic)
	{
		foreach (KeyValuePair<string, object> keyValuePair in dic)
		{
			Sefira sefira = this.GetSefira(keyValuePair.Key);
			if (sefira != null)
			{
				Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
				if (dictionary != null)
				{
					sefira.LoadData(dictionary);
					for (int i = 1; i <= sefira.openLevel; i++)
					{
						MapGraph.instance.ActivateArea(sefira.indexString, i.ToString());
						SefiraCharacterManager.instance.OnOpenSefira(sefira.sefiraEnum);
					}
				}
			}
		}
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x001993BC File Offset: 0x001975BC
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"Malkut",
				this.GetSefira(SefiraEnum.MALKUT).GetSaveData()
			},
			{
				"Netzach",
				this.GetSefira(SefiraEnum.NETZACH).GetSaveData()
			},
			{
				"Hod",
				this.GetSefira(SefiraEnum.HOD).GetSaveData()
			},
			{
				"Yesod",
				this.GetSefira(SefiraEnum.YESOD).GetSaveData()
			},
			{
				"Tiphereth1",
				this.GetSefira(SefiraEnum.TIPERERTH1).GetSaveData()
			},
			{
				"Tiphereth2",
				this.GetSefira(SefiraEnum.TIPERERTH2).GetSaveData()
			},
			{
				"Geburah",
				this.GetSefira(SefiraEnum.GEBURAH).GetSaveData()
			},
			{
				"Chesed",
				this.GetSefira(SefiraEnum.CHESED).GetSaveData()
			},
			{
				"Binah",
				this.GetSefira(SefiraEnum.BINAH).GetSaveData()
			},
			{
				"Chokhmah",
				this.GetSefira(SefiraEnum.CHOKHMAH).GetSaveData()
			},
			{
				"Kether",
				this.GetSefira(SefiraEnum.KETHER).GetSaveData()
			},
			{
				"Daat",
				this.GetSefira(SefiraEnum.DAAT).GetSaveData()
			}
		};
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x001994E8 File Offset: 0x001976E8
	public Sefira GetSefira(int index)
	{
		if (index > this.sefiraIndexMax || index < 0)
		{
			Debug.Log("out of sefira index");
			return null;
		}
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.index.Equals(index))
			{
				return sefira;
			}
		}
		return null;
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x00199578 File Offset: 0x00197778
	public Sefira[] GetActivatedSefiras()
	{
		List<Sefira> list = new List<Sefira>();
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.activated)
			{
				list.Add(sefira);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x001995EC File Offset: 0x001977EC
	public List<CreatureModel> GetEscapedCreatures()
	{
		List<CreatureModel> list = new List<CreatureModel>();
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.activated)
			{
				List<CreatureModel> escapedCreatures = sefira.GetEscapedCreatures();
				list.AddRange(escapedCreatures);
			}
		}
		List<CreatureModel> list2 = new List<CreatureModel>();
		foreach (CreatureModel creatureModel in list)
		{
			if (!creatureModel.IsEscaped())
			{
				list2.Add(creatureModel);
			}
		}
		foreach (CreatureModel item in list2)
		{
			list.Remove(item);
		}
		return list;
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x00199710 File Offset: 0x00197910
	public Sefira GetSefira(string str)
	{
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.indexString.Equals(str) || sefira.name.Equals(str))
			{
				return sefira;
			}
		}
		return null;
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x00199794 File Offset: 0x00197994
	public int GetSefiraOpenLevel(SefiraEnum sefiraEnum)
	{
		Sefira sefira = SefiraManager.instance.GetSefira(sefiraEnum);
		return sefira.openLevel;
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x001997B4 File Offset: 0x001979B4
	public bool IsOpened(SefiraEnum sefiraEnum)
	{
		Sefira sefira = SefiraManager.instance.GetSefira(sefiraEnum);
		return sefira.activated;
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x001997D4 File Offset: 0x001979D4
	public bool IsOpened(string str)
	{
		Sefira sefira = SefiraManager.instance.GetSefira(str);
		return sefira.activated;
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x00199578 File Offset: 0x00197778
	public Sefira[] GetOpendSefiraList()
	{
		List<Sefira> list = new List<Sefira>();
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.activated)
			{
				list.Add(sefira);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060042F9 RID: 17145 RVA: 0x00038EFF File Offset: 0x000370FF
	public void AddActivatedSefira(Sefira sefira)
	{
		if (!this.activatedSefira.Contains(sefira))
		{
			this.activatedSefira.Add(sefira);
			Notice.instance.Send(NoticeName.SefiraEnabled, new object[]
			{
				sefira
			});
		}
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x00038F37 File Offset: 0x00037137
	public void DisabledSefira(Sefira sefira)
	{
		if (this.activatedSefira.Contains(sefira))
		{
			this.activatedSefira.Remove(sefira);
			Notice.instance.Send(NoticeName.SefiraDisabled, new object[]
			{
				sefira
			});
		}
	}

	// Token: 0x060042FB RID: 17147 RVA: 0x001997F4 File Offset: 0x001979F4
	public void OnStageStart_first()
	{
		foreach (Sefira sefira in this.sefiraList)
		{
			sefira.OnStageStart_first();
			if (sefira.activated)
			{
				this.AddActivatedSefira(sefira);
			}
		}
	}

	// Token: 0x060042FC RID: 17148 RVA: 0x00038F70 File Offset: 0x00037170
	public void SefiraIsolateLoad(Dictionary<string, SefiraEnum> table)
	{
		this.isLoadedSefiaIsolateData = true;
		this._GenNodeSefiraTable = table;
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x00199864 File Offset: 0x00197A64
	public bool GetSefiraByGenNodeId(string id, out Sefira sefira)
	{
		SefiraEnum sefira2 = SefiraEnum.DUMMY;
		bool flag = this._GenNodeSefiraTable.TryGetValue(id, out sefira2);
		if (!flag)
		{
			sefira = null;
		}
		else
		{
			sefira = this.GetSefira(sefira2);
		}
		return flag;
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x0019989C File Offset: 0x00197A9C
	public Sefira GetSefira(SefiraEnum sefira)
	{
		Sefira result;
		if (this._sefiraDic.TryGetValue(sefira, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x001998C0 File Offset: 0x00197AC0
	public bool StartValidateCheck(ref Sefira notallocated)
	{
		foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
		{
			if (sefira.sefiraEnum != SefiraEnum.DAAT)
			{
				if (!SefiraBossManager.Instance.CheckBossActivation(sefira.sefiraEnum) || sefira.sefiraEnum == SefiraEnum.KETHER)
				{
					if (sefira.agentList.Count == 0)
					{
						notallocated = sefira;
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06004300 RID: 17152 RVA: 0x00199940 File Offset: 0x00197B40
	public void OpenSefira(SefiraEnum sefiraEnum)
	{
		Sefira sefira = this.GetSefira(sefiraEnum);
		if (!sefira.activated)
		{
			sefira.Activate();
			SefiraCharacterManager.instance.OnOpenSefira(sefira.sefiraEnum);
			Notice.instance.Send(NoticeName.OpenArea, new object[]
			{
				sefira
			});
		}
		sefira.AddOpenLevel();
		if (sefiraEnum == SefiraEnum.TIPERERTH1 && sefira.openLevel >= 3)
		{
			Sefira sefira2 = this.GetSefira(SefiraEnum.TIPERERTH2);
			if (!sefira2.activated)
			{
				sefira2.Activate();
				MapGraph.instance.ActivateArea(sefira2.indexString, "1");
				Notice.instance.Send(NoticeName.OpenArea, new object[]
				{
					sefira2
				});
			}
			sefira2.SetOpenLevel(sefira.openLevel);
		}
		else if (sefiraEnum == SefiraEnum.KETHER && sefira.openLevel >= 5)
		{
			Sefira sefira3 = this.GetSefira(SefiraEnum.DAAT);
			if (!sefira3.activated)
			{
				sefira3.Activate();
				MapGraph.instance.ActivateArea(sefira3.indexString, "1");
				Notice.instance.Send(NoticeName.OpenArea, new object[]
				{
					sefira3
				});
				sefira3.SetOpenLevel(1);
			}
		}
		MapGraph.instance.ActivateArea(sefira.indexString, sefira.openLevel.ToString());
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x0000403D File Offset: 0x0000223D
	public void OpenSefiraWithCreatureDebug(SefiraEnum sefiraEnum)
	{
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x00199A88 File Offset: 0x00197C88
	public void OpenSefiraWithCreature(SefiraEnum sefiraEnum)
	{
		Sefira sefira = this.GetSefira(sefiraEnum);
		if (!sefira.activated)
		{
			sefira.Activate();
			SefiraCharacterManager.instance.OnOpenSefira(sefira.sefiraEnum);
			Notice.instance.Send(NoticeName.OpenArea, new object[]
			{
				sefira
			});
		}
		sefira.AddOpenLevel();
		if (sefiraEnum == SefiraEnum.TIPERERTH1 && sefira.openLevel >= 3)
		{
			Sefira sefira2 = this.GetSefira(SefiraEnum.TIPERERTH2);
			if (!sefira2.activated)
			{
				sefira2.Activate();
				MapGraph.instance.ActivateArea(sefira2.indexString, "1");
				Notice.instance.Send(NoticeName.OpenArea, new object[]
				{
					sefira2
				});
			}
			sefira2.SetOpenLevel(sefira.openLevel);
			MapGraph.instance.ActivateArea(sefira2.indexString, "1");
		}
		else if (sefiraEnum == SefiraEnum.KETHER && sefira.openLevel >= 5)
		{
			Sefira sefira3 = this.GetSefira(SefiraEnum.DAAT);
			if (!sefira3.activated)
			{
				sefira3.Activate();
				MapGraph.instance.ActivateArea(sefira3.indexString, "1");
				Notice.instance.Send(NoticeName.OpenArea, new object[]
				{
					sefira3
				});
				sefira3.SetOpenLevel(1);
			}
		}
		MapGraph.instance.ActivateArea(sefira.indexString, sefira.openLevel.ToString());
		if (sefira.openLevel <= 4)
		{
			if (sefiraEnum == SefiraEnum.TIPERERTH1)
			{
				if (sefira.openLevel <= 2)
				{
					this.AddCreature(this.GetCreatureGenerationList(sefira.openLevel), sefira);
				}
				else
				{
					Sefira sefira4 = this.GetSefira(SefiraEnum.TIPERERTH2);
					this.AddCreature(this.GetCreatureGenerationList(sefira.openLevel), sefira4);
				}
			}
			else if (sefiraEnum == SefiraEnum.KETHER)
			{
				this.AddCreature(this.GetCreatureGenerationList(sefira.openLevel), sefira);
			}
			else if (sefiraEnum != SefiraEnum.DAAT)
			{
				this.AddCreature(this.GetCreatureGenerationList(sefira.openLevel), sefira);
			}
		}
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x00199C80 File Offset: 0x00197E80
	public void MakeTutorialCreature()
	{
		Sefira sefira = this.GetSefira(SefiraEnum.MALKUT);
		if (!sefira.activated)
		{
			sefira.Activate();
			SefiraCharacterManager.instance.OnOpenSefira(sefira.sefiraEnum);
			Notice.instance.Send(NoticeName.OpenArea, new object[]
			{
				sefira
			});
		}
		sefira.AddOpenLevel();
		MapGraph.instance.ActivateArea(sefira.indexString, sefira.openLevel.ToString());
		long[] creatureIdAry = new long[]
		{
			100000L
		};
		SefiraIsolate[] array = sefira.isolateManagement.GenIsolateByCreatureAryByOrder(creatureIdAry);
		foreach (SefiraIsolate sefiraIsolate in array)
		{
			CreatureManager.instance.AddCreature(sefiraIsolate.creatureId, sefiraIsolate, sefira.indexString);
		}
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x00199D54 File Offset: 0x00197F54
	private long[] GetCreatureGenerationList(SefiraEnum sefiraEnum, int openLevel)
	{
		if (openLevel > 4)
		{
			return new long[0];
		}
		switch (sefiraEnum)
		{
		case SefiraEnum.MALKUT:
			return CreatureGenerateInfo.r1[openLevel - 1];
		case SefiraEnum.YESOD:
			return CreatureGenerateInfo.r2[openLevel - 1];
		case SefiraEnum.HOD:
		case SefiraEnum.NETZACH:
			return CreatureGenerateInfo.r3[openLevel - 1];
		case SefiraEnum.TIPERERTH1:
		case SefiraEnum.TIPERERTH2:
			return CreatureGenerateInfo.r4[openLevel - 1];
		case SefiraEnum.GEBURAH:
		case SefiraEnum.CHESED:
			return CreatureGenerateInfo.r5[openLevel - 1];
		case SefiraEnum.BINAH:
		case SefiraEnum.CHOKHMAH:
		case SefiraEnum.KETHER:
		case SefiraEnum.DAAT:
			return CreatureGenerateInfo.r6[openLevel - 1];
		default:
			return new long[0];
		}
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x00199DEC File Offset: 0x00197FEC
	private long[] GetCreatureGenerationList(int openLevel)
	{
		if (openLevel > 4)
		{
			return new long[0];
		}
		long item = -1L;
		List<long> list = new List<long>();
		while (PlayerModel.instance.GetWaitingCreature(out item))
		{
			list.Add(item);
		}
		return list.ToArray();
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x00199E34 File Offset: 0x00198034
	private CreatureModel AddCreature(long metadataId, SefiraIsolate sefiraIsolateData, string sefiraNum)
	{
		return CreatureManager.instance.AddCreature(metadataId, sefiraIsolateData, sefiraNum);
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x00199E50 File Offset: 0x00198050
	private void AddCreature_Debug(SefiraEnum sefiraEnum)
	{
		Sefira sefira = this.GetSefira(sefiraEnum);
		List<long> list = new List<long>(CreatureGenerateInfo.GetAll(false));
		List<long> list2 = new List<long>();
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			list.Remove(creatureModel.metadataId);
		}
		foreach (long item in this.GetCreatureGenerationList(sefiraEnum, sefira.openLevel))
		{
			if (list.Contains(item))
			{
				list2.Add(item);
			}
		}
		if (list2.Count == 0)
		{
			foreach (long item2 in CreatureGenerateInfo.all)
			{
				if (list.Contains(item2))
				{
					list2.Add(item2);
				}
			}
		}
		SefiraIsolate notUsed = sefira.isolateManagement.GetNotUsed();
		notUsed.creatureId = list2[UnityEngine.Random.Range(0, list2.Count)];
		CreatureManager.instance.AddCreature(notUsed.creatureId, notUsed, sefira.indexString);
	}

	// Token: 0x06004308 RID: 17160 RVA: 0x00199F7C File Offset: 0x0019817C
	private void AddCreature(long[] list, Sefira sefira)
	{
		if (list.Length == 0)
		{
			return;
		}
		List<long> list2 = new List<long>(CreatureGenerateInfo.GetAll(false));
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			list2.Remove(creatureModel.metadataId);
		}
		List<long> list3 = new List<long>();
		foreach (long item in list)
		{
			if (list2.Contains(item))
			{
				list3.Add(item);
			}
		}
		if (list3.Count == 0)
		{
			return;
		}
		List<long> list4 = new List<long>();
		list4 = list3;
		SefiraIsolate[] array = sefira.isolateManagement.GenIsolateByCreatureAryByOrder(list4.ToArray());
		foreach (SefiraIsolate sefiraIsolate in array)
		{
			CreatureManager.instance.AddCreature(sefiraIsolate.creatureId, sefiraIsolate, sefira.indexString);
		}
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x0019A078 File Offset: 0x00198278
	public int GetOfficerAliveLevel(SefiraEnum sefira)
	{
		Sefira sefira2 = this.GetSefira(sefira);
		if (sefira2 == null)
		{
			return 0;
		}
		return sefira2.GetOfficerAliveLevel();
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x00038F80 File Offset: 0x00037180
	private void CheckGameState()
	{
		if (this.GameOverCheck() && !PlaySpeedSettingUI.instance.available)
		{
			PlaySpeedSettingUI.instance.ForcleyReleaseSetting();
		}
	}

	// Token: 0x0600430B RID: 17163 RVA: 0x0019A09C File Offset: 0x0019829C
	private void OnFixedUpdate()
	{
		foreach (Sefira sefira in this.sefiraList)
		{
			sefira.OnFixedUpdate();
		}
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x0019A0F8 File Offset: 0x001982F8
	public void OnNotice(string notice, params object[] param)
	{
		if (GameManager.currentGameManager != null)
		{
			if (notice == NoticeName.Update)
			{
				this.CheckGameState();
			}
			else if (notice == NoticeName.FixedUpdate)
			{
				this.OnFixedUpdate();
			}
		}
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x0019A148 File Offset: 0x00198348
	public Sprite LoadSefiraSprite(SefiraEnum targetSefira)
	{
		string name = this.GetSefira(targetSefira).name;
		string path = "Sprites/Sefira/Character/" + name.ToLower() + "_portrait";
		return Resources.Load<Sprite>(path);
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x0019A184 File Offset: 0x00198384
	public bool CheckEscapedState()
	{
		bool result = true;
		foreach (Sefira sefira in this.sefiraList)
		{
			if (sefira.activated)
			{
				if (!sefira.CheckEscapedCreature())
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x00038FA6 File Offset: 0x000371A6
	public SefiraLevel GetSefiraLevel(SefiraEnum sefira)
	{
		switch (sefira)
		{
		case SefiraEnum.MALKUT:
		case SefiraEnum.YESOD:
		case SefiraEnum.HOD:
		case SefiraEnum.NETZACH:
			return SefiraLevel.UP;
		case SefiraEnum.TIPERERTH1:
		case SefiraEnum.TIPERERTH2:
		case SefiraEnum.GEBURAH:
		case SefiraEnum.CHESED:
			return SefiraLevel.MIDDILE;
		default:
			return SefiraLevel.DOWN;
		}
	}

	// Token: 0x04003DBE RID: 15806
	public const string SefiraCharacterSpritePrefix = "Sprites/Sefira/Character/";

	// Token: 0x04003DBF RID: 15807
	public const string SefiraCharacterSpritePosfix = "_portrait";

	// Token: 0x04003DC0 RID: 15808
	private static SefiraManager _instance;

	// Token: 0x04003DC1 RID: 15809
	public int sefiraIndexMax = 13;

	// Token: 0x04003DC2 RID: 15810
	public List<Sefira> sefiraList;

	// Token: 0x04003DC3 RID: 15811
	private Dictionary<SefiraEnum, Sefira> _sefiraDic;

	// Token: 0x04003DC4 RID: 15812
	private List<Sefira> activatedSefira;

	// Token: 0x04003DC5 RID: 15813
	public bool isLoadedOfficerSpecialAction;

	// Token: 0x04003DC6 RID: 15814
	public bool isLoadedSefiaIsolateData;

	// Token: 0x04003DC7 RID: 15815
	private Dictionary<string, SefiraEnum> _GenNodeSefiraTable = new Dictionary<string, SefiraEnum>();
}
