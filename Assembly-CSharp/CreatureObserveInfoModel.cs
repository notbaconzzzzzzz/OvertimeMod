using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000603 RID: 1539
public class CreatureObserveInfoModel
{
	// Token: 0x06003507 RID: 13575 RVA: 0x00030838 File Offset: 0x0002EA38
	public CreatureObserveInfoModel(long creatureTypeId)
	{
		this.creatureTypeId = creatureTypeId;
		this._metaInfo = CreatureTypeList.instance.GetData(creatureTypeId);
		this.InitData();
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x0016142C File Offset: 0x0015F62C
	public void InitData()
	{
		try
		{
			this.InitObserveRegion(CreatureTypeList.instance.GetData(this.creatureTypeId).observeData);
		}
		catch (Exception arg)
		{
			Debug.LogError(string.Format("{0}\n{1}", this.creatureTypeId, arg));
		}
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x0016148C File Offset: 0x0015F68C
	public void InitObserveRegion(List<ObserveInfoData> data)
	{
		this.observeRegions.Clear();
		foreach (ObserveInfoData info in data)
		{
			ObserveRegion observeRegion = new ObserveRegion
			{
				info = info,
				isObserved = false
			};
			this.observeRegions.Add(observeRegion.info.regionName, observeRegion);
		}
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x00161514 File Offset: 0x0015F714
	public int GetObservationLevel()
	{
		if (this._metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			int num = 0;
			if (this.GetObserveState(CreatureModel.regionName[0]))
			{
				num++;
			}
			if (this.GetObserveState(CreatureModel.regionName[1]))
			{
				num++;
			}
			bool flag = true;
			for (int i = 2; i <= 5; i++)
			{
				if (!this.GetObserveState(CreatureModel.regionName[i]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num++;
			}
			bool flag2 = true;
			foreach (string key in CreatureModel.careTakingRegion)
			{
				ObserveRegion observeRegion = null;
				if (this.GetObserveRegion(key, out observeRegion) && !this.GetObserveState(key))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				num++;
			}
			return num;
		}
		if (this._metaInfo.creatureWorkType == CreatureWorkType.KIT)
		{
			int result = 0;
			int num2;
			if (this._metaInfo.creatureKitType == CreatureKitType.ONESHOT)
			{
				num2 = this.totalKitUseCount;
			}
			else
			{
				num2 = (int)this.totalKitUseTime;
			}
			for (int k = 0; k < CreatureModel.careTakingRegion.Length; k++)
			{
				ObserveRegion observeRegion2 = null;
				if (this.GetObserveRegion(CreatureModel.careTakingRegion[k], out observeRegion2) && observeRegion2.info.observeCost <= num2)
				{
					result = k + 1;
				}
			}
			return result;
		}
		return 0;
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x00030869 File Offset: 0x0002EA69
	public bool GetObserveRegion(string key, out ObserveRegion output)
	{
		return this.observeRegions.TryGetValue(key, out output);
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x00161680 File Offset: 0x0015F880
	public bool GetObserveState(string key)
	{
		ObserveRegion observeRegion = null;
		return !(key == string.Empty) && this.GetObserveRegion(key, out observeRegion) && observeRegion.isObserved;
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x001616B8 File Offset: 0x0015F8B8
	public int GetObserveCost(string key)
	{
		ObserveRegion observeRegion = null;
		if (key == string.Empty)
		{
			return -1;
		}
		if (this.GetObserveRegion(key, out observeRegion))
		{
			return observeRegion.info.observeCost;
		}
		return -1;
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x001616F4 File Offset: 0x0015F8F4
	private void AddObserveRegionData(Dictionary<string, object> dic)
	{
		foreach (ObserveRegion observeRegion in this.observeRegions.Values)
		{
			dic.Add(observeRegion.info.regionName, observeRegion.isObserved);
		}
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x0016176C File Offset: 0x0015F96C
	private void GetObserveRegionData(Dictionary<string, object> dic)
	{
		foreach (ObserveRegion observeRegion in this.observeRegions.Values)
		{
			GameUtil.TryGetValue<bool>(dic, observeRegion.info.regionName, ref observeRegion.isObserved);
		}
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x00030869 File Offset: 0x0002EA69
	public bool GetRegion(string key, out ObserveRegion region)
	{
		return this.observeRegions.TryGetValue(key, out region);
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x001617E0 File Offset: 0x0015F9E0
	public void OnObserveRegion(string region)
	{
		ObserveRegion observeRegion = null;
		if (this.GetRegion(region, out observeRegion))
		{
			observeRegion.isObserved = true;
		}
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x00161804 File Offset: 0x0015FA04
	public void ObserveAll(params string[] ignore)
	{
		List<string> list = new List<string>(ignore);
		foreach (string text in this.observeRegions.Keys)
		{
			if (!list.Contains(text))
			{
				this.OnObserveRegion(text);
			}
		}
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x00161878 File Offset: 0x0015FA78
	public void OnResetObserve()
	{
		foreach (ObserveRegion observeRegion in this.observeRegions.Values)
		{
			observeRegion.isObserved = false;
		}
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x001618DC File Offset: 0x0015FADC
	public Dictionary<string, object> GetSaveGlobalData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("observeProgress", this.observeProgress);
		dictionary.Add("cubeNum", this.cubeNum);
		dictionary.Add("totalKitUseCount", this.totalKitUseCount);
		dictionary.Add("totalKitUseTime", this.totalKitUseTime);
		this.AddObserveRegionData(dictionary);
		return dictionary;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x00161950 File Offset: 0x0015FB50
	public void LoadGlobalData(Dictionary<string, object> dic)
	{
		GameUtil.TryGetValue<int>(dic, "observeProgress", ref this.observeProgress);
		GameUtil.TryGetValue<int>(dic, "cubeNum", ref this.cubeNum);
		GameUtil.TryGetValue<int>(dic, "totalKitUseCount", ref this.totalKitUseCount);
		GameUtil.TryGetValue<float>(dic, "totalKitUseTime", ref this.totalKitUseTime);
		this.GetObserveRegionData(dic);
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x001619AC File Offset: 0x0015FBAC
	public bool IsMaxObserved()
	{
		if (this._metaInfo.creatureWorkType == CreatureWorkType.KIT)
		{
			if (this._metaInfo.specialSkillTable == null)
			{
				this._metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(this._metaInfo.id).GetCopy();
			}
			int num = Mathf.Max(this._metaInfo.specialSkillTable.descList.Count, this._metaInfo.desc.Count);
			num = Mathf.Min(num, CreatureModel.careTakingRegion.Length);
			return this.GetObservationLevel() >= num;
		}
		return this.GetObservationLevel() >= 4;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x00030878 File Offset: 0x0002EA78
	public void Transaction(int trans)
	{
		this.cubeNum += trans;
	}

	// Token: 0x0400314C RID: 12620
	private CreatureTypeInfo _metaInfo;

	// Token: 0x0400314D RID: 12621
	public long creatureTypeId;

	// Token: 0x0400314E RID: 12622
	public int observeProgress;

	// Token: 0x0400314F RID: 12623
	public int cubeNum;

	// Token: 0x04003150 RID: 12624
	public int totalKitUseCount;

	// Token: 0x04003151 RID: 12625
	public float totalKitUseTime;

	// Token: 0x04003152 RID: 12626
	private Dictionary<string, ObserveRegion> observeRegions = new Dictionary<string, ObserveRegion>();
}
