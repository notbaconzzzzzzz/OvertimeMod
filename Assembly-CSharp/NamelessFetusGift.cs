using System;

// Token: 0x0200063E RID: 1598
public class NamelessFetusGift : EquipmentScriptBase
{
	// Token: 0x060035CA RID: 13770 RVA: 0x00030BA6 File Offset: 0x0002EDA6
	public NamelessFetusGift()
	{
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x0003111D File Offset: 0x0002F31D
	public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		dmg.min *= 0.95f;
		dmg.max *= 0.95f;
		return base.OnTakeDamage(actor, ref dmg);
	}
}
