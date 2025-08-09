using System;
using System.Collections.Generic;
using System.Linq;

namespace NotbaconOvertimeMod
{
    public class NotbaconGarlandGift4 : EquipmentScriptBase
    {
        public override void OnFixedUpdate()
        {
            if (_recoverTimer.RunTimer())
            {
                WorkerModel workerModel = base.model.owner as WorkerModel;
                if (workerModel != null)
                {
                    workerModel.RecoverHPv2(2f);
                    if (workerModel.GetMovableNode().currentPassage != null)
                    {
                        foreach (MovableObjectNode node in workerModel.GetMovableNode().currentPassage.GetEnteredTargets())
                        {
                            UnitModel unit = node.GetUnit();
                            if (unit == null || unit == workerModel || !(unit is WorkerModel)) continue;
                            IrisGarlandBuf buf = unit.GetUnitBufByType(UnitBufType.NOTBACON_IRIS_GARLAND) as IrisGarlandBuf;
                            if (buf == null)
                            {
                                unit.AddUnitBuf(new IrisGarlandBuf(this));
                            }
                            else
                            {
                                if (!buf.stacking.Contains(this)) buf.stacking.Add(this);
                            }
                        }
                    }
                }
            }
            if (!_recoverTimer.started)
            {
                _recoverTimer.StartTimer(4f);
            }
        }

        public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
        {
            return 20f;
        }

        private Timer _recoverTimer = new Timer();

        public class IrisGarlandBuf : UnitBuf
        {
            public IrisGarlandBuf(NotbaconGarlandGift4 garland)
            {
                type = UnitBufType.NOTBACON_IRIS_GARLAND;
                duplicateType = BufDuplicateType.ONLY_ONE;
                stacking.Add(garland);
            }
            public override void Init(UnitModel model)
            {
                base.Init(model);
                worker = (model as WorkerModel);
            }

            public override void FixedUpdate()
            {
                if (_recoverTimer.RunTimer())
                {
                    if (model.GetMovableNode().currentPassage == null)
                    {
                        Destroy();
                        return;
                    }
                    MovableObjectNode[] nearby = model.GetMovableNode().currentPassage.GetEnteredTargetsAsArray();
                    for (int i = 0; i < stacking.Count; i++)
                    {
                        if (!nearby.Contains(stacking[i].model.owner.GetMovableNode()))
                        {
                            stacking.RemoveAt(i);
                            i--;
                        }
                    }
                    if (stacking.Count <= 0)
                    {
                        Destroy();
                        return;
                    }
                    if (worker != null)
                    {
                        worker.RecoverMentalv2(0.5f * (float)stacking.Count);
                    }
                }
                if (!_recoverTimer.started)
                {
                    _recoverTimer.StartTimer(4f);
                }
            }

            public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
            {
                switch (stacking.Count)
                {
                    case 0: return 0f;
                    case 1: return 5f;
                    case 2: return 9f;
                    case 3: return 12f;
                    case 4: return 14f;
                    case 5: default: return 15f;
                }
            }

            private WorkerModel worker;

            public List<NotbaconGarlandGift4> stacking = new List<NotbaconGarlandGift4>();

            private Timer _recoverTimer = new Timer();
        }
    }
}
