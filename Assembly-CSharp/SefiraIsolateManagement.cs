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
	{
		foreach (SefiraIsolate sefiraIsolate in this._notUsed)
		{
			if (sefiraIsolate.nodeId == nodeId)
			{
				this._notUsed.Remove(sefiraIsolate);
				this._used.Add(sefiraIsolate);
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
	{
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
		return list3.ToArray();
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x0019B5FC File Offset: 0x001997FC
	public SefiraIsolate[] GenIsolateByCreatureAryByOrder(long[] creatureIdAry)
	{
		List<SefiraIsolate> list = new List<SefiraIsolate>();
		foreach (long creatureId in creatureIdAry)
		{
			SefiraIsolate notUsed = this.GetNotUsed();
			notUsed.creatureId = creatureId;
			list.Add(notUsed);
		}
		return list.ToArray();
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x0019B648 File Offset: 0x00199848
	public SefiraIsolate GenIsolateByCreatureAryByOrder(long creatureId)
	{
		SefiraIsolate notUsed = this.GetNotUsed();
		notUsed.creatureId = creatureId;
		return notUsed;
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x0019B664 File Offset: 0x00199864
	public SefiraIsolate GenIsolateByCreatureByNodeId(long creatureId, string nodeId)
	{
		SefiraIsolate byNodeId = this.GetByNodeId(nodeId);
		if (byNodeId == null)
		{
			throw new SefiraIsolateException
			{
				nodeId = nodeId
			};
		}
		byNodeId.creatureId = creatureId;
		return byNodeId;
	}

	// Token: 0x04003DDE RID: 15838
	public List<SefiraIsolate> list;

	// Token: 0x04003DDF RID: 15839
	private List<SefiraIsolate> _notUsed;

	// Token: 0x04003DE0 RID: 15840
	private List<SefiraIsolate> _used;
}
