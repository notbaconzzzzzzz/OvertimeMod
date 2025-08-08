using System;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace CreatureInfo
{
	// Token: 0x02000AC4 RID: 2756
	[Serializable]
	public class WeaponSlot : EquipSlot
	{
		// Token: 0x060052BC RID: 21180 RVA: 0x0004322C File Offset: 0x0004142C
		public WeaponSlot()
		{
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060052BD RID: 21181 RVA: 0x0004323F File Offset: 0x0004143F
		private WeaponModel WeaponModel
		{
			get
			{
				return base.Model as WeaponModel;
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x001DCE68 File Offset: 0x001DB068
		public override void SetModel(EquipmentModel Model)
		{
			base.SetModel(Model);
			UnitModel owner = Model.owner;
			this.ItemGrade.text = Model.metaInfo.Grade.ToString();
			InventoryItemController.SetGradeText(Model.metaInfo.Grade, this.ItemGrade);
			this.DamageRange.text = (int)this.WeaponModel.GetDamage(owner).min + "-" + (int)this.WeaponModel.GetDamage(owner).max;
			this.AttackSpeed.text = this.WeaponModel.metaInfo.attackSpeed + string.Empty;
			this.AttackRange.text = this.WeaponModel.metaInfo.range + string.Empty;
			this.CheckMakeCount();
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x001DCF5C File Offset: 0x001DB15C
		public override void SetModel(EquipmentTypeInfo info)
		{
			base.SetModel(info);
			string empty = string.Empty;
			string empty2 = string.Empty;
			InventoryItemDescGetter.GetWeaponDesc(info, out empty2, out empty);
			this.ItemGrade.text = info.Grade.ToString();
			InventoryItemController.SetGradeText(info.Grade, this.ItemGrade);
			this.DamageRange.text = (int)info.damageInfo.min + "-" + (int)info.damageInfo.max;
			this.AttackSpeed.text = empty2;
			this.AttackRange.text = empty;
			this.ItemName.text = info.Name;
			RwbpType type = info.damageInfo.type;
			Color white = Color.white;
			Color white2 = Color.white;
			UIColorManager.instance.GetRWBPTypeColor(type, out white, out white2);
			switch (type)
			{
			case RwbpType.R:
				this.TypeText.text = "RED";
				break;
			case RwbpType.W:
				this.TypeText.text = "WHITE";
				break;
			case RwbpType.B:
				this.TypeText.text = "BLACK";
				break;
			case RwbpType.P:
				this.TypeText.text = "PALE";
				break;
			default:
				this.TypeText.text = "NONE";
				break;
			}
			this.TypeText.color = white;
			this.TypeFill.color = Color.white;
			this.TypeFill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
			Sprite weaponSprite = WorkerSpriteManager.instance.GetWeaponSprite(info.weaponClassType, info.sprite);
			this.ItemImage.sprite = weaponSprite;
			this.ItemImage.SetNativeSize();
			if (weaponSprite == null)
			{
				this.ItemImage.enabled = false;
			}
			else
			{
				this.ItemImage.enabled = true;
			}
			this.CheckMakeCount();
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x001DD160 File Offset: 0x001DB360
		public void CheckMakeCount()
		{
			int num = 0;
			int num2 = 0;
			if (InventoryModel.Instance.GetEquipCount(base.Info.id, out num, out num2))
			{
				this.cost = num + "/" + num2;
				this.MakeCount.text = this.cost;
			}
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0004324C File Offset: 0x0004144C
		public void OnEnter()
		{
			this.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_Cost") + " " + this.Cost;
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x0004327D File Offset: 0x0004147D
		public void OnExit()
		{
			this.MakeCount.text = this.cost;
		}

		// Token: 0x04004C4F RID: 19535
		public TooltipMouseOver MakeWeaponTooltip;

		// Token: 0x04004C50 RID: 19536
		public Image ItemImage;

		// Token: 0x04004C51 RID: 19537
		public Text ItemGrade;

		// Token: 0x04004C52 RID: 19538
		public Text DamageRange;

		// Token: 0x04004C53 RID: 19539
		public Text AttackSpeed;

		// Token: 0x04004C54 RID: 19540
		public Text AttackRange;

		// Token: 0x04004C55 RID: 19541
		public Text MakeCount;

		// Token: 0x04004C56 RID: 19542
		public Image TypeFill;

		// Token: 0x04004C57 RID: 19543
		public Text TypeText;

		// Token: 0x04004C58 RID: 19544
		public Button BuildButton;

		// Token: 0x04004C59 RID: 19545
		private string cost = string.Empty;

		// Token: 0x04004C5A RID: 19546
		[NonSerialized]
		public int Cost;

		// Token: 0x04004C5B RID: 19547
		[NonSerialized]
		public CreatureModel currentCreature;
	}
}
