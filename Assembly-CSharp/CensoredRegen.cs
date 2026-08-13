using System;
using UnityEngine;

public class CensoredRegen : UnitBuf
{
	public CensoredRegen(float _hp, float _mental)
	{
		type = UnitBufType.OVERTIME_OUTTER_GOD_ANTIRECOVERY;
		duplicateType = BufDuplicateType.UNLIMIT;
        hpHealing = _hp / 8f;
        mentalHealing = _mental / 8f;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		remainTime = 1f;
        timer = 0f;
		times = 8;
        newTime = 1f / 8f;
		if (!(model is WorkerModel)) return;
		worker = model as WorkerModel;
	}

    public override void FixedUpdate()
    {
		timer += Time.fixedDeltaTime;
		if (times > 0 && timer >= newTime)
		{
			if (hpHealing != 0f) worker.RecoverHPv2(hpHealing, false);
			if (mentalHealing != 0f) worker.RecoverMentalv2(mentalHealing, false);
			times -= 1;
			newTime += 1f / 8f;
		}
        base.FixedUpdate();
    }

    private WorkerModel worker;
    
    private float timer;
    
    private int times;
    
    private float newTime;

    private float hpHealing;

    private float mentalHealing;
}
