/*
private static int[] ids // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public class OutterGodOrdeal : OrdealBase
{
	// Token: 0x06003CDF RID: 15583 RVA: 0x000357EB File Offset: 0x000339EB
	public OutterGodOrdeal()
	{
		this.SetColor();
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x0003580F File Offset: 0x00033A0F
	protected void SetColor()
	{
		ColorUtility.TryParseHtmlString("#9F68EAFF", out this._color);
		this.OrdealColor = this._color;
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x0017B8A4 File Offset: 0x00179AA4
	public virtual OutterGodOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node)
	{
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)OutterGodOrdeal.ids[(int)level], node, this);
		(ordealCreatureModel.script as OutterGodOrdealCreature).SetOrdeal(this, level);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		return ordealCreatureModel.script as OutterGodOrdealCreature;
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x0003582E File Offset: 0x00033A2E
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

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00035853 File Offset: 0x00033A53
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x00035863 File Offset: 0x00033A63
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x0017B8F0 File Offset: 0x00179AF0
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.SetColor();
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/OutterGod/OutterGod_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x0017B964 File Offset: 0x00179B64
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
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/OutterGod/OutterGod_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x00035872 File Offset: 0x00033A72
	// Note: this type is marked as 'beforefieldinit'.
	static OutterGodOrdeal()
	{
	}

	// Token: 0x0400377E RID: 14206
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x0400377F RID: 14207
	protected Color _color;

	// Token: 0x04003780 RID: 14208
	private static int[] ids = new int[]
	{ // <Mod>
		200009,
		200010,
		200011,
		200012,
		200109,
		200110,
		200111,
		200112
	};

	// Token: 0x04003781 RID: 14209
	protected string _ordealName = "outtergod_noon";
}
