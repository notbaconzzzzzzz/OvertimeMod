using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMalkutBossBase : SefiraBossBase
{
	public OvertimeMalkutBossBase()
	{
		sefiraEnum = SefiraEnum.MALKUT;
	}

	private OvertimeMalkutCoreScript Script
	{
		get
		{
			return model.script as OvertimeMalkutCoreScript;
		}
	}

	public override float GetDescFreq()
	{
		if (QliphothOverloadLevel >= 4)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeMalkutCoreScript", "MalkutCoreAnim", 400001L);
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		_cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		psycho = Camera.main.gameObject.AddComponent<CameraFilterPack_Vision_Psycho>();
		auraDistortion = Camera.main.gameObject.AddComponent<CameraFilterPack_Vision_AuraDistortion>();
		psycho.HoleSize = 0.75f;
		auraDistortion.Twist = 0.12f;
		Script.bossBase = this;
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Custom/Sounds/BGM/Boss/Malkuth/1_Malkuth_Battle1",
			"Custom/Sounds/BGM/Boss/Malkuth/2_Malkuth_Battle2",
			"Custom/Sounds/BGM/Boss/Malkuth/3_Malkuth_Battle3"
		});
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		RandomizeMapGraph(0.25f, 0.04f, 0f);
	}

	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeMalkutCoreScript", "MalkutCoreAnim", 400001L);
		GameObject gameObject = model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		Script.bossBase = this;
		//SefiraBossManager.Instance.RandomizeWorkId();
	}

	public void StartEffect()
	{
		_startEffectTimer.StartTimer(Script.AnimScript.startEffectTime);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_startEffectTimer.started)
		{
			if (psycho != null)
			{
				psycho.HoleSize = Script.AnimScript.startPsychoCurve.Evaluate(_startEffectTimer.Rate);
			}
			if (_startEffectTimer.RunTimer() && psycho != null)
			{
				psycho.HoleSize = 0.75f;
			}
		}
		if (SefiraBossManager.Instance.IsKetherBoss())
		{
			return;
		}
		int qliphothOverloadLevel = CreatureOverloadManager.instance.GetQliphothOverloadLevel();
		if (CreatureOverloadManager.instance.GetQliphothOverloadLevel() == 4)
		{
			Script.AnimScript.OnChange();
		}
	}

	public override void OnOverloadActivated(int currentLevel)
	{
        if (IsCleared())
        {
            return;
        }
		if (QliphothOverloadLevel == 2)
		{
			OnChangePhase();
            RandomizeMapGraph(0.5f, 0.5f, 0.1f);
		}
		else if (QliphothOverloadLevel == 5)
		{
			OnChangePhase();
            RandomizeMapGraph(1f, 1f, 1f);
		}
        else
        {
            switch (_phase)
            {
                case 0:
                    RandomizeMapGraph(0.1f, 0f, 0f);
                    break;
                case 1:
                    RandomizeMapGraph(0.15f, 0.15f, 0f);
                    break;
                case 2:
                    RandomizeMapGraph(0.3f, 0.3f, 0.3f);
                    break;
            }
        }
	}

	public override void OnChangePhase()
	{
		_phase++;
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		//SefiraBossManager.Instance.RandomizeWorkId();
		StartEffect();
		ShockwaveEffect.Invoker(model.GetCurrentViewPosition(), model, 3f, 600f, EffectLifetimeType.NORMAL);
		MakeSoundAttachCamera("SefiraBoss/Boss_Malkut");
	}

	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 8;
	}

	public override void OnCleared()
	{
		base.OnCleared();
		Script.AnimScript.OnClear();
		Vector3 currentViewPosition = Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

    public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
    {
        if (_ordealSpawnTime[2] >= 7)
		{
			_ordealSpawnTime[2] = 6;
		}
    }

	public override bool IsStartEmergencyBgm()
	{
		return false;
	}

    public void PrepareMapGraph()
    {
        elevatorGroups = new List<ElevatorGroup>();
        elevatorEdges = new List<MapEdge>();
        roadEdges = new List<MapEdge>();
        isolateEdges = new List<MapEdge>();
		MapEdge[] allEdges = MapGraph.instance.GetGraphEdges();
		foreach (MapEdge edge in allEdges)
		{
			if (edge.type == "door" && edge.node1.activate && edge.node2.activate)
			{
				int type = -1;
                ElevatorGroup elevator = null;
				if (edge.node2.GetAttachedPassage().type == PassageType.ISOLATEROOM)
				{
					type = 2;
				}
				else if (edge.node1.GetElevator() != null)
				{
					type = 0;
                    elevator = elevatorGroups.Find((ElevatorGroup x) => x.node == edge.node1);
                    if (elevator == null)
                    {
                        elevator = new ElevatorGroup(edge.node1);
                        elevatorGroups.Add(elevator);
                    }
				}
				else if (edge.node2.GetElevator() != null)
				{
					type = 0;
                    elevator = elevatorGroups.Find((ElevatorGroup x) => x.node == edge.node2);
                    if (elevator == null)
                    {
                        elevator = new ElevatorGroup(edge.node2);
                        elevatorGroups.Add(elevator);
                    }
				}
				else
				{
					type = 1;
				}
				if (type != -1)
				{
					edge.originalNode1 = edge.node1;
					edge.originalNode2 = edge.node2;
					edge.originalCost = edge.cost;
                    if (type == 0)
                    {
                        elevatorEdges.Add(edge);
                        if (elevator != null)
                        {
                            elevator.AddEdge(edge);
                        }
                    }
                    else if (type == 1)
                    {
                        roadEdges.Add(edge);
                    }
                    else if (type == 2)
                    {
                        isolateEdges.Add(edge);
                    }
				}
			}
		}
		foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
		{
			MapNode node = creature.entryNode;
			if (node == null) continue;
			MapEdge[] edges = node.GetEdges();
			foreach (MapEdge edge in edges)
			{
				if (edge.node1.GetId() == node.GetId() + "@outter")
				{
					node = edge.node1;
					break;
				}
			}
			edges = node.GetEdges();
			foreach (MapEdge edge in edges)
			{
				if (edge.type == "door")
				{
					isolateEdges.Add(edge);
					edge.originalNode1 = edge.node1;
					edge.originalNode2 = edge.node2;
					edge.originalCost = edge.cost;
					break;
				}
			}
		}/*
        Notice.instance.Send(NoticeName.AddSystemLog, new object[]
        {
            "Total Elevator Edges : " + elevatorEdges.Count.ToString()
        });
        Notice.instance.Send(NoticeName.AddSystemLog, new object[]
        {
            "Total Elevator Groups : " + elevatorGroups.Count.ToString()
        });
        Notice.instance.Send(NoticeName.AddSystemLog, new object[]
        {
            "Total Road Edges : " + roadEdges.Count.ToString()
        });
        Notice.instance.Send(NoticeName.AddSystemLog, new object[]
        {
            "Total Isolate Edges : " + isolateEdges.Count.ToString()
        });*/
    }

    public void RandomizeMapGraph(float elevatorRate, float roadRate, float isolateRate)
    {
		if (elevatorGroups == null)
		{
			PrepareMapGraph();
		}
        if (elevatorRate > 0f)
        {
            int amt = Mathf.CeilToInt((float)elevatorGroups.Count * elevatorRate) + 1;
            List<MapEdge> randomEdges;
            List<MapNode> nodePool = new List<MapNode>();
            if (amt >= elevatorGroups.Count)
            {
                amt = elevatorGroups.Count;
                randomEdges = new List<MapEdge>();
                for (int i = 0; i < amt; i++)
                {
                    ElevatorGroup elevator = elevatorGroups[i];
                    int num = UnityEngine.Random.Range(0, elevator.edges.Count);
                    MapEdge edge = elevator.edges[num];
                    randomEdges.Add(edge);
                    if (elevator.dir[num])
                    {
                        nodePool.Add(edge.node1);
                        edge.node1.RemoveEdge(edge);
                        edge.node1 = null;
                    }
                    else
                    {
                        nodePool.Add(edge.node2);
                        edge.node2.RemoveEdge(edge);
                        edge.node2 = null;
                    }
                }
            }
            else
            {
                randomEdges = new List<MapEdge>();
                List<ElevatorGroup> list = new List<ElevatorGroup>(elevatorGroups);
                for (int i = 0; i < amt; i++)
                {
                    int ind = UnityEngine.Random.Range(0, list.Count);
                    ElevatorGroup elevator = list[ind];
                    int num = UnityEngine.Random.Range(0, elevator.edges.Count);
                    MapEdge edge = elevator.edges[num];
                    randomEdges.Add(edge);
                    list.RemoveAt(ind);
                    if (elevator.dir[num])
                    {
                        nodePool.Add(edge.node1);
                        edge.node1.RemoveEdge(edge);
                        edge.node1 = null;
                    }
                    else
                    {
                        nodePool.Add(edge.node2);
                        edge.node2.RemoveEdge(edge);
                        edge.node2 = null;
                    }
                }
            }
            for (int i = 0; i < amt; i++)
            {
                MapEdge edge = randomEdges[i];
                int ind = UnityEngine.Random.Range(0, nodePool.Count);
                if (nodePool.Count == 2)
                {
                    ind = 1;
                }
                MapNode node = nodePool[ind];
                nodePool.RemoveAt(ind);
                if (edge.node1 == null)
                {
                    edge.node1 = node;
                }
                else
                {
                    edge.node2 = node;
                }
                node.AddEdge(edge);
            }/*
            Notice.instance.Send(NoticeName.AddSystemLog, new object[]
            {
                "Randomized Elevator Count : " + amt.ToString()
            });*/
        }/*
		catch (Exception ex)
		{
			string error = ex.Message + " in " + ex.Source;
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				error
			});
		}*/
        if (roadRate > 0f)
        {
            int amt = Mathf.CeilToInt((float)roadEdges.Count * roadRate) + 1;
            List<MapEdge> randomEdges;
            List<MapNode> nodePool = new List<MapNode>();
            if (amt >= roadEdges.Count)
            {
                amt = roadEdges.Count;
                randomEdges = new List<MapEdge>(roadEdges);
                for (int i = 0; i < amt; i++)
                {
                    MapEdge edge = randomEdges[i];
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        nodePool.Add(edge.node1);
                        edge.node1.RemoveEdge(edge);
                        edge.node1 = null;
                    }
                    else
                    {
                        nodePool.Add(edge.node2);
                        edge.node2.RemoveEdge(edge);
                        edge.node2 = null;
                    }
                }
            }
            else
            {
                randomEdges = new List<MapEdge>();
                List<MapEdge> list = new List<MapEdge>(roadEdges);
                for (int i = 0; i < amt; i++)
                {
                    int ind = UnityEngine.Random.Range(0, list.Count);
                    MapEdge edge = list[ind];
                    randomEdges.Add(edge);
                    list.RemoveAt(ind);
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        nodePool.Add(edge.node1);
                        edge.node1.RemoveEdge(edge);
                        edge.node1 = null;
                    }
                    else
                    {
                        nodePool.Add(edge.node2);
                        edge.node2.RemoveEdge(edge);
                        edge.node2 = null;
                    }
                }
            }
            for (int i = 0; i < amt; i++)
            {
                MapEdge edge = randomEdges[i];
                int ind = UnityEngine.Random.Range(0, nodePool.Count);
                if (nodePool.Count == 2)
                {
                    ind = 1;
                }
                MapNode node = nodePool[ind];
                nodePool.RemoveAt(ind);
                if (edge.node1 == null)
                {
                    edge.node1 = node;
                }
                else
                {
                    edge.node2 = node;
                }
                node.AddEdge(edge);
            }/*
            Notice.instance.Send(NoticeName.AddSystemLog, new object[]
            {
                "Randomized Road Count : " + amt.ToString()
            });*/
        }/*
		catch (Exception ex)
		{
			string error = ex.Message + " in " + ex.Source;
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				error
			});
		}*/
		try
		{
        if (isolateRate > 0f)
        {
            int amt = Mathf.CeilToInt((float)isolateEdges.Count * isolateRate) + 1;
            List<MapEdge> randomEdges;
            List<MapNode> nodePool = new List<MapNode>();
            if (amt >= isolateEdges.Count)
            {
                amt = isolateEdges.Count;
                randomEdges = new List<MapEdge>(isolateEdges);
                for (int i = 0; i < amt; i++)
                {
                    MapEdge edge = randomEdges[i];
                    nodePool.Add(edge.node1);
                    edge.node1.RemoveEdge(edge);
                    edge.node1 = null;
                }
            }
            else
            {
                randomEdges = new List<MapEdge>();
                List<MapEdge> list = new List<MapEdge>(isolateEdges);
                for (int i = 0; i < amt; i++)
                {
                    int ind = UnityEngine.Random.Range(0, list.Count);
                    MapEdge edge = list[ind];
                    randomEdges.Add(edge);
                    list.RemoveAt(ind);
                    nodePool.Add(edge.node1);
                    edge.node1.RemoveEdge(edge);
                    edge.node1 = null;
                }
            }
            for (int i = 0; i < amt; i++)
            {
                MapEdge edge = randomEdges[i];
                int ind = UnityEngine.Random.Range(0, nodePool.Count);
                if (nodePool.Count == 2)
                {
                    ind = 1;
                }
                MapNode node = nodePool[ind];
                nodePool.RemoveAt(ind);
                edge.node1 = node;
                node.AddEdge(edge);
            }/*
            Notice.instance.Send(NoticeName.AddSystemLog, new object[]
            {
                "Randomized Isolate Count : " + amt.ToString()
            });*/
        }
		}
		catch (Exception ex)
		{
			string error = ex.Message + " in " + ex.Source;
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				error
			});
		}
    }

    public void ResetMapGraph()
    {
        if (elevatorGroups == null)
        {
            return;
        }
        foreach (MapEdge edge in elevatorEdges)
        {
			edge.node1.RemoveEdge(edge);
			edge.node2.RemoveEdge(edge);
            edge.node1 = edge.originalNode1;
            edge.node2 = edge.originalNode2;
            edge.cost = edge.originalCost;
			edge.node1.AddEdge(edge);
			edge.node2.AddEdge(edge);
            edge.originalNode1 = null;
            edge.originalNode2 = null;
            edge.originalCost = -1f;
        }
        foreach (MapEdge edge in roadEdges)
        {
			edge.node1.RemoveEdge(edge);
			edge.node2.RemoveEdge(edge);
            edge.node1 = edge.originalNode1;
            edge.node2 = edge.originalNode2;
            edge.cost = edge.originalCost;
			edge.node1.AddEdge(edge);
			edge.node2.AddEdge(edge);
            edge.originalNode1 = null;
            edge.originalNode2 = null;
            edge.originalCost = -1f;
        }
        foreach (MapEdge edge in isolateEdges)
        {
			edge.node1.RemoveEdge(edge);
			edge.node2.RemoveEdge(edge);
            edge.node1 = edge.originalNode1;
            edge.node2 = edge.originalNode2;
            edge.cost = edge.originalCost;
			edge.node1.AddEdge(edge);
			edge.node2.AddEdge(edge);
            edge.originalNode1 = null;
            edge.originalNode2 = null;
            edge.originalCost = -1f;
        }
        elevatorEdges = null;
        roadEdges = null;
        isolateEdges = null;
        elevatorGroups = null;
    }

	private SefiraBossCreatureModel model;

	private Timer _startEffectTimer = new Timer();

	private float totalEnergy;

	private CameraFilterPack_Vision_Psycho psycho;

	private CameraFilterPack_Vision_AuraDistortion auraDistortion;

	private CameraFilterPack_Film_Grain grain;

	private int _phase = 0;

    private List<MapEdge> elevatorEdges;

    private List<MapEdge> roadEdges;

    private List<MapEdge> isolateEdges;

    private List<ElevatorGroup> elevatorGroups;

    public class ElevatorGroup
    {
        public ElevatorGroup(MapNode _node)
        {
            node = _node;
        }

        public void AddEdge(MapEdge edge)
        {
            edges.Add(edge);
            if (edge.node1 == node)
            {
                dir.Add(false);
            }
            else
            {
                dir.Add(true);
            }
        }

        public MapNode node;

        public List<MapEdge> edges = new List<MapEdge>();

        public List<bool> dir = new List<bool>();
    }
}
