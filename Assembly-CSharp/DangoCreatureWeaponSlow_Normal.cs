using System;

// Token: 0x02000659 RID: 1625
public class DangoCreatureWeaponSlow_Normal : UnitBuf
{
	// Token: 0x0600360A RID: 13834 RVA: 0x0003120B File Offset: 0x0002F40B
	public DangoCreatureWeaponSlow_Normal()
	{
		this.remainTime = 1f;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.DANGO_CREATURE_WEAPON_SLOW_NORMAL;
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x00161E18 File Offset: 0x00160018
	public override void Init(UnitModel model)
	{ // <Mod>
		base.Init(model);
		this.remainTime = 1f;
		UnitBuf unitBufByType = model.GetUnitBufByType(UnitBufType.DANGO_CREATURE_WEAPON_SLOW_SPECIAL);
		if (unitBufByType != null)
		{
			model.RemoveUnitBuf(unitBufByType);
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x00030BC3 File Offset: 0x0002EDC3
	public override float MovementScale()
	{
		return 0.3f;
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x000239A2 File Offset: 0x00021BA2
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x0400320A RID: 12810
	private const float speedFactor = 0.3f;

	// Token: 0x0400320B RID: 12811
	private const float defaultLifeTime = 1f;

	// Token: 0x0400320C RID: 12812
	private CreatureModel creature;
}
