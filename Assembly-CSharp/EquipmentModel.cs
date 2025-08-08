/*
public float GetDamageFactor() // Weapons that don't scale with attack speed instead scale damage
+public void OnTakeDamage_After(UnitModel actor, DamageInfo dmg) // 
public bool CheckRequire(UnitModel unit) // 
+public int GetUpgradeRisk // 
*/
using System;
using System.Linq; //
using UnityEngine;
using LobotomyBaseMod; // 

// Token: 0x02000693 RID: 1683
public class EquipmentModel
{
	// Token: 0x060036D3 RID: 14035 RVA: 0x000313D1 File Offset: 0x0002F5D1
	public EquipmentModel()
	{
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060036D4 RID: 14036 RVA: 0x000313E4 File Offset: 0x0002F5E4
	public UnitModel owner
	{
		get
		{
			return this._owner;
		}
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x000313EC File Offset: 0x0002F5EC
	public virtual void OnFixedUpdate()
	{
		this.script.OnFixedUpdate();
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x000313F9 File Offset: 0x0002F5F9
	public void OnPrepareWeapon(UnitModel actor)
	{
		this.script.OnPrepareWeapon(actor);
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00031407 File Offset: 0x0002F607
	public float GetDamageFactor()
	{ // <Mod>
		float num = 1f;
		if (this.owner is AgentModel && this.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON) {
			if (this.metaInfo.weaponClassType == WeaponClassType.PISTOL || EquipmentTypeInfo.NonScaleWeaponIds.Contains(this.metaInfo.LcId)) {
				int grade = this.GetUpgradeRisk;
				float assume = (float)EquipmentTypeInfo.NonScaleJust[grade - 1];
				AgentModel agent = this.owner as AgentModel;
				float num2 = 1f;
				if (this.metaInfo.weaponClassType != WeaponClassType.PISTOL) {
					num2 = 0.8f + assume / 143f;
				}
				float num3 = (0.8f + agent.attackSpeed / 143f);
				num *= num3 / num2;
			}
			num *= 1f + EGOrealizationManager.instance.WeaponUpgrade(this.metaInfo);
		}
		if (this.script == null)
		{
			return num;
		}
		return this.script.GetDamageFactor() * num;
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x00031425 File Offset: 0x0002F625
	public void OnCancelWeapon(UnitModel actor)
	{
		this.script.OnCancelWeapon(actor);
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x00031433 File Offset: 0x0002F633
	public void OnEquip(UnitModel newOwner)
	{
		this._owner = newOwner;
		this.currentTarget = null;
		this.script.OnEquip(newOwner);
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x0003144F File Offset: 0x0002F64F
	public void OnRelease()
	{
		this._owner = null;
		this.currentTarget = null;
		this.script.OnRelease();
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x0003146A File Offset: 0x0002F66A
	public void OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		this.script.OnTakeDamage(actor, ref dmg);
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x0003147A File Offset: 0x0002F67A
	public void OnTakeDamage_After(float value, RwbpType type)
	{
		this.script.OnTakeDamage_After(value, type);
	}

	// <Mod>
	public void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
	{
		script.OnTakeDamage_After(actor, dmg);
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x00163344 File Offset: 0x00161544
	public bool CheckRequire(UnitModel unit)
	{ // <Patch> <Mod>
		if (!(unit is AgentModel))
		{
			return true;
		}
		AgentModel agent = (AgentModel)unit;
		LcId lcId = EquipmentTypeInfo.GetLcId(this.metaInfo);
		if (lcId == 300034 || lcId == 200034)
		{
			if (!agent.spriteData.FrontHair.Equals(Resources.Load<Sprite>("Sprites/Worker/Basic/Hair/Front/Bald")))
			{
				return false;
			}
			if (!agent.spriteData.FrontHair.Equals(Resources.Load<Sprite>("Sprites/Worker/Basic/Hair/Front/Bald")))
			{
				return false;
			}
		}
		if (lcId == new LcId("NotbaconOvertimeMod", 230905) || lcId == new LcId("NotbaconOvertimeMod", 330905))
		{
			return true;
		}
		foreach (EgoRequire egoRequire in metaInfo.requires)
		{
			int level = 1;
			switch (egoRequire.type)
			{
				case EgoRequireType.level:
					level = agent.level;
					break;
				case EgoRequireType.R:
					level = AgentModel.CalculateStatLevelForCustomizing(agent.fortitudeStat);
					break;
				case EgoRequireType.W:
					level = AgentModel.CalculateStatLevelForCustomizing(agent.prudenceStat);
					break;
				case EgoRequireType.B:
					level = AgentModel.CalculateStatLevelForCustomizing(agent.temperanceStat);
					break;
				case EgoRequireType.P:
					level = AgentModel.CalculateStatLevelForCustomizing(agent.justiceStat);
					break;
			}
			if (level < egoRequire.value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x0003148A File Offset: 0x0002F68A
	public EGObonusInfo GetBonus(UnitModel actor)
	{
		return this.script.GetBonus(actor);
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x00031498 File Offset: 0x0002F698
	public float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		return this.script.GetWorkProbSpecialBonus(actor, skill);
	}
    
    // <Mod>
    public int GetUpgradeRisk
	{
		get
		{
			if (EquipmentTypeInfo.BossIds.Contains(this.metaInfo.LcId)) {
				return 6;
			}
			return (int)this.metaInfo.Grade + 1;
		}
	}

	// Token: 0x0400329D RID: 12957
	public EquipmentTypeInfo metaInfo;

	// Token: 0x0400329E RID: 12958
	public long instanceId;

	// Token: 0x0400329F RID: 12959
	private UnitModel _owner;

	// Token: 0x040032A0 RID: 12960
	public EquipmentScriptBase script = new EquipmentScriptBase();

	// Token: 0x040032A1 RID: 12961
	public UnitModel currentTarget;
}
