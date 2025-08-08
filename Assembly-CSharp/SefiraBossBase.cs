/*
+public virtual void AdjustOrdealSpawnTime(int[] _ordealSpawnTime) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007FF RID: 2047
public class SefiraBossBase
{
	// Token: 0x06003F22 RID: 16162 RVA: 0x00187600 File Offset: 0x00185800
	public SefiraBossBase()
	{
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06003F23 RID: 16163 RVA: 0x00036D52 File Offset: 0x00034F52
	public virtual int QliphothOverloadLevel
	{
		get
		{
			return CreatureOverloadManager.instance.GetQliphothOverloadLevel();
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06003F24 RID: 16164 RVA: 0x00036D5E File Offset: 0x00034F5E
	public Sefira Sefira
	{
		get
		{
			return SefiraManager.instance.GetSefira(this.sefiraEnum);
		}
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x000043A5 File Offset: 0x000025A5
	public virtual void OnStageStart()
	{
	}

	// Token: 0x06003F26 RID: 16166 RVA: 0x000043A5 File Offset: 0x000025A5
	public virtual void OnKetherStart()
	{
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x000043A5 File Offset: 0x000025A5
	public virtual void OnStageEnd()
	{
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00187654 File Offset: 0x00185854
	public virtual void FixedUpdate()
	{
		if (this._cameraDescTimer.started && this._cameraDescTimer.RunTimer())
		{
			this._cameraDescTimer.StartTimer(3f * UnityEngine.Random.value);
			if (UnityEngine.Random.value <= 0.3f)
			{
				string empty = string.Empty;
				SefiraBossDescType descType = this.GetDescType(0.5f);
				if (SefiraBossManager.Instance.TryGetBossDesc(this.sefiraEnum, descType, out empty))
				{
					SefiraBossUI.TextColorSet color = SefiraBossUI.Instance.GetColor(this.sefiraEnum);
					SefiraBossDescUI sefiraBossDescUI = SefiraBossDescUI.GenDesc(empty, color.TextColor, color.OutlineColor, this.GetDescFreq());
					sefiraBossDescUI.baseScript = this;
					this.descList.Add(sefiraBossDescUI);
				}
			}
		}
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x00036D70 File Offset: 0x00034F70
	public void OnRemoveDesc(SefiraBossDescUI ui)
	{
		this.descList.Remove(ui);
	}

	// Token: 0x06003F2A RID: 16170 RVA: 0x00187710 File Offset: 0x00185910
	public virtual void Update()
	{
		if (this._closeTimer.started)
		{
			if (this._closeEffectMethod != null)
			{
				this._closeEffectMethod(this.currentCloseEffectParam.ToArray());
			}
			if (this._closeTimer.RunTimer())
			{
				this._closeTimer.elapsed = this._closeTimer.maxTime;
				this._closeEffectMethod(this.currentCloseEffectParam.ToArray());
			}
		}
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x0018778C File Offset: 0x0018598C
	public virtual void OnCleared()
	{
		this.currentCloseEffectParam.Clear();
		this._closeTimer.StartTimer(5f);
		this._cameraDescTimer.StopTimer();
		this.MakeSoundAttachCamera("SefiraBoss/Boss_Dead");
		SefiraBossCloseEffectType sefiraBossCloseEffectType = this.closeEffectType;
		if (sefiraBossCloseEffectType == SefiraBossCloseEffectType.DEFAULT)
		{
			CameraFilterPack_FX_EarthQuake cameraFilterPack_FX_EarthQuake = Camera.main.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>();
			cameraFilterPack_FX_EarthQuake.Speed = 0f;
			CameraFilterPack_Distortion_ShockWaveManual cameraFilterPack_Distortion_ShockWaveManual = Camera.main.gameObject.AddComponent<CameraFilterPack_Distortion_ShockWaveManual>();
			cameraFilterPack_Distortion_ShockWaveManual.Size = 2f;
			cameraFilterPack_Distortion_ShockWaveManual.Value = 0f;
			this._closeEffectMethod = new SefiraBossBase.CloseEffect(this.DefaultClearEffect);
			this.currentCloseEffectParam.Add(cameraFilterPack_FX_EarthQuake);
			this.currentCloseEffectParam.Add(cameraFilterPack_Distortion_ShockWaveManual);
		}
		this.ClearDescTexts();
		if (SefiraBossManager.Instance.IsKetherBoss())
		{
			return;
		}
		string empty = string.Empty;
		SefiraBossDescType type = SefiraBossDescType.FINISH;
		if (SefiraBossManager.Instance.TryGetBossDesc(this.sefiraEnum, type, out empty))
		{
			SefiraBossUI.TextColorSet color = SefiraBossUI.Instance.GetColor(this.sefiraEnum);
			SefiraBossDescUI sefiraBossDescUI = SefiraBossDescUI.GenFinishDesc(empty, color.TextColor, color.OutlineColor, 0.1f);
			sefiraBossDescUI.baseScript = this;
		}
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x001878BC File Offset: 0x00185ABC
	public virtual void DefaultClearEffect(params object[] param)
	{
		CameraFilterPack_FX_EarthQuake cameraFilterPack_FX_EarthQuake = (CameraFilterPack_FX_EarthQuake)param[0];
		CameraFilterPack_Distortion_ShockWaveManual cameraFilterPack_Distortion_ShockWaveManual = (CameraFilterPack_Distortion_ShockWaveManual)param[1];
		float rate = this._closeTimer.Rate;
		float speed = Mathf.Lerp(0f, 100f, rate);
		float value = SefiraBossUI.Instance.defaultCloseShockWaveCurve.Evaluate(rate);
		cameraFilterPack_FX_EarthQuake.Speed = speed;
		cameraFilterPack_Distortion_ShockWaveManual.Value = value;
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x000043A5 File Offset: 0x000025A5
	public virtual void OnOverloadActivated(int currentLevel)
	{
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x0018791C File Offset: 0x00185B1C
	public virtual void OnDestroy()
	{
		foreach (SefiraBossCreatureModel sefiraBossCreatureModel in this.modelList)
		{
			sefiraBossCreatureModel.OnDestroy();
		}
	}

	// Token: 0x06003F2F RID: 16175 RVA: 0x000044AF File Offset: 0x000026AF
	public virtual bool IsCleared()
	{
		return false;
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x00036D7F File Offset: 0x00034F7F
	public virtual bool IsReadyToClose()
	{
		return !this._closeTimer.started;
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x00036D8F File Offset: 0x00034F8F
	public virtual DamageInfo GetDamageInfo()
	{
		return SefiraBossBase.DefaultDamageInfo;
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x00036D96 File Offset: 0x00034F96
	public virtual DefenseInfo GetDefenseInfo()
	{
		return SefiraBossBase.DefaultDefenseInfo;
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x00187978 File Offset: 0x00185B78
	public virtual SefiraBossDescType GetDescType(float defaultProb = 0.5f)
	{
		SefiraBossDescType result = SefiraBossDescType.DEFAULT;
		if (UnityEngine.Random.value > defaultProb && this.QliphothOverloadLevel <= 5)
		{
			result = (SefiraBossDescType)this.QliphothOverloadLevel;
		}
		return result;
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x00030C4F File Offset: 0x0002EE4F
	public virtual float GetDescFreq()
	{
		return 0.3f;
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x000043A5 File Offset: 0x000025A5
	public virtual void OnChangePhase()
	{
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x001879A8 File Offset: 0x00185BA8
	public virtual void ClearDescTexts()
	{
		foreach (SefiraBossDescUI sefiraBossDescUI in this.descList)
		{
			UnityEngine.Object.Destroy(sefiraBossDescUI.gameObject);
		}
		this.descList.Clear();
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x00187A14 File Offset: 0x00185C14
	public virtual SoundEffectPlayer MakeSound(string src)
	{
		SoundEffectPlayer result;
		try
		{
			SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce(src, this.Sefira.sefiraPassage.centerNode.GetPosition());
			result = soundEffectPlayer;
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x00187A64 File Offset: 0x00185C64
	public virtual SoundEffectPlayer MakeSoundAttachCamera(string src)
	{
		SoundEffectPlayer soundEffectPlayer = this.MakeSound(src);
		if (soundEffectPlayer == null)
		{
			return null;
		}
		soundEffectPlayer.transform.SetParent(Camera.main.transform);
		soundEffectPlayer.transform.localPosition = new Vector3(0f, 0f, 1f);
		return soundEffectPlayer;
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x00014079 File Offset: 0x00012279
	public virtual bool IsStartEmergencyBgm()
	{
		return true;
	}

	// <Mod>
	public virtual void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
	{

	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x00187ABC File Offset: 0x00185CBC
	// Note: this type is marked as 'beforefieldinit'.
	static SefiraBossBase()
	{
	}

	// Token: 0x040039CA RID: 14794
	private const float _closeEffectTime = 5f;

	// Token: 0x040039CB RID: 14795
	private const float _defaultDescFreq = 3f;

	// Token: 0x040039CC RID: 14796
	public const float _descAppearProb = 0.3f;

	// Token: 0x040039CD RID: 14797
	public const string generalScript = "SefiraBossBase";

	// Token: 0x040039CE RID: 14798
	public const string generalAnim = "SefiraBossGeneral";

	// Token: 0x040039CF RID: 14799
	public const string bgmSoundPrefix = "Sounds/BGM/Boss/";

	// Token: 0x040039D0 RID: 14800
	private List<object> currentCloseEffectParam = new List<object>();

	// Token: 0x040039D1 RID: 14801
	public List<SefiraBossCreatureModel> modelList = new List<SefiraBossCreatureModel>();

	// Token: 0x040039D2 RID: 14802
	public SefiraEnum sefiraEnum = SefiraEnum.DUMMY;

	// Token: 0x040039D3 RID: 14803
	public SefiraBossCloseEffectType closeEffectType;

	// Token: 0x040039D4 RID: 14804
	public static DamageInfo DefaultDamageInfo = new DamageInfo(RwbpType.R, 1f);

	// Token: 0x040039D5 RID: 14805
	public static DefenseInfo DefaultDefenseInfo = new DefenseInfo
	{
		R = 1f,
		W = 1f,
		B = 1f,
		P = 1f
	};

	// Token: 0x040039D6 RID: 14806
	public UnscaledTimer _closeTimer = new UnscaledTimer();

	// Token: 0x040039D7 RID: 14807
	public SefiraBossBase.CloseEffect _closeEffectMethod;

	// Token: 0x040039D8 RID: 14808
	protected Timer _cameraDescTimer = new Timer();

	// Token: 0x040039D9 RID: 14809
	public List<SefiraBossDescUI> descList = new List<SefiraBossDescUI>();

	// Token: 0x02000800 RID: 2048
	// (Invoke) Token: 0x06003F3C RID: 16188
	public delegate void CloseEffect(params object[] param);
}
