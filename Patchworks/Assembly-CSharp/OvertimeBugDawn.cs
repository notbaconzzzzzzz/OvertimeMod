using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeBugDawn : BugDawn
{
	public OvertimeBugDawn()
	{
	}

	private static float beforeteleportTime
	{
		get
		{
			return UnityEngine.Random.Range(0.1f, 0.3f);
		}
	}

	private static float teleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	private static float attackDelay
	{
		get
		{
			return UnityEngine.Random.Range(1.25f, 1.5f);
		}
	}

	private static float appearanceDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(3, 6);
		}
	}

	private static int appearDmg
	{
		get
		{
			return UnityEngine.Random.Range(10, 21);
		}
	}

	private static float speed
	{
		get
		{
			return UnityEngine.Random.Range(4.2f, 4.8f);
		}
	}
	public override void Init()
	{
		animScript.SetScript(this);
		model.movementScale = speed;
	}
	public override void ProcessMoving()
	{
		if (_currentTarget != null && (!_currentTarget.IsAttackTargetable() || _currentTarget.GetMovableNode().GetPassage() != currentPassage || _currentTarget.GetMovableNode().GetPassage() == null))
		{
			StopMovement();
			_currentTarget = null;
		}
		if (_currentTarget == null)
		{
			FindTarget();
		}
		if (!attackDelayTimer.started)
		{
			if (_currentTarget != null)
			{
				if (MovableObjectNode.GetDistance(_currentTarget.GetMovableNode(), movable) < 10f)
				{
					attackDelayTimer.StartTimer(attackDelay);
					StartAttack();
					return;
				}
				if (!movable.IsMoving())
				{
					movable.MoveToMovableNode(_currentTarget.GetMovableNode(), false);
				}
			}
			else if (!movable.IsMoving())
			{
				MakeMovement();
			}
		}
	}

	public override void OnEndDigIn()
	{
		if (_targetNode == null)
		{
			Debug.Log("Teleport failed");
			return;
		}
		movable.SetCurrentNode(_targetNode);
		Sefira sefira = SefiraManager.instance.GetSefira(_targetNode.GetAttachedPassage().GetSefiraName());
		model.sefira = sefira;
		model.sefiraNum = sefira.indexString;
		appearanceTimer.StartTimer(appearanceDelay);
	}

	public override void OnEndDigOut()
	{
		_phase = BugOrdealCreature.BugPhase.Moving;
		teleportDelayTimer.StartTimer(teleportDelay);
		model.movementScale = speed;
		StopMovement();
		GiveAppearDmg(5f);
	}

	public override void StartAttack()
	{
		if (_currentTarget == null)
		{
			return;
		}
		if (currentPassage != null && currentPassage == _currentTarget.GetMovableNode().GetPassage())
		{
			_phase = BugOrdealCreature.BugPhase.Attack;
			oldPosX = movable.GetCurrentViewPosition().x;
			if (_currentTarget.GetCurrentViewPosition().x < oldPosX)
			{
				attackDirection = UnitDirection.LEFT;
			}
			else if (_currentTarget.GetCurrentViewPosition().x > oldPosX)
			{
				attackDirection = UnitDirection.RIGHT;
			}
			else
			{
				attackDirection = movable.GetDirection();
			}
			float distance = MovableObjectNode.GetDistance(_currentTarget.GetMovableNode(), movable);
			if (distance > 6f)
			{
				distance = 6f;
			}
			model.movementScale = 10f * distance / 5f + UnityEngine.Random.Range(1f, 3f);
			movable.MoveBy(attackDirection, 10f);
			animScript.SetAnimation(BugDawn.AnimationState.ATTACK);
			damagedUnits.Clear();
		}
	}

	public override void ProcessAttack(float baseRange)
	{
		float x = movable.GetCurrentViewPosition().x;
		PassageObjectModel passage = movable.GetPassage();
		if (passage != null && passage == _currentTarget.GetMovableNode().currentPassage)
		{
			List<UnitModel> list = new List<UnitModel>();
			float num = baseRange * movable.currentScale;
            foreach (MovableObjectNode objectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = objectNode.GetUnit();
				if (damagedUnits.Contains(unit) || unit.hp <= 0f) continue;
				if (!IsHostile(unit)) continue;
				float x2 = objectNode.GetCurrentViewPosition().x;
				if (oldPosX < x)
				{
					if (oldPosX - num < x2 && x2 < x + num)
					{
						list.Add(unit);
					}
				}
				else if (x - num < x2 && x2 < oldPosX + num)
				{
					list.Add(unit);
				}
			}
			foreach (UnitModel unitModel in list)
			{
				if (unitModel == _currentTarget)
				{
					unitModel.TakeDamage(model, new DamageInfo(RwbpType.R, (float)attackDmg));
					float healing = 3f;
					OvertimeBugDawnDebuf buf = unitModel.GetUnitBufByType(UnitBufType.OVERTIME_BUG_DAWN) as OvertimeBugDawnDebuf;
					if (buf == null)
					{
						unitModel.AddUnitBuf(new OvertimeBugDawnDebuf());
					}
					else
					{
						healing *= (float)Mathf.Min(buf.stacks + 25, 200) / 100f;
						buf.BugDawnHit(true);
					}
					model.hp += healing;
					if (model.hp > model.maxHp)
					{
						model.hp = model.maxHp;
					}
					GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
					gameObject.transform.SetParent(animScript.gameObject.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.identity;
				}
				else
				{
					unitModel.TakeDamage(model, new DamageInfo(RwbpType.R, (float)attackDmg * 0.3f));
					float healing = 1f;
					OvertimeBugDawnDebuf buf = unitModel.GetUnitBufByType(UnitBufType.OVERTIME_BUG_DAWN) as OvertimeBugDawnDebuf;
					if (buf == null)
					{
						unitModel.AddUnitBuf(new OvertimeBugDawnDebuf());
					}
					else
					{
						healing *= (float)Mathf.Min(buf.stacks + 25, 200) / 100f;
						buf.BugDawnHit(false);
					}
					model.hp += healing;
					if (model.hp > model.maxHp)
					{
						model.hp = model.maxHp;
					}
				}
				damagedUnits.Add(unitModel);
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, model);
			}
		}
		oldPosX = x;
	}

	public override void GiveAppearDmg(float baseRange)
	{
		if (currentPassage == null)
		{
			return;
		}
		List<MovableObjectNode> list = new List<MovableObjectNode>(currentPassage.GetEnteredTargets(movable));
		float num = baseRange * movable.currentScale;
		foreach (MovableObjectNode movableObjectNode in list)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (IsHostile(unit))
			{
				if (MovableObjectNode.GetDistance(movableObjectNode, movable) <= num)
				{
					unit.TakeDamage(model, new DamageInfo(RwbpType.R, (float)appearDmg));
					DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, RwbpType.R, model);
					OvertimeBugDawnDebuf buf = unit.GetUnitBufByType(UnitBufType.OVERTIME_BUG_DAWN) as OvertimeBugDawnDebuf;
					if (buf == null)
					{
						unit.AddUnitBuf(new OvertimeBugDawnDebuf());
					}
					for (int i = 0; i < 5; i++)
					{
						buf.BugDawnHit(true);
					}
				}
			}
		}
	}

    public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
    {
		List<MovableObjectNode> list = new List<MovableObjectNode>(currentPassage.GetEnteredTargets(movable));
		OvertimeBugDawnProtectionBuf selfBuf = model.GetUnitBufByType(UnitBufType.OVERTIME_BUG_DAWN_PROT) as OvertimeBugDawnProtectionBuf;
		int amt = 100;
		if (selfBuf != null)
		{
			amt = 10000 / (selfBuf.stacks + 100);
		}
        foreach (MovableObjectNode movableObjectNode in list)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit != model && unit is OrdealCreatureModel && (unit as OrdealCreatureModel).script is OvertimeBugDawn)
			{
				OvertimeBugDawnProtectionBuf buf = unit.GetUnitBufByType(UnitBufType.OVERTIME_BUG_DAWN_PROT) as OvertimeBugDawnProtectionBuf;
				if (buf == null)
				{
					buf = new OvertimeBugDawnProtectionBuf();
					unit.AddUnitBuf(buf);
				}
				buf.BugDawnAttacked(amt);
			}
		}
    }
}