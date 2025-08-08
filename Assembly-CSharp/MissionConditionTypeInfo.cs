using System;

// Token: 0x0200071B RID: 1819
[Serializable]
public class MissionConditionTypeInfo
{
	// Token: 0x060039EF RID: 14831 RVA: 0x00033B36 File Offset: 0x00031D36
	public MissionConditionTypeInfo()
	{
	}

	// Token: 0x0400350A RID: 13578
	public ConditionCategory condition_Category = ConditionCategory.ACTION_CONDITION;

	// Token: 0x0400350B RID: 13579
	public ConditionType condition_Type = ConditionType.CLEAR_DAY;

	// Token: 0x0400350C RID: 13580
	public GoalType goal_Type = GoalType.MIN;

	// Token: 0x0400350D RID: 13581
	public int index;

	// Token: 0x0400350E RID: 13582
	public int goal;

	// Token: 0x0400350F RID: 13583
	public int stat;

	// Token: 0x04003510 RID: 13584
	public int minimumSecond = 600;

	// Token: 0x04003511 RID: 13585
	public float var1 = 27f;

	// Token: 0x04003512 RID: 13586
	public float var2 = 30f;
}
