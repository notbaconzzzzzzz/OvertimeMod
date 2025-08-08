using System;
using UnityEngine;
using WorkerSprite;

// Token: 0x02000496 RID: 1174
public class SpiderMom : CreatureBase
{
	// Token: 0x06002ABC RID: 10940 RVA: 0x00029D76 File Offset: 0x00027F76
	public SpiderMom()
	{
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06002ABD RID: 10941 RVA: 0x00029D94 File Offset: 0x00027F94
	public SpiderMomAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = this.model.GetAnimScript();
			}
			return this._animScript as SpiderMomAnim;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002ABE RID: 10942 RVA: 0x00029DC3 File Offset: 0x00027FC3
	public SpiderMomCocoonScript cocoonScript
	{
		get
		{
			if (this._cocoonScript == null && this.animScript != null)
			{
				this._cocoonScript = this.animScript.cocoonScript;
			}
			return this._cocoonScript;
		}
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x00125CB8 File Offset: 0x00123EB8
	public override void ParamInit()
	{
		this.animScript.SetScript(this);
		this.cocoonScript.Init();
		this.isKilling = false;
		this.eventStarted = false;
		this.target = null;
		this.cocoonCount = 0;
		this.skillActivateTimer.StopTimer();
		this.cocoonTimer.StopTimer();
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x00029DFE File Offset: 0x00027FFE
	public override void OnStageStart()
	{
		if (this.amb == null)
		{
			this.amb = base.Unit.PlaySoundLoop(SpiderMom.SpiderMomSoundKey.loop);
		}
		this.ParamInit();
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x00125D10 File Offset: 0x00123F10
	public override void OnFinishWork(UseSkill skill)
	{
		skill.successCount += this.cocoonCount;
		Debug.Log("successCount : " + skill.successCount);
		if (!skill.agent.invincible && (skill.skillTypeInfo.id == 2L || skill.agent.prudenceLevel < 2))
		{
			this.target = skill.agent;
			this.ActivateSkill(skill);
			skill.PauseWorking();
		}
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x00125D98 File Offset: 0x00123F98
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this.skillActivateTimer.RunTimer() && this.isKilling)
		{
			this.isKilling = false;
			this.PickUpAgent();
		}
		if (this.cocoonTimer.RunTimer())
		{
			this.MakeCocoon();
		}
		if (this.target != null && !this.isKilling && this.target.GetCurrentNode() != null && this.target.GetCurrentNode() == this.model.GetCustomNode() && this.eventStarted)
		{
			this.eventStarted = false;
			this.target.animationMessageRecevied = this;
			AgentUnit unit = this.target.GetUnit();
			this.isKilling = true;
			unit.BlockMove();
			this.target.SetWorkerFaceType(WorkerFaceType.PANIC);
			this.target.workerAnimator.SetBool("UniqueDead", true);
			this.target.workerAnimator.SetBool("Panic", true);
			this.target.workerAnimator.SetTrigger("Suicide");
			this.RoomSkillSpriteOn();
		}
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x00125EB0 File Offset: 0x001240B0
	public override void OnStageRelease()
	{
		if (this.amb != null)
		{
			this.amb.Stop();
			this.amb = null;
		}
		if (this.target != null)
		{
			this.target.Die();
		}
		this.ParamInit();
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x00125EFC File Offset: 0x001240FC
	private void ActivateSkill(UseSkill skill)
	{
		this.target = skill.agent;
		this.eventStarted = true;
		this.animScript.SpiderMomMove(true);
		base.Unit.PlaySound(SpiderMom.SpiderMomSoundKey.spiderMomMove);
		this.skillActivateTimer.StartTimer(10f);
		this.cocoonCount++;
		this.target.LoseControl();
		this.target.MoveToNode(this.model.GetCustomNode(), false);
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x00029E2D File Offset: 0x0002802D
	private void PickUpAgent()
	{
		this.animScript.PickAgent(this.target.GetUnit());
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x00029E45 File Offset: 0x00028045
	public void PickUpSound()
	{
		base.Unit.PlaySound(SpiderMom.SpiderMomSoundKey.pickUp);
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x00029E58 File Offset: 0x00028058
	public void StartCocoonDelay()
	{
		this.cocoonTimer.StartTimer(5f);
		base.Unit.PlaySound(SpiderMom.SpiderMomSoundKey.eat);
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x00125F7C File Offset: 0x0012417C
	private void MakeCocoon()
	{
		this.target.Die();
		this.target = null;
		if (this.model.state == CreatureState.WORKING_SCENE)
		{
			this.model.state = CreatureState.WAIT;
		}
		this.cocoonScript.SendCocoon();
		base.Unit.PlaySound(SpiderMom.SpiderMomSoundKey.cocoon);
		this.RoomSkillSpriteOff();
	}

	// Token: 0x040028D7 RID: 10455
	private const long targetSkill = 2L;

	// Token: 0x040028D8 RID: 10456
	private bool isKilling;

	// Token: 0x040028D9 RID: 10457
	private bool eventStarted;

	// Token: 0x040028DA RID: 10458
	private int cocoonCount;

	// Token: 0x040028DB RID: 10459
	private Timer skillActivateTimer = new Timer();

	// Token: 0x040028DC RID: 10460
	private const float skillTime = 10f;

	// Token: 0x040028DD RID: 10461
	private Timer cocoonTimer = new Timer();

	// Token: 0x040028DE RID: 10462
	private const float cocoonTime = 5f;

	// Token: 0x040028DF RID: 10463
	private AgentModel target;

	// Token: 0x040028E0 RID: 10464
	private SoundEffectPlayer amb;

	// Token: 0x040028E1 RID: 10465
	private CreatureAnimScript _animScript;

	// Token: 0x040028E2 RID: 10466
	private SpiderMomCocoonScript _cocoonScript;

	// Token: 0x02000497 RID: 1175
	public class SpiderMomSoundKey
	{
		// Token: 0x06002AC9 RID: 10953 RVA: 0x000042F0 File Offset: 0x000024F0
		public SpiderMomSoundKey()
		{
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00029E7B File Offset: 0x0002807B
		// Note: this type is marked as 'beforefieldinit'.
		static SpiderMomSoundKey()
		{
		}

		// Token: 0x040028E3 RID: 10467
		public static string loop = "loop";

		// Token: 0x040028E4 RID: 10468
		public static string childTraple = "childTrample";

		// Token: 0x040028E5 RID: 10469
		public static string eat = "eat";

		// Token: 0x040028E6 RID: 10470
		public static string spiderMomMove = "spiderMomMove";

		// Token: 0x040028E7 RID: 10471
		public static string pickUp = "pickUp";

		// Token: 0x040028E8 RID: 10472
		public static string cocoon = "cocoon";
	}
}
