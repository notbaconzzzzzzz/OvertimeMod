/*
public void AttachEGOgift(EGOgiftModel gift) // (!) 
public float GetDamageFactorByEquipment() // 
public virtual void MakeDamageEffect(RwbpType type, float value, DefenseInfo.Type defense) // Overtime Yesod Suppression
public UnitBuf AddUnitBuf(UnitBuf buf) // Changed the way it handles UnitBufType.UNKNOWN slightly
+public List<UnitStatBuf> GetStatBufList() // 
+public List<BarrierBuf> GetBarrierBufList() // 
+public bool IsEtcUnit() // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200076A RID: 1898
[Serializable]
public class UnitModel
{
	// Token: 0x06003A5C RID: 14940 RVA: 0x00172588 File Offset: 0x00170788
	public UnitModel()
	{
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06003A5D RID: 14941 RVA: 0x00033871 File Offset: 0x00031A71
	public UnitEquipSpace Equipment
	{
		get
		{
			return this._equipment;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06003A5E RID: 14942 RVA: 0x00021B3D File Offset: 0x0001FD3D
	public virtual float radius
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06003A5F RID: 14943 RVA: 0x00033879 File Offset: 0x00031A79
	public virtual int maxHp
	{
		get
		{
			return this.baseMaxHp + this.GetMaxHpBuf();
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06003A60 RID: 14944 RVA: 0x00033888 File Offset: 0x00031A88
	public virtual int maxMental
	{
		get
		{
			return this.baseMaxMental + this.GetMaxMentalBuf();
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003A61 RID: 14945 RVA: 0x00033897 File Offset: 0x00031A97
	public virtual float movement
	{
		get
		{
			return this.GetMovementBuf();
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003A62 RID: 14946 RVA: 0x0003389F File Offset: 0x00031A9F
	public virtual int regeneration
	{
		get
		{
			return this.baseRegeneration;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003A63 RID: 14947 RVA: 0x000338A7 File Offset: 0x00031AA7
	public virtual float regenerationDelay
	{
		get
		{
			return this.baseRegenerationDelay;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06003A64 RID: 14948 RVA: 0x000338AF File Offset: 0x00031AAF
	public virtual DefenseInfo defense
	{
		get
		{
			if (this._equipment.armor != null)
			{
				return this._equipment.armor.GetDefense(this);
			}
			return DefenseInfo.zero;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06003A65 RID: 14949 RVA: 0x00021B3D File Offset: 0x0001FD3D
	public virtual float attackSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06003A66 RID: 14950 RVA: 0x00021A33 File Offset: 0x0001FC33
	public virtual float damage
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06003A67 RID: 14951 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual int physicalDefense
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06003A68 RID: 14952 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual int mentalDefense
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool CanOpenDoor()
	{
		return true;
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x00021A3D File Offset: 0x0001FC3D
	public virtual string GetUnitName()
	{
		return string.Empty;
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void InteractWithDoor(DoorObjectModel door)
	{
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnStopMovableByShield(AgentModel shielder)
	{
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x000338D5 File Offset: 0x00031AD5
	public virtual MovableObjectNode GetMovableNode()
	{
		return this.movableNode;
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x000338DD File Offset: 0x00031ADD
	public virtual Vector3 GetCurrentViewPosition()
	{
		return this.movableNode.GetCurrentViewPosition();
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x0017260C File Offset: 0x0017080C
	public void SetWeapon(WeaponModel weapon)
	{
		if (this._equipment.weapon != null && this._equipment.weapon.owner != null)
		{
			if (this._equipment.weapon.owner != this)
			{
				this._equipment.weapon.owner.ReleaseWeaponV2();
			}
			else
			{
				this.ReleaseWeaponV2();
			}
		}
		if (weapon.owner != null)
		{
			weapon.owner.ReleaseWeaponV2();
		}
		this._equipment.weapon = weapon;
		this._equipment.weapon.OnEquip(this);
		this.OnSetWeapon();
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x000338EA File Offset: 0x00031AEA
	public void ReleaseWeaponV2()
	{
		if (this._equipment.weapon == null)
		{
			return;
		}
		this._equipment.weapon.OnRelease();
		this._equipment.weapon = WeaponModel.GetDummyWeapon();
		this.OnReleaseWeapon();
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x001726A0 File Offset: 0x001708A0
	public void SetArmor(ArmorModel armor)
	{
		if (this._equipment.armor != null && this._equipment.armor.owner != null)
		{
			if (this._equipment.armor.owner != this)
			{
				this._equipment.armor.owner.ReleaseArmor();
			}
			else
			{
				this.ReleaseArmor();
			}
		}
		if (armor.owner != null)
		{
			armor.owner.ReleaseArmor();
		}
		this._equipment.armor = armor;
		this._equipment.armor.OnEquip(this);
		this.OnSetArmor();
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x00033920 File Offset: 0x00031B20
	public void ReleaseArmor()
	{
		if (this._equipment.armor == null)
		{
			return;
		}
		this._equipment.armor.OnRelease();
		this._equipment.armor = ArmorModel.GetDummyArmor();
		this.OnReleaseArmor();
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x00172734 File Offset: 0x00170934
	public void AttachEGOgift(EGOgiftModel gift)
	{ // <Patch> <Mod>
		int stackingMax = UnitEGOgiftSpace.GetStackingMax();
		LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(gift.metaInfo);
		if (this._equipment.gifts.HasEquipment_Mod(lcId))
		{
			return;
		}
		if (!UnitEGOgiftSpace.IsUniqueLock((long)gift.metaInfo.id) || lcId.packageId != string.Empty)
		{
			if (stackingMax != -1 && _equipment.gifts.GiftCount(gift.metaInfo.attachType, gift.metaInfo.attachPos) >= stackingMax && this._equipment.gifts.GetLockState(gift.metaInfo))
			{
				return;
			}
		}
		else
		{
			this._equipment.gifts.SetLockState(gift, false);
		}
		this._equipment.gifts.AttachGift(this, gift);
		if (this.hp > (float)this.maxHp)
		{
			this.hp = (float)this.maxHp;
		}
		if (this.hp < 1f)
		{
			this.hp = 1f;
		}
		if (this.mental > (float)this.maxMental)
		{
			this.mental = (float)this.maxMental;
		}
		this.OnChangeGift();
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x001727E8 File Offset: 0x001709E8
	public void ReleaseEGOgift(EGOgiftModel model)
	{ // <Patch>
		ReleaseEGOGift_Mod(EquipmentTypeInfo.GetLcId(model.metaInfo));
		/*
		List<EGOgiftModel> addedGifts = this._equipment.gifts.addedGifts;
		EGOgiftModel egogiftModel = addedGifts.Find((EGOgiftModel x) => x.metaInfo.id == model.metaInfo.id);
		if (egogiftModel != null)
		{
			egogiftModel.OnRelease();
			addedGifts.Remove(egogiftModel);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel);
			return;
		}
		List<EGOgiftModel> replacedGifts = this._equipment.gifts.replacedGifts;
		EGOgiftModel egogiftModel2 = replacedGifts.Find((EGOgiftModel x) => x.metaInfo.id == model.metaInfo.id);
		if (egogiftModel2 != null)
		{
			egogiftModel2.OnRelease();
			replacedGifts.Remove(egogiftModel2);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel2);
			return;
		}*/
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x001728A4 File Offset: 0x00170AA4
	public bool ReleaseEGOGift(int id)
	{ // <Patch>
		return ReleaseEGOGift_Mod(new LobotomyBaseMod.LcId(id));
		/*
		List<EGOgiftModel> addedGifts = this._equipment.gifts.addedGifts;
		EGOgiftModel egogiftModel = addedGifts.Find((EGOgiftModel x) => x.metaInfo.id == id);
		if (egogiftModel != null)
		{
			egogiftModel.OnRelease();
			addedGifts.Remove(egogiftModel);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel);
			return true;
		}
		List<EGOgiftModel> replacedGifts = this._equipment.gifts.replacedGifts;
		EGOgiftModel egogiftModel2 = replacedGifts.Find((EGOgiftModel x) => x.metaInfo.id == id);
		if (egogiftModel2 != null)
		{
			egogiftModel2.OnRelease();
			replacedGifts.Remove(egogiftModel2);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel2);
			return true;
		}
		return false;*/
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x00033956 File Offset: 0x00031B56
	public void SetGiftDisplayState(EGOgiftModel model, bool state)
	{
		this._equipment.gifts.SetDisplayState(model, state);
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x0003396A File Offset: 0x00031B6A
	public bool GetGiftDisplayState(EGOgiftModel gift)
	{
		return this._equipment.gifts.GetDisplayState(gift);
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x0003397D File Offset: 0x00031B7D
	public void SetGiftLockState(EGOgiftModel model, bool state)
	{
		this._equipment.gifts.SetLockState(model, state);
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x00172960 File Offset: 0x00170B60
	public void SetKitCreature(CreatureModel kitCreature)
	{
		if (kitCreature != null && (kitCreature.metaInfo.creatureWorkType != CreatureWorkType.KIT || kitCreature.metaInfo.creatureKitType != CreatureKitType.EQUIP))
		{
			Debug.LogError("invalid kit creature");
			return;
		}
		if (this.Equipment.kitCreature != null)
		{
			Debug.LogError("alreay exists kitCreature");
			return;
		}
		this.Equipment.kitCreature = kitCreature;
		kitCreature.kitEquipOwner = (AgentModel)this;
		kitCreature.script.kitEvent.OnUseKit((AgentModel)this);
		this.OnSetKitCreature();
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x001729E4 File Offset: 0x00170BE4
	public void ReleaseKitCreature(bool stageEnd)
	{
		if (this.Equipment.kitCreature == null)
		{
			return;
		}
		CreatureModel kitCreature = this.Equipment.kitCreature;
		this.Equipment.kitCreature.kitEquipOwner = null;
		this.Equipment.kitCreature = null;
		kitCreature.script.kitEvent.OnReleaseKitEquip((AgentModel)this, stageEnd);
		this.OnReleaseKitCreature();
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnSetWeapon()
	{
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnReleaseWeapon()
	{
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnSetArmor()
	{
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnReleaseArmor()
	{
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnChangeGift()
	{
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnSetKitCreature()
	{
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void OnReleaseKitCreature()
	{
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x00021A3D File Offset: 0x0001FC3D
	public string GetWeaponSpriteSrc()
	{
		return string.Empty;
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x00003E72 File Offset: 0x00002072
	public virtual Sprite GetWeaponSprite()
	{
		return null;
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x00033991 File Offset: 0x00031B91
	public virtual void PrepareWeapon()
	{
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnPrepareWeapon(this);
		}
		if (this._equipment.armor != null)
		{
			this._equipment.armor.OnPrepareWeapon(this);
		}
	}

	// Token: 0x06003A85 RID: 14981 RVA: 0x00172A44 File Offset: 0x00170C44
	public virtual void CancelWeapon()
	{
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.remainDelay = 0f;
			this._equipment.weapon.OnCancelWeapon(this);
		}
		if (this._equipment.armor != null)
		{
			this._equipment.armor.OnCancelWeapon(this);
		}
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x00172AA4 File Offset: 0x00170CA4
	public virtual void Attack(UnitModel target)
	{
		if (this._equipment.weapon == null)
		{
			return;
		}
		if (this.IsAttackState())
		{
			return;
		}
		if (!this._equipment.weapon.InRange(this, target))
		{
			return;
		}
		if (this._equipment.kitCreature != null && this is AgentModel)
		{
			this._equipment.kitCreature.script.kitEvent.OnAttack(this as AgentModel, target);
		}
		if (this.movableNode.GetCurrentViewPosition().x > target.GetMovableNode().GetCurrentViewPosition().x)
		{
			this.movableNode.SetDirection(UnitDirection.LEFT);
		}
		else
		{
			this.movableNode.SetDirection(UnitDirection.RIGHT);
		}
		string animationName = this._equipment.weapon.OnAttack(this, target);
		this.PlayAttackAnimation(animationName);
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x000339CF File Offset: 0x00031BCF
	public virtual bool IsAttackState()
	{
		return this._equipment.weapon != null && this._equipment.weapon.remainDelay > 0f;
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x000339F7 File Offset: 0x00031BF7
	public virtual bool InWeaponRange(UnitModel target)
	{
		return this._equipment.weapon != null && this._equipment.weapon.InRange(this, target);
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x00033A1A File Offset: 0x00031C1A
	public void StopAttack()
	{
		WeaponModel weapon = this._equipment.weapon;
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x00033A28 File Offset: 0x00031C28
	public void OnGiveDamageByWeapon()
	{
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnGiveDamage(this);
		}
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x00172B6C File Offset: 0x00170D6C
	public float GetDamageFactorByEquipment()
	{ // <Mod> fixed unit bufs not fucking working
		float num = 1f;
		try
		{
			if (this._equipment == null)
			{
				return 1f;
			}
			if (this._equipment.weapon != null)
			{
				num *= this._equipment.weapon.GetDamageFactor();
			}
			if (this._equipment.weapon != null)
			{
				num *= this._equipment.armor.GetDamageFactor();
			}
			if (this._equipment.gifts != null)
			{
				num *= this._equipment.gifts.GetDamageFactor();
			}
			if (this._bufList.Count > 0)
			{
				foreach (UnitBuf unitBuf in this._bufList)
				{
					num *= unitBuf.GetDamageFactor();
				}
			}
		}
		catch (Exception)
		{
		}
		return num;
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x00021A33 File Offset: 0x0001FC33
	public virtual float GetDamageFactorBySefiraAbility()
	{
		return 1f;
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x00033A48 File Offset: 0x00031C48
	public void OnEndAttackCycle()
	{
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnEndAttackCycle();
		}
		this.tempAnim.EndAttack();
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x00033A72 File Offset: 0x00031C72
	protected virtual void PlayAttackAnimation(string animationName)
	{
		this.tempAnim.PlayAttackAnimation(new DummyAttackAnimator.Callback(this.OnEndAttackCycle));
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00003E35 File Offset: 0x00002035
	protected virtual void EndAttackAnimation()
	{
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00033A8B File Offset: 0x00031C8B
	public EGObonusInfo GetEGObonus()
	{
		return this._equipment.GetBonus(this);
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x00033A99 File Offset: 0x00031C99
	public bool HasEquipment(int id)
	{ // <Patch>
		return HasEquipment_Mod(new LobotomyBaseMod.LcId(id));
		// return this._equipment.HasEquipment(id);
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x00172C58 File Offset: 0x00170E58
	public void AddSuperArmorMax(float value)
	{
		Debug.Log("Superamror added: " + value);
		this.superArmorMax += value;
		this.superArmor += value;
		Debug.Log(string.Concat(new object[]
		{
			"CurrentSuperArmor : ",
			this.superArmorMax,
			" ",
			this.superArmor
		}));
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x00172CD4 File Offset: 0x00170ED4
	public void SubSuperArmorMax(float value)
	{
		this.superArmorMax -= value;
		if (this.superArmorMax < 0f)
		{
			this.superArmorMax = 0f;
		}
		if (this.superArmor > this.superArmorMax)
		{
			this.superArmor = this.superArmorMax;
		}
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00033AA7 File Offset: 0x00031CA7
	public void TakeDamage(DamageInfo dmg)
	{
		this.TakeDamage(null, dmg);
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void TakeDamage(UnitModel actor, DamageInfo dmg)
	{
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void TakeDamageWithoutEffect(UnitModel actor, DamageInfo dmg)
	{
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x00033AB1 File Offset: 0x00031CB1
	public virtual void MakeDamageEffect(RwbpType type, float value, DefenseInfo.Type defense)
	{ // <Mod> Overtime Yesod Suppression
		if (ResearchDataModel.instance.IsUpgradedAbility("damage_text") || (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && GlobalGameManager.instance.tutorialStep > 1))
		{
			if (!(this is AgentModel) || !(this as AgentModel).ForceHideUI)
			{
				DamageEffect.Invoker(this.movableNode, type, (int)value, defense, this);
			}
		}
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void UnderAttack(UnitModel attacker)
	{
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x00033AEF File Offset: 0x00031CEF
	public void ClearWorkerEncounting()
	{
		this.encounteredWorker.Clear();
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x00172D24 File Offset: 0x00170F24
	public void CheckNearWorkerEncounting()
	{
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage != null)
		{
			try
			{
				foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (unit != this && unit is WorkerModel)
					{
						WorkerModel workerModel = (WorkerModel)unit;
						if (!workerModel.IsDead() && !this.encounteredWorker.Contains(workerModel) && (!(workerModel is AgentModel) || (workerModel as AgentModel).activated))
						{
							this.encounteredWorker.Add(workerModel);
							workerModel.InitialEncounteredCreature(this);
							if (!workerModel.returnPanic)
							{
								workerModel.EncounterCreature(this);
							}
						}
					}
				}
			}
			catch (InvalidOperationException message)
			{
				Debug.Log(message);
			}
		}
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x00033AFC File Offset: 0x00031CFC
	public virtual bool IsStunned()
	{
		return this.isStun;
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSuperArmorBreak()
	{
	}

	// Token: 0x06003A9D RID: 15005 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool IsAttackTargetable()
	{
		return false;
	}

	// Token: 0x06003A9E RID: 15006 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool IsHostile(UnitModel target)
	{
		return false;
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x00033B04 File Offset: 0x00031D04
	public void SetMoveDelay(float moveDelay)
	{
		this.remainMoveDelay = moveDelay;
	}

	// Token: 0x06003AA0 RID: 15008 RVA: 0x00003E35 File Offset: 0x00002035
	public void SetAttackDelay()
	{
	}

	// Token: 0x06003AA1 RID: 15009 RVA: 0x0002FA4D File Offset: 0x0002DC4D
	public void SetAttackDelay(float value)
	{
		this.remainAttackDelay = value;
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x00172DF8 File Offset: 0x00170FF8
	public virtual void UpdateBufState()
	{
		UnitBuf[] array = this._bufList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FixedUpdate();
		}
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual int GetRiskLevel()
	{
		return 1;
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual int GetAttackLevel()
	{
		return 1;
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual int GetDefenseLevel()
	{
		return 1;
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x00172E28 File Offset: 0x00171028
	public UnitBuf AddUnitBuf(UnitBuf buf)
	{ // <Mod>
		if (buf.duplicateType == BufDuplicateType.ONLY_ONE)
		{
			UnitBuf unitBuf;
			if (buf.type != UnitBufType.ADD_SUPERARMOR && buf.type != UnitBufType.UNKNOWN)
			{
				unitBuf = this.GetUnitBufByType(buf.type);
			}
			else
			{
				unitBuf = this.GetUnitBufByName(buf.GetType().Name);
			}
			if (unitBuf != null)
			{
				this.RemoveUnitBuf(unitBuf);
			}
		}
		buf.Init(this);
		this._bufList.Add(buf);
		if (buf is UnitStatBuf)
		{
			this._statBufList.Add((UnitStatBuf)buf);
		}
		if (buf is BarrierBuf)
		{
			this._barrierBufList.Add((BarrierBuf)buf);
		}
		return buf;
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x00172EB8 File Offset: 0x001710B8
	public bool HasUnitBuf(UnitBufType type)
	{
		using (List<UnitBuf>.Enumerator enumerator = this._bufList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.type == type)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x00172F14 File Offset: 0x00171114
	public UnitBuf GetUnitBufByType(UnitBufType type)
	{
		UnitBuf result = null;
		foreach (UnitBuf unitBuf in this._bufList)
		{
			if (unitBuf.type == type)
			{
				result = unitBuf;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x00172F70 File Offset: 0x00171170
	public void RemoveUnitBuf(UnitBuf buf)
	{
		if (this._bufList.Contains(buf))
		{
			this._bufList.Remove(buf);
			if (buf is UnitStatBuf)
			{
				this._statBufList.Remove((UnitStatBuf)buf);
			}
			if (buf is BarrierBuf)
			{
				this._barrierBufList.Remove((BarrierBuf)buf);
			}
			buf.OnDestroy();
		}
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x00172FD4 File Offset: 0x001711D4
	public int GetMaxHpBuf()
	{
		int num = 0;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.maxHp;
		}
		return num;
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x0017302C File Offset: 0x0017122C
	public int GetMaxMentalBuf()
	{
		int num = 0;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.maxMental;
		}
		return num;
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x00173084 File Offset: 0x00171284
	public int GetCubeSpeedBuf()
	{
		int num = 0;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.cubeSpeed;
		}
		return num;
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x001730DC File Offset: 0x001712DC
	public int GetWorkProbBuf()
	{
		int num = 0;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.workProb;
		}
		return num;
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x00173134 File Offset: 0x00171334
	public float GetAttackSpeedBuf()
	{
		float num = 0f;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.attackSpeed;
		}
		return num;
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x00173190 File Offset: 0x00171390
	public float GetMovementBuf()
	{
		float num = 0f;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			num += unitStatBuf.movementSpeed;
		}
		return num;
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x001731EC File Offset: 0x001713EC
	public WorkerPrimaryStatBonus GetPrimaryStatBuf()
	{
		WorkerPrimaryStatBonus workerPrimaryStatBonus = new WorkerPrimaryStatBonus();
		workerPrimaryStatBonus.hp = 0;
		workerPrimaryStatBonus.mental = 0;
		workerPrimaryStatBonus.work = 0;
		workerPrimaryStatBonus.battle = 0;
		foreach (UnitStatBuf unitStatBuf in this._statBufList)
		{
			workerPrimaryStatBonus.hp += unitStatBuf.primaryStat.hp;
			workerPrimaryStatBonus.mental += unitStatBuf.primaryStat.mental;
			workerPrimaryStatBonus.work += unitStatBuf.primaryStat.work;
			workerPrimaryStatBonus.battle += unitStatBuf.primaryStat.battle;
		}
		return workerPrimaryStatBonus;
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x001732BC File Offset: 0x001714BC
	public float GetMovementScaleByBuf()
	{
		float num = 1f;
		foreach (UnitBuf unitBuf in this._bufList)
		{
			num *= unitBuf.MovementScale();
		}
		return num;
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x00033B0D File Offset: 0x00031D0D
	public virtual void SetFaction(FactionTypeInfo type)
	{
		this.factionTypeInfo = type;
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x00173318 File Offset: 0x00171518
	public virtual void SetFaction(string factionCode)
	{
		FactionTypeInfo faction = FactionTypeList.instance.GetFaction(factionCode);
		this.factionTypeInfo = faction;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00033B16 File Offset: 0x00031D16
	public FactionTypeInfo GetFaction()
	{
		return this.factionTypeInfo;
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x00173318 File Offset: 0x00171518
	public void SetFactionForcely(string factionCode)
	{
		FactionTypeInfo faction = FactionTypeList.instance.GetFaction(factionCode);
		this.factionTypeInfo = faction;
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x00033B1E File Offset: 0x00031D1E
	public virtual void OnStun(float stunVal)
	{
		this.isStun = true;
		if (!this.stunTimer.isInitialized)
		{
			this.stunTimer.Init();
		}
		this.stunTimer.StartTimer(stunVal, new AutoTimer.TargetMethod(this.OnStunEnd), AutoTimer.UpdateMode.FIXEDUPDATE);
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x00033B59 File Offset: 0x00031D59
	public virtual void OnStunEnd()
	{
		this.isStun = false;
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x00173338 File Offset: 0x00171538
	public static float GetDmgMultiplierByEgoLevel(int attackLevel, int defenseLevel)
	{
		int num = attackLevel - defenseLevel;
		if (num <= -4)
		{
			return 0.4f;
		}
		if (num == -3)
		{
			return 0.6f;
		}
		if (num == -2)
		{
			return 0.7f;
		}
		if (num == -1)
		{
			return 0.8f;
		}
		if (num == 0)
		{
			return 1f;
		}
		if (num == 1)
		{
			return 1f;
		}
		if (num == 2)
		{
			return 1.2f;
		}
		if (num == 3)
		{
			return 1.5f;
		}
		return 2f;
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x001733A0 File Offset: 0x001715A0
	public virtual float GetBufDamageMultiplier(UnitModel attacker, DamageInfo damage)
	{
		float num = 1f;
		foreach (UnitBuf unitBuf in this._bufList)
		{
			num *= unitBuf.OnTakeDamage(attacker, damage);
		}
		return num;
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00173400 File Offset: 0x00171600
	public UnitBuf GetUnitBufByName(string name)
	{
		UnitBuf result = null;
		foreach (UnitBuf unitBuf in this._bufList)
		{
			if (unitBuf.GetType().Name == name)
			{
				result = unitBuf;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x00033B62 File Offset: 0x00031D62
	public List<UnitBuf> GetUnitBufList()
	{
		return this._bufList;
	}

	// <Patch>
	public bool HasEquipment_Mod(LobotomyBaseMod.LcId id)
	{
		return this._equipment.HasEquipment_Mod(id);
	}

	// <Patch>
	public bool ReleaseEGOGift_Mod(LobotomyBaseMod.LcId id)
	{
		List<EGOgiftModel> addedGifts = this._equipment.gifts.addedGifts;
		EGOgiftModel egogiftModel = addedGifts.Find((EGOgiftModel x) => EquipmentTypeInfo.GetLcId(x.metaInfo) == id);
		if (egogiftModel != null)
		{
			egogiftModel.OnRelease();
			addedGifts.Remove(egogiftModel);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel);
			return true;
		}
		List<EGOgiftModel> replacedGifts = this._equipment.gifts.replacedGifts;
		EGOgiftModel egogiftModel2 = replacedGifts.Find((EGOgiftModel x) => EquipmentTypeInfo.GetLcId(x.metaInfo) == id);
		if (egogiftModel2 != null)
		{
			egogiftModel2.OnRelease();
			replacedGifts.Remove(egogiftModel2);
			this.OnChangeGift();
			this._equipment.gifts.ReleaseGift(egogiftModel2);
			return true;
		}
		return false;
	}

	// <Mod>
	public List<UnitStatBuf> GetStatBufList()
	{
		return _statBufList;
	}

	// <Mod>
	public List<BarrierBuf> GetBarrierBufList()
	{
		return _barrierBufList;
	}
	
	// <Mod>
	public virtual bool IsEtcUnit()
	{
		return false;
	}
	
	// <Mod>
	public virtual bool IgnoreDoors()
	{
		return false;
	}

	// Token: 0x04003612 RID: 13842
	public const float stunCriteria = 2f;

	// Token: 0x04003613 RID: 13843
	public const string defaultStunEffectSrc = "Effect/Stun";

	// Token: 0x04003614 RID: 13844
	public long instanceId;

	// Token: 0x04003615 RID: 13845
	protected MovableObjectNode movableNode;

	// Token: 0x04003616 RID: 13846
	public UnitShieldEquipment shield;

	// Token: 0x04003617 RID: 13847
	protected UnitEquipSpace _equipment = new UnitEquipSpace();

	// Token: 0x04003618 RID: 13848
	public DummyAttackAnimator tempAnim = new DummyAttackAnimator();

	// Token: 0x04003619 RID: 13849
	protected FactionTypeInfo factionTypeInfo;

	// Token: 0x0400361A RID: 13850
	protected AutoTimer stunTimer = new AutoTimer();

	// Token: 0x0400361B RID: 13851
	public float hp;

	// Token: 0x0400361C RID: 13852
	public float mental;

	// Token: 0x0400361D RID: 13853
	public int baseMaxHp;

	// Token: 0x0400361E RID: 13854
	public int baseMaxMental;

	// Token: 0x0400361F RID: 13855
	public float baseMovement;

	// Token: 0x04003620 RID: 13856
	public int baseRegeneration;

	// Token: 0x04003621 RID: 13857
	public float baseRegenerationDelay = 5f;

	// Token: 0x04003622 RID: 13858
	public DefenseInfo additionalDef = new DefenseInfo();

	// Token: 0x04003623 RID: 13859
	public float superArmorMax;

	// Token: 0x04003624 RID: 13860
	public float superArmor;

	// Token: 0x04003625 RID: 13861
	public float superArmorDefense;

	// Token: 0x04003626 RID: 13862
	public float remainMoveDelay;

	// Token: 0x04003627 RID: 13863
	public float remainAttackDelay;

	// Token: 0x04003628 RID: 13864
	protected bool isStun;

	// Token: 0x04003629 RID: 13865
	public StatTransform damageTransform = new IdentityTransform();

	// Token: 0x0400362A RID: 13866
	public int basePhysicalDefense;

	// Token: 0x0400362B RID: 13867
	public int baseMentalDefense;

	// Token: 0x0400362C RID: 13868
	public List<WorkerModel> encounteredWorker = new List<WorkerModel>();

	// Token: 0x0400362D RID: 13869
	protected List<UnitBuf> _bufList = new List<UnitBuf>();

	// Token: 0x0400362E RID: 13870
	protected List<UnitStatBuf> _statBufList = new List<UnitStatBuf>();

	// Token: 0x0400362F RID: 13871
	protected List<BarrierBuf> _barrierBufList = new List<BarrierBuf>();
}
