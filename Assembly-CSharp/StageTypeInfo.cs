using System;
using UnityEngine;

// Token: 0x02000733 RID: 1843
public class StageTypeInfo
{
	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06003A8D RID: 14989 RVA: 0x00034297 File Offset: 0x00032497
	public static StageTypeInfo instnace
	{
		get
		{
			if (StageTypeInfo._instance == null)
			{
				StageTypeInfo._instance = new StageTypeInfo();
			}
			return StageTypeInfo._instance;
		}
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x000342B2 File Offset: 0x000324B2
	public float GetEnergyNeed(int day)
	{
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			return 20f;
		}
		if (day >= this.energyVal.Length)
		{
			return this.GetEnergyNeedInUnlimitMode(day);
		}
		return (float)this.energyVal[day];
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x000342E9 File Offset: 0x000324E9
	private float GetEnergyNeedInUnlimitMode(int day)
	{
		if (day < 20)
		{
			Debug.LogError("not unlimit mode");
			return 100f;
		}
		return (float)(this.energyVal[19] + (day - 19) * 30);
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00179F24 File Offset: 0x00178124
	public int EndRank(int day, float time)
	{
		if (day >= this.stageTime.Length)
		{
			return 0;
		}
		if (time < (float)(this.stageTime[day] - 60))
		{
			return 4;
		}
		if (time >= (float)(this.stageTime[day] - 60) && time < (float)(this.stageTime[day] - 30))
		{
			return 3;
		}
		if (time >= (float)(this.stageTime[day] - 30) && time < (float)(this.stageTime[day] + 30))
		{
			return 2;
		}
		if (time >= (float)(this.stageTime[day] + 30) && time < (float)(this.stageTime[day] + 90))
		{
			return 1;
		}
		if (time >= (float)(this.stageTime[day] + 90))
		{
			return 0;
		}
		return 0;
	}

	// Token: 0x040035C1 RID: 13761
	private static StageTypeInfo _instance;

	// Token: 0x040035C2 RID: 13762
	public int goal = 100;

	// Token: 0x040035C3 RID: 13763
	public int[] energyVal = new int[]
	{
		15,
		30,
		45,
		60,
		80,
		80,
		100,
		120,
		140,
		170,
		170,
		190,
		210,
		230,
		260,
		260,
		280,
		300,
		330,
		360,
		420,
		460,
		500,
		550,
		600,
		630,
		660,
		700,
		740,
		800,
		840,
		880,
		930,
		980,
		1050,
		1090,
		1130,
		1180,
		1230,
		1300,
		1350,
		1400,
		1460,
		1520,
		1600,
		1680,
		1780,
		1880,
		2000,
		1500
	};

	// Token: 0x040035C4 RID: 13764
	public int[] stageTime = new int[]
	{
		180,
		240,
		300,
		360,
		480,
		420,
		420,
		480,
		480,
		600,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200,
		1200
	};

	// Token: 0x040035C5 RID: 13765
	public int[] allocateAgent = new int[]
	{
		3,
		4,
		5,
		5,
		5
	};
}
