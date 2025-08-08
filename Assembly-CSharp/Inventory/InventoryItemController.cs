using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	// Token: 0x020009D8 RID: 2520
	public class InventoryItemController : MonoBehaviour
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x0003F7FA File Offset: 0x0003D9FA
		public Color NormalEquip
		{
			get
			{
				return DeployUI.instance.UIDefaultFill;
			}
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0003F806 File Offset: 0x0003DA06
		private void Awake()
		{
			this.selectedLevel = -1;
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x001C9548 File Offset: 0x001C7748
		public void Init()
		{ // <Patch>
			if (this.slotDicMod == null)
			{
				this.slotDicMod = new Dictionary<LobotomyBaseMod.LcId, InventorySlot>();
			}
			this._currentDetailMod = new LobotomyBaseMod.LcId(-1);
			Dictionary<LobotomyBaseMod.LcId, List<EquipmentModel>> equipmentListByTypeInfo = InventoryModel.Instance.GetEquipmentListByTypeInfo_Mod();
			if (equipmentListByTypeInfo == null)
			{
				Debug.LogError("inventory is null");
				return;
			}
			List<LobotomyBaseMod.LcId> list = new List<LobotomyBaseMod.LcId>();
			foreach (KeyValuePair<LobotomyBaseMod.LcId, List<EquipmentModel>> keyValuePair in equipmentListByTypeInfo)
			{
				EquipmentTypeInfo data = EquipmentTypeList.instance.GetData_Mod(keyValuePair.Key);
				if (data.type != EquipmentTypeInfo.EquipmentType.SPECIAL)
				{
					if (EquipmentTypeInfo.GetLcId(data) != 2)
					{
						InventorySlot inventorySlot = null;
						if (!this.slotDicMod.TryGetValue(keyValuePair.Key, out inventorySlot))
						{
							if (data.type == EquipmentTypeInfo.EquipmentType.WEAPON)
							{
								GameObject gameObject = Prefab.LoadPrefab("UIComponent/Inventory/EquipmentSlot_Weapon");
								inventorySlot = gameObject.GetComponent<InventoryWeaponSlot>();
								inventorySlot.RectTransform.SetParent(this.WeaponListParent);
							}
							else
							{
								GameObject gameObject = Prefab.LoadPrefab("UIComponent/Inventory/EquipmentSlot_Armor");
								inventorySlot = gameObject.GetComponent<InventoryArmorSlot>();
								inventorySlot.RectTransform.SetParent(this.ArmorListParent);
							}
							inventorySlot.RectTransform.localScale = Vector3.one;
							inventorySlot.SetModel(data, keyValuePair.Value);
							this.slotDicMod.Add(keyValuePair.Key, inventorySlot);
						}
						else if (keyValuePair.Value.Count == 0)
						{
							list.Add(keyValuePair.Key);
						}
						else
						{
							inventorySlot.UpdateList(keyValuePair.Value);
						}
					}
				}
			}
			foreach (LobotomyBaseMod.LcId key in list)
			{
				InventorySlot inventorySlot2 = null;
				if (this.slotDicMod.TryGetValue(key, out inventorySlot2))
				{
					UnityEngine.Object.Destroy(inventorySlot2.gameObject);
				}
				this.slotDicMod.Remove(key);
			}
			this.OnClickButton((int)this._currentWeaponType);
			if (this.selectedLevel == -1)
			{
				this.ClearButtonRankColor();
			}
			else
			{
				this.SetButtonRankColor();
			}
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x001C97A8 File Offset: 0x001C79A8
		public void OnClickButton(int index)
		{
			if (index == 1)
			{
				this.WeaponControl.gameObject.SetActive(false);
				this.ArmorControl.gameObject.SetActive(true);
				this.ArmorButton.interactable = false;
				this.WeaponButton.interactable = true;
				this.WeaponButton.OnPointerExit(null);
				this.ToolTipControl.SetParent(this.ArmorListParent);
				this.ToolTipControl.SetAsLastSibling();
				this.ToolTipControl.gameObject.SetActive(false);
				this._currentDetail = -1;
			}
			else
			{
				this.WeaponControl.gameObject.SetActive(true);
				this.ArmorControl.gameObject.SetActive(false);
				this.ArmorButton.interactable = true;
				this.WeaponButton.interactable = false;
				this.ArmorButton.OnPointerExit(null);
				this.ToolTipControl.SetParent(this.WeaponListParent);
				this.ToolTipControl.SetAsLastSibling();
				this.ToolTipControl.gameObject.SetActive(false);
				this._currentDetail = -1;
			}
			this._currentWeaponType = (InventoryItemType)index;
			this.OnClickSort(this.selectedLevel, true);
			InventoryItemTypeObject inventoryItemTypeObject = null;
			InventoryItemType currentWeaponType = this._currentWeaponType;
			if (currentWeaponType != InventoryItemType.ARMOR)
			{
				if (currentWeaponType == InventoryItemType.WEAPON)
				{
					inventoryItemTypeObject = new InventoryItemTypeObject(this._currentWeaponType);
				}
			}
			else
			{
				inventoryItemTypeObject = new InventoryItemTypeObject(this._currentWeaponType);
			}
			Notice.instance.Send(NoticeName.OnChangeInventoryTap, new object[]
			{
				inventoryItemTypeObject
			});
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x001C9920 File Offset: 0x001C7B20
		public void OnEquipAction(EquipmentModel equipment, AgentModel agent = null)
		{ // <Patch>
			InventorySlot inventorySlot = null;
			if (!this.GetSlot(equipment, out inventorySlot))
			{
				string str = "Couldn't find slot about ";
				LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(equipment.metaInfo);
				Debug.LogError(str + ((lcId != null) ? lcId.ToString() : null));
				return;
			}
			if (agent == null)
			{
				if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
				{
					if (equipment.owner != null)
					{
						equipment.owner.ReleaseArmor();
					}
				}
				else if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON && equipment.owner != null)
				{
					equipment.owner.ReleaseWeaponV2();
				}
				inventorySlot.CheckOwner();
				return;
			}
			if (!equipment.CheckRequire(agent))
			{
				return;
			}
			EquipmentModel equipmentModel = null;
			if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				equipmentModel = agent.Equipment.armor;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseArmor();
				}
				if (equipmentModel != null)
				{
					if (equipmentModel.instanceId == equipment.instanceId)
					{
						agent.ReleaseArmor();
					}
					else
					{
						agent.SetArmor(equipment as ArmorModel);
					}
				}
				else
				{
					agent.SetArmor(equipment as ArmorModel);
				}
			}
			else if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				equipmentModel = agent.Equipment.weapon;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseWeaponV2();
				}
				if (equipmentModel != null)
				{
					if (equipmentModel.instanceId == equipment.instanceId)
					{
						agent.ReleaseWeaponV2();
					}
					else
					{
						agent.SetWeapon(equipment as WeaponModel);
					}
				}
				else
				{
					agent.SetWeapon(equipment as WeaponModel);
				}
			}
			if (equipmentModel != null)
			{
				InventorySlot inventorySlot2 = null;
				if (this.GetSlot(equipmentModel, out inventorySlot2))
				{
					inventorySlot2.CheckOwner();
				}
			}
			inventorySlot.CheckOwner();
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x001C9AD8 File Offset: 0x001C7CD8
		public void OnEquipAction(AgentModel agent, EquipmentModel equipment)
		{
			if (!equipment.CheckRequire(agent))
			{
				return;
			}
			if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				InventoryArmorSlot inventoryArmorSlot = null;
				ArmorModel armor = agent.Equipment.armor;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseArmor();
				}
				if (armor != null)
				{
					if (armor.instanceId == equipment.instanceId)
					{
						agent.ReleaseArmor();
					}
					else
					{
						agent.SetArmor(equipment as ArmorModel);
					}
				}
				else
				{
					agent.SetArmor(equipment as ArmorModel);
				}
				if (armor != null && this.armorDic.TryGetValue(armor.instanceId, out inventoryArmorSlot))
				{
					inventoryArmorSlot.CheckOwner();
				}
				if (this.armorDic.TryGetValue(equipment.instanceId, out inventoryArmorSlot))
				{
					inventoryArmorSlot.CheckOwner();
				}
			}
			else if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				WeaponModel weapon = agent.Equipment.weapon;
				InventoryWeaponSlot inventoryWeaponSlot = null;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseWeaponV2();
				}
				if (weapon != null)
				{
					if (weapon.instanceId == equipment.instanceId)
					{
						agent.ReleaseWeaponV2();
					}
					else
					{
						agent.SetWeapon(equipment as WeaponModel);
					}
				}
				else
				{
					agent.SetWeapon(equipment as WeaponModel);
				}
				if (weapon != null && this.weaponDic.TryGetValue(weapon.instanceId, out inventoryWeaponSlot))
				{
					inventoryWeaponSlot.CheckOwner();
				}
				if (this.weaponDic.TryGetValue(equipment.instanceId, out inventoryWeaponSlot))
				{
					inventoryWeaponSlot.CheckOwner();
				}
			}
			InventoryUI.CurrentWindow.AgentController.SetAgent(agent);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x001C9C74 File Offset: 0x001C7E74
		public void OnEquipAction(EquipmentModel equipment)
		{
			if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				InventoryArmorSlot inventoryArmorSlot = null;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseArmor();
				}
				if (this.armorDic.TryGetValue(equipment.instanceId, out inventoryArmorSlot))
				{
					inventoryArmorSlot.CheckOwner();
				}
			}
			else if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				InventoryWeaponSlot inventoryWeaponSlot = null;
				if (equipment.owner != null)
				{
					equipment.owner.ReleaseWeaponV2();
				}
				if (this.weaponDic.TryGetValue(equipment.instanceId, out inventoryWeaponSlot))
				{
					inventoryWeaponSlot.CheckOwner();
				}
			}
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x001C9D14 File Offset: 0x001C7F14
		public bool GetSlot(EquipmentModel equipment, out InventorySlot slot)
		{ // <Patch>
			LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(equipment.metaInfo);
			return this.slotDicMod.TryGetValue(lcId, out slot);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x001C9D3C File Offset: 0x001C7F3C
		public bool GetSlot(EquipmentTypeInfo info, out InventorySlot slot)
		{ // <Patch>
			LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(info);
			return this.slotDicMod.TryGetValue(lcId, out slot);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x001C9D60 File Offset: 0x001C7F60
		public void OnClickDetailInfo(EquipmentTypeInfo info)
		{ // <Patch>
			InventorySlot inventorySlot = null;
			InventoryUI.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			this.TooltipDesc.text = info.Description;
			this.TooltipPosSet();
			if (!this.GetSlot(info, out inventorySlot))
			{
				return;
			}
			if (this._currentDetailMod == EquipmentTypeInfo.GetLcId(info))
			{
				this.ToolTipControl.gameObject.SetActive(false);
				this._currentDetailMod = new LobotomyBaseMod.LcId(-1);
				inventorySlot.TooltipButton.interactable = true;
				inventorySlot.TooltipButton.OnPointerExit(null);
				return;
			}
			InventorySlot inventorySlot2 = null;
			if (this.slotDicMod.TryGetValue(this._currentDetailMod, out inventorySlot2))
			{
				inventorySlot2.TooltipButton.interactable = true;
				inventorySlot2.TooltipButton.OnPointerExit(null);
			}
			if (!this.ToolTipControl.gameObject.activeInHierarchy)
			{
				this.ToolTipControl.gameObject.SetActive(true);
			}
			this.TooltipTitle_ItemName.text = info.Name;
			string specialDesc = info.SpecialDesc;
			if (info.GetLocalizedText("specialDesc", out specialDesc))
			{
				if (specialDesc == "UNKOWN")
				{
					this.MiddleActive.SetActive(false);
				}
				else
				{
					this.Tooltip_Middle.text = info.SpecialDesc;
					this.MiddleActive.SetActive(true);
				}
			}
			else
			{
				this.MiddleActive.SetActive(false);
			}
			int siblingIndex = this.ToolTipControl.GetSiblingIndex();
			int num = inventorySlot.RectTransform.GetSiblingIndex();
			if (num > siblingIndex)
			{
				num--;
			}
			this.ToolTipControl.SetSiblingIndex(num + 1);
			this._currentDetailMod = EquipmentTypeInfo.GetLcId(info);
			this.ToolTipControl.transform.parent.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			this.ToolTipControl.transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
			this.ToolTipControl.transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x0003F80F File Offset: 0x0003DA0F
		public void CloseTooltip()
		{ // <Patch>
			if (this._currentDetailMod != -1)
			{
				this.OnClickDetailInfo(EquipmentTypeList.instance.GetData_Mod(this._currentDetailMod));
			}
			this._currentDetailMod = new LobotomyBaseMod.LcId(-1);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x001C9F4C File Offset: 0x001C814C
		public void OnClickDetailInfo(EquipmentModel equipment)
		{ // <Patch>
            OnClickDetailInfo(equipment.metaInfo);
            /*
			InventoryUI.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			this.TooltipDesc.text = equipment.metaInfo.Description;
			this.TooltipPosSet();
			if ((long)this._currentDetail == equipment.instanceId)
			{
				this.ToolTipControl.gameObject.SetActive(false);
				this._currentDetail = -1;
				if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
				{
					InventoryArmorSlot inventoryArmorSlot = null;
					if (this.armorDic.TryGetValue(equipment.instanceId, out inventoryArmorSlot))
					{
						inventoryArmorSlot.TooltipButton.interactable = true;
						inventoryArmorSlot.TooltipButton.OnPointerExit(null);
					}
				}
				else
				{
					InventoryWeaponSlot inventoryWeaponSlot = null;
					if (this.weaponDic.TryGetValue(equipment.instanceId, out inventoryWeaponSlot))
					{
						inventoryWeaponSlot.TooltipButton.interactable = true;
						inventoryWeaponSlot.TooltipButton.OnPointerExit(null);
					}
				}
				return;
			}
			InventoryArmorSlot inventoryArmorSlot2 = null;
			if (this.armorDic.TryGetValue((long)this._currentDetail, out inventoryArmorSlot2))
			{
				inventoryArmorSlot2.TooltipButton.interactable = true;
				inventoryArmorSlot2.TooltipButton.OnPointerExit(null);
			}
			InventoryWeaponSlot inventoryWeaponSlot2 = null;
			if (this.weaponDic.TryGetValue((long)this._currentDetail, out inventoryWeaponSlot2))
			{
				inventoryWeaponSlot2.TooltipButton.interactable = true;
				inventoryWeaponSlot2.TooltipButton.OnPointerExit(null);
			}
			if (!this.ToolTipControl.gameObject.activeInHierarchy)
			{
				this.ToolTipControl.gameObject.SetActive(true);
			}
			this.TooltipTitle_ItemName.text = equipment.metaInfo.Name;
			string specialDesc = equipment.metaInfo.SpecialDesc;
			if (equipment.metaInfo.GetLocalizedText("specialDesc", out specialDesc))
			{
				if (specialDesc == "UNKOWN")
				{
					this.MiddleActive.SetActive(false);
				}
				else
				{
					this.Tooltip_Middle.text = equipment.metaInfo.SpecialDesc;
					this.MiddleActive.SetActive(true);
				}
			}
			else
			{
				this.MiddleActive.SetActive(false);
			}
			int siblingIndex = this.ToolTipControl.GetSiblingIndex();
			if (this._currentWeaponType == InventoryItemType.ARMOR)
			{
				InventoryArmorSlot inventoryArmorSlot3 = null;
				if (this.armorDic.TryGetValue(equipment.instanceId, out inventoryArmorSlot3))
				{
					int num = inventoryArmorSlot3.RectTransform.GetSiblingIndex();
					if (num > siblingIndex)
					{
						num--;
					}
					this.ToolTipControl.SetSiblingIndex(num + 1);
				}
			}
			else if (this._currentWeaponType == InventoryItemType.WEAPON)
			{
				InventoryWeaponSlot inventoryWeaponSlot3 = null;
				if (this.weaponDic.TryGetValue(equipment.instanceId, out inventoryWeaponSlot3))
				{
					int num2 = inventoryWeaponSlot3.RectTransform.GetSiblingIndex();
					if (num2 > siblingIndex)
					{
						num2--;
					}
					this.ToolTipControl.SetSiblingIndex(num2 + 1);
				}
			}
			this.ToolTipControl.transform.parent.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            */
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x001CA218 File Offset: 0x001C8418
		public void TooltipPosSet()
		{
			this.tooltipRect.anchoredPosition = new Vector2(this.tooltipRect.anchoredPosition.x, 0f);
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x001CA250 File Offset: 0x001C8450
		public void SetGradeText(int grade, Text target)
		{
			RiskLevel riskLevel = (RiskLevel)grade;
			target.text = riskLevel.ToString();
			target.color = UIColorManager.instance.GetRiskColor((RiskLevel)grade);
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0003F83A File Offset: 0x0003DA3A
		public static void SetGradeText(RiskLevel level, Text target)
		{
			target.text = level.ToString();
			target.color = UIColorManager.instance.GetRiskColor(level);
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x001CA284 File Offset: 0x001C8484
		public void ForcelyRelaseEquipment(EquipmentModel equipment, AgentModel owner)
		{
			if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				owner.ReleaseArmor();
			}
			else if (equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				owner.ReleaseWeaponV2();
			}
			InventorySlot inventorySlot = null;
			if (this.GetSlot(equipment, out inventorySlot))
			{
				inventorySlot.CheckOwner();
			}
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x001CA2DC File Offset: 0x001C84DC
		public void SetList()
		{ // <Patch>
			List<InventorySlot> list = new List<InventorySlot>();
			RiskLevel riskLevel = RiskLevel.ZAYIN;
			if (this.selectedLevel != -1)
			{
				riskLevel = (RiskLevel)this.selectedLevel;
			}
			foreach (InventorySlot inventorySlot in this.slotDicMod.Values)
			{
				if (this._currentWeaponType == InventoryItemType.WEAPON)
				{
					if (inventorySlot.Info.type != EquipmentTypeInfo.EquipmentType.WEAPON)
					{
						inventorySlot.gameObject.SetActive(false);
						continue;
					}
				}
				else if (inventorySlot.Info.type != EquipmentTypeInfo.EquipmentType.ARMOR)
				{
					inventorySlot.gameObject.SetActive(false);
					continue;
				}
				if (this.selectedLevel == -1)
				{
					list.Add(inventorySlot);
					inventorySlot.gameObject.SetActive(true);
				}
				else if (riskLevel == this.GetRiskLevel(inventorySlot))
				{
					list.Add(inventorySlot);
					inventorySlot.gameObject.SetActive(true);
				}
				else
				{
					inventorySlot.gameObject.SetActive(false);
				}
			}
			List<InventorySlot> list2 = list;
			if (InventoryItemController.cache0 == null)
			{
				InventoryItemController.cache0 = new Comparison<InventorySlot>(InventorySlot.SortCompare);
			}
			list2.Sort(InventoryItemController.cache0);
			this.SortList(list);
			this.CurrentDisplayed = list;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0003F860 File Offset: 0x0003DA60
		public void OnClickSort(int i)
		{
			this.OnClickSort(i, false);
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001CA42C File Offset: 0x001C862C
		public void OnClickSort(int i, bool changedType)
		{
			if (i == this.selectedLevel && !changedType)
			{
				this.selectedLevel = -1;
			}
			else
			{
				this.selectedLevel = i;
			}
			if (this.selectedLevel == -1)
			{
				this.ClearButtonRankColor();
			}
			else
			{
				this.SetButtonRankColor();
			}
			this.SetList();
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x001CA484 File Offset: 0x001C8684
		private void ClearButtonRankColor()
		{
			foreach (InventoryRankButton inventoryRankButton in this.rankButton)
			{
				inventoryRankButton.Init();
			}
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x001CA4B8 File Offset: 0x001C86B8
		private void SetButtonRankColor()
		{
			foreach (InventoryRankButton inventoryRankButton in this.rankButton)
			{
				inventoryRankButton.OnChangeButton(this.selectedLevel);
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x001CA4F0 File Offset: 0x001C86F0
		private void SortList(List<InventorySlot> sorted)
		{
			int count = sorted.Count;
			for (int i = 0; i < count; i++)
			{
				InventorySlot inventorySlot = sorted[i];
				inventorySlot.transform.SetAsLastSibling();
			}
			this.CloseTooltip();
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0003F86A File Offset: 0x0003DA6A
		private RiskLevel GetRiskLevel(InventorySlot slot)
		{
			return (RiskLevel)(int.Parse(slot.Info.grade) - 1);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0003F87E File Offset: 0x0003DA7E
		public bool IsSelectedRank()
		{
			return -1 == this.selectedLevel;
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0003F889 File Offset: 0x0003DA89
		public int GetSelectedIndex()
		{
			return this.selectedLevel;
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x001CA530 File Offset: 0x001C8730
		public void CheckAgentContains(AgentModel target, Color c)
		{ // <Patch>
			foreach (InventorySlot inventorySlot in this.slotDicMod.Values)
			{
				int agentSlotIndex = inventorySlot.GetAgentSlotIndex(target);
				if (agentSlotIndex != -1)
				{
					inventorySlot.ownerSlot[agentSlotIndex].SetTextureColor(c);
					break;
				}
			}
		}

		// Token: 0x040046FD RID: 18173
		private const string _armorSlot = "UIComponent/Inventory/EquipmentSlot_Armor";

		// Token: 0x040046FE RID: 18174
		private const string _weaponSlot = "UIComponent/Inventory/EquipmentSlot_Weapon";

		// Token: 0x040046FF RID: 18175
		public Button WeaponButton;

		// Token: 0x04004700 RID: 18176
		public Button ArmorButton;

		// Token: 0x04004701 RID: 18177
		public RectTransform WeaponControl;

		// Token: 0x04004702 RID: 18178
		public RectTransform ArmorControl;

		// Token: 0x04004703 RID: 18179
		public RectTransform WeaponListParent;

		// Token: 0x04004704 RID: 18180
		public RectTransform ArmorListParent;

		// Token: 0x04004705 RID: 18181
		public ScrollRect WeaponScroll;

		// Token: 0x04004706 RID: 18182
		public ScrollRect ArmorScroll;

		// Token: 0x04004707 RID: 18183
		public RectTransform ToolTipControl;

		// Token: 0x04004708 RID: 18184
		public Text TooltipTitle_ItemName;

		// Token: 0x04004709 RID: 18185
		public Text Tooltip_Middle;

		// Token: 0x0400470A RID: 18186
		public GameObject MiddleActive;

		// Token: 0x0400470B RID: 18187
		public Text TooltipDesc;

		// Token: 0x0400470C RID: 18188
		public RectTransform tooltipRect;

		// Token: 0x0400470D RID: 18189
		[Header("GradeColor")]
		public Color[] gradeColor;

		// Token: 0x0400470E RID: 18190
		[Header("SortButton")]
		public Button[] sortButton;

		// Token: 0x0400470F RID: 18191
		public InventoryRankButton[] rankButton;

		// Token: 0x04004710 RID: 18192
		[Header("OtherColor")]
		public Color FailEqiup;

		// Token: 0x04004711 RID: 18193
		private Dictionary<long, InventoryWeaponSlot> weaponDic = new Dictionary<long, InventoryWeaponSlot>();

		// Token: 0x04004712 RID: 18194
		private Dictionary<long, InventoryArmorSlot> armorDic = new Dictionary<long, InventoryArmorSlot>();

		// Token: 0x04004713 RID: 18195
		private Dictionary<int, InventorySlot> slotDic = new Dictionary<int, InventorySlot>();

		// Token: 0x04004714 RID: 18196
		private List<InventorySlot> CurrentDisplayed = new List<InventorySlot>();

		// Token: 0x04004715 RID: 18197
		private InventoryItemType _currentWeaponType;

		// Token: 0x04004716 RID: 18198
		private int _currentDetail = -1;

		// Token: 0x04004717 RID: 18199
		private int selectedLevel = -1;

		// <Patch>
		[NonSerialized]
		private LobotomyBaseMod.LcId _currentDetailMod;

		// <Patch>
		[NonSerialized]
		private Dictionary<LobotomyBaseMod.LcId, InventorySlot> slotDicMod;

		// Token: 0x04004718 RID: 18200
		[CompilerGenerated]
		private static Comparison<InventorySlot> cache0;

		// Token: 0x020009D9 RID: 2521
		private enum SlotState
		{
			// Token: 0x0400471A RID: 18202
			ADDED,
			// Token: 0x0400471B RID: 18203
			REDUCED,
			// Token: 0x0400471C RID: 18204
			BROKEN
		}
	}
}
