/*
public OfficerModel CreateOfficerModel(string sefira) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BAC RID: 2988
public class OfficerManager
{
	// Token: 0x06005A84 RID: 23172 RVA: 0x000488BA File Offset: 0x00046ABA
	public OfficerManager()
	{
		this.Init();
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06005A85 RID: 23173 RVA: 0x000488D4 File Offset: 0x00046AD4
	public static OfficerManager instance
	{
		get
		{
			if (OfficerManager._instance == null)
			{
				OfficerManager._instance = new OfficerManager();
			}
			return OfficerManager._instance;
		}
	}

	// Token: 0x06005A86 RID: 23174 RVA: 0x000488EF File Offset: 0x00046AEF
	public void Init()
	{
		this.officerList = new List<OfficerModel>();
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x000488FC File Offset: 0x00046AFC
	public void Clear()
	{
		this.ClearOfficer();
		this.Init();
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x00202DF4 File Offset: 0x00200FF4
	public OfficerModel CreateDebugOfficer()
	{
		OfficerModel officerModel = this.CreateOfficerModel("1");
		officerModel.isDebugger = true;
		return officerModel;
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x00202E18 File Offset: 0x00201018
	public OfficerModel CreateDebugOfficer(string nodeId)
	{
		OfficerModel officerModel = this.CreateOfficerModel("1");
		officerModel.GetMovableNode().SetCurrentNode(MapGraph.instance.GetNodeById(nodeId));
		officerModel.isDebugger = true;
		return officerModel;
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x00202E50 File Offset: 0x00201050
	public OfficerModel CreateOfficerModel(string sefira)
	{ // <Mod>
		long id;
		this.nextInstId = (id = this.nextInstId) + 1L;
		OfficerModel officerModel = new OfficerModel(id, sefira);
		officerModel.name = OfficerManager.GetRandomName(sefira);
		officerModel.currentSefira = sefira;
		int num = 0;
		if (ResearchDataModel.instance.IsUpgradedAbility("officer_stat"))
		{
			num += 5;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_defense"))
		{
			num += 10;
		}
		officerModel.baseMaxHp = UnityEngine.Random.Range(10, 16) + num;
		officerModel.hp = (float)officerModel.maxHp;
		officerModel.mental = (float)(officerModel.baseMaxMental = UnityEngine.Random.Range(10, 16) + num);
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.MALKUT))
		{
			officerModel.baseMovement = UnityEngine.Random.Range(5f, 5.5f);
		}
		else
		{
			officerModel.baseMovement = UnityEngine.Random.Range(4f, 4.5f);
		}
		officerModel.GetMovableNode().SetActive(true);
		this.officerList.Add(officerModel);
		SefiraManager.instance.GetSefira(sefira).AddUnit(officerModel);
		WorkerSpriteManager.instance.GetRandomBasicData(officerModel.spriteData, false);
		Notice.instance.Send(NoticeName.AddOfficer, new object[]
		{
			officerModel
		});
		return officerModel;
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x00202F44 File Offset: 0x00201144
	private static string GetRandomName(string sefira)
	{
		int num = UnityEngine.Random.Range(0, OfficerManager.nameList.Length - 1);
		string str = OfficerManager.instance.officerList.Count.ToString();
		return OfficerManager.nameList[num] + str;
	}

	// Token: 0x06005A8C RID: 23180 RVA: 0x00202F8C File Offset: 0x0020118C
	public void ClearOfficer()
	{
		foreach (OfficerModel officerModel in this.officerList)
		{
			Notice.instance.Send(NoticeName.RemoveOfficer, new object[]
			{
				officerModel
			});
			officerModel.GetMovableNode().SetActive(false);
		}
		this.officerList.Clear();
		SefiraManager.instance.ClearOfficer();
	}

	// Token: 0x06005A8D RID: 23181 RVA: 0x0004890A File Offset: 0x00046B0A
	public IList<OfficerModel> GetOfficerList()
	{
		return this.officerList.AsReadOnly();
	}

	// Token: 0x06005A8E RID: 23182 RVA: 0x0020301C File Offset: 0x0020121C
	public OfficerModel[] GetNearOfficers(MovableObjectNode node)
	{
		List<OfficerModel> list = new List<OfficerModel>();
		foreach (OfficerModel officerModel in this.officerList)
		{
			if (!officerModel.IsDead())
			{
				if (node.GetPassage() != null)
				{
					if (MovableObjectNode.GetDistance(node, officerModel.GetMovableNode()) <= 5f)
					{
						list.Add(officerModel);
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005A8F RID: 23183 RVA: 0x002030BC File Offset: 0x002012BC
	public void OnStageEnd()
	{
		foreach (OfficerModel officerModel in this.GetOfficerList())
		{
			officerModel.OnStageEnd();
		}
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x00203114 File Offset: 0x00201314
	public void OnStageRelease()
	{
		foreach (OfficerModel officerModel in this.GetOfficerList())
		{
			officerModel.OnStageRelease();
		}
		this.Clear();
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x00203174 File Offset: 0x00201374
	public void OnFixedUpdate()
	{
		foreach (OfficerModel officerModel in this.officerList)
		{
			try
			{
				officerModel.OnFixedUpdate();
			}
			catch (Exception ex)
			{
			}
		}
	}

	// Token: 0x06005A92 RID: 23186 RVA: 0x002031E8 File Offset: 0x002013E8
	// Note: this type is marked as 'beforefieldinit'.
	static OfficerManager()
	{
	}

	// Token: 0x0400530C RID: 21260
	public static string[] nameList = new string[]
	{
		"Alpha",
		"Beta",
		"Gamma",
		"Delta",
		"Epsilon",
		"Zeta",
		"Eta",
		"Theta",
		"Iota"
	};

	// Token: 0x0400530D RID: 21261
	private static OfficerManager _instance;

	// Token: 0x0400530E RID: 21262
	private long nextInstId = 100000L;

	// Token: 0x0400530F RID: 21263
	private List<OfficerModel> officerList;

	// Token: 0x04005310 RID: 21264
	private static int agentImgRange = 9;

	// Token: 0x04005311 RID: 21265
	public bool isLoadedActionList;
}
