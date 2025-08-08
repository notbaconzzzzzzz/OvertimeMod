using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ControlGroupManager
{
	public ControlGroupManager()
	{
		_controlGroups = new ControlGroup[16];
		for (int i = 0; i < 16; i++)
		{
			_controlGroups[i] = new ControlGroup();
		}
	}

	public void SaveControlGroup(int index, List<AgentModel> agents)
	{
		if (index < 0 || index >= _controlGroups.Length)
		{
			return;
		}
		_controlGroups[index].SaveAgents(agents);
		SaveDataFile();
	}

	public List<UnitMouseEventTarget> GetControlGroupTargets(int index)
	{
		List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
		if (index < 0)
		{
			return list;
		}
		if (index < _controlGroups.Length)
		{
			foreach (long id in _controlGroups[index].GetIDs())
			{
				AgentModel agent = AgentManager.instance.GetAgent(id);
				if (agent != null)
				{
					UnitMouseEventTarget unitMouseTarget = agent.GetUnitMouseTarget();
					if (!(unitMouseTarget == null) && unitMouseTarget.IsSelectable())
					{
						list.Add(unitMouseTarget);
					}
				}
			}
			return list;
		}
		if (index < 16 || index >= 20)
		{
			return list;
		}
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			bool flag = false;
			AgentAIState state = agentModel.GetState();
			if (index == 16)
			{
				flag = true;
			}
			if (index == 17)
			{
				flag = (agentModel.currentSkill != null || state == AgentAIState.MANAGE || state == AgentAIState.OBSERVE || state == AgentAIState.RETURN_CREATURE);
			}
			if (index == 18)
			{
				flag = (state == AgentAIState.SUPPRESS_CREATURE || state == AgentAIState.SUPPRESS_WORKER || state == AgentAIState.SUPPRESS_OBJECT);
			}
			if (index == 19)
			{
				flag = (state == AgentAIState.IDLE);
			}
			if (flag)
			{
				UnitMouseEventTarget unitMouseTarget2 = agentModel.GetUnitMouseTarget();
				if (!(unitMouseTarget2 == null) && unitMouseTarget2.IsSelectable())
				{
					list.Add(unitMouseTarget2);
				}
			}
		}
		return list;
	}

	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		for (int i = 0; i < 7; i++)
		{
			data.Add(i.ToString(), _controlGroups[i].GetSaveData());
		}
		return data;
	}

	public void LoadData(Dictionary<string, object> data)
	{
		for (int i = 0; i < 7; i++)
		{
			List<long> data2 = null;
			if (GameUtil.TryGetValue<List<long>>(data, i.ToString(), ref data2))
			{
				_controlGroups[i].LoadData(data2);
			}
		}
	}

	public static ControlGroupManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ControlGroupManager();
				_instance.LoadDataFile();
			}
			return _instance;
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
			Dictionary<string, object> data = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			LoadData(data);
		}
	}

	public void ResetData()
	{
		for (int i = 0; i < _controlGroups.Length; i++)
		{
			_controlGroups[i].Clear();
		}
		SaveDataFile();
	}

	public string dataPath
	{
		get
		{
			return Application.persistentDataPath + "/ControlGroups.dat";
		}
	}

	public void ResetUpperGroups()
	{
		for (int i = 7; i < _controlGroups.Length; i++)
		{
			_controlGroups[i].Clear();
		}
	}

	private static ControlGroupManager _instance;

	private ControlGroup[] _controlGroups;

	public class ControlGroup
	{
		public ControlGroup()
		{
		}

		public void SaveAgents(List<AgentModel> agents)
		{
			agentIDs.Clear();
			foreach (AgentModel agentModel in agents)
			{
				agentIDs.Add(agentModel.instanceId);
			}
		}

		public List<long> GetIDs()
		{
			return agentIDs;
		}

		public List<long> GetSaveData()
		{
			return agentIDs;
		}

		public void LoadData(List<long> data)
		{
			agentIDs = data;
		}

		public void Clear()
		{
			agentIDs.Clear();
		}

		private List<long> agentIDs = new List<long>();
	}
}
