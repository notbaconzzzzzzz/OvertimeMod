using System;

// Token: 0x0200065C RID: 1628
public class DeathAngelWeaponSlow : UnitBuf
{
	// Token: 0x0600362B RID: 13867 RVA: 0x000313C4 File Offset: 0x0002F5C4
	public DeathAngelWeaponSlow()
	{
		this.remainTime = 0.5f;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.DEATH_ANGEL_WEAPON_SLOW;
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x001624C4 File Offset: 0x001606C4
	public override void Init(UnitModel model)
	{ // <Mod>
		base.Init(model);
		this.remainTime = 0.5f;
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00028B62 File Offset: 0x00026D62
	public override float MovementScale()
	{
		return 0.4f;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x000239A2 File Offset: 0x00021BA2
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x0400322A RID: 12842
	private const float speedFactor = 0.4f;

	// Token: 0x0400322B RID: 12843
	private const float defaultLifeTime = 0.5f;

	// Token: 0x0400322C RID: 12844
	private CreatureModel creature;
}
