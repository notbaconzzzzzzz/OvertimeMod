using System;

// Token: 0x02000625 RID: 1573
public class OrchestraArmor : EquipmentScriptBase
{
	// Token: 0x0600357C RID: 13692 RVA: 0x00030B37 File Offset: 0x0002ED37
	public OrchestraArmor()
	{
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x00160ED0 File Offset: 0x0015F0D0
	public override DefenseInfo GetDefense(UnitModel actor)
	{
		if (actor != null && actor.HasEquipment(400019))
		{
			DefenseInfo defense = base.GetDefense(actor);
			defense.W = -1f;
			return defense;
		}
		return base.GetDefense(actor);
	}
}
