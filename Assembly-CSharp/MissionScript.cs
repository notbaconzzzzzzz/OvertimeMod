using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript
{
    public virtual void Init()
    {

    }

    public virtual void CheckConditions(string notice, object[] param)
    {
        
    }

    public virtual bool CheckSuccess()
    {
        bool flag = CheckDefault(mission.successCondition);
		if (flag)
		{
			mission.isCleared = true;
			mission.OnDisabled();
			return true;
		}
		return false;
    }

    public virtual bool CheckDefault(Condition condition)
    {
        GoalType goal_Type = condition.goal_Type;
		if (goal_Type == GoalType.MAX)
		{
			return condition.goal >= condition.current;
		}
		if (goal_Type != GoalType.MIN)
		{
			return goal_Type == GoalType.SAME && condition.goal == condition.current;
		}
		return condition.goal <= condition.current;
    }

    public virtual int GetLineNum()
    {
        return 1;
    }

    public virtual string GetLineText(int line)
    {
        string text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			mission.sefira_Name,
			"Name"
		}) + " : ";
        
        if (mission.isCleared)
        {
            text += "'" + 
                LocalizeTextDataModel.instance.GetText(mission.metaInfo.title) +
                "' " + 
                LocalizeTextDataModel.instance.GetTextAppend(new string[] { "MissionUI", "Clear" });
        }
        else
        {
		    text += LocalizeTextDataModel.instance.GetText(mission.metaInfo.shortDesc) + " ";
        }
        return text;
    }

    public Mission mission;
}