using System;
using System.Collections.Generic;

// Token: 0x02000657 RID: 1623
public class ButterflyWeapon : EquipmentScriptBase
{
	// Token: 0x06003647 RID: 13895 RVA: 0x00166E14 File Offset: 0x00165014
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		list.Add(base.model.metaInfo.damageInfos[0].Copy());
		list.Add(base.model.metaInfo.damageInfos[1].Copy());
		animationName = base.model.metaInfo.animationNames[0];
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}
}
