using System;

namespace NotbaconOvertimeMod
{
    public class NotbaconGravityRockGift : EquipmentScriptBase
    {
        public override void OnStageStart()
        {
            model.owner.AddUnitBuf(new UnitStatBuf(float.PositiveInfinity)
            {
                primaryStat = new WorkerPrimaryStatBonus() { mental = -21 }
            });
        }

        public override void OnEquip(UnitModel actor)
        {
            if (GameManager.currentGameManager != null && GameManager.currentGameManager.state != GameState.STOP)
            {
                actor.AddUnitBuf(new UnitStatBuf(float.PositiveInfinity)
                {
                    primaryStat = new WorkerPrimaryStatBonus() { mental = -21 }
                });
            }
        }
    }
}
