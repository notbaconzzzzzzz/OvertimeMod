using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeNetzachBossBase : SefiraBossBase
{
	public OvertimeNetzachBossBase()
	{
		sefiraEnum = SefiraEnum.NETZACH;
	}

	private OvertimeNetzachCoreScript Script
	{
		get
		{
			return model.script as OvertimeNetzachCoreScript;
		}
	}

	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeNetzachCoreScript", "NetzachCoreAnim", 400001L);
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		Script.bossBase = this;
		waterDrop = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_WaterDrop>();
		waterDrop.Distortion = 40f;
		waterDrop.SizeX = 0.3f;
		waterDrop.SizeY = 0.5f;
		waterDrop.Speed = 4f;
		fog = Camera.main.gameObject.AddComponent<CameraFilterPack_Atmosphere_Fog>();
		fog._Near = 0f;
		fog._Far = 0.5f;
		Color green = Color.green;
		if (ColorUtility.TryParseHtmlString("#5BD417FF", out green))
		{
			fog.FogColor = green;
		}
		fog.Fade = 0.135f;
		vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		vignetting.Vignetting = 1f;
		vignetting.VignettingColor = Color.black;
		vignetting.VignettingDirt = 0f;
		vignetting.VignettingFull = 0f;
		_cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Custom/Sounds/BGM/Boss/Netzach/1_Netzach_Battle1",
			"Custom/Sounds/BGM/Boss/Netzach/2_Netzach_Battle2",
			"Custom/Sounds/BGM/Boss/Netzach/3_Netzach_Battle3"
		});
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		GenerateNetzachBuf();
        UpdateBufs(-0.1f, -0.2f, 1f);
	}

	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeNetzachCoreScript", "NetzachCoreAnim", 400001L);
		GameObject gameObject = model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		Script.bossBase = this;
	}

	public void StartEffect()
	{
		_startEffectTimer.StartTimer(Script.AnimScript.startEffectTime);
	}

	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= totalEnergy && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 8;
	}

	public override bool IsReadyToClose()
	{
		return base.IsReadyToClose();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_startEffectTimer.started)
		{
			float rate = _startEffectTimer.Rate;
			float distortion = Mathf.Lerp(40f, 64f, rate);
			float speed = Mathf.Lerp(0.19f, 4f, 1f - rate);
			if (_startEffectTimer.RunTimer())
			{
				distortion = 64f;
				speed = 0.19f;
			}
			if (waterDrop != null)
			{
				waterDrop.Distortion = distortion;
				waterDrop.Speed = speed;
			}
		}
	}

	public override float GetDescFreq()
	{
		if (QliphothOverloadLevel >= 3)
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
            UpdateBufs(-0.25f, -0.6f, 1f);
		}
		else if (QliphothOverloadLevel == 5)
		{
			OnChangePhase();
            UpdateBufs(-1f / 3f, -1f, 1f);
		}
	}

	public override void OnChangePhase()
	{
		_phase++;
		SefiraBossManager.Instance.PlayBossBgm(_phase);
        StartEffect();
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
    
    public void GenerateNetzachBuf()
	{
		bufList = new List<OvertimeNetzachBossBuf>();
		foreach (UnitModel unitModel in AgentManager.instance.GetAgentList())
		{
			OvertimeNetzachBossBuf netzachBossBuf = new OvertimeNetzachBossBuf();
			unitModel.AddUnitBuf(netzachBossBuf);
			bufList.Add(netzachBossBuf);
		}
	}

    public void UpdateBufs(float normal, float regenerator, float bullet)
    {
        foreach (OvertimeNetzachBossBuf buf in bufList)
        {
            buf.UpdateValues(normal, regenerator, bullet);
        }
    }

	private float totalEnergy;

	private SefiraBossCreatureModel model;

	private CameraFilterPack_AAA_WaterDrop waterDrop;

	private CameraFilterPack_Atmosphere_Fog fog;

	private CameraFilterPack_TV_Vignetting vignetting;

	private Timer _startEffectTimer = new Timer();

	public List<OvertimeNetzachBossBuf> bufList = new List<OvertimeNetzachBossBuf>();

	private int _phase = 0;
}
