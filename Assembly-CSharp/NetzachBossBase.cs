/*
+public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class NetzachBossBase : SefiraBossBase
{
	// Token: 0x06004079 RID: 16505 RVA: 0x00037ABE File Offset: 0x00035CBE
	public NetzachBossBase()
	{
		this.sefiraEnum = SefiraEnum.NETZACH;
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x0600407A RID: 16506 RVA: 0x00037ADF File Offset: 0x00035CDF
	private NetzachCoreScript Script
	{
		get
		{
			return this.model.script as NetzachCoreScript;
		}
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x0018FD88 File Offset: 0x0018DF88
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "NetzachCoreScript", "NetzachCoreAnim", 400001L);
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
		this.waterDrop = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_WaterDrop>();
		this.waterDrop.Distortion = 40f;
		this.waterDrop.SizeX = 0.3f;
		this.waterDrop.SizeY = 0.5f;
		this.waterDrop.Speed = 4f;
		this.fog = Camera.main.gameObject.AddComponent<CameraFilterPack_Atmosphere_Fog>();
		this.fog._Near = 0f;
		this.fog._Far = 0.5f;
		Color green = Color.green;
		if (ColorUtility.TryParseHtmlString("#5BD417FF", out green))
		{
			this.fog.FogColor = green;
		}
		this.fog.Fade = 0.135f;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingColor = Color.black;
		this.vignetting.VignettingDirt = 0f;
		this.vignetting.VignettingFull = 0f;
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Netzach/1_3 - Abandoned",
			"Sounds/BGM/Boss/Netzach/2_Tilarids - Blue Dots"
		});
		SefiraBossManager.Instance.SetRecoverBlockState(true);
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x0018FF44 File Offset: 0x0018E144
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "NetzachCoreScript", "NetzachCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x00037AF1 File Offset: 0x00035CF1
	public void StartEffect()
	{
		this._startEffectTimer.StartTimer(this.Script.AnimScript.startEffectTime);
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x0018FFCC File Offset: 0x0018E1CC
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 6;
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x00037B0E File Offset: 0x00035D0E
	public override bool IsReadyToClose()
	{
		return base.IsReadyToClose();
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x00190004 File Offset: 0x0018E204
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this._startEffectTimer.started)
		{
			float rate = this._startEffectTimer.Rate;
			float distortion = Mathf.Lerp(40f, 64f, rate);
			float speed = Mathf.Lerp(0.19f, 4f, 1f - rate);
			if (this._startEffectTimer.RunTimer())
			{
				distortion = 64f;
				speed = 0.19f;
			}
			if (this.waterDrop != null)
			{
				this.waterDrop.Distortion = distortion;
				this.waterDrop.Speed = speed;
			}
		}
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00037B16 File Offset: 0x00035D16
	public override float GetDescFreq()
	{
		if (this.QliphothOverloadLevel >= 3)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x001900A0 File Offset: 0x0018E2A0
	public override void OnCleared()
	{
		base.OnCleared();
		this.Script.AnimScript.OnClear();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00190104 File Offset: 0x0018E304
	public override void OnOverloadActivated(int currentLevel)
	{
		if (this.QliphothOverloadLevel == 1)
		{
			this.OnChangePhase();
		}
		else if (this.QliphothOverloadLevel == 3)
		{
			this.OnChangePhase();
		}
		this.RecoverAll();
		this.MakeSoundAttachCamera("SefiraBoss/Boss_Nezach");
		this.StartEffect();
		ShockwaveEffect.Invoker(this.model.GetCurrentViewPosition(), this.model, 3f, 600f, EffectLifetimeType.NORMAL);
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x00037B2F File Offset: 0x00035D2F
	public override void OnChangePhase()
	{
		this._phase++;
		SefiraBossManager.Instance.PlayBossBgm(this._phase);
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x00190174 File Offset: 0x0018E374
	public void RecoverAll()
	{
		SefiraBossManager.Instance.SetRecoverBlockState(false);
		List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
		foreach (AgentModel agentModel in list)
		{
			if (!agentModel.IsDead())
			{
				if (agentModel.unconAction == null)
				{
					agentModel.RecoverHP((float)agentModel.maxHp);
					agentModel.RecoverMental((float)agentModel.maxMental);
				}
			}
		}
		SefiraBossManager.Instance.SetRecoverBlockState(true);
	}

    // <Mod>
    public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
    {
        if (_ordealSpawnTime[1] >= 5)
		{
			_ordealSpawnTime[1] = UnityEngine.Random.Range(3, 5);
		}
    }

	// Token: 0x04003B5F RID: 15199
	private const string animSrc = "NetzachCoreAnim";

	// Token: 0x04003B60 RID: 15200
	private const string netzachBase = "NetzachCoreScript";

	// Token: 0x04003B61 RID: 15201
	private const string bgm1 = "Netzach/1_3 - Abandoned";

	// Token: 0x04003B62 RID: 15202
	private const string bgm2 = "Netzach/2_Tilarids - Blue Dots";

	// Token: 0x04003B63 RID: 15203
	private const string phaseSound = "SefiraBoss/Boss_Nezach";

	// Token: 0x04003B64 RID: 15204
	private const int clearQliphothLevel = 6;

	// Token: 0x04003B65 RID: 15205
	private const int changeQliphothLevel = 3;

	// Token: 0x04003B66 RID: 15206
	private const float descDelay = 15f;

	// Token: 0x04003B67 RID: 15207
	private float totalEnergy;

	// Token: 0x04003B68 RID: 15208
	private SefiraBossCreatureModel model;

	// Token: 0x04003B69 RID: 15209
	private CameraFilterPack_AAA_WaterDrop waterDrop;

	// Token: 0x04003B6A RID: 15210
	private CameraFilterPack_Atmosphere_Fog fog;

	// Token: 0x04003B6B RID: 15211
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003B6C RID: 15212
	private Timer _startEffectTimer = new Timer();

	// Token: 0x04003B6D RID: 15213
	private int _phase = -1;
}
