using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript_PanicDrill : MissionScript
{
    public MissionScript_PanicDrill(Mission _mission)
    {
        mission = _mission;
        mission.successCondition.metaInfo.goal = 4;
        RestoredSlots = new int[4];
        TrackerSlots = new int[4];
        SelectedSlots = new int[4];
    }

    public override void Init()
    {
        Trackers.Clear();
        for (int i = 0; i < RestoredSlots.Length; i++)
        {
            RestoredSlots[i] = 0;
            TrackerSlots[i] = 0;
            SelectedSlots[i] = 0;
        }
    }

    public override void CheckConditions(string notice, object[] param)
    {
        bool updateUI = false;
		if (notice == NoticeName.OnAgentPanic)
        {
            AgentModel model = (AgentModel)param[0];
            int num = 0;
			for (int i = 0; i < RestoredSlots.Length; i++)
			{
				if (RestoredSlots[i] >= 1)
				{
					num++;
				}
			}
            if (num <= 0)
            {
				int type = IsAgentValid(model);
				if (type != -1)
				{
					SuppressTracker tracker = new SuppressTracker(model);
					tracker.type = type;
					TrackerSlots[type]++;
					Trackers.Add(tracker);
					UpdateTrackingList();
					updateUI = true;
				}
            }
        }
		else if (notice == NoticeName.OnAgentPanicReturn)
        {
			for (int i = 0; i < Trackers.Count; i++)
			{
				SuppressTracker tracker = Trackers[i];
				if (tracker.agent == (AgentModel)param[0])
				{
                    tracker.panicEnd = true;
					break;
				}
			}
        }
		else if (notice == NoticeName.FixedUpdate)
        {
			for (int i = 0; i < Trackers.Count; i++)
			{
				SuppressTracker tracker = Trackers[i];
				if (tracker.panicEnd)
				{
					if (tracker.agent.unconAction == null)
					{
						RestoredSlots[tracker.type]++;
						TrackerSlots[tracker.type]--;
						Trackers.Remove(tracker);
						UpdateTrackingList();
					}
					else
					{
						TrackerSlots[tracker.type]--;
						Trackers.Remove(tracker);
						UpdateTrackingList();
					}
                    updateUI = true;
				}
			}
        }
		else if (notice == NoticeName.OnAgentDead)
		{
			for (int i = 0; i < Trackers.Count; i++)
			{
				SuppressTracker tracker = Trackers[i];
				if (tracker.agent == (AgentModel)param[0])
				{
					TrackerSlots[tracker.type]--;
					Trackers.Remove(tracker);
					UpdateTrackingList();
                    updateUI = true;
                    break;
				}
			}
		}
		else if (notice == NoticeName.Update)
        {
			int num = 0;
			int[] prevSlots = new int[4];
			for (int i = 0; i < RestoredSlots.Length; i++)
			{
				prevSlots[i] = SelectedSlots[i];
				SelectedSlots[i] = 0;
				if (RestoredSlots[i] >= 1)
				{
					num++;
				}
			}
			if (num > 0) return;
			List<AgentModel> agents = UnitMouseEventManager.instance.GetSelectedAgents();
			num = 0;
			foreach (AgentModel agent in agents)
			{
				int type = IsAgentValid(agent);
				if (type == -1) continue;
				num += (int)Mathf.Pow(2, type);
				SelectedSlots[type]++;
			}
			for (int i = 0; i < SelectedSlots.Length; i++)
			{
				if (SelectedSlots[i] != prevSlots[i])
				{
					updateUI = true;
					break;
				}
			}
			if (num == 15 && Input.GetKeyDown(KeyCode.P) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				List<MapNode> validDestinations = new List<MapNode>();
				foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
				{
					foreach (PassageObjectModel passage in sefira.passageList)
					{
						if (passage.isActivate)
						{
							if (passage.type == PassageType.HORIZONTAL)
							{
								validDestinations.AddRange(passage.GetNodeList());
							}
						}
					}
				}
				if (validDestinations.Count <= 0) return;
				foreach (AgentModel agent in agents)
				{
					if (IsAgentValid(agent) == -1) continue;
					MapNode dest = validDestinations[UnityEngine.Random.Range(0, validDestinations.Count)];
					EffectInvoker.Invoker("Ordeal/Clown_2", agent.GetMovableNode(), 0.5f, false);
					agent.GetMovableNode().SetCurrentNode(dest);
					agent.mental = 0f;
					agent.Panic();
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

	public int IsAgentValid(AgentModel model)
	{
		if (model.level >= 3 && model.fortitudeLevel >= 2 && model.prudenceLevel >= 2 && model.temperanceLevel >= 2 && model.justiceLevel >= 2)
		{
			int type = 0;
			if (model.CurrentPanicAction == null || model.CurrentPanicAction is PanicReady)
			{
				type = (int)model.bestRwbp - 1;
			}
			else if (model.CurrentPanicAction is PanicViolence)
			{
				type = 0;
			}
			else if (model.CurrentPanicAction is PanicSuicideExecutor)
			{
				type = 1;
			}
			else if (model.CurrentPanicAction is PanicRoaming)
			{
				type = 2;
			}
			else if (model.CurrentPanicAction is PanicOpenIsolate)
			{
				type = 3;
			}
			if (model.GetDefenseLevel() >= 3 && model.Equipment.armor.GetDefense(model).W <= 1f && (type != 0 || model.GetAttackLevel() >= 3))
			{
				return type;
			}
		}
		return -1;
	}

    public void UpdateTrackingList()
    {
		bool reset = false;
		int num = 0;
		for (int i = 0; i < RestoredSlots.Length; i++)
		{
			if (RestoredSlots[i] + TrackerSlots[i] < 1)
			{
				reset = true;
				break;
			}
			if (RestoredSlots[i] >= 1)
			{
				num++;
			}
		}
        if (reset)
		{
			for (int i = 0; i < RestoredSlots.Length; i++)
			{
				RestoredSlots[i] = 0;
			}
		}
		else
		{
			mission.successCondition.current = num;
		}
        CheckSuccess();
    }

    public override int GetLineNum()
    {
        return 6;
    }

    public override string GetLineText(int line)
    {
        string text = "";
        if (line == 0)
        {
            if (!mission.isCleared)
            {
                text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
                {
                    mission.sefira_Name,
                    "Name"
                }) + " : ";
                int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < RestoredSlots.Length; i++)
				{
					if (RestoredSlots[i] >= 1)
					{
						num++;
					}
					if (TrackerSlots[i] >= 1)
					{
						num2++;
					}
					if (SelectedSlots[i] >= 1)
					{
						num3++;
						if (SelectedSlots[i] > 1)
						{
							num4 = SelectedSlots[i];
						}
					}
				}
                if (num > 0 || num2 >= 4)
                {
                    text += "Employees' sanity restored " + num.ToString() + "/4";
                }
                else if (num3 <= 0 || num2 > 0)
                {
                    text += "Concurrently panicking employees " + num2.ToString() + "/4";
                }
                else if (num4 > 0)
                {
                    text += "To many valid employees selected (ctrl+P) " + num4.ToString();
                }
                else
                {
                    text += "Valid employees selected (ctrl+P) " + num3.ToString() + "/4";
                }
            }
            else
            {
                text = base.GetLineText(line);
            }
            return text;
        }
        switch (line)
        {
        case 1:
            return "Each employee must have a different of the 4 panic resposes    |";
        case 2:
            return "Must be at least level 3 and have at least level 2 in every stat    |";
        case 3:
            return "Must be wearing at least HE armor with an unmodified W res of 1.0 or better    |";
        case 4:
            return "(The employee with the murder panic response must have an HE weapon or higher)    |";
        case 5:
            return "Press [ctrl]+[P] with 4 valid employees selected to instaintly panic them    |";
        }
        return text;
    }

    public int[] RestoredSlots;

    public int[] TrackerSlots;

    public int[] SelectedSlots;

    private List<SuppressTracker> Trackers = new List<SuppressTracker>();

    public class SuppressTracker
    {
        public SuppressTracker(AgentModel model)
        {
            agent = model;
        }
		
        public AgentModel agent;

        public int type;

        public bool panicEnd;
    }
}