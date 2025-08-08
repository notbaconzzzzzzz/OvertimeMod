using System;

// Token: 0x02000640 RID: 1600
public class OneBadManyGoodGift : EquipmentScriptBase // changed from internal
{
	// Token: 0x060035DA RID: 13786 RVA: 0x00030BAD File Offset: 0x0002EDAD
	public OneBadManyGoodGift()
	{
	}

    // <Mod>
	public override float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		AgentModel agent = actor as AgentModel;
		if (agent.currentSkill != null)
		{
			if (agent.currentSkill.targetCreature.metaInfo.LcId == 100009L)
			{
				return 10f;
			}
		}
		else
		{
			if (CommandWindow.CommandWindow.CurrentWindow.CurrentWindowType == CommandType.Management && ((CreatureModel)CommandWindow.CommandWindow.CurrentWindow.CurrentTarget).metaInfo.LcId == 100009L)
			{
				return 10f;
			}
		}
		return base.GetWorkProbSpecialBonus(actor, skill);
	}

    /*
	// Token: 0x060035DB RID: 13787 RVA: 0x0016316C File Offset: 0x0016136C
	public override EGObonusInfo GetBonus(UnitModel actor)
	{
		AgentModel agentModel = actor as AgentModel;
		EGObonusInfo egobonusInfo = new EGObonusInfo();
		if (agentModel.currentSkill != null && agentModel.currentSkill.targetCreature.metadataId == 100009L)
		{
			egobonusInfo.workProb = 10;
		}
		return egobonusInfo + base.GetBonus(actor);
	}*/
}
