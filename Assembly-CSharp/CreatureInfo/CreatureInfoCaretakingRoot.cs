using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CreatureInfo
{
	// Token: 0x02000ACA RID: 2762
	public class CreatureInfoCaretakingRoot : CreatureInfoController
	{
		// Token: 0x0600534A RID: 21322 RVA: 0x00043CDB File Offset: 0x00041EDB
		public CreatureInfoCaretakingRoot()
		{
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x001E586C File Offset: 0x001E3A6C
		public void SetSlotAreaName(string[] names)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				this.slots[i].SetArea(names[i]);
			}
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x00043CEE File Offset: 0x00041EEE
		public override void Initialize(CreatureModel creature)
		{
			base.Initialize(creature);
			this.ListInit(creature);
			this.listParent.anchoredPosition = Vector2.zero;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x00043D0E File Offset: 0x00041F0E
		public override void Initialize()
		{
			base.Initialize();
			this.ListInit();
			this.listParent.anchoredPosition = Vector2.zero;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001E58AC File Offset: 0x001E3AAC
		private void ListInit(CreatureModel creature)
		{
			int count = creature.metaInfo.specialSkillTable.descList.Count;
			this.revealedCount = count;
			string unitName = creature.GetUnitName();
			int i = 0;
			while (i < count)
			{
				string original = creature.metaInfo.specialSkillTable.descList[i].original;
				string text = original;
				int num = 0;
				for (;;)
				{
					int num2 = text.IndexOf("#" + num);
					if (text == string.Empty || text == null)
					{
						break;
					}
					if (num2 == -1)
					{
						break;
					}
					AgentName agentName = null;
					if (!base.MetaInfo.GetAgentName(i * 100 + num, out agentName))
					{
						agentName = AgentNameList.instance.GetFakeNameByInfo();
					}
					text = text.Replace("#" + num, agentName.GetName());
					num++;
				}
				IL_EA:
				text = text.Replace("$0", unitName);
				CreatureInfoCaretakingSlot creatureInfoCaretakingSlot = this.slots[i];
				this.slots[i].gameObject.SetActive(true);
				creatureInfoCaretakingSlot.SetData(text);
				if (i > 0)
				{
					creatureInfoCaretakingSlot.PrevSlot = this.slots[i - 1];
				}
				i++;
				continue;
				goto IL_EA;
			}
			for (int j = count; j < 7; j++)
			{
				this.slots[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001E5A38 File Offset: 0x001E3C38
		private void ListInit()
		{
			if (base.MetaInfo.specialSkillTable == null)
			{
				base.MetaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(base.MetaInfo.id).GetCopy();
			}
			int count = base.MetaInfo.specialSkillTable.descList.Count;
			this.revealedCount = count;
			string unitName = CreatureModel.GetUnitName(base.MetaInfo, base.ObserveInfo);
			int i = 0;
			while (i < count)
			{
				string original = base.MetaInfo.specialSkillTable.descList[i].original;
				string text = original;
				int num = 0;
				for (;;)
				{
					int num2 = text.IndexOf("#" + num);
					if (text == string.Empty || text == null)
					{
						break;
					}
					if (num2 == -1)
					{
						break;
					}
					AgentName agentName = null;
					if (!base.MetaInfo.GetAgentName(i * 100 + num, out agentName))
					{
						agentName = AgentNameList.instance.GetFakeNameByInfo();
						base.MetaInfo.AddAgentName(i * 100 + num, agentName);
					}
					text = text.Replace("#" + num, agentName.GetName());
					num++;
				}
				IL_140:
				text = text.Replace("$0", unitName);
				CreatureInfoCaretakingSlot creatureInfoCaretakingSlot = this.slots[i];
				this.slots[i].gameObject.SetActive(true);
				creatureInfoCaretakingSlot.SetData(text);
				if (i > 0)
				{
					creatureInfoCaretakingSlot.PrevSlot = this.slots[i - 1];
				}
				i++;
				continue;
				goto IL_140;
			}
			for (int j = count; j < 7; j++)
			{
				this.slots[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x00043D2C File Offset: 0x00041F2C
		public override void OnPurchase()
		{
			this._isOpened = true;
			this.SetEnabled();
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x00043D3B File Offset: 0x00041F3B
		public override bool OnClick()
		{
			return CreatureInfoWindow.CurrentWindow.OnTryPurchase(this);
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x000043CD File Offset: 0x000025CD
		private void SetEnabled()
		{
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x000043CD File Offset: 0x000025CD
		private void SetDisabled()
		{
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x001E5C18 File Offset: 0x001E3E18
		public override bool IsOpened()
		{
			bool result = true;
			for (int i = 0; i < this.revealedCount; i++)
			{
				if (!this.slots[i].IsOpened())
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x00043D48 File Offset: 0x00041F48
		public int GetIndex(CreatureInfoCaretakingSlot slot)
		{
			return this.slots.IndexOf(slot);
		}

		// Token: 0x04004CB5 RID: 19637
		private const string slotSrc = "UIComponent/CreatureInfo/CareTakingSlot";

		// Token: 0x04004CB6 RID: 19638
		public RectTransform listParent;

		// Token: 0x04004CB7 RID: 19639
		public ScrollRect scroll;

		// Token: 0x04004CB8 RID: 19640
		public List<CreatureInfoCaretakingSlot> slots = new List<CreatureInfoCaretakingSlot>();

		// Token: 0x04004CB9 RID: 19641
		private int revealedCount;
	}
}
