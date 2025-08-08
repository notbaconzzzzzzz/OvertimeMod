using System;
using System.Collections;
using System.Collections.Generic;
using CreatureGenerate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000AB8 RID: 2744
public class AlterTitleController : MonoBehaviour
{
	// Token: 0x06005279 RID: 21113 RVA: 0x0004348D File Offset: 0x0004168D
	public AlterTitleController()
	{
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x0600527A RID: 21114 RVA: 0x000434AB File Offset: 0x000416AB
	// (set) Token: 0x0600527B RID: 21115 RVA: 0x000434B2 File Offset: 0x000416B2
	public static AlterTitleController Controller { get; private set; }

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x0600527C RID: 21116 RVA: 0x000434BA File Offset: 0x000416BA
	private UIController _buttonCTRL
	{
		get
		{
			return this._buttonRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x0600527D RID: 21117 RVA: 0x000434C7 File Offset: 0x000416C7
	private UIController _gadgetCTRL
	{
		get
		{
			return this._gadgetRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x0600527E RID: 21118 RVA: 0x000434D4 File Offset: 0x000416D4
	private UIController _logoCTRL
	{
		get
		{
			return this._logoRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x0600527F RID: 21119 RVA: 0x000434E1 File Offset: 0x000416E1
	private UIController _languageCTRL
	{
		get
		{
			return this._languageRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06005280 RID: 21120 RVA: 0x000434EE File Offset: 0x000416EE
	private UIController _resetCTRL
	{
		get
		{
			return this._resetRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06005281 RID: 21121 RVA: 0x000434FB File Offset: 0x000416FB
	private UIController _creditCTRL
	{
		get
		{
			return this._creditRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06005282 RID: 21122 RVA: 0x00043508 File Offset: 0x00041708
	private UIController _tutorialCTRL
	{
		get
		{
			return this._tutorialRoot.GetComponent<UIController>();
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06005283 RID: 21123 RVA: 0x0002C928 File Offset: 0x0002AB28
	private string CurrentLanguage
	{
		get
		{
			return GlobalGameManager.instance.GetCurrentLanguage();
		}
	}

	// Token: 0x06005284 RID: 21124 RVA: 0x001E1C24 File Offset: 0x001DFE24
	private void Awake()
	{
		AlterTitleController.Controller = this;
		this._actionLibrary.Clear();
		this._actionLibrary.Add(AlterTitleController.TitleActionType.TUTORIAL, new AlterTitleController.TitleCall(this.CallTutorial));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.NEWGAME, new AlterTitleController.TitleCall(this.CallNewgame));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.CONTINUE, new AlterTitleController.TitleCall(this.CallContinue));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.CHALLENGE, new AlterTitleController.TitleCall(this.CallChallenge));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.LANGUAGE, new AlterTitleController.TitleCall(this.CallLanguage));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.EXIT, new AlterTitleController.TitleCall(this.CallExit));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.STORY_REVIEW, new AlterTitleController.TitleCall(this.CallStoryReview));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.CODEX, new AlterTitleController.TitleCall(this.CallCodex));
		this._actionLibrary.Add(AlterTitleController.TitleActionType.RESET, new AlterTitleController.TitleCall(this.CallReset));
		this._cameraAnim.enabled = false;
		this._initialInput = false;
		this._nextLoading = false;
		this._creditPanelsDic.Clear();
		foreach (AlterTitleController.CreditPanel creditPanel in this.creditPanels)
		{
			this._creditPanelsDic.Add(creditPanel.language, creditPanel);
		}
	}

	// Token: 0x06005285 RID: 21125 RVA: 0x001E1D9C File Offset: 0x001DFF9C
	private void Start()
	{
		this.LoadBackgroundImage();
		this._logoCTRL.Show();
		this.CheckSaveState();
		this.GameVersionChecker.rectTransform.sizeDelta = this.GameVersionChecker.rectTransform.sizeDelta + new Vector2(0f, 50f);
		this.GameVersionChecker.rectTransform.localPosition = this.GameVersionChecker.rectTransform.localPosition + new Vector3(0f, -25f, 0f);
		this.GameVersionChecker.text = string.Concat(new object[]
		{
			GlobalGameManager.instance.BuildVer,
			"\nBaseMod ",
			Add_On.version,
			" ver \nmade by abcdcode"
		});
		this.LanguageText.text = SupportedLanguage.GetCurrentLanguageName(this.CurrentLanguage);
		int num = GlobalGameManager.instance.PreLoadData() + 1;
		string text = "Day " + num;
		Text challengeDayText = this.ChallengeDayText;
		string text2 = text;
		this.ContinueDayText.text = text2;
		challengeDayText.text = text2;
		this.ResetButton.interactable = true;
	}

	// Token: 0x06005286 RID: 21126 RVA: 0x001E1EC4 File Offset: 0x001E00C4
	private void CheckSaveState()
	{
		this.GetButton(AlterTitleController.TitleActionType.CONTINUE).interactable = true;
		this.GetButton(AlterTitleController.TitleActionType.CHALLENGE).interactable = true;
		if (!GlobalGameManager.instance.IsPlaying())
		{
			if (!GlobalGameManager.instance.ExistSaveData())
			{
				this.GetButton(AlterTitleController.TitleActionType.CONTINUE).interactable = false;
			}
			if (!GlobalGameManager.instance.ExistUnlimitData())
			{
				this.GetButton(AlterTitleController.TitleActionType.CHALLENGE).interactable = false;
			}
		}
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x001E1F34 File Offset: 0x001E0134
	private void Update()
	{
		if (this._blackFadeOn)
		{
			this.UpdateFade();
			return;
		}
		if (!this._initialInput && Input.anyKeyDown)
		{
			this._initialInput = true;
			if (this.CheckBackgroundCondition() && CreatureManager.instance.IsMaxHiddenProgress())
			{
				this.InitHiddenStoryEffect();
				return;
			}
			this.InitEffect();
		}
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x00043515 File Offset: 0x00041715
	public void InitEffect()
	{
		this._buttonCTRL.Show();
		this._gadgetCTRL.Show();
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x0004352D File Offset: 0x0004172D
	private void InitHiddenStoryEffect()
	{
		this._fadeProgress = 0f;
		this._blackFadeOn = true;
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001E1F98 File Offset: 0x001E0198
	private void UpdateFade()
	{
		this._fadeProgress += Time.deltaTime;
		this.BlackFade.color = new Color(1f, 1f, 1f, this._fadeProgress);
		if (this._fadeProgress >= 1f)
		{
			GlobalGameManager.instance.InitHidden();
			SceneManager.LoadScene("StoryV2");
		}
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x00043541 File Offset: 0x00041741
	private Button GetButton(AlterTitleController.TitleActionType actionType)
	{
		if (actionType < AlterTitleController.TitleActionType.TUTORIAL || actionType >= AlterTitleController.TitleActionType.DUMMY)
		{
			return null;
		}
		return this.buttons[(int)actionType].button;
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x001E2000 File Offset: 0x001E0200
	public void OnClickButton(int id)
	{
		if (id < 0 || id >= 9)
		{
			Debug.LogError("Unknown Action : " + id);
			return;
		}
		AlterTitleController.TitleCall titleCall = null;
		if (this._actionLibrary.TryGetValue((AlterTitleController.TitleActionType)id, out titleCall))
		{
			Debug.Log((AlterTitleController.TitleActionType)id);
			titleCall();
		}
	}

	// Token: 0x0600528D RID: 21133 RVA: 0x00043561 File Offset: 0x00041761
	private void CallTutorial()
	{
		this._buttonCTRL.Hide();
		this._tutorialCTRL.Show();
		this.ResetButton.interactable = false;
	}

	// Token: 0x0600528E RID: 21134 RVA: 0x00043585 File Offset: 0x00041785
	private void CallNewgame()
	{ // <Mod>
		try
		{
			GlobalGameManager.instance.isPlayingTutorial = false;
			GlobalGameManager.instance.LoadGlobalData();
			CreatureGenerateInfoManager.Instance.Init();
			GlobalGameManager.instance.InitStoryMode();
			PlayerModel.instance.InitAddingCreatures();
			this.LoadStoryMode();
		}
		catch (Exception ex)
		{
			LobotomyBaseMod.ModDebug.Log("CallNewGame : " + ex.Message + " : " + ex.StackTrace);
		}
	}

	// Token: 0x0600528F RID: 21135 RVA: 0x001E205C File Offset: 0x001E025C
	private void CallContinue()
	{
		try
		{
			GlobalGameManager.instance.LoadGlobalData();
			GlobalGameManager.instance.LoadData(SaveType.LASTDAY);
			if (GlobalGameManager.instance.saveState == "story")
			{
				this.LoadStoryMode();
			}
			else if (!this._nextLoading)
			{
				this._nextLoading = true;
				GlobalGameManager.instance.lastLoaded = true;
				this.LoadMainGame();
			}
		}
		catch (Exception message)
		{
			LobotomyBaseMod.ModDebug.Log("CallContiune : " + message.Message + " : " + message.StackTrace);
			GlobalGameManager.instance.ReleaseGame();
		}
	}

	// Token: 0x06005290 RID: 21136 RVA: 0x001E20E0 File Offset: 0x001E02E0
	private void CallChallenge()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		if (!GlobalGameManager.instance.ExistUnlimitData())
		{
			return;
		}
		try
		{
			GlobalGameManager.instance.LoadUnlimitData();
			this.LoadUnlimitMode();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GlobalGameManager.instance.ReleaseGame();
		}
	}

	// Token: 0x06005291 RID: 21137 RVA: 0x000435C0 File Offset: 0x000417C0
	private void CallLanguage()
	{
		this.CallOption();
	}

	// Token: 0x06005292 RID: 21138 RVA: 0x000435C8 File Offset: 0x000417C8
	private void CallOption()
	{
		OptionUI.Instance.Open();
	}

	// Token: 0x06005293 RID: 21139 RVA: 0x0002DB97 File Offset: 0x0002BD97
	private void CallExit()
	{
		Application.Quit();
	}

	// Token: 0x06005294 RID: 21140 RVA: 0x00034CA4 File Offset: 0x00032EA4
	private void CallStoryReview()
	{
		SceneManager.LoadScene("StoryViewer");
	}

	// Token: 0x06005295 RID: 21141 RVA: 0x000435D4 File Offset: 0x000417D4
	private void CallCodex()
	{
		this._buttonCTRL.Hide();
		this._gadgetCTRL.Hide();
		CreatureInfoWindow.CreateCodexWindow();
	}

	// Token: 0x06005296 RID: 21142 RVA: 0x000435F2 File Offset: 0x000417F2
	private void CallReset()
	{
		this._buttonCTRL.Hide();
		this._resetCTRL.Show();
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x0004360A File Offset: 0x0004180A
	public void OnOpen()
	{
		this._cameraAnim.enabled = true;
	}

	// Token: 0x06005298 RID: 21144 RVA: 0x001E2150 File Offset: 0x001E0350
	public void OnSetLanguage(string ln)
	{ // <Patch>
		this.GetPlayer().OnPlayInList(1);
		GlobalGameManager.instance.ChangeLanguage_new(ln);
		this._languageCTRL.Hide();
		this._buttonCTRL.Show();
		base.StartCoroutine(this.Reload());
	}

	// Token: 0x06005299 RID: 21145 RVA: 0x001E21A4 File Offset: 0x001E03A4
	public void OnClickResetButton(bool reset)
	{
		if (Add_On.Loading("A_AlterTitleController", "OnClickResetButton", new object[]
		{
			this,
			reset
		}))
		{
			return;
		}
		this._buttonCTRL.Show();
		this._resetCTRL.Hide();
		if (reset)
		{
			this.ResetData();
		}
	}

	// Token: 0x0600529A RID: 21146 RVA: 0x00034BF9 File Offset: 0x00032DF9
	private void LoadStoryMode()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "TitleEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("StoryV2");
	}

	// Token: 0x0600529B RID: 21147 RVA: 0x00034C33 File Offset: 0x00032E33
	private void LoadMainGame()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "ContinueLoadingScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("Main");
	}

	// Token: 0x0600529C RID: 21148 RVA: 0x00043618 File Offset: 0x00041818
	private void LoadUnlimitMode()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "StoryEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("Main");
	}

	// Token: 0x0600529D RID: 21149 RVA: 0x001E21F8 File Offset: 0x001E03F8
	private void ResetData()
	{
		GlobalGameManager.instance.RemoveSaveData();
		GlobalGameManager.instance.RemoveGlobalData();
		GlobalGameManager.instance.RemoveUnlimitData();
		GlobalGameManager.instance.RemoveEtcData();
		CreatureTypeList.instance.ResetSkillTipTable();
		CreatureManager.instance.RemoveSriptSaveData();
		AgentManager.instance.RemoveCustomAgentData();
		AgentManager.instance.RemoveDelAgentData();
		GlobalGameManager.instance.ReleaseGame();
		GlobalGameManager.instance.sceneDataSaver.tooltipState = true;
		PlayerModel.instance.InitAddingCreatures();
		CreatureGenerateInfoManager.Instance.Init();
		base.StartCoroutine(this.ReloadOrigin());
	}

	// Token: 0x0600529E RID: 21150 RVA: 0x001E2290 File Offset: 0x001E0490
	private IEnumerator Reload()
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync("AlterTitleScene");
		while (!ao.isDone)
		{
			yield return true;
		}
		yield break;
	}

	// Token: 0x0600529F RID: 21151 RVA: 0x001E22A4 File Offset: 0x001E04A4
	private IEnumerator ReloadOrigin()
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync("NewTitleScene");
		while (!ao.isDone)
		{
			yield return true;
		}
		yield break;
	}

	// Token: 0x060052A0 RID: 21152 RVA: 0x001E22B8 File Offset: 0x001E04B8
	private AlterTitleController.CreditPanel GetCreditPanel(string ln)
	{
		AlterTitleController.CreditPanel result = null;
		if (this._creditPanelsDic.TryGetValue(ln, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060052A1 RID: 21153 RVA: 0x001E22E0 File Offset: 0x001E04E0
	public void OnCreditEnter(string ln)
	{
		this._creditCTRL.Show();
		AlterTitleController.CreditPanel creditPanel = this.GetCreditPanel(ln);
		if (creditPanel != null)
		{
			creditPanel.panelObject.SetActive(true);
		}
	}

	// Token: 0x060052A2 RID: 21154 RVA: 0x001E2314 File Offset: 0x001E0514
	public void OnCreditExit(string ln)
	{
		this._creditCTRL.Hide();
		AlterTitleController.CreditPanel creditPanel = this.GetCreditPanel(ln);
		if (creditPanel != null)
		{
			creditPanel.panelObject.SetActive(false);
		}
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x001E2348 File Offset: 0x001E0548
	public void OnClickTutorial(int step)
	{
		if (step == -1)
		{
			this._tutorialCTRL.Hide();
			this._buttonCTRL.Show();
			this.ResetButton.interactable = true;
			return;
		}
		GlobalGameManager.instance.tutorialStep = step;
		GlobalGameManager.instance.isPlayingTutorial = true;
		CreatureGenerateInfoManager.Instance.Init();
		GlobalGameManager.instance.InitTutorial(GlobalGameManager.instance.tutorialStep);
		GlobalGameManager.instance.sceneDataSaver.maxObservedCreauture.Clear();
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "TitleEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("Tutorial_" + GlobalGameManager.instance.tutorialStep.ToString());
	}

	// Token: 0x060052A4 RID: 21156 RVA: 0x001E241C File Offset: 0x001E061C
	public void LoadBackgroundImage()
	{
		if (this.CheckBackgroundCondition())
		{
			this._backgroundRenderer.sprite = this._light;
			this._lightSound.SetActive(true);
			this._darkSound.SetActive(false);
			this.TitleBgm = this._lightSound.GetComponent<AudioSource>();
		}
		else
		{
			this._backgroundRenderer.sprite = this._dark;
			this._lightSound.SetActive(false);
			this._darkSound.SetActive(true);
			this.TitleBgm = this._darkSound.GetComponent<AudioSource>();
		}
		this.TitleBgm.volume = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
	}

	// Token: 0x060052A5 RID: 21157 RVA: 0x00043652 File Offset: 0x00041852
	private bool CheckBackgroundCondition()
	{
		return !GlobalEtcDataModel.instance.hiddenEndingDone;
	}

	// Token: 0x060052A6 RID: 21158 RVA: 0x00043661 File Offset: 0x00041861
	public AudioClipPlayer GetPlayer()
	{
		return AlterTitleController.Controller.GetComponent<AudioClipPlayer>();
	}

	// Token: 0x04004C21 RID: 19489
	public UIController _rootController;

	// Token: 0x04004C22 RID: 19490
	public SpriteRenderer _backgroundRenderer;

	// Token: 0x04004C23 RID: 19491
	public Sprite _light;

	// Token: 0x04004C24 RID: 19492
	public Sprite _dark;

	// Token: 0x04004C25 RID: 19493
	public GameObject _lightSound;

	// Token: 0x04004C26 RID: 19494
	public GameObject _darkSound;

	// Token: 0x04004C27 RID: 19495
	public RectTransform _buttonRoot;

	// Token: 0x04004C28 RID: 19496
	public RectTransform _gadgetRoot;

	// Token: 0x04004C29 RID: 19497
	public RectTransform _logoRoot;

	// Token: 0x04004C2A RID: 19498
	public RectTransform _languageRoot;

	// Token: 0x04004C2B RID: 19499
	public RectTransform _resetRoot;

	// Token: 0x04004C2C RID: 19500
	public RectTransform _creditRoot;

	// Token: 0x04004C2D RID: 19501
	public RectTransform _tutorialRoot;

	// Token: 0x04004C2E RID: 19502
	public TitleCameraAnim _cameraAnim;

	// Token: 0x04004C2F RID: 19503
	public AlterTitleController.TitleButton[] buttons;

	// Token: 0x04004C30 RID: 19504
	public Text GameVersionChecker;

	// Token: 0x04004C31 RID: 19505
	[Header("Translations Credit")]
	public List<AlterTitleController.CreditPanel> creditPanels;

	// Token: 0x04004C32 RID: 19506
	[Header("GameInfo")]
	public Text ContinueDayText;

	// Token: 0x04004C33 RID: 19507
	public Text ChallengeDayText;

	// Token: 0x04004C34 RID: 19508
	public Text LanguageText;

	// Token: 0x04004C35 RID: 19509
	public Button ResetButton;

	// Token: 0x04004C36 RID: 19510
	[Header("HiddenEnding")]
	public Image BlackFade;

	// Token: 0x04004C37 RID: 19511
	private float _fadeProgress;

	// Token: 0x04004C38 RID: 19512
	private bool _blackFadeOn;

	// Token: 0x04004C39 RID: 19513
	private bool _nextLoading;

	// Token: 0x04004C3A RID: 19514
	private bool _initialInput;

	// Token: 0x04004C3B RID: 19515
	private Dictionary<AlterTitleController.TitleActionType, AlterTitleController.TitleCall> _actionLibrary = new Dictionary<AlterTitleController.TitleActionType, AlterTitleController.TitleCall>();

	// Token: 0x04004C3C RID: 19516
	private Dictionary<string, AlterTitleController.CreditPanel> _creditPanelsDic = new Dictionary<string, AlterTitleController.CreditPanel>();

	// Token: 0x04004C3D RID: 19517
	public AudioSource TitleBgm;

	// Token: 0x02000AB9 RID: 2745
	public enum TitleActionType
	{
		// Token: 0x04004C3F RID: 19519
		TUTORIAL,
		// Token: 0x04004C40 RID: 19520
		NEWGAME,
		// Token: 0x04004C41 RID: 19521
		CONTINUE,
		// Token: 0x04004C42 RID: 19522
		CHALLENGE,
		// Token: 0x04004C43 RID: 19523
		LANGUAGE,
		// Token: 0x04004C44 RID: 19524
		EXIT,
		// Token: 0x04004C45 RID: 19525
		STORY_REVIEW,
		// Token: 0x04004C46 RID: 19526
		CODEX,
		// Token: 0x04004C47 RID: 19527
		RESET,
		// Token: 0x04004C48 RID: 19528
		DUMMY
	}

	// Token: 0x02000ABA RID: 2746
	[Serializable]
	public class TitleButton
	{
		// Token: 0x060052A7 RID: 21159 RVA: 0x000043A0 File Offset: 0x000025A0
		public TitleButton()
		{
		}

		// Token: 0x04004C49 RID: 19529
		public AlterTitleController.TitleActionType actionType;

		// Token: 0x04004C4A RID: 19530
		public Button button;
	}

	// Token: 0x02000ABB RID: 2747
	[Serializable]
	public class CreditPanel
	{
		// Token: 0x060052A8 RID: 21160 RVA: 0x000043A0 File Offset: 0x000025A0
		public CreditPanel()
		{
		}

		// Token: 0x04004C4B RID: 19531
		public string language;

		// Token: 0x04004C4C RID: 19532
		public GameObject panelObject;
	}

	// Token: 0x02000ABC RID: 2748
	// (Invoke) Token: 0x060052AA RID: 21162
	public delegate void TitleCall();
}
