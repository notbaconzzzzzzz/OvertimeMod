using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class SilentOrchestra : CreatureBase
{
	// Token: 0x06002976 RID: 10614 RVA: 0x00121868 File Offset: 0x0011FA68
	public SilentOrchestra()
	{
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x00028F08 File Offset: 0x00027108
	public static float GetDamage(SilentOrchestra.Movement currentMovement, SilentOrchestra.Movement targetPosition)
	{
		if (currentMovement == SilentOrchestra.Movement.FINALE)
		{
			return 0f;
		}
		if (currentMovement == SilentOrchestra.Movement.NONE || targetPosition == SilentOrchestra.Movement.NONE)
		{
			return 0f;
		}
		return SilentOrchestra.dmgAry[currentMovement - SilentOrchestra.Movement.MOVEMENT1, targetPosition - SilentOrchestra.Movement.MOVEMENT1];
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06002978 RID: 10616 RVA: 0x00028F39 File Offset: 0x00027139
	public SilentOrchestra.Movement CurrentMovement
	{
		get
		{
			return this._currentMovement;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06002979 RID: 10617 RVA: 0x00028F41 File Offset: 0x00027141
	public SilentOrchestraAnim AnimScript
	{
		get
		{
			return this._animScript;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x0600297A RID: 10618 RVA: 0x001218C0 File Offset: 0x0011FAC0
	private float EffectRange
	{
		get
		{
			if (this.CurrentMovement == SilentOrchestra.Movement.NONE)
			{
				return 0f;
			}
			if (this.CurrentMovement == SilentOrchestra.Movement.FINALE)
			{
				return SilentOrchestra.effectRadius[SilentOrchestra.effectRadius.Length - 1];
			}
			float a = SilentOrchestra.effectRadius[this.CurrentMovement - SilentOrchestra.Movement.MOVEMENT1];
			float b = SilentOrchestra.effectRadius[(int)this.CurrentMovement];
			float rate = this.AnimScript.GetCurrentRange().Rate;
			return Mathf.Lerp(a, b, rate);
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x0600297B RID: 10619 RVA: 0x00028F49 File Offset: 0x00027149
	private float CurrentMovementTime
	{
		get
		{
			return SilentOrchestra.movementTime[(int)this.CurrentMovement];
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x0600297C RID: 10620 RVA: 0x00121930 File Offset: 0x0011FB30
	private DamageInfo CurrentDamageInfo
	{
		get
		{
			int num = (int)this.CurrentMovement;
			if (num == 0)
			{
				num = 1;
			}
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage(num.ToString());
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x0600297D RID: 10621 RVA: 0x000249B2 File Offset: 0x00022BB2
	private float randDeadSceneDealy
	{
		get
		{
			return UnityEngine.Random.Range(2f, 3f);
		}
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x00028F57 File Offset: 0x00027157
	private void TerminateAudio()
	{
		SoundEffectPlayer.DestroyPlayer(ref this._loop);
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x00121970 File Offset: 0x0011FB70
	public override void ParamInit()
	{
		this._currentMovement = SilentOrchestra.Movement.NONE;
		this.MovementTimer.StopTimer();
		this.FinaleTimer.StopTimer();
		this.DamageTick.StopTimer();
		if (this.AnimScript != null)
		{
			this.AnimScript.Init();
		}
		this._appearDelayTimer.StopTimer();
		if (this._finaleTargets == null)
		{
			this._finaleTargets = new List<WorkerModel>();
		}
		else
		{
			this._finaleTargets.Clear();
		}
		this._appearDelayTimer.StopTimer();
		this._finaleTimmingCheck.StopTimer();
		this._escapeReady = false;
		this._suppressed = false;
		this._finaleStarted = false;
		this._finaleActivated = false;
		this._executeDeadCommand = false;
		this._curtainReady = false;
		this._audioChange = false;
		this._dayFlag = false;
		this._isEscaped = false;
		if (PlaySpeedSettingUI.instance != null)
		{
			PlaySpeedSettingUI.instance.SetTimeMultiplierEnable(true);
			PlaySpeedSettingUI.instance.RemoveSpaceEvent(new PlaySpeedSettingUI.SpaceEvent(this.SetSpaceState));
		}
		this.SetDefenseType();
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x00028F65 File Offset: 0x00027165
	public override void OnInit()
	{
		this.model.SetFactionForcely("10");
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x00028F77 File Offset: 0x00027177
	public override void OnViewInit(CreatureUnit unit)
	{
		this._animScript = (unit.animTarget as SilentOrchestraAnim);
		this._animScript.SetScript(this);
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x00028F96 File Offset: 0x00027196
	public override void OnStageStart()
	{
		this.ParamInit();
		this.RoomEscapeSpriteOff();
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x00028FA4 File Offset: 0x000271A4
	public override void OnStageEnd()
	{
		if (this._isEscaped && this._loop != null)
		{
			SoundEffectPlayer.DestroyPlayer(ref this._loop);
		}
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x00028FCE File Offset: 0x000271CE
	public override void OnStageRelease()
	{
		this.AnimScript.ui.SetActivate(false);
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x00121A80 File Offset: 0x0011FC80
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		Debug.Log("<color=#FF0000FF>" + oldState + "</color>");
		if (oldState <= CreatureFeelingState.GOOD || oldState >= CreatureFeelingState.BAD)
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06002986 RID: 10630 RVA: 0x00024663 File Offset: 0x00022863
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
	}

	// Token: 0x06002987 RID: 10631 RVA: 0x00028FE1 File Offset: 0x000271E1
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this._appearDelayTimer.started && this._appearDelayTimer.RunTimer())
		{
			this.OnCurtainStartOpen();
		}
		if (this.MovementTimer.RunTimer())
		{
		}
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x00121AC4 File Offset: 0x0011FCC4
	public override void UniqueEscape()
	{
		this.CheckCamera();
		this.model.CheckNearWorkerEncounting();
		List<WorkerModel> allWorkers = this.GetAllWorkers();
		if (this._finaleTimmingCheck.started)
		{
			if (this._finaleTimmingCheck.RunTimer())
			{
			}
			if (!this._finaleActivated)
			{
				if (this._finaleTimmingCheck.elapsed > 17f)
				{
					this._finaleActivated = true;
					this.StartFinaleEffect();
					this._finaleStarted = true;
				}
			}
			else if (this._finaleTimmingCheck.elapsed > 21.5f && this._finaleStarted)
			{
				this._finaleStarted = false;
				this.ActivateFinale();
			}
		}
		if (this.FinaleTimer.started && this.FinaleTimer.RunTimer())
		{
			this.OnMovementEnd();
		}
		if (!this.DamageTick.started)
		{
			if (this.CurrentMovement != SilentOrchestra.Movement.NONE)
			{
				this.DamageTick.StartTimer(5f);
			}
		}
		else if (this.DamageTick.RunTimer())
		{
			foreach (WorkerModel workerModel in allWorkers)
			{
				SilentOrchestra.Movement movement = SilentOrchestra.Movement.NONE;
				if (this.IsInArea(workerModel.GetMovableNode(), out movement) && movement != SilentOrchestra.Movement.NONE)
				{
					float damage = SilentOrchestra.GetDamage(this.CurrentMovement, movement);
					workerModel.TakeDamage(this.model, this.DamageCalc(damage));
					DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(workerModel, RwbpType.W, this.model);
				}
			}
			this.DamageTick.StartTimer(5f);
		}
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x00121C74 File Offset: 0x0011FE74
	public DamageInfo DamageCalc(float factor)
	{
		DamageInfo currentDamageInfo = this.CurrentDamageInfo;
		DamageInfo damageInfo = new DamageInfo(currentDamageInfo.type, (int)currentDamageInfo.min, (int)currentDamageInfo.max);
		damageInfo.min *= factor;
		damageInfo.max *= factor;
		return damageInfo;
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x00029019 File Offset: 0x00027219
	public override void OnSuppressed()
	{
		SoundEffectPlayer.DestroyPlayer(ref this._loop);
		this.TerminateAudio();
		this.ParamInit();
		this.RoomEscapeSpriteOff();
		this.AnimScript.OnEscapeCancel();
	}

	// Token: 0x0600298B RID: 10635 RVA: 0x0001E75A File Offset: 0x0001C95A
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x0600298C RID: 10636 RVA: 0x00029044 File Offset: 0x00027244
	public override void OnReturn()
	{
		BgmManager.instance.FadeIn();
		this.model.ResetQliphothCounter();
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x00121CC0 File Offset: 0x0011FEC0
	public override void Escape()
	{
		if (this._dayFlag)
		{
			return;
		}
		this._curtainReady = true;
		this._escapeReady = true;
		this.RoomEscapeSpriteOn();
		this._isEscaped = true;
		this.AnimScript.OnEscapeStart();
		this._appearDelayTimer.StartTimer(6f);
		if (PlaySpeedSettingUI.instance != null)
		{
			PlaySpeedSettingUI.instance.SetTimeMultiplierEnable(false);
			PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
		}
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x00121D34 File Offset: 0x0011FF34
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		Sprite renderSprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/State/" + this.model.metadataId);
		base.Unit.room.StateFilter.renderSprite = renderSprite;
		base.Unit.room.StateFilter.renderAnim.StopPlayback();
		base.Unit.room.EscapeFilter.hasSpecialAnimKey = true;
		base.Unit.room.EscapeFilter.specialAnimKey = "Display";
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x00121DC8 File Offset: 0x0011FFC8
	public void OnMovementEnd()
	{
		switch (this.CurrentMovement)
		{
		case SilentOrchestra.Movement.NONE:
			this._currentMovement = SilentOrchestra.Movement.MOVEMENT1;
			this.AnimScript.SetAppearAnim(1, this.CurrentMovementTime);
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.First);
			this.AnimScript.SetUIText(0);
			break;
		case SilentOrchestra.Movement.MOVEMENT1:
			this._currentMovement = SilentOrchestra.Movement.MOVEMENT2;
			this.AnimScript.SetAppearAnim(2, this.CurrentMovementTime);
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.Second);
			this.AnimScript.SetUIText(1);
			break;
		case SilentOrchestra.Movement.MOVEMENT2:
			this._currentMovement = SilentOrchestra.Movement.MOVEMENT3;
			this.AnimScript.SetAppearAnim(3, this.CurrentMovementTime);
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.Third);
			this.AnimScript.SetUIText(2);
			break;
		case SilentOrchestra.Movement.MOVEMENT3:
			this._currentMovement = SilentOrchestra.Movement.MOVEMENT4;
			this.FinaleTimer.StartTimer(this.CurrentMovementTime);
			this.AnimScript.StartEffect(3, this.CurrentMovementTime);
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.Fourth);
			this.AnimScript.SetUIText(3);
			this.OnFinale();
			break;
		case SilentOrchestra.Movement.MOVEMENT4:
			this._currentMovement = SilentOrchestra.Movement.FINALE;
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.Fin);
			this.AnimScript.SetUIText(4);
			break;
		case SilentOrchestra.Movement.FINALE:
			this._currentMovement = SilentOrchestra.Movement.NONE;
			break;
		}
		this.SetDefenseType();
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x0010AC88 File Offset: 0x00108E88
	public string GetSoundSrc(string key)
	{
		string empty = string.Empty;
		if (!this.model.metaInfo.soundTable.TryGetValue(key, out empty))
		{
			Debug.Log("Failed To load Sound src : " + key);
		}
		return empty;
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x00121F20 File Offset: 0x00120120
	public override SoundEffectPlayer MakeSound(string src)
	{
		string text = string.Empty;
		SoundEffectPlayer soundEffectPlayer = null;
		text = this.GetSoundSrc(src);
		if (!text.Equals(string.Empty))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(text, this.movable.GetCurrentViewPosition());
			soundEffectPlayer.src.rolloffMode = AudioRolloffMode.Linear;
		}
		return soundEffectPlayer;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x0002905C File Offset: 0x0002725C
	public void OnConductorReady()
	{
		this.AnimScript.SetAppearAnim(0, this.CurrentMovementTime);
		this.AnimScript.standEffect.SetActive(true);
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x00029081 File Offset: 0x00027281
	public void OnCurtainStartOpen()
	{
		if (this._escapeReady)
		{
			this._escapeReady = false;
			this.model.Escape();
			this.AnimScript.StartConductor();
			this.MovementTimer.StartTimer();
			this.Teleport();
		}
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x00121F74 File Offset: 0x00120174
	public void SetSpaceState(bool state)
	{
		if (state)
		{
			if (this._loop != null)
			{
				this._loop.src.UnPause();
			}
		}
		else if (this._loop != null)
		{
			this._loop.src.Pause();
		}
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x000290BC File Offset: 0x000272BC
	public void OnCurtainAppear()
	{
		if (this._curtainReady)
		{
			this._curtainReady = false;
			this.SetBackGroundLoop(SilentOrchestra.SoundKey.Clap);
			PlaySpeedSettingUI.instance.AddSpaceEvent(new PlaySpeedSettingUI.SpaceEvent(this.SetSpaceState));
			this.CheckCamera();
		}
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x000290F7 File Offset: 0x000272F7
	public void SetBackGroundLoop(string key)
	{
		this._loop = this.MakeSound(key);
		this._loop.transform.SetParent(Camera.main.transform);
		this._loop.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x00121FD0 File Offset: 0x001201D0
	private List<WorkerModel> GetAllWorkers()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		List<WorkerModel> workerList = WorkerManager.instance.GetWorkerList();
		foreach (WorkerModel workerModel in workerList)
		{
			if (workerModel.IsAttackTargetable())
			{
				list.Add(workerModel);
			}
		}
		return list;
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x00122044 File Offset: 0x00120244
	private void CheckCamera()
	{
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		Camera worldCamera = base.Unit.currentCreatureCanvas.worldCamera;
		this._cameraSensing.Set(currentViewPosition.x - this.EffectRange, currentViewPosition.x + this.EffectRange, currentViewPosition.y - this.EffectRange, currentViewPosition.y + this.EffectRange);
		try
		{
			if (this._cameraSensing.Check(worldCamera.transform.position))
			{
				if (!this._audioChange)
				{
					this._audioChange = true;
					BgmManager.instance.FadeOut();
					if (this._loop != null)
					{
						this._loop.src.volume = 1f;
					}
				}
			}
			else if (this._audioChange)
			{
				this._audioChange = false;
				BgmManager.instance.FadeIn();
				if (this._loop != null)
				{
					this._loop.src.volume = 0f;
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x000043CD File Offset: 0x000025CD
	private void ForcelySuppressed()
	{
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00122178 File Offset: 0x00120378
	private MapNode GetTeleportNode()
	{
		MapNode[] nodeList = this.model.sefira.sefiraPassage.GetNodeList();
		return nodeList[nodeList.Length / 2];
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x001221A4 File Offset: 0x001203A4
	private void Teleport()
	{
		MapNode teleportNode = this.GetTeleportNode();
		this.movable.SetCurrentNode(teleportNode);
		CameraMover.instance.CameraMoveEvent(teleportNode.GetPosition(), 8f);
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x001221DC File Offset: 0x001203DC
	private bool IsInArea(MovableObjectNode mov, out SilentOrchestra.Movement type)
	{
		SilentOrchestra.Movement movement = SilentOrchestra.Movement.NONE;
		if (this.CurrentMovement == SilentOrchestra.Movement.NONE)
		{
			type = movement;
			return false;
		}
		if (this.CurrentMovement == SilentOrchestra.Movement.FINALE)
		{
			type = SilentOrchestra.Movement.FINALE;
			return true;
		}
		float distanceDouble = this.movable.GetDistanceDouble(mov);
		int num = -1;
		int currentMovement = (int)this.CurrentMovement;
		for (int i = 0; i < SilentOrchestra.effectRadius.Length; i++)
		{
			if (i == currentMovement)
			{
				break;
			}
			float num2 = SilentOrchestra.effectRadius[i] * SilentOrchestra.effectRadius[i];
			if (distanceDouble <= num2)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			if (distanceDouble > this.EffectRange * this.EffectRange)
			{
				type = movement;
				return false;
			}
			type = this.CurrentMovement;
		}
		else
		{
			switch (num)
			{
			case 0:
				type = SilentOrchestra.Movement.MOVEMENT1;
				break;
			case 1:
				type = SilentOrchestra.Movement.MOVEMENT2;
				break;
			case 2:
				type = SilentOrchestra.Movement.MOVEMENT3;
				break;
			case 3:
				type = SilentOrchestra.Movement.MOVEMENT4;
				break;
			default:
				type = SilentOrchestra.Movement.FINALE;
				break;
			}
		}
		return true;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x00029135 File Offset: 0x00027335
	private bool IsInRange(float rad, MovableObjectNode target)
	{
		return rad * rad >= this.movable.GetDistanceDouble(target);
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x0002914B File Offset: 0x0002734B
	private void OnFinale()
	{
		if (!SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
		{
			EnergyModel.instance.SubEnergy(EnergyModel.instance.GetEnergy());
		}
		this._finaleTimmingCheck.StartTimer();
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x0002917C File Offset: 0x0002737C
	private void StartFinaleEffect()
	{
		this.AnimScript.InvokeFinale();
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x00029189 File Offset: 0x00027389
	private void ActivateFinale()
	{
		this.AnimScript.InvokeActivate();
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x001222E4 File Offset: 0x001204E4
	public void OnFinaleStartDead()
	{
		List<WorkerModel> allWorkers = this.GetAllWorkers();
		foreach (WorkerModel workerModel in allWorkers)
		{
			if (workerModel.IsPanic())
			{
				if (!workerModel.invincible)
				{
					workerModel.AddUnitBuf(new SilentOrchestraDeadBuf(this.randDeadSceneDealy));
				}
			}
		}
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x00029196 File Offset: 0x00027396
	public void OnFinaleEnd()
	{
		this.ForcelyReturn();
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x00122370 File Offset: 0x00120570
	private void ForcelyReturn()
	{
		this.ParamInit();
		this.RoomEscapeSpriteOff();
		this.AnimScript.Init();
		this.model.state = CreatureState.SUPPRESSED;
		this.model.ClearCommand();
		this.model.ClearWorkerEncounting();
		this.model.sefira.OnSuppressedCreature(this.model);
		this.model.FinishReturn();
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x0001A9D1 File Offset: 0x00018BD1
	public CreatureStaticData.ParameterData GetDesc(int index)
	{
		return this.model.metaInfo.creatureStaticData.GetParam(index);
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000140A1 File Offset: 0x000122A1
	public override bool HasUniqueFaction()
	{
		return true;
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x001223D8 File Offset: 0x001205D8
	private void SetDefenseType()
	{
		int num = (int)this.CurrentMovement;
		if (num == 0)
		{
			num = 1;
		}
		this.model.SetDefenseId(num.ToString());
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x0012240C File Offset: 0x0012060C
	// Note: this type is marked as 'beforefieldinit'.
	static SilentOrchestra()
	{
	}

	// Token: 0x040027B7 RID: 10167
	public static float[,] dmgAry = new float[,]
	{
		{
			1f,
			0f,
			0f,
			0f
		},
		{
			1f,
			0.33f,
			0f,
			0f
		},
		{
			1f,
			0.5f,
			0.25f,
			0f
		},
		{
			1f,
			0.77f,
			0.66f,
			0.55f
		}
	};

	// Token: 0x040027B8 RID: 10168
	public const string FactionCode = "10";

	// Token: 0x040027B9 RID: 10169
	public static float[] movementTime = new float[]
	{
		6f,
		21f,
		15f,
		12f,
		26f,
		34f
	};

	// Token: 0x040027BA RID: 10170
	public static float[] effectRadius = new float[]
	{
		1f,
		10f,
		15f,
		20f,
		35f
	};

	// Token: 0x040027BB RID: 10171
	private const float _appearDelayTime = 6f;

	// Token: 0x040027BC RID: 10172
	private const float _damageTick = 5f;

	// Token: 0x040027BD RID: 10173
	private const float _deadSceneDelayMin = 2f;

	// Token: 0x040027BE RID: 10174
	private const float _deadSceneDelayMax = 3f;

	// Token: 0x040027BF RID: 10175
	private SilentOrchestra.Movement _currentMovement;

	// Token: 0x040027C0 RID: 10176
	private SilentOrchestraAnim _animScript;

	// Token: 0x040027C1 RID: 10177
	public Timer MovementTimer = new Timer();

	// Token: 0x040027C2 RID: 10178
	public Timer FinaleTimer = new Timer();

	// Token: 0x040027C3 RID: 10179
	private List<WorkerModel> _finaleTargets;

	// Token: 0x040027C4 RID: 10180
	private SoundEffectPlayer _loop;

	// Token: 0x040027C5 RID: 10181
	private Timer _appearDelayTimer = new Timer();

	// Token: 0x040027C6 RID: 10182
	private Timer _finaleTimmingCheck = new Timer();

	// Token: 0x040027C7 RID: 10183
	private bool _escapeReady;

	// Token: 0x040027C8 RID: 10184
	private bool _suppressed;

	// Token: 0x040027C9 RID: 10185
	private bool _finaleStarted;

	// Token: 0x040027CA RID: 10186
	private bool _finaleActivated;

	// Token: 0x040027CB RID: 10187
	private bool _executeDeadCommand;

	// Token: 0x040027CC RID: 10188
	private bool _curtainReady;

	// Token: 0x040027CD RID: 10189
	private bool _audioChange;

	// Token: 0x040027CE RID: 10190
	private bool _dayFlag;

	// Token: 0x040027CF RID: 10191
	private bool _isEscaped;

	// Token: 0x040027D0 RID: 10192
	private CreatureBase.SensingModule _cameraSensing = new CreatureBase.SensingModule();

	// Token: 0x040027D1 RID: 10193
	public Timer DamageTick = new Timer();

	// Token: 0x0200047A RID: 1146
	public enum Movement
	{
		// Token: 0x040027D3 RID: 10195
		NONE,
		// Token: 0x040027D4 RID: 10196
		MOVEMENT1,
		// Token: 0x040027D5 RID: 10197
		MOVEMENT2,
		// Token: 0x040027D6 RID: 10198
		MOVEMENT3,
		// Token: 0x040027D7 RID: 10199
		MOVEMENT4,
		// Token: 0x040027D8 RID: 10200
		FINALE
	}

	// Token: 0x0200047B RID: 1147
	public class SoundKey
	{
		// Token: 0x060029A8 RID: 10664 RVA: 0x000043A0 File Offset: 0x000025A0
		public SoundKey()
		{
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x0012245C File Offset: 0x0012065C
		// Note: this type is marked as 'beforefieldinit'.
		static SoundKey()
		{
		}

		// Token: 0x040027D9 RID: 10201
		public static string Head = "Head";

		// Token: 0x040027DA RID: 10202
		public static string Clap = "zero";

		// Token: 0x040027DB RID: 10203
		public static string First = "first";

		// Token: 0x040027DC RID: 10204
		public static string Second = "second";

		// Token: 0x040027DD RID: 10205
		public static string Third = "third";

		// Token: 0x040027DE RID: 10206
		public static string Fourth = "fourth";

		// Token: 0x040027DF RID: 10207
		public static string Fin = "finale";
	}
}
