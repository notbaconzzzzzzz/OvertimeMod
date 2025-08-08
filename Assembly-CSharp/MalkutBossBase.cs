using System;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class MalkutBossBase : SefiraBossBase
{
	// Token: 0x06004028 RID: 16424 RVA: 0x00037694 File Offset: 0x00035894
	public MalkutBossBase()
	{
		this.sefiraEnum = SefiraEnum.MALKUT;
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06004029 RID: 16425 RVA: 0x000376B5 File Offset: 0x000358B5
	private MalkutCoreScript Script
	{
		get
		{
			return this.model.script as MalkutCoreScript;
		}
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x0003719A File Offset: 0x0003539A
	public override float GetDescFreq()
	{
		if (this.QliphothOverloadLevel >= 3)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x0018CCFC File Offset: 0x0018AEFC
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "MalkutCoreScript", "MalkutCoreAnim", 400001L);
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		this.psycho = Camera.main.gameObject.AddComponent<CameraFilterPack_Vision_Psycho>();
		this.auraDistortion = Camera.main.gameObject.AddComponent<CameraFilterPack_Vision_AuraDistortion>();
		this.psycho.HoleSize = 0.75f;
		this.auraDistortion.Twist = 0.12f;
		this.Script.bossBase = this;
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Malkuth/1_Tilarids - Violation Of Black Colors",
			"Sounds/BGM/Boss/Malkuth/2_Tilarids - Red Dots"
		});
		SefiraBossManager.Instance.RandomizeWorkId();
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x0018CDEC File Offset: 0x0018AFEC
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "MalkutCoreScript", "MalkutCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
		SefiraBossManager.Instance.RandomizeWorkId();
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x000376C7 File Offset: 0x000358C7
	public void StartEffect()
	{
		this._startEffectTimer.StartTimer(this.Script.AnimScript.startEffectTime);
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x0018CE80 File Offset: 0x0018B080
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this._startEffectTimer.started)
		{
			if (this.psycho != null)
			{
				this.psycho.HoleSize = this.Script.AnimScript.startPsychoCurve.Evaluate(this._startEffectTimer.Rate);
			}
			if (this._startEffectTimer.RunTimer() && this.psycho != null)
			{
				this.psycho.HoleSize = 0.75f;
			}
		}
		if (SefiraBossManager.Instance.IsKetherBoss())
		{
			return;
		}
		int qliphothOverloadLevel = CreatureOverloadManager.instance.GetQliphothOverloadLevel();
		if (CreatureOverloadManager.instance.GetQliphothOverloadLevel() == 3)
		{
			this.Script.AnimScript.OnChange();
		}
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x000376E4 File Offset: 0x000358E4
	public override void OnOverloadActivated(int currentLevel)
	{
		if (this.QliphothOverloadLevel == 1)
		{
			this.OnChangePhase();
		}
		else if (this.QliphothOverloadLevel == 3)
		{
			this.OnChangePhase();
			SefiraBossManager.Instance.SetWorkCancelableState(false);
		}
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x0018CF4C File Offset: 0x0018B14C
	public override void OnChangePhase()
	{
		this._phase++;
		SefiraBossManager.Instance.PlayBossBgm(this._phase);
		SefiraBossManager.Instance.RandomizeWorkId();
		this.StartEffect();
		ShockwaveEffect.Invoker(this.model.GetCurrentViewPosition(), this.model, 3f, 600f, EffectLifetimeType.NORMAL);
		this.MakeSoundAttachCamera("SefiraBoss/Boss_Malkut");
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x0018CFB8 File Offset: 0x0018B1B8
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 6;
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x0018CFF0 File Offset: 0x0018B1F0
	public override void OnCleared()
	{
		base.OnCleared();
		this.Script.AnimScript.OnClear();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x04003B1C RID: 15132
	private const string animSrc = "MalkutCoreAnim";

	// Token: 0x04003B1D RID: 15133
	private const string malkutBase = "MalkutCoreScript";

	// Token: 0x04003B1E RID: 15134
	private const string bgm1 = "Malkuth/1_Tilarids - Violation Of Black Colors";

	// Token: 0x04003B1F RID: 15135
	private const string bgm2 = "Malkuth/2_Tilarids - Red Dots";

	// Token: 0x04003B20 RID: 15136
	private const string phaseSound = "SefiraBoss/Boss_Malkut";

	// Token: 0x04003B21 RID: 15137
	private const int clearQliphothLevel = 6;

	// Token: 0x04003B22 RID: 15138
	private const int changeQliphothLevel = 3;

	// Token: 0x04003B23 RID: 15139
	private const float descDelay = 15f;

	// Token: 0x04003B24 RID: 15140
	private SefiraBossCreatureModel model;

	// Token: 0x04003B25 RID: 15141
	private Timer _startEffectTimer = new Timer();

	// Token: 0x04003B26 RID: 15142
	private float totalEnergy;

	// Token: 0x04003B27 RID: 15143
	private CameraFilterPack_Vision_Psycho psycho;

	// Token: 0x04003B28 RID: 15144
	private CameraFilterPack_Vision_AuraDistortion auraDistortion;

	// Token: 0x04003B29 RID: 15145
	private CameraFilterPack_Film_Grain grain;

	// Token: 0x04003B2A RID: 15146
	private int _phase = -1;
}
