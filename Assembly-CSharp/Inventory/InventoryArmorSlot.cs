using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	// Token: 0x020009C9 RID: 2505
	public class InventoryArmorSlot : InventorySlot
	{
		// Token: 0x06004BD2 RID: 19410 RVA: 0x0003E8A8 File Offset: 0x0003CAA8
		public InventoryArmorSlot()
		{
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x0003E8BB File Offset: 0x0003CABB
		public ArmorModel Armor
		{
			get
			{
				return this._armor;
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x001BF434 File Offset: 0x001BD634
		public override void UpdateUI()
		{
			base.UpdateUI();
			UIUtil.DefenseSetOnlyText(base.Info.defenseInfo, this.DefenseInfo);
			UIUtil.DefenseSetFactor(base.Info.defenseInfo, this.DefenseFactor, true);
			this.TooltipButton.interactable = true;
			this.TooltipButton.OnPointerExit(null);
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0003E8C3 File Offset: 0x0003CAC3
		public override void ApplyPortrait()
		{
			this.portrait.SetArmor(base.Info);
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x001BF48C File Offset: 0x001BD68C
		public void SetArmor(ArmorModel armor)
		{
			if (armor == null)
			{
				Debug.Log("Armor is null");
				return;
			}
			EquipmentTypeInfo metaInfo = armor.metaInfo;
			this.Name.text = metaInfo.Name;
			InventoryItemController.SetGradeText(metaInfo.Grade, this.Grade);
			UIUtil.DefenseSetOnlyText(base.Info.defenseInfo, this.DefenseInfo);
			UIUtil.DefenseSetFactor(base.Info.defenseInfo, this.DefenseFactor, true);
			this.TooltipButton.interactable = true;
			this.SetEquipmentText();
			this.portrait.SetArmor(metaInfo);
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x0003E8D6 File Offset: 0x0003CAD6
		public void OnClickToolTip()
		{
			this.OnDisabledClick();
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0003E8DE File Offset: 0x0003CADE
		public void OnClickEquipment()
		{
			this.SetEquipmentText();
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00003FDD File Offset: 0x000021DD
		public void SetEquipmentText()
		{
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnCheckOwner()
		{
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0003E8E6 File Offset: 0x0003CAE6
		private void Start()
		{
			this.InitScroll();
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0003E8EE File Offset: 0x0003CAEE
		public void InitScroll()
		{
			this.CheckTransformRaycast(base.transform);
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x001BF520 File Offset: 0x001BD720
		private void CheckTransformRaycast(Transform t)
		{
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					MaskableGraphic component = transform.GetComponent<MaskableGraphic>();
					if (component != null && component.raycastTarget)
					{
						ScrollExchanger scrollExchanger = transform.gameObject.AddComponent<ScrollExchanger>();
						scrollExchanger.scrollRect = InventoryUI.CurrentWindow.ItemController.ArmorScroll;
					}
					this.CheckTransformRaycast(transform);
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
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnEnterEquip()
		{
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnExitEquip()
		{
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001BF5C4 File Offset: 0x001BD7C4
		public void OnDisabledClick()
		{
			if (!this.TooltipButton.interactable)
			{
				this.TooltipButton.interactable = true;
				InventoryUI.CurrentWindow.ItemController.OnClickDetailInfo(base.Info);
			}
			else
			{
				this.TooltipButton.interactable = false;
				InventoryUI.CurrentWindow.ItemController.OnClickDetailInfo(base.Info);
			}
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0003E8FC File Offset: 0x0003CAFC
		private void OnEnable()
		{
			this.TooltipButton.interactable = true;
			this.TooltipButton.OnPointerExit(null);
		}

		// Token: 0x04004651 RID: 18001
		private ArmorModel _armor;

		// Token: 0x04004652 RID: 18002
		public Text[] DefenseInfo;

		// Token: 0x04004653 RID: 18003
		public Text[] DefenseFactor;

		// Token: 0x04004654 RID: 18004
		public WorkerPortraitSetter portrait;

		// Token: 0x04004655 RID: 18005
		private string oldText = string.Empty;
	}
}
