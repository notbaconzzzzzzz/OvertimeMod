/*
public override void OnInit() // Green Midnight Rework
public override void OnFixedUpdate(CreatureModel creature) // Green Midnight Rework
public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value) // Green Midnight Rework
public override bool OnAfterSuppressed() // Green Midnight Rework
+public override float GetRadius() // Increased hurtbox size
+public override void OnDestroy() // Fixing Laser Bug; Green Midnight Rework
+public override bool SetCastingSlider(Slider castingSlider) // Green Midnight Rework
+//> Green Midnight Rework
*/
using System;
using System.Collections.Generic; //
using UnityEngine;
using UnityEngine.UI; //

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
	{ // <Mod>
		base.OnInit();
		_reworkedVersion = SpecialModeConfig.instance.GetValue<bool>("GreenMidnightRework");
		this.currentDamageCumlatived = 0f;
		plamsaShieldCnt = 0;
		hasPlasmaShield = false;
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
	{ // <Mod>
		base.OnFixedUpdate(creature);
		if (this._speedDownTimer.started && this._speedDownTimer.RunTimer())
		{
			this.AnimScript.SetRotationSpeed(0.2f);
		}
		if (this._resetTimer.started && this._resetTimer.RunTimer())
		{
			this.AnimScript.Restart();
		}
		if (plasmaShieldTimer.started && plasmaShieldTimer.RunTimer())
		{
			ActivatePlasmaShield();
		}
		if (hasPlasmaShield)
		{
			plasmaDamageTimer += Time.deltaTime;
			if (plasmaDamageTimer >= 0.5f)
			{
				plasmaDamageTimer -= 0.5f;
				DamageAllInRoom(PlasmaContinuousDamage);
			}
		}
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00035223 File Offset: 0x00033423
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{ // <Mod>
		base.OnTakeDamage(actor, dmg, value);
		this.currentDamageCumlatived += value;
		if (this.currentDamageCumlatived >= 405f)
		{
			this.currentDamageCumlatived = 0f;
			this.ActivateSpeedDown();
		}
		if (ReworkedVersion && model.hp / model.maxHp <= 0.3f - 0.4f * plamsaShieldCnt)
		{
			ChargePlasmaShield();
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
	{ // <Mod>
		this._ordealScript.OrdealEnd();
		this._resetTimer.StopTimer();
		this.AnimScript.OnSuppressed();
		if (midnightOverload != null)
		{
			midnightOverload.OnDestroy();
			midnightOverload = null;
		}
		if (hasPlasmaShield)
		{
			DeactivatePlasmaShield();
		}
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

	// <Mod> Making sure that the laser gets properly turned off when you restart the day
    public override void OnDestroy()
    {
        AnimScript.OnCloseLaser();
		if (midnightOverload != null)
		{
			midnightOverload.OnDestroy();
			midnightOverload = null;
		}
		if (hasPlasmaShield)
		{
			DeactivatePlasmaShield();
		}
    }
	
	// <Mod> Green Midnight Rework
	public override bool SetCastingSlider(Slider castingSlider)
	{
		if (!plasmaShieldTimer.started)
		{
			return false;
		}
		castingSlider.maxValue = plasmaShieldTimer.maxTime;
		castingSlider.value = plasmaShieldTimer.elapsed;
		return true;
	}

	// <Mod>
	public override float GetRadius()
	{
		return 2.5f;
	}

	//> <Mod> Green Midnight Rework
	public bool ReworkedVersion
	{
		get
		{
			return _reworkedVersion;
		}
	}

	public void ChargePlasmaShield()
	{
		if (hasPlasmaShield || plasmaShieldTimer.started) return;
		model.AddUnitBuf(new MachineMidnightPlasmaArmorBuf(0.1f - 0.4f * plamsaShieldCnt));
		plasmaShieldTimer.StartTimer(10f);
		plamsaShieldCnt++;
		if (electricity == null)
		{
			electricity = new GameObject[8];
			for (int i = 0; i < 4; i++)
			{
				electricity[i*2] = Prefab.LoadPrefab("Effect/creature/lookatme/LookAtMeSkillElectric");
				electricity[i*2].transform.SetParent(Unit.animTarget.transform);
				electricity[i*2].transform.localPosition = new Vector3(0f, 1.5f, 0f);
				electricity[i*2].transform.localScale = Vector3.one * 4.8f;
				electricity[i*2].transform.localRotation = Quaternion.identity;
				electricity[i*2].SetActive(false);
				electricity[i*2 + 1] = Prefab.LoadPrefab("Effect/creature/lookatme/LookAtMeSkillElectric");
				electricity[i*2 + 1].transform.SetParent(Unit.animTarget.transform);
				electricity[i*2 + 1].transform.localPosition = new Vector3(0f, 4.5f, 0f);
				electricity[i*2 + 1].transform.localScale = Vector3.one * 4.8f;
				electricity[i*2 + 1].transform.localRotation = Quaternion.identity;
				electricity[i*2 + 1].SetActive(false);
			}
		}
		electricity[0].SetActive(true);
		electricity[1].SetActive(true);
	}

	public void ActivatePlasmaShield()
	{
		if (hasPlasmaShield) return;
		hasPlasmaShield = true;
		model.RemoveUnitBuf(model.GetUnitBufByType(UnitBufType.MACHINE_MIDNIGHT_PLASMA_ARMOR));
		DamageAllInRoom(PlasmaInitialDamage);
		plasmaDamageTimer = 0f;
		SpawnOverloads();
		for (int i = 2; i < 8; i++)
		{
			electricity[i].SetActive(true);
		}
	}

	public void DeactivatePlasmaShield()
	{
		if (!hasPlasmaShield) return;
		hasPlasmaShield = false;
		for (int i = 0; i < 8; i++)
		{
			electricity[i].SetActive(false);
		}
	}

	public void DamageAllInRoom(DamageInfo damage)
	{
		List<UnitModel> targets = new List<UnitModel>();
		foreach (MovableObjectNode obj in model.GetMovableNode().currentPassage.GetEnteredTargets())
		{
			UnitModel unit = obj.GetUnit();
			if (unit == null) continue;
			if (!unit.IsAttackTargetable()) continue;
			if (unit is CreatureModel)
			{
				CreatureModel creature = unit as CreatureModel;
				if (creature.script is MachineOrdealCreature) continue;
				if (creature.hp <= 0f) continue;
			}
			else if (unit is WorkerModel)
			{
				WorkerModel worker = unit as WorkerModel;
				if (worker.IsDead()) continue;
			}
			else if (unit is RabbitModel)
			{
				
			}
			targets.Add(unit);
		}
		foreach (UnitModel unit in targets)
		{
			unit.TakeDamage(model, damage);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, damage.type, model);
		}
		/*GameObject test = Prefab.LoadPrefab("Effect/creature/fixerblack/FixerBlackWeaponEffect");
		test.transform.SetParent(Unit.animTarget.transform);
		test.transform.localPosition = new Vector3(-0.5f, -2f, 0f);
		test.transform.localScale = Vector3.one * 10f;
		test.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.forward);*/
	}

	public void SpawnOverloads()
	{
		midnightOverload = new MachineMidnightOverload(this);
		midnightOverload.CastOverload();
	}

	public void OverloadSuccess()
	{
		midnightOverload.OnDestroy();
		midnightOverload = null;
		DeactivatePlasmaShield();
	}

	public void OverloadFail()
	{
		midnightOverload.OnDestroy();
		midnightOverload = null;
		model.RecoverHP((float)model.maxHp * 0.2f);
		GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
		gameObject.transform.SetParent(this.Unit.animTarget.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		plamsaShieldCnt--;
		DeactivatePlasmaShield();
	}

	private MachineMidnightOverload midnightOverload;

	private Timer plasmaShieldTimer = new Timer();

	private bool hasPlasmaShield = false;

	private int plamsaShieldCnt = 0;

	private float plasmaDamageTimer;

	public static DamageInfo PlasmaInitialDamage = new DamageInfo(RwbpType.B, 225f);

	public static DamageInfo PlasmaContinuousDamage = new DamageInfo(RwbpType.B, 42, 48);

	private GameObject[] electricity;

	//<

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

	// <Mod>
	private bool _reworkedVersion;
}
