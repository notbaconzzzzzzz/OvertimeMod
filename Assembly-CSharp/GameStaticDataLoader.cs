/*
public void LoadSefiraIsolateData() // Feed 'SefiraIsolateManagement' SefiraEnum
public void LoadResearchItemData() // Load external file instead
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using Credit;
using GlobalBullet;
using Legacy;
using UnityEngine;
using WorkerSpine;

// Token: 0x02000590 RID: 1424
public class GameStaticDataLoader
{
	// Token: 0x060030CA RID: 12490 RVA: 0x00003DF4 File Offset: 0x00001FF4
	public GameStaticDataLoader()
	{
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x00148318 File Offset: 0x00146518
	public static void LoadStaticData()
	{
		GameStaticDataLoader gameStaticDataLoader = new GameStaticDataLoader();
		GameStaticDataLoader.currentLn = GlobalGameManager.instance.GetCurrentLanguage();
		new ExternalFontDataLoader().Load();
		new LocalizeTextDataLoader(GameStaticDataLoader.currentLn).LoadLocalizeTextData();
		new StoryDataLoader().LoadStoryData();
		new EquipmentDataLoader().Load();
		new AgentTitleDataLoader().Load();
		MissionTypeList.instance.LoadData();
		if (!SkillTypeList.instance.loaded)
		{
			gameStaticDataLoader.LoadSKillData();
		}
		new CreatureDataLoader().Load();
		new RabbitDataLoader().Load();
		if (!AgentLyrics.instance.IsLoaded())
		{
			gameStaticDataLoader.LoadLyricData();
			gameStaticDataLoader.LoadNewLyricData();
		}
		if (!ResearchItemTypeList.instance.loaded)
		{
			gameStaticDataLoader.LoadResearchItemData();
		}
		if (!Conversation.instance.isLoaded)
		{
			gameStaticDataLoader.LoadSefiraDescData();
		}
		if (!AngelaConversation.instance.loaded)
		{
			gameStaticDataLoader.LoadAngelaDescData();
		}
		gameStaticDataLoader.LoadOfficerActionList();
		if (!AgentNameList.instance.isLoaded)
		{
			gameStaticDataLoader.LoadAgentNameData();
		}
		if (!StageRewardTypeList.instance.loaded)
		{
			gameStaticDataLoader.LoadStageRewardData();
		}
		if (!SefiraManager.instance.isLoadedSefiaIsolateData)
		{
			gameStaticDataLoader.LoadSefiraIsolateData();
		}
		if (!SpriteLoadManager.instance.isLoaded)
		{
			gameStaticDataLoader.LoadSpriteLoadingData();
		}
		if (!PanicDataList.instance.IsLoaded())
		{
			gameStaticDataLoader.LoadPanicData();
		}
		if (!HierarchicalDataManager.instance.IsInit)
		{
			gameStaticDataLoader.LoadHierarchicalData();
		}
		if (!FactionTypeList.instance.IsLoaded)
		{
			gameStaticDataLoader.LoadFactionData();
		}
		if (!ItemObjectManager.instance.IsInit)
		{
			gameStaticDataLoader.LoadItemObjectData();
		}
		if (!RandomEventManager.instance.IsLoadedInfo)
		{
			gameStaticDataLoader.LoadRandomEventInfo();
		}
		if (!WorkerSpineAnimatorManager.instance.IsLoaded)
		{
			gameStaticDataLoader.LoadWorkerSpineData();
		}
		TutorialDataLoader.instance.Load();
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x001484B4 File Offset: 0x001466B4
	public static void ReloadData()
	{
		GameStaticDataLoader gameStaticDataLoader = new GameStaticDataLoader();
		GameStaticDataLoader.currentLn = GlobalGameManager.instance.GetCurrentLanguage();
		new LocalizeTextDataLoader(GameStaticDataLoader.currentLn).LoadLocalizeTextData();
		new StoryDataLoader().LoadStoryData();
		MissionTypeList.instance.LoadData();
		gameStaticDataLoader.LoadSKillData();
		new CreatureDataLoader().ReLoad();
		new RabbitDataLoader().Load();
		gameStaticDataLoader.LoadLyricData();
		gameStaticDataLoader.LoadNewLyricData();
		gameStaticDataLoader.LoadResearchItemData();
		gameStaticDataLoader.LoadSefiraDescData();
		bool loaded = AngelaConversation.instance.loaded;
		gameStaticDataLoader.LoadAngelaDescData();
		gameStaticDataLoader.LoadOfficerActionList();
		bool isLoaded = AgentNameList.instance.isLoaded;
		gameStaticDataLoader.LoadAgentNameData();
		bool loaded2 = StageRewardTypeList.instance.loaded;
		gameStaticDataLoader.LoadStageRewardData();
		bool isLoadedSefiaIsolateData = SefiraManager.instance.isLoadedSefiaIsolateData;
		gameStaticDataLoader.LoadSefiraIsolateData();
		bool isLoaded2 = SpriteLoadManager.instance.isLoaded;
		gameStaticDataLoader.LoadSpriteLoadingData();
		PanicDataList.instance.IsLoaded();
		gameStaticDataLoader.LoadPanicData();
		if (!WorkerSpineAnimatorManager.instance.IsLoaded)
		{
			gameStaticDataLoader.LoadWorkerSpineData();
		}
		new AgentTitleDataLoader().Load();
		TutorialDataLoader.instance.Load();
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x001485C0 File Offset: 0x001467C0
	public static void LoadCreditData()
	{
		Debug.Log("Load Credit Data");
		TextAsset textAsset = Resources.Load<TextAsset>("xml/credit");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		string lastText = xmlDocument.SelectSingleNode("Credit/LastText").InnerText.Trim();
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("Credit/Section");
		List<CreditSection> list = new List<CreditSection>();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				CreditSection creditSection = new CreditSection();
				creditSection.SectionName = xmlNode.Attributes.GetNamedItem("name").InnerText;
				string innerText = xmlNode.Attributes.GetNamedItem("type").InnerText;
				if (!string.IsNullOrEmpty(innerText))
				{
					string text = innerText.ToLower();
					if (text != null)
					{
						if (!(text == "development"))
						{
							if (!(text == "supporters"))
							{
								if (text == "special")
								{
									creditSection.sectionType = CreditSectionType.SPECIAL;
								}
							}
							else
							{
								creditSection.sectionType = CreditSectionType.SUPPORT;
							}
						}
						else
						{
							creditSection.sectionType = CreditSectionType.MAIN;
						}
					}
				}
				IEnumerator enumerator2 = xmlNode.SelectNodes("Credit").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						string innerText2 = xmlNode2.Attributes.GetNamedItem("Name").InnerText;
						string innerText3 = xmlNode2.Attributes.GetNamedItem("Num").InnerText;
						CreditItem item = new CreditItem
						{
							name = innerText2,
							num = (int)float.Parse(innerText3)
						};
						creditSection.list.Add(item);
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
				if (creditSection.sectionType == CreditSectionType.SUPPORT)
				{
					List<CreditItem> list2 = creditSection.list;
					if (GameStaticDataLoader.__mg_cache0 == null)
					{
						GameStaticDataLoader.__mg_cache0 = new Comparison<CreditItem>(CreditItem.Compare);
					}
					list2.Sort(GameStaticDataLoader.__mg_cache0);
				}
				list.Add(creditSection);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		CreditManager.instance.Init(list);
		CreditManager.instance.LastText = lastText;
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x00148808 File Offset: 0x00146A08
	public void LoadFactionData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/Factions");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		List<FactionTypeInfo> list = new List<FactionTypeInfo>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/faction").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				FactionTypeInfo factionTypeInfo = new FactionTypeInfo();
				string innerText = xmlNode.Attributes.GetNamedItem("name").InnerText;
				string innerText2 = xmlNode.Attributes.GetNamedItem("code").InnerText;
				factionTypeInfo.name = innerText;
				factionTypeInfo.code = innerText2;
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("hostile/node");
				if (xmlNodeList != null)
				{
					IEnumerator enumerator2 = xmlNodeList.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							string innerText3 = ((XmlNode)obj2).InnerText;
							factionTypeInfo.lib.Add(innerText3, FactionActionType.HOSTILE);
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
				}
				XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("friendly/node");
				if (xmlNodeList2 != null)
				{
					IEnumerator enumerator3 = xmlNodeList2.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							string innerText4 = ((XmlNode)obj3).InnerText;
							factionTypeInfo.lib.Add(innerText4, FactionActionType.FRIENDLY);
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
				}
				list.Add(factionTypeInfo);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		FactionTypeList.instance.Init(list.AsReadOnly());
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x001489E0 File Offset: 0x00146BE0
	public void LoadHierarchicalData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/HierarchicalDataList");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/domain").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				List<string> list = new List<string>();
				IEnumerator enumerator2 = xmlNode.SelectNodes("node").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						string innerText = ((XmlNode)obj2).InnerText;
						list.Add(innerText);
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
				string innerText2 = xmlNode.Attributes.GetNamedItem("name").InnerText;
				dictionary.Add(innerText2, list);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		HierarchicalDataManager.instance.Init(dictionary);
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x00148AF0 File Offset: 0x00146CF0
	private void LoadOfficerActionList()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/OfficerAction");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		IEnumerator enumerator = xmlDocument.SelectNodes("root/sefira").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("name").InnerText;
				int sefira = (int)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				OfficerSpecialActionList officerSpecialActionList = new OfficerSpecialActionList();
				officerSpecialActionList.sefira = sefira;
				IEnumerator enumerator2 = xmlNode.SelectNodes("action").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						int id = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("id").InnerText);
						string innerText2 = xmlNode2.Attributes.GetNamedItem("nodeId").InnerText;
						string innerText3 = xmlNode2.Attributes.GetNamedItem("animationParam").InnerText;
						int animVal = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("animationValue").InnerText);
						string innerText4 = xmlNode2.Attributes.GetNamedItem("move").InnerText;
						OfficerSpecialAction officerSpecialAction = new OfficerSpecialAction(id, innerText2, animVal, innerText3);
						if (innerText4 == "1")
						{
							IEnumerator enumerator3 = xmlNode2.SelectNodes("pos").GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									object obj3 = enumerator3.Current;
									XmlNode xmlNode3 = (XmlNode)obj3;
									float x = float.Parse(xmlNode3.Attributes.GetNamedItem("x").InnerText);
									float y = float.Parse(xmlNode3.Attributes.GetNamedItem("y").InnerText);
									LOOKINGDIR officerLookingDir = this.GetOfficerLookingDir(xmlNode3.Attributes.GetNamedItem("dir").InnerText);
									OfficerSpecialAction.PosData posData = new OfficerSpecialAction.PosData();
									posData.pos = new Vector3(x, y, 0f);
									posData.dir = officerLookingDir;
									officerSpecialAction.posData.Add(posData);
								}
								goto IL_238;
							}
							finally
							{
								IDisposable disposable;
								if ((disposable = (enumerator3 as IDisposable)) != null)
								{
									disposable.Dispose();
								}
							}
							goto IL_216;
						}
						goto IL_216;
						IL_238:
						officerSpecialActionList.AddList(officerSpecialAction);
						continue;
						IL_216:
						officerSpecialAction.shouldMove = false;
						officerSpecialAction.posData[0].dir = this.GetOfficerLookingDir(innerText4);
						goto IL_238;
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				SefiraManager.instance.GetSefira(officerSpecialActionList.sefira).officerSpecialAction = officerSpecialActionList;
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x0002D2C0 File Offset: 0x0002B4C0
	private LOOKINGDIR GetOfficerLookingDir(string dir)
	{
		if (dir != null)
		{
			if (dir == "L")
			{
				return LOOKINGDIR.LEFT;
			}
			if (dir == "N")
			{
				return LOOKINGDIR.NOCARE;
			}
			if (dir == "R")
			{
				return LOOKINGDIR.RIGHT;
			}
		}
		return LOOKINGDIR.NOCARE;
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x00148DE4 File Offset: 0x00146FE4
	public void LoadLyricData()
	{
		List<AgentLyrics.LyricList_old> list = new List<AgentLyrics.LyricList_old>();
		XmlDocument xmlDocument = AssetLoader.LoadExternalXmlSafe("AgentLyrics", GameStaticDataLoader.currentLn, true, "Language/");
		IEnumerator enumerator = xmlDocument.SelectNodes("root/grade").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("type");
				int danger = (int)float.Parse(xmlNode.Attributes.GetNamedItem("level").InnerText);
				IEnumerator enumerator2 = xmlNodeList.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						List<AgentLyrics.AgentLyric> list2 = new List<AgentLyrics.AgentLyric>();
						int type = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("num").InnerText);
						int inner = 1;
						if (xmlNode2.Attributes.GetNamedItem("level") != null)
						{
							inner = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("level").InnerText);
						}
						IEnumerator enumerator3 = xmlNode2.SelectNodes("item").GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj3 = enumerator3.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								int id = (int)float.Parse(xmlNode3.Attributes.GetNamedItem("id").InnerText);
								string innerText = xmlNode3.Attributes.GetNamedItem("desc").InnerText;
								AgentLyrics.AgentLyric item = new AgentLyrics.AgentLyric(id, innerText);
								list2.Add(item);
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator3 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						AgentLyrics.LyricList_old item2 = new AgentLyrics.LyricList_old(this.GetLyricType(type), list2.ToArray(), danger, inner);
						list.Add(item2);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		Dictionary<long, AgentLyrics.CreatureReactionList> dictionary = new Dictionary<long, AgentLyrics.CreatureReactionList>();
		IEnumerator enumerator4 = xmlDocument.SelectNodes("root/creatureAction/creature").GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				object obj4 = enumerator4.Current;
				XmlNode xmlNode4 = (XmlNode)obj4;
				AgentLyrics.CreatureReactionList creatureReactionList = new AgentLyrics.CreatureReactionList();
				AgentLyrics.CreatureAction creatureAction = new AgentLyrics.CreatureAction();
				long num = (long)float.Parse(xmlNode4.Attributes.GetNamedItem("id").InnerText);
				creatureReactionList.creatureId = num;
				creatureAction.creatureId = num;
				IEnumerator enumerator5 = xmlNode4.SelectNodes("item").GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						object obj5 = enumerator5.Current;
						XmlNode xmlNode5 = (XmlNode)obj5;
						int level = (int)float.Parse(xmlNode5.Attributes.GetNamedItem("level").InnerText);
						string innerText2 = xmlNode5.Attributes.GetNamedItem("desc").InnerText;
						AgentLyrics.CreatureReaction creatureReaction = new AgentLyrics.CreatureReaction();
						creatureReaction.level = level;
						creatureReaction.desc = innerText2;
						creatureReactionList.lib.Add(creatureReaction);
					}
				}
				finally
				{
					IDisposable disposable4;
					if ((disposable4 = (enumerator5 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				IEnumerator enumerator6 = xmlNode4.SelectNodes("action").GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						object obj6 = enumerator6.Current;
						XmlNode xmlNode6 = (XmlNode)obj6;
						AgentLyrics.CreatureNormal creatureNormal = new AgentLyrics.CreatureNormal();
						string innerText3 = xmlNode6.Attributes.GetNamedItem("type").InnerText;
						creatureNormal.type = innerText3;
						IEnumerator enumerator7 = xmlNode6.SelectNodes("desc").GetEnumerator();
						try
						{
							while (enumerator7.MoveNext())
							{
								object obj7 = enumerator7.Current;
								string innerText4 = ((XmlNode)obj7).InnerText;
								creatureNormal.desc.Add(innerText4);
							}
						}
						finally
						{
							IDisposable disposable5;
							if ((disposable5 = (enumerator7 as IDisposable)) != null)
							{
								disposable5.Dispose();
							}
						}
						creatureAction.lib.Add(innerText3, creatureNormal);
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
				creatureReactionList.action = creatureAction;
				dictionary.Add(num, creatureReactionList);
			}
		}
		finally
		{
			IDisposable disposable7;
			if ((disposable7 = (enumerator4 as IDisposable)) != null)
			{
				disposable7.Dispose();
			}
		}
		XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("root/creatureSay/creature");
		List<CreatureLyrics.CreatureLyricList> list3 = new List<CreatureLyrics.CreatureLyricList>();
		IEnumerator enumerator8 = xmlNodeList2.GetEnumerator();
		try
		{
			while (enumerator8.MoveNext())
			{
				object obj8 = enumerator8.Current;
				XmlNode xmlNode7 = (XmlNode)obj8;
				long id2 = (long)float.Parse(xmlNode7.Attributes.GetNamedItem("id").InnerText);
				XmlNodeList xmlNodeList3 = xmlNode7.SelectNodes("action");
				List<CreatureLyrics.CreatureLyric> list4 = new List<CreatureLyrics.CreatureLyric>();
				IEnumerator enumerator9 = xmlNodeList3.GetEnumerator();
				try
				{
					while (enumerator9.MoveNext())
					{
						object obj9 = enumerator9.Current;
						XmlNode xmlNode8 = (XmlNode)obj9;
						string innerText5 = xmlNode8.Attributes.GetNamedItem("type").InnerText;
						List<string> list5 = new List<string>();
						IEnumerator enumerator10 = xmlNode8.SelectNodes("desc").GetEnumerator();
						try
						{
							while (enumerator10.MoveNext())
							{
								object obj10 = enumerator10.Current;
								string innerText6 = ((XmlNode)obj10).InnerText;
								list5.Add(innerText6);
							}
						}
						finally
						{
							IDisposable disposable8;
							if ((disposable8 = (enumerator10 as IDisposable)) != null)
							{
								disposable8.Dispose();
							}
						}
						CreatureLyrics.CreatureLyric item3 = new CreatureLyrics.CreatureLyric(innerText5, list5.ToArray());
						list4.Add(item3);
					}
				}
				finally
				{
					IDisposable disposable9;
					if ((disposable9 = (enumerator9 as IDisposable)) != null)
					{
						disposable9.Dispose();
					}
				}
				CreatureLyrics.CreatureLyricList item4 = new CreatureLyrics.CreatureLyricList(id2, list4.ToArray());
				list3.Add(item4);
			}
		}
		finally
		{
			IDisposable disposable10;
			if ((disposable10 = (enumerator8 as IDisposable)) != null)
			{
				disposable10.Dispose();
			}
		}
		AgentLyrics.instance.Init(list, dictionary, list3);
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x00149414 File Offset: 0x00147614
	public void LoadNewLyricData()
	{
		XmlDocument xmlDocument = AssetLoader.LoadExternalXmlSafe("AgentNewLyrics", GameStaticDataLoader.currentLn, true, "Language/");
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/category");
		AgentLyrics.LyricCategory normal = new AgentLyrics.LyricCategory();
		AgentLyrics.LyricCategory panic = new AgentLyrics.LyricCategory();
		AgentLyrics.LyricCategory horror = new AgentLyrics.LyricCategory();
		AgentLyrics.LyricCategory otherdead = new AgentLyrics.LyricCategory();
		AgentLyrics.LyricCategory otherpanic = new AgentLyrics.LyricCategory();
		AgentLyrics.CreditLyric creditLyric = new AgentLyrics.CreditLyric();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				AgentLyrics.LyricCategory lyricCategory = new AgentLyrics.LyricCategory();
				string innerText = xmlNode.Attributes.GetNamedItem("type").InnerText;
				if (innerText != null)
				{
					if (!(innerText == "normal"))
					{
						if (!(innerText == "panic"))
						{
							if (!(innerText == "horror"))
							{
								if (!(innerText == "otherdead"))
								{
									if (innerText == "otherpanic")
									{
										otherpanic = lyricCategory;
									}
								}
								else
								{
									otherdead = lyricCategory;
								}
							}
							else
							{
								horror = lyricCategory;
							}
						}
						else
						{
							panic = lyricCategory;
						}
					}
					else
					{
						normal = lyricCategory;
					}
				}
				IEnumerator enumerator2 = xmlNode.SelectNodes("grade").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						AgentLyrics.LyricGrade lyricGrade = new AgentLyrics.LyricGrade();
						int level = int.Parse(xmlNode2.Attributes.GetNamedItem("level").InnerText);
						lyricGrade.level = level;
						IEnumerator enumerator3 = xmlNode2.SelectNodes("lyrics").GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj3 = enumerator3.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								AgentLyrics.LyricList lyricList = new AgentLyrics.LyricList();
								string text = xmlNode3.Attributes.GetNamedItem("rwbp").InnerText.ToLower();
								if (text != null)
								{
									if (!(text == "a"))
									{
										if (!(text == "r"))
										{
											if (!(text == "w"))
											{
												if (!(text == "b"))
												{
													if (text == "p")
													{
														lyricList.rwbp = RwbpType.P;
													}
												}
												else
												{
													lyricList.rwbp = RwbpType.B;
												}
											}
											else
											{
												lyricList.rwbp = RwbpType.W;
											}
										}
										else
										{
											lyricList.rwbp = RwbpType.R;
										}
									}
									else
									{
										lyricList.rwbp = RwbpType.A;
									}
								}
								IEnumerator enumerator4 = xmlNode3.SelectNodes("lyric").GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										object obj4 = enumerator4.Current;
										XmlNode xmlNode4 = (XmlNode)obj4;
										lyricList.list.Add(xmlNode4.InnerText);
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator4 as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
								lyricGrade.AddItem(lyricList);
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
						lyricCategory.AddItem(lyricGrade);
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator2 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		IEnumerator enumerator5 = xmlDocument.SelectNodes("root/credit/category").GetEnumerator();
		try
		{
			while (enumerator5.MoveNext())
			{
				object obj5 = enumerator5.Current;
				XmlNode xmlNode5 = (XmlNode)obj5;
				string innerText2 = xmlNode5.Attributes.GetNamedItem("type").InnerText;
				Dictionary<int, List<string>> dictionary = null;
				if (innerText2 != null)
				{
					if (!(innerText2 == "normal"))
					{
						if (!(innerText2 == "panic"))
						{
							if (!(innerText2 == "horror"))
							{
								if (!(innerText2 == "otherdead"))
								{
									if (innerText2 == "otherpanic")
									{
										dictionary = creditLyric.otherpanic;
									}
								}
								else
								{
									dictionary = creditLyric.otherdead;
								}
							}
							else
							{
								dictionary = creditLyric.horror;
							}
						}
						else
						{
							dictionary = creditLyric.panic;
						}
					}
					else
					{
						dictionary = creditLyric.normal;
					}
				}
				IEnumerator enumerator6 = xmlNode5.SelectNodes("unit").GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						object obj6 = enumerator6.Current;
						XmlNode xmlNode6 = (XmlNode)obj6;
						int key = (int)float.Parse(xmlNode6.Attributes.GetNamedItem("id").InnerText);
						List<string> list = new List<string>();
						IEnumerator enumerator7 = xmlNode6.SelectNodes("lyric").GetEnumerator();
						try
						{
							while (enumerator7.MoveNext())
							{
								object obj7 = enumerator7.Current;
								XmlNode xmlNode7 = (XmlNode)obj7;
								list.Add(xmlNode7.InnerText.Trim());
							}
						}
						finally
						{
							IDisposable disposable5;
							if ((disposable5 = (enumerator7 as IDisposable)) != null)
							{
								disposable5.Dispose();
							}
						}
						dictionary.Add(key, list);
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
			}
		}
		finally
		{
			IDisposable disposable7;
			if ((disposable7 = (enumerator5 as IDisposable)) != null)
			{
				disposable7.Dispose();
			}
		}
		AgentLyrics.instance.InitLyrics(normal, panic, horror, otherdead, otherpanic);
		AgentLyrics.instance.creditLyric = creditLyric;
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x00149948 File Offset: 0x00147B48
	private LyricType GetLyricType(int type)
	{
		switch (type)
		{
		case 1:
			return LyricType.DAY1;
		case 2:
			return LyricType.DAY;
		case 3:
			return LyricType.DAYSMALL;
		case 4:
			return LyricType.CHAT;
		case 5:
			return LyricType.MENTALBAD;
		case 6:
			return LyricType.LEVELUP;
		case 7:
			return LyricType.ESCAPE;
		case 8:
			return LyricType.SAD;
		case 9:
			return LyricType.TIRED;
		case 10:
			return LyricType.D;
		case 11:
			return LyricType.S;
		case 12:
			return LyricType.C;
		case 13:
			return LyricType.I;
		default:
			return LyricType.CHAT;
		}
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x001499B4 File Offset: 0x00147BB4
	public void LoadSKillData()
	{
		XmlNodeList xmlNodeList = AssetLoader.LoadExternalXmlSafe("Skills", GameStaticDataLoader.currentLn, true, "Language/").SelectNodes("/skills/skill");
		List<SkillTypeInfo> list = new List<SkillTypeInfo>();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				SkillTypeInfo skillTypeInfo = new SkillTypeInfo();
				skillTypeInfo.id = long.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				skillTypeInfo.name = xmlNode.Attributes.GetNamedItem("name").InnerText;
				skillTypeInfo.calledName = xmlNode.Attributes.GetNamedItem("called").InnerText;
				if (xmlNode.Attributes.GetNamedItem("type") != null)
				{
					string innerText = xmlNode.Attributes.GetNamedItem("type").InnerText;
				}
				list.Add(skillTypeInfo);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		SkillTypeList.instance.Init(list.ToArray());
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x00149AC8 File Offset: 0x00147CC8
	public void LoadSefiraDescData()
	{
		XmlNodeList xmlNodeList = AssetLoader.LoadExternalXmlSafe("SefiraDesc", GameStaticDataLoader.currentLn, true, "Language/").SelectNodes("root/Sefira");
		Dictionary<int, List<SefiraMessage>> dictionary = new Dictionary<int, List<SefiraMessage>>();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				int key = int.Parse(xmlNode.Attributes.GetNamedItem("Index").InnerText);
				XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Type");
				List<SefiraMessage> list = new List<SefiraMessage>();
				IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						int t = int.Parse(xmlNode2.Attributes.GetNamedItem("Num").InnerText);
						IEnumerator enumerator3 = xmlNode2.SelectNodes("Item").GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj3 = enumerator3.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								int d = int.Parse(xmlNode3.Attributes.GetNamedItem("ID").InnerText);
								string innerText = xmlNode3.Attributes.GetNamedItem("Desc").InnerText;
								SefiraMessage item = new SefiraMessage(t, d, innerText);
								list.Add(item);
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator3 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				dictionary.Add(key, list);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		Conversation.instance.Init(dictionary);
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x00149C98 File Offset: 0x00147E98
	public void LoadAngelaDescData()
	{
		XmlDocument xmlDocument = AssetLoader.LoadExternalXmlSafe("AngelaDesc", GameStaticDataLoader.currentLn, true, "Language/");
		List<AngelaMessage> list = new List<AngelaMessage>();
		XmlNode root = xmlDocument.SelectSingleNode("root/normal");
		AngelaMessage[] angelaMessage = this.GetAngelaMessage(root);
		XmlNode root2 = xmlDocument.SelectSingleNode("root/danger");
		AngelaMessage[] angelaMessage2 = this.GetAngelaMessage(root2);
		foreach (AngelaMessage item in angelaMessage)
		{
			list.Add(item);
		}
		foreach (AngelaMessage item2 in angelaMessage2)
		{
			list.Add(item2);
		}
		AngelaConversation.instance.Init(list.ToArray());
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x00149D44 File Offset: 0x00147F44
	private AngelaMessage[] GetAngelaMessage(XmlNode root)
	{
		List<AngelaMessage> list = new List<AngelaMessage>();
		IEnumerator enumerator = root.SelectNodes("type").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				int index = (int)float.Parse(xmlNode.Attributes.GetNamedItem("num").InnerText);
				string defaultFormat = string.Empty;
				XmlNode namedItem = xmlNode.Attributes.GetNamedItem("default");
				if (namedItem != null)
				{
					defaultFormat = namedItem.InnerText;
				}
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("item");
				List<AngelaMessageUnit> list2 = new List<AngelaMessageUnit>();
				IEnumerator enumerator2 = xmlNodeList.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						int id = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("id").InnerText);
						string innerText = xmlNode2.Attributes.GetNamedItem("desc").InnerText;
						list2.Add(new AngelaMessageUnit
						{
							id = id,
							desc = innerText
						});
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
				string innerText2 = xmlNode.Attributes.GetNamedItem("position").InnerText;
				if (innerText2 == null)
				{
					goto IL_165;
				}
				AngelaMessagePos pos;
				if (!(innerText2 == "up"))
				{
					if (!(innerText2 == "down"))
					{
						goto IL_165;
					}
					pos = AngelaMessagePos.DOWN;
				}
				else
				{
					pos = AngelaMessagePos.UP;
				}
				IL_141:
				list.Add(new AngelaMessage(AngelaConversation.GetAngelaMessageState(index), pos, list2.ToArray())
				{
					defaultFormat = defaultFormat
				});
				continue;
				IL_165:
				pos = AngelaMessagePos.DOWN;
				goto IL_141;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		return list.ToArray();
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x00149F18 File Offset: 0x00148118
	private List<AgentNameTypeInfo> LoadAgentNameInfoXml(string xml, bool isExternal)
	{
		List<AgentNameTypeInfo> list = new List<AgentNameTypeInfo>();
		XmlDocument xmlDocument;
		if (isExternal)
		{
			xmlDocument = AssetLoader.LoadExternalXML(xml);
		}
		else
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/" + xml);
			xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(textAsset.text);
		}
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/data");
		List<string> supprotedList = SupportedLanguage.GetSupprotedList();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				int nameId = (int)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				AgentNameTypeInfo agentNameTypeInfo = new AgentNameTypeInfo
				{
					nameId = nameId
				};
				List<string> list2 = new List<string>();
				foreach (string text in supprotedList)
				{
					XmlNode namedItem = xmlNode.Attributes.GetNamedItem(text);
					if (namedItem == null)
					{
						list2.Add(text);
					}
					else
					{
						string value = namedItem.InnerText.Trim();
						agentNameTypeInfo.nameDic.Add(text, value);
					}
				}
				try
				{
					string value2 = agentNameTypeInfo.nameDic["en"];
					foreach (string key in list2)
					{
						agentNameTypeInfo.nameDic.Add(key, value2);
					}
				}
				catch (Exception)
				{
				}
				list.Add(agentNameTypeInfo);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x0014A108 File Offset: 0x00148308
	private List<UniqueCreditAgentInfo> LoadUniqueCreditInfo(string xml)
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/" + xml);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		List<UniqueCreditAgentInfo> list = new List<UniqueCreditAgentInfo>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/special/custom").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				int rearHairId = -1;
				XmlNode namedItem = xmlNode.Attributes.GetNamedItem("id");
				XmlNode namedItem2 = xmlNode.Attributes.GetNamedItem("appearance");
				XmlNode namedItem3 = xmlNode.Attributes.GetNamedItem("script");
				XmlNode namedItem4 = xmlNode.Attributes.GetNamedItem("rear");
				if (namedItem != null)
				{
					int num = (int)float.Parse(namedItem.InnerText.Trim());
					if (namedItem2 != null)
					{
						int appearanceId = (int)float.Parse(namedItem2.InnerText.Trim());
						if (namedItem3 != null)
						{
							int scriptId = (int)float.Parse(namedItem3.InnerText.Trim());
							if (namedItem4 != null)
							{
								rearHairId = (int)float.Parse(namedItem4.InnerText.Trim());
							}
							UniqueCreditAgentInfo item = new UniqueCreditAgentInfo
							{
								creditIndex = num + 10000,
								appearanceId = appearanceId,
								scriptId = scriptId,
								rearHairId = rearHairId
							};
							list.Add(item);
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x0014A270 File Offset: 0x00148470
	private List<AgentName> LoadAgentNameXml(string xml, bool isExternal)
	{
		List<AgentName> list = new List<AgentName>();
		XmlDocument xmlDocument;
		if (isExternal)
		{
			xmlDocument = AssetLoader.LoadExternalXML(xml);
		}
		else
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/" + xml);
			xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(textAsset.text);
		}
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/data");
		List<string> supprotedList = SupportedLanguage.GetSupprotedList();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				AgentName agentName = new AgentName((int)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText));
				List<string> list2 = new List<string>();
				foreach (string text in supprotedList)
				{
					XmlNode namedItem = xmlNode.Attributes.GetNamedItem(text);
					if (namedItem == null)
					{
						list2.Add(text);
					}
					else
					{
						string value = namedItem.InnerText.Trim();
						agentName.nameDic.Add(text, value);
					}
				}
				try
				{
					string value2 = agentName.nameDic["en"];
					foreach (string key in list2)
					{
						agentName.nameDic.Add(key, value2);
					}
				}
				catch (Exception)
				{
				}
				list.Add(agentName);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x0014A454 File Offset: 0x00148654
	public void LoadAgentNameData()
	{
		List<AgentName> list = new List<AgentName>();
		List<AgentName> nameList = new List<AgentName>();
		List<AgentNameTypeInfo> list2 = new List<AgentNameTypeInfo>();
		List<AgentNameTypeInfo> list3 = new List<AgentNameTypeInfo>();
		list = this.LoadAgentNameXml("Language/AgentName", true);
		nameList = this.LoadAgentNameXml("Language/CreditAgentName", false);
		list2 = this.LoadAgentNameInfoXml("Language/AgentName", true);
		list3 = this.LoadAgentNameInfoXml("Language/CreditAgentName", false);
		List<UniqueCreditAgentInfo> list4 = this.LoadUniqueCreditInfo("Language/CreditAgentName");
		AgentNameList.instance.Init(list.ToArray());
		AgentNameList.instance.AddCreditNames(nameList);
		AgentNameList.instance.InitNameTypes(list2.ToArray(), list3.ToArray(), list4.ToArray());
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x0014A4F4 File Offset: 0x001486F4
	public void LoadStageRewardData()
	{
		List<StageRewardTypeInfo> list = new List<StageRewardTypeInfo>();
		TextAsset textAsset = Resources.Load<TextAsset>("xml/StageReward");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		IEnumerator enumerator = xmlDocument.SelectNodes("root/reward").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				XmlNode namedItem = xmlNode.Attributes.GetNamedItem("day");
				if (namedItem == null)
				{
					Debug.LogError("StageReward : 'day' attr Not Found ");
				}
				else
				{
					StageRewardTypeInfo stageRewardTypeInfo = new StageRewardTypeInfo();
					int day = int.Parse(namedItem.InnerText);
					stageRewardTypeInfo.day = day;
					IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.Name == "agent")
							{
								StageRewardTypeInfo.AgentRewardInfo agentRewardInfo = new StageRewardTypeInfo.AgentRewardInfo();
								XmlAttributeCollection attributes = xmlNode2.Attributes;
								XmlNode namedItem2 = attributes.GetNamedItem("lv");
								XmlNode namedItem3 = attributes.GetNamedItem("sephira");
								if (namedItem2 != null)
								{
									agentRewardInfo.level = int.Parse(namedItem2.InnerText);
								}
								if (namedItem3 != null)
								{
									agentRewardInfo.sephira = namedItem3.InnerText;
								}
								stageRewardTypeInfo.agentList.Add(agentRewardInfo);
							}
							else if (xmlNode2.Name == "money")
							{
								stageRewardTypeInfo.money = int.Parse(xmlNode2.InnerText);
							}
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
					stageRewardTypeInfo.GenerateRankLimit();
					list.Add(stageRewardTypeInfo);
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		StageRewardTypeList.instance.Init(list.ToArray());
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x0014A6D8 File Offset: 0x001488D8
	public void LoadTutorialData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/TutorialFlow");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("root/ui");
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("root/manage");
		string innerText = xmlNode.Attributes.GetNamedItem("src").InnerText;
		string innerText2 = xmlNode2.Attributes.GetNamedItem("src").InnerText;
		XmlNodeList list = xmlNode.SelectNodes("node");
		XmlNodeList list2 = xmlNode2.SelectNodes("node");
		List<TutorialNode> list3 = new List<TutorialNode>(this.LoadTutorialNode(list, innerText));
		List<TutorialNode> list4 = new List<TutorialNode>(this.LoadTutorialNode(list2, innerText2));
		Legacy.TutorialManager.instance.Init(list3.ToArray(), list4.ToArray());
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x0014A794 File Offset: 0x00148994
	private TutorialNode[] LoadTutorialNode(XmlNodeList list, string rootSrc)
	{
		List<TutorialNode> list2 = new List<TutorialNode>();
		int num = 0;
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string innerText = ((XmlNode)obj).Attributes.GetNamedItem("image").InnerText;
				TutorialNode item = new TutorialNode(num, rootSrc + "/" + innerText);
				list2.Add(item);
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list2.ToArray();
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x0002D2F3 File Offset: 0x0002B4F3
	private bool GetBooleanData(string b)
	{
		b.ToLower();
		if (b == null)
		{
			return false;
		}
		if (b == "true")
		{
			return true;
		}
		//b == "false";
		return false;
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x0014A828 File Offset: 0x00148A28
	public void LoadSefiraIsolateData()
	{ // <Mod> Feed 'SefiraIsolateManagement' the sefira's SefiraEnum
		if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseIsolate.txt"))
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/SefiraIsolateNode");
			File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseIsolate.txt", textAsset.text);
		}
		string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseIsolate.txt");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		Dictionary<string, XmlNode> dictionary = new Dictionary<string, XmlNode>();
		foreach (object obj in xmlDocument.SelectSingleNode("root"))
		{
			XmlNode xmlNode = (XmlNode)obj;
			dictionary.Add(xmlNode.Attributes.GetNamedItem("name").InnerText, xmlNode);
		}
		this.LoadModIsolate(dictionary);
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/sefira");
		Dictionary<string, SefiraEnum> dictionary2 = new Dictionary<string, SefiraEnum>();
		IEnumerator enumerator2 = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				XmlNode xmlNode2 = (XmlNode)obj2;
				string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
				Sefira sefira = SefiraManager.instance.GetSefira(innerText);
				XmlNodeList xmlNodeList2 = xmlNode2.SelectNodes("node");
				List<SefiraIsolate> list = new List<SefiraIsolate>();
				IEnumerator enumerator3 = xmlNodeList2.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						XmlNode xmlNode3 = (XmlNode)obj3;
						string innerText2 = xmlNode3.Attributes.GetNamedItem("id").InnerText;
						string oldId = "";
						if (xmlNode3.Attributes.GetNamedItem("oldId") != null)
						{
							oldId = xmlNode3.Attributes.GetNamedItem("oldId").InnerText;
							if (!SpecialModeConfig.instance.GetValue<bool>("MapGraphFix"))
							{
								string temp = oldId;
								oldId = innerText2;
								innerText2 = temp;
							}
						}
						float x = float.Parse(xmlNode3.Attributes.GetNamedItem("x").InnerText);
						float y = float.Parse(xmlNode3.Attributes.GetNamedItem("y").InnerText);
						/*
						float bumpedX = -1000f;
						float bumpedY = -1000f;
						string bumpedByNode = "";
						string bumpedBySefira = "";
						string bumpsNode = "";
						string bumpsSefira = "";
						XmlNode xmlNode5 = xmlNode3.SelectSingleNode("bumped");
						if (xmlNode5 != null)
						{
							bumpedX = float.Parse(xmlNode5.Attributes.GetNamedItem("x").InnerText);
							bumpedY = float.Parse(xmlNode5.Attributes.GetNamedItem("y").InnerText);
							string temp = xmlNode5.InnerText;
							string[] temp2 = temp.Split('.');
							if (temp2.Length >= 2)
							{
								bumpedBySefira = temp2[0];
								bumpedByNode = temp2[1];
							}
							else
							{
								bumpedByNode = temp;
							}
						}
						if (xmlNode3.Attributes.GetNamedItem("bumps") != null)
						{
							string temp = xmlNode3.Attributes.GetNamedItem("bumps").InnerText;
							string[] temp2 = temp.Split('.');
							if (temp2.Length >= 2)
							{
								bumpsSefira = temp2[0];
								bumpsNode = temp2[1];
							}
							else
							{
								bumpsNode = temp;
							}
						}*/
						string innerText3 = xmlNode3.Attributes.GetNamedItem("pos").InnerText;
						int index = (int)float.Parse(xmlNode3.Attributes.GetNamedItem("index").InnerText);
						IsolatePos pos = IsolatePos.UP;
						if (innerText3 != null)
						{
							if (!(innerText3 == "UP"))
							{
								if (innerText3 == "DOWN")
								{
									pos = IsolatePos.DOWN;
								}
							}
							else
							{
								pos = IsolatePos.UP;
							}
						}
						SefiraIsolate sefiraIsolate = new SefiraIsolate();
						sefiraIsolate.nodeId = innerText2;
						sefiraIsolate.oldId = oldId;
						sefiraIsolate.x = x;
						sefiraIsolate.y = y;/*
						sefiraIsolate.bumpedX = bumpedX;
						sefiraIsolate.bumpedY = bumpedY;
						sefiraIsolate.bumpedByNode = bumpedByNode;
						sefiraIsolate.bumpedBySefira = bumpedBySefira;
						sefiraIsolate.bumpsNode = bumpsNode;
						sefiraIsolate.bumpsSefira = bumpsSefira;*/
						sefiraIsolate.pos = pos;
						sefiraIsolate.index = index;
						XmlNode xmlNode4 = xmlNode3.SelectSingleNode("exclusive");
						if (xmlNode4 != null)
						{
							string innerText4 = xmlNode4.InnerText;
							char[] separator = new char[]
							{
								','
							};
							string[] array = innerText4.Split(separator);
							for (int i = 0; i < array.Length; i++)
							{
								long item = (long)float.Parse(array[i]);
								sefiraIsolate.exclusiveID.Add(item);
							}
						}
						if (xmlNode3.Attributes.GetNamedItem("bumps") != null)
						{
							string temp = xmlNode3.Attributes.GetNamedItem("bumps").InnerText;
							string[] temp2 = temp.Split(',');
							foreach (string temp3 in temp2)
							{
								string[] temp4 = temp.Split('.');
								if (temp4.Length >= 2)
								{
									sefiraIsolate.bumps.Add(new SefiraIsolate.SefiraNodePointer(temp4[1], temp4[0]));
								}
								else
								{
									sefiraIsolate.bumps.Add(new SefiraIsolate.SefiraNodePointer(temp3));
								}
							}
						}
						XmlNodeList xmlNodeList3 = xmlNode3.SelectNodes("bumped");
						if (xmlNodeList3 != null && xmlNodeList3.Count > 0)
						{
							IEnumerator enumerator4 = xmlNodeList3.GetEnumerator();
							try
							{
								while (enumerator4.MoveNext())
								{
									object obj4 = enumerator4.Current;
									XmlNode xmlNode5 = (XmlNode)obj4;
									float x2 = float.Parse(xmlNode5.Attributes.GetNamedItem("x").InnerText);
									float y2 = float.Parse(xmlNode5.Attributes.GetNamedItem("y").InnerText);
									SefiraIsolate.BumpDetail bump = new SefiraIsolate.BumpDetail(x2, y2);
									string temp = xmlNode5.InnerText;
									string[] temp2 = temp.Split(',');
									foreach (string temp3 in temp2)
									{
										string[] temp4 = temp.Split('.');
										if (temp4.Length >= 2)
										{
											bump.bumpedBy.Add(new SefiraIsolate.SefiraNodePointer(temp4[1], temp4[0]));
										}
										else
										{
											bump.bumpedBy.Add(new SefiraIsolate.SefiraNodePointer(temp3));
										}
									}
									sefiraIsolate.bumpData.Add(bump);
								}
							}
							finally
							{
								IDisposable disposable4;
								if ((disposable4 = (enumerator4 as IDisposable)) != null)
								{
									disposable4.Dispose();
								}
							}
						}
						dictionary2.Add(innerText2, sefira.sefiraEnum);
						list.Add(sefiraIsolate);
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
				sefira.isolateManagement = new SefiraIsolateManagement(list.ToArray(), sefira.sefiraEnum);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator2 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		SefiraManager.instance.SefiraIsolateLoad(dictionary2);
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x0014AB80 File Offset: 0x00148D80
	private void LoadSpriteLoadingData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/LoadedSpriteData");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/sefiraLinked/node");
		XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("root/commonLinked/node");
		XmlNodeList xmlNodeList3 = xmlDocument.SelectNodes("root/sefiraLinked/set");
		XmlNodeList xmlNodeList4 = xmlDocument.SelectNodes("root/commonLinked/set");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		Dictionary<string, SpriteSetLoadedScript.SRC> dictionary3 = new Dictionary<string, SpriteSetLoadedScript.SRC>();
		Dictionary<string, SpriteSetLoadedScript.SRC> dictionary4 = new Dictionary<string, SpriteSetLoadedScript.SRC>();
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("key").InnerText;
				string innerText2 = xmlNode.Attributes.GetNamedItem("src").InnerText;
				dictionary.Add(innerText, innerText2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				XmlNode xmlNode2 = (XmlNode)obj2;
				string innerText3 = xmlNode2.Attributes.GetNamedItem("key").InnerText;
				string innerText4 = xmlNode2.Attributes.GetNamedItem("src").InnerText;
				dictionary2.Add(innerText3, innerText4);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		IEnumerator enumerator3 = xmlNodeList3.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				XmlNode xmlNode3 = (XmlNode)obj3;
				string innerText5 = xmlNode3.Attributes.GetNamedItem("key").InnerText;
				string innerText6 = xmlNode3.Attributes.GetNamedItem("normal").InnerText;
				string innerText7 = xmlNode3.Attributes.GetNamedItem("over").InnerText;
				string innerText8 = xmlNode3.Attributes.GetNamedItem("disable").InnerText;
				string innerText9 = xmlNode3.Attributes.GetNamedItem("src").InnerText;
				dictionary4.Add(innerText5, new SpriteSetLoadedScript.SRC
				{
					normal = innerText6,
					over = innerText7,
					disabled = innerText8,
					src = innerText9
				});
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		IEnumerator enumerator4 = xmlNodeList4.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				object obj4 = enumerator4.Current;
				XmlNode xmlNode4 = (XmlNode)obj4;
				string innerText10 = xmlNode4.Attributes.GetNamedItem("key").InnerText;
				string innerText11 = xmlNode4.Attributes.GetNamedItem("normal").InnerText;
				string innerText12 = xmlNode4.Attributes.GetNamedItem("over").InnerText;
				string innerText13 = xmlNode4.Attributes.GetNamedItem("disable").InnerText;
				string innerText14 = xmlNode4.Attributes.GetNamedItem("src").InnerText;
				dictionary3.Add(innerText10, new SpriteSetLoadedScript.SRC
				{
					normal = innerText11,
					over = innerText12,
					disabled = innerText13,
					src = innerText14
				});
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator4 as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		SpriteLoadManager.instance.Init(dictionary2, dictionary, dictionary3, dictionary4);
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x0014AECC File Offset: 0x001490CC
	public void LoadPanicData()
	{
		XmlNode xmlNode = AssetLoader.LoadExternalXmlSafe("PanicAction", GameStaticDataLoader.currentLn, true, "Language/");
		List<PanicData> list = new List<PanicData>();
		XmlNodeList xmlNodeList = xmlNode.SelectNodes("root/panic");
		int count = xmlNodeList.Count;
		for (int i = 0; i < count; i++)
		{
			XmlNode xmlNode2 = xmlNodeList[i];
			int panicId = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("id").InnerText);
			string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
			string innerText2 = xmlNode2.Attributes.GetNamedItem("lifeStyle").InnerText;
			string innerText3 = xmlNode2.Attributes.GetNamedItem("build").InnerText;
			string innerText4 = xmlNode2.InnerText;
			list.Add(new PanicData
			{
				PanicId = panicId,
				PanicName = innerText,
				PanicLifeStyle = innerText2,
				PanicBuildCode = innerText3,
				PanicDesc = innerText4
			});
		}
		PanicDataList.instance.Init(list.ToArray());
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0014AFCC File Offset: 0x001491CC
	public void LoadResearchItemData()
	{ // <Mod> Load external file instead of resources file so that Chesed's research can be fixed
		XmlDocument xmlDocument = new XmlDocument();
		if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseResearch.txt"))
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/Research");
			File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseResearch.txt", textAsset.text);
		}
		string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseResearch.txt");
		xmlDocument.LoadXml(xml);
		List<ResearchItemTypeInfo> list = new List<ResearchItemTypeInfo>();
		IEnumerator enumerator = xmlDocument.SelectNodes("researchs/research").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				ResearchItemTypeInfo researchItemTypeInfo = new ResearchItemTypeInfo();
				XmlAttributeCollection attributes = xmlNode.Attributes;
				researchItemTypeInfo.id = int.Parse(attributes.GetNamedItem("id").InnerText);
				researchItemTypeInfo.sephira = attributes.GetNamedItem("sephira").InnerText;
				researchItemTypeInfo.maxLevel = int.Parse(attributes.GetNamedItem("maxLevel").InnerText);
				XmlNode namedItem = xmlNode.Attributes.GetNamedItem("icon");
				if (namedItem != null)
				{
					string innerText = namedItem.InnerText;
					researchItemTypeInfo.icon = innerText;
				}
				XmlNode namedItem2 = xmlNode.Attributes.GetNamedItem("atlas");
				if (namedItem2 != null)
				{
					string atlas = namedItem2.InnerText.Trim();
					researchItemTypeInfo.atlas = atlas;
				}
				XmlNode namedItem3 = xmlNode.Attributes.GetNamedItem("type");
				if (namedItem3 != null)
				{
					string innerText2 = namedItem3.InnerText;
					researchItemTypeInfo.type = ResearchDataModel.ConvertResearchType(innerText2);
				}
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("prev");
				if (xmlNode2 != null)
				{
					IEnumerator enumerator2 = xmlNode2.SelectNodes("id").GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode3 = (XmlNode)obj2;
							researchItemTypeInfo.prevResearch.Add(int.Parse(xmlNode3.InnerText));
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
				}
				List<ResearchUpgradeInfo> list2 = new List<ResearchUpgradeInfo>();
				IEnumerator enumerator3 = xmlNode.SelectNodes("upgrade").GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						XmlNode xmlNode4 = (XmlNode)obj3;
						ResearchUpgradeInfo researchUpgradeInfo = new ResearchUpgradeInfo();
						researchUpgradeInfo.cost = 0;
						IEnumerator enumerator4 = xmlNode4.ChildNodes.GetEnumerator();
						try
						{
							while (enumerator4.MoveNext())
							{
								object obj4 = enumerator4.Current;
								XmlNode xmlNode5 = (XmlNode)obj4;
								string name = xmlNode5.Name;
								if (name == "agent")
								{
									ResearchUnitStatUpgrade researchUnitStatUpgrade = new ResearchUnitStatUpgrade();
									IEnumerator enumerator5 = xmlNode5.ChildNodes.GetEnumerator();
									try
									{
										while (enumerator5.MoveNext())
										{
											object obj5 = enumerator5.Current;
											XmlNode xmlNode6 = (XmlNode)obj5;
											string name2 = xmlNode6.Name;
											uint num = ComputeStringHash(name2);
											if (num <= 1816987862U)
											{
												if (num <= 685643787U)
												{
													if (num != 89534040U)
													{
														if (num != 113109323U)
														{
															if (num == 685643787U)
															{
																if (name2 == "workSpeed")
																{
																	researchUnitStatUpgrade.workSpeed = float.Parse(xmlNode6.InnerText);
																}
															}
														}
														else if (name2 == "workProb")
														{
															researchUnitStatUpgrade.workProb = float.Parse(xmlNode6.InnerText);
														}
													}
													else if (name2 == "movement")
													{
														researchUnitStatUpgrade.movement = float.Parse(xmlNode6.InnerText);
													}
												}
												else if (num != 1362809445U)
												{
													if (num != 1420268987U)
													{
														if (num == 1816987862U)
														{
															if (name2 == "regeneration")
															{
																researchUnitStatUpgrade.regeneration = int.Parse(xmlNode6.InnerText);
															}
														}
													}
													else if (name2 == "regenerationMental")
													{
														researchUnitStatUpgrade.regenerationMental = int.Parse(xmlNode6.InnerText);
													}
												}
												else if (name2 == "hp")
												{
													researchUnitStatUpgrade.hp = int.Parse(xmlNode6.InnerText);
												}
											}
											else if (num <= 3021620921U)
											{
												if (num != 2550015916U)
												{
													if (num != 2704916724U)
													{
														if (num == 3021620921U)
														{
															if (name2 == "regenerationDelay")
															{
																researchUnitStatUpgrade.regenerationDelay = (float)int.Parse(xmlNode6.InnerText);
															}
														}
													}
													else if (name2 == "workEnergy")
													{
														researchUnitStatUpgrade.workEnergy = float.Parse(xmlNode6.InnerText);
													}
												}
												else if (name2 == "physicalDefense")
												{
													researchUnitStatUpgrade.physicalDefense = int.Parse(xmlNode6.InnerText);
												}
											}
											else if (num != 3277135992U)
											{
												if (num != 3774733082U)
												{
													if (num == 4175542724U)
													{
														if (name2 == "mentalDefense")
														{
															researchUnitStatUpgrade.mentalDefense = int.Parse(xmlNode6.InnerText);
														}
													}
												}
												else if (name2 == "regenerationMentalDelay")
												{
													researchUnitStatUpgrade.regenerationMentalDelay = (float)int.Parse(xmlNode6.InnerText);
												}
											}
											else if (name2 == "mental")
											{
												researchUnitStatUpgrade.mental = int.Parse(xmlNode6.InnerText);
											}
										}
									}
									finally
									{
										IDisposable disposable2;
										if ((disposable2 = (enumerator5 as IDisposable)) != null)
										{
											disposable2.Dispose();
										}
									}
									researchUpgradeInfo.agentStatBonus = researchUnitStatUpgrade;
								}
								else if (name == "departAbility")
								{
									researchUpgradeInfo.departAbility = new ResearchDepartability
									{
										sephira = xmlNode5.Attributes.GetNamedItem("sephira").InnerText,
										level = int.Parse(xmlNode5.Attributes.GetNamedItem("level").InnerText)
									};
								}
								else if (name == "specialAbility")
								{
									researchUpgradeInfo.specialAbility = new ResearchSpecialAbility
									{
										name = xmlNode5.InnerText
									};
								}
								else if (name == "promotionEasily")
								{
									researchUpgradeInfo.promotionEasily = new ResearchPromotionEasily();
									researchUpgradeInfo.promotionEasily.value = float.Parse(xmlNode5.InnerText);
								}
								else if (name == "bulletAbility")
								{
									ResearchBulletAbility researchBulletAbility = new ResearchBulletAbility();
									string innerText3 = xmlNode5.InnerText;
									switch (innerText3)
									{
										case "recover_hp":
											researchBulletAbility.bulletType = GlobalBulletType.RECOVER_HP;
											break;
										case "recover_mental":
											researchBulletAbility.bulletType = GlobalBulletType.RECOVER_MENTAL;
											break;
										case "resist_r":
											researchBulletAbility.bulletType = GlobalBulletType.RESIST_R;
											break;
										case "resist_w":
											researchBulletAbility.bulletType = GlobalBulletType.RESIST_W;
											break;
										case "resist_b":
											researchBulletAbility.bulletType = GlobalBulletType.RESIST_B;
											break;
										case "resist_p":
											researchBulletAbility.bulletType = GlobalBulletType.RESIST_P;
											break;
										case "slow":
											researchBulletAbility.bulletType = GlobalBulletType.SLOW;
											break;
										case "excute":
											researchBulletAbility.bulletType = GlobalBulletType.EXCUTE;
											break;
										case "stim":
											researchBulletAbility.bulletType = GlobalBulletType.STIM;
											break;
										case "tranq":
											researchBulletAbility.bulletType = GlobalBulletType.TRANQ;
											break;
									}
									researchUpgradeInfo.bulletAility = researchBulletAbility;
								}
							}
						}
						finally
						{
							IDisposable disposable3;
							if ((disposable3 = (enumerator4 as IDisposable)) != null)
							{
								disposable3.Dispose();
							}
						}
						list2.Add(researchUpgradeInfo);
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
				researchItemTypeInfo.upgradeInfos = list2.ToArray();
				list.Add(researchItemTypeInfo);
			}
		}
		finally
		{
			IDisposable disposable5;
			if ((disposable5 = (enumerator as IDisposable)) != null)
			{
				disposable5.Dispose();
			}
		}
		this.LoadResearchDescData(list);
		ResearchItemTypeList.instance.Init(list.AsReadOnly());
	}

	public static uint ComputeStringHash(string s)
	{ // <Mod> added to fix weird decompilation stuff
	// all instances of <PrivateImplementationDetails>__11.ComputeStringHash() have been replaced with ComputeStringHash()
		uint num = new uint();
		if (s != null)
		{
			num = 0x811c9dc5;
			for (int i = 0; i < s.Length; i++)
			{
				num = (s[i] ^ num) * 0x1000193;
			}
		}
		return num;
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0014B868 File Offset: 0x00149A68
	public void LoadResearchDescData(List<ResearchItemTypeInfo> research)
	{
		XmlDocument xmlDocument = AssetLoader.LoadExternalXML("Language/ResearchDesc");
		Dictionary<int, Dictionary<string, ResearchItemDesc>> dictionary = new Dictionary<int, Dictionary<string, ResearchItemDesc>>();
		List<string> list = new List<string>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/supportLanguage/ln").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				list.Add(xmlNode.InnerText);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = xmlDocument.SelectNodes("root/node").GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				XmlNode xmlNode2 = (XmlNode)obj2;
				int key = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("id").InnerText);
				Dictionary<string, ResearchItemDesc> dictionary2 = new Dictionary<string, ResearchItemDesc>();
				foreach (string text in list)
				{
					XmlNode xmlNode3 = xmlNode2.SelectSingleNode(text);
					string innerText = xmlNode3.SelectSingleNode("name").InnerText;
					string innerText2 = xmlNode3.SelectSingleNode("current").InnerText;
					string innerText3 = xmlNode3.SelectSingleNode("short").InnerText;
					innerText.Trim();
					innerText2.Trim();
					dictionary2.Add(text, new ResearchItemDesc
					{
						name = innerText,
						desc = innerText2,
						shortDesc = innerText3
					});
				}
				dictionary.Add(key, dictionary2);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		foreach (ResearchItemTypeInfo researchItemTypeInfo in research)
		{
			Dictionary<string, ResearchItemDesc> desc = null;
			if (dictionary.TryGetValue(researchItemTypeInfo.id, out desc))
			{
				researchItemTypeInfo.desc = desc;
			}
		}
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x0014BA6C File Offset: 0x00149C6C
	private void LoadItemObjectData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/StandingItemData");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		List<ItemObjectData> list = new List<ItemObjectData>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/node").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				ItemObjectData itemObjectData = new ItemObjectData();
				long id = (long)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				string innerText = xmlNode.Attributes.GetNamedItem("name").InnerText;
				int maxHp = (int)float.Parse(xmlNode.Attributes.GetNamedItem("hp").InnerText);
				float defense = float.Parse(xmlNode.Attributes.GetNamedItem("defense").InnerText);
				string innerText2 = xmlNode.Attributes.GetNamedItem("script").InnerText;
				itemObjectData.id = id;
				itemObjectData.name = innerText;
				itemObjectData.maxHp = maxHp;
				itemObjectData.defense = defense;
				itemObjectData.script = innerText2;
				list.Add(itemObjectData);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		ItemObjectManager.instance.Init(list.AsReadOnly());
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x0014BBB4 File Offset: 0x00149DB4
	public void LoadRandomEventInfo()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/RandomEvent/RandomEventInfo");
		TextAsset textAsset2 = Resources.Load<TextAsset>("xml/RandomEvent/RandomEventDesc");
		Dictionary<long, RandomEventInfo> dictionary = new Dictionary<long, RandomEventInfo>();
		new List<RandomEventInfo>();
		XmlDocument xmlDocument = new XmlDocument();
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		xmlDocument2.LoadXml(textAsset2.text);
		IEnumerator enumerator = xmlDocument.SelectNodes("root/data").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				RandomEventInfo randomEventInfo = new RandomEventInfo();
				long id = (long)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				randomEventInfo.id = id;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("script");
				if (xmlNode2 != null)
				{
					randomEventInfo.script = xmlNode2.InnerText.Trim();
				}
				else
				{
					randomEventInfo.script = "RandomEventBase";
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("rank");
				if (xmlNode3 != null)
				{
					randomEventInfo.rank = xmlNode3.InnerText.Trim();
				}
				else
				{
					randomEventInfo.rank = string.Empty;
				}
				XmlNode xmlNode4 = xmlNode.SelectSingleNode("color");
				if (xmlNode4 != null)
				{
					randomEventInfo.color = xmlNode4.InnerText.Trim();
				}
				else
				{
					randomEventInfo.color = "000000FF";
				}
				XmlNode xmlNode5 = xmlNode.SelectSingleNode("type");
				if (xmlNode5 != null)
				{
					randomEventInfo.type = xmlNode5.InnerText.Trim();
				}
				else
				{
					randomEventInfo.type = "no data";
				}
				XmlNode xmlNode6 = xmlNode.SelectSingleNode("condition");
				if (xmlNode6 != null)
				{
					XmlNode namedItem = xmlNode6.Attributes.GetNamedItem("ind");
					bool independentCondition = false;
					if (namedItem != null)
					{
						independentCondition = this.GetBooleanData(namedItem.InnerText.Trim());
					}
					randomEventInfo.independentCondition = independentCondition;
					IEnumerator enumerator2 = xmlNode6.SelectNodes("info").GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode7 = (XmlNode)obj2;
							RandomEventInfo.ConditionInfo conditionInfo = new RandomEventInfo.ConditionInfo();
							int index = (int)float.Parse(xmlNode7.Attributes.GetNamedItem("index").InnerText);
							string condition = xmlNode7.Attributes.GetNamedItem("type").InnerText.Trim();
							string innerText = xmlNode7.Attributes.GetNamedItem("iterative").InnerText;
							bool booleanData = this.GetBooleanData(innerText);
							conditionInfo.index = index;
							conditionInfo.condition = condition;
							conditionInfo.isIterative = booleanData;
							XmlNodeList xmlNodeList = xmlNode7.SelectNodes("param");
							XmlNodeList xmlNodeList2 = xmlNode7.SelectNodes("nest");
							if (xmlNodeList != null)
							{
								IEnumerator enumerator3 = xmlNodeList.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										object obj3 = enumerator3.Current;
										float item = float.Parse(((XmlNode)obj3).InnerText);
										conditionInfo.parameters.Add(item);
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator3 as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
							}
							if (xmlNodeList2 != null)
							{
								IEnumerator enumerator4 = xmlNodeList2.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										object obj4 = enumerator4.Current;
										int item2 = (int)float.Parse(((XmlNode)obj4).InnerText);
										conditionInfo.nested.Add(item2);
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
							randomEventInfo.conditions.Add(conditionInfo);
						}
					}
					finally
					{
						IDisposable disposable3;
						if ((disposable3 = (enumerator2 as IDisposable)) != null)
						{
							disposable3.Dispose();
						}
					}
					dictionary.Add(randomEventInfo.id, randomEventInfo);
				}
				else
				{
					Debug.Log("Cannot pick RandomEventInfo->Condition this will make manual condtion");
				}
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		List<string> list = new List<string>();
		IEnumerator enumerator5 = xmlDocument2.SelectNodes("root/supportLanguage/ln").GetEnumerator();
		try
		{
			while (enumerator5.MoveNext())
			{
				object obj5 = enumerator5.Current;
				XmlNode xmlNode8 = (XmlNode)obj5;
				list.Add(xmlNode8.InnerText);
			}
		}
		finally
		{
			IDisposable disposable5;
			if ((disposable5 = (enumerator5 as IDisposable)) != null)
			{
				disposable5.Dispose();
			}
		}
		IEnumerator enumerator6 = xmlDocument2.SelectNodes("root/node").GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj6 = enumerator6.Current;
				XmlNode xmlNode9 = (XmlNode)obj6;
				long num = (long)float.Parse(xmlNode9.Attributes.GetNamedItem("id").InnerText);
				RandomEventInfo randomEventInfo2 = null;
				if (!dictionary.TryGetValue(num, out randomEventInfo2))
				{
					Debug.LogError("Cannot Get " + num + " info");
				}
				else
				{
					XmlNode xmlNode10 = xmlNode9.SelectSingleNode("name");
					XmlNode xmlNode11 = xmlNode9.SelectSingleNode("desc");
					XmlNode xmlNode12 = xmlNode9.SelectSingleNode("clear");
					XmlNode xmlNode13 = xmlNode9.SelectSingleNode("type");
					XmlNode xmlNode14 = xmlNode9.SelectSingleNode("endTitle");
					foreach (string text in list)
					{
						string innerText2 = xmlNode10.SelectSingleNode(text).InnerText;
						string innerText3 = xmlNode11.SelectSingleNode(text).InnerText;
						string innerText4 = xmlNode12.SelectSingleNode(text).InnerText;
						string innerText5 = xmlNode13.SelectSingleNode(text).InnerText;
						string innerText6 = xmlNode14.SelectSingleNode(text).InnerText;
						randomEventInfo2.nameLib.Add(text, innerText2);
						randomEventInfo2.descLib.Add(text, innerText3);
						randomEventInfo2.clearLib.Add(text, innerText4);
						randomEventInfo2.typeLib.Add(text, innerText5);
						randomEventInfo2.endTitleLib.Add(text, innerText6);
					}
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
		RandomEventManager.instance.InfoInit(dictionary);
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x0014C1F0 File Offset: 0x0014A3F0
	public void LoadWorkerSpineData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("xml/Worker/WorkerUniqueAnim");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		List<WorkerSpineAnimatorData> list = new List<WorkerSpineAnimatorData>();
		IEnumerator enumerator = xmlDocument.SelectNodes("root/node").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("name").InnerText;
				int id = (int)float.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				string innerText2 = xmlNode.SelectSingleNode("animator").InnerText;
				string innerText3 = xmlNode.SelectSingleNode("skeletonData").InnerText;
				WorkerSpineAnimatorData item = new WorkerSpineAnimatorData(id, innerText, innerText2, innerText3);
				list.Add(item);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		WorkerSpineAnimatorManager.instance.Init(list);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x0002D31D File Offset: 0x0002B51D
	static GameStaticDataLoader()
	{
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x0014C2E0 File Offset: 0x0014A4E0
	public void LoadModIsolate(Dictionary<string, XmlNode> nodeRoot)
	{
		try
		{
			List<XmlDocument> list = new List<XmlDocument>();
			List<XmlDocument> list2 = new List<XmlDocument>();
			foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
			{
				string path = directoryInfo.FullName + "/SefiraIsolate";
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
					this.LoadModIsolate_Replace(nodeRoot, mxml);
				}
			}
			if (list.Count > 0)
			{
				foreach (XmlDocument mxml2 in list)
				{
					this.LoadModIsolate_Add(nodeRoot, mxml2);
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/LMIerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x0014C530 File Offset: 0x0014A730
	public void LoadModIsolate_Add(Dictionary<string, XmlNode> nodeRoot, XmlDocument mxml)
	{
		XmlNode xmlNode = mxml.SelectSingleNode("root");
		if (xmlNode != null)
		{
			foreach (object obj in xmlNode)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
				foreach (object obj2 in xmlNode2)
				{
					XmlNode xmlNode3 = (XmlNode)obj2;
					if (xmlNode3.Name == "node")
					{
						XmlNode newChild = nodeRoot[innerText].OwnerDocument.ImportNode(xmlNode3, true);
						nodeRoot[innerText].AppendChild(newChild);
					}
				}
			}
		}
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x0014C624 File Offset: 0x0014A824
	public void LoadModIsolate_Replace(Dictionary<string, XmlNode> nodeRoot, XmlDocument mxml)
	{
		try
		{
			XmlNode xmlNode = mxml.SelectSingleNode("root");
			if (xmlNode != null)
			{
				foreach (object obj in xmlNode)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					string innerText = xmlNode2.Attributes.GetNamedItem("name").InnerText;
					XmlNode xmlNode3 = nodeRoot[innerText].OwnerDocument.SelectSingleNode("root");
					XmlNode xmlNode4 = xmlNode3.OwnerDocument.ImportNode(xmlNode2, true);
					xmlNode3.RemoveChild(nodeRoot[innerText]);
					xmlNode3.AppendChild(xmlNode4);
					nodeRoot[innerText] = xmlNode4;
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/LMIRerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
	}

	// Token: 0x04002E74 RID: 11892
	private static string currentLn = string.Empty;

	// Token: 0x04002E75 RID: 11893
	[CompilerGenerated]
	private static Comparison<CreditItem> __mg_cache0;
}
