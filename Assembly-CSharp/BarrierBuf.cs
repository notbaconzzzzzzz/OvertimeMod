/*
public float UseBarrier(RwbpType dmgRwbpType, float damage) // 
*/
using System;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
public class BarrierBuf : UnitBuf
{
	// Token: 0x060056E3 RID: 22243 RVA: 0x001F0494 File Offset: 0x001EE694
	public BarrierBuf(RwbpType type, float barrierValue, float time)
	{
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		switch (type)
		{
		case RwbpType.R:
			this.type = UnitBufType.BARRIER_R;
			break;
		case RwbpType.W:
			this.type = UnitBufType.BARRIER_W;
			break;
		case RwbpType.B:
			this.type = UnitBufType.BARRIER_B;
			break;
		case RwbpType.P:
			this.type = UnitBufType.BARRIER_P;
			break;
		case RwbpType.A:
			this.type = UnitBufType.BARRIER_ALL;
			break;
		}
		this._rwbpType = type;
		this._barrierValueMax = barrierValue;
		this._barrierValue = barrierValue;
		this.remainTime = time;
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x060056E4 RID: 22244 RVA: 0x0004605F File Offset: 0x0004425F
	private float Rate
	{
		get
		{
			if (this.maxTime == 0f)
			{
				return 1f;
			}
			return this.remainTime / this.maxTime;
		}
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x001F0534 File Offset: 0x001EE734
	public override void Init(UnitModel model)
	{
		base.Init(model);
		WorkerModel workerModel = model as WorkerModel;
		if (workerModel == null)
		{
			Debug.LogError("model must be WorkerModel");
		}
		else
		{
			this.barrierEffect = BarrierEffect.MakeBarrier(workerModel);
			this.barrierEffect.SetRemainRate(1f);
			this.barrierEffect.SetRwbpType(this._rwbpType);
		}
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x001F0594 File Offset: 0x001EE794
	public float UseBarrier(RwbpType dmgRwbpType, float damage)
	{ // <Mod>
		if (this._barrierValue <= 0f)
		{
			return damage;
		}
		if (dmgRwbpType != this._rwbpType && this._rwbpType != RwbpType.A)
		{
			return damage;
		}
		if (this._barrierValue > damage)
		{
			this._barrierValue -= damage;
			if (this._barrierValue * 2f < this._barrierValueMax)
			{
				this.OnCrackBarrier();
			}
            Notice.instance.Send(NoticeName.BlockDamageByShield, new object[]
            {
                model,
                damage,
                (int)dmgRwbpType
            });
			return 0f;
		}
        float factor = 0f;
        if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_shield_bullet"))
        {
            factor = _barrierValue / damage;
        }
		_barrierValue *= factor;
		this.Destroy();
        Notice.instance.Send(NoticeName.BlockDamageByShield, new object[]
        {
            model,
            _barrierValue,
            (int)dmgRwbpType
        });
		return damage - this._barrierValue;
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x00046084 File Offset: 0x00044284
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.barrierEffect.SetRemainRate(this.Rate);
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x0004609D File Offset: 0x0004429D
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.remainTime <= 0f)
		{
			this.barrierEffect.OnDisappear();
		}
		else
		{
			this.barrierEffect.OnBreak();
		}
	}

	// Token: 0x060056E9 RID: 22249 RVA: 0x000460D0 File Offset: 0x000442D0
	public void OnCrackBarrier()
	{
		this.barrierEffect.OnCrack();
	}

	// Token: 0x04005037 RID: 20535
	private BarrierEffect barrierEffect;

	// Token: 0x04005038 RID: 20536
	private float maxTime;

	// Token: 0x04005039 RID: 20537
	private RwbpType _rwbpType;

	// Token: 0x0400503A RID: 20538
	public float _barrierValueMax; // changed from private to public

	// Token: 0x0400503B RID: 20539
	public float _barrierValue; // changed from private to public
}
