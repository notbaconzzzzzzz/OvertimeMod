using System;
using System.Collections.Generic;
using GameStatusUI;
using GlobalBullet;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A2B RID: 2603
public class PlaySpeedSettingUI : MonoBehaviour
{
    // Token: 0x06004E38 RID: 20024 RVA: 0x00040132 File Offset: 0x0003E332
    public PlaySpeedSettingUI()
    {
    }

    // Token: 0x1700073D RID: 1853
    // (get) Token: 0x06004E39 RID: 20025 RVA: 0x00040169 File Offset: 0x0003E369
    private bool CanUseTimeMultiplier
    {
        get
        {
            return this.timeMultiplierEnabled && ResearchDataModel.instance.GetSephiraAbility(SefiraManager.instance.GetSefira(SefiraEnum.MALKUT)) >= 1;
        }
    }

    // Token: 0x1700073E RID: 1854
    // (get) Token: 0x06004E3A RID: 20026 RVA: 0x00040193 File Offset: 0x0003E393
    public bool available
    {
        get
        {
            return this._available;
        }
    }

    // Token: 0x1700073F RID: 1855
    // (get) Token: 0x06004E3B RID: 20027 RVA: 0x0004019B File Offset: 0x0003E39B
    public static PlaySpeedSettingUI instance
    {
        get
        {
            return PlaySpeedSettingUI._instance;
        }
    }

    // Token: 0x06004E3C RID: 20028 RVA: 0x001CAA60 File Offset: 0x001C8C60
    private void Awake()
    {
        if (PlaySpeedSettingUI.instance != null && PlaySpeedSettingUI.instance.gameObject != base.gameObject)
        {
            UnityEngine.Object.Destroy(PlaySpeedSettingUI.instance.gameObject);
        }
        PlaySpeedSettingUI._instance = this;
    }

    // Token: 0x06004E3D RID: 20029 RVA: 0x000401A2 File Offset: 0x0003E3A2
    public void AddSpaceEvent(PlaySpeedSettingUI.SpaceEvent newEvent)
    {
        this.spaceCalled.Add(newEvent);
    }

    // Token: 0x06004E3E RID: 20030 RVA: 0x000401B0 File Offset: 0x0003E3B0
    public void RemoveSpaceEvent(PlaySpeedSettingUI.SpaceEvent targetEvent)
    {
        this.spaceCalled.Remove(targetEvent);
    }

    // Token: 0x06004E3F RID: 20031 RVA: 0x001CAAAC File Offset: 0x001C8CAC
    private bool CheckTimeStopBlocked(bool isRelease)
    {
        bool result = false;
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        PlaySpeedSettingBlockType playSpeedSettingBlockType = PlaySpeedSettingBlockType.DUMMYFORLAST;
        foreach (KeyValuePair<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> keyValuePair in this.blockedDictionary)
        {
            PlaySpeedSettingBlockedUI value = keyValuePair.Value;
            PlaySpeedSettingBlockType key = keyValuePair.Key;
            value.OnTryFunction(PlaySpeedSettingBlockFunction.TIMESTOP, true);
            if (isRelease)
            {
                value.OnTryFunction(PlaySpeedSettingBlockFunction.TIMESTOP_RESUME, true);
            }
            else
            {
                value.OnTryFunction(PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE, true);
            }
            if (value.blockFunction == PlaySpeedSettingBlockFunction.TIMESTOP || value.blockFunction == PlaySpeedSettingBlockFunction.ALL || (isRelease && value.blockFunction == PlaySpeedSettingBlockFunction.TIMESTOP_RESUME) || (!isRelease && value.blockFunction == PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE))
            {
                if (value.IsFunctionEnabled(PlaySpeedSettingBlockFunction.TIMESTOP))
                {
                    if (playSpeedSettingBlockType > key)
                    {
                        playSpeedSettingBlockedUI = value;
                        playSpeedSettingBlockType = key;
                    }
                }
            }
        }
        if (playSpeedSettingBlockedUI != null)
        {
            result = playSpeedSettingBlockedUI.IsFunctionEnabled(PlaySpeedSettingBlockFunction.TIMESTOP);
            if (!playSpeedSettingBlockedUI.IsDisplaying && playSpeedSettingBlockedUI.OnTryDisplay(PlaySpeedSettingBlockFunction.TIMESTOP))
            {
                playSpeedSettingBlockedUI.OnShow();
            }
        }
        return result;
    }

    // Token: 0x06004E40 RID: 20032 RVA: 0x001CABD4 File Offset: 0x001C8DD4
    private bool CheckTimeMultiplierBlocked()
    {
        bool result = false;
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        PlaySpeedSettingBlockType playSpeedSettingBlockType = PlaySpeedSettingBlockType.DUMMYFORLAST;
        foreach (KeyValuePair<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> keyValuePair in this.blockedDictionary)
        {
            PlaySpeedSettingBlockedUI value = keyValuePair.Value;
            PlaySpeedSettingBlockType key = keyValuePair.Key;
            value.OnTryFunction(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER, false);
            if ((value.blockFunction == PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER || value.blockFunction == PlaySpeedSettingBlockFunction.ALL) && playSpeedSettingBlockType > key)
            {
                playSpeedSettingBlockedUI = value;
                playSpeedSettingBlockType = key;
            }
        }
        if (playSpeedSettingBlockedUI != null)
        {
            result = playSpeedSettingBlockedUI.IsFunctionEnabled(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER);
            if (!playSpeedSettingBlockedUI.IsDisplaying && playSpeedSettingBlockedUI.OnTryDisplay(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER))
            {
                playSpeedSettingBlockedUI.OnShow();
            }
        }
        return result;
    }

    // Token: 0x06004E41 RID: 20033 RVA: 0x001CACA8 File Offset: 0x001C8EA8
    private bool CheckEscapeBlocked()
    {
        bool result = false;
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        PlaySpeedSettingBlockType playSpeedSettingBlockType = PlaySpeedSettingBlockType.DUMMYFORLAST;
        foreach (KeyValuePair<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> keyValuePair in this.blockedDictionary)
        {
            PlaySpeedSettingBlockedUI value = keyValuePair.Value;
            PlaySpeedSettingBlockType key = keyValuePair.Key;
            value.OnTryFunction(PlaySpeedSettingBlockFunction.ESCAPE, false);
            if ((value.blockFunction == PlaySpeedSettingBlockFunction.ESCAPE || value.blockFunction == PlaySpeedSettingBlockFunction.ALL) && playSpeedSettingBlockType > key)
            {
                playSpeedSettingBlockedUI = value;
                playSpeedSettingBlockType = key;
            }
        }
        if (playSpeedSettingBlockedUI != null)
        {
            result = playSpeedSettingBlockedUI.IsFunctionEnabled(PlaySpeedSettingBlockFunction.ESCAPE);
            if (!playSpeedSettingBlockedUI.IsDisplaying && playSpeedSettingBlockedUI.OnTryDisplay(PlaySpeedSettingBlockFunction.ESCAPE))
            {
                playSpeedSettingBlockedUI.OnShow();
            }
        }
        return result;
    }

    // Token: 0x06004E42 RID: 20034 RVA: 0x001CAD7C File Offset: 0x001C8F7C
    private bool CheckManaulBlocked()
    {
        bool result = false;
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        PlaySpeedSettingBlockType playSpeedSettingBlockType = PlaySpeedSettingBlockType.DUMMYFORLAST;
        foreach (KeyValuePair<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> keyValuePair in this.blockedDictionary)
        {
            PlaySpeedSettingBlockedUI value = keyValuePair.Value;
            PlaySpeedSettingBlockType key = keyValuePair.Key;
            value.OnTryFunction(PlaySpeedSettingBlockFunction.MANUAL, false);
            if ((value.blockFunction == PlaySpeedSettingBlockFunction.MANUAL || value.blockFunction == PlaySpeedSettingBlockFunction.ALL) && playSpeedSettingBlockType > key)
            {
                playSpeedSettingBlockedUI = value;
                playSpeedSettingBlockType = key;
            }
        }
        if (playSpeedSettingBlockedUI != null)
        {
            result = playSpeedSettingBlockedUI.IsFunctionEnabled(PlaySpeedSettingBlockFunction.MANUAL);
            if (!playSpeedSettingBlockedUI.IsDisplaying && playSpeedSettingBlockedUI.OnTryDisplay(PlaySpeedSettingBlockFunction.MANUAL))
            {
                playSpeedSettingBlockedUI.OnShow();
            }
        }
        return result;
    }

    // Token: 0x06004E43 RID: 20035 RVA: 0x000401BF File Offset: 0x0003E3BF
    private void Start()
    {
        this._available = true;
        this.observeFilter.gameObject.SetActive(false);
        this.pauseFilter.gameObject.SetActive(false);
        this.UpdateButton();
        this.timeMultiplierEnabled = true;
    }

    // Token: 0x06004E44 RID: 20036 RVA: 0x000401F7 File Offset: 0x0003E3F7
    public void SetTimeMultiplierEnable(bool state)
    {
        this.timeMultiplierEnabled = state;
    }

    // Token: 0x06004E45 RID: 20037 RVA: 0x001CAE50 File Offset: 0x001C9050
    private void Update()
    {
        if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
        {
            return;
        }
        PAUSECALL currentPauseCaller = GameManager.currentGameManager.GetCurrentPauseCaller();
        if (currentPauseCaller == PAUSECALL.INGAMEEFFECT || currentPauseCaller == PAUSECALL.OBSERVE)
        {
            return;
        }
        if (this.CanUseTimeMultiplier)
        {
            this.speedBlockMask.gameObject.SetActive(false);
        }
        else
        {
            this.speedBlockMask.gameObject.SetActive(true);
        }
        if (!ConsoleScript.instance.ConsoleWnd.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            if (!this._available)
            {
                this.CallBlockEvent(1);
                return;
            }
            if (this.CheckTimeStopBlocked(GameManager.currentGameManager.state == GameState.PAUSE))
            {
                return;
            }
            if (!MaxObservation.instance.isEnabled)
            {
                if (GameManager.currentGameManager.state == GameState.PLAYING)
                {
                    this.CheckAction(PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE);
                    this.OnPause(PAUSECALL.STOPGAME);
                }
                else if (GameManager.currentGameManager.state == GameState.PAUSE)
                {
                    this.CheckAction(PlaySpeedSettingBlockFunction.TIMESTOP_RESUME);
                    this.OnResume(PAUSECALL.STOPGAME);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!this._available)
            {
                this.CallBlockEvent(0);
                return;
            }
            if (this.CheckEscapeBlocked())
            {
                return;
            }
            if (GameManager.currentGameManager.state != GameState.STOP)
            {
                if (!EscapeUI.instance.IsOpened)
                {
                    if (GlobalBulletWindow.CurrentWindow != null && GlobalBulletWindow.CurrentWindow.CurrentSelectedBullet != GlobalBulletType.NONE)
                    {
                        GlobalBulletWindow.CurrentWindow.OnSlotSelected(GlobalBulletType.NONE);
                    }
                    else if (!CreatureInfoWindow.CurrentWindow.IsEnabled && !OptionUI.Instance.IsEnabled)
                    {
                        EscapeUI.OpenWindow();
                        CameraMover.instance.StopMove();
                        this.OnPause(PAUSECALL.ESCAPE);
                    }
                }
                else
                {
                    EscapeUI.CloseWindow();
                    CameraMover.instance.ReleaseMove();
                    this.OnResume(PAUSECALL.ESCAPE);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && this.CanUseTimeMultiplier)
        {
            if (!this._available)
            {
                this.CallBlockEvent(1);
                return;
            }
            if (this.CheckTimeMultiplierBlocked())
            {
                return;
            }
            if (!ConsoleScript.instance.ConsoleWnd.activeInHierarchy && GameManager.currentGameManager.state != GameState.PAUSE && GameManager.currentGameManager.gameSpeedLevel > 1)
            {
                GameManager.currentGameManager.SetPlaySpeed(GameManager.currentGameManager.gameSpeedLevel - 1);
                this.UpdateButton();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!this._available)
            {
                this.CallBlockEvent(1);
                return;
            }
            if (this.CheckTimeMultiplierBlocked())
            {
                return;
            }
            if (!this.CanUseTimeMultiplier)
            {
                return;
            }
            if (!ConsoleScript.instance.ConsoleWnd.activeInHierarchy)
            {
                int gameSpeedLevel = GameManager.currentGameManager.gameSpeedLevel;
                if (gameSpeedLevel >= 1 && gameSpeedLevel < 3)
                {
                    GameManager.currentGameManager.SetPlaySpeed(GameManager.currentGameManager.gameSpeedLevel + 1);
                    this.UpdateButton();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (ManualUI.Instance.IsActivated)
            {
                ManualUI.Instance.CloseManual();
            }
            else
            {
                if (this.CheckManaulBlocked())
                {
                    return;
                }
                ManualUI.Instance.OpenManual();
            }
        }
    }

    // Token: 0x06004E46 RID: 20038 RVA: 0x001CB168 File Offset: 0x001C9368
    public void SetNormalSpeedForcely()
    {
        int gameSpeedLevel = GameManager.currentGameManager.gameSpeedLevel;
        if (gameSpeedLevel > 1)
        {
            GameManager.currentGameManager.SetPlaySpeed(1);
            this.UpdateButton();
        }
    }

    // Token: 0x06004E47 RID: 20039 RVA: 0x00040200 File Offset: 0x0003E400
    public void CallBlockEvent(int index)
    {
        if (this.blockEvent != null)
        {
            this.blockEvent(index);
        }
    }

    // Token: 0x06004E48 RID: 20040 RVA: 0x00040219 File Offset: 0x0003E419
    public void ClearBlockEvent()
    {
        this.blockEvent = null;
    }

    // Token: 0x06004E49 RID: 20041 RVA: 0x00040222 File Offset: 0x0003E422
    public void SetBlockEvent(PlaySpeedSettingUI.BlockedUIEvent blockEvent)
    {
        this.blockEvent = blockEvent;
    }

    // Token: 0x06004E4A RID: 20042 RVA: 0x0004022B File Offset: 0x0003E42B
    public void BlockSetting(CreatureBase caller)
    {
        if (!this.blockedCaller.Contains(caller))
        {
            this.blockedCaller.Add(caller);
        }
        this._available = false;
    }

    // Token: 0x06004E4B RID: 20043 RVA: 0x00040251 File Offset: 0x0003E451
    public void ReleaseSetting(CreatureBase caller)
    {
        if (this.blockedCaller.Contains(caller))
        {
            this.blockedCaller.Remove(caller);
        }
        if (this.blockedCaller.Count == 0)
        {
            this._available = true;
        }
    }

    // Token: 0x06004E4C RID: 20044 RVA: 0x00040288 File Offset: 0x0003E488
    public void ForcleyReleaseSetting()
    {
        this.blockedCaller.Clear();
        this._available = true;
    }

    // Token: 0x06004E4D RID: 20045 RVA: 0x00003E21 File Offset: 0x00002021
    public void BlockImageSetActivate(bool state)
    {
    }

    // Token: 0x06004E4E RID: 20046 RVA: 0x0004029C File Offset: 0x0003E49C
    public void ForcelyPlay()
    {
        GameManager.currentGameManager.Resume(PAUSECALL.INGAMEEFFECT);
        GameManager.currentGameManager.SetPlaySpeed(1);
        CameraMover.instance.ReleaseMove();
        this.UpdateButton();
    }

    // Token: 0x06004E4F RID: 20047 RVA: 0x001CB198 File Offset: 0x001C9398
    public void OnPause(PAUSECALL caller)
    {
        if (GameManager.currentGameManager.state == GameState.PLAYING)
        {
            this.pauseFilter.gameObject.SetActive(true);
        }
        if (caller == PAUSECALL.OBSERVE)
        {
            this.pauseFilter.sprite = this.observeSprite;
        }
        else if (caller == PAUSECALL.ROULETTE)
        {
            this.pauseFilter.gameObject.SetActive(false);
        }
        else if (caller == PAUSECALL.TUTORIAL)
        {
            this.pauseFilter.gameObject.SetActive(false);
        }
        GameManager.currentGameManager.Pause(caller);
        foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in this.spaceCalled)
        {
            spaceEvent(false);
        }
        this.UpdateButton();
    }

    // Token: 0x06004E50 RID: 20048 RVA: 0x001CB278 File Offset: 0x001C9478
    public void OnResume(PAUSECALL caller)
    {
        if (caller == PAUSECALL.OBSERVE)
        {
            this.pauseFilter.sprite = this.pauseDefSprite;
        }
        else if (caller == PAUSECALL.ROULETTE)
        {
            this.pauseFilter.gameObject.SetActive(true);
        }
        GameManager.currentGameManager.Resume(caller);
        foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in this.spaceCalled)
        {
            spaceEvent(true);
        }
        this.UpdateButton();
    }

    // Token: 0x06004E51 RID: 20049 RVA: 0x001CB31C File Offset: 0x001C951C
    public void OnClickPause()
    {
        if (!this.available)
        {
            this.CallBlockEvent(1);
            return;
        }
        if (this.CheckTimeStopBlocked(false))
        {
            return;
        }
        this.CheckAction(PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE);
        foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in this.spaceCalled)
        {
            spaceEvent(false);
        }
        GameManager.currentGameManager.Pause();
        this.UpdateButton();
    }

    // Token: 0x06004E52 RID: 20050 RVA: 0x001CB3B0 File Offset: 0x001C95B0
    private void CheckAction(PlaySpeedSettingBlockFunction function)
    {
        foreach (KeyValuePair<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> keyValuePair in this.blockedDictionary)
        {
            keyValuePair.Value.OnTryFunction(function, false);
        }
    }

    // Token: 0x06004E53 RID: 20051 RVA: 0x001CB414 File Offset: 0x001C9614
    public void OnClickResume()
    {
        if (!this.available)
        {
            this.CallBlockEvent(1);
            return;
        }
        if (this.CheckTimeStopBlocked(true))
        {
            return;
        }
        this.CheckAction(PlaySpeedSettingBlockFunction.TIMESTOP_RESUME);
        foreach (PlaySpeedSettingUI.SpaceEvent spaceEvent in this.spaceCalled)
        {
            spaceEvent(true);
        }
        GameManager.currentGameManager.Resume();
        this.UpdateButton();
        if (!CameraMover.instance.IsMovable())
        {
            CameraMover.instance.ReleaseMove();
        }
    }

    // Token: 0x06004E54 RID: 20052 RVA: 0x001CB4C0 File Offset: 0x001C96C0
    public void OnClickSpeed1()
    {
        if (!this.available)
        {
            this.CallBlockEvent(1);
            return;
        }
        if (GameManager.currentGameManager.state != GameState.PAUSE)
        {
            if (this.CheckTimeMultiplierBlocked())
            {
                return;
            }
            if (!this.CanUseTimeMultiplier)
            {
                return;
            }
        }
        else if (this.CheckTimeStopBlocked(true))
        {
            return;
        }
        GameManager.currentGameManager.SetPlaySpeed(1);
        this.UpdateButton();
    }

    // Token: 0x06004E55 RID: 20053 RVA: 0x001CB52C File Offset: 0x001C972C
    public void OnClickSpeed2()
    {
        if (ResearchDataModel.instance.GetSephiraAbility(SefiraManager.instance.GetSefira(SefiraEnum.MALKUT)) < 1)
        {
            return;
        }
        if (this.CheckTimeMultiplierBlocked())
        {
            return;
        }
        if (!this.CanUseTimeMultiplier)
        {
            return;
        }
        if (!this.available)
        {
            this.CallBlockEvent(1);
            return;
        }
        if (!this.CanUseTimeMultiplier)
        {
            return;
        }
        GameManager.currentGameManager.SetPlaySpeed(2);
        this.UpdateButton();
    }

    // Token: 0x06004E56 RID: 20054 RVA: 0x001CB5A0 File Offset: 0x001C97A0
    public void OnClickSpeed3()
    {
        if (ResearchDataModel.instance.GetSephiraAbility(SefiraManager.instance.GetSefira(SefiraEnum.MALKUT)) < 1)
        {
            return;
        }
        if (this.CheckTimeMultiplierBlocked())
        {
            return;
        }
        if (!this.CanUseTimeMultiplier)
        {
            return;
        }
        if (!this.available)
        {
            this.CallBlockEvent(1);
            return;
        }
        GameManager.currentGameManager.SetPlaySpeed(3);
        this.UpdateButton();
    }

    // Token: 0x06004E57 RID: 20055 RVA: 0x001CB608 File Offset: 0x001C9808
    public void UpdateButton()
    {
        if (GameManager.currentGameManager.state == GameState.PAUSE)
        {
            this.PauseButton.interactable = false;
            this.NormalSpeed.interactable = true;
            this.OneHalfSpeed.interactable = true;
            this.TwiceSpeed.interactable = true;
            this.PauseButton.OnPointerExit(null);
            this.NormalSpeed.OnPointerExit(null);
            this.OneHalfSpeed.OnPointerExit(null);
            this.TwiceSpeed.OnPointerExit(null);
            if (!MaxObservation.instance.isEnabled)
            {
                if (GameManager.currentGameManager.GetCurrentPauseCaller() != PAUSECALL.ROULETTE && GameManager.currentGameManager.GetCurrentPauseCaller() != PAUSECALL.TUTORIAL)
                {
                    this.pauseFilter.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            int gameSpeedLevel = GameManager.currentGameManager.gameSpeedLevel;
            if (gameSpeedLevel != 1)
            {
                if (gameSpeedLevel != 2)
                {
                    if (gameSpeedLevel == 3)
                    {
                        this.PauseButton.interactable = true;
                        this.NormalSpeed.interactable = true;
                        this.OneHalfSpeed.interactable = true;
                        this.TwiceSpeed.interactable = false;
                    }
                }
                else
                {
                    this.PauseButton.interactable = true;
                    this.NormalSpeed.interactable = true;
                    this.OneHalfSpeed.interactable = false;
                    this.TwiceSpeed.interactable = true;
                }
            }
            else
            {
                this.PauseButton.interactable = true;
                this.NormalSpeed.interactable = false;
                this.OneHalfSpeed.interactable = true;
                this.TwiceSpeed.interactable = true;
            }
            this.PauseButton.OnPointerExit(null);
            this.NormalSpeed.OnPointerExit(null);
            this.OneHalfSpeed.OnPointerExit(null);
            this.TwiceSpeed.OnPointerExit(null);
            this.pauseFilter.gameObject.SetActive(false);
        }
    }

    // Token: 0x06004E58 RID: 20056 RVA: 0x001CB7D4 File Offset: 0x001C99D4
    public void OnStageStart()
    {
        PlaySpeedSettingUI._instance = this;
        this.blockedDictionary.Clear();
        this.BlockImageSetActivate(false);
        this.ClearBlockEvent();
        this.ForcleyReleaseSetting();
        if (this.CheckPlaySpeedUI())
        {
            this.OneHalfSpeed.gameObject.SetActive(true);
            this.TwiceSpeed.gameObject.SetActive(true);
        }
        else
        {
            this.OneHalfSpeed.gameObject.SetActive(false);
            this.TwiceSpeed.gameObject.SetActive(false);
        }
    }

    // Token: 0x06004E59 RID: 20057 RVA: 0x000402C4 File Offset: 0x0003E4C4
    public bool CheckPlaySpeedUI()
    {
        return ResearchDataModel.instance.GetSephiraAbility(SefiraManager.instance.GetSefira(SefiraEnum.MALKUT)) >= 1;
    }

    // Token: 0x06004E5A RID: 20058 RVA: 0x001CB85C File Offset: 0x001C9A5C
    public void AddBlockedEvent(PlaySpeedSettingBlockType blockType, PlaySpeedSettingBlockedUI script)
    {
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        if (this.blockedDictionary.TryGetValue(blockType, out playSpeedSettingBlockedUI))
        {
            if (playSpeedSettingBlockedUI != null)
            {
                UnityEngine.Object.Destroy(playSpeedSettingBlockedUI.gameObject);
            }
            this.blockedDictionary.Remove(blockType);
        }
        this.blockedDictionary.Add(blockType, script);
        script.transform.SetParent(this.BlockedUIParent.transform);
        script.transform.localScale = Vector3.one;
        script.transform.localRotation = Quaternion.Euler(Vector3.zero);
        script.transform.localPosition = Vector3.zero;
        RectTransform component = script.GetComponent<RectTransform>();
        if (component != null)
        {
            component.anchoredPosition = Vector3.zero;
        }
        script.gameObject.SetActive(false);
    }

    // Token: 0x06004E5B RID: 20059 RVA: 0x001CB92C File Offset: 0x001C9B2C
    public void RemoveBlockedEvent(PlaySpeedSettingBlockType blockType)
    {
        PlaySpeedSettingBlockedUI playSpeedSettingBlockedUI = null;
        if (this.blockedDictionary.TryGetValue(blockType, out playSpeedSettingBlockedUI))
        {
            if (playSpeedSettingBlockedUI != null)
            {
                UnityEngine.Object.Destroy(playSpeedSettingBlockedUI.gameObject);
            }
            this.blockedDictionary.Remove(blockType);
        }
    }

    // Token: 0x06004E5C RID: 20060 RVA: 0x00003E21 File Offset: 0x00002021
    // Note: this type is marked as 'beforefieldinit'.
    static PlaySpeedSettingUI()
    {
    }

    // Token: 0x0400489D RID: 18589
    public Image pauseImage;

    // Token: 0x0400489E RID: 18590
    public Image speed1Image;

    // Token: 0x0400489F RID: 18591
    public Image speed2Image;

    // Token: 0x040048A0 RID: 18592
    public Image speed3Image;

    // Token: 0x040048A1 RID: 18593
    public Image speedBlockMask;

    // Token: 0x040048A2 RID: 18594
    public Button PauseButton;

    // Token: 0x040048A3 RID: 18595
    public Button NormalSpeed;

    // Token: 0x040048A4 RID: 18596
    public Button OneHalfSpeed;

    // Token: 0x040048A5 RID: 18597
    public Button TwiceSpeed;

    // Token: 0x040048A6 RID: 18598
    public Image pauseFilter;

    // Token: 0x040048A7 RID: 18599
    public Image observeFilter;

    // Token: 0x040048A8 RID: 18600
    public Image blockedImage;

    // Token: 0x040048A9 RID: 18601
    public Sprite pauseDefSprite;

    // Token: 0x040048AA RID: 18602
    public Sprite observeSprite;

    // Token: 0x040048AB RID: 18603
    private List<PlaySpeedSettingUI.SpaceEvent> spaceCalled = new List<PlaySpeedSettingUI.SpaceEvent>();

    // Token: 0x040048AC RID: 18604
    private List<CreatureBase> blockedCaller = new List<CreatureBase>();

    // Token: 0x040048AD RID: 18605
    private PlaySpeedSettingUI.BlockedUIEvent blockEvent;

    // Token: 0x040048AE RID: 18606
    [Header("UI Blocked")]
    public Transform BlockedUIParent;

    // Token: 0x040048AF RID: 18607
    private Dictionary<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI> blockedDictionary = new Dictionary<PlaySpeedSettingBlockType, PlaySpeedSettingBlockedUI>();

    // Token: 0x040048B0 RID: 18608
    private bool timeMultiplierEnabled = true;

    // Token: 0x040048B1 RID: 18609
    private bool _available = true;

    // Token: 0x040048B2 RID: 18610
    private static PlaySpeedSettingUI _instance;

    // Token: 0x02000A2C RID: 2604
    // (Invoke) Token: 0x06004E5E RID: 20062
    public delegate void SpaceEvent(bool state);

    // Token: 0x02000A2D RID: 2605
    // (Invoke) Token: 0x06004E62 RID: 20066
    public delegate void BlockedUIEvent(int index);
}
