/*
Pretty much everything //
*/
using System;

public class SingingMachineGift : EquipmentScriptBase
{
	public SingingMachineGift()
	{
	}

	public override bool OnTakeDamage_After(float value, RwbpType type)
	{
		WorkerModel workerModel = model.owner as WorkerModel;
		if (workerModel == null)
		{
			return false;
		}
		if (workerModel.IsDead())
		{
			return false;
		}
		if (type != RwbpType.W)
		{
			return false;
		}
		if (_cooldown.started)
		{
			return false;
		}
		if (value < 5f)
		{
			return false;
		}
		workerModel.RecoverMental(value * 0.3f);
		workerModel.AddUnitBuf(new SingingMachineGiftBuf());
        _cooldown.StartTimer(20f);
		return true;
	}

    public override void OnFixedUpdate()
    {
        if (_cooldown.started)
		{
			if (_cooldown.RunTimer())
			{

			}
		}
    }

    private Timer _cooldown = new Timer();
}
