/*
public override void OnStageStart() // 
public override void OnFinishWork(UseSkill skill) // 
private void UpdateHandState(BloodBath.BloodHandState handState) // 
public void FinishKilling(AgentUnit targetUnit) // 
+private EnergyModel.CreatureEnergyIncome energyIncome // 
*/
using System;

// Token: 0x020003B2 RID: 946
public class BloodBath : CreatureBase
{
	// Token: 0x06001E14 RID: 7700 RVA: 0x0002046D File Offset: 0x0001E66D
	public BloodBath()
	{
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x0002048A File Offset: 0x0001E68A
	public override void OnInit()
	{
		base.OnInit();
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x00020492 File Offset: 0x0001E692
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.bathAnim = (BloodBathAnim)unit.animTarget;
		this.bathAnim.Init(this);
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000204B8 File Offset: 0x0001E6B8
	public override void OnStageStart()
	{ // <Mod>
		base.OnStageStart();
		this.currentHandState = BloodBath.BloodHandState.ZERO;
		this.bathAnim.UpdateHandState(this.currentHandState);
        energyIncome = new EnergyModel.CreatureEnergyIncome();
        energyIncome.model = model;
        energyIncome.baseCap = 0;
        energyIncome.flatRate = 0;
        EnergyModel.instance.AddIncome(energyIncome);
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000204D8 File Offset: 0x0001E6D8
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		if (this.currentHandState == BloodBath.BloodHandState.THREE)
		{
			this.KillAgent(skill.agent);
		}
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000F2CC0 File Offset: 0x000F0EC0
	public override void OnFinishWork(UseSkill skill)
	{ // <Mod>
		int num;
		switch (this.currentHandState)
		{
		case BloodBath.BloodHandState.ONE:
			num = 1;
			break;
		case BloodBath.BloodHandState.TWO:
			num = 2;
			break;
		case BloodBath.BloodHandState.THREE:
			num = 3;
			break;
		default:
			num = 0;
			break;
		}
		if (!SpecialModeConfig.instance.GetValue<bool>("SpiderBudAndBloodbathEnergy"))
		{
			skill.successCount += num;
		}
		if (this.currentHandState != BloodBath.BloodHandState.THREE)
		{
			int fortitudeLevel = skill.agent.fortitudeLevel;
			int temperanceLevel = skill.agent.temperanceLevel;
			if ((fortitudeLevel == this._CONDITION_FORTITUDE_LEVEL || temperanceLevel == this._CONDITION_TEMPARENCE_LEVEL) && !skill.agent.invincible)
			{
				this.KillAgent(skill.agent);
			}
		}
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x0001EE69 File Offset: 0x0001D069
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000204F9 File Offset: 0x0001E6F9
	private void UpdateHandState(BloodBath.BloodHandState handState)
	{ // <Mod>
		this.currentHandState = handState;
		this.bathAnim.UpdateHandState(this.currentHandState);
		if (SpecialModeConfig.instance.GetValue<bool>("SpiderBudAndBloodbathEnergy"))
		{
			switch (this.currentHandState)
			{
			case BloodBath.BloodHandState.ONE:
				energyIncome.baseCap = 10;
				energyIncome.flatRate = 5;
				break;
			case BloodBath.BloodHandState.TWO:
				energyIncome.baseCap = 20;
				energyIncome.flatRate = 10;
				break;
			case BloodBath.BloodHandState.THREE:
				energyIncome.baseCap = 40;
				energyIncome.flatRate = 15;
				break;
			default:
				energyIncome.baseCap = 0;
				energyIncome.flatRate = 0;
				energyIncome.current = 0;
				break;
			}
		}
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x00020513 File Offset: 0x0001E713
	private void KillAgent(AgentModel agent)
	{
		agent.StopAction();
		agent.LoseControl();
		agent.GetMovableNode().MoveToNode(this.model.GetCustomNode(), false);
		this.model.state = CreatureState.WORKING_SCENE;
		this.bathAnim.PlayKillScene(agent);
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000F2D74 File Offset: 0x000F0F74
	public void FinishKilling(AgentUnit targetUnit)
	{ // <Mod>
		targetUnit.model.Die();
		targetUnit.gameObject.SetActive(false);
		if (this.model.state == CreatureState.WORKING_SCENE)
		{
			this.model.state = CreatureState.WAIT;
		}
		switch (this.currentHandState)
		{
		case BloodBath.BloodHandState.ZERO:
			this.UpdateHandState(BloodBath.BloodHandState.ONE);
			break;
		case BloodBath.BloodHandState.ONE:
			this.UpdateHandState(BloodBath.BloodHandState.TWO);
			break;
		case BloodBath.BloodHandState.TWO:
			this.UpdateHandState(BloodBath.BloodHandState.THREE);
			break;
		case BloodBath.BloodHandState.THREE:
			this.UpdateHandState(BloodBath.BloodHandState.ZERO);
			break;
		}
	}

	// Token: 0x04001E6F RID: 7791
	private readonly int _CONDITION_FORTITUDE_LEVEL = 1;

	// Token: 0x04001E70 RID: 7792
	private readonly int _CONDITION_TEMPARENCE_LEVEL = 1;

	// Token: 0x04001E71 RID: 7793
	private BloodBathAnim bathAnim;

	// Token: 0x04001E72 RID: 7794
	private BloodBath.BloodHandState currentHandState = BloodBath.BloodHandState.ONE;

    // <Mod>
    private EnergyModel.CreatureEnergyIncome energyIncome;

	// Token: 0x020003B3 RID: 947
	public enum BloodHandState
	{
		// Token: 0x04001E74 RID: 7796
		ZERO,
		// Token: 0x04001E75 RID: 7797
		ONE,
		// Token: 0x04001E76 RID: 7798
		TWO,
		// Token: 0x04001E77 RID: 7799
		THREE
	}
}
