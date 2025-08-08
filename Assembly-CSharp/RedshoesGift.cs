using System;

// Token: 0x0200063F RID: 1599
public class RedshoesGift : EquipmentScriptBase
{
	// Token: 0x0600359D RID: 13725 RVA: 0x000301FE File Offset: 0x0002E3FE
	public RedshoesGift()
	{
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x0015F450 File Offset: 0x0015D650
	public override EGObonusInfo GetBonus(UnitModel actor)
	{ // <Mod> changed workProb from 10 to -10
		EGObonusInfo egobonusInfo = new EGObonusInfo();
		if (actor.HasEquipment(200003))
		{
			egobonusInfo.cubeSpeed = -10;
			egobonusInfo.attackSpeed = 10;
			egobonusInfo.workProb = -10;
		}
		return base.GetBonus(actor) + egobonusInfo;
	}
}
