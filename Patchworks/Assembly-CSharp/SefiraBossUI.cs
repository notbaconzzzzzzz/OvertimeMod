using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public class SefiraBossUI : MonoBehaviour
{
	// Token: 0x060042F7 RID: 17143 RVA: 0x0003942C File Offset: 0x0003762C
	public SefiraBossUI()
	{
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x060042F8 RID: 17144 RVA: 0x0003943F File Offset: 0x0003763F
	public static SefiraBossUI Instance
	{
		get
		{
			return SefiraBossUI._instnace;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x060042F9 RID: 17145 RVA: 0x0002B63D File Offset: 0x0002983D
	public Camera MainCamera
	{
		get
		{
			return Camera.main;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x060042FA RID: 17146 RVA: 0x00039446 File Offset: 0x00037646
	public Camera UiCamera
	{
		get
		{
			return UIActivateManager.instance.GetCam();
		}
	}

	// Token: 0x060042FB RID: 17147 RVA: 0x00039452 File Offset: 0x00037652
	private void Awake()
	{
		if (SefiraBossUI.Instance != null && SefiraBossUI.Instance.gameObject != null)
		{
			UnityEngine.Object.Destroy(SefiraBossUI.Instance.gameObject);
		}
		SefiraBossUI._instnace = this;
	}

	// Token: 0x060042FC RID: 17148 RVA: 0x0019A658 File Offset: 0x00198858
	private void Start()
	{
		IEnumerator enumerator = Camera.main.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Camera component = transform.GetComponent<Camera>();
				if (!(component == null))
				{
					if (!(component == Camera.main))
					{
						this.bossUI = component;
					}
				}
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
		this.bossUI.gameObject.SetActive(false);
		this.uiCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x0019A710 File Offset: 0x00198910
	public SefiraBossUI.PositionRatio GetRatio()
	{
		if (!SefiraManager.instance.IsOpened(SefiraEnum.TIPERERTH1))
		{
			return this.ratio[0];
		}
		if (SefiraManager.instance.IsOpened(SefiraEnum.BINAH) || SefiraManager.instance.IsOpened(SefiraEnum.CHOKHMAH))
		{
			return this.ratio[1];
		}
		return this.ratio[2];
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x0019A774 File Offset: 0x00198974
	private void Update()
	{
		if (this._enterEffectTimer.started)
		{
			CameraFilterPack_AAA_SuperComputer uicamFilter = this.GetUICamFilter(false);
			if (uicamFilter != null)
			{
				uicamFilter.ShapeFormula = Mathf.Lerp(-1f, 0.8f, this._enterEffectTimer.Rate);
			}
			if (this._enterEffectTimer.RunTimer())
			{
				uicamFilter.ShapeFormula = 0.8f;
			}
		}
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x0019A7E0 File Offset: 0x001989E0
	public SefiraBossUI.TextColorSet GetColor(SefiraEnum sefiraEnum)
	{
		SefiraBossUI.TextColorSet result = null;
		foreach (SefiraBossUI.TextColorSet textColorSet in this.colorList)
		{
			if (textColorSet.sefira == sefiraEnum)
			{
				result = textColorSet;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004300 RID: 17152 RVA: 0x0019A84C File Offset: 0x00198A4C
	public void OnEnterSefiraBossSession()
	{
		CameraFilterPack_AAA_SuperComputer uicamFilter = this.GetUICamFilter(true);
		if (!uicamFilter.enabled)
		{
			uicamFilter.enabled = true;
			this._enterEffectTimer.StartTimer(1f);
		}
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x0019A884 File Offset: 0x00198A84
	public CameraFilterPack_AAA_SuperComputer GetUICamFilter(bool makeIfNotExist = true)
	{
		CameraFilterPack_AAA_SuperComputer cameraFilterPack_AAA_SuperComputer = this.UiCamera.gameObject.GetComponent<CameraFilterPack_AAA_SuperComputer>();
		if (cameraFilterPack_AAA_SuperComputer == null && makeIfNotExist)
		{
			cameraFilterPack_AAA_SuperComputer = this.UiCamera.gameObject.AddComponent<CameraFilterPack_AAA_SuperComputer>();
			cameraFilterPack_AAA_SuperComputer._AlphaHexa = 1f;
			cameraFilterPack_AAA_SuperComputer.ShapeFormula = -1f;
			cameraFilterPack_AAA_SuperComputer.Shape = 1.1f;
			cameraFilterPack_AAA_SuperComputer._BorderSize = -1.02f;
			cameraFilterPack_AAA_SuperComputer._BorderColor = Color.red;
			cameraFilterPack_AAA_SuperComputer._SpotSize = 2.5f;
			cameraFilterPack_AAA_SuperComputer.center = Vector2.zero;
			cameraFilterPack_AAA_SuperComputer.Radius = 0.77f;
			cameraFilterPack_AAA_SuperComputer.enabled = false;
		}
		return cameraFilterPack_AAA_SuperComputer;
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x0019A928 File Offset: 0x00198B28
	public void OnExitSefiraBossSession()
	{
		if (this._enterEffectTimer.started)
		{
			this._enterEffectTimer.StopTimer();
		}
		CameraFilterPack_AAA_SuperComputer uicamFilter = this.GetUICamFilter(false);
		if (uicamFilter != null)
		{
			uicamFilter.enabled = false;
		}
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x0019A96C File Offset: 0x00198B6C
	public void OnStageStart()
	{
		CameraFilterPack_AAA_SuperComputer uicamFilter = this.GetUICamFilter(false);
		if (uicamFilter != null)
		{
			uicamFilter.enabled = false;
		}
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated())
		{
			this.bossUI.gameObject.SetActive(true);
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.CHESED, false) || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E2))
			{
				this.uiCanvas.gameObject.SetActive(true);
			}
		}
		else
		{
			this.bossUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x0019A9FC File Offset: 0x00198BFC
	// Note: this type is marked as 'beforefieldinit'.
	static SefiraBossUI()
	{
	}

	// Token: 0x04003D95 RID: 15765
	private static SefiraBossUI _instnace = null;

	// Token: 0x04003D96 RID: 15766
	public Canvas canvas;

	// Token: 0x04003D97 RID: 15767
	public Canvas finishCanvas;

	// Token: 0x04003D98 RID: 15768
	public Canvas uiCanvas;

	// Token: 0x04003D99 RID: 15769
	public Camera bossUI;

	// Token: 0x04003D9A RID: 15770
	public List<SefiraBossUI.TextColorSet> colorList;

	// Token: 0x04003D9B RID: 15771
	public AnimationCurve defaultCloseShockWaveCurve;

	// Token: 0x04003D9C RID: 15772
	public AnimationCurve ketherLowerRotationCurve;

	// Token: 0x04003D9D RID: 15773
	public AnimationCurve ketherWhiteGlowCurve;

	// Token: 0x04003D9E RID: 15774
	private const float _enterEffectTime = 1f;

	// Token: 0x04003D9F RID: 15775
	private const float _filterFormulaMin = -1f;

	// Token: 0x04003DA0 RID: 15776
	private const float _filterFormulaMax = 0.8f;

	// Token: 0x04003DA1 RID: 15777
	private UnscaledTimer _enterEffectTimer = new UnscaledTimer();

	// Token: 0x04003DA2 RID: 15778
	public List<SefiraBossUI.TutorialElement> tutorialSpriteSet;

	// Token: 0x04003DA3 RID: 15779
	public ChesedBossUI chesedBossUI;

	// Token: 0x04003DA4 RID: 15780
	public static SefiraBossUI.PositionRatio DefaultRatio = new SefiraBossUI.PositionRatio
	{
		xRatioMin = -0.8f,
		xRatioMax = 0.8f,
		yRatioMin = -0.8f,
		yRatioMax = 0.8f
	};

	// Token: 0x04003DA5 RID: 15781
	public List<SefiraBossUI.PositionRatio> ratio;

	// Token: 0x0200087A RID: 2170
	[Serializable]
	public class PositionRatio
	{
		// Token: 0x06004305 RID: 17157 RVA: 0x0003948E File Offset: 0x0003768E
		public PositionRatio()
		{
		}

		// Token: 0x04003DA6 RID: 15782
		public float xRatioMin = -0.8f;

		// Token: 0x04003DA7 RID: 15783
		public float xRatioMax = 0.8f;

		// Token: 0x04003DA8 RID: 15784
		public float yRatioMin = -0.8f;

		// Token: 0x04003DA9 RID: 15785
		public float yRatioMax = 0.8f;
	}

	// Token: 0x0200087B RID: 2171
	[Serializable]
	public class TutorialElement
	{
		// Token: 0x06004306 RID: 17158 RVA: 0x000394C2 File Offset: 0x000376C2
		public TutorialElement()
		{
		}

		// Token: 0x04003DAA RID: 15786
		public Vector2 pos = Vector2.zero;

		// Token: 0x04003DAB RID: 15787
		public string keyValue = string.Empty;

		// Token: 0x04003DAC RID: 15788
		public Sprite block;

		// Token: 0x04003DAD RID: 15789
		public bool horizontal;
	}

	// Token: 0x0200087C RID: 2172
	[Serializable]
	public class TextColorSet
	{
		// Token: 0x06004307 RID: 17159 RVA: 0x00004378 File Offset: 0x00002578
		public TextColorSet()
		{
		}

		// Token: 0x04003DAE RID: 15790
		public SefiraEnum sefira;

		// Token: 0x04003DAF RID: 15791
		public Color TextColor;

		// Token: 0x04003DB0 RID: 15792
		public Color OutlineColor;
	}
}
