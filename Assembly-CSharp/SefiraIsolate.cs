/*
+buncha stuff for Yesod Overtime Suppression
*/
using System;
using System.Collections.Generic;
using UnityEngine; // 

// Token: 0x02000882 RID: 2178
public class SefiraIsolate
{
	// Token: 0x06004330 RID: 17200 RVA: 0x000396CD File Offset: 0x000378CD
	public SefiraIsolate()
	{
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x0019C5AC File Offset: 0x0019A7AC
	public bool isExclusive(long target)
	{
		foreach (long num in this.exclusiveID)
		{
			if (target == num)
			{
				return true;
			}
		}
		return false;
	}
    // <Patch>
    public LobotomyBaseMod.LcIdLong GetLcId()
    {
        if (this.modid == null)
        {
            return new LobotomyBaseMod.LcIdLong(this.creatureId);
        }
        return new LobotomyBaseMod.LcIdLong(this.modid, this.creatureId);
    }

    // <Mod>
    public void Bump(CreatureModel creature, int val = 0)
    {
        if (preBumpX == -1000f || preBumpY == -1000f)
        {
            preBumpX = x;
            preBumpY = y;
        }
        if (isBumped == val) return;
		if (val >= bumpData.Count) return;
		if (val < -1) return;
        float temp = x;
        float temp2 = y;
		if (val == -1)
		{
			x = preBumpX;
			y = preBumpY;

		}
		else
		{
			BumpDetail bump = bumpData[val];
			x = bump.x;
			y = bump.y;
		}
        isBumped = val;
        if (creature != null)
        {
            PassageObjectModel passage = creature.roomNode.GetAttachedPassage();
            creature.basePosition = new Vector2(x, y);
            passage.position = new Vector3(x, y, 0f);
            Vector3 adjust = new Vector3(x - temp, y - temp2, 0f);
            foreach (MapNode node in passage.GetNodeList())
            {
                node.SetPosition(node.GetPosition() + adjust);
            }
            foreach (DoorObjectModel door in passage.GetDoorList())
            {
                door.position += adjust;
            }
        }
    }

	// Token: 0x04003DE0 RID: 15840
	public string nodeId;

	// Token: 0x04003DE1 RID: 15841
	public float x;

	// Token: 0x04003DE2 RID: 15842
	public float y;

	// Token: 0x04003DE3 RID: 15843
	public IsolatePos pos;

	// Token: 0x04003DE4 RID: 15844
	public int index;

	// Token: 0x04003DE5 RID: 15845
	public long creatureId;

	// Token: 0x04003DE6 RID: 15846
	public List<long> exclusiveID = new List<long>();

	// Token: 0x04003DE7 RID: 15847
	public bool isUsed;

    // <Patch>
    public string modid;

    //> <Mod>
	public string oldId;/*

    public bool isBumped;

	public float bumpedX = -1000f;

	public float bumpedY = -1000f;

	public string bumpedByNode;

	public string bumpedBySefira;

	public string bumpsNode;

	public string bumpsSefira;*/

    public int isBumped = -1;

	public float preBumpX = -1000f;

	public float preBumpY = -1000f;

	public List<SefiraNodePointer> bumps = new List<SefiraNodePointer>();

	public List<BumpDetail> bumpData = new List<BumpDetail>();

    public struct SefiraNodePointer
    {
        public SefiraNodePointer(string _node)
        {
            nodeId = _node;
            sefira = "";
        }

        public SefiraNodePointer(string _node, string _sefira)
        {
            nodeId = _node;
            sefira = _sefira;
        }

        public string nodeId;

        public string sefira;
    }

    public class BumpDetail
    {
        public BumpDetail(float _x, float _y)
        {
            x = _x;
            y = _y;
            bumpedBy = new List<SefiraNodePointer>();
        }

        public BumpDetail(float _x, float _y, List<SefiraNodePointer> list)
        {
            x = _x;
            y = _y;
            bumpedBy = list;
        }

        public float x;

        public float y;

        public List<SefiraNodePointer> bumpedBy;
    }
}
