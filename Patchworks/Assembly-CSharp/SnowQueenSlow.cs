using System;
using UnityEngine;

// Token: 0x02000B91 RID: 2961
public class SnowQueenSlow : UnitBuf
{
	// Token: 0x0600594B RID: 22859 RVA: 0x00047EAD File Offset: 0x000460AD
	public SnowQueenSlow()
	{
		this.type = UnitBufType.SNOWQUEEN_WEAPON_SLOW;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x001FADEC File Offset: 0x001F8FEC
	public override void Init(UnitModel model)
	{ // <Mod>
		base.Init(model);
		this.remainTime = 3f;
	}

	// Token: 0x0600594D RID: 22861 RVA: 0x00026A30 File Offset: 0x00024C30
	public override float MovementScale()
	{ // <Mod>
		return 0.7f;
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x000239A2 File Offset: 0x00021BA2
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x0400525A RID: 21082
	private CreatureModel creature;
}
