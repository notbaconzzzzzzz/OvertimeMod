using System;
using WorkerSpine;

// Token: 0x02000694 RID: 1684
public class MachineDawnInnerWeapon : EquipmentScriptBase
{
	// Token: 0x060036F0 RID: 14064 RVA: 0x00030B37 File Offset: 0x0002ED37
	public MachineDawnInnerWeapon()
	{
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x00031C89 File Offset: 0x0002FE89
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		if (target is WorkerModel)
		{
			dmg.specialDeadSceneEnable = true;
			dmg.specialDeadSceneName = WorkerSpine.AnimatorName.MachineDawnAgentDead;
		}
		SoundEffectPlayer.PlayOnce("Ordeal/MachineDawn/Machine_Atk", actor.GetCurrentViewPosition());
		return base.OnGiveDamage(actor, target, ref dmg);
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x0016553C File Offset: 0x0016373C
	public override void OnKillMainTarget(UnitModel actor, UnitModel target)
	{
		base.OnKillMainTarget(actor, target);
		OrdealCreatureModel ordealCreatureModel = actor as OrdealCreatureModel;
		if (ordealCreatureModel == null)
		{
			return;
		}
		actor.OnEndAttackCycle();
		MachineDawn machineDawn = ordealCreatureModel.script as MachineDawn;
		machineDawn.KillMotion(target);
	}
}
