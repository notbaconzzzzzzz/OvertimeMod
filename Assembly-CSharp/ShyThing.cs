using System;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public class ShyThing : CreatureBase
{
	// Token: 0x06002952 RID: 10578 RVA: 0x00120384 File Offset: 0x0011E584
	public ShyThing()
	{
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06002953 RID: 10579 RVA: 0x0001FD3E File Offset: 0x0001DF3E
	private static float _changeFaceTime
	{
		get
		{
			return UnityEngine.Random.Range(3f, 5f);
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x00028D7C File Offset: 0x00026F7C
	public ShyThingAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as ShyThingAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x00028DAB File Offset: 0x00026FAB
	public override void ParamInit()
	{
		base.ParamInit();
		this.changeFaceTimer.StopTimer();
		this.animScript.Init();
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x00028DC9 File Offset: 0x00026FC9
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.ParamInit();
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x00028DE4 File Offset: 0x00026FE4
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.changeFaceTimer.StartTimer(ShyThing._changeFaceTime);
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x001203D0 File Offset: 0x0011E5D0
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.changeFaceTimer.RunTimer())
		{
			int faceType = UnityEngine.Random.Range(0, 5);
			this.animScript.SetFaceType(faceType);
			this.changeFaceTimer.StartTimer(ShyThing._changeFaceTime);
		}
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x00028DFC File Offset: 0x00026FFC
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		if (this.changeFaceTimer.started)
		{
			this.changeFaceTimer.StopTimer();
		}
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x00028E20 File Offset: 0x00027020
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		this.changeFaceTimer.StartTimer(ShyThing._changeFaceTime);
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x00028E39 File Offset: 0x00027039
	private void SetFaceType(int feelingState)
	{
		this.animScript.SetFaceType(feelingState);
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x00120418 File Offset: 0x0011E618
	public override float GetDamageMultiplierInWork(UseSkill skill)
	{
		int faceType = this.animScript.GetFaceType();
		switch (faceType)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
			return this._dmgMultiplierByFace[faceType];
		default:
			return base.GetDamageMultiplierInWork(skill);
		}
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x00120460 File Offset: 0x0011E660
	public override int OnBonusWorkProb()
	{
		int faceType = this.animScript.GetFaceType();
		switch (faceType)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
			return this._workProbBonusByFace[faceType];
		default:
			return base.OnBonusWorkProb();
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x001204A8 File Offset: 0x0011E6A8
	public override void OnSkillSuccessWorkTick(UseSkill skill)
	{
		base.OnSkillSuccessWorkTick(skill);
		AgentModel agent = skill.agent;
		int faceType = this.animScript.GetFaceType();
		if (faceType != 0)
		{
			if (faceType == 1)
			{
				agent.RecoverMental((float)agent.maxMental * 0.2f);
			}
		}
		else
		{
			agent.RecoverHP((float)agent.maxHp * 0.2f);
			agent.RecoverMental((float)agent.maxMental * 0.2f);
		}
	}

	// Token: 0x040027A1 RID: 10145
	private Timer changeFaceTimer = new Timer();

	// Token: 0x040027A2 RID: 10146
	private const float _changeFaceTimeMin = 3f;

	// Token: 0x040027A3 RID: 10147
	private const float _changeFaceTimeMax = 5f;

	// Token: 0x040027A4 RID: 10148
	private readonly float[] _dmgMultiplierByFace = new float[]
	{
		0.5f,
		1f,
		1f,
		1.5f,
		2f
	};

	// Token: 0x040027A5 RID: 10149
	private readonly int[] _workProbBonusByFace = new int[]
	{
		30,
		10,
		0,
		-30,
		-50
	};

	// Token: 0x040027A6 RID: 10150
	public const int _defaultFaceType = 2;

	// Token: 0x040027A7 RID: 10151
	private const int _faceTypeCount = 5;

	// Token: 0x040027A8 RID: 10152
	private const float _hpHealRatio = 0.2f;

	// Token: 0x040027A9 RID: 10153
	private const float _mpHealRatio = 0.2f;

	// Token: 0x040027AA RID: 10154
	private ShyThingAnim _animScript;
}
