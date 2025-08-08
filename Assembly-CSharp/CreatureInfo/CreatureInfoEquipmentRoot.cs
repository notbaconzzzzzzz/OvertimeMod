using System;
using UnityEngine;

namespace CreatureInfo
{
	// Token: 0x02000AD3 RID: 2771
	public class CreatureInfoEquipmentRoot : CreatureInfoController
	{
		// Token: 0x060053AD RID: 21421 RVA: 0x000441B1 File Offset: 0x000423B1
		public int GetObserveLevel()
		{
			return base.ObserveInfo.GetObservationLevel();
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001E7258 File Offset: 0x001E5458
		public override void Initialize(CreatureModel creature)
		{
			base.Initialize(creature);
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR);
				this.armorSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.armorSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
				int costAfterUpgrade = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
				this.armorSlot.Cost = costAfterUpgrade;
				this.armorSlot.MakeArmorTooltip.SetDynamicTooltip(string.Format(LocalizeTextDataModel.instance.GetText(this.armorSlot.MakeArmorTooltip.ID), costAfterUpgrade));
				this.armorSlot.SetEquipInfo(creatureEquipmentMakeInfo);
				this.armorSlot.currentCreature = base.CurrentModel;
				if (CreatureInfoWindow.CurrentWindow.IsCodex)
				{
					this.armorSlot.BuildButton.gameObject.SetActive(false);
				}
				else
				{
					this.armorSlot.BuildButton.gameObject.SetActive(true);
				}
			}
			catch (Exception)
			{
				this.armorSlot.CheckBlocked(false, 4);
				this.armorSlot.NoData("Inventory_NoArmor");
			}
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON);
				this.weaponSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.weaponSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
				int costAfterUpgrade2 = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
				this.weaponSlot.Cost = costAfterUpgrade2;
				this.weaponSlot.MakeWeaponTooltip.SetDynamicTooltip(string.Format(LocalizeTextDataModel.instance.GetText(this.weaponSlot.MakeWeaponTooltip.ID), costAfterUpgrade2));
				this.weaponSlot.SetEquipInfo(creatureEquipmentMakeInfo);
				this.weaponSlot.currentCreature = base.CurrentModel;
				if (CreatureInfoWindow.CurrentWindow.IsCodex)
				{
					this.weaponSlot.BuildButton.gameObject.SetActive(false);
				}
				else
				{
					this.weaponSlot.BuildButton.gameObject.SetActive(true);
				}
			}
			catch (Exception)
			{
				this.weaponSlot.CheckBlocked(false, 4);
				this.weaponSlot.NoData("Inventory_NoWeapon");
			}
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.CurrentModel.metaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL);
				this.giftSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.giftSlot.SetProb(creatureEquipmentMakeInfo.GetProb());
				this.giftSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
			}
			catch (Exception)
			{
				this.giftSlot.CheckBlocked(false, 4);
				this.giftSlot.NoData("Inventory_NoGift");
				this.giftSlot.SetEmpty();
			}
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x001E75C0 File Offset: 0x001E57C0
		public override void Initialize()
		{
			base.Initialize();
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR);
				this.armorSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.armorSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
				int costAfterUpgrade = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
				this.armorSlot.Cost = costAfterUpgrade;
				this.armorSlot.MakeArmorTooltip.SetDynamicTooltip(string.Format(LocalizeTextDataModel.instance.GetText(this.armorSlot.MakeArmorTooltip.ID), costAfterUpgrade));
				this.armorSlot.SetEquipInfo(creatureEquipmentMakeInfo);
				this.armorSlot.currentCreature = base.CurrentModel;
				if (CreatureInfoWindow.CurrentWindow.IsCodex)
				{
					this.armorSlot.BuildButton.gameObject.SetActive(false);
				}
				else
				{
					this.armorSlot.BuildButton.gameObject.SetActive(true);
				}
			}
			catch (Exception)
			{
				this.armorSlot.CheckBlocked(false, 4);
				this.armorSlot.NoData("Inventory_NoArmor");
			}
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON);
				this.weaponSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.weaponSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
				int costAfterUpgrade2 = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
				this.weaponSlot.Cost = costAfterUpgrade2;
				this.weaponSlot.MakeWeaponTooltip.SetDynamicTooltip(string.Format(LocalizeTextDataModel.instance.GetText(this.weaponSlot.MakeWeaponTooltip.ID), costAfterUpgrade2));
				this.weaponSlot.SetEquipInfo(creatureEquipmentMakeInfo);
				this.weaponSlot.currentCreature = base.CurrentModel;
				if (CreatureInfoWindow.CurrentWindow.IsCodex)
				{
					this.weaponSlot.BuildButton.gameObject.SetActive(false);
				}
				else
				{
					this.weaponSlot.BuildButton.gameObject.SetActive(true);
				}
			}
			catch (Exception)
			{
				this.weaponSlot.CheckBlocked(false, 4);
				this.weaponSlot.NoData("Inventory_NoWeapon");
			}
			try
			{
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL);
				this.giftSlot.SetModel(creatureEquipmentMakeInfo.equipTypeInfo);
				this.giftSlot.SetProb(creatureEquipmentMakeInfo.GetProb());
				this.giftSlot.CheckBlocked(creatureEquipmentMakeInfo.level <= this.GetObserveLevel(), creatureEquipmentMakeInfo.level);
			}
			catch (Exception)
			{
				this.giftSlot.CheckBlocked(false, 4);
				this.giftSlot.NoData("Inventory_NoGift");
				this.giftSlot.SetEmpty();
			}
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x000441BE File Offset: 0x000423BE
		public override void OnPurchase()
		{
			this._isOpened = true;
			this.SetEnabled();
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x000044D7 File Offset: 0x000026D7
		public override bool OnClick()
		{
			return false;
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x000043CD File Offset: 0x000025CD
		private void SetEnabled()
		{
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x000043CD File Offset: 0x000025CD
		private void SetDisabled()
		{
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x001E7924 File Offset: 0x001E5B24
		public void OnClickArmor()
		{
			CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR);
			int costAfterUpgrade = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
			if (creatureEquipmentMakeInfo.level > this.GetObserveLevel())
			{
				Debug.Log("cannot make 1");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			if (costAfterUpgrade > base.ObserveInfo.cubeNum)
			{
				this.armorSlot.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_NoCost");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			if (!InventoryModel.Instance.CheckEquipmentCount(creatureEquipmentMakeInfo.equipTypeInfo.id))
			{
				this.armorSlot.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_NoRemain");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			CreatureInfoWindow.CurrentWindow.PurchaseAnim(costAfterUpgrade);
			EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(creatureEquipmentMakeInfo.equipTypeInfo.id);
			if (equipmentModel != null && base.CurrentModel != null)
			{
				base.ObserveInfo.Transaction(-costAfterUpgrade);
			}
			CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(2);
			this.Initialize();
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x001E7A74 File Offset: 0x001E5C74
		public void OnClickWeapon()
		{
			CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = base.MetaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON);
			int costAfterUpgrade = creatureEquipmentMakeInfo.GetCostAfterUpgrade();
			if (creatureEquipmentMakeInfo.level > this.GetObserveLevel())
			{
				Debug.Log("cannot make 1");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			if (costAfterUpgrade > base.ObserveInfo.cubeNum)
			{
				this.weaponSlot.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_NoCost");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			if (!InventoryModel.Instance.CheckEquipmentCount(creatureEquipmentMakeInfo.equipTypeInfo.id))
			{
				this.weaponSlot.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_NoRemain");
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(0);
				return;
			}
			CreatureInfoWindow.CurrentWindow.PurchaseAnim(costAfterUpgrade);
			EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(creatureEquipmentMakeInfo.equipTypeInfo.id);
			if (equipmentModel != null)
			{
				base.ObserveInfo.Transaction(-costAfterUpgrade);
			}
			CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(2);
			this.Initialize();
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x000441CD File Offset: 0x000423CD
		public void OnEnterWeapon()
		{
			this.weaponSlot.OnEnter();
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x000441DA File Offset: 0x000423DA
		public void OnExitWeapon()
		{
			this.weaponSlot.OnExit();
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x000441E7 File Offset: 0x000423E7
		public void OnEnterArmor()
		{
			this.armorSlot.OnEnter();
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x000441F4 File Offset: 0x000423F4
		public void OnExitArmor()
		{
			this.armorSlot.OnExit();
		}

		// Token: 0x04004D0F RID: 19727
		public GiftSlot giftSlot;

		// Token: 0x04004D10 RID: 19728
		public WeaponSlot weaponSlot;

		// Token: 0x04004D11 RID: 19729
		public ArmorSlot armorSlot;
	}
}
