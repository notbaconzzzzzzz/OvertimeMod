using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconCoralReef : CreatureBase, IObserver
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconCoralReefAnim)unit.animTarget;
            animscript.SetScript(this);
            ParamInit();
        }

        public override void ParamInit()
        {
            state = CoralReefState.CONTAINED;
            attackActive = false;
            stateTimer = 0f;
            forceDirection = UnitDirection.OTHER;
            damageTaken = 0f;
            model.movementScale = 1f;
        }

        public override void OnStageStart()
        {
            base.OnStageStart();
            badWorkResults = 0;
            effects = new List<WaveEffect>();
            attackDelays = new List<AttackDelay>();
            ParamInit();
            animscript.Return();
            WavePrefabW = Add_On.GetBundle("overtimeabnoscommon").LoadAsset<GameObject>("CoralReefWaveW");
            if (WavePrefabW.GetComponent<EtcUnit>() == null) WavePrefabW.AddComponent<EtcUnit>();
            WavePrefabB = Add_On.GetBundle("overtimeabnoscommon").LoadAsset<GameObject>("CoralReefWaveB");
            if (WavePrefabB.GetComponent<EtcUnit>() == null) WavePrefabB.AddComponent<EtcUnit>();
        }

        public override void OnFinishWork(UseSkill skill)
        {
            EnergyModel.instance.AddEnergy((float)(skill.successCount * 2));
        }

        public override CreatureFeelingState ModifyFeelingState(UseSkill skill, CreatureFeelingState state)
        {
            if (skill.skillTypeInfo.id == 4L)
            {
                return CreatureFeelingState.BAD;
            }
            return base.ModifyFeelingState(skill, state);
        }

        public override void OnReleaseWork(UseSkill skill)
        {
            if (model.GetFeelingState() == CreatureFeelingState.BAD)
            {
                badWorkResults++;
                alreadySub = false;
                if (GetQliphothCounterMax() < model.qliphothCounter)
                {
                    model.SubQliphothCounter();
                    alreadySub = true;
                }
            }
        }

        public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
        {
            if (oldState == CreatureFeelingState.NORM)
            {
                model.SubQliphothCounter();

            }
            else if (oldState == CreatureFeelingState.BAD)
            {
                badWorkResults++;
                if (!alreadySub) model.SubQliphothCounter();
                model.SubQliphothCounter();
                alreadySub = false;
            }
        }

        public override float TranformWorkProb(float originWorkProb)
        {
            return Mathf.Max(1f - (float)badWorkResults / 6f, 0f) * base.TranformWorkProb(originWorkProb);
        }

        public override int GetQliphothCounterMax()
        {
            switch (badWorkResults)
            {
                case 0:
                case 1:
                    return 4;
                case 2:
                case 3:
                    return 3;
                case 4:
                case 5:
                    return 2;
                case 6:
                default:
                    return 1;
            }
        }

        public override void ActivateQliphothCounter()
        {
            Escape();
        }

        public override void Escape()
        {
            forceDirection = UnitDirection.OTHER;
            model.Escape();
            switch (badWorkResults)
            {
                case 0:
                    model.baseMaxHp = 300; model.SetDefenseId("1"); break;
                case 1:
                    model.baseMaxHp = 400; model.SetDefenseId("2"); break;
                case 2:
                    model.baseMaxHp = 400; model.SetDefenseId("3"); break;
                case 3:
                    model.baseMaxHp = 500; model.SetDefenseId("4"); break;
                case 4:
                    model.baseMaxHp = 500; model.SetDefenseId("5"); break;
                case 5:
                    model.baseMaxHp = 600; model.SetDefenseId("6"); break;
                case 6:
                default:
                    model.baseMaxHp = 700; model.SetDefenseId("7"); break;
            }
            model.hp = (float)model.baseMaxHp;
            model.movementScale = 1f;
            state = CoralReefState.BREACHING;
            attackActive = false;
            stateTimer = 0f;
            eatTarget = null;
            animscript.Breach();
            hideCooldown.StartTimer(5f);
            screamCooldown.StartTimer(10f);
            eatCooldown.StartTimer(15f);
            hideTimes = 0;
            screamTimes = 0;
            SetObserver(true);
        }

        public override void OnReturn()
        {
            forceDirection = UnitDirection.OTHER;
            model.ResetQliphothCounter();
            state = CoralReefState.CONTAINED;
            attackActive = false;
            stateTimer = 0f;
            animscript.Return();
            SetObserver(false);
        }

        public override void OnStageRelease()
        {
            SetObserver(false);
        }

        public override void OnStageEnd()
        {
            SetObserver(false);
        }

        private void SetObserver(bool active)
        {
            if (active)
            {
                Notice.instance.Observe(NoticeName.FixedUpdate, this);
            }
            else
            {
                Notice.instance.Remove(NoticeName.FixedUpdate, this);
            }
        }

        public void OnNotice(string notice, params object[] param)
        {
            if (notice == NoticeName.FixedUpdate)
            {
                if (forceDirection != UnitDirection.OTHER)
                {
                    model.GetMovableNode().SetDirection(forceDirection);
                }
            }
        }

        public override void OnFixedUpdate(CreatureModel creature)
        {
            foreach (WaveEffect wave in effects)
            {
                wave.Process();
            }
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                if (!effects[i].IsEnable())
                {
                    Notice.instance.Send(NoticeName.RemoveEtcUnit, new object[]
                    {
                        effects[i]
                    });
                    effects.RemoveAt(i);
                }
            }
            for (int j = attackDelays.Count - 1; j >= 0; j--)
            {
                attackDelays[j].Process();
                if (!attackDelays[j].IsEnable())
                {
                    attackDelays.RemoveAt(j);
                }
            }
        }

        public override void UniqueEscape()
        {
            if (model.hp <= 0f) return;
            CheckNear();
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            if (passage == null || prevPassage != passage)
            {
                forceDirection = UnitDirection.OTHER;
            }
            prevPassage = passage;
            stateTimer += Time.fixedDeltaTime;
            /*
            Notice.instance.Send(NoticeName.AddSystemLog, new object[]
            {
                state.ToString() + " - " + stateTimer.ToString("0.00")
            });*/
            if (state != CoralReefState.HIDING && hideCooldown.started) hideCooldown.RunTimer();
            if (state != CoralReefState.SCREAM && screamCooldown.started) screamCooldown.RunTimer();
            if (state != CoralReefState.EAT_ATTACK && eatCooldown.started) eatCooldown.RunTimer();
            if (state == CoralReefState.IDLE)
            {
                if (stateTimer >= waitTime && passage != null)
                {
                    TryNextAction();
                }
                if (state == CoralReefState.IDLE && !model.GetMovableNode().IsMoving())
                {
                    if ((leftBias <= 0f && rightBias <= 0f) || passage == null)
                    {
                        if (stateTimer >= waitTime)
                        {
                            MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
                            model.MoveToNode(creatureRoamingPoint);
                            animscript.Move(false);
                            forceDirection = UnitDirection.OTHER;
                        }
                    }
                    else
                    {
                        float rand = UnityEngine.Random.Range(-leftBias, rightBias);
                        if (rand < 0f)
                        {
                            MovableObjectNode node = model.GetMovableNode().GetSideMovableNode(UnitDirection.LEFT, 3f);
                            float dist = MovableObjectNode.GetDistance(node, model.GetMovableNode());
                            if (node.currentPassage == passage && dist > 1f && dist < 10f)
                            {
                                model.GetMovableNode().MoveToMovableNode(node);
                                animscript.Move(forceDirection == UnitDirection.RIGHT);
                            }
                            else
                            {
                                if (model.GetMovableNode().GetCurrentViewPosition().x - passage.centerNode.GetPosition().x > 1f)
                                {
                                    model.GetMovableNode().MoveToNode(passage.centerNode);
                                }
                                else
                                {
                                    model.GetMovableNode().MoveToNode(passage.GetLeft());
                                }
                                animscript.Move(forceDirection == UnitDirection.RIGHT);
                            }
                        }
                        else
                        {
                            MovableObjectNode node = model.GetMovableNode().GetSideMovableNode(UnitDirection.RIGHT, 3f);
                            float dist = MovableObjectNode.GetDistance(node, model.GetMovableNode());
                            if (node.currentPassage == passage && dist > 1f && dist < 10f)
                            {
                                model.GetMovableNode().MoveToMovableNode(node);
                                animscript.Move(forceDirection == UnitDirection.LEFT);
                            }
                            else
                            {
                                if (model.GetMovableNode().GetCurrentViewPosition().x - passage.centerNode.GetPosition().x < -1f)
                                {
                                    model.GetMovableNode().MoveToNode(passage.centerNode);
                                }
                                else
                                {
                                    model.GetMovableNode().MoveToNode(passage.GetRight());
                                }
                                animscript.Move(forceDirection == UnitDirection.LEFT);
                            }
                        }
                    }
                }
            }
            else if (stateTimer >= 20f)
            {
                OnAttackEnd();
            }
            else if (state == CoralReefState.HIDING)
            {
                if (waveTimer.RunTimer())
                {
                    try
                    {
                        WaveEffect wave = new WaveEffect(this, (forceDirection == UnitDirection.OTHER) ? model.GetDirection() : forceDirection, passage);
                        effects.Add(wave);
                        if (badWorkResults >= 3)
                        {
                            EtcUnit unit = CreatureLayer.currentLayer.AddEtcUnit(wave, Add_On.LoadObject("overtimeabnoscommon", "CoralReefWaveB"));
                        }
                        else
                        {
                            EtcUnit unit = CreatureLayer.currentLayer.AddEtcUnit(wave, Add_On.LoadObject("overtimeabnoscommon", "CoralReefWaveW"));
                        }
                        waveTimer.StartTimer(0.6f);
                    }
                    catch (Exception ex)
                    {
                        Notice.instance.Send(NoticeName.AddSystemLog, new object[]
                        {
                            ex.Message + " : " + ex.StackTrace
                        });
                    }
                }
                if (attackActive && stateTimer >= 10f)
                {
                    attackActive = false;
                    stateTimer = 0f;
                    animscript.FinishAttack(state);
                    waveTimer.StopTimer();
                }
            }
            else if (state == CoralReefState.SCREAM)
            {
                if (attackActive && stateTimer >= 6f)
                {
                    attackActive = false;
                    stateTimer = 0f;
                    animscript.FinishAttack(state);
                }
            }
            else if (state == CoralReefState.EAT_ATTACK)
            {
                if (attackActive)
                {
                    ProccessEatAttack();
                }
            }
        }

        public void CheckNear()
        {
            leftBias = 0f;
            rightBias = 0f;
            leftClosest = 999f;
            rightClosest = 999f;
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            if (passage == null) return;
            List<WorkerModel> list = model.GetNearWorkerEncounted();
            float x = model.GetMovableNode().GetCurrentViewPosition().x / passage.scaleFactor;
            foreach (WorkerModel workerModel in list)
            {
                float x2 = workerModel.GetMovableNode().GetCurrentViewPosition().x / passage.scaleFactor;
                x2 -= x;
                if (x2 == 0f)
                {
                    leftClosest = 0f;
                    rightClosest = 0f;
                    leftBias += 1f;
                    rightBias += 1f;
                }
                else if (x2 > 0f)
                {
                    if (x2 < rightClosest) rightClosest = x2;
                    rightBias += 1f / Mathf.Max(x2, 1f);
                }
                else
                {
                    if (-x2 < leftClosest) leftClosest = -x2;
                    leftBias += 1f / Mathf.Max(-x2, 1f);
                }
            }
        }

        public void TryNextAction()
        {
            CoralReefState nextAction = CoralReefState.IDLE;
            UnitDirection direction = UnitDirection.OTHER;
            List<CoralReefState> actions = new List<CoralReefState>();
            List<float> weights = new List<float>();
            if (!hideCooldown.started)
            {
                // if (leftClosest <= 12f || rightClosest <= 12f) { actions.Add(CoralReefState.HIDING); weights.Add(2f); }
            }
            if (leftClosest <= 2.75f || rightClosest <= 2.75f) { actions.Add(CoralReefState.CLAW_ATTACK_1); weights.Add(20f); } else { actions.Add(CoralReefState.IDLE); weights.Add(100f); }
            if (leftClosest <= 3f || rightClosest <= 3f) { actions.Add(CoralReefState.CLAW_ATTACK_2); weights.Add(20f); } else { actions.Add(CoralReefState.IDLE); weights.Add(100f); }
            if (badWorkResults >= 2)
            {
                if (leftClosest <= 3.25f || rightClosest <= 3.25f) { actions.Add(CoralReefState.SLAM_ATTACK); weights.Add(25f); } else { actions.Add(CoralReefState.IDLE); weights.Add(100f); }
            }
            if (badWorkResults >= 4 && !screamCooldown.started)
            {
                // if (leftClosest <= 18f || rightClosest <= 18f) { actions.Add(CoralReefState.SCREAM); weights.Add(3f); }
            }
            if (badWorkResults >= 6 && !eatCooldown.started)
            {
                if (leftClosest <= 30f || rightClosest <= 30f) { actions.Add(CoralReefState.EAT_ATTACK); weights.Add(30f); }
            }
            float rand = 0f;
            foreach (float f in weights)
            {
                rand += f;
            }
            rand = UnityEngine.Random.Range(0f, rand);
            int i = -1;
            foreach (float f in weights)
            {
                i++;
                rand -= f;
                if (rand < 0f) break;
            }
            nextAction = actions[i];
            switch (nextAction)
            {
                case CoralReefState.CLAW_ATTACK_1:
                case CoralReefState.CLAW_ATTACK_2:
                case CoralReefState.SLAM_ATTACK:
                case CoralReefState.EAT_ATTACK:
                case CoralReefState.HIDING:
                    rand = UnityEngine.Random.Range(-leftBias, rightBias);
                    if (rand < 0f)
                    {
                        direction = UnitDirection.LEFT;
                    }
                    else if (rand > 0f)
                    {
                        direction = UnitDirection.RIGHT;
                    }
                    else
                    {
                        direction = (forceDirection == UnitDirection.OTHER) ? model.GetDirection() : forceDirection;
                    }
                    break;
            }
            if (nextAction != CoralReefState.IDLE)
            {
                state = nextAction;
                stateTimer = 0f;
                model.ClearCommand();
                model.GetMovableNode().StopMoving();
                if (direction != UnitDirection.OTHER)
                {
                    model.GetMovableNode().SetDirection(direction);
                    forceDirection = direction;
                }
                animscript.InitiateAttack(state);
                switch (state)
                {
                    case CoralReefState.HIDING:
                        attackActive = false;
                        hideCooldown.StartTimer(5f);
                        waveTimer.StartTimer(0.6f);
                        hideTimes++;
                        break;
                    case CoralReefState.SCREAM:
                        attackActive = false;
                        screamCooldown.StartTimer(10f);
                        screamTimes++;
                        break;
                    case CoralReefState.EAT_ATTACK:
                        attackActive = false;
                        eatTarget = null;
                        eatCooldown.StartTimer(15f);
                        break;
                }
            }
            else
            {
                if (leftBias <= 0f && rightBias <= 0f && !model.GetMovableNode().IsMoving())
                {
                    MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
                    model.MoveToNode(creatureRoamingPoint);
                    animscript.Move(false);
                    forceDirection = UnitDirection.OTHER;
                }
                stateTimer = waitTime - 0.1f;
            }
        }

        public DamageInfo[] GetAttackDamage(CoralReefState s)
        {
            int ind = badWorkResults;
            if (ind > 6) ind = 6;
            if (s < CoralReefState.HIDING || s > CoralReefState.EAT_ATTACK) return new DamageInfo[0];
            return DamageArray[(int)s][ind];
        }

        public void GiveAttackDamage()
        {
            DamageInfo[] damageInfos = GetAttackDamage(state);
            switch (state)
            {
                case CoralReefState.CLAW_ATTACK_1:
                    DealAttackDamage(damageInfos, 3.25f, true); break;
                case CoralReefState.CLAW_ATTACK_2:
                    DealAttackDamage(damageInfos, 3.5f, true); break;
                case CoralReefState.SLAM_ATTACK:
                    DealAttackDamage(damageInfos, -0.5f, 3.75f); break;
                case CoralReefState.SCREAM:
                    DealAttackDamage(damageInfos, -999f, 999f); break;
                case CoralReefState.EAT_ATTACK:
                    PassageObjectModel passage = model.GetMovableNode().currentPassage;
                    if (passage != null && eatTarget != null && eatTarget.GetMovableNode().currentPassage == passage) DealAttackDamage(damageInfos, eatTarget); break;
            }
        }

        public void DealAttackDamage(DamageInfo damageInfo, float range, bool halvePiercing = false)
        {
            DealAttackDamage(new DamageInfo[] { damageInfo }, 0f, range, halvePiercing);
        }

        public void DealAttackDamage(DamageInfo damageInfo, float rangeMin, float rangeMax, bool halvePiercing = false)
        {
            DealAttackDamage(new DamageInfo[] { damageInfo }, rangeMin, rangeMax, halvePiercing);
        }

        public void DealAttackDamage(DamageInfo[] damageInfos, float range, bool halvePiercing = false)
        {
            DealAttackDamage(damageInfos, 0f, range, halvePiercing);
        }

        public void DealAttackDamage(DamageInfo[] damageInfos, float rangeMin, float rangeMax, bool halvePiercing = false)
        {
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            List<UnitModel> list = new List<UnitModel>();
            float x = model.GetMovableNode().GetCurrentViewPosition().x / passage.scaleFactor;
            float closestX = 999f;
            UnitModel closest = null;
            foreach (MovableObjectNode movableNode in passage.GetEnteredTargets())
            {
                UnitModel unit = movableNode.GetUnit();
                if (unit == model || unit.hp <= 0f || !unit.IsAttackTargetable()) continue;
                float x2 = movableNode.GetCurrentViewPosition().x / passage.scaleFactor;
                x2 -= x;
                if ((forceDirection == UnitDirection.OTHER) ? (model.GetDirection() == UnitDirection.LEFT) : (forceDirection == UnitDirection.LEFT)) x2 *= -1f;
                if (x2 < rangeMin - unit.radius || x2 > rangeMax + unit.radius) continue;
                if (x2 < closestX)
                {
                    closestX = x2;
                    closest = unit;
                }
                list.Add(unit);
            }
            if (halvePiercing && closest != null)
            {
                DealAttackDamage(damageInfos, closest);
                list.Remove(closest);
                DamageInfo[] damageInfos2 = new DamageInfo[damageInfos.Length];
                for (int i = 0; i < damageInfos.Length; i++)
                {
                    damageInfos2[i] = damageInfos[i] * 0.5f;
                }
                DealAttackDamage(damageInfos2, list);
            }
            else
            {
                DealAttackDamage(damageInfos, list);
            }
        }

        public void DealAttackDamage(DamageInfo damageInfo, UnitModel target)
        {
            DealAttackDamage(new DamageInfo[] { damageInfo }, target);
        }

        public void DealAttackDamage(DamageInfo[] damageInfos, UnitModel target)
        {
            DealAttackDamage(damageInfos, new List<UnitModel>() { target });
        }

        public void DealAttackDamage(DamageInfo damageInfo, List<UnitModel> targets)
        {
            DealAttackDamage(new DamageInfo[] { damageInfo }, targets);
        }

        public void DealAttackDamage(DamageInfo[] damageInfos, List<UnitModel> targets)
        {
            foreach (UnitModel target in targets)
            {
                if (target.hp <= 0f || !target.IsAttackTargetable()) return;
                foreach (DamageInfo d in damageInfos)
                {
                    target.TakeDamage(model, d);
                    DamageParticleEffect.Invoker(target, d.type, model);
                }
            }
        }

        public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
        {
            if (state == CoralReefState.HIDING && attackActive)
            {
                float d = dmg.GetDamage();
                if (dmg.type == RwbpType.R) d *= 2f;
                damageTaken += d;
                float factor = 30f;
                switch (badWorkResults)
                {
                    case 0:
                    case 1:
                        factor = 30f;
                        break;
                    case 2:
                    case 3:
                        factor = 45f;
                        break;
                    case 4:
                    case 5:
                        factor = 60f;
                        break;
                    case 6:
                    default:
                        factor = 75f;
                        break;
                }
                factor *= (10f - stateTimer);
                if (damageTaken > factor)
                {
                    attackActive = false;
                    stateTimer = 0f;
                    animscript.FinishAttack(state);
                    waveTimer.StopTimer();
                    damageTaken = 0f;
                }
                return false;
            }
            return true;
        }

        public override float GetDamageFactor(UnitModel target, DamageInfo damage)
        {
            if (state == CoralReefState.HIDING) return 0.5f;
            if (state == CoralReefState.SCREAM && !attackActive) return 0.5f;
            return 1f;
        }

        public override void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
        {
            float d = dmg.result.resultDamage;
            if (state == CoralReefState.IDLE)
            {
                if (!hideCooldown.started && UnityEngine.Random.Range(0f, 250f - (float)model.baseMaxHp + model.hp + 200f * hideTimes) < d)
                {
                    state = CoralReefState.HIDING;
                    attackActive = false;
                    stateTimer = 0f;
                    model.ClearCommand();
                    model.GetMovableNode().StopMoving();
                    UnitDirection direction = (forceDirection == UnitDirection.OTHER) ? model.GetDirection() : forceDirection;
                    if (direction != UnitDirection.OTHER)
                    {
                        model.GetMovableNode().SetDirection(direction);
                        forceDirection = direction;
                    }
                    animscript.InitiateAttack(state);
                    hideCooldown.StartTimer(5f);
                    waveTimer.StartTimer(0.6f);
                    hideTimes++;
                }
                else if (badWorkResults >= 4 && !screamCooldown.started && UnityEngine.Random.Range(0f, 400f - (float)model.baseMaxHp + model.hp + 300f * screamTimes) < d)
                {
                    state = CoralReefState.SCREAM;
                    attackActive = false;
                    stateTimer = 0f;
                    model.ClearCommand();
                    model.GetMovableNode().StopMoving();
                    UnitDirection direction = (forceDirection == UnitDirection.OTHER) ? model.GetDirection() : forceDirection;
                    if (direction != UnitDirection.OTHER)
                    {
                        model.GetMovableNode().SetDirection(direction);
                        forceDirection = direction;
                    }
                    animscript.InitiateAttack(state);
                    screamCooldown.StartTimer(10f);
                    screamTimes++;
                }
            }
            else if (state == CoralReefState.SCREAM && attackActive)
            {
                damageTaken += d;
                if (UnityEngine.Random.Range(0f, 25f - damageTaken) < d)
                {
                    damageTaken -= 25f;
                    CreatureModel minCreature = null;
                    float minDist = 999999f;
                    foreach (CreatureModel creature in CreatureManager.instance.GetCreatureList())
                    {
                        if (creature.IsEscaped() || creature.qliphothCounter <= 0) continue;
                        float dist = (creature.GetCurrentViewPosition() - model.GetCurrentViewPosition()).magnitude;
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minCreature = creature;
                        }
                    }
                    if (minCreature != null)
                    {
                        minCreature.SubQliphothCounter();
                    }
                }
            }
        }

        public void ProccessEatAttack()
        {
            float rangeMin = 0f;
            float rangeMax = 1f;
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            float x = model.GetMovableNode().GetCurrentViewPosition().x / passage.scaleFactor;
            float closestX = 999f;
            UnitModel closest = null;
            foreach (MovableObjectNode movableNode in passage.GetEnteredTargets())
            {
                UnitModel unit = movableNode.GetUnit();
                if (unit == model || unit.hp <= 0f || !unit.IsAttackTargetable()) continue;
                float x2 = movableNode.GetCurrentViewPosition().x / passage.scaleFactor;
                x2 -= x;
                if ((forceDirection == UnitDirection.OTHER) ? (model.GetDirection() == UnitDirection.LEFT) : (forceDirection == UnitDirection.LEFT)) x2 *= -1f;
                if (x2 < rangeMin - unit.radius || x2 > rangeMax + unit.radius) continue;
                if (x2 < closestX)
                {
                    closestX = x2;
                    closest = unit;
                }
            }
            if (closest != null)
            {
                attackActive = false;
                stateTimer = 0f;
                eatTarget = closest;
                eatTarget.AddUnitBuf(new CoralReefEatDebuf());
                animscript.FinishAttack(state);
                model.ClearCommand();
                model.GetMovableNode().StopMoving();
                model.movementScale = 1f;
            }
            else if (!model.GetMovableNode().IsMoving())
            {
                attackActive = false;
                stateTimer = 0f;
                animscript.FinishAttack(state);
                model.ClearCommand();
                model.GetMovableNode().StopMoving();
                model.movementScale = 1f;
            }
        }

        public void OnBreach()
        {
            state = CoralReefState.IDLE;
            stateTimer = 0f;
            MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
            model.MoveToNode(creatureRoamingPoint);
            animscript.Move(false);
            forceDirection = UnitDirection.OTHER;
            waitTime = 0f;
        }

        public void OnAttackEnd()
        {
            model.movementScale = 1f;
            float minWaitTime = 5f / 6f;
            float maxWaitTime = 10f / 6f;
            switch (badWorkResults)
            {
                case 0:
                case 1:
                    maxWaitTime = 10f / 6f; break;
                case 2:
                case 3:
                    maxWaitTime = 9f / 6f; break;
                case 4:
                case 5:
                    maxWaitTime = 8f / 6f; break;
                case 6:
                default:
                    maxWaitTime = 7f / 6f; break;
            }
            switch (state)
            {
                case CoralReefState.HIDING:
                    minWaitTime *= 0.5f;
                    maxWaitTime *= 0.5f;
                    break;
                case CoralReefState.SCREAM:
                    minWaitTime *= 1.5f;
                    maxWaitTime *= 1.5f;
                    break;
                case CoralReefState.EAT_ATTACK:
                    minWaitTime *= 2f;
                    maxWaitTime *= 2f;
                    break;
            }
            waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            state = CoralReefState.IDLE;
            attackActive = false;
            stateTimer = 0f;
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            CheckNear();
            if (passage == null)
            {
                MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
                model.MoveToNode(creatureRoamingPoint);
                animscript.Move(false);
                forceDirection = UnitDirection.OTHER;
            }
            else if (leftBias <= 0f && rightBias <= 0f)
            {
                animscript.Idle();
            }
            else
            {
                float rand = UnityEngine.Random.Range(-leftBias, rightBias);
                if (rand < 0f)
                {
                    MovableObjectNode node = model.GetMovableNode().GetSideMovableNode(UnitDirection.LEFT, 3f);
                    float dist = MovableObjectNode.GetDistance(node, model.GetMovableNode());
                    if (node.currentPassage == passage && dist > 1f && dist < 10f)
                    {
                        model.GetMovableNode().MoveToMovableNode(node);
                        animscript.Move(forceDirection == UnitDirection.RIGHT);
                    }
                    else
                    {
                        if (model.GetMovableNode().GetCurrentViewPosition().x - passage.centerNode.GetPosition().x > 1f)
                        {
                            model.GetMovableNode().MoveToNode(passage.centerNode);
                        }
                        else
                        {
                            model.GetMovableNode().MoveToNode(passage.GetLeft());
                        }
                        animscript.Move(forceDirection == UnitDirection.RIGHT);
                    }
                }
                else
                {
                    MovableObjectNode node = model.GetMovableNode().GetSideMovableNode(UnitDirection.RIGHT, 3f);
                    float dist = MovableObjectNode.GetDistance(node, model.GetMovableNode());
                    if (node.currentPassage == passage && dist > 1f && dist < 10f)
                    {
                        model.GetMovableNode().MoveToMovableNode(node);
                        animscript.Move(forceDirection == UnitDirection.LEFT);
                    }
                    else
                    {
                        if (model.GetMovableNode().GetCurrentViewPosition().x - passage.centerNode.GetPosition().x < -1f)
                        {
                            model.GetMovableNode().MoveToNode(passage.centerNode);
                        }
                        else
                        {
                            model.GetMovableNode().MoveToNode(passage.GetRight());
                        }
                        animscript.Move(forceDirection == UnitDirection.LEFT);
                    }
                }
            }
        }

        public void OnHide()
        {
            attackActive = true;
            stateTimer = 0f;
            damageTaken = 0f;
        }

        public void OnScream()
        {
            attackActive = true;
            stateTimer = 0f;
            damageTaken = 0f;
        }

        public void OnJump()
        {
            eatTarget = null;
            attackActive = true;
            stateTimer = 0f;
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            if (passage == null)
            {
                OnAttackEnd();
                return;
            }
            model.movementScale = 10f;
            if ((forceDirection == UnitDirection.OTHER) ? (model.GetDirection() == UnitDirection.LEFT) : (forceDirection == UnitDirection.LEFT))
            {
                MapNode leftEdge = passage.GetLeft();
                model.MoveToNode(leftEdge);
            }
            else
            {
                MapNode rightEdge = passage.GetRight();
                model.MoveToNode(rightEdge);
            }
        }

        public void TrySkillAttack(UnitModel target, UnitDirection dir)
        {
            if (target == model)
            {
                return;
            }
            if (target.hp <= 0f)
            {
                return;
            }
            target.AddUnitBuf(new CoralReefWaveDebuf(dir));
            if (attackDelays.Exists((AttackDelay x) => x.GetModel() == target && x.IsEnable()))
            {
                return;
            }
            DealAttackDamage(GetAttackDamage(CoralReefState.HIDING), target);
            if (target.hp <= 0f)
            {
                return;
            }
            AttackDelay item = new AttackDelay(target, 0.6f);
            attackDelays.Add(item);
        }

        private NotbaconCoralReefAnim animscript;

        private int badWorkResults;
        private bool alreadySub;

        private UnitDirection forceDirection = UnitDirection.OTHER;

        private float leftBias;
        private float rightBias;
        private float leftClosest;
        private float rightClosest;

        private PassageObjectModel prevPassage;

        private UnitModel eatTarget;

        public CoralReefState state;
        public bool attackActive;
        public float stateTimer;

        private float waitTime;
        private Timer waveTimer = new Timer();

        private Timer hideCooldown = new Timer();
        private Timer screamCooldown = new Timer();
        private Timer eatCooldown = new Timer();
        private int hideTimes;
        private int screamTimes;

        private float damageTaken;

        public long nextEffectId = 10000L;
        public List<WaveEffect> effects = new List<WaveEffect>();
        private List<AttackDelay> attackDelays = new List<AttackDelay>();
        private GameObject WavePrefabW;
        private GameObject WavePrefabB;

        public enum CoralReefState
        {
            CONTAINED,
            IDLE,
            BREACHING,
            HIDING,
            CLAW_ATTACK_1,
            CLAW_ATTACK_2,
            SLAM_ATTACK,
            SCREAM,
            EAT_ATTACK
        }

        private DamageInfo[][][] DamageArray = new DamageInfo[][][]
        {
            new DamageInfo[][] {
            },
            new DamageInfo[][] {
            },
            new DamageInfo[][] {
            },
            new DamageInfo[][] { // Water Attack
                new DamageInfo[] { new DamageInfo(RwbpType.W, 2f, 3f) },
                new DamageInfo[] { new DamageInfo(RwbpType.W, 3f, 4f) },
                new DamageInfo[] { new DamageInfo(RwbpType.W, 3f, 4f) },
                new DamageInfo[] { new DamageInfo(RwbpType.B, 4f, 5f) },
                new DamageInfo[] { new DamageInfo(RwbpType.B, 4f, 5f) },
                new DamageInfo[] { new DamageInfo(RwbpType.B, 4f, 5f) },
                new DamageInfo[] { new DamageInfo(RwbpType.B, 5f, 7f) }
            },
            new DamageInfo[][] { // Claw Attack (1)
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 12f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 12f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 12f), new DamageInfo(RwbpType.B, 5f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 12f), new DamageInfo(RwbpType.B, 5f, 8f) }
            },
            new DamageInfo[][] { // Claw Attack (2)
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 10f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 10f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 5f, 10f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 15f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 15f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 15f), new DamageInfo(RwbpType.B, 5f, 10f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 7f, 15f), new DamageInfo(RwbpType.B, 5f, 10f) }
            },
            new DamageInfo[][] { // Slam Attack
                new DamageInfo[] { new DamageInfo(RwbpType.R, 12f, 20f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.R, 12f, 20f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.R, 12f, 20f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 12f, 20f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 18f, 24f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 18f, 24f), new DamageInfo(RwbpType.B, 2f, 4f) },
                new DamageInfo[] { new DamageInfo(RwbpType.R, 18f, 24f), new DamageInfo(RwbpType.B, 4f, 7f) }
            },
            new DamageInfo[][] { // Scream Attack
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.W, 4f, 8f) },
                new DamageInfo[] { new DamageInfo(RwbpType.W, 5f, 15f) }
            },
            new DamageInfo[][] { // Eat Attack
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }, // X
                new DamageInfo[] { new DamageInfo(RwbpType.B, 35f, 42f) }
            },
        };
        
        public class CoralReefEatDebuf : UnitBuf
        {
            public CoralReefEatDebuf()
            {
                type = UnitBufType.NOTBACON_CORAL_REEF_EAT;
                duplicateType = BufDuplicateType.ONLY_ONE;
            }

            public override void Init(UnitModel model)
            {
                base.Init(model);
                remainTime = 1.5f;
            }

            public override float MovementScale()
            {
                return 0.01f;
            }
        }

        public class CoralReefWaveDebuf : UnitBuf
        {
            public CoralReefWaveDebuf(UnitDirection dir)
            {
                type = UnitBufType.NOTBACON_CORAL_REEF_WAVE;
                duplicateType = BufDuplicateType.ONLY_ONE;
                direction = dir;
            }

            public override void Init(UnitModel model)
            {
                base.Init(model);
                remainTime = 0.6f;
            }

            public override float MovementScale()
            {
                if (model.GetMovableNode().GetDirection() == direction) return 1.5f;
                return 0.5f;
            }

            private UnitDirection direction;
        }

        public class WaveEffect : UnitModel
        {
            public WaveEffect(NotbaconCoralReef scr, UnitDirection dir, PassageObjectModel p)
            {
                script = scr;
                long nextEffectId;
                script.nextEffectId = (nextEffectId = script.nextEffectId) + 1L;
                instanceId = nextEffectId;
                movableNode = new MovableObjectNode(this);
                direction = dir;
                movableNode.SetDirection(direction);
                passage = p;
                if (direction == UnitDirection.RIGHT)
                {
                    movableNode.SetCurrentNode(passage.GetLeft());
                    goalNode = passage.GetRight();
                }
                else
                {
                    movableNode.SetCurrentNode(passage.GetRight());
                    goalNode = passage.GetLeft();
                }
                AddUnitBuf(new UnitStatBuf(float.MaxValue)
                {
                    duplicateType = BufDuplicateType.ONLY_ONE,
                    movementSpeed = 5f
                });
                enable = true;
                movableNode.MoveToNode(goalNode);
                removeTimer.StartTimer(15f);
            }

            public bool IsEnable()
            {
                return enable;
            }

            public void Process()
            {
                if (!enable)
                {
                    return;
                }
                if (passage != null)
                {
                    foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets(movableNode))
                    {
                        UnitModel unit = movableObjectNode.GetUnit();
                        if (unit != script.model && Math.Abs(movableObjectNode.GetCurrentViewPosition().x - movableNode.GetCurrentViewPosition().x) / passage.scaleFactor <= 1f)
                        {
                            script.TrySkillAttack(unit, direction);
                        }
                    }
                }
                movableNode.ProcessMoveNode(movement);
                if (!movableNode.IsMoving())
                {
                    OnArrive();
                }
                if (removeTimer.RunTimer())
                {
                    OnArrive();
                }
            }

            public void OnArrive()
            {
                enable = false;
            }

            public override bool IsEtcUnit()
            {
                return true;
            }

            public override bool CanOpenDoor()
            {
                return false;
            }

            public override bool IgnoreDoors()
            {
                return true;
            }

            private NotbaconCoralReef script;

            private MapNode goalNode;

            private PassageObjectModel passage;

            private UnitDirection direction;

            private bool enable = true;

            private Timer removeTimer = new Timer();

            private const float removeTime = 15f;
        }

        public class AttackDelay
        {
            public AttackDelay(UnitModel target, float remainDelay)
            {
                this.enable = true;
                this.target = target;
                this.remainDelay = remainDelay;
            }

            public void Process()
            {
                remainDelay -= Time.deltaTime;
                if (remainDelay <= 0f)
                {
                    enable = false;
                }
            }

            public UnitModel GetModel()
            {
                return target;
            }

            public bool IsEnable()
            {
                return enable;
            }

            private bool enable;

            private UnitModel target;

            private float remainDelay;
        }
    }
}
