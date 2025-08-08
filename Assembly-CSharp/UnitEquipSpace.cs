using System;

// Token: 0x020006B2 RID: 1714
public class UnitEquipSpace
{
	// Token: 0x060037C5 RID: 14277 RVA: 0x0016CA7C File Offset: 0x0016AC7C
	public EGObonusInfo GetBonus(UnitModel actor)
	{
		EGObonusInfo a = new EGObonusInfo();
		if (this.weapon != null)
		{
			a += this.weapon.GetBonus(actor);
		}
		if (this.armor != null)
		{
			a += this.armor.GetBonus(actor);
		}
		return a + this.gifts.GetBonus(actor);
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x0003248E File Offset: 0x0003068E
	public float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		return this.gifts.GetWorkProbSpecialBonus(actor, skill);
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0016CAE0 File Offset: 0x0016ACE0
	public bool HasEquipment(int id)
	{
		return (this.weapon != null && this.weapon.metaInfo.id == id) || (this.armor != null && this.armor.metaInfo.id == id) || this.gifts.HasEquipment(id);
	}

	// Token: 0x0400330C RID: 13068
	public WeaponModel weapon;

	// Token: 0x0400330D RID: 13069
	public ArmorModel armor;

	// Token: 0x0400330E RID: 13070
	public UnitEGOgiftSpace gifts = new UnitEGOgiftSpace();

	// Token: 0x0400330F RID: 13071
	public CreatureModel kitCreature;
}
