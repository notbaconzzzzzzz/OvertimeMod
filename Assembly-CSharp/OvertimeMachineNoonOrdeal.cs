using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMachineNoonOrdeal : MachineOrdeal
{
	public OvertimeMachineNoonOrdeal()
	{
		_ordealName = "machine_noon";
		level = OrdealLevel.OVERTIME_NOON;
		base.SetRiskLevel(RiskLevel.HE);
		OrdealColor = _color;
	}

	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		MakeMachineNoon();
	}

	private void MakeMachineNoon()
	{
		foreach (Sefira sefira in SefiraManager.instance.GetOpendSefiraList())
		{
			List<PassageObjectModel> list = new List<PassageObjectModel>();
			foreach (PassageObjectModel passageObjectModel in sefira.passageList)
			{
				if (passageObjectModel.isActivate && (passageObjectModel.type == PassageType.DEPARTMENT || passageObjectModel.type == PassageType.HORIZONTAL))
				{
					list.Add(passageObjectModel);
				}
			}
			if (list.Count == 0)
			{
				Debug.Log("no available passage");
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
                    PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
                    MapNode[] nodeList = passageObjectModel2.GetNodeList();
                    MapNode node = nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
                    MakeOrdealCreature(level, node);
                }
			}
		}
	}
}
