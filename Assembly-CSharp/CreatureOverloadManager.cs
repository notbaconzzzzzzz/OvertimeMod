using System;
using System.Collections.Generic;
using GameStatusUI;
using GlobalBullet;
using UnityEngine;

// Token: 0x02000BA0 RID: 2976
public class CreatureOverloadManager
{
	// Token: 0x06005A1E RID: 23070 RVA: 0x00048098 File Offset: 0x00046298
	public CreatureOverloadManager()
	{
	}

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06005A1F RID: 23071 RVA: 0x000480CA File Offset: 0x000462CA
	public static CreatureOverloadManager instance
	{
		get
		{
			if (CreatureOverloadManager._instance == null)
			{
				CreatureOverloadManager._instance = new CreatureOverloadManager();
			}
			return CreatureOverloadManager._instance;
		}
	}

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06005A20 RID: 23072 RVA: 0x000480E5 File Offset: 0x000462E5
	public int qliphothOverloadMax
	{
		get
		{
			return this._qliphothOverloadMax;
		}
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x001FFDF4 File Offset: 0x001FDFF4
	public void OnStageStart()
	{
		int num = PlayerModel.instance.GetDay();
		if (num >= this.overflowValue.Length)
		{
			num = this.overflowValue.Length - 1;
		}
		this.clearedBossMissions = new HashSet<SefiraEnum>();
		if (GlobalGameManager.instance.gameMode == GameMode.STORY_MODE)
		{
			foreach (Mission mission in MissionManager.instance.GetClearedOrClosedBossMissions())
			{
				SefiraEnum sefira = mission.metaInfo.sefira;
				if (!this.clearedBossMissions.Contains(sefira))
				{
					if (sefira == SefiraEnum.TIPERERTH1 || sefira == SefiraEnum.TIPERERTH2)
					{
						this.clearedBossMissions.Add(SefiraEnum.TIPERERTH1);
						this.clearedBossMissions.Add(SefiraEnum.TIPERERTH2);
					}
					else
					{
						this.clearedBossMissions.Add(sefira);
					}
				}
			}
		}
		this.qliphothOverloadGauge = 0;
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			this._qliphothOverloadMax = 3;
		}
		else
		{
			this._qliphothOverloadMax = (int)((float)this.overflowValue[num] * 0.15f);
		}
		GlobalBulletManager.instance.SetMaxBullet(this._qliphothOverloadMax);
		this.SetQliphothOverloadLevel(1);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadGauge(this.qliphothOverloadGauge, this._qliphothOverloadMax);
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x001FFF50 File Offset: 0x001FE150
	private void SetQliphothOverloadLevel(int level)
	{
		this.qliphothOverloadLevel = level;
		Notice.instance.Send(NoticeName.OnQliphothOverloadLevelChanged, new object[]
		{
			this.qliphothOverloadLevel
		});
		int num = 0;
		int num2 = 0;
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			if (creatureModel.sefiraOrigin == null || !this.clearedBossMissions.Contains(creatureModel.sefiraOrigin.sefiraEnum))
			{
				if (creatureModel.GetMaxWorkCount() != 0)
				{
					num2++;
				}
				num++;
			}
		}
		this.qliphothOverloadIsolateNum = Mathf.Min(num2, (num * this.qliphothOverloadLevel + 9) / 10);
		if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
		{
			this.qliphothOverloadIsolateNum = 0;
		}
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadLevel(this.qliphothOverloadLevel);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadIsolateNum(this.qliphothOverloadIsolateNum);
		this.CheckOrdealActivate(this.qliphothOverloadLevel);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverLoadOrdeal(this._nextOrdeal);
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x00200070 File Offset: 0x001FE270
	public void AddOverloadGague()
	{
		this.qliphothOverloadGauge++;
		if (this.qliphothOverloadGauge >= this._qliphothOverloadMax)
		{
			this.ActivateOverload();
		}
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadGauge(this.qliphothOverloadGauge, this._qliphothOverloadMax);
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x002000C0 File Offset: 0x001FE2C0
	public List<CreatureModel> ActivateOverload(int overloadCount, OverloadType type, float overloadTime, bool ignoreWork = false, bool ignoreBossReward = false, bool ignoreDefaultOverload = false, params long[] ignoredCreatureMetaId)
	{
		List<CreatureModel> list = new List<CreatureModel>();
		List<CreatureModel> list2 = new List<CreatureModel>();
		List<long> list3 = new List<long>(ignoredCreatureMetaId);
		SefiraEnum currentActivatedSefira = SefiraBossManager.Instance.CurrentActivatedSefira;
		if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
		{
			return new List<CreatureModel>();
		}
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			if (creatureModel.sefira.sefiraEnum != currentActivatedSefira)
			{
				if (!creatureModel.IsEscaped())
				{
					if (ignoreWork || !creatureModel.IsWorkingState())
					{
						if (creatureModel.GetMaxWorkCount() != 0)
						{
							if (creatureModel.metaInfo.creatureKitType != CreatureKitType.EQUIP || creatureModel.kitEquipOwner == null)
							{
								if (creatureModel.metadataId != 300101L && creatureModel.metadataId != 100024L && creatureModel.metadataId != 300110L)
								{
									if (!list3.Contains(creatureModel.metadataId))
									{
										if (SefiraBossManager.Instance.IsKetherBoss())
										{
											if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
											{
												goto IL_1BA;
											}
										}
										else if (!ignoreBossReward && creatureModel.sefiraOrigin != null && this.clearedBossMissions.Contains(creatureModel.sefiraOrigin.sefiraEnum) && currentActivatedSefira != SefiraEnum.TIPERERTH1 && currentActivatedSefira != SefiraEnum.BINAH && currentActivatedSefira != SefiraEnum.CHOKHMAH)
										{
											goto IL_1BA;
										}
										if (creatureModel.isOverloaded)
										{
											if (creatureModel.overloadType != OverloadType.DEFAULT)
											{
												goto IL_1BA;
											}
											if (!ignoreDefaultOverload)
											{
												goto IL_1BA;
											}
										}
										list.Add(creatureModel);
									}
								}
							}
						}
					}
				}
			}
			IL_1BA:;
		}
		for (int j = 0; j < overloadCount; j++)
		{
			if (list.Count == 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		foreach (CreatureModel creatureModel2 in list2)
		{
			creatureModel2.ActivateOverload(this.qliphothOverloadLevel, overloadTime, type);
		}
		return list2;
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x00200338 File Offset: 0x001FE538
	private void ActivateOverload()
	{
		GlobalBulletManager.instance.Reload();
		CreatureManager.instance.ResetProbReductionCounterAll();
		if (this._nextOrdeal != null)
		{
			OrdealManager.instance.ActivateOrdeal(this._nextOrdeal, true);
			this._nextOrdeal = null;
		}
		else if (SefiraBossManager.Instance.CurrentActivatedSefira != SefiraEnum.BINAH)
		{
			this.ActivateOverload(this.qliphothOverloadIsolateNum, OverloadType.DEFAULT, 60f, true, false, false, new long[0]);
		}
		SefiraBossManager.Instance.OnOverloadActivated(this.qliphothOverloadLevel);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverLoadUI(this.qliphothOverloadLevel.ToString());
		this.qliphothOverloadGauge = 0;
		if (this.qliphothOverloadLevel < 10)
		{
			this.SetQliphothOverloadLevel(this.qliphothOverloadLevel + 1);
		}
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x000480ED File Offset: 0x000462ED
	public void OnStageRelease()
	{
		this.qliphothOverloadGauge = 0;
		this.qliphothOverloadLevel = 1;
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x000480FD File Offset: 0x000462FD
	public int GetQliphothOverloadLevel()
	{
		return this.qliphothOverloadLevel;
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x00200400 File Offset: 0x001FE600
	private bool CheckOrdealActivate(int currentLevel)
	{
		OrdealBase ordealBase = null;
		if (currentLevel == 2)
		{
			if (OrdealManager.instance.CheckOrdealContains(OrdealLevel.DAWN, out ordealBase))
			{
			}
		}
		else if (currentLevel > 2 && currentLevel <= 5)
		{
			if (OrdealManager.instance.CheckOrdealContains(OrdealLevel.NOON, out ordealBase) && !ordealBase.isStarted)
			{
				float value = UnityEngine.Random.value;
				if (currentLevel != 3)
				{
					if (currentLevel == 4)
					{
						if (value > 0.5f)
						{
							ordealBase = null;
						}
					}
				}
				else if (value > 0.33f)
				{
					ordealBase = null;
				}
			}
		}
		else if (currentLevel > 5 && currentLevel <= 7)
		{
			if (OrdealManager.instance.CheckOrdealContains(OrdealLevel.DUSK, out ordealBase) && !ordealBase.isStarted && currentLevel == 6 && UnityEngine.Random.value > 0.5f)
			{
				ordealBase = null;
			}
		}
		else if (currentLevel != 8 || OrdealManager.instance.CheckOrdealContains(OrdealLevel.MIDNIGHT, out ordealBase))
		{
		}
		if (ordealBase != null)
		{
			if (ordealBase.level > OrdealLevel.NOON && SefiraBossManager.Instance.IsAnyBossSessionActivated() && SefiraManager.instance.GetSefiraLevel(SefiraBossManager.Instance.CurrentActivatedSefira) == SefiraLevel.UP)
			{
				this._nextOrdeal = null;
				return false;
			}
			if (!ordealBase.isStarted)
			{
				this._nextOrdeal = ordealBase;
				return true;
			}
		}
		this._nextOrdeal = null;
		return false;
	}

	// Token: 0x0400529D RID: 21149
	private static CreatureOverloadManager _instance;

	// Token: 0x0400529E RID: 21150
	private HashSet<SefiraEnum> clearedBossMissions = new HashSet<SefiraEnum>();

	// Token: 0x0400529F RID: 21151
	private int qliphothOverloadGauge;

	// Token: 0x040052A0 RID: 21152
	private int _qliphothOverloadMax;

	// Token: 0x040052A1 RID: 21153
	private int qliphothOverloadLevel = 1;

	// Token: 0x040052A2 RID: 21154
	private int qliphothOverloadIsolateNum;

	// Token: 0x040052A3 RID: 21155
	private OrdealBase _nextOrdeal;

	// Token: 0x040052A4 RID: 21156
	private readonly int[] overflowValue = new int[]
	{
		30,
		30,
		30,
		30,
		30,
		35,
		35,
		35,
		35,
		35,
		50,
		50,
		50,
		50,
		50,
		55,
		55,
		55,
		55,
		55,
		70,
		70,
		70,
		70,
		70,
		80,
		80,
		80,
		80,
		80,
		80,
		80,
		80,
		90,
		90,
		90,
		90,
		90,
		95,
		95,
		95,
		95,
		95,
		100,
		105
	};
}
