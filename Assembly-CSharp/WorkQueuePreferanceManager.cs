using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WorkQueuePreferanceManager
{
	public WorkQueuePreferanceManager()
	{

	}

	public QueuedWorkOrder.QueueConditionInfo GetDefaultCondition(CreatureModel creature)
    {
		QueuedWorkOrder.QueueConditionInfo value;
        if (!customConditions.TryGetValue(creature.metaInfo.LcId, out value))
		{
			value = defaultCondition;
		}
		return value.Copy();
    }

	public void SaveDefaultCondition(QueuedWorkOrder.QueueConditionInfo condition, CreatureModel creature = null)
    {
		if (creature == null)
		{
			defaultCondition = condition.Copy();
		}
		else
		{
			customConditions[creature.metaInfo.LcId] = condition.Copy();
		}
		SaveDataFile();
    }

	public QueuedWorkOrder.QueueConditionInfo GetDefaultCondition(LobotomyBaseMod.LcIdLong LcId)
    {
		QueuedWorkOrder.QueueConditionInfo value;
        if (!customConditions.TryGetValue(LcId, out value))
		{
			value = defaultCondition;
		}
		return value.Copy();
    }

	public void SaveDefaultCondition(QueuedWorkOrder.QueueConditionInfo condition, LobotomyBaseMod.LcIdLong LcId)
    {
		if (condition == null)
		{
			if (LcId == null)
			{
				defaultCondition = new QueuedWorkOrder.QueueConditionInfo();
				defaultCondition.minHp = -1f;
				defaultCondition.minMental = -1f;
				defaultCondition.yieldAgent = true;
				defaultCondition.yieldCreature = true;
			}
			else
			{
				customConditions.Remove(LcId);
			}
		}
		else
		{
			if (LcId == null)
			{
				defaultCondition = condition.Copy();
			}
			else
			{
				customConditions[LcId] = condition.Copy();
			}
		}
		SaveDataFile();
    }

	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Default", ConvertToSaveData(defaultCondition));
		foreach (KeyValuePair<LobotomyBaseMod.LcIdLong, QueuedWorkOrder.QueueConditionInfo> pair in customConditions)
		{
			data.Add(pair.Key.packageId + ":" + pair.Key.id.ToString(), ConvertToSaveData(pair.Value));
		}
		return data;
	}

	public void LoadData(Dictionary<string, object> data)
	{
        customConditions = new Dictionary<LobotomyBaseMod.LcIdLong, QueuedWorkOrder.QueueConditionInfo>();
		foreach (KeyValuePair<string, object> pair in data)
		{
			Dictionary<string, object> data2 = null;
			if (GameUtil.TryGetValue<Dictionary<string, object>>(data, pair.Key, ref data2))
			{
				if (pair.Key == "Default")
				{
					defaultCondition = ConvertToConditionInfo(data2);
				}
				else
				{
					LobotomyBaseMod.LcIdLong lcid;
					string[] array = pair.Key.Split(':');
					if (array.Length < 2) continue;
					long num = -1L;
					long.TryParse(array[1], out num);
					if (num == -1L) continue;
					lcid = new LobotomyBaseMod.LcIdLong(array[0], num);
					customConditions.Add(lcid, ConvertToConditionInfo(data2));
				}
			}
		}
	}

	public static WorkQueuePreferanceManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new WorkQueuePreferanceManager();
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
        else
        {
            defaultCondition = new QueuedWorkOrder.QueueConditionInfo();
            defaultCondition.minHp = -1f;
            defaultCondition.minMental = -1f;
            defaultCondition.yieldAgent = true;
            defaultCondition.yieldCreature = true;
            customConditions = new Dictionary<LobotomyBaseMod.LcIdLong, QueuedWorkOrder.QueueConditionInfo>();
			SaveDataFile();
        }
	}

	public void ResetData()
	{
        defaultCondition = new QueuedWorkOrder.QueueConditionInfo();
        defaultCondition.minHp = -1f;
        defaultCondition.minMental = -1f;
        defaultCondition.yieldAgent = true;
        defaultCondition.yieldCreature = true;
        customConditions = new Dictionary<LobotomyBaseMod.LcIdLong, QueuedWorkOrder.QueueConditionInfo>();
		SaveDataFile();
	}

	public string dataPath
	{
		get
		{
			return Application.persistentDataPath + "/WorkQueuePreferance.dat";
		}
	}
    
    public Dictionary<string, object> ConvertToSaveData(QueuedWorkOrder.QueueConditionInfo conditional)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("minHp", conditional.minHp);
        data.Add("minMental", conditional.minMental);
        data.Add("delay", conditional.delay);
        data.Add("yieldAgent", conditional.yieldAgent);
        data.Add("yieldCreature", conditional.yieldCreature);
        data.Add("asapAgent", conditional.asapAgent);
        data.Add("asapCreature", conditional.asapCreature);
        data.Add("impassableAgent", conditional.impassableAgent);
        data.Add("impassableCreature", conditional.impassableCreature);
		return data;
    }
    
    public QueuedWorkOrder.QueueConditionInfo ConvertToConditionInfo(Dictionary<string, object> data)
    {
		QueuedWorkOrder.QueueConditionInfo condition = new QueuedWorkOrder.QueueConditionInfo();
		GameUtil.TryGetValue<float>(data, "minHp", ref condition.minHp);
		GameUtil.TryGetValue<float>(data, "minMental", ref condition.minMental);
		GameUtil.TryGetValue<float>(data, "delay", ref condition.delay);
		GameUtil.TryGetValue<bool>(data, "yieldAgent", ref condition.yieldAgent);
		GameUtil.TryGetValue<bool>(data, "yieldCreature", ref condition.yieldCreature);
		GameUtil.TryGetValue<bool>(data, "asapAgent", ref condition.asapAgent);
		GameUtil.TryGetValue<bool>(data, "asapCreature", ref condition.asapCreature);
		GameUtil.TryGetValue<bool>(data, "impassableAgent", ref condition.impassableAgent);
		GameUtil.TryGetValue<bool>(data, "impassableCreature", ref condition.impassableCreature);
		return condition;
    }

	private static WorkQueuePreferanceManager _instance;

    private QueuedWorkOrder.QueueConditionInfo defaultCondition;

    private Dictionary<LobotomyBaseMod.LcIdLong, QueuedWorkOrder.QueueConditionInfo> customConditions;
}
