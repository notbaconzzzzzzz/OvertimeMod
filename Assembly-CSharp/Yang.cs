using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class Yang : YinAndYangBase
{
	// Token: 0x06002B42 RID: 11074 RVA: 0x0002A2D1 File Offset: 0x000284D1
	public Yang()
	{
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x001270B8 File Offset: 0x001252B8
	public void Escape(MapNode dst)
	{
		this.model.Escape();
		try
		{
			base.DstNode = dst;
			if (this.equipAgent.GetCurrentNode() != null)
			{
				this.model.SetCurrentNode(this.equipAgent.GetCurrentNode());
			}
			else if (this.equipAgent.GetCurrentEdge() != null)
			{
				this.movable.SetCurrentEdge(this.equipAgent.GetMovableNode());
			}
			this._animScript.Vanish(this.equipAgent.GetUnit().transform);
			this.equipAgent.ReleaseKitCreature(false);
		}
		catch
		{
			Debug.Log("No one equips Yang");
		}
		this.model.movementScale = 1f;
		this._animScript.ActivateObj(true);
		base.MoveToNode(base.DstNode);
		this._isEscaped = true;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x0002A2E4 File Offset: 0x000284E4
	public void Die()
	{
		this._animScript.PlayAnimation("Dead");
		this.model.suppressReturnTimer = 0f;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x0002A306 File Offset: 0x00028506
	internal void DecreaseYinQliphoth()
	{
		if (this._yinBase != null)
		{
			this._yinBase.model.SubQliphothCounter();
		}
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x001271A4 File Offset: 0x001253A4
	private void HealingUnits()
	{
		this._elapsedTime = 0f;
		if (!this._isEscaped || base.UnitList.Count == 0)
		{
			return;
		}
		SoundEffectPlayer.PlayOnce(this._SOUND_SRC + "YangHit", this._animScript.transform.position);
		this._animScript.MakeAuraEffect();
		foreach (object obj in base.UnitList)
		{
			if (obj is OfficerModel)
			{
				OfficerModel officerModel = obj as OfficerModel;
				if (officerModel.IsDead())
				{
					break;
				}
				officerModel.RecoverHP(20f);
				officerModel.RecoverMental(20f);
			}
			else if (obj is AgentModel)
			{
				AgentModel agentModel = obj as AgentModel;
				if (agentModel.IsDead())
				{
					break;
				}
				agentModel.RecoverHP(20f);
				agentModel.RecoverMental(20f);
			}
			else if (obj is RabbitModel)
			{
				RabbitModel rabbitModel = obj as RabbitModel;
				if (rabbitModel.IsAttackTargetable())
				{
					rabbitModel.hp += 20f;
					if (rabbitModel.hp > (float)rabbitModel.maxHp)
					{
						rabbitModel.hp = (float)rabbitModel.maxHp;
					}
					rabbitModel.mental += 20f;
					if (rabbitModel.mental > (float)rabbitModel.maxMental)
					{
						rabbitModel.mental = (float)rabbitModel.maxMental;
					}
				}
			}
		}
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x0002A323 File Offset: 0x00028523
	public override void Unite()
	{
		base.Unite();
		this._animScript.Vanish();
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x00127368 File Offset: 0x00125568
	protected override void Arrive()
	{
		base.Arrive();
		PassageObjectModel currentPassage = this.model.GetMovableNode().currentPassage;
		MapNode node;
		if (currentPassage == this._yinBase.model.GetMovableNode().currentPassage)
		{
			node = base.DstNode;
		}
		else
		{
			MapNode[] nodeList = currentPassage.GetNodeList();
			node = nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
		}
		base.MoveToNode(node);
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x0002A336 File Offset: 0x00028536
	public override void Revive()
	{
		base.Revive();
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x0002A33E File Offset: 0x0002853E
	protected override void ProcessCollision()
	{
		base.ProcessCollision();
		this.HealingUnits();
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x0002A34C File Offset: 0x0002854C
	protected override void ResetFields()
	{
		base.ResetFields();
		this._reviveTimer.StopTimer();
		this._elapsedTime = 0f;
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x0002A36A File Offset: 0x0002856A
	public override void OnInit()
	{
		base.OnInit();
		this.kitEvent = new Yang.YangKit(this);
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x0002A37E File Offset: 0x0002857E
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.kitEvent.OnViewInit(unit);
		this._animScript = (YangAnim)this.model.Unit.animTarget;
		this._animScript.SetScript(this);
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x001273D0 File Offset: 0x001255D0
	public override void OnStageStart()
	{
		base.OnStageStart();
		this._yinBase = (base.GetCreatureBase(typeof(Yin)) as Yin);
		if (this._yinBase != null)
		{
			this.hasYin = true;
		}
		else
		{
			this.hasYin = false;
		}
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x0012741C File Offset: 0x0012561C
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (!this._isEscaped)
		{
			return;
		}
		if (this._elapsedTime >= 3f)
		{
			this.HealingUnits();
		}
		else
		{
			this._elapsedTime += Time.deltaTime;
		}
		if (this._reviveTimer.RunTimer() && this._yinBase.model.state != CreatureState.SUPPRESSED)
		{
			this._animScript.PlayAnimation("Revive");
			SoundEffectPlayer.PlayOnce(this._SOUND_SRC + "Change", this._animScript.transform.position);
		}
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x0002A3BA File Offset: 0x000285BA
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return this.model.state != CreatureState.SUPPRESSED;
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x001274CC File Offset: 0x001256CC
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (actor != null)
		{
			DamageInfo damageInfo = dmg.Copy();
			damageInfo.type = RwbpType.W;
			actor.TakeDamage(damageInfo);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(actor, damageInfo.type, this.model);
			this._animScript.MakeReflectionEffect();
			SoundEffectPlayer.PlayOnce(this._SOUND_SRC + "Reflect", this._animScript.transform.position);
		}
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x00013D75 File Offset: 0x00011F75
	public override bool UniqueMoveControl()
	{
		return true;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x00013D75 File Offset: 0x00011F75
	public override bool OnAfterSuppressed()
	{
		return true;
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x00127548 File Offset: 0x00125748
	public override void OnSuppressed()
	{
		base.OnSuppressed();
		this.model.ClearCommand();
		if (base.IsUnion)
		{
			return;
		}
		if (this._yinBase.model.state == CreatureState.SUPPRESSED)
		{
			this.Die();
			this._yinBase.Die();
		}
		else
		{
			this._animScript.PlayAnimation("DeathBrink");
			SoundEffectPlayer.PlayOnce(this._SOUND_SRC + "Dead2", this._animScript.transform.position);
			this.model.suppressReturnTimer = float.NegativeInfinity;
			this._reviveTimer.StartTimer(30f);
		}
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x0002A3D0 File Offset: 0x000285D0
	public override void OnReturn()
	{
		base.OnReturn();
		this._animScript.ActivateObj(false);
		this.ResetFields();
	}

	// Token: 0x0400293B RID: 10555
	internal AgentModel equipAgent;

	// Token: 0x0400293C RID: 10556
	internal bool hasYin;

	// Token: 0x0400293D RID: 10557
	private const float _TIME_REVIVAL = 30f;

	// Token: 0x0400293E RID: 10558
	private const float _AMOUNT_RECOVERY = 20f;

	// Token: 0x0400293F RID: 10559
	private const float _FREQ_RECOVERY = 3f;

	// Token: 0x04002940 RID: 10560
	private float _elapsedTime;

	// Token: 0x04002941 RID: 10561
	private Yin _yinBase;

	// Token: 0x04002942 RID: 10562
	private YangAnim _animScript;

	// Token: 0x04002943 RID: 10563
	private bool _isEscaped;

	// Token: 0x04002944 RID: 10564
	private Timer _reviveTimer = new Timer();

	// Token: 0x020004A3 RID: 1187
	private class YangKit : CreatureBase.KitEquipEventListener
	{
		// Token: 0x06002B56 RID: 11094 RVA: 0x0002A3EA File Offset: 0x000285EA
		public YangKit(Yang m)
		{
			this._model = m;
			this._originAdditionalDef = 0f;
			this._equipElapsedTime = 0f;
			this._yinQliphothElapsedTime = 0f;
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x0002A41A File Offset: 0x0002861A
		public override void OnViewInit(CreatureUnit unit)
		{
			base.OnViewInit(unit);
			this._anim = (unit.animTarget as KitEquipCreatureAnim);
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x001275FC File Offset: 0x001257FC
		public override void OnUseKit(AgentModel actor)
		{
			base.OnUseKit(actor);
			this._anim.OnEquip();
			if (this._model.hasYin)
			{
				this._originAdditionalDef = actor.additionalDef.W;
				float w = actor.defense.W;
				actor.additionalDef.W = 0.1f / w;
				actor.AddUnitBuf(new YangEquipBuf());
			}
			this._equipElapsedTime = 0f;
			this._yinQliphothElapsedTime = 0f;
			this._model.equipAgent = actor;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x00127688 File Offset: 0x00125888
		public override void OnReleaseKitEquip(AgentModel actor, bool stageEnd)
		{
			base.OnReleaseKitEquip(actor, stageEnd);
			this._anim.OnReturn();
			if (this._model.hasYin)
			{
				actor.additionalDef.W = this._originAdditionalDef;
				actor.RemoveUnitBuf(actor.GetUnitBufByType(UnitBufType.YANG));
			}
			this._equipElapsedTime = 0f;
			this._yinQliphothElapsedTime = 0f;
			this._model.equipAgent = null;
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x001276FC File Offset: 0x001258FC
		public override void OnFixedUpdateInKitEquip(AgentModel actor)
		{
			base.OnFixedUpdateInKitEquip(actor);
			if (!this._model.hasYin)
			{
				return;
			}
			if (this._equipElapsedTime >= 1f)
			{
				actor.RecoverMental(10f);
				this._equipElapsedTime = 0f;
			}
			else
			{
				this._equipElapsedTime += Time.deltaTime;
			}
			if (this._yinQliphothElapsedTime >= 30f)
			{
				this._model.DecreaseYinQliphoth();
				this._yinQliphothElapsedTime = 0f;
			}
			else
			{
				this._yinQliphothElapsedTime += Time.deltaTime;
			}
		}

		// Token: 0x04002945 RID: 10565
		private const float _VALUE_WHITE = 0.1f;

		// Token: 0x04002946 RID: 10566
		private const float _FREQ_RECOVERY = 1f;

		// Token: 0x04002947 RID: 10567
		private const float _AMOUNT_RECOVERY = 10f;

		// Token: 0x04002948 RID: 10568
		private const float _FREQ_DECREASE_YANG_QLIPHOTH = 30f;

		// Token: 0x04002949 RID: 10569
		private Yang _model;

		// Token: 0x0400294A RID: 10570
		private float _originAdditionalDef;

		// Token: 0x0400294B RID: 10571
		private float _equipElapsedTime;

		// Token: 0x0400294C RID: 10572
		private float _yinQliphothElapsedTime;

		// Token: 0x0400294D RID: 10573
		private KitEquipCreatureAnim _anim;
	}
}
