/*
public override void UpdateUI() // 
public void SetWeapon(WeaponModel weapon) // 
*/
using System;
using System.Collections;
using Assets.Scripts.UI.Utils;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;

namespace Inventory
{
	// Token: 0x020009D7 RID: 2519
	public class InventoryWeaponSlot : InventorySlot
	{
		// Token: 0x06004C47 RID: 19527 RVA: 0x0003ECDA File Offset: 0x0003CEDA
		public InventoryWeaponSlot()
		{
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x001C1968 File Offset: 0x001BFB68
		public override void UpdateUI()
		{ // <Patch> <Mod>
			base.UpdateUI();
			float dmgFactor = 1f + EGOrealizationManager.instance.WeaponUpgrade(base.Info);
			string text = string.Format("{0}-{1}", (int)(base.Info.damageInfo.min * dmgFactor), (int)(base.Info.damageInfo.max * dmgFactor));
			RwbpType type = base.Info.damageInfo.type;
			Color color = Color.white;
			color = UIColorManager.instance.GetRWBPTypeColor(type);
			LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(this.Info);
			if (lcId == 200038 || lcId == 200004)
			{
				this.Type.text = "???";
				this.Type.color = Color.grey;
				this.DamageTypeImage.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
				this.DamageTypeImage.enabled = false;
				this.DamageTypeImage.color = Color.white;
			}
			else
			{
				this.Type.text = EnumTextConverter.GetRwbpType(type).ToUpper();
				this.Type.color = color;
				this.DamageTypeImage.enabled = true;
				this.DamageTypeImage.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
				this.DamageTypeImage.color = Color.white;
			}
			this.DamageRange.text = text;
			string empty = string.Empty;
			string empty2 = string.Empty;
			InventoryItemDescGetter.GetWeaponDesc(base.Info, out empty, out empty2);
			this.Range.text = empty2;
			this.AttackSpeed.text = empty;
			this.TooltipButton.interactable = true;
			this.TooltipButton.OnPointerExit(null);
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x001C1B00 File Offset: 0x001BFD00
		public override void ApplyPortrait()
		{ // <Patch> <Mod>
			Sprite sprite;
			if (base.Info.weaponClassType == WeaponClassType.FIST)
			{
				if (EquipmentTypeInfo.GetLcId(this.Info).packageId != "")
				{
					LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(this.Info).packageId, this.Info.sprite);
					Sprite[] fistSprite = WorkerSpriteManager.instance.GetFistSprite(ss);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
					sprite = fistSprite[1];
				}
				else
				{
					int id = (int)float.Parse(base.Info.sprite);
					Sprite[] fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite(id);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
					sprite = fistSprite[1];
				}
			}
			else
			{
				LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(this.Info).packageId, this.Info.sprite);
				sprite = WorkerSpriteManager.instance.GetWeaponSprite_Mod(this.Info.weaponClassType, ss);
			}
			this.Icon.sprite = sprite;
			this.Icon.preserveAspect = true;
			this.Icon.SetNativeSize();
			if (sprite == null)
			{
				this.Icon.gameObject.SetActive(false);
			}
			else
			{
				this.Icon.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x001C1BDC File Offset: 0x001BFDDC
		public void SetWeapon(WeaponModel weapon)
		{ // <Patch> <Mod>
			UnitModel owner = weapon.owner;
			this.Name.text = weapon.metaInfo.Name;
			float dmgFactor = 1f + EGOrealizationManager.instance.WeaponUpgrade(weapon.metaInfo);
			string text = (int)(weapon.GetDamage(owner).min * dmgFactor) + "-" + (int)(weapon.GetDamage(owner).max * dmgFactor);
			RwbpType type = weapon.GetDamage(owner).type;
			Color color;
			Color color2;
			UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
			this.Type.text = type.ToString();
			this.Type.color = color;
			string empty = string.Empty;
			string empty2 = string.Empty;
			InventoryItemDescGetter.GetWeaponDesc(weapon, out empty2, out empty);
			this.Range.text = empty2;
			this.AttackSpeed.text = empty;
			this.DamageRange.text = text;
			InventoryItemController.SetGradeText(weapon.metaInfo.Grade, this.Grade);
			this.Grade.text = weapon.metaInfo.Grade.ToString();
			this.SetEquipmentText();
			this.TooltipButton.interactable = true;
			Sprite sprite;
			if (weapon.metaInfo.weaponClassType == WeaponClassType.FIST)
			{
				if (EquipmentTypeInfo.GetLcId(weapon.metaInfo).packageId != "")
				{
					LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(weapon.metaInfo).packageId, weapon.metaInfo.sprite);
					Sprite[] fistSprite = WorkerSpriteManager.instance.GetFistSprite(ss);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
					sprite = fistSprite[1];
				}
				else
				{
					int id = (int)float.Parse(weapon.metaInfo.sprite);
					Sprite[] fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite(id);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
					sprite = fistSprite[1];
				}
			}
			else
			{
				LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(weapon.metaInfo).packageId, weapon.metaInfo.sprite);
				sprite = WorkerSpriteManager.instance.GetWeaponSprite_Mod(weapon.metaInfo.weaponClassType, ss);
			}
			Debug.Log("Weapon sprite " + sprite);
			this.Icon.sprite = sprite;
			this.Icon.SetNativeSize();
			this.Icon.preserveAspect = true;
			if (sprite == null)
			{
				this.Icon.gameObject.SetActive(false);
			}
			else
			{
				this.Icon.gameObject.SetActive(true);
			}
			this.RequireInit(weapon.metaInfo);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0003ECED File Offset: 0x0003CEED
		public void OnClickToolTip()
		{
			this.OnDisabledClick();
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x0003ECF5 File Offset: 0x0003CEF5
		public void OnClickEquipment()
		{
			this.SetEquipmentText();
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x00003FDD File Offset: 0x000021DD
		public void SetEquipmentText()
		{
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnCheckOwner()
		{
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x0003ECFD File Offset: 0x0003CEFD
		private void Start()
		{
			this.InitScroll();
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0003ED05 File Offset: 0x0003CF05
		public void InitScroll()
		{
			this.CheckTransformRaycast(base.transform);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x001C1DF8 File Offset: 0x001BFFF8
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
						scrollExchanger.scrollRect = InventoryUI.CurrentWindow.ItemController.WeaponScroll;
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

		// Token: 0x06004C52 RID: 19538 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnEnterEquip()
		{
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnExitEquip()
		{
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x001BF5C4 File Offset: 0x001BD7C4
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

		// Token: 0x06004C55 RID: 19541 RVA: 0x0003E8FC File Offset: 0x0003CAFC
		private void OnEnable()
		{
			this.TooltipButton.interactable = true;
			this.TooltipButton.OnPointerExit(null);
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0003ED13 File Offset: 0x0003CF13
		static InventoryWeaponSlot()
		{
		}

		// Token: 0x040046AC RID: 18092
		public Text Type;

		// Token: 0x040046AD RID: 18093
		public Text DamageRange;

		// Token: 0x040046AE RID: 18094
		public Text AttackSpeed;

		// Token: 0x040046AF RID: 18095
		public Text Range;

		// Token: 0x040046B0 RID: 18096
		public Image DamageTypeImage;

		// Token: 0x040046B1 RID: 18097
		public static string Newline = Environment.NewLine;

		// Token: 0x040046B2 RID: 18098
		private string oldText = string.Empty;
	}
}
