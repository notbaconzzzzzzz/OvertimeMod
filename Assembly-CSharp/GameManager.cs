using System;
using System.Collections;
using System.Collections.Generic;
using CreatureGenerate;
using GameStatusUI;
using GlobalBullet;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000588 RID: 1416
public class GameManager : MonoBehaviour
{
    // Token: 0x0600307F RID: 12415 RVA: 0x0002D046 File Offset: 0x0002B246
    public GameManager()
    {
    }

    // Token: 0x17000482 RID: 1154
    // (get) Token: 0x06003080 RID: 12416 RVA: 0x0002D06B File Offset: 0x0002B26B
    public bool StageEnded
    {
        get
        {
            return this.stageEnded;
        }
    }

    // Token: 0x17000483 RID: 1155
    // (get) Token: 0x06003081 RID: 12417 RVA: 0x0002D073 File Offset: 0x0002B273
    public float PlayTime
    {
        get
        {
            return this.playTime;
        }
    }

    // Token: 0x17000484 RID: 1156
    // (get) Token: 0x06003082 RID: 12418 RVA: 0x0002D07B File Offset: 0x0002B27B
    private bool BossEvent
    {
        get
        {
            return SefiraBossManager.Instance.CurrentActivatedSefira == SefiraEnum.CHOKHMAH || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E3);
        }
    }

    // Token: 0x17000485 RID: 1157
    // (get) Token: 0x06003083 RID: 12419 RVA: 0x0002D09C File Offset: 0x0002B29C
    public static GameManager currentGameManager
    {
        get
        {
            return GameManager._currentGameManager;
        }
    }

    // Token: 0x06003084 RID: 12420 RVA: 0x0002D0A3 File Offset: 0x0002B2A3
    private void Awake()
    {
        this.state = GameState.STOP;
        GameManager._currentGameManager = this;
        this.playerModel = PlayerModel.instance;
    }

    // Token: 0x06003085 RID: 12421 RVA: 0x0002D0BD File Offset: 0x0002B2BD
    private void Start()
    {
        this.InitGame();
    }

    // Token: 0x06003086 RID: 12422 RVA: 0x00146D60 File Offset: 0x00144F60
    public void InitGame()
    {
        GC.Collect();
        this.ManageStarted = false;
        Camera.main.orthographicSize = 17f;
        if (!GlobalGameManager.instance.IsPlaying())
        {
            GlobalGameManager.instance.InitStoryMode();
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.MALKUT);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.MALKUT);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.MALKUT);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.MALKUT);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.YESOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.YESOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.YESOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.YESOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.YESOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.NETZACH);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.NETZACH);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.NETZACH);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.NETZACH);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.NETZACH);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.HOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.HOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.HOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.HOD);
            SefiraManager.instance.OpenSefiraWithCreatureDebug(SefiraEnum.HOD);
        }
        CreatureManager.instance.OnGameInit();
        OrdealManager.instance.OnGameInit();
        SpecialEventManager.instance.OnGameInit();
        RandomEventManager.instance.UpdatedEvents();
        OverlayManager.Instance.ReadState();
        if (PlayerModel.instance.GetDay() == 0)
        {
            Notice.instance.Send(NoticeName.ResetMapGraph, new object[0]);
        }
        foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
        {
            agentModel.ReturnToSefira();
        }
        this.StartStage();
        Notice.instance.Send(NoticeName.OnInitGameManager, new object[0]);
        this.currentPauseCaller = PAUSECALL.NONE;
    }

    // Token: 0x06003087 RID: 12423 RVA: 0x00146F3C File Offset: 0x0014513C
    public void StartStage()
    {
        int num = 0;
        try
        {
            SefiraBossManager.Instance.Init();
            DeployUI.instance.Init();
            num++;
            BgmManager.instance.OnStageRelease();
            num++;
            EnergyModel.instance.OnStageStart();
            num++;
            UIEffectManager.instance.OnStageStart();
            num++;
            GameStatusUI.GameStatusUI.Window.playSpeedSetting.OnStageStart();
            num++;
            MissionManager.instance.OnStageStart();
            num++;
        }
        catch (Exception ex)
        {
            Debug.LogError(string.Concat(new object[]
            {
                "[",
                num,
                "]",
                ex
            }));
        }
        foreach (Sefira sefira in SefiraManager.instance.sefiraList)
        {
            sefira.officerSpecialAction.ResetActionAll();
        }
    }

    // Token: 0x06003088 RID: 12424 RVA: 0x0014704C File Offset: 0x0014524C
    public void StartGame()
    {
        this.state = GameState.PLAYING;
        this.ManageStarted = true;
        this.currentUIState = CurrentUIState.DEFAULT;
        base.GetComponent<RootTimer>().AddTimer(NoticeName.EnergyTimer, 10f);
        base.GetComponent<RootTimer>().AddTimer(NoticeName.AutoSaveTimer, 30f);
        int day = PlayerModel.instance.GetDay();
        int sefiraOpenLevel = SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.KETHER);
        int sefiraOpenLevel2 = SefiraManager.instance.GetSefiraOpenLevel(SefiraEnum.DAAT);
        if (sefiraOpenLevel2 > 0)
        {
            Notice.instance.Send(NoticeName.ChangeKetherImage, new object[]
            {
                6
            });
        }
        else if (sefiraOpenLevel > 0)
        {
            Notice.instance.Send(NoticeName.ChangeKetherImage, new object[]
            {
                sefiraOpenLevel
            });
        }
        CameraMover.instance.OnStageStart();
        SefiraManager.instance.OnStageStart_first();
        AgentManager.instance.OnStageStart();
        RabbitManager.instance.OnStageStart();
        RandomEventManager.instance.OnStageStart();
        CreatureManager.instance.OnStageStart();
        OrdealManager.instance.OnStageStart();
        GlobalHistory.instance.OnStageStart();
        MissionUI.instance.Init();
        UnitMouseEventManager.instance.OnStageStart();
        GameStatusUI.GameStatusUI.Window.playSpeedSetting.OnStageStart();
        foreach (Sefira sefira in SefiraManager.instance.sefiraList)
        {
            if (SefiraManager.instance.IsOpened(sefira.sefiraEnum))
            {
                sefira.OnStageStart();
            }
        }
        foreach (OfficerModel officerModel in OfficerManager.instance.GetOfficerList())
        {
            officerModel.ReturnToSefira();
        }
        foreach (OfficerModel officerModel2 in OfficerManager.instance.GetOfficerList())
        {
            base.StartCoroutine(officerModel2.StartAction());
        }
        foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
        {
        }
        AgentLayer.currentLayer.OnStageStart();
        OfficerLayer.currentLayer.OnStageStart();
        BgmManager.instance.OnManagementStart();
        SefiraBossManager.Instance.OnStageStart();
        CreatureOverloadManager.instance.OnStageStart();
        GlobalBulletManager.instance.OnStageStart();
        Notice.instance.Send(NoticeName.OnStageStart, new object[0]);
    }

    // Token: 0x06003089 RID: 12425 RVA: 0x0002D0C5 File Offset: 0x0002B2C5
    public void ReturnToIntro()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        this.EndGame();
        this.Release();
        GlobalGameManager.instance.ReleaseGame();
        SceneManager.LoadSceneAsync("Intro");
    }

    // Token: 0x0600308A RID: 12426 RVA: 0x00147330 File Offset: 0x00145530
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
        GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
        this.EndGame();
        this.Release();
        GlobalGameManager.instance.ReleaseGame();
        GlobalGameManager.instance.LoadGlobalData();
        GlobalGameManager.instance.loadingScene = "DefaultEndScene";
        GlobalGameManager.instance.loadingScreen.LoadTitleScene();
    }

    // Token: 0x0600308B RID: 12427 RVA: 0x001473C4 File Offset: 0x001455C4
    public void ReturnToCheckPoint()
    {
        if (GlobalGameManager.instance.ExistSaveData())
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            this.EndGame();
            this.Release();
            long num = -1L;
            while (PlayerModel.instance.GetWaitingCreature(out num))
            {
            }
            GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
            GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
            GlobalGameManager.instance.SaveGlobalData();
            GlobalGameManager.instance.LoadData(SaveType.CHECK_POINT);
            CreatureGenerateInfoManager.Instance.Init();
            GlobalGameManager.instance.lastLoaded = true;
            GlobalGameManager.instance.loadingScene = "StoryEndScene";
            GlobalGameManager.instance.loadingScreen.LoadScene("Main");
            return;
        }
        Debug.LogError("save file not found");
    }

    // Token: 0x0600308C RID: 12428 RVA: 0x001474A8 File Offset: 0x001456A8
    public void MoveToCredit()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        this.EndGame();
        this.Release();
        GlobalEtcDataModel.instance.trueEndingDone = true;
        GlobalEtcDataModel.instance.UpdateUnlockedMaxDay(51);
        GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
        GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
        GlobalGameManager.instance.SaveGlobalData();
        GlobalGameManager.instance.saveState = "story";
        GlobalGameManager.instance.SaveUnlimitData();
        GlobalGameManager.instance.loadingScene = "EndingLoadingScene";
        GlobalGameManager.instance.loadingScreen.LoadScene("EndingCredit");
    }

    // Token: 0x0600308D RID: 12429 RVA: 0x00147564 File Offset: 0x00145764
    public void RestartGame()
    {
        if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
        {
            if (GlobalGameManager.instance.ExistUnlimitData())
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                this.EndGame();
                this.Release();
                GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
                GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
                GlobalGameManager.instance.LoadUnlimitData();
                GlobalGameManager.instance.loadingScene = "StoryEndScene";
                GlobalGameManager.instance.loadingScreen.LoadScene("Main");
            }
        }
        else if (GlobalGameManager.instance.gameMode == GameMode.STORY_MODE)
        {
            if (!GlobalGameManager.instance.ExistSaveData())
            {
                Debug.LogError("save file not found");
                return;
            }
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            this.EndGame();
            this.Release();
            GlobalGameManager.instance.sceneDataSaver.currentBgmVolume = BgmManager.instance.currentBgmVolume;
            GlobalGameManager.instance.sceneDataSaver.currentVolume = BgmManager.instance.currentMasterVolume;
            GlobalGameManager.instance.LoadGlobalData();
            GlobalGameManager.instance.LoadData(SaveType.LASTDAY);
            GlobalGameManager.instance.lastLoaded = true;
            GlobalGameManager.instance.loadingScene = "StoryEndScene";
            GlobalGameManager.instance.loadingScreen.LoadScene("Main");
        }
        else if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            this.EndGame();
            this.Release();
            string name = SceneManager.GetActiveScene().name;
            GlobalGameManager.instance.lastLoaded = true;
            GlobalGameManager.instance.loadingScene = "TitleEndScene";
            GlobalGameManager.instance.loadingScreen.LoadScene(name);
        }
    }

    // Token: 0x0600308E RID: 12430 RVA: 0x0014774C File Offset: 0x0014594C
    private IEnumerator LoadScene()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("StoryV2");
        while (!ao.isDone)
        {
            yield return true;
        }
        yield break;
    }

    // Token: 0x0600308F RID: 12431 RVA: 0x00147760 File Offset: 0x00145960
    private IEnumerator Reload()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        while (!ao.isDone)
        {
            yield return true;
        }
        yield break;
    }

    // Token: 0x06003090 RID: 12432 RVA: 0x0002D0FC File Offset: 0x0002B2FC
    public void ExitGame()
    {
        Add_On.SaveBackUp();
        Application.Quit();
    }

    // Token: 0x06003091 RID: 12433 RVA: 0x00147774 File Offset: 0x00145974
    private void UpdateGameSpeed()
    {
        if (this.state == GameState.PLAYING)
        {
            if (this.BossEvent)
            {
                this.SetPlaySpeedForcely(this.currentSpeedValue);
                return;
            }
            int num = this.gameSpeedLevel;
            if (num != 1)
            {
                if (num != 2)
                {
                    if (num == 3)
                    {
                        Time.timeScale = 2f;
                        Time.fixedDeltaTime = 0.03f;
                    }
                }
                else
                {
                    Time.timeScale = 1.5f;
                    Time.fixedDeltaTime = 0.025f;
                }
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
        }
        else if (this.state == GameState.PAUSE)
        {
            Time.timeScale = 0f;
        }
    }

    // Token: 0x06003092 RID: 12434 RVA: 0x0014782C File Offset: 0x00145A2C
    public void SetPlaySpeedForcely(float value)
    {
        if (!this.BossEvent)
        {
            return;
        }
        this.currentSpeedValue = value;
        Time.timeScale = this.currentSpeedValue;
        if (this.currentSpeedValue <= 1f)
        {
            this.gameSpeedLevel = 1;
            Time.fixedDeltaTime = 0.02f;
        }
        else if (this.currentSpeedValue <= 1.5f)
        {
            if (this.currentSpeedValue >= 1.4f)
            {
                this.gameSpeedLevel = 2;
            }
            Time.fixedDeltaTime = 0.025f;
        }
        else
        {
            if (this.currentSpeedValue >= 2f)
            {
                this.gameSpeedLevel = 3;
            }
            Time.fixedDeltaTime = 0.03f;
        }
        if (PlaySpeedSettingUI.instance != null)
        {
            PlaySpeedSettingUI.instance.UpdateButton();
        }
    }

    // Token: 0x06003093 RID: 12435 RVA: 0x0002D108 File Offset: 0x0002B308
    public void Pause()
    {
        this.state = GameState.PAUSE;
        this.UpdateGameSpeed();
    }

    // Token: 0x06003094 RID: 12436 RVA: 0x0002D117 File Offset: 0x0002B317
    public void Resume()
    {
        this.state = GameState.PLAYING;
        this.UpdateGameSpeed();
    }

    // Token: 0x06003095 RID: 12437 RVA: 0x001478F0 File Offset: 0x00145AF0
    public void Pause(PAUSECALL caller)
    {
        int num = (int)this.currentPauseCaller;
        if (num <= (int)caller)
        {
            this.currentPauseCaller = caller;
        }
        if (this.state == GameState.PAUSE)
        {
            return;
        }
        this.Pause();
    }

    // Token: 0x06003096 RID: 12438 RVA: 0x00147928 File Offset: 0x00145B28
    public void Resume(PAUSECALL caller)
    {
        int num = (int)this.currentPauseCaller;
        if (num <= (int)caller)
        {
            this.Resume();
            this.currentPauseCaller = PAUSECALL.NONE;
            return;
        }
    }

    // Token: 0x06003097 RID: 12439 RVA: 0x0002D126 File Offset: 0x0002B326
    public void SetPlaySpeed(int level)
    {
        if (level <= 0 || level > 3)
        {
            return;
        }
        if (EscapeUI.instance.IsOpened)
        {
            return;
        }
        this.state = GameState.PLAYING;
        this.gameSpeedLevel = level;
        this.UpdateGameSpeed();
    }

    // Token: 0x06003098 RID: 12440 RVA: 0x0002D15B File Offset: 0x0002B35B
    public void TutorialPause()
    {
        this.state = GameState.PAUSE;
    }

    // Token: 0x06003099 RID: 12441 RVA: 0x0002D164 File Offset: 0x0002B364
    public void TutorialResume()
    {
        if (this.state == GameState.PAUSE)
        {
            this.state = GameState.PLAYING;
        }
    }

    // Token: 0x0600309A RID: 12442 RVA: 0x00147958 File Offset: 0x00145B58
    public void EndGame()
    {
        this.state = GameState.STOP;
        RabbitManager.instance.OnStageEnd();
        CreatureManager.instance.OnStageEnd();
        OfficerManager.instance.OnStageEnd();
        AgentManager.instance.OnStageEnd();
        RandomEventManager.instance.OnStageEnd();
        SefiraBossManager.Instance.OnStageEnd();
        CursorManager.instance.OnStageEnd();
        base.GetComponent<RootTimer>().RemoveTimer(NoticeName.EnergyTimer);
        base.GetComponent<RootTimer>().RemoveTimer(NoticeName.AutoSaveTimer);
    }

    // Token: 0x0600309B RID: 12443 RVA: 0x001479D4 File Offset: 0x00145BD4
    private void FixedUpdate()
    {
        this.FixedUpdateProccess();
        if (this.state == GameState.PLAYING)
        {
            this.playTime += Time.deltaTime;
            RabbitManager.instance.OnFixedUpdate();
            CreatureManager.instance.OnFixedUpdate();
            SpecialEventManager.instance.OnFixedUpdate();
            OrdealManager.instance.OnFixedUpdate();
            OfficerManager.instance.OnFixedUpdate();
            AgentManager.instance.OnFixedUpdate();
            RandomEventManager.instance.OnFixedUpdate();
            GlobalBulletManager.instance.OnFixedUpdate();
            Notice.instance.Send(NoticeName.FixedUpdate, new object[0]);
        }
    }

    // Token: 0x0600309C RID: 12444 RVA: 0x0002D179 File Offset: 0x0002B379
    private void Update()
    {
        Notice.instance.Send(NoticeName.Update, new object[0]);
        RandomEventManager.instance.OnUpdate();
    }

    // Token: 0x0600309D RID: 12445 RVA: 0x00147A68 File Offset: 0x00145C68
    public void FixedUpdateProccess()
    {
        int day = PlayerModel.instance.GetDay();
        float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
        float energy = EnergyModel.instance.GetEnergy();
        if (energy >= energyNeed)
        {
        }
        if (this.emergency)
        {
            this.elapsed += Time.deltaTime;
            if (this.elapsed > this.emergencyReturn)
            {
                this.elapsed = 0f;
                PlayerModel.emergencyController.ReduceSore(1f);
            }
            if (PlayerModel.instance.currentEmergencyLevel == EmergencyLevel.NORMAL)
            {
                this.emergency = false;
                this.elapsed = 0f;
            }
        }
    }

    // Token: 0x0600309E RID: 12446 RVA: 0x0002D19A File Offset: 0x0002B39A
    public void SuccessStage()
    {
        this.stageEnded = true;
        GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared = true;
    }

    // Token: 0x0600309F RID: 12447 RVA: 0x0002D1B3 File Offset: 0x0002B3B3
    public void RevertSuccess()
    {
        this.stageEnded = false;
        GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared = false;
    }

    // Token: 0x060030A0 RID: 12448 RVA: 0x00147B08 File Offset: 0x00145D08
    public void ClearStage()
    {
        int day = PlayerModel.instance.GetDay();
        float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
        float energy = EnergyModel.instance.GetEnergy();
        if (energy >= energyNeed)
        {
            this.ClearAction();
        }
    }

    // Token: 0x060030A1 RID: 12449 RVA: 0x00147B44 File Offset: 0x00145D44
    public void ClearAction()
    {
        this.stageEnded = true;
        this.ManageStarted = false;
        Time.timeScale = 0f;
        this.EndGame();
        PlayerModel.emergencyController.OnStageEnd();
        Notice.instance.Send(NoticeName.OnStageEnd, new object[0]);
        ResultScreen.instance.OnSuccessManagement();
    }

    // Token: 0x060030A2 RID: 12450 RVA: 0x00147B98 File Offset: 0x00145D98
    public void ExitStage()
    {
        if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL)
        {
            return;
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        this.stageEnded = false;
        int day = PlayerModel.instance.GetDay();
        PlayerModel.instance.Nextday();
        this.Release();
        if (day == 0)
        {
            GlobalEtcDataModel.instance.day1clearCount++;
        }
        if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
        {
            if (PlayerModel.instance.GetDay() >= 99)
            {
                GlobalGameManager.instance.ReleaseGame();
                GlobalGameManager.instance.loadingScene = "DayEndScene";
                GlobalGameManager.instance.loadingScreen.LoadScene("Intro");
                return;
            }
            GlobalGameManager.instance.loadingScene = "DayEndScene";
            GlobalGameManager.instance.loadingScreen.LoadScene("Main");
        }
        else
        {
            GlobalGameManager.instance.loadingScene = "DayEndScene";
            GlobalGameManager.instance.loadingScreen.LoadScene("StoryV2");
        }
        GlobalGameManager.instance.saveState = "story";
        int day2 = this.playerModel.GetDay();
    }

    // Token: 0x060030A3 RID: 12451 RVA: 0x00147CC0 File Offset: 0x00145EC0
    public void GameOverEnding()
    {
        GlobalGameManager.instance.SaveGlobalData();
        PlayerModel.instance.SetKetherGameOver();
        this.EndGame();
        this.Release();
        GlobalGameManager.instance.loadingScene = "DayEndScene";
        GlobalGameManager.instance.loadingScreen.LoadScene("StoryV2");
    }

    // Token: 0x060030A4 RID: 12452 RVA: 0x0002D1CC File Offset: 0x0002B3CC
    public void SpecialEnding()
    {
        this.Release();
        SceneManager.LoadSceneAsync("Ending");
    }

    // Token: 0x060030A5 RID: 12453 RVA: 0x00147D10 File Offset: 0x00145F10
    private void Release()
    {
        CameraMover.instance.SetSettingToDefault();
        RabbitManager.instance.OnStageRelease();
        CreatureManager.instance.OnStageRelease();
        SpecialEventManager.instance.OnStageRelease();
        OrdealManager.instance.OnStageRelease();
        OfficerManager.instance.OnStageRelease();
        AgentManager.instance.OnStageRelease();
        PlayerModel.emergencyController.OnStageRelease();
        BgmManager.instance.OnStageRelease();
        OverlayManager.Instance.SaveState();
        GlobalBulletManager.instance.OnStageRelease();
        Notice.instance.Send(NoticeName.OnReleaseGameManager, new object[0]);
    }

    // Token: 0x060030A6 RID: 12454 RVA: 0x0002D1DF File Offset: 0x0002B3DF
    public void Quit()
    {
        Application.Quit();
    }

    // Token: 0x060030A7 RID: 12455 RVA: 0x0002D1E6 File Offset: 0x0002B3E6
    public PAUSECALL GetCurrentPauseCaller()
    {
        return this.currentPauseCaller;
    }

    // Token: 0x060030A8 RID: 12456 RVA: 0x00147DA0 File Offset: 0x00145FA0
    public int GetMoneyReward()
    {
        int day = PlayerModel.instance.GetDay();
        StageRewardTypeInfo data = StageRewardTypeList.instance.GetData(day + 1);
        List<AgentModel> list = new List<AgentModel>();
        foreach (StageRewardTypeInfo.AgentRewardInfo agentRewardInfo in data.agentList)
        {
            AgentModel agentModel = AgentManager.instance.AddSpareAgentModel();
            list.Add(agentModel);
            agentModel.SetCurrentSefira(agentRewardInfo.sephira);
            agentModel.GetMovableNode().SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(agentRewardInfo.sephira));
        }
        int num = Mathf.Max(0, data.money + data.money * (int)((double)this.GetPenaltyValueByDead() + 0.5) - this.GetPenaltyValueByCreature());
        if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.MALKUT))
        {
            int num2 = (int)Mathf.Max(1f, (float)num * 0.2f + 0.5f);
            num += num2;
        }
        if (day == 48 && GlobalGameManager.instance.gameMode != GameMode.UNLIMIT_MODE)
        {
            num += 20;
        }
        MoneyModel.instance.Add(num);
        return num;
    }

    // Token: 0x060030A9 RID: 12457 RVA: 0x00147EDC File Offset: 0x001460DC
    public int GetPenaltyValueByCreature()
    {
        List<CreatureModel> escapedCreatures = SefiraManager.instance.GetEscapedCreatures();
        int num = 0;
        foreach (CreatureModel creatureModel in escapedCreatures)
        {
            RiskLevel riskLevel = creatureModel.metaInfo.GetRiskLevel();
            if (riskLevel == RiskLevel.ALEPH)
            {
                num = 1000;
                break;
            }
            if (creatureModel.script is PinkCorps)
            {
                num = 1000;
                break;
            }
            num += GameManager.Penalty[(int)riskLevel];
        }
        return num;
    }

    // Token: 0x060030AA RID: 12458 RVA: 0x00147F80 File Offset: 0x00146180
    public float GetPenaltyValueByDead()
    {
        int num = 0;
        float result = 0f;
        int count = AgentManager.instance.GetAgentList().Count;
        foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
        {
            if (agentModel.IsDead() || agentModel.unconAction is Uncontrollable_RedShoes)
            {
                num++;
            }
        }
        float survivalRate = AgentManager.instance.GetSurvivalRate();
        if (survivalRate == 1f)
        {
            result = 2f;
        }
        else if (survivalRate < 1f && (double)survivalRate >= 0.9)
        {
            result = 1.5f;
        }
        else if ((double)survivalRate < 0.9 && (double)survivalRate >= 0.7)
        {
            result = 1.25f;
        }
        else if ((double)survivalRate < 0.7 && (double)survivalRate >= 0.5)
        {
            result = 1f;
        }
        else if ((double)survivalRate < 0.5 && (double)survivalRate >= 0.3)
        {
            result = 0.5f;
        }
        else if ((double)survivalRate < 0.3)
        {
            result = 0f;
        }
        return result;
    }

    // Token: 0x060030AB RID: 12459 RVA: 0x001480FC File Offset: 0x001462FC
    public StageRank GetStageRank(float rate)
    {
        if (rate >= 1f)
        {
            return StageRank.S;
        }
        if (rate >= 0.9f)
        {
            return StageRank.A;
        }
        if (rate >= 0.7f)
        {
            return StageRank.B;
        }
        if (rate >= 0.5f)
        {
            return StageRank.C;
        }
        if (rate >= 0.3f)
        {
            return StageRank.D;
        }
        return StageRank.F;
    }

    // Token: 0x060030AC RID: 12460 RVA: 0x0002D1EE File Offset: 0x0002B3EE
    static GameManager()
    {
    }

    // Token: 0x04002E56 RID: 11862
    [NonSerialized]
    public static readonly int[] Penalty = new int[]
    {
        0,
        1,
        3,
        5,
        0
    };

    // Token: 0x04002E57 RID: 11863
    private const int _finalStageRewardDay = 48;

    // Token: 0x04002E58 RID: 11864
    private const int _finalRewardAdditionalLob = 20;

    // Token: 0x04002E59 RID: 11865
    private string saveFileName;

    // Token: 0x04002E5A RID: 11866
    private static GameManager _currentGameManager;

    // Token: 0x04002E5B RID: 11867
    private CurrentUIState currentUIState;

    // Token: 0x04002E5C RID: 11868
    public bool ManageStarted;

    // Token: 0x04002E5D RID: 11869
    public GameState state;

    // Token: 0x04002E5E RID: 11870
    private float elapsed;

    // Token: 0x04002E5F RID: 11871
    public float emergencyReturn = 15f;

    // Token: 0x04002E60 RID: 11872
    public bool emergency;

    // Token: 0x04002E61 RID: 11873
    private bool stageEnded;

    // Token: 0x04002E62 RID: 11874
    private PlayerModel playerModel;

    // Token: 0x04002E63 RID: 11875
    public int gameSpeedLevel = 1;

    // Token: 0x04002E64 RID: 11876
    private PAUSECALL currentPauseCaller;

    // Token: 0x04002E65 RID: 11877
    private float playTime;

    // Token: 0x04002E66 RID: 11878
    private float currentSpeedValue = 1f;
}
