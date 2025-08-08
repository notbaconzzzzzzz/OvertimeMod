/*
+public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000810 RID: 2064
public class HodBossBase : SefiraBossBase
{
	// Token: 0x06003FCF RID: 16335 RVA: 0x00037434 File Offset: 0x00035634
	public HodBossBase()
	{
		this.sefiraEnum = SefiraEnum.HOD;
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x0003744A File Offset: 0x0003564A
	private HodCoreScript Script
	{
		get
		{
			return this.model.script as HodCoreScript;
		}
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x0018C2C8 File Offset: 0x0018A4C8
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "HodCoreScript", "HodCoreAnim", 400001L);
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
		this._80 = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_80>();
		this._80.Fade = 0.135f;
		this._80.enabled = false;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.vhs = Camera.main.gameObject.AddComponent<CameraFilterPack_Real_VHS>();
		this.vhs.TRACKING = 0f;
		this.vhs.JITTER = 0.074f;
		this.vhs.GLITCH = 0.08f;
		this.vhs.NOISE = 0.659f;
		this.vhs.Brightness = 0.23f;
		this.vhs.Constrast = 0.796f;
		this.vhs.enabled = false;
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Hod/1_Theme_-_Retro_Time_ALT",
			"Sounds/BGM/Boss/Hod/2_Theme_-_Retro_Time_ALT(Mix)"
		});
	}

	// Token: 0x06003FD2 RID: 16338 RVA: 0x0018C46C File Offset: 0x0018A66C
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "HodCoreScript", "HodCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x0018C4F4 File Offset: 0x0018A6F4
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 6;
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x0018C52C File Offset: 0x0018A72C
	public override void OnCleared()
	{
		base.OnCleared();
		this.Script.AnimScript.OnClear();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x0003745C File Offset: 0x0003565C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.QliphothOverloadLevel >= 3)
		{
		}
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x00037470 File Offset: 0x00035670
	public override float GetDescFreq()
	{
		if (this.QliphothOverloadLevel >= 3)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x00037489 File Offset: 0x00035689
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
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x0018C590 File Offset: 0x0018A790
	public override void OnChangePhase()
	{
		this._phase++;
		SefiraBossManager.Instance.PlayBossBgm(this._phase);
		int phase = this._phase;
		if (phase != 0)
		{
			if (phase == 1)
			{
				foreach (HodBossBuf hodBossBuf in this.bufList)
				{
					hodBossBuf.SetReduceValue(HodBossBase._thirdReduce);
				}
				if (this.vhs != null && !this.vhs.enabled)
				{
					this.vhs.enabled = true;
				}
			}
		}
		else
		{
			foreach (HodBossBuf hodBossBuf2 in this.bufList)
			{
				hodBossBuf2.SetReduceValue(HodBossBase._secondReduce);
			}
			if (this._80 != null)
			{
				this._80.enabled = true;
			}
		}
		if (!SefiraBossManager.Instance.IsKetherBoss())
		{
			ShockwaveEffect.Invoker(this.model.GetCurrentViewPosition(), this.model, 3f, 600f, EffectLifetimeType.NORMAL);
			this.MakeSoundAttachCamera("SefiraBoss/Boss_Nezach");
		}
	}

    // <Mod>
    public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
    {
        if (_ordealSpawnTime[1] >= 5)
		{
			_ordealSpawnTime[1] = UnityEngine.Random.Range(3, 5);
		}
    }

	// Token: 0x06003FD9 RID: 16345 RVA: 0x000374B4 File Offset: 0x000356B4
	// Note: this type is marked as 'beforefieldinit'.
	static HodBossBase()
	{
	}

	// Token: 0x04003A6F RID: 14959
	private const string animSrc = "HodCoreAnim";

	// Token: 0x04003A70 RID: 14960
	private const string hodBase = "HodCoreScript";

	// Token: 0x04003A71 RID: 14961
	private const string bgm1 = "Hod/1_Theme_-_Retro_Time_ALT";

	// Token: 0x04003A72 RID: 14962
	private const string bgm2 = "Hod/2_Theme_-_Retro_Time_ALT(Mix)";

	// Token: 0x04003A73 RID: 14963
	private const string phaseSound = "SefiraBoss/Boss_Nezach";

	// Token: 0x04003A74 RID: 14964
	private const int clearQliphothLevel = 6;

	// Token: 0x04003A75 RID: 14965
	private const int changeQliphothLevel = 3;

	// Token: 0x04003A76 RID: 14966
	private const float descDelay = 15f;

	// Token: 0x04003A77 RID: 14967
	public static float _firstReduce = 15f;

	// Token: 0x04003A78 RID: 14968
	public static float _secondReduce = 25f;

	// Token: 0x04003A79 RID: 14969
	public static float _thirdReduce = 35f;

	// Token: 0x04003A7A RID: 14970
	private float totalEnergy;

	// Token: 0x04003A7B RID: 14971
	private SefiraBossCreatureModel model;

	// Token: 0x04003A7C RID: 14972
	private CameraFilterPack_TV_80 _80;

	// Token: 0x04003A7D RID: 14973
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003A7E RID: 14974
	private CameraFilterPack_Real_VHS vhs;

	// Token: 0x04003A7F RID: 14975
	public List<HodBossBuf> bufList;

	// Token: 0x04003A80 RID: 14976
	private int _phase = -1;
}
