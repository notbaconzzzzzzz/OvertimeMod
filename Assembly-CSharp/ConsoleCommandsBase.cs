using System;
using System.Collections.Generic;
using System.Linq;

public class ConsoleCommandsBase
{
    public ConsoleCommandsBase()
    {
        SetList();
    }

    public virtual void SetList()
    {
        creatureCommand.Clear();
        standardCommand.Clear();
        agentCommand.Clear();
        officerCommand.Clear();
        rootCommand.Clear();
        configCommand.Clear();
    }

    public virtual void CreatureCommandOperation(int index, params string[] param)
    {

    }

    public virtual void StandardCommandOperation(int index, params string[] param)
    {

    }

    public virtual void AgentCommandOperation(int index, params string[] param)
    {

    }

    public virtual void OfficerCommandOperation(int index, params string[] param)
    {

    }

    public virtual void RootCommandOperation(int index, params string[] param)
    {

    }

    public virtual void ConfigCommandOperation(int index, params string[] param)
    {

    }

    public static List<AgentModel> GetListOfAgents(string representation, bool[] allowAgents = null)
    {
        if (allowAgents == null) allowAgents = new bool[] {true, false, false}; // deployedAgents, spareAgents, graveyardAgents
		List<AgentModel> list = new List<AgentModel>();
        string[] ids = representation.Split(',');
		foreach (string str in ids)
		{
			if (str.Equals("a") || str.Equals("w") || str.Equals("u"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList());
				// if (allowAgents[2]) list.AddRange(
                continue;
			}
            long id = 0L;
            if (!long.TryParse(str, out id)) continue;
			if (id == -1L)
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList());
				// if (allowAgents[2]) list.AddRange(
                continue;
			}
            AgentModel agent = null;
            if (allowAgents[0]) agent = AgentManager.instance.GetAgent(id);
            if (allowAgents[1] && agent == null) agent = AgentManager.instance.GetSpareAgent(id);
            // if (allowAgents[2] && agent == null) agent = 
            if (agent != null) list.Add(agent);
		}
		return list;
    }

    public static List<OfficerModel> GetListOfOfficers(string representation)
    {
		List<OfficerModel> list = new List<OfficerModel>();
        string[] ids = representation.Split(',');
        IList<OfficerModel> officers = OfficerManager.instance.GetOfficerList();
		foreach (string str in ids)
		{
			if (str.Equals("o") || str.Equals("w") || str.Equals("u"))
			{
				list.AddRange(officers);
                continue;
			}
            long id = 0L;
            if (!long.TryParse(str, out id)) continue;
			if (id == -1L)
			{
				list.AddRange(officers);
                continue;
			}
            OfficerModel officer = null;
            officer = officers.First(x => x.instanceId == id);
            if (officer != null) list.Add(officer);
		}
		return list;
    }

    public static List<WorkerModel> GetListOfWorkers(string representation, SearchClass defaultSearch = SearchClass.Agent, bool[] allowAgents = null)
    {
        if (allowAgents == null) allowAgents = new bool[] {true, false, false}; // deployedAgents, spareAgents, graveyardAgents
		List<WorkerModel> list = new List<WorkerModel>();
        string[] ids = representation.Split(',');
        IList<OfficerModel> officers = OfficerManager.instance.GetOfficerList();
		foreach (string str in ids)
		{
			if (str.Equals("a"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
				// if (allowAgents[2]) list.AddRange(
                continue;
			}
			if (str.Equals("o"))
			{
				list.AddRange(officers.ToArray());
                continue;
			}
			if (str.Equals("w") || str.Equals("u"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
				// if (allowAgents[2]) list.AddRange(
				list.AddRange(officers.ToArray());
                continue;
			}
            switch (defaultSearch)
            {
                case SearchClass.Agent:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
                        if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
                        // if (allowAgents[2]) list.AddRange(
                        continue;
                    }
                    AgentModel agent = null;
                    if (allowAgents[0]) agent = AgentManager.instance.GetAgent(id);
                    if (allowAgents[1] && agent == null) agent = AgentManager.instance.GetSpareAgent(id);
                    // if (allowAgents[2] && agent == null) agent = 
                    if (agent != null) list.Add(agent);
                }
                break;
                case SearchClass.Officer:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        list.AddRange(officers.ToArray());
                        continue;
                    }
                    OfficerModel officer = null;
                    officer = officers.First(x => x.instanceId == id);
                    if (officer != null) list.Add(officer);
                }
                break;
            }
		}
		return list;
    }

    public static List<CreatureModel> GetListOfCreatures(string representation, SearchClass defaultSearch = SearchClass.Creature)
    {
		List<CreatureModel> list = new List<CreatureModel>();
        string[] ids = representation.Split(',');
		foreach (string str in ids)
		{
			if (str.Equals("c"))
			{
                list.AddRange(CreatureManager.instance.GetCreatureList());
                continue;
			}
			if (str.Equals("m"))
			{
                foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                {
                    list.AddRange(creature.GetAliveChilds().ToArray());
                }
                continue;
			}
			if (str.Equals("o"))
			{
				list.AddRange(OrdealManager.instance.GetOrdealCreatureList());
                continue;
			}
			if (str.Equals("e"))
			{
				list.AddRange(SpecialEventManager.instance.GetEventCreatureList());
                continue;
			}
            /*
			if (str.Equals("s"))
			{
				list.AddRange(SefiraBossManager.Instance.);
                continue;
			}*/
			if (str.Equals("u") || str.Equals("h"))
			{
                list.AddRange(CreatureManager.instance.GetCreatureList());
                foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                {
                    list.AddRange(creature.GetAliveChilds().ToArray());
                }
				list.AddRange(OrdealManager.instance.GetOrdealCreatureList());
				list.AddRange(SpecialEventManager.instance.GetEventCreatureList());
                continue;
			}
            switch (defaultSearch)
            {
                case SearchClass.Creature:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        list.AddRange(CreatureManager.instance.GetCreatureList());
                        continue;
                    }
                    CreatureModel creature = null;
                    creature = CreatureManager.instance.GetCreature(id);
                    if (creature == null)
                    {
                        creature = CreatureManager.instance.FindCreature(id);
                    }
                    if (creature != null) list.Add(creature);
                }
                break;
            }
		}
		return list;
    }

    public static List<UnitModel> GetListOfUnits(string representation, SearchClass defaultSearch = SearchClass.Agent, bool[] allowAgents = null)
    {
        if (allowAgents == null) allowAgents = new bool[] {true, false, false}; // deployedAgents, spareAgents, graveyardAgents
		List<UnitModel> list = new List<UnitModel>();
        string[] ids = representation.Split(',');
        IList<OfficerModel> officers = OfficerManager.instance.GetOfficerList();
		foreach (string str in ids)
		{
			if (str.Equals("a"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
				// if (allowAgents[2]) list.AddRange(
                continue;
			}
			if (str.Equals("o"))
			{
				list.AddRange(officers.ToArray());
                continue;
			}
			if (str.Equals("w"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
				// if (allowAgents[2]) list.AddRange(
				list.AddRange(officers.ToArray());
                continue;
			}
			/*
			if (str.Equals("r"))
			{
				list.AddRange(RabbitManager.instance.);
                continue;
			}*/
			if (str.Equals("c"))
			{
                list.AddRange(CreatureManager.instance.GetCreatureList());
                continue;
			}
			if (str.Equals("m"))
			{
                foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                {
                    list.AddRange(creature.GetAliveChilds().ToArray());
                }
                continue;
			}
			if (str.Equals("o"))
			{
				list.AddRange(OrdealManager.instance.GetOrdealCreatureList());
                continue;
			}
			if (str.Equals("e"))
			{
				list.AddRange(SpecialEventManager.instance.GetEventCreatureList());
                continue;
			}
			if (str.Equals("u"))
			{
				if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
				if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
				// if (allowAgents[2]) list.AddRange(
				list.AddRange(officers.ToArray());
                list.AddRange(CreatureManager.instance.GetCreatureList());
                foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                {
                    list.AddRange(creature.GetAliveChilds().ToArray());
                }
				list.AddRange(OrdealManager.instance.GetOrdealCreatureList());
				list.AddRange(SpecialEventManager.instance.GetEventCreatureList());
                continue;
			}
            switch (defaultSearch)
            {
                case SearchClass.Agent:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        if (allowAgents[0]) list.AddRange(AgentManager.instance.GetAgentList().ToArray());
                        if (allowAgents[1]) list.AddRange(AgentManager.instance.GetAgentSpareList().ToArray());
                        // if (allowAgents[2]) list.AddRange(
                        continue;
                    }
                    AgentModel agent = null;
                    if (allowAgents[0]) agent = AgentManager.instance.GetAgent(id);
                    if (allowAgents[1] && agent == null) agent = AgentManager.instance.GetSpareAgent(id);
                    // if (allowAgents[2] && agent == null) agent = 
                    if (agent != null) list.Add(agent);
                }
                break;
                case SearchClass.Officer:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        list.AddRange(officers.ToArray());
                        continue;
                    }
                    OfficerModel officer = null;
                    officer = officers.First(x => x.instanceId == id);
                    if (officer != null) list.Add(officer);
                }
                break;
                case SearchClass.Creature:
                {
                    long id = 0L;
                    if (!long.TryParse(str, out id)) continue;
                    if (id == -1L)
                    {
                        list.AddRange(CreatureManager.instance.GetCreatureList());
                        continue;
                    }
                    CreatureModel creature = null;
                    creature = CreatureManager.instance.GetCreature(id);
                    if (creature == null)
                    {
                        creature = CreatureManager.instance.FindCreature(id);
                    }
                    if (creature != null) list.Add(creature);
                }
                break;
            }
		}
		return list;
    }

    public List<string> creatureCommand = new List<string>();

    public List<string> standardCommand = new List<string>();

    public List<string> agentCommand = new List<string>();

    public List<string> officerCommand = new List<string>();

    public List<string> rootCommand = new List<string>();

    public List<string> configCommand = new List<string>();

    public enum SearchClass
    {
        Agent,
        Officer,
        Rabbit,
        Creature,
        Minion,
        Ordeal,
        Event,
        SefiraBoss,
        Worker,
        Hostile,
        Unit
    }
}