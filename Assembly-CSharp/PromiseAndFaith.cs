using System;
using System.Collections.Generic;

// Token: 0x02000456 RID: 1110
public class PromiseAndFaith : CreatureBase
{
	// Token: 0x060027BC RID: 10172 RVA: 0x00027927 File Offset: 0x00025B27
	public PromiseAndFaith()
	{
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x001186D4 File Offset: 0x001168D4
	public void DetermineReinforcement()
	{
		bool flag = this.Prob(85 - this._curAgent.Equipment.weapon.script.reinforcementLevel * 15);
		if (flag)
		{
			this._anim.animator.SetTrigger("Success");
			this._curAgent.workerAnimator.SetTrigger("Success");
			this._curAgent.Equipment.weapon.script.AddReinforcementLevel(1);
		}
		else
		{
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
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x00027945 File Offset: 0x00025B45
	public void ExitReinforcementProcess()
	{
		this._workTime = 1.5f;
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x001187F4 File Offset: 0x001169F4
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._anim = (PromiseAndFaithAnim)this.model.Unit.animTarget;
		this._anim.SetScript(this);
		GlobalGameManager.instance.TryGetGlobalData(out this._savedData);
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x0001E573 File Offset: 0x0001C773
	public override void OnStageStart()
	{
		base.OnStageStart();
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x00027952 File Offset: 0x00025B52
	public override void OnStageEnd()
	{
		base.OnStageEnd();
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x00118840 File Offset: 0x00116A40
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		this._curAgent = skill.agent;
		WeaponModel weapon = this._curAgent.Equipment.weapon;
		float num = this.CalculateEnergyCost(weapon.script.reinforcementLevel);
		float energy = EnergyModel.instance.GetEnergy();
		float num2 = num * energy;
		if (weapon.metaInfo.id != 1 && energy > num2)
		{
			this._workTime = 30f;
			EnergyModel.instance.SubEnergy(num2);
			this._anim.animator.SetTrigger("Start");
		}
		else
		{
			skill.CancelWork();
		}
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x0002795A File Offset: 0x00025B5A
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

	// Token: 0x060027C4 RID: 10180 RVA: 0x0002799A File Offset: 0x00025B9A
	public override float GetKitCreatureProcessTime()
	{
		return this._workTime;
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x001188E0 File Offset: 0x00116AE0
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
