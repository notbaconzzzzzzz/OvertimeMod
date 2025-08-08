/*
private void SetColor() // 
Various private static arrays // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class BugOrdeal : OrdealBase
{
	// Token: 0x06003BED RID: 15341 RVA: 0x00034BF0 File Offset: 0x00032DF0
	public BugOrdeal()
	{
		this.SetColor();
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x00034C14 File Offset: 0x00032E14
	private void SetColor()
	{ // <Mod>
		ColorUtility.TryParseHtmlString("#FE960BFF", out this._color);
		this.OrdealColor = this._color;
		_ordeal_name = _ordealName;
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x001767D0 File Offset: 0x001749D0
	public override RiskLevel GetRiskLevel(OrdealCreatureModel creature)
	{
		if (creature == null)
		{
			return base.GetRiskLevel(creature);
		}
		BugOrdealCreature bugOrdealCreature = creature.script as BugOrdealCreature;
		if (bugOrdealCreature != null)
		{
			return bugOrdealCreature.Risk;
		}
		return base.GetRiskLevel(creature);
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0017680C File Offset: 0x00174A0C
	public override string OrdealNameText(OrdealCreatureModel ordeal)
	{
		if (ordeal == null)
		{
			return base.OrdealNameText(ordeal);
		}
		BugOrdealCreature bugOrdealCreature = ordeal.script as BugOrdealCreature;
		if (bugOrdealCreature != null)
		{
			string id = string.Format("ordeal_{0}_name", bugOrdealCreature.GetOrdealName());
			return LocalizeTextDataModel.instance.GetText(id);
		}
		return base.OrdealNameText(ordeal);
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x00176860 File Offset: 0x00174A60
	public virtual BugOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node, BugMidnight midnight, params UnitDirection[] direction)
	{
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)BugOrdeal.ids[(int)level], node, this);
		(ordealCreatureModel.script as BugOrdealCreature).SetOrdeal(this, level, BugOrdeal.risks[(int)level], BugOrdeal.names[(int)level]);
		(ordealCreatureModel.script as BugOrdealCreature).SetMidnight(midnight);
		this.AddChildBug(ordealCreatureModel);
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
		return ordealCreatureModel.script as BugOrdealCreature;
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x00034C33 File Offset: 0x00032E33
	public void AddChildBug(OrdealCreatureModel child)
	{
		this._curOrdealCreatureList.Add(child);
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x00034C41 File Offset: 0x00032E41
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

	// Token: 0x06003BF4 RID: 15348 RVA: 0x00034C66 File Offset: 0x00032E66
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x00034C76 File Offset: 0x00032E76
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x00176900 File Offset: 0x00174B00
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.SetColor();
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Bug/Bug_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x00176974 File Offset: 0x00174B74
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
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Bug/Bug_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x00176A18 File Offset: 0x00174C18
	// Note: this type is marked as 'beforefieldinit'.
	static BugOrdeal()
	{
	}

	// Token: 0x040036E8 RID: 14056
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x040036E9 RID: 14057
	protected Color _color;

	// Token: 0x040036EA RID: 14058
	private static int[] ids = new int[]
	{ // <Mod>
		200013,
		200014,
		200015,
		200016,
		200113,
		200114,
		200115,
		200116
	};

	// Token: 0x040036EB RID: 14059
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

	// Token: 0x040036EC RID: 14060
	private static string[] names = new string[]
	{ // <Mod>
		"bug_dawn",
		"bug_noon",
		"bug_dusk",
		"bug_midnight",
		"bug_dawn",
		"bug_noon",
		"bug_dusk",
		"bug_midnight"
	};

	// Token: 0x040036ED RID: 14061
	protected string _ordealName = "bug_dawn";
}
