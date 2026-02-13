using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconLonelyWraith : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconLonelyWraithAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        public float AoeDmg
        {
            get
            {
                if (unitCount <= 1) return 1f;
                if (unitCount < 3) return 1f + 1f * (float)(unitCount - 1) / 2f;
                if (unitCount < 6) return 2f + 1f * (float)(unitCount - 3) / 3f;
                if (unitCount < 10) return 3f + 1f * (float)(unitCount - 6) / 4f;
                if (unitCount < 15) return 4f + 1f * (float)(unitCount - 10) / 5f;
                if (unitCount < 21) return 5f + 1f * (float)(unitCount - 15) / 6f;
                if (unitCount < 28) return 6f + 1f * (float)(unitCount - 21) / 7f;
                if (unitCount < 36) return 7f + 1f * (float)(unitCount - 28) / 8f;
                if (unitCount < 45) return 8f + 1f * (float)(unitCount - 36) / 9f;
                if (unitCount < 55) return 9f + 1f * (float)(unitCount - 45) / 10f;
                return 10f;
            }
        }

        public float UnderAttackBufOverride
        {
            get
            {
                if (unitCount < 3) return 0.1f;
                if (unitCount < 6) return 0.2f;
                if (unitCount < 10) return 0.3f;
                if (unitCount < 15) return 0.4f;
                if (unitCount < 21) return 0.5f;
                if (unitCount < 28) return 0.6f;
                if (unitCount < 36) return 0.7f;
                if (unitCount < 45) return 0.8f;
                if (unitCount < 55) return 0.9f;
                return 1f;
            }
        }

        public DamageInfo ScreechDmg
        {
            get
            {
                float min = 0f;
                float max = 0f;
                if (unitCount < 3) min = 18f;
                else if (unitCount < 6) min = 20f;
                else if (unitCount < 10) min = 22f;
                else if (unitCount < 15) min = 24f;
                else if (unitCount < 21) min = 26f;
                else if (unitCount < 28) min = 28f;
                else if (unitCount < 36) min = 30f;
                else if (unitCount < 45) min = 32f;
                else if (unitCount < 55) min = 36f;
                else min = 40f;
                if (unitCount < 10) max = 21f + 1f * (float)unitCount;
                if (unitCount < 21) max = 25f + 1f * (float)unitCount;
                if (unitCount < 36) max = 32f + 1f * (float)unitCount;
                if (unitCount < 55) max = 2f * (float)unitCount;
                else max = 110f;
                return new DamageInfo(RwbpType.W, min, max);
            }
        }

        public float ScreechSlowTime
        {
            get
            {
                if (unitCount < 6) return 0f + 1f * (float)unitCount;
                return 6f;
            }
        }

        public float ScreechSlowFactor
        {
            get
            {
                if (unitCount < 6) return 1f / 1.25f;
                if (unitCount < 10) return 1f / (1.25f + 0.25f * (float)(unitCount - 6) / 4f);
                if (unitCount < 15) return 1f / (1.5f + 0.25f * (float)(unitCount - 10) / 5f);
                if (unitCount < 21) return 1f / (1.75f + 0.25f * (float)(unitCount - 15) / 6f);
                if (unitCount < 28) return 1f / (2f + 0.25f * (float)(unitCount - 21) / 7f);
                if (unitCount < 36) return 1f / (2.25f + 0.25f * (float)(unitCount - 28) / 8f);
                if (unitCount < 45) return 1f / (2.5f + 0.25f * (float)(unitCount - 36) / 9f);
                if (unitCount < 55) return 1f / (2.75f + 0.25f * (float)(unitCount - 45) / 10f);
                return 1f / 3f;
            }
        }

        public override void OnFinishWork(UseSkill skill)
        {
            if (obsession == null && skill.skillTypeInfo.id == 3L)
            {
                obsession = skill.agent;
            }
        }

        private NotbaconLonelyWraithAnim animscript;

        private int unitCount;
        private AgentModel obsession;
    }
}
