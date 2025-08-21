using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconPaleHorse : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconPaleHorseAnim)unit.animTarget;
            animscript.SetScript(this);
            animscript.IdleAnim();
        }

        public override void OnEnterRoom(UseSkill skill)
        {
            trigger = false;
            if (skill.agent.HasUnitBuf(UnitBufType.NOTBACON_PALE_HORSE_MARKED) || skill.agent.hp / skill.agent.maxHp <= 0.25f)
            {
                trigger = true;
            }
            /*
            UnitBuf buf = skill.agent.GetUnitBufByType(UnitBufType.NOTBACON_PALE_HORSE_MARKED);
            if (buf != null)
            {
                Type markedBuf = Assembly.GetExecutingAssembly().GetType("NotbaconOvertimeMod.NotbaconPaleHorseMarkedBuf");
                MethodInfo testMethod = markedBuf.GetMethod("Test");
                testMethod.Invoke(buf, new object[0]);
            }
            */
        }

        public override void OnFinishWork(UseSkill skill)
        {
            if (skill.GetCurrentFeelingState() == CreatureFeelingState.GOOD && !skill.agent.HasUnitBuf(UnitBufType.NOTBACON_PALE_HORSE_MARKED))
            {
                if (trigger || skill.agent.justiceLevel <= 1)
                {
                    skill.agent.AddUnitBuf(new NotbaconPaleHorseMarkedBuf());
                }
            }
            trigger = false;
        }

        public override float TranformWorkProb(float originWorkProb)
        {
            if (trigger)
            {
                return originWorkProb * 1.2f;
            }
            return base.TranformWorkProb(originWorkProb);
        }

        private NotbaconPaleHorseAnim animscript;

        private bool trigger;
    }
}
