using System;
using System.Collections;
using UnityEngine;

namespace Inventory
{
	// Token: 0x020009D4 RID: 2516
	[Serializable]
	public class InventoryRequireLayout
	{
		// Token: 0x06004C20 RID: 19488 RVA: 0x00003FB0 File Offset: 0x000021B0
		public InventoryRequireLayout()
		{
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x001C1110 File Offset: 0x001BF310
		public void Init(EquipmentTypeInfo info)
		{ // <Mod> shorten stat type text to 4 letters
			IEnumerator enumerator = this.parent.GetEnumerator();
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
			if (info.requires.Count == 0)
			{
				string text = LocalizeTextDataModel.instance.GetText("Inventory_NoRequire");
				string empty = string.Empty;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.unit);
				InventoryRequireUnit component = gameObject.GetComponent<InventoryRequireUnit>();
				gameObject.transform.SetParent(this.parent);
				gameObject.transform.localScale = Vector3.one;
				if (info.id == 300034 || info.id == 200034)
				{
					text = LocalizeTextDataModel.instance.GetText("Bald");
					component.SetText(text);
				}
				else
				{
					component.SetText(text, empty, 30);
				}
				return;
			}
			foreach (EgoRequire egoRequire in info.requires)
			{
				string statType = string.Empty;
				string grade = string.Empty;
				try
				{
					int gradeFontSize = 30;
					statType = LocalizeTextDataModel.instance.GetText(InventoryRequireLayout.statName[(int)egoRequire.type]);
					if (egoRequire.value >= 6)
					{
						grade = "EX";
						gradeFontSize = 22;
					}
					else
					{
						grade = AgentModel.GetLevelGradeText(egoRequire.value);
					}
                    if (statType.Length > 5)
                    {
                        statType = statType.Substring(0, 4);
                    }
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.unit);
					InventoryRequireUnit component2 = gameObject2.GetComponent<InventoryRequireUnit>();
					gameObject2.transform.SetParent(this.parent);
					gameObject2.transform.localScale = Vector3.one;
					component2.SetText(statType, grade, gradeFontSize);
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0003EB33 File Offset: 0x0003CD33
		// Note: this type is marked as 'beforefieldinit'.
		static InventoryRequireLayout()
		{
		}

		// Token: 0x04004699 RID: 18073
		private static string[] statName = new string[]
		{
			"Inventory_Level",
			"Rstat",
			"Wstat",
			"Bstat",
			"Pstat"
		};

		// Token: 0x0400469A RID: 18074
		public GameObject ActiveControl;

		// Token: 0x0400469B RID: 18075
		public RectTransform parent;

		// Token: 0x0400469C RID: 18076
		public GameObject unit;
	}
}
