using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconGravityRock : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconGravityRockAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        public override bool isAttackInWorkProcess()
        {
            return false;
        }

        public override void OnSkillFailWorkTick(UseSkill skill)
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
                if (damageInfo.result.hpDamage > 0f && !skill.agent.IsDead()) skill.agent.hp += damageInfo.result.hpDamage * 0.75f;
                if (damageInfo.result.spDamage > 0f && !skill.agent.IsDead()) skill.agent.mental += damageInfo.result.spDamage * 0.75f;
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

        private NotbaconGravityRockAnim animscript;
    }
}
