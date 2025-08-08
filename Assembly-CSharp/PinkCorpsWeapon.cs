using System;

// Token: 0x02000678 RID: 1656
public class PinkCorpsWeapon : EquipmentScriptBase
{
	// Token: 0x060036A4 RID: 13988 RVA: 0x00030B8C File Offset: 0x0002ED8C
	public PinkCorpsWeapon()
	{
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00165380 File Offset: 0x00163580
	public override DamageInfo GetDamage(UnitModel actor)
	{
		if (actor != null)
		{
			DamageInfo damage = base.GetDamage(actor);
			if (actor.HasEquipment(300064) && actor.HasEquipment(400064))
			{
				damage.min *= 1.15f;
				damage.max *= 1.15f;
			}
			return damage;
		}
		return base.GetDamage(actor);
	}
}
