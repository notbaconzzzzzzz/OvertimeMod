using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using CreatureInfo;
using UnityEngine;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000962 RID: 2402
	[Serializable]
	public class WorkAllocateRegion : CommandWindowRegion
	{
		// Token: 0x06004857 RID: 18519 RVA: 0x0003CDB2 File Offset: 0x0003AFB2
		public WorkAllocateRegion()
		{
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06004858 RID: 18520 RVA: 0x0003CDC5 File Offset: 0x0003AFC5
		public CreatureModel CurrentModel
		{
			get
			{
				return this._currentModel;
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x001AFB28 File Offset: 0x001ADD28
		private void OnObserved(CreatureModel creature)
		{
			this.Name.text = creature.GetUnitName();
			this.MaximumCubeGenerate.text = creature.metaInfo.feelingStateCubeBounds.GetLastBound().ToString();
			this.RiskLevel.text = creature.metaInfo.riskLevelForce;
			this.Portrait.sprite = Add_On.GetPortrait(creature.metaInfo.portraitSrcForcely);
			DamageInfo workDamage = creature.metaInfo.workDamage;
			this.WorkDamageType.text = EnumTextConverter.GetRwbpType(workDamage.type).ToUpper();
			this.WorkDamageRange.text = (int)workDamage.min + "-" + (int)workDamage.max;
			RwbpType type = workDamage.type;
			Color white = Color.white;
			Color white2 = Color.white;
			UIColorManager.instance.GetRWBPTypeColor(type, out white, out white2);
			this.WorkDamageType.color = white;
			this.WorkDamageFill.color = Color.white;
			this.WorkDamageFill.enabled = true;
			this.WorkDamageFill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
			this.RiskLevel.color = DeployUI.GetCreatureRiskLevelColor(creature.metaInfo.GetRiskLevel());
			int num = creature.metaInfo.feelingStateCubeBounds.upperBounds.Length;
			int num2 = 0;
			for (int i = 0; i < 3; i++)
			{
				CreatureInfoStatFeelingStateSlot creatureInfoStatFeelingStateSlot = this.slots[i];
				if (i >= num)
				{
					creatureInfoStatFeelingStateSlot.gameObject.SetActive(false);
				}
				else
				{
					creatureInfoStatFeelingStateSlot.gameObject.SetActive(true);
					int num3 = creature.metaInfo.feelingStateCubeBounds.upperBounds[i];
					int num4 = num2;
					num2 = num3 + 1;
					string rangeDesc = num4 + " - " + num3;
					creatureInfoStatFeelingStateSlot.SetData(i, rangeDesc);
				}
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x001AFD08 File Offset: 0x001ADF08
		private void NonObserved(CreatureModel creature)
		{ // <Mod>
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
			{
				Name.text = "????????";
			}
			else
			{
				this.Name.text = creature.metaInfo.codeId;
			}
			this.MaximumCubeGenerate.text = WorkAllocateRegion.unknown_Text;
			this.RiskLevel.text = WorkAllocateRegion.unknown;
			this.RiskLevel.color = DeployUI.instance.DeployColorSet[0];
			this.Portrait.sprite = Resources.Load<Sprite>("Sprites/Unit/creature/NoData");
			DamageInfo workDamage = creature.metaInfo.workDamage;
			this.WorkDamageType.text = WorkAllocateRegion.unknown;
			this.WorkDamageFill.color = UIColorManager.Orange;
			this.WorkDamageType.color = UIColorManager.Orange;
			this.WorkDamageRange.text = WorkAllocateRegion.unknown_Text;
			this.WorkDamageFill.enabled = false;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
			{
				for (int i = 0; i < 3; i++)
				{
					CreatureInfoStatFeelingStateSlot creatureInfoStatFeelingStateSlot = this.slots[i];
					creatureInfoStatFeelingStateSlot.gameObject.SetActive(true);
					creatureInfoStatFeelingStateSlot.SetData(i, WorkAllocateRegion.unknown_Text);
				}
				return;
			}
			int num = creature.metaInfo.feelingStateCubeBounds.upperBounds.Length;
			int num2 = 0;
			for (int i = 0; i < 3; i++)
			{
				CreatureInfoStatFeelingStateSlot creatureInfoStatFeelingStateSlot = this.slots[i];
				if (i >= creature.metaInfo.feelingStateCubeBounds.upperBounds.Length)
				{
					creatureInfoStatFeelingStateSlot.gameObject.SetActive(false);
				}
				else
				{
					creatureInfoStatFeelingStateSlot.gameObject.SetActive(true);
					int num3 = creature.metaInfo.feelingStateCubeBounds.upperBounds[i];
					int num4 = num2;
					num2 = num3 + 1;
					string text = num4 + " - " + num3;
					creatureInfoStatFeelingStateSlot.SetData(i, WorkAllocateRegion.unknown_Text);
				}
			}
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x001AFE88 File Offset: 0x001AE088
		public override void SetData(UnitModel target)
		{ // <Mod>
			if (!(target is CreatureModel))
			{
				Debug.LogError("Should be creatureModel");
				return;
			}
			CreatureModel creatureModel = target as CreatureModel;
			this._currentModel = creatureModel;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
			{
				CodeNo.text = "?-??-??";
				NonObserved(creatureModel);
				return;
			}
			this.CodeNo.text = creatureModel.metaInfo.codeId;
			if (creatureModel.observeInfo.GetObserveState(CreatureModel.regionName[0]))
			{
				this.OnObserved(creatureModel);
			}
			else
			{
				this.NonObserved(creatureModel);
			}
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0003CDCD File Offset: 0x0003AFCD
		static WorkAllocateRegion()
		{
		}

		// Token: 0x04004287 RID: 17031
		[NonSerialized]
		private CreatureModel _currentModel;

		// Token: 0x04004288 RID: 17032
		public Text MaximumCubeGenerate;

		// Token: 0x04004289 RID: 17033
		public Text CodeNo;

		// Token: 0x0400428A RID: 17034
		public Text Name;

		// Token: 0x0400428B RID: 17035
		public Text RiskLevel;

		// Token: 0x0400428C RID: 17036
		public Image Portrait;

		// Token: 0x0400428D RID: 17037
		public RectTransform listParent;

		// Token: 0x0400428E RID: 17038
		public List<CreatureInfoStatFeelingStateSlot> slots = new List<CreatureInfoStatFeelingStateSlot>();

		// Token: 0x0400428F RID: 17039
		public Image WorkDamageFill;

		// Token: 0x04004290 RID: 17040
		public Text WorkDamageType;

		// Token: 0x04004291 RID: 17041
		public Text WorkDamageRange;

		// Token: 0x04004292 RID: 17042
		private static string unknown = "?";

		// Token: 0x04004293 RID: 17043
		private static string unknown_Text = "Unknown";
	}
}
