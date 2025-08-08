/*
Barren
Fixed weapon only hitting once
*/
using System;
using System.Collections.Generic;

// Token: 0x0200067F RID: 1663
public class SharkWeapon : EquipmentScriptBase
{
	// Token: 0x06003697 RID: 13975 RVA: 0x000301FE File Offset: 0x0002E3FE
	public SharkWeapon()
	{
	}
    
    public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		for (int i = 0; i < _COUNT_ATTACK_PER_ANIM; i++)
		{
			list.Add(base.model.metaInfo.damageInfos[i].Copy());
		}
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[0], list.ToArray());
	}

	private const int _COUNT_ATTACK_PER_ANIM = 4;
}
