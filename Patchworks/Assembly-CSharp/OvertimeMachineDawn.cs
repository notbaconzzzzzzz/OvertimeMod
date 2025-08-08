using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMachineDawn : MachineDawn
{
	public OvertimeMachineDawn()
	{
	}

	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		if (_ordealScript is MachineDuskOrdeal || _ordealScript is MachineMidnightOrdeal)
		{
			spawnByDuskTimer.StartTimer(MachineOrdealCreature.spawnByDuskTime);
		}
		abilityChance = 0;
	}

	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.state != CreatureState.ESCAPE)
		{
			return;
		}
		if (EnergizedLevel >= 1)
		{
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(0.5f);
            }
		}
        if (_recoverTimer.RunTimer())
        {
            if (EnergizedLevel >= 1)
            {
				model.RecoverHP(16f + EnergizedLevel * 2f);
                GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
                gameObject.transform.SetParent(anim.gameObject.transform);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
                _recoverTimer.StartTimer(3f);
            }
        }
	}

	public override UnitModel GetNearTargetUnit()
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
					float magnitude = (unitModel2.GetCurrentViewPosition() - this.model.GetCurrentViewPosition()).magnitude / movable.currentScale;
					if (unitModel2.HasUnitBuf(UnitBufType.OVERTIME_MACHINE_DAWN_DAMAGE_REDUCE))
					{
						magnitude += 2f;
					}
					if ((!cooldownTimer.started || isClerkCooldown && unitModel is AgentModel) && unitModel2.HasUnitBuf(UnitBufType.OVERTIME_MACHINE_DAWN_UNCON))
					{
						magnitude += 3f;
					}
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

	public override void BloodEffect()
	{
		if (model.hp <= 0f) return;
		DamageInfo stabDamage;
		DamageInfo areaDamage;
		if (pinFinalAttack)
		{
			stabDamage = new DamageInfo(RwbpType.R, 15, 24);
			areaDamage = new DamageInfo(RwbpType.W, 10f);
		}
		else
		{
			stabDamage = new DamageInfo(RwbpType.R, 5, 8);
			areaDamage = new DamageInfo(RwbpType.W, 4f);
		}
		List<UnitModel> list = new List<UnitModel>();
		MovableObjectNode movableNode = model.GetMovableNode();
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
						if (model.IsHostile(unit))
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
				unitModel.TakeDamage(null, areaDamage);
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.W, model);
			}
		}
		if (_killMotionTarget != null)
		{
			_killMotionTarget.TakeDamage(model, stabDamage);
			DamageParticleEffect damageParticleEffect2 = DamageParticleEffect.Invoker(_killMotionTarget, RwbpType.R, DefenseInfo.zero);
			damageParticleEffect2.transform.localPosition = damageParticleEffect2.transform.localPosition + new Vector3(0f, -1f * model.GetMovableNode().currentScale, 0f);
		}
	}

	public override void FinishEffect()
	{
		pinFinalAttack = true;
		BloodEffect();
		pinFinalAttack = false;
		if (_killMotionTarget is WorkerModel)
		{
			MakeExplodeEffect(_killMotionTarget as WorkerModel, 1f);
		}
		EnergizedBuf.SetEnergized(1, 10f);
	}

	public bool TryUseAbility()
	{
		abilityChance += 20;
		if (Prob(abilityChance))
		{
			abilityChance = 0;
			return true;
		}
		return false;
	}

	private bool pinFinalAttack;

	private int abilityChance;

    public Timer _recoverTimer = new Timer();

    public Timer cooldownTimer = new Timer();

	public bool isClerkCooldown = false;
}
