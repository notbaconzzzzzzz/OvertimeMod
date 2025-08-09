using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconPollenGift2 : EquipmentScriptBase
    {
        public override void OnFixedUpdate()
        {
            if (worker == null)
            {
                worker = base.model.owner as WorkerModel;
                if (worker == null) return;
            }
            UpdateRecoverLevel();
            if (_recoverTimer.maxTime - _recoverTimer.elapsed > RecoverInterval) _recoverTimer.StartTimer(RecoverInterval);
            if (_recoverTimer.RunTimer())
            {
                worker.RecoverHPv2(RecoverAmount);
                worker.RecoverMentalv2(RecoverAmount);
                UpdateRecoverLevel();
            }
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(RecoverInterval);
            }
        }
        public override Vector2 PercentageRecoverOnHit(UnitModel actor, DamageInfo dmg)
        {
            return new Vector2(15f, 15f);
        }

        public void UpdateRecoverLevel()
        {
            level = 0;
            float hpRatio = worker.hp / (float)worker.maxHp;
            if (hpRatio <= 0.1f) level = 3;
            if (hpRatio <= 0.5f) level = 2;
            if (hpRatio <= 0.8f) level = 1;
        }

        public float RecoverAmount
        {
            get
            {
                switch (level)
                {
                    case 0: return 0.8f;
                    case 1: return 1.2f;
                    case 2: return 1.8f;
                    case 3: return 2f;
                }
                return 0.8f;
            }
        }

        public float RecoverInterval
        {
            get
            {
                switch (level)
                {
                    case 0: return 5f;
                    case 1: return 2f;
                    case 2: return 1.2f;
                    case 3: return 0.6f;
                }
                return 0.8f;
            }
        }

        private WorkerModel worker;

        private Timer _recoverTimer = new Timer();

        private int level;
    }
}
