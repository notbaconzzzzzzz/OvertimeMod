using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMachineNoon : MachineNoon
{
	public OvertimeMachineNoon()
	{
	}

	private static float _coolDownTime
	{
		get
		{
			return UnityEngine.Random.Range(4f, 6.5f);
		}
	}

	private static float _coolDownCoolTime
	{
		get
		{
			return UnityEngine.Random.Range(6.5f, 9.5f);
		}
	}

	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(3, 6);
		}
	}

	private static int movingDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 3);
		}
	}

	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		coolDownCoolTimer.StartTimer(_coolDownCoolTime);
	}

	public override void OnFixedUpdate(CreatureModel creature)
	{
        base.OnFixedUpdate(creature);
		if (model.hp <= 0f)
		{
			return;
		}
		if (coolDownTimer.started)
		{
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(1f);
            }
		}
        if (_recoverTimer.RunTimer())
        {
            if (coolDownTimer.started)
            {
				model.RecoverHP(50f);
                GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
                gameObject.transform.SetParent(animScript.gameObject.transform);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
                _recoverTimer.StartTimer(1f);
            }
        }
	}

	public override void CoolDownStart()
	{
		this.coolDownTimer.StartTimer(_coolDownTime);
		this.animScript.OnCoolDownStart();
		this.StopMovement();
	}

	public override void CoolDownEnd()
	{
		this.animScript.OnCoolDownEnd();
		this.coolDownCoolTimer.StartTimer(_coolDownCoolTime);
        EnergizedBuf.SetEnergized(2, 15f);
		if (currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in currentPassage.GetEnteredTargets(movable))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				CreatureModel creatureModel = unit as CreatureModel;
				if (unit.hp > 0f && creatureModel != null && (creatureModel.script is OvertimeMachineDawn))
				{
                    (creatureModel.script as MachineOrdealCreature).EnergizedBuf.SetEnergized(2, 15f);
				}
			}
		}
	}

	public override void OnAttackDamageTimeCalled()
	{
		List<UnitModel> targetsInRange = this.GetTargetsInRange(4f, true);
		foreach (UnitModel unitModel in targetsInRange)
		{
			foreach (BarrierBuf barrier in unitModel.GetBarrierBufList())
			{
				barrier._barrierValue -= 5f;
				if (barrier._barrierValue <= 0f)
				{
					barrier.Destroy();
				}
			}
			float dmg = (float)attackDmg;
			if (EnergizedLevel >= 2)
			{
				dmg *= 1.2f + 0.1f * EnergizedLevel;
			}
			dmg *= 1f + 0.2f / Mathf.Max(unitModel.defense.R, 0.1f);
			DamageInfo damage = new DamageInfo(RwbpType.R, dmg);
			unitModel.TakeDamage(this.model, damage);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
			float num = damage.result.hpDamage;
			num *= 5f;
			if (num > 0f)
			{
				OvertimeMachineHemorrhageDebuf buf = unitModel.GetUnitBufByType(UnitBufType.OVERTIME_MACHINE_HEMORRHAGE) as OvertimeMachineHemorrhageDebuf;
				if (buf == null)
				{
					buf = new OvertimeMachineHemorrhageDebuf();
					unitModel.AddUnitBuf(buf);
				}
				buf.AddStacks(num);
			}
		}
	}

	public override void OnMovingDamageTimeCalled()
	{
		UnitModel nearestInRange = this.GetNearestInRangePrioritize(float.MaxValue, true);
		float num = 1f;
		if (this.currentPassage != null && !this.movable.InElevator())
		{
			num = this.currentPassage.scaleFactor;
			this.MakeEffect("MachineNoon_GunFire", Vector3.zero, 1f, false);
		}
		if (nearestInRange != null)
		{
			float dmg = (float)movingDmg;
			if (EnergizedLevel >= 2)
			{
				dmg *= 1.2f + 0.1f * EnergizedLevel;
			}
			nearestInRange.TakeDamage(this.model, new DamageInfo(RwbpType.R, dmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(nearestInRange, RwbpType.R, this.model);
			if (nearestInRange is WorkerModel)
			{
				WorkerModel worker = nearestInRange as WorkerModel;
				if (worker.IsAttackState())
				{
					//worker.SpecialAttackEnd(worker.GetWorkerUnit().animChanger.state.GetCurrent(0));
					worker.Stun(0.25f);
					if (worker is AgentModel)
					{
						(worker as AgentModel).ForcelyCancelSuppress();
						(worker as AgentModel).counterAttackEnabled = true;
					}
					else
					{
						worker.StopAction();
					}
				}
			}
		}
		else if (this.currentPassage != null)
		{
			Vector3 currentViewPosition = this.model.GetMovableNode().GetCurrentViewPosition();
			UnitDirection direction = this.model.GetMovableNode().GetDirection();
			MapNode[] nodeList = this.currentPassage.GetNodeList();
			MapNode mapNode = null;
			float num2 = (direction != UnitDirection.RIGHT) ? float.MaxValue : float.MinValue;
			foreach (MapNode mapNode2 in nodeList)
			{
				Vector3 position = mapNode2.GetPosition();
				if (direction == UnitDirection.RIGHT)
				{
					if (position.x > currentViewPosition.x)
					{
						if (num2 <= position.x)
						{
							num2 = position.x;
							mapNode = mapNode2;
						}
					}
				}
				else if (position.x < currentViewPosition.x)
				{
					if (num2 >= position.x)
					{
						num2 = position.x;
						mapNode = mapNode2;
					}
				}
			}
			if (mapNode == null)
			{
				return;
			}
			Vector3 position2 = mapNode.GetPosition();
			position2.y += 1f * num;
			this.MakeEffect("MachineNoon_WallEffect", position2, 0.25f, true);
		}
	}

	public UnitModel GetNearestInRangePrioritize(float range, bool hasDir = true)
	{
		List<UnitModel> targetsInRange = this.GetTargetsInRange(range, hasDir);
		UnitModel result = null;
		float num = 10000f;
		foreach (UnitModel unitModel in targetsInRange)
		{
			if (unitModel.IsAttackTargetable())
			{
				float distance = this.GetDistance(unitModel);
				if (unitModel is WorkerModel && (unitModel.IsAttackState() && (unitModel as WorkerModel).stunTime <= 0f))
				{
					distance -= UnityEngine.Random.Range(900f, 1000f);
				}
				if (distance < num)
				{
					result = unitModel;
					num = distance;
				}
			}
		}
		return result;
	}

    public Timer _recoverTimer = new Timer();
}
