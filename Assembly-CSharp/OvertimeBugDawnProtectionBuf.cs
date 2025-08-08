using System;

public class OvertimeBugDawnProtectionBuf : UnitBuf
{
	public OvertimeBugDawnProtectionBuf()
	{
		type = UnitBufType.OVERTIME_BUG_DAWN_PROT;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 0.5f;
	}

	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float result = 1f;
        int amt = 0;
        amt += stacks;
        result = 1f + (float)amt / 100f;
		return 1f / result;
	}

	public void BugDawnAttacked(int amt = 100)
	{
		if (remainTime < 0.5f)
		{
			_stacks = (int)((float)_stacks * remainTime / 0.5f);
		}
		remainTime = 0.5f;
		_stacks += amt;
	}

	public int stacks
	{
		get
		{
			if (remainTime < 0.5f)
			{
				return (int)((float)_stacks * remainTime / 0.5f);
			}
			return _stacks;
		}
	}

	private int _stacks;
}