using System;

// Token: 0x02000682 RID: 1666
public class SlimeGirlWeaponDebuf : UnitBuf
{
	// Token: 0x060036A6 RID: 13990 RVA: 0x000313B4 File Offset: 0x0002F5B4
	public SlimeGirlWeaponDebuf()
	{
		this.tickTimer.StartTimer(_tickTime);
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.SLIMEGIRL_WEAPON;
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x00030A46 File Offset: 0x0002EC46
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = _remain;
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00162E4C File Offset: 0x0016104C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.model.hp <= 0f)
		{
			return;
		}
		if (this.tickTimer.RunTimer())
		{
			this.model.TakeDamage(new DamageInfo(this._dmgType, _dmg));
			this.tickTimer.StartTimer(_tickTime);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(this.model, this._dmgType, this.model.defense);
		}
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x0002357A File Offset: 0x0002177A
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x000313ED File Offset: 0x0002F5ED
	public override float MovementScale()
	{
		return 0.7f;
	}

	// Token: 0x04003286 RID: 12934
	private const float _remain = 5f;

	// Token: 0x04003287 RID: 12935
	private Timer tickTimer = new Timer();

	// Token: 0x04003288 RID: 12936
	private const float _tickTime = 1f;

	// Token: 0x04003289 RID: 12937
	private const float _dmg = 2f;

	// Token: 0x0400328A RID: 12938
	private RwbpType _dmgType = RwbpType.B;
}
