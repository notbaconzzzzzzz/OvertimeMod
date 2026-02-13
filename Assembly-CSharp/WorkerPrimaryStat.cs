/*
public static int MaxStatR() // 
public static int MaxStatW() // 
public static int MaxStatB() // 
public static int MaxStatP() // 
*/
using System;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
[Serializable]
public class WorkerPrimaryStat
{
	// Token: 0x060032EA RID: 13034 RVA: 0x0002EC7C File Offset: 0x0002CE7C
	public WorkerPrimaryStat()
	{
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060032EB RID: 13035 RVA: 0x0002ECA4 File Offset: 0x0002CEA4
	public int maxHP
	{
		get
		{
			return this.hp;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060032EC RID: 13036 RVA: 0x0002ECAC File Offset: 0x0002CEAC
	public int maxMental
	{
		get
		{
			return this.mental;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060032ED RID: 13037 RVA: 0x0002ECB4 File Offset: 0x0002CEB4
	public int cubeSpeed
	{
		get
		{
			return this.work;
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060032EE RID: 13038 RVA: 0x0002ECB4 File Offset: 0x0002CEB4
	public int workProb
	{
		get
		{
			return this.work;
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060032EF RID: 13039 RVA: 0x0002ECBC File Offset: 0x0002CEBC
	public int movementSpeed
	{
		get
		{
			return this.battle;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060032F0 RID: 13040 RVA: 0x0002ECBC File Offset: 0x0002CEBC
	public int attackSpeed
	{
		get
		{
			return this.battle;
		}
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x00156628 File Offset: 0x00154828
	public WorkerPrimaryStat GetAddedStat(WorkerPrimaryStatExp expAdd)
	{
		WorkerPrimaryStat workerPrimaryStat = new WorkerPrimaryStat();
		int a = WorkerPrimaryStat.MaxStatR();
		int a2 = WorkerPrimaryStat.MaxStatW();
		int a3 = WorkerPrimaryStat.MaxStatB();
		int a4 = WorkerPrimaryStat.MaxStatP();
		workerPrimaryStat.hp = Mathf.Min(a, this.hp + Mathf.RoundToInt(expAdd.hp));
		workerPrimaryStat.mental = Mathf.Min(a2, this.mental + Mathf.RoundToInt(expAdd.mental));
		workerPrimaryStat.work = Mathf.Min(a3, this.work + Mathf.RoundToInt(expAdd.work));
		workerPrimaryStat.battle = Mathf.Min(a4, this.battle + Mathf.RoundToInt(expAdd.battle));
		return workerPrimaryStat;
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
	public static int MaxStatR()
	{ // <Mod>
		int max = 100;
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_r_2"))
		{
			max += 20;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_r"))
		{
			max += 20;
		}
		return max;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x0002ECF6 File Offset: 0x0002CEF6
	public static int MaxStatW()
	{ // <Mod>
		int max = 100;
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_w_2"))
		{
			max += 20;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_w"))
		{
			max += 20;
		}
		return max;
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0002ED28 File Offset: 0x0002CF28
	public static int MaxStatB()
	{ // <Mod>
		int max = 100;
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_b_2"))
		{
			max += 20;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 10;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("stat_max_b"))
		{
			max += 20;
		}
		return max;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x0002ED5A File Offset: 0x0002CF5A
	public static int MaxStatP()
	{ // <Mod>
		int max = 100;
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 30;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH))
		{
			max += 30;
		}
		return max;
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x001566CC File Offset: 0x001548CC
	public void UpdateStat(WorkerPrimaryStatExp exp)
	{
		WorkerPrimaryStat addedStat = this.GetAddedStat(exp);
		this.hp = addedStat.hp;
		this.mental = addedStat.mental;
		this.work = addedStat.work;
		this.battle = addedStat.battle;
	}

	// Token: 0x04003059 RID: 12377
	public int hp = 10;

	// Token: 0x0400305A RID: 12378
	public int mental = 10;

	// Token: 0x0400305B RID: 12379
	public int work = 10;

	// Token: 0x0400305C RID: 12380
	public int battle = 10;
}
