using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000973 RID: 2419
	[Serializable]
	public class AgentEquipmentSlot
	{
		// Token: 0x06004909 RID: 18697 RVA: 0x001B85A0 File Offset: 0x001B67A0
		public void SetData(AgentModel agent)
		{ // <Patch>
			this.WeaponName.text = agent.Equipment.weapon.metaInfo.Name;
			DamageInfo damage = agent.Equipment.weapon.GetDamage(agent);
			if (EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) != 200038 && EquipmentTypeInfo.GetLcId(agent.Equipment.weapon.metaInfo) != 200004)
			{
				RwbpType type = damage.type;
				Color color;
				Color color2;
				UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
				this.TypeFill.color = Color.white;
				this.TypeText.text = EnumTextConverter.GetRwbpType(type).ToUpper();
				this.TypeText.color = color;
				this.TypeText.resizeTextForBestFit = true;
				this.TypeFill.enabled = true;
				this.TypeFill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
			}
			else
			{
				this.TypeFill.enabled = false;
				this.TypeText.text = "???";
				this.TypeText.color = Color.gray;
			}
			InventoryItemController.SetGradeText(agent.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
			InventoryItemController.SetGradeText(agent.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
			string grade = agent.Equipment.weapon.metaInfo.grade;
			foreach (Text text in this.Vanlia)
			{
				text.text = grade;
			}
			this.DualValue.SetActive(false);
			DefenseInfo defense = agent.defense;
			UIUtil.DefenseSetOnlyText(defense, this.DefenseType);
			UIUtil.SetDefenseTypeIcon(defense, this.DefenseFactorRenderer);
			if (agent.Equipment.armor != null)
			{
				this.ArmorName.text = agent.Equipment.armor.metaInfo.Name;
			}
		}

		// Token: 0x04004358 RID: 17240
		public GameObject ActiveControl;

		// Token: 0x04004359 RID: 17241
		[Header("Weapon")]
		public Text WeaponName;

		// Token: 0x0400435A RID: 17242
		public Image TypeFill;

		// Token: 0x0400435B RID: 17243
		public Text TypeText;

		// Token: 0x0400435C RID: 17244
		public Text WeaponGrade;

		// Token: 0x0400435D RID: 17245
		[Space(5f)]
		public GameObject SingleValue;

		// Token: 0x0400435E RID: 17246
		public GameObject DualValue;

		// Token: 0x0400435F RID: 17247
		public List<Text> Vanlia;

		// Token: 0x04004360 RID: 17248
		public Text Additional;

		// Token: 0x04004361 RID: 17249
		[Header("Armor")]
		public Text ArmorName;

		// Token: 0x04004362 RID: 17250
		public Text[] DefenseType;

		// Token: 0x04004363 RID: 17251
		public Text ArmorGrade;

		// Token: 0x04004364 RID: 17252
		public Image[] DefenseFactorRenderer;
	}
}
