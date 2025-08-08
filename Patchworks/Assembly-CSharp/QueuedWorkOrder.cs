using System;
using System.Collections.Generic;

public class QueuedWorkOrder
{
    public QueuedWorkOrder(AgentModel _agent, CreatureModel _creature, SkillTypeInfo _skill)
    {
        agent = _agent;
        creature = _creature;
        room = creature.Unit.room;
        skill = _skill;
        conditional = new QueueConditionInfo();
    }

    public bool CheckAgentAvailable()
    {
        if (agent.IsDead())
		{
			return false;
		}
        if (agent.IsPanic())
		{
			return false;
		}
        if (agent.IsCrazy())
		{
			return false;
		}
        if (agent.IsSuppressing())
		{
			return false;
		}
		if (agent.GetState() == AgentAIState.MANAGE)
		{
			return false;
		}
        if (agent.hp / (float)agent.maxHp < conditional.minHp)
		{
			return false;
		}
        if (agent.mental / (float)agent.maxMental < conditional.minMental)
		{
			return false;
		}
        return true;
    }

    public bool CheckCreatureAvailable()
    {
        if (room.IsWorking)
        {
            return false;
        }
        if (room.IsWorkAllocated)
		{
			return false;
		}
        if (creature.IsEscaped())
        {
            return false;
        }
        if (!creature.script.IsWorkable())
		{
			return false;
		}
        if (!creature.script.CanEnterRoom())
		{
			return false;
		}
        return true;
    }

    public bool TryAllocate()
    {
        if (agent.IsDead())
		{
            Remove();
			return false;
		}
        if (!isAgentFront || !isCreatureFront)
		{
			return false;
		}
        if (!CheckCreatureAvailable()) return false;
        if (!CheckAgentAvailable()) return false;
        if (!agent.CheckWorkCommand())
        {
            Remove();
            return false;
        }
        agent.ManageCreature(creature, skill, CommandWindow.CommandWindow.CurrentWindow.GetWorkSprite(skill.rwbpType));
        agent.counterAttackEnabled = false;
        room.OnWorkAllocated(agent);
        creature.script.OnWorkAllocated(skill, agent);
        AngelaConversation.instance.MakeMessage(AngelaMessageState.MANAGE_START, new object[]
        {
            agent,
            skill,
            creature
        });
        Remove();
		return true;
    }

    public void Remove()
    {
        agent.DequeueWorkOrder(this);
        room.DequeueWorkOrder(this);
    }

	public SkillTypeInfo skill;
    
    public AgentModel agent;

    public CreatureModel creature;

    public IsolateRoom room;

    public bool isAgentFront = false;

    public bool isCreatureFront = false;

    public QueueConditionInfo conditional;

    public class QueueConditionInfo
    {
        public float minHp = 0f;
        public float minMental = 0f;
        public float delay = 0f;
        public bool yieldAgent = false;
        public bool yieldCreature = false;
        public bool asapAgent = false;
        public bool asapCreature = false;
        public bool impassableAgent = false;
        public bool impassableCreature = false;
    }
}