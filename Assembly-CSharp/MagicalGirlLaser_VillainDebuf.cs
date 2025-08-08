using System;

// Token: 0x020004E9 RID: 1257
public class MagicalGirlLaser_VillainDebuf : UnitBuf
{
	// Token: 0x06002D5A RID: 11610 RVA: 0x0002BB96 File Offset: 0x00029D96
	public MagicalGirlLaser_VillainDebuf()
	{
		this.remainTime = 1f;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.MAGICAL_LASER;
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x00131A70 File Offset: 0x0012FC70
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 1f;
		if (model is CreatureModel)
		{
			this.creature = (model as CreatureModel);
			this.creature.movementScale = this.creature.movementScale * this.MovementScale();
		}
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x00026A30 File Offset: 0x00024C30
	public override float MovementScale()
	{
		return 0.5f;
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x000239A2 File Offset: 0x00021BA2
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x0002BBB8 File Offset: 0x00029DB8
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.creature != null)
		{
			this.creature.movementScale = this.creature.movementScale / this.MovementScale();
		}
	}

	// Token: 0x04002AF4 RID: 10996
	private const float speedFactor = 0.5f;

	// Token: 0x04002AF5 RID: 10997
	private const float defaultLifeTime = 1f;

	// Token: 0x04002AF6 RID: 10998
	private CreatureModel creature;
}
