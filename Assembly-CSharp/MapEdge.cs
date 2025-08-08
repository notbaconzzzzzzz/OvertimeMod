using System;
using UnityEngine;

// Token: 0x020005D3 RID: 1491
public class MapEdge
{
	// Token: 0x0600324F RID: 12879 RVA: 0x0002E6CA File Offset: 0x0002C8CA
	public MapEdge(MapNode node1, MapNode node2, string type)
	{
		this.node1 = node1;
		this.node2 = node2;
		this.type = type;
		this.cost = Mathf.Max(Vector3.Distance(node1.GetPosition(), node2.GetPosition()), 0.01f);
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x0002E708 File Offset: 0x0002C908
	public MapEdge(MapNode node1, MapNode node2, string type, float cost)
	{
		this.node1 = node1;
		this.node2 = node2;
		this.type = type;
		this.cost = cost;
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x0002E72D File Offset: 0x0002C92D
	public void AddEdgeInNode()
	{
		this.node1.AddEdge(this);
		this.node2.AddEdge(this);
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x0002E747 File Offset: 0x0002C947
	public MapNode ConnectedNode(MapNode node)
	{
		if (node == this.node1)
		{
			return this.node2;
		}
		if (node == this.node2)
		{
			return this.node1;
		}
		return null;
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x0002E747 File Offset: 0x0002C947
	public MapNode ConnectedNodeIgoreActivate(MapNode node)
	{
		if (node == this.node1)
		{
			return this.node2;
		}
		if (node == this.node2)
		{
			return this.node1;
		}
		return null;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x0002E770 File Offset: 0x0002C970
	public MapNode GetGoalNode(EdgeDirection direction)
	{
		if (direction == EdgeDirection.FORWARD)
		{
			return this.node2;
		}
		return this.node1;
	}

	// Token: 0x04002FEB RID: 12267
	public MapNode node1;

	// Token: 0x04002FEC RID: 12268
	public MapNode node2;

	// Token: 0x04002FED RID: 12269
	public float cost;

	// Token: 0x04002FEE RID: 12270
	public string type;

	// Token: 0x04002FEF RID: 12271
	public string name;

	// Token: 0x04002FF0 RID: 12272
	public bool activated;
}
