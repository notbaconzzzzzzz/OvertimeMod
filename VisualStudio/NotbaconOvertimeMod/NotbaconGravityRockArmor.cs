using Spine;
using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconGravityRockArmor : EquipmentScriptBase
    {
        public override void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
        {
            if (model.owner.hp <= 0f) return;
            if (dmg.type != RwbpType.W) return;
            if (dmg.result.hpDamage > 0f) model.owner.hp += dmg.result.hpDamage * 0.6f;
            if (dmg.result.spDamage > 0f) model.owner.mental += dmg.result.spDamage * 0.6f;
        }
    }
}
