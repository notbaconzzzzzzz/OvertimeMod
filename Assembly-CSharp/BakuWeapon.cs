using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064C RID: 1612
internal class BakuWeapon : EquipmentScriptBase
{
	// Token: 0x06003613 RID: 13843 RVA: 0x00166128 File Offset: 0x00164328
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		for (int i = 0; i < this._COUNT_ATTACK_PER_ANIM; i++)
		{
			float value = UnityEngine.Random.value;
			DamageInfo item = base.model.metaInfo.damageInfos[0].Copy();
			if (value < 0.5f)
			{
				item = base.model.metaInfo.damageInfos[1].Copy();
			}
			list.Add(item);
		}
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[0], list.ToArray());
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x000140A1 File Offset: 0x000122A1
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		return true;
	}

	// Token: 0x040031F4 RID: 12788
	private int _COUNT_ATTACK_PER_ANIM = 3;
}
