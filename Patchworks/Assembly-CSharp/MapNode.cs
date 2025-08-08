using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class MapNode
{
	// Token: 0x0600327B RID: 12923 RVA: 0x0015585C File Offset: 0x00153A5C
	public MapNode(string id, Vector3 pos, string areaName)
	{
		this.id = id;
		this.pos = pos;
		this.areaName = areaName;
		this.attachedPassage = null;
		this._activate = true;
		this.edges = new List<MapEdge>();
		this.zNodes = new List<MapNode>();
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x001558B4 File Offset: 0x00153AB4
	public MapNode(string id, Vector3 pos, string areaName, PassageObjectModel attachedPassage)
	{
		this.id = id;
		this.pos = pos;
		this.areaName = areaName;
		this.attachedPassage = attachedPassage;
		this._activate = true;
		this.edges = new List<MapEdge>();
		this.zNodes = new List<MapNode>();
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x0600327D RID: 12925 RVA: 0x0002E868 File Offset: 0x0002CA68
	// (set) Token: 0x0600327E RID: 12926 RVA: 0x0002E870 File Offset: 0x0002CA70
	public bool activate
	{
		get
		{
			return this._activate;
		}
		set
		{
			this._activate = value;
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x0002E879 File Offset: 0x0002CA79
	public void AttachElevator(ElevatorPassageModel elevator)
	{
		this.attachedElevator = elevator;
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x0002E882 File Offset: 0x0002CA82
	public ElevatorPassageModel GetElevator()
	{
		return this.attachedElevator;
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x0002E88A File Offset: 0x0002CA8A
	public void AddZNode(MapNode node)
	{
		this.zNodes.Add(node);
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x0002E898 File Offset: 0x0002CA98
	public MapNode[] GetZNodes()
	{
		return this.zNodes.ToArray();
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x0002E8A5 File Offset: 0x0002CAA5
	public void AddEdge(MapEdge edge)
	{
		this.edges.Add(edge);
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x0002E8B3 File Offset: 0x0002CAB3
	public void RemoveEdge(MapEdge edge)
	{
		this.edges.Remove(edge);
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x0002E8C2 File Offset: 0x0002CAC2
	public void SetDoor(DoorObjectModel door)
	{
		this.door = door;
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x0002E8CB File Offset: 0x0002CACB
	public DoorObjectModel GetDoor()
	{
		return this.door;
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x0002E8D3 File Offset: 0x0002CAD3
	public Vector3 GetPosition()
	{
		return this.pos;
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x0002E8DB File Offset: 0x0002CADB
	public void SetPosition(Vector3 pos)
	{
		this.pos = pos;
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x0002E8E4 File Offset: 0x0002CAE4
	public string GetId()
	{
		return this.id;
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x0002E8EC File Offset: 0x0002CAEC
	public string GetAreaName()
	{
		return this.areaName;
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x0002E8F4 File Offset: 0x0002CAF4
	public PassageObjectModel GetAttachedPassage()
	{
		return this.attachedPassage;
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x0002E8FC File Offset: 0x0002CAFC
	public MapEdge[] GetEdges()
	{
		return this.edges.ToArray();
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x0015590C File Offset: 0x00153B0C
	public MapEdge GetEdgeByNode(MapNode node)
	{
		if (node == this)
		{
			return null;
		}
		foreach (MapEdge mapEdge in this.edges)
		{
			if (mapEdge.node1 == node || mapEdge.node2 == node)
			{
				return mapEdge;
			}
		}
		return null;
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x0002E909 File Offset: 0x0002CB09
	public static int CompareByX(MapNode a, MapNode b)
	{
		return a.pos.x.CompareTo(b.pos.x);
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x0002E926 File Offset: 0x0002CB26
	public void SetTeleport(List<MapNode> teleportTo, UnitDirection dir)
	{
		this._teleportTo = teleportTo;
		this._teleportDirectionCondition = dir;
		Notice.instance.Send(NoticeName.SetTeleportNode, new object[]
		{
			this,
			dir
		});
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x0002E958 File Offset: 0x0002CB58
	public void ClearTeleportNode()
	{
		this._teleportTo.Clear();
		Notice.instance.Send(NoticeName.RemoveTeleportNode, new object[]
		{
			this
		});
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x0015598C File Offset: 0x00153B8C
	public MapNode GetTeleportNode(MovableObjectNode mv, bool elevatorEnter = false)
	{
		if (this._teleportTo.Count == 0)
		{
			return null;
		}
		if (mv.GetDirection() == this._teleportDirectionCondition || this._teleportDirectionCondition == UnitDirection.OTHER || (elevatorEnter && this._teleportDirectionCondition == UnitDirection.ELEVATOR))
		{
			return this._teleportTo[UnityEngine.Random.Range(0, this._teleportTo.Count)];
		}
		return null;
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x001559F8 File Offset: 0x00153BF8
	public MapNode GetTeleportNode(MapNode next, bool elevatorEnter = false)
	{
		if (this._teleportTo.Count == 0)
		{
			return null;
		}
		if (next.GetPosition().x == this.GetPosition().x)
		{
			return null;
		}
		UnitDirection unitDirection = (next.GetPosition().x <= this.GetPosition().x) ? UnitDirection.LEFT : UnitDirection.RIGHT;
		if (unitDirection == this._teleportDirectionCondition || this._teleportDirectionCondition == UnitDirection.OTHER || (elevatorEnter && this._teleportDirectionCondition == UnitDirection.ELEVATOR))
		{
			return this._teleportTo[UnityEngine.Random.Range(0, this._teleportTo.Count)];
		}
		return null;
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x0002E97E File Offset: 0x0002CB7E
	public MapNode[] GetTeleportNodes()
	{
		return this._teleportTo.ToArray();
	}

	// Token: 0x04002FFF RID: 12287
	private string id;

	// Token: 0x04003000 RID: 12288
	private bool _activate;

	// Token: 0x04003001 RID: 12289
	private string areaName;

	// Token: 0x04003002 RID: 12290
	private List<MapEdge> edges;

	// Token: 0x04003003 RID: 12291
	private List<MapNode> zNodes;

	// Token: 0x04003004 RID: 12292
	private Vector3 pos;

	// Token: 0x04003005 RID: 12293
	public bool isTemporary;

	// Token: 0x04003006 RID: 12294
	public bool closed;

	// Token: 0x04003007 RID: 12295
	public CreatureModel connectedCreature;

	// Token: 0x04003008 RID: 12296
	private List<MapNode> _teleportTo = new List<MapNode>();

	// Token: 0x04003009 RID: 12297
	private UnitDirection _teleportDirectionCondition;

	// Token: 0x0400300A RID: 12298
	private DoorObjectModel door;

	// Token: 0x0400300B RID: 12299
	private PassageObjectModel attachedPassage;

	// Token: 0x0400300C RID: 12300
	private ElevatorPassageModel attachedElevator;

	// Token: 0x0400300D RID: 12301
	public bool rabbitUnpassable;
}
