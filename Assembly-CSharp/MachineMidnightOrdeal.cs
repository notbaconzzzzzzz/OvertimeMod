using System;

// Token: 0x020007A0 RID: 1952
public class MachineMidnightOrdeal : MachineOrdeal
{
	// Token: 0x06003C69 RID: 15465 RVA: 0x000352AB File Offset: 0x000334AB
	public MachineMidnightOrdeal()
	{
		this._ordealName = "machine_midnight";
		this.level = OrdealLevel.MIDNIGHT;
		base.SetRiskLevel(RiskLevel.ALEPH);
		this.OrdealColor = this._color;
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00179FC0 File Offset: 0x001781C0
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		Sefira sefira = SefiraManager.instance.GetSefira("Yesod");
		this.MakeOrdealCreature(this.level, sefira.sefiraPassage.centerNode);
	}

	// Token: 0x04003726 RID: 14118
	protected OrdealCreatureModel _ordealCreature;
}
