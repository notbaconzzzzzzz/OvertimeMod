using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AprilFoolsManager
{
	public AprilFoolsManager()
	{
        eventDic = new Dictionary<string, FoolsEvent>();
        FoolsEvent foolEvent;
        foolEvent = new AlephOneSinEvent();
        foolEvent.chance = 70f;
        foolEvent.diminish = 5f;
        eventDic.Add(foolEvent.id, foolEvent);
        foolEvent = new AllDTMEvent();
        foolEvent.chance = 15f;
        foolEvent.diminish = 20f;
        eventDic.Add(foolEvent.id, foolEvent); /*
        foolEvent = new EgoMissingEvent();
        foolEvent.chance = 25f;
        foolEvent.diminish = 10f;
        eventDic.Add(foolEvent.id, foolEvent); */
        foolEvent = new SleepyTimeEvent();
        foolEvent.chance = 50f;
        foolEvent.diminish = 10f;
        eventDic.Add(foolEvent.id, foolEvent);
	}

	public static AprilFoolsManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AprilFoolsManager();
				_instance.LoadDataFile();
			}
			return _instance;
		}
	}

	public string dataPath
	{
		get
		{
			return Application.persistentDataPath + "/AF.dat";
		}
	}

	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("LastAprilFool", lastAprilFool);
		Dictionary<string, float> data2 = new Dictionary<string, float>();
        foreach (KeyValuePair<string, FoolsEvent> kv in eventDic)
        {
			FoolsEvent foolEvent = kv.Value;
            data2.Add(foolEvent.id, foolEvent.satisfy);
        }
        data.Add("Satisfy", data2);
		return data;
	}

	public void LoadData(Dictionary<string, object> data)
	{
        GameUtil.TryGetValue<DateTime>(data, "LastAprilFool", ref lastAprilFool);
		Dictionary<string, float> data2 = null;
        if (GameUtil.TryGetValue<Dictionary<string, float>>(data, "Satisfy", ref data2))
        {
            foreach (KeyValuePair<string, float> kv in data2)
            {
                eventDic[kv.Key].satisfy = kv.Value;
            }
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
            SaveDataFile();
        }
	}

    public bool IsEventActive(string id)
    {
        return GetActiveEvent(id) != null;
    }

    public FoolsEvent GetActiveEvent(string id)
    {
        foreach (FoolsEvent foolEvent in loadedEvents)
        {
            if (foolEvent.active && foolEvent.id.Equals(id)) return foolEvent;
        }
        return null;
    }

    public FoolsEvent GetEvent(string id)
    {
        foreach (FoolsEvent foolEvent in loadedEvents)
        {
            if (foolEvent.id.Equals(id)) return foolEvent;
        }
        return null;
    }

    public bool Incompatible(FoolsEvent foolEvent)
    {
        foreach (FoolsEvent foolEvent2 in loadedEvents)
        {
            if (!foolEvent2.active) continue;
            if (foolEvent2.Incompatible(foolEvent)) return true;
            if (foolEvent.Incompatible(foolEvent2)) return true;
        }
        return false;
    }

    public void OnLoadDay()
    {
		if (DateTime.Today.Month == 4 && lastAprilFool.Year != DateTime.Today.Year)
		{
			lastAprilFool = DateTime.Today;
			foreach (KeyValuePair<string, FoolsEvent> kv in eventDic)
			{
				FoolsEvent foolEvent = kv.Value;
                foolEvent.satisfy = 0f;
			}
		}
        if (DateTime.Today.Month == 4 && (DateTime.Today.Day == 1 || DateTime.Today.Day == lastAprilFool.Day))
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, FoolsEvent> kv in eventDic)
			{
				FoolsEvent foolEvent = kv.Value;
                list.Add(foolEvent.id);
			}
			List<string> list2 = new List<string>();
            while (list.Count > 0)
            {
                int rand = UnityEngine.Random.Range(0, list.Count);
				list2.Add(list[rand]);
                list.RemoveAt(rand);
            }
			int numOfEvents = 0;
			foreach (FoolsEvent foolEvent in loadedEvents)
			{
				if (foolEvent.active) numOfEvents++;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				FoolsEvent foolEvent = eventDic[list2[i]];
				if (Incompatible(foolEvent)) continue;
				if (foolEvent.active && !foolEvent.CanDeactivate()) continue;
				float chance = foolEvent.chance;
				chance /= Mathf.Pow(foolEvent.diminish, foolEvent.satisfy);
				chance /= Mathf.Pow(10f, numOfEvents);
				if (UnityEngine.Random.Range(0f, 100f) <= chance)
				{
                    if (!foolEvent.active)
                    {
                        if (!loadedEvents.Contains(foolEvent)) loadedEvents.Add(foolEvent);
                        foolEvent.active = true;
                        numOfEvents++;
                    }
				}
                else if (foolEvent.active)
                {
                    foolEvent.active = false;
                }
			}
		}
    }

	private static AprilFoolsManager _instance;

    public List<FoolsEvent> loadedEvents = new List<FoolsEvent>();

    public Dictionary<string, FoolsEvent> eventDic;

    public DateTime lastAprilFool = new DateTime(2000, 4, 1);

    public class FoolsEvent
    {
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active == value) return;
                _active = value;
                if (_active) { hasSatisfied = false; OnActivate(); }
                else OnDeactivate();
            }
        }

        public virtual string id
        {
            get
            {
                return "";
            }
        }

        public virtual bool Incompatible(FoolsEvent other)
        {
            return false;
        }

        public virtual bool CanDeactivate()
        {
            return true;
        }

        public void Trigger()
        {
            if (hasSatisfied) return;
            hasSatisfied = true;
            satisfy += 1f;
        }

        public virtual void OnActivate()
        {

        }

        public virtual void OnDeactivate()
        {
            
        }

        private bool _active;
        protected bool hasSatisfied;

        public float chance;
        public float diminish;
        public float satisfy;
    }

    public class AlephOneSinEvent : FoolsEvent, IObserver
    {
        public override string id
        {
            get
            {
                return "AlephOneSin";
            }
        }

        public override bool Incompatible(FoolsEvent other)
        {
            if (other.id == "AllDTM") return true;
            return false;
        }

        public override void OnActivate()
        {
            CreatureTypeInfo metaData = CreatureTypeList.instance.GetData(100009);
			metaData.script = "AprilFoolsOneBadManyGood";
			metaData._riskLevel = 5;
            metaData.feelingStateCubeBounds.upperBounds = new int[]{9, 21, 30};
			metaData.workDamage.type = RwbpType.P;
			metaData.workDamage.min = 6f;
			metaData.workDamage.max = 8f;
			metaData.workProbTable.SetWorkProb(RwbpType.R, 1, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 2, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 3, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 4, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 5, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 1, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 2, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 3, 0.4f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 4, 0.45f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 5, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 1, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 2, 0.4f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 3, 0.45f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 4, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 5, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 1, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 2, 0f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 3, 0.2f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 4, 0.35f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 5, 0.45f);
			metaData.isEscapeAble = true;
			metaData.qliphothMax = 3;
			metaData.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL).prob = 0.01f;
			metaData.maxHp = 12000;
			metaData.speed = 1f;
            foreach (CreatureSpecialSkillDesc desc in CreatureTypeList.instance.GetSkillTipData(100009L).descList)
			{
				desc.original = "May god help you.";
				desc.desc = "May god help you.";
			}
			DefenseInfo defense = metaData.defenseTable.GetDefenseInfo();
			defense.R = 0.5f;
			defense.W = 0.5f;
			defense.B = 0.5f;
			defense.P = 0.3f;
			EquipmentTypeInfo weapon = EquipmentTypeList.instance.GetData(200009);
            weapon.grade = "5";
			weapon.damageInfos[0].type = RwbpType.P;
			weapon.damageInfos[0].min = 15f;
			weapon.damageInfos[0].max = 22f;
			EquipmentTypeInfo armor = EquipmentTypeList.instance.GetData(300009);
            armor.grade = "5";
			armor.defenseInfo.R = 0.8f;
			armor.defenseInfo.W = 0.6f;
			armor.defenseInfo.B = 0.8f;
			armor.defenseInfo.P = 0.4f;
			EquipmentTypeInfo gift = EquipmentTypeList.instance.GetData(400009);
			gift.bonus.hp = 3;
			gift.bonus.mental = 3;
			gift.bonus.workProb = 3;
			gift.bonus.cubeSpeed = 3;
			gift.bonus.attackSpeed = 3;
			gift.bonus.movement = 3;
			Notice.instance.Observe(NoticeName.OnReleaseWork, this);
			Notice.instance.Observe(NoticeName.OnEscape, this);
        }

        public override void OnDeactivate()
        {
            CreatureTypeInfo metaData = CreatureTypeList.instance.GetData(100009);
			metaData.script = "OneBadManyGood";
			metaData._riskLevel = 1;
            metaData.feelingStateCubeBounds.upperBounds = new int[]{3, 7, 10};
			metaData.workDamage.type = RwbpType.W;
			metaData.workDamage.min = 1f;
			metaData.workDamage.max = 2f;
			metaData.workProbTable.SetWorkProb(RwbpType.R, 1, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 2, 0.4f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 3, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 4, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.R, 5, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 1, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 2, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 3, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 4, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.W, 5, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 1, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 2, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 3, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 4, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.B, 5, 0.7f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 1, 0.5f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 2, 0.4f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 3, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 4, 0.3f);
			metaData.workProbTable.SetWorkProb(RwbpType.P, 5, 0.3f);
			metaData.isEscapeAble = false;
			metaData.qliphothMax = 0;
			metaData.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL).prob = 0.05f;
			EquipmentTypeInfo weapon = EquipmentTypeList.instance.GetData(200009);
            weapon.grade = "1";
			weapon.damageInfos[0].type = RwbpType.W;
			weapon.damageInfos[0].min = 5f;
			weapon.damageInfos[0].max = 7f;
			EquipmentTypeInfo armor = EquipmentTypeList.instance.GetData(300009);
            armor.grade = "1";
			armor.defenseInfo.R = 0.9f;
			armor.defenseInfo.W = 0.8f;
			armor.defenseInfo.B = 0.9f;
			armor.defenseInfo.P = 2f;
			EquipmentTypeInfo gift = EquipmentTypeList.instance.GetData(400009);
			gift.bonus.hp = 0;
			gift.bonus.mental = 2;
			gift.bonus.workProb = 0;
			gift.bonus.cubeSpeed = 0;
			gift.bonus.attackSpeed = 0;
			gift.bonus.movement = 0;
			Notice.instance.Remove(NoticeName.OnReleaseWork, this);
			Notice.instance.Remove(NoticeName.OnEscape, this);
        }

        public void OnNotice(string notice, params object[] param)
		{
            CreatureModel model;
            if (notice == NoticeName.OnReleaseWork)
            {
                model = param[0] as CreatureModel;
                if (model.metaInfo.LcId == new LobotomyBaseMod.LcIdLong(100009L))
                {
                    Trigger();
                }
            }
            else if (notice == NoticeName.OnEscape)
            {
                model = param[0] as CreatureModel;
                if (model.metaInfo.LcId == new LobotomyBaseMod.LcIdLong(100009L))
                {
                    Trigger();
                }
            }
		}

        public override bool CanDeactivate()
        {
            return hasSatisfied;
        }
    }

    public class AllDTMEvent : FoolsEvent , IObserver
    {
        public override string id
        {
            get
            {
                return "AllDTM";
            }
        }

        public override bool Incompatible(FoolsEvent other)
        {
            if (other.id == "AlephOneSin") return true;
            return false;
        }

        public override void OnActivate()
        {
			Notice.instance.Observe(NoticeName.OnStageStart, this);
        }

        public override void OnDeactivate()
        {
			Notice.instance.Remove(NoticeName.OnStageStart, this);
        }

        public void OnNotice(string notice, params object[] param)
		{
            if (notice == NoticeName.OnStageStart)
            {
                Trigger();
            }
		}
    }

    /*
    public class EgoMissingEvent : FoolsEvent
    {
        public override string id
        {
            get
            {
                return "EgoMissing";
            }
        }

        public override void OnActivate()
        {
			Notice.instance.Observe(NoticeName.OnStageStart, this);
        }

        public override void OnDeactivate()
        {
			Notice.instance.Remove(NoticeName.OnStageStart, this);
        }

        public void OnNotice(string notice, params object[] param)
		{
            if (notice == NoticeName.OnStageStart)
            {
                Trigger();
            }
		}
    }*/

    public class SleepyTimeEvent : FoolsEvent , IObserver
    {
        public override string id
        {
            get
            {
                return "SleepyTime";
            }
        }

        public override void OnActivate()
        {
			Notice.instance.Observe(NoticeName.OnStageStart, this);
			Notice.instance.Observe(NoticeName.FixedUpdate, this);
            lateUpdate = false;
        }

        public override void OnDeactivate()
        {
			Notice.instance.Remove(NoticeName.OnStageStart, this);
			Notice.instance.Remove(NoticeName.FixedUpdate, this);
        }

        public void OnNotice(string notice, params object[] param)
		{
            if (notice == NoticeName.OnStageStart)
            {
                Trigger();
                lateUpdate = true;
                BgmManager.instance.BGMForcelyStop();
                BgmManager.instance.SetBgmSoundVolume(BgmManager.instance.currentBgmVolume / 25f);
                Baku sheep = new Baku();
                foreach (WorkerModel worker in WorkerManager.instance.GetWorkerList())
                {
                    sheep.SleepWorker(worker);
                }
                foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                {
                    creature.TryTranquilize(60f);
                }
				Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
                for (int i = 0; i < openedAreaList.Length; i++)
                {
                    foreach (PassageObjectModel passageObjectModel in new List<PassageObjectModel>(openedAreaList[i].passageList))
                    {
                        Notice.instance.Send(NoticeName.PassageBlackOut, new object[]
                        {
                            passageObjectModel
                        });
                    }
                }
            }
            else if (lateUpdate && notice == NoticeName.FixedUpdate)
            {
                lateUpdate = false;
                AudioClip audioClip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", "creature/Happy_Teddy/happyTeddy_Enter"));
                if (audioClip != null)
                {
                    GlobalAudioManager.instance.PlayLocalClip(audioClip);
                }
            }
		}

        public override bool CanDeactivate()
        {
            return hasSatisfied;
        }

        private bool lateUpdate;
    }
}
