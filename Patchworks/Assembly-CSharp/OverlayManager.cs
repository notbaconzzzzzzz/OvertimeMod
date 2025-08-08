using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC7 RID: 2759
public class OverlayManager : MonoBehaviour
{
	// Token: 0x060052B7 RID: 21175 RVA: 0x001DE284 File Offset: 0x001DC484
	public OverlayManager()
	{
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x060052B8 RID: 21176 RVA: 0x00043816 File Offset: 0x00041A16
	public static OverlayManager Instance
	{
		get
		{
			return OverlayManager._instance;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x060052B9 RID: 21177 RVA: 0x0004381D File Offset: 0x00041A1D
	// (set) Token: 0x060052BA RID: 21178 RVA: 0x0004382E File Offset: 0x00041A2E
	public bool IsEnabled
	{
		get
		{
			return OptionUI.Instance.Opt_Tooltip.isOn;
		}
		set
		{
			this._isEnabled = value;
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x060052BB RID: 21179 RVA: 0x00043837 File Offset: 0x00041A37
	private int Width
	{
		get
		{
			return Screen.width;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x060052BC RID: 21180 RVA: 0x0004383E File Offset: 0x00041A3E
	private int Height
	{
		get
		{
			return Screen.height;
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x060052BD RID: 21181 RVA: 0x00043845 File Offset: 0x00041A45
	public bool IsActivated
	{
		get
		{
			return this.Pivot.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x001DE2E0 File Offset: 0x001DC4E0
	private void Awake()
	{
		if (OverlayManager.Instance != null)
		{
			UnityEngine.Object.Destroy(OverlayManager.Instance.gameObject);
			OverlayManager._instance = null;
		}
		OverlayManager._instance = this;
		this._textRect = this.OverlayText.rectTransform;
		this._textRect.anchoredPosition = new Vector2(this.HorizontalSpace, -this.VerticalSpace);
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x00043857 File Offset: 0x00041A57
	private void Start()
	{
		this.Pivot.gameObject.SetActive(false);
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x000043A5 File Offset: 0x000025A5
	private void OnEnable()
	{
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x0004386A File Offset: 0x00041A6A
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Home))
		{
		}
		if (this._sizeUpdate)
		{
			this._sizeUpdate = false;
		}
		if (this.IsActivated)
		{
			this.SetPosition(false);
		}
	}

	// Token: 0x060052C2 RID: 21186 RVA: 0x001DE348 File Offset: 0x001DC548
	public void SetText(string text)
	{
		if (!this.IsEnabled)
		{
			return;
		}
		if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1) || SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
		{
			return;
		}
		this.Pivot.gameObject.SetActive(true);
		this.OverlayText.text = text;
		float preferredWidth = this.OverlayText.preferredWidth;
		float preferredHeight = this.OverlayText.preferredHeight;
		float num = Mathf.Min(preferredWidth, this.MaxWidth * 2f);
		num += 10f;
		this.OverlayBox.sizeDelta = new Vector2(num * 0.5f + this.VerticalSpace * 2f, preferredHeight * 0.5f + this.VerticalSpace * 2f);
		this.SetPosition(true);
		this._sizeUpdate = true;
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x00043857 File Offset: 0x00041A57
	public void ClearOverlay()
	{
		this.Pivot.gameObject.SetActive(false);
	}

	// Token: 0x060052C4 RID: 21188 RVA: 0x001DE418 File Offset: 0x001DC618
	public void SetPosition(Camera camera, bool initial = false)
	{
		Vector3 vector = Vector3.zero;
		if (initial)
		{
			vector = (this._enableInitial = Input.mousePosition);
		}
		else
		{
			vector = this._enableInitial;
		}
		Vector2 v = new Vector3(vector.x * this.canvasScale.x / (float)this.Width, vector.y * this.canvasScale.y / (float)this.Height);
		Vector2 anchoredPosition = v + this.MouseRelative;
		float num = anchoredPosition.x + this.OverlayBox.sizeDelta.x;
		float num2 = anchoredPosition.y - this.OverlayBox.sizeDelta.y;
		if (num >= this.canvasScale.x)
		{
			anchoredPosition.x = this.canvasScale.x - this.OverlayBox.sizeDelta.x;
		}
		if (num2 <= 0f)
		{
			anchoredPosition.y += this.OverlayBox.sizeDelta.y;
		}
		this.Pivot.anchoredPosition = anchoredPosition;
	}

	// Token: 0x060052C5 RID: 21189 RVA: 0x0004389F File Offset: 0x00041A9F
	public void SetPosition(bool initial = false)
	{
		this.SetPosition(UIActivateManager.instance.UICamera, initial);
	}

	// Token: 0x060052C6 RID: 21190 RVA: 0x001DE548 File Offset: 0x001DC748
	public void UpdateSize()
	{
		float preferredWidth = this.OverlayText.preferredWidth;
		float preferredHeight = this.OverlayText.preferredHeight;
		float num = Mathf.Min(preferredWidth, this.MaxWidth * 2f);
		this._textRect.sizeDelta = new Vector2(num, preferredHeight);
		this.OverlayBox.sizeDelta = new Vector2(num * 0.5f + this.VerticalSpace * 2f, preferredHeight * 0.5f + this.VerticalSpace * 2f);
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x000043A5 File Offset: 0x000025A5
	public void ReadState()
	{
	}

	// Token: 0x060052C8 RID: 21192 RVA: 0x000043A5 File Offset: 0x000025A5
	public void SaveState()
	{
	}

	// Token: 0x060052C9 RID: 21193 RVA: 0x000043A5 File Offset: 0x000025A5
	// Note: this type is marked as 'beforefieldinit'.
	static OverlayManager()
	{
	}

	// Token: 0x04004C51 RID: 19537
	private static OverlayManager _instance;

	// Token: 0x04004C52 RID: 19538
	private const float scaleFactor = 2f;

	// Token: 0x04004C53 RID: 19539
	private const float scaleFactorInv = 0.5f;

	// Token: 0x04004C54 RID: 19540
	[Header("UI")]
	public Canvas _canvas;

	// Token: 0x04004C55 RID: 19541
	public RectTransform Pivot;

	// Token: 0x04004C56 RID: 19542
	public RectTransform OverlayBox;

	// Token: 0x04004C57 RID: 19543
	public Text OverlayText;

	// Token: 0x04004C58 RID: 19544
	public Vector2 canvasScale;

	// Token: 0x04004C59 RID: 19545
	private RectTransform _textRect;

	// Token: 0x04004C5A RID: 19546
	public Text _debugText;

	// Token: 0x04004C5B RID: 19547
	[Header("Values")]
	public float MaxWidth = 300f;

	// Token: 0x04004C5C RID: 19548
	public float VerticalSpace = 20f;

	// Token: 0x04004C5D RID: 19549
	public float HorizontalSpace = 20f;

	// Token: 0x04004C5E RID: 19550
	public float EnableDelay = 0.7f;

	// Token: 0x04004C5F RID: 19551
	public Vector2 MouseRelative;

	// Token: 0x04004C60 RID: 19552
	private bool _isEnabled = true;

	// Token: 0x04004C61 RID: 19553
	private bool _sizeUpdate;

	// Token: 0x04004C62 RID: 19554
	private Vector3 _enableInitial = Vector3.zero;

	// Token: 0x04004C63 RID: 19555
	public string prev = string.Empty;
}
