using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000804 RID: 2052
public class ChesedBossBase : SefiraBossBase
{
	// Token: 0x06003FEA RID: 16362 RVA: 0x0003755F File Offset: 0x0003575F
	public ChesedBossBase()
	{
		this.sefiraEnum = SefiraEnum.CHESED;
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06003FEB RID: 16363 RVA: 0x0003758B File Offset: 0x0003578B
	private ChesedCoreScript Script
	{
		get
		{
			return this.model.script as ChesedCoreScript;
		}
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0003759D File Offset: 0x0003579D
	private float GetDamageChangeTime()
	{
		return 10f;
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x00191470 File Offset: 0x0018F670
	public override void OnStageStart()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-chesed-4");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "ChesedCoreScript", "ChesedCoreAnim", 400001L);
		this.Script.bossBase = this;
		this._phase = 0;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.rain = Camera.main.gameObject.AddComponent<CameraFilterPack_Atmosphere_Rain_Pro>();
		this.GenBuf();
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.SetDamageMultiplied();
		this.StartEffect();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Chesed/1_Theme_-_Blues_Man",
			"Sounds/BGM/Boss/Chesed/2_Battle_-_Urgent_Encounter"
		});
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x001915A0 File Offset: 0x0018F7A0
	public override void OnKetherStart()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-chesed-4");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "ChesedCoreScript", "ChesedCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.Script.bossBase = this;
		this._phase = 0;
		this.GenBuf();
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.SetDamageMultiplied();
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x00191634 File Offset: 0x0018F834
	private void GenBuf()
	{
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			agentModel.AddUnitBuf(new ChesedBossBuf(this));
		}
		foreach (OfficerModel officerModel in OfficerManager.instance.GetOfficerList())
		{
			officerModel.AddUnitBuf(new ChesedBossBuf(this));
		}
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x001916EC File Offset: 0x0018F8EC
	public void StartEffect()
	{
		this._effectTimer.StartTimer(3f);
		if (this.rain != null)
		{
			this.rain.Speed = 0.35f;
			this.rain.Distortion = 0.26f;
			this.rain.Size = 0.4f;
			this.rain.Intensity = 2f;
			this.rain.DropOnOff = 0.9f;
		}
	}

	// Token: 0x06003FF1 RID: 16369 RVA: 0x0019176C File Offset: 0x0018F96C
	public void UpdateEffect(float rate)
	{
		float speed = Mathf.Lerp(0.35f, (this._phase == 3) ? 0.15f : 0.1f, rate);
		float distortion = Mathf.Lerp(0.26f, 0f, rate);
		float size = 0.4f;
		float intensity = Mathf.Lerp(2f, (this._phase == 3) ? 1.25f : 0.5f, rate);
		float dropOnOff = Mathf.Lerp(0.9f, 0.15f, rate);
		if (this._phase != 3)
		{
			size = Mathf.Lerp(0.4f, 1.8f, rate);
		}
		if (this.rain != null)
		{
			this.rain.Speed = speed;
			this.rain.Distortion = distortion;
			this.rain.Size = size;
			this.rain.Intensity = intensity;
			this.rain.DropOnOff = dropOnOff;
		}
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x000375A4 File Offset: 0x000357A4
	public override void OnOverloadActivated(int currentLevel)
	{
		if (this.QliphothOverloadLevel == 2)
		{
			this.OnChangePhase();
		}
		if (this.QliphothOverloadLevel == 5)
		{
			this.OnChangePhase();
		}
		this.SetDamageMultiplied();
	}

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0019185C File Offset: 0x0018FA5C
	public override void OnChangePhase()
	{
		this._phase++;
		SefiraBossManager.Instance.PlayBossBgm(this._phase);
		this.StartEffect();
		if (this._phase == 2)
		{
			this.Script.AnimScript.animator.SetInteger("Phase", 1);
			this.Script.AnimScript.animator.SetTrigger("PhaseShift");
		}
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x001918D0 File Offset: 0x0018FAD0
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && this.QliphothOverloadLevel >= 8;
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x000375D0 File Offset: 0x000357D0
	public override void OnStageEnd()
	{
		PlaySpeedSettingUI.instance.ReleaseSetting(this.Script);
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x00191904 File Offset: 0x0018FB04
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this._effectTimer.started)
		{
			this.UpdateEffect(this._effectTimer.Rate);
			if (this._effectTimer.RunTimer())
			{
				this.UpdateEffect(1f);
			}
		}
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x00191954 File Offset: 0x0018FB54
	public override void OnCleared()
	{
		base.OnCleared();
		this.currentDamageMultiplied.Clear();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x001919B0 File Offset: 0x0018FBB0
	public void SetDamageMultiplied(int count)
	{
		List<RwbpType> list = new List<RwbpType>(ChesedBossBase.rwbpTypes);
		RwbpType rwbpType = RwbpType.N;
		if (this.currentDamageMultiplied.Count == 1)
		{
			rwbpType = this.currentDamageMultiplied[0];
		}
		this.currentDamageMultiplied.Clear();
		if (count == 1)
		{
			if (rwbpType != RwbpType.N)
			{
				list.Remove(rwbpType);
			}
			list.Remove(RwbpType.P);
		}
		for (int i = 0; i < count; i++)
		{
			RwbpType item = list[UnityEngine.Random.Range(0, list.Count)];
			list.Remove(item);
			this.currentDamageMultiplied.Add(item);
			SefiraBossUI.Instance.chesedBossUI.UpdateDamageType(this.currentDamageMultiplied);
		}
		if (count == 0)
		{
			SefiraBossUI.Instance.chesedBossUI.UpdateDamageType(this.currentDamageMultiplied);
		}
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x00191A7C File Offset: 0x0018FC7C
	private void SetDamageMultiplied()
	{
		int num = this._phase + 1;
		List<RwbpType> list = new List<RwbpType>(ChesedBossBase.rwbpTypes);
		RwbpType rwbpType = RwbpType.N;
		if (this.currentDamageMultiplied.Count == 1)
		{
			rwbpType = this.currentDamageMultiplied[0];
		}
		this.currentDamageMultiplied.Clear();
		if (num == 1)
		{
			if (rwbpType != RwbpType.N)
			{
				list.Remove(rwbpType);
			}
			list.Remove(RwbpType.P);
		}
		for (int i = 0; i < num; i++)
		{
			RwbpType item = list[UnityEngine.Random.Range(0, list.Count)];
			list.Remove(item);
			this.currentDamageMultiplied.Add(item);
			SefiraBossUI.Instance.chesedBossUI.UpdateDamageType(this.currentDamageMultiplied);
		}
		if (!SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E2))
		{
			ShockwaveEffect.Invoker(this.model.GetCurrentViewPosition(), this.model, 3f, 600f, EffectLifetimeType.NORMAL);
		}
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x000375E2 File Offset: 0x000357E2
	public bool IsDamageMultiplied(RwbpType type)
	{
		return this.currentDamageMultiplied.Contains(type);
	}

	// Token: 0x04003A60 RID: 14944
	private const string animSrc = "ChesedCoreAnim";

	// Token: 0x04003A61 RID: 14945
	private const string chesedBase = "ChesedCoreScript";

	// Token: 0x04003A62 RID: 14946
	private const string bgm1 = "Chesed/1_Theme_-_Blues_Man";

	// Token: 0x04003A63 RID: 14947
	private const string bgm2 = "Chesed/2_Battle_-_Urgent_Encounter";

	// Token: 0x04003A64 RID: 14948
	private const int clearQliphothLevel = 8;

	// Token: 0x04003A65 RID: 14949
	private const string node = "dept-chesed-4";

	// Token: 0x04003A66 RID: 14950
	private const int _secondPhaseQliphothLevel = 2;

	// Token: 0x04003A67 RID: 14951
	private const int _thirdPhaseQliphothLevel = 5;

	// Token: 0x04003A68 RID: 14952
	private const float descDelay = 15f;

	// Token: 0x04003A69 RID: 14953
	private const float _effectLifeTime = 3f;

	// Token: 0x04003A6A RID: 14954
	private static RwbpType[] rwbpTypes = new RwbpType[]
	{
		RwbpType.R,
		RwbpType.W,
		RwbpType.B,
		RwbpType.P
	};

	// Token: 0x04003A6B RID: 14955
	private SefiraBossCreatureModel model;

	// Token: 0x04003A6C RID: 14956
	private List<RwbpType> currentDamageMultiplied = new List<RwbpType>();

	// Token: 0x04003A6D RID: 14957
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003A6E RID: 14958
	private CameraFilterPack_Atmosphere_Rain_Pro rain;

	// Token: 0x04003A6F RID: 14959
	private Timer _effectTimer = new Timer();

	// Token: 0x04003A70 RID: 14960
	private float totalEnergy;

	// Token: 0x04003A71 RID: 14961
	private int _phase = -1;
}
