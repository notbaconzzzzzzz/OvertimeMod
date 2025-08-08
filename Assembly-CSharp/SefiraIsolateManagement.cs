/*
public SefiraIsolate GetByNodeId(string nodeId) // 
+public SefiraIsolateManagement(SefiraIsolate[] ary, Sefira origin) // 
+public SefiraIsolate GetNotUsedUpper() // 
public SefiraIsolate[] GenIsolateByCreatureAryByOrder(long[] creatureIdAry) // 
+private SefiraEnum _sefira
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class SefiraIsolateManagement
{
	// Token: 0x06004319 RID: 17177 RVA: 0x000395F1 File Offset: 0x000377F1
	public SefiraIsolateManagement(SefiraIsolate[] ary)
	{
		this.list = new List<SefiraIsolate>(ary);
		this._notUsed = new List<SefiraIsolate>(ary);
		this._used = new List<SefiraIsolate>();
	}

	// <Mod>
	public SefiraIsolateManagement(SefiraIsolate[] ary, SefiraEnum origin)
	{
		list = new List<SefiraIsolate>(ary);
		_notUsed = new List<SefiraIsolate>(ary);
		_used = new List<SefiraIsolate>();
		_sefira = origin;
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x0019B284 File Offset: 0x00199484
	private bool isExclusiveByIsolate(long id)
	{
		foreach (SefiraIsolate sefiraIsolate in this._notUsed)
		{
			if (sefiraIsolate.isExclusive(id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x0019B2F0 File Offset: 0x001994F0
	public void LogRemain()
	{
		foreach (SefiraIsolate sefiraIsolate in this._notUsed)
		{
			Debug.Log(sefiraIsolate.nodeId);
		}
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x0019B350 File Offset: 0x00199550
	public SefiraIsolate GetByNodeId(string nodeId)
	{ // <Mod>
		foreach (SefiraIsolate sefiraIsolate in this._notUsed)
		{
			if (sefiraIsolate.nodeId == nodeId || sefiraIsolate.oldId == nodeId)
			{
				sefiraIsolate.isUsed = true;
				this._notUsed.Remove(sefiraIsolate);
				this._used.Add(sefiraIsolate);
				return sefiraIsolate;
			}
		}
		return null;
	}

	public SefiraIsolate FindByNodeId(string nodeId)
	{ // <Mod>
		foreach (SefiraIsolate sefiraIsolate in _notUsed)
		{
			if (sefiraIsolate.nodeId == nodeId || sefiraIsolate.oldId == nodeId)
			{
				return sefiraIsolate;
			}
		}
		foreach (SefiraIsolate sefiraIsolate in _used)
		{
			if (sefiraIsolate.nodeId == nodeId || sefiraIsolate.oldId == nodeId)
			{
				return sefiraIsolate;
			}
		}
		return null;
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x0019B3DC File Offset: 0x001995DC
	public SefiraIsolate GetNotUsed()
	{
		if (this._notUsed.Count == 0)
		{
			Debug.Log("All Sefira IsolateRoom is Used ");
			return null;
		}
		SefiraIsolate sefiraIsolate = this._notUsed[0];
		sefiraIsolate.isUsed = true;
		this._notUsed.Remove(sefiraIsolate);
		this._used.Add(sefiraIsolate);
		return sefiraIsolate;
	}

    // <Mod>
	public SefiraIsolate GetNotUsedUpper()
	{
		if (_notUsed.Count == 0)
		{
			Debug.Log("All Sefira IsolateRoom is Used ");
			return null;
		}
		SefiraIsolate sefiraIsolate = null;
		int thresh = 4;
		if (_sefira == SefiraEnum.KETHER)
		{
			thresh = 8;
		}
		for (int i = 0; i < _notUsed.Count; i++)
		{
			if (_notUsed[i].index >= thresh)
			{
				sefiraIsolate = _notUsed[i];
				break;
			}
		}
		if (sefiraIsolate == null)
		{
			sefiraIsolate = _notUsed[0];
		}
		sefiraIsolate.isUsed = true;
		_notUsed.Remove(sefiraIsolate);
		_used.Add(sefiraIsolate);
		return sefiraIsolate;
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x0019B434 File Offset: 0x00199634
	public SefiraIsolate GetNotUsedRandom(long targetId)
	{
		if (this._notUsed.Count == 0)
		{
			Debug.LogError("All Sefira IsolateRoom is Used");
			return null;
		}
		int i = 0;
		while (i < 20)
		{
			i++;
			int index = UnityEngine.Random.Range(0, this._notUsed.Count);
			SefiraIsolate sefiraIsolate = this._notUsed[index];
			if (!sefiraIsolate.isExclusive(targetId))
			{
				SefiraIsolate sefiraIsolate2 = sefiraIsolate;
				sefiraIsolate2.isUsed = true;
				this._notUsed.Remove(sefiraIsolate2);
				this._used.Add(sefiraIsolate2);
				return sefiraIsolate2;
			}
		}
		Debug.LogError("Cannot add Creature");
		return null;
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x0019B4D8 File Offset: 0x001996D8
	public SefiraIsolate[] GenIsolateByCreatureAry(long[] creatureIdAry)
	{ // <Mod>
		List<long> list = new List<long>();
		List<long> list2 = new List<long>();
		List<SefiraIsolate> list3 = new List<SefiraIsolate>();
		foreach (long num in creatureIdAry)
		{
			if (this.isExclusiveByIsolate(num))
			{
				list.Add(num);
			}
			else
			{
				list2.Add(num);
			}
		}
		foreach (long num2 in list)
		{
			SefiraIsolate notUsedRandom = this.GetNotUsedRandom(num2);
			notUsedRandom.creatureId = num2;
			list3.Add(notUsedRandom);
		}
		foreach (long num3 in list2)
		{
			SefiraIsolate notUsedRandom2 = this.GetNotUsedRandom(num3);
			notUsedRandom2.creatureId = num3;
			list3.Add(notUsedRandom2);
		}
		foreach (SefiraIsolate isolate in list3)
		{
			CheckBump(isolate);
		}
		return list3.ToArray();
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x0019B5FC File Offset: 0x001997FC
	public SefiraIsolate[] GenIsolateByCreatureAryByOrder(long[] creatureIdAry)
	{ // <Mod>
		List<SefiraIsolate> list = new List<SefiraIsolate>();
        int i = 0;
		bool isDouble = false;
		if (_sefira == SefiraEnum.TIPERERTH1 || _sefira == SefiraEnum.TIPERERTH2 || _sefira == SefiraEnum.KETHER)
		{
			isDouble = true;
		}
		foreach (long creatureId in creatureIdAry)
		{
			SefiraIsolate notUsed = null;
            if (isDouble ? (i % 4 >= 2) : (i % 2 == 1))
            {
                notUsed = GetNotUsedUpper();
            }
			else
			{
				notUsed = GetNotUsed();
			}
			notUsed.creatureId = creatureId;
			list.Add(notUsed);
            i++;
		}
		foreach (SefiraIsolate isolate in list)
		{
			CheckBump(isolate);
		}
		return list.ToArray();
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x0019B648 File Offset: 0x00199848
	public SefiraIsolate GenIsolateByCreatureAryByOrder(long creatureId)
	{
		SefiraIsolate notUsed = this.GetNotUsed();
		CheckBump(notUsed);
		notUsed.creatureId = creatureId;
		return notUsed;
	}

	// <Patch>
	public SefiraIsolate[] GenIsolateByCreatureAryByOrder_Mod(LobotomyBaseMod.LcIdLong[] creatureIdAry)
	{ // <Mod>
		List<SefiraIsolate> list = new List<SefiraIsolate>();
        int i = 0;
		bool isDouble = false;
		if (_sefira == SefiraEnum.TIPERERTH1 || _sefira == SefiraEnum.TIPERERTH2 || _sefira == SefiraEnum.KETHER)
		{
			isDouble = true;
		}
		foreach (LobotomyBaseMod.LcIdLong lcIdLong in creatureIdAry)
		{
			SefiraIsolate notUsed = null;
            if (isDouble ? (i % 4 >= 2) : (i % 2 == 1))
            {
                notUsed = GetNotUsedUpper();
            }
			else
			{
				notUsed = GetNotUsed();
			}
			notUsed.creatureId = lcIdLong.id;
			notUsed.modid = lcIdLong.packageId;
			list.Add(notUsed);
            i++;
		}
		foreach (SefiraIsolate isolate in list)
		{
			CheckBump(isolate);
		}
		return list.ToArray();
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x0019B664 File Offset: 0x00199864
	public SefiraIsolate GenIsolateByCreatureByNodeId(long creatureId, string nodeId)
	{ // <Mod>
		SefiraIsolate byNodeId = this.GetByNodeId(nodeId);
		if (byNodeId == null)
		{
			throw new SefiraIsolateException
			{
				nodeId = nodeId
			};
		}
		CheckBump(byNodeId);
		byNodeId.creatureId = creatureId;
		return byNodeId;
	}

	// <Mod>
	public void CheckBump(SefiraIsolate isolate)
	{
		if (SpecialModeConfig.instance.GetValue<bool>("DoubleAbno"))
		{
			isolate.Bump(null, isolate.bumpData.Count - 1);
			return;
		}
		bool a = false;
		for (int i = isolate.bumpData.Count - 1; i >= 0; i--)
		{
			SefiraIsolate.BumpDetail bump = isolate.bumpData[i];
			bool b = false;
			foreach (SefiraIsolate.SefiraNodePointer node in bump.bumpedBy)
			{
				SefiraIsolate isolate2 = null;
				if (node.nodeId == "TipherethElevator")
				{
					b = SpecialModeConfig.instance.GetValue<bool>("TipherethElevators");
					break;
				}
				else if (node.sefira != "")
				{
					Sefira sefira = SefiraManager.instance.GetSefira(node.sefira);
					if (sefira != null)
					{
						isolate2 = sefira.isolateManagement.FindByNodeId(node.nodeId);
					}
				}
				else
				{
					isolate2 = FindByNodeId(node.nodeId);
				}
				if (isolate2 != null && isolate2.isUsed)
				{
					b = true;
					break;
				}
			}
			if (b)
			{
				a = true;
				isolate.Bump(null, i);
				break;
			}
		}
		if (!a)
		{
			isolate.Bump(null, -1);
		}
		foreach (SefiraIsolate.SefiraNodePointer node in isolate.bumps)
		{
			SefiraIsolate isolate2 = null;
			Sefira sefira;
			if (node.sefira != "")
			{
				sefira = SefiraManager.instance.GetSefira(node.sefira);
				if (sefira == null) return;
				isolate2 = sefira.isolateManagement.FindByNodeId(node.nodeId);
			}
			else
			{
				sefira = SefiraManager.instance.GetSefira(_sefira);
				isolate2 = FindByNodeId(node.nodeId);
			}
			if (isolate2 != null && isolate2.isUsed)
			{
				//int bump = isolate2.bumpData.FindIndex((SefiraIsolate.BumpDetail x) => x.bumpedBy.Exists((SefiraIsolate.SefiraNodePointer y) => y.nodeId == isolate.nodeId));
				int bump = isolate2.bumpData.Count - 1;
				if (bump > isolate2.isBumped)
				{
					CreatureModel creature = null;
					foreach (CreatureModel creature2 in sefira.creatureList)
					{
						if (creature2.isolateRoomData == isolate2)
						{
							creature = creature2;
							break;
						}
					}
					if (creature != null)
					{
						isolate2.Bump(creature, bump);
					}
				}
			}
		}
		/*
		if (isolate.bumpedByNode != "")
		{
			bool bump = false;
			if (SpecialModeConfig.instance.GetValue<bool>("DoubleAbno"))
			{
				bump = true;
			}
			else
			{
				SefiraIsolate isolate2 = null;
				if (isolate.bumpedBySefira != "")
				{
					Sefira sefira = SefiraManager.instance.GetSefira(isolate.bumpedBySefira);
					if (sefira != null)
					{
						isolate2 = sefira.isolateManagement.FindByNodeId(isolate.bumpedByNode);
					}
				}
				else
				{
					isolate2 = FindByNodeId(isolate.bumpedByNode);
				}
				if (isolate2 != null && isolate2.isUsed)
				{
					bump = true;
				}
			}
			if (isolate.isBumped != bump)
			{
				isolate.Bump(null, bump);
			}
		}
		if (isolate.bumpsNode != "")
		{
			if (SpecialModeConfig.instance.GetValue<bool>("DoubleAbno")) return;
			SefiraIsolate isolate2;
			Sefira sefira;
			if (isolate.bumpsSefira != "")
			{
				sefira = SefiraManager.instance.GetSefira(isolate.bumpsSefira);
				if (sefira == null) return;
				isolate2 = sefira.isolateManagement.FindByNodeId(isolate.bumpsNode);
			}
			else
			{
				sefira = SefiraManager.instance.GetSefira(_sefira);
				isolate2 = FindByNodeId(isolate.bumpsNode);
			}
			if (isolate2 == null) return;
			if (isolate2.isUsed && !isolate2.isBumped)
			{
				CreatureModel creature = null;
				foreach (CreatureModel creature2 in sefira.creatureList)
				{
					if (creature2.isolateRoomData == isolate2)
					{
						creature = creature2;
						break;
					}
				}
				if (creature != null)
				{
					isolate2.Bump(creature, true);
				}
			}
		}*/
	}

	// Token: 0x04003DDE RID: 15838
	public List<SefiraIsolate> list;

	// Token: 0x04003DDF RID: 15839
	private List<SefiraIsolate> _notUsed;

	// Token: 0x04003DE0 RID: 15840
	private List<SefiraIsolate> _used;

	// <Mod>
	private SefiraEnum _sefira = SefiraEnum.DUMMY;
}
