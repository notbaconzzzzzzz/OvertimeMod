using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_DealDamageWeakest : MissionScript
{
    public MissionScript_DealDamageWeakest(Mission _mission)
    {
        mission = _mission;
    }

    public override void Init()
    {
        aggregateDamage = 0f;
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
        if (notice == NoticeName.CreatureDamagedByAgent)
        {
            CreatureModel model = (CreatureModel)param[0];
            DefenseInfo defense = model.defense;
            RwbpType weakest = RwbpType.N;
            if (defense.R > defense.W && defense.R > defense.B)
            {
                weakest = RwbpType.R;
            }
            if (defense.W > defense.R && defense.W > defense.B)
            {
                weakest = RwbpType.W;
            }
            if (defense.B > defense.R && defense.B > defense.W)
            {
                weakest = RwbpType.B;
            }
            if (((DamageInfo)param[2]).type == weakest)
            {
                float ratio = 1f;
                if (model.GetRiskLevel() <= 2)
                {
                    ratio = 0.25f;
                }
                else if (model.GetRiskLevel() <= 3)
                {
                    ratio = 0.5f;
                }
                aggregateDamage += (float)param[1] * ratio;
                mission.successCondition.current = (int)aggregateDamage;
                CheckSuccess();
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
        return "HEs are only worth x0.5 as much, and TETHs are only worth x0.25.    |";
    }

    public float aggregateDamage;
}