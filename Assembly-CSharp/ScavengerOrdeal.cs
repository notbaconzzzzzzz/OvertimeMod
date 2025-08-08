using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public class ScavengerOrdeal : OrdealBase
{
	// Token: 0x06003CEC RID: 15596 RVA: 0x000358BF File Offset: 0x00033ABF
	public ScavengerOrdeal()
	{
		this.SetColor();
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x000358EE File Offset: 0x00033AEE
	protected void SetColor()
	{
		this._color = UIColorManager.instance.GetSefiraColor(SefiraManager.instance.GetSefira(SefiraEnum.CHESED)).imageColor;
		this.OrdealColor = this._color;
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x0017BB44 File Offset: 0x00179D44
	public ScavengerOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node, params UnitDirection[] direction)
	{
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)ScavengerOrdeal.ids[(int)level], node, this);
		(ordealCreatureModel.script as ScavengerOrdealCreature).SetOrdeal(this, level);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		UnitDirection direction2 = UnitDirection.LEFT;
		if (direction.Length > 0)
		{
			direction2 = direction[0];
		}
		else if ((double)UnityEngine.Random.value < 0.5)
		{
			direction2 = UnitDirection.RIGHT;
		}
		ordealCreatureModel.GetMovableNode().SetDirection(direction2);
		return ordealCreatureModel.script as ScavengerOrdealCreature;
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x0003591C File Offset: 0x00033B1C
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

	// Token: 0x06003CF0 RID: 15600 RVA: 0x00035941 File Offset: 0x00033B41
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x00035951 File Offset: 0x00033B51
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x0017BBC8 File Offset: 0x00179DC8
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.SetColor();
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Scavenger/Scavenger_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x0017BC3C File Offset: 0x00179E3C
	public override void OrdealEnd()
	{
		base.OrdealEnd();
		this.SetColor();
		int num = this.ordealRewards[(int)this.level];
		if (!base.canTakeRewards)
		{
			num = 0;
		}
		EnergyModel.instance.AddEnergy(StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay()) * (float)num * 0.01f);
		this.OrdealTypo(this._ordealName, this._color, false, num);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Scavenger/Scavenger_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x00035960 File Offset: 0x00033B60
	// Note: this type is marked as 'beforefieldinit'.
	static ScavengerOrdeal()
	{
	}

	// Token: 0x04003782 RID: 14210
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x04003783 RID: 14211
	protected Color _color = Color.blue;

	// Token: 0x04003784 RID: 14212
	private static int[] ids = new int[]
	{
		200017,
		200018
	};

	// Token: 0x04003785 RID: 14213
	protected string _ordealName = "scavenger_noon";
}
