using System;

public class NothingGift : EquipmentScriptBase
{
    public override float RecoveryMultiplier(bool isMental, float amount)
    {
        return 1.05f;
    }
}