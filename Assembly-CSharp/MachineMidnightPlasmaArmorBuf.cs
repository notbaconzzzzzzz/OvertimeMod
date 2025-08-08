using System;

public class MachineMidnightPlasmaArmorBuf : UnitBuf
{
	public MachineMidnightPlasmaArmorBuf(float _threshold)
	{
		this.type = UnitBufType.MACHINE_MIDNIGHT_PLASMA_ARMOR;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		threshold = _threshold;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 10f;
	}

	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float num = model.hp / model.maxHp;
		num = (num - threshold) / 0.2f;
		if (num >= 1f)
		{
			return 1f;
		}
		if (num <= 0f)
		{
			return 0f;
		}
		return num;
	}

	private float threshold;
}
