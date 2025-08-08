using System;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
public class UIColorManager : MonoBehaviour
{
	// Token: 0x0600521C RID: 21020 RVA: 0x0000429C File Offset: 0x0000249C
	public UIColorManager()
	{
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x0600521D RID: 21021 RVA: 0x00042D68 File Offset: 0x00040F68
	public static UIColorManager instance
	{
		get
		{
			if (UIColorManager._instance == null)
			{
				UIColorManager._instance = UnityEngine.Object.FindObjectOfType<UIColorManager>();
			}
			return UIColorManager._instance;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x0600521E RID: 21022 RVA: 0x00042D89 File Offset: 0x00040F89
	public static Color Orange
	{
		get
		{
			return UIColorManager.instance.orange;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x0600521F RID: 21023 RVA: 0x00042D95 File Offset: 0x00040F95
	public static Color UITextDefColor
	{
		get
		{
			return UIColorManager.instance.uiTextDefColor;
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06005220 RID: 21024 RVA: 0x00042DA1 File Offset: 0x00040FA1
	public static Color SefiraUnlockedColor
	{
		get
		{
			return UIColorManager.instance.sefiraUnlockedColor;
		}
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x00042DAD File Offset: 0x00040FAD
	private void Awake()
	{
		if (UIColorManager._instance != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		UIColorManager._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x001DA94C File Offset: 0x001D8B4C
	public SefiraUIColor GetSefiraColor(Sefira sefira)
	{
		SefiraUIColor result = null;
		foreach (SefiraUIColor sefiraUIColor in this.sefiraColor)
		{
			if (sefiraUIColor.sefira == sefira.sefiraEnum)
			{
				result = sefiraUIColor;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005223 RID: 21027 RVA: 0x001DA994 File Offset: 0x001D8B94
	public SefiraUIColor GetSefiraColor(string sefira)
	{
		Sefira sefira2 = SefiraManager.instance.GetSefira(sefira);
		if (sefira2 != null)
		{
			return this.GetSefiraColor(sefira2);
		}
		return null;
	}

	// Token: 0x06005224 RID: 21028 RVA: 0x001DA9BC File Offset: 0x001D8BBC
	public SefiraUIColor GetSefiraColor(SefiraEnum sefira)
	{
		Sefira sefira2 = SefiraManager.instance.GetSefira(sefira);
		if (sefira2 != null)
		{
			return this.GetSefiraColor(sefira2);
		}
		return null;
	}

	// Token: 0x06005225 RID: 21029 RVA: 0x00042DDC File Offset: 0x00040FDC
	public Color GetRWBPTypeColor(RwbpType type)
	{
		return this.RWBPTypeColor[(int)type];
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x00042DEF File Offset: 0x00040FEF
	public Color GetRWBPTextColor(RwbpType type)
	{
		return this.RWBPTextColor[(int)type];
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x00042E02 File Offset: 0x00041002
	public void GetRWBPTypeColor(RwbpType type, out Color inner, out Color outter)
	{
		inner = this.GetRWBPTextColor(type);
		outter = this.RWBPTextColor[0];
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x00042E28 File Offset: 0x00041028
	public Color GetRiskColor(RiskLevel level)
	{
		return this.RiskLevelColor[(int)level];
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001DA9E4 File Offset: 0x001D8BE4
	public Color GetDisabledRoomColor(SefiraEnum sefira)
	{
		Color result;
		try
		{
			Color color = this.DisabledColor[(int)sefira];
			result = color;
		}
		catch (IndexOutOfRangeException message)
		{
			Debug.LogError(message);
			result = UIColorManager.Orange;
		}
		return result;
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x00042E3B File Offset: 0x0004103B
	public Color GetOverloadColor(OverloadType type)
	{
		return this.OverloadColor[(int)type];
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x0000403D File Offset: 0x0000223D
	// Note: this type is marked as 'beforefieldinit'.
	static UIColorManager()
	{
	}

	// Token: 0x04004BD4 RID: 19412
	private static UIColorManager _instance;

	// Token: 0x04004BD5 RID: 19413
	public SefiraUIColor[] sefiraColor;

	// Token: 0x04004BD6 RID: 19414
	public Color uiTextDefColor;

	// Token: 0x04004BD7 RID: 19415
	public Color orange;

	// Token: 0x04004BD8 RID: 19416
	public Color sefiraUnlockedColor;

	// Token: 0x04004BD9 RID: 19417
	public Color Allocate_SefiraGreen;

	// Token: 0x04004BDA RID: 19418
	public Color Allocate_SefiraGray;

	// Token: 0x04004BDB RID: 19419
	public Color[] RWBPTypeColor;

	// Token: 0x04004BDC RID: 19420
	public Color[] RWBPTextColor;

	// Token: 0x04004BDD RID: 19421
	public Color UINormalColor;

	// Token: 0x04004BDE RID: 19422
	public Color UIBlueColor;

	// Token: 0x04004BDF RID: 19423
	public Color UIRedColor;

	// Token: 0x04004BE0 RID: 19424
	public Color UIDisabledColor;

	// Token: 0x04004BE1 RID: 19425
	public Color[] RiskLevelColor;

	// Token: 0x04004BE2 RID: 19426
	[Header("IsolateRoom Color")]
	public Color[] DisabledColor;

	// Token: 0x04004BE3 RID: 19427
	public Color EscapedColor;

	// Token: 0x04004BE4 RID: 19428
	public Color[] OverloadColor;

	// Token: 0x04004BE5 RID: 19429
	public CreatureLayer.IsolateRoomUI isolateRoomUI;
}
