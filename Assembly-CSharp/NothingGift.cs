using System;

public class NothingGift : EquipmentScriptBase
{
    public override float RecoveryAdditiveMultiplier(bool isMental, float amount)
    {
        return 5f;
    }
}