using System;
using UnityEngine.UI;

public class OvertimeYesodCoreScript : YesodCoreScript
{
	public OvertimeYesodCoreScript()
	{
	}

	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
	}

	public override bool IsSensoredInPassage()
	{
		return false;
	}

	public override bool IsSuppressable()
	{
		return false;
	}

	public override bool IsAutoSuppressable()
	{
		return false;
	}

	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return false;
	}

	public override bool HasEscapeUI()
	{
		return false;
	}

	public override bool SetHpSlider(Slider slider)
	{
		return false;
	}

	public override bool IsAttackTargetable()
	{
		return false;
	}

	public OvertimeYesodBossBase bossBase;
}
