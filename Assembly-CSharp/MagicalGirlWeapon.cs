using System;
using UnityEngine;

// Token: 0x02000670 RID: 1648
public class MagicalGirlWeapon : EquipmentScriptBase
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060036AF RID: 13999 RVA: 0x00021CE2 File Offset: 0x0001FEE2
	private static float rValue
	{
		get
		{
			return UnityEngine.Random.Range(5f, 8f);
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060036B0 RID: 14000 RVA: 0x00031AAD File Offset: 0x0002FCAD
	private static DamageInfo rDmg
	{
		get
		{
			return new DamageInfo(RwbpType.R, MagicalGirlWeapon.rValue);
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060036B1 RID: 14001 RVA: 0x00021CE2 File Offset: 0x0001FEE2
	private static float wValue
	{
		get
		{
			return UnityEngine.Random.Range(5f, 8f);
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060036B2 RID: 14002 RVA: 0x00031ABA File Offset: 0x0002FCBA
	private static DamageInfo wDmg
	{
		get
		{
			return new DamageInfo(RwbpType.W, MagicalGirlWeapon.wValue);
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060036B3 RID: 14003 RVA: 0x00021CE2 File Offset: 0x0001FEE2
	private static float bValue
	{
		get
		{
			return UnityEngine.Random.Range(5f, 8f);
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x00031AC7 File Offset: 0x0002FCC7
	private static DamageInfo bDmg
	{
		get
		{
			return new DamageInfo(RwbpType.B, MagicalGirlWeapon.bValue);
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x060036B5 RID: 14005 RVA: 0x00021CE2 File Offset: 0x0001FEE2
	private static float pValue
	{
		get
		{
			return UnityEngine.Random.Range(5f, 8f);
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060036B6 RID: 14006 RVA: 0x00031AD4 File Offset: 0x0002FCD4
	private static DamageInfo pDmg
	{
		get
		{
			return new DamageInfo(RwbpType.P, MagicalGirlWeapon.pValue);
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060036B7 RID: 14007 RVA: 0x00024A14 File Offset: 0x00022C14
	private static float rHealValue
	{
		get
		{
			return UnityEngine.Random.Range(2f, 3f);
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00024A14 File Offset: 0x00022C14
	private static float wHealValue
	{
		get
		{
			return UnityEngine.Random.Range(2f, 3f);
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x060036B9 RID: 14009 RVA: 0x00024A14 File Offset: 0x00022C14
	private static float bHealValue
	{
		get
		{
			return UnityEngine.Random.Range(2f, 3f);
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x060036BA RID: 14010 RVA: 0x00024A14 File Offset: 0x00022C14
	private static float pHealValue
	{
		get
		{
			return UnityEngine.Random.Range(2f, 3f);
		}
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00031AE1 File Offset: 0x0002FCE1
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		this.ShootProjectile();
		return false;
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x001684FC File Offset: 0x001666FC
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
		string text = "Effect/Agent/MagicalGirlWeaponProjectile_";
		float value = UnityEngine.Random.value;
		RwbpType type;
		string str;
		if (value <= 0.3f)
		{
			type = RwbpType.R;
			str = "R";
		}
		else if (value <= 0.6f)
		{
			type = RwbpType.W;
			str = "W";
		}
		else if (value <= 0.90000004f)
		{
			type = RwbpType.B;
			str = "B";
		}
		else
		{
			type = RwbpType.P;
			str = "P";
		}
		text += str;
		GameObject gameObject = Prefab.LoadPrefab(text);
		MagicalGirlWeaponProjectile component = gameObject.GetComponent<MagicalGirlWeaponProjectile>();
		if (component != null)
		{
			component.script = this;
			component.type = type;
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
			}
			gameObject.transform.position = base.model.owner.GetCurrentViewPosition() + b;
			gameObject.transform.localScale = localScale;
			component.target = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
			component.isEnabled = true;
		}
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x001686F8 File Offset: 0x001668F8
	public bool CheckHit(UnitModel target, RwbpType type)
	{
		if (!target.IsAttackTargetable())
		{
			return false;
		}
		WorkerModel workerModel = base.model.owner as WorkerModel;
		if (base.model.owner.IsHostile(target))
		{
			this.GiveDamage(target, type);
			return true;
		}
		if (workerModel != null && workerModel.IsPanic())
		{
			this.GiveDamage(target, type);
			return true;
		}
		if (target is CreatureModel)
		{
			this.GiveDamage(target, type);
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
			this.Heal(target as WorkerModel, type);
		}
		return false;
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x001687C8 File Offset: 0x001669C8
	private void GiveDamage(UnitModel target, RwbpType type)
	{
		UnitModel owner = base.model.owner;
		DamageInfo damageInfo;
		switch (type)
		{
		case RwbpType.R:
			damageInfo = MagicalGirlWeapon.rDmg;
			break;
		case RwbpType.W:
			damageInfo = MagicalGirlWeapon.wDmg;
			break;
		case RwbpType.B:
			damageInfo = MagicalGirlWeapon.bDmg;
			break;
		case RwbpType.P:
			damageInfo = MagicalGirlWeapon.pDmg;
			break;
		default:
			damageInfo = MagicalGirlWeapon.rDmg;
			break;
		}
		float num = owner.GetDamageFactorByEquipment();
		num *= owner.GetDamageFactorBySefiraAbility();
		float reinforcementDmg = base.GetReinforcementDmg();
		damageInfo.min = damageInfo.min * num * reinforcementDmg;
		damageInfo.max = damageInfo.max * num * reinforcementDmg;
		target.TakeDamage(base.model.owner, damageInfo);
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, damageInfo.type, base.model.owner);
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x00168898 File Offset: 0x00166A98
	private void Heal(WorkerModel worker, RwbpType type)
	{
		switch (type)
		{
		case RwbpType.R:
			worker.RecoverHP(MagicalGirlWeapon.rHealValue);
			break;
		case RwbpType.W:
			worker.RecoverMental(MagicalGirlWeapon.wHealValue);
			break;
		case RwbpType.B:
			worker.RecoverHP(MagicalGirlWeapon.bHealValue / 2f);
			worker.RecoverMental(MagicalGirlWeapon.bHealValue / 2f);
			break;
		case RwbpType.P:
			worker.RecoverHP((float)worker.maxHp * MagicalGirlWeapon.pHealValue / 100f);
			break;
		}
	}

	// Token: 0x0400327A RID: 12922
	private const float _rDmgMin = 5f;

	// Token: 0x0400327B RID: 12923
	private const float _rDmgMax = 8f;

	// Token: 0x0400327C RID: 12924
	private const float _wDmgMin = 5f;

	// Token: 0x0400327D RID: 12925
	private const float _wDmgMax = 8f;

	// Token: 0x0400327E RID: 12926
	private const float _bDmgMin = 5f;

	// Token: 0x0400327F RID: 12927
	private const float _bDmgMax = 8f;

	// Token: 0x04003280 RID: 12928
	private const float _pDmgMin = 5f;

	// Token: 0x04003281 RID: 12929
	private const float _pDmgMax = 8f;

	// Token: 0x04003282 RID: 12930
	private const float _rHealMin = 2f;

	// Token: 0x04003283 RID: 12931
	private const float _rHealMax = 3f;

	// Token: 0x04003284 RID: 12932
	private const float _wHealMin = 2f;

	// Token: 0x04003285 RID: 12933
	private const float _wHealMax = 3f;

	// Token: 0x04003286 RID: 12934
	private const float _bHealMin = 2f;

	// Token: 0x04003287 RID: 12935
	private const float _bHealMax = 3f;

	// Token: 0x04003288 RID: 12936
	private const float _pHealMin = 2f;

	// Token: 0x04003289 RID: 12937
	private const float _pHealMax = 3f;

	// Token: 0x0400328A RID: 12938
	private const string _projectileSrc = "Effect/Agent/MagicalGirlWeaponProjectile_";

	// Token: 0x0400328B RID: 12939
	private const float _rProb = 0.3f;

	// Token: 0x0400328C RID: 12940
	private const float _wProb = 0.3f;

	// Token: 0x0400328D RID: 12941
	private const float _bProb = 0.3f;

	// Token: 0x0400328E RID: 12942
	private Vector3 _effectPos = new Vector3(1.544f, 1.088f, 0f);
}
