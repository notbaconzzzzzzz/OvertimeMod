using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class BugDawnOrdeal : BugOrdeal
{
	// Token: 0x06003C0F RID: 15375 RVA: 0x00034F08 File Offset: 0x00033108
	public BugDawnOrdeal()
	{
		this._ordealName = "bug_dawn";
		this.level = OrdealLevel.DAWN;
		base.SetRiskLevel(RiskLevel.TETH);
		this.managers = new List<BugDawnOrdeal.BugDawnManager>();
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00034F3F File Offset: 0x0003313F
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeBugs();
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x00177BD4 File Offset: 0x00175DD4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		List<BugDawnOrdeal.BugDawnManager> list = new List<BugDawnOrdeal.BugDawnManager>();
		foreach (BugDawnOrdeal.BugDawnManager bugDawnManager in this.managers)
		{
			if (bugDawnManager.IsAvailable())
			{
				bugDawnManager.Run();
			}
			else
			{
				list.Add(bugDawnManager);
			}
		}
		foreach (BugDawnOrdeal.BugDawnManager item in list)
		{
			this.managers.Remove(item);
		}
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00177CA0 File Offset: 0x00175EA0
	private PassageObjectModel GetPassage()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		for (int i = 0; i < openedAreaList.Length; i++)
		{
			foreach (PassageObjectModel passageObjectModel in openedAreaList[i].passageList)
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
										if (passageObjectModel != this._curOrdealCreatureList[0].GetMovableNode().currentPassage)
										{
											list.Add(passageObjectModel);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x00177DF4 File Offset: 0x00175FF4
	private void MakeBugs()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira[] array = openedAreaList;
		int i = 0;
		while (i < array.Length)
		{
			Sefira sefira = array[i];
			BugDawnOrdeal.BugDawnManager bugDawnManager = new BugDawnOrdeal.BugDawnManager(this);
			List<PassageObjectModel> list = new List<PassageObjectModel>();
			List<PassageObjectModel> list2 = new List<PassageObjectModel>(sefira.passageList);
			List<MapNode> list4 = new List<MapNode>();
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
			do
			{
				PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
				List<MapNode> list3 = passageObjectModel2.GetNodeList().ToList<MapNode>();
				list4.Clear();
				float num = 0f;
				float num2 = 0f;
				float scaleFactor = passageObjectModel2.scaleFactor;
				passageObjectModel2.GetVerticalRange(ref num, ref num2);
				foreach (MapNode mapNode in list3)
				{
					if (mapNode.GetPosition().x - num >= 3f * scaleFactor)
					{
						if (num2 - mapNode.GetPosition().x >= 3f * scaleFactor)
						{
							list4.Add(mapNode);
						}
					}
				}
				if (list4.Count > 0)
				{
					goto IL_1F8;
				}
				list.Remove(passageObjectModel2);
			}
			while (list.Count > 0);
			IL_256:
			this.managers.Add(bugDawnManager);
			i++;
			continue;
			IL_1F8:
			for (int j = 0; j < 5; j++)
			{
				MapNode node = list4[UnityEngine.Random.Range(0, list4.Count)];
				BugOrdealCreature bugOrdealCreature = this.MakeOrdealCreature(OrdealLevel.DAWN, node, null, new UnitDirection[0]);
				bugDawnManager.AddCreature(bugOrdealCreature as BugDawn);
			}
			goto IL_256;
		}
	}

	// Token: 0x040036F9 RID: 14073
	private const int _numOfBugs = 5;

	// Token: 0x040036FA RID: 14074
	private const float _sideNodeRemoveRange = 3f;

	// Token: 0x040036FB RID: 14075
	private List<BugDawnOrdeal.BugDawnManager> managers = new List<BugDawnOrdeal.BugDawnManager>();

	// Token: 0x02000794 RID: 1940
	public class BugDawnManager
	{
		// Token: 0x06003C14 RID: 15380 RVA: 0x00178090 File Offset: 0x00176290
		public BugDawnManager(BugDawnOrdeal script)
		{
			this.script = script;
			this.isAvailable = true;
			this.teleportTimer.StartTimer(BugDawnOrdeal.BugDawnManager.teleportTime);
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x000256A6 File Offset: 0x000238A6
		private static float teleportTime
		{
			get
			{
				return UnityEngine.Random.Range(40f, 60f);
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00034F4D File Offset: 0x0003314D
		public void AddCreature(BugDawn bug)
		{
			this.bugs.Add(bug);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x00034F5B File Offset: 0x0003315B
		public void Run()
		{
			if (!this.CheckAvailable())
			{
				this.teleportTimer.StopTimer();
				this.isAvailable = false;
				return;
			}
			if (this.teleportTimer.RunTimer())
			{
				this.ReadyToTeleport();
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x001780E0 File Offset: 0x001762E0
		private bool CheckAvailable()
		{
			List<BugDawn> list = new List<BugDawn>();
			if (this.bugs.Count <= 0)
			{
				return true;
			}
			foreach (BugDawn bugDawn in this.bugs)
			{
				if (bugDawn.model.hp <= 0f)
				{
					list.Add(bugDawn);
				}
			}
			foreach (BugDawn item in list)
			{
				this.bugs.Remove(item);
			}
			return this.bugs.Count > 0;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x001781CC File Offset: 0x001763CC
		private void ReadyToTeleport()
		{
			PassageObjectModel passage = this.script.GetPassage();
			if (passage == null)
			{
				Debug.Log("nowhere to go");
				return;
			}
			foreach (BugDawn bugDawn in this.bugs)
			{
				bugDawn.ReadyToTeleport(passage);
			}
			this.teleportTimer.StartTimer(BugDawnOrdeal.BugDawnManager.teleportTime);
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x00034F91 File Offset: 0x00033191
		public bool IsAvailable()
		{
			return this.isAvailable;
		}

		// Token: 0x040036FC RID: 14076
		private BugDawnOrdeal script;

		// Token: 0x040036FD RID: 14077
		private bool isAvailable = true;

		// Token: 0x040036FE RID: 14078
		private Timer teleportTimer = new Timer();

		// Token: 0x040036FF RID: 14079
		private const float _teleportTimeMin = 40f;

		// Token: 0x04003700 RID: 14080
		private const float _teleportTimeMax = 60f;

		// Token: 0x04003701 RID: 14081
		private List<BugDawn> bugs = new List<BugDawn>();
	}
}
