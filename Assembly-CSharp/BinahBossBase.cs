using System;
using System.Collections.Generic;
using BinahBoss;
using GameStatusUI;
using UnityEngine;

// Token: 0x02000800 RID: 2048
public class BinahBossBase : SefiraBossBase
{
	// Token: 0x06003FD7 RID: 16343 RVA: 0x00190994 File Offset: 0x0018EB94
	public BinahBossBase()
	{
		this.sefiraEnum = SefiraEnum.BINAH;
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x000374B1 File Offset: 0x000356B1
	public BinahCoreScript Script
	{
		get
		{
			return this.model.script as BinahCoreScript;
		}
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x001909EC File Offset: 0x0018EBEC
	public override void OnStageStart()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("sefira-binah-5");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "BinahCoreScript", "BinahCoreAnim", 400002L);
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.Script.SetBossBase(this);
		this._cameraDescTimer.StartTimer(5f * UnityEngine.Random.value);
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			creatureModel.Unit.room.SetBinahBoss();
		}
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/BinahBoss/Effect/BinahBlock");
		PlaySpeedSettingBlockedUI component = gameObject.GetComponent<PlaySpeedSettingBlockedUI>();
		List<string> list = new List<string>();
		foreach (int index in BinahBossBase.textIds)
		{
			string empty = string.Empty;
			if (SefiraBossManager.Instance.TryGetBossDesc(SefiraEnum.BINAH, SefiraBossDescType.BATTLE, index, out empty))
			{
				list.Add(empty);
			}
		}
		(component as BinahPlaySpeedBlockUI).SetText(list.ToArray());
		(component as BinahPlaySpeedBlockUI).SetBinah(this.model.script as BinahCoreScript);
		PlaySpeedSettingUI.instance.AddBlockedEvent(PlaySpeedSettingBlockType.BINAHBOSS, component);
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.blizzard = Camera.main.gameObject.AddComponent<CameraFilterPack_Blizzard>();
		this.blizzard._Speed = 5f;
		this.blizzard._Size = 1.5f;
		this.blizzard._Fade = 0.43f;
		this.blizzard.enabled = false;
		this.movie = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Old_Movie_2>();
		this.movie.FramePerSecond = 2.1f;
		this.movie.Contrast = 1.25f;
		this.movie.Burn = 0.47f;
		this.movie.SceneCut = 0.8f;
		this.movie.Fade = 0.08f;
		this.Script.OnStageStart();
		this._delayTimer.StartTimer(0.5f);
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x00190C5C File Offset: 0x0018EE5C
	public override void OnKetherStart()
	{
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			creatureModel.Unit.room.SetBinahBoss();
		}
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x00190C9C File Offset: 0x0018EE9C
	public void InitModel()
	{
		this.movie = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Old_Movie_2>();
		this.movie.FramePerSecond = 2.1f;
		this.movie.Contrast = 1.25f;
		this.movie.Burn = 0.47f;
		this.movie.SceneCut = 0.8f;
		this.movie.Fade = 0.08f;
		MapNode nodeById = MapGraph.instance.GetNodeById("sefira-binah-5");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "BinahCoreScript", "BinahCoreAnim", 400002L);
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.Script.SetBossBase(this);
		this.Script.OnStageStart();
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x000374C3 File Offset: 0x000356C3
	private void StartCameraMoveEndFirst()
	{
		CameraMover.instance.ReleaseMove();
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x00190D68 File Offset: 0x0018EF68
	public void StartBlizzardEffect(BinahBossBase.BlizzardEffect type)
	{
		this._blizzardTimer.StartTimer(this.blizzardTransTime);
		if (type != BinahBossBase.BlizzardEffect.DEFAULT)
		{
			if (type != BinahBossBase.BlizzardEffect.GAGING)
			{
				if (type == BinahBossBase.BlizzardEffect.GROGGY)
				{
					if (this._effect_end == BinahBossBase._effectValueGroggy)
					{
						this._blizzardTimer.StopTimer();
					}
					else if (this._effect_end == null)
					{
						this._effect_start = BinahBossBase._endBlizzard;
					}
					else
					{
						this._effect_start = this._effect_end;
					}
					this._effect_end = BinahBossBase._effectValueGroggy;
				}
			}
			else
			{
				if (this._effect_end == BinahBossBase._effectValueGaging)
				{
					this._blizzardTimer.StopTimer();
				}
				else if (this._effect_end == null)
				{
					this._effect_start = BinahBossBase._endBlizzard;
				}
				else
				{
					this._effect_start = this._effect_end;
				}
				this._effect_end = BinahBossBase._effectValueGaging;
			}
		}
		else
		{
			if (this._effect_end == null || this._effect_end == BinahBossBase._endBlizzard)
			{
				this._blizzardTimer.StopTimer();
			}
			else
			{
				this._effect_start = this._effect_end;
			}
			this._effect_end = BinahBossBase._endBlizzard;
		}
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x000374CF File Offset: 0x000356CF
	public void StartBlizzardEffect()
	{
		if (!this.blizzard)
		{
			return;
		}
		this._blizzard = true;
		this.blizzard.enabled = true;
		this._filterTimer.StartTimer(this.FilterTransTime);
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x00037506 File Offset: 0x00035706
	public void StartMovieEffect()
	{
		if (!this.movie)
		{
			return;
		}
		this._movie = true;
		this._filterTimer.StartTimer(this.FilterTransTime);
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x00037531 File Offset: 0x00035731
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.model != null)
		{
			this.model.OnFixedUpdate();
		}
	}

	// Token: 0x06003FE1 RID: 16353 RVA: 0x00190E94 File Offset: 0x0018F094
	public override void Update()
	{
		if (this._blizzardTimer.started && !this._blizzard)
		{
			float rate = this._blizzardTimer.Rate;
			float speed = Mathf.Lerp(this._effect_start.speed, this._effect_end.speed, rate);
			float size = Mathf.Lerp(this._effect_start.size, this._effect_end.size, rate);
			float fade = this._effect_start.fade;
			if (this._blizzardTimer.RunTimer())
			{
				speed = this._effect_end.speed;
				size = this._effect_end.size;
				fade = this._effect_end.fade;
			}
			if (this.blizzard != null)
			{
				this.blizzard._Speed = speed;
				this.blizzard._Size = size;
				this.blizzard._Fade = fade;
			}
		}
		if (this._delayTimer.started)
		{
			if (this._delayTimer.RunTimer())
			{
				Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
				currentViewPosition.y += 2f * this.model.GetMovableNode().currentScale;
				CameraMover.instance.SetEndCall(new CameraMover.OnCameraMoveEndEvent(this.StartCameraMoveEndFirst));
				CameraMover.instance.StopMove();
				CameraMover.instance.CameraMoveEvent(currentViewPosition, 8f);
				SefiraBossManager.Instance.AddBossBgm(new string[]
				{
					"Sounds/BGM/Boss/Binah/1_Jukai",
					"Sounds/BGM/Boss/Binah/2_Haunted Streets_1"
				});
			}
			return;
		}
		base.Update();
		if (this.model != null)
		{
			this.Script.Update();
		}
		if (this._filterTimer.started)
		{
			float rate2 = this._filterTimer.Rate;
			if (this._blizzard)
			{
				float speed2 = Mathf.Lerp(BinahBossBase._startBlizzard.speed, BinahBossBase._endBlizzard.speed, rate2);
				float size2 = Mathf.Lerp(BinahBossBase._startBlizzard.size, BinahBossBase._endBlizzard.size, rate2);
				float fade2 = BinahBossBase._startBlizzard.fade;
				this.blizzard._Speed = speed2;
				this.blizzard._Size = size2;
				this.blizzard._Fade = fade2;
			}
			if (this._movie)
			{
				float framePerSecond = Mathf.Lerp(BinahBossBase._startMovie.fps, BinahBossBase._endMovie.fps, rate2);
				float contrast = Mathf.Lerp(BinahBossBase._startMovie.contrast, BinahBossBase._endMovie.contrast, rate2);
				float burn = Mathf.Lerp(BinahBossBase._startMovie.burn, BinahBossBase._endMovie.burn, rate2);
				this.movie.FramePerSecond = framePerSecond;
				this.movie.Contrast = contrast;
				this.movie.Burn = burn;
			}
			if (this._filterTimer.RunTimer())
			{
				if (this._movie)
				{
					this.movie.FramePerSecond = BinahBossBase._endMovie.fps;
					this.movie.Contrast = BinahBossBase._endMovie.contrast;
					this.movie.Burn = BinahBossBase._endMovie.burn;
				}
				if (this._blizzard)
				{
					this.blizzard._Speed = BinahBossBase._endBlizzard.speed;
					this.blizzard._Size = BinahBossBase._endBlizzard.size;
					this.blizzard._Fade = BinahBossBase._endBlizzard.fade;
				}
				this._movie = false;
				this._blizzard = false;
			}
		}
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x0003754F File Offset: 0x0003574F
	public override bool IsCleared()
	{
		return this.Script.Phase == BinahPhase.END;
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x00037493 File Offset: 0x00035693
	public override bool IsReadyToClose()
	{
		return !this._closeTimer.started;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x00191208 File Offset: 0x0018F408
	public override void OnCleared()
	{
		base.OnCleared();
		this._closeTimer.StartTimer(3.667f);
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x0019126C File Offset: 0x0018F46C
	public override void OnChangePhase()
	{
		if (this._phase == -1)
		{
			this.StartMovieEffect();
		}
		else if (this._phase == 0)
		{
			if (SefiraBossManager.Instance.IsKetherBoss())
			{
				this.StartMovieEffect();
			}
			else
			{
				this.StartBlizzardEffect();
			}
		}
		this._phase++;
		if (!SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E3))
		{
			SefiraBossManager.Instance.PlayBossBgm(this._phase);
		}
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x001912EC File Offset: 0x0018F4EC
	public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
	{
		SefiraBossDescType result;
		switch (this.Script.Phase)
		{
		case BinahPhase.START:
		case BinahPhase.P1:
			result = SefiraBossDescType.OVERLOAD1;
			break;
		case BinahPhase.P2:
			result = SefiraBossDescType.OVERLOAD2;
			break;
		case BinahPhase.P3:
			result = SefiraBossDescType.OVERLOAD2;
			break;
		default:
			result = SefiraBossDescType.OVERLOAD1;
			break;
		}
		return result;
	}

	// Token: 0x04003A3A RID: 14906
	private const string animSrc = "BinahCoreAnim";

	// Token: 0x04003A3B RID: 14907
	private const string binahBase = "BinahCoreScript";

	// Token: 0x04003A3C RID: 14908
	private const string startNode = "sefira-binah-5";

	// Token: 0x04003A3D RID: 14909
	private const string bgm1 = "Binah/1_Jukai";

	// Token: 0x04003A3E RID: 14910
	private const string bgm2 = "Binah/2_Haunted Streets_1";

	// Token: 0x04003A3F RID: 14911
	private const long metaId = 400002L;

	// Token: 0x04003A40 RID: 14912
	private static int[] textIds = new int[]
	{
		23,
		24
	};

	// Token: 0x04003A41 RID: 14913
	public SefiraBossCreatureModel model;

	// Token: 0x04003A42 RID: 14914
	private bool _isInit;

	// Token: 0x04003A43 RID: 14915
	private int _phase = -1;

	// Token: 0x04003A44 RID: 14916
	private static BinahBossBase.BlizzardValue _startBlizzard = new BinahBossBase.BlizzardValue
	{
		speed = 1.5f,
		size = 1.5f,
		fade = 0.43f
	};

	// Token: 0x04003A45 RID: 14917
	private static BinahBossBase.BlizzardValue _endBlizzard = new BinahBossBase.BlizzardValue
	{
		speed = 0.3f,
		size = 1.25f,
		fade = 0.43f
	};

	// Token: 0x04003A46 RID: 14918
	private static BinahBossBase.MovieValue _startMovie = new BinahBossBase.MovieValue
	{
		fps = 50f,
		contrast = 4f,
		burn = 1f
	};

	// Token: 0x04003A47 RID: 14919
	private static BinahBossBase.MovieValue _endMovie = new BinahBossBase.MovieValue
	{
		fps = 2.1f,
		contrast = 1.25f,
		burn = 0.47f
	};

	// Token: 0x04003A48 RID: 14920
	private static BinahBossBase.BlizzardValue _effectValueGaging = new BinahBossBase.BlizzardValue
	{
		speed = 0.01f,
		size = 1.25f,
		fade = 0.43f
	};

	// Token: 0x04003A49 RID: 14921
	private static BinahBossBase.BlizzardValue _effectValueGroggy = new BinahBossBase.BlizzardValue
	{
		speed = 0.05f,
		size = 1.25f,
		fade = 0.43f
	};

	// Token: 0x04003A4A RID: 14922
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003A4B RID: 14923
	private CameraFilterPack_Blizzard blizzard;

	// Token: 0x04003A4C RID: 14924
	private CameraFilterPack_TV_Old_Movie_2 movie;

	// Token: 0x04003A4D RID: 14925
	private UnscaledTimer _filterTimer = new UnscaledTimer();

	// Token: 0x04003A4E RID: 14926
	private float FilterTransTime = 4f;

	// Token: 0x04003A4F RID: 14927
	private float blizzardTransTime = 3f;

	// Token: 0x04003A50 RID: 14928
	private bool _blizzard;

	// Token: 0x04003A51 RID: 14929
	private bool _movie;

	// Token: 0x04003A52 RID: 14930
	private UnscaledTimer _blizzardTimer = new UnscaledTimer();

	// Token: 0x04003A53 RID: 14931
	private Timer _delayTimer = new Timer();

	// Token: 0x04003A54 RID: 14932
	private BinahBossBase.BlizzardValue _effect_start;

	// Token: 0x04003A55 RID: 14933
	private BinahBossBase.BlizzardValue _effect_end;

	// Token: 0x02000801 RID: 2049
	private class BlizzardValue
	{
		// Token: 0x04003A56 RID: 14934
		public float speed;

		// Token: 0x04003A57 RID: 14935
		public float size;

		// Token: 0x04003A58 RID: 14936
		public float fade;
	}

	// Token: 0x02000802 RID: 2050
	public enum BlizzardEffect
	{
		// Token: 0x04003A5A RID: 14938
		DEFAULT,
		// Token: 0x04003A5B RID: 14939
		GAGING,
		// Token: 0x04003A5C RID: 14940
		GROGGY
	}

	// Token: 0x02000803 RID: 2051
	private class MovieValue
	{
		// Token: 0x04003A5D RID: 14941
		public float fps;

		// Token: 0x04003A5E RID: 14942
		public float contrast;

		// Token: 0x04003A5F RID: 14943
		public float burn;
	}
}
