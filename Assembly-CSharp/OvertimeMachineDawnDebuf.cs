using System;

public class OvertimeMachineDawnDebuf : UnitBuf
{
	public OvertimeMachineDawnDebuf()
	{
		type = UnitBufType.OVERTIME_MACHINE_DAWN_DAMAGE_REDUCE;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		remainTime = 5f;
	}

    public override float GetDamageFactor()
    {
        return 0.5f;
    }
}
