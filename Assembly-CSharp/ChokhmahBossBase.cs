using System;
using System.Collections.Generic;
using GameStatusUI;
using UnityEngine;

// Token: 0x02000807 RID: 2055
public class ChokhmahBossBase : SefiraBossBase
{
	// Token: 0x06003F65 RID: 16229 RVA: 0x00188930 File Offset: 0x00186B30
	public ChokhmahBossBase()
	{
		this.sefiraEnum = SefiraEnum.CHOKHMAH;
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06003F66 RID: 16230 RVA: 0x00036F05 File Offset: 0x00035105
	public ChokhmahCoreScript Script
	{
		get
		{
			return this.model.script as ChokhmahCoreScript;
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x00036F17 File Offset: 0x00035117
	public int ChokhmahQliphothLevel
	{
		get
		{
			return this._vhsLevel;
		}
	}

	// Token: 0x06003F68 RID: 16232 RVA: 0x0018898C File Offset: 0x00186B8C
	public override void Update()
	{
		base.Update();
		this.Script.Update();
		if (this._brokenTimer.started)
		{
			ChokhmahBossBase.BrokenScreen brokenScreen = ChokhmahBossBase.brokenStart.Lerp<ChokhmahBossBase.BrokenScreen>(ChokhmahBossBase.brokenEnd, this._brokenTimer.Rate);
			brokenScreen.SetFilterValue(this.screen);
			if (this._brokenTimer.RunTimer())
			{
				this.screen.enabled = false;
			}
		}
		if (this._vhsTimer.started)
		{
			ChokhmahBossBase.TV_Vhs tv_Vhs = ChokhmahBossBase.vhsStart.Lerp<ChokhmahBossBase.TV_Vhs>(ChokhmahBossBase.vhsEnd, this._vhsTimer.Rate);
			tv_Vhs.SetFilterValue(this.vhs);
			if (this._vhsTimer.RunTimer())
			{
				this.vhs.enabled = false;
				float grayscaleFade = this.GetGrayscaleFade(this._vhsLevel);
				if (grayscaleFade == 0f)
				{
					this.grayScale.enabled = false;
				}
				else
				{
					this.grayScale.enabled = true;
					this.grayScale._Fade = grayscaleFade;
				}
			}
		}
	}

	// Token: 0x06003F69 RID: 16233 RVA: 0x00036F1F File Offset: 0x0003511F
	public void StartBorkenEffect()
	{
		if (this._brokenTimer.started)
		{
			return;
		}
		this._brokenTimer.StartTimer(this._brokenTime);
		ChokhmahBossBase.brokenStart.SetFilterValue(this.screen);
		this.screen.enabled = true;
	}

	// Token: 0x06003F6A RID: 16234 RVA: 0x00036F5F File Offset: 0x0003515F
	public void StartVhsEffect()
	{
		if (this._vhsTimer.started)
		{
			return;
		}
		this._vhsTimer.StartTimer(this._vhsTime);
		ChokhmahBossBase.vhsStart.SetFilterValue(this.vhs);
		this.vhs.enabled = true;
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x00188A94 File Offset: 0x00186C94
	public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
	{
		if (UnityEngine.Random.value <= defaultProb)
		{
			return SefiraBossDescType.DEFAULT;
		}
		int phase = this._phase;
		switch (phase + 1)
		{
		case 0:
			return SefiraBossDescType.OVERLOAD1;
		case 1:
			return SefiraBossDescType.OVERLOAD1;
		}
		return SefiraBossDescType.OVERLOAD2;
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x00036F9F File Offset: 0x0003519F
	public override float GetDescFreq()
	{
		return base.GetDescFreq();
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x00188AD8 File Offset: 0x00186CD8
	public override void OnStageStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "ChokhmahCoreScript", "ChokhmahCoreAnim", 400001L);
		this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		this._cameraDescTimer.StartTimer(15f * UnityEngine.Random.value);
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/ChokhmahBlock");
		ChokhmahPlaySpeedBlockUI component = gameObject.GetComponent<ChokhmahPlaySpeedBlockUI>();
		component.SetChokhmaBossBase(this);
		PlaySpeedSettingUI.instance.AddBlockedEvent(PlaySpeedSettingBlockType.CHOKHMAHBOSS, component);
		this._ui = component;
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.screen = Camera.main.gameObject.AddComponent<CameraFilterPack_Broken_Screen>();
		this.screen.Fade = 0f;
		this.screen.Shadow = -0.96f;
		this.screen.enabled = false;
		this.vhs = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_VHS>();
		this.vhs.Cryptage = 164f;
		this.vhs.Parasite = 54f;
		this.vhs.Calibrage = 0f;
		this.vhs.WhiteParasite = 0f;
		this.vhs.enabled = false;
		this.grayScale = Camera.main.gameObject.AddComponent<CameraFilterPack_Color_GrayScale>();
		this.grayScale._Fade = 0f;
		this.grayScale.enabled = false;
		this.Script.SetBossBase(this);
		this._phase = -1;
		this._startDelayTimer.StartTimer(0.75f);
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Chokhmah/1_Tilarids - 090909090",
			"Sounds/BGM/Boss/Chokhmah/2_Tilarids - circle-rombed oxygen"
		});
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x00188CE0 File Offset: 0x00186EE0
	public override void OnKetherStart()
	{
		MapNode centerNode = base.Sefira.sefiraPassage.centerNode;
		this.model = SefiraBossManager.Instance.AddCreature(centerNode, this, "ChokhmahCoreScript", "ChokhmahCoreAnim", 400001L);
		GameObject gameObject = this.model.Unit.animTarget.animator.gameObject;
		gameObject.AddComponent<_FX_Hologram2_Spine>();
		GameObject gameObject2 = Prefab.LoadPrefab("Effect/SefiraBoss/ChokhmahBlock");
		ChokhmahPlaySpeedBlockUI component = gameObject2.GetComponent<ChokhmahPlaySpeedBlockUI>();
		component.SetChokhmaBossBase(this);
		PlaySpeedSettingUI.instance.AddBlockedEvent(PlaySpeedSettingBlockType.CHOKHMAHBOSS, component);
		this._ui = component;
		this.screen = Camera.main.gameObject.AddComponent<CameraFilterPack_Broken_Screen>();
		this.screen.Fade = 0f;
		this.screen.Shadow = -0.96f;
		this.screen.enabled = false;
		this.vhs = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_VHS>();
		this.vhs.Cryptage = 164f;
		this.vhs.Parasite = 54f;
		this.vhs.Calibrage = 0f;
		this.vhs.WhiteParasite = 0f;
		this.vhs.enabled = false;
		this.grayScale = Camera.main.gameObject.AddComponent<CameraFilterPack_Color_GrayScale>();
		this.grayScale._Fade = 0f;
		this.grayScale.enabled = false;
		this.Script.SetBossBase(this);
		this._phase = -1;
	}

	// Token: 0x06003F6F RID: 16239 RVA: 0x00188E54 File Offset: 0x00187054
	public void OnKehterOverloadActivated(int currentLevel)
	{
		float playSpeedForcely;
		if (currentLevel == 8)
		{
			playSpeedForcely = 1.5f;
		}
		else
		{
			if (currentLevel != 9)
			{
				return;
			}
			playSpeedForcely = 2.5f;
		}
		GameManager.currentGameManager.SetPlaySpeedForcely(playSpeedForcely);
		this.Script.AnimScript.SetAnimSpeed(ChokhmahBossBase.animValue[(currentLevel != 8) ? 4 : 0]);
		AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "SefiraBoss/Chokhmah/Chokma_Meltdown"));
		if (audioClip != null)
		{
			GlobalAudioManager.instance.PlayLocalClip(audioClip);
		}
		this._vhsLevel = currentLevel;
		this.StartVhsEffect();
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x00188EF8 File Offset: 0x001870F8
	public override void OnOverloadActivated(int currentLevel)
	{
		Debug.Log("Qliphoth : " + currentLevel);
		if (currentLevel == 3)
		{
			this.OnChangePhase();
			return;
		}
		int num = this.QliphothOverloadLevel - 6;
		if (currentLevel == 6)
		{
			this.OnChangePhase();
		}
		if (currentLevel >= 6)
		{
			float playSpeedForcely = ChokhmahBossBase.timeValue[num];
			GameManager.currentGameManager.SetPlaySpeedForcely(playSpeedForcely);
			this.Script.AnimScript.SetAnimSpeed(ChokhmahBossBase.animValue[num]);
			AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "SefiraBoss/Chokhmah/Chokma_Meltdown"));
			if (audioClip != null)
			{
				GlobalAudioManager.instance.PlayLocalClip(audioClip);
			}
			this._vhsLevel = currentLevel;
			this.StartVhsEffect();
		}
		if (currentLevel >= 10 && !this._qliphothClear)
		{
			this._qliphothClear = true;
		}
	}

	// Token: 0x06003F71 RID: 16241 RVA: 0x00036D86 File Offset: 0x00034F86
	private void StartCameraMoveEndFirst()
	{
		CameraMover.instance.ReleaseMove();
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x00188FC8 File Offset: 0x001871C8
	public override void FixedUpdate()
	{
		if (this._startDelayTimer.started)
		{
			if (this._startDelayTimer.RunTimer())
			{
				Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
				currentViewPosition.y += 5f * this.model.GetMovableNode().currentScale;
				CameraMover.instance.SetEndCall(new CameraMover.OnCameraMoveEndEvent(this.StartCameraMoveEndFirst));
				CameraMover.instance.StopMove();
				CameraMover.instance.CameraMoveEvent(currentViewPosition, 15f);
			}
			return;
		}
		base.FixedUpdate();
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x0018905C File Offset: 0x0018725C
	public override void OnChangePhase()
	{
		this._phase++;
		if (this._phase == 0 && !SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E3))
		{
			SefiraBossManager.Instance.PlayBossBgm(0);
		}
		if (this._phase == 1)
		{
			this.Script.AnimScript.OnChangePhase();
			if (!SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E3))
			{
				SefiraBossManager.Instance.PlayBossBgm(1);
			}
		}
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x001890D4 File Offset: 0x001872D4
	public override bool IsCleared()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return energy >= this.totalEnergy && this._qliphothClear;
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x00189104 File Offset: 0x00187304
	public override void OnCleared()
	{
		base.OnCleared();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 7f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 13f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x00189158 File Offset: 0x00187358
	public void OnTryTimePause()
	{
		this._currentSpaceCount++;
		this._ui.SetText(this.GetBattleDesc(ChokhmahBossBase.timeStopText));
		int num = this.GetTargetAgentCount();
		List<AgentModel> list = this.ExtractTargetableAgent();
		num = Math.Min(num, list.Count);
		if (num == 0)
		{
			return;
		}
		List<ChokhmahBossBase.ChokhmahPanelty> list2 = new List<ChokhmahBossBase.ChokhmahPanelty>();
		for (int i = 0; i < num; i++)
		{
			if (list.Count == 0)
			{
				break;
			}
			AgentModel agentModel = list[UnityEngine.Random.Range(0, list.Count)];
			list.Remove(agentModel);
			ChokhmahBossBase.ChokhmahPanelty item = new ChokhmahBossBase.ChokhmahPanelty
			{
				target = agentModel,
				paneltyType = this.GetPaneltyType(agentModel)
			};
			list2.Add(item);
		}
		this.ExecutePanelty(list2);
		this.StartBorkenEffect();
		AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "SefiraBoss/Chokhmah/Chokma_Space"));
		if (audioClip != null)
		{
			GlobalAudioManager.instance.PlayLocalClip(audioClip);
		}
	}

	// Token: 0x06003F77 RID: 16247 RVA: 0x00189258 File Offset: 0x00187458
	public void ExecutePanelty(List<ChokhmahBossBase.ChokhmahPanelty> panelties)
	{
		int count = panelties.Count;
		for (int i = count - 1; i >= 0; i--)
		{
			ChokhmahBossBase.ChokhmahPanelty chokhmahPanelty = panelties[i];
			if (chokhmahPanelty.paneltyType == ChokhmahBossBase.ChokhmahPaneltyType.DIE)
			{
				chokhmahPanelty.target.Die();
			}
			else if (chokhmahPanelty.paneltyType == ChokhmahBossBase.ChokhmahPaneltyType.PANIC && !chokhmahPanelty.target.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
			{
				chokhmahPanelty.target.mental = 0f;
				chokhmahPanelty.target.Panic();
			}
		}
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x00036FA7 File Offset: 0x000351A7
	public void OnTryTimeMultiply()
	{
		if (!this._ui.IsDisplaying)
		{
			this._ui.SetText(this.GetBattleDesc(ChokhmahBossBase.timeMultiplyText));
		}
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x001892DC File Offset: 0x001874DC
	private List<AgentModel> ExtractTargetableAgent()
	{
		List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
		List<AgentModel> list2 = new List<AgentModel>();
		foreach (AgentModel agentModel in list)
		{
			if (agentModel.IsAttackTargetable())
			{
				if (!agentModel.IsPanic())
				{
					if (!list2.Contains(agentModel))
					{
						if (!agentModel.invincible)
						{
							list2.Add(agentModel);
						}
					}
				}
			}
		}
		return list2;
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x00036FCF File Offset: 0x000351CF
	private ChokhmahBossBase.ChokhmahPaneltyType GetPaneltyType(AgentModel agent)
	{
		return (UnityEngine.Random.value > 0.5f) ? ChokhmahBossBase.ChokhmahPaneltyType.PANIC : ChokhmahBossBase.ChokhmahPaneltyType.DIE;
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00036FE7 File Offset: 0x000351E7
	public int GetTargetAgentCount()
	{
		return this._currentSpaceCount;
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x00036FEF File Offset: 0x000351EF
	public bool CheckFunction(PlaySpeedSettingBlockFunction function)
	{
		return function == PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER;
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x0018938C File Offset: 0x0018758C
	private string GetBattleDesc(params int[] id)
	{
		string empty = string.Empty;
		int index;
		if (id.Length > 1)
		{
			index = id[UnityEngine.Random.Range(0, id.Length)];
		}
		else
		{
			index = id[0];
		}
		if (SefiraBossManager.Instance.TryGetBossDesc(SefiraEnum.CHOKHMAH, SefiraBossDescType.BATTLE, index, out empty))
		{
		}
		return empty;
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x001893D8 File Offset: 0x001875D8
	private float GetGrayscaleFade(int level)
	{
		float result = 0f;
		if (level < 6)
		{
			return result;
		}
		return 0.175f * (float)level - 0.75f;
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x00189404 File Offset: 0x00187604
	// Note: this type is marked as 'beforefieldinit'.
	static ChokhmahBossBase()
	{
	}

	// Token: 0x04003A11 RID: 14865
	private const string animSrc = "ChokhmahCoreAnim";

	// Token: 0x04003A12 RID: 14866
	private const string chokhmahBase = "ChokhmahCoreScript";

	// Token: 0x04003A13 RID: 14867
	private const string bgm1 = "Chokhmah/1_Tilarids - 090909090";

	// Token: 0x04003A14 RID: 14868
	private const string bgm2 = "Chokhmah/2_Tilarids - circle-rombed oxygen";

	// Token: 0x04003A15 RID: 14869
	private const string soundPrefix = "SefiraBoss/Chokhmah/Chokma_Meltdown";

	// Token: 0x04003A16 RID: 14870
	private const string stopSound = "SefiraBoss/Chokhmah/Chokma_Space";

	// Token: 0x04003A17 RID: 14871
	private const int clearQliphothLevel = 10;

	// Token: 0x04003A18 RID: 14872
	private const int firstChangeQliphothLevel = 3;

	// Token: 0x04003A19 RID: 14873
	private const int changeQliphothLevel = 6;

	// Token: 0x04003A1A RID: 14874
	private static float[] timeValue = new float[]
	{
		1.5f,
		1.7f,
		1.9f,
		2.1f,
		2.5f
	};

	// Token: 0x04003A1B RID: 14875
	public static float[] ketherValue = new float[]
	{
		1.5f,
		2f,
		2.5f
	};

	// Token: 0x04003A1C RID: 14876
	private static float[] animValue = new float[]
	{
		1f,
		0.7f,
		0.4f,
		0.1f,
		0.05f
	};

	// Token: 0x04003A1D RID: 14877
	private static MinMax targetCount = new MinMax(3f, 5f);

	// Token: 0x04003A1E RID: 14878
	private static int[] timeStopText = new int[]
	{
		2,
		3
	};

	// Token: 0x04003A1F RID: 14879
	private static int[] timeMultiplyText = new int[]
	{
		0,
		1
	};

	// Token: 0x04003A20 RID: 14880
	private int _phase = -1;

	// Token: 0x04003A21 RID: 14881
	private float totalEnergy;

	// Token: 0x04003A22 RID: 14882
	private bool _qliphothAquired;

	// Token: 0x04003A23 RID: 14883
	private bool _qliphothClear;

	// Token: 0x04003A24 RID: 14884
	private Timer _startDelayTimer = new Timer();

	// Token: 0x04003A25 RID: 14885
	private int _currentSpaceCount;

	// Token: 0x04003A26 RID: 14886
	private SefiraBossCreatureModel model;

	// Token: 0x04003A27 RID: 14887
	private ChokhmahPlaySpeedBlockUI _ui;

	// Token: 0x04003A28 RID: 14888
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003A29 RID: 14889
	private CameraFilterPack_Broken_Screen screen;

	// Token: 0x04003A2A RID: 14890
	private CameraFilterPack_TV_VHS vhs;

	// Token: 0x04003A2B RID: 14891
	private CameraFilterPack_Color_GrayScale grayScale;

	// Token: 0x04003A2C RID: 14892
	private UnscaledTimer _brokenTimer = new UnscaledTimer();

	// Token: 0x04003A2D RID: 14893
	private float _brokenTime = 2f;

	// Token: 0x04003A2E RID: 14894
	private static ChokhmahBossBase.BrokenScreen brokenStart = new ChokhmahBossBase.BrokenScreen
	{
		fade = 0.3f,
		shadow = -0.96f
	};

	// Token: 0x04003A2F RID: 14895
	private static ChokhmahBossBase.BrokenScreen brokenEnd = new ChokhmahBossBase.BrokenScreen
	{
		fade = 0.02f,
		shadow = -0.96f
	};

	// Token: 0x04003A30 RID: 14896
	private UnscaledTimer _vhsTimer = new UnscaledTimer();

	// Token: 0x04003A31 RID: 14897
	private float _vhsTime = 1f;

	// Token: 0x04003A32 RID: 14898
	private int _vhsLevel;

	// Token: 0x04003A33 RID: 14899
	private static ChokhmahBossBase.TV_Vhs vhsStart = new ChokhmahBossBase.TV_Vhs
	{
		cryptage = 164f,
		parasite = 54f,
		calibrage = 1.5f,
		whiteparasite = 0f
	};

	// Token: 0x04003A34 RID: 14900
	private static ChokhmahBossBase.TV_Vhs vhsEnd = new ChokhmahBossBase.TV_Vhs
	{
		cryptage = 164f,
		parasite = 54f,
		calibrage = 0f,
		whiteparasite = 0f
	};

	// Token: 0x02000808 RID: 2056
	public abstract class CameraFilterValue
	{
		// Token: 0x06003F80 RID: 16256 RVA: 0x00004354 File Offset: 0x00002554
		protected CameraFilterValue()
		{
		}

		// Token: 0x06003F81 RID: 16257
		public abstract T Lerp<T>(T v2, float rate) where T : ChokhmahBossBase.CameraFilterValue;
	}

	// Token: 0x02000809 RID: 2057
	public class BrokenScreen : ChokhmahBossBase.CameraFilterValue
	{
		// Token: 0x06003F82 RID: 16258 RVA: 0x00036FF5 File Offset: 0x000351F5
		public BrokenScreen()
		{
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00189540 File Offset: 0x00187740
		public override T Lerp<T>(T v2, float rate)
		{
			ChokhmahBossBase.BrokenScreen brokenScreen = v2 as ChokhmahBossBase.BrokenScreen;
			ChokhmahBossBase.BrokenScreen brokenScreen2 = new ChokhmahBossBase.BrokenScreen
			{
				fade = Mathf.Lerp(this.fade, brokenScreen.fade, rate),
				shadow = Mathf.Lerp(this.shadow, brokenScreen.shadow, rate)
			};
			return brokenScreen2 as T;
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00036FFD File Offset: 0x000351FD
		public void SetFilterValue(CameraFilterPack_Broken_Screen filter)
		{
			filter.Fade = this.fade;
			filter.Shadow = this.shadow;
		}

		// Token: 0x04003A35 RID: 14901
		public float fade;

		// Token: 0x04003A36 RID: 14902
		public float shadow;
	}

	// Token: 0x0200080A RID: 2058
	public class TV_Vhs : ChokhmahBossBase.CameraFilterValue
	{
		// Token: 0x06003F85 RID: 16261 RVA: 0x00036FF5 File Offset: 0x000351F5
		public TV_Vhs()
		{
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x001895A4 File Offset: 0x001877A4
		public override T Lerp<T>(T v2, float rate)
		{
			ChokhmahBossBase.TV_Vhs tv_Vhs = v2 as ChokhmahBossBase.TV_Vhs;
			ChokhmahBossBase.TV_Vhs tv_Vhs2 = new ChokhmahBossBase.TV_Vhs
			{
				cryptage = Mathf.Lerp(this.cryptage, tv_Vhs.cryptage, rate),
				parasite = Mathf.Lerp(this.parasite, tv_Vhs.parasite, rate),
				calibrage = Mathf.Lerp(this.calibrage, tv_Vhs.calibrage, rate),
				whiteparasite = Mathf.Lerp(this.whiteparasite, tv_Vhs.whiteparasite, rate)
			};
			return tv_Vhs2 as T;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x00037017 File Offset: 0x00035217
		public void SetFilterValue(CameraFilterPack_TV_VHS filter)
		{
			filter.Cryptage = this.cryptage;
			filter.Parasite = this.parasite;
			filter.Calibrage = this.calibrage;
			filter.WhiteParasite = this.whiteparasite;
		}

		// Token: 0x04003A37 RID: 14903
		public float cryptage;

		// Token: 0x04003A38 RID: 14904
		public float parasite;

		// Token: 0x04003A39 RID: 14905
		public float calibrage;

		// Token: 0x04003A3A RID: 14906
		public float whiteparasite;
	}

	// Token: 0x0200080B RID: 2059
	public class GrayScaleValue : ChokhmahBossBase.CameraFilterValue
	{
		// Token: 0x06003F88 RID: 16264 RVA: 0x00036FF5 File Offset: 0x000351F5
		public GrayScaleValue()
		{
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x00189638 File Offset: 0x00187838
		public override T Lerp<T>(T v2, float rate)
		{
			ChokhmahBossBase.GrayScaleValue grayScaleValue = v2 as ChokhmahBossBase.GrayScaleValue;
			ChokhmahBossBase.GrayScaleValue grayScaleValue2 = new ChokhmahBossBase.GrayScaleValue();
			grayScaleValue2.fade = Mathf.Lerp(this.fade, grayScaleValue.fade, rate);
			return this.fade as T;
		}

		// Token: 0x04003A3B RID: 14907
		public float fade;
	}

	// Token: 0x0200080C RID: 2060
	public enum ChokhmahPaneltyType
	{
		// Token: 0x04003A3D RID: 14909
		DIE,
		// Token: 0x04003A3E RID: 14910
		PANIC
	}

	// Token: 0x0200080D RID: 2061
	public class ChokhmahPanelty
	{
		// Token: 0x06003F8A RID: 16266 RVA: 0x00004354 File Offset: 0x00002554
		public ChokhmahPanelty()
		{
		}

		// Token: 0x04003A3F RID: 14911
		public AgentModel target;

		// Token: 0x04003A40 RID: 14912
		public ChokhmahBossBase.ChokhmahPaneltyType paneltyType;
	}
}
