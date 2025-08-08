using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class MachineNoon : MachineOrdealCreature
{
	// Token: 0x060024DC RID: 9436 RVA: 0x000258CD File Offset: 0x00023ACD
	public MachineNoon()
	{
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x060024DD RID: 9437 RVA: 0x000258F6 File Offset: 0x00023AF6
	private static float _coolDownTime
	{
		get
		{
			return UnityEngine.Random.Range(4f, 6.5f);
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x060024DE RID: 9438 RVA: 0x00025907 File Offset: 0x00023B07
	private static float _coolDownCoolTime
	{
		get
		{
			return UnityEngine.Random.Range(9.5f, 13.5f);
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060024DF RID: 9439 RVA: 0x00020FFE File Offset: 0x0001F1FE
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 3);
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060024E0 RID: 9440 RVA: 0x00025918 File Offset: 0x00023B18
	private static int movingDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 2);
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060024E1 RID: 9441 RVA: 0x00025921 File Offset: 0x00023B21
	public MachineNoonAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as MachineNoonAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x0010D024 File Offset: 0x0010B224
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.motionDelayTimer.StartTimer(1f);
		this.coolDownCoolTimer.StartTimer(MachineNoon._coolDownCoolTime);
		if (this._ordealScript is MachineDuskOrdeal)
		{
			this.spawnByDuskTimer.StartTimer(MachineOrdealCreature.spawnByDuskTime);
		}
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x0010D084 File Offset: 0x0010B284
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.hp <= 0f)
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
		if (this.coolDownCoolTimer.RunTimer())
		{
			this.CoolDownStart();
			return;
		}
		if (this.motionDelayTimer.started)
		{
			this.motionDelayTimer.RunTimer();
			this.StopMovement();
			return;
		}
		if (this.coolDownTimer.started)
		{
			if (this.coolDownTimer.RunTimer())
			{
				this.CoolDownEnd();
			}
			this.StopMovement();
			return;
		}
		if (!this.animScript.CanMove())
		{
			this.StopMovement();
			return;
		}
		UnitModel nearestInRange = this.GetNearestInRange(15f, false);
		if (nearestInRange != null)
		{
			UnitModel nearestInRange2 = this.GetNearestInRange(3.5f, false);
			if (nearestInRange2 != null)
			{
				float x = nearestInRange2.GetCurrentViewPosition().x;
				float x2 = this.model.GetCurrentViewPosition().x;
				this.StopMovement();
				if (x < x2)
				{
					this.movable.SetDirection(UnitDirection.LEFT);
				}
				else if (x > x2)
				{
					this.movable.SetDirection(UnitDirection.RIGHT);
				}
				this.AttackStart();
			}
			else
			{
				this.model.MoveToMovable(nearestInRange.GetMovableNode());
			}
		}
		else if (!this.movable.IsMoving() && !this.movable.InElevator())
		{
			this.MakeMovement();
		}
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x00025950 File Offset: 0x00023B50
	private void CoolDownStart()
	{
		this.coolDownTimer.StartTimer(MachineNoon._coolDownTime);
		this.animScript.OnCoolDownStart();
		this.StopMovement();
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x00025973 File Offset: 0x00023B73
	private void CoolDownEnd()
	{
		this.animScript.OnCoolDownEnd();
		this.coolDownCoolTimer.StartTimer(MachineNoon._coolDownCoolTime);
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x0010D26C File Offset: 0x0010B46C
	private void MakeMovement()
	{
		MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
		this.model.MoveToNode(creatureRoamingPoint);
		this.animScript.OnMove();
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x00025990 File Offset: 0x00023B90
	private void StopMovement()
	{
		this.movable.StopMoving();
		this.animScript.OnStop();
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000259A8 File Offset: 0x00023BA8
	private void AttackStart()
	{
		this.animScript.OnAttackStart();
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000259B5 File Offset: 0x00023BB5
	public void OnAttackEnd()
	{
		this.motionDelayTimer.StartTimer(1f);
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x0010D29C File Offset: 0x0010B49C
	public void OnAttackDamageTimeCalled()
	{
		List<UnitModel> targetsInRange = this.GetTargetsInRange(4f, true);
		foreach (UnitModel unitModel in targetsInRange)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)MachineNoon.attackDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
		}
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x0010D320 File Offset: 0x0010B520
	public void OnMovingDamageTimeCalled()
	{
		UnitModel nearestInRange = this.GetNearestInRange(float.MaxValue, true);
		float num = 1f;
		if (this.currentPassage != null && !this.movable.InElevator())
		{
			num = this.currentPassage.scaleFactor;
			this.MakeEffect("MachineNoon_GunFire", Vector3.zero, 1f, false);
		}
		if (nearestInRange != null)
		{
			nearestInRange.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)MachineNoon.movingDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(nearestInRange, RwbpType.R, this.model);
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

	// Token: 0x060024EC RID: 9452 RVA: 0x0010D4E0 File Offset: 0x0010B6E0
	private List<UnitModel> GetTargetsInRange(float range, bool hasDir = true)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				CreatureModel creatureModel = unit as CreatureModel;
				if (unit.hp > 0f)
				{
					if (!(unit is Butterfly.ButterflyEffect))
					{
						if (!(unit is SnowWhite.VineArea))
						{
							if (!(unit is YoungPrinceFriend.Spore))
							{
								if (creatureModel == null || !(creatureModel.script is MachineOrdealCreature))
								{
									if (this.IsInRange(unit, range, hasDir))
									{
										list.Add(unit);
									}
								}
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x0010D5BC File Offset: 0x0010B7BC
	private UnitModel GetNearestInRange(float range, bool hasDir = true)
	{
		List<UnitModel> targetsInRange = this.GetTargetsInRange(range, hasDir);
		UnitModel result = null;
		float num = 10000f;
		foreach (UnitModel unitModel in targetsInRange)
		{
			if (unitModel.IsAttackTargetable())
			{
				float distance = this.GetDistance(unitModel);
				if (distance < num)
				{
					result = unitModel;
					num = distance;
				}
			}
		}
		return result;
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x0010D648 File Offset: 0x0010B848
	private bool IsInRange(UnitModel target, float range, bool hasDir = true)
	{
		try
		{
			float x = target.GetCurrentViewPosition().x;
			float x2 = this.model.GetCurrentViewPosition().x;
			if (hasDir)
			{
				if (this.model.GetDirection() == UnitDirection.LEFT)
				{
					if (x > x2)
					{
						return false;
					}
				}
				else if (x < x2)
				{
					return false;
				}
			}
			if (this.GetDistance(target) > range)
			{
				return false;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return false;
		}
		return true;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x0010C98C File Offset: 0x0010AB8C
	private float GetDistance(UnitModel target)
	{
		float x = target.GetCurrentViewPosition().x;
		float x2 = this.movable.GetCurrentViewPosition().x;
		float num = 1f;
		if (this.currentPassage != null)
		{
			num = this.currentPassage.scaleFactor;
		}
		return Math.Abs(x - x2) / num;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x0010D6F0 File Offset: 0x0010B8F0
	private GameObject MakeEffect(string src, Vector3 position, float scaleForEffect = 1f, bool isGlobal = false)
	{
		float d = 1f;
		if (this.currentPassage != null)
		{
			d = this.currentPassage.scaleFactor;
		}
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/MachineNoon/" + src);
		Vector3 vector = Vector3.one * scaleForEffect;
		if (isGlobal)
		{
			if (this.model.GetDirection() == UnitDirection.LEFT)
			{
				vector.x *= -1f;
			}
			gameObject.transform.position = position;
			vector *= d;
		}
		else
		{
			gameObject.transform.SetParent(this.animScript.gameObject.transform);
			gameObject.transform.localPosition = Vector3.zero;
		}
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = vector;
		return gameObject;
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000259C7 File Offset: 0x00023BC7
	public void MakeDeadEffect()
	{
		this.MakeEffect("MachineNoon_DeadEffect", Vector3.zero, 1f, false);
	}

	// Token: 0x040023DB RID: 9179
	private Timer motionDelayTimer = new Timer();

	// Token: 0x040023DC RID: 9180
	private const float _motionDelayTime = 1f;

	// Token: 0x040023DD RID: 9181
	private Timer coolDownTimer = new Timer();

	// Token: 0x040023DE RID: 9182
	private const float _coolDownTimeMin = 4f;

	// Token: 0x040023DF RID: 9183
	private const float _coolDownTimeMax = 6.5f;

	// Token: 0x040023E0 RID: 9184
	private Timer coolDownCoolTimer = new Timer();

	// Token: 0x040023E1 RID: 9185
	private const float _coolDownCoolTimeMin = 9.5f;

	// Token: 0x040023E2 RID: 9186
	private const float _coolDownCoolTimeMax = 13.5f;

	// Token: 0x040023E3 RID: 9187
	private const int _attackDmgMin = 1;

	// Token: 0x040023E4 RID: 9188
	private const int _attackDmgMax = 2;

	// Token: 0x040023E5 RID: 9189
	private const int _movingDmgMin = 1;

	// Token: 0x040023E6 RID: 9190
	private const int _movingDmgMax = 1;

	// Token: 0x040023E7 RID: 9191
	private const float _movingDmgRange = 3.4028235E+38f;

	// Token: 0x040023E8 RID: 9192
	private const float _attackRange = 3.5f;

	// Token: 0x040023E9 RID: 9193
	private const float _atkDmgRange = 4f;

	// Token: 0x040023EA RID: 9194
	private const float _recognizeRange = 15f;

	// Token: 0x040023EB RID: 9195
	private const float _wallEffectScale = 0.25f;

	// Token: 0x040023EC RID: 9196
	private const string _effectSrc = "Effect/Creature/MachineNoon/";

	// Token: 0x040023ED RID: 9197
	private const string _effect_wall = "MachineNoon_WallEffect";

	// Token: 0x040023EE RID: 9198
	private const string _effect_gun = "MachineNoon_GunFire";

	// Token: 0x040023EF RID: 9199
	private const string _effect_dead = "MachineNoon_DeadEffect";

	// Token: 0x040023F0 RID: 9200
	private MachineNoonAnim _animScript;
}
