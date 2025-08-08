using System;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class CircusOrdealCreature : CreatureBase
{
	// Token: 0x06002016 RID: 8214 RVA: 0x00021CBE File Offset: 0x0001FEBE
	public CircusOrdealCreature()
	{
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x00021CE5 File Offset: 0x0001FEE5
	public virtual string GetOrdealName()
	{
		return this._name;
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x00021CED File Offset: 0x0001FEED
	public virtual void SetOrdeal(CircusOrdeal ordealScript, OrdealLevel level, RiskLevel risk, string name)
	{
		this._ordealScript = ordealScript;
		this._level = level;
		this._risk = risk;
		this._name = name;
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000FB7EC File Offset: 0x000F99EC
	public virtual void OnDie()
	{
		int num = (int)this._level;
		if (--num >= 0)
		{
			MapNode mapNode = this.model.GetCurrentNode();
			if (mapNode == null)
			{
				MapEdge currentEdge = this.model.GetCurrentEdge();
				if (currentEdge == null)
				{
					Debug.Log("시공의 틈에서 죽음");
					return;
				}
				Debug.Log(this.model.GetMovableNode().edgePosRate);
				if ((double)this.model.GetMovableNode().edgePosRate > 0.5)
				{
					mapNode = currentEdge.node1;
				}
				else
				{
					mapNode = currentEdge.node2;
				}
			}
			for (int i = 0; i < this._spawnDown[num]; i++)
			{
				this._ordealScript.MakeOrdealCreature((OrdealLevel)num, mapNode);
			}
		}
		this._ordealScript.OnDie(this.model as OrdealCreatureModel);
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x0600201A RID: 8218 RVA: 0x00021D0C File Offset: 0x0001FF0C
	public RiskLevel Risk
	{
		get
		{
			return this._risk;
		}
	}

	// Token: 0x04002063 RID: 8291
	protected CircusOrdeal _ordealScript;

	// Token: 0x04002064 RID: 8292
	protected OrdealLevel _level;

	// Token: 0x04002065 RID: 8293
	protected RiskLevel _risk;

	// Token: 0x04002066 RID: 8294
	protected readonly int[] _spawnDown = new int[]
	{ // <Mod>
		3,
		1,
		0,
		0,
		4,
		1,
		0,
		0
	};

	// Token: 0x04002067 RID: 8295
	protected string _name = "circus_dawn";
}
