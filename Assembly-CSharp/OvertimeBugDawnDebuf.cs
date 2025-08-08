using System;

public class OvertimeBugDawnDebuf : UnitBuf
{
	public OvertimeBugDawnDebuf()
	{
		type = UnitBufType.OVERTIME_BUG_DAWN;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 5f;
	}

	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		float result = 1f;
		if (damageInfo.type == RwbpType.R)
		{
			int amt = 25;
			amt += stacks;
			result = 1f + (float)amt / 100f;
		}
		return result;
	}

    public override float MovementScale()
    {
		float result = 1f;
		int amt = 50;
		amt += stacks * 2;
		if (amt > 200)
		{
			amt = 200 + (amt - 200)/4;
		}
		if (amt > 400)
		{
			amt = 400;
		}
		result = 1f + (float)amt / 100f;
        return 1f / result;
    }

	public void BugDawnHit(bool mainTarget)
	{
		if (remainTime < 3f)
		{
			_stacks = (int)((float)_stacks * remainTime / 3f);
		}
		remainTime = 5f;
		if (mainTarget)
		{
			_stacks += 5;
		}
		else
		{
			_stacks += 1;
		}
	}

	public int stacks
	{
		get
		{
			if (remainTime < 3f)
			{
				return (int)((float)_stacks * remainTime / 3f);
			}
			return _stacks;
		}
	}

	private int _stacks;
}