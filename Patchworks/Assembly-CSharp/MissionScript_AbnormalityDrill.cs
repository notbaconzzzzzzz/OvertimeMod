using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_AbnormalityDrill : MissionScript
{
    public MissionScript_AbnormalityDrill(Mission _mission)
    {
        mission = _mission;
        mission.successCondition.metaInfo.goal = suppressLevels.Length;
        SuppressedSlots = new SuppressTracker[suppressLevels.Length];
    }

    public override void Init()
    {
        Trackers.Clear();
        Suppressed.Clear();
        for (int i = 0; i < SuppressedSlots.Length; i++)
        {
            SuppressedSlots[i] = null;
        }
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
		if (notice == NoticeName.OnEscape)
        {
            CreatureModel model = (CreatureModel)param[0];
            if ((int)model.GetRiskLevel() >= suppressLevels[suppressLevels.Length - 1])
            {
                SuppressTracker tracker = new SuppressTracker(model);
                float num = 0;
                for (int i = 0; i < suppressLevels.Length; i++)
                {
                    if (tracker.creatureLevel >= suppressLevels[i])
                    {
                        num = suppressDamageCap[i];
                        break;
                    }
                }
                tracker.maxDamage = num;
                Trackers.Add(tracker);
                updateUI = true;
            }
        }
		else if (notice == NoticeName.OnCreatureSuppressed)
        {
			for (int i = 0; i < Trackers.Count; i++)
			{
				SuppressTracker tracker = Trackers[i];
				if (tracker.creature == (CreatureModel)param[0])
				{
                    tracker.suppressed = true;
					Trackers.Remove(tracker);
                    Suppressed.Add(tracker);
                    UpdateSuppressedList();
                    CheckSuccess();
                    updateUI = true;
					break;
				}
			}
        }
		else if (notice == NoticeName.CreatureHitWorker)
		{
            if (param[3] is AgentModel)
            {
                CreatureModel model = (CreatureModel)param[0];
                if (model is ChildCreatureModel)
                {
                    model = (model as ChildCreatureModel).parent;
                }
                for (int i = 0; i < Trackers.Count; i++)
                {
                    SuppressTracker tracker = Trackers[i];
                    if (tracker.creature == (CreatureModel)param[0])
                    {
                        updateUI = true;
                        if (tracker.TakeDamage((float)param[1]) > tracker.maxDamage)
                        {
                            Trackers.Remove(tracker);
                        }
                        break;
                    }
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

    public void UpdateSuppressedList()
    {
        List<SuppressTracker> remaining = new List<SuppressTracker>(Suppressed);
        int num = 0;
        List<CreatureModel> used = new List<CreatureModel>();
        int minRisk = 0;
        for (int i = 0; i < SuppressedSlots.Length; i++)
        {
            SuppressedSlots[i] = null;
            if (minRisk != suppressLevels[i])
            {
                used.Clear();
            }
            minRisk = suppressLevels[i];
            float maxDamage = suppressDamageCap[i];
            SuppressTracker bestTracker = null;
            float bestDamage = -1f;
            foreach (SuppressTracker tracker in remaining)
            {
                if (tracker.creatureLevel < minRisk || tracker.damageTaken > maxDamage || used.Contains(tracker.creature)) continue;
                if (tracker.damageTaken > bestDamage)
                {
                    bestTracker = tracker;
                    bestDamage = tracker.damageTaken;
                }
            }
            if (bestTracker != null)
            {
                remaining.Remove(bestTracker);
                SuppressedSlots[i] = bestTracker;
                used.Add(bestTracker.creature);
                num++;
            }
        }
        mission.successCondition.current = num;
        remaining = new List<SuppressTracker>(Trackers);
    }

    public override int GetLineNum()
    {
        return 5;
    }

    public override string GetLineText(int line)
    {
        string text = "";
        if (line == 0)
        {
            text = base.GetLineText(line);
            if (!mission.isCleared)
            {
                if (Trackers.Count <= 0)
                {
                    text += "___ 0/___";
                }
                else
                {
                    SuppressTracker tracker = Trackers[0];
                    switch (tracker.creatureLevel)
                    {
                        case 1:
                            text += "ZAYIN";
							break;
                        case 2:
                            text += "TETH";
							break;
                        case 3:
                            text += "HE";
							break;
                        case 4:
                            text += "WAW";
							break;
                        case 5:
                            text += "ALEPH";
							break;
                    }
                    text += " " + ((int)tracker.damageTaken).ToString() + "/" + ((int)tracker.maxDamage).ToString();
                }
            }
            return text;
        }
        int num = 0;
        switch (line)
        {
        case 1:
            if (SuppressedSlots[0] != null) num++;
            return "1 WAW or higher while only taking 200 or less damage " + num.ToString() + "/1    |";
        case 2:
            if (SuppressedSlots[1] != null) num++;
            if (SuppressedSlots[2] != null) num++;
            return "2 different HE or higher while only taking 100 or less damage " + num.ToString() + "/2    |";
        case 3:
            if (SuppressedSlots[3] != null) num++;
            if (SuppressedSlots[4] != null) num++;
            if (SuppressedSlots[5] != null) num++;
            return "3 different TETH or higher while only taking 50 or less damage " + num.ToString() + "/3    |";
        case 4:
            return "'Damage taken' ignores armor and shields    |";
        }
        return text;
    }

    public SuppressTracker[] SuppressedSlots;

    public int[] suppressLevels = new int[]
    {
        4,
        3,
        3,
        2,
        2,
        2
    };

    public float[] suppressDamageCap = new float[]
    {
        200f,
        100f,
        100f,
        50f,
        50f,
        50f
    };

    private List<SuppressTracker> Trackers = new List<SuppressTracker>();

    private List<SuppressTracker> Suppressed = new List<SuppressTracker>();

    public class SuppressTracker
    {
        public SuppressTracker(CreatureModel model)
        {
            creature = model;
            creatureLevel = (int)model.GetRiskLevel();
            damageTaken = 0f;
            suppressed = false;
        }

		public float TakeDamage(float amount)
        {
            damageTaken += amount;
            return damageTaken;
        }
		
        public CreatureModel creature;

        public int creatureLevel;

        public float damageTaken;

        public float maxDamage = 999999f;

        public bool suppressed;
    }
}