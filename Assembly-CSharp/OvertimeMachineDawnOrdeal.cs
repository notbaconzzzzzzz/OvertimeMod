using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMachineDawnOrdeal : MachineOrdeal
{
	public OvertimeMachineDawnOrdeal()
	{
		_ordealName = "machine_dawn";
		level = OrdealLevel.OVERTIME_DAWN;
		base.SetRiskLevel(RiskLevel.TETH);
		OrdealColor = _color;
	}

	public override void OnGameInit()
	{
		base.OnGameInit();
	}

	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
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
			if (list.Count != 0)
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
