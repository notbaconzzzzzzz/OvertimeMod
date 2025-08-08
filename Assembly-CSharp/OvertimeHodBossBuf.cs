using System;
using UnityEngine;

public class OvertimeHodBossBuf : UnitBuf
{
	public OvertimeHodBossBuf()
	{
		type = UnitBufType.OVERTIME_HODBOSS;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		if (model is AgentModel)
		{
			agent = (model as AgentModel);
			originalStat = new WorkerPrimaryStat()
			{
				hp = agent.primaryStat.hp,
				mental = agent.primaryStat.mental,
				work = agent.primaryStat.work,
				battle = agent.primaryStat.battle
			};
		}
	}

	public override void FixedUpdate()
	{
		if (!lateInit)
		{
			LateInit();
			lateInit = true;
		}
	}

	private void LateInit()
	{
		if (this.agent == null)
		{
			return;
		}
        if (agent.hp > (float)agent.maxHp)
        {
            agent.hp = (float)agent.maxHp;
        }
        if (agent.mental > (float)agent.maxMental)
        {
            agent.mental = (float)agent.maxMental;
        }
		agent.GetUnit().agentUI.Init(agent);
	}

	public void ReduceStat(float factor, int min)
	{
		WorkerPrimaryStat primaryStat = agent.primaryStat;
		WorkerUnit workerUnit = agent.GetWorkerUnit();
		if (primaryStat.hp > min)
		{
			R_Exp -= (float)(primaryStat.hp - min) * factor;
			primaryStat.hp += Mathf.RoundToInt(R_Exp);
			R_Exp -= Mathf.Round(R_Exp);
			if (primaryStat.hp < 15)
			{
				primaryStat.hp = 15;
				R_Exp = 0f;
			}
			if (workerUnit != null) StatSubtractionEffect.MakeEffect(RwbpType.R, workerUnit.animRoot);
		}
		if (primaryStat.mental > min)
		{
			W_Exp -= (float)(primaryStat.mental - min) * factor;
			primaryStat.mental += Mathf.RoundToInt(W_Exp);
			W_Exp -= Mathf.Round(W_Exp);
			if (primaryStat.mental < 15)
			{
				primaryStat.mental = 15;
				W_Exp = 0f;
			}
			if (workerUnit != null) StatSubtractionEffect.MakeEffect(RwbpType.W, workerUnit.animRoot);
		}
		if (primaryStat.work > min)
		{
			B_Exp -= (float)(primaryStat.work - min) * factor;
			primaryStat.work += Mathf.RoundToInt(B_Exp);
			B_Exp -= Mathf.Round(B_Exp);
			if (primaryStat.work < 15)
			{
				primaryStat.work = 15;
				B_Exp = 0f;
			}
			if (workerUnit != null) StatSubtractionEffect.MakeEffect(RwbpType.B, workerUnit.animRoot);
		}
		if (primaryStat.battle > min)
		{
			P_Exp -= (float)(primaryStat.battle - min) * factor;
			primaryStat.battle += Mathf.RoundToInt(P_Exp);
			P_Exp -= Mathf.Round(P_Exp);
			if (primaryStat.battle < 15)
			{
				primaryStat.battle = 15;
				P_Exp = 0f;
			}
			if (workerUnit != null) StatSubtractionEffect.MakeEffect(RwbpType.P, workerUnit.animRoot);
		}
		lateInit = false;
	}

	public void GainExp(float val, RwbpType rwbpType)
	{
        bool increased = false;
		WorkerPrimaryStat primaryStat = agent.primaryStat;
		int dif;
		switch (rwbpType)
		{
			case RwbpType.R:
				dif = originalStat.hp - primaryStat.hp;
				if (dif > 0 || (dif == 0 && R_Exp < 0f))
				{
					if (R_Exp + val * 2f > (float)dif)
					{
						val -= ((float)dif - R_Exp) / 2f;
						R_Exp = (float)dif;
						R_Exp += val / 2f;
					}
					else
					{
						R_Exp += val * 2f;
					}
				}
				else
				{
					R_Exp += val / 2f;
				}
                dif = primaryStat.hp;
				primaryStat.hp += Mathf.RoundToInt(R_Exp);
				R_Exp -= Mathf.Round(R_Exp);
				if (primaryStat.hp > WorkerPrimaryStat.MaxStatR())
				{
					primaryStat.hp = WorkerPrimaryStat.MaxStatR();
					R_Exp = 0f;
				}
                if (primaryStat.hp > dif)
                {
                    increased = true;
                }
				break;
			case RwbpType.W:
				dif = originalStat.mental - primaryStat.mental;
				if (dif > 0 || (dif == 0 && W_Exp < 0f))
				{
					if (W_Exp + val * 2f > (float)dif)
					{
						val -= ((float)dif - W_Exp) / 2f;
						W_Exp = (float)dif;
						W_Exp += val / 2f;
					}
					else
					{
						W_Exp += val * 2f;
					}
				}
				else
				{
					W_Exp += val / 2f;
				}
                dif = primaryStat.mental;
				primaryStat.mental += Mathf.RoundToInt(W_Exp);
				W_Exp -= Mathf.Round(W_Exp);
				if (primaryStat.mental > WorkerPrimaryStat.MaxStatW())
				{
					primaryStat.mental = WorkerPrimaryStat.MaxStatW();
					W_Exp = 0f;
				}
                if (primaryStat.mental > dif)
                {
                    increased = true;
                }
				break;
			case RwbpType.B:
				dif = originalStat.work - primaryStat.work;
				if (dif > 0 || (dif == 0 && B_Exp < 0f))
				{
					if (B_Exp + val * 2f > (float)dif)
					{
						val -= ((float)dif - B_Exp) / 2f;
						B_Exp = (float)dif;
						B_Exp += val / 2f;
					}
					else
					{
						B_Exp += val * 2f;
					}
				}
				else
				{
					B_Exp += val / 2f;
				}
                dif = primaryStat.work;
				primaryStat.work += Mathf.RoundToInt(B_Exp);
				B_Exp -= Mathf.Round(B_Exp);
				if (primaryStat.work > WorkerPrimaryStat.MaxStatB())
				{
					primaryStat.work = WorkerPrimaryStat.MaxStatB();
					B_Exp = 0f;
				}
                if (primaryStat.work > dif)
                {
                    increased = true;
                }
				break;
			case RwbpType.P:
				dif = originalStat.battle - primaryStat.battle;
				if (dif > 0 || (dif == 0 && P_Exp < 0f))
				{
					if (P_Exp + val * 2f > (float)dif)
					{
						val -= ((float)dif - P_Exp) / 2f;
						P_Exp = (float)dif;
						P_Exp += val / 2f;
					}
					else
					{
						P_Exp += val * 2f;
					}
				}
				else
				{
					P_Exp += val / 2f;
				}
                dif = primaryStat.battle;
				primaryStat.battle += Mathf.RoundToInt(P_Exp);
				P_Exp -= Mathf.Round(P_Exp);
				if (primaryStat.battle > WorkerPrimaryStat.MaxStatP())
				{
					primaryStat.battle = WorkerPrimaryStat.MaxStatP();
					P_Exp = 0f;
				}
                if (primaryStat.battle > dif)
                {
                    increased = true;
                }
				break;
		}
        if (increased)
        {
            StatAdditionEffect.MakeEffect(rwbpType, agent.GetWorkerUnit().animRoot);
        }
		lateInit = false;
	}

    public void Reset()
    {
        if (reset) return;
		WorkerPrimaryStat primaryStat = agent.primaryStat;
        primaryStat.hp = originalStat.hp;
        primaryStat.mental = originalStat.mental;
        primaryStat.work = originalStat.work;
        primaryStat.battle = originalStat.battle;
        reset = true;
    }

    public override void OnDestroy()
    {
        Reset();
        base.OnDestroy();
    }

    public float R_Exp;
    public float W_Exp;
    public float B_Exp;
    public float P_Exp;

	private bool lateInit;
	private bool reset;

	private AgentModel agent;

	public WorkerPrimaryStat originalStat;
}
