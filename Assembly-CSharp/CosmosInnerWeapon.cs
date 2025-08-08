using System;
using UnityEngine;

// Token: 0x02000690 RID: 1680
public class CosmosInnerWeapon : EquipmentScriptBase
{
	// Token: 0x060036C9 RID: 14025 RVA: 0x00030466 File Offset: 0x0002E666
	public CosmosInnerWeapon()
	{
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x000315A9 File Offset: 0x0002F7A9
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[UnityEngine.Random.Range(0, 2)], new DamageInfo[]
		{
			(base.model as WeaponModel).GetDamage(actor)
		});
	}

	//> <Mod>
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		if (target is WorkerModel) {
			WorkerModel worker = (target as WorkerModel);
			_target = worker;
			this.old_sp = worker.mental;
		}
		return base.OnGiveDamage(actor, target, ref dmg);
	}

	public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
		if (SpecialModeConfig.instance.GetValue<bool>("FragmentErosion") && target is WorkerModel) {
			WorkerModel worker = (target as WorkerModel);
			if (worker == _target)
			{
				UnitBuf debuf = worker.GetUnitBufByType(UnitBufType.COSMOS_INNER_WEAPON_STAT);
				UnitStatBuf statdebuf = null;
				if (debuf is UnitStatBuf) {
					statdebuf = (debuf as UnitStatBuf);
					statdebuf.remainTime = 60f;
				}
				if (statdebuf == null) {
					statdebuf = new UnitStatBuf(60f, UnitBufType.COSMOS_INNER_WEAPON_STAT);
					worker.AddUnitBuf(statdebuf);
				}
				if (statdebuf != null) {
					statdebuf.maxMental -= (int)(Math.Ceiling(old_sp) - Math.Ceiling(worker.mental));
				}
			}
		}
		base.OnGiveDamageAfter(actor, target, dmg);
	}

	private float old_sp;

	private WorkerModel _target;
	//< <Mod>
}
