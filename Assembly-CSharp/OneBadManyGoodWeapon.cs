using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000672 RID: 1650
public class OneBadManyGoodWeapon : EquipmentScriptBase
{
	// Token: 0x06003666 RID: 13926 RVA: 0x00030EDF File Offset: 0x0002F0DF
	public OneBadManyGoodWeapon()
	{
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00030F04 File Offset: 0x0002F104
	public override void OnEquip(UnitModel actor)
	{
		this.PrintLog(base.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name);
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00161E10 File Offset: 0x00160010
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		AgentModel agentModel = actor as AgentModel;
		float num = UnityEngine.Random.Range(0f, 1f);
		if (agentModel.fortitudeLevel >= this._CONDITION_FORTITUDE_LEVEL && num < this._PROB_RECOVERY_MENTAL)
		{
			agentModel.RecoverMental(this._AMOUNT_RECOVER_MENTAL);
			this.PrintLog("Recover mental");
		}
		this.PrintLog(base.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name);
		return true;
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00030F2B File Offset: 0x0002F12B
	private void PrintLog(string s)
	{
		if (this._LOG_STATE)
		{
			Debug.LogError(s);
		}
	}

	// Token: 0x04003254 RID: 12884
	private readonly bool _LOG_STATE;

	// Token: 0x04003255 RID: 12885
	private readonly float _AMOUNT_RECOVER_MENTAL = 10f;

	// Token: 0x04003256 RID: 12886
	private readonly int _CONDITION_FORTITUDE_LEVEL = 2;

	// Token: 0x04003257 RID: 12887
	private readonly float _PROB_RECOVERY_MENTAL = 0.05f;
}
