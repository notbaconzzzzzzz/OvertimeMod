using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006EB RID: 1771
public class AgentTitleTypeList
{
	// Token: 0x0600393F RID: 14655 RVA: 0x00033301 File Offset: 0x00031501
	public AgentTitleTypeList()
	{
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06003940 RID: 14656 RVA: 0x00033314 File Offset: 0x00031514
	public static AgentTitleTypeList instance
	{
		get
		{
			if (AgentTitleTypeList._instance == null)
			{
				AgentTitleTypeList._instance = new AgentTitleTypeList();
			}
			return AgentTitleTypeList._instance;
		}
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x0003332F File Offset: 0x0003152F
	public void Init(List<AgentTitleTypeInfo> list)
	{
		this._list = list;
	}

	// Token: 0x06003942 RID: 14658 RVA: 0x00033338 File Offset: 0x00031538
	public AgentTitleTypeInfo GetDataPrefix(AgentModel agent, int level, bool randomly = true)
	{
		return this.GetData(agent, level, "prefix", randomly);
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x00033348 File Offset: 0x00031548
	public AgentTitleTypeInfo GetDataSuffix(AgentModel agent, int level, bool randomly = true)
	{
		return this.GetData(agent, level, "suffix", randomly);
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x0016E6CC File Offset: 0x0016C8CC
	private AgentTitleTypeInfo GetData(AgentModel agent, int level, string pos, bool randomly = true)
	{
		string rwbp = "A";
		int hp = agent.primaryStat.hp;
		int mental = agent.primaryStat.mental;
		int work = agent.primaryStat.work;
		int battle = agent.primaryStat.battle;
		AgentTitleTypeList.Stat[] collection = new AgentTitleTypeList.Stat[]
		{
			new AgentTitleTypeList.Stat("R", hp),
			new AgentTitleTypeList.Stat("W", mental),
			new AgentTitleTypeList.Stat("B", work),
			new AgentTitleTypeList.Stat("P", battle)
		};
		List<AgentTitleTypeList.Stat> list = new List<AgentTitleTypeList.Stat>(collection);
		list.Sort(delegate(AgentTitleTypeList.Stat x, AgentTitleTypeList.Stat y)
		{
			if (x.stat < y.stat)
			{
				return 1;
			}
			if (x.stat > y.stat)
			{
				return -1;
			}
			return 0;
		});
		if (list[0].stat != list[1].stat)
		{
			rwbp = list[0].rwbp;
		}
		List<AgentTitleTypeInfo> list2 = this._list.FindAll((AgentTitleTypeInfo x) => x.level == level && x.pos == pos);
		List<AgentTitleTypeInfo> list3 = list2.FindAll((AgentTitleTypeInfo x) => rwbp == x.rwbp);
		if (list3.Count == 0)
		{
			list3 = list2.FindAll((AgentTitleTypeInfo x) => x.rwbp == "A");
		}
		if (list3.Count == 0)
		{
			return null;
		}
		if (randomly)
		{
			return list3[UnityEngine.Random.Range(0, list3.Count)];
		}
		return list3[(hp + mental + work + battle) % list3.Count];
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x0016E86C File Offset: 0x0016CA6C
	public AgentTitleTypeInfo GetData(int id)
	{
		return this._list.Find((AgentTitleTypeInfo x) => x.id == id);
	}

	// Token: 0x04003401 RID: 13313
	private static AgentTitleTypeList _instance;

	// Token: 0x04003402 RID: 13314
	private List<AgentTitleTypeInfo> _list = new List<AgentTitleTypeInfo>();

	// Token: 0x020006EC RID: 1772
	public class Stat
	{
		// Token: 0x06003948 RID: 14664 RVA: 0x00033393 File Offset: 0x00031593
		public Stat(string rwbp, int stat)
		{
			this.rwbp = rwbp;
			this.stat = stat;
		}

		// Token: 0x04003405 RID: 13317
		public string rwbp;

		// Token: 0x04003406 RID: 13318
		public int stat;
	}
}
