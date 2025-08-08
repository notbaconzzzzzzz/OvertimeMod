using System;

// Token: 0x02000677 RID: 1655
public class PorccuWeaponDebuf : UnitBuf
{
	// Token: 0x06003675 RID: 13941 RVA: 0x00030FA1 File Offset: 0x0002F1A1
	public PorccuWeaponDebuf()
	{
		this.tickTimer.StartTimer(_tickTime);
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.type = UnitBufType.PORCCU_WEAPON;
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x000307DE File Offset: 0x0002E9DE
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.remainTime = _remain;
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x00162174 File Offset: 0x00160374
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

	// Token: 0x06003678 RID: 13944 RVA: 0x000232FA File Offset: 0x000214FA
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x0400325B RID: 12891
	private const float _remain = 5f;

	// Token: 0x0400325C RID: 12892
	private Timer tickTimer = new Timer();

	// Token: 0x0400325D RID: 12893
	private const float _tickTime = 1f;

	// Token: 0x0400325E RID: 12894
	private const float _dmg = 2f;

	// Token: 0x0400325F RID: 12895
	private RwbpType _dmgType = RwbpType.W;
}
