using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_OvertimeOverloads : MissionScript
{
    public MissionScript_OvertimeOverloads(Mission _mission)
    {
        mission = _mission;
    }

    public override void Init()
    {
        for (int i = 0; i < Overloads.Length; i++)
        {
            Overloads[i] = 0;
        }
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
		if (notice == NoticeName.OnReleaseWork)
        {
			CreatureModel model = (CreatureModel)param[0];
            UseSkill work = model.currentSkill;
            if (work != null && work._isOverloadedCreature && (bool)param[1])
            {
                int type = (int)work._overloadType - (int)OverloadType.PAIN;
                if (type >= 0 && type < 4)
                {
                    if (Overloads[type] == 0)
                    {
                        mission.successCondition.current++;
                    }
                    Overloads[type]++;
                    CheckSuccess();
                    updateUI = true;
                }
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

    public override int GetLineNum()
    {
        return 2;
    }

    public override string GetLineText(int line)
    {
        string text = "";
        if (line == 0)
        {
            text = base.GetLineText(line);
            if (!mission.isCleared)
            {
                text += " " + mission.successCondition.current.ToString() + "/" + mission.successCondition.goal.ToString();
            }
            return text;
        }
        return "Overtime Meltdowns show up at Meltdown X and beyond    |";
    }

    public int[] Overloads = new int[4];
}