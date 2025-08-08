/*
private void SetColor() // 
public override void OrdealEnd() // Overtime Core Suppressions
+public MapNode GetForcefulTeleportDestination(UnitModel unit) // 
+private List<MapNode> _validWorkerDestinations // 
+private List<MapNode> _validCreatureDestinations // 
Various private static arrays // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000797 RID: 1943
public class CircusOrdeal : OrdealBase
{
	// Token: 0x06003C1E RID: 15390 RVA: 0x00034E1B File Offset: 0x0003301B
	public CircusOrdeal()
	{
		this.SetColor();
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x00034E3F File Offset: 0x0003303F
	private void SetColor()
	{ // <Mod>
		ColorUtility.TryParseHtmlString("#DC143CFF", out this._color);
		this.OrdealColor = this._color;
		_ordeal_name = _ordealName;
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x00177D18 File Offset: 0x00175F18
	public override RiskLevel GetRiskLevel(OrdealCreatureModel creature)
	{
		if (creature == null)
		{
			return base.GetRiskLevel(creature);
		}
		CircusOrdealCreature circusOrdealCreature = creature.script as CircusOrdealCreature;
		if (circusOrdealCreature != null)
		{
			return circusOrdealCreature.Risk;
		}
		return base.GetRiskLevel(creature);
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x00177D54 File Offset: 0x00175F54
	public override string OrdealNameText(OrdealCreatureModel ordeal)
	{
		if (ordeal == null)
		{
			return base.OrdealNameText(ordeal);
		}
		CircusOrdealCreature circusOrdealCreature = ordeal.script as CircusOrdealCreature;
		if (circusOrdealCreature != null)
		{
			string id = string.Format("ordeal_{0}_name", circusOrdealCreature.GetOrdealName());
			return LocalizeTextDataModel.instance.GetText(id);
		}
		return base.OrdealNameText(ordeal);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x00177DA8 File Offset: 0x00175FA8
	public CircusOrdealCreature MakeOrdealCreature(OrdealLevel level, MapNode node)
	{
		OrdealCreatureModel ordealCreatureModel = OrdealManager.instance.AddCreature((long)CircusOrdeal.ids[(int)level], node, this);
		(ordealCreatureModel.script as CircusOrdealCreature).SetOrdeal(this, level, CircusOrdeal.risks[(int)level], CircusOrdeal.names[(int)level]);
		this.AddChildCircus(ordealCreatureModel);
		return ordealCreatureModel.script as CircusOrdealCreature;
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x00034E5E File Offset: 0x0003305E
	public void AddChildCircus(OrdealCreatureModel child)
	{
		this._curOrdealCreatureList.Add(child);
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x00034E6C File Offset: 0x0003306C
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

	// Token: 0x06003C25 RID: 15397 RVA: 0x00034E91 File Offset: 0x00033091
	protected virtual bool CheckCloseCondition()
	{
		return this._curOrdealCreatureList.Count == 0;
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x00034EA1 File Offset: 0x000330A1
	public virtual void OnDie(OrdealCreatureModel model)
	{
		this._curOrdealCreatureList.Remove(model);
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x00177E00 File Offset: 0x00176000
	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		PlayerModel.emergencyController.AddScore(this.GetRiskLevel(null));
		this.SetColor();
		this.OrdealTypo(this._ordealName, this._color, true, 0);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Circus/Circus_Start", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x00177E74 File Offset: 0x00176074
	public override void OrdealEnd()
	{ // <Mod>
		base.OrdealEnd();
		this.SetColor();
		int num = this.ordealRewards[(int)this.level];
		if (!base.canTakeRewards)
		{
			num = 0;
		}
		EnergyModel.instance.AddEnergy(StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay()) * (float)num * 0.01f / StageTypeInfo.instnace.GetPercentEnergyFactor());
		num = (int)((float)num / StageTypeInfo.instnace.GetPercentEnergyFactor());
		this.OrdealTypo(this._ordealName, this._color, false, num);
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Ordeal/Circus/Circus_End", Vector2.zero);
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = Vector3.zero;
	}

	// <Mod>
    public MapNode GetForcefulTeleportDestination(UnitModel unit)
    {
		if (_validWorkerDestinations == null || _validCreatureDestinations == null)
		{
			_validWorkerDestinations = new List<MapNode>();
			_validCreatureDestinations = new List<MapNode>();
			foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
			{
				foreach (PassageObjectModel passage in sefira.passageList)
				{
					if (passage.isActivate)
					{
						if (passage.type == PassageType.SEFIRA || passage.type == PassageType.DEPARTMENT)
						{
							_validWorkerDestinations.AddRange(passage.GetNodeList());
						}
						else if (passage.type == PassageType.HORIZONTAL)
						{
							_validCreatureDestinations.AddRange(passage.GetNodeList());
						}
					}
				}
			}
		}
        List<MapNode> mapNodes = null;
        if (unit is WorkerModel)
        {
			mapNodes = _validWorkerDestinations;
        }
        else
        {
			mapNodes = _validCreatureDestinations;
        }
		if (mapNodes == null || mapNodes.Count <= 0)
		{
			return null;
		}
		return mapNodes[UnityEngine.Random.Range(0, mapNodes.Count)];
    }

	// <Mod>
	private List<MapNode> _validWorkerDestinations = null;

	// <Mod>
	private List<MapNode> _validCreatureDestinations = null;

	// Token: 0x06003C29 RID: 15401 RVA: 0x00177F18 File Offset: 0x00176118
	// Note: this type is marked as 'beforefieldinit'.
	static CircusOrdeal()
	{
	}

	// Token: 0x04003700 RID: 14080
	protected List<OrdealCreatureModel> _curOrdealCreatureList = new List<OrdealCreatureModel>();

	// Token: 0x04003701 RID: 14081
	protected Color _color;

	// Token: 0x04003702 RID: 14082
	private static int[] ids = new int[]
	{ // <Mod>
		200005,
		200006,
		200007,
		200007,
		200105,
		200106,
		200107,
		200107
	};

	// Token: 0x04003703 RID: 14083
	private static RiskLevel[] risks = new RiskLevel[]
	{ // <Mod>
		RiskLevel.TETH,
		RiskLevel.HE,
		RiskLevel.WAW,
		RiskLevel.WAW,
		RiskLevel.TETH,
		RiskLevel.HE,
		RiskLevel.WAW,
		RiskLevel.WAW
	};

	// Token: 0x04003704 RID: 14084
	private static string[] names = new string[]
	{ // <Mod>
		"circus_dawn",
		"circus_noon",
		"circus_dusk",
		"circus_dusk",
		"circus_dawn",
		"circus_noon",
		"circus_dusk",
		"circus_dusk"
	};

	// Token: 0x04003705 RID: 14085
	protected string _ordealName = "circus_dawn";
}
