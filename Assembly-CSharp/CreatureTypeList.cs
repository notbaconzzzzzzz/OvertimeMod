using System;
using System.Collections.Generic;
using LobotomyBaseMod; // 
using CreatureGenerate;

// Token: 0x0200070D RID: 1805
public class CreatureTypeList
{
	// Token: 0x060039EA RID: 14826 RVA: 0x00033BD7 File Offset: 0x00031DD7
	private CreatureTypeList()
	{
		this._list = new List<CreatureTypeInfo>();
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060039EB RID: 14827 RVA: 0x00033BEA File Offset: 0x00031DEA
	public static CreatureTypeList instance
	{
		get
		{
			if (CreatureTypeList._instance == null)
			{
				CreatureTypeList._instance = new CreatureTypeList();
			}
			return CreatureTypeList._instance;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x060039EC RID: 14828 RVA: 0x00033C05 File Offset: 0x00031E05
	public bool loaded
	{
		get
		{
			return this._loaded;
		}
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x00033C0D File Offset: 0x00031E0D
	public void Init(CreatureTypeInfo[] list, CreatureSpecialSkillTipTable[] table, Dictionary<long, int> specialTipSize)
	{
		this._list = new List<CreatureTypeInfo>(list);
		this._tableList = new List<CreatureSpecialSkillTipTable>(table);
		this._specialTipSizeDic = specialTipSize;
		this._loaded = true;
		CreatureGenerateInfoManager.Instance.Init();
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x00033C3F File Offset: 0x00031E3F
	public CreatureSpecialSkillTipTable[] GetTable()
	{
		return this._tableList.ToArray();
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x00033C4C File Offset: 0x00031E4C
	public CreatureTypeInfo[] GetList()
	{
		return this._list.ToArray();
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x00175850 File Offset: 0x00173A50
	public CreatureTypeInfo GetData(long id)
	{
		foreach (CreatureTypeInfo creatureTypeInfo in this._list)
		{
			if (creatureTypeInfo.id == id)
			{
				return creatureTypeInfo;
			}
		}
		return null;
	}

	// Token: 0x060039F1 RID: 14833 RVA: 0x001758BC File Offset: 0x00173ABC
	public CreatureSpecialSkillTipTable GetSkillTipData(long id)
	{
		foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable in this._tableList)
		{
			if (creatureSpecialSkillTipTable.creatureTypeId == id)
			{
				return creatureSpecialSkillTipTable;
			}
		}
		return null;
	}

	// Token: 0x060039F2 RID: 14834 RVA: 0x00175928 File Offset: 0x00173B28
	public void ResetSkillTipTable()
	{
		foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable in this._tableList)
		{
			creatureSpecialSkillTipTable.Init();
		}
	}

	// Token: 0x060039F3 RID: 14835 RVA: 0x00175984 File Offset: 0x00173B84
	public int GetSpecialSkillSize(long id)
	{
		int result = 0;
		if (this._specialTipSizeDic.TryGetValue(id, out result))
		{
			return result;
		}
		return 0;
	}

    // <Patch>
    public void Init_Mod(Dictionary<string, List<CreatureTypeInfo>> CTIdic, Dictionary<string, List<CreatureSpecialSkillTipTable>> CSSTTdic)
    {
        this._modlist = CTIdic;
        this._modtableList = CSSTTdic;
    }

    // <Patch>
    public CreatureTypeInfo GetData_Mod(LobotomyBaseMod.LcIdLong lcid)
    {
        bool flag = lcid.packageId == string.Empty;
        CreatureTypeInfo result;
        if (flag)
        {
            result = CreatureTypeList.instance.GetData(lcid.id);
        }
        else
        {
            bool flag2 = this._modlist.ContainsKey(lcid.packageId);
            if (flag2)
            {
                CreatureTypeInfo creatureTypeInfo = this._modlist[lcid.packageId].Find((CreatureTypeInfo x) => x.id == lcid.id);
                result = creatureTypeInfo;
            }
            else
            {
                result = null;
            }
        }
        return result;
    }

    // <Patch>
    public CreatureSpecialSkillTipTable GetSkillTipData_Mod(LobotomyBaseMod.LcIdLong lcid)
    {
        bool flag = lcid.packageId == string.Empty;
        CreatureSpecialSkillTipTable result;
        if (flag)
        {
            result = CreatureTypeList.instance.GetSkillTipData(lcid.id);
        }
        else
        {
            bool flag2 = this._modtableList.ContainsKey(lcid.packageId);
            if (flag2)
            {
                CreatureSpecialSkillTipTable creatureSpecialSkillTipTable = this._modtableList[lcid.packageId].Find((CreatureSpecialSkillTipTable x) => x.creatureTypeId == lcid.id);
                result = creatureSpecialSkillTipTable;
            }
            else
            {
                result = null;
            }
        }
        return result;
    }

    // <Patch>
    public LobotomyBaseMod.LcIdLong GetLcId(CreatureTypeInfo info)
    {
        bool flag = info is ChildCreatureTypeInfo;
        if (flag)
        {
            Predicate<CreatureTypeInfo> pred = null;
            foreach (KeyValuePair<string, List<CreatureTypeInfo>> keyValuePair in this._modlist)
            {
                List<CreatureTypeInfo> value = keyValuePair.Value;
                Predicate<CreatureTypeInfo> match;
                if ((match = pred) == null)
                {
                    match = (pred = ((CreatureTypeInfo x) => x.childTypeInfo == info));
                }
                CreatureTypeInfo creatureTypeInfo = value.Find(match);
                bool flag2 = creatureTypeInfo != null;
                if (flag2)
                {
                    return new LobotomyBaseMod.LcIdLong(keyValuePair.Key, info.id);
                }
            }
        }
        foreach (KeyValuePair<string, List<CreatureTypeInfo>> keyValuePair2 in this._modlist)
        {
            bool flag3 = keyValuePair2.Value.Contains(info);
            if (flag3)
            {
                return new LobotomyBaseMod.LcIdLong(keyValuePair2.Key, info.id);
            }
        }
        return new LobotomyBaseMod.LcIdLong(info.id);
    }

    // <Patch>
    public LobotomyBaseMod.LcIdLong GetLcId(CreatureSpecialSkillTipTable info)
    {
        return new LobotomyBaseMod.LcIdLong(info.modid, info.creatureTypeId);
    }

    // <Patch>
    public string GetModId(CreatureTypeInfo info)
    {
        bool flag = info is ChildCreatureTypeInfo;
        if (flag)
        {
            Predicate<CreatureTypeInfo> pred = null;
            foreach (KeyValuePair<string, List<CreatureTypeInfo>> keyValuePair in this._modlist)
            {
                List<CreatureTypeInfo> value = keyValuePair.Value;
                Predicate<CreatureTypeInfo> match;
                if ((match = pred) == null)
                {
                    match = (pred = ((CreatureTypeInfo x) => x.childTypeInfo == info));
                }
                CreatureTypeInfo creatureTypeInfo = value.Find(match);
                bool flag2 = creatureTypeInfo != null;
                if (flag2)
                {
                    return keyValuePair.Key;
                }
            }
        }
        foreach (KeyValuePair<string, List<CreatureTypeInfo>> keyValuePair2 in this._modlist)
        {
            bool flag3 = keyValuePair2.Value.Contains(info);
            if (flag3)
            {
                return keyValuePair2.Key;
            }
        }
        return string.Empty;
    }

    // <Patch>
    public string GetModId(CreatureSpecialSkillTipTable info)
    {
        return info.modid;
    }

	// Token: 0x040034D9 RID: 13529
	private static CreatureTypeList _instance;

	// Token: 0x040034DA RID: 13530
	private List<CreatureTypeInfo> _list;

	// Token: 0x040034DB RID: 13531
	private List<CreatureSpecialSkillTipTable> _tableList;

	// Token: 0x040034DC RID: 13532
	private Dictionary<long, int> _specialTipSizeDic;

	// Token: 0x040034DD RID: 13533
	private bool _loaded;

    // <Patch>
    public Dictionary<string, List<CreatureTypeInfo>> _modlist;

    // <Patch>
    public Dictionary<string, List<CreatureSpecialSkillTipTable>> _modtableList;
}
