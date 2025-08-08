using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public class ResetMirror : CreatureBase
{
	// Token: 0x06002873 RID: 10355 RVA: 0x00027FBD File Offset: 0x000261BD
	public ResetMirror()
	{
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x00027FDB File Offset: 0x000261DB
	public override void OnStageStart()
	{
		base.OnStageStart();
		this._alreadyUseAgents.Clear();
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x00119FF0 File Offset: 0x001181F0
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		skill.PauseWorking();
		this._workPauseTimer.StartTimer(2f);
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/ResetMirror/StatRandomizeEffect");
		gameObject.transform.localPosition = skill.agent.GetCurrentViewPosition() + new Vector3(0f, 0f, -5f);
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x00027FEE File Offset: 0x000261EE
	public override void OnFixedUpdateInSkill(UseSkill skill)
	{
		base.OnFixedUpdateInSkill(skill);
		if (this._workPauseTimer.RunTimer())
		{
			skill.ResumeWorking();
		}
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x0011A054 File Offset: 0x00118254
	public override void OnFinishWork(UseSkill skill)
	{ // <Mod>
		base.OnFinishWork(skill);
		int num = WorkerPrimaryStat.MaxStatR();
		int num2 = WorkerPrimaryStat.MaxStatW();
		int num3 = WorkerPrimaryStat.MaxStatB();
		int num4 = WorkerPrimaryStat.MaxStatP();
		bool flag = this._alreadyUseAgents.Contains(skill.agent);
		WorkerPrimaryStat primaryStat = skill.agent.primaryStat;
		int num5 = primaryStat.hp + primaryStat.mental + primaryStat.work + primaryStat.battle;
		int maxHp = skill.agent.maxHp;
		int maxMental = skill.agent.maxMental;
		WorkerPrimaryStat workerPrimaryStat = new WorkerPrimaryStat();
		int num6 = 15;
		workerPrimaryStat.hp = num6;
		workerPrimaryStat.mental = num6;
		workerPrimaryStat.work = num6;
		workerPrimaryStat.battle = num6;
		int num7 = workerPrimaryStat.hp + workerPrimaryStat.mental + workerPrimaryStat.work + workerPrimaryStat.battle;
		int i = num5 - num7;
		if (flag)
		{
			i -= 20;
		}
		List<RwbpType> list = new List<RwbpType>(new RwbpType[]
		{
			RwbpType.R,
			RwbpType.W,
			RwbpType.B,
			RwbpType.P
		});
		while (i > 0)
		{
			if (list.Count == 0)
			{
				break;
			}
			RwbpType rwbpType = list[UnityEngine.Random.Range(0, list.Count)];
			int num8 = 0;
			int a = UnityEngine.Random.Range(1, Mathf.Min(num7 / 4, i) + 1);
			switch (rwbpType)
			{
			case RwbpType.R:
				num8 = Mathf.Min(a, num - workerPrimaryStat.hp);
				workerPrimaryStat.hp += num8;
				if (workerPrimaryStat.hp >= num)
				{
					list.Remove(RwbpType.R);
				}
				break;
			case RwbpType.W:
				num8 = Mathf.Min(a, num2 - workerPrimaryStat.mental);
				workerPrimaryStat.mental += num8;
				if (workerPrimaryStat.mental >= num2)
				{
					list.Remove(RwbpType.W);
				}
				break;
			case RwbpType.B:
				num8 = Mathf.Min(a, num3 - workerPrimaryStat.work);
				workerPrimaryStat.work += num8;
				if (workerPrimaryStat.work >= num3)
				{
					list.Remove(RwbpType.B);
				}
				break;
			case RwbpType.P:
				num8 = Mathf.Min(a, num4 - workerPrimaryStat.battle);
				workerPrimaryStat.battle += num8;
				if (workerPrimaryStat.battle >= num4)
				{
					list.Remove(RwbpType.P);
				}
				break;
			}
			i -= num8;
			if (num8 <= 0)
			{
				break;
			}
		}
		skill.agent.primaryStat = workerPrimaryStat;
		skill.agent.InitTitle();
		skill.agent.UpdateTitle(1, false);
		skill.agent.hp = skill.agent.hp * (float)skill.agent.maxHp / (float)maxHp;
		skill.agent.mental = skill.agent.mental * (float)skill.agent.maxMental / (float)maxMental;
		if (!flag)
		{
			this._alreadyUseAgents.Add(skill.agent);
		}
	}

	// Token: 0x040026DA RID: 9946
	public const string statResetEffect = "Effect/Creature/ResetMirror/StatRandomizeEffect";

	// Token: 0x040026DB RID: 9947
	private List<AgentModel> _alreadyUseAgents = new List<AgentModel>();

	// Token: 0x040026DC RID: 9948
	private Timer _workPauseTimer = new Timer();
}
