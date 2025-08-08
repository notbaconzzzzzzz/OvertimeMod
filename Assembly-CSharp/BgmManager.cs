using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000359 RID: 857
public class BgmManager : MonoBehaviour, IObserver
{
	// Token: 0x06001A62 RID: 6754 RVA: 0x0001D61E File Offset: 0x0001B81E
	public BgmManager()
	{
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0001D659 File Offset: 0x0001B859
	public static BgmManager instance
	{
		get
		{
			return BgmManager._instance;
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0001D660 File Offset: 0x0001B860
	public AudioSource audioSource
	{
		get
		{
			if (this._src == null)
			{
				this._src = base.gameObject.GetComponent<AudioSource>();
			}
			return this._src;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06001A65 RID: 6757 RVA: 0x0001D68A File Offset: 0x0001B88A
	private PlayerModel.EmergencyController controller
	{
		get
		{
			return PlayerModel.emergencyController;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0001D691 File Offset: 0x0001B891
	private float recoverMult
	{
		get
		{
			if (!this.canRecover)
			{
				return 0f;
			}
			if (this.stageAgentMax == 0)
			{
				return 1f;
			}
			return (float)this.currentAgent / (float)this.stageAgentMax;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0001D6CD File Offset: 0x0001B8CD
	// (set) Token: 0x06001A67 RID: 6759 RVA: 0x0001D6C4 File Offset: 0x0001B8C4
	private float currentDangerScore
	{
		get
		{
			return this._currentDangerScore;
		}
		set
		{
			this._currentDangerScore = value;
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0001D6D5 File Offset: 0x0001B8D5
	public bool isPlaying
	{
		get
		{
			return this.audioSource.isPlaying;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0001D6E2 File Offset: 0x0001B8E2
	public bool IsBossActivated
	{
		get
		{
			return SefiraBossManager.Instance.IsAnyBossSessionActivated();
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x0001D6EE File Offset: 0x0001B8EE
	private void Awake()
	{
		BgmManager._instance = this;
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x0001D6F6 File Offset: 0x0001B8F6
	private void Start()
	{
		this.audioSource.clip = this.normal.GetRandomClip();
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0001D70E File Offset: 0x0001B90E
	public void InitVolume(float master, float bgm)
	{
		this.currentMasterVolume = master;
		this.currentBgmVolume = bgm;
		AudioListener.volume = master;
		this.audioSource.volume = bgm;
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000E1168 File Offset: 0x000DF368
	public BgmManager.AudioClipList GetClipList(EmergencyLevel level)
	{
		switch (level)
		{
		case EmergencyLevel.NORMAL:
			return this.normal;
		case EmergencyLevel.LEVEL1:
			return this.emergencyLevel_1;
		case EmergencyLevel.LEVEL2:
			return this.emergencyLevel_2;
		case EmergencyLevel.LEVEL3:
			return this.emergencyLevel_3;
		case EmergencyLevel.CHAOS:
			return this.emergencyLevel_3;
		default:
			return this.normal;
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x000E11C0 File Offset: 0x000DF3C0
	private void FixedUpdate()
	{
		this.PlayingState = this.isPlaying;
		float currentScore = this.controller.GetCurrentScore();
		if (this.recoverBlockTimer.started)
		{
			if (this.recoverBlockTimer.RunTimer())
			{
				this.elapsed = 0f;
			}
		}
		else if (currentScore > 0f)
		{
			this.elapsed += Time.deltaTime;
			if (this.elapsed >= this.recoveryTime)
			{
				this.elapsed = 0f;
				float num = currentScore;
				num -= this.recoverMult;
				if (num < 0f)
				{
					num = 0f;
				}
				if (num > 100f)
				{
					num = 100f;
				}
				this.controller.SetCurrentScore(num);
			}
		}
		else
		{
			this.elapsed = 0f;
		}
		if (!this.isPlaying)
		{
			if (this._isUnique)
			{
				this._isUnique = false;
			}
			if (!this.randTimer.started)
			{
				this.randTimer.StartTimer(UnityEngine.Random.Range(this.delayMin, this.delayTime));
			}
			else if (this.randTimer.RunTimer())
			{
				this.PlayBgm(this.GetCurrentClip(this.currentLevel));
			}
		}
		else if (this.randTimer.started)
		{
			this.randTimer.StopTimer();
		}
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000E1328 File Offset: 0x000DF528
	public void OnStageRelease()
	{
		this.isTimerRunning = false;
		this.currentLevel = EmergencyLevel.NORMAL;
		float num = 0f;
		float num2 = 0f;
		num = GlobalGameManager.instance.sceneDataSaver.currentVolume;
		num2 = GlobalGameManager.instance.sceneDataSaver.currentBgmVolume;
		try
		{
			EscapeUI.instance.MasterFill.fillAmount = num;
			EscapeUI.instance.MusicFill.fillAmount = num2;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		BgmManager.instance.InitVolume(num, num2);
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x0001D730 File Offset: 0x0001B930
	public void OnManagementStart()
	{
		this.stageAgentMax = AgentManager.instance.GetAgentList().Count;
		this.currentAgent = this.stageAgentMax;
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x0001D753 File Offset: 0x0001B953
	public void BlockRecover()
	{
		this.recoverBlockTimer.StartTimer(30f);
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0001D765 File Offset: 0x0001B965
	public void BlockRecoverInf()
	{
		this.recoverBlockTimer.StartTimer();
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0001D772 File Offset: 0x0001B972
	public void ReleaseRecoverBlock()
	{
		this.recoverBlockTimer.elapsed = this.recoverBlockTimer.maxTime;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x000E13BC File Offset: 0x000DF5BC
	public void SetBgm(EmergencyLevel level)
	{
		if (this.currentLevel == level)
		{
			return;
		}
		if (this.currentLevel > level)
		{
			return;
		}
		if (this.randTimer.started)
		{
			this.randTimer.StopTimer();
		}
		if (this.IsBossActivated)
		{
			return;
		}
		if (this._isUnique)
		{
			return;
		}
		this.currentLevel = level;
		this.PlayBgm(this.GetCurrentClip(this.currentLevel));
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x0001D78A File Offset: 0x0001B98A
	public void ResetBgm()
	{
		if (this.IsBossActivated)
		{
			return;
		}
		if (this._isUnique)
		{
			return;
		}
		if (this.currentLevel != EmergencyLevel.NORMAL)
		{
			this.currentLevel = EmergencyLevel.NORMAL;
			this.PlayBgm(this.GetCurrentClip(this.currentLevel));
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x000E1430 File Offset: 0x000DF630
	private AudioClip GetCurrentClip(EmergencyLevel level)
	{
		AudioClip result;
		if (this.IsBossActivated)
		{
			result = this.GetBossClip();
		}
		else
		{
			result = this.GetClipList(level).GetRandomClip();
		}
		return result;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000E1464 File Offset: 0x000DF664
	private void PlayBgm(AudioClip clip)
	{
		if (this.audioSource.isPlaying)
		{
			this.audioSource.Stop();
		}
		if (clip == null)
		{
			Debug.LogError("Clip Is Null");
			this.audioSource.clip = this.GetClip(this.currentLevel);
			this.audioSource.Play();
			return;
		}
		Debug.Log("<color=red>Bgm Play</color>" + clip.name);
		this.audioSource.clip = clip;
		this.audioSource.Play();
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x0001D7C8 File Offset: 0x0001B9C8
	public void SubAgent()
	{
		this.currentAgent--;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x0001D7D8 File Offset: 0x0001B9D8
	public void Halt()
	{
		this.audioSource.Pause();
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x0001D7E5 File Offset: 0x0001B9E5
	public void Resume()
	{
		this.audioSource.UnPause();
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x0001D7F2 File Offset: 0x0001B9F2
	public void BGMForcelyStop()
	{
		this.audioSource.Stop();
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000E14F4 File Offset: 0x000DF6F4
	private AudioClip GetClip(EmergencyLevel level)
	{
		AudioClip result = null;
		switch (level)
		{
		case EmergencyLevel.NORMAL:
			result = this.normal.GetRandomClip();
			break;
		case EmergencyLevel.LEVEL1:
			result = this.emergencyLevel_1.GetRandomClip();
			break;
		case EmergencyLevel.LEVEL2:
			result = this.emergencyLevel_2.GetRandomClip();
			break;
		case EmergencyLevel.LEVEL3:
			result = this.emergencyLevel_3.GetRandomClip();
			break;
		case EmergencyLevel.CHAOS:
			result = this.emergencyLevel_3.GetRandomClip();
			break;
		}
		return result;
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000043A5 File Offset: 0x000025A5
	public void OnNotice(string notice, params object[] param)
	{
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x0001D7FF File Offset: 0x0001B9FF
	public bool FadeOut()
	{
		if (!this.IsBossActivated)
		{
			this.fadeEnabled = true;
			this.fadeElap = 0f;
			this.isFadeIn = false;
		}
		return !this.IsBossActivated;
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0001D82E File Offset: 0x0001BA2E
	public bool FadeIn()
	{
		if (!this.IsBossActivated)
		{
			this.fadeEnabled = true;
			this.fadeElap = 0f;
			this.isFadeIn = true;
		}
		return !this.IsBossActivated;
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000E1578 File Offset: 0x000DF778
	public void Update()
	{
		if (this.fadeEnabled)
		{
			this.fadeElap += Time.unscaledDeltaTime;
			float num = Mathf.Lerp(0f, this.currentBgmVolume, this.fadeElap / this.fadeTime);
			if (!this.isFadeIn)
			{
				num = this.currentBgmVolume - num;
			}
			this.audioSource.volume = num;
			if (this.fadeElap >= this.fadeTime)
			{
				this.fadeElap = 0f;
				this.fadeEnabled = false;
				if (this.isFadeIn)
				{
					if (this.fadeInEvent != null)
					{
						this.fadeInEvent();
						this.fadeInEvent = null;
					}
				}
				else if (this.fadeOutEvent != null)
				{
					this.fadeOutEvent();
					this.fadeOutEvent = null;
				}
			}
		}
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x0001D85D File Offset: 0x0001BA5D
	public void ClearUniqueBgm()
	{
		this._isUnique = false;
		this.ResetBgm();
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000E1650 File Offset: 0x000DF850
	public void OnClick(string level)
	{
		float levelScore = this.controller.GetLevelScore(CreatureTypeInfo.GetRiskLevelStringToEnum(level));
		this.controller.AddScore(levelScore);
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000E167C File Offset: 0x000DF87C
	public void LoadUniqueBgm(string src)
	{
		AudioClip audioClip = Resources.Load<AudioClip>(src);
		if (audioClip != null)
		{
			this.audioSource.Stop();
			this.audioSource.clip = audioClip;
			this.audioSource.Play();
			this._isUnique = true;
		}
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0001D86C File Offset: 0x0001BA6C
	public void SetMasterSoundVolume(float val)
	{
		AudioListener.volume = Mathf.Clamp(val, 0f, 1f);
		this.currentMasterVolume = AudioListener.volume;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0001D88E File Offset: 0x0001BA8E
	public void SetBgmSoundVolume(float val)
	{
		this.audioSource.volume = Mathf.Clamp(val, 0f, 1f);
		this.currentBgmVolume = this.audioSource.volume;
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x0001D8BC File Offset: 0x0001BABC
	public AudioClip GetBossClip()
	{
		return this._bossClip;
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000E16C8 File Offset: 0x000DF8C8
	public void SetBossClip(string src)
	{
		AudioClip audioClip = Resources.Load<AudioClip>(src);
		if (audioClip != null)
		{
			this._bossClip = audioClip;
		}
		this.PlayBgm(this.GetCurrentClip(this.currentLevel));
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0001D8C4 File Offset: 0x0001BAC4
	public void SetBossClip()
	{
		this._bossClip = this.GetClip(EmergencyLevel.LEVEL2);
		this.PlayBgm(this.GetCurrentClip(this.currentLevel));
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000043A5 File Offset: 0x000025A5
	// Note: this type is marked as 'beforefieldinit'.
	static BgmManager()
	{
	}

	// Token: 0x04001B21 RID: 6945
	private const float recoverBlockTime = 30f;

	// Token: 0x04001B22 RID: 6946
	private static BgmManager _instance;

	// Token: 0x04001B23 RID: 6947
	private const string alarmBeep = "alertBeep";

	// Token: 0x04001B24 RID: 6948
	private AudioSource _src;

	// Token: 0x04001B25 RID: 6949
	public BgmManager.AudioClipList normal;

	// Token: 0x04001B26 RID: 6950
	public BgmManager.AudioClipList emergencyLevel_1;

	// Token: 0x04001B27 RID: 6951
	public BgmManager.AudioClipList emergencyLevel_2;

	// Token: 0x04001B28 RID: 6952
	public BgmManager.AudioClipList emergencyLevel_3;

	// Token: 0x04001B29 RID: 6953
	public float fadeTime;

	// Token: 0x04001B2A RID: 6954
	private float fadeElap;

	// Token: 0x04001B2B RID: 6955
	private bool fadeEnabled;

	// Token: 0x04001B2C RID: 6956
	private bool isFadeIn;

	// Token: 0x04001B2D RID: 6957
	public float currentBgmVolume;

	// Token: 0x04001B2E RID: 6958
	public float currentMasterVolume;

	// Token: 0x04001B2F RID: 6959
	public Text ScoreDisplay;

	// Token: 0x04001B30 RID: 6960
	public Text currentLevelDisplay;

	// Token: 0x04001B31 RID: 6961
	public Text SoundPlayTime;

	// Token: 0x04001B32 RID: 6962
	public BgmManager.FadeEffectEvent fadeInEvent;

	// Token: 0x04001B33 RID: 6963
	public BgmManager.FadeEffectEvent fadeOutEvent;

	// Token: 0x04001B34 RID: 6964
	private Timer randTimer = new Timer();

	// Token: 0x04001B35 RID: 6965
	private Timer recoverBlockTimer = new Timer();

	// Token: 0x04001B36 RID: 6966
	private bool isTimerRunning;

	// Token: 0x04001B37 RID: 6967
	public float delayMin = 10f;

	// Token: 0x04001B38 RID: 6968
	[Range(10f, 30f)]
	public float delayTime;

	// Token: 0x04001B39 RID: 6969
	private bool _isUnique;

	// Token: 0x04001B3A RID: 6970
	private float elapsed;

	// Token: 0x04001B3B RID: 6971
	private float recoveryTime = 5f;

	// Token: 0x04001B3C RID: 6972
	public bool canRecover = true;

	// Token: 0x04001B3D RID: 6973
	private float _currentDangerScore;

	// Token: 0x04001B3E RID: 6974
	private int stageAgentMax;

	// Token: 0x04001B3F RID: 6975
	private int currentAgent;

	// Token: 0x04001B40 RID: 6976
	public bool PlayingState;

	// Token: 0x04001B41 RID: 6977
	private EmergencyLevel currentLevel;

	// Token: 0x04001B42 RID: 6978
	private AudioClip _bossClip;

	// Token: 0x0200035A RID: 858
	[Serializable]
	public class AudioClipList
	{
		// Token: 0x06001A8B RID: 6795 RVA: 0x00004378 File Offset: 0x00002578
		public AudioClipList()
		{
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0001D8E5 File Offset: 0x0001BAE5
		public AudioClip GetRandomClip()
		{
			return this.GetClip(UnityEngine.Random.Range(0, this.list.Count));
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0001D8FE File Offset: 0x0001BAFE
		public AudioClip GetClip(int index)
		{
			if (index >= this.list.Count || index < 0)
			{
				return null;
			}
			this.lastPlayed = index;
			return this.list[index];
		}

		// Token: 0x04001B43 RID: 6979
		public List<AudioClip> list;

		// Token: 0x04001B44 RID: 6980
		public int lastPlayed;
	}

	// Token: 0x0200035B RID: 859
	// (Invoke) Token: 0x06001A8F RID: 6799
	public delegate bool FadeEffectEvent();
}
