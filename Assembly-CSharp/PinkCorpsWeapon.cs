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

    public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
    {
        if (target == null) return true;
        MovableObjectNode self = actor.GetMovableNode();
        MovableObjectNode other = target.GetMovableNode();
        if (self == null || other == null) return true;
        PassageObjectModel passage = self.GetPassage();
        if (passage != other.GetPassage() || passage == null) return true;
        float x = self.GetCurrentViewPosition().x;
        float x2 = other.GetCurrentViewPosition().x;
        float num = Math.Abs(x - x2) / self.currentScale;
		float num2 = 1f;
        if (num >= 45f)
        {
            num2 = 2f;
        }
        else if (num >= 27f)
        {
            num2 = 1.6f;
        }
        else if (num >= 15f)
        {
            num2 = 1.3f;
        }
        else if (num >= 8f)
        {
            num2 = 1.1f;
        }
        dmg.min *= num2;
        dmg.max *= num2;
        return true;
    }
}
