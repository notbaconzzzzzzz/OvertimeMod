/*
+Various new overridable functions // 
*/
using System;
using UnityEngine;

// Token: 0x0200069C RID: 1692
public class EquipmentScriptBase
{
	// Token: 0x0600373F RID: 14143 RVA: 0x00004380 File Offset: 0x00002580
	public EquipmentScriptBase()
	{
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06003740 RID: 14144 RVA: 0x00031F41 File Offset: 0x00030141
	public EquipmentModel model
	{
		get
		{
			return this._model;
		}
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x00031F49 File Offset: 0x00030149
	public void SetModel(EquipmentModel m)
	{
		this._model = m;
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnEquip(UnitModel actor)
	{
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnRelease()
	{
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x00031F52 File Offset: 0x00030152
	public virtual void OnStageStart()
	{
		this._reinforcementLevel = 0;
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x00031F52 File Offset: 0x00030152
	public virtual void OnStageRelease()
	{
		this._reinforcementLevel = 0;
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnPrepareWeapon(UnitModel actor)
	{
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnCancelWeapon(UnitModel actor)
	{
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0016872C File Offset: 0x0016692C
	public virtual EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		if (!(this._model is WeaponModel))
		{
			return null;
		}
		return new EquipmentScriptBase.WeaponDamageInfo(this._model.metaInfo.animationNames[0], new DamageInfo[]
		{
			(this._model as WeaponModel).GetDamage(actor)
		});
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnAttackEnd(UnitModel actor, UnitModel target)
	{
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnKillMainTarget(UnitModel actor, UnitModel target)
	{
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x00014081 File Offset: 0x00012281
	public virtual bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		return true;
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x00014081 File Offset: 0x00012281
	public virtual bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		return true;
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00014081 File Offset: 0x00012281
	public virtual bool OnTakeDamage_After(float value, RwbpType type)
	{
		return true;
	}

    // <Mod>
	public virtual void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
	{
		
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x0600374F RID: 14159 RVA: 0x00031F5B File Offset: 0x0003015B
	public int reinforcementLevel
	{
		get
		{
			return this._reinforcementLevel;
		}
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0016877C File Offset: 0x0016697C
	public float GetReinforcementDmg()
	{
		float result;
		switch (this._reinforcementLevel)
		{
		case 0:
			result = 1f;
			break;
		case 1:
			result = 1.2f;
			break;
		case 2:
			result = 1.4f;
			break;
		case 3:
			result = 1.7f;
			break;
		case 4:
			result = 2f;
			break;
		default:
			result = 1f;
			break;
		}
		return result;
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00031F63 File Offset: 0x00030163
	public void AddReinforcementLevel(int lv)
	{
		this._reinforcementLevel += lv;
		this._reinforcementLevel = Mathf.Clamp(this._reinforcementLevel, 0, 4);
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x00031F86 File Offset: 0x00030186
	public virtual DefenseInfo GetDefense(UnitModel actor)
	{
		return this.model.metaInfo.defenseInfo.Copy();
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0002215C File Offset: 0x0002035C
	public virtual float GetDamageFactor()
	{
		return 1f;
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x00031F9D File Offset: 0x0003019D
	public virtual DamageInfo GetDamage(UnitModel actor)
	{
		return this.model.metaInfo.damageInfo.Copy();
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnFixedUpdate()
	{
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x00031FB4 File Offset: 0x000301B4
	public virtual EGObonusInfo GetBonus(UnitModel actor)
	{
		return this.model.metaInfo.bonus;
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x00022270 File Offset: 0x00020470
	public virtual float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		return 0f;
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OwnerHeal(bool isMental, ref float amount)
	{
	}

	// <Mod>
	public virtual float RecoveryMultiplier(bool isMental, float amount)
	{
		return 1f;
	}

	// <Mod>
	public virtual float RecoveryAdditiveMultiplier(bool isMental, float amount)
	{
		return 0f;
	}

	// <Mod>
	public virtual float WorkSpeedModifier(CreatureModel target, SkillTypeInfo skill)
	{
		return 1f;
	}

    // <Mod>
	public virtual Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
	{
		return Vector2.zero;
	}

	// Token: 0x040032E2 RID: 13026
	private EquipmentModel _model;

	// Token: 0x040032E3 RID: 13027
	private int _reinforcementLevel;

	// Token: 0x040032E4 RID: 13028
	protected const int MAX_REINFORCEMENT_LEVEL = 4;

	// Token: 0x0200069D RID: 1693
	public class WeaponDamageInfo
	{
		// Token: 0x06003759 RID: 14169 RVA: 0x00004380 File Offset: 0x00002580
		public WeaponDamageInfo()
		{
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x00031FC6 File Offset: 0x000301C6
		public WeaponDamageInfo(string animationName, DamageInfo[] dmgs)
		{
			this.animationName = animationName;
			this.dmgs = dmgs;
		}

		// Token: 0x040032E5 RID: 13029
		public string animationName;

		// Token: 0x040032E6 RID: 13030
		public DamageInfo[] dmgs;
	}
}
