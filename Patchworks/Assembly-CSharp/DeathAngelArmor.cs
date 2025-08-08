/*
public override void OnStageStart() // 
changed some static values
*/
using System;

// Token: 0x0200061A RID: 1562
public class DeathAngelArmor : EquipmentScriptBase
{
	// Token: 0x0600353C RID: 13628 RVA: 0x00030466 File Offset: 0x0002E666
	public DeathAngelArmor()
	{
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x0015E64C File Offset: 0x0015C84C
	public override void OnStageStart()
	{ // <Mod> Keep Ability Mode
		base.OnStageStart();
		if (CreatureManager.instance.FindCreature(100015L) == null && !SpecialModeConfig.instance.GetValue<bool>("PLKeepAbilityWhenWNAbsent"))
		{
			this._whiteNight = false;
		}
		else
		{
			this._whiteNight = true;
		}
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x0015E68C File Offset: 0x0015C88C
	public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		WorkerModel workerModel = base.model.owner as WorkerModel;
		AgentModel agentModel = workerModel as AgentModel;
		if (workerModel != null)
		{
			if (agentModel == null || agentModel.currentSkill == null)
			{
				float num = 1f;
				if (actor != null)
				{
					num = UnitModel.GetDmgMultiplierByEgoLevel(actor.GetAttackLevel(), workerModel.GetDefenseLevel());
				}
				num *= workerModel.GetBufDamageMultiplier(actor, dmg);
				float num2 = dmg.GetDamageWithDefenseInfo(workerModel.defense) * num;
				if (dmg.type == RwbpType.P)
				{
					float num3 = num2 / 100f;
					num2 = (float)workerModel.maxHp * num3;
				}
				if (workerModel.HasEquipment(_DEATH_ANGEL_GIFT))
				{
					if (num2 <= _ABSORB_WITH_GIFT)
					{
						dmg.min = 0f;
						dmg.max = 0f;
						if (dmg.type == RwbpType.W)
						{
							workerModel.RecoverMental(num2);
						}
						else if (dmg.type == RwbpType.B)
						{
							workerModel.RecoverHP(num2);
							workerModel.RecoverMental(num2);
						}
						else
						{
							workerModel.RecoverHP(num2);
						}
					}
				}
				else if (num2 <= _IGNORE_DMG_COND)
				{
					dmg.min = 0f;
					dmg.max = 0f;
				}
			}
		}
		return base.OnTakeDamage(actor, ref dmg);
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x0015E7CC File Offset: 0x0015C9CC
	public override DefenseInfo GetDefense(UnitModel actor)
	{
		if (!this._whiteNight)
		{
			DefenseInfo defenseInfo = base.GetDefense(actor).Copy();
			defenseInfo.R = 0.5f;
			defenseInfo.W = 0.5f;
			defenseInfo.B = 0.5f;
			defenseInfo.P = 0.3f;
			return defenseInfo;
		}
		return base.GetDefense(actor);
	}

	// Token: 0x04003183 RID: 12675
	private const float _IGNORE_DMG_COND = 3f; // <Mod> changed from 5

	// Token: 0x04003184 RID: 12676
	private const float _ABSORB_WITH_GIFT = 6f; // <Mod> changed from 10

	// Token: 0x04003185 RID: 12677
	private const int _DEATH_ANGEL_GIFT = 400015;

	// Token: 0x04003186 RID: 12678
	private bool _whiteNight;
}
