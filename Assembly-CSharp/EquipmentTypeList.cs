using System;
using System.Collections.Generic;

// Token: 0x0200071A RID: 1818
public class EquipmentTypeList
{
	// Token: 0x06003A22 RID: 14882 RVA: 0x00033D75 File Offset: 0x00031F75
	private EquipmentTypeList()
	{
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003A23 RID: 14883 RVA: 0x00033D88 File Offset: 0x00031F88
	public static EquipmentTypeList instance
	{
		get
		{
			if (EquipmentTypeList._instance == null)
			{
				EquipmentTypeList._instance = new EquipmentTypeList();
			}
			return EquipmentTypeList._instance;
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06003A24 RID: 14884 RVA: 0x00033DA3 File Offset: 0x00031FA3
	public bool loaded
	{
		get
		{
			return this._loaded;
		}
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x00033DAB File Offset: 0x00031FAB
	public void Init(Dictionary<int, EquipmentTypeInfo> info)
	{
		this._dic = info;
		this._loaded = true;
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x0017671C File Offset: 0x0017491C
	public EquipmentTypeInfo GetData(int id)
	{
		EquipmentTypeInfo result = null;
		this._dic.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x0017673C File Offset: 0x0017493C
	public List<EquipmentTypeInfo> GetAllData()
	{
		List<EquipmentTypeInfo> list = new List<EquipmentTypeInfo>();
		foreach (KeyValuePair<int, EquipmentTypeInfo> keyValuePair in this._dic)
		{
			list.Add(keyValuePair.Value);
		}
		return list;
	}

    // <Patch>
    public void Init_Mod(Dictionary<string, Dictionary<int, EquipmentTypeInfo>> dic)
    {
        this.moddic = dic;
    }

    // <Patch>
    public EquipmentTypeInfo GetData_Mod(LobotomyBaseMod.LcId id)
    {
        if (id.packageId == string.Empty)
        {
            LobotomyBaseMod.ModDebug.Log("none packageId");
            return EquipmentTypeList.instance.GetData(id.id);
        }
        if (this.moddic.ContainsKey(id.packageId))
        {
            EquipmentTypeInfo equipmentTypeInfo = null;
            this.moddic[id.packageId].TryGetValue(id.id, out equipmentTypeInfo);
            return equipmentTypeInfo;
        }
        return null;
    }

    // <Patch>
    public string GetModId(EquipmentTypeInfo equip)
    {
        return equip.modid;
    }

	// Token: 0x0400353A RID: 13626
	private static EquipmentTypeList _instance;

	// Token: 0x0400353B RID: 13627
	private Dictionary<int, EquipmentTypeInfo> _dic = new Dictionary<int, EquipmentTypeInfo>();

	// Token: 0x0400353C RID: 13628
	private bool _loaded;

    // <Patch>
    public Dictionary<string, Dictionary<int, EquipmentTypeInfo>> moddic;
}
