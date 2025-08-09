using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class CensoredWeapon : EquipmentScriptBase
{
	// Token: 0x06003664 RID: 13924 RVA: 0x000316C8 File Offset: 0x0002F8C8
	public CensoredWeapon()
	{
		this.defaultInfo.type = SplashType.NONE;
		this.specialInfo.type = SplashType.PENETRATION;
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x000316FE File Offset: 0x0002F8FE
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.SetNextAttackType();
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x001696EC File Offset: 0x001678EC
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		if (this.isSpecial)
		{
			for (int i = 0; i < 3; i++)
			{
				list.Add(base.model.metaInfo.damageInfos[i].Copy());
			}
			animationName = base.model.metaInfo.animationNames[0];
		}
		else
		{
			for (int j = 3; j < 5; j++)
			{
				list.Add(base.model.metaInfo.damageInfos[j].Copy());
			}
			animationName = base.model.metaInfo.animationNames[1];
		}
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x0003170C File Offset: 0x0002F90C
	public override void OnAttackEnd(UnitModel actor, UnitModel target)
	{
		base.OnAttackEnd(actor, target);
		this.SetNextAttackType();
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x001697A8 File Offset: 0x001679A8
	private void SetNextAttackType()
	{
		if (UnityEngine.Random.value < 0.1f)
		{
			this.isSpecial = true;
			base.model.metaInfo.range = 10f;
			base.model.metaInfo.splashInfo = this.specialInfo;
		}
		else
		{
			this.isSpecial = false;
			base.model.metaInfo.range = 3f;
			base.model.metaInfo.splashInfo = this.defaultInfo;
		}
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x0003171C File Offset: 0x0002F91C
    /*
	public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		this.hp_old = base.model.owner.hp;
		this.mp_old = base.model.owner.mental;
		return base.OnTakeDamage(actor, ref dmg);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00169830 File Offset: 0x00167A30
	public override bool OnTakeDamage_After(float value, RwbpType type)
	{
		WorkerModel workerModel = base.model.owner as WorkerModel;
		if (workerModel == null)
		{
			return false;
		}
		float num = this.hp_old - workerModel.hp;
		float num2 = this.mp_old - workerModel.mental;
		if (num > 0f)
		{
			workerModel.RecoverHP(num * 0.4f);
		}
		if (num2 > 0f)
		{
			workerModel.RecoverMental(num2 * 0.4f);
		}
		return base.OnTakeDamage_After(value, type);
	}*/

    // <Mod>
    public override Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
    {
        return new Vector2(40f, 40f);
    }

	// Token: 0x04003237 RID: 12855
	private const int _defaultRange = 3;

	// Token: 0x04003238 RID: 12856
	private const int _specialRange = 10;

	// Token: 0x04003239 RID: 12857
	private const float _healRatio = 0.4f;

	// Token: 0x0400323A RID: 12858
	private const float _specialProb = 0.1f;

	// Token: 0x0400323B RID: 12859
	private SplashInfo defaultInfo = new SplashInfo();

	// Token: 0x0400323C RID: 12860
	private SplashInfo specialInfo = new SplashInfo();

	// Token: 0x0400323D RID: 12861
	private bool isSpecial;

	// Token: 0x0400323E RID: 12862
	private float hp_old;

	// Token: 0x0400323F RID: 12863
	private float mp_old;
}
