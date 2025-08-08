using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class MapGraph : IObserver
{
	// Token: 0x0600325B RID: 12891 RVA: 0x0002E867 File Offset: 0x0002CA67
	public MapGraph()
	{
		this.loaded = false;
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x0600325C RID: 12892 RVA: 0x0002E876 File Offset: 0x0002CA76
	public static MapGraph instance
	{
		get
		{
			if (MapGraph._instance == null)
			{
				MapGraph._instance = new MapGraph();
			}
			return MapGraph._instance;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x0600325E RID: 12894 RVA: 0x0002E89A File Offset: 0x0002CA9A
	// (set) Token: 0x0600325D RID: 12893 RVA: 0x0002E891 File Offset: 0x0002CA91
	public bool loaded { get; private set; }

	// Token: 0x0600325F RID: 12895 RVA: 0x00154A10 File Offset: 0x00152C10
	public MapNode GetNodeById(string id)
	{
		MapNode result = null;
		this.graphNodes.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x00154A30 File Offset: 0x00152C30
	public MapNode GetCreatureRoamingPoint()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira sefira = openedAreaList[UnityEngine.Random.Range(0, openedAreaList.Length)];
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		foreach (PassageObjectModel passageObjectModel in sefira.passageList)
		{
			if (passageObjectModel.isActivate)
			{
				if (passageObjectModel.type == PassageType.HORIZONTAL || passageObjectModel.type == PassageType.SEFIRA || passageObjectModel.type == PassageType.DEPARTMENT)
				{
					list.Add(passageObjectModel);
				}
			}
		}
		PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
		IList<MapNode> nodeListAsReadOnly = passageObjectModel2.GetNodeListAsReadOnly();
		return nodeListAsReadOnly[UnityEngine.Random.Range(0, nodeListAsReadOnly.Count)];
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x00154B10 File Offset: 0x00152D10
	public MapNode GetRoamingNodeByRandom(string area)
	{
		MapSefiraArea mapSefiraArea = null;
		this.mapAreaTable.TryGetValue(area, out mapSefiraArea);
		PassageObjectModel[] roamingPassageList = mapSefiraArea.GetRoamingPassageList();
		PassageObjectModel passageObjectModel = roamingPassageList[UnityEngine.Random.Range(0, roamingPassageList.Length)];
		MapNode[] nodeList = passageObjectModel.GetNodeList();
		return nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x00154B58 File Offset: 0x00152D58
	public MapNode GetRoamingNodeByRandom()
	{
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		foreach (KeyValuePair<string, PassageObjectModel> keyValuePair in this.passageTable)
		{
			if (keyValuePair.Value.isActivate)
			{
				if (keyValuePair.Value.type == PassageType.HORIZONTAL || keyValuePair.Value.type == PassageType.SEFIRA || keyValuePair.Value.type == PassageType.DEPARTMENT)
				{
					list.Add(keyValuePair.Value);
				}
			}
		}
		PassageObjectModel passageObjectModel = list[UnityEngine.Random.Range(0, list.Count)];
		MapNode[] nodeList = passageObjectModel.GetNodeList();
		return nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x00154C38 File Offset: 0x00152E38
	public MapNode GetSepiraNodeByRandom(string area)
	{
		MapNode[] sefiraNodes = this.GetSefiraNodes(area);
		if (sefiraNodes.Length == 0)
		{
			return this.GetNodeById("sefira-malkuth-5");
		}
		return sefiraNodes[UnityEngine.Random.Range(0, sefiraNodes.Length)];
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x00154C6C File Offset: 0x00152E6C
	public MapNode GetSefiraAndDeptByRandom(string area)
	{
		List<MapNode> list = new List<MapNode>();
		list.AddRange(this.GetSefiraNodes(area));
		list.AddRange(this.GetAdditionalSefira(area));
		if (list.Count == 0)
		{
			return this.GetNodeById("sefira-malkuth-5");
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x00154CC4 File Offset: 0x00152EC4
	public MovableObjectNode GetSefiraMovableNodeByRandom(string area)
	{
		MapNode mapNode = null;
		MapNode mapNode2 = null;
		if (area == "1")
		{
			mapNode = this.GetNodeById("sefira-malkuth-2");
			mapNode2 = this.GetNodeById("sefira-malkuth-8");
		}
		else if (area == "2")
		{
			mapNode = this.GetNodeById("sefira-netzach-2");
			mapNode2 = this.GetNodeById("sefira-netzach-8");
		}
		else if (area == "3")
		{
			mapNode = this.GetNodeById("sefira-hod-2");
			mapNode2 = this.GetNodeById("sefira-hod-8");
		}
		else if (area == "4")
		{
			mapNode = this.GetNodeById("sefira-tessod-2");
			mapNode2 = this.GetNodeById("sefira-tessod-8");
		}
		else if (area == "5")
		{
			mapNode = this.GetNodeById("sefira-tiphereth1-2");
			mapNode2 = this.GetNodeById("sefira-tiphereth1-8");
		}
		else if (area == "6")
		{
			mapNode = this.GetNodeById("sefira-tiphereth2-2");
			mapNode2 = this.GetNodeById("sefira-tiphereth2-8");
		}
		else if (area == "7")
		{
			mapNode = this.GetNodeById("sefira-geburah-2");
			mapNode2 = this.GetNodeById("sefira-geburah-5");
		}
		else if (area == "8")
		{
			mapNode = this.GetNodeById("sefira-chesed-2");
			mapNode2 = this.GetNodeById("sefira-chesed-5");
		}
		else if (area == "9")
		{
			mapNode = this.GetNodeById("sefira-binah-2");
			mapNode2 = this.GetNodeById("sefira-binah-5");
		}
		else if (area == "10")
		{
			mapNode = this.GetNodeById("sefira-chokhmah-2");
			mapNode2 = this.GetNodeById("sefira-chokhmah-5");
		}
		else if (area == "11")
		{
			mapNode = this.GetNodeById("sefira-keter2");
			mapNode2 = this.GetNodeById("sefira-keter9");
		}
		else if (area == "12")
		{
			mapNode = this.GetNodeById("sefira-extra-2");
			mapNode2 = this.GetNodeById("sefira-extra-9");
		}
		if (mapNode == null || mapNode2 == null)
		{
			Debug.LogError("GetSefiraMovableNodeByRandom >> area is invalid");
			mapNode = this.GetNodeById("sefira-malkuth-2");
			mapNode2 = this.GetNodeById("sefira-malkuth-8");
		}
		if (mapNode == null || mapNode2 == null)
		{
			UnityEngine.Random.Range(0, this.sefiraPassageTable[area].Count);
			mapNode = this.sefiraPassageTable[area][UnityEngine.Random.Range(0, this.sefiraPassageTable[area].Count)];
			mapNode2 = this.sefiraPassageTable[area][UnityEngine.Random.Range(0, this.sefiraPassageTable[area].Count)];
		}
		PathResult pathResult = GraphAstar.SearchPath(mapNode, mapNode2, false);
		if (pathResult == null)
		{
			return null;
		}
		float num = UnityEngine.Random.Range(0f, pathResult.totalCost);
		MovableObjectNode movableObjectNode = null;
		float num2 = num;
		for (int i = 0; i < pathResult.pathEdges.Length; i++)
		{
			MapEdge mapEdge = pathResult.pathEdges[i];
			if (num2 <= mapEdge.cost)
			{
				movableObjectNode = new MovableObjectNode(false);
				movableObjectNode.SetCurrentEdge(mapEdge, num2 / mapEdge.cost, pathResult.edgeDirections[i]);
				break;
			}
			num2 -= mapEdge.cost;
		}
		return movableObjectNode;
	}

	// Token: 0x06003266 RID: 12902 RVA: 0x00154FD8 File Offset: 0x001531D8
	public MapNode[] GetSefiraNodes(string area)
	{
		List<MapNode> list;
		if (this.sefiraCoreNodesTable.TryGetValue(area, out list))
		{
			return list.ToArray();
		}
		return new MapNode[0];
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x0002E8A2 File Offset: 0x0002CAA2
	public MapNode[] GetSefiraNodes(Sefira sefira)
	{
		return this.GetSefiraNodes(sefira.indexString);
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x00155008 File Offset: 0x00153208
	public PassageObjectModel GetSefiraPassage(string area)
	{
		MapNode[] sefiraNodes = this.GetSefiraNodes(area);
		return sefiraNodes[0].GetAttachedPassage();
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x00155028 File Offset: 0x00153228
	public MapNode[] GetAdditionalSefira(string area)
	{
		List<MapNode> list;
		if (this.additionalSefiraTable.TryGetValue(area, out list))
		{
			List<MapNode> list2 = new List<MapNode>();
			foreach (MapNode mapNode in list)
			{
				if (mapNode.activate)
				{
					list2.Add(mapNode);
				}
			}
			return list2.ToArray();
		}
		return new MapNode[0];
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x0002E8B0 File Offset: 0x0002CAB0
	public MapNode[] GetAdditionalSefira(Sefira sefira)
	{
		return this.GetAdditionalSefira(sefira.indexString);
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x001550B0 File Offset: 0x001532B0
	public void ActivateArea(string name)
	{
		MapSefiraArea mapSefiraArea;
		if (this.mapAreaTable.TryGetValue(name, out mapSefiraArea))
		{
			mapSefiraArea.ActivateArea();
		}
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x001550D8 File Offset: 0x001532D8
	public Dictionary<string, List<string>> GetActivatedAreaList()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (KeyValuePair<string, MapSefiraArea> keyValuePair in this.mapAreaTable)
		{
			List<string> activatedAreas = keyValuePair.Value.GetActivatedAreas();
			if (activatedAreas.Count > 0)
			{
				dictionary.Add(keyValuePair.Key, activatedAreas);
			}
		}
		return dictionary;
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x0015515C File Offset: 0x0015335C
	public void ActivateArea(string name, string passageGroupName)
	{
		MapSefiraArea mapSefiraArea;
		if (this.mapAreaTable.TryGetValue(name, out mapSefiraArea))
		{
			mapSefiraArea.ActivateArea(passageGroupName);
		}
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x00155184 File Offset: 0x00153384
	public void DeactivateArea(string name)
	{
		MapSefiraArea mapSefiraArea;
		if (this.mapAreaTable.TryGetValue(name, out mapSefiraArea))
		{
			mapSefiraArea.DeactivateArea();
		}
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x001551AC File Offset: 0x001533AC
	public void DeactivateAll()
	{
		foreach (MapSefiraArea mapSefiraArea in this.mapAreaTable.Values)
		{
			mapSefiraArea.InitActivates();
		}
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x0015520C File Offset: 0x0015340C
	public void LoadMap()
	{
		if (this.loaded)
		{
			return;
		}
		string path = Application.dataPath + "/Managed/BaseMod/BaseMapGraph.txt";
		if (!File.Exists(path))
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/MapGraph_final2");
			File.WriteAllText(path, textAsset.text);
		}
		string xml = File.ReadAllText(path);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode nodeRoot = xmlDocument.SelectSingleNode("/map_graph/node_list");
		XmlNode edgeRoot = xmlDocument.SelectSingleNode("/map_graph/edge_list");
		this.LoadMap(nodeRoot, edgeRoot);
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x00155288 File Offset: 0x00153488
	public void LoadMap(XmlNode nodeRoot, XmlNode edgeRoot)
	{
		try
		{
			int num = 1;
			XmlNodeList xmlNodeList = nodeRoot.SelectNodes("area");
			Dictionary<string, XmlNode> dictionary = new Dictionary<string, XmlNode>();
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("name").InnerText;
				dictionary.Add(innerText, xmlNode);
			}
			this.LoadModMap(dictionary, edgeRoot);
			edgeRoot.OwnerDocument.Save(Application.dataPath + "/Managed/BaseMod/NewMap.xml");
			Dictionary<string, MapNode> dictionary2 = new Dictionary<string, MapNode>();
			Dictionary<string, List<MapNode>> dictionary3 = new Dictionary<string, List<MapNode>>();
			Dictionary<string, List<MapNode>> dictionary4 = new Dictionary<string, List<MapNode>>();
			Dictionary<string, List<MapNode>> dictionary5 = new Dictionary<string, List<MapNode>>();
			Dictionary<string, List<MapNode>> dictionary6 = new Dictionary<string, List<MapNode>>();
			Dictionary<string, List<MapNode>> dictionary7 = new Dictionary<string, List<MapNode>>();
			Dictionary<string, MapSefiraArea> dictionary8 = new Dictionary<string, MapSefiraArea>();
			Dictionary<string, PassageObjectModel> dictionary9 = new Dictionary<string, PassageObjectModel>();
			Dictionary<string, List<PassageObjectModel>> dictionary10 = new Dictionary<string, List<PassageObjectModel>>();
			List<MapNode> list = new List<MapNode>();
			IEnumerator enumerator2 = dictionary.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					XmlNode xmlNode2 = (XmlNode)obj2;
					MapSefiraArea mapSefiraArea = new MapSefiraArea();
					List<MapNode> list2 = new List<MapNode>();
					List<MapNode> list3 = new List<MapNode>();
					List<MapNode> list4 = new List<MapNode>();
					List<MapNode> list5 = new List<MapNode>();
					List<MapNode> list6 = new List<MapNode>();
					string innerText2 = xmlNode2.Attributes.GetNamedItem("name").InnerText;
					mapSefiraArea.sefiraName = innerText2;
					int.Parse(xmlNode2.Attributes.GetNamedItem("sub").InnerText);
					Sefira sefira = SefiraManager.instance.GetSefira(innerText2);
					IEnumerator enumerator3 = xmlNode2.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							XmlNode xmlNode3 = (XmlNode)obj3;
							if (xmlNode3.Name == "passage")
							{
								string text = "group@" + num;
								num++;
								XmlAttributeCollection attributes = xmlNode3.Attributes;
								XmlNode namedItem = attributes.GetNamedItem("src");
								XmlNode namedItem2 = attributes.GetNamedItem("x");
								XmlNode namedItem3 = attributes.GetNamedItem("y");
								XmlNode namedItem4 = attributes.GetNamedItem("passageGroup");
								XmlNode namedItem5 = attributes.GetNamedItem("rabbitTeamGroup");
								XmlNode namedItem6 = attributes.GetNamedItem("scale");
								PassageObjectModel passageObjectModel = null;
								float x = 0f;
								float y = 0f;
								if (namedItem2 != null)
								{
									x = float.Parse(namedItem2.InnerText);
								}
								if (namedItem3 != null)
								{
									y = float.Parse(namedItem3.InnerText);
								}
								if (namedItem != null)
								{
									passageObjectModel = new PassageObjectModel(text, innerText2, namedItem.InnerText);
								}
								else
								{
									passageObjectModel = new PassageObjectModel(text, innerText2);
								}
								if (namedItem4 != null)
								{
									passageObjectModel.passageGroup = namedItem4.InnerText;
								}
								if (namedItem5 != null)
								{
									passageObjectModel.rabbitTeamGroup = namedItem5.InnerText;
								}
								if (namedItem6 != null)
								{
									passageObjectModel.scaleFactor = float.Parse(namedItem6.InnerText);
								}
								passageObjectModel.position = new Vector3(x, y, 0f);
								XmlNode namedItem7 = attributes.GetNamedItem("passageType");
								if (namedItem7 != null)
								{
									PassageType passageTypeByString = PassageObjectModel.GetPassageTypeByString(namedItem7.InnerText);
									passageObjectModel.SetPassageType(passageTypeByString);
								}
								XmlNode xmlNode4 = xmlNode3.SelectSingleNode("ground");
								XmlNode xmlNode5 = xmlNode3.SelectSingleNode("wall");
								XmlNode xmlNode6 = xmlNode3.SelectSingleNode("connected");
								XmlNode xmlNode7 = xmlNode3.SelectSingleNode("height");
								if (xmlNode6 != null)
								{
									XmlNodeList xmlNodeList2 = xmlNode6.SelectNodes("connect");
									if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
									{
										IEnumerator enumerator4 = xmlNodeList2.GetEnumerator();
										try
										{
											while (enumerator4.MoveNext())
											{
												object obj4 = enumerator4.Current;
												XmlNode xmlNode8 = (XmlNode)obj4;
												passageObjectModel.AddAttachedSefira(xmlNode8.InnerText);
											}
										}
										finally
										{
											IDisposable disposable2;
											if ((disposable2 = (enumerator4 as IDisposable)) != null)
											{
												disposable2.Dispose();
											}
										}
									}
								}
								if (xmlNode7 != null)
								{
									float height = float.Parse(xmlNode7.InnerText);
									passageObjectModel.height = height;
								}
								if (passageObjectModel.height == 0f)
								{
									passageObjectModel.height = 2.5f;
								}
								if (xmlNode4 != null)
								{
									PassageGroundInfo passageGroundInfo = new PassageGroundInfo();
									XmlNode namedItem8 = xmlNode4.Attributes.GetNamedItem("height");
									if (namedItem8 != null)
									{
										passageGroundInfo.height = float.Parse(namedItem8.InnerText);
									}
									string[] array = new string[]
									{
										"Sprites/Blood/blood_ground_00",
										"Sprites/Blood/blood_ground_01",
										"Sprites/Blood/blood_ground_02",
										"Sprites/Blood/blood_ground_03",
										"Sprites/Blood/blood_ground_04"
									};
									string[] array2 = new string[]
									{
										"Sprites/Blood/Creature/blood_ground_00",
										"Sprites/Blood/Creature/blood_ground_01",
										"Sprites/Blood/Creature/blood_ground_02",
										"Sprites/Blood/Creature/blood_ground_03",
										"Sprites/Blood/Creature/blood_ground_04"
									};
									foreach (string name in array)
									{
										Sprite sprite = ResourceCache.instance.GetSprite(name);
										passageGroundInfo.bloodSprites.Add(sprite);
									}
									foreach (string name2 in array2)
									{
										Sprite sprite2 = ResourceCache.instance.GetSprite(name2);
										passageGroundInfo.alterSprites.Add(sprite2);
									}
									passageObjectModel.groundInfo = passageGroundInfo;
								}
								if (xmlNode5 != null)
								{
									PassageWallInfo passageWallInfo = new PassageWallInfo();
									XmlNode namedItem9 = xmlNode5.Attributes.GetNamedItem("height");
									if (namedItem9 != null)
									{
										passageWallInfo.height = float.Parse(namedItem9.InnerText);
									}
									string[] array4 = new string[]
									{
										"Sprites/Blood/blood_wall_00",
										"Sprites/Blood/blood_wall_01",
										"Sprites/Blood/blood_wall_02",
										"Sprites/Blood/blood_wall_03",
										"Sprites/Blood/blood_wall_04"
									};
									string[] array5 = new string[]
									{
										"Sprites/Blood/Creature/blood_ground_00",
										"Sprites/Blood/Creature/blood_ground_01",
										"Sprites/Blood/Creature/blood_ground_02",
										"Sprites/Blood/Creature/blood_ground_03",
										"Sprites/Blood/Creature/blood_ground_04"
									};
									foreach (string name3 in array4)
									{
										Sprite sprite3 = ResourceCache.instance.GetSprite(name3);
										passageWallInfo.bloodSprites.Add(sprite3);
									}
									foreach (string name4 in array5)
									{
										Sprite sprite4 = ResourceCache.instance.GetSprite(name4);
										passageWallInfo.alterSprites.Add(sprite4);
									}
									passageObjectModel.wallInfo = passageWallInfo;
								}
								IEnumerator enumerator5 = xmlNode3.SelectNodes("node").GetEnumerator();
								try
								{
									while (enumerator5.MoveNext())
									{
										object obj5 = enumerator5.Current;
										XmlNode xmlNode9 = (XmlNode)obj5;
										string innerText3 = xmlNode9.Attributes.GetNamedItem("id").InnerText;
										float x2 = float.Parse(xmlNode9.Attributes.GetNamedItem("x").InnerText);
										float y2 = float.Parse(xmlNode9.Attributes.GetNamedItem("y").InnerText);
										XmlNode namedItem10 = xmlNode9.Attributes.GetNamedItem("rabbitUnpassable");
										xmlNode9.Attributes.GetNamedItem("type");
										MapNode mapNode = new MapNode(innerText3, new Vector2(x2, y2), innerText2, passageObjectModel);
										if (namedItem10 != null)
										{
											mapNode.rabbitUnpassable = true;
										}
										XmlNode namedItem11 = xmlNode9.Attributes.GetNamedItem("elevator");
										if (namedItem11 != null)
										{
											if (passageObjectModel.type != PassageType.VERTICAL)
											{
												Debug.LogError("elevator passage's type must be VERTICAL");
											}
											ElevatorPassageModel elevatorPassageModel = new ElevatorPassageModel(mapNode, passageObjectModel, namedItem11.InnerText);
											MapNode mapNode2 = new MapNode("elevator1-" + innerText3, new Vector3(-1f, -0.8f, 0f), innerText2, passageObjectModel);
											MapNode mapNode3 = new MapNode("elevator2-" + innerText3, new Vector3(-0.5f, -0.8f, 0f), innerText2, passageObjectModel);
											MapNode mapNode4 = new MapNode("elevator3-" + innerText3, new Vector3(0f, -0.8f, 0f), innerText2, passageObjectModel);
											MapNode mapNode5 = new MapNode("elevator4-" + innerText3, new Vector3(0.5f, -0.8f, 0f), innerText2, passageObjectModel);
											MapNode mapNode6 = new MapNode("elevator5-" + innerText3, new Vector3(1f, -0.8f, 0f), innerText2, passageObjectModel);
											elevatorPassageModel.AddNode(mapNode2);
											elevatorPassageModel.AddNode(mapNode3);
											elevatorPassageModel.AddNode(mapNode4);
											elevatorPassageModel.AddNode(mapNode5);
											elevatorPassageModel.AddNode(mapNode6);
											dictionary2.Add(mapNode2.GetId(), mapNode2);
											dictionary2.Add(mapNode3.GetId(), mapNode3);
											dictionary2.Add(mapNode4.GetId(), mapNode4);
											dictionary2.Add(mapNode5.GetId(), mapNode5);
											dictionary2.Add(mapNode6.GetId(), mapNode6);
											mapNode.AttachElevator(elevatorPassageModel);
											list.Add(mapNode);
										}
										XmlNode namedItem12 = xmlNode9.Attributes.GetNamedItem("pos");
										if (namedItem12 != null && namedItem12.InnerText == "center")
										{
											passageObjectModel.centerNode = mapNode;
										}
										mapNode.activate = false;
										dictionary2.Add(innerText3, mapNode);
										list5.Add(mapNode);
										if (passageObjectModel != null && passageObjectModel.GetPassageType() == PassageType.SEFIRA)
										{
											list2.Add(mapNode);
										}
										else if (passageObjectModel != null && passageObjectModel.GetPassageType() == PassageType.DEPARTMENT)
										{
											list3.Add(mapNode);
										}
										else
										{
											list6.Add(mapNode);
										}
										MapNode mapNode7 = null;
										XmlNode xmlNode10 = xmlNode9.SelectSingleNode("door");
										if (xmlNode10 != null)
										{
											DoorObjectModel doorObjectModel = new DoorObjectModel(innerText3 + "@door", xmlNode10.InnerText, passageObjectModel, mapNode);
											doorObjectModel.position = new Vector3(mapNode.GetPosition().x, mapNode.GetPosition().y, -0.01f);
											passageObjectModel.AddDoor(doorObjectModel);
											mapNode.SetDoor(doorObjectModel);
											doorObjectModel.Close();
										}
										if (namedItem11 == null)
										{
											list4.Add(mapNode);
										}
										if (passageObjectModel != null)
										{
											passageObjectModel.AddNode(mapNode);
										}
										mapSefiraArea.AddNode(mapNode);
										if (mapNode7 != null)
										{
											mapSefiraArea.AddNode(mapNode7);
										}
									}
								}
								finally
								{
									IDisposable disposable3;
									if ((disposable3 = (enumerator5 as IDisposable)) != null)
									{
										disposable3.Dispose();
									}
								}
								if (passageObjectModel != null)
								{
									dictionary9.Add(text, passageObjectModel);
									mapSefiraArea.AddPassage(passageObjectModel);
									if (passageObjectModel.rabbitTeamGroup != string.Empty)
									{
										if (!dictionary10.ContainsKey(passageObjectModel.rabbitTeamGroup))
										{
											dictionary10.Add(passageObjectModel.rabbitTeamGroup, new List<PassageObjectModel>());
										}
										dictionary10[passageObjectModel.rabbitTeamGroup].Add(passageObjectModel);
									}
									passageObjectModel.CheckCenter();
								}
								if (passageObjectModel != null)
								{
									sefira.AddPassage(passageObjectModel);
								}
							}
							else if (!(xmlNode3.Name == "#comment"))
							{
								Debug.Log("this is not passage >>> " + xmlNode3.Name);
							}
						}
					}
					finally
					{
						IDisposable disposable4;
						if ((disposable4 = (enumerator3 as IDisposable)) != null)
						{
							disposable4.Dispose();
						}
					}
					dictionary8.Add(innerText2, mapSefiraArea);
					dictionary3.Add(innerText2, list2);
					dictionary4.Add(innerText2, list3);
					dictionary5.Add(innerText2, list4);
					dictionary6.Add(innerText2, list5);
					dictionary7.Add(innerText2, list6);
				}
			}
			finally
			{
				IDisposable disposable5;
				if ((disposable5 = (enumerator2 as IDisposable)) != null)
				{
					disposable5.Dispose();
				}
			}
			XmlNodeList xmlNodeList3 = edgeRoot.SelectNodes("edge");
			List<MapEdge> list7 = new List<MapEdge>();
			IEnumerator enumerator6 = xmlNodeList3.GetEnumerator();
			try
			{
				while (enumerator6.MoveNext())
				{
					object obj6 = enumerator6.Current;
					XmlNode xmlNode11 = (XmlNode)obj6;
					string innerText4 = xmlNode11.Attributes.GetNamedItem("node1").InnerText;
					string innerText5 = xmlNode11.Attributes.GetNamedItem("node2").InnerText;
					string innerText6 = xmlNode11.Attributes.GetNamedItem("type").InnerText;
					MapNode mapNode8;
					MapNode mapNode9;
					if (!dictionary2.TryGetValue(innerText4, out mapNode8) || !dictionary2.TryGetValue(innerText5, out mapNode9))
					{
						Debug.Log(string.Concat(new string[]
						{
							"cannot create edge - (",
							innerText4,
							", ",
							innerText5,
							")"
						}));
					}
					else
					{
						XmlNode namedItem13 = xmlNode11.Attributes.GetNamedItem("cost");
						MapEdge mapEdge;
						if (namedItem13 != null)
						{
							mapEdge = new MapEdge(mapNode8, mapNode9, innerText6, float.Parse(namedItem13.InnerText));
						}
						else
						{
							mapEdge = new MapEdge(mapNode8, mapNode9, innerText6);
						}
						list7.Add(mapEdge);
						mapNode8.AddEdge(mapEdge);
						mapNode9.AddEdge(mapEdge);
					}
				}
			}
			finally
			{
				IDisposable disposable6;
				if ((disposable6 = (enumerator6 as IDisposable)) != null)
				{
					disposable6.Dispose();
				}
			}
			this.graphNodes = dictionary2;
			this.edges = list7;
			this.mapAreaTable = dictionary8;
			this.sefiraCoreNodesTable = dictionary3;
			this.additionalSefiraTable = dictionary4;
			this.sefiraRoamingNodesTable = dictionary5;
			this.sefiraContainsTable = dictionary6;
			this.sefiraPassageTable = dictionary7;
			this.passageTable = dictionary9;
			this.rabbitTeamGroupTable = dictionary10;
			this.elevatorList = new List<ElevatorPassageModel>();
			foreach (MapNode mapNode10 in list)
			{
				MapEdge[] array6 = mapNode10.GetEdges();
				if (array6.Length > 1)
				{
					List<MapNode> list8 = new List<MapNode>();
					List<MapNode> list9 = new List<MapNode>();
					MapEdge[] array7 = array6;
					for (int j = 0; j < array7.Length; j++)
					{
						MapNode mapNode11 = array7[j].ConnectedNodeIgoreActivate(mapNode10);
						if (mapNode11.GetPosition().y > mapNode10.GetPosition().y)
						{
							list8.Add(mapNode11);
						}
						else
						{
							list9.Add(mapNode11);
						}
					}
					if (list8.Count > 0 && list9.Count > 0)
					{
						ElevatorPassageModel elevator = mapNode10.GetElevator();
						GameObject elevatorPrefab = elevator.GetElevatorPrefab();
						if (elevatorPrefab != null)
						{
							ElevatorPassageObject component = elevatorPrefab.GetComponent<ElevatorPassageObject>();
							elevator.AddFloorInfo(list8.ToArray(), new Vector3(0f, component.top, 0f) + mapNode10.GetPosition());
							elevator.AddFloorInfo(list9.ToArray(), new Vector3(0f, component.bottom, 0f) + mapNode10.GetPosition());
						}
						else
						{
							elevator.AddFloorInfo(list8.ToArray(), new Vector3(0f, 3f, 0f) + mapNode10.GetPosition());
							elevator.AddFloorInfo(list9.ToArray(), new Vector3(0f, -3.5f, 0f) + mapNode10.GetPosition());
						}
						this.elevatorList.Add(elevator);
						mapNode10.GetAttachedPassage().AddElevator(elevator);
					}
					else
					{
						mapNode10.AttachElevator(null);
					}
				}
				else
				{
					mapNode10.AttachElevator(null);
				}
			}
			this.loaded = true;
			Notice.instance.Observe(NoticeName.FixedUpdate, this);
			Notice.instance.Observe(NoticeName.OnStageEnd, this);
			Notice.instance.Send(NoticeName.LoadMapGraphComplete, new object[0]);
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/LMerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x001561D4 File Offset: 0x001543D4
	public void Reset()
	{
		SefiraManager.instance.ResetPassageData();
		Notice.instance.Remove(NoticeName.FixedUpdate, this);
		Notice.instance.Remove(NoticeName.OnStageEnd, this);
		this.loaded = false;
		this.LoadMap();
		Notice.instance.Send(NoticeName.ResetMapGraph, new object[0]);
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x00156230 File Offset: 0x00154430
	public MapNode[] GetGraphNodes()
	{
		MapNode[] array = new MapNode[this.graphNodes.Count];
		int num = 0;
		foreach (KeyValuePair<string, MapNode> keyValuePair in this.graphNodes)
		{
			array[num++] = keyValuePair.Value;
		}
		return array;
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x0002E8BE File Offset: 0x0002CABE
	public MapEdge[] GetGraphEdges()
	{
		return this.edges.ToArray();
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x0002E8CB File Offset: 0x0002CACB
	public void RegisterPassage(PassageObjectModel passage)
	{
		this.passageTable.Add(passage.GetId(), passage);
		Notice.instance.Send(NoticeName.AddPassageObject, new object[]
		{
			passage
		});
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x0002E8F8 File Offset: 0x0002CAF8
	public PassageObjectModel[] GetPassageObjectList()
	{
		return new List<PassageObjectModel>(this.passageTable.Values).ToArray();
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x0002E90F File Offset: 0x0002CB0F
	public ElevatorPassageModel[] GetElevatorPassageList()
	{
		return this.elevatorList.ToArray();
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x001562A8 File Offset: 0x001544A8
	public List<PassageObjectModel> GetPassageListByRabbitGroup(string id)
	{
		List<PassageObjectModel> collection;
		if (this.rabbitTeamGroupTable.TryGetValue(id, out collection))
		{
			return new List<PassageObjectModel>(collection);
		}
		return new List<PassageObjectModel>();
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x001562D4 File Offset: 0x001544D4
	public MapNode[] GetSefiraAllNodes(string area)
	{
		List<MapNode> list = null;
		if (this.sefiraContainsTable.TryGetValue(area, out list))
		{
			return list.ToArray();
		}
		return null;
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x00156300 File Offset: 0x00154500
	public MapNode[] GetSefiraPassagePointNode(string area)
	{
		List<MapNode> list = null;
		List<MapNode> list2 = new List<MapNode>();
		if (this.sefiraPassageTable.TryGetValue(area, out list))
		{
			foreach (MapNode mapNode in list)
			{
				if (mapNode.activate)
				{
					list2.Add(mapNode);
				}
			}
			return list2.ToArray();
		}
		return null;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x00156388 File Offset: 0x00154588
	private void FixedUpdate()
	{
		foreach (PassageObjectModel passageObjectModel in this.passageTable.Values)
		{
			passageObjectModel.FixedUpdate();
		}
		foreach (ElevatorPassageModel elevatorPassageModel in this.elevatorList)
		{
			elevatorPassageModel.OnFixedUpdate();
		}
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x00156434 File Offset: 0x00154634
	private void StageEnd()
	{
		foreach (PassageObjectModel passageObjectModel in this.passageTable.Values)
		{
			passageObjectModel.OnStageEnd();
		}
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x0002E91C File Offset: 0x0002CB1C
	public void OnNotice(string name, params object[] param)
	{
		if (name == NoticeName.FixedUpdate)
		{
			this.FixedUpdate();
		}
		if (name == NoticeName.OnStageEnd)
		{
			this.StageEnd();
		}
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00156494 File Offset: 0x00154694
	public void LoadModMap(Dictionary<string, XmlNode> nodeRoot, XmlNode edgeRoot)
	{
		try
		{
			List<XmlDocument> list = new List<XmlDocument>();
			List<XmlDocument> list2 = new List<XmlDocument>();
			foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
			{
				string path = directoryInfo.FullName + "/MapGraph";
				if (Directory.Exists(path))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
					{
						if (fileInfo.Name.ToLower() == "add.xml" || fileInfo.Name.ToLower() == "add.txt")
						{
							XmlDocument xmlDocument = new XmlDocument();
							xmlDocument.LoadXml(File.ReadAllText(fileInfo.FullName));
							list.Add(xmlDocument);
						}
						if (fileInfo.Name.ToLower() == "replace.xml" || fileInfo.Name.ToLower() == "replace.txt")
						{
							XmlDocument xmlDocument2 = new XmlDocument();
							xmlDocument2.LoadXml(File.ReadAllText(fileInfo.FullName));
							list2.Add(xmlDocument2);
						}
					}
				}
			}
			if (list2.Count > 0)
			{
				foreach (XmlDocument mxml in list2)
				{
					this.LoadModMap_Replace(nodeRoot, edgeRoot, mxml);
				}
			}
			if (list.Count > 0)
			{
				foreach (XmlDocument mxml2 in list)
				{
					this.LoadModMap_Add(nodeRoot, edgeRoot, mxml2);
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/LMMerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x001566E8 File Offset: 0x001548E8
	public void LoadModMap_Add(Dictionary<string, XmlNode> nodeRoot, XmlNode edgeRoot, XmlDocument mxml)
	{
		XmlNode xmlNode = mxml.SelectSingleNode("map_graph/node_list");
		if (xmlNode != null)
		{
			foreach (object obj in xmlNode)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
				foreach (object obj2 in xmlNode2)
				{
					XmlNode xmlNode3 = (XmlNode)obj2;
					if (xmlNode3.Name == "passage")
					{
						XmlNode newChild = nodeRoot[innerText].OwnerDocument.ImportNode(xmlNode3, true);
						nodeRoot[innerText].AppendChild(newChild);
					}
				}
			}
		}
		XmlNode xmlNode4 = mxml.SelectSingleNode("map_graph/edge_list");
		if (xmlNode4 != null)
		{
			foreach (object obj3 in xmlNode4)
			{
				XmlNode node = (XmlNode)obj3;
				XmlNode newChild2 = edgeRoot.OwnerDocument.ImportNode(node, true);
				edgeRoot.AppendChild(newChild2);
			}
		}
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x00156848 File Offset: 0x00154A48
	public void LoadModMap_Replace(Dictionary<string, XmlNode> nodeRoot, XmlNode edgeRoot, XmlDocument mxml)
	{
		try
		{
			XmlNode xmlNode = mxml.SelectSingleNode("map_graph/node_list");
			if (xmlNode != null)
			{
				foreach (object obj in xmlNode)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
					XmlNode xmlNode3 = edgeRoot.OwnerDocument.SelectSingleNode("map_graph/node_list");
					XmlNode xmlNode4 = xmlNode3.OwnerDocument.ImportNode(xmlNode2, true);
					xmlNode3.RemoveChild(nodeRoot[innerText]);
					xmlNode3.AppendChild(xmlNode4);
					nodeRoot[innerText] = xmlNode4;
				}
			}
			XmlNode xmlNode5 = mxml.SelectSingleNode("map_graph/edge_list");
			if (xmlNode5 != null)
			{
				foreach (object obj2 in xmlNode5)
				{
					XmlNode node = (XmlNode)obj2;
					XmlNode newChild = edgeRoot.OwnerDocument.ImportNode(node, true);
					edgeRoot.AppendChild(newChild);
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/LMMRerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x04002FF7 RID: 12279
	private static MapGraph _instance;

	// Token: 0x04002FF8 RID: 12280
	private Dictionary<string, MapNode> graphNodes;

	// Token: 0x04002FF9 RID: 12281
	private Dictionary<string, List<MapNode>> sefiraCoreNodesTable;

	// Token: 0x04002FFA RID: 12282
	private Dictionary<string, List<MapNode>> additionalSefiraTable;

	// Token: 0x04002FFB RID: 12283
	private Dictionary<string, List<MapNode>> sefiraRoamingNodesTable;

	// Token: 0x04002FFC RID: 12284
	private Dictionary<string, List<MapNode>> sefiraContainsTable;

	// Token: 0x04002FFD RID: 12285
	private Dictionary<string, List<List<MapNode>>> deptNodeTable;

	// Token: 0x04002FFE RID: 12286
	private Dictionary<string, List<MapNode>> sefiraPassageTable;

	// Token: 0x04002FFF RID: 12287
	private Dictionary<string, MapSefiraArea> mapAreaTable;

	// Token: 0x04003000 RID: 12288
	private Dictionary<string, PassageObjectModel> passageTable;

	// Token: 0x04003001 RID: 12289
	private Dictionary<string, List<PassageObjectModel>> rabbitTeamGroupTable;

	// Token: 0x04003002 RID: 12290
	private List<MapEdge> edges;

	// Token: 0x04003003 RID: 12291
	private List<ElevatorPassageModel> elevatorList;
}
