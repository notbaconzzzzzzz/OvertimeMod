/*
private int GetSkillProb(UseSkill skill) // Filter out chances less than 3%
*/
using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class ViscusSnake : CreatureBase
{
	// Token: 0x06002AF1 RID: 10993 RVA: 0x0002A15C File Offset: 0x0002835C
	public ViscusSnake()
	{
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x000291C8 File Offset: 0x000273C8
	private float DefaultSoundDelay
	{
		get
		{
			return UnityEngine.Random.Range(12f, 18f);
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x0002A17A File Offset: 0x0002837A
	private ViscusSnakeAnim AnimScript
	{
		get
		{
			return base.Unit.animTarget as ViscusSnakeAnim;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x00022F64 File Offset: 0x00021164
	private IsolateFilter Filter
	{
		get
		{
			return base.Unit.room.SkillFilter;
		}
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x00004381 File Offset: 0x00002581
	public static void Log(string text)
	{
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x0002A18C File Offset: 0x0002838C
	public override void OnViewInit(CreatureUnit unit)
	{
		this.AnimScript.SetScript(this);
		this.Filter.renderAnim.enabled = false;
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x0002A1AB File Offset: 0x000283AB
	public override void OnFixedUpdateInSkill(UseSkill skill)
	{
		if (this._skillActivateTimer.started)
		{
			this.SetFilterAlpha();
			if (this._skillActivateTimer.RunTimer())
			{
				this.OnSkillActivateTimerExpired();
			}
		}
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x00126160 File Offset: 0x00124360
	private void SetFilterScale()
	{
		float curve = this.AnimScript.GetCurve(this._skillActivateTimer.Rate);
		Vector3 localScale = curve * this.originalScale;
		this.Filter.transform.localScale = localScale;
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x001261A4 File Offset: 0x001243A4
	private void SetFilterAlpha()
	{
		Color renderColor = this.Filter.renderColor;
		renderColor.a = this.AnimScript.GetAlpha(this._skillActivateTimer.Rate);
		this.Filter.renderColor = renderColor;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x001261E8 File Offset: 0x001243E8
	public override void OnSkillGoalComplete(UseSkill skill)
	{
		skill.PauseWorking();
		this.originalScale = this.Filter.transform.localScale;
		this._skillActivateTimer.StartTimer(4f);
		ViscusSnake.Log("StartFilter");
		this.RoomSkillSpriteOn();
		if (this.IsSkillActivated(skill))
		{
			ViscusSnake.Log("Skill Activated");
			this.InfestAgent(skill.agent);
		}
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x00126254 File Offset: 0x00124454
	private void OnSkillActivateTimerExpired()
	{
		try
		{
			this.model.currentSkill.ResumeWorking();
			this.RoomSkillSpriteOff();
			ViscusSnake.Log("EndFilter");
		}
		catch (NullReferenceException message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public void InfestAgent(WorkerModel worker)
	{
		ViscusSnake.Log("<color=#FF2222>INFESTED</color> " + worker.GetUnitName());
		worker.AddUnitBuf(new ViscusSnakeBuf(this));
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x001262A4 File Offset: 0x001244A4
	private int GetSkillProb(UseSkill skill)
	{ // <Mod>
		int num = ViscusSnake.Result_ProbValue[(int)skill.GetCurrentFeelingState()];
		if (skill.agent.hp == 0f || skill.agent.maxHp == 0)
		{
			return 0;
		}
		float num2 = ((float)skill.agent.maxHp - skill.agent.hp) / (float)skill.agent.maxHp;
		num = (int)((float)num * num2);
        if (num < 3)
        {
            num = 0;
        }
		ViscusSnake.Log("Prob :" + num);
		return num;
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x0002A1FD File Offset: 0x000283FD
	private bool IsSkillActivated(UseSkill skill)
	{
		return this.Prob(this.GetSkillProb(skill));
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x0002A20C File Offset: 0x0002840C
	public override void OnEnterRoom(UseSkill skill)
	{
		this.AnimScript.StartWork();
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x0002A219 File Offset: 0x00028419
	public override void OnReleaseWork(UseSkill skill)
	{
		this.AnimScript.EndWork();
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x00022FCE File Offset: 0x000211CE
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		base.Unit.room.SkillFilter.specialAnimKey = "Display";
		base.Unit.room.SkillFilter.hasSpecialAnimKey = true;
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x0012632C File Offset: 0x0012452C
	public bool CheckInfection(WorkerModel worker)
	{
		int probability = 30;
		return this.Prob(probability);
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x0002A226 File Offset: 0x00028426
	public override void OnStageStart()
	{
		base.OnStageStart();
		this._defaultSoundTimer.StartTimer(this.DefaultSoundDelay);
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x0002A23F File Offset: 0x0002843F
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this._defaultSoundTimer.RunTimer())
		{
			this.MakeSound("creature/ViscusSnake/GutSnake_Default");
			this._defaultSoundTimer.StartTimer(this.DefaultSoundDelay);
		}
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x00126344 File Offset: 0x00124544
	public override SoundEffectPlayer MakeSound(string src)
	{
		return SoundEffectPlayer.PlayOnce(src, this.movable.GetCurrentViewPosition());
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x0002A26E File Offset: 0x0002846E
	// Note: this type is marked as 'beforefieldinit'.
	static ViscusSnake()
	{
	}

	// Token: 0x04002900 RID: 10496
	private const string DebugPrefix = "<color=#AAFFAA>[ViscusSnake]</color> ";

	// Token: 0x04002901 RID: 10497
	private const string _defaultSound = "creature/ViscusSnake/GutSnake_Default";

	// Token: 0x04002902 RID: 10498
	public const string InfectSound = "creature/ViscusSnake/Infest_Change";

	// Token: 0x04002903 RID: 10499
	public const string _infestedSound = "creature/ViscusSnake/Infest_Gas";

	// Token: 0x04002904 RID: 10500
	private static int[] Result_ProbValue = new int[]
	{
		0,
		0,
		30,
		60
	};

	// Token: 0x04002905 RID: 10501
	public const float SkillActivateTime = 4f;

	// Token: 0x04002906 RID: 10502
	private const int _infectionProb = 30;

	// Token: 0x04002907 RID: 10503
	private Timer _skillActivateTimer = new Timer();

	// Token: 0x04002908 RID: 10504
	private Vector3 originalScale;

	// Token: 0x04002909 RID: 10505
	private Timer _defaultSoundTimer = new Timer();
}
