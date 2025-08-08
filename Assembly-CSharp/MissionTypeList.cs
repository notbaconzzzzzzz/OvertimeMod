using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //
using System.Xml;
using UnityEngine;

// Token: 0x0200071C RID: 1820
public class MissionTypeList
{
	// Token: 0x060039E6 RID: 14822 RVA: 0x000338F5 File Offset: 0x00031AF5
	private MissionTypeList()
	{
		this._list = new List<MissionTypeInfo>();
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x00033908 File Offset: 0x00031B08
	public static MissionTypeList instance
	{
		get
		{
			if (MissionTypeList._instance == null)
			{
				MissionTypeList._instance = new MissionTypeList();
			}
			return MissionTypeList._instance;
		}
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x00171F98 File Offset: 0x00170198
	public void LoadData()
	{ // <Mod> Load external file
		List<MissionTypeInfo> list = new List<MissionTypeInfo>();
		List<string> list2 = new List<string>
		{
			"max",
			"min",
			"same"
		};
		List<string> list3 = new List<string>
		{
			"WORK_RESULT",
			"WORK_TYPE"
		};
		List<string> list4 = new List<string>
		{
			"AGENT_STAT",
			"AGENT_LEVEL",
			"EGO_GIFT_COUNT"
		};
		List<string> list5 = new List<string>
		{
			"CREATURE_LEVEL",
			"CREATURE_OBSERVATION"
		};
		List<string> list6 = new List<string>
		{
			"AGENT_DEAD",
			"AGENT_PANIC",
			"WORK_BAD"
		};
		List<string> list7 = new List<string>
		{
			"WORK",
			"SUPPRESS_CREATURE_BY_KIND",
			"PROMOTE_AGENT",
			"CLEAR_DAY",
			"CLEAR_TIME",
			"SUPPRESS_CREATURE",
			"QLIPHOTH_OVERLOAD",
			"WORK_TO_OVERLOADED",
			"ORDEAL_END",
			"AGENT_PER_SEFIRA",
			"CLEAR_WITH_AGENT_BY_CONDITION",
			"MAKE_EQUIPMENT",
			"ISOLATE_OVERLOAD",
			"CREATURES_IN_CONDITION_ALL",
			"EQUIPMENTS_IN_CONDITION",
			"DESTROY_CORE"
		};
		List<string> list9 = new List<string>
		{
			"SUPPRESS_CREATURE_BY_TIME",
			"DONT_OPEN_INFO_WINDOW",
			"BALANCE_WORK_RESULTS",
			"BALANCE_WORK_TYPES",
			"AGENT_PREV_LEVEL",
			"SPECIAL_SUPPRESS_CREATURE",
			"RECOVER_BY_REGENERATOR",
			"CLERK_DEAD",
			"SPECIAL_SUPPRESS_PANIC",
			"PRODUCE_EXCESS_ENERGY",
			"WORK_TO_OVERTIME_OVERLOADED",
			"SPECIAL_DEAL_DAMANGE_WEAKEST",
			"COMPLETION_TIME",
			"RECOVER_BY_BULLET",
			"BLOCK_DAMAGE_BY_BULLET",
			"USE_BULLET",
			"CURRENTLY_EQUIPPED",
			"STATS_AT_MAX",
			"DONT_PAUSE"
		};
		List<ConditionCategory> list10 = new List<ConditionCategory>
		{
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.FAIL_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.AGENT_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.FAIL_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.FAIL_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.ACTION_CONDITION,
			ConditionCategory.CREATURE_CONDITION,
			ConditionCategory.AGENT_CONDITION,
			ConditionCategory.FAIL_CONDITION
		};
		List<string> list8 = new List<string>();
		list8.AddRange(list3);
		list8.AddRange(list4);
		list8.AddRange(list5);
		list8.AddRange(list6);
		list8.AddRange(list7);
		list8.AddRange(list9);
		int[] array = new int[]
		{
			list3.Count,
			list4.Count,
			list5.Count,
			list6.Count,
			list7.Count,
			list9.Count
		};
        if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseMissionTable.txt"))
		{
			TextAsset textAsset = Resources.Load<TextAsset>("xml/MissionTable");
			File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseMissionTable.txt", textAsset.text);
		}
		string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseMissionTable.txt");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("missions/mission");
		for (int i = 0; i < xmlNodeList.Count; i++)
		{
			XmlNode xmlNode = xmlNodeList[i];
			MissionTypeInfo missionTypeInfo = new MissionTypeInfo();
			XmlNode namedItem = xmlNode.Attributes.GetNamedItem("id");
			if (namedItem != null)
			{
				int id = (int)float.Parse(namedItem.InnerText);
				missionTypeInfo.id = id;
			}
			XmlNode namedItem2 = xmlNode.Attributes.GetNamedItem("sefira");
			if (namedItem2 != null)
			{
				string innerText = namedItem2.InnerText;
				missionTypeInfo.sefira_Name = innerText;
				missionTypeInfo.sefira = SefiraName.GetSefiraEnum(innerText);
			}
			XmlNode namedItem3 = xmlNode.Attributes.GetNamedItem("level");
			if (namedItem3 != null)
			{
				int sefira_Level = (int)float.Parse(namedItem3.InnerText);
				missionTypeInfo.sefira_Level = sefira_Level;
			}
			XmlNode namedItem4 = xmlNode.Attributes.GetNamedItem("isGlobal");
			if (namedItem4 != null)
			{
				bool isGlobal = namedItem4.InnerText == "true";
				missionTypeInfo.isGlobal = isGlobal;
			}
			XmlNode namedItem5 = xmlNode.Attributes.GetNamedItem("intro");
			if (namedItem5 != null)
			{
				missionTypeInfo.intro = namedItem5.InnerText;
			}
			XmlNode namedItem6 = xmlNode.Attributes.GetNamedItem("clear");
			if (namedItem6 != null)
			{
				missionTypeInfo.clear = namedItem6.InnerText;
			}
			XmlNode namedItem7 = xmlNode.Attributes.GetNamedItem("title");
			if (namedItem7 != null)
			{
				missionTypeInfo.title = namedItem7.InnerText;
			}
			XmlNode namedItem8 = xmlNode.Attributes.GetNamedItem("desc");
			if (namedItem8 != null)
			{
				missionTypeInfo.desc = namedItem8.InnerText;
			}
			XmlNode namedItem9 = xmlNode.Attributes.GetNamedItem("diag");
			if (namedItem9 != null)
			{
				missionTypeInfo.diag = namedItem9.InnerText;
			}
			XmlNode namedItem10 = xmlNode.Attributes.GetNamedItem("short");
			if (namedItem10 != null)
			{
				missionTypeInfo.shortDesc = namedItem10.InnerText;
			}
			missionTypeInfo.requires = new List<MissionPrerequisite>();
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("require");
			IEnumerator enumerator = xmlNodeList2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode2 = (XmlNode)obj;
					MissionPrerequisite missionPrerequisite = new MissionPrerequisite();
					XmlNode namedItem11 = xmlNode2.Attributes.GetNamedItem("sefira");
					missionPrerequisite.sefira = SefiraName.GetSefiraEnum(namedItem11.InnerText);
					XmlNode namedItem12 = xmlNode2.Attributes.GetNamedItem("level");
					missionPrerequisite.level = int.Parse(namedItem12.InnerText);
					missionTypeInfo.requires.Add(missionPrerequisite);
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
			XmlNodeList xmlNodeList3 = xmlNode.SelectNodes("condition");
			for (int j = 0; j < xmlNodeList3.Count; j++)
			{
				XmlNode xmlNode3 = xmlNodeList3[j];
				MissionConditionTypeInfo missionConditionTypeInfo = new MissionConditionTypeInfo();
				XmlNode namedItem13 = xmlNode3.Attributes.GetNamedItem("index");
				if (namedItem13 != null)
				{
					int index = (int)float.Parse(namedItem13.InnerText);
					missionConditionTypeInfo.index = index;
				}
				XmlNode namedItem14 = xmlNode3.Attributes.GetNamedItem("goalValue");
				if (namedItem14 != null)
				{
					int goal = (int)float.Parse(namedItem14.InnerText);
					missionConditionTypeInfo.goal = goal;
				}
				XmlNode namedItem15 = xmlNode3.Attributes.GetNamedItem("goalType");
				if (namedItem15 != null && list2.Contains(namedItem15.InnerText))
				{
					missionConditionTypeInfo.goal_Type = (GoalType)list2.IndexOf(namedItem15.InnerText);
				}
				XmlNode namedItem16 = xmlNode3.Attributes.GetNamedItem("type");
				if (namedItem16 != null && list8.Contains(namedItem16.InnerText))
				{
					ConditionType conditionType = (ConditionType)list8.IndexOf(namedItem16.InnerText);
					missionConditionTypeInfo.condition_Type = conditionType;
					if (conditionType == ConditionType.AGENT_STAT || conditionType == ConditionType.RECOVER_BY_REGENERATOR || conditionType == ConditionType.RECOVER_BY_BULLET || conditionType == ConditionType.BLOCK_DAMAGE_BY_BULLET)
					{
						XmlNode namedItem17 = xmlNode3.Attributes.GetNamedItem("stat");
						if (namedItem17 != null)
						{
							int stat = (int)float.Parse(namedItem17.InnerText);
							missionConditionTypeInfo.stat = stat;
						}
					}
					else if (conditionType == ConditionType.CLEAR_TIME || conditionType == ConditionType.COMPLETION_TIME)
					{
						XmlNode namedItem18 = xmlNode3.Attributes.GetNamedItem("minimum");
						if (namedItem18 != null)
						{
							int minimumSecond = (int)float.Parse(namedItem18.InnerText);
							missionConditionTypeInfo.minimumSecond = minimumSecond;
						}
						XmlNode namedItem19 = xmlNode3.Attributes.GetNamedItem("var1");
						if (namedItem19 != null)
						{
							float var = float.Parse(namedItem19.InnerText);
							missionConditionTypeInfo.var1 = var;
						}
						XmlNode namedItem20 = xmlNode3.Attributes.GetNamedItem("var2");
						if (namedItem20 != null)
						{
							float var2 = float.Parse(namedItem20.InnerText);
							missionConditionTypeInfo.var2 = var2;
						}
					}
					else if (conditionType == ConditionType.SUPPRESS_CREATURE_BY_KIND || conditionType == ConditionType.CLERK_DEAD)
					{
						XmlNode namedItem21 = xmlNode3.Attributes.GetNamedItem("percent");
						if (namedItem21 != null)
						{
							float percent = float.Parse(namedItem21.InnerText);
							missionConditionTypeInfo.percent = percent;
						}
					}
					else if (conditionType == ConditionType.SUPPRESS_CREATURE_BY_TIME)
					{
						XmlNode namedItem22 = xmlNode3.Attributes.GetNamedItem("minimum");
						if (namedItem22 != null)
						{
							int minimum = (int)float.Parse(namedItem22.InnerText);
							missionConditionTypeInfo.minimumSecond = minimum;
						}
					}
				}
				int num = 0;
				bool flag = false;
				for (int k = 0; k < array.Length; k++)
				{
					for (int l = 0; l < array[k]; l++)
					{
						if (missionConditionTypeInfo.condition_Type == (ConditionType)num)
						{
							if (k < 5)
							{
								missionConditionTypeInfo.condition_Category = (ConditionCategory)k;
							}
							else
							{
								missionConditionTypeInfo.condition_Category = list10[l];
							}
							flag = true;
							break;
						}
						num++;
					}
					if (flag)
					{
						break;
					}
				}
				missionTypeInfo.conditions.Add(missionConditionTypeInfo);
			}
			list.Add(missionTypeInfo);
		}
		this._list = list;
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x0017270C File Offset: 0x0017090C
	public MissionTypeInfo GetData(int id)
	{
		return this._list.Find((MissionTypeInfo x) => x.id == id);
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00033923 File Offset: 0x00031B23
	public IList<MissionTypeInfo> GetList()
	{
		return this._list.AsReadOnly();
	}

	// Token: 0x0400351F RID: 13599
	private static MissionTypeList _instance;

	// Token: 0x04003520 RID: 13600
	private List<MissionTypeInfo> _list;
}
