using System;
using System.Collections.Generic;

// Token: 0x02000BC5 RID: 3013
public class WorkerManager
{
	// Token: 0x06005C19 RID: 23577 RVA: 0x000496D4 File Offset: 0x000478D4
	public WorkerManager()
	{
		this.workerList = new List<WorkerModel>();
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06005C1A RID: 23578 RVA: 0x000496EF File Offset: 0x000478EF
	public static WorkerManager instance
	{
		get
		{
			if (WorkerManager._instance == null)
			{
				WorkerManager._instance = new WorkerManager();
			}
			return WorkerManager._instance;
		}
	}

	// Token: 0x06005C1B RID: 23579 RVA: 0x0004970A File Offset: 0x0004790A
	public void AddWorker(WorkerModel worker)
	{
		this.workerList.Add(worker);
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x00049718 File Offset: 0x00047918
	public void RemoveWorker(WorkerModel worker)
	{
		this.workerList.Remove(worker);
	}

	// Token: 0x06005C1D RID: 23581 RVA: 0x00215798 File Offset: 0x00213998
	public void RemoveWorkers(WorkerModel[] workers)
	{
		foreach (WorkerModel worker in workers)
		{
			this.RemoveWorker(worker);
		}
	}

	// Token: 0x06005C1E RID: 23582 RVA: 0x002157C8 File Offset: 0x002139C8
	public List<WorkerModel> GetWorkerList()
	{
		List<WorkerModel> list = new List<WorkerModel>(AgentManager.instance.GetAgentList().Count + OfficerManager.instance.GetOfficerList().Count);
		foreach (AgentModel item in AgentManager.instance.GetAgentList())
		{
			list.Add(item);
		}
		foreach (OfficerModel item2 in OfficerManager.instance.GetOfficerList())
		{
			list.Add(item2);
		}
		return list;
	}

    // <Mod>
    public int EmployeePower
    {
		get
		{
			int num = 0;
			if (GameManager.currentGameManager.state == GameState.STOP)
			{
				foreach (Sefira sefira in SefiraManager.instance.GetOpendSefiraList())
				{
					for (int i = 0; i < sefira.openLevel * 2; i++)
					{
						int power = 1;
						power *= EPLevelFactor[0];
						power *= EPEgoFactor[0];
						num += power;
					}
				}
			}
			else
			{
				foreach (OfficerModel officer in OfficerManager.instance.GetOfficerList())
				{
					if (officer.IsDead()) continue;
					int power = 1;
					power *= EPLevelFactor[0];
					power *= EPEgoFactor[0];
					if (officer.IsCrazy())
					{
						num += power / 2;
						continue;
					}
					num += power;
				}
			}
			foreach (AgentModel agent in AgentManager.instance.GetAgentList())
			{
				if (agent.IsDead()) continue;
				int power = 1;
				int agentLevel = agent.level;
				EGObonusInfo bonus = agent.GetEGObonus();
				if (agent.originFortitudeStat + bonus.hp + agent.originPrudenceStat + bonus.mental + agent.originTemperanceStat + (bonus.workProb + bonus.cubeSpeed) / 2 + agent.originJusticeStat + (bonus.attackSpeed + bonus.movement) / 2 >= 400)
				{
					agentLevel = 6;
				}
				if (agentLevel < 0) agentLevel = 0;
				else if (agentLevel >= EPLevelFactor.Length) agentLevel = EPLevelFactor.Length - 1;
				power *= EPLevelFactor[agentLevel];
				int egoTotal = 1;
				if (agent.Equipment.weapon == null) egoTotal += 0;
				else egoTotal += agent.Equipment.weapon.GetUpgradeRisk;
				if (agent.Equipment.armor == null) egoTotal += 0;
				else egoTotal += agent.Equipment.armor.GetUpgradeRisk;
				if (egoTotal < 0) egoTotal = 0;
				else if (egoTotal >= EPEgoFactor.Length) egoTotal = EPEgoFactor.Length - 1;
				power *= EPEgoFactor[egoTotal];
				if (agent.IsCrazy())
				{
					num += power / 2;
					continue;
				}
				num += power;
			}
			return num;
		}
    }

	// Token: 0x0400542D RID: 21549
	private static WorkerManager _instance;

	// Token: 0x0400542E RID: 21550
	private long nextInstId = 1L;

	// Token: 0x0400542F RID: 21551
	private List<WorkerModel> workerList;

	// Token: 0x04005430 RID: 21552
	private AgentManager agentManager;

	// Token: 0x04005431 RID: 21553
	private OfficerManager officerManager;

	// <Mod>
	public static int[] EPLevelFactor = new int[] {
		1,
		10,
		16,
		25,
		40,
		64,
		100
	};

	// <Mod>
	public static int[] EPEgoFactor = new int[] {
		8,
		12,
		20,
		25,
		32,
		40,
		50,
		64,
		80,
		100,
		128,
		160,
		200
	};
}
