/*
public void SetData(WorkerModel worker) // Resistances will use 2 decimal points
*/
using System;
using Assets.Scripts.UI.Utils;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000958 RID: 2392
	[Serializable]
	public class WorkerSuppressRegion : CommandWindowRegion
	{
		// Token: 0x060047E7 RID: 18407 RVA: 0x0003BF2F File Offset: 0x0003A12F
		public WorkerSuppressRegion()
		{
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0003BF5D File Offset: 0x0003A15D
		public override void SetData(UnitModel target)
		{
			if (!(target is WorkerModel))
			{
				Debug.LogError("Target Should Be Worker");
				return;
			}
			base.SetData(target);
			this.SetData(target as WorkerModel);
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x001ACE08 File Offset: 0x001AB008
		public void SetData(WorkerModel worker)
		{ // <Mod> resistances will now use 2 decimal points
			if (worker is AgentModel)
			{
				AgentModel agentModel = worker as AgentModel;
				this.GradeImage.sprite = DeployUI.GetAgentGradeSprite(agentModel);
				this.AgentName.text = agentModel.GetUnitName();
				this.portrait.SetWorker(worker);
				this.Title.text = agentModel.GetTitle();
				int num = agentModel.maxHp - agentModel.primaryStat.maxHP;
				int num2 = agentModel.maxMental - agentModel.primaryStat.maxMental;
				int num3 = agentModel.workProb - agentModel.primaryStat.workProb;
				int num4 = agentModel.workSpeed - agentModel.primaryStat.cubeSpeed;
				if (num > 0)
				{
					this.Stat_R.slots[0].SetText(agentModel.primaryStat.maxHP + string.Empty, "+" + num);
				}
				else if (num < 0)
				{
					this.Stat_R.slots[0].SetText(agentModel.primaryStat.maxHP + string.Empty, "-" + -num);
				}
				else
				{
					this.Stat_R.slots[0].SetText(agentModel.primaryStat.maxHP + string.Empty);
				}
				if (num2 > 0)
				{
					this.Stat_W.slots[0].SetText(agentModel.primaryStat.maxMental + string.Empty, "+" + num2);
				}
				else if (num2 < 0)
				{
					this.Stat_W.slots[0].SetText(agentModel.primaryStat.maxMental + string.Empty, "-" + -num2);
				}
				else
				{
					this.Stat_W.slots[0].SetText(agentModel.primaryStat.maxMental + string.Empty);
				}
				if (num3 > 0)
				{
					this.Stat_B.slots[0].SetText(agentModel.primaryStat.workProb + string.Empty, "+" + num3);
				}
				else if (num3 < 0)
				{
					this.Stat_B.slots[0].SetText(agentModel.primaryStat.workProb + string.Empty, "-" + -num3);
				}
				else
				{
					this.Stat_B.slots[0].SetText(agentModel.primaryStat.workProb + string.Empty);
				}
				if (num4 > 0)
				{
					this.Stat_B.slots[1].SetText(agentModel.primaryStat.cubeSpeed + string.Empty, "+" + num4);
				}
				else if (num4 < 0)
				{
					this.Stat_B.slots[1].SetText(agentModel.primaryStat.cubeSpeed + string.Empty, "-" + -num4);
				}
				else
				{
					this.Stat_B.slots[1].SetText(agentModel.primaryStat.cubeSpeed + string.Empty);
				}
				this.Stat_P.slots[0].SetText(agentModel.primaryStat.attackSpeed + string.Empty);
				this.Stat_P.slots[1].SetText((int)agentModel.movement + string.Empty);
				this.Stat_R.Fill_Inner.text = AgentModel.GetLevelGradeText(agentModel.Rstat);
				this.Stat_W.Fill_Inner.text = AgentModel.GetLevelGradeText(agentModel.Wstat);
				this.Stat_B.Fill_Inner.text = AgentModel.GetLevelGradeText(agentModel.Bstat);
				this.Stat_P.Fill_Inner.text = AgentModel.GetLevelGradeText(agentModel.Pstat);
				this.Weapon.StatName.text = agentModel.Equipment.weapon.metaInfo.Name;
				DamageInfo damage = agentModel.Equipment.weapon.GetDamage(agentModel);
				RwbpType type = damage.type;
				this.Weapon.Fill_Inner.text = EnumTextConverter.GetRwbpType(type).ToUpper();
				Color color;
				Color color2;
				UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
				this.Weapon.Fill_Inner.color = color;
				this.Weapon.Fill.color = color;
				string text = string.Format("{0}-{1}", (int)damage.min, (int)damage.max);
				this.Weapon.slots[0].SetText(text);
				DefenseInfo defense = agentModel.defense;
				UIUtil.DefenseSetOnlyText(defense, this.DefenseType);
				UIUtil.DefenseSetFactor(defense, this.DefenseFactor, false);
				if (agentModel.Equipment.armor != null)
				{
					this.ArmorName.text = agentModel.Equipment.armor.metaInfo.Name;
				}
				else
				{
					this.ArmorName.text = "Armor is missing";
				}
				InventoryItemController.SetGradeText(agentModel.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
				InventoryItemController.SetGradeText(agentModel.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
			}
			else if (worker is OfficerModel)
			{
				OfficerModel officerModel = worker as OfficerModel;
				this.GradeImage.sprite = DeployUI.GetGradeSprite(1);
				this.AgentName.text = officerModel.GetUnitName();
				this.portrait.SetWorker(officerModel);
				this.Stat_R.slots[0].SetText("15");
				this.Stat_W.slots[0].SetText("15");
				this.Stat_B.slots[0].SetText("15");
				this.Stat_B.slots[1].SetText("15");
				this.Stat_P.slots[0].SetText("15");
				this.Stat_P.slots[1].SetText("15");
				this.Stat_R.Fill_Inner.text = AgentModel.GetLevelGradeText(1);
				this.Stat_W.Fill_Inner.text = AgentModel.GetLevelGradeText(1);
				this.Stat_B.Fill_Inner.text = AgentModel.GetLevelGradeText(1);
				this.Stat_P.Fill_Inner.text = AgentModel.GetLevelGradeText(1);
				this.Weapon.StatName.text = officerModel.Equipment.weapon.metaInfo.Name;
				DamageInfo damage2 = officerModel.Equipment.weapon.GetDamage(officerModel);
				RwbpType type2 = damage2.type;
				this.Weapon.Fill_Inner.text = type2.ToString();
				Color color3;
				Color color4;
				UIColorManager.instance.GetRWBPTypeColor(type2, out color3, out color4);
				this.Weapon.Fill_Inner.color = color3;
				this.Weapon.Fill.color = color3;
				string text2 = string.Format("{0}-{1}", (int)damage2.min, (int)damage2.max);
				this.Weapon.slots[0].SetText(text2);
				DefenseInfo defense2 = officerModel.defense;
				UIUtil.DefenseSetOnlyText(defense2, this.DefenseType);
				if (officerModel.Equipment.armor != null)
				{
					this.ArmorName.text = officerModel.Equipment.armor.metaInfo.Name;
				}
				else
				{
					this.ArmorName.text = "Armor is missing";
				}
				InventoryItemController.SetGradeText(officerModel.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
				if (officerModel.Equipment.armor != null)
				{
					InventoryItemController.SetGradeText(officerModel.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
				}
			}
		}

		// Token: 0x0400423C RID: 16956
		public Image GradeImage;

		// Token: 0x0400423D RID: 16957
		public Text AgentName;

		// Token: 0x0400423E RID: 16958
		public Text Title;

		// Token: 0x0400423F RID: 16959
		public AgentInfoWindow.StatObject Stat_R;

		// Token: 0x04004240 RID: 16960
		public AgentInfoWindow.StatObject Stat_W;

		// Token: 0x04004241 RID: 16961
		public AgentInfoWindow.StatObject Stat_B;

		// Token: 0x04004242 RID: 16962
		public AgentInfoWindow.StatObject Stat_P;

		// Token: 0x04004243 RID: 16963
		public AgentInfoWindow.StatObject Weapon;

		// Token: 0x04004244 RID: 16964
		public WorkerPortraitSetter portrait;

		// Token: 0x04004245 RID: 16965
		public Text WeaponGrade;

		// Token: 0x04004246 RID: 16966
		public Text ArmorGrade;

		// Token: 0x04004247 RID: 16967
		public Text ArmorName;

		// Token: 0x04004248 RID: 16968
		public Text[] DefenseType;

		// Token: 0x04004249 RID: 16969
		public Text[] DefenseInner;

		// Token: 0x0400424A RID: 16970
		public Text[] DefenseFactor;

		// Token: 0x0400424B RID: 16971
		public Image[] DefenseFill;
	}
}
