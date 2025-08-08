/*
public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target) // Fixed weapon only hitting once
*/
using System;
using System.Collections.Generic; //

// Token: 0x02000676 RID: 1654
public class PorccuWeapon : EquipmentScriptBase
{
	// Token: 0x06003672 RID: 13938 RVA: 0x000301FE File Offset: 0x0002E3FE
	public PorccuWeapon()
	{
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00030F65 File Offset: 0x0002F165
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{ // <Mod> fixed weapon only hitting once
		SoundEffectPlayer.PlayOnce(_sound_AtkStart, actor.GetCurrentViewPosition(), volume);
        List<DamageInfo> list = new List<DamageInfo>();
		for (int i = 0; i < _COUNT_ATTACK_PER_ANIM; i++)
		{
			list.Add(base.model.metaInfo.damageInfo);
		}
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[0], list.ToArray());
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x00030F8A File Offset: 0x0002F18A
	public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
		base.OnGiveDamageAfter(actor, target, dmg);
		target.AddUnitBuf(new PorccuWeaponDebuf());
	}

	// Token: 0x04003259 RID: 12889
	private const string _sound_AtkStart = "creature/Porccu/Porccu_Atk1";

	// Token: 0x0400325A RID: 12890
	private const float volume = 1f;


    private const int _COUNT_ATTACK_PER_ANIM = 4; //
}
