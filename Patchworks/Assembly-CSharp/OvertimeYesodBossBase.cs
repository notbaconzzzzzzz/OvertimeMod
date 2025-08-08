using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeYesodBossBase : SefiraBossBase
{
	public OvertimeYesodBossBase()
	{
		sefiraEnum = SefiraEnum.YESOD;
	}

	private OvertimeYesodCoreScript Script
	{
		get
		{
			return model.script as OvertimeYesodCoreScript;
		}
	}

	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeYesodCoreScript", "YesodCoreAnim", 400001L);
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		Script.bossBase = this;
		_cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		noiseTv = Camera.main.gameObject.AddComponent<CameraFilterPack_Noise_TV>();
		noiseTv.Fade = 0.075f;
		vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		vignetting.Vignetting = 1f;
		vignetting.VignettingFull = 0f;
		vignetting.VignettingFull = 0f;
		vignetting.VignettingColor = Color.black;
		glitch3 = Camera.main.gameObject.AddComponent<CameraFilterPack_FX_Glitch3>();
		glitch3._Noise = 1f;
		glitch3._Glitch = 0.06f;
		glitch3.enabled = false;
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Custom/Sounds/BGM/Boss/Yesod/1_Yesod_Battle1",
			"Custom/Sounds/BGM/Boss/Yesod/2_Yesod_Battle2",
			"Custom/Sounds/BGM/Boss/Yesod/3_Yesod_Battle3"
		});
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		StartEffect();
        PrepareContainmentUnits();
        RandomizeContainmentUnits();
	}

	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "YesodCoreScript", "YesodCoreAnim", 400001L);
		GameObject gameObject = model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		Script.bossBase = this;
	}

	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 8;
	}

	public void StartEffect()
	{
		_startEffectTimer.StartTimer(3f);
		noiseTv.Fade = 0.075f;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_startEffectTimer.started)
		{
			float fade;
			if (_startEffectTimer.Rate < 0.5f)
			{
				fade = Mathf.Lerp(0.075f, 1f, _startEffectTimer.Rate / 0.5f);
			}
			else
			{
				fade = Mathf.Lerp(1f, 0.075f, _startEffectTimer.Rate / 0.5f - 1f);
			}
			if (noiseTv != null)
			{
				noiseTv.Fade = fade;
			}
			if (_startEffectTimer.RunTimer() && noiseTv != null)
			{
				noiseTv.Fade = 0.075f;
			}
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

	public override void OnCleared()
	{
		base.OnCleared();
		Script.AnimScript.OnClear();
		Vector3 currentViewPosition = Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	public override void OnOverloadActivated(int currentLevel)
	{
		if (QliphothOverloadLevel == 2)
		{
			OnChangePhase();
		}
		else if (QliphothOverloadLevel == 5)
		{
			OnChangePhase();
		}
	}

	public override void OnChangePhase()
	{
		_phase++;
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		if (_phase == 1)
		{
			foreach (SoundEffectPlayer sound in UnityEngine.Object.FindObjectsOfType<SoundEffectPlayer>())
			{
				if (sound.transform.parent != Camera.main.transform)
				{
					sound.transform.localPosition = new Vector3(0f, 0f, 999999f);
				}
			}
			SoundEffectPlayer.silenceNonCamera = true;
		}
		else if (_phase == 2)
		{
			glitch3.enabled = true;
		}
		StartEffect();
		ShockwaveEffect.Invoker(model.GetCurrentViewPosition(), model, 3f, 600f, EffectLifetimeType.NORMAL);
		MakeSoundAttachCamera("SefiraBoss/Boss_Yesod");
        RandomizeContainmentUnits();
	}

    public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
    {
        _ordealSpawnTime[1] = 5;
		_ordealSpawnTime[2] = 7;
    }

	public override bool IsStartEmergencyBgm()
	{
		return false;
	}

    public void PrepareContainmentUnits()
    {
		creaturePositions.Clear();
		creatureRooms.Clear();
        foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
		{
            if (creature.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
			{
				creature.Unit.room.SetOvertimeYesodBoss();
				creature.Unit.room.OnObservationLevelChanged();
			}
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
			MapEdge edge2 = null;
			foreach (MapEdge edge in edges)
			{
				if (edge.type == "door")
				{
					edge2 = edge;
					edge.originalNode1 = edge.node1;
					edge.originalNode2 = edge.node2;
					edge.originalCost = edge.cost;
					break;
				}
			}
			if (edge2 == null) continue;
			CreaturePositionData positionData = new CreaturePositionData(edge2.node1, node, creature.basePosition, creature.sefira, creature.specialSkillPos);
			creaturePositions.Add(positionData);
			creatureRooms.Add(new CreatureRoomData(creature.Unit.room, edge2, creature.roomNode.GetAttachedPassage(), positionData));
		}
    }

    public void RandomizeContainmentUnits()
    {
		List<CreaturePositionData> list = new List<CreaturePositionData>(creaturePositions);
        foreach (CreatureRoomData roomData in creatureRooms)
		{
			if (list.Count <= 0) break;
			int ind = UnityEngine.Random.Range(0, list.Count);
			CreaturePositionData positionData = list[ind];
			list.RemoveAt(ind);
			SetToPosition(roomData, positionData);
		}
		List<CreatureModel> list2 = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
		List<CreatureModel> list3 = new List<CreatureModel>();
		List<int> list4 = new List<int>();
		int a = 0;
        while (list2.Count > 0)
		{
			int ind = UnityEngine.Random.Range(0, list2.Count);
			CreatureModel creature = list2[ind];
			list2.RemoveAt(ind);
			if (creature.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
			{
				list3.Add(creature);
				if (a > 0)
				{
					list4.Add(a);
				}
				a++;
			}
		}
		int num = 6;
		List<int> list5 = new List<int>();
		list5.Add(0);
		for (int i = 1; i < num; i++)
		{
			int ind = UnityEngine.Random.Range(0, list4.Count);
			int result = list4[ind];
			list4.RemoveAt(ind);
			list5.Add(result);
		}
		for (int i = 0; i < list3.Count; i++)
		{
			IsolateRoom room = list3[i].Unit.room;
			List<CreatureTypeInfo> metaInfos = new List<CreatureTypeInfo>();
			foreach (int n in list5)
			{
				metaInfos.Add(list3[(i + n) % list3.Count].metaInfo);
			}
			List<CreatureTypeInfo> metaInfos2 = new List<CreatureTypeInfo>();
			room.DescController.Teriminate();
			while (metaInfos.Count > 0)
			{
				int ind = UnityEngine.Random.Range(0, metaInfos.Count);
				CreatureTypeInfo metaInfo = metaInfos[ind];
				metaInfos.RemoveAt(ind);
				metaInfos2.Add(metaInfo);
				string desc = metaInfo.collectionName;
				room.WorkDescFreq = (float)(num - metaInfos.Count) / (float)num;
				room.DescController.Display(desc, -1);
			}
			room.WorkDescFreq = 1f;
			room.SetOvertimeYesodOptions(metaInfos2);
			bool[] hints = new bool[7];
			switch (_phase)
			{
				case 0:
					break;
				case 1:
					hints[0] = true;
					hints[UnityEngine.Random.Range(1, 3)] = true;
					break;
				case 2:
					hints[0] = true;
					hints[1] = true;
					hints[2] = true;
					hints[UnityEngine.Random.Range(3, 5)] = true;
					hints[UnityEngine.Random.Range(5, 7)] = true;
					break;
			}
			room.SetOvertimeYesodHints(hints);
		}
        foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
		{
			if (creature.metaInfo.creatureWorkType == CreatureWorkType.KIT) continue;
			IsolateRoom room = creature.Unit.room;
			/*if (creature.currentSkill != null)
			{
				return;
			}*/
			room.ProgressBar.InitializeState();
			if (room.IsWorkAllocated)
			{
				Notice.instance.Send(NoticeName.ManageCancel, new object[]
				{
					creature
				});
				room.OnCancelWork();
			}
			room.ClearWorkOrderQueue();
			if (creature.feelingState != CreatureFeelingState.NONE)
			{
				creature.feelingStateRemainTime = 0f;
				creature.script.OnAllocatedWork(null);
				creature.script.OnReleaseWorkAllocated();
				creature.script.OnWorkCoolTimeEnd(creature.feelingState);
				Notice.instance.Send(NoticeName.OnWorkCoolTimeEnd, new object[]
				{
					this,
					creature.feelingState
				});
				room.OnFeelingStateDisplayEnd();
				creature.feelingState = CreatureFeelingState.NONE;
			}
		}
    }

    public void ResetContainmentUnits()
    {
        foreach (CreatureRoomData roomData in creatureRooms)
		{
			SetToPosition(roomData, roomData.original);
			MapEdge edge = roomData.edge;
            edge.originalNode1 = null;
            edge.originalNode2 = null;
            edge.originalCost = -1f;
		}
        foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
		{
			creature.Unit.room.DisableOvertimeYesodBoss();
			creature.Unit.room.OnObservationLevelChanged();
			creature.Unit.room.transform.position = creature.basePosition;
		}
    }

	public void SetToPosition(CreatureRoomData roomData, CreaturePositionData positionData)
	{
		IsolateRoom room = roomData.room;
		CreatureModel creature = room.TargetUnit.model;
		room.transform.position = positionData.position;
		roomData.edge.node1.RemoveEdge(roomData.edge);
		roomData.edge.node1 = positionData.node;
		roomData.edge.node1.AddEdge(roomData.edge);
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		if (positionData.isolatePos != IsolatePos.DOWN)
		{
			if (positionData.isolatePos == IsolatePos.UP)
			{
				zero.y -= 4.4f;
				zero2.y -= 4.4f;
			}
		}
		else
		{
			zero.y += 4.4f;
			zero2.y += 4.4f;
		}
		zero.x = 0.31f;
		room.NarrationFadeEffect.transform.localPosition = zero2;
		room.SpecialSkillRoot.transform.localPosition = zero;
		room.RoomColor = UIColorManager.instance.GetDisabledRoomColor(positionData.sefira.sefiraEnum);
		creature.sefiraOrigin = positionData.sefira;
		creature.sefiraNum = positionData.sefira.indexString;
		if (true)
		{
			creature.sefira = positionData.sefira;
		}
		creature.entryNode = positionData.entryNode;
		positionData.entryNode.connectedCreature = creature;
		creature.entryNodeId = positionData.entryNode.GetId();
		roomData.passage.position = new Vector3(positionData.position.x, positionData.position.y, 0f);
		Vector2 adjust2 = positionData.position - roomData.curPos;
		Vector3 adjust3 = new Vector3(adjust2.x, adjust2.y, 0f);
		foreach (MapNode node in roomData.passage.GetNodeList())
		{
			node.SetPosition(node.GetPosition() + adjust3);
		}
		roomData.curPos = positionData.position;
	}

	private float totalEnergy;

	private SefiraBossCreatureModel model;

	private CameraFilterPack_Noise_TV noiseTv;

	private CameraFilterPack_TV_Vignetting vignetting;

	private CameraFilterPack_FX_Glitch3 glitch3;

	private Timer _startEffectTimer = new Timer();

	private int _phase = 0;

	private Timer _forcePhaseTimer = new Timer();

	private List<CreaturePositionData> creaturePositions = new List<CreaturePositionData>();

	private List<CreatureRoomData> creatureRooms = new List<CreatureRoomData>();

	public class CreaturePositionData
	{
		public CreaturePositionData(MapNode _node, MapNode _entryNode, Vector2 _position, Sefira _sefira, IsolatePos _isolatePos)
		{
			node = _node;
			entryNode = _entryNode;
			position = _position;
			sefira = _sefira;
			isolatePos = _isolatePos;
		}

		public MapNode node;

		public MapNode entryNode;

		public Vector2 position;

		public Sefira sefira;

		public IsolatePos isolatePos;
	}

	public class CreatureRoomData
	{
		public CreatureRoomData(IsolateRoom _room, MapEdge _edge, PassageObjectModel _passage, CreaturePositionData _original)
		{
			room = _room;
			edge = _edge;
			passage = _passage;
			original = _original;
			curPos = original.position;
		}

		public IsolateRoom room;

		public MapEdge edge;

		public PassageObjectModel passage;

		public CreaturePositionData original;

		public Vector2 curPos;
	}
}
