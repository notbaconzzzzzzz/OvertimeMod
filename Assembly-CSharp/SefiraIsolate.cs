using System;
using System.Collections.Generic;

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
}
