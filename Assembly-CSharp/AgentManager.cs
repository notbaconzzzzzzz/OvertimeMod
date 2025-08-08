/*
public AgentModel AddAgentModelCustom(AgentData genData) // Title Specification
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Customizing;
using UnityEngine;

// Token: 0x02000BB4 RID: 2996
public class AgentManager : IObserver, ISerializablePlayData
{
	// Token: 0x06005ACB RID: 23243 RVA: 0x00048BD6 File Offset: 0x00046DD6
	public AgentManager()
	{
		this.Init();
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06005ACC RID: 23244 RVA: 0x00048BE4 File Offset: 0x00046DE4
	public static AgentManager instance
	{
		get
		{
			if (AgentManager._instance == null)
			{
				AgentManager._instance = new AgentManager();
			}
			return AgentManager._instance;
		}
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06005ACD RID: 23245 RVA: 0x00048BFF File Offset: 0x00046DFF
	private string DeletedAgentData
	{
		get
		{
			return Application.persistentDataPath + "/agentData/170808.dat";
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06005ACE RID: 23246 RVA: 0x00048C10 File Offset: 0x00046E10
	private string CustomAgentData
	{
		get
		{
			return Application.persistentDataPath + "/agentData/170808custom.dat";
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06005ACF RID: 23247 RVA: 0x00048C21 File Offset: 0x00046E21
	private string AgentDataSrc
	{
		get
		{
			return Application.persistentDataPath + "/agentData";
		}
	}

	// Token: 0x06005AD0 RID: 23248 RVA: 0x00048C32 File Offset: 0x00046E32
	private void InitValues()
	{
		this.nextInstId = 1;
		this.agentList = new List<AgentModel>();
		this.agentListSpare = new List<AgentModel>();
		this.deletedAgent = new List<AgentManager.PermanetlyDeletedAgent>();
		this.customAgent = new List<AgentManager.CustomizedAgentData>();
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x00048C67 File Offset: 0x00046E67
	public void Init()
	{
		this.InitValues();
		Notice.instance.Observe(NoticeName.ChangeAgentSefira, this);
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x00048C7F File Offset: 0x00046E7F
	public void Clear()
	{
		this.InitValues();
		Notice.instance.Send(NoticeName.ClearAgent, new object[0]);
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x00209148 File Offset: 0x00207348
	public AgentModel AddSpareAgentModel()
	{
		while (this.DeletedContain((long)this.nextInstId))
		{
			this.nextInstId++;
		}
		AgentModel agentModel = new AgentModel((long)this.nextInstId++);
		agentModel._agentName = AgentManager.GetRandomAgentName();
		agentModel.name = agentModel._agentName.GetName();
		agentModel.hp = (float)agentModel.maxHp;
		agentModel.mental = (float)agentModel.maxMental;
		this.agentListSpare.Add(agentModel);
		WorkerSpriteManager.instance.GetRandomBasicData(agentModel.spriteData, true);
		WorkerSpriteManager.instance.GetArmorData(0, ref agentModel.spriteData);
		Notice.instance.Send(NoticeName.AddNewAgent, new object[]
		{
			agentModel
		});
		return agentModel;
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x00209210 File Offset: 0x00207410
	public AgentModel AddAgentModelCustom(AgentData genData)
	{ // <Mod>
		while (this.DeletedContain((long)this.nextInstId))
		{
			this.nextInstId++;
		}
		AgentModel agentModel = null;
		if (genData.isCustomTitles)
		{
			agentModel = new AgentModel((long)this.nextInstId++, genData.customTitles);
		}
		else
		{
			agentModel = new AgentModel((long)this.nextInstId++);
		}
		agentModel._agentName = genData.agentName;
		agentModel.name = agentModel._agentName.GetName();
		agentModel.primaryStat = genData.stat;
		agentModel.hp = (float)agentModel.maxHp;
		agentModel.mental = (float)agentModel.maxMental;
		this.agentListSpare.Add(agentModel);
		agentModel.spriteData = genData.appearance.spriteSet;
		WorkerSpriteManager.instance.GetArmorData(0, ref agentModel.spriteData);
		Notice.instance.Send(NoticeName.AddNewAgent, new object[]
		{
			agentModel
		});
		return agentModel;
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x002092E8 File Offset: 0x002074E8
	private void ActivateAgent(AgentModel unit)
	{
		unit.activated = true;
		unit.GetMovableNode().SetActive(true);
		this.agentListSpare.Remove(unit);
		this.agentList.Add(unit);
		Notice.instance.Send(NoticeName.DeployAgent, new object[]
		{
			unit
		});
	}

	// Token: 0x06005AD6 RID: 23254 RVA: 0x0020933C File Offset: 0x0020753C
	private void DeactivateAgent(AgentModel unit)
	{
		unit.activated = false;
		unit.GetMovableNode().SetActive(false);
		this.agentList.Remove(unit);
		Notice.instance.Send(NoticeName.RemoveAgent, new object[]
		{
			unit
		});
		this.agentListSpare.Add(unit);
	}

	// Token: 0x06005AD7 RID: 23255 RVA: 0x00209390 File Offset: 0x00207590
	public AgentModel GetSpareAgent(long id)
	{
		AgentModel result = null;
		foreach (AgentModel agentModel in this.agentListSpare)
		{
			if (agentModel.instanceId == id)
			{
				result = agentModel;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x00048C9C File Offset: 0x00046E9C
	private void RemoveAgent(AgentModel model)
	{
		if (model.activated)
		{
			this.DeactivateAgent(model);
		}
		this.agentListSpare.Remove(model);
	}

	// Token: 0x06005AD9 RID: 23257 RVA: 0x002093FC File Offset: 0x002075FC
	public void RemoveAgent(long id)
	{
		AgentModel agent = this.GetAgent(id);
		if (agent != null)
		{
			Sefira currentSefira = agent.GetCurrentSefira();
			if (currentSefira.agentList.Contains(agent))
			{
				currentSefira.RemoveAgent(agent);
			}
			this.RemoveAgent(agent);
		}
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x00209440 File Offset: 0x00207640
	public AgentModel GetAgent(long id)
	{
		AgentModel result = null;
		foreach (AgentModel agentModel in this.agentList)
		{
			if (agentModel.instanceId == id)
			{
				result = agentModel;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x00048CBD File Offset: 0x00046EBD
	public IList<AgentModel> GetAgentList()
	{
		return this.agentList.AsReadOnly();
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x00048CCA File Offset: 0x00046ECA
	public IList<AgentModel> GetAgentSpareList()
	{
		return this.agentListSpare.AsReadOnly();
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x00048CD7 File Offset: 0x00046ED7
	public int GetAllAgentCount()
	{
		return this.agentList.Count + this.agentListSpare.Count;
	}

	// Token: 0x06005ADE RID: 23262 RVA: 0x002094AC File Offset: 0x002076AC
	public AgentModel BuyAgent()
	{
		float energy = EnergyModel.instance.GetEnergy();
		return AgentManager.instance.AddSpareAgentModel();
	}

	// Token: 0x06005ADF RID: 23263 RVA: 0x002094D0 File Offset: 0x002076D0
	private static bool TryGetValue<T>(Dictionary<string, object> dic, string name, ref T field)
	{
		object obj;
		if (dic.TryGetValue(name, out obj) && obj is T)
		{
			field = (T)((object)obj);
			return true;
		}
		return false;
	}

	// Token: 0x06005AE0 RID: 23264 RVA: 0x00209508 File Offset: 0x00207708
	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("nextInstId", this.nextInstId);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (AgentModel agentModel in this.agentList)
		{
			Dictionary<string, object> saveData = agentModel.GetSaveData();
			list.Add(saveData);
		}
		foreach (AgentModel agentModel2 in this.agentListSpare)
		{
			Dictionary<string, object> saveData2 = agentModel2.GetSaveData();
			list.Add(saveData2);
		}
		dictionary.Add("agentList", list);
		return dictionary;
	}

	// Token: 0x06005AE1 RID: 23265 RVA: 0x002095F4 File Offset: 0x002077F4
	public void LoadData(Dictionary<string, object> dic)
	{
		AgentManager.TryGetValue<int>(dic, "nextInstId", ref this.nextInstId);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		AgentManager.TryGetValue<List<Dictionary<string, object>>>(dic, "agentList", ref list);
		foreach (Dictionary<string, object> dic2 in list)
		{
			long id = 0L;
			string empty = string.Empty;
			AgentManager.TryGetValue<long>(dic2, "instanceId", ref id);
			if (!this.DeletedContain(id))
			{
				AgentManager.TryGetValue<string>(dic2, "currentSefira", ref empty);
				AgentModel agentModel = new AgentModel(id);
				agentModel.LoadData(dic2);
				this.agentListSpare.Add(agentModel);
				Notice.instance.Send(NoticeName.AddNewAgent, new object[]
				{
					agentModel
				});
				agentModel.SetCurrentSefira(empty);
			}
		}
	}

	// Token: 0x06005AE2 RID: 23266 RVA: 0x002096E4 File Offset: 0x002078E4
	public AgentModel[] GetNearAgents(MovableObjectNode node)
	{
		List<AgentModel> list = new List<AgentModel>();
		foreach (AgentModel agentModel in this.agentList)
		{
			if (!agentModel.IsDead())
			{
				if (agentModel.GetMovableNode().GetPassage() != null)
				{
					if (MovableObjectNode.GetDistance(node, agentModel.GetMovableNode()) <= 5f)
					{
						list.Add(agentModel);
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005AE3 RID: 23267 RVA: 0x00048CF0 File Offset: 0x00046EF0
	private static AgentName GetRandomAgentName()
	{
		return AgentNameList.instance.GetRandomNameByInfo();
	}

	// Token: 0x06005AE4 RID: 23268 RVA: 0x00209788 File Offset: 0x00207988
	private void OnChangeAgentSefira(AgentModel agentModel, string oldSefira)
	{
		Sefira sefira = SefiraManager.instance.GetSefira(oldSefira);
		if (sefira != null)
		{
			sefira.RemoveAgent(agentModel);
		}
		Sefira sefira2 = SefiraManager.instance.GetSefira(agentModel.currentSefira);
		if (sefira2 != null)
		{
			sefira2.AddAgent(agentModel);
		}
		if (agentModel.activated && sefira2 == null)
		{
			this.DeactivateAgent(agentModel);
		}
		else if (!agentModel.activated && sefira2 != null)
		{
			this.ActivateAgent(agentModel);
		}
	}

	// Token: 0x06005AE5 RID: 23269 RVA: 0x00209804 File Offset: 0x00207A04
	public void OnStageStart()
	{
		foreach (AgentModel agentModel in this.GetAgentList())
		{
			agentModel.OnStageStart();
		}
		foreach (AgentModel agentModel2 in this.agentListSpare)
		{
			agentModel2.lastServiceSefira = "0";
		}
	}

	// Token: 0x06005AE6 RID: 23270 RVA: 0x00209894 File Offset: 0x00207A94
	public void OnStageEnd()
	{
		foreach (AgentModel agentModel in this.GetAgentList())
		{
			agentModel.history.AddWorkDay();
			agentModel.OnStageEnd();
		}
	}

	// Token: 0x06005AE7 RID: 23271 RVA: 0x002098EC File Offset: 0x00207AEC
	public void OnStageRelease()
	{
		foreach (AgentModel agentModel in new List<AgentModel>(this.GetAgentList()))
		{
			agentModel.OnStageRelease();
			if (agentModel.IsDead())
			{
				if (!agentModel.IsDead())
				{
					agentModel.Die();
				}
				Sefira sefira = SefiraManager.instance.GetSefira(agentModel.currentSefira);
				if (sefira != null)
				{
					sefira.RemoveAgent(agentModel);
				}
				this.RemoveAgent(agentModel);
			}
		}
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x00209990 File Offset: 0x00207B90
	public void OnFixedUpdate()
	{
		foreach (AgentModel agentModel in this.agentList)
		{
			try
			{
				agentModel.OnFixedUpdate();
			}
			catch (Exception ex)
			{
			}
		}
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x00209A04 File Offset: 0x00207C04
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.ChangeAgentSefira)
		{
			AgentModel agentModel = (AgentModel)param[0];
			string oldSefira = (string)param[1];
			this.OnChangeAgentSefira(agentModel, oldSefira);
		}
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x00209A3C File Offset: 0x00207C3C
	public void DeleteAgentPermanently(ref Dictionary<string, object> dic, AgentModel agent)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "agentList", ref list);
		dic.Remove("agentList");
		Dictionary<string, object> item = null;
		if (this.HasSavedataTargetAgent(agent.instanceId, list, out item))
		{
			list.Remove(item);
		}
		dic.Add("agentList", list);
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x00209A98 File Offset: 0x00207C98
	private bool HasSavedataTargetAgent(long id, List<Dictionary<string, object>> agentDataList, out Dictionary<string, object> agentData)
	{
		foreach (Dictionary<string, object> dictionary in agentDataList)
		{
			long num = 0L;
			GameUtil.TryGetValue<long>(dictionary, "instanceId", ref num);
			if (num == id)
			{
				agentData = dictionary;
				return true;
			}
		}
		agentData = null;
		return false;
	}

	// Token: 0x06005AEC RID: 23276 RVA: 0x00209B10 File Offset: 0x00207D10
	public void AddPermantelyDeleteAgent(AgentModel agent)
	{
		if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
		{
			return;
		}
		AgentManager.PermanetlyDeletedAgent permanetlyDeletedAgent = new AgentManager.PermanetlyDeletedAgent();
		permanetlyDeletedAgent.instId = agent.instanceId;
		permanetlyDeletedAgent.nameId = agent._agentName.id;
		foreach (AgentManager.PermanetlyDeletedAgent permanetlyDeletedAgent2 in this.deletedAgent)
		{
			if (permanetlyDeletedAgent2.nameId == agent._agentName.id)
			{
				return;
			}
		}
		this.deletedAgent.Add(permanetlyDeletedAgent);
		this.SaveDelAgentData();
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x00209BC8 File Offset: 0x00207DC8
	private void SaveDelAgentData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(this.AgentDataSrc);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(this.DeletedAgentData);
		binaryFormatter.Serialize(fileStream, new Dictionary<string, object>
		{
			{
				"deleted",
				this.deletedAgent
			}
		});
		fileStream.Close();
		Debug.Log("deleted agent data saved");
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x00209C34 File Offset: 0x00207E34
	private void SaveCustomAgentData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(this.AgentDataSrc);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(this.CustomAgentData);
		binaryFormatter.Serialize(fileStream, new Dictionary<string, object>
		{
			{
				"custom",
				this.customAgent
			}
		});
		fileStream.Close();
		Debug.Log("Saved new customized agent");
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x00209CA0 File Offset: 0x00207EA0
	public void LoadDelAgentData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(this.AgentDataSrc);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		if (!File.Exists(this.DeletedAgentData))
		{
			Debug.Log(this.DeletedAgentData + " doesn't exist");
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(this.DeletedAgentData, FileMode.Open);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
		fileStream.Close();
		this.deletedAgent = null;
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x00209D20 File Offset: 0x00207F20
	public void LoadCustomAgentData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(this.AgentDataSrc);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		if (!File.Exists(this.CustomAgentData))
		{
			Debug.Log(this.CustomAgentData + " doesn't exist");
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(this.CustomAgentData, FileMode.Open);
		Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
		fileStream.Close();
		this.customAgent = null;
		if (GameUtil.TryGetValue<List<AgentManager.CustomizedAgentData>>(dic, "custom", ref this.customAgent))
		{
			foreach (AgentManager.CustomizedAgentData customizedAgentData in this.customAgent)
			{
				Debug.Log("PrevSet");
			}
		}
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x00048CFC File Offset: 0x00046EFC
	public void RemoveDelAgentData()
	{
		if (File.Exists(this.DeletedAgentData))
		{
			File.Delete(this.DeletedAgentData);
		}
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x00048D19 File Offset: 0x00046F19
	public void RemoveCustomAgentData()
	{
		if (File.Exists(this.CustomAgentData))
		{
			File.Delete(this.CustomAgentData);
		}
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x00209E0C File Offset: 0x0020800C
	public bool DeletedContain(long id)
	{
		foreach (AgentManager.PermanetlyDeletedAgent permanetlyDeletedAgent in this.deletedAgent)
		{
			if (permanetlyDeletedAgent.instId.Equals(id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x00209E7C File Offset: 0x0020807C
	public AgentManager.CustomizedAgentData AddCustomAgent(AgentModel agent)
	{
		if (this.customAgent == null)
		{
			this.customAgent = new List<AgentManager.CustomizedAgentData>();
		}
		AgentManager.CustomizedAgentData customizedAgentData = new AgentManager.CustomizedAgentData();
		customizedAgentData.instId = agent.instanceId;
		customizedAgentData.nameId = agent._agentName.id;
		customizedAgentData.nameString = agent._agentName.GetName();
		customizedAgentData.genDay = PlayerModel.instance.GetDay();
		if (this.customAgent.Contains(customizedAgentData))
		{
			return customizedAgentData;
		}
		this.customAgent.Add(customizedAgentData);
		Debug.Log("new Custom Data " + customizedAgentData.nameId);
		this.SaveCustomAgentData();
		return customizedAgentData;
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x00209F24 File Offset: 0x00208124
	public AgentManager.CustomizedAgentData GetCustomAgentData(long instId)
	{
		AgentManager.CustomizedAgentData result = null;
		foreach (AgentManager.CustomizedAgentData customizedAgentData in this.customAgent)
		{
			if (customizedAgentData.instId == instId)
			{
				result = customizedAgentData;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x00209F90 File Offset: 0x00208190
	public float GetSurvivalRate()
	{
		List<AgentModel> list = new List<AgentModel>(this.GetAgentList());
		int count = list.Count;
		int num = 0;
		if (count == 0)
		{
			return 0f;
		}
		foreach (AgentModel agentModel in list)
		{
			if (!agentModel.IsDead())
			{
				num++;
			}
		}
		return (float)num / (float)count;
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x0020A01C File Offset: 0x0020821C
	public bool RemoveAllDlcEquipment()
	{ // <Patch>
		bool result = false;
		List<AgentModel> list = new List<AgentModel>(this.agentList);
		list.AddRange(this.agentListSpare);
		foreach (long id in CreatureGenerateInfo.creditCreatures)
		{
			foreach (AgentModel agentModel in list)
			{
				CreatureTypeInfo data = CreatureTypeList.instance.GetData(id);
				if (data != null)
				{
					foreach (CreatureEquipmentMakeInfo creatureEquipmentMakeInfo in data.equipMakeInfos)
					{
						if (agentModel.Equipment.weapon != null && EquipmentTypeInfo.GetLcId(creatureEquipmentMakeInfo.equipTypeInfo) == EquipmentTypeInfo.GetLcId(agentModel.Equipment.weapon.metaInfo))
						{
							agentModel.ReleaseWeaponV2();
							result = true;
						}
						else if (agentModel.Equipment.armor != null && EquipmentTypeInfo.GetLcId(creatureEquipmentMakeInfo.equipTypeInfo) == EquipmentTypeInfo.GetLcId(agentModel.Equipment.armor.metaInfo))
						{
							agentModel.ReleaseArmor();
							result = true;
						}
						else if (agentModel.ReleaseEGOGift_Mod(EquipmentTypeInfo.GetLcId(creatureEquipmentMakeInfo.equipTypeInfo)))
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04005384 RID: 21380
	private static AgentManager _instance;

	// Token: 0x04005385 RID: 21381
	private int nextInstId;

	// Token: 0x04005386 RID: 21382
	private List<AgentModel> agentList;

	// Token: 0x04005387 RID: 21383
	public List<AgentModel> agentListSpare;

	// Token: 0x04005388 RID: 21384
	public List<AgentManager.PermanetlyDeletedAgent> deletedAgent;

	// Token: 0x04005389 RID: 21385
	public List<AgentManager.CustomizedAgentData> customAgent;

	// Token: 0x0400538A RID: 21386
	public const int agentMaxCount = 50;

	// Token: 0x02000BB5 RID: 2997
	[Serializable]
	public class PermanetlyDeletedAgent
	{
		// Token: 0x0400538B RID: 21387
		public long instId;

		// Token: 0x0400538C RID: 21388
		public int nameId;
	}

	// Token: 0x02000BB6 RID: 2998
	[Serializable]
	public class CustomizedAgentData
	{
		// Token: 0x0400538D RID: 21389
		public long instId;

		// Token: 0x0400538E RID: 21390
		public int nameId;

		// Token: 0x0400538F RID: 21391
		public string nameString;

		// Token: 0x04005390 RID: 21392
		public int genDay;
	}
}
