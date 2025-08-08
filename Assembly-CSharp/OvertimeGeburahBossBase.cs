using System;
using System.Collections.Generic;
using LobotomyBaseMod;
using GeburahBoss;
using UnityEngine;

public class OvertimeGeburahBossBase : SefiraBossBase
{
	public OvertimeGeburahBossBase()
	{
		sefiraEnum = SefiraEnum.GEBURAH;
		ColorUtility.TryParseHtmlString("#350000FF", out hexa_black);
		ColorUtility.TryParseHtmlString("#FF0000FF", out hexa_red);
	}

	private OvertimeGeburahCoreScript Script
	{
		get
		{
			return model.script as OvertimeGeburahCoreScript;
		}
	}

	public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
	{
		SefiraBossDescType result = SefiraBossDescType.DEFAULT;
		if (UnityEngine.Random.value <= defaultProb)
		{
			return result;
		}
		switch (Script.Phase)
		{
		case GeburahPhase.START:
		case GeburahPhase.P1:
			result = SefiraBossDescType.OVERLOAD1;
			break;
		case GeburahPhase.P2:
			result = SefiraBossDescType.OVERLOAD2;
			break;
		case GeburahPhase.P3:
			result = SefiraBossDescType.OVERLOAD3;
			break;
		default:
			result = SefiraBossDescType.OVERLOAD4;
			break;
		}
		return result;
	}

	public override void OnStageStart()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-geburah-4");
		model = SefiraBossManager.Instance.AddCreature(nodeById, this, "OvertimeGeburahCoreScript", "GeburahCoreAnim", 400001L);
		model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		vignetting.Vignetting = 1f;
		vignetting.VignettingFull = 0.245f;
		vignetting.VignettingDirt = 0.476f;
		vignetting.VignettingColor = Color.black;
		superComputer = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperComputer>();
		superComputer._AlphaHexa = 1f;
		superComputer.ShapeFormula = 9f;
		superComputer.Shape = 0.77f;
		superComputer._BorderSize = 0.28f;
		superComputer._BorderColor = Color.red;
		superComputer.Radius = 0.92f;
		hexagon = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperHexagon>();
		hexagon._HexaColor = hexa_black;
		hexagon._AlphaHexa = 1f;
		hexagon.HexaSize = 2.93f;
		hexagon._SpotSize = 2.5f;
		hexagon.Radius = 0.25f;
		hexagon.enabled = false;
		_phase = 0;
		Script.SetBossBase(this);
		Script.maximumPhase = GeburahPhase.P1;
		Script.OnStageStart();
		StartHexagonEffect();
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Custom/Sounds/BGM/Boss/Geburah/1_Geburah_Battle1",
			"Custom/Sounds/BGM/Boss/Geburah/2_Geburah_Battle2",
			"Custom/Sounds/BGM/Boss/Geburah/3_Geburah_Battle3"
		});
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		_cameraDescTimer.StartTimer(5f * UnityEngine.Random.value);
	}

	public override void OnOverloadActivated(int currentLevel)
	{
		if (QliphothOverloadLevel == 1)
		{
			SpawnAbnoWave(1);
		}
		if (QliphothOverloadLevel == 2)
		{
			SpawnAbnoWave(2);
		}
		if (QliphothOverloadLevel == 3)
		{
			SpawnAbnoWave(3);
		}
		if (QliphothOverloadLevel == 4)
		{
			SpawnAbnoWave(4);
		}
		if (QliphothOverloadLevel == 2)
		{
			OnChangePhase();
			Script.maximumPhase = GeburahPhase.P2;
			if (Script._recoverTimer.started && Script._recoverTimer.maxTime > 10f) Script._recoverTimer.StartTimer(1f);
		}
		if (QliphothOverloadLevel == 4)
		{
			Script.maximumPhase = GeburahPhase.P3;
			if (Script._recoverTimer.started && Script._recoverTimer.maxTime > 10f) Script._recoverTimer.StartTimer(1f);
		}
		if (QliphothOverloadLevel == 6)
		{
			OnChangePhase();
			Script.maximumPhase = GeburahPhase.P4;
			if (Script._recoverTimer.started && Script._recoverTimer.maxTime > 10f) Script._recoverTimer.StartTimer(1f);
		}
	}

	public override void OnKetherStart()
	{
	}

	public override bool IsStartEmergencyBgm()
	{
		return false;
	}

	public void InitModel()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-geburah-4");
		model = SefiraBossManager.Instance.AddCreature(nodeById, this, "OvertimeGeburahCoreScript", "GeburahCoreAnim", 400001L);
		model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		Script.SetBossBase(this);
		Script.maximumPhase = GeburahPhase.P1;
		Script.OnStageStart();
	}

	private void StartCameraMoveEndFirst()
	{
		CameraMover.instance.ReleaseMove();
	}

	public override bool IsCleared()
	{
		return Script.Phase == GeburahPhase.END;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (model != null)
		{
			model.OnFixedUpdate();
		}
	}

	public override void Update()
	{
		base.Update();
		if (_hexaEffect.started)
		{
			Color hexaColor = Color.Lerp(hexa_red, hexa_black, _hexaEffect.Rate);
			hexagon._HexaColor = hexaColor;
			if (_hexaEffect.RunTimer())
			{
				hexagon.enabled = false;
				if (!_isInit)
				{
					_isInit = true;
					Vector3 currentViewPosition = model.GetCurrentViewPosition();
					currentViewPosition.y += 2f * model.GetMovableNode().currentScale;
					CameraMover.instance.SetEndCall(new CameraMover.OnCameraMoveEndEvent(StartCameraMoveEndFirst));
					CameraMover.instance.StopMove();
					CameraMover.instance.CameraMoveEvent(currentViewPosition, 8f);
				}
			}
		}
	}

	public void StartHexagonEffect()
	{
		if (hexagon != null)
		{
			hexagon._HexaColor = hexa_red;
			_hexaEffect.StartTimer(4f);
			hexagon.enabled = true;
		}
	}

	public override void OnChangePhase()
	{
		_phase++;
		if (!SefiraBossManager.Instance.IsKetherBoss())
		{
			SefiraBossManager.Instance.PlayBossBgm(_phase);
		}
	}

	public override void OnCleared()
	{
		base.OnCleared();
		Vector3 currentViewPosition = Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	public void OnGeburaSuppress(GeburahPhase phase)
	{
		switch (phase)
		{
			case GeburahPhase.P1:
				SpawnAbnoWave(1);
				break;
			case GeburahPhase.P2:
				SpawnAbnoWave(2);
				break;
			case GeburahPhase.P3:
				SpawnAbnoWave(3);
				break;
			case GeburahPhase.P4:
				SpawnAbnoWave(4);
				break;
		}
	}

	public void SpawnAbnoWave(int level)
	{
		Sefira[] sefiras = SefiraManager.instance.GetActivatedSefiras();
		foreach (SpawnInfo spawnInfo in spawnList[level])
		{
			try
			{
				Sefira sefira = sefiras[UnityEngine.Random.Range(0, sefiras.Length)];
				CreatureModel creature = CreatureManager.instance.AddCreature_Mod(spawnInfo.id, null, sefira.indexString);
				creature.SetCurrentNode(sefira.GetRandomWayPoint());
				creature.script.ForceSpawnWithoutRoom();
			}
			catch (Exception ex)
			{
				LobotomyBaseMod.ModDebug.Log("Failed to spawn abno " + spawnInfo.id.ToString() + " : " + ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
	}

	public SefiraBossCreatureModel model;

	private CameraFilterPack_TV_Vignetting vignetting;

	private CameraFilterPack_AAA_SuperComputer superComputer;

	private CameraFilterPack_AAA_SuperHexagon hexagon;

	private Color hexa_black = Color.black;

	private Color hexa_red = Color.red;

	private UnscaledTimer _hexaEffect = new UnscaledTimer();

	private bool _isInit;

	private int _phase = 0;

	public static List<SpawnInfo>[] spawnList = new List<SpawnInfo>[] {
		new List<SpawnInfo>() {
			
		},
		new List<SpawnInfo>() {
			new SpawnInfo(100001L),
			new SpawnInfo(100018L),
			new SpawnInfo(100020L), // Punishing Bird
			new SpawnInfo(100036L),
			new SpawnInfo(100054L), // Meat Lantern
			new SpawnInfo(100060L),
			new SpawnInfo(100106L),
			new SpawnInfo("NotbaconOvertimeMod", 130917L), // Coral Reef
			// new SpawnInfo("NotbaconOvertimeMod", 130932L), // Balloon
		},
		new List<SpawnInfo>() {
			new SpawnInfo(100011L),
			new SpawnInfo(100016L),
			// new SpawnInfo(100026L), // Queen Bee
			// new SpawnInfo(100040L), // Little Prince
			// new SpawnInfo(100041L), // Laetitia
			new SpawnInfo(100043L), // Funeral
			new SpawnInfo(100049L),
			new SpawnInfo(100050L),
			new SpawnInfo(100051L), // Woodsman
			new SpawnInfo(100057L),
			// new SpawnInfo(100062L), // Parasite Tree
			// new SpawnInfo(100063L), // Melting Love
			// new SpawnInfo("NotbaconOvertimeMod", 130906L), // Loney Wraith
		},
		new List<SpawnInfo>() {
			new SpawnInfo(100004L), // Queen of Hatred
			new SpawnInfo(100008L),
			new SpawnInfo(100023L), // Snow White's Apple
			new SpawnInfo(100029L),
			new SpawnInfo(100032L),
			new SpawnInfo(100033L),
			new SpawnInfo(100035L),
			new SpawnInfo(100039L),
			new SpawnInfo(100044L),
			new SpawnInfo(100045L), // Dream of a Black Swan
			new SpawnInfo(100047L),
			new SpawnInfo(100048L), // Knight of Despair
			new SpawnInfo(100055L),
			new SpawnInfo(100061L),
			// new SpawnInfo(100063L), // Melting Love
			new SpawnInfo(100065L), // La Luna
			new SpawnInfo(100104L), // Yin
			new SpawnInfo(300109L), // Yang
			new SpawnInfo(100105L),
			// new SpawnInfo("NotbaconOvertimeMod", 130910L), // Angler Fish
		},
		new List<SpawnInfo>() {
			new SpawnInfo(100005L),
			new SpawnInfo(100019L), // Silent Orchestra
			new SpawnInfo(100042L),
			new SpawnInfo(100056L),
			new SpawnInfo(100058L),
			new SpawnInfo(100063L), // Melting Love
			new SpawnInfo(100064L), // Army in Black
			// new SpawnInfo("NotbaconOvertimeMod", 130901L), // Shattered Mirror
		},
		new List<SpawnInfo>() {
			new SpawnInfo(100015L), // WhiteNight
			new SpawnInfo(100038L), // Apocalypse Bird
			// new SpawnInfo("NotbaconOvertimeMod", 130913L), // Pollen
			// new SpawnInfo("NotbaconOvertimeMod", 130922L), // Dimensional Rift Generator
			// new SpawnInfo("NotbaconOvertimeMod", 130926L), // Vestige of Pain
			// new SpawnInfo("NotbaconOvertimeMod", 130927L), // Vestige of Grief
			// new SpawnInfo("NotbaconOvertimeMod", 130928L), // Vestige of Ruin
			// new SpawnInfo("NotbaconOvertimeMod", 130929L), // Vestige of Oblivion
			// new SpawnInfo("NotbaconOvertimeMod", 130941L), // The Voiceless Symphony
		}
	};

	public class SpawnInfo
	{
		public SpawnInfo(long i)
		{
			id = new LcIdLong(i);
		}

		public SpawnInfo(string package, long i)
		{
			id = new LcIdLong(package, i);
		}

		public SpawnInfo(LcIdLong i)
		{
			id = i;
		}

		public LcIdLong id;
	}
}
