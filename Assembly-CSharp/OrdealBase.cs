using System;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class OrdealBase
{
	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06003C6A RID: 15466 RVA: 0x00179154 File Offset: 0x00177354
	// (set) Token: 0x06003C6B RID: 15467 RVA: 0x000351B0 File Offset: 0x000333B0
	public bool canTakeRewards
	{
		protected get
		{
			bool canTakeRewards = this._canTakeRewards;
			this._canTakeRewards = true;
			return canTakeRewards;
		}
		set
		{
			this._canTakeRewards = value;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06003C6C RID: 15468 RVA: 0x00179170 File Offset: 0x00177370
	public virtual string OrdealTypeText
	{
		get
		{
			string id = string.Format("ordeal_{0}_type", this._ordeal_name);
			return LocalizeTextDataModel.instance.GetText(id);
		}
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x0017919C File Offset: 0x0017739C
	public virtual string OrdealNameText(OrdealCreatureModel ordeal)
	{
		string id = string.Format("ordeal_{0}_name", this._ordeal_name);
		return LocalizeTextDataModel.instance.GetText(id);
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x0000403D File Offset: 0x0000223D
	public virtual void OnGameInit()
	{
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x0000403D File Offset: 0x0000223D
	public virtual void OnDestroy()
	{
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x0000403D File Offset: 0x0000223D
	public virtual void OnOrdealStart()
	{
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x000351B9 File Offset: 0x000333B9
	public virtual void OrdealEnd()
	{
		this.isStarted = false;
		OrdealManager.instance.OnOrdealEnd(this, this._canTakeRewards);
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x0000403D File Offset: 0x0000223D
	public virtual void FixedUpdate()
	{
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x001791C8 File Offset: 0x001773C8
	public virtual void OrdealTypo(string ordealName, Color color, bool isStart = true, int reward = 0)
	{
		string id = string.Format("ordeal_{0}_type", ordealName);
		string id2 = string.Format("ordeal_{0}_name", ordealName);
		string text = string.Format("ordeal_{0}_", ordealName);
		this._ordeal_name = ordealName;
		string topText;
		if (isStart)
		{
			text += "start";
			topText = string.Empty;
		}
		else
		{
			text += "end";
			topText = string.Format(LocalizeTextDataModel.instance.GetText("ordeal_end_reward"), reward);
		}
		RandomEventLayer.currentLayer.AddTypo(RandomEventLayer.currentLayer.MakeDefaultTypoSession(LocalizeTextDataModel.instance.GetText(id), LocalizeTextDataModel.instance.GetText(id2), LocalizeTextDataModel.instance.GetText(text), color, topText));
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x000351D3 File Offset: 0x000333D3
	public virtual bool IsStartable()
	{
		return !this.isStarted;
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x000351E3 File Offset: 0x000333E3
	public virtual RiskLevel GetRiskLevel(OrdealCreatureModel creature)
	{
		return this.riskLevel;
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x000351EB File Offset: 0x000333EB
	public void SetRiskLevel(RiskLevel value)
	{
		this.riskLevel = value;
	}

	// Token: 0x0400372D RID: 14125
	public OrdealLevel level;

	// Token: 0x0400372E RID: 14126
	private RiskLevel riskLevel;

	// Token: 0x0400372F RID: 14127
	public readonly int[] ordealRewards = new int[]
	{
		10,
		15,
		20,
		25,
		15,
		20,
		25,
		30
	};

	// Token: 0x04003730 RID: 14128
	public int startTime = 10;

	// Token: 0x04003731 RID: 14129
	public bool isStarted;

	// Token: 0x04003732 RID: 14130
	public string _ordeal_name = string.Empty; // <Mod> changed from private to public

	// Token: 0x04003733 RID: 14131
	public Color OrdealColor = Color.white;

	// Token: 0x04003734 RID: 14132
	private bool _canTakeRewards = true;
}
