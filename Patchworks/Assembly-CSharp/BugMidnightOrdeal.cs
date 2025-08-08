/*
BugMidnightManager.public void OnDie() // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class BugMidnightOrdeal : BugOrdeal
{
	// Token: 0x06003C4A RID: 15434 RVA: 0x00035482 File Offset: 0x00033682
	public BugMidnightOrdeal()
	{
		this._ordealName = "bug_midnight";
		this.level = OrdealLevel.MIDNIGHT;
		base.SetRiskLevel(RiskLevel.ALEPH);
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x000354AE File Offset: 0x000336AE
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeBug();
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x0017A348 File Offset: 0x00178548
	private void MakeBug()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		List<PassageObjectModel> list2 = new List<PassageObjectModel>();
		MapNode mapNode = null;
		foreach (Sefira sefira in openedAreaList)
		{
			PassageObjectModel passageObjectModel = MapGraph.instance.GetSefiraPassage(sefira.indexString);
			if (sefira.name == "Tiphereth2")
			{
				passageObjectModel = sefira.departPassageList[1];
			}
			else if (sefira.name == "Tiphereth1" || sefira.name == "Tiphereth")
			{
				passageObjectModel = sefira.departPassageList[1];
			}
			foreach (MovableObjectNode movableObjectNode in passageObjectModel.GetEnteredTargets())
			{
				WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
				if (workerModel != null)
				{
					if (!workerModel.IsDead())
					{
						list2.Add(passageObjectModel);
						break;
					}
				}
			}
			list.Add(passageObjectModel);
		}
		int num = 2;
		if (list2.Count < 2)
		{
			list2 = list;
		}
		while (list2.Count > 0 && num > 0)
		{
			PassageObjectModel passageObjectModel2 = list2[UnityEngine.Random.Range(0, list2.Count)];
			list2.Remove(passageObjectModel2);
			if (passageObjectModel2 != null)
			{
				mapNode = passageObjectModel2.centerNode;
			}
			if (mapNode != null)
			{
				this.MakeOrdealCreature(OrdealLevel.MIDNIGHT, mapNode, null, new UnitDirection[0]);
			}
			else
			{
				Debug.LogError("null node");
			}
			num--;
		}
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x0017A518 File Offset: 0x00178718
	public override BugOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node, BugMidnight midnight, params UnitDirection[] direction)
	{
		BugOrdealCreature bugOrdealCreature = base.MakeOrdealCreature(level, node, midnight, direction);
		if (midnight == null)
		{
			if (bugOrdealCreature is BugMidnight)
			{
				BugMidnightOrdeal.BugMidnightManager bugMidnightManager = new BugMidnightOrdeal.BugMidnightManager(bugOrdealCreature as BugMidnight);
				Sefira sefira = SefiraManager.instance.GetSefira(node.GetAttachedPassage().GetSefiraEnum());
				bugMidnightManager.SetSefira(sefira);
				this.managers.Add(bugMidnightManager);
			}
		}
		else
		{
			foreach (BugMidnightOrdeal.BugMidnightManager bugMidnightManager2 in this.managers)
			{
				if (bugMidnightManager2.Script == midnight)
				{
					bugMidnightManager2.AddSpawn(bugOrdealCreature);
					break;
				}
			}
		}
		return bugOrdealCreature;
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x0017A5E0 File Offset: 0x001787E0
	public override void OnDie(OrdealCreatureModel model)
	{
		base.OnDie(model);
		foreach (BugMidnightOrdeal.BugMidnightManager bugMidnightManager in this.managers)
		{
			if (bugMidnightManager.Script.model == model)
			{
				bugMidnightManager.OnDie();
				break;
			}
		}
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x0017A658 File Offset: 0x00178858
	public void OnTeleport(MapNode node, BugMidnight midnight)
	{
		foreach (BugMidnightOrdeal.BugMidnightManager bugMidnightManager in this.managers)
		{
			if (bugMidnightManager.Script == midnight)
			{
				Sefira sefira = SefiraManager.instance.GetSefira(node.GetAttachedPassage().GetSefiraEnum());
				bugMidnightManager.SetSefira(sefira);
				break;
			}
		}
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x0017A6DC File Offset: 0x001788DC
	public MapNode GetTargetNode()
	{
		List<Sefira> list = new List<Sefira>();
		List<PassageObjectModel> list2 = new List<PassageObjectModel>();
		List<PassageObjectModel> list3 = new List<PassageObjectModel>();
		PassageObjectModel passageObjectModel = null;
		MapNode result = null;
		foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
		{
			bool flag = true;
			foreach (BugMidnightOrdeal.BugMidnightManager bugMidnightManager in this.managers)
			{
				if (bugMidnightManager.Sefira == sefira)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(sefira);
			}
		}
		foreach (Sefira sefira2 in list)
		{
			PassageObjectModel passageObjectModel2 = MapGraph.instance.GetSefiraPassage(sefira2.indexString);
			if (sefira2.name == "Tiphereth2")
			{
				passageObjectModel2 = sefira2.departPassageList[1];
			}
			else if (sefira2.name == "Tiphereth1" || sefira2.name == "Tiphereth")
			{
				passageObjectModel2 = sefira2.departPassageList[1];
			}
			foreach (MovableObjectNode movableObjectNode in passageObjectModel2.GetEnteredTargets())
			{
				WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
				if (workerModel != null)
				{
					if (!workerModel.IsDead())
					{
						list3.Add(passageObjectModel2);
						break;
					}
				}
			}
			list2.Add(passageObjectModel2);
		}
		if (list3.Count <= 0)
		{
			list3 = list2;
		}
		if (list3.Count > 0)
		{
			passageObjectModel = list3[UnityEngine.Random.Range(0, list3.Count)];
		}
		if (passageObjectModel != null)
		{
			result = passageObjectModel.centerNode;
		}
		return result;
	}

	// Token: 0x04003730 RID: 14128
	private const int _spawnNum = 2;

	// Token: 0x04003731 RID: 14129
	private List<BugMidnightOrdeal.BugMidnightManager> managers = new List<BugMidnightOrdeal.BugMidnightManager>();

	// Token: 0x0200079D RID: 1949
	public class BugMidnightManager
	{
		// Token: 0x06003C51 RID: 15441 RVA: 0x000354BC File Offset: 0x000336BC
		public BugMidnightManager(BugMidnight script)
		{
			this.script = script;
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x000354D6 File Offset: 0x000336D6
		public BugMidnight Script
		{
			get
			{
				return this.script;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06003C53 RID: 15443 RVA: 0x000354DE File Offset: 0x000336DE
		public Sefira Sefira
		{
			get
			{
				return this.sefira;
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000354E6 File Offset: 0x000336E6
		public void AddSpawn(BugOrdealCreature spawn)
		{
			this.spawns.Add(spawn);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0017A940 File Offset: 0x00178B40
		public void OnDie()
		{ // <Mod>
			/*
			foreach (BugOrdealCreature bugOrdealCreature in this.spawns)
			{
				if (bugOrdealCreature.model.hp <= 0f)
				{
					continue;
				}
				bugOrdealCreature.model.hp = 0f;
				bugOrdealCreature.model.Suppressed();
			}*/
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000354F4 File Offset: 0x000336F4
		public void SetSefira(Sefira sefira)
		{
			this.sefira = sefira;
		}

		// Token: 0x04003732 RID: 14130
		private BugMidnight script;

		// Token: 0x04003733 RID: 14131
		private Sefira sefira;

		// Token: 0x04003734 RID: 14132
		private List<BugOrdealCreature> spawns = new List<BugOrdealCreature>();
	}
}
