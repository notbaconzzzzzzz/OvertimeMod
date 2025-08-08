using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x0200051F RID: 1311
public class EquipmentDataLoader
{
	// Token: 0x06002EB2 RID: 11954 RVA: 0x0013E194 File Offset: 0x0013C394
	public void Load()
	{
        try
        {
            LobotomyBaseMod.ModDebug.Log("EDL Load 1");
            XmlDocument xmlDocument = new XmlDocument();
            if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt"))
            {
                TextAsset textAsset = Resources.Load<TextAsset>("xml/Equipment/Equipment");
                File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt", textAsset.text);
            }
            string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt");
            xmlDocument.LoadXml(xml);
            Dictionary<int, EquipmentTypeInfo> dictionary = this.LoadEquips(xmlDocument);
            Dictionary<string, Dictionary<int, EquipmentTypeInfo>> dictionary2 = new Dictionary<string, Dictionary<int, EquipmentTypeInfo>>();
            LobotomyBaseMod.ModDebug.Log("EDL Load 2");
            foreach (ModInfo modInfo_patch in Add_On.instance.ModList)
            {
                DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(modInfo_patch.modpath, "Equipment");
                if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/txts"))
                {
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/txts");
                    if (directoryInfo2.GetFiles().Length != 0)
                    {
                        if (modInfo_patch.modid == string.Empty)
                        {
                            foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
                            {
                                if (fileInfo.Name.Contains(".txt") || fileInfo.Name.Contains(".xml"))
                                {
                                    XmlDocument xmlDocument2 = new XmlDocument();
                                    xmlDocument2.LoadXml(File.ReadAllText(fileInfo.FullName));
                                    foreach (KeyValuePair<int, EquipmentTypeInfo> keyValuePair in LoadEquips(xmlDocument2))
                                    {
                                        if (dictionary.ContainsKey(keyValuePair.Key))
                                        {
                                            dictionary.Remove(keyValuePair.Key);
                                        }
                                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Dictionary<int, EquipmentTypeInfo> dictionary3 = new Dictionary<int, EquipmentTypeInfo>();
                            foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles())
                            {
                                if (fileInfo2.Name.Contains(".txt") || fileInfo2.Name.Contains(".xml"))
                                {
                                    XmlDocument xmlDocument3 = new XmlDocument();
                                    xmlDocument3.LoadXml(File.ReadAllText(fileInfo2.FullName));
                                    foreach (KeyValuePair<int, EquipmentTypeInfo> keyValuePair2 in LoadEquips(xmlDocument3))
                                    {
                                        keyValuePair2.Value.modid = modInfo_patch.modid;
                                        if (dictionary3.ContainsKey(keyValuePair2.Key))
                                        {
                                            dictionary3.Remove(keyValuePair2.Key);
                                        }
                                        dictionary3.Add(keyValuePair2.Key, keyValuePair2.Value);
                                    }
                                }
                            }
                            dictionary2[modInfo_patch.modid] = dictionary3;
                        }
                    }
                }
            }
            LobotomyBaseMod.ModDebug.Log("EDL Load 3");
            EquipmentTypeList.instance.Init(dictionary);
            EquipmentTypeList.instance.Init_Mod(dictionary2);
            LobotomyBaseMod.ModDebug.Log("EDL Load 4");
        }
        catch (Exception ex)
        {
            LobotomyBaseMod.ModDebug.Log("EDL Load Error - " + ex.Message + Environment.NewLine + ex.StackTrace);
        }
        /*
        XmlDocument xmlDocument = new XmlDocument();
        if (!File.Exists(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt"))
        {
            TextAsset textAsset = Resources.Load<TextAsset>("xml/Equipment/Equipment");
            File.WriteAllText(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt", textAsset.text);
        }
        string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/BaseEquipment.txt");
        xmlDocument.LoadXml(xml);
        Dictionary<int, EquipmentTypeInfo> dictionary = this.LoadEquips(xmlDocument);
		foreach (DirectoryInfo dir in Add_On.instance.DirList)
		{
			DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Equipment");
			if (directoryInfo != null && Directory.Exists(directoryInfo.FullName + "/txts"))
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/txts");
				if (directoryInfo2.GetFiles().Length != 0)
				{
					foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
					{
						if (fileInfo.Name.Contains(".txt"))
						{
							XmlDocument xmlDocument2 = new XmlDocument();
							xmlDocument2.LoadXml(File.ReadAllText(fileInfo.FullName));
							new Dictionary<int, EquipmentTypeInfo>();
							foreach (KeyValuePair<int, EquipmentTypeInfo> keyValuePair in this.LoadEquips(xmlDocument2))
							{
								if (dictionary.ContainsKey(keyValuePair.Key))
								{
									dictionary.Remove(keyValuePair.Key);
								}
								dictionary.Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
			}
		}
		EquipmentTypeList.instance.Init(dictionary);*/
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x0013E394 File Offset: 0x0013C594
	public EquipmentTypeInfo LoadWeapon(EquipmentTypeInfo info, XmlNode equipNode)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		IEnumerator enumerator = equipNode.SelectNodes("damage").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode damageNode = (XmlNode)obj;
				list.Add(EquipmentDataLoader.ConvertToDamageInfo(damageNode));
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
		if (list.Count > 0)
		{
			info.damageInfos = list.ToArray();
		}
		List<string> list2 = new List<string>();
		IEnumerator enumerator2 = equipNode.SelectNodes("animation").GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				XmlNode xmlNode = (XmlNode)obj2;
				list2.Add(xmlNode.InnerText);
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
		if (list2.Count > 0)
		{
			info.animationNames = list2.ToArray();
		}
		XmlNode xmlNode2 = equipNode.SelectSingleNode("weaponClassType");
		if (xmlNode2 != null)
		{
			info.weaponClassType = EquipmentDataLoader.ConvertToWeaponClassType(xmlNode2.InnerText);
		}
		XmlNode xmlNode3 = equipNode.SelectSingleNode("sprite");
		if (xmlNode3 != null)
		{
			info.sprite = xmlNode3.InnerText;
		}
		XmlNode xmlNode4 = equipNode.SelectSingleNode("specialWeaponAnim");
		if (xmlNode4 != null)
		{
			info.specialWeaponAnim = xmlNode4.InnerText;
		}
		XmlNode xmlNode5 = equipNode.SelectSingleNode("range");
		info.range = float.Parse(xmlNode5.InnerText);
		XmlNode xmlNode6 = equipNode.SelectSingleNode("splash");
		if (xmlNode6 != null)
		{
			XmlNode namedItem = xmlNode6.Attributes.GetNamedItem("type");
			if (namedItem != null)
			{
				string innerText = namedItem.InnerText;
				if (innerText != null)
				{
					if (!(innerText == "splash"))
					{
						if (innerText == "penetration")
						{
							info.splashInfo.type = SplashType.PENETRATION;
						}
					}
					else
					{
						info.splashInfo.type = SplashType.SPLASH;
						info.splashInfo.range = float.Parse(xmlNode6.Attributes.GetNamedItem("range").InnerText);
					}
				}
				XmlNode namedItem2 = xmlNode6.Attributes.GetNamedItem("iff");
				if (namedItem2 != null)
				{
					info.splashInfo.iff = bool.Parse(namedItem2.InnerText.Trim());
				}
			}
		}
		XmlNode xmlNode7 = equipNode.SelectSingleNode("attackSpeed");
		if (xmlNode7 != null)
		{
			info.attackSpeed = float.Parse(xmlNode7.InnerText);
		}
		return info;
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x0013E5F4 File Offset: 0x0013C7F4
	public EquipmentTypeInfo LoadArmor(EquipmentTypeInfo info, XmlNode equipNode)
	{
		XmlNode xmlNode = equipNode.SelectSingleNode("defense");
		if (xmlNode != null)
		{
			IEnumerator enumerator = xmlNode.SelectNodes("defenseElement").GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode2 = (XmlNode)obj;
					string innerText = xmlNode2.Attributes.GetNamedItem("type").InnerText;
					if (innerText != null)
					{
						if (!(innerText == "R"))
						{
							if (!(innerText == "W"))
							{
								if (!(innerText == "B"))
								{
									if (innerText == "P")
									{
										info.defenseInfo.P = float.Parse(xmlNode2.InnerText);
									}
								}
								else
								{
									info.defenseInfo.B = float.Parse(xmlNode2.InnerText);
								}
							}
							else
							{
								info.defenseInfo.W = float.Parse(xmlNode2.InnerText);
							}
						}
						else
						{
							info.defenseInfo.R = float.Parse(xmlNode2.InnerText);
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
		}
		XmlNode xmlNode3 = equipNode.SelectSingleNode("armorId");
		if (xmlNode3 != null)
		{
			info.armorId = int.Parse(xmlNode3.InnerText);
		}
		XmlNode xmlNode4 = equipNode.SelectSingleNode("attackSpeed");
		if (xmlNode4 != null)
		{
			info.attackSpeed = float.Parse(xmlNode4.InnerText);
		}
		return info;
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x0013E760 File Offset: 0x0013C960
	public void LoadEGOgift(EquipmentTypeInfo info, XmlNode equipNode)
	{
		XmlNode xmlNode = equipNode.SelectSingleNode("attachPos");
		info.attachPos = xmlNode.InnerText;
		string innerText = equipNode.SelectSingleNode("attachType").InnerText;
		if (innerText != null)
		{
			if (!(innerText == "add"))
			{
				if (!(innerText == "replace"))
				{
					if (innerText == "special_add")
					{
						info.attachType = EGOgiftAttachType.SPECIAL_ADD;
					}
				}
				else
				{
					info.attachType = EGOgiftAttachType.REPLACE;
				}
			}
			else
			{
				info.attachType = EGOgiftAttachType.ADD;
			}
		}
		XmlNode xmlNode2 = equipNode.SelectSingleNode("sprite");
		if (xmlNode2 != null)
		{
			info.sprite = xmlNode2.InnerText;
		}
		info.bonus = new EGObonusInfo();
		XmlNode xmlNode3 = equipNode.SelectSingleNode("bonus");
		if (xmlNode3 != null)
		{
			XmlNode xmlNode4 = xmlNode3.SelectSingleNode("hp");
			if (xmlNode4 != null)
			{
				info.bonus.hp = int.Parse(xmlNode4.InnerText);
			}
			XmlNode xmlNode5 = xmlNode3.SelectSingleNode("mental");
			if (xmlNode5 != null)
			{
				info.bonus.mental = int.Parse(xmlNode5.InnerText);
			}
			XmlNode xmlNode6 = xmlNode3.SelectSingleNode("cubeSpeed");
			if (xmlNode6 != null)
			{
				info.bonus.cubeSpeed = int.Parse(xmlNode6.InnerText);
			}
			XmlNode xmlNode7 = xmlNode3.SelectSingleNode("workProb");
			if (xmlNode7 != null)
			{
				info.bonus.workProb = int.Parse(xmlNode7.InnerText);
			}
			XmlNode xmlNode8 = xmlNode3.SelectSingleNode("movement");
			if (xmlNode8 != null)
			{
				info.bonus.movement = int.Parse(xmlNode8.InnerText);
			}
			XmlNode xmlNode9 = xmlNode3.SelectSingleNode("attackSpeed");
			if (xmlNode9 != null)
			{
				info.bonus.attackSpeed = int.Parse(xmlNode9.InnerText);
			}
		}
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x0013E904 File Offset: 0x0013CB04
	public static WeaponClassType ConvertToWeaponClassType(string text)
	{
		switch (text)
		{
			case "mace":
				return WeaponClassType.MACE;
			case "axe":
				return WeaponClassType.AXE;
			case "knife":
				return WeaponClassType.KNIFE;
			case "pistol":
				return WeaponClassType.PISTOL;
			case "bowgun":
				return WeaponClassType.BOWGUN;
			case "spear":
				return WeaponClassType.SPEAR;
			case "rifle":
				return WeaponClassType.RIFLE;
			case "fist":
				return WeaponClassType.FIST;
			case "cannon":
				return WeaponClassType.CANNON;
			case "hammer":
				return WeaponClassType.HAMMER;
			case "officer":
				return WeaponClassType.OFFICER;
			case "special":
				return WeaponClassType.SPECIAL;
		}
		return WeaponClassType.NONE;
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x0013EAB4 File Offset: 0x0013CCB4
	public static DamageInfo ConvertToDamageInfo(XmlNode damageNode)
	{
		RwbpType type = EquipmentDataLoader.ConvertToRWBP(damageNode.Attributes.GetNamedItem("type").InnerText);
		int min = int.Parse(damageNode.Attributes.GetNamedItem("min").InnerText);
		int max = int.Parse(damageNode.Attributes.GetNamedItem("max").InnerText);
		DamageInfo damageInfo = new DamageInfo(type, min, max);
		XmlNode xmlNode = damageNode.SelectSingleNode("soundInfo");
		if (xmlNode != null)
		{
			SoundInfo soundInfo = new SoundInfo();
			string soundSrc = xmlNode.InnerText.Trim();
			if (xmlNode.Attributes.GetNamedItem("type").InnerText == "damage")
			{
				soundInfo.soundType = DamageInfo_EffectType.DAMAGE_INVOKED;
			}
			else
			{
				soundInfo.soundType = DamageInfo_EffectType.ANIM_START;
			}
			soundInfo.soundSrc = soundSrc;
			damageInfo.soundInfo = soundInfo;
		}
		List<EffectInfo> list = new List<EffectInfo>();
		IEnumerator enumerator = damageNode.SelectNodes("effectInfo").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode2 = (XmlNode)obj;
				EffectInfo effectInfo = new EffectInfo();
				if (xmlNode2.Attributes.GetNamedItem("type").InnerText == "damage")
				{
					effectInfo.effectType = DamageInfo_EffectType.DAMAGE_INVOKED;
				}
				else
				{
					effectInfo.effectType = DamageInfo_EffectType.ANIM_START;
				}
				XmlNode xmlNode3 = xmlNode2.SelectSingleNode("attach");
				if (xmlNode3 != null && xmlNode3.InnerText.Trim().ToLower() == "victim")
				{
					effectInfo.invokedUnit = EffectInvokedUnit.VICTIM;
				}
				else
				{
					effectInfo.invokedUnit = EffectInvokedUnit.OWNER;
				}
				XmlNode xmlNode4 = xmlNode2.SelectSingleNode("src");
				if (xmlNode4 != null)
				{
					effectInfo.effectSrc = xmlNode4.InnerText.Trim();
				}
				else
				{
					Debug.LogError("Error in Damage effect");
				}
				XmlNode xmlNode5 = xmlNode2.SelectSingleNode("pos");
				if (xmlNode5 != null)
				{
					float x = 0f;
					float y = 0f;
					float z = 0f;
					XmlNode xmlNode6 = xmlNode5.SelectSingleNode("x");
					XmlNode xmlNode7 = xmlNode5.SelectSingleNode("y");
					XmlNode xmlNode8 = xmlNode5.SelectSingleNode("z");
					if (xmlNode6 != null)
					{
						x = float.Parse(xmlNode6.InnerText.Trim());
					}
					if (xmlNode7 != null)
					{
						y = float.Parse(xmlNode7.InnerText.Trim());
					}
					if (xmlNode8 != null)
					{
						z = float.Parse(xmlNode8.InnerText.Trim());
					}
					effectInfo.relativePosition = new Vector3(x, y, z);
				}
				XmlNode xmlNode9 = xmlNode2.SelectSingleNode("rot");
				if (xmlNode9 != null)
				{
					float rotation = float.Parse(xmlNode9.InnerText);
					effectInfo.rotation = rotation;
				}
				XmlNode xmlNode10 = xmlNode2.SelectSingleNode("lifetime");
				if (xmlNode10 != null)
				{
					float lifetime = float.Parse(xmlNode10.InnerText);
					effectInfo.lifetime = lifetime;
				}
				XmlNode xmlNode11 = xmlNode2.SelectSingleNode("unscaled");
				if (xmlNode11 != null)
				{
					bool unscaled = bool.Parse(xmlNode11.InnerText.Trim());
					effectInfo.unscaled = unscaled;
				}
				XmlNode xmlNode12 = xmlNode2.SelectSingleNode("invokeOnce");
				if (xmlNode12 != null)
				{
					bool invokeOnce = bool.Parse(xmlNode12.InnerText.Trim());
					effectInfo.invokeOnce = invokeOnce;
				}
				list.Add(effectInfo);
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
		damageInfo.effectInfos = list;
		damageInfo.effectInfo = null;
		return damageInfo;
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x0013BF10 File Offset: 0x0013A110
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

	// Token: 0x06002EB9 RID: 11961 RVA: 0x0013EE00 File Offset: 0x0013D000
	public Dictionary<int, EquipmentTypeInfo> LoadEquips(XmlDocument document)
	{
		Dictionary<int, EquipmentTypeInfo> dictionary = new Dictionary<int, EquipmentTypeInfo>();
		IEnumerator enumerator = document.SelectSingleNode("equipment_list").SelectNodes("equipment").GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				EquipmentTypeInfo equipmentTypeInfo = new EquipmentTypeInfo();
				int num = int.Parse(xmlNode.Attributes.GetNamedItem("id").InnerText);
				string innerText = xmlNode.Attributes.GetNamedItem("type").InnerText;
				equipmentTypeInfo.id = num;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("name");
				if (xmlNode2 != null)
				{
					equipmentTypeInfo.localizeData.Add("name", xmlNode2.InnerText.Trim());
				}
				else
				{
					equipmentTypeInfo.localizeData.Add("name", num + "name");
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("no");
				if (xmlNode3 != null)
				{
					equipmentTypeInfo.localizeData.Add("no", xmlNode3.InnerText.Trim());
					equipmentTypeInfo.no = xmlNode3.InnerText;
				}
				else
				{
					equipmentTypeInfo.no = num.ToString();
				}
				XmlNode xmlNode4 = xmlNode.SelectSingleNode("desc");
				if (xmlNode4 != null)
				{
					equipmentTypeInfo.localizeData.Add("desc", xmlNode4.InnerText.Trim());
				}
				XmlNode xmlNode5 = xmlNode.SelectSingleNode("specialDesc");
				if (xmlNode5 != null)
				{
					equipmentTypeInfo.localizeData.Add("specialDesc", xmlNode5.InnerText.Trim());
				}
				XmlNode xmlNode6 = xmlNode.SelectSingleNode("script");
				if (xmlNode6 != null)
				{
					equipmentTypeInfo.script = xmlNode6.InnerText;
				}
				equipmentTypeInfo.requires = new List<EgoRequire>();
				IEnumerator enumerator2 = xmlNode.SelectNodes("require").GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode7 = (XmlNode)obj2;
						string innerText2 = xmlNode7.Attributes.GetNamedItem("type").InnerText;
						int value = int.Parse(xmlNode7.InnerText);
						EgoRequire egoRequire = new EgoRequire();
						if (innerText2 == "R")
						{
							egoRequire.type = EgoRequireType.R;
							egoRequire.value = value;
						}
						if (innerText2 == "W")
						{
							egoRequire.type = EgoRequireType.W;
							egoRequire.value = value;
						}
						if (innerText2 == "B")
						{
							egoRequire.type = EgoRequireType.B;
							egoRequire.value = value;
						}
						if (innerText2 == "P")
						{
							egoRequire.type = EgoRequireType.P;
							egoRequire.value = value;
						}
						if (innerText2 == "level")
						{
							egoRequire.type = EgoRequireType.level;
							egoRequire.value = value;
						}
						equipmentTypeInfo.requires.Add(egoRequire);
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
				XmlNode xmlNode8 = xmlNode.SelectSingleNode("maxNum");
				if (xmlNode8 != null)
				{
					equipmentTypeInfo.maxNum = int.Parse(xmlNode8.InnerText);
				}
				XmlNode xmlNode9 = xmlNode.SelectSingleNode("grade");
				if (xmlNode9 != null)
				{
					equipmentTypeInfo.grade = xmlNode9.InnerText.Trim();
				}
				try
				{
					if (innerText != null)
					{
						if (!(innerText == "weapon"))
						{
							if (!(innerText == "armor"))
							{
								if (innerText == "special")
								{
									equipmentTypeInfo.type = EquipmentTypeInfo.EquipmentType.SPECIAL;
									this.LoadEGOgift(equipmentTypeInfo, xmlNode);
									dictionary.Add(num, equipmentTypeInfo);
								}
							}
							else
							{
								equipmentTypeInfo.type = EquipmentTypeInfo.EquipmentType.ARMOR;
								this.LoadArmor(equipmentTypeInfo, xmlNode);
								dictionary.Add(num, equipmentTypeInfo);
							}
						}
						else
						{
							equipmentTypeInfo.type = EquipmentTypeInfo.EquipmentType.WEAPON;
							this.LoadWeapon(equipmentTypeInfo, xmlNode);
							dictionary.Add(num, equipmentTypeInfo);
						}
					}
				}
				catch (ArgumentException arg)
				{
					Debug.LogError(num + " " + arg);
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
		return dictionary;
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x0013F204 File Offset: 0x0013D404
	public static DirectoryInfo CheckNamedDir(DirectoryInfo dir, string name)
	{
		foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
		{
			if (directoryInfo.Name == name)
			{
				return directoryInfo;
			}
		}
		return null;
	}
}
