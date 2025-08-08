using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A1A RID: 2586
public class OptionUI : MonoBehaviour
{
	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x06004E92 RID: 20114 RVA: 0x00040C6A File Offset: 0x0003EE6A
	// (set) Token: 0x06004E91 RID: 20113 RVA: 0x00040C62 File Offset: 0x0003EE62
	public static OptionUI Instance { get; private set; }

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06004E93 RID: 20115 RVA: 0x00040C71 File Offset: 0x0003EE71
	// (set) Token: 0x06004E94 RID: 20116 RVA: 0x00040C79 File Offset: 0x0003EE79
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
		set
		{
			this._isEnabled = value;
		}
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001D29F4 File Offset: 0x001D0BF4
	private void Awake()
	{ // <Patch>
		if (OptionUI.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
        OptionUI.credit = new string[]
        {
            "Poten / Ro / KevinGlass / ade007 / Amiba / Sea / Nicholas_Okra",
            "acane / 甘輪 / いすひろし / とらきす / 翻訳協力者" + Environment.NewLine + "もちみかん / Youkan / ログノ / 6人目のサメの餌",
            "surolanter",
            "Tales&Stories Team / GregorLesnov / BlinkRaven / Knightey",
            "Misui",
            "Dimitar Topkov (di_TOP)",
            "Main Translator : @UrathObsidian" + Environment.NewLine + "Programmer/Language Assistant : @casual_watson ",
            "Cool Kids Club Translation : NEETPenguin and Casual Watson\r\nHelpers : Catling and Kuroteru\r\nLastly… Many thanks to all who tested our script and supported us!",
            "Traduction Eden Office : Azuro, Nakys, Skriff, Skun et Pacman\r\nTous nos remerciements à l'ensemble de nos testeurs et aux personnes qui nous ont aidé !",
            "Muito Obrigado Pela Tradução!\r\n\r\nTradução Team P.A.T.O: Shoes & Arucato\r\nAgradecimentos adicionais a nosso grande testador: Efeshis, todos aqueles que nos ajudaram até aqui, e a equipe de nosso companheiro Milk!",
            "OBRIGADO PELA VOSSA TRADUÇÃO!\r\nTradutora : R.ANAKOVA"
        };
		OptionUI.Instance = this;
		this.creditText.Add("cn", OptionUI.credit[0]);
		this.creditText.Add("jp", OptionUI.credit[1]);
		this.creditText.Add("cn_tr", OptionUI.credit[2]);
		this.creditText.Add("ru", OptionUI.credit[3]);
		this.creditText.Add("vn", OptionUI.credit[4]);
		this.creditText.Add("bg", OptionUI.credit[5]);
		this.creditText.Add("es", OptionUI.credit[6]);
        this.creditText.Add("en", OptionUI.credit[7]);
        this.creditText.Add("fr", OptionUI.credit[8]);
        this.creditText.Add("pt_br", OptionUI.credit[9]);
        this.creditText.Add("pt_pt", OptionUI.credit[10]);
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x00040C82 File Offset: 0x0003EE82
	private void Start()
	{
		base.gameObject.SetActive(false);
		this.CheckCredit();
		this.CheckVolume();
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x00040C9C File Offset: 0x0003EE9C
	private void OnLevelWasLoaded(int level)
	{
		this._isEnabled = false;
		base.gameObject.SetActive(false);
		this.CheckCredit();
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x00040CB7 File Offset: 0x0003EEB7
	public void CheckVolume()
	{
		this.Opt_MasterVolume.value = GlobalGameManager.instance.sceneDataSaver.currentVolume;
		this.Opt_MusicVolume.value = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x001D2AC4 File Offset: 0x001D0CC4
	public void CheckCredit()
	{
		string currentLanguage = GlobalGameManager.instance.GetCurrentLanguage();
		string empty = string.Empty;
		if (this.creditText.TryGetValue(currentLanguage, out empty) && !string.IsNullOrEmpty(empty))
		{
			this.CreditLabel.text = empty;
			this.CreditTitle.gameObject.SetActive(true);
			this.CreditRoot.SetActive(true);
			this.OverlayDesc.gameObject.SetActive(false);
			return;
		}
		this.CreditRoot.SetActive(false);
		this.OverlayDesc.gameObject.SetActive(false);
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x00040CED File Offset: 0x0003EEED
	public void OnPointerEnterDlc()
	{
		this.CreditLabel.text = string.Empty;
		this.CreditRoot.SetActive(true);
		this.CreditTitle.gameObject.SetActive(false);
		this.OverlayDesc.gameObject.SetActive(true);
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x00040D2D File Offset: 0x0003EF2D
	public void OnPointerExitDlc()
	{
		this.CheckCredit();
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001D2B54 File Offset: 0x001D0D54
	public void Open()
	{
		if (!this.IsEnabled && !base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
			this._controllerAnim.Show();
		}
		this._isEnabled = true;
		this.CheckLanguageOptionUsable();
		this.CheckDlcOptionUsable();
		this.currentOptionSetted.Clear();
		this.OnOpen();
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001D2BB4 File Offset: 0x001D0DB4
	public void Close()
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this._isEnabled = false;
		this._controllerAnim.Hide();
		if (this.BaseModBack != null)
		{
			UnityEngine.Object.Destroy(this.BaseModBack);
			this.BaseModBack = null;
		}
		GlobalGameManager.instance.SaveStateData();
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001D2C08 File Offset: 0x001D0E08
	public void OnClose()
	{
		if (GameManager.currentGameManager && GameManager.currentGameManager.state != GameState.STOP && PlaySpeedSettingUI.instance)
		{
			CameraMover.instance.ReleaseMove();
			PlaySpeedSettingUI.instance.OnResume(PAUSECALL.ESCAPE);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x00040D35 File Offset: 0x0003EF35
	public void OnOpen()
	{
		this.Opt_Display.Init();
		this.Opt_Texture.Init();
		this.Opt_Resolution.Init();
		this.Opt_Dlc.isOn = GlobalGameManager.instance.dlcCreatureOn;
		this.makeMODbutton();
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x00040D73 File Offset: 0x0003EF73
	public void OnSetLanguage(string language)
	{
		this.AddValueChanged(OptionUI.OptionAction.LANGUAGE, language);
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001D2C5C File Offset: 0x001D0E5C
	public void OnSetMasterVolume(float volume)
	{
		this.Opt_MasterFill.fillAmount = volume;
		if (BgmManager.instance != null)
		{
			BgmManager.instance.SetMasterSoundVolume(volume);
		}
		else if (StoryBgm.instance)
		{
			StoryBgm.instance.SetMasterVolume(volume);
		}
		AudioListener.volume = Mathf.Clamp(volume, 0f, 1f);
		GlobalGameManager.instance.sceneDataSaver.currentVolume = volume;
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001D2CCC File Offset: 0x001D0ECC
	public void OnSetMusicVolume(float volume)
	{
		this.Opt_MusicFill.fillAmount = volume;
		GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = volume;
		if (BgmManager.instance != null)
		{
			BgmManager.instance.SetBgmSoundVolume(volume);
		}
		else if (StoryBgm.instance)
		{
			StoryBgm.instance.SetBgmVolume(volume);
		}
		if (NewTitleScript.instance)
		{
			NewTitleScript.instance.TitleBgm.volume = volume;
		}
		if (AlterTitleController.Controller)
		{
			AlterTitleController.Controller.TitleBgm.volume = volume;
		}
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x00040D7D File Offset: 0x0003EF7D
	public void OnSetDisplayMode(int type)
	{
		this.AddValueChanged(OptionUI.OptionAction.DISPLAYMODE, type);
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x00040D8C File Offset: 0x0003EF8C
	public void OnSetTextureQuality(int quality)
	{
		this.AddValueChanged(OptionUI.OptionAction.TEXTUREQUALITY, quality);
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x00040D9B File Offset: 0x0003EF9B
	public void OnSetResolution(Resolution r)
	{
		this.AddValueChanged(OptionUI.OptionAction.RESOLUTION, r);
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x00040DAA File Offset: 0x0003EFAA
	public void OnSetTooltip()
	{
		if (this._initstate)
		{
			return;
		}
		GlobalGameManager.instance.sceneDataSaver.tooltipState = this.Opt_Tooltip.isOn;
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x00040DCF File Offset: 0x0003EFCF
	public void OnClickSaveAndQuit()
	{
		this.ExecuteActions();
		this.Close();
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x00040DDD File Offset: 0x0003EFDD
	private void AddValueChanged(OptionUI.OptionAction action, object value)
	{
		if (!this.currentOptionSetted.ContainsKey(action))
		{
			this.currentOptionSetted.Add(action, value);
			return;
		}
		this.currentOptionSetted[action] = value;
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001D2D60 File Offset: 0x001D0F60
	private T GetParam<T>(OptionUI.OptionAction action)
	{
		object obj = null;
		if (this.currentOptionSetted.TryGetValue(action, out obj))
		{
			return (T)((object)obj);
		}
		return default(T);
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x001D2D90 File Offset: 0x001D0F90
	public void ExecuteActions()
	{
		GameFullScreenMode mode = GameSettingModel.instance.GetCurrentFullScreenMode();
		Resolution resolution = new Resolution
		{
			width = Screen.width,
			height = Screen.height
		};
		int num = -1;
		string text = string.Empty;
		foreach (KeyValuePair<OptionUI.OptionAction, object> keyValuePair in this.currentOptionSetted)
		{
			switch (keyValuePair.Key)
			{
			case OptionUI.OptionAction.LANGUAGE:
				text = (string)keyValuePair.Value;
				break;
			case OptionUI.OptionAction.DISPLAYMODE:
				mode = (GameFullScreenMode)keyValuePair.Value;
				break;
			case OptionUI.OptionAction.TEXTUREQUALITY:
				num = (int)keyValuePair.Value;
				break;
			case OptionUI.OptionAction.RESOLUTION:
				resolution = (Resolution)keyValuePair.Value;
				break;
			}
		}
		if (num != -1)
		{
			GameSettingModel.instance.SetCurrentTextureQuality(num);
		}
		GameSettingModel.instance.SetResolution(resolution.width, resolution.height, mode);
		if (!string.IsNullOrEmpty(text))
		{
			this.ChangeLanguage(text);
		}
		this.ChangeDlc(this.Opt_Dlc.isOn);
		this.Opt_Display.Init();
		this.Opt_Texture.Init();
		this.Opt_Resolution.Init();
		this.Opt_Language.Init();
	}

	// Token: 0x06004EAB RID: 20139 RVA: 0x00040E08 File Offset: 0x0003F008
	public bool ValidateLanguageOption()
	{
		return !(AlterTitleController.Controller == null) || !(NewTitleScript.instance == null);
	}

	// Token: 0x06004EAC RID: 20140 RVA: 0x001D2EE4 File Offset: 0x001D10E4
	public void CheckLanguageOptionUsable()
	{
		if (!this.ValidateLanguageOption())
		{
			this.Opt_Language.dropDown.interactable = false;
			this._languageArrow.enabled = false;
			return;
		}
		this.Opt_Language.dropDown.interactable = true;
		this._languageArrow.enabled = true;
	}

	// Token: 0x06004EAD RID: 20141 RVA: 0x00040E27 File Offset: 0x0003F027
	public void CheckDlcOptionUsable()
	{
		if (!this.ValidateLanguageOption())
		{
			this.Opt_DlcRoot.SetActive(false);
			return;
		}
		this.Opt_DlcRoot.SetActive(true);
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x00040E4A File Offset: 0x0003F04A
	public void CheckTooltipState()
	{
		this._initstate = true;
		this.Opt_Tooltip.isOn = GlobalGameManager.instance.sceneDataSaver.tooltipState;
		this._initstate = false;
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001D2F34 File Offset: 0x001D1134
	public void ChangeLanguage(string ln)
	{
		if (NewTitleScript.instance)
		{
			NewTitleScript.instance.OnSetLanguage(ln);
			return;
		}
		if (AlterTitleController.Controller)
		{
			AlterTitleController.Controller.OnSetLanguage(ln);
			return;
		}
		Debug.Log("Could Not Change Language Because Current Scene is Not Title " + ln);
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x00040E74 File Offset: 0x0003F074
	public void ChangeDlc(bool b)
	{
		GlobalGameManager.instance.dlcCreatureOn = b;
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001D2FF4 File Offset: 0x001D11F4
	public void OnClickMODbutton()
	{
		if (!(this.BaseModBack != null))
		{
			GameObject gameObject = new GameObject("BackGround");
			Image image = gameObject.AddComponent<Image>();
			image.transform.SetParent(base.gameObject.transform);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/Back.png"));
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
			image.sprite = sprite;
			image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
			gameObject.transform.localScale = new Vector3(1f, 1f);
			gameObject.transform.localPosition = new Vector3(30f, 150f);
			gameObject.AddComponent<ModList>();
			gameObject.SetActive(true);
			this.BaseModBack = gameObject;
			return;
		}
		if (this.BaseModBack.active)
		{
			this.BaseModBack.SetActive(false);
			return;
		}
		this.BaseModBack.SetActive(true);
	}

	// Token: 0x06004EB3 RID: 20147 RVA: 0x001D3124 File Offset: 0x001D1324
	public void makeMODbutton()
	{
		if (this.BaseModButton != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("ModButton");
		Image image = gameObject.AddComponent<Image>();
		image.transform.SetParent(this.Opt_MusicVolume.transform.parent);
		Button button = gameObject.AddComponent<Button>();
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/ModButton.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
		image.sprite = sprite;
		image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
		button.targetGraphic = image;
		button.onClick.AddListener(delegate()
		{
			this.OnClickMODbutton();
		});
		gameObject.SetActive(true);
		gameObject.transform.localScale = new Vector2(1f, 1f);
		gameObject.transform.localPosition = new Vector2(-250f, -365f);
		this.BaseModButton = gameObject;
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x000043CD File Offset: 0x000025CD
	private void Update()
	{
	}

	// Token: 0x040048B1 RID: 18609
	public UIController _controllerAnim;

	// Token: 0x040048B2 RID: 18610
	private Dictionary<OptionUI.OptionAction, object> currentOptionSetted = new Dictionary<OptionUI.OptionAction, object>();

	// Token: 0x040048B3 RID: 18611
	private bool _isEnabled;

	// Token: 0x040048B4 RID: 18612
	public LanguageDropdown Opt_Language;

	// Token: 0x040048B5 RID: 18613
	public Toggle Opt_Tooltip;

	// Token: 0x040048B6 RID: 18614
	public GameObject Opt_DlcRoot;

	// Token: 0x040048B7 RID: 18615
	public Toggle Opt_Dlc;

	// Token: 0x040048B8 RID: 18616
	public Slider Opt_MasterVolume;

	// Token: 0x040048B9 RID: 18617
	public Image Opt_MasterFill;

	// Token: 0x040048BA RID: 18618
	public Slider Opt_MusicVolume;

	// Token: 0x040048BB RID: 18619
	public Image Opt_MusicFill;

	// Token: 0x040048BC RID: 18620
	public DisplayDropdown Opt_Display;

	// Token: 0x040048BD RID: 18621
	public TextureDropdown Opt_Texture;

	// Token: 0x040048BE RID: 18622
	public ResolutionDropdown Opt_Resolution;

	// Token: 0x040048BF RID: 18623
	public Text _languageTooltip;

	// Token: 0x040048C0 RID: 18624
	public Image _languageArrow;

	// Token: 0x040048C1 RID: 18625
	[Header("Translation Credit")]
	public GameObject CreditRoot;

	// Token: 0x040048C2 RID: 18626
	public Text CreditTitle;

	// Token: 0x040048C3 RID: 18627
	public Text CreditLabel;

	// Token: 0x040048C4 RID: 18628
	public Text OverlayDesc;

	// Token: 0x040048C5 RID: 18629
	private static string[] credit = new string[]
	{
		"Poten / Ro / KevinGlass / ade007 / Amiba / Sea / Nicholas_Okra",
		"acane / 甘輪 / いすひろし / とらきす / 翻訳協力者" + Environment.NewLine + "もちみかん / Youkan / ログノ / 6人目のサメの餌",
		"surolanter",
		"Tales&Stories Team / GregorLesnov / BlinkRaven",
		"Misui",
		"Dimitar Topkov (di_TOP)",
		"Main Translator : @UrathObsidian" + Environment.NewLine + "Programmer/Language Assistant : @casual_watson "
	};

	// Token: 0x040048C6 RID: 18630
	private Dictionary<string, string> creditText = new Dictionary<string, string>();

	// Token: 0x040048C7 RID: 18631
	private bool _initstate;

	// Token: 0x040048C8 RID: 18632
	public GameObject BaseModButton;

	// Token: 0x040048C9 RID: 18633
	public GameObject BaseModBack;

	// Token: 0x040048CA RID: 18634
	public GameObject BaseModTest;

	// Token: 0x02000A1B RID: 2587
	public enum OptionAction
	{
		// Token: 0x040048CC RID: 18636
		LANGUAGE,
		// Token: 0x040048CD RID: 18637
		DISPLAYMODE,
		// Token: 0x040048CE RID: 18638
		TEXTUREQUALITY,
		// Token: 0x040048CF RID: 18639
		RESOLUTION
	}
}
