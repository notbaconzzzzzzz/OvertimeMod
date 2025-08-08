/*
private void MakeBugs() // Daat null Hallway Fix
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public class BugDuskOrdeal : BugOrdeal
{
	// Token: 0x06003C1B RID: 15387 RVA: 0x00034F99 File Offset: 0x00033199
	public BugDuskOrdeal()
	{
		this._ordealName = "bug_dusk";
		this.level = OrdealLevel.DUSK;
		base.SetRiskLevel(RiskLevel.WAW);
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x00034FBA File Offset: 0x000331BA
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeBugs();
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00178258 File Offset: 0x00176458
	private void MakeBugs()
	{ // <Mod>
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira[] array = openedAreaList;
		int i = 0;
		while (i < array.Length)
		{
			Sefira sefira = array[i];
			List<PassageObjectModel> list = new List<PassageObjectModel>();
			List<PassageObjectModel> list2 = new List<PassageObjectModel>(sefira.passageList);
			List<MapNode> list3;
			foreach (PassageObjectModel passageObjectModel in list2)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBLONG)
								{
									if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.NONE)
									{
										list.Add(passageObjectModel);
									}
								}
							}
						}
					}
				}
			}
			if (list.Count <= 0)
			{
				continue;
			}
			do
			{
				PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
				list3 = new List<MapNode>(passageObjectModel2.GetNodeList());
				if (list3.Count > 0)
				{
					goto IL_162;
				}
				list.Remove(passageObjectModel2);
			}
			while (list.Count > 0);
			IL_1EB:
			i++;
			continue;
			IL_162:
			UnitDirection unitDirection;
			MapNode attackNode;
			MapNode node;
			if (UnityEngine.Random.value < 0.5f)
			{
				unitDirection = UnitDirection.LEFT;
				attackNode = list3[0];
				node = list3[list3.Count - 1];
			}
			else
			{
				unitDirection = UnitDirection.RIGHT;
				attackNode = list3[list3.Count - 1];
				node = list3[0];
			}
			BugOrdealCreature bugOrdealCreature = this.MakeOrdealCreature(OrdealLevel.DUSK, node, null, new UnitDirection[]
			{
				unitDirection
			});
			(bugOrdealCreature as BugDusk).SetAttackNode(attackNode);
			goto IL_1EB;
		}
	}

	// Token: 0x04003702 RID: 14082
	private const float _sideNodeRemoveRange = 4f;
}
