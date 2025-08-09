using System;

public class BunnyGift : EquipmentScriptBase
{
    public override float WorkSpeedModifier(CreatureModel target, SkillTypeInfo skill)
    {
        if (target.metaInfo.LcId == 100054L) return 0.9f;
        return base.WorkSpeedModifier(target, skill);
    }
}