/*
+many public static arrays
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200070B RID: 1803
public class EquipmentTypeInfo
{
	// Token: 0x06003991 RID: 14737 RVA: 0x00170B0C File Offset: 0x0016ED0C
	public EquipmentTypeInfo()
	{
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06003992 RID: 14738 RVA: 0x00170BD8 File Offset: 0x0016EDD8
	public string Name
	{
		get
		{
			string empty = string.Empty;
			this.GetLocalizedText("name", out empty);
			return empty;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06003993 RID: 14739 RVA: 0x00170BFC File Offset: 0x0016EDFC
	public string No
	{
		get
		{
			string empty = string.Empty;
			this.GetLocalizedText("no", out empty);
			return empty;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06003994 RID: 14740 RVA: 0x00170C20 File Offset: 0x0016EE20
	public string Description
	{
		get
		{
			string empty = string.Empty;
			this.GetLocalizedText("desc", out empty);
			return empty;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06003995 RID: 14741 RVA: 0x00170C44 File Offset: 0x0016EE44
	public string SpecialDesc
	{
		get
		{
			string empty = string.Empty;
			this.GetLocalizedText("specialDesc", out empty);
			return empty;
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06003996 RID: 14742 RVA: 0x00170C68 File Offset: 0x0016EE68
	public int MaxNum
	{ // <Mod>
		get
		{
			if (this.type != EquipmentTypeInfo.EquipmentType.SPECIAL && MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.GEBURAH) && this.id != 200038 && this.id != 300038 && this.id != 200015)
			{
				return Mathf.Min(5, this.maxNum + 1);
			}
			return this.maxNum;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06003997 RID: 14743 RVA: 0x00170CE8 File Offset: 0x0016EEE8
	public RiskLevel Grade
	{
		get
		{
			float num = 0f;
			if (float.TryParse(this.grade, out num))
			{
				return (RiskLevel)num - 1;
			}
			return RiskLevel.ZAYIN;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06003998 RID: 14744 RVA: 0x00033006 File Offset: 0x00031206
	public DamageInfo damageInfo
	{
		get
		{
			return this.damageInfos[0].Copy();
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06003999 RID: 14745 RVA: 0x00170D18 File Offset: 0x0016EF18
	public EGOgiftAttachRegion AttachRegion
	{
		get
		{
			EGOgiftAttachRegion result = EGOgiftAttachRegion.HEAD;
			if (!EGOGiftRegionKey.ParseRegion(this.attachPos, out result))
			{
				Debug.LogError("Could not found Region " + this.attachPos);
			}
			return result;
		}
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x00033015 File Offset: 0x00031215
	public static EquipmentTypeInfo GetDummyInfo()
	{
		return EquipmentTypeList.instance.GetData(1);
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x00170D50 File Offset: 0x0016EF50
	public bool GetLocalizedText(string region, out string output)
	{
		string empty = string.Empty;
		output = string.Empty;
		if (this.localizeData.TryGetValue(region, out empty))
		{
			string text = LocalizeTextDataModel.instance.GetText(empty);
			output = text;
			return true;
		}
		return false;
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x00033022 File Offset: 0x00031222
	public static EquipmentTypeInfo GetDummyArmorInfo()
	{
		return EquipmentTypeList.instance.GetData(22);
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x00033030 File Offset: 0x00031230
	public static EquipmentTypeInfo GetDummyGiftInfo()
	{
		return EquipmentTypeList.instance.GetData(300001);
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x00170D90 File Offset: 0x0016EF90
	public static EquipmentTypeInfo MakeWeaponInfoByDamageInfo(DamageInfo dmg)
	{
		return new EquipmentTypeInfo
		{
			type = EquipmentTypeInfo.EquipmentType.WEAPON,
			icon = "dummyIcon",
			defenseInfo = DefenseInfo.zero,
			range = 3f,
			damageInfos = new DamageInfo[]
			{
				dmg
			}
		};
	}

	// Token: 0x040034AB RID: 13483
	public int id;

	// Token: 0x040034AC RID: 13484
	public EquipmentTypeInfo.EquipmentType type;

	// Token: 0x040034AD RID: 13485
	public string icon;

	// Token: 0x040034AE RID: 13486
	public string sprite = string.Empty;

	// Token: 0x040034AF RID: 13487
	public int maxNum = 1;

	// Token: 0x040034B0 RID: 13488
	public DefenseInfo defenseInfo = DefenseInfo.zero;

	// Token: 0x040034B1 RID: 13489
	public List<EgoRequire> requires = new List<EgoRequire>();

	// Token: 0x040034B2 RID: 13490
	public string no;

	// Token: 0x040034B3 RID: 13491
	public string script = string.Empty;

	// Token: 0x040034B4 RID: 13492
	public string grade = "1";

	// Token: 0x040034B5 RID: 13493
	public int armorId;

	// Token: 0x040034B6 RID: 13494
	public WeaponClassType weaponClassType = WeaponClassType.AXE;

	// Token: 0x040034B7 RID: 13495
	public int weaponId;

	// Token: 0x040034B8 RID: 13496
	public string specialWeaponAnim = string.Empty;

	// Token: 0x040034B9 RID: 13497
	public string[] animationNames = new string[]
	{
		"test",
		"test2"
	};

	// Token: 0x040034BA RID: 13498
	public float range;

	// Token: 0x040034BB RID: 13499
	public DamageInfo[] damageInfos = new DamageInfo[]
	{
		DamageInfo.zero
	};

	// Token: 0x040034BC RID: 13500
	public SplashInfo splashInfo = new SplashInfo();

	// Token: 0x040034BD RID: 13501
	public Dictionary<string, string> localizeData = new Dictionary<string, string>();

	// Token: 0x040034BE RID: 13502
	public float attackSpeed = 1f;

	// Token: 0x040034BF RID: 13503
	public EGObonusInfo bonus = new EGObonusInfo();

	// Token: 0x040034C0 RID: 13504
	public string attachPos = string.Empty;

	// Token: 0x040034C1 RID: 13505
	public EGOgiftAttachType attachType;

	// Token: 0x0200070C RID: 1804
	public enum EquipmentType
	{
		// Token: 0x040034C3 RID: 13507
		WEAPON,
		// Token: 0x040034C4 RID: 13508
		ARMOR,
		// Token: 0x040034C5 RID: 13509
		SPECIAL
	}

    //> <Mod> added many arrays
	public static int[] BossIds = new int[] {
		100015,
		200015,
		300015,
		400015,
		100038,
		200038,
		300038,
		400038
	};

	public static float[] WeaponUpgrade = new float[] {
		1f,
		1.6f,
		1.4f,
		1.3f,
		1.25f,
		1.22f,
		1.2f,
	};

	public static float[] WeaponDowngrade = new float[] {
		1f,
		0.6f,
		0.4f,
		0.24f,
		0.16f,
		0.1f,
		0.06f
	};

	public static float[] ArmorUpgrade = new float[] {
		0f,
		-0.2f,
		-0.15f,
		-0.12f,
		-0.1f,
		-0.2f / 3f,
		-0.05f,
	};

	public static float[] ArmorDowngrade = new float[] {
		0f,
		0.1f,
		0.15f,
		0.2f,
		0.25f,
		0.3f,
		0.35f
	};

	public static int[] NonScaleWeaponIds = new int[] {
		200028,
		200020,
		200021,
		200052,
		200034,
		200059,
		200060,
		200057,
		200049,
		200048,
		200103,
		200045,
		200043,
		200032,
		200033,
		200004,
		200104,
		200061,
		200105,
		200047,
		200065,
		200042,
		200056,
		200058,
		200063,
		200064,
		200015,
		200038
	};

	public static int[] NonScaleJust = new int[] {
		20,
		35,
		60,
		80,
		100,
		115
	};
    //< <Mod>
}
