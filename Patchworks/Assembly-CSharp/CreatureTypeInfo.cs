/*
public string collectionName // 
*/
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Token: 0x02000700 RID: 1792
[Serializable]
public class CreatureTypeInfo
{
	// Token: 0x0600397B RID: 14715 RVA: 0x001702A0 File Offset: 0x0016E4A0
	public CreatureTypeInfo()
	{
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x001703E4 File Offset: 0x0016E5E4
	public static string GetRiskLevelEnumToString(RiskLevel level)
	{
		switch (level)
		{
		case RiskLevel.ZAYIN:
			return "ZAYIN";
		case RiskLevel.TETH:
			return "TETH";
		case RiskLevel.HE:
			return "HE";
		case RiskLevel.WAW:
			return "WAW";
		case RiskLevel.ALEPH:
			return "ALEPH";
		default:
			return string.Empty;
		}
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x00170434 File Offset: 0x0016E634
	public static RiskLevel GetRiskLevelStringToEnum(string level)
	{
		if (level == "ZAYIN")
		{
			return RiskLevel.ZAYIN;
		}
		if (level == "TETH")
		{
			return RiskLevel.TETH;
		}
		if (level == "HE")
		{
			return RiskLevel.HE;
		}
		if (level == "WAW")
		{
			return RiskLevel.WAW;
		}
		if (level == "ALEPH")
		{
			return RiskLevel.ALEPH;
		}
		return RiskLevel.ZAYIN;
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x00033511 File Offset: 0x00031711
	public int CurrentObserveLevel
	{
		get
		{
			return CreatureManager.instance.GetObserveLevel(this.id);
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x0017049C File Offset: 0x0016E69C
	public string name
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("name");
			if (list == null)
			{
				return "Unknown";
			}
			object dataTemp = list.GetDataTemp();
			if (dataTemp == null)
			{
				return this.codeId;
			}
			return (string)dataTemp;
		}
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06003980 RID: 14720 RVA: 0x001704E0 File Offset: 0x0016E6E0
	public string collectionName
	{ // <Mod>
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("name");
			/*
            string Prefix = "";
            bool hasGift = false;
            if (SpecialModeConfig.instance.GetValue<bool>("EgoGiftHelper"))
            {
                AgentInfoWindow infoWindow = AgentInfoWindow.currentWindow;
                if (infoWindow != null)
                {
                    AgentModel agent = infoWindow.CurrentAgent;
                    if (agent != null)
                    {
                        int gift = -1;
                        switch (id)
                        {
                            case 100037L:
                                if (agent.HasEquipment(4000371) || agent.HasEquipment(4000372) || agent.HasEquipment(4000373) || agent.HasEquipment(4000374))
                                {
                                    hasGift = true;
                                }
                                break;
                            case 100102L:
                                gift = 1023;
                                break;
                            default:
                                switch (id)
                                {
                                    case 100032L:
                                    case 100033L:
                                        if (agent.HasEquipment(1033))
                                        {
                                            Prefix += "(**) ";
                                        }
                                        break;
                                    case 100008L:
                                    case 100020L:
                                    case 100035L:
                                        if (agent.HasEquipment(400038))
                                        {
                                            Prefix += "(**) ";
                                        }
                                        break;
                                }
                                CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL);
                                if (creatureEquipmentMakeInfo != null)
                                {
                                    gift = creatureEquipmentMakeInfo.equipTypeInfo.id;
                                }
                                break;
                        }
                        if (gift != -1 && agent.HasEquipment(gift))
                        {
                            hasGift = true;
                        }
                    }
                }
            }
            if (hasGift)
            {
                Prefix += "(*) ";
            }
			if (list == null)
			{
				return Prefix + "Unknown";
			}
			object data = list.GetData(this.CurrentObserveLevel);
			if (data == null)
			{
				return Prefix + this.codeId;
			}
			return Prefix + (string)data;*/
			if (list == null)
			{
				return "Unknown";
			}
			object data = list.GetData(this.CurrentObserveLevel);
			if (data == null)
			{
				return this.codeId;
			}
			return (string)data;
		}
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06003981 RID: 14721 RVA: 0x0017052C File Offset: 0x0016E72C
	public string codeId
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("codeNo");
			return (string)list.GetDataTemp();
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003982 RID: 14722 RVA: 0x00170558 File Offset: 0x0016E758
	public string riskLevelForce
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("riskLevel");
			if (list == null)
			{
				return "Unknown";
			}
			object dataTemp = list.GetDataTemp();
			if (dataTemp == null)
			{
				return "Unknown";
			}
			return (string)dataTemp;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003983 RID: 14723 RVA: 0x0017059C File Offset: 0x0016E79C
	public virtual string riskLevel
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("riskLevel");
			if (list == null)
			{
				return "Unknown";
			}
			object data = list.GetData(this.CurrentObserveLevel);
			if (data == null)
			{
				return "Unknown";
			}
			return (string)data;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06003984 RID: 14724 RVA: 0x001705E8 File Offset: 0x0016E7E8
	public string portraitSrc
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("portrait");
			if (list == null)
			{
				return "Sprites/Unit/creature/NoData";
			}
			object data = list.GetData(this.CurrentObserveLevel);
			if (data == null)
			{
				return "Sprites/Unit/creature/NoData";
			}
			return (string)data;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06003985 RID: 14725 RVA: 0x00170634 File Offset: 0x0016E834
	public string portraitSrcForcely
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("portrait");
			if (list == null)
			{
				return "Sprites/Unit/creature/NoData";
			}
			object dataTemp = list.GetDataTemp();
			return (string)dataTemp;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06003986 RID: 14726 RVA: 0x0017066C File Offset: 0x0016E86C
	public string specialSkillName
	{
		get
		{
			CreatureTypeInfo.CreatureDataList list = this.dataTable.GetList("specialName");
			if (list == null)
			{
				return "Unknown";
			}
			if (this.activateSpecialSkill)
			{
				return (string)list.GetDataTemp();
			}
			object data = list.GetData(this.CurrentObserveLevel);
			if (data == null)
			{
				return "Unknown";
			}
			return (string)data;
		}
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x00033523 File Offset: 0x00031723
	public void ActivatedSpecialSkill()
	{
		this.activateSpecialSkill = true;
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x0003352C File Offset: 0x0003172C
	public virtual RiskLevel GetRiskLevel()
	{
		return CreatureTypeInfo.GetRiskLevelStringToEnum(this.riskLevelForce);
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x00033539 File Offset: 0x00031739
	public bool GetAgentName(int index, out AgentName name)
	{
		return this.collectionUsedAgentName.TryGetValue(index, out name);
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x00033548 File Offset: 0x00031748
	public void AddAgentName(int index, AgentName input)
	{
		this.collectionUsedAgentName.Add(index, input);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x001706CC File Offset: 0x0016E8CC
	// Note: this type is marked as 'beforefieldinit'.
	static CreatureTypeInfo()
	{
	}

	// Token: 0x04003455 RID: 13397
	public static string[] stringData = new string[]
	{
		"codeNo",
		"portrait",
		"name",
		"creatureType",
		"riskLevel",
		"specialName"
	};

	// Token: 0x04003456 RID: 13398
	public static string[] intData = new string[]
	{
		"intelligence",
		"physical",
		"mental"
	};

	// Token: 0x04003457 RID: 13399
	public ChildCreatureTypeInfo childTypeInfo;

	// Token: 0x04003458 RID: 13400
	public const string noDataString = "Unknown";

	// Token: 0x04003459 RID: 13401
	public long id;

	// Token: 0x0400345A RID: 13402
	public int maxHp = 1;

	// Token: 0x0400345B RID: 13403
	public CreatureWorkType creatureWorkType;

	// Token: 0x0400345C RID: 13404
	public CreatureKitType creatureKitType;

	// Token: 0x0400345D RID: 13405
	public string kitIconSrc = string.Empty;

	// Token: 0x0400345E RID: 13406
	public string workAnim = "KitEquipCreatureUse";

	// Token: 0x0400345F RID: 13407
	public string workAnimFace = "default";

	// Token: 0x04003460 RID: 13408
	public FeelingStateCubeBounds feelingStateCubeBounds = new FeelingStateCubeBounds();

	// Token: 0x04003461 RID: 13409
	public CreatureSpecialDamageTable creatureSpecialDamageTable = new CreatureSpecialDamageTable();

	// Token: 0x04003462 RID: 13410
	public DamageInfo workDamage = DamageInfo.zero;

	// Token: 0x04003463 RID: 13411
	public CreatureDefenseTable defenseTable = new CreatureDefenseTable();

	// Token: 0x04003464 RID: 13412
	public int workCooltime = 10;

	// Token: 0x04003465 RID: 13413
	public float cubeSpeed = 1f;

	// Token: 0x04003466 RID: 13414
	public CreatureWorkProbTable workProbTable = new CreatureWorkProbTable();

	// Token: 0x04003467 RID: 13415
	public List<ObserveInfoData> observeData = new List<ObserveInfoData>();

	// Token: 0x04003468 RID: 13416
	public List<CreatureEquipmentMakeInfo> equipMakeInfos = new List<CreatureEquipmentMakeInfo>();

	// Token: 0x04003469 RID: 13417
	public CreatureObserveBonusList observeBonus = new CreatureObserveBonusList();

	// Token: 0x0400346A RID: 13418
	public int maxWorkCount;

	// Token: 0x0400346B RID: 13419
	public int maxProbReductionCounter = -1;

	// Token: 0x0400346C RID: 13420
	public float probReduction = 0.01f;

	// Token: 0x0400346D RID: 13421
	public string animSrc = string.Empty;

	// Token: 0x0400346E RID: 13422
	public string roomReturnSrc;

	// Token: 0x0400346F RID: 13423
	public string script;

	// Token: 0x04003470 RID: 13424
	public int qliphothMax;

	// Token: 0x04003471 RID: 13425
	public Dictionary<string, string> typoTable;

	// Token: 0x04003472 RID: 13426
	public Dictionary<string, string> narrationTable;

	// Token: 0x04003473 RID: 13427
	public Dictionary<string, string> soundTable;

	// Token: 0x04003474 RID: 13428
	public string observe;

	// Token: 0x04003475 RID: 13429
	public string openText = string.Empty;

	// Token: 0x04003476 RID: 13430
	public string[] observeList;

	// Token: 0x04003477 RID: 13431
	public XmlNodeList nodeInfo;

	// Token: 0x04003478 RID: 13432
	public XmlNodeList edgeInfo;

	// Token: 0x04003479 RID: 13433
	public CreatureTypeInfo.CreatureDataTable dataTable = new CreatureTypeInfo.CreatureDataTable();

	// Token: 0x0400347A RID: 13434
	public CreatureTypeInfo.ObserveTable observeTable = new CreatureTypeInfo.ObserveTable();

	// Token: 0x0400347B RID: 13435
	public CreatureSpecialSkillTipTable specialSkillTable;

	// Token: 0x0400347C RID: 13436
	public Dictionary<int, AgentName> collectionUsedAgentName = new Dictionary<int, AgentName>();

	// Token: 0x0400347D RID: 13437
	public bool isEscapeAble = true;

	// Token: 0x0400347E RID: 13438
	public int MaxObserveLevel;

	// Token: 0x0400347F RID: 13439
	public float speed = 1f;

	// Token: 0x04003480 RID: 13440
	public bool _isChildAndHasData;

	// Token: 0x04003481 RID: 13441
	public string _tempPortrait = string.Empty;

	// Token: 0x04003482 RID: 13442
	private bool activateSpecialSkill;

	// Token: 0x04003483 RID: 13443
	public Sprite tempPortrait;

	// Token: 0x04003484 RID: 13444
	public List<string> desc = new List<string>();

	// Token: 0x04003485 RID: 13445
	public List<string> observeRecord = new List<string>();

	// Token: 0x04003486 RID: 13446
	public SkillTriggerCheck skillTriggerCheck = new SkillTriggerCheck();

	// Token: 0x04003487 RID: 13447
	public CreatureMaxObserve maxObserveModule = new CreatureMaxObserve();

	// Token: 0x04003488 RID: 13448
	public CreatureStaticData creatureStaticData = new CreatureStaticData();

	// Token: 0x02000701 RID: 1793
	public class CreatureData
	{
		// Token: 0x0600398C RID: 14732 RVA: 0x00004074 File Offset: 0x00002274
		public CreatureData()
		{
		}

		// Token: 0x04003489 RID: 13449
		public object data;

		// Token: 0x0400348A RID: 13450
		public int openLevel;
	}

	// Token: 0x02000702 RID: 1794
	public class CreatureDataWithType<T>
	{
		// Token: 0x0600398D RID: 14733 RVA: 0x00004074 File Offset: 0x00002274
		public CreatureDataWithType()
		{
		}

		// Token: 0x0400348B RID: 13451
		public T data;

		// Token: 0x0400348C RID: 13452
		public int openLevel;
	}

	// Token: 0x02000703 RID: 1795
	public class CreatureDataList
	{
		// Token: 0x0600398E RID: 14734 RVA: 0x00033557 File Offset: 0x00031757
		public CreatureDataList()
		{
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x00170738 File Offset: 0x0016E938
		public object GetData(int level)
		{
			object result = null;
			foreach (CreatureTypeInfo.CreatureData creatureData in this.list)
			{
				if (creatureData.openLevel <= level)
				{
					result = creatureData.data;
				}
				if (creatureData.openLevel > level)
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x001707B4 File Offset: 0x0016E9B4
		public T GetData<T>(int level)
		{
			object obj = null;
			foreach (CreatureTypeInfo.CreatureData creatureData in this.list)
			{
				if (creatureData.openLevel <= level)
				{
					obj = creatureData;
				}
				if (creatureData.openLevel > level)
				{
					break;
				}
			}
			return (T)((object)obj);
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x00170830 File Offset: 0x0016EA30
		public int GetLevel(int level)
		{
			int result = 1000;
			foreach (CreatureTypeInfo.CreatureData creatureData in this.list)
			{
				if (creatureData.openLevel <= level)
				{
					result = creatureData.openLevel;
				}
				if (creatureData.openLevel > level)
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0003356A File Offset: 0x0003176A
		public void AddData(CreatureTypeInfo.CreatureData data)
		{
			this.list.Add(data);
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x00033578 File Offset: 0x00031778
		public int GetCount()
		{
			return this.list.Count;
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x001708B0 File Offset: 0x0016EAB0
		public object GetDataTemp()
		{
			object result = null;
			foreach (CreatureTypeInfo.CreatureData creatureData in this.list)
			{
				if (creatureData != null)
				{
					result = creatureData.data;
					break;
				}
			}
			return result;
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0017091C File Offset: 0x0016EB1C
		public bool GetAllData(ref List<object> objects, ref List<int> levels)
		{
			List<object> list = new List<object>();
			List<int> list2 = new List<int>();
			foreach (CreatureTypeInfo.CreatureData creatureData in this.list)
			{
				list.Add(creatureData.data);
				list2.Add(creatureData.openLevel);
			}
			objects = list;
			levels = list2;
			return list.Count != 0;
		}

		// Token: 0x0400348D RID: 13453
		public string itemName;

		// Token: 0x0400348E RID: 13454
		private List<CreatureTypeInfo.CreatureData> list = new List<CreatureTypeInfo.CreatureData>();
	}

	// Token: 0x02000704 RID: 1796
	public class CreatureDataTable
	{
		// Token: 0x06003996 RID: 14742 RVA: 0x00033585 File Offset: 0x00031785
		public CreatureDataTable()
		{
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x001709A8 File Offset: 0x0016EBA8
		public CreatureTypeInfo.CreatureDataList GetList(string key)
		{
			CreatureTypeInfo.CreatureDataList result = null;
			if (this.dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			Debug.LogError("Dictionary error in collection //Not founded " + key + " CreatureDataList");
			return null;
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x001709E4 File Offset: 0x0016EBE4
		public void PrintElementName()
		{
			foreach (KeyValuePair<string, CreatureTypeInfo.CreatureDataList> keyValuePair in this.dictionary)
			{
				Debug.Log(keyValuePair.Value.itemName + " " + keyValuePair.Value.GetCount());
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x00170A68 File Offset: 0x0016EC68
		public string[] GetDataString()
		{
			List<string> list = new List<string>();
			foreach (string text in CreatureTypeInfo.stringData)
			{
				CreatureTypeInfo.CreatureDataList creatureDataList;
				if (this.dictionary.TryGetValue(text, out creatureDataList))
				{
					list.Add(text);
					List<object> list2 = new List<object>();
					List<int> list3 = new List<int>();
					if (creatureDataList.GetAllData(ref list2, ref list3))
					{
						for (int j = 0; j < list2.Count; j++)
						{
							string item = (string)list2[j] + " " + list3[j];
							list.Add(item);
						}
					}
				}
				else
				{
					Debug.LogWarning("dictionary does not have string data of " + text);
				}
			}
			list.Add("end of string");
			return list.ToArray();
		}

		// Token: 0x0400348F RID: 13455
		public Dictionary<string, CreatureTypeInfo.CreatureDataList> dictionary = new Dictionary<string, CreatureTypeInfo.CreatureDataList>();
	}

	// Token: 0x02000705 RID: 1797
	public class ObserveTable
	{
		// Token: 0x0600399A RID: 14746 RVA: 0x00033598 File Offset: 0x00031798
		public ObserveTable()
		{
			this.desc = new List<int>();
			this.record = new List<int>();
		}

		// Token: 0x04003490 RID: 13456
		public List<int> desc;

		// Token: 0x04003491 RID: 13457
		public List<int> record;
	}
}
