/*
private void LoadCreatureStat(XmlNode stat, XmlNode statCreature, CreatureTypeInfo model) // (!) 
private void LoadCreatureStat_Mod(XmlNode stat, XmlNode statCreature, CreatureTypeInfo model, string modid) // 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using LobotomyBaseMod; // 
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class CreatureDataLoader
{
	// Token: 0x06002E72 RID: 11890 RVA: 0x0002C007 File Offset: 0x0002A207
	public CreatureDataLoader()
	{
	}

	// Token: 0x06002E73 RID: 11891 RVA: 0x00135F9C File Offset: 0x0013419C
	public void Load()
	{ // <Patch>
		LobotomyBaseMod.ModDebug.Log("CDL Load 1");
		try
		{
			LobotomyBaseMod.ModDebug.Log("CDL Load 2");
			FieldInfo field = typeof(CreatureGenerateInfo).GetField("all", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			List<long> list = new List<long>();
			List<LobotomyBaseMod.LcIdLong> list2 = new List<LobotomyBaseMod.LcIdLong>();
			string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatureGen.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			foreach (object obj in xmlDocument.SelectNodes("/All/id"))
			{
				long item = (long)int.Parse(((XmlNode)obj).InnerText);
				list.Add(item);
			}
			foreach (ModInfo modInfo in Add_On.instance.ModList)
			{
				DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(modInfo.modpath, "Creature");
				bool flag = directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/CreatureGen");
				if (flag)
				{
					DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/CreatureGen");
					bool flag2 = directoryInfo2.GetFiles().Length != 0;
					if (flag2)
					{
						foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
						{
							bool flag3 = fileInfo.Name.Contains(".xml") || fileInfo.Name.Contains(".txt");
							if (flag3)
							{
								xml = File.ReadAllText(fileInfo.FullName);
								XmlDocument xmlDocument2 = new XmlDocument();
								xmlDocument2.LoadXml(xml);
								bool flag4 = modInfo.modid == string.Empty;
								if (flag4)
								{
									foreach (object obj2 in xmlDocument2.SelectNodes("/All/add"))
									{
										long item2 = (long)int.Parse(((XmlNode)obj2).InnerText);
										list.Add(item2);
									}
									foreach (object obj3 in xmlDocument2.SelectNodes("/All/remove"))
									{
										long item3 = (long)int.Parse(((XmlNode)obj3).InnerText);
										list.Remove(item3);
									}
								}
								else
								{
									foreach (object obj4 in xmlDocument2.SelectNodes("/All/add"))
									{
										long id = (long)int.Parse(((XmlNode)obj4).InnerText);
										LobotomyBaseMod.LcIdLong item4 = new LobotomyBaseMod.LcIdLong(modInfo.modid, id);
										list2.Add(item4);
									}
									foreach (object obj5 in xmlDocument2.SelectNodes("/All/remove"))
									{
										long id2 = (long)int.Parse(((XmlNode)obj5).InnerText);
										LobotomyBaseMod.LcIdLong item5 = new LobotomyBaseMod.LcIdLong(modInfo.modid, id2);
										list2.Remove(item5);
									}
								}
							}
						}
					}
				}
			}
			LobotomyBaseMod.ModDebug.Log("CDL Load 3");
			field.SetValue(null, list.ToArray());
			CreatureGenerateInfo.all_mod = list2;
			bool flag5 = !EquipmentTypeList.instance.loaded;
			if (flag5)
			{
				LobotomyBaseMod.ModDebug.Log("LoadCreatureList >> EquipmentTypeList must be loaded. ");
			}
			this.currentLn = GlobalGameManager.instance.GetCurrentLanguage();
			string xml2 = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseList.txt");
			XmlDocument xmlDocument3 = new XmlDocument();
			xmlDocument3.LoadXml(xml2);
			XmlNodeList xmlNodeList = xmlDocument3.SelectNodes("/creature_list/creature");
			List<CreatureTypeInfo> list3 = new List<CreatureTypeInfo>();
			Dictionary<string, List<CreatureTypeInfo>> dictionary = new Dictionary<string, List<CreatureTypeInfo>>();
			List<CreatureSpecialSkillTipTable> list4 = new List<CreatureSpecialSkillTipTable>();
			Dictionary<string, List<CreatureSpecialSkillTipTable>> dictionary2 = new Dictionary<string, List<CreatureSpecialSkillTipTable>>();
			Dictionary<long, int> dictionary3 = new Dictionary<long, int>();
			Loading(xmlNodeList, list3, list4, dictionary3, false);
			LobotomyBaseMod.ModDebug.Log("CDL Load 4");
			foreach (ModInfo modInfo2 in Add_On.instance.ModList)
			{
				DirectoryInfo directoryInfo3 = EquipmentDataLoader.CheckNamedDir(modInfo2.modpath, "Creature");
				bool flag6 = directoryInfo3 != null && Directory.Exists(directoryInfo3.FullName + "/CreatureList");
				if (flag6)
				{
					DirectoryInfo directoryInfo4 = new DirectoryInfo(directoryInfo3.FullName + "/CreatureList");
					bool flag7 = directoryInfo4.GetFiles().Length != 0;
					if (flag7)
					{
						bool flag8 = modInfo2.modid != string.Empty;
						if (flag8)
						{
							dictionary[modInfo2.modid] = new List<CreatureTypeInfo>();
							dictionary2[modInfo2.modid] = new List<CreatureSpecialSkillTipTable>();
						}
						foreach (FileInfo fileInfo2 in directoryInfo4.GetFiles())
						{
							bool flag9 = fileInfo2.Name.Contains(".txt") || fileInfo2.Name.Contains(".xml");
							if (flag9)
							{
								bool flag10 = modInfo2.modid == string.Empty;
								if (flag10)
								{
									XmlDocument xmlDocument4 = new XmlDocument();
									xmlDocument4.LoadXml(File.ReadAllText(fileInfo2.FullName));
									XmlNodeList xmlNodeList2 = xmlDocument4.SelectNodes("/creature_list/creature");
									List<CreatureTypeInfo> list5 = new List<CreatureTypeInfo>();
									List<CreatureSpecialSkillTipTable> list6 = new List<CreatureSpecialSkillTipTable>();
									Dictionary<long, int> dictionary4 = new Dictionary<long, int>();
									Loading(xmlNodeList2, list5, list6, dictionary4, true);
									foreach (KeyValuePair<long, int> keyValuePair in dictionary4)
									{
										bool flag11 = dictionary3.ContainsKey(keyValuePair.Key);
										if (flag11)
										{
											for (int k = 0; k < list3.Count; k++)
											{
												dictionary3.Remove(keyValuePair.Key);
												dictionary3.Add(keyValuePair.Key, keyValuePair.Value);
											}
										}
									}
									foreach (CreatureTypeInfo creatureTypeInfo in list5)
									{
										foreach (CreatureTypeInfo creatureTypeInfo2 in list3)
										{
											bool flag12 = creatureTypeInfo.id == creatureTypeInfo2.id;
											if (flag12)
											{
												list3.Remove(creatureTypeInfo2);
												break;
											}
										}
										list3.Add(creatureTypeInfo);
									}
									foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable in list6)
									{
										foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable2 in list4)
										{
											bool flag13 = creatureSpecialSkillTipTable2.creatureTypeId == creatureSpecialSkillTipTable.creatureTypeId;
											if (flag13)
											{
												list4.Remove(creatureSpecialSkillTipTable2);
												break;
											}
										}
										list4.Add(creatureSpecialSkillTipTable);
									}
								}
								else
								{
									XmlDocument xmlDocument5 = new XmlDocument();
									xmlDocument5.LoadXml(File.ReadAllText(fileInfo2.FullName));
									XmlNodeList xmlNodeList3 = xmlDocument5.SelectNodes("/creature_list/creature");
									List<CreatureTypeInfo> list7 = new List<CreatureTypeInfo>();
									List<CreatureSpecialSkillTipTable> list8 = new List<CreatureSpecialSkillTipTable>();
									Dictionary<long, int> specialTipSize = new Dictionary<long, int>();
									this.Loading_Mod(xmlNodeList3, list7, list8, specialTipSize, modInfo2.modid);
									foreach (CreatureTypeInfo creatureTypeInfo3 in list7)
									{
										creatureTypeInfo3.modid = modInfo2.modid;
									}
									foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable3 in list8)
									{
										creatureSpecialSkillTipTable3.modid = modInfo2.modid;
									}
									dictionary[modInfo2.modid].AddRange(list7);
									dictionary2[modInfo2.modid].AddRange(list8);
								}
							}
						}
					}
				}
			}
			LobotomyBaseMod.ModDebug.Log("CDL Load 5");
			CreatureTypeList.instance.Init_Mod(dictionary, dictionary2);
			CreatureTypeList.instance.Init(list3.ToArray(), list4.ToArray(), dictionary3);
			LobotomyBaseMod.ModDebug.Log("CDL Load 6");
		}
		catch (Exception ex)
		{
			LobotomyBaseMod.ModDebug.Log("CDLerror - " + ex.Message + Environment.NewLine + ex.StackTrace);
		}
		/*
		try
		{
			FieldInfo field = typeof(CreatureGenerateInfo).GetField("all", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			List<long> list = new List<long>();
			string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatureGen.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			foreach (object obj in xmlDocument.SelectNodes("/All/id"))
			{
				long item = (long)int.Parse(((XmlNode)obj).InnerText);
				list.Add(item);
			}
			foreach (DirectoryInfo dir in Add_On.instance.DirList)
			{
				DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Creature");
				if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/CreatureGen"))
				{
					DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/CreatureGen");
					if (directoryInfo2.GetFiles().Length != 0)
					{
						foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
						{
							if (fileInfo.Name.Contains(".xml") || fileInfo.Name.Contains(".txt"))
							{
								xml = File.ReadAllText(fileInfo.FullName);
								XmlDocument xmlDocument2 = new XmlDocument();
								xmlDocument2.LoadXml(xml);
								foreach (object obj2 in xmlDocument2.SelectNodes("/All/add"))
								{
									long item2 = (long)int.Parse(((XmlNode)obj2).InnerText);
									list.Add(item2);
								}
								foreach (object obj3 in xmlDocument2.SelectNodes("/All/remove"))
								{
									long item3 = (long)int.Parse(((XmlNode)obj3).InnerText);
									list.Remove(item3);
								}
							}
						}
					}
				}
			}
			field.SetValue(null, list.ToArray());
			if (!EquipmentTypeList.instance.loaded)
			{
				Debug.LogError("LoadCreatureList >> EquipmentTypeList must be loaded. ");
			}
			this.currentLn = GlobalGameManager.instance.GetCurrentLanguage();
			string xml2 = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseList.txt");
			XmlDocument xmlDocument3 = new XmlDocument();
			xmlDocument3.LoadXml(xml2);
			XmlNodeList xmlNodeList = xmlDocument3.SelectNodes("/creature_list/creature");
			List<CreatureTypeInfo> list2 = new List<CreatureTypeInfo>();
			List<CreatureSpecialSkillTipTable> list3 = new List<CreatureSpecialSkillTipTable>();
			Dictionary<long, int> dictionary = new Dictionary<long, int>();
			this.Loading(xmlNodeList, list2, list3, dictionary, false);
			foreach (DirectoryInfo dir2 in Add_On.instance.DirList)
			{
				DirectoryInfo directoryInfo3 = EquipmentDataLoader.CheckNamedDir(dir2, "Creature");
				if (directoryInfo3 != null && Directory.Exists(directoryInfo3.FullName + "/CreatureList"))
				{
					DirectoryInfo directoryInfo4 = new DirectoryInfo(directoryInfo3.FullName + "/CreatureList");
					if (directoryInfo4.GetFiles().Length != 0)
					{
						foreach (FileInfo fileInfo2 in directoryInfo4.GetFiles())
						{
							if (fileInfo2.Name.Contains(".txt") || fileInfo2.Name.Contains(".xml"))
							{
								XmlDocument xmlDocument4 = new XmlDocument();
								xmlDocument4.LoadXml(File.ReadAllText(fileInfo2.FullName));
								XmlNodeList xmlNodeList2 = xmlDocument4.SelectNodes("/creature_list/creature");
								List<CreatureTypeInfo> list4 = new List<CreatureTypeInfo>();
								List<CreatureSpecialSkillTipTable> list5 = new List<CreatureSpecialSkillTipTable>();
								Dictionary<long, int> dictionary2 = new Dictionary<long, int>();
								this.Loading(xmlNodeList2, list4, list5, dictionary2, true);
								foreach (KeyValuePair<long, int> keyValuePair in dictionary2)
								{
									if (dictionary.ContainsKey(keyValuePair.Key))
									{
										for (int j = 0; j < list2.Count; j++)
										{
											dictionary.Remove(keyValuePair.Key);
											dictionary.Add(keyValuePair.Key, keyValuePair.Value);
										}
									}
								}
								foreach (CreatureTypeInfo creatureTypeInfo in list4)
								{
									foreach (CreatureTypeInfo creatureTypeInfo2 in list2)
									{
										if (creatureTypeInfo.id == creatureTypeInfo2.id)
										{
											list2.Remove(creatureTypeInfo2);
											break;
										}
									}
									list2.Add(creatureTypeInfo);
								}
								foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable in list5)
								{
									foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable2 in list3)
									{
										if (creatureSpecialSkillTipTable2.creatureTypeId == creatureSpecialSkillTipTable.creatureTypeId)
										{
											list3.Remove(creatureSpecialSkillTipTable2);
											break;
										}
									}
									list3.Add(creatureSpecialSkillTipTable);
								}
							}
						}
					}
				}
			}
			CreatureTypeList.instance.Init(list2.ToArray(), list3.ToArray(), dictionary);
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/CDLerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}*/
	}

	// Token: 0x06002E74 RID: 11892 RVA: 0x00136658 File Offset: 0x00134858
	private void LoadCreatureStat(XmlNode stat, XmlNode statCreature, CreatureTypeInfo model)
	{ // <Mod> initialize gifts with a reference to the abnormality that they belong to
		XmlNode xmlNode;
		if ((xmlNode = statCreature.SelectSingleNode("script")) != null)
		{
			model.script = xmlNode.InnerText;
		}
		XmlNode xmlNode32;
		if ((xmlNode32 = stat.SelectSingleNode("riskLevel")) != null)
		{
			string innerText = xmlNode32.InnerText;
			model._riskLevel = int.Parse(innerText);
		}
		XmlNode xmlNode2;
		if ((xmlNode2 = statCreature.SelectSingleNode("workAnim")) != null)
		{
			model.workAnim = xmlNode2.InnerText;
			XmlNode namedItem = xmlNode2.Attributes.GetNamedItem("face");
			if (namedItem != null)
			{
				model.workAnimFace = namedItem.InnerText;
			}
		}
		XmlNode xmlNode3;
		if ((xmlNode3 = statCreature.SelectSingleNode("kitIcon")) != null)
		{
			model.kitIconSrc = xmlNode3.InnerText;
		}
		XmlNode xmlNode4;
		if ((xmlNode4 = stat.SelectSingleNode("workType")) != null)
		{
			string innerText = xmlNode4.InnerText;
			if (innerText != null)
			{
				if (!(innerText == "normal"))
				{
					if (innerText == "kit")
					{
						model.creatureWorkType = CreatureWorkType.KIT;
					}
				}
				else
				{
					model.creatureWorkType = CreatureWorkType.NORMAL;
				}
			}
		}
		XmlNode xmlNode5;
		if ((xmlNode5 = stat.SelectSingleNode("kitType")) != null)
		{
			string innerText2 = xmlNode5.InnerText;
			if (innerText2 != null)
			{
				if (!(innerText2 == "equip"))
				{
					if (!(innerText2 == "channel"))
					{
						if (innerText2 == "oneshot")
						{
							model.creatureKitType = CreatureKitType.ONESHOT;
						}
					}
					else
					{
						model.creatureKitType = CreatureKitType.CHANNEL;
					}
				}
				else
				{
					model.creatureKitType = CreatureKitType.EQUIP;
				}
			}
		}
		XmlNode xmlNode6;
		if ((xmlNode6 = stat.SelectSingleNode("qliphoth")) != null)
		{
			model.qliphothMax = int.Parse(xmlNode6.InnerText);
		}
		XmlNode xmlNode7;
		if ((xmlNode7 = stat.SelectSingleNode("speed")) != null)
		{
			model.speed = float.Parse(xmlNode7.InnerText);
		}
		XmlNode xmlNode8 = stat.SelectSingleNode("escapeable");
		if (xmlNode8 != null)
		{
			bool booleanData = this.GetBooleanData(xmlNode8.InnerText.Trim());
			model.isEscapeAble = booleanData;
		}
		else
		{
			model.isEscapeAble = true;
		}
		XmlNode xmlNode9 = stat.SelectSingleNode("hp");
		if (xmlNode9 != null)
		{
			model.maxHp = (int)float.Parse(xmlNode9.InnerText);
		}
		else
		{
			model.maxHp = 5;
		}
		IEnumerator enumerator = stat.SelectNodes("workProb").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode10 = (XmlNode)obj;
				RwbpType type = CreatureDataLoader.ConvertToRWBP(xmlNode10.Attributes.GetNamedItem("type").InnerText);
				IEnumerator enumerator2 = xmlNode10.SelectNodes("prob").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode11 = (XmlNode)obj2;
						int level = int.Parse(xmlNode11.Attributes.GetNamedItem("level").InnerText);
						float prob = float.Parse(xmlNode11.InnerText);
						model.workProbTable.SetWorkProb(type, level, prob);
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
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		XmlNode xmlNode12 = stat.SelectSingleNode("workCooltime");
		if (xmlNode12 != null)
		{
			model.workCooltime = int.Parse(xmlNode12.InnerText);
		}
		XmlNode xmlNode13 = stat.SelectSingleNode("workSpeed");
		if (xmlNode13 != null)
		{
			model.cubeSpeed = float.Parse(xmlNode13.InnerText);
		}
		XmlNode xmlNode14 = statCreature.SelectSingleNode("skillTrigger");
		if (xmlNode14 != null)
		{
			this.LoadSkillTrigger(xmlNode14, model);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		IEnumerator enumerator3 = statCreature.SelectNodes("sound").GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				XmlNode xmlNode15 = (XmlNode)obj3;
				string innerText3 = xmlNode15.Attributes.GetNamedItem("action").InnerText;
				string innerText4 = xmlNode15.Attributes.GetNamedItem("src").InnerText;
				dictionary.Add(innerText3, innerText4);
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
		model.soundTable = dictionary;
		model.nodeInfo = statCreature.SelectNodes("graph/node");
		model.edgeInfo = statCreature.SelectNodes("graph/edge");
		XmlNode xmlNode16 = statCreature.SelectSingleNode("anim");
		if (xmlNode16 != null)
		{
			model.animSrc = xmlNode16.Attributes.GetNamedItem("prefab").InnerText;
		}
		XmlNode xmlNode17 = statCreature.SelectSingleNode("returnImg");
		if (xmlNode17 != null)
		{
			model.roomReturnSrc = xmlNode17.Attributes.GetNamedItem("src").InnerText;
		}
		else
		{
			model.roomReturnSrc = string.Empty;
		}
		XmlNode xmlNode18 = stat.SelectSingleNode("feelingStateCubeBounds");
		if (xmlNode18 != null)
		{
			List<int> list = new List<int>();
			IEnumerator enumerator4 = xmlNode18.SelectNodes("cube").GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					XmlNode xmlNode19 = (XmlNode)obj4;
					list.Add(int.Parse(xmlNode19.InnerText));
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
			model.feelingStateCubeBounds.upperBounds = list.ToArray();
		}
		XmlNode xmlNode20 = stat.SelectSingleNode("workDamage");
		if (xmlNode20 != null)
		{
			model.workDamage = CreatureDataLoader.ConvertToDamageInfo(xmlNode20);
		}
		XmlNode xmlNode21 = stat.SelectSingleNode("specialDamage");
		if (xmlNode21 != null)
		{
			Dictionary<string, EquipmentTypeInfo> dictionary2 = new Dictionary<string, EquipmentTypeInfo>();
			IEnumerator enumerator5 = xmlNode21.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					object obj5 = enumerator5.Current;
					XmlNode xmlNode22 = (XmlNode)obj5;
					if (xmlNode22.Name == "damage")
					{
						string innerText5 = xmlNode22.Attributes.GetNamedItem("id").InnerText;
						EquipmentTypeInfo value = EquipmentTypeInfo.MakeWeaponInfoByDamageInfo(CreatureDataLoader.ConvertToDamageInfo(xmlNode22));
						dictionary2.Add(innerText5, value);
					}
					else if (xmlNode22.Name == "weapon")
					{
						string innerText6 = xmlNode22.Attributes.GetNamedItem("id").InnerText;
						string innerText7 = xmlNode22.Attributes.GetNamedItem("weaponId").InnerText;
						EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(int.Parse(innerText7));
						dictionary2.Add(innerText6, data);
					}
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
			model.creatureSpecialDamageTable.Init(dictionary2);
		}
		Dictionary<string, DefenseInfo> dictionary3 = new Dictionary<string, DefenseInfo>();
		IEnumerator enumerator6 = stat.SelectNodes("defense").GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj6 = enumerator6.Current;
				XmlNode xmlNode23 = (XmlNode)obj6;
				string innerText8 = xmlNode23.Attributes.GetNamedItem("id").InnerText;
				DefenseInfo defenseInfo = new DefenseInfo();
				IEnumerator enumerator7 = xmlNode23.SelectNodes("defenseElement").GetEnumerator();
				try
				{
					while (enumerator7.MoveNext())
					{
						object obj7 = enumerator7.Current;
						XmlNode xmlNode24 = (XmlNode)obj7;
						string innerText9 = xmlNode24.Attributes.GetNamedItem("type").InnerText;
						if (innerText9 != null)
						{
							if (!(innerText9 == "R"))
							{
								if (!(innerText9 == "W"))
								{
									if (!(innerText9 == "B"))
									{
										if (innerText9 == "P")
										{
											defenseInfo.P = float.Parse(xmlNode24.InnerText);
										}
									}
									else
									{
										defenseInfo.B = float.Parse(xmlNode24.InnerText);
									}
								}
								else
								{
									defenseInfo.W = float.Parse(xmlNode24.InnerText);
								}
							}
							else
							{
								defenseInfo.R = float.Parse(xmlNode24.InnerText);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable6;
					if ((disposable6 = (enumerator7 as IDisposable)) != null)
					{
						disposable6.Dispose();
					}
				}
				dictionary3.Add(innerText8, defenseInfo);
			}
		}
		finally
		{
			IDisposable disposable7;
			if ((disposable7 = (enumerator6 as IDisposable)) != null)
			{
				disposable7.Dispose();
			}
		}
		model.defenseTable.Init(dictionary3);
		XmlNode xmlNode25 = stat.SelectSingleNode("observeInfo");
		if (xmlNode25 != null)
		{
			List<ObserveInfoData> list2 = new List<ObserveInfoData>();
			IEnumerator enumerator8 = xmlNode25.SelectNodes("observeElement").GetEnumerator();
			try
			{
				while (enumerator8.MoveNext())
				{
					object obj8 = enumerator8.Current;
					XmlNode xmlNode26 = (XmlNode)obj8;
					string regionName = xmlNode26.Attributes.GetNamedItem("name").InnerText.Trim();
					int observeCost = (int)float.Parse(xmlNode26.Attributes.GetNamedItem("cost").InnerText);
					ObserveInfoData item = new ObserveInfoData
					{
						observeCost = observeCost,
						regionName = regionName
					};
					list2.Add(item);
				}
			}
			finally
			{
				IDisposable disposable8;
				if ((disposable8 = (enumerator8 as IDisposable)) != null)
				{
					disposable8.Dispose();
				}
			}
			model.observeData = list2;
		}
		else
		{
			List<ObserveInfoData> list3 = new List<ObserveInfoData>();
			for (int i = 0; i < CreatureModel.regionName.Length; i++)
			{
				ObserveInfoData item2 = new ObserveInfoData
				{
					observeCost = 0,
					regionName = CreatureModel.regionName[i]
				};
				list3.Add(item2);
			}
			for (int j = 0; j < CreatureModel.careTakingRegion.Length; j++)
			{
				ObserveInfoData item3 = new ObserveInfoData
				{
					observeCost = 0,
					regionName = CreatureModel.careTakingRegion[j]
				};
				list3.Add(item3);
			}
			model.observeData = list3;
		}
		List<CreatureEquipmentMakeInfo> list4 = new List<CreatureEquipmentMakeInfo>();
		IEnumerator enumerator9 = stat.SelectNodes("equipment").GetEnumerator();
		try
		{
			while (enumerator9.MoveNext())
			{
				object obj9 = enumerator9.Current;
				XmlNode xmlNode27 = (XmlNode)obj9;
				XmlNode namedItem2 = xmlNode27.Attributes.GetNamedItem("equipId");
				XmlNode namedItem3 = xmlNode27.Attributes.GetNamedItem("level");
				XmlNode namedItem4 = xmlNode27.Attributes.GetNamedItem("cost");
				XmlNode namedItem5 = xmlNode27.Attributes.GetNamedItem("prob");
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = new CreatureEquipmentMakeInfo();
				if (namedItem2 != null)
				{
					int id = int.Parse(namedItem2.InnerText);
					creatureEquipmentMakeInfo.equipTypeInfo = EquipmentTypeList.instance.GetData(id);
					if (creatureEquipmentMakeInfo.equipTypeInfo == null)
					{
						continue;
					}
				}
				if (namedItem3 != null)
				{
					creatureEquipmentMakeInfo.level = int.Parse(namedItem3.InnerText);
				}
				if (namedItem4 != null)
				{
					creatureEquipmentMakeInfo.cost = int.Parse(namedItem4.InnerText);
				}
				if (namedItem5 != null)
				{
					creatureEquipmentMakeInfo.prob = float.Parse(namedItem5.InnerText);
				}
				creatureEquipmentMakeInfo.creature = model; // 
				list4.Add(creatureEquipmentMakeInfo);
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
		model.equipMakeInfos = list4;
		List<CreatureObserveBonusData> list5 = new List<CreatureObserveBonusData>();
		IEnumerator enumerator10 = stat.SelectNodes("observeBonus").GetEnumerator();
		try
		{
			while (enumerator10.MoveNext())
			{
				object obj10 = enumerator10.Current;
				XmlNode xmlNode28 = (XmlNode)obj10;
				int level2 = int.Parse(xmlNode28.Attributes.GetNamedItem("level").InnerText);
				string innerText10 = xmlNode28.Attributes.GetNamedItem("type").InnerText;
				CreatureObserveBonusData creatureObserveBonusData = new CreatureObserveBonusData();
				if (innerText10 != null)
				{
					if (!(innerText10 == "prob"))
					{
						if (innerText10 == "speed")
						{
							creatureObserveBonusData.bonus = CreatureObserveBonusData.BonusType.SPEED;
						}
					}
					else
					{
						creatureObserveBonusData.bonus = CreatureObserveBonusData.BonusType.PROB;
					}
				}
				creatureObserveBonusData.level = level2;
				creatureObserveBonusData.value = int.Parse(xmlNode28.InnerText);
				list5.Add(creatureObserveBonusData);
			}
		}
		finally
		{
			IDisposable disposable10;
			if ((disposable10 = (enumerator10 as IDisposable)) != null)
			{
				disposable10.Dispose();
			}
		}
		model.observeBonus.Init(list5);
		XmlNode xmlNode29 = stat.SelectSingleNode("maxWorkCount");
		if (xmlNode29 != null)
		{
			model.maxWorkCount = int.Parse(xmlNode29.InnerText);
		}
		XmlNode xmlNode30 = stat.SelectSingleNode("maxProbReductionCounter");
		if (xmlNode30 != null)
		{
			model.maxProbReductionCounter = int.Parse(xmlNode30.InnerText);
		}
		XmlNode xmlNode31 = stat.SelectSingleNode("probReduction");
		if (xmlNode31 != null)
		{
			model.probReduction = float.Parse(xmlNode31.InnerText);
		}
	}

	// Token: 0x06002E75 RID: 11893 RVA: 0x0013723C File Offset: 0x0013543C
	public CreatureTypeInfo LoadCreatureTypeInfo(XmlDocument doc, ref List<CreatureSpecialSkillTipTable> creatureSpecialSkillTipList, ref Dictionary<long, int> specialTipSizeLib, out ChildCreatureData childData)
	{
		XmlNode xmlNode = doc.SelectSingleNode("/creature/info");
		XmlNode xmlNode2 = doc.SelectSingleNode("/creature/observe");
		XmlNode xmlNode3 = doc.SelectSingleNode("/creature/etc");
		XmlNode xmlNode4 = doc.SelectSingleNode("/creature/child");
		CreatureTypeInfo creatureTypeInfo = new CreatureTypeInfo();
		creatureTypeInfo.id = long.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
		LobotomyBaseMod.ModDebug.Log(creatureTypeInfo.id.ToString());
		ChildCreatureData childCreatureData = new ChildCreatureData();
		if (xmlNode4 != null)
		{
			XmlNode xmlNode5 = xmlNode4.SelectSingleNode("name");
			if (xmlNode5 != null)
			{
				childCreatureData.name = xmlNode5.InnerText;
			}
			XmlNode xmlNode6 = xmlNode4.SelectSingleNode("codeId");
			if (xmlNode6 != null)
			{
				childCreatureData.codeId = xmlNode6.InnerText;
			}
		}
		childData = childCreatureData;
		CreatureSpecialSkillTipTable creatureSpecialSkillTipTable = new CreatureSpecialSkillTipTable(creatureTypeInfo.id);
		XmlNode xmlNode7 = xmlNode2.SelectSingleNode("specialTipSize");
		if (xmlNode7 != null)
		{
			int value = (int)float.Parse(xmlNode7.Attributes.GetNamedItem("size").InnerText);
			specialTipSizeLib.Add(creatureTypeInfo.id, value);
			XmlNodeList xmlNodeList = xmlNode7.SelectNodes("specialTip");
			int num = 0;
			IEnumerator enumerator = xmlNodeList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode8 = (XmlNode)obj;
					CreatureSpecialSkillDesc creatureSpecialSkillDesc = new CreatureSpecialSkillDesc();
					creatureSpecialSkillDesc.key = xmlNode8.Attributes.GetNamedItem("key").InnerText;
					if (xmlNode8.Attributes.GetNamedItem("openLevel") != null)
					{
						creatureSpecialSkillDesc.openLevel = (int)float.Parse(xmlNode8.Attributes.GetNamedItem("openLevel").InnerText);
					}
					else
					{
						creatureSpecialSkillDesc.openLevel = 1;
					}
					creatureSpecialSkillDesc.index = num;
					creatureSpecialSkillDesc.desc = xmlNode8.InnerText;
					creatureSpecialSkillDesc.original = creatureSpecialSkillDesc.desc;
					num++;
					creatureSpecialSkillTipTable.descList.Add(creatureSpecialSkillDesc);
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
			creatureSpecialSkillTipList.Add(creatureSpecialSkillTipTable);
		}
		XmlNode xmlNode9 = xmlNode2.SelectSingleNode("max");
		if (xmlNode9 != null)
		{
			CreatureMaxObserve maxObserveModule = creatureTypeInfo.maxObserveModule;
			XmlNodeList xmlNodeList2 = xmlNode9.SelectNodes("desc");
			if (xmlNodeList2 != null)
			{
				IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode10 = (XmlNode)obj2;
						CreatureMaxObserve.Desc desc = new CreatureMaxObserve.Desc();
						desc.id = (int)float.Parse(xmlNode10.Attributes.GetNamedItem("id").InnerText);
						desc.selectId = (int)float.Parse(xmlNode10.Attributes.GetNamedItem("select").InnerText);
						string[] textFromFormatProcessText = TextConverter.GetTextFromFormatProcessText(xmlNode10.InnerText.Trim());
						desc.Init(textFromFormatProcessText);
						maxObserveModule.descs.Add(desc);
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
			string[] textFromFormatProcessText2 = TextConverter.GetTextFromFormatProcessText(xmlNode9.SelectSingleNode("desc").InnerText.Trim());
			maxObserveModule.desc.Init(textFromFormatProcessText2);
			XmlNodeList xmlNodeList3 = xmlNode9.SelectNodes("select");
			if (xmlNodeList3 != null)
			{
				IEnumerator enumerator3 = xmlNodeList3.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						XmlNode xmlNode11 = (XmlNode)obj3;
						XmlNodeList xmlNodeList4 = xmlNode11.SelectNodes("node");
						CreatureMaxObserve.Select select = new CreatureMaxObserve.Select();
						select.id = (int)float.Parse(xmlNode11.Attributes.GetNamedItem("id").InnerText);
						IEnumerator enumerator4 = xmlNodeList4.GetEnumerator();
						try
						{
							while (enumerator4.MoveNext())
							{
								object obj4 = enumerator4.Current;
								XmlNode xmlNode12 = (XmlNode)obj4;
								CreatureMaxObserve.Select.SelectNode selectNode = new CreatureMaxObserve.Select.SelectNode();
								selectNode.desc = xmlNode12.Attributes.GetNamedItem("desc").InnerText;
								selectNode.isAnswer = this.GetBooleanData(xmlNode12.Attributes.GetNamedItem("isAnswer").InnerText);
								if (selectNode.isAnswer)
								{
									if (xmlNode12.Attributes.GetNamedItem("message") != null)
									{
										selectNode.message = xmlNode12.Attributes.GetNamedItem("message").InnerText;
									}
									else
									{
										selectNode.message = null;
									}
								}
								if (xmlNode12.Attributes.GetNamedItem("target") != null && xmlNode12.Attributes.GetNamedItem("target").InnerText != string.Empty)
								{
									selectNode.targetId = (int)float.Parse(xmlNode12.Attributes.GetNamedItem("target").InnerText);
								}
								else
								{
									selectNode.targetId = -1;
								}
								maxObserveModule.select.list.Add(selectNode);
								select.list.Add(selectNode);
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
						maxObserveModule.selects.Add(select);
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
			}
			string[] textFromFormatProcessText3 = TextConverter.GetTextFromFormatProcessText(xmlNode9.SelectSingleNode("angela").InnerText.Trim());
			maxObserveModule.angela.Init(textFromFormatProcessText3);
			maxObserveModule.init = true;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		IEnumerator enumerator5 = xmlNode.SelectNodes("narration").GetEnumerator();
		try
		{
			while (enumerator5.MoveNext())
			{
				object obj5 = enumerator5.Current;
				XmlNode xmlNode13 = (XmlNode)obj5;
				string innerText = xmlNode13.Attributes.GetNamedItem("action").InnerText;
				string value2 = xmlNode13.InnerText.Trim();
				dictionary.Add(innerText, value2);
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
		creatureTypeInfo.narrationTable = dictionary;
		creatureTypeInfo.MaxObserveLevel = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("level").InnerText);
		XmlNode xmlNode14 = xmlNode2.SelectSingleNode("collection");
		this.LoadCreatureCollectionInfo(xmlNode14, creatureTypeInfo);
		XmlNode xmlNode15 = xmlNode14.SelectSingleNode("openText");
		if (xmlNode15 != null)
		{
			string openText = xmlNode15.InnerText.Trim();
			creatureTypeInfo.openText = openText;
		}
		IEnumerator enumerator6 = xmlNode2.SelectNodes("desc").GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj6 = enumerator6.Current;
				XmlNode xmlNode16 = (XmlNode)obj6;
				int item = (int)float.Parse(xmlNode16.Attributes.GetNamedItem("openLevel").InnerText);
				string textFromFormatAlter = TextConverter.GetTextFromFormatAlter(xmlNode16.InnerText.Trim());
				creatureTypeInfo.desc.Add(textFromFormatAlter);
				creatureTypeInfo.observeTable.desc.Add(item);
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
		IEnumerator enumerator7 = xmlNode2.SelectNodes("record").GetEnumerator();
		try
		{
			while (enumerator7.MoveNext())
			{
				object obj7 = enumerator7.Current;
				XmlNode xmlNode17 = (XmlNode)obj7;
				int item2 = (int)float.Parse(xmlNode17.Attributes.GetNamedItem("openLevel").InnerText);
				string textFromFormatAlter2 = TextConverter.GetTextFromFormatAlter(xmlNode17.InnerText.Trim());
				creatureTypeInfo.observeRecord.Add(textFromFormatAlter2);
				creatureTypeInfo.observeTable.record.Add(item2);
			}
		}
		finally
		{
			IDisposable disposable7;
			if ((disposable7 = (enumerator7 as IDisposable)) != null)
			{
				disposable7.Dispose();
			}
		}
		if (xmlNode3 != null)
		{
			IEnumerator enumerator8 = xmlNode3.SelectNodes("param").GetEnumerator();
			try
			{
				while (enumerator8.MoveNext())
				{
					object obj8 = enumerator8.Current;
					XmlNode xmlNode18 = (XmlNode)obj8;
					CreatureStaticData.ParameterData parameterData = new CreatureStaticData.ParameterData();
					string innerText2 = xmlNode18.Attributes.GetNamedItem("key").InnerText;
					int index = (int)float.Parse(xmlNode18.Attributes.GetNamedItem("index").InnerText);
					string innerText3 = xmlNode18.InnerText;
					parameterData.desc = innerText3;
					parameterData.index = index;
					parameterData.key = innerText2;
					creatureTypeInfo.creatureStaticData.paramList.Add(parameterData);
				}
			}
			finally
			{
				IDisposable disposable8;
				if ((disposable8 = (enumerator8 as IDisposable)) != null)
				{
					disposable8.Dispose();
				}
			}
		}
		return creatureTypeInfo;
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x00137AD0 File Offset: 0x00135CD0
	public void LoadSkillTrigger(XmlNode triggerRoot, CreatureTypeInfo typeinfo)
	{
		IEnumerator enumerator = triggerRoot.SelectNodes("useSkill").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				UseSkillTrigger useSkillTrigger = new UseSkillTrigger();
				string innerText = xmlNode.Attributes.GetNamedItem("checkTime").InnerText;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("skillType");
				if (xmlNode2 != null)
				{
					long skillId = (long)float.Parse(xmlNode2.InnerText);
					int maxCount = (int)float.Parse(xmlNode2.Attributes.GetNamedItem("max").InnerText);
					useSkillTrigger.skillId = skillId;
					useSkillTrigger.maxCount = maxCount;
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("clear");
				useSkillTrigger._ClearOnActivated = this.SkillTriggerClearEvent(xmlNode3.SelectSingleNode("activated"));
				useSkillTrigger._ClearOnFalse = this.SkillTriggerClearEvent(xmlNode3.SelectSingleNode("onCheckFalse"));
				IEnumerator enumerator2 = xmlNode.SelectNodes("calledEvent").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode4 = (XmlNode)obj2;
						SkillTrigger.CalledEvent calledEvent = new SkillTrigger.CalledEvent();
						calledEvent.eventName = xmlNode4.InnerText;
						XmlNode namedItem;
						if ((namedItem = xmlNode4.Attributes.GetNamedItem("time")) != null)
						{
							calledEvent.eventTime = this.GetCreatureEventCallTime(namedItem.InnerText);
						}
						if (!int.TryParse(xmlNode4.Attributes.GetNamedItem("count").InnerText, out calledEvent.calledCount))
						{
							calledEvent.calledCount = useSkillTrigger.maxCount;
						}
						useSkillTrigger.calledEvent.Add(calledEvent);
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
				if (innerText != null && innerText == "OnEnterRoom")
				{
					typeinfo.skillTriggerCheck.onEnterRoom.Add(useSkillTrigger);
				}
				typeinfo.skillTriggerCheck.total.Add(useSkillTrigger);
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
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x00137CEC File Offset: 0x00135EEC
	public SkillTrigger.ClearEvent SkillTriggerClearEvent(XmlNode node)
	{
		SkillTrigger.ClearEvent clearEvent = new SkillTrigger.ClearEvent();
		string innerText = node.Attributes.GetNamedItem("clear").InnerText;
		bool booleanData = this.GetBooleanData(innerText);
		string innerText2 = node.InnerText;
		if (innerText2 == null || innerText2 == string.Empty || innerText2 == string.Empty)
		{
			clearEvent.hasEvent = false;
		}
		else
		{
			XmlNode namedItem = node.Attributes.GetNamedItem("time");
			if (namedItem != null)
			{
				clearEvent.eventTime = this.GetCreatureEventCallTime(namedItem.InnerText);
			}
			clearEvent.hasEvent = true;
		}
		clearEvent.clear = booleanData;
		return clearEvent;
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x00137D84 File Offset: 0x00135F84
	public CreatureEventCallTime GetCreatureEventCallTime(string time)
	{
		CreatureEventCallTime result = CreatureEventCallTime.Immediately;
		string text = time.ToLower();
		if (text != null)
		{
			if (!(text == "onroomenter"))
			{
				if (text == "onrelease")
				{
					result = CreatureEventCallTime.onRelease;
				}
			}
			else
			{
				result = CreatureEventCallTime.OnRoomEnter;
			}
		}
		return result;
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x00137DC0 File Offset: 0x00135FC0
	public string LoadCollectionStringItem(XmlNode node, ref int level)
	{
		XmlNode namedItem = node.Attributes.GetNamedItem("openLevel");
		if (namedItem == null)
		{
			Debug.Log("openLevel not found : " + node.Name);
			level = 0;
			return node.InnerText;
		}
		level = (int)float.Parse(namedItem.InnerText);
		return node.InnerText;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x00137E14 File Offset: 0x00136014
	public int LoadCollectionIntegerItem(XmlNode node, ref int level)
	{
		XmlNode namedItem = node.Attributes.GetNamedItem("openLevel");
		if (namedItem == null)
		{
			Debug.LogError("openLevel not found : " + node.Name);
			return 0;
		}
		level = (int)float.Parse(namedItem.InnerText);
		return (int)float.Parse(node.InnerText);
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x00137E68 File Offset: 0x00136068
	public CreatureTypeInfo.CreatureDataList GetCreatureDataList(XmlNodeList nodes, string itemName, bool isInt)
	{
		CreatureTypeInfo.CreatureDataList creatureDataList = new CreatureTypeInfo.CreatureDataList();
		creatureDataList.itemName = itemName;
		IEnumerator enumerator = nodes.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode node = (XmlNode)obj;
				CreatureTypeInfo.CreatureData creatureData = new CreatureTypeInfo.CreatureData();
				int openLevel = -1;
				if (isInt)
				{
					int num = this.LoadCollectionIntegerItem(node, ref openLevel);
					creatureData.data = num;
				}
				else
				{
					string data = this.LoadCollectionStringItem(node, ref openLevel).Trim();
					creatureData.data = data;
				}
				creatureData.openLevel = openLevel;
				creatureDataList.AddData(creatureData);
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
		return creatureDataList;
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x00137F14 File Offset: 0x00136114
	public void LoadCreatureCollectionInfo(XmlNode collection, CreatureTypeInfo model)
	{
		CreatureTypeInfo.CreatureDataTable creatureDataTable = new CreatureTypeInfo.CreatureDataTable();
		foreach (string text in CreatureTypeInfo.stringData)
		{
			XmlNodeList nodes = collection.SelectNodes(text);
			CreatureTypeInfo.CreatureDataList creatureDataList = this.GetCreatureDataList(nodes, text, false);
			creatureDataTable.dictionary.Add(text, creatureDataList);
		}
		model.dataTable = creatureDataTable;
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x0002C025 File Offset: 0x0002A225
	public string LoadCollectionItem(XmlNode collection, string target, ref int level)
	{
		level = (int)float.Parse(collection.SelectSingleNode(target).Attributes.GetNamedItem("openLevel").InnerText);
		return collection.SelectSingleNode(target).InnerText;
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x00137F6C File Offset: 0x0013616C
	public static DamageInfo ConvertToDamageInfo(XmlNode damageNode)
	{
		RwbpType type = CreatureDataLoader.ConvertToRWBP(damageNode.Attributes.GetNamedItem("type").InnerText);
		int min = int.Parse(damageNode.Attributes.GetNamedItem("min").InnerText);
		int max = int.Parse(damageNode.Attributes.GetNamedItem("max").InnerText);
		return new DamageInfo(type, min, max);
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x00137FD0 File Offset: 0x001361D0
	public static RwbpType ConvertToRWBP(string text)
	{
		if (text != null)
		{
			if (text == "R")
			{
				return RwbpType.R;
			}
			if (text == "W")
			{
				return RwbpType.W;
			}
			if (text == "B")
			{
				return RwbpType.B;
			}
			if (text == "P")
			{
				return RwbpType.P;
			}
		}
		return RwbpType.N;
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x0002C056 File Offset: 0x0002A256
	public bool GetBooleanData(string b)
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
		b = "false";
		return false;
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x00138020 File Offset: 0x00136220
	public XmlDocument LoadDoc(string src, string currentLn, bool IsMod = false)
	{
		XmlDocument xmlDocument = new XmlDocument();
		if (!IsMod)
		{
			xmlDocument = AssetLoader.LoadExternalXML(string.Format(this.documentSrcFormat, currentLn, src));
			if (xmlDocument == null)
			{
				xmlDocument = AssetLoader.LoadExternalXML(string.Format(this.documentSrcFormat, "en", src));
			}
			return xmlDocument;
		}
		foreach (DirectoryInfo dir in Add_On.instance.DirList)
		{
			DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Creature");
			if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/CreatureInfo/" + currentLn))
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/CreatureInfo/" + currentLn);
				if (directoryInfo2.GetFiles().Length != 0)
				{
					foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
					{
						if (fileInfo.Name == src + ".xml" || fileInfo.Name == src + ".txt")
						{
							string xml = File.ReadAllText(fileInfo.FullName);
							xmlDocument.LoadXml(xml);
							return xmlDocument;
						}
					}
				}
			}
		}
		return xmlDocument;
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x0013816C File Offset: 0x0013636C
	public void Loading(XmlNodeList xmlNodeList, List<CreatureTypeInfo> list, List<CreatureSpecialSkillTipTable> list2, Dictionary<long, int> specialTipSize, bool isMod = false)
	{
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("src").InnerText;
				string xml = "";
				if (!isMod)
				{
					if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseCreatures/" + xmlNode.SelectSingleNode("stat").InnerText + ".txt"))
					{
						TextAsset textAsset = Resources.Load<TextAsset>("xml/creatureStats/" + xmlNode.SelectSingleNode("stat").InnerText);
						File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/" + xmlNode.SelectSingleNode("stat").InnerText + ".txt", textAsset.text);
					}
					xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/" + xmlNode.SelectSingleNode("stat").InnerText + ".txt");
				}
				else
				{
					foreach (DirectoryInfo dir in Add_On.instance.DirList)
					{
						bool flag = false;
						DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Creature");
						if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/Creatures"))
						{
							DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Creatures");
							if (directoryInfo2.GetFiles().Length != 0)
							{
								foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
								{
									if (fileInfo.Name == xmlNode.SelectSingleNode("stat").InnerText + ".txt" || fileInfo.Name == xmlNode.SelectSingleNode("stat").InnerText + ".xml")
									{
										xml = File.ReadAllText(fileInfo.FullName);
										flag = true;
										break;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				XmlDocument xmlDocument = new XmlDocument();
				XmlDocument doc = this.LoadDoc(innerText, this.currentLn, isMod);
				xmlDocument.LoadXml(xml);
				ChildCreatureData data = null;
				CreatureTypeInfo creatureTypeInfo = this.LoadCreatureTypeInfo(doc, ref list2, ref specialTipSize, out data);
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("creature");
				XmlNode stat = xmlDocument.SelectSingleNode("creature/stat");
				XmlNode xmlNode3 = xmlNode2.SelectSingleNode("child");
				this.LoadCreatureStat(stat, xmlNode2, creatureTypeInfo);
				if (xmlNode3 != null)
				{
					string innerText2 = xmlNode3.InnerText;
					string text = "";
					foreach (DirectoryInfo directoryInfo3 in Add_On.instance.DirList)
					{
						if (Directory.Exists(directoryInfo3.FullName + "/Creature/Creatures"))
						{
							foreach (FileInfo fileInfo2 in new DirectoryInfo(directoryInfo3.FullName + "/Creature/Creatures").GetFiles())
							{
								if (fileInfo2.Name == innerText2 + ".txt" || fileInfo2.Name == innerText2 + ".xml")
								{
									text = File.ReadAllText(fileInfo2.FullName);
									break;
								}
							}
							if (text != "")
							{
								break;
							}
						}
					}
					if (text == "")
					{
						if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt"))
						{
							text = Resources.Load<TextAsset>("xml/creatureStats/" + innerText2).text;
							File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt", text);
						}
						text = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt");
					}
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(text);
					ChildCreatureTypeInfo childCreatureTypeInfo = new ChildCreatureTypeInfo();
					XmlNode xmlNode4 = xmlDocument2.SelectSingleNode("creature");
					childCreatureTypeInfo.maxHp = (int)float.Parse(xmlNode4.SelectSingleNode("stat/hp").InnerText);
					childCreatureTypeInfo.speed = float.Parse(xmlNode4.SelectSingleNode("stat/speed").InnerText);
					XmlNode xmlNode5 = xmlNode4.SelectSingleNode("anim");
					if (xmlNode5 != null)
					{
						childCreatureTypeInfo.animSrc = xmlNode5.Attributes.GetNamedItem("prefab").InnerText;
					}
					XmlNode xmlNode6 = xmlNode4.SelectSingleNode("riskLevel");
					if (xmlNode6 != null)
					{
						int riskLevelOpen = (int)float.Parse(xmlNode6.Attributes.GetNamedItem("openLevel").InnerText);
						string innerText3 = xmlNode6.InnerText;
						childCreatureTypeInfo.riskLevelOpen = riskLevelOpen;
						childCreatureTypeInfo._riskLevel = innerText3;
					}
					XmlNode xmlNode7 = xmlNode4.SelectSingleNode("attackType");
					if (xmlNode7 != null)
					{
						int attackTypeOpen = (int)float.Parse(xmlNode7.Attributes.GetNamedItem("openLevel").InnerText);
						string innerText4 = xmlNode7.InnerText;
						childCreatureTypeInfo.attackTypeOpen = attackTypeOpen;
						childCreatureTypeInfo._attackType = innerText4;
					}
					Dictionary<string, DefenseInfo> dictionary = new Dictionary<string, DefenseInfo>();
					IEnumerator enumerator3 = xmlNode4.SelectNodes("stat/defense").GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj2 = enumerator3.Current;
							XmlNode xmlNode8 = (XmlNode)obj2;
							string innerText5 = xmlNode8.Attributes.GetNamedItem("id").InnerText;
							DefenseInfo defenseInfo = new DefenseInfo();
							IEnumerator enumerator4 = xmlNode8.SelectNodes("defenseElement").GetEnumerator();
							try
							{
								while (enumerator4.MoveNext())
								{
									object obj3 = enumerator4.Current;
									XmlNode xmlNode9 = (XmlNode)obj3;
									string innerText6 = xmlNode9.Attributes.GetNamedItem("type").InnerText;
									if (innerText6 != null)
									{
										if (!(innerText6 == "R"))
										{
											if (!(innerText6 == "W"))
											{
												if (!(innerText6 == "B"))
												{
													if (innerText6 == "P")
													{
														defenseInfo.P = float.Parse(xmlNode9.InnerText);
													}
												}
												else
												{
													defenseInfo.B = float.Parse(xmlNode9.InnerText);
												}
											}
											else
											{
												defenseInfo.W = float.Parse(xmlNode9.InnerText);
											}
										}
										else
										{
											defenseInfo.R = float.Parse(xmlNode9.InnerText);
										}
									}
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
							dictionary.Add(innerText5, defenseInfo);
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
					childCreatureTypeInfo.defenseTable.Init(dictionary);
					XmlNode xmlNode10 = xmlNode4.SelectSingleNode("script");
					if (xmlNode10 != null)
					{
						childCreatureTypeInfo.script = xmlNode10.InnerText;
					}
					XmlNode xmlNode11 = xmlNode4.SelectSingleNode("portrait");
					if (xmlNode11 != null)
					{
						childCreatureTypeInfo._tempPortrait = xmlNode11.InnerText.Trim();
						childCreatureTypeInfo._isChildAndHasData = true;
					}
					XmlNode xmlNode12 = xmlNode4.SelectSingleNode("metaInfo");
					if (xmlNode12 != null)
					{
						string innerText7 = xmlNode12.InnerText;
						CreatureTypeInfo creatureTypeInfo2 = this.LoadChildMeta(innerText7, ref list2, ref specialTipSize, isMod);
						XmlNode statCreature = xmlNode4;
						XmlNode stat2 = xmlNode4.SelectSingleNode("stat");
						this.LoadCreatureStat(stat2, statCreature, creatureTypeInfo2);
						list.Add(creatureTypeInfo2);
						childCreatureTypeInfo.id = creatureTypeInfo2.id;
						childCreatureTypeInfo.isHasBaseMeta = true;
					}
					XmlNodeList xmlNodeList2 = xmlNode4.SelectNodes("sound");
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					IEnumerator enumerator5 = xmlNodeList2.GetEnumerator();
					try
					{
						while (enumerator5.MoveNext())
						{
							object obj4 = enumerator5.Current;
							XmlNode xmlNode13 = (XmlNode)obj4;
							string innerText8 = xmlNode13.Attributes.GetNamedItem("action").InnerText;
							string innerText9 = xmlNode13.Attributes.GetNamedItem("src").InnerText;
							dictionary2.Add(innerText8, innerText9);
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
					childCreatureTypeInfo.soundTable = dictionary2;
					creatureTypeInfo.childTypeInfo = childCreatureTypeInfo;
					creatureTypeInfo.childTypeInfo.data = data;
				}
				list.Add(creatureTypeInfo);
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
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x001389F4 File Offset: 0x00136BF4
	public CreatureTypeInfo LoadChildMeta(string src, ref List<CreatureSpecialSkillTipTable> creatureSpecialSkillTipList, ref Dictionary<long, int> specialTipSizeLib, bool isMod)
	{
		XmlDocument doc = this.LoadDoc(src, this.currentLn, isMod);
		ChildCreatureData childCreatureData = null;
		return this.LoadCreatureTypeInfo(doc, ref creatureSpecialSkillTipList, ref specialTipSizeLib, out childCreatureData);
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x00138A20 File Offset: 0x00136C20
	public void ReLoad()
	{
		if (!EquipmentTypeList.instance.loaded)
		{
			Debug.LogError("LoadCreatureList >> EquipmentTypeList must be loaded. ");
		}
		this.currentLn = GlobalGameManager.instance.GetCurrentLanguage();
		string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseList.txt");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/creature_list/creature");
		List<CreatureTypeInfo> list = new List<CreatureTypeInfo>();
		List<CreatureSpecialSkillTipTable> list2 = new List<CreatureSpecialSkillTipTable>();
		Dictionary<long, int> dictionary = new Dictionary<long, int>();
		this.Loading(xmlNodeList, list, list2, dictionary, false);
		foreach (DirectoryInfo dir in Add_On.instance.DirList)
		{
			DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Creature");
			if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/CreatureList"))
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/CreatureList");
				if (directoryInfo2.GetFiles().Length != 0)
				{
					foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
					{
						if (fileInfo.Name.Contains(".txt"))
						{
							XmlDocument xmlDocument2 = new XmlDocument();
							xmlDocument2.LoadXml(File.ReadAllText(fileInfo.FullName));
							XmlNodeList xmlNodeList2 = xmlDocument2.SelectNodes("/creature_list/creature");
							List<CreatureTypeInfo> list3 = new List<CreatureTypeInfo>();
							List<CreatureSpecialSkillTipTable> list4 = new List<CreatureSpecialSkillTipTable>();
							Dictionary<long, int> dictionary2 = new Dictionary<long, int>();
							this.Loading(xmlNodeList2, list3, list4, dictionary2, true);
							foreach (KeyValuePair<long, int> keyValuePair in dictionary2)
							{
								if (dictionary.ContainsKey(keyValuePair.Key))
								{
									for (int j = 0; j < list.Count; j++)
									{
										dictionary.Remove(keyValuePair.Key);
										dictionary.Add(keyValuePair.Key, keyValuePair.Value);
									}
								}
							}
							foreach (CreatureTypeInfo creatureTypeInfo in list3)
							{
								foreach (CreatureTypeInfo creatureTypeInfo2 in list)
								{
									if (creatureTypeInfo.id == creatureTypeInfo2.id)
									{
										list.Remove(creatureTypeInfo2);
										break;
									}
								}
								list.Add(creatureTypeInfo);
							}
							foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable in list4)
							{
								foreach (CreatureSpecialSkillTipTable creatureSpecialSkillTipTable2 in list2)
								{
									if (creatureSpecialSkillTipTable2.creatureTypeId == creatureSpecialSkillTipTable.creatureTypeId)
									{
										list2.Remove(creatureSpecialSkillTipTable2);
										break;
									}
								}
								list2.Add(creatureSpecialSkillTipTable);
							}
						}
					}
				}
			}
		}
		CreatureTypeList.instance.Init(list.ToArray(), list2.ToArray(), dictionary);
	}

	// <Patch>
	private void LoadCreatureStat_Mod(XmlNode stat, XmlNode statCreature, CreatureTypeInfo model, string modid)
	{ // <Mod>
		XmlNode xmlNode;
		bool flag = (xmlNode = statCreature.SelectSingleNode("script")) != null;
		if (flag)
		{
			model.script = xmlNode.InnerText;
		}
		XmlNode xmlNode32;
		if ((xmlNode32 = stat.SelectSingleNode("riskLevel")) != null)
		{
			string innerText = xmlNode32.InnerText;
			model._riskLevel = int.Parse(innerText);
		}
		XmlNode xmlNode2;
		bool flag2 = (xmlNode2 = statCreature.SelectSingleNode("workAnim")) != null;
		if (flag2)
		{
			model.workAnim = xmlNode2.InnerText;
			XmlNode namedItem = xmlNode2.Attributes.GetNamedItem("face");
			bool flag3 = namedItem != null;
			if (flag3)
			{
				model.workAnimFace = namedItem.InnerText;
			}
		}
		XmlNode xmlNode3;
		bool flag4 = (xmlNode3 = statCreature.SelectSingleNode("kitIcon")) != null;
		if (flag4)
		{
			model.kitIconSrc = xmlNode3.InnerText;
		}
		XmlNode xmlNode4;
		bool flag5 = (xmlNode4 = stat.SelectSingleNode("workType")) != null;
		if (flag5)
		{
			string innerText = xmlNode4.InnerText;
			bool flag6 = innerText != null;
			if (flag6)
			{
				bool flag7 = !(innerText == "normal");
				if (flag7)
				{
					bool flag8 = innerText == "kit";
					if (flag8)
					{
						model.creatureWorkType = CreatureWorkType.KIT;
					}
				}
				else
				{
					model.creatureWorkType = CreatureWorkType.NORMAL;
				}
			}
		}
		XmlNode xmlNode5;
		bool flag9 = (xmlNode5 = stat.SelectSingleNode("kitType")) != null;
		if (flag9)
		{
			string innerText2 = xmlNode5.InnerText;
			bool flag10 = innerText2 != null;
			if (flag10)
			{
				bool flag11 = !(innerText2 == "equip");
				if (flag11)
				{
					bool flag12 = !(innerText2 == "channel");
					if (flag12)
					{
						bool flag13 = innerText2 == "oneshot";
						if (flag13)
						{
							model.creatureKitType = CreatureKitType.ONESHOT;
						}
					}
					else
					{
						model.creatureKitType = CreatureKitType.CHANNEL;
					}
				}
				else
				{
					model.creatureKitType = CreatureKitType.EQUIP;
				}
			}
		}
		XmlNode xmlNode6;
		bool flag14 = (xmlNode6 = stat.SelectSingleNode("qliphoth")) != null;
		if (flag14)
		{
			model.qliphothMax = int.Parse(xmlNode6.InnerText);
		}
		XmlNode xmlNode7;
		bool flag15 = (xmlNode7 = stat.SelectSingleNode("speed")) != null;
		if (flag15)
		{
			model.speed = float.Parse(xmlNode7.InnerText);
		}
		XmlNode xmlNode8 = stat.SelectSingleNode("escapeable");
		bool flag16 = xmlNode8 != null;
		if (flag16)
		{
			bool booleanData = GetBooleanData(xmlNode8.InnerText.Trim());
			model.isEscapeAble = booleanData;
		}
		else
		{
			model.isEscapeAble = true;
		}
		XmlNode xmlNode9 = stat.SelectSingleNode("hp");
		bool flag17 = xmlNode9 != null;
		if (flag17)
		{
			model.maxHp = (int)float.Parse(xmlNode9.InnerText);
		}
		else
		{
			model.maxHp = 5;
		}
		IEnumerator enumerator = stat.SelectNodes("workProb").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode10 = (XmlNode)obj;
				RwbpType type = CreatureDataLoader.ConvertToRWBP(xmlNode10.Attributes.GetNamedItem("type").InnerText);
				IEnumerator enumerator2 = xmlNode10.SelectNodes("prob").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode11 = (XmlNode)obj2;
						int level = int.Parse(xmlNode11.Attributes.GetNamedItem("level").InnerText);
						float prob = float.Parse(xmlNode11.InnerText);
						model.workProbTable.SetWorkProb(type, level, prob);
					}
				}
				finally
				{
					IDisposable disposable;
					bool flag18 = (disposable = (enumerator2 as IDisposable)) != null;
					if (flag18)
					{
						disposable.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			bool flag19 = (disposable2 = (enumerator as IDisposable)) != null;
			if (flag19)
			{
				disposable2.Dispose();
			}
		}
		XmlNode xmlNode12 = stat.SelectSingleNode("workCooltime");
		bool flag20 = xmlNode12 != null;
		if (flag20)
		{
			model.workCooltime = int.Parse(xmlNode12.InnerText);
		}
		XmlNode xmlNode13 = stat.SelectSingleNode("workSpeed");
		bool flag21 = xmlNode13 != null;
		if (flag21)
		{
			model.cubeSpeed = float.Parse(xmlNode13.InnerText);
		}
		XmlNode xmlNode14 = statCreature.SelectSingleNode("skillTrigger");
		bool flag22 = xmlNode14 != null;
		if (flag22)
		{
			LoadSkillTrigger(xmlNode14, model);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		IEnumerator enumerator3 = statCreature.SelectNodes("sound").GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				XmlNode xmlNode15 = (XmlNode)obj3;
				string innerText3 = xmlNode15.Attributes.GetNamedItem("action").InnerText;
				string innerText4 = xmlNode15.Attributes.GetNamedItem("src").InnerText;
				dictionary.Add(innerText3, innerText4);
			}
		}
		finally
		{
			IDisposable disposable3;
			bool flag23 = (disposable3 = (enumerator3 as IDisposable)) != null;
			if (flag23)
			{
				disposable3.Dispose();
			}
		}
		model.soundTable = dictionary;
		model.nodeInfo = statCreature.SelectNodes("graph/node");
		model.edgeInfo = statCreature.SelectNodes("graph/edge");
		XmlNode xmlNode16 = statCreature.SelectSingleNode("anim");
		bool flag24 = xmlNode16 != null;
		if (flag24)
		{
			model.animSrc = xmlNode16.Attributes.GetNamedItem("prefab").InnerText;
		}
		XmlNode xmlNode17 = statCreature.SelectSingleNode("returnImg");
		bool flag25 = xmlNode17 != null;
		if (flag25)
		{
			model.roomReturnSrc = xmlNode17.Attributes.GetNamedItem("src").InnerText;
		}
		else
		{
			model.roomReturnSrc = string.Empty;
		}
		XmlNode xmlNode18 = stat.SelectSingleNode("feelingStateCubeBounds");
		bool flag26 = xmlNode18 != null;
		if (flag26)
		{
			List<int> list = new List<int>();
			IEnumerator enumerator4 = xmlNode18.SelectNodes("cube").GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					XmlNode xmlNode19 = (XmlNode)obj4;
					list.Add(int.Parse(xmlNode19.InnerText));
				}
			}
			finally
			{
				IDisposable disposable4;
				bool flag27 = (disposable4 = (enumerator4 as IDisposable)) != null;
				if (flag27)
				{
					disposable4.Dispose();
				}
			}
			model.feelingStateCubeBounds.upperBounds = list.ToArray();
		}
		XmlNode xmlNode20 = stat.SelectSingleNode("workDamage");
		bool flag28 = xmlNode20 != null;
		if (flag28)
		{
			model.workDamage = CreatureDataLoader.ConvertToDamageInfo(xmlNode20);
		}
		XmlNode xmlNode21 = stat.SelectSingleNode("specialDamage");
		bool flag29 = xmlNode21 != null;
		if (flag29)
		{
			Dictionary<string, EquipmentTypeInfo> dictionary2 = new Dictionary<string, EquipmentTypeInfo>();
			IEnumerator enumerator5 = xmlNode21.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					object obj5 = enumerator5.Current;
					XmlNode xmlNode22 = (XmlNode)obj5;
					bool flag30 = xmlNode22.Name == "damage";
					if (flag30)
					{
						string innerText5 = xmlNode22.Attributes.GetNamedItem("id").InnerText;
						EquipmentTypeInfo value = EquipmentTypeInfo.MakeWeaponInfoByDamageInfo(CreatureDataLoader.ConvertToDamageInfo(xmlNode22));
						dictionary2.Add(innerText5, value);
					}
					else
					{
						bool flag31 = xmlNode22.Name == "weapon";
						if (flag31)
						{
							string innerText6 = xmlNode22.Attributes.GetNamedItem("id").InnerText;
							string innerText7 = xmlNode22.Attributes.GetNamedItem("weaponId").InnerText;
							EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(int.Parse(innerText7));
							dictionary2.Add(innerText6, data);
						}
					}
				}
			}
			finally
			{
				IDisposable disposable5;
				bool flag32 = (disposable5 = (enumerator5 as IDisposable)) != null;
				if (flag32)
				{
					disposable5.Dispose();
				}
			}
			model.creatureSpecialDamageTable.Init(dictionary2);
		}
		Dictionary<string, DefenseInfo> dictionary3 = new Dictionary<string, DefenseInfo>();
		IEnumerator enumerator6 = stat.SelectNodes("defense").GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj6 = enumerator6.Current;
				XmlNode xmlNode23 = (XmlNode)obj6;
				string innerText8 = xmlNode23.Attributes.GetNamedItem("id").InnerText;
				DefenseInfo defenseInfo = new DefenseInfo();
				IEnumerator enumerator7 = xmlNode23.SelectNodes("defenseElement").GetEnumerator();
				try
				{
					while (enumerator7.MoveNext())
					{
						object obj7 = enumerator7.Current;
						XmlNode xmlNode24 = (XmlNode)obj7;
						string innerText9 = xmlNode24.Attributes.GetNamedItem("type").InnerText;
						bool flag33 = innerText9 != null;
						if (flag33)
						{
							bool flag34 = !(innerText9 == "R");
							if (flag34)
							{
								bool flag35 = !(innerText9 == "W");
								if (flag35)
								{
									bool flag36 = !(innerText9 == "B");
									if (flag36)
									{
										bool flag37 = innerText9 == "P";
										if (flag37)
										{
											defenseInfo.P = float.Parse(xmlNode24.InnerText);
										}
									}
									else
									{
										defenseInfo.B = float.Parse(xmlNode24.InnerText);
									}
								}
								else
								{
									defenseInfo.W = float.Parse(xmlNode24.InnerText);
								}
							}
							else
							{
								defenseInfo.R = float.Parse(xmlNode24.InnerText);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable6;
					bool flag38 = (disposable6 = (enumerator7 as IDisposable)) != null;
					if (flag38)
					{
						disposable6.Dispose();
					}
				}
				dictionary3.Add(innerText8, defenseInfo);
			}
		}
		finally
		{
			IDisposable disposable7;
			bool flag39 = (disposable7 = (enumerator6 as IDisposable)) != null;
			if (flag39)
			{
				disposable7.Dispose();
			}
		}
		model.defenseTable.Init(dictionary3);
		XmlNode xmlNode25 = stat.SelectSingleNode("observeInfo");
		bool flag40 = xmlNode25 != null;
		if (flag40)
		{
			List<ObserveInfoData> list2 = new List<ObserveInfoData>();
			IEnumerator enumerator8 = xmlNode25.SelectNodes("observeElement").GetEnumerator();
			try
			{
				while (enumerator8.MoveNext())
				{
					object obj8 = enumerator8.Current;
					XmlNode xmlNode26 = (XmlNode)obj8;
					string regionName = xmlNode26.Attributes.GetNamedItem("name").InnerText.Trim();
					int observeCost = (int)float.Parse(xmlNode26.Attributes.GetNamedItem("cost").InnerText);
					ObserveInfoData item = new ObserveInfoData
					{
						observeCost = observeCost,
						regionName = regionName
					};
					list2.Add(item);
				}
			}
			finally
			{
				IDisposable disposable8;
				bool flag41 = (disposable8 = (enumerator8 as IDisposable)) != null;
				if (flag41)
				{
					disposable8.Dispose();
				}
			}
			model.observeData = list2;
		}
		else
		{
			List<ObserveInfoData> list3 = new List<ObserveInfoData>();
			for (int i = 0; i < CreatureModel.regionName.Length; i++)
			{
				ObserveInfoData item2 = new ObserveInfoData
				{
					observeCost = 0,
					regionName = CreatureModel.regionName[i]
				};
				list3.Add(item2);
			}
			for (int j = 0; j < CreatureModel.careTakingRegion.Length; j++)
			{
				ObserveInfoData item3 = new ObserveInfoData
				{
					observeCost = 0,
					regionName = CreatureModel.careTakingRegion[j]
				};
				list3.Add(item3);
			}
			model.observeData = list3;
		}
		List<CreatureEquipmentMakeInfo> list4 = new List<CreatureEquipmentMakeInfo>();
		IEnumerator enumerator9 = stat.SelectNodes("equipment").GetEnumerator();
		try
		{
			while (enumerator9.MoveNext())
			{
				object obj9 = enumerator9.Current;
				XmlNode xmlNode27 = (XmlNode)obj9;
				XmlNode namedItem2 = xmlNode27.Attributes.GetNamedItem("equipId");
				XmlNode namedItem3 = xmlNode27.Attributes.GetNamedItem("level");
				XmlNode namedItem4 = xmlNode27.Attributes.GetNamedItem("cost");
				XmlNode namedItem5 = xmlNode27.Attributes.GetNamedItem("prob");
				CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = new CreatureEquipmentMakeInfo();
				bool flag42 = namedItem2 != null;
				if (flag42)
				{
					int id = int.Parse(namedItem2.InnerText);
					LobotomyBaseMod.LcId id2 = new LobotomyBaseMod.LcId(modid, id);
					creatureEquipmentMakeInfo.equipTypeInfo = EquipmentTypeList.instance.GetData_Mod(id2);
					bool flag43 = creatureEquipmentMakeInfo.equipTypeInfo == null;
					if (flag43)
					{
						continue;
					}
				}
				bool flag44 = namedItem3 != null;
				if (flag44)
				{
					creatureEquipmentMakeInfo.level = int.Parse(namedItem3.InnerText);
				}
				bool flag45 = namedItem4 != null;
				if (flag45)
				{
					creatureEquipmentMakeInfo.cost = int.Parse(namedItem4.InnerText);
				}
				bool flag46 = namedItem5 != null;
				if (flag46)
				{
					creatureEquipmentMakeInfo.prob = float.Parse(namedItem5.InnerText);
				}
				list4.Add(creatureEquipmentMakeInfo);
				creatureEquipmentMakeInfo.creature = model; // 
			}
		}
		finally
		{
			IDisposable disposable9;
			bool flag47 = (disposable9 = (enumerator9 as IDisposable)) != null;
			if (flag47)
			{
				disposable9.Dispose();
			}
		}
		model.equipMakeInfos = list4;
		List<CreatureObserveBonusData> list5 = new List<CreatureObserveBonusData>();
		IEnumerator enumerator10 = stat.SelectNodes("observeBonus").GetEnumerator();
		try
		{
			while (enumerator10.MoveNext())
			{
				object obj10 = enumerator10.Current;
				XmlNode xmlNode28 = (XmlNode)obj10;
				int level2 = int.Parse(xmlNode28.Attributes.GetNamedItem("level").InnerText);
				string innerText10 = xmlNode28.Attributes.GetNamedItem("type").InnerText;
				CreatureObserveBonusData creatureObserveBonusData = new CreatureObserveBonusData();
				bool flag48 = innerText10 != null;
				if (flag48)
				{
					bool flag49 = !(innerText10 == "prob");
					if (flag49)
					{
						bool flag50 = innerText10 == "speed";
						if (flag50)
						{
							creatureObserveBonusData.bonus = CreatureObserveBonusData.BonusType.SPEED;
						}
					}
					else
					{
						creatureObserveBonusData.bonus = CreatureObserveBonusData.BonusType.PROB;
					}
				}
				creatureObserveBonusData.level = level2;
				creatureObserveBonusData.value = int.Parse(xmlNode28.InnerText);
				list5.Add(creatureObserveBonusData);
			}
		}
		finally
		{
			IDisposable disposable10;
			bool flag51 = (disposable10 = (enumerator10 as IDisposable)) != null;
			if (flag51)
			{
				disposable10.Dispose();
			}
		}
		model.observeBonus.Init(list5);
		XmlNode xmlNode29 = stat.SelectSingleNode("maxWorkCount");
		bool flag52 = xmlNode29 != null;
		if (flag52)
		{
			model.maxWorkCount = int.Parse(xmlNode29.InnerText);
		}
		XmlNode xmlNode30 = stat.SelectSingleNode("maxProbReductionCounter");
		bool flag53 = xmlNode30 != null;
		if (flag53)
		{
			model.maxProbReductionCounter = int.Parse(xmlNode30.InnerText);
		}
		XmlNode xmlNode31 = stat.SelectSingleNode("probReduction");
		bool flag54 = xmlNode31 != null;
		if (flag54)
		{
			model.probReduction = float.Parse(xmlNode31.InnerText);
		}
	}

	// <Patch>
	public void Loading_Mod(XmlNodeList xmlNodeList, List<CreatureTypeInfo> list, List<CreatureSpecialSkillTipTable> list2, Dictionary<long, int> specialTipSize, string modid)
	{
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes.GetNamedItem("src").InnerText;
				string xml = "";
				foreach (DirectoryInfo dir in Add_On.instance.DirList)
				{
					bool flag = false;
					DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Creature");
					bool flag2 = directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/Creatures");
					if (flag2)
					{
						DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Creatures");
						bool flag3 = directoryInfo2.GetFiles().Length != 0;
						if (flag3)
						{
							FileInfo[] files = directoryInfo2.GetFiles();
							foreach (FileInfo fileInfo in files)
							{
								bool flag4 = fileInfo.Name == xmlNode.SelectSingleNode("stat").InnerText + ".txt" || fileInfo.Name == xmlNode.SelectSingleNode("stat").InnerText + ".xml";
								if (flag4)
								{
									xml = File.ReadAllText(fileInfo.FullName);
									flag = true;
									break;
								}
							}
						}
					}
					bool flag5 = flag;
					if (flag5)
					{
						break;
					}
				}
				XmlDocument xmlDocument = new XmlDocument();
				XmlDocument doc = LoadDoc(innerText, this.currentLn, true);
				xmlDocument.LoadXml(xml);
				ChildCreatureData data = null;
				CreatureTypeInfo creatureTypeInfo = LoadCreatureTypeInfo(doc, ref list2, ref specialTipSize, out data);
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("creature");
				XmlNode stat = xmlDocument.SelectSingleNode("creature/stat");
				XmlNode xmlNode3 = xmlNode2.SelectSingleNode("child");
				this.LoadCreatureStat_Mod(stat, xmlNode2, creatureTypeInfo, modid);
				bool flag6 = xmlNode3 != null;
				if (flag6)
				{
					string innerText2 = xmlNode3.InnerText;
					string text = "";
					foreach (DirectoryInfo directoryInfo3 in Add_On.instance.DirList)
					{
						bool flag7 = !Directory.Exists(directoryInfo3.FullName + "/Creature/Creatures");
						if (!flag7)
						{
							FileInfo[] files2 = new DirectoryInfo(directoryInfo3.FullName + "/Creature/Creatures").GetFiles();
							foreach (FileInfo fileInfo2 in files2)
							{
								bool flag8 = fileInfo2.Name == innerText2 + ".txt" || fileInfo2.Name == innerText2 + ".xml";
								if (flag8)
								{
									text = File.ReadAllText(fileInfo2.FullName);
									break;
								}
							}
							bool flag9 = text != "";
							if (flag9)
							{
								break;
							}
						}
					}
					bool flag10 = text == "";
					if (flag10)
					{
						bool flag11 = !File.Exists(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt");
						if (flag11)
						{
							text = Resources.Load<TextAsset>("xml/creatureStats/" + innerText2).text;
							File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt", text);
						}
						text = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseCreatures/ChildCreatures/" + innerText2 + ".txt");
					}
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(text);
					ChildCreatureTypeInfo childCreatureTypeInfo = new ChildCreatureTypeInfo();
					XmlNode xmlNode4 = xmlDocument2.SelectSingleNode("creature");
					childCreatureTypeInfo.maxHp = (int)float.Parse(xmlNode4.SelectSingleNode("stat/hp").InnerText);
					childCreatureTypeInfo.speed = float.Parse(xmlNode4.SelectSingleNode("stat/speed").InnerText);
					XmlNode xmlNode5 = xmlNode4.SelectSingleNode("anim");
					bool flag12 = xmlNode5 != null;
					if (flag12)
					{
						childCreatureTypeInfo.animSrc = xmlNode5.Attributes.GetNamedItem("prefab").InnerText;
					}
					XmlNode xmlNode6 = xmlNode4.SelectSingleNode("riskLevel");
					bool flag13 = xmlNode6 != null;
					if (flag13)
					{
						int riskLevelOpen = (int)float.Parse(xmlNode6.Attributes.GetNamedItem("openLevel").InnerText);
						string innerText3 = xmlNode6.InnerText;
						childCreatureTypeInfo.riskLevelOpen = riskLevelOpen;
						childCreatureTypeInfo._riskLevel = innerText3;
					}
					XmlNode xmlNode7 = xmlNode4.SelectSingleNode("attackType");
					bool flag14 = xmlNode7 != null;
					if (flag14)
					{
						int attackTypeOpen = (int)float.Parse(xmlNode7.Attributes.GetNamedItem("openLevel").InnerText);
						string innerText4 = xmlNode7.InnerText;
						childCreatureTypeInfo.attackTypeOpen = attackTypeOpen;
						childCreatureTypeInfo._attackType = innerText4;
					}
					Dictionary<string, DefenseInfo> dictionary = new Dictionary<string, DefenseInfo>();
					IEnumerator enumerator4 = xmlNode4.SelectNodes("stat/defense").GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							object obj2 = enumerator4.Current;
							XmlNode xmlNode8 = (XmlNode)obj2;
							string innerText5 = xmlNode8.Attributes.GetNamedItem("id").InnerText;
							DefenseInfo defenseInfo = new DefenseInfo();
							IEnumerator enumerator5 = xmlNode8.SelectNodes("defenseElement").GetEnumerator();
							try
							{
								while (enumerator5.MoveNext())
								{
									object obj3 = enumerator5.Current;
									XmlNode xmlNode9 = (XmlNode)obj3;
									string innerText6 = xmlNode9.Attributes.GetNamedItem("type").InnerText;
									string a = innerText6;
									if (!(a == "P"))
									{
										if (!(a == "B"))
										{
											if (!(a == "W"))
											{
												if (a == "R")
												{
													defenseInfo.R = float.Parse(xmlNode9.InnerText);
												}
											}
											else
											{
												defenseInfo.W = float.Parse(xmlNode9.InnerText);
											}
										}
										else
										{
											defenseInfo.B = float.Parse(xmlNode9.InnerText);
										}
									}
									else
									{
										defenseInfo.P = float.Parse(xmlNode9.InnerText);
									}
								}
							}
							finally
							{
								IDisposable disposable;
								bool flag15 = (disposable = (enumerator5 as IDisposable)) != null;
								if (flag15)
								{
									disposable.Dispose();
								}
							}
							dictionary.Add(innerText5, defenseInfo);
						}
					}
					finally
					{
						IDisposable disposable2;
						bool flag16 = (disposable2 = (enumerator4 as IDisposable)) != null;
						if (flag16)
						{
							disposable2.Dispose();
						}
					}
					childCreatureTypeInfo.defenseTable.Init(dictionary);
					XmlNode xmlNode10 = xmlNode4.SelectSingleNode("script");
					bool flag17 = xmlNode10 != null;
					if (flag17)
					{
						childCreatureTypeInfo.script = xmlNode10.InnerText;
					}
					XmlNode xmlNode11 = xmlNode4.SelectSingleNode("portrait");
					bool flag18 = xmlNode11 != null;
					if (flag18)
					{
						childCreatureTypeInfo._tempPortrait = xmlNode11.InnerText.Trim();
						childCreatureTypeInfo._isChildAndHasData = true;
					}
					XmlNode xmlNode12 = xmlNode4.SelectSingleNode("metaInfo");
					bool flag19 = xmlNode12 != null;
					if (flag19)
					{
						string innerText7 = xmlNode12.InnerText;
						CreatureTypeInfo creatureTypeInfo2 = LoadChildMeta(innerText7, ref list2, ref specialTipSize, true);
						XmlNode statCreature = xmlNode4;
						XmlNode stat2 = xmlNode4.SelectSingleNode("stat");
						this.LoadCreatureStat_Mod(stat2, statCreature, creatureTypeInfo2, modid);
						list.Add(creatureTypeInfo2);
						childCreatureTypeInfo.id = creatureTypeInfo2.id;
						childCreatureTypeInfo.isHasBaseMeta = true;
					}
					XmlNodeList xmlNodeList2 = xmlNode4.SelectNodes("sound");
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					IEnumerator enumerator6 = xmlNodeList2.GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							object obj4 = enumerator6.Current;
							XmlNode xmlNode13 = (XmlNode)obj4;
							string innerText8 = xmlNode13.Attributes.GetNamedItem("action").InnerText;
							string innerText9 = xmlNode13.Attributes.GetNamedItem("src").InnerText;
							dictionary2.Add(innerText8, innerText9);
						}
					}
					finally
					{
						IDisposable disposable3;
						bool flag20 = (disposable3 = (enumerator6 as IDisposable)) != null;
						if (flag20)
						{
							disposable3.Dispose();
						}
					}
					childCreatureTypeInfo.soundTable = dictionary2;
					creatureTypeInfo.childTypeInfo = childCreatureTypeInfo;
					creatureTypeInfo.childTypeInfo.data = data;
				}
				list.Add(creatureTypeInfo);
			}
		}
		finally
		{
			IDisposable disposable4;
			bool flag21 = (disposable4 = (enumerator as IDisposable)) != null;
			if (flag21)
			{
				disposable4.Dispose();
			}
		}
	}

	// Token: 0x04002C19 RID: 11289
	public string currentLn = "en";

	// Token: 0x04002C1A RID: 11290
	public string documentSrcFormat = "Language/{0}/creatures/{1}_{0}";
}
