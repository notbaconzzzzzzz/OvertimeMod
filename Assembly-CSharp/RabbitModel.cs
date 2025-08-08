using System;
using System.Collections.Generic;
using DeadEffect;
using Rabbit;
using UnityEngine;

// Token: 0x020006E3 RID: 1763
public class RabbitModel : UnitModel, IMouseCommandTargetModel
{
	// Token: 0x060038E8 RID: 14568 RVA: 0x00170210 File Offset: 0x0016E410
	public RabbitModel(RwbpType rwbpType)
	{
		this.baseMaxHp = 150;
		this.hp = 150f;
		this.baseMaxMental = 150;
		this.mental = 150f;
		this.movableNode = new MovableObjectNode(this);
		this.movableNode.SetTeleportable(false);
		this._commandQueue = new RabbitCommandQueue(this);
		base.SetWeapon(WeaponModel.MakeWeapon(EquipmentTypeList.instance.GetData(4)));
		base.SetArmor(ArmorModel.MakeArmor(EquipmentTypeList.instance.GetData(5)));
		this._rwbpType = rwbpType;
		this.tempAnim.SetAttackDuraction(10);
		this.baseMovement = UnityEngine.Random.Range(3f, 3.3f);
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.PanicWorker);
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060038E9 RID: 14569 RVA: 0x000331AB File Offset: 0x000313AB
	public RabbitUnit Unit
	{
		get
		{
			return this._unit;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060038EA RID: 14570 RVA: 0x000331B3 File Offset: 0x000313B3
	public RwbpType rwbpType
	{
		get
		{
			return this._rwbpType;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060038EB RID: 14571 RVA: 0x000331BB File Offset: 0x000313BB
	public string name
	{
		get
		{
			return LocalizeTextDataModel.instance.GetText("Rabbit_Unit_Name");
		}
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x000331CC File Offset: 0x000313CC
	public void SetUnit(RabbitUnit unit)
	{
		this._unit = unit;
	}

	// Token: 0x060038ED RID: 14573 RVA: 0x000331D5 File Offset: 0x000313D5
	public void ClearOperation()
	{
		this.OnClearProtocol();
		this._unit.Speech(RabbitConversationType.SECTORCLEAR, 0.8f, 3f);
		this.GetMovableNode().SetActive(false);
		this._activated = false;
		this._commandQueue.Clear();
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x00033211 File Offset: 0x00031411
	public override bool IsAttackTargetable()
	{
		return !this.IsDead() && this._activated;
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x00033227 File Offset: 0x00031427
	public override bool IsHostile(UnitModel target)
	{
		return !(target is RabbitModel);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x00033235 File Offset: 0x00031435
	public UnitCommand GetCurrentCommand()
	{
		return this._commandQueue.GetCurrentCmd();
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x00033242 File Offset: 0x00031442
	public void SetCommand(UnitCommand cmd)
	{
		this._commandQueue.SetCommand(cmd);
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x0017030C File Offset: 0x0016E50C
	public void OnFixedUpdate()
	{
		this.UpdateBufState();
		if (this.IsDead())
		{
			return;
		}
		if (!this._activated)
		{
			return;
		}
		if (this._remainFireDelay > 0f)
		{
			this._remainFireDelay -= Time.deltaTime;
		}
		this._commandQueue.Execute(this);
		this.movableNode.ProcessMoveNode(this.baseMovement * base.GetMovementScaleByBuf());
		this.CheckNear();
		if (this._encounterClearTimer.started)
		{
			if (this._encounterClearTimer.RunTimer())
			{
				this.encountered.Clear();
				return;
			}
		}
		else
		{
			this._encounterClearTimer.StartTimer(this.encountClearFreq);
		}
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x001703B4 File Offset: 0x0016E5B4
	public override void TakeDamageWithoutEffect(UnitModel actor, DamageInfo dmg)
	{
		if (this.IsDead())
		{
			return;
		}
		base.TakeDamageWithoutEffect(actor, dmg);
		float num = dmg.GetDamage();
		if (dmg.type == RwbpType.R)
		{
			float hp = this.hp;
			this.hp -= num;
			float num2 = hp - this.hp;
		}
		else if (dmg.type == RwbpType.W)
		{
			float mental = this.mental;
			this.mental -= num;
			float num3 = mental - this.mental;
		}
		else if (dmg.type == RwbpType.B)
		{
			float hp2 = this.hp;
			this.hp -= num;
			this.mental -= num;
		}
		else if (dmg.type == RwbpType.P)
		{
			float num4 = num / 100f;
			num = (float)this.maxHp * num4;
			this.hp -= num;
		}
		else if (dmg.type == RwbpType.N)
		{
			float hp3 = this.hp;
			this.hp -= num;
			float num5 = hp3 - this.hp;
		}
		this.hp = Mathf.Clamp(this.hp, 0f, (float)this.maxHp);
		this.mental = Mathf.Clamp(this.mental, 0f, (float)this.maxMental);
		if (this.hp <= 0f)
		{
			this.OnDie();
		}
		else if (this.mental <= 0f)
		{
			this.OnDieByMental();
		}
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x00170544 File Offset: 0x0016E744
	public override void TakeDamage(UnitModel actor, DamageInfo dmg)
	{
		dmg = dmg.Copy();
		if (this.IsDead())
		{
			return;
		}
		base.TakeDamage(actor, dmg);
		float num = 1f;
		if (actor != null)
		{
			num = UnitModel.GetDmgMultiplierByEgoLevel(actor.GetAttackLevel(), this.GetDefenseLevel());
		}
		num *= this.GetBufDamageMultiplier(actor, dmg);
		float num2 = dmg.GetDamageWithDefenseInfo(this.defense) * num;
		if (dmg.type == RwbpType.R)
		{
			foreach (BarrierBuf barrierBuf in this._barrierBufList.ToArray())
			{
				num2 = barrierBuf.UseBarrier(RwbpType.R, num2);
			}
			float hp = this.hp;
			this.hp -= num2;
			float value = (float)((int)hp - (int)this.hp);
			this.MakeDamageEffect(RwbpType.R, value, this.defense.GetDefenseType(RwbpType.R));
		}
		else if (dmg.type == RwbpType.W)
		{
			foreach (BarrierBuf barrierBuf2 in this._barrierBufList.ToArray())
			{
				num2 = barrierBuf2.UseBarrier(RwbpType.W, num2);
			}
			float mental = this.mental;
			this.mental -= num2;
			float value2 = mental - this.mental;
			this.MakeDamageEffect(RwbpType.W, value2, this.defense.GetDefenseType(RwbpType.W));
		}
		else if (dmg.type == RwbpType.B)
		{
			foreach (BarrierBuf barrierBuf3 in this._barrierBufList.ToArray())
			{
				num2 = barrierBuf3.UseBarrier(RwbpType.B, num2);
			}
			float hp2 = this.hp;
			this.hp -= num2;
			float value3 = (float)((int)hp2 - (int)this.hp);
			this.mental -= num2;
			this.MakeDamageEffect(RwbpType.B, value3, this.defense.GetDefenseType(RwbpType.B));
		}
		else if (dmg.type == RwbpType.P)
		{
			float num3 = num2 / 100f;
			num2 = (float)this.maxHp * num3;
			foreach (BarrierBuf barrierBuf4 in this._barrierBufList.ToArray())
			{
				num2 = barrierBuf4.UseBarrier(RwbpType.P, num2);
			}
			float hp3 = this.hp;
			this.hp -= num2;
			float value4 = (float)((int)hp3 - (int)this.hp);
			this.MakeDamageEffect(RwbpType.P, value4, this.defense.GetDefenseType(RwbpType.P));
		}
		else if (dmg.type == RwbpType.N)
		{
			float hp4 = this.hp;
			this.hp -= num2;
			float value5 = hp4 - this.hp;
			this.MakeDamageEffect(RwbpType.N, value5, DefenseInfo.Type.NONE);
		}
		this.hp = Mathf.Clamp(this.hp, 0f, (float)this.maxHp);
		this.mental = Mathf.Clamp(this.mental, 0f, (float)this.maxMental);
		if (this.hp <= 0f)
		{
			this.OnDie();
		}
		else if (this.mental <= 0f)
		{
			this.OnDieByMental();
		}
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x00033250 File Offset: 0x00031450
	public bool IsDead()
	{
		return this._isDead;
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x00033258 File Offset: 0x00031458
	public void OnDie()
	{
		RabbitManager.instance.OnRabbitDead(this);
		this._isDead = true;
		this._activated = false;
		this.GetMovableNode().SetActive(false);
		this._commandQueue.Clear();
		this._unit.OnDie();
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x0017086C File Offset: 0x0016EA6C
	public void OnDieByMental()
	{
		RabbitManager.instance.OnRabbitDead(this);
		this._activated = false;
		this.GetMovableNode().SetActive(false);
		this._commandQueue.Clear();
		this._unit.OnDieByMental();
		this._unit.Speech(RabbitConversationType.PANIC, 1f, 3f);
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x00033295 File Offset: 0x00031495
	public bool IsFireState()
	{
		return this._remainFireDelay > 0f;
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x001708C4 File Offset: 0x0016EAC4
	public override void Attack(UnitModel target)
	{
		if (this.IsAttackState())
		{
			return;
		}
		if (this.IsFireState())
		{
			return;
		}
		this._currentWeapon = RabbitWeaponEnum.KNIFE;
		if (!this._equipment.weapon.InRange(this, target))
		{
			return;
		}
		if (this.movableNode.GetCurrentViewPosition().x > target.GetMovableNode().GetCurrentViewPosition().x)
		{
			this.movableNode.SetDirection(UnitDirection.LEFT);
		}
		else
		{
			this.movableNode.SetDirection(UnitDirection.RIGHT);
		}
		string animationName = this._equipment.weapon.OnAttack(this, target);
		this.PlayAttackAnimation(animationName);
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x0017096C File Offset: 0x0016EB6C
	protected override void PlayAttackAnimation(string animationName)
	{
		this.tempAnim.PlayAttackAnimation(new DummyAttackAnimator.Callback(base.OnEndAttackCycle));
		int index = 1;
		string text = animationName.ToLower();
		if (text != null)
		{
			if (!(text == "knife1"))
			{
				if (!(text == "knife2"))
				{
					if (text == "knife3")
					{
						index = 3;
					}
				}
				else
				{
					index = 2;
				}
			}
			else
			{
				index = 1;
			}
		}
		this._unit.Attack(index);
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x000332A4 File Offset: 0x000314A4
	public void Fire()
	{
		if (this.IsAttackState())
		{
			return;
		}
		if (this.IsFireState())
		{
			return;
		}
		this._currentWeapon = RabbitWeaponEnum.RIFLE;
		this._remainFireDelay = 15f;
		this._unit.Fire();
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x001709F8 File Offset: 0x0016EBF8
	private void OnGiveDamageByRifle()
	{
		PassageObjectModel passage = this.GetMovableNode().GetPassage();
		DamageInfo damageInfo = base.Equipment.weapon.metaInfo.damageInfos[3].Copy();
		damageInfo.type = this.rwbpType;
		damageInfo.soundInfo.PlaySound(this.GetCurrentViewPosition());
		if (passage != null)
		{
			UnitModel unitModel = null;
			float num = 100000f;
			float x = this.GetMovableNode().GetCurrentViewPosition().x;
			UnitDirection direction = this.GetMovableNode().GetDirection();
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit != null)
				{
					if (!(unit is RabbitModel))
					{
						if (unit.IsAttackTargetable())
						{
							float x2 = movableObjectNode.GetCurrentViewPosition().x;
							if (direction == UnitDirection.LEFT && x2 < x)
							{
								float num2 = x - x2;
								if (num2 < num)
								{
									unitModel = unit;
									num = num2;
								}
							}
							else if (direction == UnitDirection.RIGHT && x2 > x)
							{
								float num3 = x2 - x;
								if (num3 < num)
								{
									unitModel = unit;
									num = num3;
								}
							}
						}
					}
				}
			}
			if (unitModel != null)
			{
				unitModel.TakeDamage(this, damageInfo);
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, damageInfo.type, this.defense, direction);
			}
			else if (this.IsFireState())
			{
				this._unit.StopFire();
			}
		}
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x000332DB File Offset: 0x000314DB
	public void OnGiveDamage()
	{
		if (this._currentWeapon == RabbitWeaponEnum.KNIFE)
		{
			base.OnGiveDamageByWeapon();
		}
		else
		{
			this.OnGiveDamageByRifle();
		}
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x000332F9 File Offset: 0x000314F9
	public void OnEndCycle()
	{
		if (this._currentWeapon == RabbitWeaponEnum.KNIFE)
		{
			base.OnEndAttackCycle();
		}
		else
		{
			this._remainFireDelay = 0f;
		}
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x0003331C File Offset: 0x0003151C
	public override int GetRiskLevel()
	{
		return 4;
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x00030370 File Offset: 0x0002E570
	public override int GetAttackLevel()
	{
		return this.GetRiskLevel();
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x00030370 File Offset: 0x0002E570
	public override int GetDefenseLevel()
	{
		return this.GetRiskLevel();
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0003331F File Offset: 0x0003151F
	public void OnClearProtocol()
	{
		this._unit.OnClear();
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x00170BA8 File Offset: 0x0016EDA8
	public void OnAnimEventCalled(int index)
	{
		switch (index)
		{
		case 20:
			this.MakeSound("Rabbit/RabbitTeam_Sword");
			break;
		case 21:
			this.MakeSound("Rabbit/RabbitTeam_SwordSkill1");
			break;
		case 22:
			this.MakeSound("Rabbit/RabbitTeam_SwordSkill2");
			break;
		default:
			if (index != 30)
			{
				if (index != 31)
				{
					if (index != 40)
					{
						if (index != 41)
						{
							if (index != 11)
							{
								if (index == 100)
								{
									if (this._unit != null)
									{
										WorkerExecutionEffect workerExecutionEffect = this._unit.executionCenter.gameObject.AddComponent<WorkerExecutionEffect>();
										GameObject gameObject = Prefab.LoadPrefab("Effect/Bullet/RabbitFallback");
										gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
										gameObject.transform.position = this._unit.executionCenter.transform.position;
										gameObject.transform.localScale = Vector3.one * 3f * this.movableNode.currentScale;
										if (workerExecutionEffect != null)
										{
											workerExecutionEffect.Init(this);
										}
									}
									else
									{
										Debug.Log("Failed to load Unit");
									}
								}
							}
							else
							{
								this.MakeSound("Rabbit/RabbitTeam_GunReload");
							}
						}
						else
						{
							this.MakeSound("RabbitTeam_Panic");
						}
					}
					else
					{
						this.MakeSound("creature/BossBird/BossBird_Ulti_Mouth2");
						GameObject gameObject2 = Prefab.LoadPrefab("Effect/Rabbit/RabbitHeadExplosion");
						gameObject2.transform.SetParent(EffectLayer.currentLayer.transform);
						gameObject2.transform.position = this._unit.faceFollower.transform.position;
						gameObject2.transform.localScale = Vector3.one * this.movableNode.currentScale;
						gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
						this._isDead = true;
					}
				}
				else
				{
					this.MakeSound("Rabbit/RabbitTeam_End");
				}
			}
			else
			{
				this.MakeSound("Rabbit/RabbitTeam_Clear");
			}
			break;
		}
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x0003332C File Offset: 0x0003152C
	public void SetUnitState(bool state)
	{
		this._unit.gameObject.SetActive(state);
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x00170DBC File Offset: 0x0016EFBC
	public SoundEffectPlayer MakeSound(string src)
	{
		return SoundEffectPlayer.PlayOnce(src, this.movableNode.GetCurrentViewPosition());
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x00170DE4 File Offset: 0x0016EFE4
	public void CheckNear()
	{
		if (this.movableNode.currentPassage == null)
		{
			return;
		}
		List<UnitModel> list = new List<UnitModel>();
		foreach (MovableObjectNode movableObjectNode in this.movableNode.currentPassage.GetEnteredTargets(this.movableNode))
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit != null)
			{
				if (unit.IsAttackTargetable())
				{
					if (!(unit is RabbitModel))
					{
						if (!list.Contains(unit))
						{
							list.Add(unit);
						}
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		foreach (UnitModel unitModel in list)
		{
			if (!this.encountered.Contains(unitModel))
			{
				RabbitConversationType rabbitConversationType = RabbitConversationType.NONE;
				if (unitModel is CreatureModel)
				{
					CreatureModel creatureModel = unitModel as CreatureModel;
					RiskLevel riskLevel = creatureModel.metaInfo.GetRiskLevel();
					if (riskLevel != RiskLevel.TETH)
					{
						if (riskLevel != RiskLevel.WAW)
						{
							if (riskLevel == RiskLevel.ALEPH)
							{
								rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_ALEPH;
							}
						}
						else
						{
							rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_WAW;
						}
					}
					else
					{
						rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_TETH;
					}
				}
				else if (unitModel is AgentModel)
				{
					AgentModel agentModel = unitModel as AgentModel;
					rabbitConversationType = RabbitConversationType.ENCOUNTER_AGENT;
				}
				else if (unitModel is OfficerModel)
				{
					OfficerModel officerModel = unitModel as OfficerModel;
					rabbitConversationType = RabbitConversationType.ENCOUNTER_OFFICER;
				}
				else if (unitModel is OrdealCreatureModel)
				{
					OrdealCreatureModel ordealCreatureModel = unitModel as OrdealCreatureModel;
					RiskLevel riskLevel2 = ordealCreatureModel.OrdealBase.GetRiskLevel(ordealCreatureModel);
					if (riskLevel2 != RiskLevel.TETH)
					{
						if (riskLevel2 != RiskLevel.WAW)
						{
							if (riskLevel2 == RiskLevel.ALEPH)
							{
								rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_ALEPH;
							}
						}
						else
						{
							rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_WAW;
						}
					}
					else
					{
						rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_TETH;
					}
				}
				else if (unitModel is ChildCreatureModel)
				{
					ChildCreatureModel childCreatureModel = unitModel as ChildCreatureModel;
					RiskLevel riskLevel3;
					if (childCreatureModel.script is BossBird)
					{
						riskLevel3 = RiskLevel.ALEPH;
					}
					else
					{
						riskLevel3 = RiskLevel.HE;
					}
					if (riskLevel3 != RiskLevel.TETH)
					{
						if (riskLevel3 != RiskLevel.WAW)
						{
							if (riskLevel3 == RiskLevel.ALEPH)
							{
								rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_ALEPH;
							}
						}
						else
						{
							rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_WAW;
						}
					}
					else
					{
						rabbitConversationType = RabbitConversationType.ENCOUNTER_CREATURE_TETH;
					}
				}
				if (rabbitConversationType != RabbitConversationType.NONE)
				{
					this._unit.Speech(rabbitConversationType, 0.6f, 3f);
				}
				this.encountered.Add(unitModel);
			}
		}
	}

	// Token: 0x040033F4 RID: 13300
	private RabbitCommandQueue _commandQueue;

	// Token: 0x040033F5 RID: 13301
	private RabbitUnit _unit;

	// Token: 0x040033F6 RID: 13302
	private RwbpType _rwbpType;

	// Token: 0x040033F7 RID: 13303
	private RabbitWeaponEnum _currentWeapon = RabbitWeaponEnum.RIFLE;

	// Token: 0x040033F8 RID: 13304
	private float _remainFireDelay;

	// Token: 0x040033F9 RID: 13305
	private bool _isDead;

	// Token: 0x040033FA RID: 13306
	private bool _activated = true;

	// Token: 0x040033FB RID: 13307
	private List<UnitModel> encountered = new List<UnitModel>();

	// Token: 0x040033FC RID: 13308
	private Timer _encounterClearTimer = new Timer();

	// Token: 0x040033FD RID: 13309
	private float encountClearFreq = 3f;
}
