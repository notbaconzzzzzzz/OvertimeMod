/*
private void OnChangeAgent() // Ego Gift Helper
public static AgentInfoWindow EnforcementWindow() // Recustomizing
public void OnClearOverlay() // Ego Gift Helper
private void InitGiftArea() // Hod/Keter service bonuses
UIComponent public void SetData(AgentModel agent) // (!) Display employee's base stats and fixed stat tooltips
InGameComponent public void SetUI(AgentModel agent) // (!) Resistances will use 2 decimal points; fixed stat tooltips
WorkerPrimaryStatUI public void SetStat(AgentModel agent) // Display employee's current exp and base stats
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using Customizing;
using InGameUI;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E4 RID: 2532
public class AgentInfoWindow : MonoBehaviour, IObserver, IDeployResetCalled
{
    // Token: 0x06004CA6 RID: 19622 RVA: 0x00004080 File Offset: 0x00002280
    public AgentInfoWindow()
    {
    }

    // Token: 0x17000712 RID: 1810
    // (get) Token: 0x06004CA7 RID: 19623 RVA: 0x0003ED22 File Offset: 0x0003CF22
    // (set) Token: 0x06004CA8 RID: 19624 RVA: 0x0003ED2A File Offset: 0x0003CF2A
    public bool IsGiftAreaEnabled
    {
        get
        {
            return this._isGiftAreaEnabled;
        }
        private set
        {
            this._isGiftAreaEnabled = value;
            this.GiftRoot.gameObject.SetActive(this._isGiftAreaEnabled);
        }
    }

    // Token: 0x17000713 RID: 1811
    // (get) Token: 0x06004CA9 RID: 19625 RVA: 0x0003ED49 File Offset: 0x0003CF49
    // (set) Token: 0x06004CAA RID: 19626 RVA: 0x0003ED51 File Offset: 0x0003CF51
    public AgentInfoWindow.AgentInfoWindowType WindowType { get; private set; }

    // Token: 0x17000714 RID: 1812
    // (get) Token: 0x06004CAB RID: 19627 RVA: 0x0003ED5A File Offset: 0x0003CF5A
    public AgentModel PinnedAgent
    {
        get
        {
            return this._pinnedAgent;
        }
    }

    // Token: 0x17000715 RID: 1813
    // (get) Token: 0x06004CAC RID: 19628 RVA: 0x0003ED62 File Offset: 0x0003CF62
    // (set) Token: 0x06004CAD RID: 19629 RVA: 0x0003ED6A File Offset: 0x0003CF6A
    public AgentModel CurrentAgent
    {
        get
        {
            return this._currentAgent;
        }
        set
        {
            this.OnChangeAgent(value);
        }
    }

    // Token: 0x17000716 RID: 1814
    // (get) Token: 0x06004CAE RID: 19630 RVA: 0x0003ED73 File Offset: 0x0003CF73
    // (set) Token: 0x06004CAF RID: 19631 RVA: 0x0003ED7B File Offset: 0x0003CF7B
    public bool IsEnabled
    {
        get
        {
            return this._isEanbled;
        }
        set
        {
            this._isEanbled = value;
            AgentInfoWindow.currentWindow.gameObject.SetActive(value);
        }
    }

    // Token: 0x06004CB0 RID: 19632 RVA: 0x001C3184 File Offset: 0x001C1384
    public static AgentInfoWindow GenerateWindow()
    {
        AgentInfoWindow.currentWindow.IsEnabled = true;
        AgentInfoWindow.currentWindow.UIComponents.portrait.SetAgentArmor();
        AgentInfoWindow.currentWindow.customizingWindow.buttonControl.SetActive(true);
        AgentInfoWindow.currentWindow.customizingBlock.SetActive(true);
        AgentInfoWindow.currentWindow.UIComponents.portrait.SetAgentArmor();
        AgentInfoWindow.currentWindow.AppearanceActiveControl.SetActive(true);
        CustomizingWindow.GenerationWindow();
        AgentInfoWindow.currentWindow.UIComponents.SetData(CustomizingWindow.CurrentWindow.CurrentData);
        AgentInfoWindow.currentWindow.EnforcenButton.gameObject.SetActive(false);
        AgentInfoWindow.currentWindow.DeployActiveControl.SetActive(true);
        AgentInfoWindow.currentWindow.IngameActiveControl.SetActive(false);
        AgentInfoWindow.currentWindow.SetGiftButtonInteractable(false);
        AgentInfoWindow.currentWindow.customizingWindow.IsEnabled = true;
        AgentInfoWindow.currentWindow.WindowType = AgentInfoWindow.AgentInfoWindowType.GENERATE;
        return AgentInfoWindow.currentWindow;
    }

    // Token: 0x06004CB1 RID: 19633 RVA: 0x001C3278 File Offset: 0x001C1478
    public static AgentInfoWindow EnforcementWindow()
    { // <Mod>
		AgentInfoWindow.currentWindow.gameObject.transform.localScale = Vector3.one;
        AgentInfoWindow.currentWindow.IsEnabled = true;
        AgentInfoWindow.currentWindow.customizingWindow.buttonControl.SetActive(true);
        AgentInfoWindow.currentWindow.customizingBlock.SetActive(true);
        AgentInfoWindow.currentWindow.AppearanceActiveControl.SetActive(true);
        AgentInfoWindow.currentWindow.UIComponents.portrait.SetAgentArmor();
        CustomizingWindow.ReviseWindow(AgentInfoWindow.currentWindow.PinnedAgent);
		AgentInfoWindow.currentWindow.UIComponents.SetData(CustomizingWindow.CurrentWindow.CurrentData);
        AgentInfoWindow.currentWindow.EnforcenButton.gameObject.SetActive(false);
        AgentInfoWindow.currentWindow.DeployActiveControl.SetActive(true);
        AgentInfoWindow.currentWindow.IngameActiveControl.SetActive(false);
        AgentInfoWindow.currentWindow.SetGiftButtonInteractable(true);
        AgentInfoWindow.currentWindow.customizingWindow.IsEnabled = true;
        AgentInfoWindow.currentWindow.WindowType = AgentInfoWindow.AgentInfoWindowType.ENFORCEMENT;
        return AgentInfoWindow.currentWindow;
    }

    // Token: 0x06004CB2 RID: 19634 RVA: 0x001C3348 File Offset: 0x001C1548
    public static AgentInfoWindow CreateWindow(AgentModel target, bool forcely = false)
    { // <Mod>
        if (AgentInfoWindow.currentWindow.IsEnabled && !forcely && AgentInfoWindow.currentWindow.WindowType != AgentInfoWindow.AgentInfoWindowType.NORMAL)
        {
            return null;
        }
		AgentInfoWindow.currentWindow.gameObject.transform.localScale = Vector3.one * SpecialModeConfig.instance.GetValue<float>("WindowScaleAgent");
        AgentInfoWindow.currentWindow.customizingWindow.IsEnabled = false;
        AgentInfoWindow.currentWindow.customizingWindow.buttonControl.SetActive(false);
        AgentInfoWindow.currentWindow.customizingBlock.SetActive(false);
        if (GameManager.currentGameManager.state == GameState.STOP)
        {
            AgentInfoWindow.currentWindow.EnforcenButton.gameObject.SetActive(true);
            AgentInfoWindow.currentWindow.UIComponents.SetData(target);
            AgentInfoWindow.currentWindow.IngameActiveControl.SetActive(false);
            AgentInfoWindow.currentWindow.DeployActiveControl.SetActive(true);
        }
        else
        {
            AgentInfoWindow.currentWindow.EnforcenButton.gameObject.SetActive(false);
            AgentInfoWindow.currentWindow.inGameModeComponent.SetUI(target);
            AgentInfoWindow.currentWindow.IngameActiveControl.SetActive(true);
            AgentInfoWindow.currentWindow.DeployActiveControl.SetActive(false);
        }
        AgentInfoWindow.currentWindow.WindowType = AgentInfoWindow.AgentInfoWindowType.NORMAL;
        if (AgentInfoWindow.currentWindow.IsEnabled)
        {
            if (AgentInfoWindow.currentWindow.CurrentAgent == target)
            {
                return AgentInfoWindow.currentWindow;
            }
            if (AgentInfoWindow.currentWindow.CurrentAgent != null)
            {
                AgentUnit agent = AgentLayer.currentLayer.GetAgent(AgentInfoWindow.currentWindow.CurrentAgent.instanceId);
                if (agent != null)
                {
                    agent.clicked = false;
                }
            }
        }
        else
        {
            AgentInfoWindow.currentWindow.IsEnabled = true;
        }
        AgentInfoWindow.currentWindow.CurrentAgent = target;
        AgentInfoWindow.currentWindow.SetGiftAreaAgent(target);
        AgentInfoWindow.currentWindow.SetGiftButtonInteractable(true);
        return AgentInfoWindow.currentWindow;
    }

    // Token: 0x06004CB3 RID: 19635 RVA: 0x0003ED94 File Offset: 0x0003CF94
    public void PinCurrentAgent()
    {
        this._pinnedAgent = this._currentAgent;
        this.InitGiftArea();
    }

    // Token: 0x06004CB4 RID: 19636 RVA: 0x001C34D4 File Offset: 0x001C16D4
    public void OnClearOverlay()
    { // <Mod> Ego Gift Helper
        if (this.PinnedAgent != null)
        {
            this.UIComponents.SetData(this.PinnedAgent);
            if (GameManager.currentGameManager.state == GameState.STOP)
            {
                this.UIComponents.SetData(this.PinnedAgent);
            }
            else
            {
                this.inGameModeComponent.SetUI(this.PinnedAgent);
            }
            this.SetGiftAreaAgent(this.PinnedAgent);
            CurrentAgent = PinnedAgent;
            return;
        }
        this.CloseWindow();
    }

    // Token: 0x06004CB5 RID: 19637 RVA: 0x0003EDA8 File Offset: 0x0003CFA8
    public void UnPinCurrentAgent()
    {
        this._pinnedAgent = null;
    }

    // Token: 0x06004CB6 RID: 19638 RVA: 0x001C3540 File Offset: 0x001C1740
    public void CloseWindow()
    {
        AgentInfoWindow.currentWindow.IsEnabled = false;
        if (AgentInfoWindow.currentWindow.CurrentAgent != null && GameManager.currentGameManager.state != GameState.STOP)
        {
            AgentUnit agent = AgentLayer.currentLayer.GetAgent(AgentInfoWindow.currentWindow.CurrentAgent.instanceId);
            if (agent != null)
            {
                agent.clicked = false;
            }
        }
        if (this._pinnedAgent != null)
        {
            this.UnPinCurrentAgent();
        }
    }

    // Token: 0x06004CB7 RID: 19639 RVA: 0x001C35AC File Offset: 0x001C17AC
    public void OnClickPortrait()
    {
        Vector2 vector = AgentInfoWindow.currentWindow.CurrentAgent.GetCurrentViewPosition();
        Camera.main.transform.position = new Vector3(vector.x, vector.y, Camera.main.transform.position.z);
    }

    // Token: 0x06004CB8 RID: 19640 RVA: 0x0003EDB1 File Offset: 0x0003CFB1
    private void Awake()
    {
        AgentInfoWindow.currentWindow = this;
        this.IsEnabled = false;
        this.EnforcenButton.gameObject.SetActive(false);
        AgentInfoWindow.currentWindow._currentAgent = null;
        AgentInfoWindow.currentWindow.customizingWindow.AgentInfoWindowInit();
    }

    // Token: 0x06004CB9 RID: 19641 RVA: 0x0003EDEB File Offset: 0x0003CFEB
    private void Start()
    {
        this.Registration();
    }

    // Token: 0x06004CBA RID: 19642 RVA: 0x001C3604 File Offset: 0x001C1804
    private void SetGiftAreaAgent(AgentModel agent)
    {
        try
        {
            if (this.giftWindow != null)
            {
                this.giftWindow.SetAgent(agent);
            }
        }
        catch (Exception message)
        {
            Debug.LogError(message);
        }
    }

    // Token: 0x06004CBB RID: 19643 RVA: 0x001C3644 File Offset: 0x001C1844
    private void InitGiftArea()
    { // <Mod> Hod alternative service bonus; Keter alternative service bonus
        if (this.PinnedAgent == null)
        {
            return;
        }
        try
        {
            Sefira sefira = SefiraManager.instance.GetSefira(this.PinnedAgent.lastServiceSefira);
            if (sefira != null)
            {
                Sefira sefira2 = sefira;
                SefiraEnum sefiraEnum = sefira.sefiraEnum;
                if (sefiraEnum == SefiraEnum.TIPERERTH2)
                {
                    sefiraEnum = SefiraEnum.TIPERERTH1;
                    sefira2 = SefiraManager.instance.GetSefira(sefiraEnum);
                }
                this.GiftSlot_SymbolImage.gameObject.SetActive(true);
                this.GiftSlot_SymbolImage.sprite = WorkerSpriteManager.instance.GetSefiraSymbol(sefira.sefiraEnum, this.PinnedAgent.GetContinuousServiceLevel());
                this.GiftSlot_Title.text = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_title"), LocalizeTextDataModel.instance.GetTextAppend(new string[]
                {
                    SefiraName.GetSefiraByEnum(sefiraEnum),
                    "Name"
                }), this.PinnedAgent.continuousServiceDay);
                if (sefira.sefiraEnum == SefiraEnum.HOD && this.PinnedAgent.GetContinuousServiceLevel() >= 4)
                {
                    GiftSlot_EffectText.text = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_hod_alt"), SefiraAbilityValueInfo.GetContinuousServiceValues(sefira.sefiraEnum)[PinnedAgent.GetContinuousServiceLevel() - 1]);
                }
                else if (sefira.sefiraEnum == SefiraEnum.KETHER)
                {
                    GiftSlot_EffectText.text = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_kether"), SefiraAbilityValueInfo.ketherContinuousServiceValues[PinnedAgent.GetContinuousServiceLevel() - 1], SefiraAbilityValueInfo.ketherContinuousServiceValues2[PinnedAgent.GetContinuousServiceLevel() - 1]);
                }
                else
                {
                    this.GiftSlot_EffectText.text = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_" + sefira2.name.ToLower()), SefiraAbilityValueInfo.GetContinuousServiceValues(sefira.sefiraEnum)[this.PinnedAgent.GetContinuousServiceLevel() - 1]);
                }
            }
            else
            {
                this.GiftSlot_SymbolImage.gameObject.SetActive(false);
                this.GiftSlot_Title.text = string.Empty;
                this.GiftSlot_EffectText.text = string.Empty;
            }
            IEnumerator enumerator = this.GiftSlotParent.GetEnumerator();
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
            foreach (EquipmentModel equipmentModel in this.PinnedAgent.Equipment.gifts.addedGifts)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GiftSlot);
                gameObject.GetComponent<Text>().text = equipmentModel.metaInfo.Name + " : " + equipmentModel.metaInfo.Description;
                gameObject.transform.SetParent(this.GiftSlotParent, false);
                gameObject.transform.localScale = Vector3.one;
            }
            foreach (EquipmentModel equipmentModel2 in this.PinnedAgent.Equipment.gifts.replacedGifts)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.GiftSlot);
                gameObject2.GetComponent<Text>().text = equipmentModel2.metaInfo.Name + " : " + equipmentModel2.metaInfo.Description;
                gameObject2.transform.SetParent(this.GiftSlotParent, false);
                gameObject2.transform.localScale = Vector3.one;
            }
        }
        catch (Exception message)
        {
            Debug.LogError(message);
        }
    }

    // Token: 0x06004CBC RID: 19644 RVA: 0x001C39C8 File Offset: 0x001C1BC8
    public void SetGiftButtonInteractable(bool state)
    {
        this.IsGiftAreaEnabled = false;
        foreach (Button button in this.GiftAreaActiveButton)
        {
            button.interactable = state;
        }
    }

    // Token: 0x06004CBD RID: 19645 RVA: 0x001C3A20 File Offset: 0x001C1C20
    public void OnClickGiftArea(float position)
    {
        this.IsGiftAreaEnabled = !this.IsGiftAreaEnabled;
        Vector2 anchoredPosition = this.GiftRoot.anchoredPosition;
        anchoredPosition.x = position;
        this.GiftRoot.anchoredPosition = anchoredPosition;
    }

    // Token: 0x06004CBE RID: 19646 RVA: 0x00003E21 File Offset: 0x00002021
    private void Update()
    {
    }

    // Token: 0x06004CBF RID: 19647 RVA: 0x00003E21 File Offset: 0x00002021
    private void Init()
    {
    }

    // Token: 0x06004CC0 RID: 19648 RVA: 0x0003EDF3 File Offset: 0x0003CFF3
    private void OnEnable()
    {
        Notice.instance.Observe("AgentDie", this);
        if (this.CurrentAgent != null)
        {
            Notice.instance.Observe(NoticeName.UpdateAgentState + this.CurrentAgent.instanceId, this);
        }
    }

    // Token: 0x06004CC1 RID: 19649 RVA: 0x0003EE32 File Offset: 0x0003D032
    private void OnDisable()
    {
        Notice.instance.Remove("AgentDie", this);
        if (this.CurrentAgent != null)
        {
            Notice.instance.Remove(NoticeName.UpdateAgentState + this.CurrentAgent.instanceId, this);
        }
    }

    // Token: 0x06004CC2 RID: 19650 RVA: 0x0003EE71 File Offset: 0x0003D071
    public void OnUpdateAgent()
    {
        this.UIComponents.SetData(this.CurrentAgent);
    }

    // Token: 0x06004CC3 RID: 19651 RVA: 0x0003EE84 File Offset: 0x0003D084
    public void OnNotice(string notice, params object[] param)
    {
        if (notice.Split(new char[]
        {
            '_'
        })[0] == "UpdateAgentState")
        {
            this.OnUpdateAgent();
            return;
        }
        if (notice == "AgentDie")
        {
            this.CurrentAgent = null;
        }
    }

    // Token: 0x06004CC4 RID: 19652 RVA: 0x001C3A5C File Offset: 0x001C1C5C
    private void OnChangeAgent(AgentModel newAgent)
    { // <Mod> Ego Gift Healper
        if (this._currentAgent != null)
        {
            Notice.instance.Remove(NoticeName.UpdateAgentState + this.CurrentAgent.instanceId, this);
        }
        this._currentAgent = newAgent;
        if (newAgent != null)
        {
            Notice.instance.Observe(NoticeName.UpdateAgentState + this.CurrentAgent.instanceId, this);
        }
        if (SpecialModeConfig.instance.GetValue<bool>("EgoGiftHelper"))
		{
			foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
			{
				creature.Unit.room.UpdateGiftHealper();
			}
		}
    }

    // Token: 0x06004CC5 RID: 19653 RVA: 0x0003EEC0 File Offset: 0x0003D0C0
    public static int GetValueRate_FiveStep(float Max, float value)
    {
        return (int)(value / Max * 100f / 20f);
    }

    // Token: 0x06004CC6 RID: 19654 RVA: 0x0003D6A8 File Offset: 0x0003B8A8
    public void Registration()
    {
        DeployUI.instance.RegistDeployReset(this);
    }

    // Token: 0x06004CC7 RID: 19655 RVA: 0x0003EED2 File Offset: 0x0003D0D2
    public void DeployResetCalled()
    {
        if (DeployUI.instance.IsEnabled)
        {
            this.UnPinCurrentAgent();
            this.CloseWindow();
        }
    }

    // Token: 0x06004CC8 RID: 19656 RVA: 0x0003EEEC File Offset: 0x0003D0EC
    private Color GetCurrentColor()
    {
        if (this.CurrentAgent == null)
        {
            return Color.white;
        }
        return DeployUI.instance.CurrentDeployColor;
    }

    // Token: 0x06004CC9 RID: 19657 RVA: 0x0003EF06 File Offset: 0x0003D106
    public void EquipEvent()
    {
        this.OnUpdateAgent();
    }

    // Token: 0x06004CCA RID: 19658 RVA: 0x00003E21 File Offset: 0x00002021
    public void DeployColorSetted(Color c)
    {
    }

    // Token: 0x06004CCB RID: 19659 RVA: 0x0003EF0E File Offset: 0x0003D10E
    public void OnClickEnforceButton()
    {
        AgentInfoWindow.EnforcementWindow();
        OverlayManager.Instance.ClearOverlay();
    }

    // Token: 0x06004CCC RID: 19660 RVA: 0x001C3AC8 File Offset: 0x001C1CC8
    static AgentInfoWindow()
    {
    }

    // Token: 0x04004705 RID: 18181
    private const float _fillMin = 0f;

    // Token: 0x04004706 RID: 18182
    private const float _fillMax = 0f;

    // Token: 0x04004707 RID: 18183
    public static Vector3 MainPivotPos = new Vector3(-287.5f, -218.5f, 0f);

    // Token: 0x04004708 RID: 18184
    public static Vector3 SubPivotPos = new Vector3(-287.5f, -270.5f, 0f);

    // Token: 0x04004709 RID: 18185
    private static float[] _fillLevel = new float[]
    {
        0f,
        0.3f,
        0.5f,
        0.7f,
        1f
    };

    // Token: 0x0400470A RID: 18186
    [HideInInspector]
    public static AgentInfoWindow currentWindow = null;

    // Token: 0x0400470B RID: 18187
    public Canvas rootCanvas;

    // Token: 0x0400470C RID: 18188
    [Space(10f)]
    public AgentInfoWindow.UIComponent UIComponents;

    // Token: 0x0400470D RID: 18189
    [Space(10f)]
    public AgentInfoWindow.InGameModeComponent inGameModeComponent;

    // Token: 0x0400470E RID: 18190
    [Space(10f)]
    public GameObject DeployActiveControl;

    // Token: 0x0400470F RID: 18191
    public GameObject IngameActiveControl;

    // Token: 0x04004710 RID: 18192
    public CustomizingWindow customizingWindow;

    // Token: 0x04004711 RID: 18193
    public GameObject customizingBlock;

    // Token: 0x04004712 RID: 18194
    public GameObject AppearanceActiveControl;

    // Token: 0x04004713 RID: 18195
    public Button EnforcenButton;

    // Token: 0x04004714 RID: 18196
    public Sprite[] LevelImage;

    // Token: 0x04004715 RID: 18197
    public string Additional_Plus_ValueColor;

    // Token: 0x04004716 RID: 18198
    public string Additional_Minus_ValueColor;

    // Token: 0x04004717 RID: 18199
    private bool _isGiftAreaEnabled;

    // Token: 0x04004718 RID: 18200
    [Header("Gift Area")]
    public RectTransform GiftRoot;

    // Token: 0x04004719 RID: 18201
    public Text GiftSlot_Title;

    // Token: 0x0400471A RID: 18202
    public Text GiftSlot_EffectText;

    // Token: 0x0400471B RID: 18203
    public Image GiftSlot_SymbolImage;

    // Token: 0x0400471C RID: 18204
    public RectTransform GiftSlotParent;

    // Token: 0x0400471D RID: 18205
    public GameObject GiftSlot;

    // Token: 0x0400471E RID: 18206
    public List<Button> GiftAreaActiveButton;

    // Token: 0x0400471F RID: 18207
    [Header("Gift Window")]
    public AgentGiftWindow giftWindow;

    // Token: 0x04004721 RID: 18209
    [NonSerialized]
    private AgentModel _pinnedAgent;

    // Token: 0x04004722 RID: 18210
    [NonSerialized]
    private AgentModel _currentAgent;

    // Token: 0x04004723 RID: 18211
    private bool _isEanbled;

    // Token: 0x020009E5 RID: 2533
    public enum AgentInfoWindowType
    {
        // Token: 0x04004725 RID: 18213
        GENERATE,
        // Token: 0x04004726 RID: 18214
        ENFORCEMENT,
        // Token: 0x04004727 RID: 18215
        NORMAL
    }

    // Token: 0x020009E6 RID: 2534
    [Serializable]
    public class FillComponent
    {
        // Token: 0x06004CCD RID: 19661 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public FillComponent()
        {
        }

        // Token: 0x06004CCE RID: 19662 RVA: 0x0003EF20 File Offset: 0x0003D120
        public void SetFillLevel(int level)
        {
            if (level < 0)
            {
                level = 0;
            }
            if (level > 4)
            {
                level = 4;
            }
            this.Fill.fillAmount = AgentInfoWindow._fillLevel[level];
            this.Icon.sprite = AgentInfoWindow.currentWindow.LevelImage[level];
        }

        // Token: 0x04004728 RID: 18216
        public Image Icon;

        // Token: 0x04004729 RID: 18217
        public Image Fill;

        // Token: 0x0400472A RID: 18218
        public Text FillText;
    }

    // Token: 0x020009E7 RID: 2535
    [Serializable]
    public class StatSlot
    {
        // Token: 0x06004CCF RID: 19663 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public StatSlot()
        {
        }

        // Token: 0x06004CD0 RID: 19664 RVA: 0x001C3B24 File Offset: 0x001C1D24
        public void SetText(string vanlia)
        {
            foreach (Text text in this.Vanila)
            {
                text.text = vanlia;
            }
            this.NoAddition.SetActive(true);
            this.ContainsAddition.SetActive(false);
        }

        // Token: 0x06004CD1 RID: 19665 RVA: 0x001C3B90 File Offset: 0x001C1D90
        public void SetText(string vanila, string addition)
        {
            foreach (Text text in this.Vanila)
            {
                text.text = vanila;
            }
            this.Addition.text = addition;
            this.NoAddition.SetActive(false);
            this.ContainsAddition.SetActive(true);
        }

        // Token: 0x0400472B RID: 18219
        public Text StatName;

        // Token: 0x0400472C RID: 18220
        public GameObject NoAddition;

        // Token: 0x0400472D RID: 18221
        public GameObject ContainsAddition;

        // Token: 0x0400472E RID: 18222
        public List<Text> Vanila;

        // Token: 0x0400472F RID: 18223
        public Text Addition;
    }

    // Token: 0x020009E8 RID: 2536
    [Serializable]
    public class StatObject
    {
        // Token: 0x06004CD2 RID: 19666 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public StatObject()
        {
        }

        // Token: 0x04004730 RID: 18224
        public Image Fill;

        // Token: 0x04004731 RID: 18225
        public Text Fill_Inner;

        // Token: 0x04004732 RID: 18226
        public Text StatName;

        // Token: 0x04004733 RID: 18227
        public AgentInfoWindow.StatSlot[] slots;
    }

    // Token: 0x020009E9 RID: 2537
    [Serializable]
    public class UIComponent
    {
        // Token: 0x06004CD3 RID: 19667 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public UIComponent()
        {
        }

        // Token: 0x06004CD4 RID: 19668 RVA: 0x00003E21 File Offset: 0x00002021
        public void SetColorData()
        {
        }

        // Token: 0x06004CD5 RID: 19669 RVA: 0x00003E21 File Offset: 0x00002021
        public void Start()
        {
        }

        // Token: 0x06004CD6 RID: 19670 RVA: 0x001C3C08 File Offset: 0x001C1E08
        public void SetData(AgentData agentData)
        {
            if (agentData.originalModel == null)
            {
                this.SetColorData();
                this.GradeImage.sprite = DeployUI.GetGradeSprite(agentData.GetVanliaLevel());
                this.AgentName.text = agentData.agentName.GetName();
                this.AgentTitle.enabled = false;
                this.Stat_R.slots[0].SetText(agentData.stat.maxHP + string.Empty);
                this.Stat_W.slots[0].SetText(agentData.stat.maxMental + string.Empty);
                this.Stat_B.slots[0].SetText(agentData.stat.workProb + string.Empty);
                this.Stat_B.slots[1].SetText(agentData.stat.cubeSpeed + string.Empty);
                this.Stat_P.slots[0].SetText(agentData.stat.attackSpeed + string.Empty);
                this.Stat_P.slots[1].SetText(agentData.stat.movementSpeed + string.Empty);
                this.Stat_R.Fill_Inner.text = AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.hp));
                this.Stat_W.Fill_Inner.text = AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.mental));
                this.Stat_B.Fill_Inner.text = AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.work));
                this.Stat_P.Fill_Inner.text = AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.battle));
                this.Stat_R.Fill_Inner.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Rstat"), AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.hp)));
                this.Stat_W.Fill_Inner.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Wstat"), AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.mental)));
                this.Stat_B.Fill_Inner.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Bstat"), AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.work)));
                this.Stat_P.Fill_Inner.text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText("Pstat"), AgentModel.GetLevelGradeText(AgentModel.CalculateStatLevel(agentData.stat.battle)));
                WeaponModel dummyWeapon = WeaponModel.GetDummyWeapon();
                this.Weapon.StatName.text = dummyWeapon.metaInfo.Name;
                DamageInfo damageInfo = dummyWeapon.metaInfo.damageInfo;
                RwbpType type = damageInfo.type;
                this.Weapon.Fill_Inner.text = EnumTextConverter.GetRwbpType(type).ToUpper();
                Color color;
                Color color2;
                UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
                this.Weapon.Fill_Inner.color = color;
                this.Weapon.Fill.color = Color.white;
                this.Weapon.Fill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
                string text = string.Format("{0}-{1}", (int)damageInfo.min, (int)damageInfo.max);
                this.Weapon.slots[0].SetText(text);
                ArmorModel dummyArmor = ArmorModel.GetDummyArmor();
                DefenseInfo defenseInfo = dummyArmor.metaInfo.defenseInfo;
                UIUtil.DefenseSetOnlyText(defenseInfo, this.DefenseType);
                UIUtil.SetDefenseTypeIcon(defenseInfo, this.DefenseTypeRenderer);
                this.ArmorName.text = dummyArmor.metaInfo.Name;
                InventoryItemController.SetGradeText(dummyWeapon.metaInfo.Grade, this.WeaponGrade);
                InventoryItemController.SetGradeText(dummyArmor.metaInfo.Grade, this.ArmorGrade);
                return;
            }
            this.SetData(agentData.originalModel);
        }

        // Token: 0x06004CD7 RID: 19671 RVA: 0x001C4048 File Offset: 0x001C2248
        public void SetData(AgentModel agent)
        { // <Patch> <Mod> Base stats are now displayed; also fixed Ws, As, and Ms stat tooltips so that they show the correct number instead of all saying stat * 0.2%;
        // also cut off names if they're too long; also made weapon damage update with damage multipliers
            if (agent == null)
            {
                return;
            }
            this.SetColorData();
            this.AgentTitle.enabled = true;
            this.GradeImage.sprite = DeployUI.GetAgentGradeSprite(agent);
            this.AgentName.text = agent.GetUnitName();
            this.AgentTitle.text = agent.GetTitle();
            this.portrait.SetWorker(agent);
            WorkerPrimaryStatBonus titleBonus = agent.titleBonus;
            int originFortitudeStat = agent.originFortitudeStat;
            int originPrudenceStat = agent.originPrudenceStat;
            int originTemperanceStat = agent.originTemperanceStat;
            int originTemperanceStat2 = agent.originTemperanceStat;
            int originJusticeStat = agent.originJusticeStat;
            int originJusticeStat2 = agent.originJusticeStat;
            int num = agent.maxHp - originFortitudeStat;
            int num2 = agent.maxMental - originPrudenceStat;
            int num3 = agent.workProb - originTemperanceStat;
            int num4 = agent.workSpeed - originTemperanceStat2;
            int num5 = (int)agent.attackSpeed - originJusticeStat;
            int num6 = (int)agent.movement - originJusticeStat2;
            if (num > 0)
            {
                this.Stat_R.slots[0].SetText(originFortitudeStat + string.Empty, "+" + num);
            }
            else if (num < 0)
            {
                this.Stat_R.slots[0].SetText(originFortitudeStat + string.Empty, "-" + -num);
            }
            else
            {
                this.Stat_R.slots[0].SetText(originFortitudeStat + string.Empty);
            }
            if (num2 > 0)
            {
                this.Stat_W.slots[0].SetText(originPrudenceStat + string.Empty, "+" + num2);
            }
            else if (num2 < 0)
            {
                this.Stat_W.slots[0].SetText(originPrudenceStat + string.Empty, "-" + -num2);
            }
            else
            {
                this.Stat_W.slots[0].SetText(originPrudenceStat + string.Empty);
            }
            if (num3 > 0)
            {
                this.Stat_B.slots[0].SetText(originTemperanceStat + string.Empty, "+" + num3);
            }
            else if (num3 < 0)
            {
                this.Stat_B.slots[0].SetText(originTemperanceStat + string.Empty, "-" + -num3);
            }
            else
            {
                this.Stat_B.slots[0].SetText(originTemperanceStat + string.Empty);
            }
            if (num4 > 0)
            {
                this.Stat_B.slots[1].SetText(originTemperanceStat2 + string.Empty, "+" + num4);
            }
            else if (num4 < 0)
            {
                this.Stat_B.slots[1].SetText(originTemperanceStat2 + string.Empty, "-" + -num4);
            }
            else
            {
                this.Stat_B.slots[1].SetText(originTemperanceStat2 + string.Empty);
            }
            if (num5 > 0)
            {
                this.Stat_P.slots[0].SetText(originJusticeStat + string.Empty, "+" + num5);
            }
            else if (num5 < 0)
            {
                this.Stat_P.slots[0].SetText(originJusticeStat + string.Empty, "-" + -num5);
            }
            else
            {
                this.Stat_P.slots[0].SetText(originJusticeStat + string.Empty);
            }
            if (num6 > 0)
            {
                this.Stat_P.slots[1].SetText(originJusticeStat2 + string.Empty, "+" + num6);
            }
            else if (num6 < 0)
            {
                this.Stat_P.slots[1].SetText(originJusticeStat2 + string.Empty, "-" + -num6);
            }
            else
            {
                this.Stat_P.slots[1].SetText(originJusticeStat2 + string.Empty);
            }
			string name;
			name = LocalizeTextDataModel.instance.GetText("Rstat");
			if (name.Length > 5)
			{
				name = name.Substring(0, 4);
			}
            this.Stat_R.Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(agent.Rstat), agent.primaryStat.maxHP);
			name = LocalizeTextDataModel.instance.GetText("Wstat");
			if (name.Length > 5)
			{
				name = name.Substring(0, 4);
			}
            this.Stat_W.Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(agent.Wstat), agent.primaryStat.maxMental);
			name = LocalizeTextDataModel.instance.GetText("Bstat");
			if (name.Length > 5)
			{
				name = name.Substring(0, 4);
			}
            this.Stat_B.Fill_Inner.text = string.Format("{0}{3}{1} ({2})", name, AgentModel.GetLevelGradeText(agent.Bstat), agent.primaryStat.workProb, "\n");
			name = LocalizeTextDataModel.instance.GetText("Pstat");
			if (name.Length > 5)
			{
				name = name.Substring(0, 4);
			}
            this.Stat_P.Fill_Inner.text = string.Format("{0}{3}{1} ({2})", name, AgentModel.GetLevelGradeText(agent.Pstat), agent.primaryStat.attackSpeed, "\n");
            this.Weapon.StatName.text = agent.Equipment.weapon.metaInfo.Name;
            if (EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) == 200038 || EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) == 200004)
            {
                DamageInfo damage = agent.Equipment.weapon.GetDamage(agent);
                RwbpType type = damage.type;
                this.Weapon.Fill_Inner.text = "???";
                this.Weapon.Fill_Inner.color = Color.gray;
                this.Weapon.Fill.color = Color.white;
                this.Weapon.Fill.enabled = false;
                float num7 = agent.GetDamageFactorByEquipment();
                num7 *= agent.GetDamageFactorBySefiraAbility();
                float reinforcementDmg = agent.Equipment.weapon.script.GetReinforcementDmg();
                string text = string.Format("{0}-{1}", (int)(damage.min * num7 * reinforcementDmg), (int)(damage.max * num7 * reinforcementDmg));
                this.Weapon.slots[0].SetText(text);
            }
            else
            {
                this.Weapon.Fill.enabled = true;
                DamageInfo damage2 = agent.Equipment.weapon.GetDamage(agent);
                RwbpType type2 = damage2.type;
                this.Weapon.Fill_Inner.text = EnumTextConverter.GetRwbpType(type2).ToUpper();
                Color color;
                Color color2;
                UIColorManager.instance.GetRWBPTypeColor(type2, out color, out color2);
                this.Weapon.Fill_Inner.color = color;
                this.Weapon.Fill.color = Color.white;
                this.Weapon.Fill.sprite = IconManager.instance.DamageIcon[type2 - RwbpType.R];
                float num7 = agent.GetDamageFactorByEquipment();
                num7 *= agent.GetDamageFactorBySefiraAbility();
                float reinforcementDmg = agent.Equipment.weapon.script.GetReinforcementDmg();
                string text2 = string.Format("{0}-{1}", (int)(damage2.min * num7 * reinforcementDmg), (int)(damage2.max * num7 * reinforcementDmg));
                this.Weapon.slots[0].SetText(text2);
            }
            DefenseInfo defense = agent.defense;
            UIUtil.DefenseSetOnlyText(defense, this.DefenseType);
            UIUtil.SetDefenseTypeIcon(defense, this.DefenseTypeRenderer);
            if (agent.Equipment.armor != null)
            {
                this.ArmorName.text = agent.Equipment.armor.metaInfo.Name;
            }
            else
            {
                this.ArmorName.text = "Armor is missing";
            }
            InventoryItemController.SetGradeText(agent.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
            InventoryItemController.SetGradeText(agent.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
            for (int i = 0; i < this.StatTooltips.Length; i++)
            {
                string text3 = LocalizeTextDataModel.instance.GetText(this.StatTooltips[i].ID);
                string arg = "?";
                switch (i)
                {
                    case 0:
                        arg = agent.fortitudeLevel.ToString();
                        break;
                    case 1:
                        arg = agent.prudenceLevel.ToString();
                        break;
                    case 2:
                        arg = agent.temperanceLevel.ToString();
                        break;
                    case 3:
                        arg = agent.justiceLevel.ToString();
                        break;
                    case 4:
                        arg = ((float)agent.workProb / 5f).ToString();
                        break;
                    case 5:
                        arg = ((float)agent.workSpeed).ToString();
                        break;
                    case 6:
                        arg = (agent.attackSpeed / 1.43f - 20f).ToString();
                        break;
                    case 7:
                        arg = (agent.movement).ToString();
                        break;
                }
                string dynamicTooltip = string.Format(text3, arg);
                this.StatTooltips[i].SetDynamicTooltip(dynamicTooltip);
            }
            for (int j = 0; j < this.DefenseTooltips.Length; j++)
            {
                string text4 = LocalizeTextDataModel.instance.GetText(this.DefenseTooltips[j].ID);
                string defenseTypeText = this.GetDefenseTypeText(agent.defense, j + RwbpType.R);
                string dynamicTooltip2 = string.Format(text4, defenseTypeText);
                this.DefenseTooltips[j].SetDynamicTooltip(dynamicTooltip2);
            }
        }

        // Token: 0x06004CD8 RID: 19672 RVA: 0x001C4994 File Offset: 0x001C2B94
        private string GetDefenseTypeText(DefenseInfo def, RwbpType t)
        {
            DefenseInfo.Type defenseType = def.GetDefenseType(t);
            string result = "?";
            switch (defenseType)
            {
                case DefenseInfo.Type.NONE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_None");
                    break;
                case DefenseInfo.Type.WEAKNESS:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Weak");
                    break;
                case DefenseInfo.Type.SUPER_WEAKNESS:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_SuperWeak");
                    break;
                case DefenseInfo.Type.ENDURE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Endure");
                    break;
                case DefenseInfo.Type.RESISTANCE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Resist");
                    break;
                case DefenseInfo.Type.IMMUNE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Immune");
                    break;
            }
            return result;
        }

        // Token: 0x04004734 RID: 18228
        public TooltipMouseOver[] StatTooltips;

        // Token: 0x04004735 RID: 18229
        public TooltipMouseOver[] DefenseTooltips;

        // Token: 0x04004736 RID: 18230
        public Image GradeImage;

        // Token: 0x04004737 RID: 18231
        public Text AgentName;

        // Token: 0x04004738 RID: 18232
        public Text AgentTitle;

        // Token: 0x04004739 RID: 18233
        public WorkerPortraitSetter portrait;

        // Token: 0x0400473A RID: 18234
        public AgentInfoWindow.StatObject Stat_R;

        // Token: 0x0400473B RID: 18235
        public AgentInfoWindow.StatObject Stat_W;

        // Token: 0x0400473C RID: 18236
        public AgentInfoWindow.StatObject Stat_B;

        // Token: 0x0400473D RID: 18237
        public AgentInfoWindow.StatObject Stat_P;

        // Token: 0x0400473E RID: 18238
        public AgentInfoWindow.StatObject Weapon;

        // Token: 0x0400473F RID: 18239
        public Text WeaponGrade;

        // Token: 0x04004740 RID: 18240
        public Text ArmorName;

        // Token: 0x04004741 RID: 18241
        public Text ArmorGrade;

        // Token: 0x04004742 RID: 18242
        public Text[] DefenseType;

        // Token: 0x04004743 RID: 18243
        public Text[] DefenseInner;

        // Token: 0x04004744 RID: 18244
        public Image[] DefenseFill;

        // Token: 0x04004745 RID: 18245
        public Image[] DefenseTypeRenderer;

        // Token: 0x04004746 RID: 18246
        public List<MaskableGraphic> Colored_R;

        // Token: 0x04004747 RID: 18247
        public List<MaskableGraphic> Colored_W;

        // Token: 0x04004748 RID: 18248
        public List<MaskableGraphic> Colored_B;

        // Token: 0x04004749 RID: 18249
        public List<MaskableGraphic> Colored_P;
    }

    // Token: 0x020009EA RID: 2538
    [Serializable]
    public class InGameModeComponent
    {
        // Token: 0x06004CD9 RID: 19673 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public InGameModeComponent()
        {
        }

        // Token: 0x06004CDA RID: 19674 RVA: 0x001C4A3C File Offset: 0x001C2C3C
        public void SetUI(AgentModel agent)
        { // <Patch> <Mod> resistances will now use 2 decimal points; fixed Ws, As, and Ms stat tooltips so that they show the correct number instead of all saying stat * 0.2%
            this.AgentTitle.enabled = true;
            this.GradeImage.sprite = DeployUI.GetAgentGradeSprite(agent);
            this.AgentName.text = agent.GetUnitName();
            string str = string.Empty;
            Sefira sefira = SefiraManager.instance.GetSefira(agent.lastServiceSefira);
            if (sefira != null)
            {
                SefiraEnum sefiraEnum = sefira.sefiraEnum;
                if (sefiraEnum == SefiraEnum.TIPERERTH2)
                {
                    sefiraEnum = SefiraEnum.TIPERERTH1;
                }
                str = string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_title2"), LocalizeTextDataModel.instance.GetTextAppend(new string[]
                {
                    SefiraName.GetSefiraByEnum(sefiraEnum),
                    "Name"
                }), agent.continuousServiceDay) + " ";
            }
            this.AgentTitle.text = str + LocalizeTextDataModel.instance.GetText("continous_service_ability_cur_blank") + agent.GetTitle();
            this.portrait.SetWorker(agent);
            AgentInfoWindow.WorkerPrimaryStatUI[] array = this.statUI;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].SetStat(agent);
            }
            this.Weapon.StatName.text = agent.Equipment.weapon.metaInfo.Name;
            DamageInfo damage = agent.Equipment.weapon.GetDamage(agent);
            if (EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) == 200038 || EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) == 200004)
            {
                this.Weapon.Fill.enabled = false;
                this.Weapon.Fill_Inner.text = "???";
                this.Weapon.Fill_Inner.color = Color.gray;
                float num = agent.GetDamageFactorByEquipment();
                num *= agent.GetDamageFactorBySefiraAbility();
                float reinforcementDmg = agent.Equipment.weapon.script.GetReinforcementDmg();
                string text = string.Format("{0}-{1}", (int)(damage.min * num * reinforcementDmg), (int)(damage.max * num * reinforcementDmg));
                this.Weapon.slots[0].SetText(text);
            }
            else
            {
                this.Weapon.Fill.enabled = true;
                RwbpType type = damage.type;
                this.Weapon.Fill_Inner.text = EnumTextConverter.GetRwbpType(type).ToUpper();
                Color color;
                Color color2;
                UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
                this.Weapon.Fill_Inner.color = color;
                this.Weapon.Fill.color = Color.white;
                this.Weapon.Fill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
                float num2 = agent.GetDamageFactorByEquipment();
                num2 *= agent.GetDamageFactorBySefiraAbility();
                float reinforcementDmg2 = agent.Equipment.weapon.script.GetReinforcementDmg();
                string text2 = string.Format("{0}-{1}", (int)(damage.min * num2 * reinforcementDmg2), (int)(damage.max * num2 * reinforcementDmg2));
                this.Weapon.slots[0].SetText(text2);
            }
            DefenseInfo defense = agent.defense;
            UIUtil.DefenseSetFactor(defense, this.DefenseType, false);
            UIUtil.SetDefenseTypeIcon(defense, this.DefenseIcon);
            if (agent.Equipment.armor != null)
            {
                this.ArmorName.text = agent.Equipment.armor.metaInfo.Name;
            }
            else
            {
                this.ArmorName.text = "Armor is missing";
            }
            InventoryItemController.SetGradeText(agent.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
            InventoryItemController.SetGradeText(agent.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
            for (int j = 0; j < this.StatTooltips.Length; j++)
            {
                string text3 = LocalizeTextDataModel.instance.GetText(this.StatTooltips[j].ID);
                string arg = "?";
                switch (j)
                {
                    case 0:
                        arg = agent.fortitudeLevel.ToString();
                        break;
                    case 1:
                        arg = agent.prudenceLevel.ToString();
                        break;
                    case 2:
                        arg = agent.temperanceLevel.ToString();
                        break;
                    case 3:
                        arg = agent.justiceLevel.ToString();
                        break;
                    case 4:
                        arg = ((float)agent.workProb / 5f).ToString();
                        break;
                    case 5:
                        arg = ((float)agent.workSpeed).ToString();
                        break;
                    case 6:
                        arg = (agent.attackSpeed / 1.43f - 20f).ToString();
                        break;
                    case 7:
                        arg = (agent.movement).ToString();
                        break;
                }
                string dynamicTooltip = string.Format(text3, arg);
                this.StatTooltips[j].SetDynamicTooltip(dynamicTooltip);
            }
            for (int k = 0; k < this.DefenseTooltips.Length; k++)
            {
                string text4 = LocalizeTextDataModel.instance.GetText(this.DefenseTooltips[k].ID);
                string defenseTypeText = this.GetDefenseTypeText(agent.defense, k + RwbpType.R);
                string dynamicTooltip2 = string.Format(text4, defenseTypeText);
                this.DefenseTooltips[k].SetDynamicTooltip(dynamicTooltip2);
            }
        }

        // Token: 0x06004CDB RID: 19675 RVA: 0x001C4994 File Offset: 0x001C2B94
        public string GetDefenseTypeText(DefenseInfo def, RwbpType t)
        {
            DefenseInfo.Type defenseType = def.GetDefenseType(t);
            string result = "?";
            switch (defenseType)
            {
                case DefenseInfo.Type.NONE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_None");
                    break;
                case DefenseInfo.Type.WEAKNESS:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Weak");
                    break;
                case DefenseInfo.Type.SUPER_WEAKNESS:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_SuperWeak");
                    break;
                case DefenseInfo.Type.ENDURE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Endure");
                    break;
                case DefenseInfo.Type.RESISTANCE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Resist");
                    break;
                case DefenseInfo.Type.IMMUNE:
                    result = LocalizeTextDataModel.instance.GetText("DefenseType_Immune");
                    break;
            }
            return result;
        }

        // Token: 0x0400474A RID: 18250
        public TooltipMouseOver[] StatTooltips;

        // Token: 0x0400474B RID: 18251
        public TooltipMouseOver[] DefenseTooltips;

        // Token: 0x0400474C RID: 18252
        public Image GradeImage;

        // Token: 0x0400474D RID: 18253
        public Text AgentName;

        // Token: 0x0400474E RID: 18254
        public Text AgentTitle;

        // Token: 0x0400474F RID: 18255
        public WorkerPortraitSetter portrait;

        // Token: 0x04004750 RID: 18256
        [Header("Stat")]
        public AgentInfoWindow.WorkerPrimaryStatUI[] statUI;

        // Token: 0x04004751 RID: 18257
        [Header("Weapon")]
        public AgentInfoWindow.StatObject Weapon;

        // Token: 0x04004752 RID: 18258
        public Text WeaponGrade;

        // Token: 0x04004753 RID: 18259
        [Header("Armor")]
        public Text ArmorName;

        // Token: 0x04004754 RID: 18260
        public Text ArmorGrade;

        // Token: 0x04004755 RID: 18261
        public Text[] DefenseType;

        // Token: 0x04004756 RID: 18262
        public Image[] DefenseIcon;
    }

    // Token: 0x020009EB RID: 2539
    [Serializable]
    public class WorkerPrimaryStatUnit
    {
        // Token: 0x06004CDC RID: 19676 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public WorkerPrimaryStatUnit()
        {
        }

        // Token: 0x06004CDD RID: 19677 RVA: 0x001C4F84 File Offset: 0x001C3184
        public void SetStat(int originalValue, int addtionalValue)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            if (addtionalValue == 0)
            {
                this.StatValue.text = originalValue.ToString();
                return;
            }
            if (addtionalValue > 0)
            {
                text = AgentInfoWindow.currentWindow.Additional_Plus_ValueColor;
                text2 = "+";
            }
            else
            {
                text = AgentInfoWindow.currentWindow.Additional_Minus_ValueColor;
                text2 = "-";
            }
            this.StatValue.text = string.Format("{0}<color=#{1}>{2}{3}</color>", new object[]
            {
                originalValue,
                text,
                text2,
                Mathf.Abs(addtionalValue)
            });
        }

        // Token: 0x04004757 RID: 18263
        public Text StatValue;
    }

    // Token: 0x020009EC RID: 2540
    [Serializable]
    public class WorkerPrimaryStatUI
    {
        // Token: 0x06004CDE RID: 19678 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public WorkerPrimaryStatUI()
        {
        }

        // Token: 0x06004CDF RID: 19679 RVA: 0x001C5018 File Offset: 0x001C3218
        public void SetStat(AgentModel agent)
        { // <Mod> exp shows up in the employee window; also shows base stats; also cuts off stat names if they're too long; Overtime Hod Suppression
            string text = string.Empty;
            bool isOvertimeHod = SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.HOD, true);
            OvertimeHodBossBuf hodBuf = null;
            if (isOvertimeHod)
            {
                hodBuf = agent.GetUnitBufByType(UnitBufType.OVERTIME_HODBOSS) as OvertimeHodBossBuf;
            }
            string name = "";
            string color = "FFFFFF";
            int baseStat = 0;
            int level = 1;
            int exp = 0;
            int num;
            int additionalValue;
            switch (this.type)
            {
                case RwbpType.R:
                    {
                        name = LocalizeTextDataModel.instance.GetText("Rstat");
                        color = "CE2842";
                        level = agent.Rstat;
                        baseStat = agent.primaryStat.maxHP;
                        exp = Mathf.RoundToInt(agent.primaryStatExp.hp);
                        if (exp + baseStat > WorkerPrimaryStat.MaxStatR())
                        {
                            exp = WorkerPrimaryStat.MaxStatR() - baseStat;
                        }
                        if (isOvertimeHod)
                        {
                            exp = hodBuf.originalStat.hp;
                        }
                        num = agent.primaryStat.maxHP + agent.titleBonus.maxHP;
                        additionalValue = agent.maxHp - num;
                        this.list[0].SetStat(num, additionalValue);
                        break;
                    }
                case RwbpType.W:
                    {
                        name = LocalizeTextDataModel.instance.GetText("Wstat");
                        color = "EFEBBD";
                        level = agent.Wstat;
                        baseStat = agent.primaryStat.maxMental;
                        exp = Mathf.RoundToInt(agent.primaryStatExp.mental);
                        if (exp + baseStat > WorkerPrimaryStat.MaxStatW())
                        {
                            exp = WorkerPrimaryStat.MaxStatW() - baseStat;
                        }
                        if (isOvertimeHod)
                        {
                            exp = hodBuf.originalStat.mental;
                        }
                        num = agent.primaryStat.maxMental + agent.titleBonus.maxMental;
                        additionalValue = agent.maxMental - num;
                        this.list[0].SetStat(num, additionalValue);
                        break;
                    }
                case RwbpType.B:
                    {
                        name = LocalizeTextDataModel.instance.GetText("Bstat");
                        color = "844884";
                        level = agent.Bstat;
                        baseStat = agent.primaryStat.workProb;
                        exp = Mathf.RoundToInt(agent.primaryStatExp.work);
                        if (exp + baseStat > WorkerPrimaryStat.MaxStatB())
                        {
                            exp = WorkerPrimaryStat.MaxStatB() - baseStat;
                        }
                        if (isOvertimeHod)
                        {
                            exp = hodBuf.originalStat.work;
                        }
                        num = agent.primaryStat.workProb + agent.titleBonus.workProb;
                        additionalValue = agent.workProb - num;
                        this.list[0].SetStat(num, additionalValue);
                        num = agent.primaryStat.cubeSpeed + agent.titleBonus.cubeSpeed;
                        additionalValue = agent.workSpeed - num;
                        this.list[1].SetStat(num, additionalValue);
                        break;
                    }
                case RwbpType.P:
                    {
                        name = LocalizeTextDataModel.instance.GetText("Pstat");
                        color = "42CBBD";
                        level = agent.Pstat;
                        baseStat = agent.primaryStat.attackSpeed;
                        exp = Mathf.RoundToInt(agent.primaryStatExp.battle);
                        if (exp + baseStat > WorkerPrimaryStat.MaxStatP())
                        {
                            exp = WorkerPrimaryStat.MaxStatP() - baseStat;
                        }
                        if (isOvertimeHod)
                        {
                            exp = hodBuf.originalStat.battle;
                        }
                        num = agent.primaryStat.attackSpeed + agent.titleBonus.attackSpeed;
                        additionalValue = (int)agent.attackSpeed - num;
                        this.list[0].SetStat(num, additionalValue);
                        num = agent.primaryStat.movementSpeed + agent.titleBonus.movementSpeed;
                        additionalValue = (int)agent.movement - num;
                        this.list[1].SetStat(num, additionalValue);
                        break;
                    }
            }
			if (name.Length > 5)
			{
				name = name.Substring(0, 4);
			}
            if (isOvertimeHod)
            {
                if (baseStat >= exp)
                {
                    color = AgentInfoWindow.currentWindow.Additional_Plus_ValueColor;
                }
                else
                {
                    color = AgentInfoWindow.currentWindow.Additional_Minus_ValueColor;
                }
                text = string.Format("{0} {1} ({3}) <b><color=#{4}>: {2}</color></b>", name, AgentModel.GetLevelGradeText(level), baseStat, exp, color);
            }
            else if (exp == 0 || !SpecialModeConfig.instance.GetValue<bool>("RevealEXP"))
            {
                text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(level), baseStat);
            }
            else
            {
                text = string.Format("{0} {1} ({2})<color=#{5}>{4}{3}</color>", name, AgentModel.GetLevelGradeText(level), baseStat, Mathf.Abs(exp), exp > 0 ? "+" : "-", color);
            }
            this.StatName.text = text;
        }

        // Token: 0x04004758 RID: 18264
        public RwbpType type;

        // Token: 0x04004759 RID: 18265
        public Text StatName;

        // Token: 0x0400475A RID: 18266
        public Image Icon;

        // Token: 0x0400475B RID: 18267
        public List<AgentInfoWindow.WorkerPrimaryStatUnit> list;
    }

    // Token: 0x020009ED RID: 2541
    [Serializable]
    public class ColorSet
    {
        // Token: 0x06004CE0 RID: 19680 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public ColorSet()
        {
        }

        // Token: 0x06004CE1 RID: 19681 RVA: 0x001C5254 File Offset: 0x001C3454
        public void SetColor(Color n, Color i)
        {
            MaskableGraphic[] array = this.normal;
            for (int j = 0; j < array.Length; j++)
            {
                array[j].color = n;
            }
            array = this.inverse;
            for (int j = 0; j < array.Length; j++)
            {
                array[j].color = i;
            }
        }

        // Token: 0x0400475C RID: 18268
        public MaskableGraphic[] normal;

        // Token: 0x0400475D RID: 18269
        public MaskableGraphic[] inverse;
    }
}
