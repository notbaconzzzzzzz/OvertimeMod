using System;

public class OvertimeCircusDawnSlow : UnitBuf
{
	public OvertimeCircusDawnSlow()
	{
		type = UnitBufType.OVERTIME_CIRCUS_DAWN_SLOW;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = 0.5f;
	}

    public override float MovementScale()
    {
        return 0.25f;
    }
}