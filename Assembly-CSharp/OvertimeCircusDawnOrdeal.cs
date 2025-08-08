using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeCircusDawnOrdeal : CircusOrdeal
{
	public OvertimeCircusDawnOrdeal()
	{
		_ordealName = "circus_dawn";
		level = OrdealLevel.OVERTIME_DAWN;
		base.SetRiskLevel(RiskLevel.TETH);
	}

	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		initialSpawn = true;
		initialTimer.StartTimer(4f);
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		foreach (Sefira sefira in openedAreaList)
		{
			MapNode randomWayPoint = sefira.GetRandomWayPoint();
			OvertimeCircusDawn creature = base.MakeOrdealCreature(OrdealLevel.OVERTIME_DAWN, randomWayPoint) as OvertimeCircusDawn;
            creature.meltdownType = UnityEngine.Random.Range(0, 4);
		}
	}

    public override void FixedUpdate()
    {
        base.FixedUpdate();
		if (initialTimer.started && initialTimer.RunTimer())
		{
			initialSpawn = false;
		}
    }

    public override bool IsStartable()
	{
		return !isStarted;
	}

	public CreatureModel GetTarget(out bool hasError)
	{
		List<CreatureModel> list = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
		List<CreatureModel> list2 = new List<CreatureModel>();
		hasError = false;
		if (list.Count > 1 && _currentWaitingCreature != null && list.Contains(_currentWaitingCreature))
		{
			list.Remove(_currentWaitingCreature);
		}
		foreach (CreatureModel creatureModel in list)
		{
			if (!creatureModel.IsEscaped() && creatureModel.script.HasRoomCounter() && creatureModel.qliphothCounter > 0)
			{
				list2.Add(creatureModel);
			}
		}
		if (list2.Count == 0)
		{
			foreach (CreatureModel creatureModel2 in list)
			{
				if (!creatureModel2.IsEscaped())
				{
					list2.Add(creatureModel2);
				}
			}
		}
		if (list2.Count == 0)
		{
			hasError = true;
			list2 = new List<CreatureModel>(list);
		}
		try
		{
			return list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		catch (IndexOutOfRangeException ex)
		{
		}
		return null;
	}

	public bool ReadyForTeleport()
	{
		bool result = false;
		CreatureModel target = GetTarget(out result);
		if (target != null)
		{
			_currentWaitingCreature = target;
		}
		return result;
	}

	public MapNode GetTeleportNode()
	{
		MapNode result = null;
		if (_currentWaitingCreature != null)
		{
			MapNode entryNode = _currentWaitingCreature.GetEntryNode();
			PassageObjectModel attachedPassage = entryNode.GetAttachedPassage();
			if (attachedPassage != null)
			{
				MapNode[] nodeList = attachedPassage.GetNodeList();
				result = nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
			}
		}
		return result;
	}

	private CreatureModel _currentWaitingCreature;

	public bool initialSpawn;

	private Timer initialTimer = new Timer();
}
