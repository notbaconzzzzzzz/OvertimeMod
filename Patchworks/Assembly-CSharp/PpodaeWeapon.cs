using System;
using System.Collections.Generic;

// Token: 0x0200065F RID: 1631
public class PpodaeWeapon : EquipmentScriptBase
{
	// Token: 0x0600360E RID: 13838 RVA: 0x0015F790 File Offset: 0x0015D990
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		for (int i = 0; i < this._COUNT_ATTACK_PER_ANIM; i++)
		{
			list.Add(base.model.metaInfo.damageInfo);
		}
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[0], list.ToArray());
	}

	// Token: 0x0400321E RID: 12830
	private int _COUNT_ATTACK_PER_ANIM = 2;
}
