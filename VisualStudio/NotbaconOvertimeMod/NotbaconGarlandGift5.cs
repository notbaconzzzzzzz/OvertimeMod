using System;
using System.Collections.Generic;
using System.Linq;

namespace NotbaconOvertimeMod
{
    public class NotbaconGarlandGift5 : EquipmentScriptBase
    {
        public override void OnFixedUpdate()
        {
            if (worker == null)
            {
                worker = base.model.owner as WorkerModel;
                if (worker == null) return;
            }
            if (_recoverTimer.RunTimer())
            {
                worker.RecoverMentalv2(3f);
            }
            if (_recoverTimer2.RunTimer())
            {
                _recoverTimer2.StartTimer(2f);
                if (worker.GetMovableNode().currentPassage != null)
                {
                    List<AgentModel> list = new List<AgentModel>();
                    foreach (MovableObjectNode node in worker.GetMovableNode().currentPassage.GetEnteredTargets())
                    {
                        UnitModel unit = node.GetUnit();
                        if (unit == null || unit == worker || !(unit is AgentModel)) continue;
                        AgentModel agent = unit as AgentModel;
                        if (agent.hp <= (float)agent.maxHp * 0.3f) continue;
                        list.Add(agent);
                    }
                    if (list.Count > 0)
                    {
                        float subAmount = 1f;
                        float healAmount = 1f * list.Count;
                        if (list.Count >= 20)
                        {
                            healAmount = 1f * 20;
                            subAmount = 2f * worker.GetHPRecoveryMult(healAmount);
                        }
                        else if (list.Count >= 10)
                        {
                            subAmount = 2f * worker.GetHPRecoveryMult(healAmount);
                        }
                        else if (list.Count > 5)
                        {
                            subAmount = ((float)list.Count / 5f) * worker.GetHPRecoveryMult(healAmount);
                        }
                        else if (list.Count > 1)
                        {
                            subAmount = 1f * worker.GetHPRecoveryMult(healAmount);
                        }
                        foreach (AgentModel agent in list)
                        {
                            agent.hp -= subAmount;
                        }
                        worker.RecoverHPv2(healAmount);
                    }
                }
            }
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(4f);
            }
            if (worker.hp <= (float)worker.maxHp * 0.3f && !_recoverTimer2.started)
            {
                _recoverTimer2.StartTimer(2f);
            }
            if (worker.hp >= (float)worker.maxHp * 0.7f && _recoverTimer2.started)
            {
                _recoverTimer2.StopTimer();
            }
        }

        public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
        {
            return 25f;
        }

        private WorkerModel worker;

        private Timer _recoverTimer = new Timer();

        private Timer _recoverTimer2 = new Timer();
    }
}
