using System;
using UnityEngine;
using WorkerSpine;

// Token: 0x0200040F RID: 1039
public class JusticeReceiver : CreatureBase
{
	// Token: 0x060023AB RID: 9131 RVA: 0x0001E726 File Offset: 0x0001C926
	public JusticeReceiver()
	{
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x000248C7 File Offset: 0x00022AC7
	public override void OnInit()
	{
		base.OnInit();
		this.kitEvent = new JusticeReceiver.JusticeReceiverKit(this);
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x00022B75 File Offset: 0x00020D75
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.kitEvent.OnViewInit(unit);
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x00022B8A File Offset: 0x00020D8A
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
	}

	// Token: 0x02000410 RID: 1040
	public class JusticeReceiverKit : CreatureBase.KitEquipEventListener
	{
		// Token: 0x060023AF RID: 9135 RVA: 0x000248DB File Offset: 0x00022ADB
		public JusticeReceiverKit(JusticeReceiver m)
		{
			this._model = m;
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000248EA File Offset: 0x00022AEA
		public override void OnViewInit(CreatureUnit unit)
		{
			base.OnViewInit(unit);
			this._anim = (KitEquipCreatureAnim)unit.animTarget;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x00108C50 File Offset: 0x00106E50
		public override void OnUseKit(AgentModel actor)
		{
			base.OnUseKit(actor);
			this._equipElapsedTime = 0f;
			this._anim.OnEquip();
			float num = actor.mental / (float)actor.maxMental;
			actor.AddUnitBuf(new JusticeReceiver.JusticeReceiverEquipBuf(15, -10));
			actor.mental = (float)actor.maxMental * num;
			this._equipAgent = actor;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x00108CB0 File Offset: 0x00106EB0
		public override void OnFixedUpdateInKitEquip(AgentModel actor)
		{
			base.OnFixedUpdateInKitEquip(actor);
			this._equipElapsedTime += Time.deltaTime;
			if (actor.mental <= 0f || actor.IsPanic())
			{
				actor.Die();
				AgentUnit unit = actor.GetUnit();
				unit.animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.JusticeReceiverAgentDead, false);
				unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.MakeAgentDeadSound));
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x00108D28 File Offset: 0x00106F28
		public override void OnReleaseKitEquip(AgentModel actor, bool stageEnd)
		{
			base.OnReleaseKitEquip(actor, stageEnd);
			this._anim.OnReturn();
			if (this._equipElapsedTime < 30f && !actor.invincible)
			{
				actor.Die();
				AgentUnit unit = actor.GetUnit();
				unit.animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.JusticeReceiverAgentDead, false);
				unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.MakeAgentDeadSound));
			}
			else
			{
				actor.RemoveUnitBuf(actor.GetUnitBufByType(UnitBufType.JUSTICE_RECEIVER));
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x00108DAC File Offset: 0x00106FAC
		private void MakeAgentDeadSound(int i)
		{
			Debug.LogError(i);
			if (i != 0)
			{
				if (i == 1)
				{
					SoundEffectPlayer.PlayOnce("creature/JusticeReceiver/Justice_Dead2", this._equipAgent.GetUnit().transform.position);
				}
			}
			else
			{
				SoundEffectPlayer.PlayOnce("creature/JusticeReceiver/Justice_Dead1", this._equipAgent.GetUnit().transform.position);
			}
		}

		// Token: 0x040022E5 RID: 8933
		private const float _LIMIT_USED_TIME = 30f;

		// Token: 0x040022E6 RID: 8934
		private const int _AMOUNT_JUSTICE = 15;

		// Token: 0x040022E7 RID: 8935
		private const int _AMOUNT_PRUDENCE = -10;

		// Token: 0x040022E8 RID: 8936
		private const string _SOUND_SRC_DEAD1 = "creature/JusticeReceiver/Justice_Dead1";

		// Token: 0x040022E9 RID: 8937
		private const string _SOUND_SRC_DEAD2 = "creature/JusticeReceiver/Justice_Dead2";

		// Token: 0x040022EA RID: 8938
		private JusticeReceiver _model;

		// Token: 0x040022EB RID: 8939
		private KitEquipCreatureAnim _anim;

		// Token: 0x040022EC RID: 8940
		private float _equipElapsedTime;

		// Token: 0x040022ED RID: 8941
		private AgentModel _equipAgent;
	}

	// Token: 0x02000411 RID: 1041
	public class JusticeReceiverEquipBuf : UnitStatBuf
	{
		// Token: 0x060023B5 RID: 9141 RVA: 0x00024904 File Offset: 0x00022B04
		public JusticeReceiverEquipBuf(int j, int p) : base(0f, UnitBufType.JUSTICE_RECEIVER)
		{
			this._amountJustice = j;
			this._amountPrudence = p;
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00108E2C File Offset: 0x0010702C
		public override void Init(UnitModel model)
		{
			base.Init(model);
			this.primaryStat.battle = this._amountJustice;
			this.primaryStat.mental = this._amountPrudence;
			this._bufMaxMental = model.maxMental + this.primaryStat.mental;
			if (model is WorkerModel)
			{
				WorkerModel workerModel = model as WorkerModel;
				WorkerUnit workerUnit = workerModel.GetWorkerUnit();
				if (workerUnit != null)
				{
					StatAdditionEffect.MakeEffect(RwbpType.B, workerUnit.animRoot);
					StatSubtractionEffect.MakeEffect(RwbpType.W, workerUnit.animRoot);
				}
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x00108EBC File Offset: 0x001070BC
		public override void OnDestroy()
		{
			base.OnDestroy();
			float num = this.model.mental / (float)this.model.maxMental;
			this.model.mental = (this.model.mental - (float)this.primaryStat.mental) * num;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void FixedUpdate()
		{
		}

		// Token: 0x040022EE RID: 8942
		private int _amountJustice;

		// Token: 0x040022EF RID: 8943
		private int _amountPrudence;

		// Token: 0x040022F0 RID: 8944
		private int _bufMaxMental;
	}
}
