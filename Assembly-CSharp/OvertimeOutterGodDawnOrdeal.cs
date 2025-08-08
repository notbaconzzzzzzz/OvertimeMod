using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeOutterGodDawnOrdeal : OutterGodOrdeal
{
	public OvertimeOutterGodDawnOrdeal()
	{
		this._ordealName = "outtergod_dawn";
		this.level = OrdealLevel.OVERTIME_DAWN;
		base.SetRiskLevel(RiskLevel.TETH);
		base.SetColor();
	}

	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeOutterGods();
	}

	private void MakeOutterGods()
	{
		foreach (Sefira sefira in SefiraManager.instance.GetOpendSefiraList())
		{
			int num = 1;
			if (sefira.name.Contains("Tiphereth"))
			{
				num = 1;
			}
			List<PassageObjectModel> list = new List<PassageObjectModel>();
			foreach (PassageObjectModel passageObjectModel in sefira.passageList)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.type != PassageType.VERTICAL)
					{
						if (passageObjectModel.type != PassageType.SEFIRA)
						{
							if (passageObjectModel.type != PassageType.DEPARTMENT)
							{
								if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
								{
									if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
									{
										list.Add(passageObjectModel);
									}
								}
							}
						}
					}
				}
			}
			if (list.Count != 0)
			{
				for (int j = 0; j < num; j++)
				{
					PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
					List<MapNode> list2 = new List<MapNode>(passageObjectModel2.GetNodeList());
					MapNode node = list2[UnityEngine.Random.Range(0, list2.Count)];
					this.MakeOrdealCreature(this.level, node);
					list.Remove(passageObjectModel2);
					if (list.Count == 0)
					{
						break;
					}
				}
			}
		}
	}

	private const int _dawnCount = 1;
}
