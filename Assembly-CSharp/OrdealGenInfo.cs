/*
Various changes
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007A7 RID: 1959
public class OrdealGenInfo
{
	// Token: 0x06003C98 RID: 15512 RVA: 0x00004074 File Offset: 0x00002274
	public OrdealGenInfo()
	{
	}

	// Token: 0x06003C99 RID: 15513 RVA: 0x0017A8E0 File Offset: 0x00178AE0
	private static OrdealBase CreateRandomDawn(int index = -1)
	{ // <Mod>
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 4) : index)
		{
		case 0:
			ordeal = new MachineDawnOrdeal();
			break;
		case 1:
			ordeal = new CircusDawnOrdeal();
			break;
		case 2:
			ordeal = new BugDawnOrdeal();
			break;
		case 3:
			ordeal = new OutterGodDawnOrdeal();
			break;
		default:
			ordeal = new BugDawnOrdeal();
			break;
		}
		return ordeal;
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x0017A930 File Offset: 0x00178B30
	private static OrdealBase CreateRandomNoon(int day, int index = -1)
	{ // <Mod>
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, day <= OrdealGenInfo._scanvengerAttidionDay ? 3 : 4) : index)
		{
		case 0:
			ordeal = new MachineNoonOrdeal();
			break;
		case 1:
			ordeal = new CircusNoonOrdeal();
			break;
		case 2:
			ordeal = new OutterGodNoonOrdeal();
			break;
		case 3:
			ordeal = new ScavengerNoonOrdeal();
			break;
		default:
			ordeal = new MachineNoonOrdeal();
			break;
		}
		return ordeal;
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x0017A9C0 File Offset: 0x00178BC0
	private static OrdealBase CreateRandomDusk(int index = -1)
	{ // <Mod>
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 3) : index)
		{
		case 0:
			ordeal = new BugDuskOrdeal();
			break;
		case 1:
			ordeal = new CircusDuskOrdeal();
			break;
		case 2:
			ordeal = new MachineDuskOrdeal();
			break;
		default:
			ordeal = new CircusDuskOrdeal();
			break;
		}
		return ordeal;
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x0017AA08 File Offset: 0x00178C08
	private static OrdealBase CreateRandomMidnight(int index = -1)
	{ // <Mod>
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 3) : index)
		{
		case 0:
			ordeal = new BugMidnightOrdeal();
			break;
		case 1:
			ordeal = new MachineMidnightOrdeal();
			break;
		case 2:
			ordeal = new OutterGodMidnightOrdeal();
			break;
		default:
			ordeal = new OutterGodMidnightOrdeal();
			break;
		}
		return ordeal;
	}

    //> <Mod>
	private static OrdealBase CreateRandomOvertimeDawn(int index = -1)
	{
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 4) : index)
		{
		case 0:
			ordeal = new OvertimeMachineDawnOrdeal();
			break;
		case 1:
			ordeal = new OvertimeCircusDawnOrdeal();
			break;
		case 2:
			ordeal = new OvertimeBugDawnOrdeal();
			break;
		case 3:
			ordeal = new OvertimeOutterGodDawnOrdeal();
			break;
		default:
			ordeal = new BugDawnOrdeal();
			break;
		}
		if (ordeal.level < OrdealLevel.OVERTIME_DAWN)
		{
			ordeal.level += (int)OrdealLevel.OVERTIME_DAWN;
		}
		return ordeal;
	}

	private static OrdealBase CreateRandomOvertimeNoon(int day, int index = -1)
	{
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 0) /*UnityEngine.Random.Range(0, day <= OrdealGenInfo._scanvengerAttidionDay ? 3 : 4)*/ : index)
		{
		case 0:
			ordeal = new OvertimeMachineNoonOrdeal();
			break;
		case 1:
			ordeal = new CircusNoonOrdeal();
			break;
		case 2:
			ordeal = new OutterGodNoonOrdeal();
			break;
		case 3:
			ordeal = new ScavengerNoonOrdeal();
			break;
		default:
			ordeal = new OvertimeMachineNoonOrdeal();
			break;
		}
		if (ordeal.level < OrdealLevel.OVERTIME_DAWN)
		{
			ordeal.level += (int)OrdealLevel.OVERTIME_DAWN;
		}
		return ordeal;
	}

	private static OrdealBase CreateRandomOvertimeDusk(int index = -1)
	{
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 3) : index)
		{
		case 0:
			ordeal = new BugDuskOrdeal();
			break;
		case 1:
			ordeal = new CircusDuskOrdeal();
			break;
		case 2:
			ordeal = new MachineDuskOrdeal();
			break;
		default:
			ordeal = new CircusDuskOrdeal();
			break;
		}
		if (ordeal.level < OrdealLevel.OVERTIME_DAWN)
		{
			ordeal.level += (int)OrdealLevel.OVERTIME_DAWN;
		}
		return ordeal;
	}

	private static OrdealBase CreateRandomOvertimeMidnight(int index = -1)
	{
		OrdealBase ordeal = null;
		switch (index == -1 ? UnityEngine.Random.Range(0, 3) : index)
		{
		case 0:
			ordeal = new BugMidnightOrdeal();
			break;
		case 1:
			ordeal = new MachineMidnightOrdeal();
			break;
		case 2:
			ordeal = new OutterGodMidnightOrdeal();
			break;
		default:
			ordeal = new OutterGodMidnightOrdeal();
			break;
		}
		if (ordeal.level < OrdealLevel.OVERTIME_DAWN)
		{
			ordeal.level += (int)OrdealLevel.OVERTIME_DAWN;
		}
		return ordeal;
	}
    //< <Mod>

	// Token: 0x06003C9D RID: 15517 RVA: 0x0003554D File Offset: 0x0003374D
	private static OrdealBase CreateFixerOrdeal(OrdealLevel level)
	{
		return new FixerOrdeal(level);
	}

	// Token: 0x06003C9E RID: 15518 RVA: 0x0017AA50 File Offset: 0x00178C50
	public static List<OrdealBase> GenerateOrdeals(int day)
	{ // <Mod>
		if (day == OrdealGenInfo._endingDay)
		{
			return new List<OrdealBase>();
		}
		List<OrdealBase> list = new List<OrdealBase>();
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			list.Add(OrdealGenInfo.CreateRandomDawn());
		}
		if (SefiraBossManager.Instance.IsKetherBoss())
		{
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
			{
				return list;
			}
            if (SpecialModeConfig.instance.GetValue<bool>("EarlyOvertimeOrdeals"))
            {
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.OVERTIME_DAWN));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.OVERTIME_NOON));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.OVERTIME_DUSK));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.OVERTIME_MIDNIGHT));
			}
			else
			{
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.DAWN));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.NOON));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.DUSK));
				list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.MIDNIGHT));
			}
            if (SpecialModeConfig.instance.GetValue<bool>("OvertimeOrdeals"))
            {
                list.Add(CreateRandomOvertimeDawn());
                list.Add(CreateRandomOvertimeNoon(day));
                list.Add(CreateRandomOvertimeDusk());
                list.Add(CreateRandomOvertimeMidnight());
            }
		}
		else
		{
            if (SpecialModeConfig.instance.GetValue<bool>("EarlyOvertimeOrdeals"))
            {
				if (day >= OrdealGenInfo._dawnAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomOvertimeDawn());
				}
				if (day >= OrdealGenInfo._noonAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomOvertimeNoon(day));
				}
				if (day >= OrdealGenInfo._duskAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomOvertimeDusk());
				}
				if (day >= OrdealGenInfo._midnightAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomOvertimeMidnight());
				}
			}
			else
			{
				if (day >= OrdealGenInfo._dawnAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomDawn());
				}
				if (day >= OrdealGenInfo._noonAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomNoon(day));
				}
				if (day >= OrdealGenInfo._duskAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomDusk());
				}
				if (day >= OrdealGenInfo._midnightAdditionDay)
				{
					list.Add(OrdealGenInfo.CreateRandomMidnight());
				}
			}
            if (SpecialModeConfig.instance.GetValue<bool>("OvertimeOrdeals"))
            {
                if (day >= _overtimeDawnAdditionDay)
                {
                    list.Add(CreateRandomOvertimeDawn());
                }
                if (day >= _overtimeNoonAdditionDay)
                {
                    list.Add(CreateRandomOvertimeNoon(day));
                }
                if (day >= _overtimeDuskAdditionDay)
                {
                    list.Add(CreateRandomOvertimeDusk());
                }
                if (day >= _overtimeMidnightAdditionDay)
                {
                    list.Add(CreateRandomOvertimeMidnight());
                }
            }
		}
		return list;
	}

	// <Mod>
	public static List<OrdealBase> GenerateSecondaryOrdeals(int day, List<OrdealBase> noDuplicates)
	{
		if (day == OrdealGenInfo._endingDay)
		{
			return new List<OrdealBase>();
		}
		List<OrdealBase> list = new List<OrdealBase>();
		List<int> dawns = new List<int>();
		for (int i = 0; i < 4; i++)
		{
			dawns.Add(i);
		}
		List<int> noons = new List<int>();
		for (int i = 0; i < (day <= OrdealGenInfo._scanvengerAttidionDay ? 3 : 4); i++)
		{
			noons.Add(i);
		}
		List<int> dusks = new List<int>();
		for (int i = 0; i < 3; i++)
		{
			dusks.Add(i);
		}
		List<int> midnights = new List<int>();
		for (int i = 0; i < 3; i++)
		{
			midnights.Add(i);
		}
		foreach (OrdealBase ordeal in noDuplicates)
		{
			switch (ordeal.GetType().ToString())
			{
				case "OvertimeMachineDawnOrdeal": dawns.Remove(0); break;
				case "OvertimeCircusDawnOrdeal": dawns.Remove(1); break;
				case "OvertimeBugDawnOrdeal": dawns.Remove(2); break;
				case "OvertimeOutterGodDawnOrdeal": dawns.Remove(3); break;
				case "OvertimeMachineNoonOrdeal": noons.Remove(0); break;
				case "OvertimeCircusNoonOrdeal": noons.Remove(1); break;
				case "OvertimeOutterGodNoonOrdeal": noons.Remove(2); break;
				case "OvertimeScavengerNoonOrdeal": noons.Remove(3); break;
				case "OvertimeBugDuskOrdeal": dusks.Remove(0); break;
				case "OvertimeCircusDuskOrdeal": dusks.Remove(1); break;
				case "OvertimeMachineDuskOrdeal": dusks.Remove(2); break;
				case "OvertimeBugMidnightOrdeal": midnights.Remove(0); break;
				case "OvertimeMachineMidnightOrdeal": midnights.Remove(1); break;
				case "OvertimeOutterGodMidnightOrdeal": midnights.Remove(2); break;
			}
		}
		if (day >= _overtimeDawnAdditionDay)
		{
			if (dawns.Count <= 0)
			{
				list.Add(CreateRandomOvertimeDawn());
			}
			else
			{
				list.Add(CreateRandomOvertimeDawn(dawns[UnityEngine.Random.Range(0, dawns.Count)]));
			}
		}
		if (day >= _overtimeNoonAdditionDay)
		{
			if (noons.Count <= 0)
			{
				list.Add(CreateRandomOvertimeNoon(day));
			}
			else
			{
				list.Add(CreateRandomOvertimeNoon(day, noons[UnityEngine.Random.Range(0, noons.Count)]));
			}
		}
		if (day >= _overtimeDuskAdditionDay)
		{
			if (dusks.Count <= 0)
			{
				list.Add(CreateRandomOvertimeDusk());
			}
			else
			{
				list.Add(CreateRandomOvertimeDusk(dusks[UnityEngine.Random.Range(0, dusks.Count)]));
			}
		}
		if (day >= _overtimeMidnightAdditionDay)
		{
			if (midnights.Count <= 0)
			{
				list.Add(CreateRandomOvertimeMidnight());
			}
			else
			{
				list.Add(CreateRandomOvertimeMidnight(midnights[UnityEngine.Random.Range(0, midnights.Count)]));
			}
		}
		return list;
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x00035555 File Offset: 0x00033755
	// Note: this type is marked as 'beforefieldinit'.
	static OrdealGenInfo()
	{
	}

	// Token: 0x0400374C RID: 14156
	public static int _dawnAdditionDay = 5;

	// Token: 0x0400374D RID: 14157
	public static int _noonAdditionDay = 10;

	// Token: 0x0400374E RID: 14158
	public static int _duskAdditionDay = 20;

	// Token: 0x0400374F RID: 14159
	public static int _midnightAdditionDay = 25;

	// Token: 0x04003750 RID: 14160
	public static int _scanvengerAttidionDay = 25;

	// Token: 0x04003751 RID: 14161
	public static int _fixerAddtionDay = 45;

	// Token: 0x04003752 RID: 14162
	public static int _endingDay = 50;

    //> <Mod>
	public static int _overtimeDawnAdditionDay = 20;

	public static int _overtimeNoonAdditionDay = 10;

	public static int _overtimeDuskAdditionDay = 5;

	public static int _overtimeMidnightAdditionDay = 0;
    //< <Mod>
}
