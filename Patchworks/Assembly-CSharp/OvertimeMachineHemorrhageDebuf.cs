using System;
using UnityEngine;

public class OvertimeMachineHemorrhageDebuf : UnitBuf
{
	public OvertimeMachineHemorrhageDebuf()
	{
		type = UnitBufType.OVERTIME_MACHINE_HEMORRHAGE;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		reduceTimer.StartTimer(1f);
	}

    public override void FixedUpdate()
    {
        if (stacks <= 0f) Destroy();
        if (reduceTimer.RunTimer())
		{
            stacks *= 0.95f;
			reduceTimer.StartTimer(1f);
		}
    }

    public override float RecoveryMultiplier(bool isMental, float amount)
    {
		if (isMental) return 1f;
        float num = Mathf.Clamp(stacks / amount * 0.2f, 0.2f, 0.8f);
        if (num * amount > stacks)
        {
            num = 1f - stacks / amount;
            stacks = 0f;
        }
		stacks -= amount * num;
        return 1f - num;
    }

	public void AddStacks(float amt)
	{
		stacks += amt;
	}

	public float stacks;

	private Timer reduceTimer = new Timer();
}
