using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_BalanceWorkResults : MissionScript
{
    public MissionScript_BalanceWorkResults(Mission _mission)
    {
        mission = _mission;
    }

    public override void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            skillResults[i] = 0;
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
			int result = (int)creatureModel.feelingState - 1;
			if (result >= 0 && result < 3)
			{
				skillResults[result]++;
                if (skillResults[0] == skillResults[1] && skillResults[0] == skillResults[2])
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
            text += skillResults[0].ToString() + "-" + skillResults[1].ToString() + "-" + skillResults[2].ToString();
        }
        return text;
    }

    private int[] skillResults = new int[4];
}