using System;
using System.Collections.Generic;
using System.Linq;

namespace NotbaconOvertimeMod
{
    public class NotbaconGarlandGift6 : EquipmentScriptBase
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
                worker.RecoverHPv2(4f);
            }
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(4f);
            }
            _active = worker.hp > (float)worker.maxHp * 0.3f;
        }

        public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
        {
            return 30f;
        }

        public override float GetDamageFactor()
        {
            if (_active) return 1.15f;
            return base.GetDamageFactor();
        }

        public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
        {
            if (!_active) return;
            /*
            Notice.instance.Send(NoticeName.AddSystemLog, new object[]
            {
                dmg.result.ToString()
            });*/
            float num = dmg.result.originDamage / 1.15f * 0.15f;
            if (num > 0)
            {
                worker.TakeDamage(null, new DamageInfo(RwbpType.R, num));
                DamageParticleEffect.Invoker(worker, RwbpType.R, worker);
            }
        }

        private WorkerModel worker;

        private Timer _recoverTimer = new Timer();

        private bool _active;
    }
}
