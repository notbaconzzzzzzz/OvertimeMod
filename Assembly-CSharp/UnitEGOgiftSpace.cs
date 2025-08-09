/*
public void AttachGift(UnitModel owner, EGOgiftModel model) // Allow EGO gifts to stack even if you occupy the same slot
+Various new functions // 
+public static int GetStackingMax() // 
+public int GiftCount(EGOgiftAttachType type, string pos) // 
+public static arrays // 
*/
using System;
using System.Linq; //
using System.Collections.Generic;
using LobotomyBaseMod; // 
using UnityEngine;

// Token: 0x020006AA RID: 1706
public class UnitEGOgiftSpace
{
	// Token: 0x06003744 RID: 14148 RVA: 0x00031822 File Offset: 0x0002FA22
	public UnitEGOgiftSpace()
	{
		this.replacedGifts = new List<EGOgiftModel>();
		this.addedGifts = new List<EGOgiftModel>();
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x00031856 File Offset: 0x0002FA56
	public static int GetRegionId(EquipmentTypeInfo info)
	{
		return (int)info.attachType * 100 + (int)info.AttachRegion;
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x00164B04 File Offset: 0x00162D04
	public static string GetRegionName(int regionIndex)
	{
		string text = string.Empty;
		switch (regionIndex)
		{
		case 0:
			text = "Gift_Hat";
			goto IL_CB;
		case 1:
			text = "Gift_Eye";
			goto IL_CB;
		case 2:
			text = "Gift_Mouth";
			goto IL_CB;
		case 3:
			text = "Gift_Helmet";
			goto IL_CB;
		case 4:
			text = "Gift_RightHand";
			goto IL_CB;
		case 5:
			text = "Gift_Brooch";
			goto IL_CB;
		case 6:
			text = "Gift_Ribborn";
			goto IL_CB;
		case 7:
			text = "Gift_RightCheek";
			goto IL_CB;
		case 8:
			text = "Gift_Face";
			goto IL_CB;
		case 9:
			text = "Gift_BackRight";
			goto IL_CB;
		case 11:
			text = "Gift_BackLeft";
			goto IL_CB;
		}
		if (regionIndex != 102)
		{
			if (regionIndex != 104)
			{
				if (regionIndex != 210)
				{
					text = string.Empty;
				}
				else
				{
					text = "Gift_HeadBack";
				}
			}
			else
			{
				text = "Gift_RightHand_replace";
			}
		}
		else
		{
			text = "Gift_Mouth_replace";
		}
		IL_CB:
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		return LocalizeTextDataModel.instance.GetText(text);
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x00031868 File Offset: 0x0002FA68
	public static string GetRegionName(EquipmentTypeInfo info)
	{
		return UnitEGOgiftSpace.GetRegionName(UnitEGOgiftSpace.GetRegionId(info));
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x00164BF8 File Offset: 0x00162DF8
	public static bool IsUniqueLock(long id)
	{
		for (int i = 0; i < UnitEGOgiftSpace.uniqueLock.Length; i++)
		{
			if (id == UnitEGOgiftSpace.uniqueLock[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x00164C30 File Offset: 0x00162E30
	public Dictionary<string, object> GetSaveData()
	{ // <Patch>
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			list.Add(egogiftModel.metaInfo.id);
			list2.Add(EquipmentTypeInfo.GetLcId(egogiftModel.metaInfo).packageId);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			list.Add(egogiftModel2.metaInfo.id);
			list2.Add(EquipmentTypeInfo.GetLcId(egogiftModel2.metaInfo).packageId);
		}
		Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
		foreach (KeyValuePair<int, UnitEGOgiftSpace.GiftLockState> keyValuePair in this.lockState)
		{
			dictionary2[keyValuePair.Key] = keyValuePair.Value.modid;
		}
		dictionary.Add("giftTypeIdList", list);
		dictionary.Add("giftTypeIdListMod", list2);
		dictionary.Add("lockState", this.lockState);
		dictionary.Add("lockStateMod", dictionary2);
		dictionary.Add("displayState", this.displayState);
		return dictionary;
		/*
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<int> list = new List<int>();
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			list.Add(egogiftModel.metaInfo.id);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			list.Add(egogiftModel2.metaInfo.id);
		}
		dictionary.Add("giftTypeIdList", list);
		dictionary.Add("lockState", this.lockState);
		dictionary.Add("displayState", this.displayState);
		return dictionary;*/
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x00164D2C File Offset: 0x00162F2C
	public void LoadDataAndAttach(UnitModel owner, Dictionary<string, object> dic)
	{ // <Patch>
		new Dictionary<string, object>();
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		GameUtil.TryGetValue<List<int>>(dic, "giftTypeIdList", ref list);
		bool flag = GameUtil.TryGetValue<List<string>>(dic, "giftTypeIdListMod", ref list2);
		try
		{
			GameUtil.TryGetValue<Dictionary<int, UnitEGOgiftSpace.GiftLockState>>(dic, "lockState", ref this.lockState);
			GameUtil.TryGetValue<Dictionary<int, bool>>(dic, "displayState", ref this.displayState);
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			GameUtil.TryGetValue<Dictionary<int, string>>(dic, "lockStateMod", ref dictionary);
			foreach (KeyValuePair<int, UnitEGOgiftSpace.GiftLockState> keyValuePair in this.lockState)
			{
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					keyValuePair.Value.modid = dictionary[keyValuePair.Key];
				}
				else
				{
					keyValuePair.Value.modid = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			this.lockState.Clear();
			this.displayState.Clear();
		}
		int num = 0;
		foreach (int id in list)
		{
			if (flag)
			{
				EquipmentTypeInfo data_Mod = EquipmentTypeList.instance.GetData_Mod(new LobotomyBaseMod.LcId(list2[num], id));
				if (data_Mod != null)
				{
					if (data_Mod.type != EquipmentTypeInfo.EquipmentType.SPECIAL)
					{
						LobotomyBaseMod.ModDebug.Log("id : " + id.ToString() + " is not gift");
					}
					else
					{
						EGOgiftModel gift = EGOgiftModel.MakeGift(data_Mod);
						owner.AttachEGOgift(gift);
					}
				}
			}
			else
			{
				EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(id);
				if (data != null)
				{
					if (data.type != EquipmentTypeInfo.EquipmentType.SPECIAL)
					{
						LobotomyBaseMod.ModDebug.Log("id : " + id.ToString() + " is not gift");
					}
					else
					{
						EGOgiftModel gift2 = EGOgiftModel.MakeGift(data);
						owner.AttachEGOgift(gift2);
					}
				}
			}
			num++;
		}
		/*
		new Dictionary<string, object>();
		List<int> list = new List<int>();
		GameUtil.TryGetValue<List<int>>(dic, "giftTypeIdList", ref list);
		try
		{
			GameUtil.TryGetValue<Dictionary<int, UnitEGOgiftSpace.GiftLockState>>(dic, "lockState", ref this.lockState);
			GameUtil.TryGetValue<Dictionary<int, bool>>(dic, "displayState", ref this.displayState);
		}
		catch (Exception)
		{
			this.lockState.Clear();
			this.displayState.Clear();
		}
		foreach (int num in list)
		{
			EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(num);
			if (data != null)
			{
				if (data.type != EquipmentTypeInfo.EquipmentType.SPECIAL)
				{
					Debug.LogError("id : " + num + " is not gift");
				}
				else
				{
					EGOgiftModel gift = EGOgiftModel.MakeGift(data);
					owner.AttachEGOgift(gift);
				}
			}
		}*/
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x00164E1C File Offset: 0x0016301C
	public void AttachGift(UnitModel owner, EGOgiftModel model)
	{ // <Patch> <Mod> allow EGO gifts to stack even if you occupy the same slot, but not if they're exclusive to each other
		int stackingMax = GetStackingMax();
        if (stackingMax != 1)
        {
			LcId[] exclude = null;
            foreach (LcId[] group in exclusiveGifts)
            {
                if (group.Contains(EquipmentTypeInfo.GetLcId(model.metaInfo)))
				{
					exclude = group;
					break;
				}
            }
			if (exclude != null)
			{
				EGOgiftModel egogiftModel = (model.metaInfo.attachType == EGOgiftAttachType.REPLACE ? this.replacedGifts : this.addedGifts).Find((EGOgiftModel x) => exclude.Contains(EquipmentTypeInfo.GetLcId(x.metaInfo)));
				if (egogiftModel != null)
				{
					egogiftModel.OnRelease();
					(egogiftModel.metaInfo.attachType == EGOgiftAttachType.REPLACE ? this.replacedGifts : this.addedGifts).Remove(egogiftModel);
					Notice.instance.Send(NoticeName.OnChangeGift, new object[]
					{
						owner
					});
				}
			}
        }
		if (model.metaInfo.attachType == EGOgiftAttachType.ADD)
		{
			if (stackingMax != -1)
			{
				List<EGOgiftModel> stackedGifts = addedGifts.FindAll((EGOgiftModel x) => x.metaInfo.attachType == EGOgiftAttachType.ADD && x.metaInfo.attachPos == model.metaInfo.attachPos);
				if (stackedGifts.Count >= stackingMax)
				{
					EGOgiftModel egogiftModel = stackedGifts[0];
                    egogiftModel.OnRelease();
                    addedGifts.Remove(egogiftModel);
					Notice.instance.Send(NoticeName.OnChangeGift, new object[]
					{
						owner
					});
				}
			}
			this.addedGifts.Add(model);
			model.OnEquip(owner);
		}
		else if (model.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD)
		{
			if (stackingMax != -1)
			{
				List<EGOgiftModel> stackedGifts = addedGifts.FindAll((EGOgiftModel x) => x.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD && x.metaInfo.attachPos == model.metaInfo.attachPos);
				if (stackedGifts.Count >= stackingMax)
				{
					EGOgiftModel egogiftModel = stackedGifts[0];
                    egogiftModel.OnRelease();
                    addedGifts.Remove(egogiftModel);
					Notice.instance.Send(NoticeName.OnChangeGift, new object[]
					{
						owner
					});
				}
			}
			this.addedGifts.Add(model);
			model.OnEquip(owner);
		}
		else if (model.metaInfo.attachType == EGOgiftAttachType.REPLACE)
		{
			if (stackingMax != -1)
			{
				List<EGOgiftModel> stackedGifts = replacedGifts.FindAll((EGOgiftModel x) => x.metaInfo.attachType == EGOgiftAttachType.REPLACE && x.metaInfo.attachPos == model.metaInfo.attachPos);
				if (stackedGifts.Count >= stackingMax)
				{
					EGOgiftModel egogiftModel = stackedGifts[0];
                    egogiftModel.OnRelease();
                    replacedGifts.Remove(egogiftModel);
					Notice.instance.Send(NoticeName.OnChangeGift, new object[]
					{
						owner
					});
				}
			}
			this.replacedGifts.Add(model);
			model.OnEquip(owner);
		}
		if (!this.displayState.ContainsKey(UnitEGOgiftSpace.GetRegionId(model.metaInfo)))
		{
			this.displayState.Add(UnitEGOgiftSpace.GetRegionId(model.metaInfo), true);
		}
		if (!this.lockState.ContainsKey(UnitEGOgiftSpace.GetRegionId(model.metaInfo)))
		{
			this.lockState.Add(UnitEGOgiftSpace.GetRegionId(model.metaInfo), new UnitEGOgiftSpace.GiftLockState
			{
				id = (long)model.metaInfo.id,
				state = false
			});
			this.lockState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)].modid = EquipmentTypeInfo.GetLcId(model.metaInfo).packageId; // 
		}
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x00165058 File Offset: 0x00163258
	public void SetDisplayState(EGOgiftModel model, bool state)
	{
		if (this.displayState.ContainsKey(UnitEGOgiftSpace.GetRegionId(model.metaInfo)))
		{
			this.displayState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)] = state;
		}
		else
		{
			this.displayState.Add(UnitEGOgiftSpace.GetRegionId(model.metaInfo), state);
		}
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x001650B4 File Offset: 0x001632B4
	public bool GetDisplayState(EGOgiftModel model)
	{
		if (this.displayState.ContainsKey(UnitEGOgiftSpace.GetRegionId(model.metaInfo)))
		{
			return this.displayState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)];
		}
		this.displayState.Add(UnitEGOgiftSpace.GetRegionId(model.metaInfo), true);
		return true;
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x0016510C File Offset: 0x0016330C
	public void SetLockState(EGOgiftModel model, bool state)
	{ // <Patch>
		if (this.lockState.ContainsKey(UnitEGOgiftSpace.GetRegionId(model.metaInfo)))
		{
			UnitEGOgiftSpace.GiftLockState giftLockState = this.lockState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)];
			giftLockState.id = (long)model.metaInfo.id;
			giftLockState.modid = EquipmentTypeInfo.GetLcId(model.metaInfo).packageId; // 
			giftLockState.state = state;
			this.lockState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)] = giftLockState;
		}
		else
		{
			this.lockState.Add(UnitEGOgiftSpace.GetRegionId(model.metaInfo), new UnitEGOgiftSpace.GiftLockState
			{
				id = (long)model.metaInfo.id,
				state = false
			});
			this.lockState[UnitEGOgiftSpace.GetRegionId(model.metaInfo)].modid = EquipmentTypeInfo.GetLcId(model.metaInfo).packageId; // 
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x001651B8 File Offset: 0x001633B8
	public bool GetLockState(EquipmentTypeInfo info)
	{ // <Patch>
		if (this.lockState.ContainsKey(UnitEGOgiftSpace.GetRegionId(info)))
		{
			UnitEGOgiftSpace.GiftLockState giftLockState = this.lockState[UnitEGOgiftSpace.GetRegionId(info)];
			return EquipmentTypeInfo.GetLcId(info) != UnitEGOgiftSpace.GiftLockState.GetLcId(giftLockState) && giftLockState.state; // return (long)info.id != giftLockState.id && giftLockState.state;
		}
		this.lockState.Add(UnitEGOgiftSpace.GetRegionId(info), new UnitEGOgiftSpace.GiftLockState
		{
			id = (long)info.id,
			state = false
		});
		this.lockState[UnitEGOgiftSpace.GetRegionId(info)].modid = EquipmentTypeInfo.GetLcId(info).packageId; // 
		return false;
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x00165238 File Offset: 0x00163438
	public bool GetLockStateUI(EquipmentTypeInfo info)
	{ // <Patch>
		if (this.lockState.ContainsKey(UnitEGOgiftSpace.GetRegionId(info)))
		{
			return this.lockState[UnitEGOgiftSpace.GetRegionId(info)].state;
		}
		this.lockState.Add(UnitEGOgiftSpace.GetRegionId(info), new UnitEGOgiftSpace.GiftLockState
		{
			id = (long)info.id,
			state = false
		});
		this.lockState[UnitEGOgiftSpace.GetRegionId(info)].modid = EquipmentTypeInfo.GetLcId(info).packageId;
		return false;
		/*
		if (this.lockState.ContainsKey(UnitEGOgiftSpace.GetRegionId(info)))
		{
			return this.lockState[UnitEGOgiftSpace.GetRegionId(info)].state;
		}
		this.lockState.Add(UnitEGOgiftSpace.GetRegionId(info), new UnitEGOgiftSpace.GiftLockState
		{
			id = (long)info.id,
			state = false
		});
		return false;*/
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00031875 File Offset: 0x0002FA75
	public void ReleaseGift(EGOgiftModel gift)
	{ // <Patch>
		this.displayState.Remove(UnitEGOgiftSpace.GetRegionId(gift.metaInfo)); // this.displayState.Remove(gift.metaInfo.id);
		this.lockState.Remove(UnitEGOgiftSpace.GetRegionId(gift.metaInfo));
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x001652A0 File Offset: 0x001634A0
	public EGObonusInfo GetBonus(UnitModel actor)
	{
		EGObonusInfo egobonusInfo = new EGObonusInfo();
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			egobonusInfo += egogiftModel.GetBonus(actor);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			egobonusInfo += egogiftModel2.GetBonus(actor);
		}
		return egobonusInfo;
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x00165360 File Offset: 0x00163560
	public float GetDamageFactor()
	{
		float num = 1f;
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			num *= egogiftModel.GetDamageFactor();
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			num *= egogiftModel2.GetDamageFactor();
		}
		return num;
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x00165414 File Offset: 0x00163614
	public float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		float num = 0f;
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			num += egogiftModel.GetWorkProbSpecialBonus(actor, skill);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			num += egogiftModel2.GetWorkProbSpecialBonus(actor, skill);
		}
		return num;
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x001654CC File Offset: 0x001636CC
	public void OnFixedUpdate()
	{
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			egogiftModel.script.OnFixedUpdate();
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			egogiftModel2.script.OnFixedUpdate();
		}
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x0016557C File Offset: 0x0016377C
	public void OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			egogiftModel.script.OnTakeDamage(actor, ref dmg);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			egogiftModel2.script.OnTakeDamage(actor, ref dmg);
		}
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x00165634 File Offset: 0x00163834
	public void OnTakeDamage_After(float value, RwbpType type)
	{
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			egogiftModel.script.OnTakeDamage_After(value, type);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			egogiftModel2.script.OnTakeDamage_After(value, type);
		}
	}

	// <Mod>
	public void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
	{
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			egogiftModel.script.OnTakeDamage_After(actor, dmg);
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			egogiftModel2.script.OnTakeDamage_After(actor, dmg);
		}
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x001656EC File Offset: 0x001638EC
	public bool HasEquipment(int id)
	{ // <Patch>
		return HasEquipment_Mod(new LobotomyBaseMod.LcId(id));
		/*
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			if (egogiftModel.metaInfo.id == id)
			{
				return true;
			}
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			if (egogiftModel2.metaInfo.id == id)
			{
				return true;
			}
		}
		return false;*/
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x001657BC File Offset: 0x001639BC
	public int CountGifts()
	{
		int count = this.replacedGifts.Count;
		int count2 = this.addedGifts.Count;
		return count + count2;
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x000318A5 File Offset: 0x0002FAA5
	static UnitEGOgiftSpace()
	{
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x001657E8 File Offset: 0x001639E8
	public void OwnerHeal(bool isMental, ref float amount)
	{
		foreach (EGOgiftModel egogiftModel in this.addedGifts)
		{
			if (egogiftModel.script != null)
			{
				egogiftModel.script.OwnerHeal(isMental, ref amount);
			}
		}
		foreach (EGOgiftModel egogiftModel2 in this.replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				egogiftModel2.script.OwnerHeal(isMental, ref amount);
			}
		}
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x0016589C File Offset: 0x00163A9C
	public bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		bool result = true;
		foreach (EGOgiftModel egogiftModel in this.addedGifts)
		{
			if (egogiftModel.script != null && !egogiftModel.script.OnGiveDamage(actor, target, ref dmg))
			{
				result = false;
			}
		}
		foreach (EGOgiftModel egogiftModel2 in this.replacedGifts)
		{
			if (egogiftModel2.script != null && !egogiftModel2.script.OnGiveDamage(actor, target, ref dmg))
			{
				result = false;
			}
		}
		return result;
	}

	// <Mod>
	public void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
		foreach (EGOgiftModel egogiftModel in this.addedGifts)
		{
			if (egogiftModel.script != null) egogiftModel.script.OnGiveDamageAfter(actor, target, dmg);
		}
		foreach (EGOgiftModel egogiftModel2 in this.replacedGifts)
		{
			if (egogiftModel2.script != null) egogiftModel2.script.OnGiveDamageAfter(actor, target, dmg);
		}
	}

	// <Patch>
	public bool HasEquipment_Mod(LobotomyBaseMod.LcId id)
	{
		foreach (EGOgiftModel egogiftModel in this.replacedGifts)
		{
			if (EquipmentTypeInfo.GetLcId(egogiftModel.metaInfo) == id)
			{
				return true;
			}
		}
		foreach (EGOgiftModel egogiftModel2 in this.addedGifts)
		{
			if (EquipmentTypeInfo.GetLcId(egogiftModel2.metaInfo) == id)
			{
				return true;
			}
		}
		return false;
	}

	// <Mod>
	public float RecoveryMultiplier(bool isMental, float amount)
	{
		float num = 1f;
		foreach (EGOgiftModel egogiftModel in addedGifts)
		{
			if (egogiftModel.script != null)
			{
				num *= egogiftModel.script.RecoveryMultiplier(isMental, amount);
			}
		}
		foreach (EGOgiftModel egogiftModel2 in replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				num *= egogiftModel2.script.RecoveryMultiplier(isMental, amount);
			}
		}
		return num;
	}

	// <Mod>
	public float RecoveryAdditiveMultiplier(bool isMental, float amount)
	{
		float num = 0f;
		foreach (EGOgiftModel egogiftModel in addedGifts)
		{
			if (egogiftModel.script != null)
			{
				num += egogiftModel.script.RecoveryAdditiveMultiplier(isMental, amount);
			}
		}
		foreach (EGOgiftModel egogiftModel2 in replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				num += egogiftModel2.script.RecoveryAdditiveMultiplier(isMental, amount);
			}
		}
		return num;
	}

	// <Mod>
	public float WorkSpeedModifier(CreatureModel target, SkillTypeInfo skill)
	{
		float num = 1f;
		foreach (EGOgiftModel egogiftModel in addedGifts)
		{
			if (egogiftModel.script != null)
			{
				num *= egogiftModel.script.WorkSpeedModifier(target, skill);
			}
		}
		foreach (EGOgiftModel egogiftModel2 in replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				num *= egogiftModel2.script.WorkSpeedModifier(target, skill);
			}
		}
		return num;
	}

	// <Mod>
	public Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
	{
		Vector2 num = new Vector2(0f, 0f);
		foreach (EGOgiftModel egogiftModel in addedGifts)
		{
			if (egogiftModel.script != null)
			{
				num += egogiftModel.script.PercentageRecoverOnHit(actor, dmg);
			}
		}
		foreach (EGOgiftModel egogiftModel2 in replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				num += egogiftModel2.script.PercentageRecoverOnHit(actor, dmg);
			}
		}
		return num;
	}

	// <Mod>
	public void OnStageStart()
	{
		foreach (EGOgiftModel egogiftModel in addedGifts)
		{
			if (egogiftModel.script != null)
			{
				egogiftModel.script.OnStageStart();
			}
		}
		foreach (EGOgiftModel egogiftModel2 in replacedGifts)
		{
			if (egogiftModel2.script != null)
			{
				egogiftModel2.script.OnStageStart();
			}
		}
	}

	// <Mod>
	public static int GetStackingMax()
	{
		if (SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions"))
		{
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.BINAH))
			{
				return -1;
			}
			if (ResearchDataModel.instance.IsUpgradedAbility("gift_stacking"))
			{
				return 2;
			}
			return 1;
		}
		if (SpecialModeConfig.instance.GetValue<bool>("StackableEgoGifts"))
		{
			return -1;
		}
		return 1;
	}
	

	// <Mod>
	public int GiftCount(EGOgiftAttachType type, string pos)
	{
		List<EGOgiftModel> stackedGifts = addedGifts.FindAll((EGOgiftModel x) => x.metaInfo.attachType == type && x.metaInfo.attachPos == pos);
		stackedGifts.AddRange(replacedGifts.FindAll((EGOgiftModel x) => x.metaInfo.attachType == type && x.metaInfo.attachPos == pos));
		return stackedGifts.Count;
	}

	// Token: 0x040032BF RID: 12991
	public static long[] uniqueLock = new long[] // <Mod> made not readonly
	{
		4000371L,
		4000372L,
		4000373L,
		4000374L,
		1021L,
		1022L
	};

    // <Mod> Groups of EGO gifts that aren't allowed to co-exist on the same employee
	public static LcId[][] exclusiveGifts = new LcId[][]
	{
		new LcId[] {
            new LcId(4000371),
            new LcId(4000372),
            new LcId(4000373),
            new LcId(4000374)
        }
	};

	// Token: 0x040032C0 RID: 12992
	public List<EGOgiftModel> replacedGifts;

	// Token: 0x040032C1 RID: 12993
	public List<EGOgiftModel> addedGifts;

	// Token: 0x040032C2 RID: 12994
	public Dictionary<int, bool> displayState = new Dictionary<int, bool>();

	// Token: 0x040032C3 RID: 12995
	public Dictionary<int, UnitEGOgiftSpace.GiftLockState> lockState = new Dictionary<int, UnitEGOgiftSpace.GiftLockState>();

	// Token: 0x020006AB RID: 1707
	[Serializable]
	public class GiftLockState
	{
		// Token: 0x0600375D RID: 14173 RVA: 0x00003E08 File Offset: 0x00002008
		public GiftLockState()
		{
		}

		// <Patch>
		public static LobotomyBaseMod.LcId GetLcId(UnitEGOgiftSpace.GiftLockState LockState)
		{
			if (LockState.modid == null)
			{
				return new LobotomyBaseMod.LcId((int)LockState.id);
			}
			return new LobotomyBaseMod.LcId(LockState.modid, (int)LockState.id);
		}

		// Token: 0x040032C4 RID: 12996
		public long id;

		// Token: 0x040032C5 RID: 12997
		public bool state;

		// <Patch>
		[NonSerialized]
		public string modid;
	}
}
