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

    public static List<WorkerModel> GetListOfWorkers(string representation, bool[] allowAgents = null)
    {
        if (allowAgents == null) allowAgents = new bool[] {true, false, false}; // deployedAgents, spareAgents, graveyardAgents
		List<WorkerModel> list = new List<WorkerModel>();
        string[] ids = representation.Split(',');
        IList<OfficerModel> officers = OfficerManager.instance.GetOfficerList();
		foreach (string str in ids)
		{
            long id = 0L;
            if (!long.TryParse(str, out id)) continue;
			if (id == -1L)
			{
				list.AddRange(officers);
                continue;
			}
            WorkerModel worker = null;
            officer = officers.First(x => x.instanceId == id);
            if (worker != null) list.Add(worker);
		}
		return list;
    }

    public List<string> creatureCommand = new List<string>();

    public List<string> standardCommand = new List<string>();

    public List<string> agentCommand = new List<string>();

    public List<string> officerCommand = new List<string>();

    public List<string> rootCommand = new List<string>();

    public List<string> configCommand = new List<string>();
}