using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
public class SpecialEventManager
{
	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x0600444A RID: 17482 RVA: 0x0003A0BD File Offset: 0x000382BD
	public static SpecialEventManager instance
	{
		get
		{
			if (SpecialEventManager._instance == null)
			{
				SpecialEventManager._instance = new SpecialEventManager();
			}
			return SpecialEventManager._instance;
		}
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x001A4B48 File Offset: 0x001A2D48
	public void OnGameInit()
	{
		int day = PlayerModel.instance.GetDay();
		this.eventCreatureList = new List<EventCreatureModel>();
		this.eventlist.Clear();
		foreach (EventBase item in EventGenInfo.GenerateEvents(day))
		{
			this.eventlist.Add(item);
		}
	}

	// Token: 0x0600444C RID: 17484 RVA: 0x001A4BCC File Offset: 0x001A2DCC
	public void ClearCreatures()
	{
		foreach (EventCreatureModel eventCreatureModel in this.eventCreatureList)
		{
			eventCreatureModel.OnDestroy();
		}
		this.eventCreatureList.Clear();
	}

	// Token: 0x0600444D RID: 17485 RVA: 0x0003A0D8 File Offset: 0x000382D8
	public void OnStageRelease()
	{
		this.ClearCreatures();
	}

	// Token: 0x0600444E RID: 17486 RVA: 0x001A4C34 File Offset: 0x001A2E34
	public bool ActivateEvent(EventBase _event)
	{
		if (!_event.IsStartable())
		{
			return false;
		}
		try
		{
			_event.isStarted = true;
			_event.OnEventStart();
			this.activatedEvents.Add(_event);
			this.eventlist.Remove(_event);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return false;
		}
		return true;
	}

	// Token: 0x0600444F RID: 17487 RVA: 0x001A4C9C File Offset: 0x001A2E9C
	public void OnFixedUpdate()
	{
		foreach (EventCreatureModel eventCreatureModel in this.eventCreatureList)
		{
			eventCreatureModel.OnFixedUpdate();
		}
		foreach (EventBase eventBase in this.activatedEvents)
		{
			eventBase.FixedUpdate();
		}
		foreach (EventBase item in this.removedEvents)
		{
			this.activatedEvents.Remove(item);
		}
		this.removedEvents = new List<EventBase>();
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x001A4DA0 File Offset: 0x001A2FA0
	public EventCreatureModel AddCreature(long metadataId, MapNode pos, EventBase eventBase)
	{ // <Patch>
        return AddCreature_Mod(new LobotomyBaseMod.LcIdLong(metadataId), pos, eventBase);
		/*
        EventCreatureModel eventCreatureModel = new EventCreatureModel((long)this.nextInstId++);
		this.BuildCreature(eventCreatureModel, metadataId);
		eventCreatureModel.GetMovableNode().SetCurrentNode(pos);
		eventCreatureModel.GetMovableNode().SetActive(true);
		eventCreatureModel.baseMaxHp = eventCreatureModel.metaInfo.maxHp;
		eventCreatureModel.hp = (float)eventCreatureModel.metaInfo.maxHp;
		eventCreatureModel.SetEventBase(eventBase);
		this.eventCreatureList.Add(eventCreatureModel);
		Notice.instance.Send(NoticeName.AddEventCreature, new object[]
		{
			eventCreatureModel
		});
		eventCreatureModel.script.OnInit();
		Sefira sefira = SefiraManager.instance.GetSefira(pos.GetAttachedPassage().GetSefiraName());
		eventCreatureModel.sefira = sefira;
		eventCreatureModel.sefiraNum = sefira.indexString;
		return eventCreatureModel;*/
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x001A4E6C File Offset: 0x001A306C
	private void BuildCreature(EventCreatureModel model, long metadataId)
	{ // <Patch>
        BuildCreature_Mod(model, new LobotomyBaseMod.LcIdLong(metadataId));
        /*
		model.observeInfo = new CreatureObserveInfoModel(metadataId);
		string text = "1";
		model.sefira = SefiraManager.instance.GetSefira(text);
		model.sefiraNum = text;
		CreatureTypeInfo data = CreatureTypeList.instance.GetData(metadataId);
		model.metadataId = metadataId;
		model.metaInfo = data;
		if (CreatureTypeList.instance.GetSkillTipData(metadataId) != null)
		{
			model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData(metadataId).GetCopy();
		}
		object obj = null;
		foreach (Assembly assembly in Add_On.instance.AssemList)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == data.script)
				{
					obj = Activator.CreateInstance(type);
				}
			}
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(Type.GetType(data.script));
		}
		if (obj is CreatureBase)
		{
			model.script = (CreatureBase)obj;
		}
		else
		{
			Debug.Log("Creature Script not found");
		}
		model.script.SetModel(model);
		model.script.OnInitialBuild();*/
	}

	// Token: 0x06004452 RID: 17490 RVA: 0x0003A0E0 File Offset: 0x000382E0
	public void OnEventEnd(EventBase _event)
	{
		this.removedEvents.Add(_event);
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x0003A0EE File Offset: 0x000382EE
	public EventCreatureModel[] GetEventCreatureList()
	{
		return this.eventCreatureList.ToArray();
	}

	// Token: 0x06004454 RID: 17492 RVA: 0x001A4FB4 File Offset: 0x001A31B4
	public bool CheckEventContains(EventBase.EventType type, out EventBase script)
	{
		script = null;
		foreach (EventBase eventBase in this.eventlist)
		{
			if (eventBase.type == type)
			{
				script = eventBase;
				return true;
			}
		}
		return false;
	}

    // <Patch>
    private void BuildCreature_Mod(EventCreatureModel model, LobotomyBaseMod.LcIdLong metadataId)
    {
        model.observeInfo = new CreatureObserveInfoModel(metadataId.id);
        model.observeInfo.InitData_Mod(metadataId);
        string text = "1";
        model.sefira = SefiraManager.instance.GetSefira(text);
        model.sefiraNum = text;
        CreatureTypeInfo data_Mod = CreatureTypeList.instance.GetData_Mod(metadataId);
        model.metadataId = metadataId.id;
        model.metaInfo = data_Mod;
        if (CreatureTypeList.instance.GetSkillTipData_Mod(metadataId) != null)
        {
            model.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(metadataId).GetCopy();
        }
        object obj = LobotomyBaseMod.ExtenionUtil.GetTypeInstance<CreatureBase>(data_Mod.script);
        if (obj == null)
        {
            obj = Activator.CreateInstance(Type.GetType(data_Mod.script));
        }
        if (obj is CreatureBase)
        {
            model.script = (CreatureBase)obj;
        }
        else
        {
            Debug.Log("Creature Script not found");
        }
        model.script.SetModel(model);
        model.script.OnInitialBuild();
    }

    // <Patch>
    public EventCreatureModel AddCreature_Mod(LobotomyBaseMod.LcIdLong metadataId, MapNode pos, EventBase eventBase)
    {
        int num = this.nextInstId;
        this.nextInstId = num + 1;
        EventCreatureModel eventCreatureModel = new EventCreatureModel((long)num);
        this.BuildCreature_Mod(eventCreatureModel, metadataId);
        eventCreatureModel.GetMovableNode().SetCurrentNode(pos);
        eventCreatureModel.GetMovableNode().SetActive(true);
        eventCreatureModel.baseMaxHp = eventCreatureModel.metaInfo.maxHp;
        eventCreatureModel.hp = (float)eventCreatureModel.metaInfo.maxHp;
        eventCreatureModel.SetEventBase(eventBase);
        this.eventCreatureList.Add(eventCreatureModel);
        Notice.instance.Send(NoticeName.AddEventCreature, new object[]
        {
            eventCreatureModel
        });
        eventCreatureModel.script.OnInit();
        Sefira sefira = SefiraManager.instance.GetSefira(pos.GetAttachedPassage().GetSefiraName());
        eventCreatureModel.sefira = sefira;
        eventCreatureModel.sefiraNum = sefira.indexString;
        return eventCreatureModel;
    }

	// Token: 0x04003EDA RID: 16090
	private static SpecialEventManager _instance;

	// Token: 0x04003EDB RID: 16091
	private List<EventBase> eventlist = new List<EventBase>();

	// Token: 0x04003EDC RID: 16092
	private List<EventBase> activatedEvents = new List<EventBase>();

	// Token: 0x04003EDD RID: 16093
	private List<EventBase> removedEvents = new List<EventBase>();

	// Token: 0x04003EDE RID: 16094
	private List<EventCreatureModel> eventCreatureList = new List<EventCreatureModel>();

	// Token: 0x04003EDF RID: 16095
	private int nextInstId = 1000;
}
