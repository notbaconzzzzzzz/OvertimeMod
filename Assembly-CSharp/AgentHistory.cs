using System;
using System.Collections.Generic;

// Token: 0x02000331 RID: 817
public class AgentHistory
{
	// Token: 0x0600191C RID: 6428 RVA: 0x0001C716 File Offset: 0x0001A916
	public AgentHistory()
	{
		this.workDay = 0;
		this.oneday = new AgentHistory.History();
		this.total = new AgentHistory.History();
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600191D RID: 6429 RVA: 0x0001C73B File Offset: 0x0001A93B
	// (set) Token: 0x0600191E RID: 6430 RVA: 0x0001C743 File Offset: 0x0001A943
	private int workDay { get; set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600191F RID: 6431 RVA: 0x0001C74C File Offset: 0x0001A94C
	// (set) Token: 0x06001920 RID: 6432 RVA: 0x0001C754 File Offset: 0x0001A954
	private AgentHistory.History oneday { get; set; }

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06001921 RID: 6433 RVA: 0x0001C75D File Offset: 0x0001A95D
	// (set) Token: 0x06001922 RID: 6434 RVA: 0x0001C765 File Offset: 0x0001A965
	private AgentHistory.History total { get; set; }

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06001923 RID: 6435 RVA: 0x0001C76E File Offset: 0x0001A96E
	public int WorkDay
	{
		get
		{
			return this.workDay;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06001924 RID: 6436 RVA: 0x0001C776 File Offset: 0x0001A976
	public AgentHistory.History Oneday
	{
		get
		{
			return this.oneday;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06001925 RID: 6437 RVA: 0x0001C77E File Offset: 0x0001A97E
	public AgentHistory.History Total
	{
		get
		{
			return this.total;
		}
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x0001C786 File Offset: 0x0001A986
	public void AddWorkDay()
	{
		this.workDay++;
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.WorkDay);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.WorkDay);
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0001C7B6 File Offset: 0x0001A9B6
	public void AddWorkSuccess()
	{
		this.oneday.AddWorkSuccess();
		this.total.AddWorkSuccess();
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Success);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Success);
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x0001C7EE File Offset: 0x0001A9EE
	public void AddWorkFail()
	{
		this.oneday.AddWorkFail();
		this.total.AddWorkFail();
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Fail);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Fail);
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x0001C826 File Offset: 0x0001AA26
	public void PhysicalDamage(float damage)
	{
		this.oneday.PhysicalDamage((int)damage);
		this.total.PhysicalDamage((int)damage);
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Phyiscal);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Phyiscal);
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x0001C862 File Offset: 0x0001AA62
	public void MentalDamage(float damage)
	{
		this.oneday.MentalDamage((int)damage);
		this.total.MentalDamage((int)damage);
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Mental);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Mental);
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0001C89E File Offset: 0x0001AA9E
	public void WitnessDeathByCreature()
	{
		this.oneday.WitnessDeathByCreature();
		this.total.WitnessDeathByCreature();
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x0001C8B6 File Offset: 0x0001AAB6
	public void WitnessPanicByCreature()
	{
		this.oneday.WitnessPanicByCreature();
		this.total.WitnessPanicByCreature();
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x0001C8CE File Offset: 0x0001AACE
	public void WitnessDeathByWorker()
	{
		this.oneday.WitnessDeathByWorker();
		this.total.WitnessPanicByCreature();
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x0001C8E6 File Offset: 0x0001AAE6
	public void AddPanic()
	{
		this.oneday.AddPanic();
		this.total.AddPanic();
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.PanicCnt);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.PanicCnt);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x0001C91E File Offset: 0x0001AB1E
	public void CreatureAttack(int damage)
	{
		this.oneday.CreatureAttack(damage);
		this.total.CreatureAttack(damage);
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x0001C938 File Offset: 0x0001AB38
	public void WorkerAttack(int damage)
	{
		this.oneday.WorkerAttack(damage);
		this.total.WorkerAttack(damage);
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x0001C952 File Offset: 0x0001AB52
	public void PanicWorkerAttack(int damage)
	{
		this.oneday.PanicWorkerAttack(damage);
		this.total.PanicWorkerAttack(damage);
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x0001C96C File Offset: 0x0001AB6C
	public void Suppress(int damage)
	{
		this.oneday.Suppress(damage);
		this.total.Suppress(damage);
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0001C986 File Offset: 0x0001AB86
	public void SuccessCreatureSuppress()
	{
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Suppresss);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Suppresss);
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0001C986 File Offset: 0x0001AB86
	public void SuccessWorkerSuppress()
	{
		this.oneday.PromotionValAdd(AgentHistory.PromotionValScore.Suppresss);
		this.total.PromotionValAdd(AgentHistory.PromotionValScore.Suppresss);
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x0001C9A8 File Offset: 0x0001ABA8
	public void Disposition()
	{
		this.oneday.Disposition();
		this.total.Disposition();
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x0001C9C0 File Offset: 0x0001ABC0
	public void AddWorkCubeCount(RwbpType type, int val)
	{
		this.oneday.AddWorkCount(type, val);
		this.total.AddWorkCount(type, val);
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x0001C9DC File Offset: 0x0001ABDC
	public void EndOneDay()
	{
		this.oneday.Clear();
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000DCFA4 File Offset: 0x000DB1A4
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"historyworkDay",
				this.workDay
			},
			{
				"workSuccess",
				this.total.workSuccess
			},
			{
				"workFail",
				this.total.workFail
			},
			{
				"physicalDamage",
				this.total.takePhysicalDamage
			},
			{
				"mentalDamage",
				this.total.takeMentalDamage
			},
			{
				"deathByCreature",
				this.total.deathByCreature
			},
			{
				"panicByCreature",
				this.total.panicByCreature
			},
			{
				"deathByWorker",
				this.total.deathByWorker
			},
			{
				"panic",
				this.total.panic
			},
			{
				"creatureDamage",
				this.total.creatureDamage
			},
			{
				"workerDamage",
				this.total.workerDamage
			},
			{
				"panicWorkerDamage",
				this.total.panicWorkerDamage
			},
			{
				"suppressDamage",
				this.total.suppressDamage
			},
			{
				"disposition",
				this.total.disposal
			},
			{
				"promotionVal",
				this.total.promotionVal
			},
			{
				"workCubeCounts",
				this.total.workCubeCounts
			}
		};
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000DD160 File Offset: 0x000DB360
	public void LoadData(Dictionary<string, object> dic)
	{
		int[] array = new int[15];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 0;
		}
		GameUtil.TryGetValue<int>(dic, "workDay", ref array[0]);
		GameUtil.TryGetValue<int>(dic, "workSuccess", ref array[1]);
		GameUtil.TryGetValue<int>(dic, "workFail", ref array[2]);
		GameUtil.TryGetValue<int>(dic, "physicalDamage", ref array[3]);
		GameUtil.TryGetValue<int>(dic, "mentalDamage", ref array[4]);
		GameUtil.TryGetValue<int>(dic, "deathByCreature", ref array[5]);
		GameUtil.TryGetValue<int>(dic, "panicByCreature", ref array[6]);
		GameUtil.TryGetValue<int>(dic, "deathByWorker", ref array[7]);
		GameUtil.TryGetValue<int>(dic, "panic", ref array[8]);
		GameUtil.TryGetValue<int>(dic, "creatureDamage", ref array[9]);
		GameUtil.TryGetValue<int>(dic, "workerDamage", ref array[10]);
		GameUtil.TryGetValue<int>(dic, "panicWorkerDamage", ref array[11]);
		GameUtil.TryGetValue<int>(dic, "suppressDamage", ref array[12]);
		GameUtil.TryGetValue<int>(dic, "disposition", ref array[13]);
		GameUtil.TryGetValue<int>(dic, "promotionVal", ref array[14]);
		GameUtil.TryGetValue<Dictionary<RwbpType, int>>(dic, "workCubeCounts", ref this.total.workCubeCounts);
		this.workDay = array[0];
		this.total.workSuccess = array[1];
		this.total.workFail = array[2];
		this.total.takePhysicalDamage = array[3];
		this.total.takeMentalDamage = array[4];
		this.total.deathByCreature = array[5];
		this.total.panicByCreature = array[6];
		this.total.deathByWorker = array[7];
		this.total.panic = array[8];
		this.total.creatureDamage = array[9];
		this.total.workerDamage = array[10];
		this.total.panicWorkerDamage = array[11];
		this.total.suppressDamage = array[12];
		this.total.disposal = array[13];
		this.total.promotionVal = array[14];
	}

	// Token: 0x02000332 RID: 818
	public class History
	{
		// Token: 0x0600193A RID: 6458 RVA: 0x0001C9E9 File Offset: 0x0001ABE9
		public History()
		{
			this.Clear();
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x0001CA02 File Offset: 0x0001AC02
		// (set) Token: 0x0600193C RID: 6460 RVA: 0x0001CA0A File Offset: 0x0001AC0A
		public int workSuccess { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x0001CA13 File Offset: 0x0001AC13
		// (set) Token: 0x0600193E RID: 6462 RVA: 0x0001CA1B File Offset: 0x0001AC1B
		public int workFail { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x0001CA24 File Offset: 0x0001AC24
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x0001CA2C File Offset: 0x0001AC2C
		public int takePhysicalDamage { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x0001CA35 File Offset: 0x0001AC35
		// (set) Token: 0x06001942 RID: 6466 RVA: 0x0001CA3D File Offset: 0x0001AC3D
		public int takeMentalDamage { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x0001CA46 File Offset: 0x0001AC46
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x0001CA4E File Offset: 0x0001AC4E
		public int deathByCreature { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x0001CA57 File Offset: 0x0001AC57
		// (set) Token: 0x06001946 RID: 6470 RVA: 0x0001CA5F File Offset: 0x0001AC5F
		public int panicByCreature { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x0001CA68 File Offset: 0x0001AC68
		// (set) Token: 0x06001948 RID: 6472 RVA: 0x0001CA70 File Offset: 0x0001AC70
		public int deathByWorker { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x0001CA79 File Offset: 0x0001AC79
		// (set) Token: 0x0600194A RID: 6474 RVA: 0x0001CA81 File Offset: 0x0001AC81
		public int panic { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x0001CA8A File Offset: 0x0001AC8A
		// (set) Token: 0x0600194C RID: 6476 RVA: 0x0001CA92 File Offset: 0x0001AC92
		public int creatureDamage { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x0001CA9B File Offset: 0x0001AC9B
		// (set) Token: 0x0600194E RID: 6478 RVA: 0x0001CAA3 File Offset: 0x0001ACA3
		public int workerDamage { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x0001CAAC File Offset: 0x0001ACAC
		// (set) Token: 0x06001950 RID: 6480 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		public int panicWorkerDamage { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x0001CABD File Offset: 0x0001ACBD
		// (set) Token: 0x06001952 RID: 6482 RVA: 0x0001CAC5 File Offset: 0x0001ACC5
		public int suppressDamage { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x0001CACE File Offset: 0x0001ACCE
		// (set) Token: 0x06001954 RID: 6484 RVA: 0x0001CAD6 File Offset: 0x0001ACD6
		public int disposal { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06001955 RID: 6485 RVA: 0x0001CADF File Offset: 0x0001ACDF
		// (set) Token: 0x06001956 RID: 6486 RVA: 0x0001CAE7 File Offset: 0x0001ACE7
		public int promotionVal { get; set; }

		// Token: 0x06001957 RID: 6487 RVA: 0x000DD39C File Offset: 0x000DB59C
		public void Clear()
		{
			this.workSuccess = 0;
			this.workFail = 0;
			this.takePhysicalDamage = 0;
			this.takeMentalDamage = 0;
			this.deathByCreature = 0;
			this.panicByCreature = 0;
			this.deathByWorker = 0;
			this.panic = 0;
			this.creatureDamage = 0;
			this.workerDamage = 0;
			this.panicWorkerDamage = 0;
			this.suppressDamage = 0;
			this.disposal = 0;
			this.panic = 0;
			this.promotionVal = 0;
			this.workCubeCounts.Clear();
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
		public void AddWorkSuccess()
		{
			this.workSuccess++;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0001CB00 File Offset: 0x0001AD00
		public void AddWorkFail()
		{
			this.workFail++;
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0001CB10 File Offset: 0x0001AD10
		public void PhysicalDamage(int damage)
		{
			this.takePhysicalDamage += damage;
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0001CB20 File Offset: 0x0001AD20
		public void MentalDamage(int damage)
		{
			this.takeMentalDamage += damage;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0001CB30 File Offset: 0x0001AD30
		public void WitnessDeathByCreature()
		{
			this.deathByCreature++;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0001CB40 File Offset: 0x0001AD40
		public void WitnessPanicByCreature()
		{
			this.panicByCreature++;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0001CB50 File Offset: 0x0001AD50
		public void WitnessDeathByWorker()
		{
			this.deathByWorker++;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0001CB60 File Offset: 0x0001AD60
		public void AddPanic()
		{
			this.panic++;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0001CB70 File Offset: 0x0001AD70
		public void CreatureAttack(int damage)
		{
			this.creatureDamage += damage;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0001CB80 File Offset: 0x0001AD80
		public void WorkerAttack(int damage)
		{
			this.workerDamage += damage;
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0001CB90 File Offset: 0x0001AD90
		public void PanicWorkerAttack(int damage)
		{
			this.panicWorkerDamage += damage;
			this.WorkerAttack(damage);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0001CBA7 File Offset: 0x0001ADA7
		public void Suppress(int damage)
		{
			this.suppressDamage += damage;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0001CBB7 File Offset: 0x0001ADB7
		public void Disposition()
		{
			this.disposal++;
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0001CBC7 File Offset: 0x0001ADC7
		public void WorkResult(bool result)
		{
			if (result)
			{
				this.workSuccess++;
			}
			else
			{
				this.workFail++;
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
		public void PromotionValAdd(int val)
		{
			this.promotionVal += val;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0001CC00 File Offset: 0x0001AE00
		public void PromotionValReset()
		{
			this.promotionVal = 0;
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000DD420 File Offset: 0x000DB620
		public void AddWorkCount(RwbpType type, int val)
		{
			if (this.workCubeCounts.ContainsKey(type))
			{
				Dictionary<RwbpType, int> dictionary;
				(dictionary = this.workCubeCounts)[type] = dictionary[type] + val;
			}
			else
			{
				this.workCubeCounts[type] = val;
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000DD46C File Offset: 0x000DB66C
		public int GetWorkCount(RwbpType type)
		{
			int result;
			if (this.workCubeCounts.TryGetValue(type, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x04001A18 RID: 6680
		public Dictionary<RwbpType, int> workCubeCounts = new Dictionary<RwbpType, int>();
	}

	// Token: 0x02000333 RID: 819
	private class PromotionValScore
	{
		// Token: 0x0600196A RID: 6506 RVA: 0x000042F0 File Offset: 0x000024F0
		public PromotionValScore()
		{
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0001CC09 File Offset: 0x0001AE09
		// Note: this type is marked as 'beforefieldinit'.
		static PromotionValScore()
		{
		}

		// Token: 0x04001A1A RID: 6682
		public static int Success = 2;

		// Token: 0x04001A1B RID: 6683
		public static int Fail = 1;

		// Token: 0x04001A1C RID: 6684
		public static int WorkDay = 3;

		// Token: 0x04001A1D RID: 6685
		public static int PanicCnt = 4;

		// Token: 0x04001A1E RID: 6686
		public static int Phyiscal = 1;

		// Token: 0x04001A1F RID: 6687
		public static int Mental = 1;

		// Token: 0x04001A20 RID: 6688
		public static int Suppresss = 6;
	}

	// Token: 0x02000334 RID: 820
	public class PromotionNeedVal
	{
		// Token: 0x0600196C RID: 6508 RVA: 0x000042F0 File Offset: 0x000024F0
		public PromotionNeedVal()
		{
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0001CC35 File Offset: 0x0001AE35
		public static int GetNeedVal(int level)
		{
			switch (level)
			{
			case 1:
				return AgentHistory.PromotionNeedVal.level1;
			case 2:
				return AgentHistory.PromotionNeedVal.level2;
			case 3:
				return AgentHistory.PromotionNeedVal.level3;
			case 4:
				return AgentHistory.PromotionNeedVal.level4;
			default:
				return int.MaxValue;
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0001CC71 File Offset: 0x0001AE71
		// Note: this type is marked as 'beforefieldinit'.
		static PromotionNeedVal()
		{
		}

		// Token: 0x04001A21 RID: 6689
		public static int level1 = 10;

		// Token: 0x04001A22 RID: 6690
		public static int level2 = 30;

		// Token: 0x04001A23 RID: 6691
		public static int level3 = 55;

		// Token: 0x04001A24 RID: 6692
		public static int level4 = 85;
	}
}
