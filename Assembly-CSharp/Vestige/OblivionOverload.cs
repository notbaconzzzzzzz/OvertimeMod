using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class OblivionOverload : OvertimeOverload
    {
        public OblivionOverload(CreatureModel _creature) : base(_creature)
		{
            
		}

        public override OverloadType Type
        {
            get
            {
                return OverloadType.OBLIVION;
            }
        }

        public override void InitAfterWork()
		{
            base.InitAfterWork();
            creature.ProbReductionValue = 5;
			foreach (AgentModel agent in AgentManager.instance.GetAgentList())
			{
				if (agent.GetCurrentCommand() != null && (agent.GetCurrentCommand() is ManageCreatureAgentCommand) && (agent.GetCurrentCommand() as ManageCreatureAgentCommand).targetCreature == creature)
				{
					agent.AddUnitBuf(new OblivionOverloadSlowBuf(creature));
				}
			}
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.currentGameManager.state == GameState.PAUSE && GameManager.currentGameManager.GetCurrentPauseCaller() == PAUSECALL.STOPGAME)
            {
                if (failed)
                {
                    damageDelay -= Time.unscaledDeltaTime;
                    if (damageDelay <= 0f)
                    {
                        DamageAll();
                    }
                    return;
                }
                if (workStarted) return;
                if (!creature.isOverloaded)
                {
                    DestroyOverload();
                    return;
                }
                float timerSpeed = 1f;
                if (creature.state == CreatureState.WAIT && creature.feelingState == CreatureFeelingState.NONE)
                {
                    timerSpeed = 2f;
                }
                creature.overloadTimer += Time.unscaledDeltaTime * timerSpeed;
                if (creature.overloadTimer >= creature.currentOverloadMaxTime)
                {
                    creature.ExplodeOverload();
                }
            }
        }

        public override void OnWorkAllocated(AgentModel agent)
		{
            if (isInitialized)
            {
                agent.AddUnitBuf(new OblivionOverloadSlowBuf(creature));
            }
        }
    }
}