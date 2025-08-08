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
	private static OrdealBase CreateRandomDawn()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			return new MachineDawnOrdeal();
		case 1:
			return new CircusDawnOrdeal();
		case 2:
			return new BugDawnOrdeal();
		case 3:
			return new OutterGodDawnOrdeal();
		default:
			return new BugDawnOrdeal();
		}
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x0017A930 File Offset: 0x00178B30
	private static OrdealBase CreateRandomNoon(int day)
	{
		if (day <= OrdealGenInfo._scanvengerAttidionDay)
		{
			int num = UnityEngine.Random.Range(0, 3);
			if (num == 0)
			{
				return new MachineNoonOrdeal();
			}
			if (num == 1)
			{
				return new CircusNoonOrdeal();
			}
			if (num == 2)
			{
				return new OutterGodNoonOrdeal();
			}
		}
		else
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				return new MachineNoonOrdeal();
			case 1:
				return new CircusNoonOrdeal();
			case 2:
				return new OutterGodNoonOrdeal();
			case 3:
				return new ScavengerNoonOrdeal();
			}
		}
		return new MachineNoonOrdeal();
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x0017A9C0 File Offset: 0x00178BC0
	private static OrdealBase CreateRandomDusk()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			return new BugDuskOrdeal();
		}
		if (num == 1)
		{
			return new CircusDuskOrdeal();
		}
		if (num != 2)
		{
			return new CircusDuskOrdeal();
		}
		return new MachineDuskOrdeal();
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x0017AA08 File Offset: 0x00178C08
	private static OrdealBase CreateRandomMidnight()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			return new BugMidnightOrdeal();
		}
		if (num == 1)
		{
			return new MachineMidnightOrdeal();
		}
		if (num != 2)
		{
			return new OutterGodMidnightOrdeal();
		}
		return new OutterGodMidnightOrdeal();
	}

	// Token: 0x06003C9D RID: 15517 RVA: 0x0003554D File Offset: 0x0003374D
	private static OrdealBase CreateFixerOrdeal(OrdealLevel level)
	{
		return new FixerOrdeal(level);
	}

	// Token: 0x06003C9E RID: 15518 RVA: 0x0017AA50 File Offset: 0x00178C50
	public static List<OrdealBase> GenerateOrdeals(int day)
	{
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
			list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.DAWN));
			list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.NOON));
			list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.DUSK));
			list.Add(OrdealGenInfo.CreateFixerOrdeal(OrdealLevel.MIDNIGHT));
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
}
