/*
private void Update() // Hp Bar Stacking
+private void UpdateBarStacking() // Hp Bar Stacking
+public void LiftHpBar(AgentUnit unit, List<AgentUnit> obst) // Hp Bar Stacking
+public float CurrentBarAdjust // Hp Bar Stacking
+private float _currentBarAdjust // Hp Bar Stacking
*/
using System;
using System.Collections;
using System.Collections.Generic;
using InGameUI;
using UnityEngine;
using UnityEngine.UI;
using WorkerSpine;
using WorkerSprite;

// Token: 0x02000B01 RID: 2817
public class AgentUnit : WorkerUnit, IOverlapOnclick, IMouseOnSelectListener, IMouseOnPointListener, IMouseOnDragListener, IMouseCommandTarget
{
	// Token: 0x060054AB RID: 21675 RVA: 0x00044823 File Offset: 0x00042A23
	public AgentUnit()
	{
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x00044832 File Offset: 0x00042A32
	private void Awake()
	{
		this.model = null;
		this.workerModel = null;
		this._animController = base.GetComponent<UnitAnimatorController>();
		this.changer = base.GetComponentInChildren<AgentSpriteChanger>();
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x001E5AB4 File Offset: 0x001E3CB4
	public void Start()
	{
		if (this.model == null)
		{
			return;
		}
		this.spriteSetter.SetWorkerType(this.model);
		this.effectAttached.Clear();
		this.ui.activateUI(this.model);
		this.ui.Initial(this.model);
		this.ui.initUI();
		Canvas component = this.showSpeech.GetComponent<Canvas>();
		component.worldCamera = Camera.main;
		this.model.SetUnit(this);
		this.showSpeech.Init(this.model);
		this.OnOverlayDisabled();
		this.animEventHandler.SetDamageEvent(new AnimatorEventHandler.EventDelegate(this.model.OnGiveDamageByWeapon));
		this.animEventHandler.SetAttackEndEvent(new AnimatorEventHandler.EventDelegate(base.EndAttack));
		this.animEventHandler.SetSuicideEvent(new AnimatorEventHandler.EventDelegate(this.OnSuicide));
		base.gameObject.name = string.Concat(new object[]
		{
			"AgentUnit(",
			this.model.instanceId,
			") : ",
			this.model.GetUnitName()
		});
		this.agentUI.Init(this.model);
		this.spriteSetter.InitBasicSet();
		this.OnChangeWeapon();
		this.OnChangeArmor();
		this.UpdateGiftModel();
		this.DisappearNote();
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
		{
			int layer = LayerMask.NameToLayer("Front1");
			IEnumerator enumerator = this.showSpeech.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.layer = layer;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		this.continueUI.SetAgent(this.model);
	}

	// Token: 0x060054AE RID: 21678 RVA: 0x0004485A File Offset: 0x00042A5A
	public void OnSuicide()
	{
		DamageParticleEffect.Invoker(this.model, RwbpType.R, 4);
		SoundEffectPlayer.PlayOnce("Agent/officerDie", this.model.GetCurrentViewPosition());
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x000040A1 File Offset: 0x000022A1
	public void FlipPuppetNode(bool isLeft)
	{
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x000040A1 File Offset: 0x000022A1
	public void UpdateHair()
	{
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x001E5CA8 File Offset: 0x001E3EA8
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.model == null)
		{
			return;
		}
		if (this.speech_force)
		{
			if (this.model.CannotControll() || this.model.IsDead())
			{
				this.speech_force = false;
			}
			else
			{
				this.speech_force_elapsed += Time.deltaTime;
				if (this.speech_force_elapsed > 2f)
				{
					this.speech_force = false;
					this.SpeechDefaultLyric();
				}
			}
		}
		else if (!this.model.CannotControll() && !this.model.IsDead() && this.model.IsIdle())
		{
			this.speech_ealpsed += Time.deltaTime;
			if (this.speech_enable)
			{
				if (this.speech_ealpsed >= this.speech_frequency)
				{
					this.speech_ealpsed = 0f;
					if (UnityEngine.Random.Range(0, 100) < this.speech_percentage)
					{
						this.speech_enable = false;
						this.SpeechDefaultLyric();
					}
				}
			}
			else if (this.speech_ealpsed >= this.speech_cooltime + 5f)
			{
				this.speech_ealpsed = 0f;
				this.speech_enable = true;
			}
		}
		this.ui.setUIValue(this.model);
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x00044885 File Offset: 0x00042A85
	public void SpeechDefaultLyricForce()
	{
		this.speech_force = true;
		this.speech_force_elapsed = 0f;
	}

	// Token: 0x060054B3 RID: 21683 RVA: 0x001E5DFC File Offset: 0x001E3FFC
	public void SpeechDefaultLyric()
	{
		string text = string.Empty;
		LyricTypeNew type;
		if (this.model.IsPanic())
		{
			type = LyricTypeNew.PANIC;
		}
		else
		{
			type = LyricTypeNew.NORMAL;
		}
		text = AgentLyrics.instance.GetLyricText(type, this.model.level, this.model.bestRwbp, this.model.uniqueScriptIndex);
		this.showSpeech.ShowAgentLyric(type, text, this.model.level);
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x001E5E70 File Offset: 0x001E4070
	public void SpeechHorrorLyric(int level)
	{
		LyricTypeNew type = LyricTypeNew.HORROR;
		this.showSpeech.ShowAgentLyric(type, AgentLyrics.instance.GetLyricText(type, level, this.model.bestRwbp, this.model.uniqueScriptIndex), level);
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x001E5EB0 File Offset: 0x001E40B0
	public void SpeechOtherdeadLyric(int level, string param)
	{
		LyricTypeNew type = LyricTypeNew.OTHERDEAD;
		string text = string.Format(AgentLyrics.instance.GetLyricText(type, level, this.model.bestRwbp, this.model.uniqueScriptIndex), param);
		this.showSpeech.ShowAgentLyric(type, text, level);
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x001E5EF8 File Offset: 0x001E40F8
	public void SpeechOtherpanicLyric(int level, string param)
	{
		LyricTypeNew type = LyricTypeNew.OTHERPANIC;
		string text = string.Format(AgentLyrics.instance.GetLyricText(type, level, this.model.bestRwbp, this.model.uniqueScriptIndex), param);
		this.showSpeech.ShowAgentLyric(type, text, level);
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x001E5F40 File Offset: 0x001E4140
	public void SpeechSet(bool isClick)
	{
		if (GameManager.currentGameManager.state == GameState.PAUSE)
		{
			return;
		}
		Sefira sefira = SefiraManager.instance.GetSefira(this.model.currentSefira);
		if (this.model.CurrentPanicAction != null)
		{
			this.showSpeech.ShowSpeech_old(AgentLyrics.instance.GetLyricByType(LyricType.MENTALBAD));
		}
		else if (this.model.GetState() != AgentAIState.CANNOT_CONTROLL)
		{
			AgentLyrics.LyricList_old lyricList_old;
			if (isClick)
			{
				lyricList_old = AgentLyrics.instance.GetLyricByType(LyricType.CHAT);
			}
			else
			{
				lyricList_old = AgentLyrics.instance.GetIdleLyricByDay(PlayerModel.instance.GetDay());
			}
			if (sefira.IsAnyCreatureEscaped())
			{
				CreatureModel creatureModel = null;
				if (sefira.IsAgentInEsacpedCreaturePassage(this.model, out creatureModel))
				{
					bool flag = false;
					if (creatureModel != null)
					{
						AgentLyrics.CreatureReactionList creatureReaction = AgentLyrics.instance.GetCreatureReaction(creatureModel.metadataId);
						if (creatureReaction != null && creatureReaction.action != null && creatureReaction.action.GetActionDesc("escaped") != null)
						{
							this.showSpeech.ShowCreatureActionLyric(creatureReaction.action, "escaped");
							flag = true;
						}
					}
					if (!flag)
					{
						lyricList_old = AgentLyrics.instance.GetLyricByType(LyricType.ESCAPE);
					}
				}
			}
			else if (AngelaConversationUI.instance.IsAgentDead())
			{
				if (sefira.IsAgentInDeadAgentPassage(this.model))
				{
					lyricList_old = AgentLyrics.instance.GetLyricByType(LyricType.SAD);
				}
			}
			else if (this.model.GetState() == AgentAIState.SUPPRESS_CREATURE)
			{
				lyricList_old = AgentLyrics.instance.GetLyricByType(LyricType.ESCAPE);
			}
			else if (this.model.GetState() == AgentAIState.SUPPRESS_WORKER)
			{
				bool flag2 = this.model.targetWorker.unconAction is Uncontrollable_RedShoes;
				Debug.Log(this.model.targetWorker.name + " " + flag2);
				if (this.model.targetWorker != null)
				{
					if (this.model.targetWorker.unconAction is Uncontrollable_RedShoes)
					{
						lyricList_old = AgentLyrics.instance.GetLyricByType(LyricType.ESCAPE);
					}
					else
					{
						lyricList_old = null;
					}
				}
			}
			if (lyricList_old != null)
			{
				this.showSpeech.ShowSpeech_old(lyricList_old);
			}
		}
	}

	// Token: 0x060054B8 RID: 21688 RVA: 0x001E616C File Offset: 0x001E436C
	private void Update()
	{ // <Mod>
		this._animChangeTimer.RunTimer();
		base.UpdateDirection();
		base.UpdateViewPosition();
		this.UpdateAnimationQuality();
		if (!this._animChangeTimer.started)
		{
			base.UpdateAnimatorChange();
		}
		if (this.model.IsDead())
		{
			if (!this.dead)
			{
				this.ui.initUI();
				this.ui.kitCreatureIcon.gameObject.SetActive(false);
				this.dead = true;
			}
			return;
		}
		if (this.dead && this.uiActivated)
		{
			this.ui.activateUI(this.model);
		}
		if (SefiraManager.instance.GetSefira(SefiraEnum.YESOD).GetCurrentAbilityLevel() > 0)
		{
			if (this.uiActivated)
			{
				this.ui.activateUI(this.model);
			}
		}
		else
		{
			this.ui.initUI();
		}
		this.SetSelectIconColor();
		UpdateBarStacking();
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x001E6260 File Offset: 0x001E4460
	protected override void UpdateAnimationQuality()
	{
		base.UpdateAnimationQuality();
		if (GameManager.currentGameManager != null && GameManager.currentGameManager.state == GameState.STOP)
		{
			this.spineRenderer.optimizeLevel = 5;
			return;
		}
		if (this._inCamera)
		{
			if (this.model != null)
			{
				float currentScale = this.workerModel.GetMovableNode().currentScale;
			}
			float num = Camera.main.orthographicSize / this.workerModel.GetMovableNode().currentScale;
			if (num > 80f)
			{
				this.spineRenderer.optimizeLevel = 4;
			}
			else if (num > 50f)
			{
				this.spineRenderer.optimizeLevel = 3;
			}
			else if (num > 30f)
			{
				this.spineRenderer.optimizeLevel = 2;
			}
			else if (num > 25f)
			{
				this.spineRenderer.optimizeLevel = 1;
			}
			else
			{
				this.spineRenderer.optimizeLevel = 0;
			}
		}
		else
		{
			this.spineRenderer.optimizeLevel = 5;
		}
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x000040A1 File Offset: 0x000022A1
	private void SetSlider()
	{
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x000040A1 File Offset: 0x000022A1
	public void SetAgentAnimatorModel()
	{
	}

	// Token: 0x060054BC RID: 21692 RVA: 0x00044899 File Offset: 0x00042A99
	public void OnClick()
	{
		if (this.model.IsDead())
		{
			return;
		}
		this.model.OnClick();
		this.clicked = true;
		this.OpenStatusWindow();
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x000448C4 File Offset: 0x00042AC4
	public void OnEnter()
	{
		if (this.model.IsDead())
		{
			return;
		}
		this.OnOverlayEnabled();
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x000448DD File Offset: 0x00042ADD
	public void OnExit()
	{
		if (this.model.IsDead())
		{
			return;
		}
		this.OnOverlayDisabled();
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x000448F6 File Offset: 0x00042AF6
	public bool HasPointListener()
	{
		return !this.model.IsDead();
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x0004490B File Offset: 0x00042B0B
	public void OnPointEnter()
	{
		if (this.model.IsDead())
		{
			return;
		}
		if (this._selected)
		{
			return;
		}
		this.OnEnter();
		this.agentUI.SetOverlayState(true);
		AgentInfoWindow.CreateWindow(this.model, false);
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x001E6378 File Offset: 0x001E4578
	public void OnPointExit()
	{
		Debug.Log("OnPointerExit");
		if (AgentInfoWindow.currentWindow.IsEnabled && AgentInfoWindow.currentWindow.CurrentAgent == this.model)
		{
			AgentInfoWindow.currentWindow.OnClearOverlay();
		}
		if (this.model.IsDead())
		{
			return;
		}
		if (this._selected)
		{
			return;
		}
		this.OnExit();
		this.agentUI.SetOverlayState(false);
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x00044949 File Offset: 0x00042B49
	public bool IsDragSelectable()
	{
		return !this.model.IsDead() && !this.model.IsCrazy();
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x0004496E File Offset: 0x00042B6E
	public void OnEnterDragArea()
	{
		if (this.model.IsDead())
		{
			return;
		}
		if (this._selected)
		{
			return;
		}
		this.OnEnter();
		this.agentUI.SetOverlayState(true);
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x0004499F File Offset: 0x00042B9F
	public void OnExitDragArea()
	{
		if (this.model.IsDead())
		{
			return;
		}
		if (this._selected)
		{
			return;
		}
		this.OnExit();
		this.agentUI.SetOverlayState(false);
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x000448F6 File Offset: 0x00042AF6
	public bool IsSelectable()
	{
		return !this.model.IsDead();
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x001E63EC File Offset: 0x001E45EC
	public void OnSelect()
	{
		this._selected = true;
		this.model.OnClick();
		this.OnEnter();
		this.agentUI.SetSelectState(true);
		if (AgentInfoWindow.currentWindow.CurrentAgent == this.model)
		{
			AgentInfoWindow.currentWindow.PinCurrentAgent();
		}
		this.SpeechDefaultLyric();
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x000449D0 File Offset: 0x00042BD0
	public void OnUnselect()
	{
		this._selected = false;
		this.OnExit();
		this.agentUI.SetSelectState(false);
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x000449EB File Offset: 0x00042BEB
	public IMouseCommandTargetModel GetCommandTargetModel()
	{
		return this.model;
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x000449F3 File Offset: 0x00042BF3
	public void OpenStatusWindow()
	{
		this.model.OnClick();
		AgentInfoWindow.CreateWindow(this.model, false);
		this.SpeechDefaultLyric();
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x00034BFA File Offset: 0x00032DFA
	public override void RemoveShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(false);
		}
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x00044A13 File Offset: 0x00042C13
	public void RevealShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(true);
		}
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x001E6444 File Offset: 0x001E4644
	public void PrepareWeapon()
	{
		if (this.model.Equipment.weapon != null && this.model.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
		{
			this._animChangeReady = true;
			this._animChangeTimer.StartTimer(0.5f);
		}
		this._animController.SetBattleReady(true);
		if (!this.model.IsPanic())
		{
			base.SetWorkerFaceType(WorkerFaceType.BATTLE);
		}
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x001E64C4 File Offset: 0x001E46C4
	public void CancelWeapon()
	{
		if (this.model.Equipment.weapon != null && this.model.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
		{
			this._animChangeReady = false;
			this._animChangeTimer.StartTimer(0.5f);
		}
		this._animController.SetBattleReady(false);
		this._animController.SetBattle(false);
		if (!this.model.IsPanic())
		{
			base.SetWorkerFaceType(WorkerFaceType.DEFAULT);
		}
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001E6550 File Offset: 0x001E4750
	public void OnChangeWeapon()
	{
		if (this.model == null)
		{
			return;
		}
		if (this.model.Equipment.weapon == null)
		{
			return;
		}
		if (this.model.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.NONE)
		{
			return;
		}
		this.weaponSetter.SetWeapon(this.model.Equipment.weapon);
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001E65BC File Offset: 0x001E47BC
	public void OnChangeArmor()
	{ // <Patch>
		if (this.model == null)
		{
			return;
		}
		if (this.model.Equipment.armor == null)
		{
			return;
		}
		this.spriteSetter.ArmorEquip_Mod(new LobotomyBaseMod.LcId(EquipmentTypeInfo.GetLcId(this.model.Equipment.armor.metaInfo).packageId, this.model.Equipment.armor.metaInfo.armorId));
		// this.spriteSetter.ArmorEquip(this.model.Equipment.armor.metaInfo.armorId);
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x001E6610 File Offset: 0x001E4810
	public void OnChangeKitCreature()
	{
		if (this.model == null)
		{
			return;
		}
		if (this.model.Equipment.kitCreature != null)
		{
			this.ui.kitCreatureIcon.enabled = true;
			this.ui.kitCreatureIcon.sprite = Resources.Load<Sprite>(this.model.Equipment.kitCreature.metaInfo.kitIconSrc);
		}
		else
		{
			this.ui.kitCreatureIcon.enabled = false;
		}
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x001E6694 File Offset: 0x001E4894
	public void UIRecoilInput(int level, int target)
	{
		RecoilEffectUI recoilEffectUI = null;
		Vector3 vector = default(Vector3);
		List<RecoilArrow> list = RecoilEffect.MakeRecoilArrow(level * recoilEffectUI.recoilCount);
		Queue<Vector3> queue = new Queue<Vector3>();
		foreach (RecoilArrow arrow in list)
		{
			queue.Enqueue(RecoilEffect.GetVector(arrow, vector, recoilEffectUI.scale));
		}
		queue.Enqueue(vector);
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.UIRecoil(queue, recoilEffectUI));
		}
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x001E6744 File Offset: 0x001E4944
	public void CharRecoilInput(int level)
	{
		RecoilEffect recoilEffect = null;
		recoilEffect = new RecoilEffect();
		recoilEffect.scale = 0.05f;
		List<RecoilArrow> list = RecoilEffect.MakeRecoilArrow(level * recoilEffect.recoilCount);
		Queue<Vector3> queue = new Queue<Vector3>();
		foreach (RecoilArrow arrow in list)
		{
			queue.Enqueue(RecoilEffect.GetVector(arrow, new Vector3(0f, 0f, 0f), recoilEffect.scale));
		}
		queue.Enqueue(new Vector3(0f, 0f, 0f));
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.CharRecoil(queue, recoilEffect));
		}
	}

	// Token: 0x060054D3 RID: 21715 RVA: 0x001E681C File Offset: 0x001E4A1C
	private IEnumerator UIRecoil(Queue<Vector3> queue, RecoilEffectUI recoil)
	{
		int val = queue.Count;
		float step = recoil.maxTime / (float)val;
		while (queue.Count > 1)
		{
			yield return new WaitForSeconds(step);
			recoil.rect.localPosition = queue.Dequeue();
		}
		recoil.rect.localPosition = queue.Dequeue();
		yield break;
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x001E6840 File Offset: 0x001E4A40
	private IEnumerator CharRecoil(Queue<Vector3> queue, RecoilEffect recoil)
	{
		int val = queue.Count;
		float step = recoil.maxTime / (float)val;
		while (queue.Count > 1)
		{
			yield return new WaitForSeconds(step);
			this.recoilPosition = queue.Dequeue();
		}
		this.recoilPosition = queue.Dequeue();
		yield break;
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x001E686C File Offset: 0x001E4A6C
	private IEnumerator MannualMoving(Vector3 pos, bool blockMoving)
	{
		Transform target = base.gameObject.transform;
		Vector3 initial = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, 0f);
		int cnt = 3;
		this.blockMoving = blockMoving;
		while (cnt > 0)
		{
			yield return new WaitForSeconds(0.1f);
			target.position = new Vector3(initial.x + reference.x / 3f * (float)(4 - cnt), initial.y, initial.z);
			cnt--;
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x001E6898 File Offset: 0x001E4A98
	public bool MannualMovingCall(Vector3 pos)
	{
		if (!this.isMovingStarted)
		{
			this.isMovingStarted = true;
			this.isMovingByMannually = false;
			base.StartCoroutine(this.MannualMoving(pos, true));
			return false;
		}
		if (this.isMovingByMannually)
		{
			this.isMovingByMannually = false;
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x001E68EC File Offset: 0x001E4AEC
	private IEnumerator MannualMoving(Vector3 pos, bool blockMoving, float unitWaitTime)
	{
		Transform target = base.gameObject.transform;
		Vector3 initial = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, 0f);
		int cnt = 3;
		this.blockMoving = blockMoving;
		while (cnt > 0)
		{
			yield return new WaitForSeconds(unitWaitTime);
			target.position = new Vector3(initial.x + reference.x / 3f * (float)(4 - cnt), initial.y + reference.y / 3f * (float)(4 - cnt), initial.z);
			cnt--;
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x001E691C File Offset: 0x001E4B1C
	public bool MannualMovingCall(Vector3 pos, float unitWaitTime)
	{
		if (!this.isMovingStarted)
		{
			this.isMovingStarted = true;
			this.isMovingByMannually = false;
			base.StartCoroutine(this.MannualMoving(pos, true, unitWaitTime));
			return false;
		}
		if (this.isMovingByMannually)
		{
			this.isMovingByMannually = false;
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x060054D9 RID: 21721 RVA: 0x001E6970 File Offset: 0x001E4B70
	private IEnumerator MannualMovingWithTime(Vector3 pos, bool blockMoving, float time)
	{
		Transform target = base.gameObject.transform;
		Vector3 initial = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, 0f);
		float elapsedTime = 0f;
		this.blockMoving = blockMoving;
		while (elapsedTime < time)
		{
			yield return new WaitForFixedUpdate();
			target.localPosition = new Vector3(initial.x + reference.x * (elapsedTime / time), initial.y + reference.y * (elapsedTime / time), initial.z);
			elapsedTime += Time.deltaTime;
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x001E69A0 File Offset: 0x001E4BA0
	public bool MannualMovingCallWithTime(Vector3 pos, float time)
	{
		if (!this.isMovingStarted)
		{
			this.isMovingStarted = true;
			this.isMovingByMannually = false;
			base.StartCoroutine(this.MannualMovingWithTime(pos, true, time));
			return false;
		}
		if (this.isMovingByMannually)
		{
			this.isMovingByMannually = false;
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x060054DB RID: 21723 RVA: 0x001E69F4 File Offset: 0x001E4BF4
	public SoundEffectPlayer PlaySound(string src, string key, bool isLoop)
	{
		SoundEffectPlayer result;
		if (isLoop)
		{
			result = SoundEffectPlayer.Play(src, base.gameObject.transform);
		}
		else
		{
			result = SoundEffectPlayer.PlayOnce(src, base.gameObject.transform.position);
		}
		return result;
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x00044A32 File Offset: 0x00042C32
	public void OnLateInit()
	{
		if (!this.lateInit)
		{
			this.lateInit = true;
			Debug.Log("this2?");
		}
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x001E6A40 File Offset: 0x001E4C40
	public void OnDie()
	{
		this._animChangeReady = false;
		if (!this.model.specialDeadScene)
		{
			if (this._animChanged)
			{
				base.UpdateAnimatorChange();
			}
		}
		else
		{
			this._animChangeTimer.StopTimer();
			this._animChanged = false;
		}
		this.spriteSetter.SetWeaponTransparent();
		if (!this.model.IsPanic() && this.model.DeadType != DeadType.EXECUTION)
		{
			this.spriteSetter.SetWorkerFaceType(WorkerFaceType.DEAD);
		}
		this._animController.OnDie();
		this.ui.Name.gameObject.SetActive(false);
		if (this.ui.title != null)
		{
			this.ui.title.gameObject.SetActive(false);
		}
		this.RemoveShadow();
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x00044A50 File Offset: 0x00042C50
	public void OnResurrect()
	{
		this.ui.Name.gameObject.SetActive(true);
		this.RevealShadow();
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x00044A6E File Offset: 0x00042C6E
	public void SelectIconForcelyEnable()
	{
		this.OnOverlayEnabled();
		this.selectIconState = true;
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x00044A7D File Offset: 0x00042C7D
	public void ManagingCreature()
	{
		this.managing = true;
		this.SelectIconForcelyEnable();
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x00044A8C File Offset: 0x00042C8C
	public void StartWork()
	{
		this._animController.SetWork(true);
		this._animController.StartWork();
		this.AppearNote();
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x00044AAB File Offset: 0x00042CAB
	public void EndWork()
	{
		this._animController.SetWork(false);
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x00044AB9 File Offset: 0x00042CB9
	public void SelectIconDisable()
	{
		this.selectIconState = false;
		this.managing = false;
		this.OnOverlayDisabled();
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x00044ACF File Offset: 0x00042CCF
	public void OnOverlayEnabled()
	{
		this.selectedIcon.gameObject.SetActive(true);
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x001E6B18 File Offset: 0x001E4D18
	public void SetSelectIconColor()
	{
		this.selectedIcon.color = this.colors.Normal;
		if (this._selected)
		{
			this.selectedIcon.color = this.colors.Selected;
		}
		if (this.model.currentSkill != null || this.model.GetState() == AgentAIState.MANAGE || this.managing)
		{
			this.selectedIcon.color = this.colors.Work;
		}
		if (this.model.IsSuppressing())
		{
			this.selectedIcon.color = this.colors.Attack;
		}
	}

	// Token: 0x060054E6 RID: 21734 RVA: 0x00044AE2 File Offset: 0x00042CE2
	public void OnOverlayDisabled()
	{
		if (this.selectIconState)
		{
			return;
		}
		this.selectedIcon.gameObject.SetActive(false);
	}

	// Token: 0x060054E7 RID: 21735 RVA: 0x001E6BC4 File Offset: 0x001E4DC4
	public GameObject MakeCreatureEffect(CreatureModel model)
	{
		Transform hairTransform = this.GetHairTransform();
		return this.MakeEffectAttach("Effect/Creature/AgentAttached/" + model.metadataId + "_Effect", hairTransform, true);
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x001E6BFC File Offset: 0x001E4DFC
	public GameObject MakeCreatureEffect(CreatureModel model, bool addlist)
	{
		Transform hairTransform = this.GetHairTransform();
		return this.MakeEffectAttach("Effect/Creature/AgentAttached/" + model.metadataId + "_Effect", hairTransform, addlist);
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x001E6C34 File Offset: 0x001E4E34
	public GameObject MakeCreatureEffect(long modelID)
	{
		Transform hairTransform = this.GetHairTransform();
		string name = "Effect/Creature/AgentAttached/" + modelID + "_Effect";
		GameObject gameObject = Prefab.LoadPrefab(name);
		if (gameObject == null)
		{
			Debug.Log(this.model);
			return null;
		}
		GameObject gameObject2 = gameObject;
		gameObject2.transform.SetParent(hairTransform);
		gameObject2.transform.position = hairTransform.transform.position;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		return gameObject2;
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x001E6CC4 File Offset: 0x001E4EC4
	public GameObject MakeEffectAttach(string src, Transform pos, bool addedList)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		if (gameObject == null)
		{
			Debug.Log(this.model);
			return null;
		}
		GameObject gameObject2 = gameObject;
		gameObject2.transform.SetParent(pos);
		gameObject2.transform.position = pos.transform.position;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		if (addedList)
		{
			this.effectAttached.Add(gameObject2);
		}
		return gameObject;
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x001E6D4C File Offset: 0x001E4F4C
	public GameObject MakeEffectAttach(string src, Transform pos)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.SetParent(pos);
		gameObject.transform.position = pos.transform.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x00176E10 File Offset: 0x00175010
	public void ClearEffect()
	{
		foreach (GameObject obj in this.effectAttached)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.effectAttached.Clear();
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x00044B01 File Offset: 0x00042D01
	public void BlockMove()
	{
		this.blockMoving = true;
	}

	// Token: 0x060054EE RID: 21742 RVA: 0x00034C6F File Offset: 0x00032E6F
	public void ReleaseMove()
	{
		this.blockMoving = false;
	}

	// Token: 0x060054EF RID: 21743 RVA: 0x00034CBA File Offset: 0x00032EBA
	public override void ShutUp()
	{
		this.showSpeech.DisableText();
	}

	// Token: 0x060054F0 RID: 21744 RVA: 0x00044B0A File Offset: 0x00042D0A
	public void SetGiftModel(EGOgiftModel model, bool state)
	{
		if (state)
		{
			this.spriteSetter.AddGiftModel(model);
		}
		else
		{
			this.spriteSetter.RemoveGiftModel(model);
		}
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x001E6DA8 File Offset: 0x001E4FA8
	public void UpdateGiftModel()
	{
		foreach (EGOgiftModel gift in this.model.Equipment.gifts.replacedGifts)
		{
			bool giftDisplayState = this.model.GetGiftDisplayState(gift);
			this.SetGiftModel(gift, giftDisplayState);
		}
		foreach (EGOgiftModel gift2 in this.model.Equipment.gifts.addedGifts)
		{
			bool giftDisplayState2 = this.model.GetGiftDisplayState(gift2);
			this.SetGiftModel(gift2, giftDisplayState2);
		}
		this.spriteSetter.CheckGiftModel(this.model.GetAllGifts());
	}

	// Token: 0x060054F2 RID: 21746 RVA: 0x00044B2F File Offset: 0x00042D2F
	public void AppearNote()
	{
		this.spriteSetter.NoteRenderer.gameObject.SetActive(true);
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x00044B47 File Offset: 0x00042D47
	public void DisappearNote()
	{
		this.spriteSetter.NoteRenderer.gameObject.SetActive(false);
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x001E6EA4 File Offset: 0x001E50A4
	public void SetWorkNote(int id)
	{
		Sprite workNoteSprite = WorkerSpriteManager.instance.GetWorkNoteSprite(id);
		this.spriteSetter.SetWorkNoteSprite(workNoteSprite);
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x001E6ECC File Offset: 0x001E50CC
	public void StageStartCheck()
	{
		this.ui.activateUI(this.model);
		this.ui.Initial(this.model);
		this.ui.initUI();
		this.agentUI.Init(this.model);
		this.continueUI.SetAgent(this.model);
	}

	//> <Mod> Hp Bar Stacking
	private void UpdateBarStacking()
	{
		if (!SpecialModeConfig.instance.GetValue<bool>("HpBarStackingAgent")) return;
		if (GameManager.currentGameManager.state == GameState.STOP) return;
		if (model.GetMovableNode().currentPassage == null) return;
		float newBarAdjust = CurrentBarAdjust - 0.5f;
		if (newBarAdjust < 0f)
		{
			newBarAdjust = 0f;
		}
		Vector3 position = transform.position;
		List<AgentUnit> obst = new List<AgentUnit>();
		foreach (MovableObjectNode node in model.GetMovableNode().currentPassage.GetEnteredTargets())
		{
			if (!(node.GetUnit() is AgentModel)) continue;
			AgentModel agent = node.GetUnit() as AgentModel;
			if (model.instanceId == agent.instanceId) continue;
			AgentUnit unit = agent.GetUnit();
			int ind = 0;
			foreach (AgentUnit unit2 in obst)
			{
				if (CurrentBarAdjust <= unit2.CurrentBarAdjust) break;
				ind++;
			}
			obst.Insert(ind, unit);
			unit.ShutUp();
		}
		foreach (AgentUnit unit in obst)
		{
			Vector3 position2 = unit.transform.position;
			float offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
			if (offset > 3f) continue;
			float dest;
			if (offset <= 2f)
			{
				if (newBarAdjust - unit.CurrentBarAdjust > 0.99f || newBarAdjust - unit.CurrentBarAdjust < -0.99f) continue;
				dest = unit.CurrentBarAdjust + 1f;
			}
			else
			{
				if (newBarAdjust - unit.CurrentBarAdjust > 2.99f - offset || newBarAdjust - unit.CurrentBarAdjust < -0.99f) continue;
				dest = unit.CurrentBarAdjust + 3f - offset;
			}
			if (unit.CurrentBarAdjust > CurrentBarAdjust)
			{
				unit.LiftHpBar(this, obst);
				continue;
			}
			if (dest > CurrentBarAdjust + 1f) continue;
			newBarAdjust = dest;
		}
		newBarAdjust = Mathf.Clamp(newBarAdjust, 0f, CurrentBarAdjust + 1f);
		CurrentBarAdjust = newBarAdjust;
	}

	public void LiftHpBar(AgentUnit unit, List<AgentUnit> obst)
	{
		float newBarAdjust = CurrentBarAdjust;
		Vector3 position = transform.position;
		Vector3 position2 = unit.transform.position;
		float offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
		float dest;
		if (offset <= 2f)
		{
			if (newBarAdjust - unit.CurrentBarAdjust > 0.99f || newBarAdjust - unit.CurrentBarAdjust < -0.99f) return;
			dest = unit.CurrentBarAdjust + 1f;
		}
		else
		{
			if (newBarAdjust - unit.CurrentBarAdjust > 2.99f - offset || newBarAdjust - unit.CurrentBarAdjust < -0.99f) return;
			dest = unit.CurrentBarAdjust + 3f - offset;
		}
		if (dest > CurrentBarAdjust + 1f) return;
		newBarAdjust = dest;
		foreach (AgentUnit unit2 in obst)
		{
			if (unit2 == this) continue;
			position2 = unit2.transform.position;
			offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
			if (offset > 3f) continue;
			if (offset <= 2f)
			{
				if (newBarAdjust - unit2.CurrentBarAdjust > 0.99f || newBarAdjust - unit2.CurrentBarAdjust < -0.99f) continue;
				dest = unit2.CurrentBarAdjust + 1f;
			}
			else
			{
				if (newBarAdjust - unit2.CurrentBarAdjust > 2.99f - offset || newBarAdjust - unit2.CurrentBarAdjust < -0.99f) continue;
				dest = unit2.CurrentBarAdjust + 3f - offset;
			}
			if (unit2.CurrentBarAdjust > CurrentBarAdjust)
			{
				unit2.LiftHpBar(this, obst);
				continue;
			}
			if (dest > CurrentBarAdjust + 1f) continue;
			newBarAdjust = dest;
		}
		newBarAdjust = Mathf.Clamp(newBarAdjust, 0f, CurrentBarAdjust + 1f);
		CurrentBarAdjust = newBarAdjust;
	}

	public float CurrentBarAdjust
	{
		get
		{
			return _currentBarAdjust;
		}
		set
		{
			if (_currentBarAdjust == value) return;
			agentUI.ActiveControl.gameObject.transform.Translate(0f, 0.5f * (float)(value - _currentBarAdjust) * transform.localScale.x, 0f);
			_currentBarAdjust = value;
		}
	}

	private float _currentBarAdjust = 0f;

	//< <Mod>

	// Token: 0x04004E2E RID: 20014
	public AgentModel model;

	// Token: 0x04004E2F RID: 20015
	public AgentUnit.SelectedColors colors;

	// Token: 0x04004E30 RID: 20016
	public SpriteRenderer selectedIcon;

	// Token: 0x04004E31 RID: 20017
	[NonSerialized]
	public bool clicked;

	// Token: 0x04004E32 RID: 20018
	[NonSerialized]
	public float speech_cooltime;

	// Token: 0x04004E33 RID: 20019
	[NonSerialized]
	public float speech_frequency;

	// Token: 0x04004E34 RID: 20020
	[NonSerialized]
	public int speech_percentage;

	// Token: 0x04004E35 RID: 20021
	private bool speech_enable = true;

	// Token: 0x04004E36 RID: 20022
	private float speech_ealpsed;

	// Token: 0x04004E37 RID: 20023
	private bool selectIconState;

	// Token: 0x04004E38 RID: 20024
	private bool speech_force;

	// Token: 0x04004E39 RID: 20025
	private float speech_force_elapsed;

	// Token: 0x04004E3A RID: 20026
	private bool managing;

	// Token: 0x04004E3B RID: 20027
	public Text speechText;

	// Token: 0x04004E3C RID: 20028
	public AgentUnitUI ui;

	// Token: 0x04004E3D RID: 20029
	public AgentUI agentUI;

	// Token: 0x04004E3E RID: 20030
	public AgentSpriteChanger changer;

	// Token: 0x04004E3F RID: 20031
	public AgentContinueUI continueUI;

	// Token: 0x04004E40 RID: 20032
	private bool _selected;

	// Token: 0x04004E41 RID: 20033
	private bool dead;

	// Token: 0x04004E42 RID: 20034
	private bool lateInit;

	// Token: 0x04004E43 RID: 20035
	private bool isModifiedPuppetScale;

	// Token: 0x04004E44 RID: 20036
	private bool puppetAnimHasMoveCheck;

	// Token: 0x04004E45 RID: 20037
	private RuntimeAnimatorController oldPuppetAnimController;

	// Token: 0x04004E46 RID: 20038
	public bool isMovingByMannually;

	// Token: 0x04004E47 RID: 20039
	private bool isMovingStarted;

	// Token: 0x02000B02 RID: 2818
	[Serializable]
	public class SelectedColors
	{
		// Token: 0x060054F6 RID: 21750 RVA: 0x00004074 File Offset: 0x00002274
		public SelectedColors()
		{
		}

		// Token: 0x04004E48 RID: 20040
		public Color Selected;

		// Token: 0x04004E49 RID: 20041
		public Color Work;

		// Token: 0x04004E4A RID: 20042
		public Color Attack;

		// Token: 0x04004E4B RID: 20043
		public Color Normal;
	}
}
