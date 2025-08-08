using System;
using System.Collections;
using System.Collections.Generic;
using CreatureInfo;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AE2 RID: 2786
public class CreatureInfoWindow : MonoBehaviour
{
	// Token: 0x06005409 RID: 21513 RVA: 0x001E8AB0 File Offset: 0x001E6CB0
	public CreatureInfoWindow()
	{
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x0600540A RID: 21514 RVA: 0x000443D0 File Offset: 0x000425D0
	// (set) Token: 0x0600540B RID: 21515 RVA: 0x000443D7 File Offset: 0x000425D7
	public static CreatureInfoWindow CurrentWindow
	{
		get
		{
			return CreatureInfoWindow._currentWindow;
		}
		private set
		{
			CreatureInfoWindow._currentWindow = value;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x0600540C RID: 21516 RVA: 0x000443DF File Offset: 0x000425DF
	// (set) Token: 0x0600540D RID: 21517 RVA: 0x001E8B0C File Offset: 0x001E6D0C
	public long CurrentMetaId
	{
		get
		{
			return this._currentCreatureMetaId;
		}
		private set
		{
			this._currentCreatureMetaId = value;
			if (value == -1L)
			{
				this._metaInfo = null;
				this._observeInfo = null;
				this._currentModel = null;
			}
			else
			{
				this._metaInfo = CreatureTypeList.instance.GetData(value);
				this._observeInfo = CreatureManager.instance.GetObserveInfo(value);
				this._currentModel = CreatureManager.instance.FindCreature(value);
			}
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600540E RID: 21518 RVA: 0x000443E7 File Offset: 0x000425E7
	public CreatureTypeInfo MetaInfo
	{
		get
		{
			return this._metaInfo;
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x0600540F RID: 21519 RVA: 0x000443EF File Offset: 0x000425EF
	public CreatureObserveInfoModel ObserveInfo
	{
		get
		{
			return this._observeInfo;
		}
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x06005410 RID: 21520 RVA: 0x000443F7 File Offset: 0x000425F7
	public CreatureModel CurrentModel
	{
		get
		{
			return this._currentModel;
		}
	}

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x06005411 RID: 21521 RVA: 0x000443FF File Offset: 0x000425FF
	// (set) Token: 0x06005412 RID: 21522 RVA: 0x001E8B78 File Offset: 0x001E6D78
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
		private set
		{
			if (value)
			{
				this.SetActive(true);
				this.OpenEffect();
				this.WindowAnimCTRL.Show();
				if (CameraMover.instance)
				{
					CameraMover.instance.StopMove();
				}
			}
			else
			{
				this.WindowAnimCTRL.Hide();
				if (CameraMover.instance)
				{
					CameraMover.instance.ReleaseMove();
				}
			}
		}
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06005413 RID: 21523 RVA: 0x00044407 File Offset: 0x00042607
	// (set) Token: 0x06005414 RID: 21524 RVA: 0x0004440F File Offset: 0x0004260F
	public bool IsCodex { get; private set; }

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06005415 RID: 21525 RVA: 0x00044418 File Offset: 0x00042618
	private GameObject _descriptionActiveControl
	{
		get
		{
			return this.DescriptionPanel.gameObject;
		}
	}

	// Token: 0x06005416 RID: 21526 RVA: 0x001E8BE8 File Offset: 0x001E6DE8
	public static CreatureInfoWindow CreateWindow(long metaId)
	{
		CreatureInfoWindow.CurrentWindow.IsCodex = false;
		CreatureInfoWindow.CurrentWindow.SetWindowType(false);
		CreatureInfoWindow.CurrentWindow.CurrentMetaId = metaId;
		CreatureInfoWindow.CurrentWindow.IsEnabled = true;
		CreatureInfoWindow.CurrentWindow.OnChangeCreature();
		try
		{
			CreatureInfoWindow.CurrentWindow.InfoCodexArrowRoot.SetActive(false);
		}
		catch (Exception ex)
		{
		}
		return CreatureInfoWindow.CurrentWindow;
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x001E8C5C File Offset: 0x001E6E5C
	public static CreatureInfoWindow CreateCodexWindow()
	{
		CreatureInfoWindow.CurrentWindow.IsCodex = true;
		CreatureInfoWindow.CurrentWindow.SetWindowType(true);
		CreatureInfoWindow.CurrentWindow.CurrentMetaId = -1L;
		CreatureInfoWindow.CurrentWindow.IsEnabled = true;
		CreatureInfoWindow.CurrentWindow.codex.OnOpen();
		try
		{
			CreatureInfoWindow.CurrentWindow.InfoCodexArrowRoot.SetActive(false);
		}
		catch (Exception ex)
		{
		}
		return CreatureInfoWindow.CurrentWindow;
	}

	// Token: 0x06005418 RID: 21528 RVA: 0x00044425 File Offset: 0x00042625
	public static bool IsCurrentMetaNull()
	{
		return CreatureInfoWindow.CurrentWindow.CurrentMetaId == -1L;
	}

	// Token: 0x06005419 RID: 21529 RVA: 0x00044435 File Offset: 0x00042635
	public static bool IsCurrentModelNull()
	{
		return CreatureInfoWindow.CurrentWindow._currentModel == null;
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x00044444 File Offset: 0x00042644
	private void Awake()
	{
		CreatureInfoWindow._currentWindow = this;
		this.CurrentMetaId = -1L;
	}

	// Token: 0x0600541B RID: 21531 RVA: 0x001E8CD8 File Offset: 0x001E6ED8
	private void OpenEffect()
	{
		Vector2 one = Vector2.one;
		try
		{
			if (GameManager.currentGameManager != null)
			{
				Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
				Vector2 vector2 = Input.mousePosition;
				one = new Vector2(vector2.x / vector.x, vector2.y / vector.y);
			}
		}
		catch (Exception ex)
		{
		}
		RectTransform component = this.WindowAnimCTRL.GetComponent<RectTransform>();
		RectTransform rectTransform = component;
		Vector2 vector3 = one;
		component.pivot = vector3;
		vector3 = vector3;
		component.anchorMax = vector3;
		rectTransform.anchorMin = vector3;
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001E8D88 File Offset: 0x001E6F88
	private void Start()
	{
		this.WindowAnimCTRL.onHide.AddListener(delegate()
		{
			this.SetActive(false);
		});
		this.InfoPanel.SetActive(true);
		this._descriptionActiveControl.SetActive(false);
		this.InfoButton.interactable = false;
		this.DescButton.interactable = true;
		this.statRoot.SetArea(CreatureModel.regionName[0]);
		this.escapeRoot.SetArea(CreatureModel.regionName[1]);
		this.workSlots[0].SetArea(CreatureModel.regionName[2]);
		this.workSlots[1].SetArea(CreatureModel.regionName[3]);
		this.workSlots[2].SetArea(CreatureModel.regionName[4]);
		this.workSlots[3].SetArea(CreatureModel.regionName[5]);
		this.caretakingRoot.SetSlotAreaName(CreatureModel.careTakingRegion);
		this._controllers.Add(this.statRoot);
		this._controllers.Add(this.escapeRoot);
		for (int i = 0; i < 4; i++)
		{
			CreatureInfoWorkSlot creatureInfoWorkSlot = this.workSlots[i];
			creatureInfoWorkSlot.SetRWBPType(i + RwbpType.R);
			this._controllers.Add(creatureInfoWorkSlot);
		}
		foreach (CreatureInfoCaretakingSlot creatureInfoCaretakingSlot in this.caretakingRoot.slots)
		{
			this._controllers.Add(creatureInfoCaretakingSlot);
			creatureInfoCaretakingSlot.Open.UniqueBlockedText = true;
		}
		this._isEnabled = false;
		if (this.codex != null)
		{
			this.codex.Init();
		}
		this.SetActive(false);
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001E8F5C File Offset: 0x001E715C
	public void SetWindowType(bool isCodex)
	{
		try
		{
			this.TitleText.gameObject.SetActive(!isCodex);
		}
		catch (Exception ex)
		{
		}
		try
		{
			this.normalCreatureArea.SetActive(!isCodex);
		}
		catch (Exception ex2)
		{
		}
		try
		{
			this.kitCreatureArea.SetActive(!isCodex);
		}
		catch (Exception ex3)
		{
		}
		try
		{
			this.codex._activeControl.SetActive(isCodex);
			this.CodexFrame.color = ((!isCodex) ? this.BrightColor : this.RedColor);
		}
		catch (Exception ex4)
		{
		}
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x00044454 File Offset: 0x00042654
	public void OpenCodexCreatureInfo(CreatureTypeInfo metaInfo)
	{
		this.InfoCodexArrowRoot.SetActive(true);
		this.SetWindowType(false);
		this.CurrentMetaId = metaInfo.id;
		this.OnChangeCreature();
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x0004447B File Offset: 0x0004267B
	public void OpenCodexCreatureInfo(long metaId)
	{
		this.InfoCodexArrowRoot.SetActive(true);
		this.SetWindowType(false);
		this.CurrentMetaId = metaId;
		this.OnChangeCreature();
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001E9030 File Offset: 0x001E7230
	public void SetDesc(int level)
	{
		CreatureTypeInfo.ObserveTable observeTable = this.MetaInfo.observeTable;
		int count = observeTable.desc.Count;
		char[] separator = new char[]
		{
			'*'
		};
		new List<string>();
		this.descList.Clear();
		IEnumerator enumerator = this.listParent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
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
		float num = 0f;
		string unitName = CreatureModel.GetUnitName(this.MetaInfo, this.ObserveInfo);
		for (int i = 0; i < count; i++)
		{
			if (level >= observeTable.desc[i])
			{
				foreach (string text in this.MetaInfo.desc[i].Split(separator))
				{
					if (!text.Equals(" ") && !text.Equals(string.Empty))
					{
						string text2 = text.Replace("$0", unitName);
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.descUnit);
						gameObject.transform.SetParent(this.listParent);
						gameObject.transform.localScale = Vector3.one;
						Text component = gameObject.GetComponent<Text>();
						this.descList.Add(component);
						component.text = text2;
					}
				}
			}
		}
		for (int k = 0; k < this.descList.Count; k++)
		{
			Text text3 = this.descList[k];
			text3.rectTransform.anchoredPosition = new Vector2(0f, -num);
			num += text3.preferredHeight + this.Spacing;
		}
		num += this.LowerSpacing;
		this.listParent.sizeDelta = new Vector2(this.listParent.sizeDelta.x, num);
		this.listParent.anchoredPosition = Vector2.zero;
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001E9248 File Offset: 0x001E7448
	private void Update()
	{
		if (this.IsEnabled)
		{
			if (Input.GetKeyDown(KeyCode.Escape) && GlobalGameManager.instance.gameMode != GameMode.TUTORIAL)
			{
				this.CloseWindow();
			}
			if (this.IsCodex && !this.codex._activeControl.activeInHierarchy)
			{
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					this.codex.MoveNext();
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					this.codex.MovePrev();
				}
			}
		}
		if (!CreatureInfoWindow.IsCurrentModelNull() && this.IsEnabled)
		{
			this.SetCost();
		}
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x0004449D File Offset: 0x0004269D
	private void SetCost()
	{
		this.CurrentCumlativeCube_Cost.text = this.ObserveInfo.cubeNum + string.Empty;
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001E92F8 File Offset: 0x001E74F8
	public void OnClickDescButton()
	{
		this.InfoPanel.SetActive(false);
		this._descriptionActiveControl.SetActive(true);
		this.InfoButton.interactable = true;
		this.DescButton.interactable = false;
		this.DescButton.OnPointerExit(null);
		this.InfoButton.OnPointerExit(null);
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001E9350 File Offset: 0x001E7550
	public void OnClickInfoButton()
	{
		this.InfoPanel.SetActive(true);
		this._descriptionActiveControl.SetActive(false);
		this.InfoButton.interactable = false;
		this.DescButton.interactable = true;
		this.InfoButton.OnPointerExit(null);
		this.DescButton.OnPointerExit(null);
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x001E93A8 File Offset: 0x001E75A8
	private void OnChangeCreature()
	{
		if (this.MetaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			this.normalCreatureArea.SetActive(true);
			this.kitCreatureArea.SetActive(false);
			this._costTable.Clear();
			foreach (CreatureInfoController creatureInfoController in this._controllers)
			{
				creatureInfoController.Initialize();
			}
			int observationLevel = this.ObserveInfo.GetObservationLevel();
			this.ObserveLevelImage.sprite = this.ObserveLevelSprite[observationLevel];
			this.OnObserveLevelChanged(observationLevel);
			this.SetCost();
		}
		else
		{
			this.normalCreatureArea.SetActive(false);
			this.kitCreatureArea.SetActive(true);
			this.kitStatRoot.Initialize();
			this.kitLayerController.SetDataList(this.MetaInfo, this.ObserveInfo);
			int observeLevel = Math.Min(4, this.GetObservationLevel());
			this.OnObserveLevelChanged_kit(observeLevel);
		}
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x000444C4 File Offset: 0x000426C4
	private int GetObservationLevel()
	{
		if (CreatureInfoWindow.IsCurrentMetaNull())
		{
			return 0;
		}
		return this.ObserveInfo.GetObservationLevel();
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x000444DD File Offset: 0x000426DD
	public void SetActive(bool state)
	{
		this._isEnabled = state;
		this.RootCanvas.gameObject.SetActive(state);
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001E94B8 File Offset: 0x001E76B8
	public void CloseWindow()
	{
		if (this.IsCodex && !this.codex._activeControl.activeInHierarchy)
		{
			this.SetWindowType(true);
			this.InfoCodexArrowRoot.SetActive(false);
			this.codex.OnMetaClose();
			this.codex.OnOpen();
			this.CurrentMetaId = -1L;
			return;
		}
		this.CurrentMetaId = -1L;
		if (AlterTitleController.Controller)
		{
			AlterTitleController.Controller.InitEffect();
		}
		this.IsEnabled = false;
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x001E9540 File Offset: 0x001E7740
	public bool OnTryPurchase(CreatureInfoController controller)
	{
		if (CreatureInfoWindow.IsCurrentMetaNull() || CreatureInfoWindow.IsCurrentModelNull())
		{
			return false;
		}
		int observeCost = this.ObserveInfo.GetObserveCost(controller.AreaName);
		if (!this.CurrentModel.CanPurhcase(observeCost))
		{
			return false;
		}
		if (controller.IsOpened())
		{
			return false;
		}
		if (this.CurrentModel.TransactionCube(observeCost))
		{
			controller.OnPurchase();
			this.ObserveInfo.OnObserveRegion(controller.AreaName);
			int observationLevel = this.ObserveInfo.GetObservationLevel();
			if (this._oldLevel != observationLevel)
			{
				this.OnObserveLevelChanged(observationLevel);
				this.CurrentModel.AddObservationLevel();
			}
			this.CurrentPayedCost.text = observeCost + string.Empty;
			this.PaymentAnimCTRL.SetTrigger("Payment");
			this.SetCost();
		}
		return true;
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x000444F7 File Offset: 0x000426F7
	public void PurchaseAnim(int value)
	{
		this.CurrentPayedCost.text = value + string.Empty;
		this.PaymentAnimCTRL.SetTrigger("Payment");
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x001E961C File Offset: 0x001E781C
	private void OnObserveLevelChanged(int observeLevel)
	{
		this._oldLevel = observeLevel;
		if (this._oldLevel == 0)
		{
			this.ObserveLevelImage.enabled = false;
		}
		else
		{
			this.ObserveLevelImage.enabled = true;
			this.ObserveLevelImage.sprite = this.ObserveLevelSprite[this._oldLevel];
		}
		for (int i = 0; i < this.observeLevelSlot.Count; i++)
		{
			try
			{
				CreatureInfoObserveLevelEffectSlot creatureInfoObserveLevelEffectSlot = this.observeLevelSlot[i];
				CreatureObserveBonusData creatureObserveBonusData = this.MetaInfo.observeBonus.bonusList[i];
				string text = string.Empty;
				string arg = string.Empty;
				int value = creatureObserveBonusData.value;
				if (creatureObserveBonusData.bonus == CreatureObserveBonusData.BonusType.PROB)
				{
					arg = LocalizeTextDataModel.instance.GetText("CreatureInfo_ObserveBounus_Prob");
					text = string.Format("{0} +{1}%", arg, value);
				}
				else
				{
					arg = LocalizeTextDataModel.instance.GetText("CreatureInfo_ObserveBounus_Speed");
					text = string.Format("{0} +{1}", arg, value);
				}
				creatureInfoObserveLevelEffectSlot.SetText(text);
				if (i < this._oldLevel)
				{
					creatureInfoObserveLevelEffectSlot.SetState(true);
				}
				else
				{
					creatureInfoObserveLevelEffectSlot.SetState(false);
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"observe bonus of ",
					this.MetaInfo.name,
					" missing :",
					i
				}));
			}
		}
		if (this.ChallangeModeText != null && this.ChallangeModeAsterisk != null)
		{
			if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
			{
				this.ChallangeModeText.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_challange");
				this.ChallangeModeAsterisk.gameObject.SetActive(true);
				this.ChallangeModeText.gameObject.SetActive(true);
			}
			else if (GlobalGameManager.instance.gameMode == GameMode.STORY_MODE)
			{
				this.ChallangeModeAsterisk.gameObject.SetActive(false);
				this.ChallangeModeText.gameObject.SetActive(false);
			}
		}
		this.caretakingRoot.Initialize();
		this.equipmentRoot.Initialize();
		this.statRoot.Initialize();
		this.SetDesc(this._oldLevel);
		this.CurrentUsableCost.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("CreatureInfo_CurrentObserveLevel"), this._oldLevel);
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x001E98A0 File Offset: 0x001E7AA0
	private void OnObserveLevelChanged_kit(int observeLevel)
	{
		for (int i = 0; i < this.observeLevelSlot.Count; i++)
		{
			if (observeLevel <= i)
			{
				this.kitObserveLevelSlot[i].SetState(false);
			}
			else
			{
				this.kitObserveLevelSlot[i].SetState(true);
			}
		}
		this.kitObserveLevelText.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_CurrentObserveLevel") + " " + observeLevel;
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x00044524 File Offset: 0x00042724
	public void OnBinahAbilityChanged()
	{
		if (!this.IsEnabled && !CreatureInfoWindow.IsCurrentMetaNull() && this.MetaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			this.equipmentRoot.Initialize();
		}
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x00044556 File Offset: 0x00042756
	public bool GetCost(CreatureInfoController ctrl, out int cost)
	{
		cost = 3;
		return true;
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x001E9924 File Offset: 0x001E7B24
	static CreatureInfoWindow()
	{
	}

	// Token: 0x04004D51 RID: 19793
	public const long EmptyId = -1L;

	// Token: 0x04004D52 RID: 19794
	private static string[] tableName = new string[]
	{
		"stat",
		"work",
		"escape",
		"caretaking",
		"equipment",
		"work1",
		"work2",
		"work3",
		"work4",
		"care1",
		"care2",
		"care3",
		"care4",
		"care5",
		"care6",
		"care7"
	};

	// Token: 0x04004D53 RID: 19795
	private static CreatureInfoWindow _currentWindow;

	// Token: 0x04004D54 RID: 19796
	private long _currentCreatureMetaId = -1L;

	// Token: 0x04004D55 RID: 19797
	private CreatureTypeInfo _metaInfo;

	// Token: 0x04004D56 RID: 19798
	private CreatureObserveInfoModel _observeInfo;

	// Token: 0x04004D57 RID: 19799
	[NonSerialized]
	private CreatureModel _currentModel;

	// Token: 0x04004D58 RID: 19800
	private bool _isEnabled;

	// Token: 0x04004D5A RID: 19802
	[Header("Default Inspector")]
	public Canvas RootCanvas;

	// Token: 0x04004D5B RID: 19803
	[Space(10f)]
	[SerializeField]
	private CreatureInfoWindow.UI _UI;

	// Token: 0x04004D5C RID: 19804
	[Header("Creature Area")]
	public GameObject normalCreatureArea;

	// Token: 0x04004D5D RID: 19805
	public GameObject kitCreatureArea;

	// Token: 0x04004D5E RID: 19806
	[Space(10f)]
	public AudioClipPlayer audioClipPlayer;

	// Token: 0x04004D5F RID: 19807
	public Button InfoButton;

	// Token: 0x04004D60 RID: 19808
	public Button DescButton;

	// Token: 0x04004D61 RID: 19809
	public Text CurrentCumlativeCube_Cost;

	// Token: 0x04004D62 RID: 19810
	public UIController WindowAnimCTRL;

	// Token: 0x04004D63 RID: 19811
	public Text CurrentPayedCost;

	// Token: 0x04004D64 RID: 19812
	public Animator PaymentAnimCTRL;

	// Token: 0x04004D65 RID: 19813
	[Header("DefaultData")]
	public List<CreatureInfoObserveLevelEffectSlot> observeLevelSlot;

	// Token: 0x04004D66 RID: 19814
	public Text CurrentUsableCost;

	// Token: 0x04004D67 RID: 19815
	public Image ObserveLevelImage;

	// Token: 0x04004D68 RID: 19816
	public Text ChallangeModeAsterisk;

	// Token: 0x04004D69 RID: 19817
	public Text ChallangeModeText;

	// Token: 0x04004D6A RID: 19818
	[Header("Controllers")]
	public CreatureInfoStatRoot statRoot;

	// Token: 0x04004D6B RID: 19819
	public CreatureInfoWorkRoot workRoot;

	// Token: 0x04004D6C RID: 19820
	public CreatureInfoEscapeRoot escapeRoot;

	// Token: 0x04004D6D RID: 19821
	public CreatureInfoCaretakingRoot caretakingRoot;

	// Token: 0x04004D6E RID: 19822
	public CreatureInfoEquipmentRoot equipmentRoot;

	// Token: 0x04004D6F RID: 19823
	public List<CreatureInfoWorkSlot> workSlots;

	// Token: 0x04004D70 RID: 19824
	[Header("KitCreatureData")]
	public CreatureInfoKitStatRoot kitStatRoot;

	// Token: 0x04004D71 RID: 19825
	public CreatureInfoKitLayoutController kitLayerController;

	// Token: 0x04004D72 RID: 19826
	public List<CreatureInfoKitObserveLevelEffectSlot> kitObserveLevelSlot;

	// Token: 0x04004D73 RID: 19827
	public Text kitObserveLevelText;

	// Token: 0x04004D74 RID: 19828
	[Header("UI Data")]
	public Sprite DisabledCubeImage;

	// Token: 0x04004D75 RID: 19829
	public Sprite EnabledCubeImage;

	// Token: 0x04004D76 RID: 19830
	public Color DisabledTextColor;

	// Token: 0x04004D77 RID: 19831
	public Color EnabledTextColor;

	// Token: 0x04004D78 RID: 19832
	public Color RedColor;

	// Token: 0x04004D79 RID: 19833
	public Color BrightColor;

	// Token: 0x04004D7A RID: 19834
	public Color OrangeColor;

	// Token: 0x04004D7B RID: 19835
	public Sprite[] ObserveLevelSprite;

	// Token: 0x04004D7C RID: 19836
	[Header("Description Panel")]
	public RectTransform DescriptionPanel;

	// Token: 0x04004D7D RID: 19837
	public GameObject InfoPanel;

	// Token: 0x04004D7E RID: 19838
	[Header("Description")]
	public RectTransform listParent;

	// Token: 0x04004D7F RID: 19839
	public GameObject descUnit;

	// Token: 0x04004D80 RID: 19840
	public float Spacing = 50f;

	// Token: 0x04004D81 RID: 19841
	public float LowerSpacing = 200f;

	// Token: 0x04004D82 RID: 19842
	[Header("Codex")]
	public CreatureInfoCodex codex;

	// Token: 0x04004D83 RID: 19843
	public Image CodexFrame;

	// Token: 0x04004D84 RID: 19844
	public GameObject InfoCodexArrowRoot;

	// Token: 0x04004D85 RID: 19845
	public Button PrevCodex;

	// Token: 0x04004D86 RID: 19846
	public Button NextCodex;

	// Token: 0x04004D87 RID: 19847
	public Text TitleText;

	// Token: 0x04004D88 RID: 19848
	[NonSerialized]
	public List<Text> descList = new List<Text>();

	// Token: 0x04004D89 RID: 19849
	private List<CreatureInfoController> _controllers = new List<CreatureInfoController>();

	// Token: 0x04004D8A RID: 19850
	private Dictionary<CreatureInfoController, int> _costTable = new Dictionary<CreatureInfoController, int>();

	// Token: 0x04004D8B RID: 19851
	private int _oldLevel = -1;

	// Token: 0x02000AE3 RID: 2787
	[Serializable]
	public class UI
	{
		// Token: 0x06005431 RID: 21553 RVA: 0x00044565 File Offset: 0x00042765
		public UI()
		{
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x00044578 File Offset: 0x00042778
		public void SetCreature(CreatureModel creature)
		{
			this.temp = creature.GetUnitName();
		}

		// Token: 0x04004D8C RID: 19852
		public string temp = string.Empty;
	}
}
