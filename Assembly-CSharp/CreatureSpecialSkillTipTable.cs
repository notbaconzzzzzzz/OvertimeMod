using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000605 RID: 1541
[Serializable]
public class CreatureSpecialSkillTipTable
{
	// Token: 0x0600351B RID: 13595 RVA: 0x0003089D File Offset: 0x0002EA9D
	public CreatureSpecialSkillTipTable(long creatureTypeId)
	{ // <Patch>
		this.creatureTypeId = creatureTypeId;
		this.descList = new List<CreatureSpecialSkillDesc>();
		this.modid = string.Empty;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x00161B1C File Offset: 0x0015FD1C
	public void Init()
	{
		foreach (CreatureSpecialSkillDesc creatureSpecialSkillDesc in this.descList)
		{
			creatureSpecialSkillDesc.Reset();
		}
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x00161B78 File Offset: 0x0015FD78
	public Dictionary<string, object> GetSaveGlobalData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (CreatureSpecialSkillDesc creatureSpecialSkillDesc in this.descList)
		{
			dictionary.Add(creatureSpecialSkillDesc.index + string.Empty, creatureSpecialSkillDesc);
		}
		return dictionary;
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x00161BF0 File Offset: 0x0015FDF0
	public void LoadGlobalData(Dictionary<string, object> dic, int count)
	{
		for (int i = 0; i < count; i++)
		{
			CreatureSpecialSkillDesc item = null;
			GameUtil.TryGetValue<CreatureSpecialSkillDesc>(dic, i + string.Empty, ref item);
			this.descList.Add(item);
		}
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x00161C38 File Offset: 0x0015FE38
	public void ActivateDesc(CreatureModel model, string key, params object[] param)
	{
		CreatureSpecialSkillDesc desc = this.GetDesc(key);
		if (desc == null)
		{
			Debug.Log("Cannot find " + key);
			return;
		}
		desc.ActivateDesc(model, param);
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x00161C6C File Offset: 0x0015FE6C
	public CreatureSpecialSkillDesc GetDesc(string key)
	{
		CreatureSpecialSkillDesc creatureSpecialSkillDesc = null;
		foreach (CreatureSpecialSkillDesc creatureSpecialSkillDesc2 in this.descList)
		{
			if (creatureSpecialSkillDesc2.key == key)
			{
				creatureSpecialSkillDesc = creatureSpecialSkillDesc2;
				break;
			}
		}
		if (creatureSpecialSkillDesc == null)
		{
			Debug.LogError("Cannot find " + key);
			return null;
		}
		return creatureSpecialSkillDesc;
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x00161CF4 File Offset: 0x0015FEF4
	public void OnCreatureNameRevealed(CreatureModel model)
	{
		if (model.metaInfo.collectionName != model.metaInfo.name)
		{
			return;
		}
		foreach (CreatureSpecialSkillDesc creatureSpecialSkillDesc in this.descList)
		{
			creatureSpecialSkillDesc.desc = model.ConvertCodeIDToName(creatureSpecialSkillDesc.desc);
		}
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x00161D7C File Offset: 0x0015FF7C
	public CreatureSpecialSkillTipTable GetCopy()
	{
		CreatureSpecialSkillTipTable creatureSpecialSkillTipTable = new CreatureSpecialSkillTipTable(this.creatureTypeId);
		foreach (CreatureSpecialSkillDesc item in this.descList)
		{
			creatureSpecialSkillTipTable.descList.Add(item);
		}
		return creatureSpecialSkillTipTable;
	}

	// Token: 0x0400315A RID: 12634
	public List<CreatureSpecialSkillDesc> descList;

	// Token: 0x0400315B RID: 12635
	public long creatureTypeId;
	
	// <Patch>
	[NonSerialized]
	public string modid;
}
