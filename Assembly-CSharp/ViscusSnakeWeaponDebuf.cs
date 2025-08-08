using System;

// Token: 0x0200068A RID: 1674
public class ViscusSnakeWeaponDebuf : UnitBuf
{
	// Token: 0x060036BB RID: 14011 RVA: 0x000312F4 File Offset: 0x0002F4F4
	public ViscusSnakeWeaponDebuf()
	{
		this.type = UnitBufType.VISCUSSNAKE_WEAPON;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x000304B8 File Offset: 0x0002E6B8
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = _remain_time;
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x00162FE4 File Offset: 0x001611E4
	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float result = 1f;
		if (damageInfo.type == _targetType)
		{
			result = _debufDmgRatio;
		}
		return result;
	}

	// Token: 0x04003291 RID: 12945
	private const RwbpType _targetType = RwbpType.R;

	// Token: 0x04003292 RID: 12946
	private const float _debufDmgRatio = 1.5f;

	// Token: 0x04003293 RID: 12947
	private const float _remain_time = 3f;
}
