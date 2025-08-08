/*
public void SetUI() // Resistances will use 2 decimal points
*/
using System;
using System.Collections;
using Assets.Scripts.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	// Token: 0x020009C0 RID: 2496
	public class InventoryAgentController : MonoBehaviour
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x00004094 File Offset: 0x00002294
		public InventoryAgentController()
		{
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06004B9D RID: 19357 RVA: 0x0003E499 File Offset: 0x0003C699
		public AgentModel CurrentAgent
		{
			get
			{
				return this._currentAgent;
			}
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x001BDF20 File Offset: 0x001BC120
		public void SetAgent(AgentModel agent)
		{
			this._currentAgent = agent;
			Notice.instance.Send(NoticeName.OnInventoryAgentChanged, new object[]
			{
				agent
			});
			if (agent == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			this.SetUI();
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x001BDF74 File Offset: 0x001BC174
		public void SetUI()
		{ // <Patch> <Mod> resistances will now use 2 decimal points
			if (this.CurrentAgent == null)
			{
				return;
			}
			this.AgentSlot.SetAgent(this.CurrentAgent);
			try
			{
				DamageInfo damage = this.CurrentAgent.Equipment.weapon.GetDamage(this.CurrentAgent);
				float dmgFactor = 1f;
				dmgFactor *= this.CurrentAgent.GetDamageFactorByEquipment();
				dmgFactor *= this.CurrentAgent.GetDamageFactorBySefiraAbility();
				dmgFactor *= this.CurrentAgent.Equipment.weapon.script.GetReinforcementDmg();
				if (EquipmentTypeInfo.GetLcId(this.CurrentAgent.Equipment.weapon.metaInfo) != 200038 && EquipmentTypeInfo.GetLcId(this.CurrentAgent.Equipment.weapon.metaInfo) != 200004)
				{
					RwbpType type = damage.type;
					Color color;
					Color color2;
					UIColorManager.instance.GetRWBPTypeColor(type, out color, out color2);
					this.TypeFill.enabled = true;
					this.TypeFill.color = Color.white;
					this.TypeFill.sprite = IconManager.instance.DamageIcon[type - RwbpType.R];
					this.TypeText.color = color;
					this.TypeText.text = EnumTextConverter.GetRwbpType(type).ToUpper();
					string text = string.Format("{0}-{1}", (int)(damage.min * dmgFactor), (int)(damage.max * dmgFactor));
					this.DamageText.text = text;
				}
				else
				{
					this.TypeFill.color = Color.white;
					this.TypeFill.enabled = false;
					this.TypeText.color = Color.gray;
					this.TypeText.text = "???";
					string text2 = string.Format("{0}-{1}", (int)(damage.min * dmgFactor), (int)(damage.max * dmgFactor));
					this.DamageText.text = text2;
				}
				WorkerPrimaryStatBonus titleBonus = this.CurrentAgent.titleBonus;
				int num = this.CurrentAgent.primaryStat.maxHP + titleBonus.maxHP;
				int num2 = this.CurrentAgent.primaryStat.maxMental + titleBonus.maxMental;
				int num3 = this.CurrentAgent.primaryStat.workProb + titleBonus.workProb;
				int num4 = this.CurrentAgent.primaryStat.cubeSpeed + titleBonus.cubeSpeed;
				int num5 = this.CurrentAgent.primaryStat.attackSpeed + titleBonus.attackSpeed;
				int num6 = this.CurrentAgent.primaryStat.movementSpeed + titleBonus.movementSpeed;
				int num7 = this.CurrentAgent.maxHp - num;
				int num8 = this.CurrentAgent.maxMental - num2;
				int num9 = this.CurrentAgent.workProb - num3;
				int num10 = this.CurrentAgent.workSpeed - num4;
				int num11 = (int)this.CurrentAgent.attackSpeed - num5;
				int num12 = (int)this.CurrentAgent.movement - num6;
				if (num7 > 0)
				{
					this.Stats[0].slots[0].SetText(num + string.Empty, "+" + num7);
				}
				else if (num7 < 0)
				{
					this.Stats[0].slots[0].SetText(num + string.Empty, "-" + -num7);
				}
				else
				{
					this.Stats[0].slots[0].SetText(num + string.Empty);
				}
				if (num8 > 0)
				{
					this.Stats[1].slots[0].SetText(num2 + string.Empty, "+" + num8);
				}
				else if (num8 < 0)
				{
					this.Stats[1].slots[0].SetText(num2 + string.Empty, "-" + -num8);
				}
				else
				{
					this.Stats[1].slots[0].SetText(num2 + string.Empty);
				}
				if (num9 > 0)
				{
					this.Stats[2].slots[0].SetText(num3 + string.Empty, "+" + num9);
				}
				else if (num9 < 0)
				{
					this.Stats[2].slots[0].SetText(num3 + string.Empty, "-" + -num9);
				}
				else
				{
					this.Stats[2].slots[0].SetText(num3 + string.Empty);
				}
				if (num10 > 0)
				{
					this.Stats[2].slots[1].SetText(num4 + string.Empty, "+" + num10);
				}
				else if (num10 < 0)
				{
					this.Stats[2].slots[1].SetText(num4 + string.Empty, "-" + -num10);
				}
				else
				{
					this.Stats[2].slots[1].SetText(num4 + string.Empty);
				}
				if (num12 > 0)
				{
					this.Stats[3].slots[0].SetText(num6 + string.Empty, "+" + num12);
				}
				else if (num12 < 0)
				{
					this.Stats[3].slots[0].SetText(num6 + string.Empty, "-" + -num12);
				}
				else
				{
					this.Stats[3].slots[0].SetText(num6 + string.Empty);
				}
				if (num11 > 0)
				{
					this.Stats[3].slots[1].SetText(num5 + string.Empty, "+" + num11);
				}
				else if (num11 < 0)
				{
					this.Stats[3].slots[1].SetText(num5 + string.Empty, "-" + -num11);
				}
				else
				{
					this.Stats[3].slots[1].SetText(num5 + string.Empty);
				}
				string name;
				name = LocalizeTextDataModel.instance.GetText("Rstat");
				if (name.Length > 5)
				{
					name = name.Substring(0, 4);
				}
				this.Stats[0].Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(CurrentAgent.Rstat), CurrentAgent.primaryStat.maxHP);
				name = LocalizeTextDataModel.instance.GetText("Wstat");
				if (name.Length > 5)
				{
					name = name.Substring(0, 4);
				}
				this.Stats[1].Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(CurrentAgent.Wstat), CurrentAgent.primaryStat.maxMental);
				name = LocalizeTextDataModel.instance.GetText("Bstat");
				if (name.Length > 5)
				{
					name = name.Substring(0, 4);
				}
				this.Stats[2].Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(CurrentAgent.Bstat), CurrentAgent.primaryStat.workProb);
				name = LocalizeTextDataModel.instance.GetText("Pstat");
				if (name.Length > 5)
				{
					name = name.Substring(0, 4);
				}
				this.Stats[3].Fill_Inner.text = string.Format("{0} {1} ({2})", name, AgentModel.GetLevelGradeText(CurrentAgent.Pstat), CurrentAgent.primaryStat.attackSpeed);
				DefenseInfo defense = this.CurrentAgent.defense;
				UIUtil.DefenseSetOnlyText(defense, this.DefenseType);
				UIUtil.DefenseSetFactor(defense, this.DefenseFactor, false);
				name = this.CurrentAgent.Equipment.weapon.metaInfo.Name;
				if (name == "UNKNOWN")
				{
					this.WeaponTitle.text = LocalizeTextDataModel.instance.GetText("Inventory_WeaponTitle");
				}
				else
				{
					this.WeaponTitle.text = name;
				}
				string name2 = this.CurrentAgent.Equipment.armor.metaInfo.Name;
				if (name2 == "UNKNOWN")
				{
					this.ArmorTitle.text = LocalizeTextDataModel.instance.GetText("Inventory_ArmorTitle");
				}
				else
				{
					this.ArmorTitle.text = name2;
				}
				this.SubEquipTitle.text = LocalizeTextDataModel.instance.GetText("Inventory_GiftTitle");
				IEnumerator enumerator = this.SubEquipListParent.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						UnityEngine.Object.Destroy(transform.gameObject);
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
				foreach (EquipmentModel equipmentModel in this.CurrentAgent.Equipment.gifts.addedGifts)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.attachUnit);
					InventoryAttachmentUnit component = gameObject.GetComponent<InventoryAttachmentUnit>();
					component.text.text = equipmentModel.metaInfo.Name + " : " + equipmentModel.metaInfo.Description;
					gameObject.transform.SetParent(this.SubEquipListParent.transform);
					gameObject.transform.localScale = Vector3.one;
				}
				foreach (EquipmentModel equipmentModel2 in this.CurrentAgent.Equipment.gifts.replacedGifts)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.attachUnit);
					InventoryAttachmentUnit component2 = gameObject2.GetComponent<InventoryAttachmentUnit>();
					component2.text.text = equipmentModel2.metaInfo.Name + " : " + equipmentModel2.metaInfo.Description;
					gameObject2.transform.SetParent(this.SubEquipListParent.transform);
					gameObject2.transform.localScale = Vector3.one;
				}
				InventoryItemController.SetGradeText(this.CurrentAgent.Equipment.weapon.metaInfo.Grade, this.WeaponGrade);
				InventoryItemController.SetGradeText(this.CurrentAgent.Equipment.armor.metaInfo.Grade, this.ArmorGrade);
				this.WeaponImage.sprite = this.CurrentAgent.GetWeaponSprite();
				this.ArmorImage.SetArmor(this.CurrentAgent.Equipment.armor.metaInfo);
				VerticalLayoutGroup component3 = this.SubEquipListParent.transform.GetComponent<VerticalLayoutGroup>();
				component3.enabled = false;
				component3.enabled = true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x04004617 RID: 17943
		public InventoryAgentSlot AgentSlot;

		// Token: 0x04004618 RID: 17944
		[Header("SubEquipment")]
		public Text SubEquipTitle;

		// Token: 0x04004619 RID: 17945
		public RectTransform SubEquipListParent;

		// Token: 0x0400461A RID: 17946
		public GameObject attachUnit;

		// Token: 0x0400461B RID: 17947
		[Header("Weapon")]
		public Text WeaponTitle;

		// Token: 0x0400461C RID: 17948
		public Image TypeFill;

		// Token: 0x0400461D RID: 17949
		public Text TypeText;

		// Token: 0x0400461E RID: 17950
		public Text DamageText;

		// Token: 0x0400461F RID: 17951
		public Text WeaponGrade;

		// Token: 0x04004620 RID: 17952
		public Image WeaponImage;

		// Token: 0x04004621 RID: 17953
		[Header("Armor")]
		public Text ArmorTitle;

		// Token: 0x04004622 RID: 17954
		public Text[] DefenseType;

		// Token: 0x04004623 RID: 17955
		public Text[] DefenseFactor;

		// Token: 0x04004624 RID: 17956
		public Text ArmorGrade;

		// Token: 0x04004625 RID: 17957
		public WorkerPortraitSetter ArmorImage;

		// Token: 0x04004626 RID: 17958
		[Header("Stat")]
		public AgentInfoWindow.StatObject[] Stats;

		// Token: 0x04004627 RID: 17959
		[NonSerialized]
		private AgentModel _currentAgent;
	}
}
