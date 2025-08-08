using System;

// Token: 0x0200068F RID: 1679
public class YoungPrinceWeaponDebuf : UnitBuf
{
	// Token: 0x060036C5 RID: 14021 RVA: 0x00031341 File Offset: 0x0002F541
	public YoungPrinceWeaponDebuf()
	{
		this.type = UnitBufType.YOUNGPRINCE_WEAPON;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x000304B8 File Offset: 0x0002E6B8
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = _remain_time;
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x001630C4 File Offset: 0x001612C4
	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float result = 1f;
		if (damageInfo.type == _targetType)
		{
			result = _debufDmgRatio;
		}
		return result;
	}

	// Token: 0x04003297 RID: 12951
	private const RwbpType _targetType = RwbpType.W;

	// Token: 0x04003298 RID: 12952
	private const float _debufDmgRatio = 1.5f;

	// Token: 0x04003299 RID: 12953
	private const float _remain_time = 1.5f;
}
