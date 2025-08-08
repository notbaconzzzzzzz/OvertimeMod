using System;
using System.Collections.Generic;
using DeadEffect;
using GeburahBoss;
using UnityEngine;

public class OvertimeGeburahCoreScript : GeburahCoreScript
{
	public void SetBossBase(OvertimeGeburahBossBase b)
	{
		this.bossBase = b;
	}

	public override OvertimeGeburahBossBase OvertimeBossBase
	{
		get
		{
			return bossBase;
		}
	}

	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase) _recoverTimer.StartTimer(float.PositiveInfinity);
	}

	public override bool IsAutoSuppressable()
	{
		return !(_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase);
	}

	public override bool IsSuppressable()
	{
		return !(_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase);
	}

	public override bool IsIndirectSuppressable()
	{
		return !(_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase);
	}

	public override bool IsAttackTargetable()
	{
		return !(_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase);
	}

	public override bool IsSensoredInPassage()
	{
		return !(_recoverTimer.started && maximumPhase < GeburahPhase.P4 && Phase >= maximumPhase);
	}

	private OvertimeGeburahBossBase bossBase;

	public GeburahPhase maximumPhase = GeburahPhase.P1;
}
