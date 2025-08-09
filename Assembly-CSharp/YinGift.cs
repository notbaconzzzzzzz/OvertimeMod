using System;
using UnityEngine;

// Token: 0x0200064A RID: 1610
public class YinGift : EquipmentScriptBase
{
	// Token: 0x06003622 RID: 13858 RVA: 0x000313FC File Offset: 0x0002F5FC
	public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		return base.OnTakeDamage(actor, ref dmg);
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x001684D8 File Offset: 0x001666D8
    /*
	public override bool OnTakeDamage_After(float value, RwbpType type)
	{
		if (UnityEngine.Random.value < 0.08f)
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
			switch (type)
			{
			case RwbpType.N:
				workerModel.RecoverHP(value);
				break;
			case RwbpType.R:
				workerModel.RecoverHP(value);
				break;
			case RwbpType.W:
				workerModel.RecoverMental(value);
				break;
			case RwbpType.B:
				workerModel.RecoverHP(value);
				workerModel.RecoverMental(value);
				break;
			case RwbpType.P:
				workerModel.RecoverHP(value);
				break;
			}
		}
		return base.OnTakeDamage_After(value, type);
	}*/

    // <Mod>
    public override Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
    {
		if (UnityEngine.Random.value < 0.08f) return new Vector2(100f, 100f);
        return base.PercentageRecoverOnHit(actor, dmg);
    }
}
