using System;

namespace NotbaconOvertimeMod
{
    public class NotbaconGarlandGift2 : EquipmentScriptBase
    {
        public override void OnFixedUpdate()
        {
            if (_recoverTimer.RunTimer())
            {
                WorkerModel workerModel = base.model.owner as WorkerModel;
                if (workerModel != null)
                {
                    workerModel.RecoverHPv2(2f);
                }
            }
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(8f);
            }
        }

        public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
        {
            return 6f;
        }

        public override float WorkSpeedModifier(CreatureModel target, SkillTypeInfo skill)
        {
            if (skill.rwbpType == RwbpType.R) return 1.25f;
            return base.WorkSpeedModifier(target, skill);
        }

        private Timer _recoverTimer = new Timer();
    }
}
