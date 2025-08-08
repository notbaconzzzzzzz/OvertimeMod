using System;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class MachineMidnight : MachineOrdealCreature
{
	// Token: 0x06003C5F RID: 15455 RVA: 0x000351BF File Offset: 0x000333BF
	public MachineMidnight()
	{
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x000351DD File Offset: 0x000333DD
	public MachineMidnightAnim AnimScript
	{
		get
		{
			return base.Unit.animTarget as MachineMidnightAnim;
		}
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x000351EF File Offset: 0x000333EF
	public override void OnInit()
	{
		base.OnInit();
		this.currentDamageCumlatived = 0f;
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00035202 File Offset: 0x00033402
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.movable.SetDirection(UnitDirection.RIGHT);
		this.AnimScript.SetScript(this);
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00179830 File Offset: 0x00177A30
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this._speedDownTimer.started && this._speedDownTimer.RunTimer())
		{
			this.AnimScript.SetRotationSpeed(0.2f);
		}
		if (this._resetTimer.started && this._resetTimer.RunTimer())
		{
			this.AnimScript.Restart();
		}
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00035223 File Offset: 0x00033423
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		this.currentDamageCumlatived += value;
		if (this.currentDamageCumlatived >= 405f)
		{
			this.currentDamageCumlatived = 0f;
			this.ActivateSpeedDown();
		}
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x001798A0 File Offset: 0x00177AA0
	private void ActivateSpeedDown()
	{
		if (!this._speedDownTimer.started)
		{
			this._speedDownTimer.StartTimer(10f);
		}
		else
		{
			this._speedDownTimer.StartTimer(Mathf.Clamp(10f + (this._speedDownTimer.maxTime - this._speedDownTimer.elapsed), 0f, 100f));
		}
		this.AnimScript.SetRotationSpeed(0.1f);
		this.AnimScript.ActivateSpark();
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x0003525D File Offset: 0x0003345D
	public void OnStartAttack()
	{
		this._rotationStarted = true;
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x00035266 File Offset: 0x00033466
	public void SetRotationSpeed(float rate)
	{
		this.AnimScript.SetRotationSpeed(rate);
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x00035274 File Offset: 0x00033474
	public override bool OnAfterSuppressed()
	{
		this._ordealScript.OrdealEnd();
		this._resetTimer.StopTimer();
		this.AnimScript.OnSuppressed();
		return true;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x00035298 File Offset: 0x00033498
	public void OnRotationEnd()
	{
		this._resetTimer.StartTimer(10f);
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00179924 File Offset: 0x00177B24
	public override SoundEffectPlayer MakeSound(string src)
	{
		SoundEffectPlayer soundEffectPlayer = base.MakeSound(src);
		soundEffectPlayer.transform.SetParent(this.AnimScript.transform);
		return soundEffectPlayer;
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00179950 File Offset: 0x00177B50
	public override SoundEffectPlayer MakeSoundLoop(string src)
	{
		SoundEffectPlayer soundEffectPlayer = base.MakeSoundLoop(src);
		soundEffectPlayer.transform.SetParent(this.AnimScript.transform);
		return soundEffectPlayer;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x000352AA File Offset: 0x000334AA
	// Note: this type is marked as 'beforefieldinit'.
	static MachineMidnight()
	{
	}

	// Token: 0x04003725 RID: 14117
	private const float _defaultSpeed = 0.2f;

	// Token: 0x04003726 RID: 14118
	private const float _damageValue = 405f;

	// Token: 0x04003727 RID: 14119
	private const float _speedDownTime = 10f;

	// Token: 0x04003728 RID: 14120
	private const float _speedDownRate = 0.5f;

	// Token: 0x04003729 RID: 14121
	private const float _resetDelay = 10f;

	// Token: 0x0400372A RID: 14122
	private bool _rotationStarted;

	// Token: 0x0400372B RID: 14123
	private Timer _speedDownTimer = new Timer();

	// Token: 0x0400372C RID: 14124
	private Timer _resetTimer = new Timer();

	// Token: 0x0400372D RID: 14125
	public static DamageInfo DamageInfo = new DamageInfo(RwbpType.B, 12, 20);

	// Token: 0x0400372E RID: 14126
	private float currentDamageCumlatived;
}
