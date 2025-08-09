using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public class NothingWeapon : EquipmentScriptBase
{
	// Token: 0x060036E2 RID: 14050 RVA: 0x00030DC9 File Offset: 0x0002EFC9
	public NothingWeapon()
	{
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x0016B260 File Offset: 0x00169460
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		if (UnityEngine.Random.value > 0.1f)
		{
			list.Add(base.model.metaInfo.damageInfos[0].Copy());
			animationName = base.model.metaInfo.animationNames[0];
		}
		else
		{
			list.Add(base.model.metaInfo.damageInfos[1].Copy());
			animationName = base.model.metaInfo.animationNames[1];
		}
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x00031BE8 File Offset: 0x0002FDE8
    /*
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		this._hpOld = target.hp;
		return base.OnGiveDamage(actor, target, ref dmg);
	}*/

	// Token: 0x060036E5 RID: 14053 RVA: 0x0016B2FC File Offset: 0x001694FC
	public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
		base.OnGiveDamageAfter(actor, target, dmg);
        /*
		float num = this._hpOld - target.hp;
		if (num > 0f && actor is WorkerModel)
		{
			WorkerModel workerModel = actor as WorkerModel;
			workerModel.RecoverHP(num * 0.25f);
		}*/
		if (dmg.result.resultDamage > 0f && actor is WorkerModel)
		{
			WorkerModel workerModel = actor as WorkerModel;
			workerModel.RecoverHP(dmg.result.resultDamage * 0.25f);
		}
	}

	// Token: 0x040032AA RID: 12970
	private const float lifeGainRatio = 0.25f;

	// Token: 0x040032AB RID: 12971
	private const float pattern2Prob = 0.1f;

	// Token: 0x040032AC RID: 12972
	private float _hpOld;
}
