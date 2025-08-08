/*
public override void OnStageStart() // 
+public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg) // Gives debufs that multiply Pale damage taken
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class DeathAngelWeapon : EquipmentScriptBase
{
	// Token: 0x06003601 RID: 13825 RVA: 0x001605B4 File Offset: 0x0015E7B4
	public DeathAngelWeapon()
	{
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06003602 RID: 13826 RVA: 0x00030E0D File Offset: 0x0002F00D
	private static float skillCoolTime
	{
		get
		{
			return UnityEngine.Random.Range(20f, 20f);
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06003603 RID: 13827 RVA: 0x00030E1E File Offset: 0x0002F01E
	private static DamageInfo Dmg_1st
	{
		get
		{
			return new DamageInfo(RwbpType.P, 16, 20);
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06003604 RID: 13828 RVA: 0x00030E2A File Offset: 0x0002F02A
	private static DamageInfo Dmg_2nd
	{
		get
		{
			return new DamageInfo(RwbpType.P, 19, 23);
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06003605 RID: 13829 RVA: 0x00030E36 File Offset: 0x0002F036
	private static DamageInfo Dmg_3rd
	{
		get
		{
			return new DamageInfo(RwbpType.P, 22, 28);
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06003606 RID: 13830 RVA: 0x000299E0 File Offset: 0x00027BE0
	private static float HealValue
	{
		get
		{
			return UnityEngine.Random.Range(2f, 4f);
		}
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00160608 File Offset: 0x0015E808
	public override void OnStageStart()
	{ // <Mod> Keep Ability Mode
		base.OnStageStart();
		this.worker = (base.model.owner as WorkerModel);
		if (CreatureManager.instance.FindCreature(100015L) == null && !SpecialModeConfig.instance.GetValue<bool>("PLKeepAbilityWhenWNAbsent"))
		{
			this._whiteNight = false;
		}
		else
		{
			this._whiteNight = true;
		}
		this.atkInit = false;
		this.SetNextAttackType();
		this.LoadEffect();
		this.SetEffectActive(false);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x00030E42 File Offset: 0x0002F042
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ClearEffect();
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x00160678 File Offset: 0x0015E878
	private void LoadEffect()
	{
		this._skillEffect = Prefab.LoadPrefab("Effect/Agent/DeathAngelWeaponTrail");
		Transform giftPos = this.worker.GetWorkerUnit().spriteSetter.GetGiftPos(EGOgiftAttachRegion.RIGHTHAND);
		this._skillEffect.transform.SetParent(giftPos);
		this._skillEffect.transform.localPosition = this.trailPos;
		this._skillEffect.transform.localScale = Vector3.one;
		this._skillEffect.transform.localRotation = Quaternion.Euler(this.trailRot);
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x00030E50 File Offset: 0x0002F050
	private void SetEffectActive(bool state)
	{
		this._skillEffect.SetActive(state);
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x00030E5E File Offset: 0x0002F05E
	private void ClearEffect()
	{
		UnityEngine.Object.Destroy(this._skillEffect);
		this._skillEffect = null;
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x00030E72 File Offset: 0x0002F072
	public override void OnPrepareWeapon(UnitModel actor)
	{
		base.OnPrepareWeapon(actor);
		this._isBattle = true;
		this.SetNextAttackType();
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x00030E88 File Offset: 0x0002F088
	public override void OnCancelWeapon(UnitModel actor)
	{
		this._isBattle = false;
		this.SetEffectActive(false);
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x00160704 File Offset: 0x0015E904
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		if (this.isSpecial)
		{
			this.skillCoolTimer.StartTimer(4f);
			list.Add(base.model.metaInfo.damageInfos[1].Copy());
			animationName = base.model.metaInfo.animationNames[1];
			this.GetBarrier();
		}
		else
		{
			list.Add(base.model.metaInfo.damageInfos[0].Copy());
			animationName = base.model.metaInfo.animationNames[0];
			this.atkInit = false;
		}
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x00030E98 File Offset: 0x0002F098
	public override void OnAttackEnd(UnitModel actor, UnitModel target)
	{
		base.OnAttackEnd(actor, target);
		this.atkInit = false;
		if (this.isSpecial)
		{
			this.skillCoolTimer.StartTimer(DeathAngelWeapon.skillCoolTime);
		}
		this.SetNextAttackType();
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x001607B8 File Offset: 0x0015E9B8
	private void SetNextAttackType()
	{
		this.atkInit = false;
		if (!this._whiteNight)
		{
			this.isSpecial = false;
		}
		else if (!this.skillCoolTimer.started)
		{
			this.isSpecial = true;
		}
		else
		{
			this.isSpecial = false;
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00030ECA File Offset: 0x0002F0CA
	private void GetBarrier()
	{
		if (this.worker != null)
		{
			this.worker.AddUnitBuf(new BarrierBuf(RwbpType.A, 100f, 10f));
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x00160808 File Offset: 0x0015EA08
	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if (this.skillCoolTimer.started)
		{
			if (this._skillEffect.activeInHierarchy)
			{
				this.SetEffectActive(false);
			}
			this.skillCoolTimer.RunTimer();
		}
		else if (this._isBattle && this._whiteNight && !this._skillEffect.activeInHierarchy)
		{
			this.SetEffectActive(true);
		}
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x00160880 File Offset: 0x0015EA80
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		if (this.isSpecial)
		{
            bool output = base.OnGiveDamage(actor, target, ref dmg);
			return output;
		}
		if (this.atkInit)
		{
			return false;
		}
		List<UnitModel> targets = this.GetTargets();
		this.atkInit = true;
		int count = targets.Count;
		if (count <= 0)
		{
			return false;
		}
		int cnt;
		if (count < 2)
		{
			cnt = 3;
		}
		else if (count < 6)
		{
			cnt = 2;
		}
		else
		{
			cnt = 1;
		}
		foreach (UnitModel target2 in targets)
		{
			this.GiveDamage(target2, cnt);
		}
		if (dmg.soundInfo != null && dmg.soundInfo.soundType == DamageInfo_EffectType.DAMAGE_INVOKED)
		{
			dmg.soundInfo.PlaySound(actor.GetCurrentViewPosition());
		}
		return false;
	}

    // <Mod> gives debufs that multiply Pale damage taken, normal attack x1.2, special attack x2.0
    public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
    {
		if (isSpecial)
		{
			if (target.hp > 0f)
			{
				target.AddUnitBuf(new DeathAngelWeaponSpecialDebuf());
			}
			return;
		}
        target.AddUnitBuf(new DeathAngelWeaponNormalDebuf());
    }


    // Token: 0x06003614 RID: 13844 RVA: 0x00160970 File Offset: 0x0015EB70
    private List<UnitModel> GetTargets()
	{
		List<UnitModel> list = new List<UnitModel>();
		PassageObjectModel currentPassage = base.model.owner.GetMovableNode().currentPassage;
		if (currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in currentPassage.GetEnteredTargets(base.model.owner.GetMovableNode()))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (this.IsHostile(unit))
				{
					list.Add(unit);
				}
			}
		}
		return list;
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x001609F4 File Offset: 0x0015EBF4
	private void GiveDamage(UnitModel target, int cnt)
	{
		UnitModel owner = base.model.owner;
		WorkerModel workerModel = owner as WorkerModel;
		DamageInfo damageInfo = DeathAngelWeapon.Dmg_1st;
		if (cnt != 1)
		{
			if (cnt != 2)
			{
				if (cnt == 3)
				{
					damageInfo = DeathAngelWeapon.Dmg_3rd;
				}
			}
			else
			{
				damageInfo = DeathAngelWeapon.Dmg_2nd;
			}
		}
		else
		{
			damageInfo = DeathAngelWeapon.Dmg_1st;
		}
		if (workerModel != null)
		{
			workerModel.RecoverHP(DeathAngelWeapon.HealValue);
			workerModel.RecoverMental(DeathAngelWeapon.HealValue);
		}
		else if (owner != null)
		{
			float num = (float)owner.maxHp - owner.hp;
			float healValue = DeathAngelWeapon.HealValue;
			if (num > healValue)
			{
				owner.hp += healValue;
			}
			else
			{
				owner.hp = (float)owner.maxHp;
			}
		}
		float num2 = owner.GetDamageFactorByEquipment();
		num2 *= owner.GetDamageFactorBySefiraAbility();
		float reinforcementDmg = base.GetReinforcementDmg();
		damageInfo.min = damageInfo.min * num2 * reinforcementDmg;
		damageInfo.max = damageInfo.max * num2 * reinforcementDmg;
		target.TakeDamage(owner, damageInfo);
		this.OnGiveDamageAfter(owner, target, damageInfo);
		if (target is WorkerModel && (target as WorkerModel).IsDead())
		{
			this.OnKillMainTarget(owner, target);
		}
		this.MakeEffect(target, cnt);
		if (target.hp > 0f)
		{
			target.AddUnitBuf(new DeathAngelWeaponSlow());
		}
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, damageInfo.type, owner);
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x00160B64 File Offset: 0x0015ED64
	private void MakeEffect(UnitModel target, int cnt)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Invoke/DamageInfo/DeathAngelWeaponeffect");
		DeathAngelWeaponEffect component = gameObject.GetComponent<DeathAngelWeaponEffect>();
		if (component != null)
		{
			Vector3 currentViewPosition = target.GetCurrentViewPosition();
			float currentScale = target.GetMovableNode().currentScale;
			gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
			gameObject.transform.position = currentViewPosition;
			gameObject.transform.localScale = new Vector3(currentScale, currentScale, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
			component.SetActive(cnt);
		}
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x00160BF4 File Offset: 0x0015EDF4
	private bool IsHostile(UnitModel target)
	{
		if (!target.IsAttackTargetable())
		{
			return false;
		}
		WorkerModel workerModel = base.model.owner as WorkerModel;
		return target != workerModel && (base.model.owner.IsHostile(target) || (workerModel != null && workerModel.IsPanic()) || target is CreatureModel);
	}

	// Token: 0x040031FE RID: 12798
	private Timer skillCoolTimer = new Timer();

	// Token: 0x040031FF RID: 12799
	private const float _skillCoolTimeMin = 20f;

	// Token: 0x04003200 RID: 12800
	private const float _skillCoolTimeMax = 20f;

	// Token: 0x04003201 RID: 12801
	private const int _dmgMin_1st = 16;

	// Token: 0x04003202 RID: 12802
	private const int _dmgMax_1st = 20;

	// Token: 0x04003203 RID: 12803
	private const RwbpType _dmgType = RwbpType.P;

	// Token: 0x04003204 RID: 12804
	private const int _dmgMin_2nd = 19;

	// Token: 0x04003205 RID: 12805
	private const int _dmgMax_2nd = 23;

	// Token: 0x04003206 RID: 12806
	private const int _dmgMin_3rd = 22;

	// Token: 0x04003207 RID: 12807
	private const int _dmgMax_3rd = 28;

	// Token: 0x04003208 RID: 12808
	private const float _healMin = 2f;

	// Token: 0x04003209 RID: 12809
	private const float _healMax = 4f;

	// Token: 0x0400320A RID: 12810
	private const float _barrierValue = 100f;

	// Token: 0x0400320B RID: 12811
	private const float _barrierTime = 10f;

	// Token: 0x0400320C RID: 12812
	private const int _effectConditionFirst = 2;

	// Token: 0x0400320D RID: 12813
	private const int _effectConditionSecond = 6;

	// Token: 0x0400320E RID: 12814
	private const string _effectSrc = "Effect/Invoke/DamageInfo/DeathAngelWeaponeffect";

	// Token: 0x0400320F RID: 12815
	private const string _trailSrc = "Effect/Agent/DeathAngelWeaponTrail";

	// Token: 0x04003210 RID: 12816
	private readonly Vector3 trailPos = new Vector3(0f, 0f, 0f);

	// Token: 0x04003211 RID: 12817
	private readonly Vector3 trailRot = new Vector3(0f, 0f, 94.5f);

	// Token: 0x04003212 RID: 12818
	private GameObject _skillEffect;

	// Token: 0x04003213 RID: 12819
	private WorkerModel worker;

	// Token: 0x04003214 RID: 12820
	private bool isSpecial;

	// Token: 0x04003215 RID: 12821
	private bool atkInit;

	// Token: 0x04003216 RID: 12822
	private bool _isBattle;

	// Token: 0x04003217 RID: 12823
	private bool _whiteNight;
}
