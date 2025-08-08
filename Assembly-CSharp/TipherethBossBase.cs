using System;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public class TipherethBossBase : SefiraBossBase
{
	// Token: 0x060040DB RID: 16603 RVA: 0x00037F10 File Offset: 0x00036110
	public TipherethBossBase()
	{
		this.sefiraEnum = SefiraEnum.TIPERERTH1;
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060040DC RID: 16604 RVA: 0x00037F3C File Offset: 0x0003613C
	private TipherethCoreScript Script
	{
		get
		{
			return this.model.script as TipherethCoreScript;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060040DD RID: 16605 RVA: 0x00037F4E File Offset: 0x0003614E
	// (set) Token: 0x060040DE RID: 16606 RVA: 0x00037F56 File Offset: 0x00036156
	private int phase
	{
		get
		{
			return this._phase;
		}
		set
		{
			this._phase = value;
		}
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x0019624C File Offset: 0x0019444C
	public void CalculateFailureTime()
	{
		int day = PlayerModel.instance.GetDay();
		this._failureTimer.StartTimer(float.PositiveInfinity);
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x00196274 File Offset: 0x00194474
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "TipherethCoreScript", "TipherethCoreAnim", 400001L);
		this.Script.bossBase = this;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.glow = Camera.main.gameObject.AddComponent<CameraFilterPack_Glow_Glow_Color>();
		this.glow.Amount = 10f;
		this.glow.FastFilter = 7;
		this.glow.Threshold = 0f;
		this.glow.Intensity = 1.4f;
		this.glow.Precision = 1f;
		Color white = Color.white;
		ColorUtility.TryParseHtmlString("#FFC50BFF", out white);
		this.glow.GlowColor = white;
		PlaySpeedSettingUI.instance.BlockSetting(this.Script);
		this._failTimerRun = false;
		this.CalculateFailureTime();
		this.phase = 0;
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		this.StartGlowFilter();
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Tiphereth/1_Eternal",
			"Sounds/BGM/Boss/Tiphereth/2_Dark Fantasy Scene"
		});
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x001963F8 File Offset: 0x001945F8
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "TipherethCoreScript", "TipherethCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.Script.bossBase = this;
		PlaySpeedSettingUI.instance.BlockSetting(this.Script);
		this._failTimerRun = false;
		this.phase = 0;
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x00196484 File Offset: 0x00194684
	private void StartGlowFilter()
	{
		if (this.glow != null)
		{
			this.glow.Amount = 10f;
			this.glow.Threshold = 0f;
		}
		this._glowFilter.StartTimer(3f);
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x001964D4 File Offset: 0x001946D4
	private void GlowFilterUpdate()
	{
		try
		{
			float t = this.Script.AnimScript.effectCurve.Evaluate(Mathf.Clamp(this._glowFilter.Rate, 0f, 1f));
			float amount = Mathf.Lerp(10f, 0f, t);
			float threshold = Mathf.Lerp(0f, 1f, t);
			if (this.glow != null)
			{
				this.glow.Amount = amount;
				this.glow.Threshold = threshold;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x00037F5F File Offset: 0x0003615F
	public override void OnOverloadActivated(int currentLevel)
	{
		if (this.QliphothOverloadLevel == 3)
		{
			this.OnChangePhase();
		}
		if (this.QliphothOverloadLevel == 6)
		{
			this.OnChangePhase();
		}
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x00196580 File Offset: 0x00194780
	public override void OnChangePhase()
	{
		this.phase++;
		if (!SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E2))
		{
			SefiraBossManager.Instance.PlayBossBgm(this.phase - 1);
			this.StartGlowFilter();
		}
		this.Script.AnimScript.OnPhaseChange(this.phase);
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x001965DC File Offset: 0x001947DC
	public override void Update()
	{
		base.Update();
		if (this._failureTimer.started)
		{
			if (this.IsCleared())
			{
				this._failureTimer.StopTimer();
				return;
			}
			if (this._failureTimer.RunTimer())
			{
				this._failTimerRun = true;
				Debug.LogError("티페리트 보스 실패");
			}
		}
		if (this._glowFilter.started)
		{
			this.GlowFilterUpdate();
			if (this._glowFilter.RunTimer() && this.glow != null)
			{
				this.glow.Amount = 0f;
				this.glow.Threshold = 1f;
			}
		}
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x00037F85 File Offset: 0x00036185
	public override bool IsCleared()
	{
		return this.QliphothOverloadLevel >= 10 && !this._failTimerRun;
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x00037FA0 File Offset: 0x000361A0
	public override void OnStageEnd()
	{
		PlaySpeedSettingUI.instance.ReleaseSetting(this.Script);
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x00196690 File Offset: 0x00194890
	public override void OnCleared()
	{
		base.OnCleared();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x00037FB2 File Offset: 0x000361B2
	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x00037EE8 File Offset: 0x000360E8
	public override bool IsReadyToClose()
	{
		return base.IsReadyToClose();
	}

	// Token: 0x04003B9A RID: 15258
	private const string animSrc = "TipherethCoreAnim";

	// Token: 0x04003B9B RID: 15259
	private const string tipherethBase = "TipherethCoreScript";

	// Token: 0x04003B9C RID: 15260
	private const int clearQliphothLevel = 10;

	// Token: 0x04003B9D RID: 15261
	private const float _minimunFailureTime = float.PositiveInfinity;

	// Token: 0x04003B9E RID: 15262
	private const int _secondPhaseQliphothLevel = 3;

	// Token: 0x04003B9F RID: 15263
	private const int _thirdPhaseQliphothLevel = 6;

	// Token: 0x04003BA0 RID: 15264
	private const float descDelay = 15f;

	// Token: 0x04003BA1 RID: 15265
	private const float glowFilterTime = 3f;

	// Token: 0x04003BA2 RID: 15266
	private const string glowColor = "#FFC50BFF";

	// Token: 0x04003BA3 RID: 15267
	private const string bgm1 = "Tiphereth/1_Eternal";

	// Token: 0x04003BA4 RID: 15268
	private const string bgm2 = "Tiphereth/2_Dark Fantasy Scene";

	// Token: 0x04003BA5 RID: 15269
	private SefiraBossCreatureModel model;

	// Token: 0x04003BA6 RID: 15270
	private bool _failTimerRun;

	// Token: 0x04003BA7 RID: 15271
	private UnscaledTimer _failureTimer = new UnscaledTimer();

	// Token: 0x04003BA8 RID: 15272
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003BA9 RID: 15273
	private CameraFilterPack_Glow_Glow_Color glow;

	// Token: 0x04003BAA RID: 15274
	private UnscaledTimer _glowFilter = new UnscaledTimer();

	// Token: 0x04003BAB RID: 15275
	private int _phase = -1;
}
