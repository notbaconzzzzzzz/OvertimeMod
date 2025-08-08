using System;

// Token: 0x02000686 RID: 1670
public class SnowQueenWeapon : EquipmentScriptBase
{
	// Token: 0x060036C2 RID: 14018 RVA: 0x000308CC File Offset: 0x0002EACC
	public SnowQueenWeapon()
	{
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00031908 File Offset: 0x0002FB08
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		this.hpOld = 0f;
		this.slow = false;
		return base.OnAttackStart(actor, target);
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x00031924 File Offset: 0x0002FB24
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		this.hpOld = target.hp;
		return base.OnGiveDamage(actor, target, ref dmg);
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0003193B File Offset: 0x0002FB3B
	public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{ // <Mod>
		if (this.hpOld > target.hp)
		{
			target.AddUnitBuf(new SnowQueenSlow());
		}
		base.OnGiveDamageAfter(actor, target, dmg);
	}

	// Token: 0x040032A2 RID: 12962
	public const float slowRatio = 0.7f;

	// Token: 0x040032A3 RID: 12963
	public const float slowTime = 3f;

	// Token: 0x040032A4 RID: 12964
	private float hpOld;

	// Token: 0x040032A5 RID: 12965
	private bool slow;
}
