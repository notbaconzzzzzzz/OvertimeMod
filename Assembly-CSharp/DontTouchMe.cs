using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003EF RID: 1007
public class DontTouchMe : CreatureBase
{
	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x060021B8 RID: 8632 RVA: 0x00022CED File Offset: 0x00020EED
	public DontTouchMeAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as DontTouchMeAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x00103510 File Offset: 0x00101710
	public override void OnInit()
	{
		Sprite[] collection = Resources.LoadAll<Sprite>("Sprites/CreatureSprite/Isolate/Skill");
		this.randomFilterList.AddRange(collection);
		this.randomFilterList.RemoveAll((Sprite x) => x.name.Contains("100102"));
		this.randomFilterList.RemoveAll((Sprite x) => x.name.Contains("300006"));
		this.touchKillSprites = Resources.LoadAll<Sprite>("Sprites/effect/touchKill");
		this.touchWarningSprites = Resources.LoadAll<Sprite>("Sprites/effect/touchWarning");
		this._maxWorkCount = UnityEngine.Random.Range(2, 6);
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x00022D1C File Offset: 0x00020F1C
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetCreatureScript(this);
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x001035B4 File Offset: 0x001017B4
	public override void ParamInit()
	{
		this.skillCoolTimer.StopTimer();
		this.illusionTimer.StopTimer();
		this.filterTimer.StopTimer();
		this.isolateClicked = new Queue<float>();
		if (this.allocateFilters.Count > 0)
		{
			foreach (MonoBehaviour obj in this.allocateFilters)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
		this.allocateFilters = new List<MonoBehaviour>();
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x00022D31 File Offset: 0x00020F31
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
		this.skillCoolTimer.StartTimer(50f);
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x00022D4F File Offset: 0x00020F4F
	public override void OnStageRelease()
	{
		base.OnStageEnd();
		this.ParamInit();
		this._maxWorkCount = UnityEngine.Random.Range(2, 6);
	}

	// Token: 0x060021BE RID: 8638 RVA: 0x00103658 File Offset: 0x00101858
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this.skillCoolTimer.RunTimer())
		{
			if (this.Prob(50))
			{
				this.MakeIllusion();
			}
			else
			{
				this.ShowRandomFilter();
			}
		}
		if (this.illusionTimer.RunTimer())
		{
			this.RemoveIllusion();
		}
		if (this.filterTimer.RunTimer())
		{
			this.HideRandomFilter();
		}
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x001036C0 File Offset: 0x001018C0
	private void ShowRandomFilter()
	{
		if (this.randomFilterList.Count <= 0)
		{
			return;
		}
		this.filterTimer.StartTimer(15f);
		base.Unit.room.StateFilter.renderSprite = this.randomFilterList[UnityEngine.Random.Range(0, this.randomFilterList.Count)];
		base.Unit.room.StateFilter.Activated = true;
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x00022D6A File Offset: 0x00020F6A
	private void HideRandomFilter()
	{
		this.skillCoolTimer.StartTimer(50f);
		base.Unit.room.StateFilter.Activated = false;
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x00103738 File Offset: 0x00101938
	private void MakeIllusion()
	{
		this.illusionTimer.StartTimer(15f);
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(this.illusionCreatureList[UnityEngine.Random.Range(0, this.illusionCreatureList.Length)]);
		GameObject gameObject = Prefab.LoadPrefab(data.animSrc);
		this.illusionCreature = gameObject.GetComponent<CreatureAnimScript>();
		gameObject.transform.SetParent(base.Unit.transform, false);
		base.Unit.animTarget.gameObject.SetActive(false);
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x001037BC File Offset: 0x001019BC
	private void RemoveIllusion()
	{
		this.skillCoolTimer.StartTimer(50f);
		if (this.illusionCreature != null)
		{
			UnityEngine.Object.Destroy(this.illusionCreature.gameObject);
			this.illusionCreature = null;
			base.Unit.animTarget.gameObject.SetActive(true);
		}
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x00103818 File Offset: 0x00101A18
	public override bool OnOpenWorkWindow()
	{
		if (this.AddExitClickCount())
		{
			return false;
		}
		PlaySpeedSettingUI.instance.ForcelyPlay();
		if (base.Unit.animTarget.isActiveAndEnabled)
		{
			base.Unit.animTarget.SendMessage("PlayAnimation");
		}
		if (this.Prob(50))
		{
			this.KillAllWorkerStart();
		}
		else
		{
			this.PanicAllWorker();
		}
		return false;
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x00022D92 File Offset: 0x00020F92
	public override bool OnOpenCollectionWindow()
	{
		if (GameManager.currentGameManager.ManageStarted)
		{
			this.SetAllQliphothCounter();
		}
		else
		{
			this.ClickOnAllocate();
		}
		return false;
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x00022DB5 File Offset: 0x00020FB5
	public override int GetMaxWorkCountView()
	{
		return this._maxWorkCount;
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x00103888 File Offset: 0x00101A88
	private void KillAllWorkerStart()
	{
		base.Unit.PlaySoundMono(this.deadSound[UnityEngine.Random.Range(0, this.deadSound.Length)]);
		UIEffectManager.instance.StartCustomEffect(this.touchKillSprites, 24, new Callback(this.KillAllWorker));
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x001038D4 File Offset: 0x00101AD4
	private void KillAllWorker()
	{
		foreach (WorkerModel workerModel in WorkerManager.instance.GetWorkerList())
		{
			workerModel.Die();
		}
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x00103934 File Offset: 0x00101B34
	private void PanicAllWorker()
	{
		base.Unit.PlaySoundMono("panic");
		CameraMover.instance.Recoil(1, 3f);
		foreach (WorkerModel workerModel in WorkerManager.instance.GetWorkerList())
		{
			workerModel.mental = 0f;
			workerModel.Panic();
		}
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x001039C0 File Offset: 0x00101BC0
	private void SetAllQliphothCounter()
	{
		if (this.AddExitClickCount())
		{
			return;
		}
		PlaySpeedSettingUI.instance.ForcelyPlay();
		if (base.Unit.animTarget.isActiveAndEnabled)
		{
			base.Unit.animTarget.SendMessage("PlayAnimation");
		}
		base.Unit.PlaySoundMono("moodDown");
		UIEffectManager.instance.StartCustomEffect(this.touchWarningSprites, 24);
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			creatureModel.SetQliphothCounter(0);
		}
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x00103A5C File Offset: 0x00101C5C
	private void ClickOnAllocate()
	{
		base.Unit.PlaySoundMono("glass");
		if (this.exitTimer.started)
		{
			return;
		}
		if (this.allocateFilters.Count >= 3)
		{
			base.Unit.PlaySoundMono("shout");
			this.exitTimer.Init();
			this.exitTimer.StartTimer(4f, new AutoTimer.TargetMethod(this.ExitGame), AutoTimer.UpdateMode.UPDATE);
		}
		else
		{
			Camera cam = UIActivateManager.instance.GetCam();
			CameraFilterPack_TV_BrokenGlass cameraFilterPack_TV_BrokenGlass = cam.gameObject.AddComponent<CameraFilterPack_TV_BrokenGlass>();
			int count = this.allocateFilters.Count;
			if (count != 0)
			{
				if (count != 1)
				{
					if (count == 2)
					{
						cameraFilterPack_TV_BrokenGlass.Broken_Big = 128f;
					}
				}
				else
				{
					cameraFilterPack_TV_BrokenGlass.Broken_High = 128f;
				}
			}
			else
			{
				cameraFilterPack_TV_BrokenGlass.Broken_High = 36f;
			}
			Debug.Log(cameraFilterPack_TV_BrokenGlass);
			this.allocateFilters.Add(cameraFilterPack_TV_BrokenGlass);
		}
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x00103B58 File Offset: 0x00101D58
	private void ExitStart()
	{
		if (this.exitTimer.started)
		{
			return;
		}
		PlaySpeedSettingUI.instance.ForcelyPlay();
		PlaySpeedSettingUI.instance.BlockSetting(this);
		if (base.Unit.animTarget.isActiveAndEnabled)
		{
			base.Unit.animTarget.SendMessage("PlayAnimation");
		}
		base.Unit.PlaySoundMono("shout");
		CameraMover.instance.Recoil(2, 5f);
		this.exitTimer.Init();
		this.exitTimer.StartTimer(4f, new AutoTimer.TargetMethod(this.ExitGame), AutoTimer.UpdateMode.UPDATE);
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x00103C00 File Offset: 0x00101E00
	private bool AddExitClickCount()
	{
		this.isolateClicked.Enqueue(Time.realtimeSinceStartup);
		while (this.isolateClicked.Count > 0 && this.isolateClicked.Peek() + 10f < Time.realtimeSinceStartup)
		{
			this.isolateClicked.Dequeue();
		}
		if (this.isolateClicked.Count >= 5)
		{
			this.ExitStart();
			return true;
		}
		return false;
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x00103C74 File Offset: 0x00101E74
	private void ExitGame()
	{
        if (!SpecialModeConfig.instance.GetValue<bool>("DontTouchMeFix"))
        {
            int observationLevel = this.model.GetObservationLevel();
            this.model.observeInfo.OnObserveRegion("stat");
            if (observationLevel < this.model.GetObservationLevel())
            {
                this.model.AddObservationLevel();
            }
            GlobalGameManager.instance.SaveGlobalData();
        }
		SceneManager.LoadScene("ForceExitScene");
	}

	// Token: 0x04002122 RID: 8482
	private const int allocateClickMax = 3;

	// Token: 0x04002123 RID: 8483
	private const int isolateClickMax = 5;

	// Token: 0x04002124 RID: 8484
	private Queue<float> isolateClicked = new Queue<float>();

	// Token: 0x04002125 RID: 8485
	private Timer skillCoolTimer = new Timer();

	// Token: 0x04002126 RID: 8486
	private const float skillCoolTime = 50f;

	// Token: 0x04002127 RID: 8487
	private Timer illusionTimer = new Timer();

	// Token: 0x04002128 RID: 8488
	private const float illusionTime = 15f;

	// Token: 0x04002129 RID: 8489
	private Timer filterTimer = new Timer();

	// Token: 0x0400212A RID: 8490
	private const float filterTime = 15f;

	// Token: 0x0400212B RID: 8491
	private const int illusionProb = 50;

	// Token: 0x0400212C RID: 8492
	private const int dieProb = 50;

	// Token: 0x0400212D RID: 8493
	private CreatureAnimScript illusionCreature;

	// Token: 0x0400212E RID: 8494
	private long[] illusionCreatureList = new long[]
	{
		100002L,
		100009L,
		100000L,
		100006L
	};

	// Token: 0x0400212F RID: 8495
	private List<Sprite> randomFilterList = new List<Sprite>();

	// Token: 0x04002130 RID: 8496
	private const string filterDirectory = "Sprites/CreatureSprite/Isolate/Skill";

	// Token: 0x04002131 RID: 8497
	private int _maxWorkCount = 2;

	// Token: 0x04002132 RID: 8498
	private string[] deadSound = new string[]
	{
		"dead1",
		"dead2"
	};

	// Token: 0x04002133 RID: 8499
	private const string qliphothDownSound = "moodDown";

	// Token: 0x04002134 RID: 8500
	private const string offSound = "off";

	// Token: 0x04002135 RID: 8501
	private const string shoutSound = "shout";

	// Token: 0x04002136 RID: 8502
	private const string panicSound = "panic";

	// Token: 0x04002137 RID: 8503
	private const string glassSound = "glass";

	// Token: 0x04002138 RID: 8504
	private AutoTimer exitTimer = new AutoTimer();

	// Token: 0x04002139 RID: 8505
	private const float exitDelay = 4f;

	// Token: 0x0400213A RID: 8506
	private Sprite[] touchKillSprites;

	// Token: 0x0400213B RID: 8507
	private Sprite[] touchWarningSprites;

	// Token: 0x0400213C RID: 8508
	private List<MonoBehaviour> allocateFilters = new List<MonoBehaviour>();

	// Token: 0x0400213D RID: 8509
	private DontTouchMeAnim _animScript;
}
