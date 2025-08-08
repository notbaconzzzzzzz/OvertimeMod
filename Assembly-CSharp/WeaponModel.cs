using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000695 RID: 1685
public class WeaponModel : EquipmentModel
{
	// Token: 0x060036E4 RID: 14052 RVA: 0x00031702 File Offset: 0x0002F902
	public WeaponModel()
	{
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x001635A8 File Offset: 0x001617A8
	public bool InRange(UnitModel actor, UnitModel target)
	{
		float distance = MovableObjectNode.GetDistance(actor.GetMovableNode(), target.GetMovableNode());
		float num = distance - actor.radius - target.radius;
		return num <= this.metaInfo.range;
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x001635E8 File Offset: 0x001617E8
	public bool InRangeDirectionalAtDamageTime(UnitModel actor, UnitModel target, float addedRange)
	{
		UnitDirection direction = actor.GetMovableNode().GetDirection();
		float x = actor.GetMovableNode().GetCurrentViewPosition().x;
		float x2 = target.GetMovableNode().GetCurrentViewPosition().x;
		bool flag = x < x2;
		bool flag2 = x > x2;
		if (flag && direction == UnitDirection.LEFT)
		{
			return false;
		}
		if (flag2 && direction == UnitDirection.RIGHT)
		{
			return false;
		}
		float distance = MovableObjectNode.GetDistance(actor.GetMovableNode(), target.GetMovableNode());
		float num = distance - actor.radius - target.radius;
		return num <= this.metaInfo.range + addedRange;
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00163690 File Offset: 0x00161890
	public string OnAttack(UnitModel actor, UnitModel target)
	{
		this.fireEffectRunned = false;
		this.remainDelay = 8f;
		this.currentTarget = target;
		EquipmentScriptBase.WeaponDamageInfo weaponDamageInfo = this.script.OnAttackStart(actor, target);
		this._currentDamageInfos = new Queue<DamageInfo>(weaponDamageInfo.dmgs);
		if (weaponDamageInfo.dmgs[0].soundInfo != null && weaponDamageInfo.dmgs[0].soundInfo.soundType == DamageInfo_EffectType.ANIM_START)
		{
			weaponDamageInfo.dmgs[0].soundInfo.PlaySound(target.GetCurrentViewPosition());
		}
		if (weaponDamageInfo.dmgs[0].effectInfos.Count > 0)
		{
			foreach (EffectInfo effectInfo in weaponDamageInfo.dmgs[0].effectInfos)
			{
				if (effectInfo.effectType == DamageInfo_EffectType.ANIM_START)
				{
					if (effectInfo.invokedUnit == EffectInvokedUnit.OWNER)
					{
						if (!this.fireEffectRunned)
						{
							effectInfo.MakeEffect(actor.GetMovableNode());
							this.fireEffectRunned = true;
						}
					}
					else
					{
						effectInfo.MakeEffect(target.GetMovableNode());
					}
				}
			}
		}
		return weaponDamageInfo.animationName;
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00031715 File Offset: 0x0002F915
	public void OnEndAttackCycle()
	{
		this.script.OnAttackEnd(base.owner, this.currentTarget);
		this.remainDelay = 0f;
		this._currentDamageInfos.Clear();
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x00031744 File Offset: 0x0002F944
	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if (this.remainDelay > 0f)
		{
			this.remainDelay -= Time.deltaTime;
		}
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x001637D0 File Offset: 0x001619D0
	public void OnGiveDamage(UnitModel actor)
	{
		try
		{
			if (this.currentTarget != null)
			{
				this.fireEffectRunned = false;
				if (this._currentDamageInfos.Count != 0)
				{
					DamageInfo damageInfo = this._currentDamageInfos.Dequeue();
					float num = actor.GetDamageFactorByEquipment();
					num *= actor.GetDamageFactorBySefiraAbility();
					float reinforcementDmg = this.script.GetReinforcementDmg();
					damageInfo.min = damageInfo.min * num * reinforcementDmg;
					damageInfo.max = damageInfo.max * num * reinforcementDmg;
					List<UnitModel> list = new List<UnitModel>();
					SplashInfo splashInfo = this.metaInfo.splashInfo;
					if ((splashInfo.type == SplashType.PENETRATION || splashInfo.type == SplashType.SPLASH) && actor.GetMovableNode().GetPassage() != null)
					{
						foreach (MovableObjectNode movableObjectNode in actor.GetMovableNode().GetPassage().GetEnteredTargets())
						{
							UnitModel unit = movableObjectNode.GetUnit();
							if (unit != null && unit != actor && unit != this.currentTarget && unit.IsAttackTargetable() && (actor.IsHostile(unit) || !splashInfo.iff))
							{
								list.Add(unit);
							}
						}
					}
					if (this.currentTarget.IsAttackTargetable() && this.InRangeDirectionalAtDamageTime(actor, this.currentTarget, 2f))
					{
						DamageInfo damageInfo2 = damageInfo.Copy();
						bool flag = this.script.OnGiveDamage(actor, this.currentTarget, ref damageInfo2);
						if (actor.Equipment.armor != null && actor.Equipment.armor.script != null && !actor.Equipment.armor.script.OnGiveDamage(actor, this.currentTarget, ref damageInfo2))
						{
							flag = false;
						}
						if (actor.Equipment.gifts != null && !actor.Equipment.gifts.OnGiveDamage(actor, this.currentTarget, ref damageInfo2))
						{
							flag = false;
						}
						if (actor.GetUnitBufList().Count > 0)
						{
							using (List<UnitBuf>.Enumerator enumerator2 = actor.GetUnitBufList().GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (!enumerator2.Current.OnGiveDamage(actor, this.currentTarget, ref damageInfo2))
									{
										flag = false;
									}
								}
							}
						}
						if (flag)
						{
							this.currentTarget.TakeDamage(actor, damageInfo2);
							this.script.OnGiveDamageAfter(actor, this.currentTarget, damageInfo2);
							if (actor.Equipment.armor != null && actor.Equipment.armor.script != null)
							{
								actor.Equipment.armor.script.OnGiveDamageAfter(actor, this.currentTarget, damageInfo2);
							}
							if (actor.GetUnitBufList().Count > 0)
							{
								foreach (UnitBuf unitBuf in actor.GetUnitBufList())
								{
									unitBuf.OnGiveDamageAfter(actor, this.currentTarget, damageInfo2);
								}
							}
							if (this.currentTarget is WorkerModel && (this.currentTarget as WorkerModel).IsDead())
							{
								this.script.OnKillMainTarget(actor, this.currentTarget);
							}
							this.InvokeEffect(this.currentTarget, damageInfo2, this.GetDir(actor, this.currentTarget));
						}
					}
					if (splashInfo.type == SplashType.PENETRATION)
					{
						using (List<UnitModel>.Enumerator enumerator4 = list.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								UnitModel unitModel = enumerator4.Current;
								if (unitModel.IsAttackTargetable() && this.InRangeDirectionalAtDamageTime(actor, unitModel, 1f))
								{
									DamageInfo damageInfo3 = damageInfo.Copy();
									bool flag2 = this.script.OnGiveDamage(actor, unitModel, ref damageInfo3);
									if (actor.Equipment.armor != null && actor.Equipment.armor.script != null && !actor.Equipment.armor.script.OnGiveDamage(actor, this.currentTarget, ref damageInfo3))
									{
										flag2 = false;
									}
									if (!actor.Equipment.gifts.OnGiveDamage(actor, this.currentTarget, ref damageInfo3))
									{
										flag2 = false;
									}
									if (actor.GetUnitBufList().Count > 0)
									{
										using (List<UnitBuf>.Enumerator enumerator5 = actor.GetUnitBufList().GetEnumerator())
										{
											while (enumerator5.MoveNext())
											{
												if (!enumerator5.Current.OnGiveDamage(actor, this.currentTarget, ref damageInfo3))
												{
													flag2 = false;
												}
											}
										}
									}
									if (flag2)
									{
										unitModel.TakeDamage(actor, damageInfo3);
										this.script.OnGiveDamageAfter(actor, unitModel, damageInfo3);
										actor.Equipment.armor.script.OnGiveDamageAfter(actor, this.currentTarget, damageInfo3);
										if (actor.GetUnitBufList().Count > 0)
										{
											foreach (UnitBuf unitBuf2 in actor.GetUnitBufList())
											{
												unitBuf2.OnGiveDamageAfter(actor, this.currentTarget, damageInfo3);
											}
										}
										this.InvokeEffect(unitModel, damageInfo3, this.GetDir(actor, unitModel));
									}
								}
							}
							return;
						}
					}
					if (this.metaInfo.splashInfo.type == SplashType.SPLASH)
					{
						foreach (UnitModel unitModel2 in list)
						{
							if (unitModel2.IsAttackTargetable() && MovableObjectNode.GetDistance(unitModel2.GetMovableNode(), this.currentTarget.GetMovableNode()) - unitModel2.radius <= this.metaInfo.splashInfo.range)
							{
								DamageInfo damageInfo4 = damageInfo.Copy();
								bool flag3 = this.script.OnGiveDamage(actor, unitModel2, ref damageInfo4);
								if (actor.Equipment.armor != null && actor.Equipment.armor.script != null && !actor.Equipment.armor.script.OnGiveDamage(actor, this.currentTarget, ref damageInfo4))
								{
									flag3 = false;
								}
								if (!actor.Equipment.gifts.OnGiveDamage(actor, this.currentTarget, ref damageInfo4))
								{
									flag3 = false;
								}
								if (actor.GetUnitBufList().Count > 0)
								{
									using (List<UnitBuf>.Enumerator enumerator7 = actor.GetUnitBufList().GetEnumerator())
									{
										while (enumerator7.MoveNext())
										{
											if (!enumerator7.Current.OnGiveDamage(actor, this.currentTarget, ref damageInfo4))
											{
												flag3 = false;
											}
										}
									}
								}
								if (flag3)
								{
									unitModel2.TakeDamage(actor, damageInfo4);
									this.script.OnGiveDamageAfter(actor, unitModel2, damageInfo4);
									actor.Equipment.armor.script.OnGiveDamageAfter(actor, this.currentTarget, damageInfo4);
									if (actor.GetUnitBufList().Count > 0)
									{
										foreach (UnitBuf unitBuf3 in actor.GetUnitBufList())
										{
											unitBuf3.OnGiveDamageAfter(actor, this.currentTarget, damageInfo4);
										}
									}
									this.InvokeEffect(unitModel2, damageInfo4, this.GetDir(actor, unitModel2));
								}
							}
						}
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x00163FA4 File Offset: 0x001621A4
	public UnitDirection GetDir(UnitModel attacker, UnitModel target)
	{
		float x = target.GetCurrentViewPosition().x;
		float x2 = attacker.GetCurrentViewPosition().x;
		return (x < x2) ? UnitDirection.LEFT : UnitDirection.RIGHT;
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x00163FE0 File Offset: 0x001621E0
	public void InvokeEffect(UnitModel unit, DamageInfo damageInfo, UnitDirection dir)
	{
		RwbpType type = damageInfo.type;
		DefenseInfo defense = unit.defense;
		if (damageInfo.soundInfo != null && damageInfo.soundInfo.soundType == DamageInfo_EffectType.DAMAGE_INVOKED)
		{
			damageInfo.soundInfo.PlaySound(unit.GetCurrentViewPosition());
		}
		if (damageInfo.effectInfos.Count > 0)
		{
			foreach (EffectInfo effectInfo in damageInfo.effectInfos)
			{
				if (effectInfo.effectType == DamageInfo_EffectType.DAMAGE_INVOKED)
				{
					if (effectInfo.invokedUnit == EffectInvokedUnit.OWNER)
					{
						if (!effectInfo.invokeOnce)
						{
							this.fireEffectRunned = false;
						}
						if (!this.fireEffectRunned)
						{
							effectInfo.MakeEffect(base.owner.GetMovableNode());
							this.fireEffectRunned = true;
						}
					}
					else
					{
						effectInfo.MakeEffect(unit.GetMovableNode());
					}
				}
			}
		}
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, type, defense, dir);
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x001640F4 File Offset: 0x001622F4
	public static WeaponModel MakeWeapon(EquipmentTypeInfo info)
	{
		WeaponModel weaponModel = new WeaponModel();
		weaponModel.metaInfo = info;
		Type type = Type.GetType(info.script);
		object obj = null;
		if (type != null)
		{
			foreach (Assembly assembly in Add_On.instance.AssemList)
			{
				foreach (Type type2 in assembly.GetTypes())
				{
					if (type2.Name == info.script)
					{
						obj = Activator.CreateInstance(type2);
					}
				}
			}
			if (obj == null)
			{
				obj = Activator.CreateInstance(type);
			}
		}
		if (obj is EquipmentScriptBase)
		{
			weaponModel.script = (EquipmentScriptBase)obj;
		}
		weaponModel.script.SetModel(weaponModel);
		return weaponModel;
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x0003176E File Offset: 0x0002F96E
	public static WeaponModel GetDummyWeapon()
	{
		return WeaponModel.MakeWeapon(EquipmentTypeInfo.GetDummyInfo());
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x0003177A File Offset: 0x0002F97A
	public static WeaponModel GetOfficerWeapon()
	{
		return WeaponModel.MakeWeapon(EquipmentTypeList.instance.GetData(2));
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x0003178C File Offset: 0x0002F98C
	public DamageInfo GetDamage(UnitModel actor)
	{
		return this.script.GetDamage(actor);
	}

	// Token: 0x040032A6 RID: 12966
	public float remainDelay;

	// Token: 0x040032A7 RID: 12967
	private bool fireEffectRunned;

	// Token: 0x040032A8 RID: 12968
	private Queue<DamageInfo> _currentDamageInfos = new Queue<DamageInfo>();
}
