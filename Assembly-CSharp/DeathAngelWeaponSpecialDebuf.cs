using System;

public class DeathAngelWeaponSpecialDebuf : UnitBuf
{
	public DeathAngelWeaponSpecialDebuf()
	{
		this.type = UnitBufType.DEATH_ANGEL_WEAPON_SPECIAL_RES;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = _remain_time;
	}

	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float result = 1f;
		if (damageInfo.type == _targetType)
		{
			result = _debufDmgRatio;
		}
		return result;
	}

	private const RwbpType _targetType = RwbpType.P;

	private const float _debufDmgRatio = 2f;

	private const float _remain_time = 5f;
}
