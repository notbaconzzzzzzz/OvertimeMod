using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009B9 RID: 2489
public class IconManager : MonoBehaviour
{
	// Token: 0x06004B77 RID: 19319 RVA: 0x00004300 File Offset: 0x00002500
	public IconManager()
	{
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06004B78 RID: 19320 RVA: 0x0003EB08 File Offset: 0x0003CD08
	public static IconManager instance
	{
		get
		{
			return IconManager._instance;
		}
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001BD248 File Offset: 0x001BB448
	public void Awake()
	{
		IconManager._instance = this;
		foreach (IconManager.WorkIcon workIcon in this.workIcon)
		{
			workIcon.Init();
		}
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x000040A1 File Offset: 0x000022A1
	public void Start()
	{
	}

	// Token: 0x06004B7B RID: 19323 RVA: 0x001BD2AC File Offset: 0x001BB4AC
	public IconManager.Icon GetIcon(string name)
	{
		IconManager.Icon result = null;
		foreach (IconManager.Icon icon in this.list)
		{
			if (icon.name == name)
			{
				result = icon;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x001BD31C File Offset: 0x001BB51C
	public IconManager.Icon GetIcon(int id)
	{
		IconManager.Icon result = null;
		foreach (IconManager.Icon icon in this.list)
		{
			if (icon.id == id)
			{
				result = icon;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x001BD388 File Offset: 0x001BB588
	public IconManager.WorkIcon GetWorkIcon(int id)
	{
		IconManager.WorkIcon result = null;
		foreach (IconManager.WorkIcon workIcon in this.workIcon)
		{
			if (workIcon.id == id)
			{
				result = workIcon;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x0003EB0F File Offset: 0x0003CD0F
	public Sprite GetDefenseIcon(DefenseInfo.Type type)
	{
		return this.DefenseIcon[(int)type];
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x001BD3F4 File Offset: 0x001BB5F4
	public Sprite GetOrdealIcon(OrdealLevel level)
	{
		Sprite result;
		try
		{
			result = this.OrdealIcon[(int)level];
		}
		catch (IndexOutOfRangeException ex)
		{
			result = this.OrdealIcon[0];
		}
		return result;
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x000040A1 File Offset: 0x000022A1
	// Note: this type is marked as 'beforefieldinit'.
	static IconManager()
	{
	}

	// Token: 0x040045DE RID: 17886
	private static IconManager _instance;

	// Token: 0x040045DF RID: 17887
	public List<IconManager.Icon> list;

	// Token: 0x040045E0 RID: 17888
	public List<IconManager.WorkIcon> workIcon;

	// Token: 0x040045E1 RID: 17889
	public Sprite[] DamageIcon;

	// Token: 0x040045E2 RID: 17890
	public Sprite[] DefenseIcon;

	// Token: 0x040045E3 RID: 17891
	public Sprite[] OrdealIcon;

	// Token: 0x020009BA RID: 2490
	[Serializable]
	public class Icon
	{
		// Token: 0x06004B81 RID: 19329 RVA: 0x00004074 File Offset: 0x00002274
		public Icon()
		{
		}

		// Token: 0x040045E4 RID: 17892
		public string name;

		// Token: 0x040045E5 RID: 17893
		public int id;

		// Token: 0x040045E6 RID: 17894
		public Sprite icon;
	}

	// Token: 0x020009BB RID: 2491
	[Serializable]
	public class WorkIcon
	{
		// Token: 0x06004B82 RID: 19330 RVA: 0x00004074 File Offset: 0x00002274
		public WorkIcon()
		{
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x001BD430 File Offset: 0x001BB630
		public void Init()
		{
			foreach (Sprite icon in this.sprites)
			{
				IconManager.Icon icon2 = new IconManager.Icon();
				icon2.icon = icon;
				icon2.id = this.id;
				icon2.name = this.name + this.id.ToString();
				this.icons.Add(icon2);
			}
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x001BD4CC File Offset: 0x001BB6CC
		public IconManager.Icon GetIcon(ManageWorkIconState state)
		{
			int num = (int)state;
			if (num >= this.sprites.Count)
			{
				num = this.defaultIndex;
			}
			return this.icons[num];
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x0003EB19 File Offset: 0x0003CD19
		public IconManager.Icon GetDefault()
		{
			return this.icons[this.defaultIndex];
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x0003EB2C File Offset: 0x0003CD2C
		public IconManager.Icon GetCurrent()
		{
			return this.icons[this.currnetIndex];
		}

		// Token: 0x040045E7 RID: 17895
		public string name;

		// Token: 0x040045E8 RID: 17896
		public List<Sprite> sprites;

		// Token: 0x040045E9 RID: 17897
		[HideInInspector]
		public List<IconManager.Icon> icons;

		// Token: 0x040045EA RID: 17898
		public int id;

		// Token: 0x040045EB RID: 17899
		public int defaultIndex;

		// Token: 0x040045EC RID: 17900
		public int currnetIndex;
	}
}
