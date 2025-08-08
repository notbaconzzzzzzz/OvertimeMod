using System;
using System.Collections.Generic;
using System.Linq; // 
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
		{ // <Mod>
			int count = creature.metaInfo.specialSkillTable.descList.Count;
			TryExtendSlots(count);
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
			for (int j = count; j < this.slots.Count; j++)
			{
				this.slots[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001E5A38 File Offset: 0x001E3C38
		private void ListInit()
		{ // <Patch> <Mod>
			if (this.MetaInfo.specialSkillTable == null)
			{
				this.MetaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(CreatureTypeInfo.GetLcId(this.MetaInfo)).GetCopy();
			}
			int count = base.MetaInfo.specialSkillTable.descList.Count;
			TryExtendSlots(count);
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
			for (int j = count; j < this.slots.Count; j++)
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

		// <Mod>
		public void TryExtendSlots(int count)
		{
			if (this.slots.Count >= count) return;
			GameObject template = slots[0].gameObject;
			for (int i = this.slots.Count; i < count; i++)
			{
				GameObject obj = GameObject.Instantiate(template);
				obj.transform.SetParent(listParent);
				obj.layer = template.layer;
				obj.transform.localPosition = template.transform.localPosition;
				obj.transform.localRotation = template.transform.localRotation;
				obj.transform.localScale = template.transform.localScale;
				obj.SetActive(true);
				CreatureInfoCaretakingSlot component = obj.GetComponent<CreatureInfoCaretakingSlot>();
				if (i < CreatureModel.careTakingRegion.Length)
				{
					component.SetArea(CreatureModel.careTakingRegion[i]);
				}
				component.Open = obj.GetComponentInChildren<CreatureInfoOpenArea>();
				slots.Add(component);
				CreatureInfoWindow.CurrentWindow.AddController(component);
				string name = slots[0].Open.CubeImage.name;
				component.Open.CubeImage = obj.GetComponentsInChildren<Image>().ToList<Image>().Find((Image x) => x.name == name);
				name = slots[0].Open.CostText.name;
				component.Open.CostText = obj.GetComponentsInChildren<Text>().ToList<Text>().Find((Text x) => x.name == name);
				name = slots[0].Open.AreaText.name;
				component.Open.AreaText = obj.GetComponentsInChildren<Text>().ToList<Text>().Find((Text x) => x.name == name);
				name = slots[0].Open.AnimCTRL.name;
				component.Open.AnimCTRL = obj.GetComponentsInChildren<Animator>().ToList<Animator>().Find((Animator x) => x.name == name);
				component.Open.Init(component);
				name = slots[0].Title.name;
				component.Title = obj.GetComponentsInChildren<Text>().ToList<Text>().Find((Text x) => x.name == name);
				name = slots[0].CaretakingSection.name;
				component.CaretakingSection = obj.GetComponentsInChildren<Text>().ToList<Text>().Find((Text x) => x.name == name);
				component.listParent = listParent;
				name = slots[0].scrollExchanger.name;
				component.scrollExchanger = obj.GetComponentsInChildren<ScrollExchanger>().ToList<ScrollExchanger>().Find((ScrollExchanger x) => x.name == name);
				component.scrollExchanger.messageReceiver = component;
				component.scrollExchanger.MessageRecieveInit();/*
				if (component.ArrowRoot == null)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						"Panic1"
					});
				}
				if (component.Arrow_Up == null)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						"Panic2"
					});
				}
				if (component.Arrow_Down == null)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						"Panic3"
					});
				}
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					i + " : components"
				});
				foreach (Component comp in obj.GetComponents<Component>())
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						comp.name + " : " + comp.GetType().FullName
					});
				}
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					i + " : components in children"
				});
				foreach (Component comp in obj.GetComponentsInChildren<Component>())
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						comp.name + " : " + comp.GetType().FullName
					});
				}*/
			}
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
