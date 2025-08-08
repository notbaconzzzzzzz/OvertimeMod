/*
public override void OrdealEnd() // Overtime Core Suppressions
Various private static arrays // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public class MachineOrdeal : OrdealBase
{
	// Token: 0x06003C52 RID: 15442 RVA: 0x000351C5 File Offset: 0x000333C5
	public MachineOrdeal()
	{
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x00179850 File Offset: 0x00177A50
	public virtual MachineOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node)
	{
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)MachineOrdeal.ids[(int)level], node, this);
		(ordealCreatureModel.script as MachineOrdealCreature).SetOrdeal(this, level, MachineOrdeal.risks[(int)level], MachineOrdeal.names[(int)level]);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		return ordealCreatureModel.script as MachineOrdealCreature;
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x001798AC File Offset: 0x00177AAC
	public override RiskLevel GetRiskLevel(OrdealCreatureModel creature)
	{
		if (creature == null)
		{
			return base.GetRiskLevel(creature);
		}
		MachineOrdealCreature machineOrdealCreature = creature.script as MachineOrdealCreature;
		if (machineOrdealCreature != null)
		{
			return machineOrdealCreature.Risk;
		}
		return base.GetRiskLevel(creature);
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x001798E8 File Offset: 0x00177AE8
	public override string OrdealNameText(OrdealCreatureModel ordeal)
	{
		if (ordeal == null)
		{
			return base.OrdealNameText(ordeal);
		}
		MachineOrdealCreature machineOrdealCreature = ordeal.script as MachineOrdealCreature;
		if (machineOrdealCreature != null)
		{
			string id = string.Format("ordeal_{0}_name", machineOrdealCreature.GetOrdealName());
			return LocalizeTextDataModel.instance.GetText(id);
		}
		return base.OrdealNameText(ordeal);
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x000351FD File Offset: 0x000333FD
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.isStarted)
		{
			return;
		}
		if (this.CheckCloseCondition())
		{
			this.OrdealEnd();
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x00035222 File Offset: 0x00033422
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x00035232 File Offset: 0x00033432
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x0017993C File Offset: 0x00177B3C
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Machine/Machine_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x001799AC File Offset: 0x00177BAC
	public override void OrdealEnd()
	{ // <Mod>
		base.OrdealEnd();
		int num = this.ordealRewards[(int)this.level];
		if (!base.canTakeRewards)
		{
			num = 0;
		}
		EnergyModel.instance.AddEnergy(StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay()) * (float)num * 0.01f / StageTypeInfo.instnace.GetPercentEnergyFactor());
		num = (int)((float)num / StageTypeInfo.instnace.GetPercentEnergyFactor());
		this.OrdealTypo(this._ordealName, this._color, false, num);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Machine/Machine_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00179A4C File Offset: 0x00177C4C
	// Note: this type is marked as 'beforefieldinit'.
	static MachineOrdeal()
	{
	}

	// Token: 0x0400371A RID: 14106
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x0400371B RID: 14107
	protected readonly Color _color = new Color(0.4117647f, 0.6431373f, 0.28235295f);

	// Token: 0x0400371C RID: 14108
	private static int[] ids = new int[]
	{ // <Mod>
		200001,
		200002,
		200003,
		200004,
		200101,
		200102,
		200103,
		200104
	};

	// Token: 0x0400371D RID: 14109
	private static RiskLevel[] risks = new RiskLevel[]
	{ // <Mod>
		RiskLevel.TETH,
		RiskLevel.HE,
		RiskLevel.WAW,
		RiskLevel.ALEPH,
		RiskLevel.TETH,
		RiskLevel.HE,
		RiskLevel.WAW,
		RiskLevel.ALEPH
	};

	// Token: 0x0400371E RID: 14110
	private static string[] names = new string[]
	{ // <Mod>
		"machine_dawn",
		"machine_noon",
		"machine_dusk",
		"machine_midnight",
		"machine_dawn",
		"machine_noon",
		"machine_dusk",
		"machine_midnight"
	};

	// Token: 0x0400371F RID: 14111
	protected string _ordealName = "machine_dawn";
}
