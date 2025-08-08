/*
private void Update() // Stop from registering keys while ctrl is pressed
private void UpdateSniping() // New bullet types
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalBullet
{
	// Token: 0x020009BB RID: 2491
	public class GlobalBulletWindow : MonoBehaviour
	{
		// Token: 0x06004B6D RID: 19309 RVA: 0x0003E9CB File Offset: 0x0003CBCB
		public GlobalBulletWindow()
		{
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x0003E9DB File Offset: 0x0003CBDB
		// (set) Token: 0x06004B6F RID: 19311 RVA: 0x0003E9E2 File Offset: 0x0003CBE2
		public static GlobalBulletWindow CurrentWindow
		{
			get
			{
				return GlobalBulletWindow._currentWindow;
			}
			private set
			{
				GlobalBulletWindow._currentWindow = value;
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x001BD5A8 File Offset: 0x001BB7A8
		private void Awake()
		{
			if (GlobalBulletWindow.CurrentWindow != null)
			{
				if (GlobalBulletWindow.CurrentWindow.gameObject != null)
				{
					UnityEngine.Object.Destroy(GlobalBulletWindow.CurrentWindow.gameObject);
				}
				GlobalBulletWindow.CurrentWindow = null;
			}
			GlobalBulletWindow._currentWindow = this;
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06004B71 RID: 19313 RVA: 0x0003E9EA File Offset: 0x0003CBEA
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06004B72 RID: 19314 RVA: 0x0003E9F2 File Offset: 0x0003CBF2
		public GlobalBulletType CurrentSelectedBullet
		{
			get
			{
				return this._currentSelectedBullet;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06004B73 RID: 19315 RVA: 0x0003E9FA File Offset: 0x0003CBFA
		// (set) Token: 0x06004B74 RID: 19316 RVA: 0x0003EA02 File Offset: 0x0003CC02
		public int CurrentBulletCount
		{
			get
			{
				return this._currentBulletCount;
			}
			private set
			{
				this._currentBulletCount = value;
				this.CurrentUsableBulletCount.text = this._currentBulletCount.ToString();
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06004B75 RID: 19317 RVA: 0x0003EA27 File Offset: 0x0003CC27
		// (set) Token: 0x06004B76 RID: 19318 RVA: 0x0003EA2F File Offset: 0x0003CC2F
		public int CurrentBulletMax
		{
			get
			{
				return this._bulletCountMax;
			}
			set
			{
				this._bulletCountMax = value;
				this.BulletCountMax.text = this._bulletCountMax.ToString();
			}
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0003EA54 File Offset: 0x0003CC54
		private void Start()
		{
			this.Init();
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x0003EA5C File Offset: 0x0003CC5C
		public void Init()
		{
			this.ActiveControl.SetActive(false);
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x0003EA6A File Offset: 0x0003CC6A
		public void SetActive(bool b)
		{
			this.ActiveControl.SetActive(b);
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x001BD5F8 File Offset: 0x001BB7F8
		public void SetSlotActive(GlobalBulletType bulletType, bool b)
		{
			GlobalBulletUISlot globalBulletUISlot = this.slots[bulletType - GlobalBulletType.RECOVER_HP];
			globalBulletUISlot.SetAcitve(b);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x001BD61C File Offset: 0x001BB81C
		public void OnSlotSelected(GlobalBulletType index)
		{
			GlobalBulletUISlot globalBulletUISlot = null;
			UnitMouseEventManager.instance.CancelDrag();
			if (index != GlobalBulletType.NONE)
			{
				globalBulletUISlot = this.slots[index - GlobalBulletType.RECOVER_HP];
			}
			if (globalBulletUISlot != null && !globalBulletUISlot.IsEnabled)
			{
				return;
			}
			if (this.CurrentSelectedBullet == index)
			{
				if (this.CurrentSelectedBullet != GlobalBulletType.NONE)
				{
					this.slots[this.CurrentSelectedBullet - GlobalBulletType.RECOVER_HP].SetSelected(false);
				}
				this._currentSelectedBullet = GlobalBulletType.NONE;
			}
			else
			{
				if (this.CurrentSelectedBullet != GlobalBulletType.NONE)
				{
					this.slots[this.CurrentSelectedBullet - GlobalBulletType.RECOVER_HP].SetSelected(false);
				}
				this._currentSelectedBullet = index;
				if (globalBulletUISlot != null)
				{
					globalBulletUISlot.SetSelected(true);
				}
			}
			try
			{
				GlobalBulletWindow.CurrentWindow.GetComponent<AudioClipPlayer>().OnPlayInList(1);
			}
			catch (Exception ex)
			{
			}
			this.UpdatePointer();
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x001BD710 File Offset: 0x001BB910
		private void UpdatePointer()
		{
			if (this.CurrentSelectedBullet == GlobalBulletType.NONE)
			{
				CursorManager.instance.ForcelyCurserSet(MouseCursorType.NORMAL);
			}
			else
			{
				MouseCursorType type;
				if (this.CurrentSelectedBullet == GlobalBulletType.EXCUTE)
				{
					type = MouseCursorType.BULLET_EXECUTION;
				}
				else
				{
					type = MouseCursorType.BULLET_EXECUTION + (int)this.CurrentSelectedBullet;
				}
				CursorManager.instance.CursorSet(type);
			}
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0003EA78 File Offset: 0x0003CC78
		public void SetBulletFillGauge(float value)
		{
			if (value > 1f)
			{
				value = 1f;
			}
			if (value < 0f)
			{
				value = 0f;
			}
			this.CurrentBulletFillGauge.fillAmount = value;
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x001BD764 File Offset: 0x001BB964
		public void SetBulletCount(int count)
		{
			if (count < 0)
			{
				count = 0;
			}
			if (count > this._bulletCountMax)
			{
				count = this._bulletCountMax;
			}
			this.CurrentBulletCount = count;
			if (count == 0)
			{
				this.CurrentUsableBulletCount.color = this.RedColor;
			}
			else
			{
				this.CurrentUsableBulletCount.color = this.CyanColor;
			}
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x0003EAAA File Offset: 0x0003CCAA
		public void SetBulletCountMax(int maxCount)
		{
			if (maxCount < 1)
			{
				maxCount = 1;
			}
			this.CurrentBulletMax = maxCount;
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001BD7C4 File Offset: 0x001BB9C4
		private void Update()
		{ // <Mod>
			if (GameManager.currentGameManager.state != GameState.STOP && ConsoleScript.instance != null && !ConsoleScript.instance.ConsoleWnd.activeInHierarchy && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.OnSlotSelected(GlobalBulletType.RECOVER_HP);
				}
				if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.OnSlotSelected(GlobalBulletType.RECOVER_MENTAL);
				}
				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.OnSlotSelected(GlobalBulletType.RESIST_R);
				}
				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.OnSlotSelected(GlobalBulletType.RESIST_W);
				}
				if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.OnSlotSelected(GlobalBulletType.RESIST_B);
				}
				if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					this.OnSlotSelected(GlobalBulletType.RESIST_P);
				}
				if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					this.OnSlotSelected(GlobalBulletType.SLOW);
				}
				if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					this.OnSlotSelected(GlobalBulletType.EXCUTE);
				}
			}
			if (this._currentSelectedBullet != GlobalBulletType.NONE)
			{
				this.UpdateSniping();
			}
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x001BD894 File Offset: 0x001BBA94
		private void UpdateSniping()
		{ // <Mod>
			bool flag = true;
			bool flag2 = false;
			GlobalBulletType currentSelectedBullet = this._currentSelectedBullet;
			if (currentSelectedBullet == GlobalBulletType.SLOW)
			{
				flag = false;
				flag2 = true;
			}
			else if (currentSelectedBullet == GlobalBulletType.STIM)
			{
				if (false)
				{
					flag2 = true;
				}
			}
			else if (currentSelectedBullet == GlobalBulletType.TRANQ)
			{
				flag = false;
			}
			List<UnitModel> list = new List<UnitModel>();
			Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D[] array = Physics2D.OverlapPointAll(vector);
			if (UnitMouseEventManager.instance.isPointerEntered)
			{
				foreach (Collider2D collider2D in array)
				{
					UnitMouseEventTarget component = collider2D.GetComponent<UnitMouseEventTarget>();
					if (!(component == null))
					{
						if (flag && component.GetCommandTargetModel() is WorkerModel)
						{
							list.Add(component.GetCommandTargetModel() as WorkerModel);
						}
						if (flag2)
						{
							if (component.GetCommandTargetModel() is CreatureModel)
							{
								list.Add(component.GetCommandTargetModel() as CreatureModel);
							}
							else
							{
								WorkerModel workerModel = component.GetCommandTargetModel() as WorkerModel;
								if (workerModel != null && workerModel.IsPanic())
								{
									list.Add(workerModel);
								}
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				CursorManager.instance.OnEnteredTarget();
			}
			else
			{
				CursorManager.instance.OnExitTarget();
			}
			if (Input.GetMouseButtonDown(0) && UnitMouseEventManager.instance.isPointerEntered && GlobalBulletManager.instance.ActivateBullet(this._currentSelectedBullet, list))
			{
				GlobalBulletEffect.GenEffect(this._currentSelectedBullet, vector);
				this.OnShoot();
			}
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x0003EABD File Offset: 0x0003CCBD
		public void OnShoot()
		{
			this.OnSlotSelected(GlobalBulletType.NONE);
			this.UpdatePointer();
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x0000431D File Offset: 0x0000251D
		static GlobalBulletWindow()
		{
		}

		// Token: 0x040045E0 RID: 17888
		private static GlobalBulletWindow _currentWindow;

		// Token: 0x040045E1 RID: 17889
		public bool isolateEntered;

		// Token: 0x040045E2 RID: 17890
		private bool _isEnabled;

		// Token: 0x040045E3 RID: 17891
		private GlobalBulletType _currentSelectedBullet;

		// Token: 0x040045E4 RID: 17892
		public GameObject ActiveControl;

		// Token: 0x040045E5 RID: 17893
		[Header("Color")]
		public Color OrangeColor;

		// Token: 0x040045E6 RID: 17894
		public Color CyanColor;

		// Token: 0x040045E7 RID: 17895
		public Color RedColor;

		// Token: 0x040045E8 RID: 17896
		[Header("Slots")]
		public List<GlobalBulletUISlot> slots;

		// Token: 0x040045E9 RID: 17897
		[Header("BulletState")]
		public Image CurrentBulletFillGauge;

		// Token: 0x040045EA RID: 17898
		public Text CurrentUsableBulletCount;

		// Token: 0x040045EB RID: 17899
		public Text BulletCountMax;

		// Token: 0x040045EC RID: 17900
		private int _currentBulletCount;

		// Token: 0x040045ED RID: 17901
		private int _bulletCountMax = 15;
	}
}
