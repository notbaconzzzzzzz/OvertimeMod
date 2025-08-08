/*
public virtual void Update() // Hp Bar Stacking, Display Abno Hp
public void OnClickCollectionFunc() // Add Notice Send; Overtime Yesod Suppression
public void OnClickByRoom(PointerEventData pData) // Queue Work Orders
+public void UpdateBarStacking() // Hp Bar Stacking
+public void LiftHpBar(CreatureUnit unit, List<CreatureUUnit> obst) // Hp Bar Stacking
+public float CurrentBarAdjust // Hp Bar Stacking
+private float _currentBarAdjust // Hp Bar Stacking
*/
using System;
using System.Collections.Generic; // 
using CommandWindow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B14 RID: 2836
public class CreatureUnit : MonoBehaviour, IMouseOnSelectListener, IMouseCommandTarget
{
	// Token: 0x0600556A RID: 21866 RVA: 0x001E9960 File Offset: 0x001E7B60
	public CreatureUnit()
	{
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x00044E38 File Offset: 0x00043038
	public void SetSliderColor(CreatureUnit.SliderColorSet set)
	{
		this.currentSliderColor = set;
		this.hpBg.color = set.bg;
		this.hpFill.color = set.fill;
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x00044E63 File Offset: 0x00043063
	public virtual void Awake()
	{
		this.debugText.gameObject.SetActive(false);
		this.currentSliderColor = this.defHp;
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x00044E82 File Offset: 0x00043082
	public void TempUpdateViewPos()
	{
		this.UpdateViewPosition();
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x001E81E8 File Offset: 0x001E63E8
	protected virtual void UpdateViewPosition()
	{
		MapEdge currentEdge = this.model.GetCurrentEdge();
		base.transform.localScale = new Vector3(this.model.GetMovableNode().currentScale, this.model.GetMovableNode().currentScale, base.transform.localScale.z);
		if (currentEdge != null && currentEdge.type == "door")
		{
			if (this.visible)
			{
				this.visible = false;
				Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
				currentViewPosition.z = 100000f;
				this.viewPosition = currentViewPosition;
			}
		}
		else
		{
			if (!this.visible)
			{
				this.visible = true;
			}
			this.viewPosition = this.model.GetCurrentViewPosition();
		}
		base.transform.localPosition = this.viewPosition;
	}

	// Token: 0x0600556F RID: 21871 RVA: 0x001E99B0 File Offset: 0x001E7BB0
	protected virtual void UpdateDirection()
	{
		MovableObjectNode movableNode = this.model.GetMovableNode();
		UnitDirection direction = movableNode.GetDirection();
		Vector3 vector = this.directionScaleFactor;
		if (direction == UnitDirection.RIGHT)
		{
			if (vector.x < 0f)
			{
				vector.x = -vector.x;
			}
		}
		else if (vector.x > 0f)
		{
			vector.x = -vector.x;
		}
		this.directionScaleFactor = vector;
	}

	// Token: 0x06005570 RID: 21872 RVA: 0x001E8348 File Offset: 0x001E6548
	protected virtual void UpdateScale()
	{
		if (this.scaleSetting)
		{
			return;
		}
		if (this.animTarget != null)
		{
			Vector3 localScale = this.animTarget.transform.localScale;
			if (localScale.x < 0f && this.directionScaleFactor.x > 0f)
			{
				localScale.x = -localScale.x;
			}
			if (localScale.x > 0f && this.directionScaleFactor.x < 0f)
			{
				localScale.x = -localScale.x;
			}
			this.animTarget.transform.localScale = localScale;
		}
	}

	// Token: 0x06005571 RID: 21873 RVA: 0x00044E8A File Offset: 0x0004308A
	public virtual void FixedUpdate()
	{
		this.UpdateViewPosition();
		this.UpdateDirection();
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001E9A2C File Offset: 0x001E7C2C
	public virtual void Update()
	{ // <Mod>
		if (this.oldState != this.model.state)
		{
			this.OnChangeState();
			this.oldState = this.model.state;
		}
		if (this.model.state == CreatureState.ESCAPE && (ResearchDataModel.instance.IsUpgradedAbility("show_agent_ui") || (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && GlobalGameManager.instance.tutorialStep > 1)) && !SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
		{
			if (!this.escapeUIRoot.activeInHierarchy && this.model.script.HasEscapeUI())
			{
				this.escapeUIRoot.SetActive(true);
				if (this.model is OrdealCreatureModel)
				{
					this.escapeRisk.text = (this.model as OrdealCreatureModel).OrdealBase.GetRiskLevel(this.model as OrdealCreatureModel).ToString().ToUpper();
					this.escapeCreatureName.text = (this.model as OrdealCreatureModel).OrdealBase.OrdealNameText(this.model as OrdealCreatureModel);
					this.escapeRisk.color = UIColorManager.instance.GetRiskColor((this.model as OrdealCreatureModel).OrdealBase.GetRiskLevel(this.model as OrdealCreatureModel));
				}
				else
				{
					this.escapeRisk.text = this.model.script.GetRiskLevel();
					this.escapeCreatureName.text = this.model.script.GetName();
					if (this.model is ChildCreatureModel)
					{
						this.escapeRisk.color = UIColorManager.instance.GetRiskColor(CreatureTypeInfo.GetRiskLevelStringToEnum(this.model.script.GetRiskLevel()));
					}
					else if (this.model.observeInfo.GetObserveState("stat"))
					{
						this.escapeRisk.color = UIColorManager.instance.GetRiskColor(CreatureTypeInfo.GetRiskLevelStringToEnum(this.model.metaInfo.riskLevelForce));
					}
					else
					{
						this.escapeRisk.color = Color.white;
					}
				}
			}
		}
		else if (this.escapeUIRoot.activeInHierarchy)
		{
			this.escapeUIRoot.SetActive(false);
		}
		if (this.model.script.SetHpSlider(this.hpSlider))
		{
			if (!this.hpSlider.gameObject.activeInHierarchy)
			{
				this.hpSlider.gameObject.SetActive(true);
			}
		}
		else if (this.hpSlider.gameObject.activeInHierarchy)
		{
			this.hpSlider.gameObject.SetActive(false);
		}
		if (this.castingSlider != null)
		{
			if (this.model.script.SetCastingSlider(this.castingSlider))
			{
				if (!this.castingSlider.gameObject.activeInHierarchy)
				{
					this.castingSlider.gameObject.SetActive(true);
				}
			}
			else if (this.castingSlider.gameObject.activeInHierarchy)
			{
				this.castingSlider.gameObject.SetActive(false);
			}
		}
		UpdateBarStacking();
		if (hpSlider.gameObject.activeInHierarchy)
		{
			AbnoHpDisplayMode displayeMode = SpecialModeConfig.instance.GetValue<AbnoHpDisplayMode>("DisplayAbnoHp");
			if (displayeMode == AbnoHpDisplayMode.NAME_AND_HP)
			{
				string str = string.Concat(new object[]
				{
					" (",
					Math.Round((decimal)model.hp, 0),
					"/",
					model.maxHp,
					")"
				});
				int num = escapeCreatureName.text.IndexOf("(");
				if (num > 0)
				{
					escapeCreatureName.text = escapeCreatureName.text.Substring(0, num - 1);
				}
				escapeCreatureName.text += str;
			}
			else if (displayeMode == AbnoHpDisplayMode.HP_ONLY)
			{
				string str = string.Concat(new object[]
				{
					"(",
					Math.Round((decimal)model.hp, 0),
					"/",
					model.maxHp,
					")"
				});
				escapeCreatureName.text = str;
			}
		}
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x00044E98 File Offset: 0x00043098
	public virtual void LateUpdate()
	{
		this.UpdateScale();
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x001E9D78 File Offset: 0x001E7F78
	public virtual void Start()
	{
		if (this.model.canBeSuppressed && this.model.state == CreatureState.SUPPRESSED)
		{
			if (this.animTarget != null)
			{
				this.animTarget.gameObject.SetActive(false);
			}
			this.returnObject.SetActive(true);
			this.currentSliderColor = this.defHp;
		}
		else
		{
			if (this.animTarget != null)
			{
				this.animTarget.gameObject.SetActive(true);
			}
			this.returnObject.SetActive(false);
		}
		this.model.script.OnViewInitPrev(this);
		this.model.script.OnViewInit(this);
		if (this._unitMouseEventTarget != null)
		{
			BoxCollider2D component = this._unitMouseEventTarget.GetComponent<BoxCollider2D>();
			component.size = new Vector2(this.model.script.GetRadius() * 2f + 3f, component.size.y);
		}
		this.currentCreatureCanvas.worldCamera = Camera.main;
		this.model.script.RoomSpriteInit();
		base.gameObject.name = string.Concat(new object[]
		{
			"CreatureBase(",
			this.model.instanceId,
			") : ",
			this.model.metaInfo.name
		});
		this.escapeUIRoot.SetActive(false);
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x001E9F00 File Offset: 0x001E8100
	public virtual void OnChangeState()
	{
		if (this.model.canBeSuppressed && (this.model.state == CreatureState.SUPPRESSED || this.model.state == CreatureState.SUPPRESSED_RETURN))
		{
			if (!this.model.script.OnAfterSuppressed())
			{
				if (this.animTarget.HasDeadMotion())
				{
					this.animTarget.PlayDeadMotion();
				}
				else
				{
					if (this.animTarget != null)
					{
						this.animTarget.gameObject.SetActive(false);
					}
					this.returnObject.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			this.animTarget.PlayRevivalMotion();
			if (this.animTarget != null)
			{
				this.animTarget.gameObject.SetActive(true);
			}
			this.returnObject.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x00044EA0 File Offset: 0x000430A0
	public Vector3 GetScaleFactor()
	{
		return this.scaleFactor;
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x00044EA8 File Offset: 0x000430A8
	public void SetScaleFactor(float x, float y, float z)
	{
		this.scaleFactor = new Vector3(x, y, z);
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001E9FEC File Offset: 0x001E81EC
	public virtual void OnDestroy()
	{
		if (this.model.script != null)
		{
			this.model.script.OnViewDestroy();
		}
		if (this.room != null)
		{
			UnityEngine.Object.Destroy(this.room.gameObject);
		}
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x001EA03C File Offset: 0x001E823C
	public SoundEffectPlayer PlaySoundMono(string soundKey)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, base.transform.position);
			if (soundEffectPlayer != null)
			{
				soundEffectPlayer.AttachToCamera();
			}
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding " + soundKey);
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x001EA0B0 File Offset: 0x001E82B0
	public SoundEffectPlayer PlaySound(string soundKey)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, base.transform.position);
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding " + soundKey);
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x001EA110 File Offset: 0x001E8310
	public SoundEffectPlayer PlaySound(string soundKey, float volume)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, base.transform.position, volume);
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding");
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x001EA16C File Offset: 0x001E836C
	public SoundEffectPlayer PlaySound(string soundKey, AudioRolloffMode mode)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, base.transform.position, mode);
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding");
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x001EA1C8 File Offset: 0x001E83C8
	public SoundEffectPlayer PlaySoundLoop(string soundKey)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.Play(filename, base.transform);
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding");
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x001EA218 File Offset: 0x001E8418
	public SoundEffectPlayer PlaySoundLoop(string soundKey, float volume)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(soundKey, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.Play(filename, base.transform, volume);
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in sound founding");
		}
		return soundEffectPlayer;
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x00044EB8 File Offset: 0x000430B8
	public void OnClickCollectionFunc()
	{ // <Mod>
		if (!this.model.script.OnOpenCollectionWindow())
		{
			return;
		}
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
		{
			return;
		}
		CreatureInfoWindow.CreateWindow(this.model.metadataId);
		Notice.instance.Send(NoticeName.OnOpenNameplate, new object[]
		{
			this
		});
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x001EA26C File Offset: 0x001E846C
	public void OnClicked()
	{
		try
		{
			if (RabbitManager.instance.ExistsSquad(this.model.GetMovableNode().GetCurrentStandingSefira().sefiraEnum))
			{
				return;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		if (this.model.state == CreatureState.ESCAPE)
		{
			if (!this.model.script.IsSuppressable())
			{
				return;
			}
			if (this.model.script is RedShoes)
			{
				EffectSound.instance.PlayEffectSound(EffectSoundType.CLICK);
				WorkerModel attractTargetAgent = (this.model.script.skill as RedShoesSkill).attractTargetAgent;
				CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, attractTargetAgent);
				return;
			}
			CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, this.model);
			EffectSound.instance.PlayEffectSound(EffectSoundType.CLICK);
			return;
		}
	}

	// Token: 0x06005581 RID: 21889 RVA: 0x00044EE1 File Offset: 0x000430E1
	public IMouseCommandTargetModel GetCommandTargetModel()
	{
		return this.model;
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x001EA348 File Offset: 0x001E8548
	public void OnClickByRoom(PointerEventData pData)
	{ // <Mod>
		if (CommandWindow.CommandWindow.isWorkOrderQueueEnabled && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && model.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			CommandWindow.CommandWindow.CreateWindow(CommandType.Management, model, true);
			room.audioClipPlayer.OnPlayInList(2);
			return;
		}
		if (CommandWindow.CommandWindow.isWorkOrderQueueEnabled && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && model.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			room.ClearWorkOrderQueue();
			room.audioClipPlayer.OnPlayInList(2);
			return;
		}
		if (this.model.state == CreatureState.ESCAPE || this.model.state == CreatureState.SUPPRESSED || this.model.state == CreatureState.SUPPRESSED_RETURN)
		{
			return;
		}
		if (this.model.IsWorkCountFull())
		{
			if (this.model.script.OnOpenWorkWindow())
			{
				this.room.audioClipPlayer.OnPlayInList(2);
			}
			return;
		}
		if (this.model.script.isWorkAllocated && this.model.state == CreatureState.WAIT)
		{
			if (pData.button == PointerEventData.InputButton.Left && SefiraBossManager.Instance.IsWorkCancelable)
			{
				Notice.instance.Send(NoticeName.ManageCancel, new object[]
				{
					this.model
				});
				this.room.OnCancelWork();
			}
			return;
		}
		if (pData.button == PointerEventData.InputButton.Right && UnitMouseEventManager.instance.GetSelectedAgents().Count == 0)
		{
			return;
		}
		if (this.model.script.OnOpenWorkWindow())
		{
			if (this.model.metaInfo.creatureWorkType == CreatureWorkType.KIT)
			{
				CommandWindow.CommandWindow.CreateWindow(CommandType.KitCreature, this.model);
			}
			else
			{
				CommandWindow.CommandWindow.CreateWindow(CommandType.Management, this.model);
			}
		}
		this.room.audioClipPlayer.OnPlayInList(1);
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x00044EE9 File Offset: 0x000430E9
	public bool IsSelectable()
	{
		return this.model.state == CreatureState.ESCAPE && this.model.hp > 0f;
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x00044F16 File Offset: 0x00043116
	public void OnSelect()
	{
		this.OnClicked();
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x000040A1 File Offset: 0x000022A1
	public void OnUnselect()
	{
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x00044F1E File Offset: 0x0004311E
	public void ResetAnimatorTransform()
	{
		AnimatorManager.instance.ResetCreatureAnimatorTransform(this.model.instanceId);
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x000040A1 File Offset: 0x000022A1
	public void SetRoomFog(float alpha)
	{
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x000040A1 File Offset: 0x000022A1
	public void SetRoomFog(float alpha, float time)
	{
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x000040A1 File Offset: 0x000022A1
	public void ReleaseFog()
	{
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x00044F35 File Offset: 0x00043135
	public void SpecialSkill()
	{
		this.room.ActivatedSkill();
	}

	//> <Mod> Hp Bar Stacking
	public void UpdateBarStacking()
	{
		if (!SpecialModeConfig.instance.GetValue<bool>("HpBarStackingAbnormality")) return;
		if (GameManager.currentGameManager.state == GameState.STOP) return;
		if (model.GetMovableNode().currentPassage == null) return;
		if (!hpSlider.gameObject.activeInHierarchy) return;
		float newBarAdjust = CurrentBarAdjust - 0.5f;
		if (newBarAdjust < 0f)
		{
			newBarAdjust = 0f;
		}
		Vector3 position = transform.position;
		List<CreatureUnit> obst = new List<CreatureUnit>();
		foreach (MovableObjectNode node in model.GetMovableNode().currentPassage.GetEnteredTargets())
		{
			if (!(node.GetUnit() is CreatureModel)) continue;
			CreatureModel creature = node.GetUnit() as CreatureModel;
			if (model.instanceId == creature.instanceId) continue;
			CreatureUnit unit = creature.Unit;
			if (!unit.hpSlider.gameObject.activeInHierarchy) continue;
			int ind = 0;
			foreach (CreatureUnit unit2 in obst)
			{
				if (CurrentBarAdjust <= unit2.CurrentBarAdjust) break;
				ind++;
			}
			obst.Insert(ind, unit);
		}
		foreach (CreatureUnit unit in obst)
		{
			Vector3 position2 = unit.transform.position;
			float offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
			if (offset > 5f) continue;
			float dest;
			if (offset <= 4f)
			{
				if (newBarAdjust - unit.CurrentBarAdjust > 0.99f || newBarAdjust - unit.CurrentBarAdjust < -0.99f) continue;
				dest = unit.CurrentBarAdjust + 1f;
			}
			else
			{
				if (newBarAdjust - unit.CurrentBarAdjust > 4.99f - offset || newBarAdjust - unit.CurrentBarAdjust < -0.99f) continue;
				dest = unit.CurrentBarAdjust + 5f - offset;
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

	public void LiftHpBar(CreatureUnit unit, List<CreatureUnit> obst)
	{
		float newBarAdjust = CurrentBarAdjust;
		Vector3 position = transform.position;
		Vector3 position2 = unit.transform.position;
		float offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
		float dest;
		if (offset <= 4f)
		{
			if (newBarAdjust - unit.CurrentBarAdjust > 0.99f || newBarAdjust - unit.CurrentBarAdjust < -0.99f) return;
			dest = unit.CurrentBarAdjust + 1f;
		}
		else
		{
			if (newBarAdjust - unit.CurrentBarAdjust > 4.99f - offset || newBarAdjust - unit.CurrentBarAdjust < -0.99f) return;
			dest = unit.CurrentBarAdjust + 5f - offset;
		}
		if (dest > CurrentBarAdjust + 1f) return;
		newBarAdjust = dest;
		foreach (CreatureUnit unit2 in obst)
		{
			if (unit2 == this) continue;
			position2 = unit2.transform.position;
			offset = Mathf.Abs(position.x - position2.x) / transform.localScale.x;
			if (offset > 5f) continue;
			if (offset <= 4f)
			{
				if (newBarAdjust - unit2.CurrentBarAdjust > 0.99f || newBarAdjust - unit2.CurrentBarAdjust < -0.99f) continue;
				dest = unit2.CurrentBarAdjust + 1f;
			}
			else
			{
				if (newBarAdjust - unit2.CurrentBarAdjust > 4.99f - offset || newBarAdjust - unit2.CurrentBarAdjust < -0.99f) continue;
				dest = unit2.CurrentBarAdjust + 5f - offset;
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
			escapeUIRoot.gameObject.transform.Translate(0f, -0.75f * (float)(value - _currentBarAdjust) * transform.localScale.x, 0f);
			_currentBarAdjust = value;
		}
	}

	private float _currentBarAdjust = 0f;

	//< <Mod>

	// Token: 0x04004EC1 RID: 20161
	public CreatureModel model;

	// Token: 0x04004EC2 RID: 20162
	public IsolateRoom room;

	// Token: 0x04004EC3 RID: 20163
	public SpriteRenderer returnSpriteRenderer;

	// Token: 0x04004EC4 RID: 20164
	public GameObject returnObject;

	// Token: 0x04004EC5 RID: 20165
	public Canvas currentCreatureCanvas;

	// Token: 0x04004EC6 RID: 20166
	public Slider castingSlider;

	// Token: 0x04004EC7 RID: 20167
	public Image cameraSensingArea;

	// Token: 0x04004EC8 RID: 20168
	public CreatureAnimScript animTarget;

	// Token: 0x04004EC9 RID: 20169
	public UnitMouseEventTarget defaultMouseTarget;

	// Token: 0x04004ECA RID: 20170
	public UnitMouseEventTarget _unitMouseEventTarget;

	// Token: 0x04004ECB RID: 20171
	public CreatureSpeech speech;

	// Token: 0x04004ECC RID: 20172
	protected Vector3 directionScaleFactor = new Vector3(1f, 1f, 1f);

	// Token: 0x04004ECD RID: 20173
	protected Vector3 scaleFactor = new Vector3(1f, 1f, 1f);

	// Token: 0x04004ECE RID: 20174
	private Vector2 oldScale;

	// Token: 0x04004ECF RID: 20175
	protected Vector3 viewPosition;

	// Token: 0x04004ED0 RID: 20176
	protected bool visible = true;

	// Token: 0x04004ED1 RID: 20177
	[NonSerialized]
	public bool scaleSetting;

	// Token: 0x04004ED2 RID: 20178
	[Header("Debug")]
	public Text debugText;

	// Token: 0x04004ED3 RID: 20179
	[Header("UI")]
	public GameObject escapeUIRoot;

	// Token: 0x04004ED4 RID: 20180
	public Slider hpSlider;

	// Token: 0x04004ED5 RID: 20181
	public Image hpBg;

	// Token: 0x04004ED6 RID: 20182
	public Image hpFill;

	// Token: 0x04004ED7 RID: 20183
	public Text escapeRisk;

	// Token: 0x04004ED8 RID: 20184
	public Text escapeCreatureName;

	// Token: 0x04004ED9 RID: 20185
	[Space(10f)]
	public CreatureUnit.SliderColorSet defHp;

	// Token: 0x04004EDA RID: 20186
	public CreatureUnit.SliderColorSet casting;

	// Token: 0x04004EDB RID: 20187
	private CreatureUnit.SliderColorSet currentSliderColor;

	// Token: 0x04004EDC RID: 20188
	protected CreatureState oldState;

	// Token: 0x02000B15 RID: 2837
	[Serializable]
	public class SliderColorSet
	{
		// Token: 0x0600558B RID: 21899 RVA: 0x00004074 File Offset: 0x00002274
		public SliderColorSet()
		{
		}

		// Token: 0x04004EDD RID: 20189
		public Color bg;

		// Token: 0x04004EDE RID: 20190
		public Color fill;
	}
}
