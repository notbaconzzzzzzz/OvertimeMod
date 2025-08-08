using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // 

// Token: 0x02000AED RID: 2797
public class EscapeUI : MonoBehaviour
{
	// Token: 0x060054B7 RID: 21687 RVA: 0x00004644 File Offset: 0x00002844
	public EscapeUI()
	{
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x060054B8 RID: 21688 RVA: 0x00044945 File Offset: 0x00042B45
	public static EscapeUI instance
	{
		get
		{
			return EscapeUI._instance;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x060054B9 RID: 21689 RVA: 0x0004494C File Offset: 0x00042B4C
	public bool IsOpened
	{
		get
		{
			return this._isOpened;
		}
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x001EEA8C File Offset: 0x001ECC8C
	private void Start()
	{
		if (BgmManager.instance == null)
		{
			this.MasterVolume.value = GlobalGameManager.instance.sceneDataSaver.currentVolume;
			this.MusicVolume.value = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
			StoryBgm.instance.SetMasterVolume(this.MasterVolume.value);
			StoryBgm.instance.SetBgmVolume(this.MusicVolume.value);
		}
		this.OnCloseWindow();
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x00044954 File Offset: 0x00042B54
	private void Awake()
	{ // <Mod>
		if (EscapeUI.instance != null && EscapeUI.instance.gameObject)
		{
			UnityEngine.Object.Destroy(EscapeUI.instance.gameObject);
		}
		EscapeUI._instance = this;
        if (SpecialModeConfig.instance.GetValue<bool>("Blind"))
        {
            try
            {
                LobotomyBaseMod.ModDebug.Debug_Log("Blinder Parent : " + ActiveControl.transform.parent.name);
                foreach (Component component in ActiveControl.transform.parent.GetComponents<Component>())
				{
					LobotomyBaseMod.ModDebug.Debug_Log(component.GetType().ToString());
				}
                LobotomyBaseMod.ModDebug.Debug_Log("Blinder Parent Parent : " + ActiveControl.transform.parent.parent.name);
                foreach (Component component in ActiveControl.transform.parent.parent.GetComponents<Component>())
				{
					LobotomyBaseMod.ModDebug.Debug_Log(component.GetType().ToString());
				}
                LobotomyBaseMod.ModDebug.Debug_Log("ActiveControl : " + ActiveControl.transform.parent.parent.name);
                foreach (Component component in ActiveControl.GetComponents<Component>())
				{
					LobotomyBaseMod.ModDebug.Debug_Log(component.GetType().ToString());
				}
                
                GameObject gameObject = new GameObject("Blinder");
                Image image = gameObject.AddComponent<Image>();
                Canvas canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				//canvas.worldCamera = ActiveControl.GetComponent<Canvas>().worldCamera;
				canvas.planeDistance = ActiveControl.GetComponent<Canvas>().planeDistance;
                CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
				//canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                //image.transform.SetParent(ActiveControl.transform.parent.parent);
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/AssetDump/Blinder.png"));
                Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                gameObject.layer = ActiveControl.layer;
				//image.transform.position = new Vector3(0f, 0f, 0f);
				//image.transform.localScale = new Vector3(20f, 20f, 1f);
				//image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height) / 2f;
				/*
                image.rectTransform.sizeDelta = new Vector2(1f, 1f);
                image.rectTransform.anchorMin = new Vector2(0f, 0f);
                image.rectTransform.anchorMax = new Vector2(1f, 1f);
				image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				image.preserveAspect = false;*/
                gameObject.SetActive(true);
                LobotomyBaseMod.ModDebug.Debug_Log("Blinder Position : " + gameObject.transform.position.ToString() + " : " + gameObject.transform.localScale.ToString());
                /*
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/AssetDump/Blinder.png"));
                Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                
                Image image = ActiveControl.transform.parent.parent.gameObject.AddComponent<Image>();
                image.sprite = sprite;
                Image image2 = ActiveControl.transform.parent.gameObject.AddComponent<Image>();
                image2.sprite = sprite;
                Image image3 = ActiveControl.AddComponent<Image>();
                image3.sprite = sprite;*/
            }
            catch
            {
                LobotomyBaseMod.ModDebug.Debug_LogError("Blinder Error");
            }
        }
	}

	// Token: 0x060054BC RID: 21692 RVA: 0x0004498F File Offset: 0x00042B8F
	public static void OpenWindow()
	{
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			return;
		}
		EscapeUI.instance.OnOpenWindow();
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x000449AC File Offset: 0x00042BAC
	public static void CloseWindow()
	{
		if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
		{
			return;
		}
		EscapeUI.instance.OnCloseWindow();
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x000449C9 File Offset: 0x00042BC9
	public static void OpenWindow_Tutorial()
	{
		EscapeUI.instance.OnOpenWindow();
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x000449D5 File Offset: 0x00042BD5
	public static void CloseWindow_Tutorial()
	{
		EscapeUI.instance.OnCloseWindow();
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x001EEB10 File Offset: 0x001ECD10
	private void OnOpenWindow()
	{
		this._isOpened = true;
		this.ActiveControl.gameObject.SetActive(true);
		this.OverlayArrow.gameObject.SetActive(false);
		this.MiddleAreaControl.gameObject.SetActive(true);
		this.CheckPointConfirmControl.gameObject.SetActive(false);
		this.OnExitButton(0);
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x000449E1 File Offset: 0x00042BE1
	private void OnCloseWindow()
	{
		this._isOpened = false;
		this.ActiveControl.gameObject.SetActive(false);
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x001EEB70 File Offset: 0x001ECD70
	public void OnEnterButton(int index)
	{
		for (int i = 0; i < this.OverlayableButton.Length; i++)
		{
			if (i == index)
			{
				this.SetButtonBright(this.OverlayableButton[i]);
			}
			else
			{
				this.SetButtonDark(this.OverlayableButton[i]);
			}
		}
		switch (index)
		{
		case 0:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.RETRY);
			break;
		case 1:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.CHECKPOINT);
			break;
		case 2:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.NORMAL);
			break;
		case 3:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.NORMAL);
			break;
		case 4:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.NORMAL);
			break;
		case 5:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.NORMAL);
			break;
		case 6:
			this.OverlayArrowUI.UpdateArrow(EscapeMenuArrowUI.ArrowType.NORMAL);
			break;
		}
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x001EEC60 File Offset: 0x001ECE60
	public void OnExitButton(int index)
	{
		foreach (Image buttonDark in this.OverlayableButton)
		{
			this.SetButtonDark(buttonDark);
		}
		this.OverlayArrow.gameObject.SetActive(false);
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x000449FB File Offset: 0x00042BFB
	private void SetButtonBright(Image b)
	{
		this.OverlayArrow.SetParent(b.transform.GetChild(0).transform);
		this.OverlayArrow.gameObject.SetActive(true);
		this.OverlayArrow.anchoredPosition = Vector2.zero;
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x000043E5 File Offset: 0x000025E5
	private void SetButtonDark(Image b)
	{
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x001EECA4 File Offset: 0x001ECEA4
	public void OnClickButton(int index)
	{
		switch (index)
		{
		case 0:
			this.RestartAtCheckpoint();
			break;
		case 1:
			this.MiddleAreaControl.gameObject.SetActive(false);
			this.CheckPointConfirmControl.gameObject.SetActive(true);
			this.CheckPointConfirmText.text = LocalizeTextDataModel.instance.GetText("Escape_Checkpoint_Confirm");
			this.OnSetCheckPointDescText(this.CheckPointConfirmText);
			break;
		case 2:
			this.MoveTitle();
			break;
		case 3:
			this.ExitGame();
			break;
		case 4:
			this.OnClickCheckPointConfirm();
			break;
		case 5:
			this.OnClickCheckPointCancel();
			break;
		case 6:
			this.OpenOptionUI();
			break;
		}
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x00044A3A File Offset: 0x00042C3A
	private void ReturnToCheckpoint()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.ReturnToCheckPoint();
		}
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x00044A56 File Offset: 0x00042C56
	private void MoveTitle()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.ReturnToTitle();
		}
		else
		{
			GlobalGameManager.instance.StoryReturnTitle();
		}
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x00044A81 File Offset: 0x00042C81
	private void RestartAtCheckpoint()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.RestartGame();
		}
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x00044A9D File Offset: 0x00042C9D
	private void ExitGame()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.ExitGame();
		}
		else
		{
			Application.Quit();
		}
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x00044A3A File Offset: 0x00042C3A
	private void OnClickCheckPointConfirm()
	{
		if (GameManager.currentGameManager != null)
		{
			GameManager.currentGameManager.ReturnToCheckPoint();
		}
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x00044AC3 File Offset: 0x00042CC3
	private void OnClickCheckPointCancel()
	{
		this.MiddleAreaControl.gameObject.SetActive(true);
		this.CheckPointConfirmControl.gameObject.SetActive(false);
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x00044AE7 File Offset: 0x00042CE7
	public void OnSetCheckPointDescText(Text self)
	{
		self.text = self.text.Replace("$0", "DAY " + (GlobalGameManager.instance.LoadCheckPointDay() - 10000 + 1));
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001EED6C File Offset: 0x001ECF6C
	public void OnSetMasterVolume(float value)
	{
		this.MasterFill.fillAmount = value;
		if (BgmManager.instance != null)
		{
			BgmManager.instance.SetMasterSoundVolume(value);
		}
		else
		{
			GlobalGameManager.instance.sceneDataSaver.currentVolume = value;
			StoryBgm.instance.SetMasterVolume(value);
		}
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001EEDC0 File Offset: 0x001ECFC0
	public void OnSetMusicVolume(float value)
	{
		this.MusicFill.fillAmount = value;
		if (BgmManager.instance != null)
		{
			BgmManager.instance.SetBgmSoundVolume(value);
		}
		else
		{
			GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = value;
			StoryBgm.instance.SetBgmVolume(value);
		}
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x00044B20 File Offset: 0x00042D20
	public void OpenManual()
	{
		ManualUI.Instance.OpenManual();
		EscapeUI.CloseWindow();
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x00044B31 File Offset: 0x00042D31
	public void OnClickTooltipToggle()
	{
		if (this._initstate)
		{
			return;
		}
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x00044B3F File Offset: 0x00042D3F
	public void OpenOptionUI()
	{
		OptionUI.Instance.Open();
		EscapeUI.CloseWindow();
	}

	// Token: 0x060054D3 RID: 21715 RVA: 0x000043E5 File Offset: 0x000025E5
	// Note: this type is marked as 'beforefieldinit'.
	static EscapeUI()
	{
	}

	// Token: 0x04004DED RID: 19949
	private static EscapeUI _instance;

	// Token: 0x04004DEE RID: 19950
	public GameObject ActiveControl;

	// Token: 0x04004DEF RID: 19951
	public GameObject MiddleAreaControl;

	// Token: 0x04004DF0 RID: 19952
	public GameObject CheckPointConfirmControl;

	// Token: 0x04004DF1 RID: 19953
	public Text CheckPointConfirmText;

	// Token: 0x04004DF2 RID: 19954
	public RectTransform OverlayArrow;

	// Token: 0x04004DF3 RID: 19955
	public EscapeMenuArrowUI OverlayArrowUI;

	// Token: 0x04004DF4 RID: 19956
	public Image[] OverlayableButton;

	// Token: 0x04004DF5 RID: 19957
	public Slider MasterVolume;

	// Token: 0x04004DF6 RID: 19958
	public Image MasterFill;

	// Token: 0x04004DF7 RID: 19959
	public Slider MusicVolume;

	// Token: 0x04004DF8 RID: 19960
	public Image MusicFill;

	// Token: 0x04004DF9 RID: 19961
	public Toggle tooltipToggle;

	// Token: 0x04004DFA RID: 19962
	private bool _isOpened;

	// Token: 0x04004DFB RID: 19963
	private bool _initstate;

	// Token: 0x02000AEE RID: 2798
	public enum EscapeButtonAction
	{
		// Token: 0x04004DFD RID: 19965
		RESTART,
		// Token: 0x04004DFE RID: 19966
		CHECKPOINT,
		// Token: 0x04004DFF RID: 19967
		TITLE,
		// Token: 0x04004E00 RID: 19968
		ESCAPE,
		// Token: 0x04004E01 RID: 19969
		CHECKPOINT_OK,
		// Token: 0x04004E02 RID: 19970
		CHECKPOINT_CANCEL,
		// Token: 0x04004E03 RID: 19971
		OPTION
	}
}
