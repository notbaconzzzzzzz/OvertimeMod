using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class MachineDawn : MachineOrdealCreature
{
	// Token: 0x060024BB RID: 9403 RVA: 0x000256FE File Offset: 0x000238FE
	public MachineDawn()
	{
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x0002571C File Offset: 0x0002391C
	public override void OnInit()
	{
		base.OnInit();
		this.model.SetWeapon(WeaponModel.MakeWeapon(this.model.metaInfo.creatureSpecialDamageTable.GetSpecialWeapon("1")));
		this._killMotionTarget = null;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x0010B9D4 File Offset: 0x00109BD4
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.anim = (MachineDawnAnim)unit.animTarget;
		this.anim.SetScript(this);
		if (this._ordealScript is MachineDuskOrdeal || this._ordealScript is MachineMidnightOrdeal)
		{
			this.spawnByDuskTimer.StartTimer(MachineOrdealCreature.spawnByDuskTime);
		}
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x0010BA28 File Offset: 0x00109C28
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.state != CreatureState.ESCAPE)
		{
			return;
		}
		this.model.CheckNearWorkerEncounting();
		if (this.spawnByDuskTimer.started)
		{
			this.model.movementScale = 15f / this.model.metaInfo.speed;
			this.movable.StopMoving();
			this.movable.MoveBy(UnitDirection.RIGHT, 10f);
			if (this.spawnByDuskTimer.RunTimer())
			{
				this.model.movementScale = 1f;
			}
			return;
		}
		this.killMotionAfterDelayTimer.RunTimer();
		if (this.killMotionAfterDelayTimer.started)
		{
			if (_killMotionTarget == null)
			{
				movable.StopMoving();
				model.ClearCommand();
				return;
			}
			MovableObjectNode node = _killMotionTarget.GetMovableNode();
			if (node == null || node.currentPassage == null || movable.currentPassage == null || node.currentPassage != movable.currentPassage)
			{
				movable.StopMoving();
				model.ClearCommand();
				return;
			}
			float num = (node.GetCurrentViewPosition().x - movable.GetCurrentViewPosition().x) / movable.currentScale;
			if (num > 2f || num < -2f) return;
			movable.StopMoving();
			model.ClearCommand();
			return;
		}
		if (this.model.GetCurrentCommand() == null)
		{
			this.MakeMovement();
		}
		else
		{
			UnitModel nearTargetUnit = this.GetNearTargetUnit();
			CreatureCommand creatureCurrentCmd = this.model.GetCreatureCurrentCmd();
			if (nearTargetUnit != null && ((creatureCurrentCmd is AttackCommand_creature && (creatureCurrentCmd as AttackCommand_creature).GetTarget() != nearTargetUnit) || !(creatureCurrentCmd is AttackCommand_creature)))
			{
				this.model.AttackTarget(new AttackCommand_creature(nearTargetUnit));
			}
		}
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x00022010 File Offset: 0x00020210
	private void MakeMovement()
	{
		this.model.MoveToNode(MapGraph.instance.GetCreatureRoamingPoint());
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x0010BB5C File Offset: 0x00109D5C
	public virtual UnitModel GetNearTargetUnit()
	{
		List<UnitModel> list = new List<UnitModel>();
		MovableObjectNode movableNode = this.model.GetMovableNode();
		PassageObjectModel passage = movableNode.GetPassage();
		if (passage != null)
		{
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				CreatureModel creatureModel = unit as CreatureModel;
				if (unit != null)
				{
					if (unit.IsAttackTargetable())
					{
						if (this.model.IsHostile(unit))
						{
							if (creatureModel == null || !(creatureModel.script is MachineOrdealCreature))
							{
								list.Add(unit);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				UnitModel unitModel = null;
				float num = 100000f;
				foreach (UnitModel unitModel2 in list)
				{
					float magnitude = (unitModel2.GetCurrentViewPosition() - this.model.GetCurrentViewPosition()).magnitude;
					if (num > magnitude)
					{
						num = magnitude;
						unitModel = unitModel2;
					}
				}
				if (unitModel != null)
				{
					return unitModel;
				}
			}
		}
		return null;
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x00025755 File Offset: 0x00023955
	public void KillMotion(UnitModel target)
	{
		this._killMotionTarget = target;
		this.killMotionAfterDelayTimer.StartTimer(this.killMotionAfterDelay);
		this.anim.KillMotion();
		model.ClearCommand();
		movable.StopMoving();
		MovableObjectNode node = target.GetMovableNode();
		if (node == null || node.currentPassage == null || movable.currentPassage == null || node.currentPassage != movable.currentPassage) return;
		float num = (node.GetCurrentViewPosition().x - movable.GetCurrentViewPosition().x) / movable.currentScale;
		UnitDirection direction = UnitDirection.RIGHT;
		if (num < 0f)
		{
			direction = UnitDirection.LEFT;
			num *= -1;
		}
		if (num <= 2f) return;
		movable.StopMoving();
		movable.MoveBy(direction, num - 2f);
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x0010BCD4 File Offset: 0x00109ED4
	public virtual void BloodEffect()
	{
		List<UnitModel> list = new List<UnitModel>();
		MovableObjectNode movableNode = this.model.GetMovableNode();
		PassageObjectModel passage = movableNode.GetPassage();
		if (passage != null)
		{
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit != null)
				{
					if (unit.IsAttackTargetable())
					{
						if (this.model.IsHostile(unit))
						{
							if (unit is WorkerModel)
							{
								list.Add(unit);
							}
						}
					}
				}
			}
			foreach (UnitModel unitModel in list)
			{
				unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.W, 3f));
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.W, this.model);
			}
		}
		if (this._killMotionTarget != null)
		{
			DamageParticleEffect damageParticleEffect2 = DamageParticleEffect.Invoker(this._killMotionTarget, RwbpType.R, DefenseInfo.zero);
			damageParticleEffect2.transform.localPosition = damageParticleEffect2.transform.localPosition + new Vector3(0f, -1f * this.model.GetMovableNode().currentScale, 0f);
		}
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x0002577A File Offset: 0x0002397A
	public virtual void FinishEffect()
	{
		this.BloodEffect();
		if (this._killMotionTarget is WorkerModel)
		{
			this.MakeExplodeEffect(this._killMotionTarget as WorkerModel, 1f);
		}
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000F8724 File Offset: 0x000F6924
	public void MakeExplodeEffect(WorkerModel target, float size)
	{
		ExplodeGutEffect explodeGutEffect = null;
		WorkerUnit workerUnit = target.GetWorkerUnit();
		if (workerUnit == null)
		{
			return;
		}
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(size);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.CENTRAL, null);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		if (target.IsDead())
		{
			workerUnit.gameObject.SetActive(false);
		}
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x000257A8 File Offset: 0x000239A8
	public override bool OnAfterSuppressed()
	{
		this.OnDie();
		return base.OnAfterSuppressed();
	}

	// Token: 0x040023CC RID: 9164
	public MachineDawnAnim anim;

	// Token: 0x040023CD RID: 9165
	private float killMotionAfterDelay = 4f;

	// Token: 0x040023CE RID: 9166
	public Timer killMotionAfterDelayTimer = new Timer();

	// Token: 0x040023CF RID: 9167
	public UnitModel _killMotionTarget;
}
