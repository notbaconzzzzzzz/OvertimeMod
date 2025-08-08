using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_BalanceWorkTypes : MissionScript
{
    public MissionScript_BalanceWorkTypes(Mission _mission)
    {
        mission = _mission;
    }

    public override void Init()
    {
        for (int i = 0; i < 4; i++)
        {
            skillsUsed[i] = 0;
        }
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
		if (notice == NoticeName.OnReleaseWork)
        {
            CreatureModel creatureModel = param[0] as CreatureModel;
            if (creatureModel == null)
            {
                return;
            }
			int type = (int)creatureModel.currentSkill.skillTypeInfo.id - 1;
			if (type >= 0 && type < 4)
			{
				skillsUsed[type]++;
                if (skillsUsed[0] == skillsUsed[1] && skillsUsed[0] == skillsUsed[2] && skillsUsed[0] == skillsUsed[3])
                {
                    mission.successCondition.current = 1;
                }
                else
                {
                    mission.successCondition.current = 0;
                }
				updateUI = true;
			}
        }
		else if (notice == NoticeName.OnStageEnd)
		{
			if (CheckSuccess())
			{
				updateUI = true;
			}
		}
        if (updateUI)
        {
            Notice.instance.Send(NoticeName.OnMissionProgressed, new object[]
            {
                mission
            });
        }
    }

    public override string GetLineText(int line)
    {
        string text = base.GetLineText(line);
        if (!mission.isCleared)
        {
            text += skillsUsed[0].ToString() + "-" + skillsUsed[1].ToString() + "-" + skillsUsed[2].ToString() + "-" + skillsUsed[3].ToString();
        }
        return text;
    }

    private int[] skillsUsed = new int[4];
}