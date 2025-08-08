/*
public override void OnEnterRoom(UseSkill skill) // 
*/
using System;
using System.Collections.Generic;

// Token: 0x02000456 RID: 1110
public class PromiseAndFaith : CreatureBase
{
	// Token: 0x060027BC RID: 10172
	public PromiseAndFaith()
	{
	}

	// Token: 0x060027BD RID: 10173
	public void DetermineReinforcement()
	{
		if (this.Prob(85 - this._curAgent.Equipment.weapon.script.reinforcementLevel * 15))
		{
			this._anim.animator.SetTrigger("Success");
			this._curAgent.workerAnimator.SetTrigger("Success");
			this._curAgent.Equipment.weapon.script.AddReinforcementLevel(1);
			return;
		}
		this._anim.animator.SetTrigger("Fail");
		this._curAgent.workerAnimator.SetTrigger("Fail");
		long instanceId = this._curAgent.Equipment.weapon.instanceId;
		InventoryModel.Instance.RemoveEquipment(this._curAgent.Equipment.weapon);
		this._curAgent.ReleaseWeaponV2();
		this._isFailed = true;
		Dictionary<string, object> globalSaveData = InventoryModel.Instance.GetGlobalSaveData();
		this._savedData["inventory"] = globalSaveData;
		GlobalGameManager.instance.TrySetGlobalInventoryData(this._savedData);
	}

	// Token: 0x060027BE RID: 10174
	public void ExitReinforcementProcess()
	{
		this._workTime = 1.5f;
	}

	// Token: 0x060027BF RID: 10175
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._anim = (PromiseAndFaithAnim)this.model.Unit.animTarget;
		this._anim.SetScript(this);
		GlobalGameManager.instance.TryGetGlobalData(out this._savedData);
	}

	// Token: 0x060027C0 RID: 10176
	public override void OnStageStart()
	{
		base.OnStageStart();
	}

	// Token: 0x060027C1 RID: 10177
	public override void OnStageEnd()
	{
		base.OnStageEnd();
	}

	// Token: 0x060027C2 RID: 10178
	public override void OnEnterRoom(UseSkill skill)
	{ // <Mod>
		base.OnEnterRoom(skill);
		this._curAgent = skill.agent;
		WeaponModel weapon = this._curAgent.Equipment.weapon;
		float num3 = this.CalculateEnergyCost(weapon.script.reinforcementLevel);
		float energy = EnergyModel.instance.GetEnergy();
		float num2 = num3 * energy;
        if (ResearchDataModel.instance.IsUpgradedAbility("energy_discount"))
        {
            num2 *= 0.9f;
        }
		if (weapon.metaInfo.id != 1 && energy > num2)
		{
			this._workTime = 30f;
			EnergyModel.instance.SubEnergy(num2);
			this._anim.animator.SetTrigger("Start");
			return;
		}
		skill.CancelWork();
	}

	// Token: 0x060027C3 RID: 10179
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		this._anim.animator.SetTrigger("Default");
		if (this._isFailed)
		{
			skill.agent.SetWeapon(WeaponModel.GetDummyWeapon());
			this._isFailed = false;
		}
	}

	// Token: 0x060027C4 RID: 10180
	public override float GetKitCreatureProcessTime()
	{
		return this._workTime;
	}

	// Token: 0x060027C5 RID: 10181
	private float CalculateEnergyCost(int lv)
	{
		float result;
		switch (lv)
		{
		case 0:
			result = 0.02f;
			break;
		case 1:
			result = 0.02f;
			break;
		case 2:
			result = 0.03f;
			break;
		case 3:
			result = 0.05f;
			break;
		case 4:
			result = 0.12f;
			break;
		default:
			result = 0.05f;
			break;
		}
		return result;
	}

	// Token: 0x04002667 RID: 9831
	private PromiseAndFaithAnim _anim;

	// Token: 0x04002668 RID: 9832
	private AgentModel _curAgent;

	// Token: 0x04002669 RID: 9833
	private Dictionary<string, object> _savedData = new Dictionary<string, object>();

	// Token: 0x0400266A RID: 9834
	private float _workTime = 1.5f;

	// Token: 0x0400266B RID: 9835
	private bool _isFailed;
}
