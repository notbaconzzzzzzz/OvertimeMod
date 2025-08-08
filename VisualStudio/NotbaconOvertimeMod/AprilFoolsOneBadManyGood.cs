using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace NotbaconOvertimeMod
{
    public class AprilFoolsOneBadManyGood : CreatureBase
    {
        public override void OnStageStart()
        {
            qliphothSubTimer.StartTimer(60f);
        }

        public override void ActivateQliphothCounter()
        {
            base.ActivateQliphothCounter();
            Escape();
        }

        public override void OnReturn()
        {
            model.ResetQliphothCounter();
            ParamInit();
            qliphothSubTimer.StartTimer(60f);
        }

        public override void ParamInit()
        {
            currentTargetNode = null;
            encounterWorker = new List<WorkerModel>();
        }

        public override void OnSkillSuccessWorkTick(UseSkill skill)
        {
            if (skill.IsWorkPlaying)
            {
                DamageInfo damageInfo = model.metaInfo.workDamage.Copy();
                damageInfo.type = RwbpType.W;
                if (skill._isOverloadedCreature && skill._overloadType == OverloadType.PAIN)
                {
                    damageInfo *= 1.5f;
                    damageInfo.min += 2f;
                    damageInfo.max += 2f;
                }
                skill.agent.TakeDamage(skill.targetCreature, damageInfo);
                if (!skill.agent.ForceHideUI)
                {
                    RwbpType type = damageInfo.type;
                    DefenseInfo defense = skill.agent.defense;
                    UnitDirection dir = UnitDirection.LEFT;
                    DamageParticleEffect.Invoker(skill.agent, type, defense, dir);
                }
                skill.room.DamageFilter.sprite = CreatureLayer.IsolateRoomUIData.GetDamageSprite(RwbpType.W);
                skill.targetCreatureView.room.OnDamageInvoked(damageInfo);
            }
        }

        public override void OnSkillFailWorkTick(UseSkill skill)
        {
            if (skill.IsWorkPlaying)
            {
                skill.room.DamageFilter.sprite = CreatureLayer.IsolateRoomUIData.GetDamageSprite(RwbpType.P);
            }
        }

        public override void OnEnterRoom(UseSkill skill)
        {
            qliphothSubTimer.StopTimer();
        }

        public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
        {
            int amount = 0;
            if (oldState != CreatureFeelingState.BAD)
            {
                if (oldState == CreatureFeelingState.NORM)
                {
                    amount = 1;
                }
            }
            else
            {
                amount = 2;
            }
            for (int i = 0; i < amount; i++)
            {
                model.SubQliphothCounter();
            }
            qliphothSubTimer.StartTimer(60f);
        }

        public override void OnFixedUpdate(CreatureModel creature)
        {
            if (qliphothSubTimer.started)
            {
                if (model.IsEscaped())
                {
                    qliphothSubTimer.StopTimer();
                }
                if (qliphothSubTimer.RunTimer())
                {
                    model.SubQliphothCounter();
                    if (model.qliphothCounter > 0)
                    {
                        qliphothSubTimer.StartTimer(60f);
                    }
                }
            }
        }

        public override void Escape()
        {
            model.Escape();
            attackTimer.StartTimer(5f);
            specialAbilityTimer.StartTimer(30f);
            overloadAmount = 3;
            MakeMovement();
        }

        public override void OnSuppressed()
        {
            model.ClearCommand();
            attackTimer.StopTimer();
            specialAbilityTimer.StopTimer();
        }

        public override void UniqueEscape()
        {
            List<WorkerModel> near = new List<WorkerModel>(CheckNearWorker());
            if (attackTimer.RunTimer())
            {
                NearDamage(near);
                attackTimer.StartTimer(5f);
            }
            if (specialAbilityTimer.RunTimer())
            {
                ShockwaveEffect.Invoker(model.GetCurrentViewPosition(), model, 2f, 100f, EffectLifetimeType.NORMAL);
                SinOverload overload = new SinOverload(this);
                overload.CastOverload(overloadAmount);
                overloadAmount += 1;
                if (overloadAmount > 12) overloadAmount = 12;
                specialAbilityTimer.StartTimer(60f);
            }
            if (currentTargetNode != null && !movable.IsMoving())
            {
                if (movable.GetCurrentNode() == currentTargetNode)
                {
                    currentTargetNode = null;
                }
                MakeMovement();
            }
            if (movable.currentPassage == null)
            {
                model.movementScale = 10f;
            }
            else
            {
                model.movementScale = 1f;
            }
        }

        // Token: 0x060028CC RID: 10444 RVA: 0x0011F764 File Offset: 0x0011D964
        private void NearDamage(List<WorkerModel> near)
        {
            if (near == null || near.Count == 0)
            {
                return;
            }
            for (int i = 0; i < near.Count; i++)
            {
                near[i].TakeDamage(model, new DamageInfo(RwbpType.P, 10f, 20f));
                DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(near[i], RwbpType.P, model);
                near[i].TakeDamage(model, new DamageInfo(RwbpType.W, 50f, 70f));
                DamageParticleEffect damageParticleEffect2 = DamageParticleEffect.Invoker(near[i], RwbpType.W, model);
            }
        }

        private void MakeMovement()
        {
            if (model.state == CreatureState.SUPPRESSED)
            {
                return;
            }
            if (model.state == CreatureState.SUPPRESSED_RETURN)
            {
                return;
            }
            if (currentTargetNode == null)
            {
                Sefira[] sefiras = SefiraManager.instance.GetActivatedSefiras();
                Sefira sefira = sefiras[UnityEngine.Random.Range(0, sefiras.Length)];
                PassageObjectModel passage = sefira.sefiraPassage;
                currentTargetNode = passage.GetNodeList()[passage.GetNodeList().Length / 2];
            }
            movable.MoveToNode(currentTargetNode, false);
        }

        private void StopMovement()
        {
            movable.StopMoving();
        }

        private List<WorkerModel> CheckNearWorker()
        {
            List<WorkerModel> list = new List<WorkerModel>();
            if (currentPassage == null)
            {
                return list;
            }
            foreach (MovableObjectNode movableObjectNode in currentPassage.GetEnteredTargets())
            {
                UnitModel unit = movableObjectNode.GetUnit();
                if (unit.hp > 0f)
                {
                    if (unit is WorkerModel)
                    {
                        WorkerModel workerModel = unit as WorkerModel;
                        if (!list.Contains(workerModel))
                        {
                            list.Add(workerModel);
                            if (!encounterWorker.Contains(workerModel))
                            {
                                encounterWorker.Add(workerModel);
                                if (workerModel is OfficerModel)
                                {
                                    (workerModel as OfficerModel).EncounterCreature(model);
                                }
                                workerModel.InitialEncounteredCreature(model);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public void OverloadSuccess()
        {
            foreach (AgentModel agent in AgentManager.instance.GetAgentList())
            {
                if (!agent.IsDead())
                {
                    agent.RecoverHPv2(66f);
                }
            }
        }

        public void OverloadFail()
        {
            foreach (AgentModel agent in AgentManager.instance.GetAgentList())
            {
                if (!agent.IsDead())
                {
                    agent.TakeDamage(new DamageInfo(RwbpType.P, 66f));
                }
            }
        }

        private Timer qliphothSubTimer = new Timer();

        private Timer attackTimer = new Timer();

        private Timer specialAbilityTimer = new Timer();

        private MapNode currentTargetNode;

        private List<WorkerModel> encounterWorker = new List<WorkerModel>();

        private int overloadAmount;

        public class SinOverload : IObserver
        {
            public SinOverload(AprilFoolsOneBadManyGood _oneSin)
            {
                oneSin = _oneSin;
                Notice.instance.Observe(NoticeName.OnIsolateOverloaded, this);
                Notice.instance.Observe(NoticeName.OnIsolateOverloadCanceled, this);
                Notice.instance.Observe(NoticeName.Update, this);
            }

            public void OnDestroy()
            {
                Notice.instance.Remove(NoticeName.OnIsolateOverloaded, this);
                Notice.instance.Remove(NoticeName.OnIsolateOverloadCanceled, this);
                Notice.instance.Remove(NoticeName.Update, this);
            }

            public void CastOverload(int overloadTargetCount)
            {
                overloadedCreatures.AddRange(CreatureOverloadManager.instance.ActivateOverload(overloadTargetCount, OVERLOAD_TYPE, DURATION, false, true, true, new long[0]));
                if (overloadedCreatures.Count <= 0)
                {
                    OnSuccess();
                }
            }

            public int GetCreatureCount()
            {
                return overloadedCreatures.Count;
            }

            public void OnSuccess()
            {
                oneSin.OverloadSuccess();
                OnDestroy();
            }

            public void OnFail()
            {
                oneSin.OverloadFail();
            }

            public void OnReducedCreature(CreatureModel creature)
            {
                if (overloadedCreatures.Contains(creature))
                {
                    overloadedCreatures.Remove(creature);
                    if (overloadedCreatures.Count == 0)
                    {
                        OnSuccess();
                    }
                }
            }

            public void OnNotice(string notice, params object[] param)
            {
                if (notice == NoticeName.OnIsolateOverloaded)
                {
                    CreatureModel item = param[0] as CreatureModel;
                    OverloadType overloadType = (OverloadType)param[1];
                    if (overloadType == OverloadType.SIN && overloadedCreatures.Contains(item))
                    {
                        OnFail();
                        return;
                    }
                }
                else if (notice == NoticeName.OnIsolateOverloadCanceled)
                {
                    CreatureModel creatureModel = param[0] as CreatureModel;
                    OverloadType overloadType2 = (OverloadType)param[1];
                    if (overloadType2 == OverloadType.SIN && overloadedCreatures.Contains(creatureModel))
                    {
                        OnReducedCreature(creatureModel);
                    }
                }
                else if (notice == NoticeName.OnProcessWorkTick)
                {
                    CreatureModel creatureModel = param[0] as CreatureModel;
                    if (overloadedCreatures.Contains(creatureModel))
                    {
                        foreach (AgentModel agent in AgentManager.instance.GetAgentList())
                        {
                            if (!agent.IsDead())
                            {
                                agent.RecoverMentalv2(1f);
                            }
                        }
                    }
                }
                else if (notice == NoticeName.Update)
                {
                    Update();
                }
            }

            public void Update()
            {
                foreach (CreatureModel creatureModel in overloadedCreatures)
                {
                    if (!creatureModel.isOverloaded)
                    {
                        overloadedCreatures.Remove(creatureModel);
                        overloadedCreatures.AddRange(CreatureOverloadManager.instance.ActivateOverload(1, OVERLOAD_TYPE, DURATION, false, true, true));
                    }
                }
            }

            public List<CreatureModel> overloadedCreatures = new List<CreatureModel>();

            public AprilFoolsOneBadManyGood oneSin;

            public const OverloadType OVERLOAD_TYPE = OverloadType.SIN;

            public const float DURATION = 45f;
        }
    }
}