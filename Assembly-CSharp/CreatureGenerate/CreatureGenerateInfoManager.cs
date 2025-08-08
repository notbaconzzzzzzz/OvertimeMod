using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace CreatureGenerate
{
	// Token: 0x020007E3 RID: 2019
	public class CreatureGenerateInfoManager
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x00183EF4 File Offset: 0x001820F4
		public CreatureGenerateInfoManager()
		{
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06003E4D RID: 15949 RVA: 0x000366E4 File Offset: 0x000348E4
		public static CreatureGenerateInfoManager Instance
		{
			get
			{
				if (CreatureGenerateInfoManager._instance == null)
				{
					CreatureGenerateInfoManager._instance = new CreatureGenerateInfoManager();
				}
				return CreatureGenerateInfoManager._instance;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x000366FF File Offset: 0x000348FF
		public bool IsloadedDayData
		{
			get
			{
				if (!this._isLoadedDayData)
				{
					this._isLoadedDayData = this.LoadStaticData();
				}
				return this._isLoadedDayData;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06003E4F RID: 15951 RVA: 0x0003671E File Offset: 0x0003491E
		public bool IsInitiated
		{
			get
			{
				return this._isInitiated;
			}
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x00036726 File Offset: 0x00034926
		public static void Log(string text, bool isError = false)
		{
			if (isError)
			{
				Debug.LogError("<color=#FF2323>[CreatureGenerate]</color> " + text);
			}
			else
			{
				Debug.Log("<color=#FF2323>[CreatureGenerate]</color> " + text);
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x00036753 File Offset: 0x00034953
		public int GenDay
		{
			get
			{
				return this._genDay;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0003675B File Offset: 0x0003495B
		// (set) Token: 0x06003E53 RID: 15955 RVA: 0x00036763 File Offset: 0x00034963
		public bool GenKit
		{
			get
			{
				return this._genKit;
			}
			set
			{
				this._genKit = value;
			}
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x00183F44 File Offset: 0x00182144
		public void InitCreatureList()
		{
			this.CreatureList.Clear();
			foreach (CreatureTypeInfo creatureTypeInfo in CreatureTypeList.instance.GetList())
			{
				if (!this.CheckGenerationIgnore(creatureTypeInfo.id))
				{
					List<long> list2 = null;
					if (!this.CreatureList.TryGetValue(creatureTypeInfo.GetRiskLevel(), out list2))
					{
						list2 = new List<long>();
						this.CreatureList.Add(creatureTypeInfo.GetRiskLevel(), list2);
					}
					list2.Add(creatureTypeInfo.id);
				}
			}
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x000044AF File Offset: 0x000026AF
		private bool CheckGenerationIgnore(long id)
		{
			return false;
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x00183FD4 File Offset: 0x001821D4
		public void Init()
		{
			this.InitCreatureList();
			this.activateStateDic.Clear();
			this._isInitiated = true;
			foreach (long id in CreatureGenerateInfo.GetAll(false))
			{
				CreatureTypeInfo data = CreatureTypeList.instance.GetData(id);
				RiskLevel riskLevel = data.GetRiskLevel();
				long id2 = data.id;
				bool isUsed = this.IsUsedCreature(id2);
				bool isKit = data.creatureWorkType == CreatureWorkType.KIT;
				ActivateStateModel model = new ActivateStateModel
				{
					riskLevel = riskLevel,
					id = id2,
					isUsed = isUsed,
					isKit = isKit
				};
				ActivateStateList activateStateList = null;
				if (this.activateStateDic.TryGetValue(riskLevel, out activateStateList))
				{
					activateStateList.Add(model);
				}
				else
				{
					activateStateList = new ActivateStateList
					{
						riskLevel = riskLevel
					};
					activateStateList.Add(model);
					this.activateStateDic.Add(riskLevel, activateStateList);
				}
			}
			this.CheckCreatureUseState();
			if (this.IsloadedDayData)
			{
				CreatureGenerateInfoManager.Log("Loaded", false);
				this.Print();
				return;
			}
			CreatureGenerateInfoManager.Log("Load Fail", true);
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0003676C File Offset: 0x0003496C
		public void OnDayChanged()
		{
			this.CheckCreatureUseState();
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00036774 File Offset: 0x00034974
		public bool GetCreatureState(RiskLevel risk, out ActivateStateList list)
		{
			return this.activateStateDic.TryGetValue(risk, out list);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x001840DC File Offset: 0x001822DC
		private void CheckCreatureUseState()
		{
			foreach (ActivateStateList activateStateList in this.activateStateDic.Values)
			{
				activateStateList.DayUpdate();
				activateStateList.CheckUsableState();
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00184144 File Offset: 0x00182344
		private bool LoadStaticData()
		{
			char c = ',';
			try
			{
				TextAsset textAsset = Resources.Load<TextAsset>("xml/CreatureGenInfo");
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(textAsset.text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("root/day");
				IEnumerator enumerator = xmlNodeList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlNode xmlNode = (XmlNode)obj;
						int num = int.Parse(xmlNode.Attributes.GetNamedItem("day").InnerText);
						XmlNode namedItem = xmlNode.Attributes.GetNamedItem("door1");
						XmlNode namedItem2 = xmlNode.Attributes.GetNamedItem("door2");
						XmlNode namedItem3 = xmlNode.Attributes.GetNamedItem("door3");
						XmlNode namedItem4 = xmlNode.Attributes.GetNamedItem("action");
						CreatureGenerateModel creatureGenerateModel = new CreatureGenerateModel();
						CreatureSelectData creatureSelectData = new CreatureSelectData(num);
						List<float> d = null;
						List<float> d2 = null;
						List<float> d3 = null;
						bool flag = false;
						if (namedItem4 != null)
						{
							flag = true;
							creatureGenerateModel.commonAction = creatureGenerateModel.ParseActionNode(namedItem4.InnerText.Trim());
							string text = namedItem4.InnerText.Trim();
							string[] array = text.Split(new char[]
							{
								c
							});
							List<long> list = new List<long>();
							GenerateCommonAction action = GenerateCommonAction.NONE;
							foreach (string text2 in array)
							{
								long item = -1L;
								if (long.TryParse(text2, out item))
								{
									list.Add(item);
								}
								else if (text2 != null)
								{
									if (!(text2 == "only"))
									{
										if (text2 == "remove")
										{
											action = GenerateCommonAction.REMOVE;
										}
									}
									else
									{
										action = GenerateCommonAction.ONLY;
									}
								}
							}
							creatureSelectData.SetActionType(action, list.ToArray());
						}
						if (namedItem != null)
						{
							flag = true;
							this.LoadDoor(namedItem.InnerText, out creatureGenerateModel.door1);
							d = this.ParseDoor(namedItem.InnerText);
						}
						if (namedItem2 != null)
						{
							flag = true;
							this.LoadDoor(namedItem2.InnerText, out creatureGenerateModel.door2);
							d2 = this.ParseDoor(namedItem2.InnerText);
						}
						if (namedItem3 != null)
						{
							flag = true;
							this.LoadDoor(namedItem3.InnerText, out creatureGenerateModel.door3);
							d3 = this.ParseDoor(namedItem3.InnerText);
						}
						creatureSelectData.SetProb(d, d2, d3);
						this.SelectData.Add(num, creatureSelectData);
						if (flag)
						{
							creatureGenerateModel.day = num;
							this.dayGenInfoDic.Add(num, creatureGenerateModel);
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
			}
			catch (Exception ex)
			{
				CreatureGenerateInfoManager.Log(ex.Message, true);
				return false;
			}
			return true;
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00184444 File Offset: 0x00182644
		private bool LoadDoor(string parsed, out CreatureGenerateDoor door)
		{
			try
			{
				door = CreatureGenerateDoor.Parse(parsed);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				door = null;
				return false;
			}
			return true;
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00184484 File Offset: 0x00182684
		private List<float> ParseDoor(string text)
		{
			string[] array = text.Split(new char[]
			{
				','
			});
			List<float> list = new List<float>();
			foreach (string s in array)
			{
				float item = -1f;
				if (float.TryParse(s, out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00036783 File Offset: 0x00034983
		private bool IsUsedCreature(long id)
		{
			return CreatureManager.instance.IsCreatureActivated(id);
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x001844E8 File Offset: 0x001826E8
		public void Print()
		{
			foreach (CreatureGenerateModel creatureGenerateModel in this.dayGenInfoDic.Values)
			{
			}
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00184544 File Offset: 0x00182744
		public bool HasUniqueAction(string[] split, out int index)
		{
			bool result = false;
			index = -1;
			for (int i = 0; i < split.Length; i++)
			{
				if (this.GenerateCommonActionList.Contains(split[i].Trim()))
				{
					index = i;
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00184590 File Offset: 0x00182790
		public void RemoveAction(long id)
		{
			CreatureTypeInfo data = CreatureTypeList.instance.GetData(id);
			ActivateStateList activateStateList = null;
			if (this.GetCreatureState(data.GetRiskLevel(), out activateStateList))
			{
				activateStateList.RemoveAction(id);
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x001845C8 File Offset: 0x001827C8
		public void DebugCheck(int day)
		{
			CreatureGenerateModel creatureGenerateModel = null;
			if (this.dayGenInfoDic.TryGetValue(day, out creatureGenerateModel))
			{
				creatureGenerateModel.SetCreature();
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x001845F0 File Offset: 0x001827F0
		public List<long> GetCreatureNew()
		{
			CreatureSelectData creatureSelectData = null;
			int genDay = this.GenDay;
			List<long> result;
			if (this.SelectData.TryGetValue(genDay, out creatureSelectData))
			{
				result = creatureSelectData.GetCreature();
			}
			else
			{
				Debug.LogError("Zero");
				result = new List<long>();
			}
			return result;
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00184638 File Offset: 0x00182838
		public List<long> GetCreature()
		{
			CreatureGenerateModel creatureGenerateModel = null;
			int key = this.GenDay;
			if (this.dayGenInfoDic.TryGetValue(key, out creatureGenerateModel))
			{
				try
				{
					creatureGenerateModel.SetCreature();
				}
				catch (CreatureGenerateInfoManager.ProbCheckExeption probCheckExeption)
				{
					key = -2;
					if (this.dayGenInfoDic.TryGetValue(key, out creatureGenerateModel))
					{
						try
						{
							creatureGenerateModel.SetCreature();
						}
						catch (CreatureGenerateInfoManager.ProbCheckExeption probCheckExeption2)
						{
							Debug.LogError("Failed To Gen Creature");
						}
					}
				}
				return creatureGenerateModel.creature;
			}
			return null;
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x001846C8 File Offset: 0x001828C8
		public void CalculateDay()
		{
			int day = PlayerModel.instance.GetDay();
			int num = CreatureManager.instance.GetCreatureList().Length;
			this._genDay = day;
			int num2 = day - day / 5;
			if (day >= 20 && day < 25)
			{
				int num3 = day - 20;
				num2 += num3;
			}
			else if (day >= 25)
			{
				num2 += 4;
			}
			if (day % 5 == 4)
			{
				this._genDay++;
			}
			else if (num > num2)
			{
				this._genDay++;
			}
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00184758 File Offset: 0x00182958
		public void OnUsed(long id)
		{
			CreatureTypeInfo data = CreatureTypeList.instance.GetData(id);
			ActivateStateList activateStateList = null;
			if (this.activateStateDic.TryGetValue(data.GetRiskLevel(), out activateStateList))
			{
				activateStateList.OnUsed(id);
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00184794 File Offset: 0x00182994
		public bool CheckKitCreatureRemains()
		{
			long[] kitCreature = CreatureGenerateInfo.kitCreature;
			List<long> list = new List<long>(kitCreature);
			List<CreatureModel> list2 = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
			foreach (CreatureModel creatureModel in list2)
			{
				long metadataId = creatureModel.metadataId;
				if (list.Contains(metadataId))
				{
					list.Remove(metadataId);
				}
			}
			return list.Count > 0;
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00036790 File Offset: 0x00034990
		static CreatureGenerateInfoManager()
		{
		}

		// Token: 0x04003908 RID: 14600
		private const string DebugPrefix = "<color=#FF2323>[CreatureGenerate]</color> ";

		// Token: 0x04003909 RID: 14601
		private const string XMLFileSrc = "xml/CreatureGenInfo";

		// Token: 0x0400390A RID: 14602
		private static CreatureGenerateInfoManager _instance = null;

		// Token: 0x0400390B RID: 14603
		public static readonly string[] GenerateCommonActionString = new string[]
		{
			"remove",
			"only"
		};

		// Token: 0x0400390C RID: 14604
		public List<string> GenerateCommonActionList = new List<string>(CreatureGenerateInfoManager.GenerateCommonActionString);

		// Token: 0x0400390D RID: 14605
		public Dictionary<RiskLevel, ActivateStateList> activateStateDic = new Dictionary<RiskLevel, ActivateStateList>();

		// Token: 0x0400390E RID: 14606
		public Dictionary<int, CreatureGenerateModel> dayGenInfoDic = new Dictionary<int, CreatureGenerateModel>();

		// Token: 0x0400390F RID: 14607
		public Dictionary<RiskLevel, List<long>> CreatureList = new Dictionary<RiskLevel, List<long>>();

		// Token: 0x04003910 RID: 14608
		public Dictionary<int, CreatureSelectData> SelectData = new Dictionary<int, CreatureSelectData>();

		// Token: 0x04003911 RID: 14609
		private bool _isLoadedDayData;

		// Token: 0x04003912 RID: 14610
		private bool _isInitiated;

		// Token: 0x04003913 RID: 14611
		private int _genDay;

		// Token: 0x04003914 RID: 14612
		private bool _genKit;

		// Token: 0x020007E4 RID: 2020
		public class ProbCheckExeption : Exception
		{
			// Token: 0x06003E68 RID: 15976 RVA: 0x0001E1A2 File Offset: 0x0001C3A2
			public ProbCheckExeption()
			{
			}
		}
	}
}
