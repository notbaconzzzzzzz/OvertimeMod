using System;

// Token: 0x02000685 RID: 1669
public class SnowWhiteWeapon : EquipmentScriptBase
{
	// Token: 0x060036AF RID: 13999 RVA: 0x000301FE File Offset: 0x0002E3FE
	public SnowWhiteWeapon()
	{
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x00162EF0 File Offset: 0x001610F0
	public override DamageInfo GetDamage(UnitModel actor)
	{ // <Mod> change damage increase from 5 to 2
		if (actor != null && actor.HasEquipment(400023))
		{
			DamageInfo damage = base.GetDamage(actor);
			damage.min += 2f;
			damage.max += 2f;
			return damage;
		}
		return base.GetDamage(actor);
	}
}
