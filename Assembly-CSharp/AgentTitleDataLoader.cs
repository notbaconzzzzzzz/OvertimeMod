/*
public void Load() // Load external file instead
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; // 
using System.Xml;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class AgentTitleDataLoader
{
	// Token: 0x06002E70 RID: 11888 RVA: 0x00003DF4 File Offset: 0x00001FF4
	public AgentTitleDataLoader()
	{
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x00135B20 File Offset: 0x00133D20
	public void Load()
	{ // <Mod> Load external file instead of resources file so that Title Bonuses can be modified
		XmlDocument xmlDocument = new XmlDocument();
		if (!File.Exists(Application.dataPath + "/Managed/BaseMod/AgentTitleList.txt"))
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/AgentTitle/AgentTitleList");
			File.WriteAllText(Application.dataPath + "/Managed/BaseMod/AgentTitleList.txt", textAsset.text);
		}
		string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/AgentTitleList.txt");
		xmlDocument.LoadXml(xml);
		List<AgentTitleTypeInfo> list = new List<AgentTitleTypeInfo>();
		XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
		IEnumerator enumerator = xmlNode.SelectNodes("list").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode2 = (XmlNode)obj;
				int level = int.Parse(xmlNode2.Attributes.GetNamedItem("level").InnerText);
				IEnumerator enumerator2 = xmlNode2.SelectNodes("title").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode3 = (XmlNode)obj2;
						XmlAttributeCollection attributes = xmlNode3.Attributes;
						XmlNode namedItem = attributes.GetNamedItem("id");
						XmlNode namedItem2 = attributes.GetNamedItem("pos");
						XmlNode namedItem3 = attributes.GetNamedItem("rwbp");
						XmlNode namedItem4 = attributes.GetNamedItem("hp");
						XmlNode namedItem5 = attributes.GetNamedItem("mental");
						XmlNode namedItem6 = attributes.GetNamedItem("cubeSpeed");
						XmlNode namedItem7 = attributes.GetNamedItem("workProb");
						XmlNode namedItem8 = attributes.GetNamedItem("movementSpeed");
						XmlNode namedItem9 = attributes.GetNamedItem("attackSpeed");
						AgentTitleTypeInfo agentTitleTypeInfo = new AgentTitleTypeInfo();
						agentTitleTypeInfo.level = level;
						agentTitleTypeInfo.nameId = xmlNode3.InnerText;
						agentTitleTypeInfo.name = LocalizeTextDataModel.instance.GetText(agentTitleTypeInfo.nameId);
						agentTitleTypeInfo.id = int.Parse(namedItem.InnerText);
						if (namedItem2 != null)
						{
							agentTitleTypeInfo.pos = namedItem2.InnerText;
						}
						if (namedItem3 != null)
						{
							agentTitleTypeInfo.rwbp = namedItem3.InnerText;
						}
						if (namedItem4 != null)
						{
							agentTitleTypeInfo.hp = int.Parse(namedItem4.InnerText);
						}
						if (namedItem5 != null)
						{
							agentTitleTypeInfo.mental = int.Parse(namedItem5.InnerText);
						}
						if (namedItem6 != null)
						{
							agentTitleTypeInfo.cubeSpeed = int.Parse(namedItem6.InnerText);
						}
						if (namedItem7 != null)
						{
							agentTitleTypeInfo.workProb = int.Parse(namedItem7.InnerText);
						}
						if (namedItem8 != null)
						{
							agentTitleTypeInfo.movementSpeed = int.Parse(namedItem8.InnerText);
						}
						if (namedItem9 != null)
						{
							agentTitleTypeInfo.attackSpeed = int.Parse(namedItem9.InnerText);
						}
						list.Add(agentTitleTypeInfo);
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
		AgentTitleTypeList.instance.Init(list);
	}
}
