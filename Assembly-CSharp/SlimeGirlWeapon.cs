using System;
using UnityEngine;

// Token: 0x02000681 RID: 1665
public class SlimeGirlWeapon : EquipmentScriptBase
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060036A0 RID: 13984 RVA: 0x00031369 File Offset: 0x0002F569
	private static DamageInfo Dmg
	{
		get
		{
			return new DamageInfo(RwbpType.B, _dmgMin, _dmgMax );
		}
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x00031375 File Offset: 0x0002F575
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		this.ShootProjectile();
		if (dmg.soundInfo != null && dmg.soundInfo.soundType == DamageInfo_EffectType.DAMAGE_INVOKED)
		{
			dmg.soundInfo.PlaySound(actor.GetCurrentViewPosition());
		}
		return false;
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x00162A94 File Offset: 0x00160C94
	private void ShootProjectile()
	{
		if (base.model.owner.GetMovableNode().currentPassage == null)
		{
			return;
		}
		PassageObjectModel currentPassage = base.model.owner.GetMovableNode().currentPassage;
		float scaleFactor = currentPassage.scaleFactor;
		float num = 0f;
		float num2 = 0f;
		Vector3 localScale = Vector3.one * scaleFactor;
		Vector3 b = this._effectPos * scaleFactor;
		currentPassage.GetVerticalRange(ref num, ref num2);
		GameObject gameObject = Prefab.LoadPrefab("Effect/Agent/SlimeGirlWeaponProjectile");
		SlimeGirlWeaponProjectile component = gameObject.GetComponent<SlimeGirlWeaponProjectile>();
		if (component != null)
		{
			component.script = this;
			float x;
			if (base.model.owner.GetMovableNode().GetDirection() == UnitDirection.RIGHT)
			{
				float dir = 1f;
				component.dir = dir;
				x = num2;
			}
			else
			{
				float dir = -1f;
				component.dir = dir;
				x = num;
				b.x *= -1f;
				localScale.x *= -1f;
			}
			gameObject.transform.position = base.model.owner.GetCurrentViewPosition() + b;
			gameObject.transform.localScale = localScale;
			component.target = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
			component.isEnabled = true;
		}
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x00162C20 File Offset: 0x00160E20
	public bool CheckHit(UnitModel target)
	{
		if (!target.IsAttackTargetable())
		{
			return false;
		}
		WorkerModel workerModel = base.model.owner as WorkerModel;
		if (base.model.owner.IsHostile(target))
		{
			this.GiveDamage(target);
			return true;
		}
		if (workerModel != null && workerModel.IsPanic())
		{
			this.GiveDamage(target);
			return true;
		}
		if (target is CreatureModel)
		{
			this.GiveDamage(target);
			return true;
		}
		if (target is WorkerModel)
		{
			WorkerModel workerModel2 = target as WorkerModel;
			if (workerModel2 == base.model.owner as WorkerModel)
			{
				return false;
			}
			if (workerModel2.CannotControll())
			{
				return false;
			}
			if (workerModel2.unconAction != null)
			{
				return false;
			}
		}
		return false;
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00162CE0 File Offset: 0x00160EE0
	private void GiveDamage(UnitModel target)
	{
		UnitModel owner = base.model.owner;
		DamageInfo dmg = SlimeGirlWeapon.Dmg;
		float num = owner.GetDamageFactorByEquipment();
		num *= owner.GetDamageFactorBySefiraAbility();
		float reinforcementDmg = base.GetReinforcementDmg();
		dmg.min = dmg.min * num * reinforcementDmg;
		dmg.max = dmg.max * num * reinforcementDmg;
		target.TakeDamage(base.model.owner, dmg);
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, dmg.type, base.model.owner);
		if (target.hp > 0f)
		{
			target.AddUnitBuf(new SlimeGirlWeaponDebuf());
		}
		MovableObjectNode movableNode = target.GetMovableNode();
		if (movableNode != null)
		{
			this.MakeEffect(movableNode.GetCurrentViewPosition(), movableNode.currentScale);
		}
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00162DA4 File Offset: 0x00160FA4
	private GameObject MakeEffect(Vector3 pos, float scale)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/SlimeGirl/SlimeGirlProjectileHitEffect");
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = pos;
		gameObject.transform.localScale = Vector3.one * scale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Vector3 lossyScale = gameObject.transform.lossyScale;
		Vector3 localScale = gameObject.transform.localScale;
		float num = lossyScale.x / lossyScale.z;
		localScale.z *= num;
		gameObject.transform.localScale = localScale;
		return gameObject;
	}

	// Token: 0x0400327F RID: 12927
	public const float slowRatio = 0.3f;

	// Token: 0x04003280 RID: 12928
	private const int _dmgMin = 22;

	// Token: 0x04003281 RID: 12929
	private const int _dmgMax = 44;

	// Token: 0x04003282 RID: 12930
	private const RwbpType _dmgType = RwbpType.B;

	// Token: 0x04003283 RID: 12931
	private const string _projectileSrc = "Effect/Agent/SlimeGirlWeaponProjectile";

	// Token: 0x04003284 RID: 12932
	private const string _projectileHitSrc = "Effect/Creature/SlimeGirl/SlimeGirlProjectileHitEffect";

	// Token: 0x04003285 RID: 12933
	private Vector3 _effectPos = new Vector3(1.544f, 0.94f, 0f);
}
