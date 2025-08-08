/*
public void OnStageStart() // 
private void SetQliphothOverloadLevel(int level) // 
private void ActivateOverload() // 
private bool CheckOrdealActivate(int currentLevel) // 
+things // Work Compression
+things // Secondary Qliphoth Overload
*/
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
	{ // <Mod> call OvertimeOverloadManager.OnStageStart; remove qliphoth immunities in OvertimeMode; initialize WorkCompression
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
				if (SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") && PlayerModel.instance.IsOvertimeMode() && mission.metaInfo.sefira_Level <= 5)
				{
					continue;
				}
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
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.TIPERERTH1, true))
			{
				_qliphothOverloadMax = 20;
			}
			else if (SpecialModeConfig.instance.GetValue<bool>("AutoQliphoth"))
			{
				_qliphothOverloadMax = overflowValue[num] / 5;
			}
			else
			{
				this._qliphothOverloadMax = (int)((float)this.overflowValue[num] * 0.15f);
			}
		}
		GlobalBulletManager.instance.SetMaxBullet(this._qliphothOverloadMax);
		Vestige.OvertimeOverloadManager.instance.OnStageStart();
		this.SetQliphothOverloadLevel(1);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadGauge(this.qliphothOverloadGauge, this._qliphothOverloadMax);
		_workCompression = true;
		if (!IsWorkCompressed())
		{
			_workCompression = false;
		}
		secondaryOverloadLevel = 0;
		_secondaryOverloadMax = 9;
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x001FFF50 File Offset: 0x001FE150
	private void SetQliphothOverloadLevel(int level)
	{ // <Mod> Secondary Qliphoth Overload
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
		this.CheckOrdealActivate(this.qliphothOverloadLevel);
		overtimeOverloadIsolateNum = 0;
		if (_nextOrdeal == null && (SpecialModeConfig.instance.GetValue<bool>("EarlyOvertimeOverloads") || SpecialModeConfig.instance.GetValue<bool>("OvertimeOverloads") && qliphothOverloadLevel >= 10))
		{
			overtimeOverloadIsolateNum = Vestige.OvertimeOverloadManager.instance.GetNextOverloadNum(true);
		}
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadLevel(this.qliphothOverloadLevel);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadIsolateNum(this.qliphothOverloadIsolateNum);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOvertimeOverloadIsolateNum(overtimeOverloadIsolateNum);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverLoadOrdeal(this._nextOrdeal);
		secondaryOverloadDelta = 0;
		secondaryOverloaded = false;
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x00200070 File Offset: 0x001FE270
	public void AddOverloadGague()
	{ // <Mod> Overtime Yesod Suppression
		if (GameManager.currentGameManager.autoQliphoth && GameManager.currentGameManager.autoQliphothTime < 5f)
		{
			return;
		}
		this.qliphothOverloadGauge++;
		if (secondaryOverloaded && qliphothOverloadGauge >= secondaryOverloadPosition)
		{
			secondaryOverloaded = false;
			if (_secondaryOrdeal != null)
			{
				OrdealManager.instance.ActivateOrdeal(_secondaryOrdeal, true);
			}
			else
			{
				Vestige.OvertimeOverloadManager.instance.ActivateOverload(secondaryOverloadIsolateNum, true);
			}
			GameStatusUI.GameStatusUI.Window.energyContorller.ClearSecondaryOverload();
			GameStatusUI.GameStatusUI.Window.energyContorller.SetSecondaryOverloadGauge(secondaryOverloadGauge, _secondaryOverloadMax);
			GameStatusUI.GameStatusUI.Window.energyContorller.SetOverLoadUI(secondaryOverloadLevel.ToString());
		}
		if (this.qliphothOverloadGauge >= this._qliphothOverloadMax)
		{
			if (!SefiraBossManager.Instance.HesitateOverloadGuage())
			{
				this.ActivateOverload();
			}
		}
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverloadGauge(this.qliphothOverloadGauge, this._qliphothOverloadMax);
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x002000C0 File Offset: 0x001FE2C0
	public List<CreatureModel> ActivateOverload(int overloadCount, OverloadType type, float overloadTime, bool ignoreWork = false, bool ignoreBossReward = false, bool ignoreDefaultOverload = false, params long[] ignoredCreatureMetaId)
	{
		return ActivateOverload(overloadCount, type, overloadTime, ignoreWork, ignoreBossReward, ignoreDefaultOverload, false, ignoredCreatureMetaId);
	}

	// <Mod>
	public List<CreatureModel> ActivateOverload(int overloadCount, OverloadType type, float overloadTime, bool ignoreWork = false, bool ignoreBossReward = false, bool ignoreDefaultOverload = false, bool isNatural = false, params long[] ignoredCreatureMetaId)
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
			creatureModel2.ActivateOverload(this.qliphothOverloadLevel, overloadTime, type, isNatural);
		}
		return list2;
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x00200338 File Offset: 0x001FE538
	private void ActivateOverload()
	{ // <Mod>
		GlobalBulletManager.instance.Reload();
		CreatureManager.instance.ResetProbReductionCounterAll();
		if (this._nextOrdeal != null)
		{
			OrdealManager.instance.ActivateOrdeal(this._nextOrdeal, true);
			this._nextOrdeal = null;
		}
		else
		{
			if (overtimeOverloadIsolateNum > 0)
			{
				Vestige.OvertimeOverloadManager.instance.ActivateOverload(overtimeOverloadIsolateNum, true);
			}
			if (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.BINAH, false))
			{
				this.ActivateOverload(this.qliphothOverloadIsolateNum, OverloadType.DEFAULT, 60f, true, false, false, true, new long[0]);
			}
		}
		SefiraBossManager.Instance.OnOverloadActivated(this.qliphothOverloadLevel);
		GameStatusUI.GameStatusUI.Window.energyContorller.SetOverLoadUI(this.qliphothOverloadLevel.ToString());
		this.qliphothOverloadGauge = 0;
		if (this.qliphothOverloadLevel < 20)
		{
			this.SetQliphothOverloadLevel(this.qliphothOverloadLevel + 1);
		}
		else
		{
			SetQliphothOverloadLevel(20);
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
	{ // <Mod>
		OrdealBase ordealBase = null;
		for (int i = 0; i < 8; i++)
		{
			OrdealLevel level = (OrdealLevel)i;
			if (currentLevel == OrdealManager.instance.GetOrdealSpawnTime(level) && OrdealManager.instance.CheckOrdealContains(level, out ordealBase))
			{
				break;
			}
		}
		if (ordealBase != null)
		{
			/*
			if (ordealBase.level > OrdealLevel.NOON && SefiraBossManager.Instance.IsAnyBossSessionActivated() && SefiraManager.instance.GetSefiraLevel(SefiraBossManager.Instance.CurrentActivatedSefira) == SefiraLevel.UP)
			{
				this._nextOrdeal = null;
				return false;
			}
			*/
			if (!ordealBase.isStarted)
			{
				this._nextOrdeal = ordealBase;
				return true;
			}
		}
		this._nextOrdeal = null;
		return false;
	}

	//> <Mod>
	public int workCompressionLimit
	{
		get
		{
			int num = 0;
			if ((!SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") && PlayerModel.instance.IsOvertimeMode()) || PlayerModel.instance.IsExtraOvertimeMode())
			{
				num = SpecialModeConfig.instance.GetValue<int>("WorkCompression");
			}
			else if (PlayerModel.instance.IsOvertimeMode())
			{
				num = SpecialModeConfig.instance.GetValue<int>("WorkCompressionOvertime");
			}
			else
			{
				num = SpecialModeConfig.instance.GetValue<int>("WorkCompressionAlways");
			} 
			if (ResearchDataModel.instance.IsUpgradedAbility("work_compression"))
			{
				num += 2;
			}
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1))
			{
				num += 1;
			}
			return num;
		}
	}

	public bool IsWorkCompressed()
	{
		if (!_workCompression) return false;
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated()) return false;
		if (GetQliphothOverloadLevel() <= workCompressionLimit)
		{
			return true;
		}
		if (GetQliphothOverloadLevel() <= 10 && MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1))
		{
			int num = (GetQliphothOverloadLevel() - workCompressionLimit - 1) * _qliphothOverloadMax + qliphothOverloadGauge;
			if (num % 6 == 4)
			{
				return true;
			}
		}
		return false;
	}

	public void ToggleWorkCompression(bool sendLog = true)
	{
		if (_workCompression)
		{
			_workCompression = false;
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				" -- Work Compression Disabled -- "
			});
		}
		else if (!SefiraBossManager.Instance.IsAnyBossSessionActivated())
		{
			_workCompression = true;
			if (!IsWorkCompressed() && !(MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1) && GetQliphothOverloadLevel() <= 10))
			{
				_workCompression = false;
			}
			else
			{
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					" -- Work Compression Enabled -- "
				});
			}
		}
	}

	public void AddSecondaryGague(int num = 1)
	{
		if (!SpecialModeConfig.instance.GetValue<bool>("SecondaryQliphothOverload")) return;
		if (secondaryOverloadDelta + num > _secondaryOverloadMax)
		{
			num = _secondaryOverloadMax - secondaryOverloadDelta;
		}
		secondaryOverloadGauge += num;
		secondaryOverloadDelta += num;
		if (secondaryOverloadGauge >= _secondaryOverloadMax && !secondaryOverloaded)
		{
			secondaryOverloadGauge -= _secondaryOverloadMax;
			secondaryOverloaded = true;
			secondaryOverloadPosition = (qliphothOverloadGauge + _qliphothOverloadMax + 1) / 2;
			secondaryOverloadLevel++;
			int secondaryOrdealLevel = -1;
			if (secondaryOverloadLevel == 2) secondaryOrdealLevel = 0;
			else if (secondaryOverloadLevel == 4) secondaryOrdealLevel = 1;
			else if (secondaryOverloadLevel == 6) secondaryOrdealLevel = 2;
			else if (secondaryOverloadLevel == 8) secondaryOrdealLevel = 3;
			if (secondaryOverloadLevel == -1 || !OrdealManager.instance.CheckSecondaryOrdealContains((OrdealLevel)(secondaryOrdealLevel + 4), out _secondaryOrdeal))
			{
				secondaryOverloadIsolateNum = Vestige.OvertimeOverloadManager.instance.GetNextOverloadNum(true);
				if (secondaryOverloadLevel <= 4)
				{
					secondaryOverloadIsolateNum += 1;
				}
				else if (secondaryOverloadLevel <= 6)
				{
					secondaryOverloadIsolateNum += 2;
				}
				else
				{
					secondaryOverloadIsolateNum += 4;
				}
				_secondaryOrdeal = null;
			}
			GameStatusUI.GameStatusUI.Window.energyContorller.SetSecondaryOverload(secondaryOverloadPosition, _qliphothOverloadMax, _secondaryOrdeal, secondaryOverloadIsolateNum);
			AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "rabbit/RabbitTeam_Alert"));
			//AudioClip audioClip2 = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "alertBeep"));
			if (audioClip != null)
			{
				if (secondaryOverloadSound != null)// && secondaryOverloadSound.clip == audioClip2)
				{
					secondaryOverloadSound.Stop();
				}
				secondaryOverloadSound = null; //GlobalAudioManager.instance.GetIdleSource();
				GlobalAudioManager.instance.PlayLocalClip(audioClip);
			}
		}
		else
		{
			AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "alertBeep"));
			if (audioClip != null)
			{
				if (secondaryOverloadSound != null)// && secondaryOverloadSound.clip == audioClip)
				{
					secondaryOverloadSound.Stop();
				}
				secondaryOverloadSound = GlobalAudioManager.instance.GetIdleSource();
				GlobalAudioManager.instance.PlayLocalClip(audioClip);
			}
		}
		GameStatusUI.GameStatusUI.Window.energyContorller.SetSecondaryOverloadGauge(secondaryOverloadGauge, _secondaryOverloadMax);
	}

	private bool _workCompression;

	private int secondaryOverloadGauge;

	private int secondaryOverloadLevel;

	private int secondaryOverloadDelta;

	private bool secondaryOverloaded;

	private int secondaryOverloadPosition;

	private OrdealBase _secondaryOrdeal;

	private int secondaryOverloadIsolateNum;

	private int _secondaryOverloadMax = 9;

	private int overtimeOverloadIsolateNum;

	private AudioSource secondaryOverloadSound;
	//< <Mod>

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
