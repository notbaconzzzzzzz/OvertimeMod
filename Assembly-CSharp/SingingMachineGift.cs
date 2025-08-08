using System;

// Token: 0x02000643 RID: 1603
public class SingingMachineGift : EquipmentScriptBase
{
	// Token: 0x060035C2 RID: 13762 RVA: 0x00030B37 File Offset: 0x0002ED37
	public SingingMachineGift()
	{
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x00161634 File Offset: 0x0015F834
	public override bool OnTakeDamage_After(float value, RwbpType type)
	{
		WorkerModel workerModel = base.model.owner as WorkerModel;
		if (workerModel == null)
		{
			return false;
		}
		if (workerModel.IsDead())
		{
			return false;
		}
		if (type == RwbpType.W)
		{
			workerModel.RecoverMental(value * 0.2f);
			workerModel.AddUnitBuf(new SingingMachineGiftBuf());
		}
		return base.OnTakeDamage_After(value, type);
	}
}
