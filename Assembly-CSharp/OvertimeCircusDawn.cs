using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeCircusDawn : CircusDawn
{
	public OvertimeCircusDawn()
	{
	}

	public override void OnFixedUpdate(CreatureModel creature)
	{
        base.OnFixedUpdate(creature);
		if (model.hp <= 0f)
		{
			return;
		}
		if (_isSuppressed)
		{
			return;
		}
		if (_retaliateTimer.started)
		{
			if (_retaliateTimer.RunTimer())
			{
				Retaliate();
                _retaliateTimer.StartTimer(2f);
			}
		}
    }

	public override void OnViewInit(CreatureUnit unit)
	{
		animScript = (unit.animTarget as CircusDawnAnim);
		animScript.SetScript(this);
		if (_ordealScript is OvertimeCircusDawnOrdeal && (_ordealScript as OvertimeCircusDawnOrdeal).initialSpawn)
		{
			if (currentPassage != null)
			{
				List<MapNode> list = new List<MapNode>(currentPassage.GetNodeList());
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].connectedCreature == null)
					{
						list.RemoveAt(i);
					}
				}
				if (list.Count > 0)
				{
					_currentWaitingCreature = list[UnityEngine.Random.Range(0, list.Count)].connectedCreature;
					EndTelport();
				}
				else
				{
					ReadyForTeleport();
				}
			}
		}
		else
		{
			ReadyForTeleport();
		}
		unit.escapeRisk.text = _ordealScript.GetRiskLevel(model as OrdealCreatureModel).ToString().ToUpper();
		unit.escapeRisk.color = UIColorManager.instance.GetRiskColor(_ordealScript.GetRiskLevel(model as OrdealCreatureModel));
	}

    public override void EndTelport()
    {
        base.EndTelport();
        _retaliateTargets.Clear();
        _retaliateTimer.StopTimer();
		if (_currentWaitingCreature == null) return;
        List<UnitModel> list = new List<UnitModel>();
        foreach (MovableObjectNode objectNode in currentPassage.GetEnteredTargets())
        {
            UnitModel unit = objectNode.GetUnit();
            if (unit.hp <= 0f) continue;
            if (unit == model) continue;
            if (unit is OrdealCreatureModel && (unit as OrdealCreatureModel).script is CircusOrdealCreature)
			{
				return;
			}
            list.Add(unit);
        }
        foreach (UnitModel unit in list)
        {
            MapNode destination = GetForcefulTeleportDestination(unit);
            if (destination == null) continue;
            EffectInvoker.Invoker("Ordeal/Clown_2", unit.GetMovableNode(), 0.5f, false);
            unit.GetMovableNode().SetCurrentNode(destination);
            Sefira sefira = SefiraManager.instance.GetSefira(destination.GetAttachedPassage().GetSefiraName());
			if (unit is AgentModel)
			{
				AgentModel agent = unit as AgentModel;
				agent.SetWaitingPassage(destination.GetAttachedPassage());
				agent.StopAction();
				agent.counterAttackEnabled = true;
			}
        }
    }

    public MapNode GetForcefulTeleportDestination(UnitModel unit)
    {
        return _ordealScript.GetForcefulTeleportDestination(unit);
    }
    
	public override void OnDie()
	{
		if (meltdownType > -1)
		{
            if (_trickCastTimer.started && _currentTrickCreature != null)
            {
				if (!_currentTrickCreature.IsEscaped() && (_currentTrickCreature.metaInfo.creatureKitType != CreatureKitType.EQUIP || _currentTrickCreature.kitEquipOwner == null))
				{
					if (_currentTrickCreature.metadataId != 300101L && _currentTrickCreature.metadataId != 100024L && _currentTrickCreature.metadataId != 300110L)
					{
						if (!_currentTrickCreature.isOverloaded || _currentTrickCreature.overloadType == OverloadType.DEFAULT)
						{
                			Vestige.OvertimeOverloadManager.instance.MakeOverload((OverloadType)(meltdownType + (int)OverloadType.PAIN), _currentTrickCreature);
						}
					}
				}
            }/*
			List<UnitModel> list = new List<UnitModel>();
			foreach (MovableObjectNode objectNode in currentPassage.GetEnteredTargets())
			{
				UnitModel unit = objectNode.GetUnit();
				if (unit.hp <= 0f) continue;
				if (unit == model) continue;
				if (unit is OrdealCreatureModel && (unit as OrdealCreatureModel).script is CircusOrdealCreature) continue;
				list.Add(unit);
			}
			foreach (UnitModel unit in list)
			{
				MapNode destination = GetForcefulTeleportDestination(unit);
				if (destination == null) continue;
				EffectInvoker.Invoker("Ordeal/Clown_2", unit.GetMovableNode(), 0.5f, false);
				unit.GetMovableNode().SetCurrentNode(destination);
				Sefira sefira = SefiraManager.instance.GetSefira(destination.GetAttachedPassage().GetSefiraName());
				if (unit is AgentModel)
				{
					AgentModel agent = unit as AgentModel;
					agent.SetWaitingPassage(destination.GetAttachedPassage());
					agent.StopAction();
					agent.counterAttackEnabled = true;
				}
			}*/
			MapNode mapNode = model.GetCurrentNode();
			if (mapNode == null)
			{
				MapEdge currentEdge = model.GetCurrentEdge();
				if (currentEdge == null)
				{
					Debug.Log("시공의 틈에서 죽음");
					return;
				}
				Debug.Log(model.GetMovableNode().edgePosRate);
				if ((double)model.GetMovableNode().edgePosRate > 0.5)
				{
					mapNode = currentEdge.node1;
				}
				else
				{
					mapNode = currentEdge.node2;
				}
			}
			for (int i = 0; i < 2; i++)
			{
				CreatureModel model = _ordealScript.MakeOrdealCreature(OrdealLevel.OVERTIME_DAWN, mapNode).model;
                model.hp = model.hp / 2f;
			}
		}
		_ordealScript.OnDie(model as OrdealCreatureModel);
	}

    public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
    {
        if (actor == null || !actor.IsAttackTargetable()) return;
        actor.AddUnitBuf(new OvertimeCircusDawnSlow());
        if (_retaliateTargets.Contains(actor)) return;
        _retaliateTargets.Add(actor);
        if (!_retaliateTimer.started)
        {
            _retaliateTimer.StartTimer(2f);
        }
    }

    public void Retaliate()
    {
        foreach (UnitModel unit in _retaliateTargets)
        {
			if (MovableObjectNode.GetDistance(model.GetMovableNode(), unit.GetMovableNode()) > 16f)
			{
				unit.TakeDamage(model, _retaliateDamage[UnityEngine.Random.Range(0, _retaliateDamage.Length)]);
			}
        }
    }

    public int meltdownType = -1;

    private Timer _retaliateTimer = new Timer();

    private List<UnitModel> _retaliateTargets = new List<UnitModel>();

	private static DamageInfo[] _retaliateDamage = new DamageInfo[]
	{
		new DamageInfo(RwbpType.R, 13, 17),
		new DamageInfo(RwbpType.W, 13, 17),
		new DamageInfo(RwbpType.B, 13, 17),
		new DamageInfo(RwbpType.P, 13, 17)
	};
}
