using Spine;
using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconGravityRockWeapon : EquipmentScriptBase
    {
        public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
        {
            if (target.hp <= 0f) return;
            if (dmg.result.hpDamage > 0f) target.hp += dmg.result.hpDamage * 0.5f;
            if (dmg.result.spDamage > 0f && (target is WorkerModel || target is RabbitModel)) target.mental += dmg.result.spDamage * 0.5f;
        }
    }
}
