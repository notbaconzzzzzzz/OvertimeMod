using System;
using UnityEngine;

// Token: 0x02000690 RID: 1680
public class CosmosInnerWeapon : EquipmentScriptBase
{
	// Token: 0x060036C9 RID: 14025 RVA: 0x00030466 File Offset: 0x0002E666
	public CosmosInnerWeapon()
	{
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x000315A9 File Offset: 0x0002F7A9
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[UnityEngine.Random.Range(0, 2)], new DamageInfo[]
		{
			(base.model as WeaponModel).GetDamage(actor)
		});
	}
}
