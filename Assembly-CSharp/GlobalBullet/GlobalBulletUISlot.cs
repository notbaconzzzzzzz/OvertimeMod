using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalBullet
{
	// Token: 0x020009BB RID: 2491
	public class GlobalBulletUISlot : MonoBehaviour
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x0003F191 File Offset: 0x0003D391
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0003F199 File Offset: 0x0003D399
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x0003F1A1 File Offset: 0x0003D3A1
		public bool IsPointerEntered
		{
			get
			{
				return this._isPointerEntered;
			}
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x0003F1A9 File Offset: 0x0003D3A9
		private void Start()
		{
			this.SetGraphicsColor(false);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x000043CD File Offset: 0x000025CD
		private void Update()
		{
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x0003F1B2 File Offset: 0x0003D3B2
		public void SetAcitve(bool state)
		{
			this._isEnabled = state;
			this.DisbaledBlockObject.SetActive(!state);
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x0003F1CA File Offset: 0x0003D3CA
		public void SetSelected(bool state)
		{
			this._isSelected = state;
			if (state)
			{
				this.SetGraphicsColor(true);
			}
			else if (!this.IsPointerEntered)
			{
				this.SetGraphicsColor(false);
			}
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x001C57E0 File Offset: 0x001C39E0
		public void OnEnterPointer()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			this._isPointerEntered = true;
			if (this.IsSelected)
			{
				return;
			}
			this.SetGraphicsColor(true);
			try
			{
				GlobalBulletWindow.CurrentWindow.GetComponent<AudioClipPlayer>().OnPlayInList(0);
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x0003F1F7 File Offset: 0x0003D3F7
		public void OnExitPointer()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			this._isPointerEntered = false;
			if (this.IsSelected)
			{
				return;
			}
			this.SetGraphicsColor(false);
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x0003F21F File Offset: 0x0003D41F
		public void OnClick()
		{ // <Mod>
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (SlotType == GlobalBulletType.SLOW && ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.STIM))
				{
					GlobalBulletWindow.CurrentWindow.OnSlotSelected(GlobalBulletType.STIM);
					return;
				}
				else if (SlotType == GlobalBulletType.EXCUTE && ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.TRANQ))
				{
					GlobalBulletWindow.CurrentWindow.OnSlotSelected(GlobalBulletType.TRANQ);
					return;
				}
				else if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1))
				{
					GlobalBulletWindow.CurrentWindow.OnSlotSelected(this.SlotType + (int)GlobalBulletType.TRANQ);
					return;
				}
			}
			if (!this.IsEnabled)
			{
				return;
			}
			GlobalBulletWindow.CurrentWindow.OnSlotSelected(this.SlotType);
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x001C5840 File Offset: 0x001C3A40
		public void SetGraphicsColor(Color c)
		{
			foreach (MaskableGraphic maskableGraphic in this.ColoredGraphics)
			{
				maskableGraphic.color = c;
			}
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x001C589C File Offset: 0x001C3A9C
		public void SetGraphicsColor(bool isSelected)
		{
			if (isSelected)
			{
				this.SetGraphicsColor(GlobalBulletWindow.CurrentWindow.CyanColor);
				foreach (MaskableGraphic maskableGraphic in this.ColoredGraphics_Alter)
				{
					maskableGraphic.color = this.AlterColor;
				}
			}
			else
			{
				this.SetGraphicsColor(GlobalBulletWindow.CurrentWindow.OrangeColor);
				foreach (MaskableGraphic maskableGraphic2 in this.ColoredGraphics_Alter)
				{
					maskableGraphic2.color = this.AlterDisabledColor;
				}
			}
		}

		// Token: 0x0400462F RID: 17967
		public List<MaskableGraphic> ColoredGraphics;

		// Token: 0x04004630 RID: 17968
		public List<MaskableGraphic> ColoredGraphics_Alter;

		// Token: 0x04004631 RID: 17969
		public Color AlterColor;

		// Token: 0x04004632 RID: 17970
		public Color AlterDisabledColor;

		// Token: 0x04004633 RID: 17971
		public GameObject DisbaledBlockObject;

		// Token: 0x04004634 RID: 17972
		public GlobalBulletType SlotType;

		// Token: 0x04004635 RID: 17973
		private bool _isEnabled;

		// Token: 0x04004636 RID: 17974
		private bool _isSelected;

		// Token: 0x04004637 RID: 17975
		private bool _isPointerEntered;
	}
}
