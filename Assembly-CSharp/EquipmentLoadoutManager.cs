using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EquipmentLoadoutManager
{
    public EquipmentLoadoutManager()
    {
        _loadouts = new Loadout[10];
        for (int i = 0; i < 10; i++)
        {
            _loadouts[i] = new Loadout();
        }
    }

    public static EquipmentLoadoutManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EquipmentLoadoutManager();
                _instance.LoadDataFile();
            }
            return _instance;
        }
    }

    public string dataPath
    {
        get
        {
            return Application.persistentDataPath + "/EquipmentLoadouts.dat";
        }
    }

    public void SaveDataFile()
    {
        SaveUtil.WriteSerializableFile(dataPath, GetSaveData());
    }

    public void LoadDataFile()
    {
        if (File.Exists(dataPath))
        {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(dataPath, FileMode.Open);
			Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
            LoadData(dic);
        }
    }

    public void ResetData()
    {
		for (int i = 0; i < 10; i++)
		{
			_loadouts[i].Clear();
		}
        SaveDataFile();
    }

    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
		for (int i = 0; i < 7; i++)
		{
			data.Add(i.ToString(), _loadouts[i].GetSaveData());
		}
        return data;
    }

    public void LoadData(Dictionary<string, object> data)
    {
        for (int i = 0; i < 7; i++)
        {
            Dictionary<string, object> data2 = null;
            if (GameUtil.TryGetValue<Dictionary<string, object>>(data, i.ToString(), ref data2))
            {
                _loadouts[i].LoadData(data2);
            }
        }
    }

	public void Load(int index)
	{
		if (index < 0 || index >= _loadouts.Length) return;
        try
        {
            _loadouts[index].Load();
        }
        catch (Exception ex)
        {
            UnityEngine.GUIUtility.systemCopyBuffer = "Load error " + index + " : " + ex.Message + " : " + ex.StackTrace;
        }
	}

	public void Save(int index)
	{
		if (index < 0 || index >= _loadouts.Length) return;
        try
        {
		    _loadouts[index].Save();
        }
        catch (Exception ex)
        {
			UnityEngine.GUIUtility.systemCopyBuffer = "Save error " + index + " : " + ex.Message + " : " + ex.StackTrace;
        }
		SaveDataFile();
	}

    private static EquipmentLoadoutManager _instance;

    private Loadout[] _loadouts;

    public class Loadout
    {
        public void Load()
        {
            string str = "";
            List<EquipmentModel> equipList = InventoryModel.Instance.equipList;
			List<EquipData> remainEquip = new List<EquipData>();
			List<EquipData> remainAgent = new List<EquipData>();
			foreach (EquipData equip in agentList)
			{
				AgentModel agent = AgentManager.instance.GetAgent(equip.agentInstance);
				if (agent == null)
				{
					agent = AgentManager.instance.GetSpareAgent(equip.agentInstance);
					if (agent == null)
					{
                        str += "Ra" + equip.agentInstance;
					    remainAgent.Add(equip);
						continue;
					}
				}
				EquipmentModel model = equipList.Find((EquipmentModel x) => x.instanceId == equip.equipmentInstance);
                if (model == null)
                {
                    str += "Re" + equip.agentInstance;
					EquipmentTypeInfo typeInfo = EquipmentTypeList.instance.GetData_Mod(new LobotomyBaseMod.LcId(equip.equipmentModId, equip.equipmentId));
					if (typeInfo == null) continue;
					if (typeInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
					{
						agent.ReleaseWeaponV2();
					}
					if (typeInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
					{
						agent.ReleaseArmor();
					}
                    remainEquip.Add(equip);
					continue;
                }
                if (!model.CheckRequire(agent)) continue;
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
				{
					if (model.owner != null)
					{
						model.owner.ReleaseWeaponV2();
					}
                    str += "Ew" + equip.agentInstance;
					agent.SetWeapon(model as WeaponModel);
				}
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
				{
					if (model.owner != null)
					{
						model.owner.ReleaseArmor();
					}
                    str += "Ea" + equip.agentInstance;
					agent.SetArmor(model as ArmorModel);
				}
			}
            int num = remainEquip.Count;
            str += "\nNext\n";
			foreach (EquipData equip in remainAgent)
			{
				AgentModel agent = null;
				foreach (AgentModel agent2 in AgentManager.instance.GetAgentList())
				{
					if (agent2.name == equip.agentName)
					{
						agent = agent2;
						break;
					}
				}
                if (agent == null) continue;
				EquipmentModel model = equipList.Find((EquipmentModel x) => x.instanceId == equip.equipmentInstance);
                if (model == null)
                {
                    str += "Re" + equip.agentInstance;
					EquipmentTypeInfo typeInfo = EquipmentTypeList.instance.GetData_Mod(new LobotomyBaseMod.LcId(equip.equipmentModId, equip.equipmentId));
					if (typeInfo == null) continue;
					if (typeInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
					{
						agent.ReleaseWeaponV2();
					}
					if (typeInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
					{
						agent.ReleaseArmor();
					}
                    remainEquip.Add(equip);
					continue;
                }
                if (!model.CheckRequire(agent)) continue;
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
				{
					if (model.owner != null)
					{
						model.owner.ReleaseWeaponV2();
					}
                    str += "Ew" + equip.agentInstance;
					agent.SetWeapon(model as WeaponModel);
				}
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
				{
					if (model.owner != null)
					{
						model.owner.ReleaseArmor();
					}
                    str += "Ea" + equip.agentInstance;
					agent.SetArmor(model as ArmorModel);
				}
			}
            str += "\nNext\n";
            int num2 = 0;
			foreach (EquipData equip in remainEquip)
			{
                AgentModel agent = null;
                if (num2++ < num)
                {
                    agent = AgentManager.instance.GetAgent(equip.agentInstance);
                    if (agent == null)
                    {
                        agent = AgentManager.instance.GetSpareAgent(equip.agentInstance);
                        if (agent == null)
                        {
                            str += "Anf!";
                            continue;
                        }
                    }
                }
                else
                {
					foreach (AgentModel agent2 in AgentManager.instance.GetAgentList())
					{
						if (agent2.name == equip.agentName)
						{
							agent = agent2;
							break;
						}
					}
                }
				EquipmentModel model = equipList.Find((EquipmentModel x) => x.metaInfo.id == equip.equipmentId && x.metaInfo.modid == equip.equipmentModId && x.owner == null);
                if (model == null)
                {
                    str += "Enf!";
					continue;
                }
                if (!model.CheckRequire(agent)) continue;
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
				{
                    str += "Ew" + equip.agentInstance;
					agent.SetWeapon(model as WeaponModel);
				}
                if (model.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
				{
                    str += "Ea" + equip.agentInstance;
					agent.SetArmor(model as ArmorModel);
				}
			}
            //UnityEngine.GUIUtility.systemCopyBuffer = str;
        }

        public void Save()
        {
            string str = "";
            Clear();
			foreach (AgentModel agent in AgentManager.instance.GetAgentList())
			{
				WeaponModel weapon = agent.Equipment.weapon;
				ArmorModel armor = agent.Equipment.armor;
                if (weapon != null && weapon.metaInfo.LcId != 1)
				{
                    str += agent.instanceId + "," + weapon.metaInfo.id + "\n";
					agentList.Add(new EquipData(agent.instanceId, agent.name, weapon.instanceId, weapon.metaInfo.id, weapon.metaInfo.modid));
				}
                if (armor != null && armor.metaInfo.LcId != 22)
				{
                    str += agent.instanceId + "," + armor.metaInfo.id + "\n";
					agentList.Add(new EquipData(agent.instanceId, agent.name, armor.instanceId, armor.metaInfo.id, armor.metaInfo.modid));
				}
			}
			foreach (AgentModel agent in AgentManager.instance.GetAgentSpareList())
			{
				WeaponModel weapon = agent.Equipment.weapon;
				ArmorModel armor = agent.Equipment.armor;
                if (weapon != null && weapon.metaInfo.LcId != 1)
				{
                    str += agent.instanceId + "," + weapon.metaInfo.id + "\n";
					agentList.Add(new EquipData(agent.instanceId, agent.name, weapon.instanceId, weapon.metaInfo.id, weapon.metaInfo.modid));
				}
                if (armor != null && armor.metaInfo.LcId != 22)
				{
                    str += agent.instanceId + "," + armor.metaInfo.id + "\n";
					agentList.Add(new EquipData(agent.instanceId, agent.name, armor.instanceId, armor.metaInfo.id, armor.metaInfo.modid));
				}
			}
            //UnityEngine.GUIUtility.systemCopyBuffer = str;
        }

        public void LoadData(Dictionary<string, object> data)
        {
			List<long> list1 = null;
			List<string> list2 = null;
			List<long> list3 = null;
			List<int> list4 = null;
			List<string> list5 = null;
			if (GameUtil.TryGetValue<List<long>>(data, "agentInstance", ref list1) && GameUtil.TryGetValue<List<string>>(data, "agentName", ref list2) && GameUtil.TryGetValue<List<long>>(data, "equipmentInstance", ref list3) && GameUtil.TryGetValue<List<int>>(data, "equipmentId", ref list4))
			{
                if (GameUtil.TryGetValue<List<string>>(data, "equipmentModId", ref list5))
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        agentList.Add(new EquipData(list1[i], list2[i], list3[i], list4[i], list5[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        agentList.Add(new EquipData(list1[i], list2[i], list3[i], list4[i]));
                    }
                }
			}
        }

        public Dictionary<string, object> GetSaveData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            List<long> list1 = new List<long>();
            List<string> list2 = new List<string>();
            List<long> list3 = new List<long>();
            List<int> list4 = new List<int>();
            List<string> list5 = new List<string>();
            foreach (EquipData equip in agentList)
            {
                list1.Add(equip.agentInstance);
                list2.Add(equip.agentName);
                list3.Add(equip.equipmentInstance);
                list4.Add(equip.equipmentId);
                list5.Add(equip.equipmentModId);
            }
            data.Add("agentInstance", list1);
            data.Add("agentName", list2);
            data.Add("equipmentInstance", list3);
            data.Add("equipmentId", list4);
            data.Add("equipmentModId", list5);
            return data;
        }

        public void Clear()
        {
            agentList.Clear();
        }

        private List<EquipData> agentList = new List<EquipData>();
		
		public class EquipData
		{
			public EquipData()
			{

			}

			public EquipData(long a, string b, long c, int d)
			{
				agentInstance = a;
				agentName = b;
				equipmentInstance = c;
				equipmentId = d;
                equipmentModId = "";
			}

			public EquipData(long a, string b, long c, int d, string e)
			{
				agentInstance = a;
				agentName = b;
				equipmentInstance = c;
				equipmentId = d;
                equipmentModId = e;
			}

            public long agentInstance;

            public string agentName;

            public long equipmentInstance;

            public int equipmentId;

            public string equipmentModId;
		}
    }
}