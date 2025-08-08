using System;
using System.Collections.Generic;
using UnityEngine;

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

    public QueuedWorkOrder(AgentModel _agent, CreatureModel _creature, SkillTypeInfo _skill, QueueConditionInfo _condition)
    {
        agent = _agent;
        creature = _creature;
        room = creature.Unit.room;
        skill = _skill;
        conditional = _condition;
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
        if (conditional.minHp == -1f || conditional.minMental == -1f)
        {
            CalculateAutoThresholds();
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
        if (creature.isTranquilized)
        {
            return false;
        }
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
        if (room.QueueWaitedTime < conditional.delay)
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
		}/*
        if (!isAgentFront || !isCreatureFront)
		{
			return false;
		}*/
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

    public bool CheckAgentQueue()
    {
        List<QueuedWorkOrder> list = agent.GetWorkOrderQueue();
		bool yielded = true;
		for (int i = 0; i < list.Count; i++)
		{
			QueuedWorkOrder work = list[i];
            if (work == this)
            {
                return yielded || conditional.asapAgent;
            }
            if (work.conditional.impassableAgent)
            {
                return false;
            }
			if (yielded && work.conditional.yieldAgent)
			{
				yielded = true;
			}
			else
			{
				yielded = false;
			}
		}
        Remove();
        return false;
    }

    public void CalculateAutoThresholds()
    {
        int maxCubeCount = creature.metaInfo.feelingStateCubeBounds.GetLastBound();
		bool wantNeutral = false;
		if (creature.metaInfo.LcId == 100029L || creature.metaInfo.LcId == 100019L)
		{
			wantNeutral = true;
		}
		float num = creature.GetWorkSuccessProb(agent, skill);
		if (!wantNeutral)
		{
			num += (float)creature.GetObserveBonusProb() / 100f;
		}
		num += (float)agent.workProb / 500f;
		if (num > 0.95f)
		{
			num = 0.95f;
		}
		float num2 = (float)creature.GetRedusedWorkProbByCounter() / 100f;
		float num3 = (float)creature.ProbReductionValue / 100f;
		if (num3 > 0f)
		{
			num -= num3;
		}
		else
		{
			num -= num2;
		}
		if (creature.sefira.agentDeadPenaltyActivated)
		{
			num -= 0.5f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		if (num < 0f)
		{
			num = 0f;
		}
        if (wantNeutral)
        {
			float ideal = 0.95f;
			FeelingStateCubeBounds bounds = creature.metaInfo.feelingStateCubeBounds;
			if (bounds.upperBounds.Length == 3)
			{
				ideal = ((float)(bounds.upperBounds[0] + bounds.upperBounds[1]) / 2f + 1f) / (float)(maxCubeCount + 1);
			}
			float num69 = Mathf.Clamp(ideal, num - (float)creature.GetObserveBonusProb() / 100f, num + (float)creature.GetObserveBonusProb() / 100f);
            num *= num69;
        }
        else
        {
            num *= num;
        }
        DamageInfo damageInfo = creature.metaInfo.workDamage.Copy();
        float expectedDamage = (float)maxCubeCount * (1f - num) * (damageInfo.min + damageInfo.max) / 2f;
        expectedDamage *= agent.defense.GetMultiplier(damageInfo.type) * UnitModel.GetDmgMultiplierByEgoLevel(creature.GetAttackLevel(), agent.GetDefenseLevel());
        float expectedHpDamage = 0f;
        float expectedMentalDamage = 0f;
        switch (damageInfo.type)
        {
            case RwbpType.R:
                expectedHpDamage = expectedDamage;
				break;
            case RwbpType.W:
                expectedMentalDamage = expectedDamage;
				break;
            case RwbpType.B:
                expectedHpDamage = expectedDamage;
                expectedMentalDamage = expectedDamage;
				break;
            case RwbpType.P:
                expectedHpDamage = expectedDamage * agent.maxHp / 100f;
				break;
            default:
                expectedHpDamage = expectedDamage;
				break;
        }
		int level = creature.GetRiskLevel() - agent.level + 1;
		if (creature.metaInfo.LcId == 100015L)
		{
			if (agent.level <= 3)
			{
				level = 6;
			}
			else if (agent.level <= 4)
			{
				level = 5;
			}
			else
			{
				level = 2;
			}
		}
		else if (creature.script is CensoredCreatureBase)
		{
			if ((creature.script as CensoredCreatureBase) is Censored)
			{
				if (agent.level <= 4)
				{
					level = 5;
				}
				else
				{
					level = 4;
				}
			}
			else if (agent.level <= 3)
			{
				level = 5;
			}
			else if (agent.level <= 4)
			{
				level = 4;
			}
			else
			{
				level = 3;
			}
		}
		else if (level < 0)
		{
			level = 0;
		}
		else if (level > 5)
        {
			level = 5;
		}
		float num420 = 0f;
		switch (level)
		{
		case 0:
		case 1:
			num420 = 0f;
			break;
		case 2:
			num420 = 10f;
			break;
		case 3:
			num420 = 30f;
			break;
		case 4:
			num420 = 60f;
			break;
		case 5:
		case 6:
			num420 = 100f;
			break;
		}
		int num4 = (int)((float)agent.maxMental * num420 / 100f);
        expectedMentalDamage += (float)num4;
        if (expectedHpDamage > 0f)
        {
            expectedHpDamage += 1f;
        }
        if (expectedMentalDamage > 0f)
        {
            expectedMentalDamage += 1f;
        }
        if (conditional.minHp == -1f)
        {
            conditional.minHp = expectedHpDamage / (float)agent.maxHp;
            if (conditional.minHp > 1f)
            {
                conditional.minHp = 1f;
            }
        }
        if (conditional.minMental == -1f)
        {
            conditional.minMental = expectedMentalDamage / (float)agent.maxMental;
            if (conditional.minMental > 1f)
            {
                conditional.minMental = 1f;
            }
        }
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
		public QueueConditionInfo()
		{

		}

		public QueueConditionInfo(QueueConditionInfo condition)
		{
			minHp = condition.minHp;
			minMental = condition.minMental;
			delay = condition.delay;
			yieldAgent = condition.yieldAgent;
			yieldCreature = condition.yieldCreature;
			asapAgent = condition.asapAgent;
			asapCreature = condition.asapCreature;
			impassableAgent = condition.impassableAgent;
			impassableCreature = condition.impassableCreature;
		}

		public QueueConditionInfo Copy()
		{
			return new QueueConditionInfo(this);
		}

        public float minHp = 0; // -1f = auto
        public float minMental = 0f; // -1f = auto
        public float delay = 0f;
        public bool yieldAgent = false;
        public bool yieldCreature = false;
        public bool asapAgent = false;
        public bool asapCreature = false;
        public bool impassableAgent = false;
        public bool impassableCreature = false;
    }
}
