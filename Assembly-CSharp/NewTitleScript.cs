using System;
using System.Collections;
using System.IO;
using CreatureGenerate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200077C RID: 1916
public class NewTitleScript : MonoBehaviour, ILanguageLinkedData, IAnimatorEventCalled
{
	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06003B64 RID: 15204 RVA: 0x000349ED File Offset: 0x00032BED
	public static NewTitleScript instance
	{
		get
		{
			return NewTitleScript._instance;
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06003B65 RID: 15205 RVA: 0x000349F4 File Offset: 0x00032BF4
	// (set) Token: 0x06003B66 RID: 15206 RVA: 0x000349FC File Offset: 0x00032BFC
	public bool IsOpenedOption
	{
		get
		{
			return this._isOpenedOption;
		}
		set
		{
			this.OnSetOption(value);
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06003B67 RID: 15207 RVA: 0x0002C925 File Offset: 0x0002AB25
	private string currentLanguage
	{
		get
		{
			return GlobalGameManager.instance.GetCurrentLanguage();
		}
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x00034A05 File Offset: 0x00032C05
	private void OnSetOption(bool state)
	{
		this._isOpenedOption = state;
		this.OptionWindow.gameObject.SetActive(state);
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x00034A1F File Offset: 0x00032C1F
	private void Awake()
	{
		NewTitleScript._instance = this;
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x00034A27 File Offset: 0x00032C27
	private void OnReset()
	{
		this._pinned = false;
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x000043CD File Offset: 0x000025CD
	private void CheckFont()
	{
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x0017B52C File Offset: 0x0017972C
	private void Start()
	{
		this.OnExitCredit();
		this.StartNewSceneControl.enabled = false;
		this.MenuPanelAnim.enabled = false;
		this.IsOpenedOption = false;
		this.ButtonAreaGroup.alpha = 0f;
		this.ButtonAreaGroup.interactable = false;
		this.ButtonAreaGroup.blocksRaycasts = false;
		this.DeleteMaxObserveData();
		bool flag = false;
		this.hiddenRoot.SetActive(false);
		this.defRoot.SetActive(true);
		this.TitleBgm.volume = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
		Image[] array = this.hiddenOverlay;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		if (!GlobalGameManager.instance.IsPlaying())
		{
			if (!GlobalGameManager.instance.ExistSaveData())
			{
				Debug.Log("No data");
				this.contMenuActivated = false;
				this.continueText.color = this.Disabled;
			}
			else
			{
				flag = true;
			}
			if (!GlobalGameManager.instance.ExistUnlimitData())
			{
				this.chalMenuActivated = false;
				this.challengeText.color = this.Disabled;
			}
		}
		if (!flag)
		{
			this.Init();
		}
		this.newGameTooltip.SetActive(false);
		this.GameVersionChecker.rectTransform.sizeDelta = this.GameVersionChecker.rectTransform.sizeDelta + new Vector2(0f, 50f);
		this.GameVersionChecker.rectTransform.localPosition = this.GameVersionChecker.rectTransform.localPosition + new Vector3(0f, -25f, 0f);
		this.GameVersionChecker.text = string.Concat(new object[]
		{
			GlobalGameManager.instance.BuildVer,
			"\nBaseMod ",
			Add_On.version,
			" ver \nmade by abcdcode"
		});
		this.newGameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClickExitGame));
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x00034A30 File Offset: 0x00032C30
	public void OnEffectRun()
	{
		this.TitleEffectAnimator.SetTrigger("Run");
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x00034A42 File Offset: 0x00032C42
	public void OnSetLanguage(string language)
	{
		GlobalGameManager.instance.ChangeLanguage(GlobalGameManager.instance.GetLanguage(language));
		if (GlobalEtcDataModel.instance.trueEndingDone)
		{
			SceneManager.LoadSceneAsync("AlterTitleScene");
			return;
		}
		SceneManager.LoadSceneAsync("NewTitleScene");
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x00034A7C File Offset: 0x00032C7C
	private void DeleteMaxObserveData()
	{
		GlobalGameManager.instance.sceneDataSaver.maxObservedCreauture.Clear();
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x00034A92 File Offset: 0x00032C92
	private void Init()
	{
		if (GlobalGameManager.instance.sceneDataSaver.maxObservedCreauture.Count != 0)
		{
			GlobalGameManager.instance.sceneDataSaver.maxObservedCreauture.Clear();
		}
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x0017B720 File Offset: 0x00179920
	private void Update()
	{
		if (!this._effectStarted && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
		{
			if (!this.isAlter)
			{
				this._effectStarted = true;
				this.OnEffectRun();
			}
			else
			{
				this._effectStarted = true;
				this.OnCalled(2);
			}
		}
		if (this.StartEffectTime.started)
		{
			this.ButtonAreaGroup.alpha = this.StartEffectTime.Rate;
			if (this.StartEffectTime.RunTimer())
			{
				this.ButtonAreaGroup.interactable = true;
				this.ButtonAreaGroup.blocksRaycasts = true;
				if (this._gadgets)
				{
					this._gadgets.Show();
				}
			}
		}
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x0017B7CC File Offset: 0x001799CC
	public NewTitleScript.TitleObject GetObject(int id)
	{
		NewTitleScript.TitleObject result = null;
		foreach (NewTitleScript.TitleObject titleObject in this.titleObjects)
		{
			if (id.Equals(titleObject.indexer))
			{
				result = titleObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0017B814 File Offset: 0x00179A14
	public void OnPointerEnter(int id)
	{
		if (id == 1)
		{
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(0);
		}
		else if (id == 2)
		{
			if (!this.contMenuActivated)
			{
				return;
			}
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(0);
		}
		else if (id == 3)
		{
			if (!this.chalMenuActivated)
			{
				return;
			}
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(0);
		}
		else
		{
			if (id == 4)
			{
				this.IsOpenedOption = true;
				NewTitleScript.TitleObject @object = this.GetObject(id);
				@object.dayObject.SetActive(true);
				@object.dayText.text = SupportedLanguage.GetCurrentLanguageName(this.currentLanguage);
				NewTitleScript.instance.audioClipPlayer.OnPlayInList(0);
				return;
			}
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(0);
		}
		NewTitleScript.TitleObject object2 = this.GetObject(id);
		if (object2 != null)
		{
			if (id > 1 && id <= 3)
			{
				int day = GlobalGameManager.instance.PreLoadData() + 1;
				object2.OnEnter(day);
			}
			else if (id == 6)
			{
				object2.OnEnter(-1);
			}
			else if (id == 1)
			{
				object2.OnEnter(1);
			}
		}
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0017B93C File Offset: 0x00179B3C
	public void OnPointerExit(int id)
	{
		if (id == 2 && !this.contMenuActivated)
		{
			return;
		}
		if (id == 3)
		{
			if (!this.chalMenuActivated)
			{
				return;
			}
		}
		else if (id == 4 && !this._pinned)
		{
			this.IsOpenedOption = false;
		}
		NewTitleScript.TitleObject @object = this.GetObject(id);
		if (@object != null)
		{
			@object.OnExit();
		}
		else if (id == 6)
		{
			@object.OnExit();
		}
		this.overlayWithoutDay.SetActive(false);
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x0017B9C0 File Offset: 0x00179BC0
	public void OnClickContinue()
	{
		Debug.Log("Continue");
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		if (!this.contMenuActivated)
		{
			return;
		}
		this.StartNewSceneControl.enabled = true;
		this.newGameObject.SetActive(false);
		this.continueGameObject.SetActive(true);
		this._end = new NewTitleScript.OnMoveEndDelegate(this.ClickAfterContinue);
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x00034AC6 File Offset: 0x00032CC6
	public void OnMoveEnd()
	{
		if (this._end != null)
		{
			this._end();
		}
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x0017BA28 File Offset: 0x00179C28
	public void OnClickTutorial(int step)
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		this.newGameObject.SetActive(false);
		this.StartNewSceneControl.enabled = true;
		GlobalGameManager.instance.tutorialStep = step;
		this._end = new NewTitleScript.OnMoveEndDelegate(this.ClickAfterTutorial);
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x0017BA7C File Offset: 0x00179C7C
	public void OnClickNewGame()
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		this.MenuPanelAnim.enabled = true;
		this.StartAndResetText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			"Main window",
			"NewGameToolTip"
		});
		this.MenuPanelAnim.SetTrigger("ResetOpen");
		this.isNewGame = true;
		this._end = new NewTitleScript.OnMoveEndDelegate(this.ClickAfterNewGame);
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x0017BAFC File Offset: 0x00179CFC
	private void ClickAfterContinue()
	{
		try
		{
			GlobalGameManager.instance.LoadGlobalData();
			GlobalGameManager.instance.LoadData(SaveType.LASTDAY);
			if (GlobalGameManager.instance.saveState == "story")
			{
				this.LoadStoryMode();
			}
			else if (!this.nextLoading)
			{
				this.nextLoading = true;
				GlobalGameManager.instance.lastLoaded = true;
				this.LoadMainGame();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GlobalGameManager.instance.ReleaseGame();
		}
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x00034ADE File Offset: 0x00032CDE
	public void ClickAfterTutorial()
	{
		GlobalGameManager.instance.isPlayingTutorial = true;
		CreatureGenerateInfoManager.Instance.Init();
		GlobalGameManager.instance.InitTutorial(GlobalGameManager.instance.tutorialStep);
		this.Init();
		this.LoadTutorial();
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0017BB98 File Offset: 0x00179D98
	private void ClickAfterNewGame()
	{
		GlobalGameManager.instance.isPlayingTutorial = true;
		GlobalGameManager.instance.LoadGlobalData();
		CreatureGenerateInfoManager.Instance.Init();
		GlobalGameManager.instance.InitStoryMode();
		PlayerModel.instance.InitAddingCreatures();
		this.Init();
		this.LoadStoryMode();
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x00034B15 File Offset: 0x00032D15
	public void OnClickOption()
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		OptionUI.Instance.Open();
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00034B31 File Offset: 0x00032D31
	public void OnClickTutorial()
	{
		this._pinned = !this._pinned;
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		this.MenuPanelAnim.enabled = true;
		this.MenuPanelAnim.SetTrigger("TutorialOpen");
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x00034B6E File Offset: 0x00032D6E
	public void OnClickCloseTutorial()
	{
		this.MenuPanelAnim.SetTrigger("TutorialClose");
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x00034B80 File Offset: 0x00032D80
	public void OnClickExitGame()
	{
		Debug.Log("Exit");
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		Application.Quit();
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0017BBE4 File Offset: 0x00179DE4
	public void OnClickInfinite()
	{
		if (!this.chalMenuActivated)
		{
			return;
		}
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
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
			GlobalGameManager.instance.LoadUnlimitData();
			GlobalGameManager.instance.loadingScene = "StoryEndScene";
			GlobalGameManager.instance.loadingScreen.LoadScene("Main");
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GlobalGameManager.instance.ReleaseGame();
		}
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x0017BC8C File Offset: 0x00179E8C
	public void OnClickHiddenButton()
	{
		if (this.MenuPanelAnim.GetCurrentAnimatorStateInfo(0).IsName("Default"))
		{
			NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
			this.StartAndResetText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Main window",
				"ResetDesc"
			});
			this.isNewGame = false;
			this.MenuPanelAnim.enabled = true;
			this.MenuPanelAnim.SetTrigger("ResetOpen");
		}
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x00034BA1 File Offset: 0x00032DA1
	public void OnClickHiddenYes()
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		this.hiddenOverlay[0].enabled = false;
		this.Reset();
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x00034BC7 File Offset: 0x00032DC7
	public void OnClickHiddenNo()
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		this.hiddenOverlay[1].enabled = false;
		this.hiddenRoot.SetActive(false);
		this.defRoot.SetActive(true);
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x00034BFF File Offset: 0x00032DFF
	public void HiddenButtonEnter(int i)
	{
		this.hiddenOverlay[i].enabled = true;
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x00034C0F File Offset: 0x00032E0F
	public void HiddenButtonExit(int i)
	{
		this.hiddenOverlay[i].enabled = false;
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0017BD18 File Offset: 0x00179F18
	private void Reset()
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
		base.StartCoroutine(this.Reload());
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x00034C1F File Offset: 0x00032E1F
	private IEnumerator Reload()
	{
		AsyncOperation ao;
		if (GlobalEtcDataModel.instance.trueEndingDone)
		{
			ao = SceneManager.LoadSceneAsync("AlterTitleScene");
		}
		else
		{
			ao = SceneManager.LoadSceneAsync("NewTitleScene");
		}
		while (!ao.isDone)
		{
			yield return true;
		}
		yield break;
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x00034C27 File Offset: 0x00032E27
	public void LoadStoryMode()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "TitleEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("StoryV2");
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x00034C61 File Offset: 0x00032E61
	public void LoadMainGame()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "ContinueLoadingScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("Main");
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0017BDB0 File Offset: 0x00179FB0
	public void LoadTutorial()
	{
		if (GlobalGameManager.instance.loadingScreen.isLoading)
		{
			return;
		}
		GlobalGameManager.instance.loadingScene = "TitleEndScene";
		GlobalGameManager.instance.loadingScreen.LoadScene("Tutorial_" + GlobalGameManager.instance.tutorialStep.ToString());
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnNewGameEnter()
	{
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnNewGameExit()
	{
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x00034C9B File Offset: 0x00032E9B
	public void OnClickCredit()
	{
		SceneManager.LoadSceneAsync("Credit");
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void OnLanguageChanged()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnCalled()
	{
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x000043CD File Offset: 0x000025CD
	public void StartLogoEffect()
	{
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0017BE10 File Offset: 0x0017A010
	public void OnCalled(int i)
	{
		if (i != 1)
		{
			if (i != 2)
			{
				if (i == 3)
				{
					this.OnMoveEnd();
				}
			}
			else
			{
				this.StartEffectTime.StartTimer(this.StartEffectTimer);
			}
		}
		else
		{
			this.LogoAnimator.SetTrigger("Run");
		}
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void AgentReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void SimpleReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void AnimatorEventInit()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void CreatureAnimCall(int i, CreatureBase script)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void AttackCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void AttackDamageTimeCalled()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public void SoundMake(string src)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0017BE70 File Offset: 0x0017A070
	public void OnClickLanguage(int index)
	{
		NewTitleScript.instance.audioClipPlayer.OnPlayInList(1);
		switch (index)
		{
		case 0:
			this.OnSetLanguage("kr");
			break;
		case 1:
			this.OnSetLanguage("en");
			break;
		case 2:
			this.OnSetLanguage("cn");
			break;
		case 3:
			this.OnSetLanguage("jp");
			break;
		case 4:
			this.OnSetLanguage("cn_tr");
			break;
		case 5:
			this.OnSetLanguage("ru");
			break;
		case 6:
			this.OnSetLanguage("vn");
			break;
		}
		this.MenuPanelAnim.SetTrigger("LanguageClose");
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0017BF34 File Offset: 0x0017A134
	public void OnCickReset(int index)
	{
		if (index == 1)
		{
			this.MenuPanelAnim.SetTrigger("ResetClose");
			return;
		}
		bool flag = this.isNewGame;
		if (index == 1)
		{
			this.MenuPanelAnim.SetTrigger("ResetClose");
			return;
		}
		this.newGameObject.SetActive(true);
		this.continueGameObject.SetActive(false);
		if (this.isNewGame)
		{
			this.StartNewSceneControl.enabled = true;
			return;
		}
		this.isNewGame = false;
		this.Reset();
		this.MenuPanelAnim.SetTrigger("ResetClose");
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x00034CB8 File Offset: 0x00032EB8
	public void OnEnterCredit()
	{
		this.translationCanvas.Show();
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x00034CC5 File Offset: 0x00032EC5
	public void OnExitCredit()
	{
		this.translationCanvas.Hide();
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x00034CD2 File Offset: 0x00032ED2
	public void MoveToStoryTester()
	{
		SceneManager.LoadScene("StoryViewer");
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x00034CDE File Offset: 0x00032EDE
	public void OpenCodex()
	{
		if (CreatureInfoWindow.CurrentWindow)
		{
			CreatureInfoWindow.CreateCodexWindow();
		}
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0017BFBC File Offset: 0x0017A1BC
	public void SetCreditLayout(int index)
	{
		if (index == 0)
		{
			this.jpCanvas.SetActive(true);
			this.cnCanvas.SetActive(false);
			this.cn_trCanvas.SetActive(false);
			this.ruCanvas.SetActive(false);
			this.vnCanvas.SetActive(false);
		}
		else if (index == 1)
		{
			this.jpCanvas.SetActive(false);
			this.cnCanvas.SetActive(true);
			this.cn_trCanvas.SetActive(false);
			this.ruCanvas.SetActive(false);
			this.vnCanvas.SetActive(false);
		}
		else if (index == 2)
		{
			this.jpCanvas.SetActive(false);
			this.cnCanvas.SetActive(false);
			this.cn_trCanvas.SetActive(true);
			this.ruCanvas.SetActive(false);
			this.vnCanvas.SetActive(false);
		}
		else if (index == 3)
		{
			this.jpCanvas.SetActive(false);
			this.cnCanvas.SetActive(false);
			this.cn_trCanvas.SetActive(false);
			this.ruCanvas.SetActive(true);
			this.vnCanvas.SetActive(false);
		}
		else if (index == 6)
		{
			this.vnCanvas.SetActive(true);
			this.jpCanvas.SetActive(false);
			this.cnCanvas.SetActive(false);
			this.cn_trCanvas.SetActive(false);
			this.ruCanvas.SetActive(false);
		}
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0017C12C File Offset: 0x0017A32C
	private void ShowBaseMod()
	{
		GameObject gameObject = new GameObject("unname");
		gameObject.AddComponent<SpriteRenderer>();
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/MOD/Image/Version.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 168f, 70f), new Vector2((float)(texture2D.width / 336), (float)texture2D.height / 140f));
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
		gameObject.transform.SetParent(this.ButtonAreaGroup.transform.parent);
		gameObject.transform.localScale = new Vector2(100f, 100f);
		gameObject.transform.position = new Vector2(-16.3f, 10.4f);
		gameObject.SetActive(true);
	}

	// Token: 0x040036AA RID: 13994
	private NewTitleScript.OnMoveEndDelegate _end;

	// Token: 0x040036AB RID: 13995
	private static NewTitleScript _instance;

	// Token: 0x040036AC RID: 13996
	public Text dayCount;

	// Token: 0x040036AD RID: 13997
	public GameObject overlayTortal;

	// Token: 0x040036AE RID: 13998
	public GameObject overlayWithDay;

	// Token: 0x040036AF RID: 13999
	public GameObject overlayWithoutDay;

	// Token: 0x040036B0 RID: 14000
	public AudioClipPlayer audioClipPlayer;

	// Token: 0x040036B1 RID: 14001
	public NewTitleScript.TitleObject[] titleObjects;

	// Token: 0x040036B2 RID: 14002
	public Image[] hiddenOverlay;

	// Token: 0x040036B3 RID: 14003
	public GameObject defRoot;

	// Token: 0x040036B4 RID: 14004
	public GameObject hiddenRoot;

	// Token: 0x040036B5 RID: 14005
	public GameObject newGameTooltip;

	// Token: 0x040036B6 RID: 14006
	private bool contMenuActivated = true;

	// Token: 0x040036B7 RID: 14007
	private bool chalMenuActivated = true;

	// Token: 0x040036B8 RID: 14008
	private bool nextLoading;

	// Token: 0x040036B9 RID: 14009
	public Animator TitleEffectAnimator;

	// Token: 0x040036BA RID: 14010
	public Animator LogoAnimator;

	// Token: 0x040036BB RID: 14011
	public Color Disabled;

	// Token: 0x040036BC RID: 14012
	public EventColorSetting eventColorSettingContinue;

	// Token: 0x040036BD RID: 14013
	public Text continueText;

	// Token: 0x040036BE RID: 14014
	public EventColorSetting eventColorSettingChallenge;

	// Token: 0x040036BF RID: 14015
	public Text challengeText;

	// Token: 0x040036C0 RID: 14016
	public CanvasGroup ButtonAreaGroup;

	// Token: 0x040036C1 RID: 14017
	public float StartEffectTimer;

	// Token: 0x040036C2 RID: 14018
	private Timer StartEffectTime = new Timer();

	// Token: 0x040036C3 RID: 14019
	public Animator StartNewSceneControl;

	// Token: 0x040036C4 RID: 14020
	private bool _isOpenedOption;

	// Token: 0x040036C5 RID: 14021
	public RectTransform OptionWindow;

	// Token: 0x040036C6 RID: 14022
	public Animator MenuPanelAnim;

	// Token: 0x040036C7 RID: 14023
	public Text GameVersionChecker;

	// Token: 0x040036C8 RID: 14024
	public float[] AnchorY;

	// Token: 0x040036C9 RID: 14025
	[Header("Translation Credits")]
	public UIController translationCanvas;

	// Token: 0x040036CA RID: 14026
	public GameObject cnCanvas;

	// Token: 0x040036CB RID: 14027
	public GameObject jpCanvas;

	// Token: 0x040036CC RID: 14028
	public GameObject cn_trCanvas;

	// Token: 0x040036CD RID: 14029
	public GameObject ruCanvas;

	// Token: 0x040036CE RID: 14030
	public GameObject vnCanvas;

	// Token: 0x040036CF RID: 14031
	public AudioSource TitleBgm;

	// Token: 0x040036D0 RID: 14032
	private bool isNewGame;

	// Token: 0x040036D1 RID: 14033
	public bool isAlter;

	// Token: 0x040036D2 RID: 14034
	public UIController _gadgets;

	// Token: 0x040036D3 RID: 14035
	private bool _pinned;

	// Token: 0x040036D4 RID: 14036
	private bool _effectStarted;

	// Token: 0x040036D5 RID: 14037
	public GameObject newGameObject;

	// Token: 0x040036D6 RID: 14038
	public GameObject continueGameObject;

	// Token: 0x040036D7 RID: 14039
	public Text StartAndResetText;

	// Token: 0x040036D8 RID: 14040
	private GameObject BaseModButton;

	// Token: 0x0200077D RID: 1917
	public static class TranslateCredit
	{
		// Token: 0x040036D9 RID: 14041
		public static string[] jp = new string[]
		{
			"acane",
			"甘輪",
			"いすひろし",
			"とらきす",
			"翻訳協力者",
			"もちみかん",
			"Youkan",
			"ログノ"
		};

		// Token: 0x040036DA RID: 14042
		public static string[] cn = new string[]
		{
			"Ro",
			"KevinGlass",
			"Ade007",
			"Sea",
			"Amiba",
			"Poten",
			"Nicholas_Orka",
			"Kaisim"
		};
	}

	// Token: 0x0200077E RID: 1918
	// (Invoke) Token: 0x06003BA5 RID: 15269
	public delegate void OnMoveEndDelegate();

	// Token: 0x0200077F RID: 1919
	[Serializable]
	public class TitleObject
	{
		// Token: 0x06003BA9 RID: 15273 RVA: 0x00034D0C File Offset: 0x00032F0C
		public void OnEnter(int day)
		{
			if (this.dayText != null && this.dayObject != null)
			{
				this.dayObject.SetActive(true);
				this.SetDay(day);
			}
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00034D43 File Offset: 0x00032F43
		public void OnExit()
		{
			if (this.dayObject != null)
			{
				this.dayObject.SetActive(false);
			}
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x00034D62 File Offset: 0x00032F62
		public void SetDay(int day)
		{
			this.dayText.text = day.ToString();
		}

		// Token: 0x040036DB RID: 14043
		public int indexer = -1;

		// Token: 0x040036DC RID: 14044
		public GameObject button;

		// Token: 0x040036DD RID: 14045
		public GameObject dayObject;

		// Token: 0x040036DE RID: 14046
		public Text dayText;
	}
}
