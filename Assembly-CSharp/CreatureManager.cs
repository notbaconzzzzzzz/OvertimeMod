/*
public void ChangeCreaturePos(CreatureModel caller, CreatureModel changed) // 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

// Token: 0x02000B9E RID: 2974
public class CreatureManager : IObserver, ISerializablePlayData
{
	// Token: 0x060059D8 RID: 23000 RVA: 0x0004784E File Offset: 0x00045A4E
	private CreatureManager()
	{
		this.observeInfoList = new Dictionary<long, CreatureObserveInfoModel>();
		this.specialSkillTable = new Dictionary<long, CreatureSpecialSkillTipTable>();
		this.Init();
	}

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x060059D9 RID: 23001 RVA: 0x0004787A File Offset: 0x00045A7A
	public static CreatureManager instance
	{
		get
		{
			if (CreatureManager._instance == null)
			{
				CreatureManager._instance = new CreatureManager();
			}
			return CreatureManager._instance;
		}
	}

	// Token: 0x060059DA RID: 23002 RVA: 0x00047895 File Offset: 0x00045A95
	private void InitValues()
	{
		this.nextInstId = 1L;
		this.creatureList = new List<CreatureModel>();
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x000478AA File Offset: 0x00045AAA
	public void Init()
	{ // <Patch>
		this.observeInfoList_mod = new Dictionary<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel>();
		this.InitValues();
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x000478B2 File Offset: 0x00045AB2
	public void Clear()
	{
		this.InitValues();
		Notice.instance.Send(NoticeName.ClearCreature, new object[0]);
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x001FE384 File Offset: 0x001FC584
	public CreatureModel AddCreature(long metadataId, SefiraIsolate roomData, string sefiraNum)
	{ // <Patch>
		return AddCreature_Mod(new LobotomyBaseMod.LcIdLong(metadataId), roomData, sefiraNum);
		/*
		long instanceId;
		this.nextInstId = (instanceId = this.nextInstId) + 1L;
		CreatureModel creatureModel = new CreatureModel(instanceId);
		this.BuildCreatureModel(creatureModel, metadataId, roomData, sefiraNum);
		this.AddCreatureInSefira(creatureModel, sefiraNum);
		this.RegisterCreature(creatureModel);
		creatureModel.script.OnInit();
		creatureModel.script.OnInitialBuild();
		return creatureModel;*/
	}

	// Token: 0x060059DE RID: 23006 RVA: 0x001FE3DC File Offset: 0x001FC5DC
	public CreatureModel ReplaceCreature(long metadataId, CreatureModel exist)
	{ // <Patch>
		return ReplaceCreature_Mod(new LobotomyBaseMod.LcIdLong(metadataId), exist);
		/*
		long instanceId;
		this.nextInstId = (instanceId = this.nextInstId) + 1L;
		CreatureModel creatureModel = new CreatureModel(instanceId);
		this.ReplaceBuildCreatureModel(creatureModel, metadataId, exist);
		this.ReplaceCommand(exist, creatureModel);
		this.UnRegisterCreature(exist);
		this.RegisterByReplace(creatureModel);
		creatureModel.script.OnInit();
		return creatureModel;*/
	}

	// Token: 0x060059DF RID: 23007 RVA: 0x001FE42C File Offset: 0x001FC62C
	public bool ReplaceAllDlcCreature()
	{ // <Patch>
		List<long> list = new List<long>(CreatureGenerateInfo.GetAll(false));
		List<long> list2 = new List<long>();
		List<CreatureModel> list3 = new List<CreatureModel>();
		CreatureModel[] array = this.GetCreatureList();
		foreach (CreatureModel creatureModel in array)
		{
			bool flag = CreatureTypeList.instance.GetModId(creatureModel.metaInfo) == string.Empty;
			if (flag)
			{
				list.Remove(creatureModel.metadataId);
				bool flag2 = creatureModel.metadataId == 100015L;
				if (flag2)
				{
					list.Remove(100014L);
				}
				foreach (long num in CreatureGenerateInfo.creditCreatures)
				{
					bool flag3 = num == creatureModel.metadataId;
					if (flag3)
					{
						list3.Add(creatureModel);
					}
				}
			}
		}
		List<long>[] array3 = new List<long>[5];
		for (int k = 0; k < 5; k++)
		{
			array3[k] = new List<long>();
		}
		foreach (long num2 in list)
		{
			CreatureTypeInfo data = CreatureTypeList.instance.GetData(num2);
			bool flag4 = data != null;
			if (flag4)
			{
				int num3 = (int)data.GetRiskLevel();
				bool flag5 = num2 == 100064L;
				if (flag5)
				{
					num3 = 4;
				}
				array3[num3].Add(num2);
			}
		}
		bool result = false;
		foreach (CreatureModel creatureModel2 in list3)
		{
			CreatureTypeInfo metaInfo = creatureModel2.metaInfo;
			bool flag6 = metaInfo != null;
			if (flag6)
			{
				int num4 = (int)metaInfo.GetRiskLevel();
				bool flag7 = metaInfo.id == 100064L;
				if (flag7)
				{
					num4 = 4;
				}
				bool flag8 = array3[num4].Count > 0;
				if (flag8)
				{
					int index = UnityEngine.Random.Range(0, array3[num4].Count);
					long num5 = array3[num4][index];
					this.ReplaceCreature_Mod(new LobotomyBaseMod.LcIdLong(num5), creatureModel2);
					array3[num4].Remove(num5);
					list.Remove(num5);
					result = true;
				}
				else
				{
					int index2 = UnityEngine.Random.Range(0, list.Count);
					long num6 = list[index2];
					this.ReplaceCreature_Mod(new LobotomyBaseMod.LcIdLong(num6), creatureModel2);
					list.Remove(num6);
					foreach (List<long> list4 in array3)
					{
						list4.Remove(num6);
					}
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x001FE6D0 File Offset: 0x001FC8D0
	public void ChangeCreaturePos(CreatureModel caller, CreatureModel changed)
	{ // <Mod> Made it change the index in creatureList too
		Sefira sefiraOrigin = caller.sefiraOrigin;
		Sefira sefira = caller.sefira;
		Sefira sefira2 = changed.sefira;
		string sefiraNum = caller.sefiraNum;
		SefiraIsolate isolateRoomData = caller.isolateRoomData;
		IsolatePos pos = isolateRoomData.pos;
		MapNode roomNode = caller.GetRoomNode();
		MapNode customNode = caller.GetCustomNode();
		MapNode workspaceNode = caller.GetWorkspaceNode();
		IsolateRoom room = caller.Unit.room;
		int index = sefira.creatureList.IndexOf(caller);
		int index2 = sefira2.creatureList.IndexOf(changed);
		sefira.creatureList[index] = changed;
		sefira2.creatureList[index2] = caller;
		index = creatureList.IndexOf(caller);
		index2 = creatureList.IndexOf(changed);
		creatureList[index] = changed;
		creatureList[index2] = caller;
		caller.sefiraOrigin = SefiraManager.instance.GetSefira(changed.sefiraOrigin.sefiraEnum);
		caller.sefira = SefiraManager.instance.GetSefira(changed.sefira.sefiraEnum);
		caller.sefiraNum = changed.sefiraNum;
		caller.isolateRoomData = changed.isolateRoomData;
		caller.specialSkillPos = changed.specialSkillPos;
		caller.basePosition = new Vector2(changed.isolateRoomData.x, changed.isolateRoomData.y);
		caller.entryNodeId = changed.isolateRoomData.nodeId;
		MapNode nodeById = MapGraph.instance.GetNodeById(changed.isolateRoomData.nodeId);
		caller.entryNode = nodeById;
		nodeById.connectedCreature = caller;
		changed.CopyNodeData(caller);
		changed.sefiraOrigin = SefiraManager.instance.GetSefira(sefiraOrigin.sefiraEnum);
		changed.sefira = SefiraManager.instance.GetSefira(sefiraOrigin.sefiraEnum);
		changed.sefiraNum = changed.sefira.indexString;
		changed.isolateRoomData = isolateRoomData;
		changed.specialSkillPos = pos;
		changed.basePosition = new Vector2(isolateRoomData.x, isolateRoomData.y);
		changed.entryNodeId = isolateRoomData.nodeId;
		MapNode nodeById2 = MapGraph.instance.GetNodeById(isolateRoomData.nodeId);
		changed.entryNode = nodeById2;
		nodeById2.connectedCreature = changed;
		changed.SetRoomNode(roomNode);
		changed.SetCustomNode(customNode);
		changed.SetWorkspaceNode(workspaceNode);
		CreatureLayer.currentLayer.ChangeCreaturePos(caller, changed);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x001FE8DC File Offset: 0x001FCADC
	public void ReplaceCommand(CreatureModel old, CreatureModel replaced)
	{
		int index = this.creatureList.IndexOf(old);
		int index2 = old.sefira.creatureList.IndexOf(old);
		this.creatureList[index] = replaced;
		old.sefira.creatureList[index2] = replaced;
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x000478CF File Offset: 0x00045ACF
	public void AddCreatureInSefira(CreatureModel creature, string sefira)
	{
		SefiraManager.instance.GetSefira(sefira).creatureList.Add(creature);
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x000478E7 File Offset: 0x00045AE7
	public void RemoveCreatureInSefira(CreatureModel creature, string sefira)
	{
		SefiraManager.instance.GetSefira(sefira).creatureList.Remove(creature);
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x00047900 File Offset: 0x00045B00
	public void AddChildObserveInfo(CreatureObserveInfoModel infoModel)
	{ // <Patch>
		if (infoModel.creatureTypeId == 100038L && infoModel.lcid.packageId == string.Empty && !this.observeInfoList.ContainsKey(100038L))
		{
			this.observeInfoList.Add(infoModel.creatureTypeId, infoModel);
		}
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x00047940 File Offset: 0x00045B40
	public void RegisterCreature(CreatureModel model)
	{
		model.GetMovableNode().SetActive(true);
		this.creatureList.Add(model);
		Notice.instance.Send(NoticeName.AddCreature, new object[]
		{
			model
		});
	}

	// Token: 0x060059E6 RID: 23014 RVA: 0x00047973 File Offset: 0x00045B73
	public void RegisterByReplace(CreatureModel model)
	{
		model.GetMovableNode().SetActive(true);
		Notice.instance.Send(NoticeName.AddCreature, new object[]
		{
			model
		});
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x0004799A File Offset: 0x00045B9A
	public void UnRegisterCreature(CreatureModel model)
	{
		model.GetMovableNode().SetActive(false);
		this.creatureList.Remove(model);
		Notice.instance.Send(NoticeName.RemoveCreature, new object[]
		{
			model
		});
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x001FE928 File Offset: 0x001FCB28
	private void BuildCreatureModel(CreatureModel model, long metadataId, SefiraIsolate roomData, string sefiraNum)
	{ // <Patch>
		BuildCreatureModel_Mod(model, new LobotomyBaseMod.LcIdLong(metadataId), roomData, sefiraNum);
		/*
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(metadataId);
		if (data == null)
		{
			return;
		}
		object obj = null;
		foreach (Assembly assembly in Add_On.instance.AssemList)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == data.script)
				{
					obj = Activator.CreateInstance(type);
				}
			}
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(Type.GetType(data.script));
		}
		model.script = (CreatureBase)obj;
		if (this.observeInfoList.ContainsKey(metadataId))
		{
			this.observeInfoList.TryGetValue(metadataId, out model.observeInfo);
		}
		else
		{
			model.observeInfo = new CreatureObserveInfoModel(metadataId);
			this.observeInfoList.Add(metadataId, model.observeInfo);
		}
		model.sefira = (model.sefiraOrigin = SefiraManager.instance.GetSefira(sefiraNum));
		model.sefiraNum = sefiraNum;
		model.specialSkillPos = roomData.pos;
		model.isolateRoomData = roomData;
		model.metadataId = metadataId;
		model.metaInfo = data;
		if (CreatureTypeList.instance.GetSkillTipData(metadataId) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(metadataId).GetCopy();
		}
		model.basePosition = new Vector2(roomData.x, roomData.y);
		model.script.SetModel(model);
		model.entryNodeId = roomData.nodeId;
		MapNode nodeById = MapGraph.instance.GetNodeById(roomData.nodeId);
		model.entryNode = nodeById;
		nodeById.connectedCreature = model;
		Dictionary<string, MapNode> dictionary = new Dictionary<string, MapNode>();
		List<MapEdge> list = new List<MapEdge>();
		MapNode mapNode = null;
		PassageObjectModel passageObjectModel = null;
		passageObjectModel = new PassageObjectModel(roomData.nodeId + "@creature", nodeById.GetAreaName(), "Map/Passage/PassageEmpty");
		passageObjectModel.isDynamic = true;
		passageObjectModel.Activate();
		passageObjectModel.scaleFactor = 0.75f;
		passageObjectModel.SetToIsolate();
		passageObjectModel.position = new Vector3(roomData.x, roomData.y, 0f);
		passageObjectModel.type = PassageType.ISOLATEROOM;
		IEnumerator enumerator2 = data.nodeInfo.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				XmlNode xmlNode = (XmlNode)obj2;
				string text = roomData.nodeId + "@" + xmlNode.Attributes.GetNamedItem("id").InnerText;
				float x = model.basePosition.x + float.Parse(xmlNode.Attributes.GetNamedItem("x").InnerText);
				float y = model.basePosition.y + float.Parse(xmlNode.Attributes.GetNamedItem("y").InnerText);
				XmlNode namedItem = xmlNode.Attributes.GetNamedItem("type");
				MapNode mapNode2;
				if (namedItem != null && namedItem.InnerText == "workspace")
				{
					mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
					passageObjectModel.AddNode(mapNode2);
					model.SetWorkspaceNode(mapNode2);
				}
				else if (namedItem != null && namedItem.InnerText == "custom")
				{
					mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
					passageObjectModel.AddNode(mapNode2);
					model.SetCustomNode(mapNode2);
				}
				else if (namedItem != null && namedItem.InnerText == "creature")
				{
					mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
					passageObjectModel.AddNode(mapNode2);
					model.SetRoomNode(mapNode2);
					model.SetCurrentNode(mapNode2);
				}
				else
				{
					if (namedItem == null || !(namedItem.InnerText == "innerDoor"))
					{
						continue;
					}
					mapNode = (mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel));
					passageObjectModel.AddNode(mapNode2);
					DoorObjectModel doorObjectModel = new DoorObjectModel(string.Concat(new object[]
					{
						nodeById,
						"@",
						text,
						"@inner"
					}), "DoorIsolate", passageObjectModel, mapNode);
					doorObjectModel.position = new Vector3(mapNode.GetPosition().x, mapNode.GetPosition().y, -0.01f);
					passageObjectModel.AddDoor(doorObjectModel);
					mapNode.SetDoor(doorObjectModel);
					doorObjectModel.Close();
				}
				dictionary.Add(text, mapNode2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator2 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		PassageObjectModel attachedPassage = nodeById.GetAttachedPassage();
		MapNode mapNode3 = new MapNode(roomData.nodeId + "@outter", new Vector2(nodeById.GetPosition().x, nodeById.GetPosition().y), nodeById.GetAreaName(), attachedPassage);
		string id = roomData.nodeId + "@outterDoor";
		string type2 = "MalkuthDoorMiddle";
		switch (model.sefira.sefiraEnum)
		{
		case SefiraEnum.MALKUT:
			type2 = "MalkuthDoorMiddle";
			break;
		case SefiraEnum.YESOD:
			type2 = "YesodDoorMiddle";
			break;
		case SefiraEnum.HOD:
			type2 = "HodDoorMiddle";
			break;
		case SefiraEnum.NETZACH:
			type2 = "NetzachDoorMiddle";
			break;
		case SefiraEnum.TIPERERTH1:
		case SefiraEnum.TIPERERTH2:
			type2 = "TipherethDoorMiddle";
			break;
		case SefiraEnum.GEBURAH:
			type2 = "GeburahDoorMiddle";
			break;
		case SefiraEnum.CHESED:
			type2 = "ChesedDoorMiddle";
			break;
		case SefiraEnum.BINAH:
			type2 = "BinahDoorMiddle";
			break;
		case SefiraEnum.CHOKHMAH:
			type2 = "ChokhmahDoorMiddle";
			break;
		case SefiraEnum.KETHER:
			type2 = "KetherDoorMiddle";
			break;
		}
		DoorObjectModel doorObjectModel2 = new DoorObjectModel(id, type2, attachedPassage, mapNode3);
		doorObjectModel2.position = new Vector3(mapNode3.GetPosition().x, mapNode3.GetPosition().y, -0.01f);
		attachedPassage.AddDoor(doorObjectModel2);
		mapNode3.SetDoor(doorObjectModel2);
		doorObjectModel2.Close();
		attachedPassage.AddNode(mapNode3);
		MapEdge mapEdge = new MapEdge(mapNode3, nodeById, "road");
		list.Add(mapEdge);
		mapNode3.AddEdge(mapEdge);
		nodeById.AddEdge(mapEdge);
		if (mapNode != null)
		{
			MapEdge mapEdge2 = new MapEdge(mapNode3, mapNode, "door", 0.01f);
			doorObjectModel2.Connect(mapNode.GetDoor());
			list.Add(mapEdge2);
			mapNode3.AddEdge(mapEdge2);
			mapNode.AddEdge(mapEdge2);
		}
		dictionary.Add(mapNode3.GetId(), mapNode3);
		if (model.GetCustomNode() == null)
		{
			model.SetCustomNode(model.GetCurrentNode());
		}
		IEnumerator enumerator3 = data.edgeInfo.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				XmlNode xmlNode2 = (XmlNode)obj3;
				string text2 = roomData.nodeId + "@" + xmlNode2.Attributes.GetNamedItem("node1").InnerText;
				string text3 = roomData.nodeId + "@" + xmlNode2.Attributes.GetNamedItem("node2").InnerText;
				string innerText = xmlNode2.Attributes.GetNamedItem("type").InnerText;
				MapNode mapNode4 = null;
				MapNode mapNode5 = null;
				if (!dictionary.TryGetValue(text2, out mapNode4) || !dictionary.TryGetValue(text3, out mapNode5))
				{
					Debug.Log(string.Concat(new string[]
					{
						"cannot create edge - (",
						text2,
						", ",
						text3,
						")"
					}));
				}
				XmlNode namedItem2 = xmlNode2.Attributes.GetNamedItem("cost");
				MapEdge mapEdge3;
				if (namedItem2 != null)
				{
					mapEdge3 = new MapEdge(mapNode4, mapNode5, innerText, float.Parse(namedItem2.InnerText));
				}
				else
				{
					mapEdge3 = new MapEdge(mapNode4, mapNode5, innerText);
				}
				list.Add(mapEdge3);
				mapNode4.AddEdge(mapEdge3);
				mapNode5.AddEdge(mapEdge3);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator3 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		MapGraph.instance.RegisterPassage(passageObjectModel);*/
	}

	// Token: 0x060059E9 RID: 23017 RVA: 0x001FF164 File Offset: 0x001FD364
	private void ReplaceBuildCreatureModel(CreatureModel model, long metadataId, CreatureModel old)
	{ // <Patch>
		ReplaceBuildCreatureModel_Mod(model, new LobotomyBaseMod.LcIdLong(metadataId), old);
		/*
		if (this.observeInfoList.ContainsKey(metadataId))
		{
			this.observeInfoList.TryGetValue(metadataId, out model.observeInfo);
		}
		else
		{
			model.observeInfo = new CreatureObserveInfoModel(metadataId);
			this.observeInfoList.Add(metadataId, model.observeInfo);
		}
		model.sefiraOrigin = old.sefiraOrigin;
		model.sefira = old.sefira;
		model.sefiraNum = old.sefiraNum;
		SefiraIsolate isolateRoomData = old.isolateRoomData;
		model.specialSkillPos = isolateRoomData.pos;
		model.isolateRoomData = isolateRoomData;
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(metadataId);
		model.metadataId = metadataId;
		model.metaInfo = data;
		if (CreatureTypeList.instance.GetSkillTipData(metadataId) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(metadataId).GetCopy();
		}
		model.basePosition = new Vector2(isolateRoomData.x, isolateRoomData.y);
		model.script = (CreatureBase)Activator.CreateInstance(Type.GetType(data.script));
		model.script.SetModel(model);
		model.entryNodeId = isolateRoomData.nodeId;
		MapNode nodeById = MapGraph.instance.GetNodeById(isolateRoomData.nodeId);
		model.entryNode = nodeById;
		nodeById.connectedCreature = model;
		old.CopyNodeData(model);
		model.script.OnInitialBuild();*/
	}

	// Token: 0x060059EA RID: 23018 RVA: 0x000479CE File Offset: 0x00045BCE
	public CreatureModel[] GetCreatureList()
	{
		return this.creatureList.ToArray();
	}

	// Token: 0x060059EB RID: 23019 RVA: 0x000479DB File Offset: 0x00045BDB
	public int GetCreatureCount()
	{
		return this.creatureList.Count;
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x001FF2B8 File Offset: 0x001FD4B8
	public CreatureModel FindCreature(long metaId)
	{ // <Patch>
		return FindCreature_Mod(new LobotomyBaseMod.LcIdLong(metaId));
		/*
		CreatureModel result = null;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (creatureModel.metadataId == metaId)
			{
				result = creatureModel;
				break;
			}
		}
		return result;*/
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x001FF324 File Offset: 0x001FD524
	public CreatureModel GetCreature(long id)
	{ // <Patch>
		//return GetCreature_Mod(new LobotomyBaseMod.LcIdLong(id));
		
		CreatureModel result = null;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (creatureModel.instanceId == id)
			{
				result = creatureModel;
				break;
			}
		}
		return result;
	}

	// Token: 0x060059EE RID: 23022 RVA: 0x001FF390 File Offset: 0x001FD590
	public int GetObserveLevel(long metadataId)
	{ // <Patch>
		return GetObserveLevel_Mod(new LobotomyBaseMod.LcIdLong(metadataId));
		/*
		CreatureObserveInfoModel creatureObserveInfoModel = null;
		if (this.observeInfoList.TryGetValue(metadataId, out creatureObserveInfoModel))
		{
			return creatureObserveInfoModel.GetObservationLevel();
		}
		return 0;*/
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x001FD68C File Offset: 0x001FB88C
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

	// Token: 0x060059F0 RID: 23024 RVA: 0x001FF3BC File Offset: 0x001FD5BC
	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("nextInstId", this.nextInstId);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			Dictionary<string, object> saveData = creatureModel.GetSaveData();
			list.Add(saveData);
		}
		dictionary.Add("creatureList", list);
		return dictionary;
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x001FF450 File Offset: 0x001FD650
	public void LoadData(Dictionary<string, object> dic)
	{ // <Patch>
		bool flag = !GameUtil.TryGetValue<long>(dic, "nextInstId", ref this.nextInstId);
		if (flag)
		{
			int num = 0;
			bool flag2 = GameUtil.TryGetValue<int>(dic, "nextInstId", ref num);
			if (flag2)
			{
				this.nextInstId = (long)num;
			}
		}
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		GameUtil.TryGetValue<List<Dictionary<string, object>>>(dic, "creatureList", ref list);
		List<long> list2 = new List<long>();
		foreach (Dictionary<string, object> dic2 in list)
		{
			long item = 0L;
			LobotomyBaseMod.ExtenionUtil.TryGetValue<long>(dic2, "metadataId", ref item);
			list2.Add(item);
		}
		foreach (Dictionary<string, object> dic3 in list)
		{
			long id = 0L;
			string empty = string.Empty;
			GameUtil.TryGetValue<long>(dic3, "metadataId", ref id);
			GameUtil.TryGetValue<string>(dic3, "modid", ref empty);
			LobotomyBaseMod.LcIdLong lcIdLong = new LobotomyBaseMod.LcIdLong(empty, id);
			bool flag3 = CreatureTypeList.instance.GetData_Mod(lcIdLong) != null;
			if (flag3)
			{
				long num2 = 0L;
				GameUtil.TryGetValue<long>(dic3, "instanceId", ref num2);
				CreatureModel creatureModel = new CreatureModel(num2);
				creatureModel.LoadData(dic3);
				bool flag4 = empty == string.Empty;
				if (flag4)
				{
					bool flag5 = creatureModel.metadataId == 300109L && !list2.Contains(100104L);
					if (flag5)
					{
						new List<long>(CreatureGenerateInfo.GetAll(false));
						creatureModel.metadataId = 100104L;
						list2.Add(100104L);
					}
				}
				Sefira sefira = SefiraManager.instance.GetSefira(creatureModel.sefiraNum);
				try
				{
					creatureModel.isolateRoomData = sefira.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel.metadataId, creatureModel.entryNodeId);
				}
				catch (SefiraIsolateException ex)
				{
					Debug.LogError("Emergency Load " + ex.nodeId);
					string nodeId = ex.nodeId;
					Sefira sefira2 = null;
					bool flag6 = !SefiraManager.instance.GetSefiraByGenNodeId(nodeId, out sefira2);
					if (flag6)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Failed to gen sefira creature + ",
							nodeId,
							"   Creature ID : ",
							num2
						}));
						continue;
					}
					creatureModel.isolateRoomData = sefira2.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel.metadataId, creatureModel.entryNodeId);
				}
				this.BuildCreatureModel_Mod(creatureModel, lcIdLong, creatureModel.isolateRoomData, creatureModel.sefiraNum);
				this.RegisterCreature(creatureModel);
				this.AddCreatureInSefira(creatureModel, creatureModel.sefiraNum);
				creatureModel.script.OnInit();
			}
			else
			{
				bool flag7 = empty == string.Empty;
				if (flag7)
				{
					foreach (ModInfo modInfo_patch in Add_On.instance.ModList)
					{
						bool flag8 = modInfo_patch.modid != string.Empty;
						if (flag8)
						{
							lcIdLong = new LobotomyBaseMod.LcIdLong(modInfo_patch.modid, id);
							bool flag9 = CreatureTypeList.instance.GetData_Mod(lcIdLong) != null;
							if (flag9)
							{
								long num3 = 0L;
								GameUtil.TryGetValue<long>(dic3, "instanceId", ref num3);
								CreatureModel creatureModel2 = new CreatureModel(num3);
								creatureModel2.LoadData(dic3);
								bool flag10 = empty == string.Empty;
								if (flag10)
								{
									bool flag11 = creatureModel2.metadataId == 300109L && !list2.Contains(100104L);
									if (flag11)
									{
										new List<long>(CreatureGenerateInfo.GetAll(false));
										creatureModel2.metadataId = 100104L;
										list2.Add(100104L);
									}
								}
								Sefira sefira3 = SefiraManager.instance.GetSefira(creatureModel2.sefiraNum);
								try
								{
									creatureModel2.isolateRoomData = sefira3.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel2.metadataId, creatureModel2.entryNodeId);
								}
								catch (SefiraIsolateException ex2)
								{
									Debug.LogError("Emergency Load " + ex2.nodeId);
									string nodeId2 = ex2.nodeId;
									Sefira sefira4 = null;
									bool flag12 = !SefiraManager.instance.GetSefiraByGenNodeId(nodeId2, out sefira4);
									if (flag12)
									{
										Debug.LogError(string.Concat(new object[]
										{
											"Failed to gen sefira creature + ",
											nodeId2,
											"   Creature ID : ",
											num3
										}));
										break;
									}
									creatureModel2.isolateRoomData = sefira4.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel2.metadataId, creatureModel2.entryNodeId);
								}
								this.BuildCreatureModel_Mod(creatureModel2, lcIdLong, creatureModel2.isolateRoomData, creatureModel2.sefiraNum);
								this.RegisterCreature(creatureModel2);
								this.AddCreatureInSefira(creatureModel2, creatureModel2.sefiraNum);
								creatureModel2.script.OnInit();
								break;
							}
						}
					}
				}
			}
		}
		/*
		if (!CreatureManager.TryGetValue<long>(dic, "nextInstId", ref this.nextInstId))
		{
			int num = 0;
			if (CreatureManager.TryGetValue<int>(dic, "nextInstId", ref num))
			{
				this.nextInstId = (long)num;
			}
		}
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		CreatureManager.TryGetValue<List<Dictionary<string, object>>>(dic, "creatureList", ref list);
		List<long> list2 = new List<long>();
		foreach (Dictionary<string, object> dic2 in list)
		{
			long item = 0L;
			CreatureManager.TryGetValue<long>(dic2, "metadataId", ref item);
			list2.Add(item);
		}
		foreach (Dictionary<string, object> dic3 in list)
		{
			long id = 0L;
			GameUtil.TryGetValue<long>(dic3, "metadataId", ref id);
			if (CreatureTypeList.instance.GetData(id) != null)
			{
				long num2 = 0L;
				CreatureManager.TryGetValue<long>(dic3, "instanceId", ref num2);
				CreatureModel creatureModel = new CreatureModel(num2);
				creatureModel.LoadData(dic3);
				if (creatureModel.metadataId == 300109L && !list2.Contains(100104L))
				{
					new List<long>(CreatureGenerateInfo.GetAll(false));
					creatureModel.metadataId = 100104L;
					list2.Add(100104L);
				}
				Sefira sefira = SefiraManager.instance.GetSefira(creatureModel.sefiraNum);
				try
				{
					creatureModel.isolateRoomData = sefira.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel.metadataId, creatureModel.entryNodeId);
				}
				catch (SefiraIsolateException ex)
				{
					Debug.LogError("Emergency Load " + ex.nodeId);
					string nodeId = ex.nodeId;
					Sefira sefira2 = null;
					if (!SefiraManager.instance.GetSefiraByGenNodeId(nodeId, out sefira2))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Failed to gen sefira creature + ",
							nodeId,
							"   Creature ID : ",
							num2
						}));
						break;
					}
					creatureModel.isolateRoomData = sefira2.isolateManagement.GenIsolateByCreatureByNodeId(creatureModel.metadataId, creatureModel.entryNodeId);
				}
				this.BuildCreatureModel(creatureModel, creatureModel.metadataId, creatureModel.isolateRoomData, creatureModel.sefiraNum);
				this.RegisterCreature(creatureModel);
				this.AddCreatureInSefira(creatureModel, creatureModel.sefiraNum);
				creatureModel.script.OnInit();
			}
		}*/
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x001FF6F0 File Offset: 0x001FD8F0
	public Dictionary<string, object> GetSaveObserveData()
	{ // <Patch>
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<long, Dictionary<string, object>> dictionary2 = new Dictionary<long, Dictionary<string, object>>();
		Dictionary<string, Dictionary<long, Dictionary<string, object>>> dictionary3 = new Dictionary<string, Dictionary<long, Dictionary<string, object>>>();
		foreach (KeyValuePair<long, CreatureObserveInfoModel> keyValuePair in this.observeInfoList)
		{
			dictionary2.Add(keyValuePair.Key, keyValuePair.Value.GetSaveGlobalData());
		}
		foreach (KeyValuePair<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel> keyValuePair2 in this.observeInfoList_mod)
		{
			bool flag = !dictionary3.ContainsKey(keyValuePair2.Key.packageId);
			if (flag)
			{
				dictionary3[keyValuePair2.Key.packageId] = new Dictionary<long, Dictionary<string, object>>();
			}
			dictionary3[keyValuePair2.Key.packageId][keyValuePair2.Key.id] = keyValuePair2.Value.GetSaveGlobalData();
		}
		dictionary.Add("observeList", dictionary2);
		dictionary.Add("observeListMod", dictionary3);
		return dictionary;
	}

	// Token: 0x060059F3 RID: 23027 RVA: 0x001FF778 File Offset: 0x001FD978
	public void LoadObserveData(Dictionary<string, object> dic)
	{ // <Patch>
		this.observeInfoList = new Dictionary<long, CreatureObserveInfoModel>();
		this.observeInfoList_mod = new Dictionary<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel>();
		Dictionary<long, Dictionary<string, object>> dictionary = new Dictionary<long, Dictionary<string, object>>();
		Dictionary<string, Dictionary<long, Dictionary<string, object>>> dictionary2 = new Dictionary<string, Dictionary<long, Dictionary<string, object>>>();
		GameUtil.TryGetValue<Dictionary<string, Dictionary<long, Dictionary<string, object>>>>(dic, "observeListMod", ref dictionary2);
		foreach (KeyValuePair<string, Dictionary<long, Dictionary<string, object>>> keyValuePair in dictionary2)
		{
			foreach (KeyValuePair<long, Dictionary<string, object>> keyValuePair2 in keyValuePair.Value)
			{
				try
				{
					LobotomyBaseMod.LcIdLong lcIdLong = new LobotomyBaseMod.LcIdLong(keyValuePair.Key, keyValuePair2.Key);
					bool flag = CreatureTypeList.instance.GetData_Mod(lcIdLong) != null;
					if (flag)
					{
						CreatureObserveInfoModel creatureObserveInfoModel = new CreatureObserveInfoModel(keyValuePair2.Key);
						creatureObserveInfoModel.Init_Mod(lcIdLong);
						creatureObserveInfoModel.LoadGlobalData(keyValuePair2.Value);
						this.observeInfoList_mod.Add(lcIdLong, creatureObserveInfoModel);
						string str = "Add ObserveData - ";
						LobotomyBaseMod.LcIdLong lcIdLong2 = lcIdLong;
						LobotomyBaseMod.ModDebug.Log(str + ((lcIdLong2 != null) ? lcIdLong2.ToString() : null));
					}
				}
				catch (Exception ex)
				{
					LobotomyBaseMod.ModDebug.Log("CM.LODerror_Mod - " + ex.Message + Environment.NewLine + ex.StackTrace);
				}
			}
		}
		GameUtil.TryGetValue<Dictionary<long, Dictionary<string, object>>>(dic, "observeList", ref dictionary);
		foreach (KeyValuePair<long, Dictionary<string, object>> keyValuePair3 in dictionary)
		{
			try
			{
				bool flag2 = CreatureTypeList.instance.GetData(keyValuePair3.Key) != null;
				if (flag2)
				{
					CreatureObserveInfoModel creatureObserveInfoModel2 = new CreatureObserveInfoModel(keyValuePair3.Key);
					creatureObserveInfoModel2.Init_Mod(new LobotomyBaseMod.LcIdLong(keyValuePair3.Key));
					creatureObserveInfoModel2.LoadGlobalData(keyValuePair3.Value);
					this.observeInfoList.Add(keyValuePair3.Key, creatureObserveInfoModel2);
					LobotomyBaseMod.ModDebug.Log("Add ObserveData - " + keyValuePair3.Key.ToString());
				}
				else
				{
					foreach (ModInfo modInfo_patch in Add_On.instance.ModList)
					{
						bool flag3 = modInfo_patch.modid != string.Empty;
						if (flag3)
						{
							LobotomyBaseMod.LcIdLong lcIdLong3 = new LobotomyBaseMod.LcIdLong(modInfo_patch.modid, keyValuePair3.Key);
							bool flag4 = this.observeInfoList_mod.ContainsKey(lcIdLong3);
							if (!flag4)
							{
								bool flag5 = CreatureTypeList.instance.GetData_Mod(lcIdLong3) != null;
								if (flag5)
								{
									CreatureObserveInfoModel creatureObserveInfoModel3 = new CreatureObserveInfoModel(keyValuePair3.Key);
									creatureObserveInfoModel3.Init_Mod(lcIdLong3);
									creatureObserveInfoModel3.LoadGlobalData(keyValuePair3.Value);
									this.observeInfoList_mod.Add(lcIdLong3, creatureObserveInfoModel3);
									string str2 = "Add ObserveData - ";
									LobotomyBaseMod.LcIdLong lcIdLong4 = lcIdLong3;
									LobotomyBaseMod.ModDebug.Log(str2 + ((lcIdLong4 != null) ? lcIdLong4.ToString() : null));
									break;
								}
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				LobotomyBaseMod.ModDebug.Log("CM.LODerror - " + ex2.Message + Environment.NewLine + ex2.StackTrace);
			}
		}
		/*
		this.observeInfoList = new Dictionary<long, CreatureObserveInfoModel>();
		Dictionary<long, Dictionary<string, object>> dictionary = new Dictionary<long, Dictionary<string, object>>();
		GameUtil.TryGetValue<Dictionary<long, Dictionary<string, object>>>(dic, "observeList", ref dictionary);
		foreach (KeyValuePair<long, Dictionary<string, object>> keyValuePair in dictionary)
		{
			try
			{
				if (CreatureTypeList.instance.GetData(keyValuePair.Key) != null)
				{
					CreatureObserveInfoModel creatureObserveInfoModel = new CreatureObserveInfoModel(keyValuePair.Key);
					creatureObserveInfoModel.LoadGlobalData(keyValuePair.Value);
					this.observeInfoList.Add(keyValuePair.Key, creatureObserveInfoModel);
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Crerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}*/
	}

	// Token: 0x060059F4 RID: 23028 RVA: 0x001FF860 File Offset: 0x001FDA60
	public Dictionary<string, object> GetSaveSpecialSkillTable()
	{
		return null;
	}

	// Token: 0x060059F5 RID: 23029 RVA: 0x001FF870 File Offset: 0x001FDA70
	public void LoadSpecialSkillTable(Dictionary<string, object> dic)
	{
	}

	// Token: 0x060059F6 RID: 23030 RVA: 0x001FF880 File Offset: 0x001FDA80
	public void ResetObserveData()
	{ // <Patch>
		this.observeInfoList_mod = new Dictionary<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel>();
		this.observeInfoList = new Dictionary<long, CreatureObserveInfoModel>();
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			string modId = CreatureTypeList.instance.GetModId(creatureModel.metaInfo);
			creatureModel.observeInfo.InitData_Mod(new LobotomyBaseMod.LcIdLong(modId, creatureModel.metaInfo.id));
		}
		/*
		this.observeInfoList = new Dictionary<long, CreatureObserveInfoModel>();
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.observeInfo.InitData();
		}*/
	}

	// Token: 0x060059F7 RID: 23031 RVA: 0x001FF8EC File Offset: 0x001FDAEC
	public List<CreatureObserveInfoModel> GetObserveInfoList()
	{ // <Patch>
		List<CreatureObserveInfoModel> list = new List<CreatureObserveInfoModel>();
		foreach (CreatureObserveInfoModel item in this.observeInfoList.Values)
		{
			list.Add(item);
		}
		foreach (KeyValuePair<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel> keyValuePair in this.observeInfoList_mod)
		{
			list.Add(keyValuePair.Value);
		}
		return list;
	}

	// Token: 0x060059F8 RID: 23032 RVA: 0x000479E8 File Offset: 0x00045BE8
	public int GetMaxHiddenProgressByObserveLevel()
	{
		return CreatureGenerateInfo.all_for_codex.Length;
	}

	// Token: 0x060059F9 RID: 23033 RVA: 0x001FF954 File Offset: 0x001FDB54
	public int GetHiddenProgressByObserveLevel()
	{
		int num = 0;
		foreach (long key in CreatureGenerateInfo.all_for_codex)
		{
			CreatureObserveInfoModel creatureObserveInfoModel = null;
			if (this.observeInfoList.TryGetValue(key, out creatureObserveInfoModel) && creatureObserveInfoModel.IsMaxObserved())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060059FA RID: 23034 RVA: 0x000479F1 File Offset: 0x00045BF1
	public bool IsMaxHiddenProgress()
	{
		return this.GetHiddenProgressByObserveLevel() >= this.GetMaxHiddenProgressByObserveLevel();
	}

	// Token: 0x060059FB RID: 23035 RVA: 0x001FF9A8 File Offset: 0x001FDBA8
	public void ResetSpecialSkillTable()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(creatureModel.metadataId).GetCopy();
			creatureModel.observeInfo.observeProgress = 0;
			creatureModel.metaInfo.specialSkillTable.Init();
		}
	}

	// Token: 0x060059FC RID: 23036 RVA: 0x001FFA3C File Offset: 0x001FDC3C
	public CreatureModel[] GetNearSuppressedCreatures(MovableObjectNode node)
	{
		List<CreatureModel> list = new List<CreatureModel>();
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (creatureModel.state == CreatureState.SUPPRESSED)
			{
				list.Add(creatureModel);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x001FFAB8 File Offset: 0x001FDCB8
	public void OnFixedUpdate()
	{
		try
		{
			foreach (CreatureModel creatureModel in this.creatureList)
			{
				try
				{
					creatureModel.OnFixedUpdate();
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
		}
		catch (InvalidOperationException arg)
		{
			Debug.LogError("list modified" + arg);
		}
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x00003E35 File Offset: 0x00002035
	public void OnNotice(string notice, params object[] param)
	{
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x001FFB58 File Offset: 0x001FDD58
	public void OnStageStart()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.OnStageStart();
			if (creatureModel.script.skill != null)
			{
				creatureModel.script.skill.OnStageStart();
			}
		}
	}

	// Token: 0x06005A00 RID: 23040 RVA: 0x001FFBD4 File Offset: 0x001FDDD4
	public void OnStageRelease()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			try
			{
				creatureModel.OnStageRelease();
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Release Error : ",
					creatureModel.metadataId,
					Environment.NewLine,
					ex.ToString()
				}));
			}
		}
	}

	// Token: 0x06005A01 RID: 23041 RVA: 0x001FFC80 File Offset: 0x001FDE80
	public void OnStageEnd()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.script.OnStageEnd();
			creatureModel.OnStageEnd();
		}
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x001FFCE8 File Offset: 0x001FDEE8
	public void OnGameInit()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.OnGameInit();
			creatureModel.script.OnGamemanagerInit();
		}
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x001FFD50 File Offset: 0x001FDF50
	public void RemoveSriptSaveData()
	{
		string path = Application.persistentDataPath + "/creatureData";
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		if (directoryInfo.Exists)
		{
			directoryInfo.Delete(true);
		}
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x001FFD88 File Offset: 0x001FDF88
	public void LoadScriptSaveData()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (creatureModel.script.HasScriptSaveData())
			{
				creatureModel.script.LoadScriptData();
			}
		}
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x001FFDF8 File Offset: 0x001FDFF8
	public CreatureModel PickOtherSefiraCreatureByRandom(CreatureModel exclude)
	{
		if (PlayerModel.instance.GetOpenedAreaCount() == 1)
		{
			return null;
		}
		List<Sefira> list = new List<Sefira>(SefiraManager.instance.GetActivatedSefiras());
		list.Remove(exclude.sefira);
		foreach (Sefira sefira in list)
		{
			if (sefira.sefiraEnum == SefiraEnum.DAAT)
			{
				list.Remove(sefira);
				break;
			}
		}
		Sefira sefira2 = list[UnityEngine.Random.Range(0, list.Count)];
		return sefira2.creatureList[UnityEngine.Random.Range(0, sefira2.creatureList.Count)];
	}

	// Token: 0x06005A06 RID: 23046 RVA: 0x001FFEC4 File Offset: 0x001FE0C4
	public void OnAddCreatureWorkCountInSefira(Sefira s)
	{
		bool flag = true;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (!creatureModel.IsWorkCountFull())
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			foreach (CreatureModel creatureModel2 in this.creatureList)
			{
				creatureModel2.ResetWorkCount();
			}
		}
	}

	// Token: 0x06005A07 RID: 23047 RVA: 0x001FFF80 File Offset: 0x001FE180
	public int GetSefiraWorkCount(Sefira s)
	{
		int num = 0;
		foreach (CreatureModel creatureModel in s.creatureList)
		{
			if (creatureModel.IsWorkCountFull())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005A08 RID: 23048 RVA: 0x001FFFF4 File Offset: 0x001FE1F4
	public int GetSefiraMaxWorkCount(Sefira s)
	{
		int num = 0;
		foreach (CreatureModel creatureModel in s.creatureList)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x00200050 File Offset: 0x001FE250
	public void ResetProbReductionCounterAll()
	{
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			creatureModel.ResetProbReductionCounter();
		}
	}

	// Token: 0x06005A0A RID: 23050 RVA: 0x002000AC File Offset: 0x001FE2AC
	public CreatureObserveInfoModel GetObserveInfo(long metadataId)
	{ // <Patch>
		return GetObserveInfo_Mod(new LobotomyBaseMod.LcIdLong(metadataId));
		/*
		CreatureObserveInfoModel result = null;
		if (this.observeInfoList.TryGetValue(metadataId, out result))
		{
			return result;
		}
		return null;*/
	}

	// Token: 0x06005A0B RID: 23051 RVA: 0x002000D4 File Offset: 0x001FE2D4
	public bool IsCreatureActivated(long metaId)
	{ // <Patch>
		return IsCreatureActivated_Mod(new LobotomyBaseMod.LcIdLong(metaId));
		/*
		bool result = false;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			if (creatureModel.metadataId == metaId)
			{
				result = true;
				break;
			}
		}
		return result;*/
	}

	// <Patch>
	public CreatureModel ReplaceCreature_Mod(LobotomyBaseMod.LcIdLong metadataId, CreatureModel exist)
	{
		long instanceId;
		this.nextInstId = (instanceId = this.nextInstId) + 1L;
		CreatureModel creatureModel = new CreatureModel(instanceId);
		this.ReplaceBuildCreatureModel_Mod(creatureModel, metadataId, exist);
		this.ReplaceCommand(exist, creatureModel);
		this.UnRegisterCreature(exist);
		this.RegisterByReplace(creatureModel);
		creatureModel.script.OnInit();
		return creatureModel;
	}

	// <Patch>
	public void ReplaceBuildCreatureModel_Mod(CreatureModel model, LobotomyBaseMod.LcIdLong metadataId, CreatureModel old)
	{
		bool flag = metadataId.packageId == string.Empty;
		if (flag)
		{
			bool flag2 = this.observeInfoList.ContainsKey(metadataId.id);
			if (flag2)
			{
				this.observeInfoList.TryGetValue(metadataId.id, out model.observeInfo);
			}
			else
			{
				model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
				model.observeInfo.Init_Mod(metadataId);
				this.observeInfoList.Add(metadataId.id, model.observeInfo);
			}
		}
		else
		{
			bool flag3 = this.observeInfoList_mod.ContainsKey(metadataId);
			if (flag3)
			{
				this.observeInfoList_mod.TryGetValue(metadataId, out model.observeInfo);
			}
			else
			{
				model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
				model.observeInfo.Init_Mod(metadataId);
				this.observeInfoList_mod.Add(metadataId, model.observeInfo);
			}
		}
		model.sefiraOrigin = old.sefiraOrigin;
		model.sefira = old.sefira;
		model.sefiraNum = old.sefiraNum;
		SefiraIsolate isolateRoomData = old.isolateRoomData;
		model.specialSkillPos = isolateRoomData.pos;
		model.isolateRoomData = isolateRoomData;
		CreatureTypeInfo data_Mod = CreatureTypeList.instance.GetData_Mod(metadataId);
		model.metadataId = metadataId.id;
		model.metaInfo = data_Mod;
		bool flag4 = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId) != null;
		if (flag4)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId).GetCopy();
		}
		model.basePosition = new Vector2(isolateRoomData.x, isolateRoomData.y);
		model.script = LobotomyBaseMod.ExtenionUtil.GetTypeInstance<CreatureBase>(data_Mod.script);
		model.script.SetModel(model);
		model.entryNodeId = isolateRoomData.nodeId;
		MapNode nodeById = MapGraph.instance.GetNodeById(isolateRoomData.nodeId);
		model.entryNode = nodeById;
		nodeById.connectedCreature = model;
		old.CopyNodeData(model);
		model.script.OnInitialBuild();
	}

	// <Patch>
	public bool IsCreatureActivated_Mod(LobotomyBaseMod.LcIdLong metaId)
	{
		bool result = false;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			bool flag = creatureModel.metadataId == metaId.id && CreatureTypeList.instance.GetModId(creatureModel.metaInfo) == metaId.packageId;
			if (flag)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// <Patch>
	public int GetObserveLevel_Mod(LobotomyBaseMod.LcIdLong metadataId)
	{
		CreatureObserveInfoModel creatureObserveInfoModel = null;
		bool flag = metadataId.IsBasic();
		if (flag)
		{
			bool flag2 = this.observeInfoList.TryGetValue(metadataId.id, out creatureObserveInfoModel);
			if (flag2)
			{
				return creatureObserveInfoModel.GetObservationLevel();
			}
		}
		bool flag3 = this.observeInfoList_mod.TryGetValue(metadataId, out creatureObserveInfoModel);
		int result;
		if (flag3)
		{
			result = creatureObserveInfoModel.GetObservationLevel();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	// <Patch>
	public CreatureObserveInfoModel GetObserveInfo_Mod(LobotomyBaseMod.LcIdLong metadataId)
	{
		CreatureObserveInfoModel creatureObserveInfoModel = null;
		bool flag = metadataId.IsBasic();
		if (flag)
		{
			bool flag2 = this.observeInfoList.TryGetValue(metadataId.id, out creatureObserveInfoModel);
			if (flag2)
			{
				return creatureObserveInfoModel;
			}
		}
		bool flag3 = this.observeInfoList_mod.TryGetValue(metadataId, out creatureObserveInfoModel);
		CreatureObserveInfoModel result;
		if (flag3)
		{
			result = creatureObserveInfoModel;
		}
		else
		{
			result = null;
		}
		return result;
	}

	// <Patch>
	public CreatureModel GetCreature_Mod(LobotomyBaseMod.LcIdLong id)
	{
		CreatureModel result = null;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			LobotomyBaseMod.LcIdLong lhs = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel.metaInfo), creatureModel.metadataId);
			bool flag = lhs == id;
			if (flag)
			{
				result = creatureModel;
				break;
			}
		}
		return result;
	}

	// <Patch>
	public CreatureModel FindCreature_Mod(LobotomyBaseMod.LcIdLong metaId)
	{
		CreatureModel result = null;
		foreach (CreatureModel creatureModel in this.creatureList)
		{
			LobotomyBaseMod.LcIdLong lhs = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel.metaInfo), creatureModel.metadataId);
			bool flag = lhs == metaId;
			if (flag)
			{
				result = creatureModel;
				break;
			}
		}
		return result;
	}

	// <Patch>
	public CreatureModel AddCreature_Mod(LobotomyBaseMod.LcIdLong metadataId, SefiraIsolate roomData, string sefiraNum)
	{ // <Mod>
		long instanceId;
		this.nextInstId = (instanceId = this.nextInstId) + 1L;
		CreatureModel creatureModel = new CreatureModel(instanceId);
		this.BuildCreatureModel_Mod(creatureModel, metadataId, roomData, sefiraNum);
		if (roomData != null) this.AddCreatureInSefira(creatureModel, sefiraNum);
		this.RegisterCreature(creatureModel);
		creatureModel.script.OnInit();
		creatureModel.script.OnInitialBuild();
		return creatureModel;
	}

	// <Patch>
	public void BuildCreatureModel_Mod(CreatureModel model, LobotomyBaseMod.LcIdLong metadataId, SefiraIsolate roomData, string sefiraNum)
	{ // <Mod>
		CreatureTypeInfo data_Mod = CreatureTypeList.instance.GetData_Mod(metadataId);
		if (AprilFoolsManager.instance.IsEventActive("AllDTM")) data_Mod = CreatureTypeList.instance.GetData_Mod(new LobotomyBaseMod.LcIdLong(100024L));
		bool flag = data_Mod == null;
		if (!flag)
		{
			object obj = null;
			foreach (Assembly assembly in Add_On.instance.AssemList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					bool flag2 = type.Name == data_Mod.script;
					if (flag2)
					{
						obj = Activator.CreateInstance(type);
					}
				}
			}
			bool flag3 = obj == null;
			if (flag3)
			{
				obj = Activator.CreateInstance(Type.GetType(data_Mod.script));
			}
			model.script = (CreatureBase)obj;
			bool flag4 = metadataId.packageId == string.Empty;
			if (flag4)
			{
				bool flag5 = this.observeInfoList.ContainsKey(metadataId.id);
				if (flag5)
				{
					this.observeInfoList.TryGetValue(metadataId.id, out model.observeInfo);
				}
				else
				{
					model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
					model.observeInfo.Init_Mod(metadataId);
					this.observeInfoList.Add(metadataId.id, model.observeInfo);
				}
			}
			else
			{
				bool flag6 = this.observeInfoList_mod.ContainsKey(metadataId);
				if (flag6)
				{
					this.observeInfoList_mod.TryGetValue(metadataId, out model.observeInfo);
				}
				else
				{
					model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
					model.observeInfo.Init_Mod(metadataId);
					this.observeInfoList_mod.Add(metadataId, model.observeInfo);
				}
			}
			model.sefira = (model.sefiraOrigin = SefiraManager.instance.GetSefira(sefiraNum));
			model.sefiraNum = sefiraNum;
			if (roomData != null)
			{
				model.specialSkillPos = roomData.pos;
				model.isolateRoomData = roomData;
			}
			model.metadataId = metadataId.id;
			model.metaInfo = data_Mod;
			bool flag7 = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId) != null;
			if (flag7)
			{
				model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId).GetCopy();
			}
			if (roomData != null) model.basePosition = new Vector2(roomData.x, roomData.y);
			model.script.SetModel(model);
			MapNode nodeById = null;
			if (roomData != null)
			{
				model.entryNodeId = roomData.nodeId;
				nodeById = MapGraph.instance.GetNodeById(roomData.nodeId);
				model.entryNode = nodeById;
				nodeById.connectedCreature = model;
			}
			Dictionary<string, MapNode> dictionary = new Dictionary<string, MapNode>();
			List<MapEdge> list = new List<MapEdge>();
			MapNode mapNode = null;
			if (roomData != null)
			{
				PassageObjectModel passageObjectModel = new PassageObjectModel(roomData.nodeId + "@creature", nodeById.GetAreaName(), "Map/Passage/PassageEmpty");
				passageObjectModel.isDynamic = true;
				passageObjectModel.Activate();
				passageObjectModel.scaleFactor = 0.75f;
				passageObjectModel.SetToIsolate();
				passageObjectModel.position = new Vector3(roomData.x, roomData.y, 0f);
				passageObjectModel.type = PassageType.ISOLATEROOM;
				IEnumerator enumerator2 = data_Mod.nodeInfo.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode = (XmlNode)obj2;
						string text = roomData.nodeId + "@" + xmlNode.Attributes.GetNamedItem("id").InnerText;
						float x = model.basePosition.x + float.Parse(xmlNode.Attributes.GetNamedItem("x").InnerText);
						float y = model.basePosition.y + float.Parse(xmlNode.Attributes.GetNamedItem("y").InnerText);
						XmlNode namedItem = xmlNode.Attributes.GetNamedItem("type");
						bool flag8 = namedItem != null && namedItem.InnerText == "workspace";
						MapNode mapNode2;
						if (flag8)
						{
							mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
							passageObjectModel.AddNode(mapNode2);
							model.SetWorkspaceNode(mapNode2);
						}
						else
						{
							bool flag9 = namedItem != null && namedItem.InnerText == "custom";
							if (flag9)
							{
								mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
								passageObjectModel.AddNode(mapNode2);
								model.SetCustomNode(mapNode2);
							}
							else
							{
								bool flag10 = namedItem != null && namedItem.InnerText == "creature";
								if (flag10)
								{
									mapNode2 = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel);
									passageObjectModel.AddNode(mapNode2);
									model.SetRoomNode(mapNode2);
									model.SetCurrentNode(mapNode2);
								}
								else
								{
									bool flag11 = namedItem == null || !(namedItem.InnerText == "innerDoor");
									if (flag11)
									{
										continue;
									}
									mapNode2 = (mapNode = new MapNode(text, new Vector2(x, y), nodeById.GetAreaName(), passageObjectModel));
									passageObjectModel.AddNode(mapNode2);
									DoorObjectModel doorObjectModel = new DoorObjectModel(string.Concat(new object[]
									{
										nodeById,
										"@",
										text,
										"@inner"
									}), "DoorIsolate", passageObjectModel, mapNode);
									doorObjectModel.position = new Vector3(mapNode.GetPosition().x, mapNode.GetPosition().y, -0.01f);
									passageObjectModel.AddDoor(doorObjectModel);
									mapNode.SetDoor(doorObjectModel);
									doorObjectModel.Close();
								}
							}
						}
						dictionary.Add(text, mapNode2);
					}
				}
				finally
				{
					IDisposable disposable;
					bool flag12 = (disposable = (enumerator2 as IDisposable)) != null;
					if (flag12)
					{
						disposable.Dispose();
					}
				}
				PassageObjectModel attachedPassage = nodeById.GetAttachedPassage();
				MapNode mapNode3 = new MapNode(roomData.nodeId + "@outter", new Vector2(nodeById.GetPosition().x, nodeById.GetPosition().y), nodeById.GetAreaName(), attachedPassage);
				string id = roomData.nodeId + "@outterDoor";
				string type2 = "MalkuthDoorMiddle";
				switch (model.sefira.sefiraEnum)
				{
				case SefiraEnum.MALKUT:
					type2 = "MalkuthDoorMiddle";
					break;
				case SefiraEnum.YESOD:
					type2 = "YesodDoorMiddle";
					break;
				case SefiraEnum.HOD:
					type2 = "HodDoorMiddle";
					break;
				case SefiraEnum.NETZACH:
					type2 = "NetzachDoorMiddle";
					break;
				case SefiraEnum.TIPERERTH1:
				case SefiraEnum.TIPERERTH2:
					type2 = "TipherethDoorMiddle";
					break;
				case SefiraEnum.GEBURAH:
					type2 = "GeburahDoorMiddle";
					break;
				case SefiraEnum.CHESED:
					type2 = "ChesedDoorMiddle";
					break;
				case SefiraEnum.BINAH:
					type2 = "BinahDoorMiddle";
					break;
				case SefiraEnum.CHOKHMAH:
					type2 = "ChokhmahDoorMiddle";
					break;
				case SefiraEnum.KETHER:
					type2 = "KetherDoorMiddle";
					break;
				}
				DoorObjectModel doorObjectModel2 = new DoorObjectModel(id, type2, attachedPassage, mapNode3);
				doorObjectModel2.position = new Vector3(mapNode3.GetPosition().x, mapNode3.GetPosition().y, -0.01f);
				attachedPassage.AddDoor(doorObjectModel2);
				mapNode3.SetDoor(doorObjectModel2);
				doorObjectModel2.Close();
				attachedPassage.AddNode(mapNode3);
				MapEdge mapEdge = new MapEdge(mapNode3, nodeById, "road");
				list.Add(mapEdge);
				mapNode3.AddEdge(mapEdge);
				nodeById.AddEdge(mapEdge);
				bool flag13 = mapNode != null;
				if (flag13)
				{
					MapEdge mapEdge2 = new MapEdge(mapNode3, mapNode, "door", 0.01f);
					doorObjectModel2.Connect(mapNode.GetDoor());
					list.Add(mapEdge2);
					mapNode3.AddEdge(mapEdge2);
					mapNode.AddEdge(mapEdge2);
				}
				dictionary.Add(mapNode3.GetId(), mapNode3);
				bool flag14 = model.GetCustomNode() == null;
				if (flag14)
				{
					model.SetCustomNode(model.GetCurrentNode());
				}
				IEnumerator enumerator3 = data_Mod.edgeInfo.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						XmlNode xmlNode2 = (XmlNode)obj3;
						string text2 = roomData.nodeId + "@" + xmlNode2.Attributes.GetNamedItem("node1").InnerText;
						string text3 = roomData.nodeId + "@" + xmlNode2.Attributes.GetNamedItem("node2").InnerText;
						string innerText = xmlNode2.Attributes.GetNamedItem("type").InnerText;
						MapNode mapNode4 = null;
						MapNode mapNode5 = null;
						bool flag15 = !dictionary.TryGetValue(text2, out mapNode4) || !dictionary.TryGetValue(text3, out mapNode5);
						if (flag15)
						{
							Debug.Log(string.Concat(new string[]
							{
								"cannot create edge - (",
								text2,
								", ",
								text3,
								")"
							}));
						}
						XmlNode namedItem2 = xmlNode2.Attributes.GetNamedItem("cost");
						bool flag16 = namedItem2 != null;
						MapEdge mapEdge3;
						if (flag16)
						{
							mapEdge3 = new MapEdge(mapNode4, mapNode5, innerText, float.Parse(namedItem2.InnerText));
						}
						else
						{
							mapEdge3 = new MapEdge(mapNode4, mapNode5, innerText);
						}
						list.Add(mapEdge3);
						mapNode4.AddEdge(mapEdge3);
						mapNode5.AddEdge(mapEdge3);
					}
				}
				finally
				{
					IDisposable disposable2;
					bool flag17 = (disposable2 = (enumerator3 as IDisposable)) != null;
					if (flag17)
					{
						disposable2.Dispose();
					}
				}
				MapGraph.instance.RegisterPassage(passageObjectModel);
			}
		}
	}

	// Token: 0x04005273 RID: 21107
	private static CreatureManager _instance;

	// Token: 0x04005274 RID: 21108
	public GameObject creatureListNode;

	// Token: 0x04005275 RID: 21109
	private List<CreatureModel> creatureList;

	// Token: 0x04005276 RID: 21110
	private Dictionary<long, CreatureObserveInfoModel> observeInfoList;

	// Token: 0x04005277 RID: 21111
	private Dictionary<long, CreatureSpecialSkillTipTable> specialSkillTable;

	// Token: 0x04005278 RID: 21112
	private long nextInstId = 1L;

	// <Patch>
	public Dictionary<LobotomyBaseMod.LcIdLong, CreatureObserveInfoModel> observeInfoList_mod;
}
