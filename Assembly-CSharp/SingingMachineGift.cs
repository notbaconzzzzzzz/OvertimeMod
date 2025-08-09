/*
Pretty much everything //
*/
using System;
using UnityEngine; // 

public class SingingMachineGift : EquipmentScriptBase
{
	public SingingMachineGift()
	{
	}

	/*
	public override bool OnTakeDamage_After(float value, RwbpType type)
	{
		WorkerModel workerModel = model.owner as WorkerModel;
		if (workerModel == null) return false;
		if (workerModel.IsDead()) return false;
		if (type != RwbpType.W) return false;
		if (_cooldown.started) return false;
		if (value < 6f) return false;
		workerModel.RecoverMental(value / 3f);
		workerModel.AddUnitBuf(new SingingMachineGiftBuf());
        _cooldown.StartTimer(20f);
		return true;
	}*/

    // <Mod>
    public override Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
    {
		WorkerModel workerModel = model.owner as WorkerModel;
		if (workerModel == null) base.PercentageRecoverOnHit(actor, dmg);
		if (workerModel.IsDead()) base.PercentageRecoverOnHit(actor, dmg);
		if (dmg.type != RwbpType.W) base.PercentageRecoverOnHit(actor, dmg);
		if (_cooldown.started) base.PercentageRecoverOnHit(actor, dmg);
		if (dmg.result.resultDamage < 6f) base.PercentageRecoverOnHit(actor, dmg);
		workerModel.AddUnitBuf(new SingingMachineGiftBuf());
        _cooldown.StartTimer(20f);
        return new Vector2(100f / 3f, 100f / 3f);
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
