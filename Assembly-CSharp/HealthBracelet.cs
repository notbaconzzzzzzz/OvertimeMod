using System;
using UnityEngine;
using WorkerSpine;

// Token: 0x02000406 RID: 1030
public class HealthBracelet : CreatureBase
{
	// Token: 0x0600234B RID: 9035 RVA: 0x0001E031 File Offset: 0x0001C231
	public HealthBracelet()
	{
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x00023C69 File Offset: 0x00021E69
	public override void OnInit()
	{
		base.OnInit();
		this.kitEvent = new HealthBracelet.HealthBraceletKit(this);
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x00022348 File Offset: 0x00020548
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.kitEvent.OnViewInit(unit);
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x0002235D File Offset: 0x0002055D
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
	}

	// Token: 0x02000407 RID: 1031
	public class HealthBraceletKit : CreatureBase.KitEquipEventListener
	{
		// Token: 0x0600234F RID: 9039 RVA: 0x00023C7D File Offset: 0x00021E7D
		public HealthBraceletKit(HealthBracelet m)
		{
			this._model = m;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x00023CA2 File Offset: 0x00021EA2
		public override void OnViewInit(CreatureUnit unit)
		{
			base.OnViewInit(unit);
			this._anim = (KitEquipCreatureAnim)unit.animTarget;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x001050CC File Offset: 0x001032CC
		public override void OnUseKit(AgentModel actor)
		{
			base.OnUseKit(actor);
			this._equipElapsedTime = 0f;
			this._anim.OnEquip();
			float num = actor.hp / (float)actor.maxHp;
			actor.AddUnitBuf(new HealthBracelet.HealthBraceletEquipBuf(_AMOUNT_BUF_FORTITUDE));
			actor.hp = (float)actor.maxHp * num;
			this._safeTimer.StartTimer(_TIME_SAFETY);
			this._checkMaxHp = false;
			this._recoveryTimer.StartTimer(_FREQ_RECOVERY);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0010514C File Offset: 0x0010334C
		public override void OnFixedUpdateInKitEquip(AgentModel actor)
		{
			if (this._recoveryTimer.RunTimer())
			{
				actor.RecoverHP(_AMOUNT_RECOVERY);
				this._recoveryTimer.StartTimer();
			}
			if (this._safeTimer.RunTimer())
			{
				this._checkMaxHp = true;
			}
			if (this._checkMaxHp && actor.hp >= (float)actor.maxHp)
			{
				this._equipElapsedTime += Time.deltaTime;
			}
			else
			{
				this._equipElapsedTime = 0f;
			}
			if (this._equipElapsedTime >= _TIME_DEAD && !actor.invincible)
			{
				AgentUnit unit = actor.GetUnit();
				unit.animChanger.ChangeAnimator(WorkerSpine.AnimatorName.HealthBraceletDead);
				actor.Die();
			}
			base.OnFixedUpdateInKitEquip(actor);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x00105214 File Offset: 0x00103414
		public override void OnReleaseKitEquip(AgentModel actor, bool stageEnd)
		{
			base.OnReleaseKitEquip(actor, stageEnd);
			this._anim.OnReturn();
			float num = actor.hp / (float)actor.maxHp;
			actor.RemoveUnitBuf(actor.GetUnitBufByType(UnitBufType.HEALTH_BRACELET));
			actor.hp = (float)actor.maxHp * num;
			if (actor.hp < (float)actor.maxHp && !actor.invincible)
			{
				AgentUnit unit = actor.GetUnit();
				unit.animChanger.ChangeAnimator(WorkerSpine.AnimatorName.HealthBraceletDead);
				actor.Die();
			}
		}

		// Token: 0x0400229B RID: 8859
		private const float _TIME_SAFETY = 15f;

		// Token: 0x0400229C RID: 8860
		private const float _TIME_DEAD = 60f;

		// Token: 0x0400229D RID: 8861
		private const int _AMOUNT_BUF_FORTITUDE = 15;

		// Token: 0x0400229E RID: 8862
		private const float _FREQ_RECOVERY = 3f;

		// Token: 0x0400229F RID: 8863
		private const float _AMOUNT_RECOVERY = 2.4f;

		// Token: 0x040022A0 RID: 8864
		private HealthBracelet _model;

		// Token: 0x040022A1 RID: 8865
		private KitEquipCreatureAnim _anim;

		// Token: 0x040022A2 RID: 8866
		private float _equipElapsedTime;

		// Token: 0x040022A3 RID: 8867
		private Timer _safeTimer = new Timer();

		// Token: 0x040022A4 RID: 8868
		private bool _checkMaxHp;

		// Token: 0x040022A5 RID: 8869
		private Timer _recoveryTimer = new Timer();
	}

	// Token: 0x02000408 RID: 1032
	public class HealthBraceletEquipBuf : UnitStatBuf
	{
		// Token: 0x06002354 RID: 9044 RVA: 0x00023CBC File Offset: 0x00021EBC
		public HealthBraceletEquipBuf(int a) : base(0f, UnitBufType.HEALTH_BRACELET)
		{
			this.amount = a;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0010529C File Offset: 0x0010349C
		public override void Init(UnitModel model)
		{
			base.Init(model);
			this.primaryStat.hp = this.amount;
			if (model is WorkerModel)
			{
				WorkerModel workerModel = model as WorkerModel;
				WorkerUnit workerUnit = workerModel.GetWorkerUnit();
				if (workerUnit != null)
				{
					StatAdditionEffect.MakeEffect(RwbpType.R, workerUnit.animRoot);
				}
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x00023CD2 File Offset: 0x00021ED2
		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x00003E35 File Offset: 0x00002035
		public override void FixedUpdate()
		{
		}

		// Token: 0x040022A6 RID: 8870
		private int amount;
	}
}
