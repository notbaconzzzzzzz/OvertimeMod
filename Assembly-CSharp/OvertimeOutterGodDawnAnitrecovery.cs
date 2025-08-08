using System;

public class OvertimeOutterGodDawnAnitrecovery : UnitBuf
{
	public OvertimeOutterGodDawnAnitrecovery()
	{
		type = UnitBufType.OVERTIME_OUTTER_GOD_ANTIRECOVERY;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		remainTime = 10f;
		if (!(model is WorkerModel)) return;
		worker = model as WorkerModel;
		worker.blockRecover = true;
	}

    public override void OnDestroy()
    {
		if (worker == null) return;
        worker.blockRecover = false;
    }

	private WorkerModel worker;
}
