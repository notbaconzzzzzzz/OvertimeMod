using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_TimedSuppression : MissionScript
{
    public MissionScript_TimedSuppression(Mission _mission)
    {
        mission = _mission;
		_maxTime = mission.successCondition.metaInfo.minimumSecond;
    }

    public override void Init()
    {
        Trackers.Clear();
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
		if (notice == NoticeName.OnEscape)
        {
			Trackers.Add(new SuppressTracker((CreatureModel)param[0]));
            updateUI = true;
        }
		else if (notice == NoticeName.OnCreatureSuppressed)
        {
			for (int i = 0; i < Trackers.Count; i++)
			{
				SuppressTracker tracker = Trackers[i];
				if (tracker.creature == (CreatureModel)param[0])
				{
					mission.successCondition.current++;
                    CheckSuccess();
					Trackers.Remove(tracker);
                    updateUI = true;
					break;
				}
			}
        }
		else if (notice == NoticeName.FixedUpdate)
		{
			foreach (SuppressTracker tracker in Trackers)
			{
				if (tracker.FixedUpdate())
                {
                    updateUI = true;
                }
				if (tracker.timer >= _maxTime)
				{
					Trackers.Remove(tracker);
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

    public override string GetLineText(int line)
    {
        string text = base.GetLineText(line);
        if (!mission.isCleared)
        {
            text += mission.successCondition.current + "/" + mission.successCondition.goal + " ";
            if (Trackers.Count <= 0)
            {
				text += "_:__";
            }
            else
            {
				int num = _maxTime - (int)Trackers[0].timer;
                text += (num / 60).ToString("D1") + ":" + (num % 60).ToString("D2");
            }
        }
        return text;
    }

	public int _maxTime = 10;

    private List<SuppressTracker> Trackers = new List<SuppressTracker>();

    public class SuppressTracker
    {
        public SuppressTracker(CreatureModel model)
        {
            creature = model;
            timer = 0f;
        }

		public bool FixedUpdate()
		{
            int num = (int)timer;
			timer += Time.deltaTime;
            return num != (int)timer;
		}
		
        public CreatureModel creature;

        public float timer;
    }
}