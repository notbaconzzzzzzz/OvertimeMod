using System;
using System.Collections.Generic;
using UnityEngine;
using WorkerSpine;

// Token: 0x02000437 RID: 1079
public class OtherWorldPortrait : CreatureBase, IObserver
{
	// Token: 0x06002634 RID: 9780 RVA: 0x0001E726 File Offset: 0x0001C926
	public OtherWorldPortrait()
	{
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06002635 RID: 9781 RVA: 0x000268FB File Offset: 0x00024AFB
	public float insteadDmgRatio
	{
		get
		{
			if (this._accumulatedDmgValue >= 200f)
			{
				return 2.5f;
			}
			if (this._accumulatedDmgValue >= 100f)
			{
				return 2f;
			}
			return 1.5f;
		}
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x0002692E File Offset: 0x00024B2E
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._anim = (base.Unit.animTarget as OtherWorldPortraitAnim);
		this._anim.SetScript(this);
		this._anim.SetPortraitCanvasImg(-1);
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x00113ED4 File Offset: 0x001120D4
	public override void OnStageStart()
	{
		base.OnStageStart();
		this._targetAgent = null;
		this._victimAgent = null;
		Notice.instance.Observe(NoticeName.OnAgentPanic, this);
		Notice.instance.Observe(NoticeName.OnAgentDead, this);
		this._accumulatedDmgValue = 0f;
		this._anim.SetPortraitCanvasImg(-1);
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x00026965 File Offset: 0x00024B65
	public override void OnStageEnd()
	{
		base.OnStageEnd();
		Notice.instance.Remove(NoticeName.OnAgentPanic, this);
		Notice.instance.Remove(NoticeName.OnAgentDead, this);
		this._accumulatedDmgValue = 0f;
		this._anim.SetPortraitCanvasImg(-1);
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x00113F2C File Offset: 0x0011212C
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		if (this._targetAgent == null)
		{
			this._targetAgent = skill.agent;
		}
		else
		{
			if (this._targetAgent == skill.agent)
			{
				return;
			}
			this.KillPortraitAgent();
			this._targetAgent = skill.agent;
		}
		this._victimAgent = this.GetRandomAgent();
		if (this._victimAgent != null)
		{
			this._anim.SetPortraitCanvasImg(0);
			SoundEffectPlayer.PlayOnce("creature/OtherWorldPortrait/Portrait_Use", Time.timeScale, this._anim.transform.position);
			this._targetAgent.AddUnitBuf(new OtherWorldPortrait.OtherWorldPortraitBuf(this._victimAgent, this));
			this._targetEffect = this._anim.SetEffect(0, this._targetAgent.GetUnit().transform);
			this._victimEffect = this._anim.SetEffect(0, this._victimAgent.GetUnit().transform);
			this._anim.SetAgentSprite(this._targetAgent);
		}
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x0011403C File Offset: 0x0011223C
	public void ReleaseVictim(AgentModel agent)
	{
		this._victimAgent = this.GetRandomAgent();
		UnitBuf unitBufByType = agent.GetUnitBufByType(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM);
		if (unitBufByType != null)
		{
			agent.RemoveUnitBuf(unitBufByType);
		}
		if (this._victimAgent == null)
		{
			this.KillPortraitAgent();
		}
		else
		{
			UnityEngine.Object.Destroy(this._targetEffect);
			UnityEngine.Object.Destroy(this._victimEffect);
			OtherWorldPortrait.OtherWorldPortraitBuf otherWorldPortraitBuf = this._targetAgent.GetUnitBufByType(UnitBufType.OTHER_WORLD_PORTRAIT) as OtherWorldPortrait.OtherWorldPortraitBuf;
			otherWorldPortraitBuf.SetAnotherVictim(this._victimAgent);
			this._targetEffect = this._anim.SetEffect(0, this._targetAgent.GetUnit().transform);
			this._victimEffect = this._anim.SetEffect(0, this._victimAgent.GetUnit().transform);
		}
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x001140FC File Offset: 0x001122FC
	public void OnNotice(string notice, object[] param)
	{
		if (this._victimAgent == null)
		{
			return;
		}
		if (notice == NoticeName.OnAgentDead)
		{
			AgentModel agentModel = param[0] as AgentModel;
			if (agentModel == this._targetAgent)
			{
				this.KillPortraitAgent();
			}
			else if (agentModel == this._victimAgent)
			{
				this._victimAgent = this.GetRandomAgent();
				if (this._victimAgent == null)
				{
					this.KillPortraitAgent();
				}
				else
				{
					UnityEngine.Object.Destroy(this._targetEffect);
					UnityEngine.Object.Destroy(this._victimEffect);
					OtherWorldPortrait.OtherWorldPortraitBuf otherWorldPortraitBuf = this._targetAgent.GetUnitBufByType(UnitBufType.OTHER_WORLD_PORTRAIT) as OtherWorldPortrait.OtherWorldPortraitBuf;
					otherWorldPortraitBuf.SetAnotherVictim(this._victimAgent);
					this._targetEffect = this._anim.SetEffect(0, this._targetAgent.GetUnit().transform);
					this._victimEffect = this._anim.SetEffect(0, this._victimAgent.GetUnit().transform);
				}
			}
		}
		else if (notice == NoticeName.OnAgentPanic)
		{
			AgentModel agentModel2 = param[0] as AgentModel;
			if (agentModel2 == this._targetAgent)
			{
				this.KillPortraitAgent();
			}
		}
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x0011421C File Offset: 0x0011241C
	public void AccumulateDmg(float value)
	{
		this._accumulatedDmgValue += value;
		if (this._accumulatedDmgValue >= 200f)
		{
			this._anim.SetPortraitCanvasImg(2);
		}
		else if (this._accumulatedDmgValue >= 100f)
		{
			this._anim.SetPortraitCanvasImg(1);
		}
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x00114274 File Offset: 0x00112474
	private AgentModel GetRandomAgent()
	{
		IList<AgentModel> agentList = AgentManager.instance.GetAgentList();
		List<AgentModel> list = new List<AgentModel>();
		foreach (AgentModel agentModel in agentList)
		{
			if (agentModel != this._targetAgent)
			{
				if (agentModel != this._victimAgent)
				{
					if (!agentModel.IsDead())
					{
						if (!agentModel.invincible)
						{
							if (!agentModel.CannotControll())
							{
								if (agentModel.unconAction == null)
								{
									list.Add(agentModel);
								}
							}
						}
					}
				}
			}
		}
		if (list.Count < 1)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x00114364 File Offset: 0x00112564
	private void KillPortraitAgent()
	{
		UnityEngine.Object.Destroy(this._targetEffect);
		UnityEngine.Object.Destroy(this._victimEffect);
		this._anim.SetPortraitCanvasImg(-1);
		if (!this._targetAgent.invincible)
		{
			AgentUnit unit = this._targetAgent.GetUnit();
			SoundEffectPlayer.PlayOnce("creature/OtherWorldPortrait/Portrait_DeadScene1", Time.timeScale, this._targetAgent.GetUnit().transform.position);
			unit.animChanger.ChangeAnimator(WorkerSpine.AnimatorName.OtherWorldPortraitDead);
			unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.OnAgentDeadAnimEvent));
			this._targetAgent.Die();
			this.deadEffectRef = this._targetAgent.GetUnit().transform;
		}
		else
		{
			this._targetAgent.RemoveUnitBuf(this._targetAgent.GetUnitBufByType(UnitBufType.OTHER_WORLD_PORTRAIT));
		}
		this._victimAgent = null;
		this._accumulatedDmgValue = 0f;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000269A4 File Offset: 0x00024BA4
	private void OnAgentDeadAnimEvent(int i)
	{
		if (i == 0)
		{
			this._anim.SetEffect(1, this.deadEffectRef);
			SoundEffectPlayer.PlayOnce("creature/OtherWorldPortrait/Portrait_DeadScene2", Time.timeScale, this.deadEffectRef.position);
		}
	}

	// Token: 0x04002567 RID: 9575
	private const string _SOUND_SRC = "creature/OtherWorldPortrait/Portrait_";

	// Token: 0x04002568 RID: 9576
	private const float _LIMIT_CHANGE_PORTRAIT_1 = 100f;

	// Token: 0x04002569 RID: 9577
	private const float _LIMIT_CHANGE_PORTRAIT_2 = 200f;

	// Token: 0x0400256A RID: 9578
	private AgentModel _targetAgent;

	// Token: 0x0400256B RID: 9579
	private AgentModel _victimAgent;

	// Token: 0x0400256C RID: 9580
	private OtherWorldPortraitAnim _anim;

	// Token: 0x0400256D RID: 9581
	private float _accumulatedDmgValue;

	// Token: 0x0400256E RID: 9582
	private GameObject _targetEffect;

	// Token: 0x0400256F RID: 9583
	private GameObject _victimEffect;

	// Token: 0x04002570 RID: 9584
	private Transform deadEffectRef;

	// Token: 0x02000438 RID: 1080
	public class OtherWorldPortraitBuf : UnitStatBuf
	{
		// Token: 0x06002640 RID: 9792 RVA: 0x000269DF File Offset: 0x00024BDF
		public OtherWorldPortraitBuf(AgentModel am, OtherWorldPortrait c) : base(0f, UnitBufType.OTHER_WORLD_PORTRAIT)
		{
			this._victim = am;
			this._creature = c;
			this._victim.AddUnitBuf(new OtherWorldPortrait.OtherWorldPortraitVictimBuf());
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x00114450 File Offset: 0x00112650
		public void SetAnotherVictim(AgentModel am)
		{
			this._victim = am;
			this._victim.AddUnitBuf(new OtherWorldPortrait.OtherWorldPortraitVictimBuf());
			this._victimBufData = this._victim.GetWorkerUnit().AddUnitBuf(this, this._creature._anim.bufRenderer, true);
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x001144A0 File Offset: 0x001126A0
		public override void Init(UnitModel model)
		{
			base.Init(model);
			if (model is AgentModel)
			{
				AgentModel agentModel = model as AgentModel;
				agentModel.GetWorkerUnit().AddUnitBuf(this, this._creature._anim.bufRenderer, true);
				this._victimBufData = this._victim.GetWorkerUnit().AddUnitBuf(this, this._creature._anim.bufRenderer, true);
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x00024529 File Offset: 0x00022729
		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00026A0D File Offset: 0x00024C0D
		public override void OnUnitDie()
		{
			base.OnUnitDie();
			this._victim.GetWorkerUnit().bufUI.RemoveBuf(UnitBufType.OTHER_WORLD_PORTRAIT);
			this._victim.RemoveUnitBuf(this._victim.GetUnitBufByType(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM));
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void FixedUpdate()
		{
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x0011450C File Offset: 0x0011270C
		public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
		{
			float num = 1f;
			if (attacker != null)
			{
				num = UnitModel.GetDmgMultiplierByEgoLevel(attacker.GetAttackLevel(), this.model.GetDefenseLevel());
			}
			float num2 = damageInfo.GetDamageWithDefenseInfo(this.model.defense) * num;
			if (damageInfo.type == RwbpType.R && num2 >= (float)this.model.maxHp)
			{
				return 1f;
			}
			if (damageInfo.type == RwbpType.W && num2 >= (float)this.model.maxMental)
			{
				return 1f;
			}
			if (damageInfo.type == RwbpType.B && (num2 >= (float)this.model.maxMental || num2 >= (float)this.model.maxHp))
			{
				return 1f;
			}
			if (this._victim.IsDead())
			{
				Debug.LogError("portrait buf remains");
				this.Destroy();
				return 1f;
			}
			this._creature.AccumulateDmg(num2);
			DamageInfo damageInfo2 = damageInfo.Copy();
			damageInfo2.min = damageInfo.min * this._creature.insteadDmgRatio;
			damageInfo2.max = damageInfo.max * this._creature.insteadDmgRatio;
			this._victim.TakeDamage(damageInfo2);
			return 0f;
		}

		// Token: 0x04002571 RID: 9585
		public AgentModel _victim;

		// Token: 0x04002572 RID: 9586
		public BufStateUI.BufData _victimBufData;

		// Token: 0x04002573 RID: 9587
		public OtherWorldPortrait _creature;
	}

	// Token: 0x02000439 RID: 1081
	public class OtherWorldPortraitVictimBuf : UnitBuf
	{
		// Token: 0x06002647 RID: 9799 RVA: 0x00026A44 File Offset: 0x00024C44
		public OtherWorldPortraitVictimBuf()
		{
			this.type = UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void FixedUpdate()
		{
		}
	}
}
