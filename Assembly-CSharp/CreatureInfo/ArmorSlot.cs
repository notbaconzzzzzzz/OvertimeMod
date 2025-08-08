using System;
using Assets.Scripts.UI.Utils;
using Inventory;
using UnityEngine.UI;

namespace CreatureInfo
{
	// Token: 0x02000AC5 RID: 2757
	[Serializable]
	public class ArmorSlot : EquipSlot
	{
		// Token: 0x060052C3 RID: 21187 RVA: 0x00043290 File Offset: 0x00041490
		public ArmorSlot()
		{
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060052C4 RID: 21188 RVA: 0x000432A3 File Offset: 0x000414A3
		private ArmorModel ArmorModel
		{
			get
			{
				return base.Model as ArmorModel;
			}
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x001DD1BC File Offset: 0x001DB3BC
		public override void SetModel(EquipmentModel Model)
		{ // <Mod>
			base.SetModel(Model);
			this.GradeText.text = Model.metaInfo.grade;
			DefenseInfo defense = Model.metaInfo.defenseInfo.Copy();
			float num = EGOrealizationManager.instance.ArmorUpgrade(Model.metaInfo);
			defense.R -= num;
			defense.W -= num;
			defense.B -= num;
			defense.P -= num;
			InventoryItemController.SetGradeText(Model.metaInfo.Grade, this.GradeText);
			UIUtil.DefenseSetOnlyText(defense, this.RWBP_Defense);
			for (int i = 1; i <= 4; i++)
			{
				RwbpType rwbpType = (RwbpType)i;
				float multiplier = defense.GetMultiplier(rwbpType);
				string text = string.Format(num == 0f ? "{0} ({1:0.0})" : "{0} ({1:0.00})", EnumTextConverter.GetRwbpType(rwbpType).ToUpper(), multiplier);
				this.TypeText[i - 1].text = text;
			}
			this.CheckMakeCount();
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x001DD26C File Offset: 0x001DB46C
		public override void SetModel(EquipmentTypeInfo info)
		{ // <Mod>
			base.SetModel(info);
			InventoryItemController.SetGradeText(info.Grade, this.GradeText);
			DefenseInfo defense = info.defenseInfo.Copy();
			float num = EGOrealizationManager.instance.ArmorUpgrade(info);
			defense.R -= num;
			defense.W -= num;
			defense.B -= num;
			defense.P -= num;
			UIUtil.DefenseSetOnlyText(defense, this.RWBP_Defense);
			for (int i = 1; i <= 4; i++)
			{
				RwbpType rwbpType = (RwbpType)i;
				float multiplier = defense.GetMultiplier(rwbpType);
				string text = string.Format(num == 0f ? "{0} ({1:0.0})" : "{0} ({1:0.00})", EnumTextConverter.GetRwbpType(rwbpType).ToUpper(), multiplier);
				this.TypeText[i - 1].text = text;
			}
			this.portrait.SetArmor(info);
			this.CheckMakeCount();
			this.ItemName.text = info.Name;
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x001DD314 File Offset: 0x001DB514
		public void CheckMakeCount()
		{ // <Patch>
			int num = 0;
			int num2 = 0;
			if (InventoryModel.Instance.GetEquipCount_Mod(EquipmentTypeInfo.GetLcId(base.Info), out num, out num2))
			{
				string text = num + "/" + num2;
				this.MakeCount.text = text;
				this.cost = text;
			}
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x000432B0 File Offset: 0x000414B0
		public void OnEnter()
		{
			this.MakeCount.text = LocalizeTextDataModel.instance.GetText("CreatureInfo_Cost") + " " + this.Cost;
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x000432E1 File Offset: 0x000414E1
		public void OnExit()
		{
			this.MakeCount.text = this.cost;
		}

		// Token: 0x04004C5C RID: 19548
		public TooltipMouseOver MakeArmorTooltip;

		// Token: 0x04004C5D RID: 19549
		public Text GradeText;

		// Token: 0x04004C5E RID: 19550
		public Text[] TypeText;

		// Token: 0x04004C5F RID: 19551
		public Text[] RWBP_Defense;

		// Token: 0x04004C60 RID: 19552
		public Text MakeCount;

		// Token: 0x04004C61 RID: 19553
		public WorkerPortraitSetter portrait;

		// Token: 0x04004C62 RID: 19554
		public Button BuildButton;

		// Token: 0x04004C63 RID: 19555
		[NonSerialized]
		public int Cost;

		// Token: 0x04004C64 RID: 19556
		private string cost = string.Empty;

		// Token: 0x04004C65 RID: 19557
		[NonSerialized]
		public CreatureModel currentCreature;
	}
}
