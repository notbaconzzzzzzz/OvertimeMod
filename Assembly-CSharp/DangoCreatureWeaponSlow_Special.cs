using System;

// Token: 0x0200065A RID: 1626
public class DangoCreatureWeaponSlow_Special : UnitBuf
{
	// Token: 0x0600360F RID: 13839 RVA: 0x0003125D File Offset: 0x0002F45D
	public DangoCreatureWeaponSlow_Special()
	{
		this.remainTime = 3f;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.DANGO_CREATURE_WEAPON_SLOW_SPECIAL;
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00161E84 File Offset: 0x00160084
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 3f;
		UnitBuf unitBufByType = model.GetUnitBufByType(UnitBufType.DANGO_CREATURE_WEAPON_SLOW_NORMAL);
		if (unitBufByType != null)
		{
			model.RemoveUnitBuf(unitBufByType);
		}
		if (model is CreatureModel)
		{
			this.creature = (model as CreatureModel);
			this.creature.movementScale = this.creature.movementScale * this.MovementScale();
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00026A30 File Offset: 0x00024C30
	public override float MovementScale()
	{
		return 0.5f;
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x000239A2 File Offset: 0x00021BA2
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x0003127F File Offset: 0x0002F47F
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.creature != null)
		{
			this.creature.movementScale = this.creature.movementScale / this.MovementScale();
		}
	}

	// Token: 0x0400320D RID: 12813
	private const float speedFactor = 0.5f;

	// Token: 0x0400320E RID: 12814
	private const float defaultLifeTime = 3f;

	// Token: 0x0400320F RID: 12815
	private CreatureModel creature;
}
