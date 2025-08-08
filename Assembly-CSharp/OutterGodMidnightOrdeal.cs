/*
private void MakeMidnight() // Generate even if there aren't enough departments
private OutterGodOrdealCreature MakeTombStones(RwbpType type, MapNode node) // 
private static int[] ids // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public class OutterGodMidnightOrdeal : OutterGodOrdeal
{
	// Token: 0x06003CD7 RID: 15575 RVA: 0x0003579E File Offset: 0x0003399E
	public OutterGodMidnightOrdeal()
	{
		this._ordealName = "outtergod_midnight";
		this.level = OrdealLevel.MIDNIGHT;
		base.SetRiskLevel(RiskLevel.ALEPH);
		base.SetColor();
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x000357C5 File Offset: 0x000339C5
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeMidnight();
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x0017B75C File Offset: 0x0017995C
	private void MakeMidnight()
	{ // <Mod>
		List<Sefira> list = new List<Sefira>(SefiraManager.instance.GetOpendSefiraList());
		List<MapNode> list2 = new List<MapNode>();
		for (int i = 0; i < 4; i++)
		{
			Sefira sefira = this.GetSefira(ref list);
			if (sefira == null)
			{
				break;
			}
			List<PassageObjectModel> passages = this.GetPassages(sefira);
			if (passages.Count <= 0)
			{
				i--;
			}
			else
			{
				PassageObjectModel passage = passages[UnityEngine.Random.Range(0, passages.Count)];
				MapNode node = this.GetNode(passage);
				if (node == null)
				{
					i--;
				}
				else
				{
					list2.Add(node);
				}
			}
		}
		if (list2.Count < 1)
		{
			return;
		}
		this.MakeTombStones(RwbpType.R, list2[0]);
		if (list2.Count < 2)
		{
			return;
		}
		this.MakeTombStones(RwbpType.W, list2[1]);
		if (list2.Count < 3)
		{
			return;
		}
		this.MakeTombStones(RwbpType.B, list2[2]);
		if (list2.Count < 4)
		{
            return;
		}
		this.MakeTombStones(RwbpType.P, list2[3]);
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x001135DC File Offset: 0x001117DC
	private Sefira GetSefira(ref List<Sefira> remain)
	{
		if (remain.Count <= 0)
		{
			return null;
		}
		Sefira sefira = remain[UnityEngine.Random.Range(0, remain.Count)];
		remain.Remove(sefira);
		return sefira;
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x00113618 File Offset: 0x00111818
	private List<PassageObjectModel> GetPassages(Sefira sefira)
	{
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		List<PassageObjectModel> list2 = new List<PassageObjectModel>(sefira.passageList);
		foreach (PassageObjectModel passageObjectModel in list2)
		{
			if (passageObjectModel.isActivate)
			{
				if (passageObjectModel.type == PassageType.HORIZONTAL)
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
		return list;
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x001136DC File Offset: 0x001118DC
	private MapNode GetNode(PassageObjectModel passage)
	{
		MapNode result = null;
		if (passage != null)
		{
			result = passage.centerNode;
		}
		return result;
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x0017B854 File Offset: 0x00179A54
	private OutterGodOrdealCreature MakeTombStones(RwbpType type, MapNode node)
	{ // <Mod>
        int num = (int)type - 1;
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)OutterGodMidnightOrdeal.ids[num], node, this);
		(ordealCreatureModel.script as OutterGodOrdealCreature).SetOrdeal(this, this.level);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		return ordealCreatureModel.script as OutterGodOrdealCreature;
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x000357D3 File Offset: 0x000339D3
	// Note: this type is marked as 'beforefieldinit'.
	static OutterGodMidnightOrdeal()
	{
	}

	// Token: 0x0400377D RID: 14205
	private static int[] ids = new int[]
	{ // <Mod>
		2000121,
		2000122,
		2000123,
		2000124,
		2000121,
		2000122,
		2000123,
		2000124
	};
}
