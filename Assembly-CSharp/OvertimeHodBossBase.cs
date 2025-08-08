using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeHodBossBase : SefiraBossBase
{
	public OvertimeHodBossBase()
	{
		sefiraEnum = SefiraEnum.HOD;
	}

	private OvertimeHodCoreScript Script
	{
		get
		{
			return model.script as OvertimeHodCoreScript;
		}
	}

	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeHodCoreScript", "HodCoreAnim", 400001L);
		totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		_80 = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_80>();
		_80.Fade = 0.135f;
		_80.enabled = false;
		vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		vignetting.Vignetting = 1f;
		vignetting.VignettingFull = 0.245f;
		vignetting.VignettingDirt = 0.476f;
		vignetting.VignettingColor = Color.black;
		vhs = Camera.main.gameObject.AddComponent<CameraFilterPack_Real_VHS>();
		vhs.TRACKING = 0f;
		vhs.JITTER = 0.037f; // 0.074f
		vhs.GLITCH = 0.04f; // 0.08f
		vhs.NOISE = 0.329f; // 0.659f
		vhs.Brightness = 0.23f;
		vhs.Constrast = 0.796f;
		vhs.enabled = false;
		_cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Custom/Sounds/BGM/Boss/Hod/1_Hod_Battle1",
			"Custom/Sounds/BGM/Boss/Hod/2_Hod_Battle2",
			"Custom/Sounds/BGM/Boss/Hod/3_Hod_Battle3"
		});
		SefiraBossManager.Instance.PlayBossBgm(_phase);
        GenerateHodBuf();
        ReduceStats(0.4f, 35);
	}

	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "OvertimeHodCoreScript", "HodCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this.Script.bossBase = this;
	}

	public override float GetDescFreq()
	{
		if (this.QliphothOverloadLevel >= 3)
		{
			return 0.1f;
		}
		return 0.3f;
	}

	public override void OnOverloadActivated(int currentLevel)
	{
        if (IsCleared())
        {
            return;
        }
		if (QliphothOverloadLevel == 2)
		{
            ReduceStats(0.5f, 25);
			OnChangePhase();
		}
		else if (QliphothOverloadLevel == 5)
		{
            ReduceStats(0.6f, 15);
			OnChangePhase();
		}
        else if (QliphothOverloadLevel >= 8)
        {
            ReduceStats(0.25f + (QliphothOverloadLevel - 8) * 0.05f, 0);
        }
	}

	public override void OnChangePhase()
	{
		_phase++;
		SefiraBossManager.Instance.PlayBossBgm(_phase);
		if (_phase == 1)
		{
			if (_80 != null)
			{
				_80.enabled = true;
			}
		}
		else if (_phase == 2)
		{
            if (vhs != null && !vhs.enabled)
            {
                vhs.enabled = true;
            }
		}
		if (!SefiraBossManager.Instance.IsKetherBoss())
		{
			ShockwaveEffect.Invoker(model.GetCurrentViewPosition(), model, 3f, 600f, EffectLifetimeType.NORMAL);
			MakeSoundAttachCamera("SefiraBoss/Boss_Nezach");
		}
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
        foreach (OvertimeHodBossBuf buf in bufList)
        {
            buf.Reset();
        }
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
    
    public void GenerateHodBuf()
	{
		bufList = new List<OvertimeHodBossBuf>();
		foreach (UnitModel unitModel in AgentManager.instance.GetAgentList())
		{
			OvertimeHodBossBuf hodBossBuf = new OvertimeHodBossBuf();
			unitModel.AddUnitBuf(hodBossBuf);
			bufList.Add(hodBossBuf);
		}
	}

    public void ReduceStats(float factor, int min)
    {
        try
        {
            foreach (OvertimeHodBossBuf buf in bufList)
            {
                buf.ReduceStat(factor, min);
            }
        }
        catch (Exception ex)
        {
            Notice.instance.Send("AddSystemLog", new object[]
            {
                ex.Message + " : " + ex.StackTrace
            });
        }
    }

	private float totalEnergy;

	private SefiraBossCreatureModel model;

	private CameraFilterPack_TV_80 _80;

	private CameraFilterPack_TV_Vignetting vignetting;

	private CameraFilterPack_Real_VHS vhs;

	public List<OvertimeHodBossBuf> bufList = new List<OvertimeHodBossBuf>();

	private int _phase = 0;
}
