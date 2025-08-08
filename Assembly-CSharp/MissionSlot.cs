/*
+public void Init(Mission mission, int line) // 
public void Refresh() // 
+public int multiLine // 
+public bool hasTimerCondition // 
*/
using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A47 RID: 2631
public class MissionSlot : MonoBehaviour
{
	// Token: 0x06004F6C RID: 20332 RVA: 0x000044BC File Offset: 0x000026BC
	public MissionSlot()
	{
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x000416BD File Offset: 0x0003F8BD
	public void Init(Mission mission)
	{
		this.mission = mission;
		this.Refresh();
		if (mission.successCondition.condition_Type == ConditionType.CLEAR_TIME)
		{
			mission.successCondition.current = 0;
			hasTimerCondition = true;
		}
        for (int i = 0; i < mission.failConditions.Count; i++)
        {
			if (mission.failConditions[i].condition_Type == ConditionType.COMPLETION_TIME)
			{
				mission.failConditions[i].current = 0;
				hasTimerCondition = true;
			}
        }
	}

	public void Init(Mission mission, int line)
	{
        multiLine = line;
		if (multiLine > 0)
		{
			txt.fontSize = txt.fontSize * 5 / 6;
		}
		Init(mission);
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001D0008 File Offset: 0x001CE208
	public void Refresh()
	{ // <Mod>
		if (this.mission == null || this.mission.successCondition == null)
		{
			Debug.Log("Invalid mission inited");
			return;
		}
        if (mission.missionScript != null)
        {
            txt.text = mission.missionScript.GetLineText(multiLine);
            if (this.AutoResize)
            {
                this.AutoResizing();
            }
            return;
        }
		this.txt.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			this.mission.sefira_Name,
			"Name"
		}) + " : ";
		Condition timeCondition = null;
		if (this.mission.successCondition.condition_Type == ConditionType.CLEAR_TIME)
		{
			timeCondition = mission.successCondition;
			timeCondition.current = (int)GlobalHistory.instance.GetCurrentTime();
		}
		bool flag = false;
        for (int i = 0; i < mission.failConditions.Count; i++)
        {
            if (!mission.doneConditions[mission.failConditions[i].index])
            {
                flag = true;
                break;
            }
			if (mission.failConditions[i].condition_Type == ConditionType.COMPLETION_TIME)
			{
				timeCondition = mission.failConditions[i];
				timeCondition.current = (int)GlobalHistory.instance.GetCurrentTime();
			}
        }
		if (this.mission.isCleared)
		{
			Text text = this.txt;
			string text2 = text.text;
			text.text = string.Concat(new string[]
			{
				text2,
				"'",
				LocalizeTextDataModel.instance.GetText(this.mission.metaInfo.title),
				"' ",
				LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					"MissionUI",
					"Clear"
				})
			});
		}
		else if (flag || (this.mission.successCondition.goal_Type == GoalType.MAX && this.mission.successCondition.goal < this.mission.successCondition.current))
		{
			Text text3 = this.txt;
			string text2 = text3.text;
			text3.text = string.Concat(new string[]
			{
				text2,
				"'",
				LocalizeTextDataModel.instance.GetText(this.mission.metaInfo.title),
				"' ",
				LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					"MissionUI",
					"Fail"
				})
			});
		}
		else
		{
			string text4 = LocalizeTextDataModel.instance.GetText(this.mission.metaInfo.shortDesc) + " ";
			if (timeCondition != null)
			{
				int num = timeCondition.goal - timeCondition.current;
				if (num < 0)
				{
					text4 = "'" + LocalizeTextDataModel.instance.GetText(this.mission.metaInfo.title) + "' " + LocalizeTextDataModel.instance.GetTextAppend(new string[]
					{
						"MissionUI",
						"Fail"
					});
				}
				else
				{
					text4 = text4 + (num / 60).ToString("D2") + ":" + (num % 60).ToString("D2");
				}
			}
			else
			{
				if (mission.successCondition.condition_Type == ConditionType.CLEAR_DAY)
				{
					int cur = 0;
					int gol = 0;
					float perc = -1f;
					for (int i = 0; i < mission.failConditions.Count; i++)
					{
						Condition failCond = mission.failConditions[i];
						int _cur = failCond.current;
						int _gol = failCond.goal;
						float _perc = -1f;
						if (failCond.goal_Type == GoalType.MAX)
						{
							_perc = (float)_cur / (float)_gol;
						}
						else if (failCond.goal_Type == GoalType.MIN)
						{
							_perc = 1f - (float)_cur / (float)_gol;
						}
						if (_perc > perc)
						{
							cur = _cur;
							gol = _gol;
							perc = _perc;
						}
					}
					string text2 = text4;
					text4 = string.Concat(new object[]
					{
						text2,
						cur,
						"/",
						gol
					});
				}
				else
				{
					string text2 = text4;
					text4 = string.Concat(new object[]
					{
						text2,
						this.mission.successCondition.current,
						"/",
						this.mission.successCondition.goal
					});
				}
			}
			Text text5 = this.txt;
			text5.text += text4;
		}
		if (this.AutoResize)
		{
			this.AutoResizing();
		}
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001D0338 File Offset: 0x001CE538
	private void AutoResizing()
	{
		float preferredWidth = this.txt.preferredWidth;
		RectTransform component = this.txt.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(preferredWidth + 5f, component.sizeDelta.y);
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06004F70 RID: 20336 RVA: 0x0003C6C5 File Offset: 0x0003A8C5
	public RectTransform rect
	{
		get
		{
			return base.gameObject.GetComponent<RectTransform>();
		}
	}

	// Token: 0x0400498F RID: 18831
	private const string textSrc = "MissionUI";

	// Token: 0x04004990 RID: 18832
	public Text txt;

	// Token: 0x04004991 RID: 18833
	public Mission mission;

	// Token: 0x04004992 RID: 18834
	public bool AutoResize;

    // <Mod>
    public int multiLine;

	// <Mod>
	public bool hasTimerCondition;
}
