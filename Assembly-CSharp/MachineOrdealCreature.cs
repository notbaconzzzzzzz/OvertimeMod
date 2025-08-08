using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class MachineOrdealCreature : CreatureBase
{
	// Token: 0x060024B9 RID: 9401 RVA: 0x000256A3 File Offset: 0x000238A3
	public MachineOrdealCreature()
	{
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x060024BA RID: 9402 RVA: 0x000256C1 File Offset: 0x000238C1
	protected static float spawnByDuskTime
	{
		get
		{
			return UnityEngine.Random.Range(0.4f, 0.5f);
		}
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000256D2 File Offset: 0x000238D2
	public virtual string GetOrdealName()
	{
		return this._name;
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000256DA File Offset: 0x000238DA
	public virtual void SetOrdeal(MachineOrdeal ordealScript, OrdealLevel level, RiskLevel risk, string name)
	{
		this._ordealScript = ordealScript;
		this._level = level;
		this._risk = risk;
		this._name = name;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000256F9 File Offset: 0x000238F9
	public virtual void OnDie()
	{
		this._ordealScript.OnDie(this.model as OrdealCreatureModel);
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x060024BE RID: 9406 RVA: 0x00025711 File Offset: 0x00023911
	public RiskLevel Risk
	{
		get
		{
			return this._risk;
		}
	}

	// Token: 0x040023C4 RID: 9156
	protected MachineOrdeal _ordealScript;

	// Token: 0x040023C5 RID: 9157
	protected OrdealLevel _level;

	// Token: 0x040023C6 RID: 9158
	protected RiskLevel _risk;

	// Token: 0x040023C7 RID: 9159
	protected string _name = "Machine_dawn";

	// Token: 0x040023C8 RID: 9160
	protected Timer spawnByDuskTimer = new Timer();

	// Token: 0x040023C9 RID: 9161
	protected const float _spawnByDuskTimeMin = 0.4f;

	// Token: 0x040023CA RID: 9162
	protected const float _spawnByDuskTimeMax = 0.5f;

	// Token: 0x040023CB RID: 9163
	protected const float _spawnedSpeed = 15f;
}
