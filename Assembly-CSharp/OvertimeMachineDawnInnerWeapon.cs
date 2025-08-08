using System;
using WorkerSpine;

public class OvertimeMachineDawnInnerWeapon : EquipmentScriptBase
{
	public OvertimeMachineDawnInnerWeapon()
	{
	}

	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		if (target is WorkerModel)
		{
			dmg.specialDeadSceneEnable = true;
			dmg.specialDeadSceneName = WorkerSpine.AnimatorName.MachineDawnAgentDead;
		}
		SoundEffectPlayer.PlayOnce("Ordeal/MachineDawn/Machine_Atk", actor.GetCurrentViewPosition());
		target.AddUnitBuf(new OvertimeMachineDawnDebuf());
		return base.OnGiveDamage(actor, target, ref dmg);
	}

    public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
    {
        base.OnGiveDamageAfter(actor, target, dmg);
		OrdealCreatureModel ordealCreatureModel = actor as OrdealCreatureModel;
		if (ordealCreatureModel == null)
		{
			return;
		}
		OvertimeMachineDawn machineDawn = ordealCreatureModel.script as OvertimeMachineDawn;
		if (target is WorkerModel)
		{
			WorkerModel model = target as WorkerModel;
			bool isClerk = model is OfficerModel;
			if (machineDawn.cooldownTimer.started && (isClerk || !machineDawn.isClerkCooldown)) return;
			if (model.unconAction != null || model.IsDead()) return;
			if (!machineDawn.TryUseAbility()) return;
			if (isClerk)
			{
				machineDawn.isClerkCooldown = true;
			}
			else
			{
				machineDawn.isClerkCooldown = false;
			}
			machineDawn.cooldownTimer.StartTimer(25f);
			try
			{
				model.SetUncontrollableAction(new Uncontrollable_OvertimeMachineDawn(model));
			}
			catch (Exception ex)
			{
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					"Error : " + ex.Message + " : " + ex.StackTrace
				});
			}
			actor.OnEndAttackCycle();
			machineDawn.KillMotion(target);
		}
    }

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

    public override void OnFixedUpdate()
    {
		OrdealCreatureModel ordealCreatureModel = model.owner as OrdealCreatureModel;
		if (ordealCreatureModel == null)
		{
			return;
		}
		OvertimeMachineDawn machineDawn = ordealCreatureModel.script as OvertimeMachineDawn;
		if (machineDawn.cooldownTimer.started)
		{
			machineDawn.cooldownTimer.RunTimer();
		}
    }
}
