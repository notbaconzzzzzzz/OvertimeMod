using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x0200069D RID: 1693
public class InventoryModel
{
	// Token: 0x06003769 RID: 14185 RVA: 0x000320D5 File Offset: 0x000302D5
	public InventoryModel()
	{
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600376A RID: 14186 RVA: 0x000320F0 File Offset: 0x000302F0
	public static InventoryModel Instance
	{
		get
		{
			if (InventoryModel._instance == null)
			{
				InventoryModel._instance = new InventoryModel();
			}
			return InventoryModel._instance;
		}
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x00032108 File Offset: 0x00030308
	public void Init()
	{
		this._equipList = new List<EquipmentModel>();
		this._nextInstanceId = 1L;
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x0016B070 File Offset: 0x00169270
	public Dictionary<string, object> GetGlobalSaveData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<InventoryModel.EquipmentSaveData> list = new List<InventoryModel.EquipmentSaveData>();
		foreach (EquipmentModel equipmentModel in this._equipList)
		{
			list.Add(new InventoryModel.EquipmentSaveData
			{
				equipTypeId = equipmentModel.metaInfo.id,
				equipInstanceId = equipmentModel.instanceId
			});
		}
		dictionary.Add("equips", list);
		dictionary.Add("nextInstanceId", this._nextInstanceId);
		return dictionary;
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x0016B114 File Offset: 0x00169314
	public void LoadGlobalData(Dictionary<string, object> dic)
	{
		try
		{
			this._equipList.Clear();
			List<InventoryModel.EquipmentSaveData> list = new List<InventoryModel.EquipmentSaveData>();
			GameUtil.TryGetValue<List<InventoryModel.EquipmentSaveData>>(dic, "equips", ref list);
			GameUtil.TryGetValue<long>(dic, "nextInstanceId", ref this._nextInstanceId);
			this._nextInstanceId += 1L;
			foreach (InventoryModel.EquipmentSaveData equipmentSaveData in list)
			{
				int equipTypeId = equipmentSaveData.equipTypeId;
				if (EquipmentTypeList.instance.GetData(equipTypeId) != null)
				{
					this.CreateEquipment(equipTypeId, equipmentSaveData.equipInstanceId);
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x0600376E RID: 14190 RVA: 0x0016B1FC File Offset: 0x001693FC
	public void OnReleaseGame()
	{
		foreach (EquipmentModel equipmentModel in this._equipList)
		{
			if (equipmentModel.owner != null)
			{
				equipmentModel.OnRelease();
			}
		}
	}

	// Token: 0x0600376F RID: 14191 RVA: 0x0003211D File Offset: 0x0003031D
	public EquipmentModel CreateEquipment(int id)
	{
		EquipmentModel equipmentModel = this.CreateEquipment(id, this._nextInstanceId);
		if (equipmentModel != null)
		{
			this._nextInstanceId += 1L;
		}
		return equipmentModel;
	}

	// Token: 0x06003770 RID: 14192 RVA: 0x0016B258 File Offset: 0x00169458
	public EquipmentModel CreateEquipmentForcely(int id)
	{
		EquipmentModel result;
		try
		{
			EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(id);
			this._equipList.FindAll((EquipmentModel x) => x.metaInfo.id == id);
			EquipmentModel equipmentModel = null;
			EquipmentTypeInfo.EquipmentType type = data.type;
			if (type != EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				if (type != EquipmentTypeInfo.EquipmentType.WEAPON)
				{
					equipmentModel = new EGOgiftModel();
				}
				else
				{
					equipmentModel = new WeaponModel();
				}
			}
			else
			{
				equipmentModel = new ArmorModel();
			}
			equipmentModel.instanceId = this._nextInstanceId;
			equipmentModel.metaInfo = data;
			object obj = null;
			try
			{
				foreach (Assembly assembly in Add_On.instance.AssemList)
				{
					foreach (Type type2 in assembly.GetTypes())
					{
						if (type2.Name == data.script)
						{
							obj = Activator.CreateInstance(type2);
						}
					}
				}
				if (obj == null)
				{
					obj = Activator.CreateInstance(Type.GetType(data.script));
				}
			}
			catch (ArgumentNullException)
			{
				obj = Activator.CreateInstance(Type.GetType("EquipmentScriptBase"));
			}
			if (obj is EquipmentScriptBase)
			{
				equipmentModel.script = (EquipmentScriptBase)obj;
				equipmentModel.script.SetModel(equipmentModel);
			}
			this._equipList.Add(equipmentModel);
			Notice.instance.Send(NoticeName.MakeEquipment, new object[]
			{
				equipmentModel
			});
			if (equipmentModel != null)
			{
				this._nextInstanceId += 1L;
			}
			result = equipmentModel;
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/error.txt", ex.Message);
			result = null;
		}
		return result;
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x0003213E File Offset: 0x0003033E
	public void RemoveEquipment(EquipmentModel model)
	{
		if (this._equipList.Contains(model))
		{
			this._equipList.Remove(model);
			Notice.instance.Send(NoticeName.RemoveEquipment, new object[]
			{
				model
			});
		}
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x0016B444 File Offset: 0x00169644
	public bool GetEquipCount(int id, out int current, out int max)
	{
		try
		{
			EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(id);
			List<EquipmentModel> list = this._equipList.FindAll((EquipmentModel x) => x.metaInfo.id == id);
			current = list.Count;
			max = data.MaxNum;
		}
		catch (Exception)
		{
			current = 0;
			max = 0;
			return false;
		}
		return true;
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x0016B4B8 File Offset: 0x001696B8
	public bool CheckEquipmentCount(int id)
	{
		EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(id);
		return this._equipList.FindAll((EquipmentModel x) => x.metaInfo.id == id).Count < data.MaxNum;
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x0016B508 File Offset: 0x00169708
	public EquipmentModel CreateEquipment(int id, long instanceId)
	{
		EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(id);
		if (this.equipList.FindAll((EquipmentModel x) => x.metaInfo.id == id).Count >= data.MaxNum)
		{
			return null;
		}
		EquipmentModel equipmentModel = null;
		EquipmentTypeInfo.EquipmentType type = data.type;
		if (type != EquipmentTypeInfo.EquipmentType.ARMOR)
		{
			if (type != EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				equipmentModel = new EGOgiftModel();
			}
			else
			{
				equipmentModel = new WeaponModel();
			}
		}
		else
		{
			equipmentModel = new ArmorModel();
		}
		equipmentModel.instanceId = instanceId;
		equipmentModel.metaInfo = data;
		object obj = null;
		foreach (Assembly assembly in Add_On.instance.AssemList)
		{
			foreach (Type type2 in assembly.GetTypes())
			{
				if (type2.Name == data.script)
				{
					obj = Activator.CreateInstance(type2);
				}
			}
		}
		if (obj == null)
		{
			try
			{
				obj = Activator.CreateInstance(Type.GetType(data.script));
			}
			catch (ArgumentNullException)
			{
				obj = Activator.CreateInstance(Type.GetType("EquipmentScriptBase"));
			}
		}
		if (obj is EquipmentScriptBase)
		{
			equipmentModel.script = (EquipmentScriptBase)obj;
			equipmentModel.script.SetModel(equipmentModel);
		}
		this.equipList.Add(equipmentModel);
		Notice.instance.Send(NoticeName.MakeEquipment, new object[]
		{
			equipmentModel
		});
		return equipmentModel;
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x00032174 File Offset: 0x00030374
	public IList<EquipmentModel> GetAllEquipmentList()
	{
		return this._equipList.AsReadOnly();
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x00032181 File Offset: 0x00030381
	public IList<EquipmentModel> GetWaitingEquipmentList()
	{
		return this._equipList.FindAll((EquipmentModel x) => x.owner == null).AsReadOnly();
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0016B694 File Offset: 0x00169894
	public EquipmentModel GetEquipment(long instanceId)
	{
		return this._equipList.Find((EquipmentModel x) => x.instanceId == instanceId);
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0016B6C8 File Offset: 0x001698C8
	public bool RemoveAllDlcEquipment()
	{
		bool result = false;
		foreach (long id in CreatureGenerateInfo.creditCreatures)
		{
			CreatureTypeInfo data = CreatureTypeList.instance.GetData(id);
			if (data != null)
			{
				using (List<CreatureEquipmentMakeInfo>.Enumerator enumerator = data.equipMakeInfos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CreatureEquipmentMakeInfo makeInfo = enumerator.Current;
						if (this._equipList.RemoveAll((EquipmentModel x) => x.metaInfo.id == makeInfo.equipTypeInfo.id) > 0)
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0016B76C File Offset: 0x0016996C
	public Dictionary<int, List<EquipmentModel>> GetEquipmentListByTypeInfo()
	{
		Dictionary<int, List<EquipmentModel>> dictionary = new Dictionary<int, List<EquipmentModel>>();
		foreach (EquipmentModel equipmentModel in this._equipList)
		{
			int id = equipmentModel.metaInfo.id;
			List<EquipmentModel> list = null;
			if (dictionary.TryGetValue(id, out list))
			{
				if (!list.Contains(equipmentModel))
				{
					list.Add(equipmentModel);
				}
			}
			else
			{
				list = new List<EquipmentModel>
				{
					equipmentModel
				};
				dictionary.Add(id, list);
			}
		}
		return dictionary;
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x000043CD File Offset: 0x000025CD
	static InventoryModel()
	{
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x0600377B RID: 14203 RVA: 0x000321B2 File Offset: 0x000303B2
	public List<EquipmentModel> equipList
	{
		get
		{
			return this._equipList;
		}
	}

	// Token: 0x040032EB RID: 13035
	private static InventoryModel _instance;

	// Token: 0x040032EC RID: 13036
	private List<EquipmentModel> _equipList = new List<EquipmentModel>();

	// Token: 0x040032ED RID: 13037
	public long _nextInstanceId = 1L;

	// Token: 0x0200069E RID: 1694
	[Serializable]
	public class EquipmentSaveData
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x000043A0 File Offset: 0x000025A0
		public EquipmentSaveData()
		{
		}

		// Token: 0x040032EE RID: 13038
		public int equipTypeId;

		// Token: 0x040032EF RID: 13039
		public long equipInstanceId;
	}
}
