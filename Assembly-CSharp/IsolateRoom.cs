using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Isolate;
using BinahBoss;
using CommandWindow;
using GlobalBullet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B12 RID: 2834
public class IsolateRoom : MonoBehaviour, IObserver
{
	// Token: 0x0600556E RID: 21870 RVA: 0x001E9424 File Offset: 0x001E7624
	public IsolateRoom()
	{
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x0600556F RID: 21871 RVA: 0x00044D7B File Offset: 0x00042F7B
	private Animator CurrentWorkAnim
	{
		get
		{
			return this.CurrentWorkRoot.GetComponent<Animator>();
		}
	}

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06005570 RID: 21872 RVA: 0x00044D88 File Offset: 0x00042F88
	private GameObject CurrentResultRoot
	{
		get
		{
			return this.CurrentResultFilter.gameObject;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06005571 RID: 21873 RVA: 0x00044D95 File Offset: 0x00042F95
	private UIController WorkResultUIController
	{
		get
		{
			return this.CurrentResultFilter.GetComponent<UIController>();
		}
	}

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06005572 RID: 21874 RVA: 0x00044DA2 File Offset: 0x00042FA2
	private UIController DamageUIController
	{
		get
		{
			return this.DamageFilter.GetComponent<UIController>();
		}
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06005573 RID: 21875 RVA: 0x00044DAF File Offset: 0x00042FAF
	private GameObject DamageFilterObject
	{
		get
		{
			return this.DamageFilter.gameObject;
		}
	}

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06005574 RID: 21876 RVA: 0x00044DBC File Offset: 0x00042FBC
	// (set) Token: 0x06005575 RID: 21877 RVA: 0x00044DC4 File Offset: 0x00042FC4
	public PointerEventData.InputButton TargetClick
	{
		get
		{
			return this._targetClick;
		}
		set
		{
			this._targetClick = value;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06005576 RID: 21878 RVA: 0x00044DCD File Offset: 0x00042FCD
	private bool OvelrayEnable
	{
		get
		{
			return this.CheckOverlayState();
		}
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001E9478 File Offset: 0x001E7678
	private void InitWorkCountUI()
	{
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001E9488 File Offset: 0x001E7688
	private bool CheckOverlayState()
	{
		CreatureState state = this.TargetUnit.model.state;
		return (state != CreatureState.WORKING || this.TargetUnit.model.metaInfo.creatureKitType == CreatureKitType.CHANNEL) && state != CreatureState.SUPPRESSED && state != CreatureState.SUPPRESSED_RETURN && (this.IsWorkAllocated || this.pointerEntered);
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x001E94F0 File Offset: 0x001E76F0
	public void SetOverloadAlarmColor(OverloadType type)
	{
		Color overloadColor = UIColorManager.instance.GetOverloadColor(type);
		this.binahOverloadUI.SetColor(overloadColor);
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x001E9518 File Offset: 0x001E7718
	public void SetBinahBoss()
	{
		GameObject gameObject = Prefab.LoadPrefab("UIComponent/BinahOverloadAlarm");
		BinahOverloadUI component = gameObject.GetComponent<BinahOverloadUI>();
		Transform parent = this.overloadUI.alarms[0].transform.parent;
		gameObject.transform.SetParent(parent.parent);
		gameObject.transform.localPosition = parent.localPosition;
		gameObject.transform.localScale = parent.localScale;
		gameObject.transform.localRotation = parent.localRotation;
		gameObject.GetComponent<RectTransform>().anchoredPosition = parent.GetComponent<RectTransform>().anchoredPosition;
		component.timerBar = this.overloadUI.timerBar;
		component.timerText = this.overloadUI.timerText;
		component.originPositionY = this.overloadUI.OriginPositionY;
		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = gameObject.transform.GetChild(i);
			Image component2 = child.GetComponent<Image>();
			if (component2 != null)
			{
				component.alarms.Add(component2);
			}
		}
		this.binahOverloadUI = component;
		this.binahOverloadUI.SetActive(false);
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x001E9648 File Offset: 0x001E7848
	private bool IsKitReturnning()
	{
		if (this.TargetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			return false;
		}
		try
		{
			if (this.TargetUnit.model.state == CreatureState.WAIT && this.TargetUnit.model.kitEquipOwner != null && this.TargetUnit.model.kitEquipOwner.GetCurrentCommand() is ReturnKitCreatureCommand)
			{
				return true;
			}
		}
		catch (Exception ex)
		{
			return false;
		}
		return false;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x00044DD5 File Offset: 0x00042FD5
	public void SetCounterText(string text)
	{
		this.CounterText.text = text;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x00044DE3 File Offset: 0x00042FE3
	public void SetCounterText(string text, Color color)
	{
		this.CounterText.text = text;
		this.CounterText.color = color;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x001E96E4 File Offset: 0x001E78E4
	public void SetCounterEnable(bool state)
	{
		this.CounterActiveControl.SetActive(true);
		this.CounterOutline.SetActive(true);
		if (!this.TargetUnit.model.observeInfo.GetObserveState("stat"))
		{
			this.CounterInnerImage.gameObject.SetActive(true);
			this.CounterText.gameObject.SetActive(false);
			this.CounterInnerImage.sprite = this.CounterImage_Unknown;
			this.CounterColor.color = this.CounterColor_Black;
			this.CounterInnerImage.color = this.CounterColor_Normal;
			this._counterObserved = false;
			return;
		}
		this._counterObserved = true;
		this._counterEnabled = state;
		if (!state)
		{
			this.CounterInnerImage.gameObject.SetActive(true);
			this.CounterText.gameObject.SetActive(false);
			this.CounterInnerImage.sprite = this.CounterImage_None;
			this.CounterColor.color = this.CounterColor_Black;
			this.CounterInnerImage.color = this.CounterColor_Normal;
		}
		else
		{
			this.CounterInnerImage.gameObject.SetActive(false);
			this.CounterText.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x001E9818 File Offset: 0x001E7A18
	public string GetStateText(string key)
	{
		string str = "AgentState_";
		switch (key)
		{
		case "cancel":
			return (!SefiraBossManager.Instance.IsWorkCancelable) ? LocalizeTextDataModel.instance.GetText(str + "Cancel_Fail") : LocalizeTextDataModel.instance.GetText(str + "Cancel");
		case "moving":
			return LocalizeTextDataModel.instance.GetText(str + "Moving");
		case "suppress":
			if (this.TargetUnit.model.script.IsIndirectSuppressable())
			{
				return LocalizeTextDataModel.instance.GetText(str + "OrderSuppress");
			}
			return LocalizeTextDataModel.instance.GetText(str + "UnknownPosition");
		case "order":
			return LocalizeTextDataModel.instance.GetText(str + "OrderManagement");
		case "order_use":
			return LocalizeTextDataModel.instance.GetText(str + "KitOrderManagement");
		case "return":
			return LocalizeTextDataModel.instance.GetText(str + "KitReturn");
		case "cancel_use":
			return LocalizeTextDataModel.instance.GetText(str + "KitCancelUse");
		case "returnCancel":
			return LocalizeTextDataModel.instance.GetText(str + "KitReturnCancel");
		}
		return "Unknown";
	}

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06005580 RID: 21888 RVA: 0x00044DFD File Offset: 0x00042FFD
	// (set) Token: 0x06005581 RID: 21889 RVA: 0x001E99F8 File Offset: 0x001E7BF8
	public CreatureUnit TargetUnit
	{
		get
		{
			return this._targetUnit;
		}
		private set
		{
			if (this._targetUnit != null)
			{
				Notice.instance.Remove("UpdateCreatureState_" + this._targetUnit.model.instanceId, this);
				Notice.instance.Remove("ShowOutsideTypo_" + this._targetUnit.model.instanceId, this);
				Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreaturePhysicsAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
				Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreatureMentalAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
				Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreatureComplexAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
			}
			this._targetUnit = value;
			if (this._targetUnit != null)
			{
				Notice.instance.Observe("UpdateCreatureState_" + this._targetUnit.model.instanceId, this);
				Notice.instance.Observe("ShowOutsideTypo_" + this._targetUnit.model.instanceId, this);
				Notice.instance.Observe(NoticeName.MakeName(NoticeName.OnCreaturePhysicsAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
				Notice.instance.Observe(NoticeName.MakeName(NoticeName.OnCreatureMentalAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
				Notice.instance.Observe(NoticeName.MakeName(NoticeName.OnCreatureComplexAttack, new string[]
				{
					this._targetUnit.model.instanceId.ToString()
				}), this);
				this.InitWorkCountUI();
			}
		}
	}

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06005582 RID: 21890 RVA: 0x00044E05 File Offset: 0x00043005
	public IsolateRoom.WorkProgress ProgressBar
	{
		get
		{
			return this._progressBar;
		}
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06005583 RID: 21891 RVA: 0x00044E0D File Offset: 0x0004300D
	// (set) Token: 0x06005584 RID: 21892 RVA: 0x00044E15 File Offset: 0x00043015
	private bool IsWorkAllocated
	{
		get
		{
			return this._isWorkAllocated;
		}
		set
		{
			this._isWorkAllocated = value;
		}
	}

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06005585 RID: 21893 RVA: 0x00044E1E File Offset: 0x0004301E
	// (set) Token: 0x06005586 RID: 21894 RVA: 0x00044E26 File Offset: 0x00043026
	private bool IsWorking
	{
		get
		{
			return this._isWorking;
		}
		set
		{
			this._isWorking = value;
			this.RoomFog.gameObject.SetActive(!this._isWorking);
		}
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x00044E48 File Offset: 0x00043048
	public void SetCreature(CreatureUnit unit)
	{
		this.TargetUnit = unit;
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x0000403D File Offset: 0x0000223D
	private void Awake()
	{
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x001E9C34 File Offset: 0x001E7E34
	private void Start()
	{
		this.CancelWork.gameObject.SetActive(false);
		base.gameObject.name = "IsolateRoom : " + this._targetUnit.model.metaInfo.name;
		this.TooltipReduceProb.gameObject.SetActive(false);
		if (this._targetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.KIT)
		{
			this.TooltipUniquePE.gameObject.SetActive(false);
			this.TooltipCurrentPE.gameObject.SetActive(false);
			this.TooltipGeneratedPE.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x00044E51 File Offset: 0x00043051
	public void OnNotice(string notice, params object[] param)
	{
		if (notice.Split(new char[]
		{
			'_'
		})[0] == "UpdateCreatureState")
		{
			this.UpdateStatus();
		}
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x001E9CDC File Offset: 0x001E7EDC
	public void Init()
	{
		DamageInfo workDamage = this.TargetUnit.model.metaInfo.workDamage;
		this.DescController.Init(this);
		this.RoomFog.gameObject.SetActive(true);
		this.oldState = this.TargetUnit.model.GetFeelingState();
		this.CreatureName.text = this.TargetUnit.model.GetUnitName();
		this._progressBar = new IsolateRoom.WorkProgress(this.CubeGridRoot, this.SuccessCubeGrid, this.FailCubeGrid, this.CurrentWorkCubeText, this.CurrentWorkCubeFill);
		this._progressBar.Init(this._targetUnit.model, this.SuccessCubeObject, this.FailCubeObject);
		this.DamageFilterObject.SetActive(false);
		this.CurrentResultRoot.SetActive(false);
		this.DamageFilter.sprite = CreatureLayer.IsolateRoomUIData.GetDamageSprite(workDamage.type);
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		IsolatePos specialSkillPos = this.TargetUnit.model.specialSkillPos;
		if (specialSkillPos != IsolatePos.DOWN)
		{
			if (specialSkillPos == IsolatePos.UP)
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
		this.NarrationFadeEffect.transform.localPosition = zero2;
		this.SpecialSkillRoot.transform.localPosition = zero;
		this.SpecialSkillRoot.SetActive(false);
		this.NarrationFadeEffect.gameObject.SetActive(false);
		this.CurrentWorkCubeText.text = string.Empty;
		this.CurrentWorkCubeFill.color = CreatureLayer.IsolateRoomUIData.DisabledColor;
		this.CurrentWorkRoot.SetActive(false);
		this.SetCumlativeCubeCount();
		this.SetRiskLevel();
		IsolateMessageSender component = this.CumlativeCubeImage.GetComponent<IsolateMessageSender>();
		component._event.AddListener(delegate()
		{
			this.OnCubeArrived();
		});
		this.RoomColor = UIColorManager.instance.GetDisabledRoomColor(this.TargetUnit.model.sefiraOrigin.sefiraEnum);
		this.OverlayImage.gameObject.SetActive(false);
		this.RabbitBlock.SetActive(false);
		this.CreatureName.color = this.NameTextColor;
		this.CreatureNameImage.color = this.NameTextureColor;
		if (this.TargetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			foreach (GameObject gameObject in this.normalCreatureUIs)
			{
				gameObject.SetActive(true);
			}
			foreach (GameObject gameObject2 in this.kitCreatureUIs)
			{
				gameObject2.SetActive(false);
			}
			this.KitObserveRoot.SetActive(false);
		}
		else
		{
			foreach (GameObject gameObject3 in this.kitCreatureUIs)
			{
				gameObject3.SetActive(true);
			}
			foreach (GameObject gameObject4 in this.normalCreatureUIs)
			{
				gameObject4.SetActive(false);
			}
			this.KitObserveRoot.SetActive(true);
		}
		this.SetKitObserveLevel();
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001EA0E0 File Offset: 0x001E82E0
	public void SetKitObserveLevel()
	{
		int observationLevel = this.TargetUnit.model.GetObservationLevel();
		for (int i = 0; i < 4; i++)
		{
			Outline component = this.KitObserveLevel[i].GetComponent<Outline>();
			if (i < observationLevel)
			{
				component.enabled = true;
				this.KitObserveLevel[i].color = this.KitNormal;
			}
			else
			{
				component.enabled = false;
				this.KitObserveLevel[i].color = this.KitDisable;
			}
		}
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x00044E80 File Offset: 0x00043080
	public void SetCumlativeCubeCount()
	{
		this.CumlativeCubeCount.text = "0";
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001EA160 File Offset: 0x001E8360
	public void SetRiskLevel()
	{
		CreatureModel model = this.TargetUnit.model;
		RiskLevel riskLevelStringToEnum = CreatureTypeInfo.GetRiskLevelStringToEnum(model.metaInfo.riskLevelForce);
		if (model.metaInfo.creatureWorkType == CreatureWorkType.KIT)
		{
			int observeCost = model.observeInfo.GetObserveCost(CreatureModel.regionName[0]);
			if (model.metaInfo.creatureKitType == CreatureKitType.ONESHOT)
			{
				if (model.observeInfo.totalKitUseCount >= observeCost)
				{
					this.RiskLevelImage.sprite = CreatureLayer.IsolateRoomUIData.RiskLevel[(int)riskLevelStringToEnum];
					return;
				}
			}
			else if (model.observeInfo.totalKitUseTime >= (float)observeCost)
			{
				this.RiskLevelImage.sprite = CreatureLayer.IsolateRoomUIData.RiskLevel[(int)riskLevelStringToEnum];
				return;
			}
		}
		else if (model.observeInfo.GetObserveState(CreatureModel.regionName[0]))
		{
			this.RiskLevelImage.sprite = CreatureLayer.IsolateRoomUIData.RiskLevel[(int)riskLevelStringToEnum];
			return;
		}
		this.RiskLevelImage.sprite = CreatureLayer.IsolateRoomUIData.RiskLevel[5];
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x0000403D File Offset: 0x0000223D
	public void UpdateStatus()
	{
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x00044E92 File Offset: 0x00043092
	public void OnWorkAllocated(AgentModel incomingWorker)
	{
		this.IsWorkAllocated = true;
		this._currentAllocateWorker = incomingWorker;
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x0000403D File Offset: 0x0000223D
	public void OnWorkEnd()
	{
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001EA264 File Offset: 0x001E8464
	private void OnDisable()
	{
		if (this._targetUnit != null)
		{
			Notice.instance.Remove("UpdateCreatureState_" + this._targetUnit.model.instanceId, this);
			Notice.instance.Remove("ShowOutsideTypo_" + this._targetUnit.model.instanceId, this);
			Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreaturePhysicsAttack, new string[]
			{
				this._targetUnit.model.instanceId.ToString()
			}), this);
			Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreatureMentalAttack, new string[]
			{
				this._targetUnit.model.instanceId.ToString()
			}), this);
			Notice.instance.Remove(NoticeName.MakeName(NoticeName.OnCreatureComplexAttack, new string[]
			{
				this._targetUnit.model.instanceId.ToString()
			}), this);
		}
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001EA384 File Offset: 0x001E8584
	public void OnClick(BaseEventData eventData)
	{
		PointerEventData pointerEventData = eventData as PointerEventData;
		if (pointerEventData.button == PointerEventData.InputButton.Left)
		{
			UnitMouseEventManager.instance.UnselectAll();
		}
		if (RabbitManager.instance.ExistsSquad(this.TargetUnit.model.sefira.sefiraEnum))
		{
			return;
		}
		if (!this._isEscaped)
		{
			if (this.IsWorking)
			{
				if (this.TargetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.KIT && this.TargetUnit.model.metaInfo.creatureKitType == CreatureKitType.CHANNEL && this.TargetUnit.model.currentSkill != null)
				{
					if (pointerEventData.button != PointerEventData.InputButton.Left)
					{
						return;
					}
					if (this.TargetUnit.model.script.PermitCancelCurrentWork())
					{
						this.TargetUnit.model.currentSkill.CancelWork();
					}
				}
			}
			else if (this.TargetUnit.model.metaInfo.creatureKitType == CreatureKitType.EQUIP)
			{
				if (this.TargetUnit.model.kitEquipOwner != null)
				{
					if (pointerEventData.button != PointerEventData.InputButton.Left)
					{
						return;
					}
					if (this.TargetUnit.model.kitEquipOwner.target == null)
					{
						this.TargetUnit.model.kitEquipOwner.ReturnKitCreature();
					}
					else
					{
						this.TargetUnit.model.kitEquipOwner.ReturnCancelKitCreature();
					}
				}
				else
				{
					this.TargetUnit.OnClickByRoom(pointerEventData);
				}
			}
			else if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && this.TargetClick != PointerEventData.InputButton.Middle)
			{
				if (this.TargetClick == pointerEventData.button)
				{
					this.TargetUnit.OnClickByRoom(pointerEventData);
				}
			}
			else
			{
				this.TargetUnit.OnClickByRoom(pointerEventData);
			}
		}
		else
		{
			if (pointerEventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.TargetUnit.model.script is RedShoes)
			{
				WorkerModel attractTargetAgent = (this.TargetUnit.model.script.skill as RedShoesSkill).attractTargetAgent;
				CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, attractTargetAgent);
				return;
			}
			if (this.TargetUnit.model.script is SingingMachine)
			{
				return;
			}
			if (this.TargetUnit.model.script is BlackSwan)
			{
				ChildCreatureModel sister = (this.TargetUnit.model.script as BlackSwan).sister;
				CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, sister);
				return;
			}
			if (this.TargetUnit.model.script is SmallBird)
			{
				SmallBird smallBird = this.TargetUnit.model.script as SmallBird;
				if (smallBird.Boss_Activated)
				{
					EventCreatureModel smallEggModel = smallBird.Boss.SmallEggModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, smallEggModel);
				}
				else if (smallBird.OtherBirdState == BossBird.OtherBirdState.MOVETOGATE)
				{
					EventCreatureModel gateWayModel = smallBird.Boss.GateWayModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, gateWayModel);
				}
			}
			else if (this.TargetUnit.model.script is BigBird)
			{
				BigBird bigBird = this.TargetUnit.model.script as BigBird;
				if (bigBird.Boss_Activated)
				{
					EventCreatureModel bigEggModel = bigBird.Boss.BigEggModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, bigEggModel);
				}
				else if (bigBird.OtherBirdState == BossBird.OtherBirdState.MOVETOGATE)
				{
					EventCreatureModel gateWayModel2 = bigBird.Boss.GateWayModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, gateWayModel2);
				}
			}
			else if (this.TargetUnit.model.script is LongBird)
			{
				LongBird longBird = this.TargetUnit.model.script as LongBird;
				if (longBird.Boss_Activated)
				{
					EventCreatureModel longEggModel = longBird.Boss.LongEggModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, longEggModel);
				}
				else if (longBird.OtherBirdState == BossBird.OtherBirdState.MOVETOGATE)
				{
					EventCreatureModel gateWayModel3 = longBird.Boss.GateWayModel;
					CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, gateWayModel3);
				}
			}
			if (this.TargetUnit.model.script.IsSuppressable())
			{
				if (!this.TargetUnit.model.script.IsIndirectSuppressable())
				{
					return;
				}
				CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, this.TargetUnit.model);
			}
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x001EA7D0 File Offset: 0x001E89D0
	public void OnClickCollectionButton(BaseEventData eventData)
	{
		PointerEventData pointerEventData = eventData as PointerEventData;
		if (pointerEventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.TargetUnit.OnClickCollectionFunc();
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x00044EA2 File Offset: 0x000430A2
	public void OnDamageInvoked(DamageInfo damage)
	{
		this.DamageUIController.Show();
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x00044EAF File Offset: 0x000430AF
	public void SetWorkIcon(Sprite s)
	{
		this.CurrentWorkIcon.sprite = s;
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x0000403D File Offset: 0x0000223D
	public void DisableWorkIcon()
	{
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x001EA7FC File Offset: 0x001E89FC
	private void SetResult(CreatureFeelingState state)
	{
		Sprite sprite = null;
		Color white = Color.white;
		CreatureLayer.IsolateRoomUIData.GetFeelingStateData(state, out sprite, out white);
		this.CurrentResultRoot.SetActive(true);
		this.CurrentResultIcon.sprite = sprite;
		this.CurrentResultFilter.color = white;
		float value = 0.25f * (float)state + 0.25f;
		this.WorkResultUIController.animator.SetFloat("Speed", value);
		this.WorkResultUIController.Show();
		this._isResultDisplaying = true;
		this._cumlativeCubeAnimRemain = this.ProgressBar.GetResultCount();
		if (this._cumlativeCubeAnimRemain > 0)
		{
			this.StartCubeAnim();
		}
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x001EA8A0 File Offset: 0x001E8AA0
	private void FixedUpdate()
	{
		if (this._isResultDisplaying)
		{
			this.CurrentResultCooltime.text = ((int)this.TargetUnit.model.feelingStateRemainTime).ToString();
		}
		this.CheckWorkProcessText();
		this.CheckQliphothCounter();
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x001EA8F0 File Offset: 0x001E8AF0
	public void CheckWorkProcessText()
	{
		if (this._workDescTimer.started && this._workDescTimer.RunTimer())
		{
			this._workDescTimer.StartTimer(this.WorkDescFreq);
			if (!this.TargetUnit.model.script.IsActivatedWorkDesc())
			{
				return;
			}
			int num = 0;
			try
			{
				if (this._currentUsedProcessText.Count == 0)
				{
					this._workDescTimer.StartTimer(this.WorkDescFreq);
					return;
				}
				num = this._currentUsedProcessText[UnityEngine.Random.Range(0, this._currentUsedProcessText.Count)];
			}
			catch (IndexOutOfRangeException ex)
			{
				Debug.LogError("Error");
				num = UnityEngine.Random.Range(0, 11);
			}
			string desc = this.GetDesc(string.Format("{0}_{1}", this._currentWorkType.ToString(), num));
			this._currentUsedProcessText.Remove(num);
			this.DescController.Display(desc, num);
		}
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x001EAA00 File Offset: 0x001E8C00
	public void CheckQliphothCounter()
	{
		if (this._counterObserved && this._counterEnabled)
		{
			this.CounterText.text = this.TargetUnit.model.qliphothCounter.ToString();
			if (this.TargetUnit.model.qliphothCounter == 0)
			{
				this.CounterText.color = this.CounterColor_White;
				this.CounterColor.color = this.CounterColor_Red;
			}
			else
			{
				this.CounterText.color = this.CounterColor_Normal;
				this.CounterColor.color = this.CounterColor_Black;
			}
		}
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001EAAAC File Offset: 0x001E8CAC
	private void CheckWorkerMoving()
	{
		if (this._currentAllocateWorker == null)
		{
			this.OnCancelWork();
			return;
		}
		if (this._currentAllocateWorker.IsDead())
		{
			this.OnCancelWork();
		}
		if (this._currentAllocateWorker.IsCrazy())
		{
			this.OnCancelWork();
		}
		if (this._currentAllocateWorker.IsPanic())
		{
			this.OnCancelWork();
		}
		if (this._currentAllocateWorker.target != this.TargetUnit.model)
		{
			this.OnCancelWork();
		}
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001EAB30 File Offset: 0x001E8D30
	private void Update()
	{
		if (this.TargetUnit == null)
		{
			return;
		}
		if (this.IsWorkAllocated)
		{
			this.CheckWorkerMoving();
		}
		foreach (MaskableGraphic maskableGraphic in this.ColorSetted)
		{
			maskableGraphic.color = this.RoomColor;
		}
		if (this.OvelrayEnable || this.IsKitReturnning())
		{
			if (!this.OverlayImage.gameObject.activeInHierarchy)
			{
				this.OverlayImage.gameObject.SetActive(true);
			}
			this.CheckOverlay();
		}
		else if (this.OverlayImage.gameObject.activeInHierarchy)
		{
			this.OverlayImage.gameObject.SetActive(false);
		}
		if (this._checkCumlatvieCubeCount)
		{
			this.CumlativeCubeCount.text = this.TargetUnit.model.GetCurrentCumlatvieCube().ToString();
		}
		this.CheckMaxworkCount();
		if (!this.nameEntered && !this.IsWorking)
		{
			if (this.TargetUnit.model.IsEscapedOnlyEscape())
			{
				this.CreatureName.color = Color.black;
				this.CreatureNameImage.color = this.CounterColor_Red;
				this.CurrentWorkCubeFill.color = this.CounterColor_Red;
				this.CurrentWorkCubeText.color = Color.black;
				this.CumlativeCubeImage.sprite = this.EscapeCubeSprite;
				this.CumlativeCubeCount.color = Color.black;
			}
			else
			{
				this.CreatureName.color = this.CounterColor_Normal;
				this.CreatureNameImage.color = this.NameTextureColor;
				this.CurrentWorkCubeFill.color = this.NameTextureColor;
				this.CurrentWorkCubeText.color = this.NameTextColor;
				this.CumlativeCubeImage.sprite = this.NormalCubeSprite;
				this.CumlativeCubeCount.color = this.CounterColor_Red;
			}
		}
		if (this._targetUnit.model.overloadType == OverloadType.DEFAULT)
		{
			if (this._targetUnit.model.isOverloaded != this.overloadUI.isActivated)
			{
				this.overloadUI.enabled = true;
				this.overloadUI.SetActive(this._targetUnit.model.isOverloaded);
				if (this.binahOverloadUI != null)
				{
					this.binahOverloadUI.enabled = false;
				}
			}
		}
		else if (this.binahOverloadUI != null && this._targetUnit.model.isOverloaded != this.binahOverloadUI.isActivated)
		{
			this.overloadUI.SetActive(false);
			this.binahOverloadUI.enabled = true;
			this.binahOverloadUI.overloadType = this._targetUnit.model.overloadType;
			this.binahOverloadUI.SetActive(this._targetUnit.model.isOverloaded);
			this.overloadUI.enabled = false;
		}
		if (this.overloadUI.isActivated)
		{
			this.overloadUI.SetTimer(this._targetUnit.model.currentOverloadMaxTime - this._targetUnit.model.overloadTimer, this._targetUnit.model.currentOverloadMaxTime);
		}
		if (this.binahOverloadUI != null && this.binahOverloadUI.isActivated)
		{
			this.binahOverloadUI.SetTimer(this._targetUnit.model.currentOverloadMaxTime - this._targetUnit.model.overloadTimer, this._targetUnit.model.currentOverloadMaxTime);
		}
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x001EAF08 File Offset: 0x001E9108
	private void CheckMaxworkCount()
	{
		int num = this.TargetUnit.model.workCount;
		int maxWorkCountView = this.TargetUnit.model.GetMaxWorkCountView();
		if (this.oldCount != num)
		{
			if (num < maxWorkCountView)
			{
				this.workCount.color = this.RoomColor;
				this._maxWorkcountReached = false;
				this.workCount.text = this.TargetUnit.model.workCount + "/" + this.TargetUnit.model.GetMaxWorkCountView();
			}
			else
			{
				this.workCount.text = "MAX";
				this.workCount.color = this.WorkCount_Enabled;
				this._maxWorkcountReached = true;
			}
			this.oldCount = this.TargetUnit.model.workCount;
			for (int i = 0; i < this.TargetUnit.model.GetMaxWorkCountView(); i++)
			{
				Image image = this.WorkCounterImage[i];
				if (i < this.oldCount)
				{
					image.color = this.WorkCount_Enabled;
				}
				else
				{
					image.color = this.WorkCount_Disabled;
				}
			}
		}
		if (!this.IsWorking && this._maxWorkcountReached)
		{
			if (!this.MaxWorkCountFilter.activeInHierarchy)
			{
				this.MaxWorkCountFilter.SetActive(true);
			}
			if (this.pointerEntered)
			{
				if (!this.MaxWorkCountLine.activeInHierarchy)
				{
					this.MaxWorkCountLine.SetActive(true);
				}
			}
			else if (this.MaxWorkCountLine.activeInHierarchy)
			{
				this.MaxWorkCountLine.SetActive(false);
			}
		}
		else
		{
			if (this.MaxWorkCountFilter.activeInHierarchy)
			{
				this.MaxWorkCountFilter.SetActive(false);
			}
			if (this.MaxWorkCountLine.activeInHierarchy)
			{
				this.MaxWorkCountLine.SetActive(false);
			}
		}
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x00044EBD File Offset: 0x000430BD
	public void OnFeelingStateDisplayStart(CreatureFeelingState state)
	{
		this.SetResult(state);
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x00044EC6 File Offset: 0x000430C6
	public void OnDescriptionUnitEnded(int id)
	{
		if (id == -1)
		{
			return;
		}
		if (!this._currentUsedProcessText.Contains(id))
		{
			this._currentUsedProcessText.Add(id);
		}
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x00021C5C File Offset: 0x0001FE5C
	public float GetCurrentWorkSpeed()
	{
		return 1f;
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x00044EED File Offset: 0x000430ED
	public void OnFeelingStateDisplayEnd()
	{
		this.CurrentResultRoot.SetActive(false);
		this.WorkResultUIController.Hide();
		this.IsWorking = false;
		this._isResultDisplaying = false;
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x00044F14 File Offset: 0x00043114
	public void OnEscape()
	{
		this._isEscaped = true;
		this.EscapeFilter.Activated = true;
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x00044F29 File Offset: 0x00043129
	public void OnReturn()
	{
		this._isEscaped = false;
		this.EscapeFilter.Activated = false;
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x001EB0F4 File Offset: 0x001E92F4
	public void OnEnterRoom(AgentModel worker, UseSkill skill)
	{
		this.IsWorkAllocated = false;
		this.IsWorking = true;
		if (skill.skillTypeInfo.id != 5L)
		{
			this.CurrentWorkRoot.SetActive(true);
			this.CurrentWorkAnim.SetTrigger("Run");
			this.DescController.Display(this.GetDesc("start_0"), -1);
			this.StartWorkDesc();
		}
		this.InitProcessText(skill.skillTypeInfo.rwbpType);
		this.TurnOnRoomLight();
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x00044F3E File Offset: 0x0004313E
	public void TurnOnRoomLight()
	{
		this.RoomColor = UIColorManager.instance.GetSefiraColor(this.TargetUnit.model.sefiraOrigin).imageColor;
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x00044F65 File Offset: 0x00043165
	public void TurnOffRoomLight()
	{
		this.RoomColor = UIColorManager.instance.GetDisabledRoomColor(this.TargetUnit.model.sefiraOrigin.sefiraEnum);
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x00044F8C File Offset: 0x0004318C
	private void InitProcessText(RwbpType type)
	{
		this._currentUsedProcessText = IsolateDescManager.GetList(type);
		this._currentWorkType = type;
	}

	// Token: 0x060055A9 RID: 21929 RVA: 0x00044FA1 File Offset: 0x000431A1
	public void OnExitRoom()
	{
		this.CurrentWorkAnim.SetTrigger("Stop");
		this.StopWorkDesc();
		this.TurnOffRoomLight();
	}

	// Token: 0x060055AA RID: 21930 RVA: 0x00044FBF File Offset: 0x000431BF
	public void StartWorkDesc()
	{
		this._workDescTimer.StartTimer(this.WorkDescFreq);
	}

	// Token: 0x060055AB RID: 21931 RVA: 0x00044FD2 File Offset: 0x000431D2
	public void StopWorkDesc()
	{
		this._workDescTimer.StopTimer();
		this.DescController.Teriminate();
	}

	// Token: 0x060055AC RID: 21932 RVA: 0x00044FEA File Offset: 0x000431EA
	public void OnCurrentWorkRootAnimEnd()
	{
		this.CurrentWorkRoot.SetActive(false);
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x001EB174 File Offset: 0x001E9374
	public void ActivatedSkill()
	{
		if (this.NarrationFadeEffect.isDisplayed)
		{
			this.NarrationFadeEffect.ForceDisable();
		}
		this.SpecialSkillName.text = "< " + this._targetUnit.model.metaInfo.specialSkillName + " >";
		this.SpecialSkillRoot.SetActive(true);
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x001EB1D8 File Offset: 0x001E93D8
	public void OnOvelrayEnter(int index)
	{
		if (index == 0)
		{
			if (RabbitManager.instance.CheckUnitRabbitExecution(this.TargetUnit.model))
			{
				this.RabbitBlock.SetActive(true);
				return;
			}
			if (GlobalBulletWindow.CurrentWindow != null)
			{
				GlobalBulletWindow.CurrentWindow.isolateEntered = true;
			}
			if (!this.IsWorking)
			{
				if (this.TargetUnit.model.script.IsWorkable() && this.TargetUnit.model.script.CanEnterRoom())
				{
					this.pointerEntered = true;
				}
				if (this.IsWorkAllocated)
				{
				}
			}
			if (this.TargetUnit.model.metaInfo.creatureKitType == CreatureKitType.CHANNEL)
			{
				this.pointerEntered = true;
			}
		}
		else if (index == 1)
		{
			this.nameEntered = true;
			if (this.TargetUnit.model.IsEscapedOnlyEscape())
			{
				this.CreatureName.color = this.NameTextureColor;
				this.CreatureNameImage.color = this.NameTextColor;
			}
			else
			{
				this.CreatureName.color = this.NameTextureColor;
				this.CreatureNameImage.color = this.NameTextColor;
			}
		}
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x001EB314 File Offset: 0x001E9514
	private void CheckOverlay()
	{
		if (this.IsWorkAllocated)
		{
			if (this.pointerEntered)
			{
				this.OverlayImage.color = this.CancelColor;
				this.OverlayText.text = this.GetStateText("cancel");
				return;
			}
			this.OverlayImage.color = this.MovingColor;
			this.OverlayText.text = this.GetStateText("moving");
			return;
		}
		else
		{
			CreatureState state = this.TargetUnit.model.state;
			if (state == CreatureState.ESCAPE)
			{
				this.OverlayImage.color = this.SuppressColor;
				this.OverlayText.text = this.GetStateText("suppress");
				return;
			}
			if (this.TargetUnit.model.IsWorkCountFull())
			{
				this.OverlayImage.color = this.OrderColor;
				this.OverlayText.text = "humm";
			}
			if (state == CreatureState.WAIT)
			{
				if (this.TargetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.KIT && this.TargetUnit.model.kitEquipOwner != null)
				{
					this.OverlayImage.color = this.CancelColor;
					this.OverlayText.text = this.GetStateText("return");
					if (this.IsKitReturnning() && this.pointerEntered)
					{
						this.OverlayText.text = this.GetStateText("returnCancel");
					}
					return;
				}
				this.OverlayImage.color = this.OrderColor;
				if (this.TargetUnit.model.metaInfo.creatureWorkType == CreatureWorkType.KIT)
				{
					this.OverlayText.text = this.GetStateText("order_use");
				}
				else
				{
					this.OverlayText.text = this.GetStateText("order");
				}
				return;
			}
			else
			{
				if (state == CreatureState.WORKING)
				{
					this.OverlayImage.color = this.CancelColor;
					this.OverlayText.text = this.GetStateText("cancel_use");
					return;
				}
				return;
			}
		}
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x001EB514 File Offset: 0x001E9714
	public void OnOverlayExit(int index)
	{
		this.RabbitBlock.SetActive(false);
		this.pointerEntered = false;
		this.nameEntered = false;
		if (index == 0)
		{
			if (GlobalBulletWindow.CurrentWindow != null)
			{
				GlobalBulletWindow.CurrentWindow.isolateEntered = false;
			}
		}
		else if (index == 1)
		{
			this.CreatureName.color = this.NameTextColor;
			this.CreatureNameImage.color = this.NameTextureColor;
		}
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x00044FF8 File Offset: 0x000431F8
	public void OnObservationLevelChanged()
	{
		this.CreatureName.text = this.TargetUnit.model.GetUnitName();
		this.SetRiskLevel();
		this.SetKitObserveLevel();
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x00045021 File Offset: 0x00043221
	public void ProcessNarration(string desc)
	{
		this.WorkNarration.text = desc;
		if (this.NarrationFadeEffect.isDisplayed)
		{
			this.NarrationFadeEffect.ReEnable();
		}
		else
		{
			this.NarrationFadeEffect.Enable();
		}
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001EB58C File Offset: 0x001E978C
	public IsolateFilter AddFilter(IsolateFilter baseFilter)
	{
		GameObject gameObject = Prefab.LoadPrefab("Unit/ETC/IsolateFilterBase");
		IsolateFilter component = gameObject.GetComponent<IsolateFilter>();
		component.type = baseFilter.type;
		component.spriteRenderer.sortingLayerID = baseFilter.spriteRenderer.sortingLayerID;
		component.spriteRenderer.sortingOrder = baseFilter.spriteRenderer.sortingOrder;
		gameObject.transform.SetParent(baseFilter.transform);
		gameObject.transform.localPosition = Vector3.zero;
		this.addedFilter.Add(component);
		return component;
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x0004505A File Offset: 0x0004325A
	public void OnDamageAnimArrived()
	{
		this.DamageUIController.Hide();
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x00045067 File Offset: 0x00043267
	private string GetDesc(string id)
	{
		return LocalizeTextDataModel.instance.GetText("WorkProcess_" + id);
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x0004507E File Offset: 0x0004327E
	public void OnCancelWork()
	{
		this.IsWorkAllocated = false;
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001EB614 File Offset: 0x001E9814
	private void StartCubeAnim()
	{
		this._checkCumlatvieCubeCount = false;
		this._cubeTimer.StartTimer(0.5f);
		this.CumlativeCubeImage.GetComponent<Animator>().SetTrigger("Add");
		this._currentCumlatvieCubeCount = (int)float.Parse(this.CumlativeCubeCount.text);
		this._currentCumlatvieCubeCount++;
		this.CumlativeCubeCount.text = this._currentCumlatvieCubeCount.ToString();
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x00045087 File Offset: 0x00043287
	public void OnCubeArrived()
	{
		this._cumlativeCubeAnimRemain--;
		if (this._cumlativeCubeAnimRemain > 0)
		{
			this.StartCubeAnim();
		}
		else
		{
			this._checkCumlatvieCubeCount = true;
		}
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001EB690 File Offset: 0x001E9890
	public void OnChangeProbReduction()
	{
		if (this.TargetUnit != null)
		{
			if (this.TargetUnit.model.probReductionCounter > 0 || (float)this.TargetUnit.model.ProbReductionValue > 0f || this.TargetUnit.model.sefira.agentDeadPenaltyActivated)
			{
				float num = 0f;
				this.probReductionUI.SetBool("Enable", true);
				this.TooltipReduceProb.gameObject.SetActive(true);
				string text = string.Empty;
				if (this.TargetUnit.model.sefira.agentDeadPenaltyActivated)
				{
					num += 50f;
					text = text + "\n" + LocalizeTextDataModel.instance.GetText(this.TooltipAllAgentDead.ID);
				}
				string text2 = string.Empty;
				if ((float)this.TargetUnit.model.ProbReductionValue > 0f)
				{
					num += (float)this.TargetUnit.model.ProbReductionValue;
					this.probReductionText.text = string.Format("-{0}%", num);
					text = text + "\n" + LocalizeTextDataModel.instance.GetText(this.TooltipOverload.ID);
					text2 = string.Format(LocalizeTextDataModel.instance.GetText(this.TooltipReduceProb.ID), num);
					this.TooltipReduceProb.SetDynamicTooltip(text + text2);
					return;
				}
				num += (float)this.TargetUnit.model.GetRedusedWorkProbByCounter();
				if (this.TargetUnit.model.GetRedusedWorkProbByCounter() > 0)
				{
					text = text + "\n" + LocalizeTextDataModel.instance.GetText(this.TooltipOverload.ID);
				}
				this.probReductionText.text = string.Format("-{0}%", num);
				text2 = string.Format(LocalizeTextDataModel.instance.GetText(this.TooltipReduceProb.ID), num);
				this.TooltipReduceProb.SetDynamicTooltip(text2 + text);
			}
			else
			{
				this.probReductionUI.SetBool("Enable", false);
				this.TooltipReduceProb.gameObject.SetActive(false);
				this.probReductionText.text = string.Empty;
			}
		}
	}

	// Token: 0x04004ECA RID: 20170
	private const float _roomPosyFix = 4.4f;

	// Token: 0x04004ECB RID: 20171
	private const float _roomPosxFix = 0.31f;

	// Token: 0x04004ECC RID: 20172
	private const float _narrationPosyFix = 4.4f;

	// Token: 0x04004ECD RID: 20173
	private const float _cubeAnimFreq = 0.5f;

	// Token: 0x04004ECE RID: 20174
	private CreatureFeelingState oldState;

	// Token: 0x04004ECF RID: 20175
	private CreatureUnit _targetUnit;

	// Token: 0x04004ED0 RID: 20176
	private IsolateRoom.WorkProgress _progressBar;

	// Token: 0x04004ED1 RID: 20177
	private Timer _workDescTimer = new Timer();

	// Token: 0x04004ED2 RID: 20178
	private List<int> _currentUsedProcessText = new List<int>();

	// Token: 0x04004ED3 RID: 20179
	private RwbpType _currentWorkType = RwbpType.R;

	// Token: 0x04004ED4 RID: 20180
	private Timer _cubeTimer = new Timer();

	// Token: 0x04004ED5 RID: 20181
	private bool _isWorking;

	// Token: 0x04004ED6 RID: 20182
	private bool _isWorkAllocated;

	// Token: 0x04004ED7 RID: 20183
	private bool _isEscaped;

	// Token: 0x04004ED8 RID: 20184
	private bool _isResultDisplaying;

	// Token: 0x04004ED9 RID: 20185
	private int _currentCumlatvieCubeCount;

	// Token: 0x04004EDA RID: 20186
	private int _cumlativeCubeAnimRemain;

	// Token: 0x04004EDB RID: 20187
	public IsolatePos attachmentPos;

	// Token: 0x04004EDC RID: 20188
	[HideInInspector]
	public List<IsolateFilter> addedFilter;

	// Token: 0x04004EDD RID: 20189
	public Color RoomColor;

	// Token: 0x04004EDE RID: 20190
	public SpriteRenderer RoomSpriteRenderer;

	// Token: 0x04004EDF RID: 20191
	public float WorkDescFreq = 1f;

	// Token: 0x04004EE0 RID: 20192
	public AudioClipPlayer audioClipPlayer;

	// Token: 0x04004EE1 RID: 20193
	[Header("Filter")]
	public IsolateFilter StateFilter;

	// Token: 0x04004EE2 RID: 20194
	public IsolateFilter EscapeFilter;

	// Token: 0x04004EE3 RID: 20195
	public IsolateFilter SkillFilter;

	// Token: 0x04004EE4 RID: 20196
	public Image DefaultEscapeFilter;

	// Token: 0x04004EE5 RID: 20197
	[Header("UI Tooltips")]
	public TooltipMouseOver TooltipName;

	// Token: 0x04004EE6 RID: 20198
	public TooltipMouseOver TooltipRiskLevel;

	// Token: 0x04004EE7 RID: 20199
	public TooltipMouseOver TooltipUniquePE;

	// Token: 0x04004EE8 RID: 20200
	public TooltipMouseOver TooltipGeneratedPE;

	// Token: 0x04004EE9 RID: 20201
	public TooltipMouseOver TooltipCurrentPE;

	// Token: 0x04004EEA RID: 20202
	public TooltipMouseOver TooltipQliphoth;

	// Token: 0x04004EEB RID: 20203
	public TooltipMouseOver TooltipReduceProb;

	// Token: 0x04004EEC RID: 20204
	public TooltipMouseOver TooltipOverload;

	// Token: 0x04004EED RID: 20205
	public TooltipMouseOver TooltipAllAgentDead;

	// Token: 0x04004EEE RID: 20206
	[Header("UI Component")]
	public RectTransform TouchArea;

	// Token: 0x04004EEF RID: 20207
	public Canvas ClickIsolateArea;

	// Token: 0x04004EF0 RID: 20208
	public Canvas ClickNameArea;

	// Token: 0x04004EF1 RID: 20209
	public Image RoomFog;

	// Token: 0x04004EF2 RID: 20210
	public Text CreatureName;

	// Token: 0x04004EF3 RID: 20211
	public Image CreatureNameImage;

	// Token: 0x04004EF4 RID: 20212
	public Image RiskLevelImage;

	// Token: 0x04004EF5 RID: 20213
	public Image Frame;

	// Token: 0x04004EF6 RID: 20214
	public Image CancelWork;

	// Token: 0x04004EF7 RID: 20215
	public GameObject CurrentWorkRoot;

	// Token: 0x04004EF8 RID: 20216
	public Image WorkGear;

	// Token: 0x04004EF9 RID: 20217
	public Image CurrentWorkIcon;

	// Token: 0x04004EFA RID: 20218
	public Image CurrentWorkCubeFill;

	// Token: 0x04004EFB RID: 20219
	public Text CurrentWorkCubeText;

	// Token: 0x04004EFC RID: 20220
	public Text WorkNarration;

	// Token: 0x04004EFD RID: 20221
	public FadeEffecter NarrationFadeEffect;

	// Token: 0x04004EFE RID: 20222
	public GameObject SpecialSkillRoot;

	// Token: 0x04004EFF RID: 20223
	public Text SpecialSkillName;

	// Token: 0x04004F00 RID: 20224
	public Image CurrentResultFilter;

	// Token: 0x04004F01 RID: 20225
	public Image CurrentResultIcon;

	// Token: 0x04004F02 RID: 20226
	public Text CurrentResultCooltime;

	// Token: 0x04004F03 RID: 20227
	public Image CumlativeCubeImage;

	// Token: 0x04004F04 RID: 20228
	public Image CumlativeCrossImage;

	// Token: 0x04004F05 RID: 20229
	public Text CumlativeCubeCount;

	// Token: 0x04004F06 RID: 20230
	public GameObject CubeGridRoot;

	// Token: 0x04004F07 RID: 20231
	public GameObject SuccessCubeObject;

	// Token: 0x04004F08 RID: 20232
	public RectTransform SuccessCubeGrid;

	// Token: 0x04004F09 RID: 20233
	public GameObject FailCubeObject;

	// Token: 0x04004F0A RID: 20234
	public RectTransform FailCubeGrid;

	// Token: 0x04004F0B RID: 20235
	public Image DamageFilter;

	// Token: 0x04004F0C RID: 20236
	public Image CurrentObserveLevelFill;

	// Token: 0x04004F0D RID: 20237
	public Text CurrentObserveLevel;

	// Token: 0x04004F0E RID: 20238
	public List<MaskableGraphic> ColorSetted;

	// Token: 0x04004F0F RID: 20239
	[NonSerialized]
	private AgentModel _currentAllocateWorker;

	// Token: 0x04004F10 RID: 20240
	public List<GameObject> normalCreatureUIs;

	// Token: 0x04004F11 RID: 20241
	public List<GameObject> kitCreatureUIs;

	// Token: 0x04004F12 RID: 20242
	public Animator probReductionUI;

	// Token: 0x04004F13 RID: 20243
	public Text probReductionText;

	// Token: 0x04004F14 RID: 20244
	public IsolateDescController DescController;

	// Token: 0x04004F15 RID: 20245
	[Header("Overlay")]
	public Image OverlayImage;

	// Token: 0x04004F16 RID: 20246
	public Text OverlayText;

	// Token: 0x04004F17 RID: 20247
	public Color OrderColor;

	// Token: 0x04004F18 RID: 20248
	public Color CancelColor;

	// Token: 0x04004F19 RID: 20249
	public Color SuppressColor;

	// Token: 0x04004F1A RID: 20250
	public Color MovingColor;

	// Token: 0x04004F1B RID: 20251
	public Color NameTextureColor;

	// Token: 0x04004F1C RID: 20252
	public Color NameTextColor;

	// Token: 0x04004F1D RID: 20253
	public GameObject RabbitBlock;

	// Token: 0x04004F1E RID: 20254
	private bool pointerEntered;

	// Token: 0x04004F1F RID: 20255
	private bool nameEntered;

	// Token: 0x04004F20 RID: 20256
	[Header("EscapeCounter")]
	public GameObject CounterActiveControl;

	// Token: 0x04004F21 RID: 20257
	public GameObject CounterOutline;

	// Token: 0x04004F22 RID: 20258
	public Text CounterText;

	// Token: 0x04004F23 RID: 20259
	public Image CounterColor;

	// Token: 0x04004F24 RID: 20260
	public Image CounterInnerImage;

	// Token: 0x04004F25 RID: 20261
	public Color CounterColor_Red;

	// Token: 0x04004F26 RID: 20262
	public Color CounterColor_Black;

	// Token: 0x04004F27 RID: 20263
	public Color CounterColor_Normal;

	// Token: 0x04004F28 RID: 20264
	public Color CounterColor_White;

	// Token: 0x04004F29 RID: 20265
	public Sprite CounterImage_Unknown;

	// Token: 0x04004F2A RID: 20266
	public Sprite CounterImage_None;

	// Token: 0x04004F2B RID: 20267
	private bool _counterObserved;

	// Token: 0x04004F2C RID: 20268
	private bool _counterEnabled;

	// Token: 0x04004F2D RID: 20269
	public Sprite NormalCubeSprite;

	// Token: 0x04004F2E RID: 20270
	public Sprite EscapeCubeSprite;

	// Token: 0x04004F2F RID: 20271
	[Header("WorkCounter")]
	public GameObject WorkCounterActiveControl;

	// Token: 0x04004F30 RID: 20272
	public RectTransform WorkCounterRayout;

	// Token: 0x04004F31 RID: 20273
	public List<Image> WorkCounterImage;

	// Token: 0x04004F32 RID: 20274
	public GameObject WorkCounterUnit;

	// Token: 0x04004F33 RID: 20275
	public GameObject MaxWorkCountFilter;

	// Token: 0x04004F34 RID: 20276
	public GameObject MaxWorkCountLine;

	// Token: 0x04004F35 RID: 20277
	public Text MaxWorkText_Upper;

	// Token: 0x04004F36 RID: 20278
	public Text MaxWorkText_Lower;

	// Token: 0x04004F37 RID: 20279
	public Text workCount;

	// Token: 0x04004F38 RID: 20280
	public Color WorkCount_Enabled;

	// Token: 0x04004F39 RID: 20281
	public Color WorkCount_Disabled;

	// Token: 0x04004F3A RID: 20282
	[Header("Overload")]
	public IsolateOverload overloadUI;

	// Token: 0x04004F3B RID: 20283
	[NonSerialized]
	public BinahOverloadUI binahOverloadUI;

	// Token: 0x04004F3C RID: 20284
	[Header("KitCreatureObserveLevel")]
	public GameObject KitObserveRoot;

	// Token: 0x04004F3D RID: 20285
	public Text[] KitObserveLevel;

	// Token: 0x04004F3E RID: 20286
	public Color KitNormal;

	// Token: 0x04004F3F RID: 20287
	public Color KitDisable;

	// Token: 0x04004F40 RID: 20288
	private int oldCount;

	// Token: 0x04004F41 RID: 20289
	private bool _checkCumlatvieCubeCount = true;

	// Token: 0x04004F42 RID: 20290
	private bool _maxWorkcountReached;

	// Token: 0x04004F43 RID: 20291
	private PointerEventData.InputButton _targetClick = PointerEventData.InputButton.Middle;

	// Token: 0x02000B13 RID: 2835
	public enum OverlayRegion
	{
		// Token: 0x04004F46 RID: 20294
		ROOM_MAIN,
		// Token: 0x04004F47 RID: 20295
		CREATURENAME
	}

	// Token: 0x02000B14 RID: 2836
	public class WorkProgress
	{
		// Token: 0x060055BB RID: 21947 RVA: 0x001EB8E0 File Offset: 0x001E9AE0
		public WorkProgress(GameObject root, RectTransform Success, RectTransform Fail, Text CubeText, Image CubeTextFill)
		{
			this.ActiveContorl = root;
			this.SuccessParent = Success;
			this.FailParent = Fail;
			this.SuccessLayout = Success.GetComponent<GridLayoutGroup>();
			this.FailLayout = Fail.GetComponent<GridLayoutGroup>();
			this.CurrentTotalCube = CubeText;
			this.CubeTextFill = CubeTextFill;
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x001EB930 File Offset: 0x001E9B30
		public void Init(CreatureModel creature, GameObject success, GameObject fail)
		{
			int lastBound = creature.metaInfo.feelingStateCubeBounds.GetLastBound();
			this._max = lastBound;
			float x = this.SuccessParent.sizeDelta.x;
			float y = this.SuccessParent.sizeDelta.y;
			float num = y - (float)(lastBound - 1) * 30f;
			float y2 = num / (float)lastBound;
			Vector2 cellSize = this.SuccessLayout.cellSize;
			cellSize.y = y2;
			this.SuccessLayout.cellSize = cellSize;
			this.FailLayout.cellSize = cellSize;
			this.ActiveContorl.SetActive(false);
			this.SuccessObject = new List<GameObject>();
			this.FailObject = new List<GameObject>();
			for (int i = 0; i < lastBound; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(success);
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(fail);
				this.InitSetting(gameObject, this.SuccessParent);
				this.InitSetting(gameObject2, this.FailParent);
				this.SuccessObject.Add(gameObject);
				this.FailObject.Add(gameObject2);
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
			}
			this.CurrentTotalCube.text = "0";
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x000450BD File Offset: 0x000432BD
		private void InitSetting(GameObject target, Transform parent)
		{
			target.transform.SetParent(parent);
			target.transform.localScale = Vector3.one;
			target.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x060055BE RID: 21950 RVA: 0x001EBA64 File Offset: 0x001E9C64
		public void InitializeState()
		{
			this._index = (this._failIndex = (this._successIndex = 0));
			for (int i = 0; i < this._max; i++)
			{
				this.SuccessObject[i].SetActive(false);
				this.FailObject[i].SetActive(false);
			}
			this.CurrentTotalCube.text = "0";
			Color white = Color.white;
			Color white2 = Color.white;
			CreatureLayer.IsolateRoomUIData.GetGeneratedEnergyColor(0, out white, out white2);
			this.CurrentTotalCube.color = white2;
			this.CubeTextFill.color = white;
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x001EBB08 File Offset: 0x001E9D08
		public void AddBar(bool isSuccess)
		{
			if (this._index >= this._max)
			{
				return;
			}
			this._index++;
			if (isSuccess)
			{
				this.SuccessObject[this._successIndex++].SetActive(true);
			}
			else
			{
				this.FailObject[this._failIndex++].SetActive(true);
			}
			int successIndex = this._successIndex;
			this.CurrentTotalCube.text = this.GenerateText(successIndex);
			Color white = Color.white;
			Color white2 = Color.white;
			CreatureLayer.IsolateRoomUIData.GetGeneratedEnergyColor(successIndex, out white, out white2);
			this.CurrentTotalCube.color = white2;
			this.CubeTextFill.color = white;
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x001EBBD0 File Offset: 0x001E9DD0
		public string GenerateText(int value)
		{
			string empty = string.Empty;
			string arg = string.Empty;
			if (value < 0)
			{
				arg = "-";
			}
			else if (value > 0)
			{
				arg = "+";
			}
			return arg + Mathf.Abs(value);
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x000450EB File Offset: 0x000432EB
		public string GetResultText()
		{
			return this.GenerateText(this._successIndex - this._failIndex);
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x00045100 File Offset: 0x00043300
		public void SetVisible(bool state)
		{
			this.ActiveContorl.SetActive(state);
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x0004510E File Offset: 0x0004330E
		public int GetResultCount()
		{
			if (this._failIndex >= this._successIndex)
			{
				return 0;
			}
			return this._successIndex;
		}

		// Token: 0x04004F48 RID: 20296
		private const float _spacing = 30f;

		// Token: 0x04004F49 RID: 20297
		public GameObject ActiveContorl;

		// Token: 0x04004F4A RID: 20298
		public RectTransform SuccessParent;

		// Token: 0x04004F4B RID: 20299
		public RectTransform FailParent;

		// Token: 0x04004F4C RID: 20300
		public GridLayoutGroup SuccessLayout;

		// Token: 0x04004F4D RID: 20301
		public GridLayoutGroup FailLayout;

		// Token: 0x04004F4E RID: 20302
		public Text CurrentTotalCube;

		// Token: 0x04004F4F RID: 20303
		public Image CubeTextFill;

		// Token: 0x04004F50 RID: 20304
		public List<GameObject> SuccessObject;

		// Token: 0x04004F51 RID: 20305
		public List<GameObject> FailObject;

		// Token: 0x04004F52 RID: 20306
		private int _max;

		// Token: 0x04004F53 RID: 20307
		private int _failIndex;

		// Token: 0x04004F54 RID: 20308
		private int _successIndex;

		// Token: 0x04004F55 RID: 20309
		private int _index;
	}
}
