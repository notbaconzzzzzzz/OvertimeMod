using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconBalloon : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconBalloonAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        public override bool isAttackInWorkProcess()
        {
            return false;
        }

        public override void OnSkillSuccessWorkTick(UseSkill skill)
        {
            if (skill.IsWorkPlaying)
            {
                DamageInfo damageInfo = model.metaInfo.workDamage.Copy();
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
                skill.targetCreatureView.room.OnDamageInvoked(damageInfo);
            }
        }

        public override void OnWorkClosed(UseSkill skill, int successCount)
        {
            qliphothChange = 0;
            if (skill.agent.IsPanic())
            {
                model.SetQliphothCounter(0);
                return;
            }
            if (skill.agent.mental / (float)skill.agent.maxMental > 0.75f)
            {
                qliphothChange = -1;
            }
            else if (skill.agent.mental / (float)skill.agent.maxMental <= 1f / 3f)
            {
                qliphothChange = 1;
            }
        }

        public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
        {
            if (qliphothChange >= 1)
            {
                model.AddQliphothCounter();
            }
            else if (qliphothChange <= -1)
            {
                model.SubQliphothCounter();
            }
            qliphothChange = 0;
        }

        public override void OnReleaseWork(UseSkill skill)
        {
            if (model.feelingState == CreatureFeelingState.GOOD)
            {

            }
        }

        public override float GetDamageFactor(UnitModel target, DamageInfo damage)
        {
            if (target is WorkerModel || target is RabbitModel)
            {
                return 0.05f;
            }
            return base.GetDamageFactor(target, damage);
        }

        private int qliphothChange;

        private NotbaconBalloonAnim animscript;
    }
}
