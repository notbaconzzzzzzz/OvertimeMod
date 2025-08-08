/*
+public bool isRegenerator // 
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
public class PassageObjectModel : ObjectModelBase, IMouseCommandTargetModel
{
	// Token: 0x06003833 RID: 14387 RVA: 0x0016BDE4 File Offset: 0x00169FE4
	public PassageObjectModel(string id, string sefiraName, string prefabSrc)
	{ // <Mod>
		this.id = id;
		this.sefiraName = sefiraName;
		this.sefiraEnum = SefiraName.GetSefiraEnum(sefiraName);
		this.src = prefabSrc;
		this.mapNodeList = new List<MapNode>();
		this.doorObjectList = new List<DoorObjectModel>();
		this.elevatorList = new List<ElevatorPassageModel>();
		this.enteredUnitList = new List<MovableObjectNode>();
		this.deletedUnitList = new List<MovableObjectNode>();
		etcUnitList = new List<MovableObjectNode>();
		this.connectedSefira = new List<string>();
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0016BE8C File Offset: 0x0016A08C
	public PassageObjectModel(string id, string sefiraName)
	{ // <Mod>
		this.id = id;
		this.sefiraName = sefiraName;
		this.src = string.Empty;
		this.mapNodeList = new List<MapNode>();
		this.doorObjectList = new List<DoorObjectModel>();
		this.elevatorList = new List<ElevatorPassageModel>();
		this.enteredUnitList = new List<MovableObjectNode>();
		this.deletedUnitList = new List<MovableObjectNode>();
		etcUnitList = new List<MovableObjectNode>();
		this.connectedSefira = new List<string>();
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x0016BF2C File Offset: 0x0016A12C
	public static PassageType GetPassageTypeByString(string str)
	{
		if (str != null)
		{
			if (str == "sefira")
			{
				return PassageType.SEFIRA;
			}
			if (str == "dept")
			{
				return PassageType.DEPARTMENT;
			}
			if (str == "vertical")
			{
				return PassageType.VERTICAL;
			}
			if (str == "horizontal")
			{
				return PassageType.HORIZONTAL;
			}
			if (str == "isolateroom")
			{
				return PassageType.ISOLATEROOM;
			}
		}
		return PassageType.NONE;
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x000328EC File Offset: 0x00030AEC
	public void SetPassageType(PassageType type)
	{
		this.type = type;
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x000328F5 File Offset: 0x00030AF5
	public PassageType GetPassageType()
	{
		return this.type;
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x000328FD File Offset: 0x00030AFD
	public void AddAttachedSefira(string sefira)
	{
		this.connectedSefira.Add(sefira);
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x0016BFA0 File Offset: 0x0016A1A0
	public void AttachToConnetedSefira()
	{
		if (this.connectedSefira.Count < 1)
		{
			return;
		}
		foreach (string str in this.connectedSefira)
		{
			SefiraManager.instance.GetSefira(str).connectedPassageList.Add(this);
		}
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x0003290B File Offset: 0x00030B0B
	public void SetToIsolate()
	{
		this.isIsolate = true;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x00032914 File Offset: 0x00030B14
	public bool IsIsolate()
	{
		return this.isIsolate;
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x0016C020 File Offset: 0x0016A220
	public void AttachBloodObject(float posx)
	{
		bool flag = UnityEngine.Random.Range(0, 2) == 1;
		if (this.groundInfo != null && this.wallInfo == null)
		{
			flag = true;
		}
		else if (this.groundInfo == null && this.wallInfo != null)
		{
			flag = false;
		}
		if (flag && this.groundInfo != null && this.groundInfo.bloodSprites.Count > 0)
		{
			BloodMapObjectModel bloodMapObjectModel = new BloodMapObjectModel();
			int index = UnityEngine.Random.Range(0, this.groundInfo.bloodSprites.Count);
			Sprite bloodSprite = this.groundInfo.bloodSprites[index];
			bloodMapObjectModel.position = new Vector3(posx, this.groundInfo.height + this.position.y, this.position.z - 0.01f);
			bloodMapObjectModel.passage = this;
			bloodMapObjectModel.bloodSprite = bloodSprite;
			this.AddBloodMapObject(bloodMapObjectModel);
		}
		if (!flag && this.wallInfo != null && this.wallInfo.bloodSprites.Count > 0)
		{
			BloodMapObjectModel bloodMapObjectModel2 = new BloodMapObjectModel();
			int index2 = UnityEngine.Random.Range(0, this.wallInfo.bloodSprites.Count);
			Sprite bloodSprite2 = this.wallInfo.bloodSprites[index2];
			bloodMapObjectModel2.position = new Vector3(posx, this.wallInfo.height + this.position.y, this.position.z - 0.01f);
			bloodMapObjectModel2.passage = this;
			bloodMapObjectModel2.bloodSprite = bloodSprite2;
			this.AddBloodMapObject(bloodMapObjectModel2);
		}
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x0016C1C0 File Offset: 0x0016A3C0
	public void AttachBloodObjectAlter(float posx)
	{
		bool flag = UnityEngine.Random.Range(0, 2) == 1;
		if (this.groundInfo != null && this.wallInfo == null)
		{
			flag = true;
		}
		else if (this.groundInfo == null && this.wallInfo != null)
		{
			flag = false;
		}
		if (flag && this.groundInfo != null && this.groundInfo.bloodSprites.Count > 0)
		{
			BloodMapObjectModel bloodMapObjectModel = new BloodMapObjectModel();
			int index = UnityEngine.Random.Range(0, this.groundInfo.alterSprites.Count);
			Sprite bloodSprite = this.groundInfo.alterSprites[index];
			bloodMapObjectModel.position = new Vector3(posx, this.groundInfo.height + this.position.y, this.position.z - 0.01f);
			bloodMapObjectModel.passage = this;
			bloodMapObjectModel.bloodSprite = bloodSprite;
			bloodMapObjectModel.color = Color.yellow;
			this.AddBloodMapObject(bloodMapObjectModel);
		}
		if (!flag && this.wallInfo != null && this.wallInfo.bloodSprites.Count > 0)
		{
			BloodMapObjectModel bloodMapObjectModel2 = new BloodMapObjectModel();
			int index2 = UnityEngine.Random.Range(0, this.wallInfo.alterSprites.Count);
			Sprite bloodSprite2 = this.wallInfo.alterSprites[index2];
			bloodMapObjectModel2.position = new Vector3(posx, this.wallInfo.height + this.position.y, this.position.z - 0.01f);
			bloodMapObjectModel2.passage = this;
			bloodMapObjectModel2.bloodSprite = bloodSprite2;
			bloodMapObjectModel2.color = Color.yellow;
			this.AddBloodMapObject(bloodMapObjectModel2);
		}
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0003291C File Offset: 0x00030B1C
	public DoorObjectModel[] GetDoorList()
	{
		return this.doorObjectList.ToArray();
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00032929 File Offset: 0x00030B29
	public float GetScaleFactor()
	{
		return this.scaleFactor;
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00032931 File Offset: 0x00030B31
	private void AddBloodMapObject(BloodMapObjectModel mapObject)
	{
		Notice.instance.Send(NoticeName.AddBloodMapObject, new object[]
		{
			mapObject
		});
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0003294C File Offset: 0x00030B4C
	public void EnterUnit(MovableObjectNode unit)
	{
		if (unit.GetUnit() != null && unit.GetUnit().IsEtcUnit())
		{
			if (!this.etcUnitList.Contains(unit))
			{
				this.etcUnitList.Add(unit);
			}
			return;
		}
		if (!this.enteredUnitList.Contains(unit))
		{
			this.enteredUnitList.Add(unit);
		}
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x0003296B File Offset: 0x00030B6B
	public void ExitUnit(MovableObjectNode unit)
	{
		if (this.enteredUnitList.Contains(unit))
		{
			this.enteredUnitList.Remove(unit);
		}
		else if (this.etcUnitList.Contains(unit))
		{
			this.etcUnitList.Remove(unit);
		}
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x0003298B File Offset: 0x00030B8B
	public void AddDeletedUnit(MovableObjectNode unit)
	{
		if (!this.deletedUnitList.Contains(unit))
		{
			this.deletedUnitList.Add(unit);
		}
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x000329AA File Offset: 0x00030BAA
	public MovableObjectNode[] GetDeletedUnits()
	{
		return this.deletedUnitList.ToArray();
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x000329B7 File Offset: 0x00030BB7
	public void RemoveDeletedUnit(MovableObjectNode unit)
	{
		if (this.deletedUnitList.Contains(unit))
		{
			this.deletedUnitList.Remove(unit);
		}
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x000329D7 File Offset: 0x00030BD7
	public void AddNode(MapNode node)
	{
		this.mapNodeList.Add(node);
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x000329E5 File Offset: 0x00030BE5
	public void AddDoor(DoorObjectModel door)
	{
		this.doorObjectList.Add(door);
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x000329F3 File Offset: 0x00030BF3
	public void AddElevator(ElevatorPassageModel elevator)
	{
		this.elevatorList.Add(elevator);
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x00032A01 File Offset: 0x00030C01
	public string GetId()
	{
		return this.id;
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x00032A09 File Offset: 0x00030C09
	public string GetSrc()
	{
		return this.src;
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x00032A11 File Offset: 0x00030C11
	public string GetSefiraName()
	{
		return this.sefiraName;
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x00032A19 File Offset: 0x00030C19
	public SefiraEnum GetSefiraEnum()
	{
		return this.sefiraEnum;
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x00032A21 File Offset: 0x00030C21
	public Sefira GetSefira()
	{
		return SefiraManager.instance.GetSefira(this.sefiraEnum);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x00032A33 File Offset: 0x00030C33
	public MapNode[] GetNodeList()
	{
		return this.mapNodeList.ToArray();
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x00032A40 File Offset: 0x00030C40
	public IList<MapNode> GetNodeListAsReadOnly()
	{
		return this.mapNodeList.AsReadOnly();
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x00032A4D File Offset: 0x00030C4D
	public ElevatorPassageModel[] GetElevatorList()
	{
		return this.elevatorList.ToArray();
	}

	// Token: 0x06003851 RID: 14417 RVA: 0x0016C378 File Offset: 0x0016A578
	public void Activate()
	{
		foreach (MapNode mapNode in this.mapNodeList)
		{
			mapNode.activate = true;
		}
		this.isActivate = true;
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x0016C3DC File Offset: 0x0016A5DC
	public virtual void FixedUpdate()
	{
		foreach (DoorObjectModel doorObjectModel in this.doorObjectList)
		{
			doorObjectModel.FixedUpdate();
		}
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x00032A5A File Offset: 0x00030C5A
	public bool isPassageEnteredAnyUnit()
	{
		return this.enteredUnitList.Count != 0;
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x0016C438 File Offset: 0x0016A638
	public bool isAreaHasOtherMov(MovableObjectNode mov)
	{
		bool result = false;
		foreach (MovableObjectNode movableObjectNode in this.enteredUnitList)
		{
			if (movableObjectNode != mov)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00032A6D File Offset: 0x00030C6D
	public ReadOnlyCollection<MovableObjectNode> GetEnteredTargets()
	{
		return this.enteredUnitList.AsReadOnly();
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00032A7A File Offset: 0x00030C7A
	public MovableObjectNode[] GetEnteredTargetsAsArray()
	{
		return this.enteredUnitList.ToArray();
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x0016C4A4 File Offset: 0x0016A6A4
	public MovableObjectNode[] GetEnteredTargets(MovableObjectNode exclude)
	{
		List<MovableObjectNode> list = new List<MovableObjectNode>();
		foreach (MovableObjectNode movableObjectNode in this.enteredUnitList)
		{
			if (movableObjectNode != exclude)
			{
				list.Add(movableObjectNode);
			}
		}
		return list.ToArray();
	}

	// <Mod>
	public ReadOnlyCollection<MovableObjectNode> GetEtcTargets()
	{
		return this.etcUnitList.AsReadOnly();
	}

	// <Mod>
	public MovableObjectNode[] GetEtcTargetsAsArray()
	{
		return this.etcUnitList.ToArray();
	}

	// <Mod>
	public MovableObjectNode[] GetEtcTargets(MovableObjectNode exclude)
	{
		List<MovableObjectNode> list = new List<MovableObjectNode>();
		foreach (MovableObjectNode movableObjectNode in this.etcUnitList)
		{
			if (movableObjectNode != exclude)
			{
				list.Add(movableObjectNode);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x0016C518 File Offset: 0x0016A718
	public MovableObjectNode GetRandomMovableNode()
	{
		List<MapNode> list = new List<MapNode>();
		MapNode mapNode = null;
		MapNode mapNode2 = null;
		MovableObjectNode movableObjectNode = null;
		foreach (MapNode mapNode3 in this.mapNodeList)
		{
			if (mapNode3.GetDoor() == null)
			{
				if (mapNode == null)
				{
					mapNode = mapNode3;
				}
				if (mapNode2 == null)
				{
					mapNode2 = mapNode3;
				}
				if (mapNode3.GetPosition().x < mapNode.GetPosition().x)
				{
					mapNode = mapNode3;
				}
				if (mapNode3.GetPosition().x > mapNode2.GetPosition().x)
				{
					mapNode2 = mapNode3;
				}
			}
		}
		if (mapNode == null || mapNode2 == null)
		{
			Debug.LogError("Non-door node Not Found");
			return null;
		}
		PathResult pathResult = GraphAstar.SearchPath(mapNode, mapNode2, false);
		if (pathResult == null || pathResult.pathEdges.Length == 0)
		{
			movableObjectNode = new MovableObjectNode(false);
			movableObjectNode.SetCurrentNode(mapNode);
			return movableObjectNode;
		}
		float num = UnityEngine.Random.Range(0f, pathResult.totalCost);
		float num2 = num;
		for (int i = 0; i < pathResult.pathEdges.Length; i++)
		{
			MapEdge mapEdge = pathResult.pathEdges[i];
			if (num2 <= mapEdge.cost)
			{
				movableObjectNode = new MovableObjectNode(false);
				movableObjectNode.SetCurrentEdge(mapEdge, num2 / mapEdge.cost, pathResult.edgeDirections[i]);
				break;
			}
			num2 -= mapEdge.cost;
		}
		return movableObjectNode;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x0016C6B8 File Offset: 0x0016A8B8
	public void CheckCenter()
	{
		if (this.centerNode != null)
		{
			return;
		}
		List<MapNode> sortedList = this.GetSortedList();
		float num = sortedList[0].GetPosition().x + sortedList[sortedList.Count - 1].GetPosition().x;
		num /= 2f;
		MapNode mapNode = sortedList[sortedList.Count / 2];
		float num2 = Mathf.Abs(mapNode.GetPosition().x - num);
		foreach (MapNode mapNode2 in sortedList)
		{
			float x = mapNode2.GetPosition().x;
			float num3 = Mathf.Abs(x - num);
			if (num3 <= num2)
			{
				num2 = num3;
				mapNode = mapNode2;
			}
		}
		this.centerNode = mapNode;
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x0016C7B8 File Offset: 0x0016A9B8
	public void GetVerticalRange(ref float left, ref float right)
	{
		List<MapNode> sortedList = this.GetSortedList();
		left = sortedList[0].GetPosition().x;
		right = sortedList[sortedList.Count - 1].GetPosition().x;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x0016C800 File Offset: 0x0016AA00
	public float GetWidth()
	{
		List<MapNode> sortedList = this.GetSortedList();
		float x = sortedList[0].GetPosition().x;
		float x2 = sortedList[sortedList.Count - 1].GetPosition().x;
		return x2 - x;
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x0016C858 File Offset: 0x0016AA58
	public MapNode GetLeft()
	{
		MapNode result;
		try
		{
			List<MapNode> sortedList = this.GetSortedList();
			result = sortedList[0];
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x0016C894 File Offset: 0x0016AA94
	public MapNode GetRight()
	{
		MapNode result;
		try
		{
			List<MapNode> sortedList = this.GetSortedList();
			result = sortedList[sortedList.Count - 1];
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x0016C8D8 File Offset: 0x0016AAD8
	private List<MapNode> GetSortedList()
	{
		if (this._sortedList == null)
		{
			this._sortedList = new List<MapNode>(this.mapNodeList.ToArray());
			List<MapNode> sortedList = this._sortedList;
			if (PassageObjectModel.cache0 == null)
			{
				PassageObjectModel.cache0 = new Comparison<MapNode>(MapNode.CompareByX);
			}
			sortedList.Sort(PassageObjectModel.cache0);
		}
		return this._sortedList;
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x00032A87 File Offset: 0x00030C87
	public MapNode[] GetSortedListAsAry()
	{
		return this.GetSortedList().ToArray();
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x00032A94 File Offset: 0x00030C94
	public void OnStageStart()
	{
		this.sefiraDisabled = false;
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00032A9D File Offset: 0x00030C9D
	public void OnStageEnd()
	{
		this.deletedUnitList.Clear();
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00032AAA File Offset: 0x00030CAA
	public void DisableSefira()
	{
		this.sefiraDisabled = true;
	}

	// Token: 0x04003389 RID: 13193
	public bool isActivate;

	// Token: 0x0400338A RID: 13194
	public bool isDynamic;

	// Token: 0x0400338B RID: 13195
	private string id;

	// Token: 0x0400338C RID: 13196
	private string src;

	// Token: 0x0400338D RID: 13197
	private string sefiraName;

	// Token: 0x0400338E RID: 13198
	private SefiraEnum sefiraEnum = SefiraEnum.DUMMY;

	// Token: 0x0400338F RID: 13199
	private bool isIsolate;

	// Token: 0x04003390 RID: 13200
	public PassageGroundInfo groundInfo;

	// Token: 0x04003391 RID: 13201
	public PassageWallInfo wallInfo;

	// Token: 0x04003392 RID: 13202
	private List<MapNode> mapNodeList;

	// Token: 0x04003393 RID: 13203
	private List<MapNode> _sortedList;

	// Token: 0x04003394 RID: 13204
	private List<DoorObjectModel> doorObjectList;

	// Token: 0x04003395 RID: 13205
	private List<ElevatorPassageModel> elevatorList;

	// Token: 0x04003396 RID: 13206
	private List<string> connectedSefira;

	// Token: 0x04003397 RID: 13207
	public MapNode centerNode;

	// Token: 0x04003398 RID: 13208
	public float height;

	// Token: 0x04003399 RID: 13209
	private List<MovableObjectNode> enteredUnitList;

	// Token: 0x0400339A RID: 13210
	private List<MovableObjectNode> deletedUnitList;

	// <Mod>
	private List<MovableObjectNode> etcUnitList;

	// Token: 0x0400339B RID: 13211
	public float scaleFactor = 1f;

	// Token: 0x0400339C RID: 13212
	public PassageType type = PassageType.NONE;

	// Token: 0x0400339D RID: 13213
	public string passageGroup = string.Empty;

	// Token: 0x0400339E RID: 13214
	public string rabbitTeamGroup = string.Empty;

	// Token: 0x0400339F RID: 13215
	public bool sefiraDisabled;

	// Token: 0x040033A0 RID: 13216
	[CompilerGenerated]
	private static Comparison<MapNode> cache0;

    // <Mod>
    public bool isRegenerator;
}
