using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CreatureInfo
{
	// Token: 0x02000AE3 RID: 2787
	public class CreatureInfoKitLayoutController : MonoBehaviour
	{
		// Token: 0x06005452 RID: 21586 RVA: 0x00044611 File Offset: 0x00042811
		public CreatureInfoKitLayoutController()
		{
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x001EA164 File Offset: 0x001E8364
		public void SetDataList(CreatureModel creature)
		{
			this.Clear();
			int num = Mathf.Max(creature.metaInfo.specialSkillTable.descList.Count, creature.metaInfo.desc.Count);
			num = Mathf.Min(num, CreatureModel.careTakingRegion.Length);
			string collectionName = creature.metaInfo.collectionName;
			int i = 0;
			while (i < num)
			{
				string text = string.Empty;
				if (i < creature.metaInfo.desc.Count)
				{
					text = creature.metaInfo.desc[i];
				}
				string text2 = string.Empty;
				if (i < creature.metaInfo.specialSkillTable.descList.Count)
				{
					text2 = creature.metaInfo.specialSkillTable.descList[i].original;
				}
				string text3 = text;
				int num2 = 0;
				for (;;)
				{
					int num3 = text3.IndexOf("#" + num2);
					if (text3 == string.Empty || text3 == null)
					{
						break;
					}
					if (num3 == -1)
					{
						break;
					}
					num2++;
				}
				IL_119:
				string text4 = text2;
				int num4 = 0;
				for (;;)
				{
					int num5 = text4.IndexOf("#" + num4);
					if (text4 == string.Empty || text4 == null)
					{
						break;
					}
					if (num5 == -1)
					{
						break;
					}
					num4++;
				}
				IL_177:
				text4 = text4.Replace("$0", collectionName);
				CreatureInfoKitDataSlot creatureInfoKitDataSlot = this.MakeEmptySlot();
				creatureInfoKitDataSlot.Left_RecordText.text = text3;
				creatureInfoKitDataSlot.Right_UseText.text = text4;
				int observeCost = creature.observeInfo.GetObserveCost(CreatureModel.careTakingRegion[i]);
				if (creature.metaInfo.creatureKitType == CreatureKitType.ONESHOT)
				{
					if (creature.observeInfo.totalKitUseCount < observeCost)
					{
						creatureInfoKitDataSlot.disabled.SetActive(true);
						creatureInfoKitDataSlot.needCostText.text = LocalizeTextDataModel.instance.GetText("CreatureInfoKit_NeedUseCount") + "\n" + observeCost.ToString();
					}
					else
					{
						creatureInfoKitDataSlot.disabled.SetActive(false);
					}
				}
				else
				{
					int num6 = (int)creature.observeInfo.totalKitUseTime;
					if (num6 < observeCost)
					{
						creatureInfoKitDataSlot.disabled.SetActive(true);
						creatureInfoKitDataSlot.needCostText.text = LocalizeTextDataModel.instance.GetText("CreatureInfoKit_NeedUseTime") + "\n" + string.Format("{0}:{1:D2}", observeCost / 60, observeCost % 60);
					}
					else
					{
						creatureInfoKitDataSlot.disabled.SetActive(false);
					}
				}
				i++;
				continue;
				goto IL_177;
				goto IL_119;
			}
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x001EA42C File Offset: 0x001E862C
		public void SetDataList(CreatureTypeInfo metaInfo, CreatureObserveInfoModel observeInfo)
		{ // <Patch>
			this.Clear();
			if (metaInfo.specialSkillTable == null)
			{
				metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(CreatureTypeInfo.GetLcId(metaInfo)).GetCopy();
			}
			int num = Mathf.Max(metaInfo.specialSkillTable.descList.Count, metaInfo.desc.Count);
			num = Mathf.Min(num, CreatureModel.careTakingRegion.Length);
			string collectionName = metaInfo.collectionName;
			int i = 0;
			while (i < num)
			{
				string text = string.Empty;
				if (i < metaInfo.desc.Count)
				{
					text = metaInfo.desc[i];
				}
				string text2 = string.Empty;
				if (i < metaInfo.specialSkillTable.descList.Count)
				{
					text2 = metaInfo.specialSkillTable.descList[i].original;
				}
				string text3 = text;
				int num2 = 0;
				for (;;)
				{
					int num3 = text3.IndexOf("#" + num2);
					if (text3 == string.Empty || text3 == null)
					{
						break;
					}
					if (num3 == -1)
					{
						break;
					}
					num2++;
				}
				IL_11E:
				string text4 = text2;
				int num4 = 0;
				for (;;)
				{
					int num5 = text4.IndexOf("#" + num4);
					if (text4 == string.Empty || text4 == null)
					{
						break;
					}
					if (num5 == -1)
					{
						break;
					}
					num4++;
				}
				IL_179:
				text4 = text4.Replace("$0", collectionName);
				CreatureInfoKitDataSlot creatureInfoKitDataSlot = this.MakeEmptySlot();
				creatureInfoKitDataSlot.Left_RecordText.text = text3;
				creatureInfoKitDataSlot.Right_UseText.text = text4;
				int observeCost = observeInfo.GetObserveCost(CreatureModel.careTakingRegion[i]);
				if (metaInfo.creatureKitType == CreatureKitType.ONESHOT)
				{
					if (observeInfo.totalKitUseCount < observeCost)
					{
						creatureInfoKitDataSlot.disabled.SetActive(true);
						creatureInfoKitDataSlot.needCostText.text = LocalizeTextDataModel.instance.GetText("CreatureInfoKit_NeedUseCount") + "\n" + observeCost.ToString();
					}
					else
					{
						creatureInfoKitDataSlot.disabled.SetActive(false);
					}
				}
				else
				{
					int num6 = (int)observeInfo.totalKitUseTime;
					if (num6 < observeCost)
					{
						creatureInfoKitDataSlot.disabled.SetActive(true);
						creatureInfoKitDataSlot.needCostText.text = LocalizeTextDataModel.instance.GetText("CreatureInfoKit_NeedUseTime") + "\n" + string.Format("{0}:{1:D2}", observeCost / 60, observeCost % 60);
					}
					else
					{
						creatureInfoKitDataSlot.disabled.SetActive(false);
					}
				}
				i++;
				continue;
				goto IL_179;
				goto IL_11E;
			}
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x001EA6E4 File Offset: 0x001E88E4
		public CreatureInfoKitDataSlot MakeEmptySlot()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.slot);
			gameObject.transform.SetParent(this.scrollParent, false);
			gameObject.transform.localScale = Vector3.one;
			float num = -((float)this.currentLastIndex * this.SlotDefaultY + (float)this.currentLastIndex * this.Spacing);
			if (this.currentLastIndex == 0)
			{
				num = 0f;
			}
			this.scrollParent.sizeDelta = new Vector2(this.scrollParent.sizeDelta.x, -num);
			gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, num, 0f);
			this.currentLastIndex++;
			CreatureInfoKitDataSlot component = gameObject.GetComponent<CreatureInfoKitDataSlot>();
			component.SetRect(this.scrollReference);
			return component;
		}

		// Token: 0x06005456 RID: 21590 RVA: 0x001EA7B4 File Offset: 0x001E89B4
		public void Clear()
		{
			this.currentLastIndex = 0;
			this.scrollParent.sizeDelta = new Vector2(this.scrollParent.sizeDelta.x, 0f);
			IEnumerator enumerator = this.scrollParent.GetEnumerator();
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
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x0004462F File Offset: 0x0004282F
		public void DebugMake()
		{
			this.MakeEmptySlot();
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x000043CD File Offset: 0x000025CD
		private void Start()
		{
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x000043CD File Offset: 0x000025CD
		private void Update()
		{
		}

		// Token: 0x04004DA2 RID: 19874
		public GameObject slot;

		// Token: 0x04004DA3 RID: 19875
		public float Spacing = 16.7f;

		// Token: 0x04004DA4 RID: 19876
		public float SlotDefaultY = 113f;

		// Token: 0x04004DA5 RID: 19877
		public RectTransform scrollReference;

		// Token: 0x04004DA6 RID: 19878
		public RectTransform scrollParent;

		// Token: 0x04004DA7 RID: 19879
		public ScrollRect scrollRect;

		// Token: 0x04004DA8 RID: 19880
		public GameObject UpperArrow;

		// Token: 0x04004DA9 RID: 19881
		public GameObject LowerArrow;

		// Token: 0x04004DAA RID: 19882
		private int currentLastIndex;
	}
}
