using System;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class YesodBossBase : SefiraBossBase
{
	// Token: 0x0600405A RID: 16474 RVA: 0x00037A3F File Offset: 0x00035C3F
	public YesodBossBase()
	{
		this.sefiraEnum = SefiraEnum.YESOD;
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x0600405B RID: 16475 RVA: 0x00037A60 File Offset: 0x00035C60
	private YesodCoreScript Script
	{
		get
		{
			return this.model.script as YesodCoreScript;
		}
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x00037A72 File Offset: 0x00035C72
	public void SetCameraScript(YesodBossCameraScript cameraScript)
	{
		this.cameraScript = cameraScript;
	}

	// Token: 0x0600405D RID: 16477 RVA: 0x0018D794 File Offset: 0x0018B994
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "YesodCoreScript", "YesodCoreAnim", 400001L);
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		this.pixelisation = Camera.main.gameObject.AddComponent<CameraFilterPack_Pixel_Pixelisation>();
		this.pixelisation._Pixelisation = 2f;
		this.pixelisation._SizeX = 0.6f;
		this.pixelisation._SizeY = 1f;
		this.pixelisation.enabled = false;
		this.noiseTv = Camera.main.gameObject.AddComponent<CameraFilterPack_Noise_TV>();
		this.noiseTv.Fade = 0.075f;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0f;
		this.vignetting.VignettingFull = 0f;
		this.vignetting.VignettingColor = Color.black;
		this.glitch3 = Camera.main.gameObject.AddComponent<CameraFilterPack_FX_Glitch3>();
		this.glitch3._Noise = 1f;
		this.glitch3._Glitch = 0.06f;
		this.glitch3.enabled = false;
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Yesod/1_Tilarids - untitled9877645623413123325",
			"Sounds/BGM/Boss/Yesod/2_Tilarids - Faded"
		});
		this.StartEffect();
	}

	// Token: 0x0600405E RID: 16478 RVA: 0x0018D944 File Offset: 0x0018BB44
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "YesodCoreScript", "YesodCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
	}

	// Token: 0x0600405F RID: 16479 RVA: 0x0018D9CC File Offset: 0x0018BBCC
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 6;
	}

	// Token: 0x06004060 RID: 16480 RVA: 0x00037A7B File Offset: 0x00035C7B
	public void StartEffect()
	{
		this._startEffectTimer.StartTimer(3f);
		this.noiseTv.Fade = 0.075f;
	}

	// Token: 0x06004061 RID: 16481 RVA: 0x0018DA04 File Offset: 0x0018BC04
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this._startEffectTimer.started)
		{
			float fade;
			if (this._startEffectTimer.Rate < 0.5f)
			{
				fade = Mathf.Lerp(0.075f, 1f, this._startEffectTimer.Rate / 0.5f);
			}
			else
			{
				fade = Mathf.Lerp(1f, 0.075f, this._startEffectTimer.Rate / 0.5f - 1f);
			}
			if (this.noiseTv != null)
			{
				this.noiseTv.Fade = fade;
			}
			if (this._startEffectTimer.RunTimer() && this.noiseTv != null)
			{
				this.noiseTv.Fade = 0.075f;
			}
		}
	}

	// Token: 0x06004062 RID: 16482 RVA: 0x00037359 File Offset: 0x00035559
	public override float GetDescFreq()
	{
		if (this.QliphothOverloadLevel >= 3)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	// Token: 0x06004063 RID: 16483 RVA: 0x0018DAE0 File Offset: 0x0018BCE0
	public override void OnCleared()
	{
		base.OnCleared();
		this.Script.AnimScript.OnClear();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06004064 RID: 16484 RVA: 0x00037372 File Offset: 0x00035572
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

	// Token: 0x06004065 RID: 16485 RVA: 0x0018DB44 File Offset: 0x0018BD44
	public override void OnChangePhase()
	{
		this._phase++;
		SefiraBossManager.Instance.PlayBossBgm(this._phase);
		if (this._phase == 0)
		{
			this.pixelisation.enabled = true;
		}
		else if (this._phase == 1)
		{
			this.glitch3.enabled = true;
		}
		this.StartEffect();
		ShockwaveEffect.Invoker(this.model.GetCurrentViewPosition(), this.model, 3f, 600f, EffectLifetimeType.NORMAL);
		this.MakeSoundAttachCamera("SefiraBoss/Boss_Yesod");
	}

	// Token: 0x04003B51 RID: 15185
	private const string animSrc = "YesodCoreAnim";

	// Token: 0x04003B52 RID: 15186
	private const string yesodBase = "YesodCoreScript";

	// Token: 0x04003B53 RID: 15187
	private const string bgm1 = "Yesod/1_Tilarids - untitled9877645623413123325";

	// Token: 0x04003B54 RID: 15188
	private const string bgm2 = "Yesod/2_Tilarids - Faded";

	// Token: 0x04003B55 RID: 15189
	private const string phaseSound = "SefiraBoss/Boss_Yesod";

	// Token: 0x04003B56 RID: 15190
	private const int clearQliphothLevel = 6;

	// Token: 0x04003B57 RID: 15191
	private const int changeQliphothLevel = 3;

	// Token: 0x04003B58 RID: 15192
	private const float descDelay = 15f;

	// Token: 0x04003B59 RID: 15193
	private const float noiseDefaultValue = 0.075f;

	// Token: 0x04003B5A RID: 15194
	private float totalEnergy;

	// Token: 0x04003B5B RID: 15195
	private SefiraBossCreatureModel model;

	// Token: 0x04003B5C RID: 15196
	private CameraFilterPack_Pixel_Pixelisation pixelisation;

	// Token: 0x04003B5D RID: 15197
	private CameraFilterPack_Noise_TV noiseTv;

	// Token: 0x04003B5E RID: 15198
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003B5F RID: 15199
	private CameraFilterPack_FX_Glitch3 glitch3;

	// Token: 0x04003B60 RID: 15200
	private YesodBossCameraScript cameraScript;

	// Token: 0x04003B61 RID: 15201
	private Timer _startEffectTimer = new Timer();

	// Token: 0x04003B62 RID: 15202
	private int _phase = -1;
}
