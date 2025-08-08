/*
public FixerOrdeal(OrdealLevel level) // 
public FixerCreature MakeOrdealCreature(RwbpType type, MapNode node, bool isOvertime = false) // 
public FixerCreature MakeOrdealCreature_Midnight(MapNode node, bool isOvertime = false) // 
public override void OnOrdealStart() // 
private void MakeClaw() // 
private void MakeFixers() // 
private List<MapNode> GetTargetNodes() // Daat null Hallway Fix
Various private static arrays // 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class FixerOrdeal : OrdealBase
{
	// Token: 0x06003C43 RID: 15427 RVA: 0x001791FC File Offset: 0x001773FC
	public FixerOrdeal(OrdealLevel level)
	{ // <Mod>
		this.level = level;
		this.SetColor();
		switch (level)
		{
		case OrdealLevel.DAWN:
		case OrdealLevel.OVERTIME_DAWN:
			this._ordealName = "fixer_dawn";
			break;
		case OrdealLevel.NOON:
		case OrdealLevel.OVERTIME_NOON:
			this._ordealName = "fixer_noon";
			break;
		case OrdealLevel.DUSK:
		case OrdealLevel.OVERTIME_DUSK:
			this._ordealName = "fixer_dusk";
			break;
		case OrdealLevel.MIDNIGHT:
		case OrdealLevel.OVERTIME_MIDNIGHT:
			this._ordealName = "fixer_midnight";
			break;
		}
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x00035168 File Offset: 0x00033368
	protected void SetColor()
	{
		this._color = Color.white;
		this.OrdealColor = this._color;
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x00179290 File Offset: 0x00177490
	public FixerCreature MakeOrdealCreature(RwbpType type, MapNode node, bool isOvertime = false)
	{ // <Mod>
		int num = type - RwbpType.R;
        if (isOvertime)
        {
            num += 5;
        }
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)FixerOrdeal.ids[num], node, this);
		(ordealCreatureModel.script as FixerCreature).SetOrdeal(this, FixerOrdeal.risks[num], FixerOrdeal.names[num]);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		return ordealCreatureModel.script as FixerCreature;
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x001792EC File Offset: 0x001774EC
	public FixerCreature MakeOrdealCreature_Midnight(MapNode node, bool isOvertime = false)
	{ // <Mod>
		int num = 4;
        if (isOvertime)
        {
            num += 5;
        }
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)FixerOrdeal.ids[num], node, this);
		(ordealCreatureModel.script as FixerCreature).SetOrdeal(this, FixerOrdeal.risks[num], FixerOrdeal.names[num]);
		this._curOrdealCreatureList.Add(ordealCreatureModel);
		return ordealCreatureModel.script as FixerCreature;
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x00035181 File Offset: 0x00033381
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

	// Token: 0x06003C48 RID: 15432 RVA: 0x000351A6 File Offset: 0x000333A6
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x000351B6 File Offset: 0x000333B6
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x00179350 File Offset: 0x00177550
	public override void OnOrdealStart()
	{ // <Mod>
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.SetColor();
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Fixer/WhiteOrdeal_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
		if (this.level == OrdealLevel.MIDNIGHT || this.level == OrdealLevel.OVERTIME_MIDNIGHT)
		{
			this.MakeClaw();
		}
		else
		{
			this.MakeFixers();
		}
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x001793E0 File Offset: 0x001775E0
	private void MakeClaw()
	{ // <Mod>
		List<MapNode> list = new List<MapNode>(this.GetTargetNodes());
		if (list.Count <= 0)
		{
			return;
		}
		MapNode mapNode = list[UnityEngine.Random.Range(0, list.Count)];
		list.Remove(mapNode);
		this.MakeOrdealCreature_Midnight(mapNode, level == OrdealLevel.OVERTIME_MIDNIGHT);
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x0017942C File Offset: 0x0017762C
	private void MakeFixers()
	{ // <Mod>
		int num = 0;
		OrdealLevel level = this.level;
        switch (level)
        {
            case OrdealLevel.DAWN:
            case OrdealLevel.OVERTIME_DAWN:
			    num = 1;
                OrdealManager.instance.InitAvailableFixers();
                break;
            case OrdealLevel.NOON:
            case OrdealLevel.OVERTIME_NOON:
			    num = 2;
                break;
            case OrdealLevel.DUSK:
            case OrdealLevel.OVERTIME_DUSK:
			    num = 4;
                OrdealManager.instance.InitAvailableFixers();
			    OrdealManager.instance.availableFixers.Add(RwbpType.P);
                break;
        }
		if (num <= 0)
		{
			return;
		}
		List<MapNode> list = new List<MapNode>(this.GetTargetNodes());
		for (int i = 0; i < num; i++)
		{
			if (OrdealManager.instance.availableFixers.Count <= 0)
			{
				OrdealManager.instance.InitAvailableFixers();
			}
			if (list.Count <= 0)
			{
				return;
			}
			RwbpType rwbpType = OrdealManager.instance.availableFixers[UnityEngine.Random.Range(0, OrdealManager.instance.availableFixers.Count)];
			OrdealManager.instance.availableFixers.Remove(rwbpType);
			MapNode mapNode = list[UnityEngine.Random.Range(0, list.Count)];
			list.Remove(mapNode);
			this.MakeOrdealCreature(rwbpType, mapNode, level >= OrdealLevel.OVERTIME_DAWN);
		}
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x00179540 File Offset: 0x00177740
	private List<MapNode> GetTargetNodes()
	{ // <Mod<
		List<MapNode> list = new List<MapNode>();
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira[] array = openedAreaList;
		int i = 0;
		while (i < array.Length)
		{
			Sefira sefira = array[i];
			List<PassageObjectModel> list2 = new List<PassageObjectModel>();
			List<PassageObjectModel> list3 = new List<PassageObjectModel>(sefira.passageList);
			foreach (PassageObjectModel passageObjectModel in list3)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								list2.Add(passageObjectModel);
							}
						}
					}
				}
			}
			MapNode mapNode;
			while (list2.Count > 0)
			{
				PassageObjectModel passageObjectModel2 = list2[UnityEngine.Random.Range(0, list2.Count)];
				list2.Remove(passageObjectModel2);
				MapNode[] nodeList = passageObjectModel2.GetNodeList();
				int num = nodeList.Count<MapNode>();
				if (num > 0)
				{
					mapNode = nodeList[UnityEngine.Random.Range(0, num)];
					if (mapNode != null)
					{
						goto IL_126;
					}
				}
			}
			IL_140:
			i++;
			continue;
			IL_126:
			list.Add(mapNode);
			goto IL_140;
		}
		return list;
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x001796B0 File Offset: 0x001778B0
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
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Fixer/WhiteOrdeal_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x00179754 File Offset: 0x00177954
	public override RiskLevel GetRiskLevel(OrdealCreatureModel creature)
	{
		if (creature == null)
		{
			return base.GetRiskLevel(creature);
		}
		FixerCreature fixerCreature = creature.script as FixerCreature;
		if (fixerCreature != null)
		{
			return fixerCreature.Risk;
		}
		return base.GetRiskLevel(creature);
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x00179790 File Offset: 0x00177990
	public override string OrdealNameText(OrdealCreatureModel ordeal)
	{
		if (ordeal == null)
		{
			return base.OrdealNameText(ordeal);
		}
		FixerCreature fixerCreature = ordeal.script as FixerCreature;
		if (fixerCreature != null)
		{
			string id = string.Format("ordeal_{0}_name", fixerCreature.GetOrdealName());
			return LocalizeTextDataModel.instance.GetText(id);
		}
		return base.OrdealNameText(ordeal);
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x001797E4 File Offset: 0x001779E4
	// Note: this type is marked as 'beforefieldinit'.
	static FixerOrdeal()
	{
	}

	// Token: 0x04003711 RID: 14097
	private const int _spawn_dawn = 1;

	// Token: 0x04003712 RID: 14098
	private const int _spawn_noon = 2;

	// Token: 0x04003713 RID: 14099
	private const int _spawn_dusk = 4;

	// Token: 0x04003714 RID: 14100
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x04003715 RID: 14101
	protected Color _color;

	// Token: 0x04003716 RID: 14102
	private static int[] ids = new int[]
	{ // <Mod>
		200021,
		200022,
		200023,
		200024,
		200025,
		200121,
		200122,
		200123,
		200124,
		200125
	};

	// Token: 0x04003717 RID: 14103
	private static RiskLevel[] risks = new RiskLevel[]
	{ // <Mod>
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.ALEPH,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.ALEPH
	};

	// Token: 0x04003718 RID: 14104
	private static string[] names = new string[]
	{ // <Mod>
		"fixer_red",
		"fixer_white",
		"fixer_black",
		"fixer_pale",
		"fixer_claw",
		"fixer_red",
		"fixer_white",
		"fixer_black",
		"fixer_pale",
		"fixer_claw"
	};

	// Token: 0x04003719 RID: 14105
	protected string _ordealName = "fixer_dawn";
}
